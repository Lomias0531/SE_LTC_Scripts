using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyUseObject("door")]
	public class MyUseObjectAirtightDoors : MyUseObjectBase
	{
		public readonly MyAirtightDoorGeneric Door;

		public readonly Matrix LocalMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => LocalMatrix * Door.WorldMatrix;

		public override MatrixD WorldMatrix => Door.WorldMatrix;

		public override uint RenderObjectID => Door.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions => PrimaryAction | SecondaryAction;

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectAirtightDoors(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			Door = (MyAirtightDoorGeneric)owner;
			LocalMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			long controllingIdentityId = myCharacter.ControllerInfo.ControllingIdentityId;
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = Door.GetUserRelationToOwner(controllingIdentityId);
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				if (!Door.AnyoneCanUse && !Door.HasLocalPlayerAccess())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
				else
				{
					Door.SetOpenRequest(!Door.Open, controllingIdentityId);
				}
				break;
			case UseActionEnum.OpenTerminal:
				if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
					{
						MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					}
				}
				else
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, myCharacter, Door);
				}
				break;
			}
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenDoor;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]"),
					Door.DefinitionDisplayNameText
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenDoor;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
					Door.DefinitionDisplayNameText
				};
				result.ShowForGamepad = true;
				return result;
			case UseActionEnum.OpenTerminal:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenControlPanel;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), "]"),
					Door.DefinitionDisplayNameText
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenControlPanel;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL),
					Door.DefinitionDisplayNameText
				};
				result.ShowForGamepad = true;
				return result;
			default:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenControlPanel;
				result.FormatParams = new object[2]
				{
					MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL),
					Door.DefinitionDisplayNameText
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

		public override void OnSelectionLost()
		{
		}
	}
}
