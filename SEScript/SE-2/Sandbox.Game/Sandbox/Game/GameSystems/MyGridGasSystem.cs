using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGridGasSystem
	{
		private static bool DEBUG_MODE = false;

		public const float OXYGEN_UNIFORMIZATION_TIME_MS = 1500f;

		private readonly Vector3I[] m_neighbours = new Vector3I[6]
		{
			new Vector3I(1, 0, 0),
			new Vector3I(-1, 0, 0),
			new Vector3I(0, 1, 0),
			new Vector3I(0, -1, 0),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 0, -1)
		};

		private readonly Vector3I[] m_neighboursForDelete = new Vector3I[7]
		{
			new Vector3I(1, 0, 0),
			new Vector3I(-1, 0, 0),
			new Vector3I(0, 1, 0),
			new Vector3I(0, -1, 0),
			new Vector3I(0, 0, 1),
			new Vector3I(0, 0, -1),
			new Vector3I(0, 0, 0)
		};

		private readonly IMyCubeGrid m_cubeGrid;

		private static readonly MySoundPair m_airleakSound = new MySoundPair("EventAirVent");

		private bool m_isProcessingData;

		private MyOxygenCube m_cubeRoom;

		private MyConcurrentList<MyOxygenRoom> m_rooms;

		private int m_lastRoomIndex;

		private Queue<Vector3I> m_blockQueue = new Queue<Vector3I>();

		private Vector3I m_storedGridMin;

		private Vector3I m_storedGridMax;

		private Vector3I m_previousGridMin;

		private Vector3I m_previousGridMax;

		private OxygenRoom[] m_savedRooms;

		private List<IMySlimBlock> m_gasBlocks = new List<IMySlimBlock>();

		private List<IMySlimBlock> m_gasBlocksForUpdate = new List<IMySlimBlock>();

		private bool m_generatedDataPending;

		private bool m_gridExpanded;

		private bool m_gridShrinked;

		private List<IMySlimBlock> m_deletedBlocks = new List<IMySlimBlock>();

		private List<IMySlimBlock> m_deletedBlocksSwap = new List<IMySlimBlock>();

		private List<IMySlimBlock> m_addedBlocks = new List<IMySlimBlock>();

		private List<IMySlimBlock> m_addedBlocksSwap = new List<IMySlimBlock>();

		private Task m_backgroundTask;

		private int m_lastUpdateTime;

		private bool isClosing;

		private HashSet<Vector3I> m_visitedBlocks = new HashSet<Vector3I>();

		private HashSet<Vector3I> m_initializedBlocks = new HashSet<Vector3I>();

		private readonly float m_debugTextlineSize = 17f;

		private bool m_debugShowTopRoom;

		private bool m_debugShowRoomIndex = true;

		private bool m_debugShowPositions;

		private int m_debugRoomIndex;

		private bool m_debugShowBlockCount;

		private bool m_debugShowOxygenAmount;

		private bool m_debugToggleView;

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (!m_isProcessingData)
				{
					if (!m_generatedDataPending && !m_gridShrinked && !m_gridExpanded && m_deletedBlocks.Count <= 0)
					{
						return m_addedBlocks.Count > 0;
					}
					return true;
				}
				return false;
			}
		}

		public MyGridGasSystem(IMyCubeGrid cubeGrid)
		{
			m_cubeGrid = cubeGrid;
			cubeGrid.OnBlockAdded += cubeGrid_OnBlockAdded;
			cubeGrid.OnBlockRemoved += cubeGrid_OnBlockRemoved;
			m_lastUpdateTime = GetTotalGamePlayTimeInMilliseconds();
		}

		private int GetTotalGamePlayTimeInMilliseconds()
		{
			return MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void MarkCubeGridForUpdate()
		{
			(m_cubeGrid as MyCubeGrid)?.MarkForUpdate();
		}

		public void OnGridClosing()
		{
			isClosing = true;
			if (m_isProcessingData)
			{
				try
				{
					m_backgroundTask.WaitOrExecute();
				}
				catch (Exception ex)
				{
					MySandboxGame.Log.WriteLineAndConsole("MyGridGasSystem.OnGridClosing: " + ex.Message + ", " + ex.StackTrace);
				}
			}
			m_cubeGrid.OnBlockAdded -= cubeGrid_OnBlockAdded;
			m_cubeGrid.OnBlockRemoved -= cubeGrid_OnBlockRemoved;
			MyCubeGrid myCubeGrid = m_cubeGrid as MyCubeGrid;
			if (myCubeGrid != null)
			{
				foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
				{
					Sandbox.ModAPI.IMyDoor myDoor = fatBlock as Sandbox.ModAPI.IMyDoor;
					if (myDoor != null)
					{
						myDoor.OnDoorStateChanged -= OnDoorStateChanged;
					}
				}
			}
			Clear();
		}

		private void Clear()
		{
			m_rooms = null;
			m_cubeRoom = null;
			m_lastRoomIndex = 0;
			m_visitedBlocks.Clear();
			m_initializedBlocks.Clear();
		}

		private void cubeGrid_OnBlockAdded(IMySlimBlock addedBlock)
		{
			if (addedBlock.FatBlock is Sandbox.ModAPI.IMyDoor)
			{
				((Sandbox.ModAPI.IMyDoor)addedBlock.FatBlock).OnDoorStateChanged += OnDoorStateChanged;
			}
			IMyGasBlock myGasBlock = addedBlock.FatBlock as IMyGasBlock;
			if (myGasBlock != null && myGasBlock.CanPressurizeRoom)
			{
				m_gasBlocks.Add(addedBlock);
			}
			if (m_gasBlocks.Count != 0)
			{
				m_addedBlocks.Add(addedBlock);
				Vector3I vector3I = (!m_isProcessingData) ? m_storedGridMin : m_previousGridMin;
				Vector3I vector3I2 = (!m_isProcessingData) ? m_storedGridMax : m_previousGridMax;
				if (Vector3I.Min(GridMin(), vector3I) != vector3I || Vector3I.Max(GridMax(), vector3I2) != vector3I2)
				{
					m_gridExpanded = true;
				}
				if (m_rooms == null)
				{
					m_generatedDataPending = true;
				}
				MarkCubeGridForUpdate();
			}
		}

		internal void OnSlimBlockBuildRatioRaised(IMySlimBlock block)
		{
			MyCubeBlockDefinition myCubeBlockDefinition = block.BlockDefinition as MyCubeBlockDefinition;
			if (myCubeBlockDefinition != null && myCubeBlockDefinition.BuildProgressModels != null && myCubeBlockDefinition.BuildProgressModels.Length != 0)
			{
				MyCubeBlockDefinition.BuildProgressModel buildProgressModel = myCubeBlockDefinition.BuildProgressModels[myCubeBlockDefinition.BuildProgressModels.Length - 1];
				if (block.BuildLevelRatio >= buildProgressModel.BuildRatioUpperBound)
				{
					cubeGrid_OnBlockAdded(block);
				}
			}
		}

		private void cubeGrid_OnBlockRemoved(IMySlimBlock deletedBlock)
		{
			Sandbox.ModAPI.IMyDoor myDoor = deletedBlock.FatBlock as Sandbox.ModAPI.IMyDoor;
			if (myDoor != null)
			{
				myDoor.OnDoorStateChanged -= OnDoorStateChanged;
			}
			IMyGasBlock myGasBlock = deletedBlock.FatBlock as IMyGasBlock;
			if (myGasBlock != null && myGasBlock.CanPressurizeRoom)
			{
				m_gasBlocks.Remove(deletedBlock);
			}
			if (m_gasBlocks.Count != 0 || myGasBlock != null)
			{
				m_deletedBlocks.Add(deletedBlock);
				MarkCubeGridForUpdate();
			}
		}

		internal void OnSlimBlockBuildRatioLowered(IMySlimBlock block)
		{
			MyCubeBlockDefinition myCubeBlockDefinition = block.BlockDefinition as MyCubeBlockDefinition;
			if (myCubeBlockDefinition == null || myCubeBlockDefinition.BuildProgressModels == null || myCubeBlockDefinition.BuildProgressModels.Length == 0)
			{
				return;
			}
			int num = 0;
			for (int num2 = myCubeBlockDefinition.BuildProgressModels.Length - 1; num2 >= 0; num2--)
			{
				if (myCubeBlockDefinition.BuildProgressModels[num2].BuildRatioUpperBound > block.BuildLevelRatio)
				{
					num = num2;
				}
			}
			if (num == myCubeBlockDefinition.BuildProgressModels.Length - 1)
			{
				cubeGrid_OnBlockRemoved(block);
			}
		}

		private void OnDoorStateChanged(Sandbox.ModAPI.IMyDoor door, bool areOpen)
		{
			if (m_gasBlocks.Count == 0)
			{
				return;
			}
			MySlimBlock mySlimBlock = door.SlimBlock as MySlimBlock;
			if (mySlimBlock != null)
			{
				if (areOpen)
				{
					m_deletedBlocks.Add(mySlimBlock);
				}
				else
				{
					m_addedBlocks.Add(mySlimBlock);
				}
			}
			MarkCubeGridForUpdate();
		}

		public void OnCubeGridShrinked()
		{
			if (m_rooms == null)
			{
				m_generatedDataPending = true;
			}
			else
			{
				m_gridShrinked = true;
			}
			MarkCubeGridForUpdate();
		}

		public void UpdateBeforeSimulation()
		{
			if (!MyFakes.BACKGROUND_OXYGEN || m_isProcessingData)
			{
				return;
			}
			MySimpleProfiler.Begin("Gas System", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
			if (m_generatedDataPending)
			{
				if (MyFakes.BACKGROUND_OXYGEN && ShouldPressurize())
				{
					StartGenerateAirtightData();
				}
				m_generatedDataPending = false;
			}
			if (m_gridShrinked)
			{
				StartShrinkData();
			}
			if (m_addedBlocks.Count > 0)
			{
				StartRefreshRoomData();
			}
			if (m_deletedBlocks.Count > 0)
			{
				StartRemoveBlocks();
			}
			MySimpleProfiler.End("UpdateBeforeSimulation");
		}

		public void UpdateBeforeSimulation100()
		{
		}

		public void OnAltitudeChanged()
		{
			if (m_rooms != null)
			{
				foreach (MyOxygenRoom room in m_rooms)
				{
					room.EnvironmentOxygen = MyOxygenProviderSystem.GetOxygenInPoint(m_cubeGrid.GridIntegerToWorld(room.StartingPosition));
				}
				MarkCubeGridForUpdate();
			}
		}

		private bool ShouldPressurize()
		{
			if (m_cubeGrid.Physics == null)
			{
				return false;
			}
			if (m_gasBlocks.Count > 0)
			{
				return true;
			}
			if (m_rooms == null)
			{
				return false;
			}
			for (int i = 0; i < m_rooms.Count; i++)
			{
				MyOxygenRoom myOxygenRoom = m_rooms[i];
				if (myOxygenRoom.IsAirtight && myOxygenRoom.OxygenAmount > 1f)
				{
					return true;
				}
				if (!myOxygenRoom.IsAirtight && (float)(GetTotalGamePlayTimeInMilliseconds() - myOxygenRoom.DepressurizationTime) < 1500f)
				{
					return true;
				}
			}
			m_rooms = null;
			m_lastRoomIndex = 0;
			m_cubeRoom = null;
			return false;
		}

		private void StartShrinkData()
		{
			if (!m_isProcessingData)
			{
				m_previousGridMin = m_storedGridMin;
				m_previousGridMax = m_storedGridMax;
				m_isProcessingData = true;
				m_gridShrinked = false;
				m_backgroundTask = Parallel.Start(ShrinkData, OnBackgroundTaskFinished);
			}
		}

		private void ShrinkData()
		{
			if (m_cubeRoom == null)
			{
				return;
			}
			Vector3I vector3I = GridMin();
			Vector3I vector3I2 = GridMax();
			Vector3I a = vector3I - m_storedGridMin;
			Vector3I a2 = m_storedGridMax - vector3I2;
			if (a != Vector3I.Zero || a2 != Vector3I.Zero)
			{
				m_storedGridMin = vector3I;
				m_storedGridMax = vector3I2;
				MyOxygenRoom myOxygenRoom = m_rooms[0];
				HashSet<Vector3I> hashSet = new HashSet<Vector3I>();
				foreach (Vector3I block in myOxygenRoom.Blocks)
				{
					if (!IsInBounds(block))
					{
						hashSet.Add(block);
					}
				}
				if (hashSet.Count > 0)
				{
					myOxygenRoom.Blocks.ExceptWith(hashSet);
					myOxygenRoom.BlockCount = myOxygenRoom.Blocks.Count;
					myOxygenRoom.StartingPosition = m_storedGridMin;
				}
			}
		}

		private void StartRefreshRoomData()
		{
			if (m_isProcessingData)
			{
				return;
			}
			if (m_cubeRoom == null)
			{
				m_addedBlocks.Clear();
				m_gridExpanded = false;
				return;
			}
			if (m_gridExpanded)
			{
				m_previousGridMin = m_storedGridMin;
				m_previousGridMax = m_storedGridMax;
			}
			List<IMySlimBlock> addedBlocksSwap = m_addedBlocksSwap;
			m_addedBlocksSwap = m_addedBlocks;
			m_addedBlocks = addedBlocksSwap;
			m_gasBlocksForUpdate.Clear();
			m_gasBlocksForUpdate.AddRange(m_gasBlocks);
			m_isProcessingData = true;
			m_backgroundTask = Parallel.Start(RefreshRoomData, OnBackgroundTaskFinished);
		}

		private void RefreshRoomData()
		{
			if (m_cubeRoom != null)
			{
				if (m_gridExpanded)
				{
					m_gridExpanded = false;
					ExpandAirtightData();
				}
				foreach (IMySlimBlock item in m_addedBlocksSwap)
				{
					AddBlock(item);
				}
				m_addedBlocksSwap.Clear();
				RefreshTopRoom();
				RefreshDirtyRooms();
				m_initializedBlocks.Clear();
				GenerateGasBlockRooms();
				GenerateEmptyRooms();
				m_initializedBlocks.Clear();
			}
		}

		private void RefreshTopRoom()
		{
			MyOxygenRoom myOxygenRoom = m_rooms[0];
			if (myOxygenRoom.IsDirty)
			{
				HashSet<Vector3I> roomBlocks = GetRoomBlocks(m_storedGridMin, myOxygenRoom);
				HashSet<Vector3I> blocks = myOxygenRoom.Blocks;
				blocks.ExceptWith(roomBlocks);
				if (blocks.Count != 0)
				{
					CreateAirtightRoom(blocks, 0f, blocks.FirstElement()).IsDirty = true;
				}
				myOxygenRoom.BlockCount = roomBlocks.Count;
				myOxygenRoom.Blocks = roomBlocks;
				myOxygenRoom.IsDirty = false;
				myOxygenRoom.StartingPosition = m_storedGridMin;
			}
		}

		private void ExpandAirtightData()
		{
			Vector3I vector3I = GridMin();
			Vector3I vector3I2 = GridMax();
			Vector3I a = m_storedGridMin - vector3I;
			Vector3I a2 = vector3I2 - m_storedGridMax;
			if (a != Vector3I.Zero || a2 != Vector3I.Zero)
			{
				_ = vector3I2 - vector3I + Vector3I.One;
				m_rooms[0].IsDirty = true;
				m_storedGridMin = vector3I;
				m_storedGridMax = vector3I2;
			}
		}

		private void AddBlock(IMySlimBlock block)
		{
			Vector3I gridPosition = block.Min;
			Vector3I start = block.Min;
			Vector3I end = block.Max;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				MyOxygenRoom oxygenRoomForCubeGridPosition = GetOxygenRoomForCubeGridPosition(ref gridPosition);
				if (oxygenRoomForCubeGridPosition != null)
				{
					oxygenRoomForCubeGridPosition.IsDirty = true;
					bool flag = false;
					Sandbox.ModAPI.IMyDoor myDoor = block.FatBlock as Sandbox.ModAPI.IMyDoor;
					if (myDoor != null)
					{
						flag = true;
						if (myDoor is MyAirtightSlideDoor)
						{
							break;
						}
					}
					MyCubeBlockDefinition myCubeBlockDefinition = block.BlockDefinition as MyCubeBlockDefinition;
					bool? flag2 = IsAirtightFromDefinition(myCubeBlockDefinition, block.BuildLevelRatio);
					if ((myCubeBlockDefinition != null && flag2 == true) || flag)
					{
						Vector3I item = gridPosition;
						oxygenRoomForCubeGridPosition.BlockCount--;
						oxygenRoomForCubeGridPosition.Blocks.Remove(item);
						m_cubeRoom[item.X, item.Y, item.Z].RoomLink = null;
					}
				}
				vector3I_RangeIterator.GetNext(out gridPosition);
			}
		}

		private void RefreshDirtyRooms()
		{
			int count = m_rooms.Count;
			for (int i = 0; i < count; i++)
			{
				MyOxygenRoom myOxygenRoom = m_rooms[i];
				if (myOxygenRoom.Index != 0)
				{
					RefreshRoomBlocks(myOxygenRoom);
				}
			}
		}

		private void RefreshRoomBlocks(MyOxygenRoom room)
		{
			if (room == null || (room.IsAirtight && !room.IsDirty))
			{
				return;
			}
			MyOxygenRoom myOxygenRoom = m_rooms[0];
			Vector3I startingPosition = room.StartingPosition;
			Vector3I item = startingPosition;
			m_blockQueue.Clear();
			m_blockQueue.Enqueue(item);
			HashSet<Vector3I> hashSet = new HashSet<Vector3I>();
			hashSet.Add(item);
			bool flag = true;
			while (m_blockQueue.Count > 0)
			{
				item = m_blockQueue.Dequeue();
				for (int i = 0; i < m_neighbours.Length; i++)
				{
					Vector3I vector3I = item + m_neighbours[i];
					if (hashSet.Contains(vector3I))
					{
						continue;
					}
					if (Vector3I.Min(vector3I, m_storedGridMin) != m_storedGridMin || Vector3I.Max(vector3I, m_storedGridMax) != m_storedGridMax)
					{
						flag = false;
						break;
					}
					bool flag2 = IsAirtightBetweenPositions(item, vector3I);
					if (flag2)
					{
						continue;
					}
					hashSet.Add(vector3I);
					IMySlimBlock cubeBlock = m_cubeGrid.GetCubeBlock(vector3I);
					if (cubeBlock != null)
					{
						Sandbox.ModAPI.IMyDoor myDoor = cubeBlock.FatBlock as Sandbox.ModAPI.IMyDoor;
						if (myDoor != null)
						{
							if (myDoor.Status == DoorStatus.Open || !flag2)
							{
								m_blockQueue.Enqueue(vector3I);
							}
							continue;
						}
					}
					MyOxygenBlock myOxygenBlock = m_cubeRoom[vector3I.X, vector3I.Y, vector3I.Z];
					if ((myOxygenBlock != null && myOxygenBlock.Room != null) || cubeBlock == null)
					{
						m_blockQueue.Enqueue(vector3I);
					}
				}
			}
			if (flag)
			{
				MyOxygenRoomLink myOxygenRoomLink = null;
				if (myOxygenRoom == room)
				{
					m_lastRoomIndex++;
					room = new MyOxygenRoom(m_lastRoomIndex);
					m_rooms.Add(room);
					myOxygenRoomLink = new MyOxygenRoomLink(room);
					room.StartingPosition = startingPosition;
					foreach (Vector3I item2 in hashSet)
					{
						if (m_cubeRoom[item2.X, item2.Y, item2.Z].Room == myOxygenRoom)
						{
							m_cubeRoom[item2.X, item2.Y, item2.Z].RoomLink = myOxygenRoomLink;
							room.BlockCount++;
							myOxygenRoom.BlockCount--;
							myOxygenRoom.Blocks.Remove(item2);
						}
					}
				}
				else
				{
					myOxygenRoomLink = room.Link;
					int blockCount = room.BlockCount;
					room.BlockCount = hashSet.Count;
					foreach (Vector3I item3 in hashSet)
					{
						if (!m_cubeRoom.TryGetValue(item3, out MyOxygenBlock value))
						{
							value = new MyOxygenBlock();
							m_cubeRoom.Add(item3, value);
						}
						value.RoomLink = myOxygenRoomLink;
					}
					if (blockCount > hashSet.Count)
					{
						HashSet<Vector3I> blocks = room.Blocks;
						blocks.ExceptWith(hashSet);
						float num = room.OxygenAmount / (float)blockCount * (float)blocks.Count;
						CreateAirtightRoom(blocks, num, blocks.FirstElement());
						room.OxygenAmount -= num;
					}
				}
				room.Blocks = hashSet;
			}
			room.IsAirtight = flag;
			room.IsDirty = false;
		}

		private void OnBackgroundTaskFinished()
		{
			m_isProcessingData = false;
		}

		private void StartGenerateAirtightData()
		{
			m_isProcessingData = true;
			m_cubeRoom = new MyOxygenCube();
			m_previousGridMin = m_storedGridMin;
			m_previousGridMax = m_storedGridMax;
			m_storedGridMin = GridMin();
			m_storedGridMax = GridMax();
			m_addedBlocks.Clear();
			m_deletedBlocks.Clear();
			m_gasBlocksForUpdate.Clear();
			m_gasBlocksForUpdate.AddRange(m_gasBlocks);
			MarkCubeGridForUpdate();
			m_backgroundTask = Parallel.Start(GenerateAirtightData, OnBackgroundTaskFinished);
		}

		private void GenerateAirtightData()
		{
			if (m_rooms == null)
			{
				m_rooms = new MyConcurrentList<MyOxygenRoom>();
			}
			else
			{
				m_lastRoomIndex = 0;
				m_rooms.Clear();
			}
			m_initializedBlocks.Clear();
			GenerateTopRoom();
			GenerateGasBlockRooms();
			GenerateEmptyRooms();
			if (m_savedRooms != null)
			{
				OxygenRoom[] savedRooms = m_savedRooms;
				for (int i = 0; i < savedRooms.Length; i++)
				{
					OxygenRoom oxygenRoom = savedRooms[i];
					if (!(Vector3I.Min(oxygenRoom.StartingPosition, m_storedGridMin) != m_storedGridMin) && !(Vector3I.Max(oxygenRoom.StartingPosition, m_storedGridMax) != m_storedGridMax))
					{
						MyOxygenBlock myOxygenBlock = m_cubeRoom[oxygenRoom.StartingPosition.X, oxygenRoom.StartingPosition.Y, oxygenRoom.StartingPosition.Z];
						if (myOxygenBlock != null && myOxygenBlock.RoomLink != null && myOxygenBlock.RoomLink.Room != null)
						{
							myOxygenBlock.RoomLink.Room.OxygenAmount = oxygenRoom.OxygenAmount;
						}
					}
				}
				m_savedRooms = null;
			}
			m_initializedBlocks.Clear();
			m_gridExpanded = false;
		}

		private void GenerateEmptyRooms()
		{
			Vector3I next = m_storedGridMin;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref m_storedGridMin, ref m_storedGridMax);
			while (vector3I_RangeIterator.IsValid())
			{
				CheckPositionForEmptyRoom(next);
				vector3I_RangeIterator.GetNext(out next);
			}
		}

		private void CheckPositionForEmptyRoom(Vector3I position)
		{
			if (m_initializedBlocks.Contains(position))
			{
				return;
			}
			if (!m_cubeRoom.TryGetValue(position, out MyOxygenBlock value))
			{
				value = new MyOxygenBlock();
				m_cubeRoom.Add(position, value);
			}
			if (value != null && value.Room != null)
			{
				return;
			}
			IMySlimBlock cubeBlock = m_cubeGrid.GetCubeBlock(position);
			if (cubeBlock != null)
			{
				MyCubeBlockDefinition myCubeBlockDefinition = cubeBlock.BlockDefinition as MyCubeBlockDefinition;
				bool? flag = IsAirtightFromDefinition(myCubeBlockDefinition, cubeBlock.BuildLevelRatio);
				if (myCubeBlockDefinition != null && flag == true)
				{
					return;
				}
				Sandbox.ModAPI.IMyDoor myDoor = cubeBlock.FatBlock as Sandbox.ModAPI.IMyDoor;
				if (myDoor != null && (myDoor.Status == DoorStatus.Closed || myDoor.Status == DoorStatus.Closing) && !(myDoor is MyAirtightSlideDoor))
				{
					return;
				}
			}
			HashSet<Vector3I> roomBlocks = GetRoomBlocks(position);
			if (roomBlocks.Count > 0)
			{
				CreateAirtightRoom(roomBlocks, 0f, position);
				m_initializedBlocks.UnionWith(roomBlocks);
			}
		}

		private void GenerateGasBlockRooms()
		{
			foreach (IMySlimBlock item in m_gasBlocksForUpdate)
			{
				Vector3I position = item.Position;
				MyOxygenBlock myOxygenBlock = m_cubeRoom[position.X, position.Y, position.Z];
				if (myOxygenBlock == null || myOxygenBlock.Room == null)
				{
					HashSet<Vector3I> roomBlocks = GetRoomBlocks(item.Position);
					CreateAirtightRoom(roomBlocks, 0f, position);
					m_initializedBlocks.UnionWith(roomBlocks);
				}
			}
		}

		private MyOxygenRoom CreateAirtightRoom(HashSet<Vector3I> roomBlocks, float oxygenAmount, Vector3I startingPosition)
		{
			m_lastRoomIndex++;
			MyOxygenRoom myOxygenRoom = new MyOxygenRoom(m_lastRoomIndex);
			myOxygenRoom.IsAirtight = true;
			myOxygenRoom.OxygenAmount = oxygenAmount;
			myOxygenRoom.EnvironmentOxygen = MyOxygenProviderSystem.GetOxygenInPoint(m_cubeGrid.GridIntegerToWorld(startingPosition));
			myOxygenRoom.DepressurizationTime = GetTotalGamePlayTimeInMilliseconds();
			myOxygenRoom.BlockCount = roomBlocks.Count;
			myOxygenRoom.Blocks = roomBlocks;
			myOxygenRoom.StartingPosition = startingPosition;
			float num = myOxygenRoom.OxygenLevel(m_cubeGrid.GridSize);
			if (myOxygenRoom.EnvironmentOxygen > num)
			{
				myOxygenRoom.OxygenAmount = myOxygenRoom.MaxOxygen(m_cubeGrid.GridSize) * myOxygenRoom.EnvironmentOxygen;
			}
			m_rooms.Add(myOxygenRoom);
			MyOxygenRoomLink roomPointer = new MyOxygenRoomLink(myOxygenRoom);
			foreach (Vector3I roomBlock in roomBlocks)
			{
				MyOxygenBlock value = new MyOxygenBlock(roomPointer);
				m_cubeRoom.Add(roomBlock, value);
			}
			return myOxygenRoom;
		}

		private void GenerateTopRoom()
		{
			HashSet<Vector3I> roomBlocks = GetRoomBlocks(m_storedGridMin);
			MyOxygenRoom myOxygenRoom = new MyOxygenRoom(0);
			myOxygenRoom.IsAirtight = false;
			myOxygenRoom.EnvironmentOxygen = MyOxygenProviderSystem.GetOxygenInPoint(m_cubeGrid.GridIntegerToWorld(m_storedGridMin));
			myOxygenRoom.DepressurizationTime = GetTotalGamePlayTimeInMilliseconds();
			myOxygenRoom.BlockCount = roomBlocks.Count;
			myOxygenRoom.Blocks = roomBlocks;
			myOxygenRoom.StartingPosition = m_storedGridMin;
			m_rooms.Add(myOxygenRoom);
			MyOxygenRoomLink roomPointer = new MyOxygenRoomLink(myOxygenRoom);
			foreach (Vector3I item in roomBlocks)
			{
				MyOxygenBlock value = new MyOxygenBlock(roomPointer);
				m_cubeRoom.Add(item, value);
				m_initializedBlocks.Add(item);
			}
		}

		private HashSet<Vector3I> GetRoomBlocks(Vector3I startPosition, MyOxygenRoom initRoom = null)
		{
			m_blockQueue.Clear();
			m_blockQueue.Enqueue(startPosition);
			m_visitedBlocks.Clear();
			m_visitedBlocks.Add(startPosition);
			HashSet<Vector3I> hashSet = new HashSet<Vector3I>();
			hashSet.Add(startPosition);
			if (initRoom != null)
			{
				if (!m_cubeRoom.TryGetValue(startPosition, out MyOxygenBlock value))
				{
					value = new MyOxygenBlock();
					m_cubeRoom.Add(startPosition, value);
				}
				value.RoomLink = initRoom.Link;
			}
			while (m_blockQueue.Count > 0)
			{
				Vector3I vector3I = m_blockQueue.Dequeue();
				for (int i = 0; i < m_neighbours.Length; i++)
				{
					Vector3I vector3I2 = vector3I + m_neighbours[i];
					if (Vector3I.Min(vector3I2, m_storedGridMin) != m_storedGridMin || Vector3I.Max(vector3I2, m_storedGridMax) != m_storedGridMax || m_visitedBlocks.Contains(vector3I2) || IsAirtightBetweenPositions(vector3I, vector3I2))
					{
						continue;
					}
					m_visitedBlocks.Add(vector3I2);
					m_blockQueue.Enqueue(vector3I2);
					Vector3I vector3I3 = vector3I2;
					hashSet.Add(vector3I3);
					if (initRoom != null)
					{
						if (!m_cubeRoom.TryGetValue(vector3I3, out MyOxygenBlock value2))
						{
							value2 = new MyOxygenBlock();
							m_cubeRoom.Add(vector3I3, value2);
						}
						value2.RoomLink = initRoom.Link;
					}
				}
			}
			return hashSet;
		}

		public static MatrixD CreateAxisAlignedMatrix(ref Vector3I vec)
		{
			MatrixD zero = MatrixD.Zero;
			if (vec.X != 0)
			{
				if (vec.X > 0)
				{
					zero.M31 = (zero.M22 = 1.0);
				}
				else
				{
					zero.M31 = (zero.M22 = -1.0);
				}
				zero.M13 = 1.0;
			}
			else if (vec.Y != 0)
			{
				if (vec.Y > 0)
				{
					zero.M32 = (zero.M21 = 1.0);
				}
				else
				{
					zero.M32 = (zero.M21 = -1.0);
				}
				zero.M13 = 1.0;
			}
			else
			{
				if (vec.Z == 0)
				{
					return MatrixD.Identity;
				}
				if (vec.Z > 0)
				{
					zero.M33 = (zero.M21 = 1.0);
				}
				else
				{
					zero.M33 = (zero.M21 = -1.0);
				}
				zero.M12 = 1.0;
			}
			return zero;
		}

		public static void AddDepressurizationEffects(MyCubeGrid grid, Vector3I from, Vector3I to)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated || from == to)
			{
				return;
			}
			Vector3D vector3D = grid.GridIntegerToWorld(from);
			Vector3D zero = Vector3D.Zero;
			Vector3I vec = to - from;
			MatrixD effectMatrix;
			if (vec.IsAxisAligned())
			{
				effectMatrix = CreateAxisAlignedMatrix(ref vec);
				effectMatrix.Translation = from * ((grid.GridSizeEnum == MyCubeSize.Small) ? 0.5f : 2.5f);
			}
			else
			{
				zero = grid.GridIntegerToWorld(to);
				effectMatrix = MatrixD.CreateFromDir(vector3D - zero);
				effectMatrix = MatrixD.Normalize(effectMatrix);
				effectMatrix.Translation = vector3D;
				effectMatrix *= grid.PositionComp.WorldMatrixNormalizedInv;
			}
			Vector3D worldPosition = vector3D;
			if (!MyParticlesManager.TryCreateParticleEffect("OxyLeakLarge", ref effectMatrix, ref worldPosition, grid.Render.GetRenderObjectID(), out MyParticleEffect effect))
			{
				return;
			}
			MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
			if (myEntity3DSoundEmitter != null)
			{
				myEntity3DSoundEmitter.SetPosition(vector3D);
				myEntity3DSoundEmitter.PlaySound(m_airleakSound);
				if (grid.Physics != null)
				{
					myEntity3DSoundEmitter.SetVelocity(grid.Physics.LinearVelocity);
				}
			}
			if (grid.GridSizeEnum == MyCubeSize.Small)
			{
				effect.UserScale = 0.2f;
			}
		}

		private Vector3I GridMin()
		{
			return m_cubeGrid.Min - Vector3I.One;
		}

		private Vector3I GridMax()
		{
			return m_cubeGrid.Max + Vector3I.One;
		}

		private bool IsAirtightBetweenPositions(Vector3I startPos, Vector3I endPos)
		{
			IMySlimBlock cubeBlock = m_cubeGrid.GetCubeBlock(startPos);
			IMySlimBlock cubeBlock2 = m_cubeGrid.GetCubeBlock(endPos);
			if (cubeBlock == cubeBlock2)
			{
				if (cubeBlock != null)
				{
					MyCubeBlockDefinition myCubeBlockDefinition = cubeBlock.BlockDefinition as MyCubeBlockDefinition;
					bool? flag = IsAirtightFromDefinition(myCubeBlockDefinition, cubeBlock.BuildLevelRatio);
					if (myCubeBlockDefinition != null)
					{
						return flag == true;
					}
					return false;
				}
				return false;
			}
			if (cubeBlock != null && IsAirtightBlock(cubeBlock, startPos, endPos - startPos))
			{
				return true;
			}
			if (cubeBlock2 != null)
			{
				return IsAirtightBlock(cubeBlock2, endPos, startPos - endPos);
			}
			return false;
		}

		private bool IsAirtightBlock(IMySlimBlock block, Vector3I pos, Vector3 normal)
		{
			MyCubeBlockDefinition myCubeBlockDefinition = block.BlockDefinition as MyCubeBlockDefinition;
			if (myCubeBlockDefinition == null)
			{
				return false;
			}
			bool? flag = IsAirtightFromDefinition(myCubeBlockDefinition, block.BuildLevelRatio);
			if (flag.HasValue)
			{
				return flag.Value;
			}
			block.Orientation.GetMatrix(out Matrix result);
			result.TransposeRotationInPlace();
			Vector3I transformedNormal = Vector3I.Round(Vector3.Transform(normal, result));
			Vector3 position = Vector3.Zero;
			if (block.FatBlock != null)
			{
				position = pos - block.FatBlock.Position;
			}
			Vector3 value = Vector3.Transform(position, result) + myCubeBlockDefinition.Center;
			if (myCubeBlockDefinition.IsCubePressurized[Vector3I.Round(value)][transformedNormal])
			{
				return true;
			}
			Sandbox.ModAPI.IMyDoor myDoor = block.FatBlock as Sandbox.ModAPI.IMyDoor;
			if (myDoor != null && (myDoor.Status == DoorStatus.Closed || myDoor.Status == DoorStatus.Closing))
			{
				return IsDoorAirtight(myDoor, ref transformedNormal, myCubeBlockDefinition);
			}
			return false;
		}

		private bool? IsAirtightFromDefinition(MyCubeBlockDefinition blockDefinition, float buildLevelRatio)
		{
			if (blockDefinition.BuildProgressModels != null && blockDefinition.BuildProgressModels.Length != 0)
			{
				MyCubeBlockDefinition.BuildProgressModel buildProgressModel = blockDefinition.BuildProgressModels[blockDefinition.BuildProgressModels.Length - 1];
				if (buildLevelRatio < buildProgressModel.BuildRatioUpperBound)
				{
					return false;
				}
			}
			return blockDefinition.IsAirTight;
		}

		private bool IsDoorAirtight(Sandbox.ModAPI.IMyDoor doorBlock, ref Vector3I transformedNormal, MyCubeBlockDefinition blockDefinition)
		{
			if (doorBlock is MyAdvancedDoor)
			{
				if (doorBlock.IsFullyClosed)
				{
					MyCubeBlockDefinition.MountPoint[] mountPoints = blockDefinition.MountPoints;
					for (int i = 0; i < mountPoints.Length; i++)
					{
						MyCubeBlockDefinition.MountPoint mountPoint = mountPoints[i];
						if (transformedNormal == mountPoint.Normal)
						{
							return false;
						}
					}
					return true;
				}
			}
			else if (doorBlock is MyAirtightSlideDoor)
			{
				if (doorBlock.IsFullyClosed && transformedNormal == Vector3I.Forward)
				{
					return true;
				}
			}
			else if (doorBlock is MyAirtightDoorGeneric)
			{
				if (doorBlock.IsFullyClosed && (transformedNormal == Vector3I.Forward || transformedNormal == Vector3I.Backward))
				{
					return true;
				}
			}
			else if (doorBlock.IsFullyClosed)
			{
				MyCubeBlockDefinition.MountPoint[] mountPoints = blockDefinition.MountPoints;
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint mountPoint2 = mountPoints[i];
					if (transformedNormal == mountPoint2.Normal)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private void StartRemoveBlocks()
		{
			if (!m_isProcessingData)
			{
				if (m_gasBlocks.Count == 0)
				{
					Clear();
				}
				if (m_rooms == null)
				{
					m_deletedBlocks.Clear();
					return;
				}
				m_isProcessingData = true;
				List<IMySlimBlock> deletedBlocksSwap = m_deletedBlocksSwap;
				m_deletedBlocksSwap = m_deletedBlocks;
				m_deletedBlocks = deletedBlocksSwap;
				m_backgroundTask = Parallel.Start(RemoveBlocks, OnBackgroundTaskFinished);
			}
		}

		private void RemoveBlocks()
		{
			bool flag = false;
			Vector3I arg = Vector3I.Zero;
			Vector3I arg2 = Vector3I.Zero;
			foreach (IMySlimBlock item in m_deletedBlocksSwap)
			{
				Vector3I next = item.Min;
				Vector3I start = item.Min;
				Vector3I end = item.Max;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				while (vector3I_RangeIterator.IsValid())
				{
					if (RemoveBlock(next, out Vector3I depressFrom, out Vector3I depressTo))
					{
						flag = true;
						arg = depressFrom;
						arg2 = depressTo;
					}
					vector3I_RangeIterator.GetNext(out next);
				}
			}
			if (flag)
			{
				_ = m_cubeGrid;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyCubeGrid.DepressurizeEffect, m_cubeGrid.EntityId, arg, arg2);
			}
			m_deletedBlocksSwap.Clear();
		}

		private bool RemoveBlock(Vector3I deletedBlockPosition, out Vector3I depressFrom, out Vector3I depressTo)
		{
			bool result = false;
			depressFrom = Vector3I.Zero;
			depressTo = Vector3I.Zero;
			Vector3I current = deletedBlockPosition;
			MyOxygenRoom myOxygenRoom = m_rooms[0];
			MyOxygenRoom myOxygenRoom2 = GetMaxBlockRoom(ref current, myOxygenRoom);
			if (myOxygenRoom2 == null)
			{
				return result;
			}
			for (int i = 0; i < m_neighboursForDelete.Length; i++)
			{
				Vector3I gridPosition = current + m_neighboursForDelete[i];
				if (!IsInBounds(gridPosition))
				{
					continue;
				}
				MyOxygenRoom oxygenRoomForCubeGridPosition = GetOxygenRoomForCubeGridPosition(ref gridPosition);
				if (oxygenRoomForCubeGridPosition == null || oxygenRoomForCubeGridPosition == myOxygenRoom2 || (current != gridPosition && IsAirtightBetweenPositions(current, gridPosition)))
				{
					continue;
				}
				if (myOxygenRoom2.IsAirtight && !oxygenRoomForCubeGridPosition.IsAirtight)
				{
					oxygenRoomForCubeGridPosition.BlockCount += myOxygenRoom2.BlockCount;
					oxygenRoomForCubeGridPosition.OxygenAmount += myOxygenRoom2.OxygenAmount;
					MergeRooms(oxygenRoomForCubeGridPosition, myOxygenRoom2, oxygenRoomForCubeGridPosition.Link);
					if (myOxygenRoom2.Blocks != null && oxygenRoomForCubeGridPosition.Blocks != null)
					{
						oxygenRoomForCubeGridPosition.Blocks.UnionWith(myOxygenRoom2.Blocks);
					}
					if (myOxygenRoom2.OxygenLevel(m_cubeGrid.GridSize) - oxygenRoomForCubeGridPosition.EnvironmentOxygen > 0.2f)
					{
						result = true;
						depressFrom = current;
						depressTo = gridPosition;
					}
					myOxygenRoom2.IsAirtight = false;
					myOxygenRoom2.OxygenAmount = 0f;
					myOxygenRoom2.EnvironmentOxygen = Math.Max(myOxygenRoom2.EnvironmentOxygen, oxygenRoomForCubeGridPosition.EnvironmentOxygen);
					myOxygenRoom2.DepressurizationTime = GetTotalGamePlayTimeInMilliseconds();
					myOxygenRoom2.Link.Room = oxygenRoomForCubeGridPosition;
					if (oxygenRoomForCubeGridPosition != myOxygenRoom2 && myOxygenRoom2 != myOxygenRoom)
					{
						myOxygenRoom2.BlockCount = 0;
						myOxygenRoom2.Blocks = null;
						m_rooms.Remove(myOxygenRoom2);
					}
					myOxygenRoom2 = oxygenRoomForCubeGridPosition;
				}
				else if (!myOxygenRoom2.IsAirtight && oxygenRoomForCubeGridPosition.IsAirtight)
				{
					myOxygenRoom2.BlockCount += oxygenRoomForCubeGridPosition.BlockCount;
					myOxygenRoom2.OxygenAmount += oxygenRoomForCubeGridPosition.OxygenAmount;
					MergeRooms(myOxygenRoom2, oxygenRoomForCubeGridPosition, myOxygenRoom2.Link);
					myOxygenRoom2.EnvironmentOxygen = Math.Max(myOxygenRoom2.EnvironmentOxygen, oxygenRoomForCubeGridPosition.EnvironmentOxygen);
					if (oxygenRoomForCubeGridPosition.OxygenLevel(m_cubeGrid.GridSize) - myOxygenRoom2.EnvironmentOxygen > 0.2f)
					{
						result = true;
						depressFrom = current;
						depressTo = gridPosition;
					}
					oxygenRoomForCubeGridPosition.IsAirtight = false;
					oxygenRoomForCubeGridPosition.OxygenAmount = 0f;
					oxygenRoomForCubeGridPosition.EnvironmentOxygen = Math.Max(myOxygenRoom2.EnvironmentOxygen, oxygenRoomForCubeGridPosition.EnvironmentOxygen);
					oxygenRoomForCubeGridPosition.DepressurizationTime = GetTotalGamePlayTimeInMilliseconds();
					oxygenRoomForCubeGridPosition.Link.Room = myOxygenRoom2;
					if (oxygenRoomForCubeGridPosition != myOxygenRoom2 && oxygenRoomForCubeGridPosition != myOxygenRoom)
					{
						oxygenRoomForCubeGridPosition.BlockCount = 0;
						oxygenRoomForCubeGridPosition.Blocks = null;
						m_rooms.Remove(oxygenRoomForCubeGridPosition);
					}
				}
				else
				{
					myOxygenRoom2.BlockCount += oxygenRoomForCubeGridPosition.BlockCount;
					myOxygenRoom2.OxygenAmount += oxygenRoomForCubeGridPosition.OxygenAmount;
					MergeRooms(myOxygenRoom2, oxygenRoomForCubeGridPosition, myOxygenRoom2.Link);
					oxygenRoomForCubeGridPosition.Link.Room = myOxygenRoom2;
					if (oxygenRoomForCubeGridPosition != myOxygenRoom2 && oxygenRoomForCubeGridPosition != myOxygenRoom)
					{
						oxygenRoomForCubeGridPosition.BlockCount = 0;
						oxygenRoomForCubeGridPosition.Blocks = null;
						m_rooms.Remove(oxygenRoomForCubeGridPosition);
					}
				}
			}
			Vector3I vector3I = current;
			MyOxygenBlock myOxygenBlock = m_cubeRoom[vector3I.X, vector3I.Y, vector3I.Z];
			if (myOxygenBlock == null)
			{
				myOxygenBlock = new MyOxygenBlock();
				m_cubeRoom.Add(vector3I, myOxygenBlock);
			}
			if (myOxygenBlock.Room == null)
			{
				myOxygenBlock.RoomLink = myOxygenRoom2.Link;
				myOxygenRoom2.BlockCount++;
				myOxygenRoom2.Blocks.Add(vector3I);
			}
			return result;
		}

		private void MergeRooms(MyOxygenRoom target, MyOxygenRoom withRoom, MyOxygenRoomLink link)
		{
			if (target.Blocks != null && withRoom.Blocks != null)
			{
				target.Blocks.UnionWith(withRoom.Blocks);
				foreach (Vector3I block in withRoom.Blocks)
				{
					m_cubeRoom[block.X, block.Y, block.Z].RoomLink = link;
				}
			}
		}

		private MyOxygenRoom GetMaxBlockRoom(ref Vector3I current, MyOxygenRoom topRoom)
		{
			MyOxygenRoom myOxygenRoom = GetOxygenRoomForCubeGridPosition(ref current);
			for (int i = 0; i < m_neighbours.Length; i++)
			{
				Vector3I gridPosition = current + m_neighbours[i];
				if (!IsInBounds(current) || !IsInBounds(gridPosition) || IsAirtightBetweenPositions(current, gridPosition))
				{
					continue;
				}
				MyOxygenRoom oxygenRoomForCubeGridPosition = GetOxygenRoomForCubeGridPosition(ref gridPosition);
				if (oxygenRoomForCubeGridPosition != null)
				{
					if (myOxygenRoom == null)
					{
						myOxygenRoom = oxygenRoomForCubeGridPosition;
					}
					else if (oxygenRoomForCubeGridPosition == topRoom)
					{
						myOxygenRoom = topRoom;
					}
					else if (myOxygenRoom.BlockCount < oxygenRoomForCubeGridPosition.BlockCount && myOxygenRoom != topRoom)
					{
						myOxygenRoom = oxygenRoomForCubeGridPosition;
					}
				}
			}
			return myOxygenRoom;
		}

		private bool IsInBounds(Vector3I pos)
		{
			if (m_storedGridMin != Vector3I.Min(pos, m_storedGridMin))
			{
				return false;
			}
			if (m_storedGridMax != Vector3I.Max(pos, m_storedGridMax))
			{
				return false;
			}
			return true;
		}

		public MyOxygenRoom GetOxygenRoomForCubeGridPosition(ref Vector3I gridPosition)
		{
			Vector3I pos = gridPosition;
			if (!IsInBounds(pos))
			{
				return null;
			}
			return m_cubeRoom[pos.X, pos.Y, pos.Z]?.Room;
		}

		public MyOxygenBlock GetOxygenBlock(Vector3D worldPosition)
		{
			Vector3I pos = m_cubeGrid.WorldToGridInteger(worldPosition);
			if (m_cubeRoom != null && IsInBounds(pos))
			{
				return m_cubeRoom[pos.X, pos.Y, pos.Z];
			}
			return new MyOxygenBlock();
		}

		public MyOxygenBlock GetSafeOxygenBlock(Vector3D position)
		{
			MyOxygenBlock oxygenBlock = GetOxygenBlock(position);
			if (oxygenBlock == null || oxygenBlock.Room == null)
			{
				Vector3D vector3D = Vector3D.Transform(position, m_cubeGrid.PositionComp.WorldMatrixNormalizedInv);
				vector3D /= (double)m_cubeGrid.GridSize;
				List<Vector3D> list = new List<Vector3D>(3);
				if (vector3D.X - Math.Floor(vector3D.X) > 0.5)
				{
					list.Add(new Vector3D(-1.0, 0.0, 0.0));
				}
				else
				{
					list.Add(new Vector3D(1.0, 0.0, 0.0));
				}
				if (vector3D.Y - Math.Floor(vector3D.Y) > 0.5)
				{
					list.Add(new Vector3D(0.0, -1.0, 0.0));
				}
				else
				{
					list.Add(new Vector3D(0.0, 1.0, 0.0));
				}
				if (vector3D.Z - Math.Floor(vector3D.Z) > 0.5)
				{
					list.Add(new Vector3D(0.0, 0.0, -1.0));
				}
				else
				{
					list.Add(new Vector3D(0.0, 0.0, 1.0));
				}
				{
					foreach (Vector3D item in list)
					{
						Vector3D position2 = vector3D;
						position2 += item;
						position2 *= (double)m_cubeGrid.GridSize;
						position2 = Vector3D.Transform(position2, m_cubeGrid.PositionComp.WorldMatrix);
						MyOxygenBlock oxygenBlock2 = GetOxygenBlock(position2);
						if (oxygenBlock2 != null && oxygenBlock2.Room != null && oxygenBlock2.Room.IsAirtight)
						{
							return oxygenBlock2;
						}
					}
					return oxygenBlock;
				}
			}
			return oxygenBlock;
		}

		public void DebugDraw()
		{
			if (m_isProcessingData || m_rooms == null)
			{
				return;
			}
			Vector2 zero = Vector2.Zero;
			MyRenderProxy.DebugDrawText2D(zero, "CTRL+ (T Toggle Top Room) (R Toggle Room Index) (Y Toggle Positions) (U Toggle View) ([ Index Down) (] Index Up) (- Index Reset) (+ Index Last)", Color.Yellow, 0.6f);
			zero.Y += m_debugTextlineSize;
			MyRenderProxy.DebugDrawText2D(zero, "Rooms Count: " + m_rooms.Count, Color.Yellow, 0.6f);
			zero.Y += m_debugTextlineSize;
			MyRenderProxy.DebugDrawText2D(zero, "Selected Room", Color.Yellow, 0.6f);
			zero.Y += m_debugTextlineSize;
			MyRenderProxy.DebugDrawText2D(zero, "   Index: " + m_debugRoomIndex, Color.Yellow, 0.6f);
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.T))
				{
					m_debugShowTopRoom = !m_debugShowTopRoom;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.R))
				{
					m_debugShowRoomIndex = !m_debugShowRoomIndex;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Y))
				{
					m_debugShowPositions = !m_debugShowPositions;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.OemOpenBrackets))
				{
					m_debugRoomIndex = ((m_debugRoomIndex != 0) ? (m_debugRoomIndex - 1) : 0);
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.OemCloseBrackets))
				{
					m_debugRoomIndex = ((m_debugRoomIndex >= m_lastRoomIndex) ? m_lastRoomIndex : (m_debugRoomIndex + 1));
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.OemPlus))
				{
					m_debugRoomIndex = m_lastRoomIndex;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.OemMinus))
				{
					m_debugRoomIndex = 0;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.U))
				{
					m_debugToggleView = !m_debugToggleView;
				}
			}
			if (m_debugToggleView)
			{
				Vector3I next = m_storedGridMin;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref m_storedGridMin, ref m_storedGridMax);
				while (vector3I_RangeIterator.IsValid())
				{
					if (m_cubeRoom.TryGetValue(next, out MyOxygenBlock value))
					{
						MyOxygenRoom room = value.Room;
						if (room != null && (room.Index != 0 || m_debugShowTopRoom))
						{
							DrawBlock(room, next);
						}
					}
					vector3I_RangeIterator.GetNext(out next);
				}
			}
			else
			{
				DrawRooms(zero);
			}
		}

		private void DrawRooms(Vector2 textPosition)
		{
			foreach (MyOxygenRoom room in m_rooms)
			{
				DrawRoomInfo(textPosition, room);
				foreach (Vector3I block in room.Blocks)
				{
					if ((room.Index != 0 || m_debugShowTopRoom) && (m_debugRoomIndex == 0 || room.Index == m_debugRoomIndex))
					{
						DrawBlock(room, block);
					}
				}
			}
		}

		private void DrawBlock(MyOxygenRoom room, Vector3I blockPosition)
		{
			Color color = room.IsAirtight ? Color.Lerp(Color.Red, Color.Green, room.OxygenLevel(m_cubeGrid.GridSize)) : Color.Blue;
			Vector3D vector3D = m_cubeGrid.GridIntegerToWorld(blockPosition);
			MyRenderProxy.DebugDrawPoint(vector3D, color, depthRead: false);
			if (m_debugShowRoomIndex)
			{
				MyRenderProxy.DebugDrawText3D(vector3D, room.Index.ToString(), Color.LightGray, 0.5f, depthRead: false);
			}
			if (m_debugShowPositions)
			{
				string text = $"{blockPosition.X}, {blockPosition.Y}, {blockPosition.Z}";
				MyRenderProxy.DebugDrawText3D(vector3D, text, Color.LightGray, 0.5f, depthRead: false);
			}
		}

		private void DrawRoomInfo(Vector2 textPosition, MyOxygenRoom room)
		{
			if (room.Index == m_debugRoomIndex)
			{
				string str = $"{room.BlockCount} : {room.Blocks.Count}";
				textPosition.Y += m_debugTextlineSize;
				MyRenderProxy.DebugDrawText2D(textPosition, "   Block Count: " + str, Color.Yellow, 0.6f);
				textPosition.Y += m_debugTextlineSize;
				MyRenderProxy.DebugDrawText2D(textPosition, "   Oxygen Amount: " + room.OxygenAmount, Color.Yellow, 0.6f);
				textPosition.Y += m_debugTextlineSize;
				MyRenderProxy.DebugDrawText2D(textPosition, "   Min: " + m_storedGridMin, Color.Yellow, 0.6f);
				textPosition.Y += m_debugTextlineSize;
				MyRenderProxy.DebugDrawText2D(textPosition, "   Max: " + m_storedGridMax, Color.Yellow, 0.6f);
			}
		}

		internal OxygenRoom[] GetOxygenAmount()
		{
			if (m_rooms != null && m_rooms.List != null)
			{
				int count = m_rooms.List.Count;
				List<MyOxygenRoom> list = m_rooms.List;
				OxygenRoom[] array = new OxygenRoom[count];
				for (int i = 0; i < count; i++)
				{
					MyOxygenRoom myOxygenRoom = list[i];
					if (myOxygenRoom != null)
					{
						array[i].OxygenAmount = myOxygenRoom.OxygenAmount;
						array[i].StartingPosition = myOxygenRoom.StartingPosition;
					}
				}
				return array;
			}
			return null;
		}

		internal void Init(OxygenRoom[] oxygenAmount)
		{
			m_savedRooms = oxygenAmount;
		}
	}
}
