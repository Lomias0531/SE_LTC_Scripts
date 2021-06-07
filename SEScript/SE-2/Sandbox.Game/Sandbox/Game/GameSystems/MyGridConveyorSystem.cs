using ParallelTasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGridConveyorSystem
	{
		private class PullRequestItemSet
		{
			private bool m_all;

			private MyObjectBuilderType? m_obType;

			private MyInventoryConstraint m_constraint;

			public void Clear()
			{
				m_all = false;
				m_obType = null;
				m_constraint = null;
			}

			public void Set(bool all)
			{
				Clear();
				m_all = all;
			}

			public void Set(MyObjectBuilderType? itemTypeId)
			{
				Clear();
				m_obType = itemTypeId;
			}

			public void Set(MyInventoryConstraint inventoryConstraint)
			{
				Clear();
				m_constraint = inventoryConstraint;
			}

			public bool Contains(MyDefinitionId itemId)
			{
				if (m_all)
				{
					return true;
				}
				if (m_obType.HasValue && m_obType.Value == itemId.TypeId)
				{
					return true;
				}
				if (m_constraint != null && m_constraint.Check(itemId))
				{
					return true;
				}
				return false;
			}
		}

		private class TransferData : WorkData
		{
			public IMyConveyorEndpointBlock m_start;

			public IMyConveyorEndpointBlock m_endPoint;

			public MyDefinitionId m_itemId;

			public bool m_isPush;

			public bool m_canTransfer;

			public TransferData(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock endPoint, MyDefinitionId itemId, bool isPush)
			{
				m_start = start;
				m_endPoint = endPoint;
				m_itemId = itemId;
				m_isPush = isPush;
			}

			public void ComputeTransfer()
			{
				IMyConveyorEndpointBlock lhs = m_start;
				IMyConveyorEndpointBlock rhs = m_endPoint;
				if (!m_isPush)
				{
					MyUtils.Swap(ref lhs, ref rhs);
				}
				m_canTransfer = ComputeCanTransfer(lhs, rhs, m_itemId);
			}

			public void StoreTransferState()
			{
				(m_start as MyCubeBlock).CubeGrid.GridSystems.ConveyorSystem.GetConveyorEndpointMapping(m_start).AddTransfer(m_endPoint, m_itemId, m_isPush, m_canTransfer);
			}
		}

		private class ConveyorEndpointMapping
		{
			public List<IMyConveyorEndpointBlock> pullElements;

			public List<IMyConveyorEndpointBlock> pushElements;

			public Dictionary<Tuple<IMyConveyorEndpointBlock, MyDefinitionId, bool>, bool> testedTransfers = new Dictionary<Tuple<IMyConveyorEndpointBlock, MyDefinitionId, bool>, bool>();

			public void AddTransfer(IMyConveyorEndpointBlock block, MyDefinitionId itemId, bool isPush, bool canTransfer)
			{
				Tuple<IMyConveyorEndpointBlock, MyDefinitionId, bool> key = new Tuple<IMyConveyorEndpointBlock, MyDefinitionId, bool>(block, itemId, isPush);
				testedTransfers[key] = canTransfer;
			}

			public bool TryGetTransfer(IMyConveyorEndpointBlock block, MyDefinitionId itemId, bool isPush, out bool canTransfer)
			{
				Tuple<IMyConveyorEndpointBlock, MyDefinitionId, bool> key = new Tuple<IMyConveyorEndpointBlock, MyDefinitionId, bool>(block, itemId, isPush);
				return testedTransfers.TryGetValue(key, out canTransfer);
			}
		}

		private static readonly float CONVEYOR_SYSTEM_CONSUMPTION = 0.005f;

		private readonly HashSet<MyCubeBlock> m_inventoryBlocks = new HashSet<MyCubeBlock>();

		private readonly HashSet<IMyConveyorEndpointBlock> m_conveyorEndpointBlocks = new HashSet<IMyConveyorEndpointBlock>();

		private readonly HashSet<MyConveyorLine> m_lines = new HashSet<MyConveyorLine>();

		private readonly HashSet<MyShipConnector> m_connectors = new HashSet<MyShipConnector>();

		private MyCubeGrid m_grid;

		private bool m_needsRecomputation = true;

		private HashSet<MyCubeGrid> m_tmpConnectedGrids = new HashSet<MyCubeGrid>();

		[ThreadStatic]
		private static List<MyPhysicalInventoryItem> m_tmpInventoryItems;

		[ThreadStatic]
		private static PullRequestItemSet m_tmpRequestedItemSetPerThread;

		[ThreadStatic]
		private static MyPathFindingSystem<IMyConveyorEndpoint> m_pathfinding = new MyPathFindingSystem<IMyConveyorEndpoint>();

		private static Dictionary<Tuple<IMyConveyorEndpointBlock, IMyConveyorEndpointBlock>, Task> m_currentTransferComputationTasks = new Dictionary<Tuple<IMyConveyorEndpointBlock, IMyConveyorEndpointBlock>, Task>();

		private Dictionary<ConveyorLinePosition, MyConveyorLine> m_lineEndpoints;

		private Dictionary<Vector3I, MyConveyorLine> m_linePoints;

		private HashSet<MyConveyorLine> m_deserializedLines;

		public bool IsClosing;

		public MyStringId HudMessage = MyStringId.NullOrEmpty;

		public string HudMessageCustom = string.Empty;

		[ThreadStatic]
		private static long m_playerIdForAccessiblePredicate;

		[ThreadStatic]
		private static MyDefinitionId m_inventoryItemDefinitionId;

		private static Predicate<IMyConveyorEndpoint> IsAccessAllowedPredicate = IsAccessAllowed;

		private static Predicate<IMyPathEdge<IMyConveyorEndpoint>> IsConveyorLargePredicate = IsConveyorLarge;

		private static Predicate<IMyPathEdge<IMyConveyorEndpoint>> IsConveyorSmallPredicate = IsConveyorSmall;

		[ThreadStatic]
		private static List<IMyConveyorEndpoint> m_reachableBuffer;

		private Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping> m_conveyorConnections = new Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping>();

		private bool m_isRecomputingGraph;

		private bool m_isRecomputationInterrupted;

		private bool m_isRecomputationIsAborted;

		private const double MAX_RECOMPUTE_DURATION_MILLISECONDS = 10.0;

		private Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping> m_conveyorConnectionsForThread = new Dictionary<IMyConveyorEndpointBlock, ConveyorEndpointMapping>();

		private IEnumerator<IMyConveyorEndpointBlock> m_endpointIterator;

		private FastResourceLock m_iteratorLock = new FastResourceLock();

		public bool NeedsUpdateLines;

		private static PullRequestItemSet m_tmpRequestedItemSet
		{
			get
			{
				if (m_tmpRequestedItemSetPerThread == null)
				{
					m_tmpRequestedItemSetPerThread = new PullRequestItemSet();
				}
				return m_tmpRequestedItemSetPerThread;
			}
		}

		private static MyPathFindingSystem<IMyConveyorEndpoint> Pathfinding
		{
			get
			{
				if (m_pathfinding == null)
				{
					m_pathfinding = new MyPathFindingSystem<IMyConveyorEndpoint>();
				}
				return m_pathfinding;
			}
		}

		public MyResourceSinkComponent ResourceSink
		{
			get;
			private set;
		}

		public bool IsInteractionPossible
		{
			get
			{
				bool flag = false;
				foreach (MyShipConnector connector in m_connectors)
				{
					flag |= connector.InConstraint;
				}
				return flag;
			}
		}

		public bool Connected
		{
			get
			{
				bool flag = false;
				foreach (MyShipConnector connector in m_connectors)
				{
					flag |= connector.Connected;
				}
				return flag;
			}
		}

		public HashSetReader<IMyConveyorEndpointBlock> ConveyorEndpointBlocks => new HashSetReader<IMyConveyorEndpointBlock>(m_conveyorEndpointBlocks);

		public HashSetReader<MyCubeBlock> InventoryBlocks => new HashSetReader<MyCubeBlock>(m_inventoryBlocks);

		public event Action<MyCubeBlock> BlockAdded;

		public event Action<MyCubeBlock> BlockRemoved;

		public event Action<IMyConveyorEndpointBlock> OnBeforeRemoveEndpointBlock;

		public event Action<IMyConveyorSegmentBlock> OnBeforeRemoveSegmentBlock;

		public MyGridConveyorSystem(MyCubeGrid grid)
		{
			m_grid = grid;
			m_lineEndpoints = null;
			m_linePoints = null;
			m_deserializedLines = null;
			ResourceSink = new MyResourceSinkComponent();
			ResourceSink.Init(MyStringHash.GetOrCompute("Conveyors"), CONVEYOR_SYSTEM_CONSUMPTION, CalculateConsumption);
			ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			ResourceSink.Update();
		}

		public void BeforeBlockDeserialization(List<MyObjectBuilder_ConveyorLine> lines)
		{
			if (lines != null)
			{
				m_lineEndpoints = new Dictionary<ConveyorLinePosition, MyConveyorLine>(lines.Count * 2);
				m_linePoints = new Dictionary<Vector3I, MyConveyorLine>(lines.Count * 4);
				m_deserializedLines = new HashSet<MyConveyorLine>();
				foreach (MyObjectBuilder_ConveyorLine line in lines)
				{
					MyConveyorLine myConveyorLine = new MyConveyorLine();
					myConveyorLine.Init(line, m_grid);
					if (myConveyorLine.CheckSectionConsistency())
					{
						ConveyorLinePosition key = new ConveyorLinePosition(line.StartPosition, line.StartDirection);
						ConveyorLinePosition key2 = new ConveyorLinePosition(line.EndPosition, line.EndDirection);
						try
						{
							m_lineEndpoints.Add(key, myConveyorLine);
							m_lineEndpoints.Add(key2, myConveyorLine);
							foreach (Vector3I item in myConveyorLine)
							{
								m_linePoints.Add(item, myConveyorLine);
							}
							m_deserializedLines.Add(myConveyorLine);
							m_lines.Add(myConveyorLine);
						}
						catch (ArgumentException)
						{
							m_lineEndpoints = null;
							m_deserializedLines = null;
							m_linePoints = null;
							m_lines.Clear();
							return;
						}
					}
				}
			}
		}

		public MyConveyorLine GetDeserializingLine(ConveyorLinePosition position)
		{
			if (m_lineEndpoints == null)
			{
				return null;
			}
			m_lineEndpoints.TryGetValue(position, out MyConveyorLine value);
			return value;
		}

		public MyConveyorLine GetDeserializingLine(Vector3I position)
		{
			if (m_linePoints == null)
			{
				return null;
			}
			m_linePoints.TryGetValue(position, out MyConveyorLine value);
			return value;
		}

		public void AfterBlockDeserialization()
		{
			m_lineEndpoints = null;
			m_linePoints = null;
			m_deserializedLines = null;
			foreach (MyConveyorLine line in m_lines)
			{
				line.UpdateIsFunctional();
			}
		}

		public void SerializeLines(List<MyObjectBuilder_ConveyorLine> resultList)
		{
			foreach (MyConveyorLine line in m_lines)
			{
				if (!line.IsEmpty || !line.IsDisconnected || line.Length != 1)
				{
					resultList.Add(line.GetObjectBuilder());
				}
			}
		}

		public void AfterGridClose()
		{
			m_lines.Clear();
		}

		public void Add(MyCubeBlock block)
		{
			m_inventoryBlocks.Add(block);
			this.BlockAdded?.Invoke(block);
		}

		public void Remove(MyCubeBlock block)
		{
			m_inventoryBlocks.Remove(block);
			this.BlockRemoved?.Invoke(block);
		}

		internal void GetGridInventories(MyEntity interactedAsEntity, List<MyEntity> outputInventories, long identityId)
		{
			GetGridInventories(interactedAsEntity, outputInventories, identityId, null);
		}

		internal void GetGridInventories(MyEntity interactedAsEntity, List<MyEntity> outputInventories, long identityId, List<long> inventoryIds = null)
		{
			foreach (MyCubeBlock inventoryBlock in m_inventoryBlocks)
			{
				if ((!(inventoryBlock is MyTerminalBlock) || (inventoryBlock as MyTerminalBlock).HasPlayerAccess(identityId)) && (interactedAsEntity == inventoryBlock || !(inventoryBlock is MyTerminalBlock) || (inventoryBlock as MyTerminalBlock).ShowInInventory))
				{
					outputInventories?.Add(inventoryBlock);
					inventoryIds?.Add(inventoryBlock.EntityId);
				}
			}
		}

		public void AddConveyorBlock(IMyConveyorEndpointBlock endpointBlock)
		{
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				m_endpointIterator = null;
				m_conveyorEndpointBlocks.Add(endpointBlock);
				if (endpointBlock is MyShipConnector)
				{
					m_connectors.Add(endpointBlock as MyShipConnector);
				}
				IMyConveyorEndpoint conveyorEndpoint = endpointBlock.ConveyorEndpoint;
				for (int i = 0; i < conveyorEndpoint.GetLineCount(); i++)
				{
					ConveyorLinePosition position = conveyorEndpoint.GetPosition(i);
					MyConveyorLine conveyorLine = conveyorEndpoint.GetConveyorLine(i);
					if (m_deserializedLines == null || !m_deserializedLines.Contains(conveyorLine))
					{
						MySlimBlock cubeBlock = m_grid.GetCubeBlock(position.NeighbourGridPosition);
						if (cubeBlock == null)
						{
							m_lines.Add(conveyorLine);
						}
						else
						{
							IMyConveyorEndpointBlock myConveyorEndpointBlock = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
							IMyConveyorSegmentBlock myConveyorSegmentBlock = cubeBlock.FatBlock as IMyConveyorSegmentBlock;
							if (myConveyorSegmentBlock != null)
							{
								if (!TryMergeEndpointSegment(endpointBlock, myConveyorSegmentBlock, position))
								{
									m_lines.Add(conveyorLine);
								}
							}
							else if (myConveyorEndpointBlock != null)
							{
								if (!TryMergeEndpointEndpoint(endpointBlock, myConveyorEndpointBlock, position, position.GetConnectingPosition()))
								{
									m_lines.Add(conveyorLine);
								}
							}
							else
							{
								m_lines.Add(conveyorLine);
							}
						}
					}
				}
			}
		}

		public void DebugDraw(MyCubeGrid grid)
		{
			foreach (MyConveyorLine line in m_lines)
			{
				line.DebugDraw(grid);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(1f, 1f), "Conveyor lines: " + m_lines.Count, Color.Red, 1f);
		}

		public void DebugDrawLinePackets()
		{
			foreach (MyConveyorLine line in m_lines)
			{
				line.DebugDrawPackets();
			}
		}

		public void UpdateBeforeSimulation()
		{
			MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
			foreach (MyConveyorLine line in m_lines)
			{
				if (!line.IsEmpty)
				{
					line.Update();
				}
			}
			MySimpleProfiler.End("UpdateBeforeSimulation");
		}

		public void UpdateBeforeSimulation10()
		{
			MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation10");
			ResourceSink.Update();
			MySimpleProfiler.End("UpdateBeforeSimulation10");
		}

		public void FlagForRecomputation()
		{
			MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group group = MyGridPhysicalHierarchy.Static.GetGroup(m_grid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node in group.Nodes)
				{
					node.NodeData.GridSystems.ConveyorSystem.m_needsRecomputation = true;
				}
			}
		}

		public void UpdateAfterSimulation()
		{
			UpdateLinesLazy();
		}

		public void UpdateAfterSimulation100()
		{
			if (m_needsRecomputation)
			{
				MySimpleProfiler.Begin("Conveyor", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation100");
				RecomputeConveyorEndpoints();
				m_needsRecomputation = false;
				MySimpleProfiler.End("UpdateAfterSimulation100");
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateLines();
		}

		public void UpdateLines()
		{
			NeedsUpdateLines = true;
			m_grid.MarkForUpdate();
		}

		public void UpdateLinesLazy()
		{
			if (NeedsUpdateLines)
			{
				NeedsUpdateLines = false;
				FlagForRecomputation();
			}
		}

		public float CalculateConsumption()
		{
			float num = 0f;
			foreach (MyConveyorLine line in m_lines)
			{
				if (line.IsFunctional)
				{
					num += 1E-07f;
				}
			}
			return num;
		}

		public void RemoveConveyorBlock(IMyConveyorEndpointBlock block)
		{
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				m_endpointIterator = null;
				m_conveyorEndpointBlocks.Remove(block);
				if (block is MyShipConnector)
				{
					m_connectors.Remove(block as MyShipConnector);
				}
				if (!IsClosing)
				{
					if (this.OnBeforeRemoveEndpointBlock != null)
					{
						this.OnBeforeRemoveEndpointBlock(block);
					}
					for (int i = 0; i < block.ConveyorEndpoint.GetLineCount(); i++)
					{
						MyConveyorLine conveyorLine = block.ConveyorEndpoint.GetConveyorLine(i);
						conveyorLine.DisconnectEndpoint(block.ConveyorEndpoint);
						if (conveyorLine.IsDegenerate)
						{
							m_lines.Remove(conveyorLine);
						}
					}
				}
			}
		}

		public void AddSegmentBlock(IMyConveyorSegmentBlock segmentBlock)
		{
			AddSegmentBlockInternal(segmentBlock, segmentBlock.ConveyorSegment.ConnectingPosition1);
			AddSegmentBlockInternal(segmentBlock, segmentBlock.ConveyorSegment.ConnectingPosition2);
			if (!m_lines.Contains(segmentBlock.ConveyorSegment.ConveyorLine) && segmentBlock.ConveyorSegment.ConveyorLine != null)
			{
				m_lines.Add(segmentBlock.ConveyorSegment.ConveyorLine);
			}
		}

		public void RemoveSegmentBlock(IMyConveyorSegmentBlock segmentBlock)
		{
			if (!IsClosing)
			{
				if (this.OnBeforeRemoveSegmentBlock != null)
				{
					this.OnBeforeRemoveSegmentBlock(segmentBlock);
				}
				MyConveyorLine conveyorLine = segmentBlock.ConveyorSegment.ConveyorLine;
				MyConveyorLine myConveyorLine = segmentBlock.ConveyorSegment.ConveyorLine.RemovePortion(segmentBlock.ConveyorSegment.ConnectingPosition1.NeighbourGridPosition, segmentBlock.ConveyorSegment.ConnectingPosition2.NeighbourGridPosition);
				if (conveyorLine.IsDegenerate)
				{
					m_lines.Remove(conveyorLine);
				}
				if (myConveyorLine != null)
				{
					UpdateLineReferences(myConveyorLine, myConveyorLine);
					m_lines.Add(myConveyorLine);
				}
			}
		}

		private void AddSegmentBlockInternal(IMyConveyorSegmentBlock segmentBlock, ConveyorLinePosition connectingPosition)
		{
			MySlimBlock cubeBlock = m_grid.GetCubeBlock(connectingPosition.LocalGridPosition);
			if (cubeBlock == null || (m_deserializedLines != null && m_deserializedLines.Contains(segmentBlock.ConveyorSegment.ConveyorLine)))
			{
				return;
			}
			IMyConveyorEndpointBlock myConveyorEndpointBlock = cubeBlock.FatBlock as IMyConveyorEndpointBlock;
			IMyConveyorSegmentBlock myConveyorSegmentBlock = cubeBlock.FatBlock as IMyConveyorSegmentBlock;
			if (myConveyorSegmentBlock != null)
			{
				MyConveyorLine conveyorLine = segmentBlock.ConveyorSegment.ConveyorLine;
				if (m_lines.Contains(conveyorLine))
				{
					m_lines.Remove(conveyorLine);
				}
				if (myConveyorSegmentBlock.ConveyorSegment.CanConnectTo(connectingPosition, segmentBlock.ConveyorSegment.ConveyorLine.Type))
				{
					MergeSegmentSegment(segmentBlock, myConveyorSegmentBlock);
				}
			}
			if (myConveyorEndpointBlock != null)
			{
				MyConveyorLine conveyorLine2 = myConveyorEndpointBlock.ConveyorEndpoint.GetConveyorLine(connectingPosition);
				if (TryMergeEndpointSegment(myConveyorEndpointBlock, segmentBlock, connectingPosition))
				{
					m_lines.Remove(conveyorLine2);
				}
			}
		}

		private bool TryMergeEndpointSegment(IMyConveyorEndpointBlock endpoint, IMyConveyorSegmentBlock segmentBlock, ConveyorLinePosition endpointPosition)
		{
			MyConveyorLine conveyorLine = endpoint.ConveyorEndpoint.GetConveyorLine(endpointPosition);
			if (conveyorLine == null)
			{
				return false;
			}
			if (!segmentBlock.ConveyorSegment.CanConnectTo(endpointPosition.GetConnectingPosition(), conveyorLine.Type))
			{
				return false;
			}
			MyConveyorLine conveyorLine2 = segmentBlock.ConveyorSegment.ConveyorLine;
			conveyorLine2.Merge(conveyorLine, segmentBlock);
			endpoint.ConveyorEndpoint.SetConveyorLine(endpointPosition, conveyorLine2);
			conveyorLine.RecalculateConductivity();
			conveyorLine2.RecalculateConductivity();
			return true;
		}

		private bool TryMergeEndpointEndpoint(IMyConveyorEndpointBlock endpointBlock1, IMyConveyorEndpointBlock endpointBlock2, ConveyorLinePosition pos1, ConveyorLinePosition pos2)
		{
			MyConveyorLine conveyorLine = endpointBlock1.ConveyorEndpoint.GetConveyorLine(pos1);
			if (conveyorLine == null)
			{
				return false;
			}
			MyConveyorLine conveyorLine2 = endpointBlock2.ConveyorEndpoint.GetConveyorLine(pos2);
			if (conveyorLine2 == null)
			{
				return false;
			}
			if (conveyorLine.Type != conveyorLine2.Type)
			{
				return false;
			}
			if (conveyorLine.GetEndpoint(1) == null)
			{
				conveyorLine.Reverse();
			}
			if (conveyorLine2.GetEndpoint(0) == null)
			{
				conveyorLine2.Reverse();
			}
			conveyorLine2.Merge(conveyorLine);
			endpointBlock1.ConveyorEndpoint.SetConveyorLine(pos1, conveyorLine2);
			conveyorLine.RecalculateConductivity();
			conveyorLine2.RecalculateConductivity();
			return true;
		}

		private void MergeSegmentSegment(IMyConveyorSegmentBlock newSegmentBlock, IMyConveyorSegmentBlock oldSegmentBlock)
		{
			MyConveyorLine conveyorLine = newSegmentBlock.ConveyorSegment.ConveyorLine;
			MyConveyorLine conveyorLine2 = oldSegmentBlock.ConveyorSegment.ConveyorLine;
			if (conveyorLine != conveyorLine2)
			{
				conveyorLine2.Merge(conveyorLine, newSegmentBlock);
			}
			UpdateLineReferences(conveyorLine, conveyorLine2);
			newSegmentBlock.ConveyorSegment.SetConveyorLine(conveyorLine2);
		}

		private void UpdateLineReferences(MyConveyorLine oldLine, MyConveyorLine newLine)
		{
			for (int i = 0; i < 2; i++)
			{
				if (oldLine.GetEndpoint(i) != null)
				{
					oldLine.GetEndpoint(i).SetConveyorLine(oldLine.GetEndpointPosition(i), newLine);
				}
			}
			foreach (Vector3I item in oldLine)
			{
				MySlimBlock cubeBlock = m_grid.GetCubeBlock(item);
				if (cubeBlock != null)
				{
					(cubeBlock.FatBlock as IMyConveyorSegmentBlock)?.ConveyorSegment.SetConveyorLine(newLine);
				}
			}
			oldLine.RecalculateConductivity();
			newLine.RecalculateConductivity();
		}

		public void ToggleConnectors()
		{
			bool flag = false;
			foreach (MyShipConnector connector in m_connectors)
			{
				flag |= connector.Connected;
			}
			foreach (MyShipConnector connector2 in m_connectors)
			{
				if (connector2.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies)
				{
					if (flag && connector2.Connected)
					{
						connector2.TryDisconnect();
						HudMessage = MySpaceTexts.NotificationConnectorsDisabled;
					}
					if (!flag)
					{
						if (connector2.IsProtectedFromLockingByTrading() || (connector2.InConstraint && connector2.Other.IsProtectedFromLockingByTrading()))
						{
							HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_TemporaryBlock), connector2.GetProtectionFromLockingTime());
						}
						else
						{
							connector2.TryConnect();
							if (connector2.InConstraint)
							{
								HudMessage = MySpaceTexts.NotificationConnectorsEnabled;
								if ((float)connector2.AutoUnlockTime > 0f || (float)connector2.Other.AutoUnlockTime > 0f)
								{
									float num = 0f;
									num = ((!((float)connector2.AutoUnlockTime > 0f)) ? ((float)connector2.Other.AutoUnlockTime) : ((!((float)connector2.Other.AutoUnlockTime > 0f)) ? ((float)connector2.AutoUnlockTime) : Math.Min(connector2.AutoUnlockTime, connector2.Other.AutoUnlockTime)));
									int num2 = (int)(num / 60f);
									int num3 = (int)(num - (float)(num2 * 60));
									HudMessageCustom = string.Format(MyTexts.GetString(MySpaceTexts.Connector_AutoUnlockWarning), num2, num3);
								}
							}
							else
							{
								HudMessage = MyStringId.NullOrEmpty;
							}
						}
					}
				}
			}
		}

		private static void SetTraversalPlayerId(long playerId)
		{
			m_playerIdForAccessiblePredicate = playerId;
		}

		private static void SetTraversalInventoryItemDefinitionId(MyDefinitionId item = default(MyDefinitionId))
		{
			m_inventoryItemDefinitionId = item;
		}

		private static bool IsAccessAllowed(IMyConveyorEndpoint endpoint)
		{
			if (endpoint.CubeBlock.GetUserRelationToOwner(m_playerIdForAccessiblePredicate) == MyRelationsBetweenPlayerAndBlock.Enemies)
			{
				return false;
			}
			MyConveyorSorter myConveyorSorter = endpoint.CubeBlock as MyConveyorSorter;
			if (myConveyorSorter != null && m_inventoryItemDefinitionId != default(MyDefinitionId))
			{
				return myConveyorSorter.IsAllowed(m_inventoryItemDefinitionId);
			}
			MyShipConnector myShipConnector = endpoint.CubeBlock as MyShipConnector;
			if (myShipConnector != null && (bool)myShipConnector.TradingEnabled)
			{
				return false;
			}
			return true;
		}

		private static bool IsConveyorLarge(IMyPathEdge<IMyConveyorEndpoint> conveyorLine)
		{
			if (conveyorLine is MyConveyorLine)
			{
				return (conveyorLine as MyConveyorLine).Type == MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE;
			}
			return true;
		}

		private static bool IsConveyorSmall(IMyPathEdge<IMyConveyorEndpoint> conveyorLine)
		{
			if (conveyorLine is MyConveyorLine)
			{
				return (conveyorLine as MyConveyorLine).Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;
			}
			return true;
		}

		private static bool NeedsLargeTube(MyDefinitionId itemDefinitionId)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(itemDefinitionId);
			if (physicalItemDefinition == null)
			{
				return true;
			}
			if (itemDefinitionId.TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
			{
				return false;
			}
			return physicalItemDefinition.Size.AbsMax() > 0.25f;
		}

		public static void AppendReachableEndpoints(IMyConveyorEndpoint source, long playerId, List<IMyConveyorEndpoint> reachable, MyDefinitionId itemId, Predicate<IMyConveyorEndpoint> endpointFilter = null)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = source.CubeBlock as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock != null)
			{
				lock (Pathfinding)
				{
					SetTraversalPlayerId(playerId);
					SetTraversalInventoryItemDefinitionId(itemId);
					Pathfinding.FindReachable(myConveyorEndpointBlock.ConveyorEndpoint, reachable, endpointFilter, IsAccessAllowedPredicate, NeedsLargeTube(itemId) ? IsConveyorLargePredicate : null);
				}
			}
		}

		public static bool Reachable(IMyConveyorEndpoint source, IMyConveyorEndpoint endPoint, long playerId, MyDefinitionId itemId, Predicate<IMyConveyorEndpoint> endpointFilter = null)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = source.CubeBlock as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock == null)
			{
				return false;
			}
			lock (Pathfinding)
			{
				SetTraversalPlayerId(playerId);
				SetTraversalInventoryItemDefinitionId(itemId);
				return Pathfinding.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, endPoint, endpointFilter, IsAccessAllowedPredicate, NeedsLargeTube(itemId) ? IsConveyorLargePredicate : null);
			}
		}

		public static bool PullAllRequest(IMyConveyorEndpointBlock start, MyInventory destinationInventory, long playerId, MyInventoryConstraint requestedTypeIds, MyFixedPoint? maxAmount = null, bool pullFullBottles = true)
		{
			MyCubeBlock myCubeBlock = start as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			m_tmpRequestedItemSet.Set(requestedTypeIds);
			MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
			ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				bool result = false;
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock2 = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock2 == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock2.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock2.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != 0 && inventory != destinationInventory)
						{
							using (MyUtils.ReuseCollection(ref m_tmpInventoryItems))
							{
								foreach (MyPhysicalInventoryItem item in inventory.GetItems())
								{
									m_tmpInventoryItems.Add(item);
								}
								foreach (MyPhysicalInventoryItem tmpInventoryItem in m_tmpInventoryItems)
								{
									if (destinationInventory.VolumeFillFactor >= 1f)
									{
										return true;
									}
									MyDefinitionId id = tmpInventoryItem.Content.GetId();
									if ((requestedTypeIds == null || m_tmpRequestedItemSet.Contains(id)) && CanTransfer(start, conveyorEndpointMapping.pullElements[i], id, isPush: false))
									{
										MyFixedPoint myFixedPoint = tmpInventoryItem.Amount;
										MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = tmpInventoryItem.Content as MyObjectBuilder_GasContainerObject;
										if (pullFullBottles || myObjectBuilder_GasContainerObject == null || !(myObjectBuilder_GasContainerObject.GasLevel >= 1f))
										{
											if (!MySession.Static.CreativeMode)
											{
												MyFixedPoint a = destinationInventory.ComputeAmountThatFits(tmpInventoryItem.Content.GetId());
												if (maxAmount.HasValue)
												{
													a = MyFixedPoint.Min(a, maxAmount.Value);
												}
												if (tmpInventoryItem.Content.TypeId != typeof(MyObjectBuilder_Ore) && tmpInventoryItem.Content.TypeId != typeof(MyObjectBuilder_Ingot))
												{
													a = MyFixedPoint.Floor(a);
												}
												myFixedPoint = MyFixedPoint.Min(a, myFixedPoint);
											}
											if (!(myFixedPoint == 0))
											{
												if (maxAmount.HasValue)
												{
													maxAmount -= myFixedPoint;
												}
												result = true;
												MyInventory.Transfer(inventory, destinationInventory, tmpInventoryItem.Content.GetId(), MyItemFlags.None, myFixedPoint);
												if (destinationInventory.CargoPercentage >= 0.99f)
												{
													break;
												}
											}
										}
									}
								}
							}
							if (destinationInventory.CargoPercentage >= 0.99f)
							{
								break;
							}
						}
					}
					if (destinationInventory.CargoPercentage >= 0.99f)
					{
						break;
					}
				}
				return result;
			}
			if (!conveyorSystem.m_isRecomputingGraph)
			{
				conveyorSystem.RecomputeConveyorEndpoints();
			}
			return false;
		}

		public static bool PullAllRequest(IMyConveyorEndpointBlock start, MyInventory destinationInventory, long playerId, MyObjectBuilderType? typeId = null)
		{
			SetTraversalPlayerId(playerId);
			m_tmpRequestedItemSet.Set(typeId);
			bool result = ItemPullAll(start, destinationInventory);
			m_tmpRequestedItemSet.Clear();
			return result;
		}

		public static bool PullAllRequest(IMyConveyorEndpointBlock start, MyInventory destinationInventory, long playerId, bool all)
		{
			SetTraversalPlayerId(playerId);
			m_tmpRequestedItemSet.Set(all);
			bool result = ItemPullAll(start, destinationInventory);
			m_tmpRequestedItemSet.Clear();
			return result;
		}

		private static bool ItemPullAll(IMyConveyorEndpointBlock start, MyInventory destinationInventory, bool calcImmediately = false)
		{
			MyCubeBlock myCubeBlock = start as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			bool result = false;
			MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
			ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock2 = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock2 == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock2.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock2.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory)
						{
							continue;
						}
						MyPhysicalInventoryItem[] array = inventory.GetItems().ToArray();
						for (int k = 0; k < array.Length; k++)
						{
							MyDefinitionId definitionId = array[k].GetDefinitionId();
							MyFixedPoint myFixedPoint = destinationInventory.ComputeAmountThatFits(definitionId);
							if (!(myFixedPoint <= 0) && CanTransfer(start, conveyorEndpointMapping.pullElements[i], definitionId, isPush: false, calcImmediately))
							{
								MyFixedPoint value = MyFixedPoint.Min(inventory.GetItemAmount(definitionId), myFixedPoint);
								MyInventory.Transfer(inventory, destinationInventory, definitionId, MyItemFlags.None, value);
								result = true;
								if (destinationInventory.CargoPercentage >= 0.99f)
								{
									break;
								}
							}
						}
						if (destinationInventory.CargoPercentage >= 0.99f)
						{
							break;
						}
					}
					if (destinationInventory.CargoPercentage >= 0.99f)
					{
						break;
					}
				}
			}
			else if (!conveyorSystem.m_isRecomputingGraph)
			{
				conveyorSystem.RecomputeConveyorEndpoints();
			}
			return result;
		}

		public static void PrepareTraversal(IMyConveyorEndpoint startingVertex, Predicate<IMyConveyorEndpoint> vertexFilter = null, Predicate<IMyConveyorEndpoint> vertexTraversable = null, Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null)
		{
			lock (Pathfinding)
			{
				Pathfinding.PrepareTraversal(startingVertex, vertexFilter, vertexTraversable, edgeTraversable);
			}
		}

		public static bool ComputeCanTransfer(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock end, MyDefinitionId? itemId)
		{
			using (MyUtils.ReuseCollection(ref m_reachableBuffer))
			{
				lock (Pathfinding)
				{
					SetTraversalPlayerId(start.ConveyorEndpoint.CubeBlock.OwnerId);
					if (itemId.HasValue)
					{
						SetTraversalInventoryItemDefinitionId(itemId.Value);
					}
					else
					{
						SetTraversalInventoryItemDefinitionId();
					}
					Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null;
					if (itemId.HasValue && NeedsLargeTube(itemId.Value))
					{
						edgeTraversable = IsConveyorLargePredicate;
					}
					Pathfinding.FindReachable(start.ConveyorEndpoint, m_reachableBuffer, (IMyConveyorEndpoint b) => b != null && b.CubeBlock == end, IsAccessAllowedPredicate, edgeTraversable);
				}
				return m_reachableBuffer.Count != 0;
			}
		}

		private static bool CanTransfer(IMyConveyorEndpointBlock start, IMyConveyorEndpointBlock endPoint, MyDefinitionId itemId, bool isPush, bool calcImmediately = false)
		{
			ConveyorEndpointMapping conveyorEndpointMapping = (start as MyCubeBlock).CubeGrid.GridSystems.ConveyorSystem.GetConveyorEndpointMapping(start);
			if (calcImmediately)
			{
				bool canTransfer = false;
				if (conveyorEndpointMapping.TryGetTransfer(endPoint, itemId, isPush, out canTransfer))
				{
					return canTransfer;
				}
				TransferData transferData = new TransferData(start, endPoint, itemId, isPush);
				transferData.ComputeTransfer();
				transferData.StoreTransferState();
				return transferData.m_canTransfer;
			}
			bool canTransfer2 = true;
			if (conveyorEndpointMapping.TryGetTransfer(endPoint, itemId, isPush, out canTransfer2))
			{
				return canTransfer2;
			}
			Tuple<IMyConveyorEndpointBlock, IMyConveyorEndpointBlock> key = new Tuple<IMyConveyorEndpointBlock, IMyConveyorEndpointBlock>(start, endPoint);
			lock (m_currentTransferComputationTasks)
			{
				if (!m_currentTransferComputationTasks.ContainsKey(key))
				{
					TransferData workData = new TransferData(start, endPoint, itemId, isPush);
					Task value = Parallel.Start(ComputeTransferData, OnTransferDataComputed, workData);
					m_currentTransferComputationTasks.Add(key, value);
				}
			}
			return false;
		}

		private static void ComputeTransferData(WorkData workData)
		{
			TransferData transferData = workData as TransferData;
			if (transferData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				transferData.ComputeTransfer();
			}
		}

		private static void OnTransferDataComputed(WorkData workData)
		{
			if (workData == null && MyFakes.FORCE_NO_WORKER)
			{
				MyLog.Default.WriteLine("OnTransferDataComputed: workData is null on MyGridConveyorSystem to Check");
				return;
			}
			TransferData transferData = workData as TransferData;
			if (transferData == null)
			{
				workData.FlagAsFailed();
				return;
			}
			transferData.StoreTransferState();
			Tuple<IMyConveyorEndpointBlock, IMyConveyorEndpointBlock> key = new Tuple<IMyConveyorEndpointBlock, IMyConveyorEndpointBlock>(transferData.m_start, transferData.m_endPoint);
			lock (m_currentTransferComputationTasks)
			{
				m_currentTransferComputationTasks.Remove(key);
			}
		}

		internal bool PullItem(MyDefinitionId itemId, MyFixedPoint amount, IMyConveyorEndpointBlock start, MyInventory destinationInventory, bool calcImmediately = false)
		{
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				List<Tuple<MyInventory, MyFixedPoint>> list = new List<Tuple<MyInventory, MyFixedPoint>>();
				MyFixedPoint myFixedPoint = amount;
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory || !CanTransfer(start, conveyorEndpointMapping.pullElements[i], itemId, isPush: false, calcImmediately))
						{
							continue;
						}
						MyFixedPoint itemAmount = inventory.GetItemAmount(itemId);
						itemAmount = MyFixedPoint.Min(itemAmount, myFixedPoint);
						if (!(itemAmount == 0))
						{
							list.Add(new Tuple<MyInventory, MyFixedPoint>(inventory, itemAmount));
							myFixedPoint -= itemAmount;
							if (myFixedPoint == 0)
							{
								break;
							}
						}
					}
					if (myFixedPoint == 0)
					{
						break;
					}
				}
				if (myFixedPoint != 0)
				{
					return false;
				}
				foreach (Tuple<MyInventory, MyFixedPoint> item in list)
				{
					MyInventory.Transfer(item.Item1, destinationInventory, itemId, MyItemFlags.None, item.Item2);
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			return true;
		}

		public MyFixedPoint PullItem(MyDefinitionId itemId, MyFixedPoint? amount, IMyConveyorEndpointBlock start, MyInventory destinationInventory, bool remove, bool calcImmediately)
		{
			MyFixedPoint result = 0;
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pullElements != null)
			{
				for (int i = 0; i < conveyorEndpointMapping.pullElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pullElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanSend) == 0 || inventory == destinationInventory || !CanTransfer(start, conveyorEndpointMapping.pullElements[i], itemId, isPush: false, calcImmediately))
						{
							continue;
						}
						MyFixedPoint itemAmount = inventory.GetItemAmount(itemId);
						if (amount.HasValue)
						{
							itemAmount = (amount.HasValue ? MyFixedPoint.Min(itemAmount, amount.Value) : itemAmount);
							if (itemAmount == 0)
							{
								continue;
							}
							if (remove)
							{
								result += inventory.RemoveItemsOfType(itemAmount, itemId);
							}
							else
							{
								result += MyInventory.Transfer(inventory, destinationInventory, itemId, MyItemFlags.None, itemAmount);
							}
							amount -= itemAmount;
							if (amount.Value == 0)
							{
								return result;
							}
						}
						else if (remove)
						{
							result += inventory.RemoveItemsOfType(itemAmount, itemId);
						}
						else
						{
							result += MyInventory.Transfer(inventory, destinationInventory, itemId, MyItemFlags.None, itemAmount);
						}
						if (destinationInventory.CargoPercentage >= 0.99f)
						{
							break;
						}
					}
					if (destinationInventory.CargoPercentage >= 0.99f)
					{
						break;
					}
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			return result;
		}

		public static MyFixedPoint ConveyorSystemItemAmount(IMyConveyorEndpointBlock start, MyInventory destinationInventory, long playerId, MyDefinitionId itemId)
		{
			MyFixedPoint result = 0;
			using (new MyConveyorLine.InvertedConductivity())
			{
				lock (Pathfinding)
				{
					SetTraversalPlayerId(playerId);
					SetTraversalInventoryItemDefinitionId(itemId);
					PrepareTraversal(start.ConveyorEndpoint, null, IsAccessAllowedPredicate, NeedsLargeTube(itemId) ? IsConveyorLargePredicate : null);
					foreach (IMyConveyorEndpoint item in Pathfinding)
					{
						MyCubeBlock myCubeBlock = (item.CubeBlock != null && item.CubeBlock.HasInventory) ? item.CubeBlock : null;
						if (myCubeBlock != null)
						{
							for (int i = 0; i < myCubeBlock.InventoryCount; i++)
							{
								MyInventory inventory = myCubeBlock.GetInventory(i);
								if ((inventory.GetFlags() & MyInventoryFlags.CanSend) != 0 && inventory != destinationInventory)
								{
									result += inventory.GetItemAmount(itemId);
								}
							}
						}
					}
					return result;
				}
			}
		}

		public static void PushAnyRequest(IMyConveyorEndpointBlock start, MyInventory srcInventory, long playerId)
		{
			if (!srcInventory.Empty())
			{
				MyPhysicalInventoryItem[] array = srcInventory.GetItems().ToArray();
				foreach (MyPhysicalInventoryItem toSend in array)
				{
					ItemPushRequest(start, srcInventory, playerId, toSend);
				}
			}
		}

		public bool PushGenerateItem(MyDefinitionId itemDefId, MyFixedPoint? amount, IMyConveyorEndpointBlock start, bool partialPush = true, bool calcImmediately = false)
		{
			bool flag = false;
			List<Tuple<MyInventory, MyFixedPoint>> list = new List<Tuple<MyInventory, MyFixedPoint>>();
			ConveyorEndpointMapping conveyorEndpointMapping = GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pushElements != null)
			{
				MyFixedPoint myFixedPoint = 0;
				if (amount.HasValue)
				{
					myFixedPoint = amount.Value;
				}
				for (int i = 0; i < conveyorEndpointMapping.pushElements.Count; i++)
				{
					MyCubeBlock myCubeBlock = conveyorEndpointMapping.pushElements[i] as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanReceive) != 0)
						{
							MyFixedPoint a = inventory.ComputeAmountThatFits(itemDefId);
							a = MyFixedPoint.Min(a, myFixedPoint);
							if (inventory.CheckConstraint(itemDefId) && !(a == 0) && CanTransfer(start, conveyorEndpointMapping.pushElements[i], itemDefId, isPush: true, calcImmediately))
							{
								list.Add(new Tuple<MyInventory, MyFixedPoint>(inventory, a));
								myFixedPoint -= a;
							}
						}
					}
					if (myFixedPoint <= 0)
					{
						flag = true;
						break;
					}
				}
			}
			else if (!m_isRecomputingGraph)
			{
				RecomputeConveyorEndpoints();
			}
			if (flag || partialPush)
			{
				MyObjectBuilder_Base objectBuilder = MyObjectBuilderSerializer.CreateNewObject(itemDefId);
				{
					foreach (Tuple<MyInventory, MyFixedPoint> item in list)
					{
						item.Item1.AddItems(item.Item2, objectBuilder);
					}
					return flag;
				}
			}
			return flag;
		}

		public static bool ItemPushRequest(IMyConveyorEndpointBlock start, MyInventory srcInventory, long playerId, MyPhysicalInventoryItem toSend, MyFixedPoint? amount = null)
		{
			MyCubeBlock myCubeBlock = start as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			bool result = false;
			MyGridConveyorSystem conveyorSystem = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem;
			ConveyorEndpointMapping conveyorEndpointMapping = conveyorSystem.GetConveyorEndpointMapping(start);
			if (conveyorEndpointMapping.pushElements != null)
			{
				MyDefinitionId id = toSend.Content.GetId();
				MyFixedPoint myFixedPoint = toSend.Amount;
				if (amount.HasValue)
				{
					myFixedPoint = amount.Value;
				}
				for (int i = 0; i < conveyorEndpointMapping.pushElements.Count; i++)
				{
					MyCubeBlock myCubeBlock2 = conveyorEndpointMapping.pushElements[i] as MyCubeBlock;
					if (myCubeBlock2 == null)
					{
						continue;
					}
					int inventoryCount = myCubeBlock2.InventoryCount;
					for (int j = 0; j < inventoryCount; j++)
					{
						MyInventory inventory = myCubeBlock2.GetInventory(j);
						if ((inventory.GetFlags() & MyInventoryFlags.CanReceive) != 0 && inventory != srcInventory)
						{
							MyFixedPoint a = inventory.ComputeAmountThatFits(id);
							a = MyFixedPoint.Min(a, myFixedPoint);
							if (inventory.CheckConstraint(id) && !(a == 0) && CanTransfer(start, conveyorEndpointMapping.pushElements[i], toSend.GetDefinitionId(), isPush: true))
							{
								MyInventory.Transfer(srcInventory, inventory, toSend.ItemId, -1, a);
								result = true;
								myFixedPoint -= a;
							}
						}
					}
					if (myFixedPoint <= 0)
					{
						break;
					}
				}
			}
			else if (!conveyorSystem.m_isRecomputingGraph)
			{
				conveyorSystem.RecomputeConveyorEndpoints();
			}
			return result;
		}

		private void RecomputeConveyorEndpoints()
		{
			m_conveyorConnections.Clear();
			if (m_isRecomputingGraph)
			{
				m_isRecomputationIsAborted = true;
			}
			else
			{
				StartRecomputationThread();
			}
		}

		private void StartRecomputationThread()
		{
			m_conveyorConnectionsForThread.Clear();
			m_isRecomputingGraph = true;
			m_isRecomputationIsAborted = false;
			m_isRecomputationInterrupted = false;
			m_endpointIterator = null;
			Parallel.Start(UpdateConveyorEndpointMapping, OnConveyorEndpointMappingUpdateCompleted);
		}

		public static void RecomputeMappingForBlock(IMyConveyorEndpointBlock processedBlock)
		{
			MyCubeBlock myCubeBlock = processedBlock as MyCubeBlock;
			if (myCubeBlock == null || myCubeBlock.CubeGrid == null || myCubeBlock.CubeGrid.GridSystems == null || myCubeBlock.CubeGrid.GridSystems.ConveyorSystem == null)
			{
				return;
			}
			ConveyorEndpointMapping value = myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.ComputeMappingForBlock(processedBlock);
			if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections.ContainsKey(processedBlock))
			{
				myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections[processedBlock] = value;
			}
			else
			{
				myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnections.Add(processedBlock, value);
			}
			if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_isRecomputingGraph)
			{
				if (myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread.ContainsKey(processedBlock))
				{
					myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread[processedBlock] = value;
				}
				else
				{
					myCubeBlock.CubeGrid.GridSystems.ConveyorSystem.m_conveyorConnectionsForThread.Add(processedBlock, value);
				}
			}
		}

		private ConveyorEndpointMapping ComputeMappingForBlock(IMyConveyorEndpointBlock processedBlock)
		{
			ConveyorEndpointMapping conveyorEndpointMapping = new ConveyorEndpointMapping();
			PullInformation pullInformation = processedBlock.GetPullInformation();
			if (pullInformation != null)
			{
				conveyorEndpointMapping.pullElements = new List<IMyConveyorEndpointBlock>();
				lock (Pathfinding)
				{
					SetTraversalPlayerId(pullInformation.OwnerID);
					if (pullInformation.ItemDefinition != default(MyDefinitionId))
					{
						SetTraversalInventoryItemDefinitionId(pullInformation.ItemDefinition);
						using (new MyConveyorLine.InvertedConductivity())
						{
							PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate, NeedsLargeTube(pullInformation.ItemDefinition) ? IsConveyorLargePredicate : null);
							AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pullElements, MyInventoryFlags.CanSend);
						}
					}
					else if (pullInformation.Constraint != null)
					{
						SetTraversalInventoryItemDefinitionId();
						using (new MyConveyorLine.InvertedConductivity())
						{
							PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
							AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pullElements, MyInventoryFlags.CanSend);
						}
					}
				}
			}
			PullInformation pushInformation = processedBlock.GetPushInformation();
			if (pushInformation != null)
			{
				conveyorEndpointMapping.pushElements = new List<IMyConveyorEndpointBlock>();
				lock (Pathfinding)
				{
					SetTraversalPlayerId(pushInformation.OwnerID);
					HashSet<MyDefinitionId> hashSet = new HashSet<MyDefinitionId>();
					if (pushInformation.ItemDefinition != default(MyDefinitionId))
					{
						hashSet.Add(pushInformation.ItemDefinition);
					}
					if (pushInformation.Constraint != null)
					{
						foreach (MyDefinitionId constrainedId in pushInformation.Constraint.ConstrainedIds)
						{
							hashSet.Add(constrainedId);
						}
						foreach (MyObjectBuilderType constrainedType in pushInformation.Constraint.ConstrainedTypes)
						{
							MyDefinitionManager.Static.TryGetDefinitionsByTypeId(constrainedType, hashSet);
						}
					}
					if (hashSet.Count != 0 || (pushInformation.Constraint != null && !(pushInformation.Constraint.Description == "Empty constraint")))
					{
						foreach (MyDefinitionId item in hashSet)
						{
							SetTraversalInventoryItemDefinitionId(item);
							if (NeedsLargeTube(item))
							{
								PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate, IsConveyorLargePredicate);
							}
							else
							{
								PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
							}
							AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive, item);
						}
						return conveyorEndpointMapping;
					}
					SetTraversalInventoryItemDefinitionId();
					PrepareTraversal(processedBlock.ConveyorEndpoint, null, IsAccessAllowedPredicate);
					AddReachableEndpoints(processedBlock, conveyorEndpointMapping.pushElements, MyInventoryFlags.CanReceive);
					return conveyorEndpointMapping;
				}
			}
			return conveyorEndpointMapping;
		}

		private static void AddReachableEndpoints(IMyConveyorEndpointBlock processedBlock, List<IMyConveyorEndpointBlock> resultList, MyInventoryFlags flagToCheck, MyDefinitionId? definitionId = null)
		{
			foreach (IMyConveyorEndpoint item in Pathfinding)
			{
				if ((item.CubeBlock != processedBlock || processedBlock.AllowSelfPulling()) && item.CubeBlock != null && item.CubeBlock.HasInventory)
				{
					IMyConveyorEndpointBlock myConveyorEndpointBlock = item.CubeBlock as IMyConveyorEndpointBlock;
					if (myConveyorEndpointBlock != null)
					{
						MyCubeBlock cubeBlock = item.CubeBlock;
						bool flag = false;
						for (int i = 0; i < cubeBlock.InventoryCount; i++)
						{
							MyInventory inventory = cubeBlock.GetInventory(i);
							if ((inventory.GetFlags() & flagToCheck) != 0 && (!definitionId.HasValue || inventory.CheckConstraint(definitionId.Value)))
							{
								flag = true;
								break;
							}
						}
						if (flag && !resultList.Contains(myConveyorEndpointBlock))
						{
							resultList.Add(myConveyorEndpointBlock);
						}
					}
				}
			}
		}

		private void UpdateConveyorEndpointMapping()
		{
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				long timestamp = Stopwatch.GetTimestamp();
				m_isRecomputationInterrupted = false;
				if (m_endpointIterator == null)
				{
					m_endpointIterator = m_conveyorEndpointBlocks.GetEnumerator();
					m_endpointIterator.MoveNext();
				}
				IMyConveyorEndpointBlock current = m_endpointIterator.Current;
				while (true)
				{
					if (current == null)
					{
						return;
					}
					if (m_isRecomputationIsAborted)
					{
						m_isRecomputationInterrupted = true;
						return;
					}
					if (new TimeSpan(Stopwatch.GetTimestamp() - timestamp).TotalMilliseconds > 10.0)
					{
						m_isRecomputationInterrupted = true;
						return;
					}
					ConveyorEndpointMapping value = ComputeMappingForBlock(current);
					m_conveyorConnectionsForThread.Add(current, value);
					if (m_endpointIterator == null)
					{
						break;
					}
					m_endpointIterator.MoveNext();
					current = m_endpointIterator.Current;
				}
				m_isRecomputationIsAborted = true;
				m_isRecomputationInterrupted = true;
			}
		}

		private void OnConveyorEndpointMappingUpdateCompleted()
		{
			using (m_iteratorLock.AcquireExclusiveUsing())
			{
				if (m_isRecomputationIsAborted)
				{
					StartRecomputationThread();
				}
				else
				{
					foreach (KeyValuePair<IMyConveyorEndpointBlock, ConveyorEndpointMapping> item in m_conveyorConnectionsForThread)
					{
						if (m_conveyorConnections.ContainsKey(item.Key))
						{
							m_conveyorConnections[item.Key] = item.Value;
						}
						else
						{
							m_conveyorConnections.Add(item.Key, item.Value);
						}
					}
					m_conveyorConnectionsForThread.Clear();
					if (m_isRecomputationInterrupted)
					{
						Parallel.Start(UpdateConveyorEndpointMapping, OnConveyorEndpointMappingUpdateCompleted);
					}
					else
					{
						m_endpointIterator = null;
						m_isRecomputingGraph = false;
						foreach (MyConveyorLine line in m_lines)
						{
							line.UpdateIsWorking();
						}
					}
				}
			}
		}

		private ConveyorEndpointMapping GetConveyorEndpointMapping(IMyConveyorEndpointBlock block)
		{
			if (m_conveyorConnections.ContainsKey(block))
			{
				return m_conveyorConnections[block];
			}
			return new ConveyorEndpointMapping();
		}

		public static void FindReachable(IMyConveyorEndpoint from, List<IMyConveyorEndpoint> reachableVertices, Predicate<IMyConveyorEndpoint> vertexFilter = null, Predicate<IMyConveyorEndpoint> vertexTraversable = null, Predicate<IMyPathEdge<IMyConveyorEndpoint>> edgeTraversable = null)
		{
			lock (Pathfinding)
			{
				Pathfinding.FindReachable(from, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
			}
		}

		public static bool Reachable(IMyConveyorEndpoint from, IMyConveyorEndpoint to)
		{
			bool flag = false;
			lock (Pathfinding)
			{
				return Pathfinding.Reachable(from, to);
			}
		}
	}
}
