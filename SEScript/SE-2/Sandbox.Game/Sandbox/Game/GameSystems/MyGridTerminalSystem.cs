using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGridTerminalSystem : Sandbox.ModAPI.IMyGridTerminalSystem, Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
	{
		private readonly int m_oreDetectorCounterValue = 50;

		private readonly HashSet<MyTerminalBlock> m_blocks = new HashSet<MyTerminalBlock>();

		private readonly List<MyTerminalBlock> m_blockList = new List<MyTerminalBlock>();

		private readonly Dictionary<long, MyTerminalBlock> m_blockTable = new Dictionary<long, MyTerminalBlock>();

		private readonly HashSet<MyTerminalBlock> m_blocksToShowOnHud = new HashSet<MyTerminalBlock>();

		private readonly HashSet<MyTerminalBlock> m_currentlyHackedBlocks = new HashSet<MyTerminalBlock>();

		private readonly List<MyBlockGroup> m_blockGroups = new List<MyBlockGroup>();

		private readonly HashSet<MyTerminalBlock> m_blocksForHud = new HashSet<MyTerminalBlock>();

		private List<string> m_debugChanges = new List<string>();

		private int m_lastHudIndex;

		private int m_oreDetectorUpdateCounter;

		private bool m_needsHudUpdate = true;

		private int m_hudLastUpdated;

		public bool NeedsHudUpdate
		{
			get
			{
				return m_needsHudUpdate;
			}
			set
			{
				if (m_needsHudUpdate != value)
				{
					m_blocksForHud.ForEach(delegate(MyTerminalBlock x)
					{
						x.CubeGrid.MarkForUpdate();
					});
					m_needsHudUpdate = value;
				}
			}
		}

		public int HudLastUpdated => m_hudLastUpdated;

		public HashSetReader<MyTerminalBlock> Blocks => new HashSetReader<MyTerminalBlock>(m_blocks);

		public HashSetReader<MyTerminalBlock> HudBlocks => new HashSetReader<MyTerminalBlock>(m_blocksForHud);

		public List<MyBlockGroup> BlockGroups => m_blockGroups;

		public event Action<MyTerminalBlock> BlockAdded;

		public event Action<MyTerminalBlock> BlockRemoved;

		public event Action BlockManipulationFinished;

		public event Action<MyBlockGroup> GroupAdded;

		public event Action<MyBlockGroup> GroupRemoved;

		public void IncrementHudLastUpdated()
		{
			m_hudLastUpdated++;
		}

		public void Add(MyTerminalBlock block)
		{
			if (!block.MarkedForClose && !block.IsBeingRemoved && !MyEntities.IsClosingAll && !m_blockTable.ContainsKey(block.EntityId))
			{
				m_blockTable.Add(block.EntityId, block);
				m_blocks.Add(block);
				m_blockList.Add(block);
				this.BlockAdded?.Invoke(block);
			}
		}

		public void Remove(MyTerminalBlock block)
		{
			if (block.MarkedForClose || MyEntities.IsClosingAll)
			{
				return;
			}
			m_blockTable.Remove(block.EntityId);
			m_blocks.Remove(block);
			m_blockList.Remove(block);
			m_blocksForHud.Remove(block);
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				myBlockGroup.Blocks.Remove(block);
				if (myBlockGroup.Blocks.Count == 0)
				{
					RemoveGroup(myBlockGroup, !block.IsBeingRemoved);
					i--;
				}
			}
			this.BlockRemoved?.Invoke(block);
		}

		public MyBlockGroup AddUpdateGroup(MyBlockGroup gridGroup, bool fireEvent, bool modify = false)
		{
			if (gridGroup.Blocks.Count == 0)
			{
				return null;
			}
			MyBlockGroup myBlockGroup = BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(gridGroup.Name) == 0);
			if (myBlockGroup == null)
			{
				myBlockGroup = new MyBlockGroup();
				myBlockGroup.Name.Clear().AppendStringBuilder(gridGroup.Name);
				BlockGroups.Add(myBlockGroup);
			}
			if (modify)
			{
				myBlockGroup.Blocks.Clear();
			}
			myBlockGroup.Blocks.UnionWith(gridGroup.Blocks);
			if (fireEvent && this.GroupAdded != null)
			{
				this.GroupAdded(gridGroup);
			}
			return gridGroup;
		}

		public void RemoveGroup(MyBlockGroup gridGroup, bool fireEvent)
		{
			MyBlockGroup myBlockGroup = BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(gridGroup.Name) == 0);
			if (myBlockGroup != null)
			{
				List<MyTerminalBlock> list = new List<MyTerminalBlock>();
				foreach (MyTerminalBlock block in gridGroup.Blocks)
				{
					if (myBlockGroup.Blocks.Contains(block))
					{
						list.Add(block);
					}
				}
				foreach (MyTerminalBlock item in list)
				{
					myBlockGroup.Blocks.Remove(item);
				}
				if (myBlockGroup.Blocks.Count == 0)
				{
					BlockGroups.Remove(myBlockGroup);
				}
			}
			if (fireEvent && this.GroupRemoved != null)
			{
				this.GroupRemoved(gridGroup);
			}
		}

		public void CopyBlocksTo(List<MyTerminalBlock> result)
		{
			foreach (MyTerminalBlock block in m_blocks)
			{
				result.Add(block);
			}
		}

		public void UpdateGridBlocksOwnership(long ownerID)
		{
			foreach (MyTerminalBlock block in m_blocks)
			{
				block.IsAccessibleForProgrammableBlock = block.HasPlayerAccess(ownerID);
			}
		}

		public void UpdateHud()
		{
			if (!NeedsHudUpdate)
			{
				return;
			}
			if (m_lastHudIndex < m_blocks.Count)
			{
				MyTerminalBlock myTerminalBlock = m_blockList[m_lastHudIndex];
				if (MeetsHudConditions(myTerminalBlock))
				{
					m_blocksForHud.Add(myTerminalBlock);
				}
				else
				{
					m_blocksForHud.Remove(myTerminalBlock);
				}
				m_lastHudIndex++;
			}
			else
			{
				m_lastHudIndex = 0;
				NeedsHudUpdate = false;
			}
			m_hudLastUpdated = 0;
		}

		private bool MeetsHudConditions(MyTerminalBlock terminalBlock)
		{
			if (terminalBlock.HasLocalPlayerAccess() && (terminalBlock.ShowOnHUD || (terminalBlock.IsBeingHacked && terminalBlock.IDModule != null && terminalBlock.IDModule.Owner != 0L) || (terminalBlock is MyCockpit && (terminalBlock as MyCockpit).Pilot != null)))
			{
				return true;
			}
			if (terminalBlock.HasLocalPlayerAccess() && terminalBlock.IDModule != null && terminalBlock.IDModule.Owner != 0L)
			{
				_ = (terminalBlock is IMyComponentOwner<MyOreDetectorComponent>);
			}
			return false;
		}

		internal void BlockManipulationFinishedFunction()
		{
			this.BlockManipulationFinished?.Invoke();
		}

		[Conditional("DEBUG")]
		private void RecordChange(string text)
		{
			m_debugChanges.Add(DateTime.Now.ToLongTimeString() + ": " + text);
			if (m_debugChanges.Count > 10)
			{
				m_debugChanges.RemoveAt(0);
			}
		}

		public void DebugDraw(MyEntity entity)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)
			{
				return;
			}
			double scaleFactor = 6.5 * 0.045;
			Vector3D vector3D = entity.WorldMatrix.Translation;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.GetPhysicalGroupAABB();
				vector3D = myCubeGrid.GetPhysicalGroupAABB().Center;
				if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					vector3D -= new Vector3D(0.0, 5.0, 0.0);
				}
			}
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(vector3D, position);
			float num = (float)Math.Atan(6.5 / Math.Max(val, 0.001));
			if (!(num <= 0.27f))
			{
				MyRenderProxy.DebugDrawText3D(vector3D, entity.ToString(), Color.Yellow, num, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				int num2 = -1;
				MyRenderProxy.DebugDrawText3D(vector3D + num2 * up * scaleFactor + right * 0.064999997615814209, $"Blocks: {m_blocks.Count}", Color.LightYellow, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num2--;
				MyRenderProxy.DebugDrawText3D(vector3D + num2 * up * scaleFactor + right * 0.064999997615814209, $"Groups: {m_blockGroups.Count}", Color.LightYellow, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num2--;
				MyRenderProxy.DebugDrawText3D(vector3D + num2 * up * scaleFactor + right * 0.064999997615814209, "Recent group changes:", Color.LightYellow, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num2--;
				foreach (string debugChange in m_debugChanges)
				{
					MyRenderProxy.DebugDrawText3D(vector3D + num2 * up * scaleFactor + right * 0.064999997615814209, debugChange, Color.White, num, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
					num2--;
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocks(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks)
		{
			blocks.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (block.IsAccessibleForProgrammableBlock)
				{
					blocks.Add(block);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroups(List<Sandbox.ModAPI.Ingame.IMyBlockGroup> blockGroups, Func<Sandbox.ModAPI.Ingame.IMyBlockGroup, bool> collect)
		{
			blockGroups?.Clear();
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				if (collect == null || collect(myBlockGroup))
				{
					blockGroups?.Add(myBlockGroup);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				T val = block as T;
				if (val != null && block.IsAccessibleForProgrammableBlock && (collect == null || collect(val)))
				{
					blocks?.Add(val);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (block as T != null && block.IsAccessibleForProgrammableBlock && (collect == null || collect(block)))
				{
					blocks?.Add(block);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.SearchBlocksOfName(string name, List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (StringExtensions.Contains(block.CustomName.ToString(), name, StringComparison.OrdinalIgnoreCase) && block.IsAccessibleForProgrammableBlock && (collect == null || collect(block)))
				{
					blocks?.Add(block);
				}
			}
		}

		Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithName(string name)
		{
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (block.CustomName.CompareTo(name) == 0 && block.IsAccessibleForProgrammableBlock)
				{
					return block;
				}
			}
			return null;
		}

		Sandbox.ModAPI.Ingame.IMyBlockGroup Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroupWithName(string name)
		{
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				if (myBlockGroup.Name.CompareTo(name) == 0)
				{
					return myBlockGroup;
				}
			}
			return null;
		}

		Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithId(long id)
		{
			if (m_blockTable.TryGetValue(id, out MyTerminalBlock value) && value.IsAccessibleForProgrammableBlock)
			{
				return value;
			}
			return null;
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlocks(List<Sandbox.ModAPI.IMyTerminalBlock> blocks)
		{
			blocks.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				blocks.Add(block);
			}
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockGroups(List<Sandbox.ModAPI.IMyBlockGroup> blockGroups)
		{
			blockGroups.Clear();
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				blockGroups.Add(blockGroup);
			}
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlocksOfType<T>(List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			blocks.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (block is T && (collect == null || collect(block)))
				{
					blocks.Add(block);
				}
			}
		}

		void Sandbox.ModAPI.IMyGridTerminalSystem.SearchBlocksOfName(string name, List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			blocks.Clear();
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (StringExtensions.Contains(block.CustomName.ToString(), name, StringComparison.OrdinalIgnoreCase) && (collect == null || collect(block)))
				{
					blocks.Add(block);
				}
			}
		}

		Sandbox.ModAPI.IMyTerminalBlock Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockWithName(string name)
		{
			foreach (MyTerminalBlock block in m_blocks)
			{
				if (block.CustomName.ToString() == name)
				{
					return block;
				}
			}
			return null;
		}

		Sandbox.ModAPI.IMyBlockGroup Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockGroupWithName(string name)
		{
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				if (blockGroup.Name.ToString() == name)
				{
					return blockGroup;
				}
			}
			return null;
		}
	}
}
