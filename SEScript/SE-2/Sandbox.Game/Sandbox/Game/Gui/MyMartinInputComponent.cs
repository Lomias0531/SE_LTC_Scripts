using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyMartinInputComponent : MyDebugComponent
	{
		public class MyMarker
		{
			public Vector3D position;

			public Color color;

			public MyMarker(Vector3D position, Color color)
			{
				this.position = position;
				this.color = color;
			}
		}

		private class TmpScreen : MyGuiScreenBase
		{
			public TmpScreen()
				: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
			{
				base.EnabledBackgroundFade = true;
				m_size = new Vector2(0.99f, 0.88544f);
				AddCaption("<new screen>", Vector4.One, new Vector2(0f, 0.03f));
				base.CloseButtonEnabled = true;
				RecreateControls(contructor: true);
			}

			public override string GetFriendlyName()
			{
				return "TmpScreen";
			}

			public override void RecreateControls(bool contructor)
			{
				base.RecreateControls(contructor);
			}
		}

		private List<MyMarker> m_markers = new List<MyMarker>();

		public MyMartinInputComponent()
		{
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Add bots", AddBots);
			AddShortcut(MyKeys.Z, newPress: true, control: false, shift: false, alt: false, () => "One AI step", OneAIStep);
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "One Voxel step", OneVoxelStep);
			AddShortcut(MyKeys.Insert, newPress: true, control: false, shift: false, alt: false, () => "Add one bot", AddOneBot);
			AddShortcut(MyKeys.Home, newPress: true, control: false, shift: false, alt: false, () => "Add one barb", AddOneBarb);
			AddShortcut(MyKeys.T, newPress: true, control: false, shift: false, alt: false, () => "Do some action", DoSomeAction);
			AddShortcut(MyKeys.Y, newPress: true, control: false, shift: false, alt: false, () => "Clear some action", ClearSomeAction);
			AddShortcut(MyKeys.B, newPress: true, control: false, shift: false, alt: false, () => "Add berries", AddBerries);
			AddShortcut(MyKeys.L, newPress: true, control: false, shift: false, alt: false, () => "return to Last bot memory", ReturnToLastMemory);
			AddShortcut(MyKeys.N, newPress: true, control: false, shift: false, alt: false, () => "select Next bot", SelectNextBot);
			AddShortcut(MyKeys.K, newPress: true, control: false, shift: false, alt: false, () => "Kill not selected bots", KillNotSelectedBots);
			AddShortcut(MyKeys.M, newPress: true, control: false, shift: false, alt: false, () => "Toggle marker", ToggleMarker);
			AddSwitch(MyKeys.NumPad0, SwitchSwitch, new MyRef<bool>(() => MyFakes.DEBUG_BEHAVIOR_TREE, delegate(bool val)
			{
				MyFakes.DEBUG_BEHAVIOR_TREE = val;
			}), "allowed debug beh tree");
			AddSwitch(MyKeys.NumPad1, SwitchSwitch, new MyRef<bool>(() => MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP, delegate(bool val)
			{
				MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP = val;
			}), "one beh tree step");
			AddSwitch(MyKeys.H, SwitchSwitch, new MyRef<bool>(() => MyFakes.ENABLE_AUTO_HEAL, delegate(bool val)
			{
				MyFakes.ENABLE_AUTO_HEAL = val;
			}), "enable auto Heal");
		}

		private bool AddBerries()
		{
			AddSomething("Berries", 10);
			return true;
		}

		private void AddSomething(string something, int amount)
		{
			foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
			{
				MyPhysicalItemDefinition myPhysicalItemDefinition = allDefinition as MyPhysicalItemDefinition;
				if (myPhysicalItemDefinition != null && myPhysicalItemDefinition.CanSpawnFromScreen && allDefinition.DisplayNameText == something)
				{
					MyInventory inventory = (MySession.Static.ControlledEntity as MyEntity).GetInventory();
					if (inventory != null)
					{
						MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(allDefinition.Id);
						inventory.DebugAddItems(amount, objectBuilder);
					}
					break;
				}
			}
		}

		private void ConsumeSomething(string something, int amount)
		{
			foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
			{
				MyPhysicalItemDefinition myPhysicalItemDefinition = allDefinition as MyPhysicalItemDefinition;
				if (myPhysicalItemDefinition != null && myPhysicalItemDefinition.CanSpawnFromScreen && allDefinition.DisplayNameText == something)
				{
					MyInventory inventory = (MySession.Static.ControlledEntity as MyEntity).GetInventory();
					if (inventory != null)
					{
						_ = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(allDefinition.Id);
						inventory.ConsumeItem(myPhysicalItemDefinition.Id, amount, MySession.Static.LocalCharacterEntityId);
					}
					break;
				}
			}
		}

		private bool ReturnToLastMemory()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
			{
				MyBotCollection bots = MyAIComponent.Static.Bots;
				foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
				{
					MyAgentBot myAgentBot = allBot.Value as MyAgentBot;
					if (myAgentBot != null && bots.IsBotSelectedForDegugging(myAgentBot))
					{
						myAgentBot.ReturnToLastMemory();
					}
				}
			}
			return true;
		}

		private bool ToggleMarker()
		{
			Vector3D outPosition = default(Vector3D);
			if (GetDirectedPositionOnGround(MySector.MainCamera.Position, MySector.MainCamera.ForwardVector, 1000f, out outPosition))
			{
				MyMarker myMarker = FindClosestMarkerInArea(outPosition, 1.0);
				if (myMarker != null)
				{
					m_markers.Remove(myMarker);
				}
				else
				{
					m_markers.Add(new MyMarker(outPosition, Color.Blue));
				}
				return true;
			}
			return false;
		}

		public bool GetDirectedPositionOnGround(Vector3D initPosition, Vector3D direction, float amount, out Vector3D outPosition, float raycastHeight = 100f)
		{
			outPosition = default(Vector3D);
			MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart("Ground");
			if (myVoxelBase == null)
			{
				return false;
			}
			Vector3D to = initPosition + direction * amount;
			LineD line = new LineD(initPosition, to);
			Vector3D? v = null;
			myVoxelBase.GetIntersectionWithLine(ref line, out v);
			if (!v.HasValue)
			{
				return false;
			}
			outPosition = v.Value;
			return true;
		}

		private MyMarker FindClosestMarkerInArea(Vector3D pos, double maxDistance)
		{
			double num = double.MaxValue;
			MyMarker result = null;
			foreach (MyMarker marker in m_markers)
			{
				double num2 = (marker.position - pos).Length();
				if (num2 < num)
				{
					result = marker;
					num = num2;
				}
			}
			if (num < maxDistance)
			{
				return result;
			}
			return null;
		}

		private void AddMarker(MyMarker marker)
		{
			m_markers.Add(marker);
		}

		public bool SelectNextBot()
		{
			MyAIComponent.Static.Bots.DebugSelectNextBot();
			return true;
		}

		public bool KillNotSelectedBots()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
			{
				MyBotCollection bots = MyAIComponent.Static.Bots;
				foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
				{
					MyAgentBot myAgentBot = allBot.Value as MyAgentBot;
					if (myAgentBot != null && !bots.IsBotSelectedForDegugging(myAgentBot) && myAgentBot.Player.Controller.ControlledEntity is MyCharacter)
					{
						MyDamageInformation damageInfo = new MyDamageInformation(isDeformation: false, 1000f, MyDamageType.Weapon, MySession.Static.LocalPlayerId);
						(myAgentBot.Player.Controller.ControlledEntity as MyCharacter).Kill(sync: true, damageInfo);
					}
				}
			}
			return true;
		}

		public bool SwitchSwitch(MyKeys key)
		{
			bool value = !GetSwitchValue(key);
			SetSwitch(key, value);
			return true;
		}

		public bool SwitchSwitchDebugBeh(MyKeys key)
		{
			MyFakes.DEBUG_BEHAVIOR_TREE = !MyFakes.DEBUG_BEHAVIOR_TREE;
			SetSwitch(key, MyFakes.DEBUG_BEHAVIOR_TREE);
			return true;
		}

		public bool SwitchSwitchOneStep(MyKeys key)
		{
			MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP = true;
			SetSwitch(key, MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP);
			return true;
		}

		private bool DoSomeAction()
		{
			return true;
		}

		private bool ClearSomeAction()
		{
			return true;
		}

		private bool AddBots()
		{
			for (int i = 0; i < 10; i++)
			{
				MyAgentDefinition agentDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_HumanoidBot), "TestingBarbarian")) as MyAgentDefinition;
				MyAIComponent.Static.SpawnNewBot(agentDefinition);
			}
			return true;
		}

		private bool AddOneBot()
		{
			MyAgentDefinition agentDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_HumanoidBot), "NormalPeasant")) as MyAgentDefinition;
			MyAIComponent.Static.SpawnNewBot(agentDefinition);
			return true;
		}

		private bool AddOneBarb()
		{
			MyAgentDefinition agentDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_HumanoidBot), "SwordBarbarian")) as MyAgentDefinition;
			MyAIComponent.Static.SpawnNewBot(agentDefinition);
			return true;
		}

		private bool OneAIStep()
		{
			MyFakes.DEBUG_ONE_AI_STEP = true;
			return true;
		}

		private bool OneVoxelStep()
		{
			MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP = true;
			return true;
		}

		public override string GetName()
		{
			return "Martin";
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			CheckAutoHeal();
			return base.HandleInput();
		}

		private void CheckAutoHeal()
		{
			if (MyFakes.ENABLE_AUTO_HEAL)
			{
				MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
				if (myCharacter != null && myCharacter.StatComp != null && myCharacter.StatComp.HealthRatio < 1f)
				{
					AddSomething("Berries", 1);
					ConsumeSomething("Berries", 1);
				}
			}
		}

		private static void VoxelCellDrawing()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null && MySector.MainCamera != null)
			{
				Vector3D worldPosition = controlledEntity.Entity.WorldMatrix.Translation;
				MyVoxelBase myVoxelBase = null;
				foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
				{
					if (instance.PositionComp.WorldAABB.Contains(worldPosition) == ContainmentType.Contains)
					{
						myVoxelBase = instance;
						break;
					}
				}
				if (myVoxelBase != null)
				{
					MyCellCoord myCellCoord = default(MyCellCoord);
					MyVoxelCoordSystems.WorldPositionToGeometryCellCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition, out myCellCoord.CoordInLod);
					MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(myVoxelBase.PositionLeftBottomCorner, ref myCellCoord.CoordInLod, out BoundingBoxD worldAABB);
					MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition, out myCellCoord.CoordInLod);
					MyVoxelCoordSystems.VoxelCoordToWorldAABB(myVoxelBase.PositionLeftBottomCorner, ref myCellCoord.CoordInLod, out BoundingBoxD worldAABB2);
					MyRenderProxy.DebugDrawAABB(worldAABB2, Vector3.UnitX);
					MyRenderProxy.DebugDrawAABB(worldAABB, Vector3.UnitY);
				}
			}
		}

		private static void VoxelPlacement()
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null)
			{
				return;
			}
			int num = 0;
			Vector3D worldPosition = mainCamera.Position + (Vector3D)mainCamera.ForwardVector * 4.5 - num;
			MyVoxelBase myVoxelBase = null;
			foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
			{
				if (instance.PositionComp.WorldAABB.Contains(worldPosition) == ContainmentType.Contains)
				{
					myVoxelBase = instance;
					break;
				}
			}
			if (myVoxelBase == null)
			{
				return;
			}
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition, out Vector3I voxelCoord);
			MyVoxelCoordSystems.VoxelCoordToWorldPosition(myVoxelBase.PositionLeftBottomCorner, ref voxelCoord, out worldPosition);
			worldPosition += (float)num;
			float num2 = 3f;
			MyVoxelCoordSystems.VoxelCoordToWorldAABB(myVoxelBase.PositionLeftBottomCorner, ref voxelCoord, out BoundingBoxD worldAABB);
			MyRenderProxy.DebugDrawAABB(worldAABB, Color.Blue);
			BoundingSphereD boundingSphereD = new BoundingSphereD(worldPosition, num2 * 0.5f);
			MyRenderProxy.DebugDrawSphere(worldPosition, num2 * 0.5f, Color.White);
			if (MyInput.Static.IsLeftMousePressed())
			{
				MyShape myShape = new MyShapeSphere
				{
					Center = boundingSphereD.Center,
					Radius = (float)boundingSphereD.Radius
				};
				if (myShape != null)
				{
					MyVoxelGenerator.CutOutShapeWithProperties(myVoxelBase, myShape, out float _, out MyVoxelMaterialDefinition _);
				}
			}
		}

		private static void VoxelReading()
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null)
			{
				return;
			}
			int num = 0;
			Vector3D vector3D = mainCamera.Position + (Vector3D)mainCamera.ForwardVector * 4.5 - num;
			MyVoxelBase myVoxelBase = null;
			foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
			{
				if (instance.PositionComp.WorldAABB.Contains(vector3D) == ContainmentType.Contains)
				{
					myVoxelBase = instance;
					break;
				}
			}
			if (myVoxelBase != null)
			{
				Vector3D worldPosition = vector3D - Vector3.One * 1f;
				Vector3D worldPosition2 = vector3D + Vector3.One * 1f;
				MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition, out Vector3I voxelCoord);
				MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition2, out Vector3I voxelCoord2);
				MyVoxelCoordSystems.VoxelCoordToWorldPosition(myVoxelBase.PositionLeftBottomCorner, ref voxelCoord, out worldPosition);
				MyVoxelCoordSystems.VoxelCoordToWorldPosition(myVoxelBase.PositionLeftBottomCorner, ref voxelCoord2, out worldPosition2);
				BoundingBoxD aabb = BoundingBoxD.CreateInvalid();
				aabb.Include(worldPosition);
				aabb.Include(worldPosition2);
				MyRenderProxy.DebugDrawAABB(aabb, Vector3.One);
				if (MyInput.Static.IsNewLeftMousePressed())
				{
					MyStorageData myStorageData = new MyStorageData();
					myStorageData.Resize(voxelCoord, voxelCoord2);
					myVoxelBase.Storage.ReadRange(myStorageData, MyStorageDataTypeFlags.Content, 0, voxelCoord, voxelCoord2);
					myVoxelBase.Storage.WriteRange(myStorageData, MyStorageDataTypeFlags.Content, voxelCoord, voxelCoord2);
				}
			}
		}

		private static void MakeScreenWithIconGrid()
		{
			TmpScreen tmpScreen = new TmpScreen();
			MyGuiControlGrid myGuiControlGrid = new MyGuiControlGrid();
			tmpScreen.Controls.Add(myGuiControlGrid);
			myGuiControlGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			myGuiControlGrid.VisualStyle = MyGuiControlGridStyleEnum.Inventory;
			myGuiControlGrid.RowsCount = 12;
			myGuiControlGrid.ColumnsCount = 18;
			foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
			{
				myGuiControlGrid.Add(new MyGuiGridItem(allDefinition.Icons, null, allDefinition.DisplayNameText));
			}
			MyGuiSandbox.AddScreen(tmpScreen);
		}

		private static void MakeCharacterFakeTarget()
		{
			if (MyFakes.FakeTarget == null)
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (localCharacter != null)
				{
					MyFakes.FakeTarget = localCharacter;
				}
			}
			else
			{
				MyFakes.FakeTarget = null;
			}
		}

		public override void Draw()
		{
			base.Draw();
			foreach (MyMarker marker in m_markers)
			{
				MyRenderProxy.DebugDrawSphere(marker.position, 0.5f, marker.color, 0.8f);
				MyRenderProxy.DebugDrawSphere(marker.position, 0.1f, marker.color, 1f, depthRead: false);
				Vector3D position = marker.position;
				position.Y += 0.60000002384185791;
				string text = $"{marker.position.X:0.0},{marker.position.Y:0.0},{marker.position.Z:0.0}";
				MyRenderProxy.DebugDrawText3D(position, text, marker.color, 1f, depthRead: false);
			}
		}
	}
}
