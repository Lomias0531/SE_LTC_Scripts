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
	[MyUseObject("terminal")]
	public class MyUseObjectTerminal : MyUseObjectBase
	{
		public readonly MyCubeBlock Block;

		public readonly Matrix LocalMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => LocalMatrix * Block.WorldMatrix;

		public override MatrixD WorldMatrix => Block.WorldMatrix;

		public override uint RenderObjectID => Block.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions
		{
			get
			{
				UseActionEnum useActionEnum = UseActionEnum.OpenTerminal;
				if (Block.GetInventory() != null)
				{
					useActionEnum |= UseActionEnum.OpenInventory;
				}
				return useActionEnum;
			}
		}

		public override UseActionEnum PrimaryAction => UseActionEnum.OpenTerminal;

		public override UseActionEnum SecondaryAction
		{
			get
			{
				if (Block.GetInventory() == null)
				{
					return UseActionEnum.None;
				}
				return UseActionEnum.OpenInventory;
			}
		}

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectTerminal(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			Block = (owner as MyCubeBlock);
			LocalMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (!Block.GetUserRelationToOwner(myCharacter.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
			{
				if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
				return;
			}
			switch (actionEnum)
			{
			case UseActionEnum.OpenTerminal:
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, myCharacter, Block);
				break;
			case UseActionEnum.OpenInventory:
				if (Block.GetInventory() != null)
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, Block);
				}
				break;
			}
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result;
			if (actionEnum != UseActionEnum.OpenTerminal && actionEnum == UseActionEnum.OpenInventory)
			{
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenInventory;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY), "]"),
					Block.DefinitionDisplayNameText
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenInventory;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
					Block.DefinitionDisplayNameText
				};
				result.ShowForGamepad = true;
				return result;
			}
			result = default(MyActionDescription);
			result.Text = MySpaceTexts.NotificationHintPressToOpenControlPanel;
			result.FormatParams = new object[2]
			{
				string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), "]"),
				Block.DefinitionDisplayNameText
			};
			result.IsTextControlHint = true;
			result.JoystickText = MySpaceTexts.NotificationHintPressToOpenControlPanel;
			result.JoystickFormatParams = new object[2]
			{
				MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL),
				Block.DefinitionDisplayNameText
			};
			result.ShowForGamepad = true;
			return result;
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
