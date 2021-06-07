using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Cube
{
	[MyUseObject("panel")]
	public class MyUseObjectPanelButton : MyUseObjectBase
	{
		private readonly MyButtonPanel m_buttonPanel;

		private readonly Matrix m_localMatrix;

		private int m_index;

		private MyGps m_buttonDesc;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => m_localMatrix * m_buttonPanel.WorldMatrix;

		public override MatrixD WorldMatrix => m_buttonPanel.WorldMatrix;

		public Vector3D MarkerPosition => ActivationMatrix.Translation;

		public override uint RenderObjectID => m_buttonPanel.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions => PrimaryAction | SecondaryAction;

		public override UseActionEnum PrimaryAction
		{
			get
			{
				if (m_buttonPanel.Toolbar.GetItemAtIndex(m_index) == null)
				{
					return UseActionEnum.None;
				}
				return UseActionEnum.Manipulate;
			}
		}

		public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectPanelButton(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			m_buttonPanel = (owner as MyButtonPanel);
			m_localMatrix = dummyData.Matrix;
			int result = 0;
			string[] array = dummyName.Split(new char[1]
			{
				'_'
			});
			int.TryParse(array[array.Length - 1], out result);
			m_index = result - 1;
			if (m_index >= m_buttonPanel.BlockDefinition.ButtonCount)
			{
				MyLog.Default.WriteLine($"{m_buttonPanel.BlockDefinition.Id.SubtypeName} Button index higher than defined count.");
				m_index = m_buttonPanel.BlockDefinition.ButtonCount - 1;
			}
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (m_buttonPanel.Components.TryGet(out MyContainerDropComponent component))
			{
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenClaimGameItem(component, myCharacter.GetPlayerIdentityId()));
				return;
			}
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
			{
				if (!m_buttonPanel.IsWorking)
				{
					break;
				}
				if (!m_buttonPanel.AnyoneCanUse && !m_buttonPanel.HasLocalPlayerAccess())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					break;
				}
				MyMultiplayer.RaiseEvent(m_buttonPanel, (MyButtonPanel x) => x.ActivateButton, m_index);
				long playerId = myCharacter?.GetPlayerIdentityId() ?? 0;
				if (MyVisualScriptLogicProvider.ButtonPressedTerminalName != null)
				{
					MyVisualScriptLogicProvider.ButtonPressedTerminalName(m_buttonPanel.CustomName.ToString(), m_index, playerId, m_buttonPanel.EntityId);
				}
				if (MyVisualScriptLogicProvider.ButtonPressedEntityName != null)
				{
					MyVisualScriptLogicProvider.ButtonPressedEntityName(m_buttonPanel.Name, m_index, playerId, m_buttonPanel.EntityId);
				}
				break;
			}
			case UseActionEnum.OpenTerminal:
				if (m_buttonPanel.HasLocalPlayerAccess())
				{
					MyToolbarComponent.CurrentToolbar = m_buttonPanel.Toolbar;
					MyGuiScreenBase myGuiScreenBase2 = MyGuiScreenToolbarConfigBase.Static;
					if (myGuiScreenBase2 == null)
					{
						myGuiScreenBase2 = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, m_buttonPanel, null);
					}
					MyToolbarComponent.AutoUpdate = false;
					myGuiScreenBase2.Closed += delegate
					{
						MyToolbarComponent.AutoUpdate = true;
					};
					MyGuiSandbox.AddScreen(myGuiScreenBase2);
				}
				break;
			}
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			m_buttonPanel.Toolbar.UpdateItem(m_index);
			MyToolbarItem itemAtIndex = m_buttonPanel.Toolbar.GetItemAtIndex(m_index);
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				if (m_buttonDesc == null)
				{
					m_buttonDesc = new MyGps();
					m_buttonDesc.Description = "";
					m_buttonDesc.CoordsFunc = (() => MarkerPosition);
					m_buttonDesc.ShowOnHud = true;
					m_buttonDesc.DiscardAt = null;
					m_buttonDesc.AlwaysVisible = true;
				}
				MyHud.ButtonPanelMarkers.RegisterMarker(m_buttonDesc);
				SetButtonName(m_buttonPanel.GetCustomButtonName(m_index));
				if (itemAtIndex != null)
				{
					result = default(MyActionDescription);
					result.Text = MyCommonTexts.NotificationHintPressToUse;
					result.FormatParams = new object[2]
					{
						string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]"),
						itemAtIndex.DisplayName
					};
					result.IsTextControlHint = true;
					result.JoystickText = MyCommonTexts.NotificationHintPressToUse;
					result.JoystickFormatParams = new object[2]
					{
						MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
						itemAtIndex.DisplayName
					};
					result.ShowForGamepad = true;
					return result;
				}
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.Blank;
				return result;
			case UseActionEnum.OpenTerminal:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenButtonPanel;
				result.FormatParams = new object[1]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), "]")
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenButtonPanel;
				result.JoystickFormatParams = new object[1]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL)
				};
				result.ShowForGamepad = true;
				return result;
			default:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenButtonPanel;
				result.FormatParams = new object[1]
				{
					MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL)
				};
				result.IsTextControlHint = true;
				result.ShowForGamepad = true;
				return result;
			}
		}

		public override bool HandleInput()
		{
			return false;
		}

		public void RemoveButtonMarker()
		{
			if (m_buttonDesc != null)
			{
				MyHud.ButtonPanelMarkers.UnregisterMarker(m_buttonDesc);
			}
		}

		public override void OnSelectionLost()
		{
			RemoveButtonMarker();
		}

		private void SetButtonName(string name)
		{
			if (m_buttonPanel.IsFunctional && m_buttonPanel.IsWorking && (m_buttonPanel.HasLocalPlayerAccess() || m_buttonPanel.AnyoneCanUse))
			{
				m_buttonDesc.Name = name;
			}
			else
			{
				m_buttonDesc.Name = "";
			}
		}
	}
}
