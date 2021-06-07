using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.Input;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenScriptingTools : MyGuiScreenDebugBase
	{
		private enum ScriptingToolsScreen
		{
			Transformation,
			Cutscenes
		}

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private static readonly float ITEM_HORIZONTAL_PADDING = 0.01f;

		private static readonly float ITEM_VERTICAL_PADDING = 0.005f;

		private static readonly Vector2 BUTTON_SIZE = new Vector2(0.06f, 0.03f);

		private static readonly Vector2 ITEM_SIZE = new Vector2(0.06f, 0.02f);

		private static readonly string ENTITY_NAME_PREFIX = "Waypoint_";

		private static int InitialShift = 0;

		private static uint m_entityCounter = 0u;

		private static ScriptingToolsScreen m_currentScreen = ScriptingToolsScreen.Transformation;

		private IMyCameraController m_previousCameraController;

		private MyGuiControlButton m_setTriggerSizeButton;

		private MyGuiControlButton m_enlargeTriggerButton;

		private MyGuiControlButton m_shrinkTriggerButton;

		private MyGuiControlListbox m_triggersListBox;

		private MyGuiControlListbox m_waypointsListBox;

		private MyGuiControlListbox m_smListBox;

		private MyGuiControlListbox m_levelScriptListBox;

		private MyGuiControlTextbox m_selectedTriggerNameBox;

		private MyGuiControlTextbox m_selectedEntityNameBox;

		private MyGuiControlTextbox m_selectedFunctionalBlockNameBox;

		private MyEntity m_selectedFunctionalBlock;

		private bool m_disablePicking;

		private readonly MyTriggerManipulator m_triggerManipulator;

		private readonly MyEntityTransformationSystem m_transformSys;

		private readonly MyVisualScriptManagerSessionComponent m_scriptManager;

		private readonly MySessionComponentScriptSharedStorage m_scriptStorage;

		private readonly StringBuilder m_helperStringBuilder = new StringBuilder();

		public List<MyEntity> m_waypoints = new List<MyEntity>();

		private Dictionary<string, Cutscene> m_cutscenes;

		private Cutscene m_cutsceneCurrent;

		private int m_selectedCutsceneNodeIndex = -1;

		private bool m_cutscenePlaying;

		private MyGuiControlCombobox m_cutsceneSelection;

		private MyGuiControlButton m_cutsceneDeleteButton;

		private MyGuiControlButton m_cutscenePlayButton;

		private MyGuiControlButton m_cutsceneRevertButton;

		private MyGuiControlButton m_cutsceneSaveButton;

		private MyGuiControlTextbox m_cutscenePropertyStartEntity;

		private MyGuiControlTextbox m_cutscenePropertyStartLookAt;

		private MyGuiControlCombobox m_cutscenePropertyNextCutscene;

		private MyGuiControlTextbox m_cutscenePropertyStartingFOV;

		private MyGuiControlCheckbox m_cutscenePropertyCanBeSkipped;

		private MyGuiControlCheckbox m_cutscenePropertyFireEventsDuringSkip;

		private MyGuiControlListbox m_cutsceneNodes;

		private MyGuiControlButton m_cutsceneNodeButtonAdd;

		private MyGuiControlButton m_cutsceneNodeButtonMoveUp;

		private MyGuiControlButton m_cutsceneNodeButtonMoveDown;

		private MyGuiControlButton m_cutsceneNodeButtonDelete;

		private MyGuiControlButton m_cutsceneNodeButtonDeleteAll;

		private MyGuiControlTextbox m_cutsceneNodePropertyTime;

		private MyGuiControlTextbox m_cutsceneNodePropertyMoveTo;

		private MyGuiControlTextbox m_cutsceneNodePropertyMoveToInstant;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateLike;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateLikeInstant;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowards;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowardsInstant;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowardsLock;

		private MyGuiControlTextbox m_cutsceneNodePropertyAttachAll;

		private MyGuiControlTextbox m_cutsceneNodePropertyAttachPosition;

		private MyGuiControlTextbox m_cutsceneNodePropertyAttachRotation;

		private MyGuiControlTextbox m_cutsceneNodePropertyEvent;

		private MyGuiControlTextbox m_cutsceneNodePropertyEventDelay;

		private MyGuiControlTextbox m_cutsceneNodePropertyFOVChange;

		private MyGuiControlTextbox m_cutsceneNodePropertyWaypoints;

		public MyGuiScreenScriptingTools()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			m_triggerManipulator = new MyTriggerManipulator((MyTriggerComponent trigger) => trigger is MyAreaTriggerComponent);
			m_transformSys = MySession.Static.GetComponent<MyEntityTransformationSystem>();
			m_transformSys.ControlledEntityChanged += TransformSysOnControlledEntityChanged;
			m_transformSys.RayCasted += TransformSysOnRayCasted;
			m_scriptManager = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			m_scriptStorage = MySession.Static.GetComponent<MySessionComponentScriptSharedStorage>();
			MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse);
			MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
			MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = true;
			RecreateControls(constructor: true);
			InitializeWaypointList();
			if (m_currentScreen == ScriptingToolsScreen.Transformation)
			{
				UpdateWaypointList();
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (m_transformSys.DisablePicking)
			{
				m_transformSys.DisablePicking = false;
			}
			if (MyInput.Static.IsNewPrimaryButtonPressed())
			{
				Vector2 normalizedCoordinateFromScreenCoordinate = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(MyInput.Static.GetMousePosition());
				Vector2 vector = GetPosition() - SCREEN_SIZE * 0.5f;
				if (normalizedCoordinateFromScreenCoordinate.X > vector.X)
				{
					m_transformSys.DisablePicking = true;
				}
			}
			if (!MyToolbarComponent.IsToolbarControlShown)
			{
				MyToolbarComponent.IsToolbarControlShown = true;
			}
			if (m_currentScreen == ScriptingToolsScreen.Transformation)
			{
				base.FocusedControl = null;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyInput.Static.IsNewKeyPressed(MyKeys.F11))
			{
				if (m_currentScreen == ScriptingToolsScreen.Transformation || !m_cutsceneSaveButton.Enabled)
				{
					CloseScreen();
				}
				else
				{
					CloseScreenWithSave();
				}
				return;
			}
			base.HandleInput(receivedFocusInThisUpdate);
			if (MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.FreeMouse)
			{
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
			}
			foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
			{
				if (!(screen is MyGuiScreenScriptingTools))
				{
					screen.HandleInput(receivedFocusInThisUpdate);
				}
			}
			if (m_currentScreen == ScriptingToolsScreen.Transformation)
			{
				HandleShortcuts();
			}
		}

		private void HandleShortcuts()
		{
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.D))
			{
				DeselectEntityOnClicked(null);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
			{
				if (MyInput.Static.IsKeyPress(MyKeys.Control))
				{
					if (MyInput.Static.IsKeyPress(MyKeys.Shift))
					{
						InitialShift -= 1000;
					}
					else
					{
						InitialShift -= 10;
					}
				}
				else if (MyInput.Static.IsKeyPress(MyKeys.Shift))
				{
					InitialShift -= 100;
				}
				else
				{
					InitialShift--;
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
			{
				if (MyInput.Static.IsKeyPress(MyKeys.Control))
				{
					if (MyInput.Static.IsKeyPress(MyKeys.Shift))
					{
						InitialShift += 1000;
					}
					else
					{
						InitialShift += 10;
					}
				}
				else if (MyInput.Static.IsKeyPress(MyKeys.Shift))
				{
					InitialShift += 100;
				}
				else
				{
					InitialShift++;
				}
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Home))
			{
				InitialShift = 0;
			}
			if (!MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyAltKeyPressed())
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
				{
					EnlargeTriggerOnClick(null);
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
				{
					ShrinkTriggerOnClick(null);
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Delete))
				{
					DeleteEntityOnClicked(null);
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.N))
				{
					SpawnEntityClicked(null);
				}
			}
		}

		public override bool CloseScreen()
		{
			MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
			if (MySession.Static.ControlledEntity != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.ControlledEntity.Entity);
			}
			MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
			MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = false;
			m_transformSys.Active = false;
			MyGuiScreenGamePlay.DisableInput = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
			return base.CloseScreen();
		}

		public override bool Update(bool hasFocus)
		{
			if (m_currentScreen == ScriptingToolsScreen.Cutscenes)
			{
				UpdateCutscenes();
				return base.Update(hasFocus);
			}
			if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition != null || MyInput.Static.IsRightMousePressed())
			{
				base.DrawMouseCursor = false;
			}
			else
			{
				base.DrawMouseCursor = true;
			}
			m_triggerManipulator.CurrentPosition = MyAPIGateway.Session.Camera.Position;
			UpdateTriggerList();
			for (int i = 0; i < m_scriptManager.FailedLevelScriptExceptionTexts.Length; i++)
			{
				string text = m_scriptManager.FailedLevelScriptExceptionTexts[i];
				if (text != null && (bool)m_levelScriptListBox.Items[i].UserData)
				{
					m_levelScriptListBox.Items[i].Text.Append(" - failed");
					m_levelScriptListBox.Items[i].FontOverride = "Red";
					m_levelScriptListBox.Items[i].ToolTip.AddToolTip(text, 0.7f, "Red");
				}
			}
			foreach (MyVSStateMachine stateMachine in m_scriptManager.SMManager.RunningMachines)
			{
				int num = m_smListBox.Items.FindIndex((MyGuiControlListbox.Item item) => (MyVSStateMachine)item.UserData == stateMachine);
				if (num == -1)
				{
					m_smListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(stateMachine.Name), userData: stateMachine, toolTip: MyTexts.Get(MyCommonTexts.Scripting_Tooltip_Cursors).ToString()));
					num = m_smListBox.Items.Count - 1;
				}
				MyGuiControlListbox.Item item2 = m_smListBox.Items[num];
				for (int num2 = item2.ToolTip.ToolTips.Count - 1; num2 >= 0; num2--)
				{
					MyColoredText myColoredText = item2.ToolTip.ToolTips[num2];
					bool flag = false;
					foreach (MyStateMachineCursor activeCursor in stateMachine.ActiveCursors)
					{
						if (myColoredText.Text.CompareTo(activeCursor.Node.Name) == 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag && num2 != 0)
					{
						item2.ToolTip.ToolTips.RemoveAtFast(num2);
					}
				}
				foreach (MyStateMachineCursor activeCursor2 in stateMachine.ActiveCursors)
				{
					bool flag2 = false;
					for (int num3 = item2.ToolTip.ToolTips.Count - 1; num3 >= 0; num3--)
					{
						if (item2.ToolTip.ToolTips[num3].Text.CompareTo(activeCursor2.Node.Name) == 0)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						item2.ToolTip.AddToolTip(activeCursor2.Node.Name);
					}
				}
			}
			if (true)
			{
				IMyCamera camera = ((IMySession)MySession.Static).Camera;
				Vector2 vector = new Vector2(camera.ViewportSize.X * 0.01f, camera.ViewportSize.Y * 0.2f);
				Vector2 vector2 = new Vector2(0f, camera.ViewportSize.Y * 0.015f);
				new Vector2(camera.ViewportSize.X * 0.05f, 0f);
				float num4 = 0.65f * Math.Min(camera.ViewportSize.X / 1920f, camera.ViewportSize.Y / 1200f);
				int num5 = InitialShift;
				foreach (IMyLevelScript levelScript in m_scriptManager.LevelScripts)
				{
					FieldInfo[] fields = levelScript.GetType().GetFields();
					MyRenderProxy.DebugDrawText2D(vector + num5 * vector2, $"Script : {levelScript.GetType().Name}", Color.Orange, num4);
					num5++;
					FieldInfo[] array = fields;
					foreach (FieldInfo fieldInfo in array)
					{
						MyRenderProxy.DebugDrawText2D(vector + num5 * vector2, $"   {fieldInfo.Name} :     {fieldInfo.GetValue(levelScript)}", Color.Yellow, num4);
						num5++;
					}
				}
				num5++;
				foreach (MyVSStateMachine runningMachine in m_scriptManager.SMManager.RunningMachines)
				{
					foreach (MyStateMachineNode value in runningMachine.AllNodes.Values)
					{
						MyVSStateMachineNode myVSStateMachineNode = value as MyVSStateMachineNode;
						if (myVSStateMachineNode != null && myVSStateMachineNode.ScriptInstance != null)
						{
							FieldInfo[] fields2 = myVSStateMachineNode.ScriptInstance.GetType().GetFields();
							MyRenderProxy.DebugDrawText2D(vector + num5 * vector2, $"Script : {myVSStateMachineNode.Name}", Color.Orange, num4);
							num5++;
							FieldInfo[] array = fields2;
							foreach (FieldInfo fieldInfo2 in array)
							{
								MyRenderProxy.DebugDrawText2D(vector + num5 * vector2, $"   {fieldInfo2.Name} :     {fieldInfo2.GetValue(myVSStateMachineNode.ScriptInstance)}", Color.Yellow, num4);
								num5++;
							}
						}
					}
				}
				num5++;
				MyRenderProxy.DebugDrawText2D(vector + num5 * vector2, $"Stored variables:", Color.Orange, num4);
				num5++;
				num5 = DrawDictionary(m_scriptStorage.GetBools(), "Bools:", vector, vector2, num4, num5);
				num5 = DrawDictionary(m_scriptStorage.GetInts(), "Ints:", vector, vector2, num4, num5);
				num5 = DrawDictionary(m_scriptStorage.GetLongs(), "Longs:", vector, vector2, num4, num5);
				num5 = DrawDictionary(m_scriptStorage.GetStrings(), "Strings:", vector, vector2, num4, num5);
				num5 = DrawDictionary(m_scriptStorage.GetFloats(), "Floats:", vector, vector2, num4, num5);
				num5 = DrawDictionary(m_scriptStorage.GetVector3D(), "Vectors:", vector, vector2, num4, num5);
			}
			return base.Update(hasFocus);
		}

		private int DrawDictionary<T>(SerializableDictionary<string, T> dict, string title, Vector2 start, Vector2 offset, float fontScale, int startIndex)
		{
			if (dict.Dictionary.Count != 0)
			{
				MyRenderProxy.DebugDrawText2D(start + startIndex * offset, $"{title}", Color.Orange, fontScale);
				startIndex++;
				{
					foreach (KeyValuePair<string, T> item in dict.Dictionary)
					{
						MyRenderProxy.DebugDrawText2D(start + startIndex * offset, $"{item.Key.ToString()} :    {item.Value.ToString()}", Color.Yellow, fontScale);
						startIndex++;
					}
					return startIndex;
				}
			}
			return startIndex;
		}

		private void UpdateTriggerList()
		{
			ObservableCollection<MyGuiControlListbox.Item> items = m_triggersListBox.Items;
			List<MyTriggerComponent> allTriggers = MySessionComponentTriggerSystem.Static.GetAllTriggers();
			for (int i = 0; i < items.Count; i++)
			{
				MyAreaTriggerComponent item2 = (MyAreaTriggerComponent)items[i].UserData;
				if (!allTriggers.Contains(item2))
				{
					items.RemoveAtFast(i);
				}
			}
			foreach (MyTriggerComponent trigger in allTriggers)
			{
				if (m_triggersListBox.Items.FindIndex((MyGuiControlListbox.Item item) => (MyTriggerComponent)item.UserData == trigger) < 0)
				{
					MyGuiControlListbox.Item item3 = CreateTriggerListItem(trigger);
					if (item3 != null)
					{
						m_triggersListBox.Add(item3);
					}
				}
			}
		}

		private MyGuiControlListbox.Item CreateTriggerListItem(MyTriggerComponent trigger)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = trigger as MyAreaTriggerComponent;
			if (myAreaTriggerComponent == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder("Trigger: ");
			stringBuilder.Append(myAreaTriggerComponent.Name).Append(" Entity: ");
			stringBuilder.Append(string.IsNullOrEmpty(myAreaTriggerComponent.Entity.Name) ? myAreaTriggerComponent.Entity.DisplayName : myAreaTriggerComponent.Entity.Name);
			return new MyGuiControlListbox.Item(stringBuilder, myAreaTriggerComponent.Name, null, myAreaTriggerComponent);
		}

		private void InitializeWaypointList()
		{
			m_waypoints.Clear();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (IsWaypoint(entity))
				{
					m_waypoints.Add(entity);
				}
			}
		}

		private bool IsWaypoint(MyEntity ent)
		{
			if (ent.Name == null)
			{
				return false;
			}
			if (ent.Name.Length < ENTITY_NAME_PREFIX.Length || !ENTITY_NAME_PREFIX.Equals(ent.Name.Substring(0, ENTITY_NAME_PREFIX.Length)))
			{
				return false;
			}
			return true;
		}

		private void UpdateWaypointList()
		{
			if (m_waypointsListBox == null)
			{
				return;
			}
			ObservableCollection<MyGuiControlListbox.Item> items = m_waypointsListBox.Items;
			for (int i = 0; i < items.Count; i++)
			{
				MyEntity item2 = (MyEntity)items[i].UserData;
				if (!m_waypoints.Contains(item2))
				{
					items.RemoveAtFast(i);
				}
			}
			foreach (MyEntity wp in m_waypoints)
			{
				if (m_waypointsListBox.Items.FindIndex((MyGuiControlListbox.Item item) => (MyEntity)item.UserData == wp) < 0)
				{
					MyEntity myEntity = wp;
					StringBuilder stringBuilder = new StringBuilder("Waypoint: ");
					stringBuilder.Append(myEntity.Name);
					m_waypointsListBox.Add(new MyGuiControlListbox.Item(stringBuilder, myEntity.Name, null, myEntity));
				}
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			float y = (SCREEN_SIZE.Y - 1f) / 2f;
			Vector2 value = new Vector2(0.02f, 0f);
			string text = null;
			text = ((m_currentScreen != 0) ? MyTexts.Get(MySpaceTexts.ScriptingToolsCutscenes).ToString() : MyTexts.Get(MySpaceTexts.ScriptingToolsTransformations).ToString());
			MyGuiControlLabel myGuiControlLabel = AddCaption(text, Color.White.ToVector4(), value + new Vector2(0f - HIDDEN_PART_RIGHT, y));
			m_currentPosition.Y = myGuiControlLabel.PositionY + myGuiControlLabel.Size.Y + ITEM_VERTICAL_PADDING;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton(MyTexts.Get(MySpaceTexts.TransformationToolsButton).ToString(), SwitchPageToTransformation, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TransformTools)),
				CreateButton(MyTexts.Get(MySpaceTexts.CutsceneToolsButton).ToString(), SwitchPageToCutscenes, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_CutsceneTools))
			});
			bool flag = m_currentScreen == ScriptingToolsScreen.Transformation;
			m_transformSys.Active = flag;
			m_canShareInput = flag;
			MyGuiScreenGamePlay.DisableInput = !flag;
			switch (m_currentScreen)
			{
			case ScriptingToolsScreen.Transformation:
				RecreateControlsTransformation();
				break;
			case ScriptingToolsScreen.Cutscenes:
				RecreateControlsCutscenes();
				break;
			}
		}

		private void SelectOperation(MyEntityTransformationSystem.OperationMode mode)
		{
			m_transformSys.ChangeOperationMode(mode);
		}

		private void SelectCoordsWorld(bool world)
		{
			m_transformSys.ChangeCoordSystem(world);
		}

		private void DeselectEntityOnClicked(MyGuiControlButton myGuiControlButton)
		{
			m_transformSys.SetControlledEntity(null);
		}

		private void RecreateControlsTransformation()
		{
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.DisableTransformation)),
				CreateCheckbox(DisableTransformationOnCheckedChanged, m_transformSys.DisableTransformation, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DisableTransform))
			});
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Translation), delegate
				{
					SelectOperation(MyEntityTransformationSystem.OperationMode.Translation);
				}, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Translation)),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Rotation), delegate
				{
					SelectOperation(MyEntityTransformationSystem.OperationMode.Rotation);
				}, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rotation))
			});
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Coords_World), delegate
				{
					SelectCoordsWorld(world: true);
				}, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_WorldCoords)),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Coords_Local), delegate
				{
					SelectCoordsWorld(world: false);
				}, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_LocalCoords))
			});
			m_selectedEntityNameBox = CreateTextbox("");
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedEntity) + ": "),
				m_selectedEntityNameBox,
				CreateButton(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_ButtonRename), RenameSelectedEntityOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rename1))
			});
			m_selectedFunctionalBlockNameBox = CreateTextbox("");
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedBlock) + ": "),
				m_selectedFunctionalBlockNameBox,
				CreateButton(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_ButtonRename), RenameFunctionalBlockOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rename2))
			});
			PositionControls(new MyGuiControlBase[4]
			{
				CreateButton(MyTexts.GetString(MySpaceTexts.SpawnEntity), SpawnEntityClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SpawnEnt)),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_DeselectEntity), DeselectEntityOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeselectEnt)),
				CreateButton(MyTexts.GetString(MySpaceTexts.DeleteEntity), DeleteEntityOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeleteEnt)),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_SetPosition), SetPositionOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SetPosition))
			});
			m_waypointsListBox = CreateListBox();
			m_waypointsListBox.Size = new Vector2(0f, 0.148f);
			m_waypointsListBox.ItemClicked += WaypointsListBoxOnItemDoubleClicked;
			PositionControl(m_waypointsListBox);
			PositionControl(CreateLabel(MyTexts.GetString(MySpaceTexts.Triggers)));
			PositionControl(CreateButton(MyTexts.GetString(MySpaceTexts.AttachToSelectedEntity), AttachTriggerOnClick));
			m_enlargeTriggerButton = CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Grow), EnlargeTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeGrow));
			m_shrinkTriggerButton = CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Shrink), ShrinkTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeShrink));
			m_setTriggerSizeButton = CreateButton(MyTexts.GetString(MyCommonTexts.Size), SetSizeOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeSet));
			PositionControls(new MyGuiControlBase[3]
			{
				m_enlargeTriggerButton,
				m_setTriggerSizeButton,
				m_shrinkTriggerButton
			});
			PositionControls(new MyGuiControlBase[3]
			{
				CreateButton(MyTexts.GetString(MyCommonTexts.Snap), SnapTriggerToCameraOrEntityOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerSnap)),
				CreateButton(MyTexts.GetString(MyCommonTexts.Select), SelectTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerSelect)),
				CreateButton(MyTexts.GetString(MyCommonTexts.Delete), DeleteTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerDelete))
			});
			m_selectedTriggerNameBox = CreateTextbox(MyTexts.GetString(MySpaceTexts.TriggerNotSelected));
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedTrigger) + ":"),
				m_selectedTriggerNameBox
			});
			m_triggersListBox = CreateListBox();
			m_triggersListBox.Size = new Vector2(0f, 0.14f);
			m_triggersListBox.ItemClicked += TriggersListBoxOnItemDoubleClicked;
			PositionControl(m_triggersListBox);
			PositionControl(CreateLabel(MyTexts.Get(MySpaceTexts.RunningLevelScripts).ToString()));
			m_levelScriptListBox = CreateListBox();
			m_levelScriptListBox.Size = new Vector2(0f, 0.07f);
			PositionControl(m_levelScriptListBox);
			string[] runningLevelScriptNames = m_scriptManager.RunningLevelScriptNames;
			foreach (string value in runningLevelScriptNames)
			{
				m_levelScriptListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(value), null, null, false));
			}
			PositionControl(CreateLabel(MyTexts.Get(MySpaceTexts.RunningStateMachines).ToString()));
			m_smListBox = CreateListBox();
			m_smListBox.Size = new Vector2(0f, 0.07f);
			PositionControl(m_smListBox);
			m_smListBox.ItemSize = new Vector2(SCREEN_SIZE.X, ITEM_SIZE.Y);
		}

		private void SwitchPageToTransformation(MyGuiControlButton myGuiControlButton)
		{
			if (m_currentScreen != 0)
			{
				if (m_cutsceneSaveButton.Enabled)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MySpaceTexts.UnsavedChanges), messageText: MyTexts.Get(MySpaceTexts.UnsavedChangesQuestion), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							SaveCutsceneClicked(m_cutsceneSaveButton);
						}
						if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
						{
							SwitchPageToTransformationInternal();
						}
					}));
				}
				else
				{
					SwitchPageToTransformationInternal();
				}
			}
		}

		private void SwitchPageToTransformationInternal()
		{
			m_currentScreen = ScriptingToolsScreen.Transformation;
			MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse);
			RecreateControls(constructor: false);
			UpdateWaypointList();
		}

		private void SwitchPageToCutscenes(MyGuiControlButton myGuiControlButton)
		{
			if (m_currentScreen != ScriptingToolsScreen.Cutscenes)
			{
				m_currentScreen = ScriptingToolsScreen.Cutscenes;
				RecreateControls(constructor: false);
			}
		}

		private void TransformSysOnRayCasted(LineD ray)
		{
			if (m_transformSys.ControlledEntity == null || m_disablePicking || m_currentScreen == ScriptingToolsScreen.Cutscenes)
			{
				return;
			}
			MyHighlightSystem.MyHighlightData data;
			if (m_selectedFunctionalBlock != null)
			{
				MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
				if (component != null)
				{
					data = new MyHighlightSystem.MyHighlightData
					{
						EntityId = m_selectedFunctionalBlock.EntityId,
						PlayerId = -1L,
						Thickness = -1
					};
					component.RequestHighlightChange(data);
				}
				m_selectedFunctionalBlock = null;
			}
			MyCubeGrid myCubeGrid = m_transformSys.ControlledEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				Vector3I? vector3I = myCubeGrid.RayCastBlocks(ray.From, ray.To);
				if (vector3I.HasValue)
				{
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(vector3I.Value);
					if (cubeBlock.FatBlock != null)
					{
						m_selectedFunctionalBlock = cubeBlock.FatBlock;
					}
				}
			}
			m_helperStringBuilder.Clear();
			if (m_selectedFunctionalBlock != null)
			{
				m_helperStringBuilder.Append(string.IsNullOrEmpty(m_selectedFunctionalBlock.Name) ? m_selectedFunctionalBlock.DisplayNameText : m_selectedFunctionalBlock.Name);
				MyHighlightSystem component2 = MySession.Static.GetComponent<MyHighlightSystem>();
				if (component2 != null)
				{
					data = new MyHighlightSystem.MyHighlightData
					{
						EntityId = m_selectedFunctionalBlock.EntityId,
						IgnoreUseObjectData = true,
						OutlineColor = Color.Blue,
						PulseTimeInFrames = 120uL,
						Thickness = 3,
						PlayerId = -1L
					};
					component2.RequestHighlightChange(data);
				}
			}
			if (m_selectedFunctionalBlockNameBox != null)
			{
				m_selectedFunctionalBlockNameBox.SetText(m_helperStringBuilder);
			}
		}

		private void RenameFunctionalBlockOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_selectedFunctionalBlock != null)
			{
				m_disablePicking = true;
				m_transformSys.DisablePicking = true;
				ValueGetScreenWithCaption valueGetScreenWithCaption = new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + m_selectedFunctionalBlock.DisplayNameText, "", delegate(string text)
				{
					if (MyEntities.TryGetEntityByName(text, out MyEntity _))
					{
						return false;
					}
					m_selectedFunctionalBlock.Name = text;
					MyEntities.SetEntityName(m_selectedFunctionalBlock);
					m_helperStringBuilder.Clear().Append(text);
					m_selectedFunctionalBlockNameBox.SetText(m_helperStringBuilder);
					return true;
				});
				valueGetScreenWithCaption.Closed += delegate
				{
					m_disablePicking = false;
					m_transformSys.DisablePicking = false;
				};
				MyGuiSandbox.AddScreen(valueGetScreenWithCaption);
			}
		}

		private void RenameSelectedEntityOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity != null)
			{
				m_disablePicking = true;
				m_transformSys.DisablePicking = true;
				MyEntity selectedEntity = m_transformSys.ControlledEntity;
				ValueGetScreenWithCaption valueGetScreenWithCaption = new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + m_transformSys.ControlledEntity.DisplayNameText, "", delegate(string text)
				{
					if (MyEntities.TryGetEntityByName(text, out MyEntity _))
					{
						return false;
					}
					selectedEntity.Name = text;
					MyEntities.SetEntityName(selectedEntity);
					m_helperStringBuilder.Clear().Append(text);
					m_selectedEntityNameBox.SetText(m_helperStringBuilder);
					InitializeWaypointList();
					UpdateWaypointList();
					return true;
				});
				valueGetScreenWithCaption.Closed += delegate
				{
					m_disablePicking = false;
					m_transformSys.DisablePicking = false;
				};
				MyGuiSandbox.AddScreen(valueGetScreenWithCaption);
			}
		}

		private void DeleteEntityOnClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity != null)
			{
				if (m_waypoints.Contains(m_transformSys.ControlledEntity))
				{
					m_waypoints.Remove(m_transformSys.ControlledEntity);
					UpdateWaypointList();
				}
				m_transformSys.ControlledEntity.Close();
				m_transformSys.SetControlledEntity(null);
			}
		}

		private void AttachTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity != null)
			{
				MyEntity selectedEntity = m_transformSys.ControlledEntity;
				MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntitySpawnOn).ToString() + ": " + m_transformSys.ControlledEntity.DisplayName, "", delegate(string text)
				{
					MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(text);
					m_triggerManipulator.SelectedTrigger = myAreaTriggerComponent;
					if (!selectedEntity.Components.Contains(typeof(MyTriggerAggregate)))
					{
						selectedEntity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
					}
					selectedEntity.Components.Get<MyTriggerAggregate>().AddComponent(m_triggerManipulator.SelectedTrigger);
					myAreaTriggerComponent.Center = MyAPIGateway.Session.Camera.Position;
					myAreaTriggerComponent.Radius = 2.0;
					myAreaTriggerComponent.CustomDebugColor = Color.Yellow;
					DeselectEntity();
					UpdateTriggerList();
					m_triggersListBox.SelectedItems.Clear();
					MyGuiControlListbox.Item item = CreateTriggerListItem(myAreaTriggerComponent);
					m_triggersListBox.Add(item);
					m_triggersListBox.SelectedItem = item;
					return true;
				}));
			}
		}

		private void DeselectEntity()
		{
			m_transformSys.SetControlledEntity(null);
			m_waypointsListBox.SelectedItems.Clear();
		}

		private void DeselectTrigger()
		{
			m_triggerManipulator.SelectedTrigger = null;
			if (m_selectedTriggerNameBox != null)
			{
				m_selectedTriggerNameBox.SetText(new StringBuilder());
			}
			if (m_triggersListBox != null)
			{
				m_triggersListBox.SelectedItems.Clear();
			}
		}

		private void DeleteTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				if (m_triggerManipulator.SelectedTrigger.Entity != null)
				{
					m_triggerManipulator.SelectedTrigger.Entity.Components.Remove(typeof(MyTriggerAggregate), m_triggerManipulator.SelectedTrigger);
				}
				m_triggerManipulator.SelectedTrigger = null;
				m_helperStringBuilder.Clear();
				m_selectedEntityNameBox.SetText(m_helperStringBuilder);
			}
		}

		private void SnapTriggerToCameraOrEntityOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
				if (m_transformSys.ControlledEntity != null)
				{
					myAreaTriggerComponent.Center = m_transformSys.ControlledEntity.PositionComp.GetPosition();
				}
				else
				{
					myAreaTriggerComponent.Center = MyAPIGateway.Session.Camera.Position;
				}
			}
		}

		private void TransformSysOnControlledEntityChanged(MyEntity oldEntity, MyEntity newEntity)
		{
			if (m_currentScreen == ScriptingToolsScreen.Cutscenes || m_disablePicking)
			{
				return;
			}
			m_helperStringBuilder.Clear();
			if (newEntity != null)
			{
				m_helperStringBuilder.Clear().Append(string.IsNullOrEmpty(newEntity.Name) ? newEntity.DisplayName : newEntity.Name);
				DeselectTrigger();
				if (!m_waypoints.Contains(newEntity))
				{
					m_waypointsListBox.SelectedItems.Clear();
				}
			}
			if (m_selectedEntityNameBox != null)
			{
				m_selectedEntityNameBox.SetText(m_helperStringBuilder);
			}
			TransformSysOnRayCasted(m_transformSys.LastRay);
		}

		private void TriggersListBoxOnItemDoubleClicked(MyGuiControlListbox listBox)
		{
			if (m_triggersListBox.SelectedItems.Count != 0)
			{
				MyAreaTriggerComponent selectedTrigger = (MyAreaTriggerComponent)m_triggersListBox.SelectedItems[0].UserData;
				m_triggerManipulator.SelectedTrigger = selectedTrigger;
				if (m_triggerManipulator.SelectedTrigger != null)
				{
					MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
					m_helperStringBuilder.Clear();
					m_helperStringBuilder.Append(myAreaTriggerComponent.Name);
					m_selectedTriggerNameBox.SetText(m_helperStringBuilder);
				}
				DeselectEntity();
			}
		}

		private void WaypointsListBoxOnItemDoubleClicked(MyGuiControlListbox listBox)
		{
			if (m_waypointsListBox.SelectedItems.Count != 0)
			{
				MyEntity controlledEntity = (MyEntity)m_waypointsListBox.SelectedItems[0].UserData;
				m_transformSys.SetControlledEntity(controlledEntity);
				DeselectTrigger();
			}
		}

		private void SetSizeOnClick(MyGuiControlButton button)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				MyAreaTriggerComponent areaTrigger = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
				MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.SetTriggerSizeDialog).ToString(), areaTrigger.Radius.ToString(CultureInfo.InvariantCulture), delegate(string text)
				{
					if (!float.TryParse(text, out float result))
					{
						return false;
					}
					areaTrigger.Radius = result;
					return true;
				}));
			}
		}

		private void SetPositionOnClicked(MyGuiControlButton button)
		{
			if (m_transformSys.ControlledEntity != null)
			{
				MyEntity entity = m_transformSys.ControlledEntity;
				Vector3D position = entity.PositionComp.GetPosition();
				MyGuiSandbox.AddScreen(new Vector3GetScreenWithCaption(MyTexts.GetString(MySpaceTexts.SetEntityPositionDialog), position.X.ToString(), position.Y.ToString(), position.Z.ToString(), delegate(string text1, string text2, string text3)
				{
					if (!double.TryParse(text1, out double result) || !double.TryParse(text2, out double result2) || !double.TryParse(text3, out double result3))
					{
						return false;
					}
					MatrixD worldMatrix = entity.WorldMatrix;
					worldMatrix.Translation = new Vector3D(result, result2, result3);
					entity.WorldMatrix = worldMatrix;
					return true;
				}));
			}
		}

		private void ShrinkTriggerOnClick(MyGuiControlButton button)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
				myAreaTriggerComponent.Radius -= 0.20000000298023224;
				if (myAreaTriggerComponent.Radius < 0.20000000298023224)
				{
					myAreaTriggerComponent.Radius = 0.20000000298023224;
				}
			}
		}

		private void EnlargeTriggerOnClick(MyGuiControlButton button)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				((MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger).Radius += 0.20000000298023224;
			}
		}

		private void SelectTriggerOnClick(MyGuiControlButton button)
		{
			m_triggerManipulator.SelectClosest(MyAPIGateway.Session.Camera.Position);
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
				m_helperStringBuilder.Clear();
				m_helperStringBuilder.Append(myAreaTriggerComponent.Name);
				m_selectedTriggerNameBox.SetText(m_helperStringBuilder);
			}
		}

		private void SpawnEntityClicked(MyGuiControlButton myGuiControlButton)
		{
			string name;
			MyEntity entity;
			do
			{
				name = ENTITY_NAME_PREFIX + m_entityCounter++;
			}
			while (MyEntities.TryGetEntityByName(name, out entity));
			MyEntity myEntity = new MyEntity
			{
				WorldMatrix = MyAPIGateway.Session.Camera.WorldMatrix,
				EntityId = MyEntityIdentifier.AllocateId(),
				DisplayName = "Entity",
				Name = name
			};
			myEntity.PositionComp.SetPosition(MyAPIGateway.Session.Camera.Position + MyAPIGateway.Session.Camera.WorldMatrix.Forward * 2.0);
			myEntity.Components.Remove<MyPhysicsComponentBase>();
			MyEntities.Add(myEntity);
			MyEntities.SetEntityName(myEntity);
			m_transformSys.SetControlledEntity(myEntity);
			m_waypoints.Add(myEntity);
			UpdateWaypointList();
		}

		private void DisableTransformationOnCheckedChanged(MyGuiControlCheckbox checkbox)
		{
			m_transformSys.DisableTransformation = checkbox.IsChecked;
		}

		private MyGuiControlCheckbox CreateCheckbox(Action<MyGuiControlCheckbox> onCheckedChanged, bool isChecked, string tooltip = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, isChecked, MyGuiControlCheckboxStyleEnum.Debug, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (!string.IsNullOrEmpty(tooltip))
			{
				myGuiControlCheckbox.SetTooltip(tooltip);
			}
			myGuiControlCheckbox.Size = ITEM_SIZE;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onCheckedChanged);
			Controls.Add(myGuiControlCheckbox);
			return myGuiControlCheckbox;
		}

		private MyGuiControlTextbox CreateTextbox(string text, Action<MyGuiControlTextbox> textChanged = null)
		{
			MyGuiControlTextbox myGuiControlTextbox = new MyGuiControlTextbox(null, text, 512, null, 0.8f, MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum.Debug);
			myGuiControlTextbox.Enabled = false;
			myGuiControlTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlTextbox.Size = ITEM_SIZE;
			myGuiControlTextbox.TextChanged += textChanged;
			Controls.Add(myGuiControlTextbox);
			return myGuiControlTextbox;
		}

		private MyGuiControlLabel CreateLabel(string text)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, ITEM_SIZE, text, null, 0.8f, "Debug", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		private MyGuiControlListbox CreateListBox()
		{
			MyGuiControlListbox myGuiControlListbox = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.Blueprints)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = new Vector2(1f, 0.15f)
			};
			myGuiControlListbox.MultiSelect = false;
			myGuiControlListbox.Enabled = true;
			myGuiControlListbox.ItemSize = new Vector2(SCREEN_SIZE.X, ITEM_SIZE.Y);
			myGuiControlListbox.TextScale = 0.6f;
			myGuiControlListbox.VisibleRowsCount = 7;
			Controls.Add(myGuiControlListbox);
			return myGuiControlListbox;
		}

		private MyGuiControlCombobox CreateComboBox()
		{
			MyGuiControlCombobox myGuiControlCombobox = new MyGuiControlCombobox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = BUTTON_SIZE
			};
			myGuiControlCombobox.Enabled = true;
			Controls.Add(myGuiControlCombobox);
			return myGuiControlCombobox;
		}

		private MyGuiControlButton CreateButton(string text, Action<MyGuiControlButton> onClick, string tooltip = null)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(m_buttonXOffset, m_currentPosition.Y), MyGuiControlButtonStyleEnum.Rectangular, null, Color.Yellow.ToVector4(), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder(text), 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * m_scale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			if (!string.IsNullOrEmpty(tooltip))
			{
				myGuiControlButton.SetTooltip(tooltip);
			}
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlButton.Size = BUTTON_SIZE;
			Controls.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		private int GetListboxSelectedIndex(MyGuiControlListbox listbox)
		{
			if (listbox.SelectedItems.Count == 0)
			{
				return -1;
			}
			for (int i = 0; i < listbox.Items.Count; i++)
			{
				if (listbox.Items[i] == listbox.SelectedItems[0])
				{
					return i;
				}
			}
			return -1;
		}

		private void SelectListboxItemAtIndex(MyGuiControlListbox listbox, int index)
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < m_cutsceneCurrent.SequenceNodes.Count; i++)
			{
				list.Add(i == index);
			}
			m_cutsceneNodes.ChangeSelection(list);
		}

		private void PositionControl(MyGuiControlBase control)
		{
			float x = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - ITEM_HORIZONTAL_PADDING * 2f;
			Vector2 size = control.Size;
			control.Position = new Vector2(m_currentPosition.X - SCREEN_SIZE.X / 2f + ITEM_HORIZONTAL_PADDING, m_currentPosition.Y + ITEM_VERTICAL_PADDING);
			control.Size = new Vector2(x, size.Y);
			m_currentPosition.Y += control.Size.Y + ITEM_VERTICAL_PADDING;
		}

		private void PositionControls(MyGuiControlBase[] controls)
		{
			float num = (SCREEN_SIZE.X - HIDDEN_PART_RIGHT - ITEM_HORIZONTAL_PADDING * 2f) / (float)controls.Length - 0.001f * (float)controls.Length;
			float num2 = num + 0.001f * (float)controls.Length;
			float num3 = 0f;
			for (int i = 0; i < controls.Length; i++)
			{
				MyGuiControlBase myGuiControlBase = controls[i];
				if (!(myGuiControlBase is MyGuiControlCheckbox))
				{
					myGuiControlBase.Size = new Vector2(num, myGuiControlBase.Size.Y);
				}
				else
				{
					myGuiControlBase.Size = new Vector2(BUTTON_SIZE.Y);
				}
				myGuiControlBase.PositionX = m_currentPosition.X + num2 * (float)i - SCREEN_SIZE.X / 2f + ITEM_HORIZONTAL_PADDING;
				myGuiControlBase.PositionY = m_currentPosition.Y + ITEM_VERTICAL_PADDING;
				if (myGuiControlBase.Size.Y > num3)
				{
					num3 = myGuiControlBase.Size.Y;
				}
			}
			m_currentPosition.Y += num3 + ITEM_VERTICAL_PADDING;
		}

		private void RecreateControlsCutscenes()
		{
			m_cutscenes = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscenes();
			m_currentPosition.Y += ITEM_SIZE.Y;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_New).ToString(), CreateNewCutsceneClicked),
				CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_ClearAllCutscenes).ToString(), ClearAllCutscenesClicked)
			});
			m_cutsceneSelection = CreateComboBox();
			foreach (Cutscene value in m_cutscenes.Values)
			{
				m_cutsceneSelection.AddItem(value.Name.GetHashCode64(), value.Name);
			}
			m_cutsceneSelection.ItemSelected += m_cutsceneSelection_ItemSelected;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Selected).ToString()),
				m_cutsceneSelection
			});
			m_cutsceneDeleteButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Delete).ToString(), DeleteCurrentCutsceneClicked);
			m_cutscenePlayButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Play).ToString(), WatchCutsceneClicked);
			m_cutscenePlayButton.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Play_Extended).ToString());
			m_cutsceneSaveButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Save).ToString(), SaveCutsceneClicked);
			m_cutsceneRevertButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Revert).ToString(), RevertCutsceneClicked);
			PositionControls(new MyGuiControlBase[4]
			{
				m_cutscenePlayButton,
				m_cutsceneSaveButton,
				m_cutsceneRevertButton,
				m_cutsceneDeleteButton
			});
			m_currentPosition.Y += ITEM_SIZE.Y / 2f;
			m_cutscenePropertyNextCutscene = CreateComboBox();
			m_cutscenePropertyNextCutscene.ItemSelected += CutscenePropertyNextCutscene_ItemSelected;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_New).ToString()),
				m_cutscenePropertyNextCutscene
			});
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_PosRot).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookRot).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOV).ToString())
			});
			m_cutscenePropertyStartEntity = CreateTextbox("", CutscenePropertyStartEntity_TextChanged);
			m_cutscenePropertyStartEntity.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_PosRot_Extended).ToString());
			m_cutscenePropertyStartLookAt = CreateTextbox("", CutscenePropertyStartLookAt_TextChanged);
			m_cutscenePropertyStartLookAt.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookRot_Extended).ToString());
			m_cutscenePropertyStartingFOV = CreateTextbox("", CutscenePropertyStartingFOV_TextChanged);
			m_cutscenePropertyStartingFOV.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOV_Extended).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				m_cutscenePropertyStartEntity,
				m_cutscenePropertyStartLookAt,
				m_cutscenePropertyStartingFOV
			});
			m_cutscenePropertyCanBeSkipped = CreateCheckbox(CutscenePropertyCanBeSkippedChanged, isChecked: true);
			m_cutscenePropertyCanBeSkipped.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Skippable).ToString());
			m_cutscenePropertyFireEventsDuringSkip = CreateCheckbox(CutscenePropertyFireEventsDuringSkipChanged, isChecked: true);
			m_cutscenePropertyFireEventsDuringSkip.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_SkipWarning).ToString());
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_CanSkip).ToString()),
				m_cutscenePropertyCanBeSkipped,
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Events).ToString()),
				m_cutscenePropertyFireEventsDuringSkip
			});
			m_currentPosition.Y += ITEM_SIZE.Y;
			m_cutsceneNodeButtonAdd = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_AddNode).ToString(), CutsceneNodeButtonAddClicked);
			m_cutsceneNodeButtonDelete = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Delete).ToString(), CutsceneNodeButtonDeleteClicked);
			m_cutsceneNodeButtonDeleteAll = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_ClearAll).ToString(), CutsceneNodeButtonDeleteAllClicked);
			m_cutsceneNodeButtonMoveUp = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveUp).ToString(), CutsceneNodeButtonMoveUpClicked);
			m_cutsceneNodeButtonMoveDown = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveDown).ToString(), CutsceneNodeButtonMoveDownClicked);
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Nodes).ToString()),
				m_cutsceneNodeButtonAdd,
				m_cutsceneNodeButtonDeleteAll
			});
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_CurrentNode).ToString()),
				m_cutsceneNodeButtonMoveUp,
				m_cutsceneNodeButtonMoveDown,
				m_cutsceneNodeButtonDelete
			});
			m_cutsceneNodes = CreateListBox();
			m_cutsceneNodes.VisibleRowsCount = 5;
			m_cutsceneNodes.Size = new Vector2(0f, 0.12f);
			m_cutsceneNodes.ItemsSelected += m_cutsceneNodes_ItemsSelected;
			PositionControl(m_cutsceneNodes);
			m_cutsceneNodePropertyTime = CreateTextbox("", CutsceneNodePropertyTime_TextChanged);
			m_cutsceneNodePropertyTime.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Time_Extended).ToString());
			m_cutsceneNodePropertyEvent = CreateTextbox("", CutsceneNodePropertyEvent_TextChanged);
			m_cutsceneNodePropertyEvent.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Event_Extended).ToString());
			m_cutsceneNodePropertyEventDelay = CreateTextbox("", CutsceneNodePropertyEventDelay_TextChanged);
			m_cutsceneNodePropertyEventDelay.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_EventDelay_Extended).ToString());
			m_cutsceneNodePropertyFOVChange = CreateTextbox("", CutsceneNodePropertyFOV_TextChanged);
			m_cutsceneNodePropertyFOVChange.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOVChange_Extended).ToString());
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Time).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Event).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_EventDelay).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOVChange).ToString())
			});
			PositionControls(new MyGuiControlBase[4]
			{
				m_cutsceneNodePropertyTime,
				m_cutsceneNodePropertyEvent,
				m_cutsceneNodePropertyEventDelay,
				m_cutsceneNodePropertyFOVChange
			});
			m_currentPosition.Y += ITEM_SIZE.Y / 2f;
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Action).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_OverTime).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Instant).ToString())
			});
			m_cutsceneNodePropertyMoveTo = CreateTextbox("", CutsceneNodePropertyMoveTo_TextChanged);
			m_cutsceneNodePropertyMoveTo.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo_Extended1).ToString());
			m_cutsceneNodePropertyMoveToInstant = CreateTextbox("", CutsceneNodePropertyMoveToInstant_TextChanged);
			m_cutsceneNodePropertyMoveToInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo_Extended2).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo).ToString()),
				m_cutsceneNodePropertyMoveTo,
				m_cutsceneNodePropertyMoveToInstant
			});
			m_cutsceneNodePropertyRotateLike = CreateTextbox("", CutsceneNodePropertyRotateLike_TextChanged);
			m_cutsceneNodePropertyRotateLike.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike_Extended1).ToString());
			m_cutsceneNodePropertyRotateLikeInstant = CreateTextbox("", CutsceneNodePropertyRotateLikeInstant_TextChanged);
			m_cutsceneNodePropertyRotateLikeInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike_Extended2).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike).ToString()),
				m_cutsceneNodePropertyRotateLike,
				m_cutsceneNodePropertyRotateLikeInstant
			});
			m_cutsceneNodePropertyRotateTowards = CreateTextbox("", CutsceneNodePropertyLookAt_TextChanged);
			m_cutsceneNodePropertyRotateTowards.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt_Extended1).ToString());
			m_cutsceneNodePropertyRotateTowardsInstant = CreateTextbox("", CutsceneNodePropertyLookAtInstant_TextChanged);
			m_cutsceneNodePropertyRotateTowardsInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt_Extended2).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt).ToString()),
				m_cutsceneNodePropertyRotateTowards,
				m_cutsceneNodePropertyRotateTowardsInstant
			});
			m_currentPosition.Y += ITEM_SIZE.Y;
			m_cutsceneNodePropertyRotateTowardsLock = CreateTextbox("", CutsceneNodePropertyLockRotationTo_TextChanged);
			m_cutsceneNodePropertyRotateTowardsLock.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended1).ToString());
			m_cutsceneNodePropertyAttachAll = CreateTextbox("", CutsceneNodePropertyAttachTo_TextChanged);
			m_cutsceneNodePropertyAttachAll.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended2).ToString());
			m_cutsceneNodePropertyAttachPosition = CreateTextbox("", CutsceneNodePropertyAttachPositionTo_TextChanged);
			m_cutsceneNodePropertyAttachPosition.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended3).ToString());
			m_cutsceneNodePropertyAttachRotation = CreateTextbox("", CutsceneNodePropertyAttachRotationTo_TextChanged);
			m_cutsceneNodePropertyAttachRotation.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended4).ToString());
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackLook).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackPosRot).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackPos).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackRot).ToString())
			});
			PositionControls(new MyGuiControlBase[4]
			{
				m_cutsceneNodePropertyRotateTowardsLock,
				m_cutsceneNodePropertyAttachAll,
				m_cutsceneNodePropertyAttachPosition,
				m_cutsceneNodePropertyAttachRotation
			});
			m_currentPosition.Y += ITEM_SIZE.Y / 2f;
			m_cutsceneNodePropertyWaypoints = CreateTextbox("", CutsceneNodePropertyWaypoints_TextChanged);
			m_cutsceneNodePropertyWaypoints.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Waypoints_Extended).ToString());
			PositionControl(CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Waypoints).ToString()));
			PositionControl(m_cutsceneNodePropertyWaypoints);
			m_cutsceneCurrent = null;
			m_selectedCutsceneNodeIndex = -1;
			m_cutsceneSaveButton.Enabled = false;
			if (m_cutscenes.Count > 0)
			{
				m_cutsceneSelection.SelectItemByIndex(0);
			}
			else
			{
				UpdateCutsceneFields();
			}
		}

		private void UpdateCutscenes()
		{
			MyGuiScreenGamePlay.DisableInput = (base.State != MyGuiScreenState.CLOSING && base.State != MyGuiScreenState.CLOSED);
			if (m_cutscenePlaying && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
			{
				base.State = MyGuiScreenState.OPENED;
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse);
				m_cutscenePlaying = false;
			}
		}

		private void UpdateCutsceneFields()
		{
			string name = (m_cutsceneSelection.GetSelectedIndex() >= 0) ? m_cutsceneSelection.GetSelectedValue().ToString() : "";
			m_cutsceneCurrent = null;
			Cutscene cutsceneCopy = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutsceneCopy(name);
			bool flag = cutsceneCopy != null;
			m_cutsceneDeleteButton.Enabled = flag;
			m_cutscenePlayButton.Enabled = flag;
			m_cutsceneSaveButton.Enabled = false;
			m_cutsceneRevertButton.Enabled = false;
			m_cutscenePropertyNextCutscene.Enabled = flag;
			m_cutscenePropertyNextCutscene.ClearItems();
			m_cutscenePropertyNextCutscene.AddItem(0L, MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_None));
			m_cutscenePropertyNextCutscene.SelectItemByIndex(0);
			m_cutscenePropertyStartEntity.Enabled = flag;
			m_cutscenePropertyStartLookAt.Enabled = flag;
			m_cutscenePropertyStartingFOV.Enabled = flag;
			m_cutscenePropertyCanBeSkipped.Enabled = flag;
			m_cutscenePropertyFireEventsDuringSkip.Enabled = flag;
			m_cutsceneNodes.ClearItems();
			if (flag)
			{
				m_cutscenePropertyStartEntity.Text = cutsceneCopy.StartEntity;
				m_cutscenePropertyStartLookAt.Text = cutsceneCopy.StartLookAt;
				m_cutscenePropertyStartingFOV.Text = cutsceneCopy.StartingFOV.ToString();
				m_cutscenePropertyCanBeSkipped.IsChecked = cutsceneCopy.CanBeSkipped;
				m_cutscenePropertyFireEventsDuringSkip.IsChecked = cutsceneCopy.FireEventsDuringSkip;
				foreach (string key in m_cutscenes.Keys)
				{
					if (!key.Equals(cutsceneCopy.Name))
					{
						m_cutscenePropertyNextCutscene.AddItem(key.GetHashCode64(), key);
						if (key.Equals(cutsceneCopy.NextCutscene))
						{
							m_cutscenePropertyNextCutscene.SelectItemByKey(key.GetHashCode64());
						}
					}
				}
				if (cutsceneCopy.SequenceNodes != null)
				{
					for (int i = 0; i < cutsceneCopy.SequenceNodes.Count; i++)
					{
						m_cutsceneNodes.Add(new MyGuiControlListbox.Item(new StringBuilder(i + 1 + ": " + cutsceneCopy.SequenceNodes[i].GetNodeSummary()), cutsceneCopy.SequenceNodes[i].GetNodeDescription()));
					}
				}
			}
			m_cutsceneCurrent = cutsceneCopy;
			UpdateCutsceneNodeFields();
		}

		private void CutsceneChanged()
		{
			m_cutsceneSaveButton.Enabled = (m_cutsceneCurrent != null);
			m_cutsceneRevertButton.Enabled = (m_cutsceneCurrent != null);
			if (m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneNodes.ItemsSelected -= m_cutsceneNodes_ItemsSelected;
				m_cutsceneNodes.Items.RemoveAt(m_selectedCutsceneNodeIndex);
				m_cutsceneNodes.Items.Insert(m_selectedCutsceneNodeIndex, new MyGuiControlListbox.Item(new StringBuilder(m_selectedCutsceneNodeIndex + 1 + ": " + m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].GetNodeSummary()), m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].GetNodeDescription()));
				SelectListboxItemAtIndex(m_cutsceneNodes, m_selectedCutsceneNodeIndex);
				m_cutsceneNodes.ItemsSelected += m_cutsceneNodes_ItemsSelected;
			}
		}

		private void UpdateCutsceneNodeFields()
		{
			bool flag = m_cutsceneCurrent != null && m_cutsceneNodes.SelectedItems.Count > 0;
			m_cutsceneNodeButtonMoveUp.Enabled = flag;
			m_cutsceneNodeButtonMoveDown.Enabled = flag;
			m_cutsceneNodeButtonDelete.Enabled = flag;
			m_cutsceneNodePropertyTime.Enabled = flag;
			m_cutsceneNodePropertyMoveTo.Enabled = flag;
			m_cutsceneNodePropertyMoveToInstant.Enabled = flag;
			m_cutsceneNodePropertyRotateLike.Enabled = flag;
			m_cutsceneNodePropertyRotateLikeInstant.Enabled = flag;
			m_cutsceneNodePropertyRotateTowards.Enabled = flag;
			m_cutsceneNodePropertyRotateTowardsInstant.Enabled = flag;
			m_cutsceneNodePropertyEvent.Enabled = flag;
			m_cutsceneNodePropertyEventDelay.Enabled = flag;
			m_cutsceneNodePropertyFOVChange.Enabled = flag;
			m_cutsceneNodePropertyRotateTowardsLock.Enabled = flag;
			m_cutsceneNodePropertyAttachAll.Enabled = flag;
			m_cutsceneNodePropertyAttachPosition.Enabled = flag;
			m_cutsceneNodePropertyAttachRotation.Enabled = flag;
			m_cutsceneNodePropertyWaypoints.Enabled = flag;
			if (!flag)
			{
				return;
			}
			m_selectedCutsceneNodeIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			m_cutsceneNodeButtonMoveUp.Enabled = (m_selectedCutsceneNodeIndex > 0 && m_cutsceneNodes.Items.Count > 1);
			m_cutsceneNodeButtonMoveDown.Enabled = (m_selectedCutsceneNodeIndex < m_cutsceneNodes.Items.Count - 1);
			m_cutsceneNodePropertyTime.Text = Math.Max(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Time, 0f).ToString();
			m_cutsceneNodePropertyMoveTo.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].MoveTo != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].MoveTo : "");
			m_cutsceneNodePropertyMoveToInstant.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetPositionTo != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetPositionTo : "");
			m_cutsceneNodePropertyRotateLike.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateLike != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateLike : "");
			m_cutsceneNodePropertyRotateLikeInstant.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetRorationLike != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetRorationLike : "");
			m_cutsceneNodePropertyRotateTowards.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateTowards != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateTowards : "");
			m_cutsceneNodePropertyRotateTowardsInstant.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LookAt != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LookAt : "");
			m_cutsceneNodePropertyEvent.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Event != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Event : "");
			m_cutsceneNodePropertyEventDelay.Text = Math.Max(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].EventDelay, 0f).ToString();
			m_cutsceneNodePropertyFOVChange.Text = Math.Max(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].ChangeFOVTo, 0f).ToString();
			m_cutsceneNodePropertyRotateTowardsLock.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo : "X"));
			m_cutsceneNodePropertyAttachAll.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo : "X"));
			m_cutsceneNodePropertyAttachPosition.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo : "X"));
			m_cutsceneNodePropertyAttachRotation.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo : "X"));
			if (m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints[i].Name);
				}
				m_cutsceneNodePropertyWaypoints.Text = stringBuilder.ToString();
			}
			else
			{
				m_cutsceneNodePropertyWaypoints.Text = "";
			}
		}

		private void CloseScreenWithSave()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					SaveCutsceneClicked(m_cutsceneSaveButton);
				}
				if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
				{
					CloseScreen();
				}
			}));
		}

		private void m_cutsceneSelection_ItemSelected()
		{
			if (m_cutsceneSaveButton.Enabled)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						SaveCutsceneClicked(m_cutsceneSaveButton);
					}
					if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
					{
						UpdateCutsceneFields();
					}
				}));
			}
			else
			{
				UpdateCutsceneFields();
			}
		}

		private void WatchCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneSelection.GetSelectedValue() != null)
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
				MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(m_cutsceneCurrent, registerEvents: false);
				base.State = MyGuiScreenState.HIDDEN;
				m_cutscenePlaying = true;
			}
		}

		private void SaveCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutscenes[m_cutsceneCurrent.Name] = m_cutsceneCurrent;
				m_cutsceneSaveButton.Enabled = false;
				m_cutsceneRevertButton.Enabled = false;
			}
		}

		private void RevertCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Revert_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Revert_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					UpdateCutsceneFields();
				}
			}));
		}

		private void ClearAllCutscenesClicked(MyGuiControlButton myGuiControlButton)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAll_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAll_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					m_cutscenes.Clear();
					m_cutsceneSelection.ClearItems();
					UpdateCutsceneFields();
				}
			}));
		}

		private void DeleteCurrentCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneSelection.GetSelectedIndex() >= 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Delete_Caption), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.Cutscene_Delete_Text), m_cutsceneSelection.GetItemByIndex(m_cutsceneSelection.GetSelectedIndex()).Value), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						m_cutscenes.Remove(m_cutsceneSelection.GetItemByIndex(m_cutsceneSelection.GetSelectedIndex()).Value.ToString());
						m_cutsceneSelection.RemoveItemByIndex(m_cutsceneSelection.GetSelectedIndex());
						if (m_cutscenes.Count > 0)
						{
							m_cutsceneSelection.SelectItemByIndex(0);
						}
						else
						{
							UpdateCutsceneFields();
						}
					}
				}));
			}
		}

		private void CreateNewCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneSaveButton.Enabled)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						SaveCutsceneClicked(m_cutsceneSaveButton);
					}
					if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
					{
						NewCutscene();
					}
				}));
			}
			else
			{
				NewCutscene();
			}
		}

		private void NewCutscene()
		{
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MyCommonTexts.Cutscene_New_Caption).ToString(), "", delegate(string text)
			{
				if (m_cutscenes.ContainsKey(text))
				{
					return false;
				}
				Cutscene value = new Cutscene
				{
					Name = text
				};
				m_cutscenes.Add(text, value);
				long hashCode = text.GetHashCode64();
				m_cutsceneSelection.AddItem(hashCode, text);
				m_cutsceneSelection.SelectItemByKey(hashCode);
				return true;
			}));
		}

		private void CutscenePropertyCanBeSkippedChanged(MyGuiControlCheckbox checkbox)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.CanBeSkipped = checkbox.IsChecked;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyFireEventsDuringSkipChanged(MyGuiControlCheckbox checkbox)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.FireEventsDuringSkip = checkbox.IsChecked;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyStartEntity_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.StartEntity = obj.Text;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyStartLookAt_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.StartLookAt = obj.Text;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyStartingFOV_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null)
			{
				if (float.TryParse(obj.Text, out float result))
				{
					m_cutsceneCurrent.StartingFOV = result;
				}
				else
				{
					m_cutsceneCurrent.StartingFOV = 70f;
				}
				CutsceneChanged();
			}
		}

		private void CutscenePropertyNextCutscene_ItemSelected()
		{
			if (m_cutsceneCurrent != null)
			{
				if (m_cutscenePropertyNextCutscene.GetSelectedKey() == 0L)
				{
					m_cutsceneCurrent.NextCutscene = null;
				}
				else
				{
					m_cutsceneCurrent.NextCutscene = m_cutscenePropertyNextCutscene.GetSelectedValue().ToString();
				}
				CutsceneChanged();
			}
		}

		private void m_cutsceneNodes_ItemsSelected(MyGuiControlListbox obj)
		{
			bool enabled = m_cutsceneSaveButton.Enabled;
			m_selectedCutsceneNodeIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			UpdateCutsceneNodeFields();
			m_cutsceneSaveButton.Enabled = enabled;
			m_cutsceneRevertButton.Enabled = enabled;
		}

		private void CutsceneNodeButtonAddClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent != null)
			{
				if (m_cutsceneCurrent.SequenceNodes == null)
				{
					m_cutsceneCurrent.SequenceNodes = new List<CutsceneSequenceNode>();
				}
				CutsceneSequenceNode cutsceneSequenceNode = new CutsceneSequenceNode();
				m_cutsceneCurrent.SequenceNodes.Add(cutsceneSequenceNode);
				m_cutsceneNodes.Add(new MyGuiControlListbox.Item(new StringBuilder(m_cutsceneCurrent.SequenceNodes.Count + ": " + cutsceneSequenceNode.GetNodeSummary()), cutsceneSequenceNode.GetNodeDescription()));
				SelectListboxItemAtIndex(m_cutsceneNodes, m_cutsceneCurrent.SequenceNodes.Count - 1);
				CutsceneChanged();
			}
		}

		private void CutsceneNodeButtonDeleteAllClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAllNodes_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAllNodes_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						m_cutsceneCurrent.SequenceNodes.Clear();
						m_cutsceneCurrent.SequenceNodes = null;
						m_cutsceneNodes.ClearItems();
						UpdateCutsceneNodeFields();
						m_cutsceneNodes.ScrollToolbarToTop();
						CutsceneChanged();
					}
				}));
			}
		}

		private void CutsceneNodeButtonDeleteClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_DeleteNode_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteNode_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES && m_selectedCutsceneNodeIndex >= 0)
					{
						m_cutsceneCurrent.SequenceNodes.RemoveAt(m_selectedCutsceneNodeIndex);
						m_cutsceneNodes.Items.RemoveAt(m_selectedCutsceneNodeIndex);
						SelectListboxItemAtIndex(m_cutsceneNodes, m_selectedCutsceneNodeIndex);
						CutsceneChanged();
					}
				}));
			}
		}

		private void CutsceneNodeButtonMoveUpClicked(MyGuiControlButton myGuiControlButton)
		{
			int listboxSelectedIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			if (m_cutsceneCurrent != null && listboxSelectedIndex >= 0)
			{
				CutsceneSequenceNode item = m_cutsceneCurrent.SequenceNodes[listboxSelectedIndex];
				m_cutsceneCurrent.SequenceNodes.RemoveAt(listboxSelectedIndex);
				m_cutsceneCurrent.SequenceNodes.Insert(listboxSelectedIndex - 1, item);
				MyGuiControlListbox.Item item2 = m_cutsceneNodes.Items[listboxSelectedIndex];
				m_cutsceneNodes.Items.RemoveAt(listboxSelectedIndex);
				m_cutsceneNodes.Items.Insert(listboxSelectedIndex - 1, item2);
				SelectListboxItemAtIndex(m_cutsceneNodes, listboxSelectedIndex - 1);
				CutsceneChanged();
			}
		}

		private void CutsceneNodeButtonMoveDownClicked(MyGuiControlButton myGuiControlButton)
		{
			int listboxSelectedIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			if (m_cutsceneCurrent != null && listboxSelectedIndex >= 0)
			{
				CutsceneSequenceNode item = m_cutsceneCurrent.SequenceNodes[listboxSelectedIndex];
				m_cutsceneCurrent.SequenceNodes.RemoveAt(listboxSelectedIndex);
				m_cutsceneCurrent.SequenceNodes.Insert(listboxSelectedIndex + 1, item);
				MyGuiControlListbox.Item item2 = m_cutsceneNodes.Items[listboxSelectedIndex];
				m_cutsceneNodes.Items.RemoveAt(listboxSelectedIndex);
				m_cutsceneNodes.Items.Insert(listboxSelectedIndex + 1, item2);
				SelectListboxItemAtIndex(m_cutsceneNodes, listboxSelectedIndex + 1);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyMoveTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].MoveTo = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyMoveToInstant_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetPositionTo = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyRotateLike_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateLike = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyRotateLikeInstant_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetRorationLike = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyLookAt_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateTowards = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyLookAtInstant_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LookAt = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyEvent_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Event = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyTime_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				if (float.TryParse(obj.Text, out float result))
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Time = Math.Max(0f, result);
				}
				else
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Time = 0f;
				}
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyFOV_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				if (float.TryParse(obj.Text, out float result))
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].ChangeFOVTo = Math.Max(0f, result);
				}
				else
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].ChangeFOVTo = 0f;
				}
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyEventDelay_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				if (float.TryParse(obj.Text, out float result))
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].EventDelay = Math.Max(0f, result);
				}
				else
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].EventDelay = 0f;
				}
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyAttachTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyAttachPositionTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyAttachRotationTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyLockRotationTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyWaypoints_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent == null || m_selectedCutsceneNodeIndex < 0)
			{
				return;
			}
			bool flag = obj.Text.Length == 0;
			if (!flag)
			{
				string[] array = obj.Text.Split(new string[1]
				{
					";"
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length != 0)
				{
					if (m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints == null)
					{
						m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints = new List<CutsceneSequenceNodeWaypoint>();
					}
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Clear();
					string[] array2 = array;
					foreach (string name in array2)
					{
						CutsceneSequenceNodeWaypoint cutsceneSequenceNodeWaypoint = new CutsceneSequenceNodeWaypoint();
						cutsceneSequenceNodeWaypoint.Name = name;
						m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Add(cutsceneSequenceNodeWaypoint);
					}
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints != null)
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Clear();
				}
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints = null;
			}
			CutsceneChanged();
		}
	}
}
