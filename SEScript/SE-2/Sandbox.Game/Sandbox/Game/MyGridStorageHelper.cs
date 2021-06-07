using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game
{
	internal class MyGridStorageHelper
	{
		public enum MyGridLoadStatus
		{
			None,
			Success,
			Fail_Unspecified,
			Fail_DetailLoadFailed,
			Fail_GridLoadFailed,
			Fail_NotEnoughPCU,
			Fail_CannotPlace,
			Fail_IdentityNotFound
		}

		public class MyGridLoadFuture
		{
			public bool IsFinished;

			public MyGridLoadStatus Status;

			public bool FailedOnSecondCheck;

			public MyGridLoadFuture()
			{
				IsFinished = false;
				Status = MyGridLoadStatus.None;
				FailedOnSecondCheck = false;
			}

			public void FailOn(MyGridLoadStatus status, bool secondCheck = false)
			{
				if (status != 0 && status != MyGridLoadStatus.Success)
				{
					FailedOnSecondCheck = secondCheck;
					Status = status;
					IsFinished = true;
				}
			}
		}

		private class LoadData : WorkData
		{
			public MyObjectBuilder_Definitions Builder;

			public MyObjectBuilder_SavedGridDetails Detail;

			public MyGridLoadFuture Future;

			public Action OnCompletion;

			public string Path_Folder;

			public string Path_Detail;

			public string Path_Blueprint;

			public long NewOwner;

			public MyPositionAndOrientation Transform;

			public Vector3D Position;

			public float Radius;
		}

		public static readonly string GRID_SAVE_FOLDER = "Grids";

		public static readonly string STORAGE_EXTENSION = ".sbc";

		public static readonly string DETAIL_NAME = "detail";

		public static List<Guid> testIdList = new List<Guid>();

		public static void TestFunctionStore(List<MyGuiControlListbox.Item> selectedItems)
		{
			if (selectedItems.Count <= 0)
			{
				return;
			}
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(((MyEntityList.MyEntityListInfoItem)selectedItems[0].UserData).EntityId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyGridStorageHelper myGridStorageHelper = new MyGridStorageHelper();
				Guid guid = Guid.NewGuid();
				if (myGridStorageHelper.StoreGrid(myCubeGrid, guid, 123456uL))
				{
					testIdList.Add(guid);
					myCubeGrid.Close();
				}
			}
		}

		public bool StoreGrid(MyCubeGrid grid, Guid guid, ulong creator, string name = null)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			bool flag = true;
			MyGridClipboard myGridClipboard = new MyGridClipboard(MyClipboardComponent.ClipboardDefinition.PastingSettings);
			myGridClipboard.CopyGroup(grid, flag ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
			string text = guid.ToString();
			string path = Path.Combine(MySession.Static.CurrentPath, GRID_SAVE_FOLDER, text);
			string text2 = Path.Combine(path, text) + STORAGE_EXTENSION;
			string path2 = Path.Combine(path, DETAIL_NAME) + STORAGE_EXTENSION;
			string path3 = text2 + "B5";
			MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
			myObjectBuilder_ShipBlueprintDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(text));
			myObjectBuilder_ShipBlueprintDefinition.CubeGrids = myGridClipboard.CopiedGrids.ToArray();
			myObjectBuilder_ShipBlueprintDefinition.RespawnShip = false;
			myObjectBuilder_ShipBlueprintDefinition.OwnerSteamId = creator;
			if (!string.IsNullOrEmpty(name))
			{
				myObjectBuilder_ShipBlueprintDefinition.DisplayName = name;
				myObjectBuilder_ShipBlueprintDefinition.CubeGrids[0].DisplayName = name;
			}
			MyObjectBuilder_SavedGridDetails myObjectBuilder_SavedGridDetails = new MyObjectBuilder_SavedGridDetails();
			myObjectBuilder_SavedGridDetails.PcuCount = grid.BlocksPCU;
			myObjectBuilder_SavedGridDetails.AABB_min = grid.PositionComp.LocalAABB.Min;
			myObjectBuilder_SavedGridDetails.AABB_max = grid.PositionComp.LocalAABB.Max;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			myObjectBuilder_Definitions.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
			myObjectBuilder_Definitions.ShipBlueprints[0] = myObjectBuilder_ShipBlueprintDefinition;
			try
			{
				if (MyObjectBuilderSerializer.SerializeXML(text2, compress: false, myObjectBuilder_Definitions))
				{
					MyObjectBuilderSerializer.SerializePB(path3, compress: false, myObjectBuilder_Definitions);
					MyObjectBuilderSerializer.SerializeXML(path2, compress: false, myObjectBuilder_SavedGridDetails);
					return true;
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine($"Failed to write prefab at file {text2}, message: {ex.Message}, stack:{ex.StackTrace}");
			}
			return false;
		}

		public static void TestFunctionLoad(bool removeFromStorage = true)
		{
			if (testIdList.Count > 0)
			{
				Guid guid = testIdList[0];
				testIdList.RemoveAt(0);
				if (!removeFromStorage)
				{
					testIdList.Add(guid);
				}
				MyGridStorageHelper myGridStorageHelper = new MyGridStorageHelper();
				MyPositionAndOrientation transform = new MyPositionAndOrientation(Vector3.Zero, Vector3.Forward, Vector3.Up);
				long localPlayerId = MySession.Static.LocalPlayerId;
				myGridStorageHelper.LoadGrid(guid, transform, localPlayerId);
			}
		}

		public MyGridLoadFuture LoadGrid(Guid guid, MyPositionAndOrientation transform, long newOwner, Action onCompletion = null)
		{
			MyGridLoadFuture myGridLoadFuture = new MyGridLoadFuture();
			if (!Sync.IsServer)
			{
				myGridLoadFuture.FailOn(MyGridLoadStatus.Fail_Unspecified);
				return myGridLoadFuture;
			}
			LoadData loadData = new LoadData();
			string text = guid.ToString();
			loadData.Path_Folder = Path.Combine(MySession.Static.CurrentPath, GRID_SAVE_FOLDER, text);
			loadData.Path_Detail = Path.Combine(loadData.Path_Folder, DETAIL_NAME) + STORAGE_EXTENSION;
			loadData.Path_Blueprint = Path.Combine(loadData.Path_Folder, text) + STORAGE_EXTENSION;
			loadData.NewOwner = newOwner;
			loadData.Transform = transform;
			loadData.Future = myGridLoadFuture;
			loadData.OnCompletion = onCompletion;
			Action<WorkData> action = delegate(WorkData workData1)
			{
				LoadData loadData3 = workData1 as LoadData;
				if (loadData3 != null)
				{
					LoadGrid_Internal_Phase1(loadData3);
				}
			};
			Action<WorkData> completionCallback = delegate(WorkData workData2)
			{
				LoadData loadData2 = workData2 as LoadData;
				if (loadData2 != null)
				{
					LoadGrid_Internal_Phase2(loadData2);
				}
			};
			Parallel.Start(action, completionCallback, loadData);
			return myGridLoadFuture;
		}

		private void LoadGrid_Internal_Phase1(LoadData data)
		{
			if (data != null)
			{
				if (!MyObjectBuilderSerializer.DeserializeXML(data.Path_Detail, out MyObjectBuilder_SavedGridDetails objectBuilder))
				{
					data.Future.FailOn(MyGridLoadStatus.Fail_DetailLoadFailed);
				}
				else
				{
					data.Detail = objectBuilder;
				}
			}
		}

		private void LoadGrid_Internal_Phase2(LoadData data)
		{
			if (data == null)
			{
				return;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(data.NewOwner);
			Vector3 value = data.Detail.AABB_min;
			Vector3 value2 = data.Detail.AABB_max;
			Vector3 v = 0.5f * (value2 + value);
			float radius = 0.5f * (value2 - value).Length();
			Vector3D value3 = Vector3D.Transform(v, data.Transform.Orientation);
			if (myIdentity == null)
			{
				data.Future.FailOn(MyGridLoadStatus.Fail_IdentityNotFound);
				return;
			}
			if (myIdentity.BlockLimits.PCU < data.Detail.PcuCount)
			{
				data.Future.FailOn(MyGridLoadStatus.Fail_NotEnoughPCU);
				return;
			}
			Vector3D? vector3D = MyEntities.FindFreePlace(data.Transform.Position + value3, radius);
			if (!vector3D.HasValue)
			{
				data.Future.FailOn(MyGridLoadStatus.Fail_CannotPlace);
				return;
			}
			data.Position = vector3D.Value;
			data.Radius = radius;
			Action<WorkData> action = delegate(WorkData workData3)
			{
				LoadData loadData2 = workData3 as LoadData;
				if (loadData2 != null)
				{
					LoadGrid_Internal_Phase3(loadData2);
				}
			};
			Action<WorkData> completionCallback = delegate(WorkData workData4)
			{
				LoadData loadData = workData4 as LoadData;
				if (loadData != null)
				{
					LoadGrid_Internal_Phase4(loadData);
				}
			};
			Parallel.Start(action, completionCallback, data);
		}

		private void LoadGrid_Internal_Phase3(LoadData data)
		{
			if (data != null)
			{
				MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyBlueprintUtils.LoadPrefab(data.Path_Blueprint);
				if (myObjectBuilder_Definitions == null)
				{
					data.Future.FailOn(MyGridLoadStatus.Fail_GridLoadFailed);
				}
				else
				{
					data.Builder = myObjectBuilder_Definitions;
				}
			}
		}

		private void LoadGrid_Internal_Phase4(LoadData data)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(data.NewOwner);
			if (myIdentity == null)
			{
				data.Future.FailOn(MyGridLoadStatus.Fail_IdentityNotFound);
				return;
			}
			if (myIdentity.BlockLimits.PCU < data.Detail.PcuCount)
			{
				data.Future.FailOn(MyGridLoadStatus.Fail_NotEnoughPCU, secondCheck: true);
				return;
			}
			if (!MyEntities.TestPlaceInSpace(data.Position, data.Radius).HasValue)
			{
				data.Future.FailOn(MyGridLoadStatus.Fail_CannotPlace, secondCheck: true);
				return;
			}
			data.Transform.Position = data.Position;
			MyObjectBuilder_CubeGrid[] cubeGrids = data.Builder.ShipBlueprints[0].CubeGrids;
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			MyObjectBuilder_CubeGrid[] array = cubeGrids;
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in array)
			{
				myObjectBuilder_CubeGrid.PositionAndOrientation = data.Transform;
				myObjectBuilder_CubeGrid.PositionAndOrientation.Value.Orientation.Normalize();
				foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
				{
					cubeBlock.Owner = data.NewOwner;
					cubeBlock.ShareMode = MyOwnershipShareModeEnum.None;
					cubeBlock.BuiltBy = data.NewOwner;
				}
				list.Add(myObjectBuilder_CubeGrid);
			}
			MyCubeGrid.RelativeOffset arg = default(MyCubeGrid.RelativeOffset);
			arg.Use = false;
			arg.RelativeToEntity = false;
			arg.SpawnerId = 0L;
			arg.OriginalSpawnPoint = Vector3D.Zero;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyCubeGrid.TryPasteGrid_Implementation, list, arg3: false, Vector3.Zero, arg5: false, arg6: true, arg);
			data.Future.Status = MyGridLoadStatus.Success;
			data.Future.IsFinished = true;
		}
	}
}
