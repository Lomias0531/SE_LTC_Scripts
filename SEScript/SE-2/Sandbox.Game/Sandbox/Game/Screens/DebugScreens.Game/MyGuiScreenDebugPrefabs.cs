using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens.Game
{
	[MyDebugScreen("Game", "Prefabs")]
	public class MyGuiScreenDebugPrefabs : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_groupCombo;

		private MyGuiControlCombobox m_prefabsCombo;

		private MyGuiControlCombobox m_behaviourCombo;

		private MyGuiControlSlider m_frequency;

		private MyGuiControlSlider m_AIactivationDistance;

		private MyGuiControlCheckbox m_isPirate;

		private MyGuiControlCheckbox m_reactorsOn;

		private MyGuiControlCheckbox m_isEncounter;

		private MyGuiControlCheckbox m_resetOwnership;

		private MyGuiControlCheckbox m_isCargoShip;

		private MyGuiControlSlider m_distanceSlider;

		public MyGuiScreenDebugPrefabs()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugPrefabs";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 vector = new Vector2(0f, 0.03f);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Prefabs", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition += vector;
			MyGuiControlButton myGuiControlButton = AddButton("Export clipboard as prefab", ExportPrefab);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Default;
			myGuiControlButton.Size *= new Vector2(4f, 1f);
			m_currentPosition += vector;
			m_isPirate = AddCheckBox("IsPirate", enabled: true);
			m_reactorsOn = AddCheckBox("ReactorsOn", enabled: true);
			m_isEncounter = AddCheckBox("IsEncounter", enabled: true);
			m_resetOwnership = AddCheckBox("ResetOwnership", enabled: true);
			m_isCargoShip = AddCheckBox("IsCargoShip", enabled: false);
			m_frequency = AddSlider("Frequency", 1f, 0f, 10f);
			m_AIactivationDistance = AddSlider("AI activation distance", 1000f, 1f, 10000f);
			m_distanceSlider = AddSlider("Spawn distance", 100f, 1f, 10000f);
			m_currentPosition.Y += 0.02f;
			m_behaviourCombo = AddCombo();
			m_behaviourCombo.AddItem(0L, "No AI");
			foreach (string key in MyDroneAIDataStatic.Presets.Keys)
			{
				m_behaviourCombo.AddItem(m_behaviourCombo.GetItemsCount(), key);
			}
			m_behaviourCombo.SelectItemByIndex(0);
			MyGuiControlButton myGuiControlButton2 = AddButton("Export world as spawn group", ExportSpawnGroup);
			myGuiControlButton2.VisualStyle = MyGuiControlButtonStyleEnum.Default;
			myGuiControlButton2.Size *= new Vector2(4f, 1f);
			m_currentPosition += vector;
			m_groupCombo = AddCombo();
			foreach (MySpawnGroupDefinition spawnGroupDefinition in MyDefinitionManager.Static.GetSpawnGroupDefinitions())
			{
				m_groupCombo.AddItem((int)spawnGroupDefinition.Id.SubtypeId, spawnGroupDefinition.Id.SubtypeName);
			}
			m_groupCombo.SelectItemByIndex(0);
			AddButton("Spawn group", SpawnGroup);
			m_currentPosition += vector;
			m_prefabsCombo = AddCombo();
			foreach (MyPrefabDefinition value in MyDefinitionManager.Static.GetPrefabDefinitions().Values)
			{
				m_prefabsCombo.AddItem((int)value.Id.SubtypeId, value.Id.SubtypeName);
			}
			m_prefabsCombo.SelectItemByIndex(0);
			AddButton("Spawn prefab", OnSpawnPrefab);
			AddButton("Summon cargo ship spawn", OnSpawnCargoShip).VisualStyle = MyGuiControlButtonStyleEnum.Default;
			myGuiControlButton2.Size *= new Vector2(4f, 1f);
		}

		private void OnSpawnCargoShip(MyGuiControlButton obj)
		{
			if (!MyFakes.ENABLE_CARGO_SHIPS || !MySession.Static.CargoShipsEnabled)
			{
				MyHud.Notifications.Add(new MyHudNotificationDebug("Cargo ships are disabled in this world", 5000));
				return;
			}
			MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
			MyGlobalEvents.RemoveGlobalEvent(eventById);
			eventById.SetActivationTime(TimeSpan.Zero);
			MyGlobalEvents.AddGlobalEvent(eventById);
			MyHud.Notifications.Add(new MyHudNotificationDebug("Cargo ship will spawn soon(tm)", 5000));
		}

		private void OnSpawnPrefab(MyGuiControlButton _)
		{
			MyCamera mainCamera = MySector.MainCamera;
			MyPrefabDefinition myPrefabDefinition = MyDefinitionManager.Static.GetPrefabDefinitions()[m_prefabsCombo.GetSelectedValue().ToString()];
			float radius = myPrefabDefinition.BoundingSphere.Radius;
			Vector3D position = mainCamera.Position + mainCamera.ForwardVector * (m_distanceSlider.Value + Math.Min(100f, radius * 1.5f));
			List<MyCubeGrid> grids = new List<MyCubeGrid>();
			Stack<Action> stack = new Stack<Action>();
			if (m_behaviourCombo.GetSelectedKey() != 0L)
			{
				string AI = m_behaviourCombo.GetSelectedValue().ToString();
				stack.Push(delegate
				{
					if (grids.Count > 0)
					{
						MyCubeGrid myCubeGrid = grids[0];
						MyVisualScriptLogicProvider.SetName(myCubeGrid.EntityId, "Drone");
						MyVisualScriptLogicProvider.SetDroneBehaviourAdvanced("Drone", AI, activate: true, assignToPirates: true, null, cycleWaypoints: false, MyEntities.GetEntities().OfType<MyCharacter>().Cast<MyEntity>()
							.ToList());
						MyVisualScriptLogicProvider.SetName(myCubeGrid.EntityId, null);
					}
				});
			}
			MatrixD matrixD = MatrixD.CreateTranslation(position);
			MyPrefabManager.Static.SpawnPrefab(grids, myPrefabDefinition.Id.SubtypeName, position, matrixD.Forward, matrixD.Up, default(Vector3), default(Vector3), null, null, SpawningOptions.UseGridOrigin, 0L, updateSync: false, stack);
		}

		private void SpawnGroup(MyGuiControlButton _)
		{
			MySpawnGroupDefinition mySpawnGroupDefinition = MyDefinitionManager.Static.GetSpawnGroupDefinitions()[m_groupCombo.GetSelectedIndex()];
			var list = mySpawnGroupDefinition.Prefabs.Select((MySpawnGroupDefinition.SpawnGroupPrefab x) => new
			{
				Position = x.Position,
				Prefab = MyDefinitionManager.Static.GetPrefabDefinition(x.SubtypeId)
			}).ToList();
			var list2 = (from x in mySpawnGroupDefinition.Voxels.Select(delegate(MySpawnGroupDefinition.SpawnGroupVoxel x)
				{
					MyOctreeStorage myOctreeStorage = (MyOctreeStorage)MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(x.StorageName));
					return (myOctreeStorage == null) ? null : new
					{
						Voxel = myOctreeStorage,
						Position = x.Offset,
						Name = x.StorageName
					};
				})
				where x != null
				select x).ToList();
			BoundingSphere boundingSphere = BoundingSphere.CreateInvalid();
			foreach (var item in list)
			{
				Vector3 translation = item.Position;
				boundingSphere.Include(item.Prefab.BoundingSphere.Translate(ref translation));
			}
			foreach (var item2 in list2)
			{
				boundingSphere.Include(new BoundingSphere(item2.Position, item2.Voxel.Size.AbsMax()));
			}
			MyCamera mainCamera = MySector.MainCamera;
			float radius = boundingSphere.Radius;
			Vector3D value = mainCamera.Position + mainCamera.ForwardVector * (m_distanceSlider.Value + Math.Min(100f, radius * 1.5f));
			foreach (var item3 in list2)
			{
				MatrixD worldMatrix = MatrixD.CreateWorld(value + item3.Position);
				MyWorldGenerator.AddVoxelMap(item3.Name, item3.Voxel, worldMatrix, 0L, lazyPhysics: false, useVoxelOffset: false);
			}
			foreach (var item4 in list)
			{
				Vector3D position = value + item4.Position;
				MatrixD matrixD = MatrixD.CreateTranslation(position);
				MyPrefabManager.Static.SpawnPrefab(item4.Prefab.Id.SubtypeName, position, matrixD.Forward, matrixD.Up, default(Vector3), default(Vector3), null, null, SpawningOptions.UseGridOrigin, 0L);
			}
		}

		private void ExportPrefab(MyGuiControlButton _)
		{
			string name = MyUtils.StripInvalidChars(MyClipboardComponent.Static.Clipboard.CopiedGridsName);
			string exportFineName = GetExportFineName("Prefabs", name, ".sbc");
			MyClipboardComponent.Static.Clipboard.SaveClipboardAsPrefab(name, exportFineName);
		}

		private void ExportSpawnGroup(MyGuiControlButton obj)
		{
			List<MyCubeGrid> list = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList();
			List<MyVoxelBase> list2 = MyEntities.GetEntities().OfType<MyVoxelBase>().ToList();
			if (list.Count != 0)
			{
				string text = MyUtils.StripInvalidChars(list[0].DisplayName ?? list[0].Name ?? list[0].DebugName ?? "No name");
				string folder = Path.Combine("SpawnGroups", Path.GetFileName(GetExportFineName("SpawnGroups", text, string.Empty)));
				MyEntity myEntity = list2.Concat<MyEntity>(list).First();
				Vector3D basePosition = myEntity.PositionComp.GetPosition();
				MyObjectBuilder_SpawnGroupDefinition myObjectBuilder_SpawnGroupDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SpawnGroupDefinition>(text);
				myObjectBuilder_SpawnGroupDefinition.Voxels = Array.Empty<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel>();
				myObjectBuilder_SpawnGroupDefinition.Prefabs = Array.Empty<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab>();
				MySpawnGroupDefinition mySpawnGroupDefinition = new MySpawnGroupDefinition();
				mySpawnGroupDefinition.Init(myObjectBuilder_SpawnGroupDefinition, MyModContext.BaseGame);
				mySpawnGroupDefinition.Id = new MyDefinitionId(typeof(MyObjectBuilder_SpawnGroupDefinition), text);
				mySpawnGroupDefinition.Frequency = m_frequency.Value;
				mySpawnGroupDefinition.IsPirate = m_isPirate.IsChecked;
				mySpawnGroupDefinition.ReactorsOn = m_reactorsOn.IsChecked;
				mySpawnGroupDefinition.IsEncounter = m_isEncounter.IsChecked;
				mySpawnGroupDefinition.IsCargoShip = m_isCargoShip.IsChecked;
				mySpawnGroupDefinition.Voxels.AddRange(list2.Select(delegate(MyVoxelBase x)
				{
					x.Storage.Save(out byte[] outCompressedData);
					string text2 = MyUtils.StripInvalidChars(x.StorageName);
					string exportFineName3 = GetExportFineName(folder, text2, ".vx2");
					text2 = Path.GetFileNameWithoutExtension(text2);
					Directory.CreateDirectory(Path.GetDirectoryName(exportFineName3));
					File.WriteAllBytes(exportFineName3, outCompressedData);
					MySpawnGroupDefinition.SpawnGroupVoxel result = default(MySpawnGroupDefinition.SpawnGroupVoxel);
					result.CenterOffset = true;
					result.StorageName = text2;
					result.Offset = x.PositionComp.GetPosition() - basePosition;
					return result;
				}));
				Vector3D firstGridPosition = list[0].PositionComp.GetPosition();
				string exportFineName = GetExportFineName(folder, text, ".sbc");
				MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = MyPrefabManager.SavePrefabToPath(Path.GetFileNameWithoutExtension(exportFineName), exportFineName, list.Select(delegate(MyCubeGrid x)
				{
					MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)x.GetObjectBuilder();
					MyPositionAndOrientation value = myObjectBuilder_CubeGrid.PositionAndOrientation.Value;
					Vector3D v = value.Position;
					v -= firstGridPosition;
					value.Position = v;
					myObjectBuilder_CubeGrid.PositionAndOrientation = value;
					foreach (MyObjectBuilder_CubeBlock cubeBlock in myObjectBuilder_CubeGrid.CubeBlocks)
					{
						cubeBlock.Owner = 0L;
						cubeBlock.BuiltBy = 0L;
					}
					return myObjectBuilder_CubeGrid;
				}).ToList());
				mySpawnGroupDefinition.Prefabs.Add(new MySpawnGroupDefinition.SpawnGroupPrefab
				{
					Speed = 0f,
					ResetOwnership = m_resetOwnership.IsChecked,
					Position = firstGridPosition - basePosition,
					BeaconText = string.Empty,
					PlaceToGridOrigin = false,
					SubtypeId = myObjectBuilder_PrefabDefinition.Id.SubtypeId,
					BehaviourActivationDistance = m_AIactivationDistance.Value,
					Behaviour = ((m_behaviourCombo.GetSelectedKey() == 0L) ? null : m_behaviourCombo.GetSelectedValue().ToString())
				});
				MyObjectBuilder_DefinitionBase objectBuilder = mySpawnGroupDefinition.GetObjectBuilder();
				string exportFineName2 = GetExportFineName(folder, "Group__" + text, ".sbc");
				objectBuilder.Save(exportFineName2);
				MyHud.Notifications.Add(new MyHudNotificationDebug("Group saved: " + exportFineName2, 10000));
			}
		}

		private string GetExportFineName(string folder, string name, string extension)
		{
			int num = 0;
			string text;
			do
			{
				string path = name + ((num++ == 0) ? string.Empty : ("_" + num)) + extension;
				text = Path.Combine(MyFileSystem.UserDataPath, "Export", folder, path);
			}
			while (MyFileSystem.FileExists(text) || MyFileSystem.DirectoryExists(text));
			return text;
		}
	}
}
