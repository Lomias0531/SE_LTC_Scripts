using Sandbox.Definitions.GUI;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.SessionComponents;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class MyVisualScriptingDebugInputComponent : MyDebugComponent
	{
		private List<MyAreaTriggerComponent> m_queriedTriggers = new List<MyAreaTriggerComponent>();

		private MyAreaTriggerComponent m_selectedTrigger;

		private MatrixD m_lastCapturedCameraMatrix;

		public MyVisualScriptingDebugInputComponent()
		{
			AddSwitch(MyKeys.NumPad0, (MyKeys keys) => ToggleDebugDraw(), new MyRef<bool>(() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER, null), "Debug Draw");
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Attach new to entity", TryPutTriggerOnEntity);
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Snap to position", SnapTriggerToPosition);
			AddShortcut(MyKeys.NumPad2, newPress: true, control: true, shift: false, alt: false, () => "Trigger: Snap to triggers position", SnapToTriggersPosition);
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Spawn Trigger", SpawnTrigger);
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Naming: FatBlock/Floating Object", TryNamingAnBlockOrFloatingObject);
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Select", SelectTrigger);
			AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Naming: Grid", TryNamingAGrid);
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Delete trigger", DeleteTrigger);
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Set Size", SetTriggerSize);
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Reset missions + run GameStarted", ResetMissionsAndRunGameStarted);
			AddShortcut(MyKeys.Add, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Enlarge", () => ResizeATrigger(enlarge: true));
			AddShortcut(MyKeys.Subtract, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Shrink", () => ResizeATrigger(enlarge: false));
			AddShortcut(MyKeys.Multiply, newPress: true, control: false, shift: false, alt: false, () => "Trigger: Rename", RenameTrigger);
			AddShortcut(MyKeys.T, newPress: true, control: true, shift: false, alt: false, () => "Copy camera data", CopyCameraDataToClipboard);
			AddShortcut(MyKeys.N, newPress: true, control: true, shift: false, alt: false, () => "Spawn empty entity", SpawnEntityDebug);
			AddShortcut(MyKeys.B, newPress: true, control: true, shift: false, alt: false, () => "Reload Screen", ReloadScreen);
			m_lastCapturedCameraMatrix = MatrixD.Identity;
		}

		private bool ReloadScreen()
		{
			string[] array = new string[2]
			{
				Path.Combine(MyFileSystem.ContentPath, "Data", "Hud.sbc"),
				Path.Combine(MyFileSystem.ContentPath, "Data", "GuiTextures.sbc")
			};
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			MyGuiTextureAtlasDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyGuiTextureAtlasDefinition>(MyStringHash.GetOrCompute("Base"));
			if (!MyObjectBuilderSerializer.DeserializeXML(array[0], out MyObjectBuilder_Definitions objectBuilder))
			{
				MyAPIGateway.Utilities.ShowNotification("Failed to load Hud.sbc!", 3000, "Red");
				return false;
			}
			hudDefinition.Init(objectBuilder.Definitions[0], hudDefinition.Context);
			if (!MyObjectBuilderSerializer.DeserializeXML(array[1], out objectBuilder))
			{
				MyAPIGateway.Utilities.ShowNotification("Failed to load GuiTextures.sbc!", 3000, "Red");
				return false;
			}
			definition.Init(objectBuilder.Definitions[0], definition.Context);
			MyScreenManager.CloseScreen(MyPerGameSettings.GUI.HUDScreen);
			MyScreenManager.AddScreen(Activator.CreateInstance(MyPerGameSettings.GUI.HUDScreen) as MyGuiScreenBase);
			return true;
		}

		private bool ResetMissionsAndRunGameStarted()
		{
			MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.Reset();
			return true;
		}

		private bool CopyCameraDataToClipboard()
		{
			MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			string clipboard = string.Concat("Position:  ", worldMatrix.Translation, "\nDirection: ", worldMatrix.Forward, "\nUp:        ", worldMatrix.Up);
			MyVRage.Platform.Clipboard = clipboard;
			m_lastCapturedCameraMatrix = new MatrixD(worldMatrix);
			return true;
		}

		public override string GetName()
		{
			return "Visual Scripting";
		}

		public override void Update10()
		{
			base.Update10();
			if (MyAPIGateway.Session != null)
			{
				m_queriedTriggers.Clear();
				foreach (MyTriggerComponent intersectingTrigger in MySessionComponentTriggerSystem.Static.GetIntersectingTriggers(MyAPIGateway.Session.Camera.Position))
				{
					MyAreaTriggerComponent myAreaTriggerComponent = intersectingTrigger as MyAreaTriggerComponent;
					if (myAreaTriggerComponent != null)
					{
						m_queriedTriggers.Add(myAreaTriggerComponent);
					}
				}
			}
		}

		private bool RenameTrigger()
		{
			if (m_selectedTrigger == null)
			{
				return false;
			}
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Rename Dialog", m_selectedTrigger.Name, delegate(string text)
			{
				m_selectedTrigger.Name = text;
				return true;
			}));
			return true;
		}

		private bool SelectTrigger()
		{
			Vector3D position = MyAPIGateway.Session.Camera.Position;
			double num = double.MaxValue;
			if (m_selectedTrigger != null)
			{
				m_selectedTrigger.CustomDebugColor = Color.Red;
			}
			foreach (MyAreaTriggerComponent queriedTrigger in m_queriedTriggers)
			{
				double num2 = (queriedTrigger.Center - position).LengthSquared();
				if (num2 < num)
				{
					num = num2;
					m_selectedTrigger = queriedTrigger;
				}
			}
			if (Math.Abs(num - double.MaxValue) < double.Epsilon)
			{
				m_selectedTrigger = null;
			}
			if (m_selectedTrigger != null)
			{
				m_selectedTrigger.CustomDebugColor = Color.Yellow;
			}
			return true;
		}

		private bool SnapToTriggersPosition()
		{
			if (m_selectedTrigger == null)
			{
				return true;
			}
			if (MySession.Static.ControlledEntity is MyCharacter)
			{
				MySession.Static.LocalCharacter.PositionComp.SetPosition(m_selectedTrigger.Center);
			}
			return true;
		}

		private bool SnapTriggerToPosition()
		{
			if (m_selectedTrigger == null)
			{
				return false;
			}
			if (MyAPIGateway.Session.CameraController is MySpectatorCameraController)
			{
				m_selectedTrigger.Center = MyAPIGateway.Session.Camera.Position;
			}
			else
			{
				m_selectedTrigger.Center = MyAPIGateway.Session.LocalHumanPlayer.GetPosition();
			}
			return true;
		}

		public bool SetTriggerSize()
		{
			if (m_selectedTrigger == null)
			{
				return false;
			}
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Set trigger size dialog", m_selectedTrigger.Radius.ToString(CultureInfo.InvariantCulture), delegate(string text)
			{
				if (!float.TryParse(text, out float result))
				{
					return false;
				}
				m_selectedTrigger.Radius = result;
				return true;
			}));
			return true;
		}

		public bool DeleteTrigger()
		{
			if (m_selectedTrigger == null)
			{
				return false;
			}
			if (m_selectedTrigger.Entity.DisplayName == "TriggerHolder")
			{
				m_selectedTrigger.Entity.Close();
			}
			else
			{
				m_selectedTrigger.Entity.Components.Remove(typeof(MyAreaTriggerComponent), m_selectedTrigger);
			}
			m_selectedTrigger = null;
			return true;
		}

		public bool ResizeATrigger(bool enlarge)
		{
			if (m_selectedTrigger == null)
			{
				return false;
			}
			m_selectedTrigger.Radius = (enlarge ? (m_selectedTrigger.Radius + 0.2) : (m_selectedTrigger.Radius - 0.2));
			return true;
		}

		public override void Draw()
		{
			base.Draw();
			if (!MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER)
			{
				return;
			}
			Vector2 screenCoord = new Vector2(350f, 10f);
			StringBuilder stringBuilder = new StringBuilder();
			MyRenderProxy.DebugDrawText2D(screenCoord, "Queried Triggers", Color.White, 0.7f);
			foreach (MyAreaTriggerComponent queriedTrigger in m_queriedTriggers)
			{
				screenCoord.Y += 20f;
				stringBuilder.Clear();
				if (queriedTrigger.Entity != null && queriedTrigger.Entity.Name != null)
				{
					stringBuilder.Append("EntityName: " + queriedTrigger.Entity.Name + " ");
				}
				stringBuilder.Append("Trigger: " + queriedTrigger.Name + " radius: " + queriedTrigger.Radius);
				MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), Color.White, 0.7f);
			}
			screenCoord.X += 250f;
			screenCoord.Y = 10f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Selected Trigger", Color.White, 0.7f);
			screenCoord.Y += 20f;
			stringBuilder.Clear();
			if (m_selectedTrigger != null)
			{
				if (m_selectedTrigger.Entity != null && m_selectedTrigger.Entity.Name != null)
				{
					stringBuilder.Append("EntityName: " + m_selectedTrigger.Entity.Name + " ");
				}
				stringBuilder.Append("Trigger: " + m_selectedTrigger.Name + " radius: " + m_selectedTrigger.Radius);
				MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), Color.White, 0.7f);
			}
			if (m_lastCapturedCameraMatrix != MatrixD.Identity)
			{
				MyRenderProxy.DebugDrawAxis(m_lastCapturedCameraMatrix, 5f, depthRead: true);
			}
		}

		public bool ToggleDebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
				MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = false;
			}
			else
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = true;
			}
			return true;
		}

		public bool SpawnTrigger()
		{
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Spawn new Trigger", "", delegate(string text)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(text);
				MyEntity myEntity = new MyEntity();
				myAreaTriggerComponent.Radius = 2.0;
				myAreaTriggerComponent.Center = MyAPIGateway.Session.Camera.Position;
				myEntity.PositionComp.SetPosition(MyAPIGateway.Session.Camera.Position);
				myEntity.PositionComp.LocalVolume = new BoundingSphere(Vector3.Zero, 0.5f);
				myEntity.EntityId = MyEntityIdentifier.AllocateId();
				myEntity.Components.Remove<MyPhysicsComponentBase>();
				myEntity.Components.Remove<MyRenderComponentBase>();
				myEntity.DisplayName = "TriggerHolder";
				MyEntities.Add(myEntity);
				if (!myEntity.Components.Contains(typeof(MyTriggerAggregate)))
				{
					myEntity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
				}
				myEntity.Components.Get<MyTriggerAggregate>().AddComponent(myAreaTriggerComponent);
				if (m_selectedTrigger != null)
				{
					m_selectedTrigger.CustomDebugColor = Color.Red;
				}
				m_selectedTrigger = myAreaTriggerComponent;
				m_selectedTrigger.CustomDebugColor = Color.Yellow;
				return true;
			}));
			return true;
		}

		public bool SpawnEntityDebug()
		{
			SpawnEntity(null);
			return true;
		}

		public static MyEntity SpawnEntity(Action<MyEntity> onEntity)
		{
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Spawn new Entity", "", delegate(string text)
			{
				MyEntity myEntity = new MyEntity();
				myEntity.WorldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
				myEntity.PositionComp.SetPosition(MyAPIGateway.Session.Camera.Position);
				myEntity.EntityId = MyEntityIdentifier.AllocateId();
				myEntity.Components.Remove<MyPhysicsComponentBase>();
				myEntity.Components.Remove<MyRenderComponentBase>();
				myEntity.DisplayName = "EmptyEntity";
				MyEntities.Add(myEntity);
				myEntity.Name = text;
				MyEntities.SetEntityName(myEntity);
				if (onEntity != null)
				{
					onEntity(myEntity);
				}
				return true;
			}));
			return null;
		}

		private bool TryPutTriggerOnEntity()
		{
			MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(worldMatrix.Translation, worldMatrix.Translation + worldMatrix.Forward * 30.0, list, 15);
			foreach (MyPhysics.HitInfo item in list)
			{
				MyPhysicsBody myPhysicsBody = (MyPhysicsBody)item.HkHitInfo.Body.UserObject;
				if (myPhysicsBody.Entity is MyCubeGrid)
				{
					MyEntity rayEntity = (MyEntity)myPhysicsBody.Entity;
					MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Entity Spawn on: " + rayEntity.DisplayName, "", delegate(string text)
					{
						if (m_selectedTrigger != null)
						{
							m_selectedTrigger.CustomDebugColor = Color.Red;
						}
						m_selectedTrigger = new MyAreaTriggerComponent(text);
						if (!rayEntity.Components.Contains(typeof(MyTriggerAggregate)))
						{
							rayEntity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
						}
						rayEntity.Components.Get<MyTriggerAggregate>().AddComponent(m_selectedTrigger);
						m_selectedTrigger.Center = MyAPIGateway.Session.Camera.Position;
						m_selectedTrigger.Radius = 2.0;
						m_selectedTrigger.CustomDebugColor = Color.Yellow;
						return true;
					}));
					return true;
				}
			}
			return false;
		}

		private void NameDialog(MyEntity entity)
		{
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Name a Grid dialog: " + entity.DisplayName, entity.Name ?? (entity.DisplayName + " has no name."), delegate(string text)
			{
				if (!MyEntities.TryGetEntityByName(text, out MyEntity _))
				{
					entity.Name = text;
					MyEntities.SetEntityName(entity);
					return true;
				}
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Entity with same name already exits, please enter different name."), new StringBuilder("Naming error")));
				return false;
			}));
		}

		public bool TryNamingAGrid()
		{
			MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(worldMatrix.Translation, worldMatrix.Translation + worldMatrix.Forward * 5.0, list, 15);
			foreach (MyPhysics.HitInfo item in list)
			{
				MyEntity myEntity = (MyEntity)item.HkHitInfo.GetHitEntity();
				if (myEntity is MyCubeGrid)
				{
					NameDialog(myEntity);
					return true;
				}
			}
			return false;
		}

		public bool TryNamingAnBlockOrFloatingObject()
		{
			MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			Vector3D position = worldMatrix.Translation + worldMatrix.Forward * 0.5;
			RayD rayD = new RayD(position, worldMatrix.Forward * 1000.0);
			BoundingSphereD boundingSphere = new BoundingSphereD(worldMatrix.Translation, 30.0);
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(worldMatrix.Translation, worldMatrix.Translation + worldMatrix.Forward * 5.0, list, 15);
			foreach (MyPhysics.HitInfo item in list)
			{
				MyPhysicsBody myPhysicsBody = (MyPhysicsBody)item.HkHitInfo.Body.UserObject;
				if (myPhysicsBody.Entity is MyFloatingObject)
				{
					MyEntity entity = (MyEntity)myPhysicsBody.Entity;
					NameDialog(entity);
					return true;
				}
			}
			foreach (MyEntity item2 in entitiesInSphere)
			{
				MyCubeGrid myCubeGrid = item2 as MyCubeGrid;
				if (myCubeGrid != null && rayD.Intersects(item2.PositionComp.WorldAABB).HasValue)
				{
					Vector3I? vector3I = myCubeGrid.RayCastBlocks(worldMatrix.Translation, worldMatrix.Translation + worldMatrix.Forward * 100.0);
					if (vector3I.HasValue)
					{
						MySlimBlock block = myCubeGrid.GetCubeBlock(vector3I.Value);
						if (block.FatBlock != null)
						{
							MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Name block dialog: " + block.FatBlock.DefinitionDisplayNameText, block.FatBlock.Name ?? (block.FatBlock.DefinitionDisplayNameText + " has no name."), delegate(string text)
							{
								if (!MyEntities.TryGetEntityByName(text, out MyEntity _))
								{
									block.FatBlock.Name = text;
									MyEntities.SetEntityName(block.FatBlock);
									return true;
								}
								MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Entity with same name already exits, please enter different name."), new StringBuilder("Naming error")));
								return false;
							}));
							entitiesInSphere.Clear();
							return true;
						}
					}
				}
			}
			entitiesInSphere.Clear();
			return false;
		}
	}
}
