using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractRepair))]
	public class MyContractRepair : MyContract
	{
		public static readonly int DISPOSE_TIME_IN_S = 10;

		private bool m_isBeingDisposed;

		private float m_disposeTime;

		private MyTimeSpan? DisposeTime;

		public Vector3D GridPosition
		{
			get;
			private set;
		}

		public long GridId
		{
			get;
			private set;
		}

		public string PrefabName
		{
			get;
			private set;
		}

		public HashSet<Vector3I> BlocksToRepair
		{
			get;
			private set;
		}

		public int UnrepairedBlockCount
		{
			get;
			private set;
		}

		public bool KeepGridAtTheEnd
		{
			get;
			private set;
		}

		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
			MyObjectBuilder_ContractRepair myObjectBuilder_ContractRepair = objectBuilder as MyObjectBuilder_ContractRepair;
			myObjectBuilder_ContractRepair.GridPosition = GridPosition;
			myObjectBuilder_ContractRepair.GridId = GridId;
			myObjectBuilder_ContractRepair.PrefabName = PrefabName;
			myObjectBuilder_ContractRepair.KeepGridAtTheEnd = KeepGridAtTheEnd;
			myObjectBuilder_ContractRepair.UnrepairedBlockCount = UnrepairedBlockCount;
			myObjectBuilder_ContractRepair.BlocksToRepair = new MySerializableList<Vector3I>();
			foreach (Vector3I item in BlocksToRepair)
			{
				myObjectBuilder_ContractRepair.BlocksToRepair.Add(item);
			}
			return objectBuilder;
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			MyObjectBuilder_ContractRepair myObjectBuilder_ContractRepair = ob as MyObjectBuilder_ContractRepair;
			if (myObjectBuilder_ContractRepair != null)
			{
				GridPosition = myObjectBuilder_ContractRepair.GridPosition;
				GridId = myObjectBuilder_ContractRepair.GridId;
				PrefabName = myObjectBuilder_ContractRepair.PrefabName;
				KeepGridAtTheEnd = myObjectBuilder_ContractRepair.KeepGridAtTheEnd;
				UnrepairedBlockCount = myObjectBuilder_ContractRepair.UnrepairedBlockCount;
				BlocksToRepair = new HashSet<Vector3I>();
				foreach (Vector3I item in myObjectBuilder_ContractRepair.BlocksToRepair)
				{
					BlocksToRepair.Add(item);
				}
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (base.State != MyContractStateEnum.Active)
			{
				return;
			}
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				SubscribeToBlocks(myCubeGrid);
				myCubeGrid.OnBlockRemoved += BlockRemoved;
				if (base.CanBeFinished)
				{
					Finish();
				}
			}
		}

		public override bool CanBeFinished_Internal()
		{
			if (base.CanBeFinished_Internal())
			{
				return UnrepairedBlockCount <= 0;
			}
			return false;
		}

		protected override void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			base.Activate_Internal(timeOfActivation);
			if (GridId <= 0)
			{
				if (string.IsNullOrEmpty(PrefabName))
				{
					Fail();
				}
				else
				{
					SpawnPrefab(PrefabName);
				}
				return;
			}
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyGps gps = PrepareGPS(myCubeGrid);
				foreach (long owner in base.Owners)
				{
					MySession.Static.Gpss.SendAddGps(owner, ref gps, GridId);
				}
				ScanForBlocksToRepair(myCubeGrid);
				SubscribeToBlocks(myCubeGrid);
				myCubeGrid.OnBlockRemoved += BlockRemoved;
				if (base.CanBeFinished)
				{
					Finish();
				}
			}
			else
			{
				Fail();
			}
		}

		private MyGps PrepareGPS(MyCubeGrid grid)
		{
			return new MyGps
			{
				DisplayName = MyTexts.GetString(MySpaceTexts.Contract_Repair_GpsName),
				Name = MyTexts.GetString(MySpaceTexts.Contract_Repair_GpsName),
				Description = MyTexts.GetString(MySpaceTexts.Contract_Repair_GpsDescription),
				Coords = GridPosition,
				ShowOnHud = true,
				DiscardAt = null,
				GPSColor = Color.DarkOrange,
				ContractId = base.Id
			};
		}

		protected void SpawnPrefab(string name)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(base.StartFaction);
			if (myFaction == null)
			{
				MyLog.Default.Error("Contract - Repair: Starting faction is not in factions!!!\n Cannot spawn prefab.");
				return;
			}
			Vector3 vector = Vector3.Normalize(MyUtils.GetRandomVector3());
			Vector3 up = Vector3.CalculatePerpendicularVector(vector);
			MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties
			{
				Position = GridPosition,
				Forward = vector,
				Up = up,
				PrefabName = name,
				OwnerId = myFaction.FounderId,
				Color = myFaction.CustomColor,
				SpawningOptions = (SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix),
				UpdateSync = true
			};
			MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, delegate
			{
				if (spawnProperties.ResultList != null && spawnProperties.ResultList.Count != 0 && spawnProperties.ResultList.Count <= 1)
				{
					MyCubeGrid myCubeGrid = spawnProperties.ResultList[0];
					GridId = myCubeGrid.EntityId;
					MyGps gps = PrepareGPS(myCubeGrid);
					foreach (long owner in base.Owners)
					{
						MySession.Static.Gpss.SendAddGps(owner, ref gps, GridId);
					}
					ScanForBlocksToRepair(myCubeGrid);
					SubscribeToBlocks(myCubeGrid);
					myCubeGrid.OnBlockRemoved += BlockRemoved;
					if (base.CanBeFinished)
					{
						Finish();
					}
				}
			});
		}

		private void BlockRemoved(MySlimBlock obj)
		{
			obj.UnsubscribeFromIsFunctionalChanged(BlockFunctionalityChanged);
			if (BlocksToRepair.Contains(obj.Position))
			{
				BlocksToRepair.Remove(obj.Position);
			}
			if (base.State == MyContractStateEnum.Active)
			{
				Fail();
			}
		}

		private void ScanForBlocksToRepair(MyCubeGrid grid)
		{
			foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
			{
				if (!cubeBlock.ComponentStack.IsFunctional)
				{
					BlocksToRepair.Add(cubeBlock.Position);
				}
			}
			UnrepairedBlockCount = BlocksToRepair.Count;
		}

		private void ClearBlocksForRepair()
		{
			BlocksToRepair.Clear();
			UnrepairedBlockCount = 0;
		}

		private void SubscribeToBlocks(MyCubeGrid grid)
		{
			foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
			{
				if (!cubeBlock.ComponentStack.IsFunctional)
				{
					BlocksToRepair.Add(cubeBlock.Position);
					cubeBlock.SubscribeForIsFunctionalChanged(BlockFunctionalityChanged);
				}
			}
		}

		private void UnsubscribeFromBlocks(MyCubeGrid grid)
		{
			foreach (Vector3I item in BlocksToRepair)
			{
				grid.GetCubeBlock(item)?.UnsubscribeFromIsFunctionalChanged(BlockFunctionalityChanged);
			}
		}

		private void BlockFunctionalityChanged(MySlimBlock block)
		{
			if (block.ComponentStack.IsFunctional)
			{
				UnrepairedBlockCount--;
			}
			else
			{
				UnrepairedBlockCount++;
			}
			if (base.CanBeFinished)
			{
				Finish();
			}
		}

		protected override void FailFor_Internal(long player, bool abandon = false)
		{
			base.FailFor_Internal(player, abandon);
			RemoveGpsForPlayer(player);
		}

		protected override void FinishFor_Internal(long player, int rewardeeCount)
		{
			base.FinishFor_Internal(player, rewardeeCount);
			RemoveGpsForPlayer(player);
		}

		protected override void CleanUp_Internal()
		{
			float disposeTime = 0f;
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.OnBlockRemoved -= BlockRemoved;
				UnsubscribeFromBlocks(myCubeGrid);
				ClearBlocksForRepair();
				if (base.State == MyContractStateEnum.Finished)
				{
					if (!KeepGridAtTheEnd)
					{
						CreateParticleEffectOnEntity("Warp", myCubeGrid.EntityId, offset: true);
						disposeTime = 10f;
					}
				}
				else if (!KeepGridAtTheEnd)
				{
					CreateParticleEffectOnEntity("Explosion_Warhead_50", myCubeGrid.EntityId, offset: false);
					disposeTime = 2f;
				}
				else
				{
					CreateParticleEffectOnEntity("", myCubeGrid.EntityId, offset: false);
					disposeTime = 0f;
				}
			}
			m_disposeTime = disposeTime;
			m_isBeingDisposed = true;
			base.State = MyContractStateEnum.ToBeDisposed;
		}

		public override void Update(MyTimeSpan currentTime)
		{
			base.Update(currentTime);
			MyContractStateEnum state = base.State;
			if (state != MyContractStateEnum.ToBeDisposed)
			{
				return;
			}
			bool flag = false;
			if (m_isBeingDisposed)
			{
				if (!DisposeTime.HasValue)
				{
					DisposeTime = currentTime + MyTimeSpan.FromSeconds(m_disposeTime);
				}
				if (DisposeTime.Value <= currentTime)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			base.State = MyContractStateEnum.Disposed;
			if (!KeepGridAtTheEnd)
			{
				MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					myCubeGrid.DismountAllCockpits();
					myCubeGrid.Close();
				}
			}
		}

		public override MyDefinitionId? GetDefinitionId()
		{
			return new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Repair");
		}

		private void RemoveGpsForPlayer(long identityId)
		{
			MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, base.Id);
			if (gpsByContractId != null)
			{
				MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
			}
		}
	}
}
