using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyUseObject("cockpit")]
	internal class MyUseObjectCockpitDoor : MyUseObjectBase
	{
		public readonly IMyEntity Cockpit;

		public readonly Matrix LocalMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => LocalMatrix * Cockpit.WorldMatrix;

		public override MatrixD WorldMatrix => Cockpit.WorldMatrix;

		public override uint RenderObjectID => Cockpit.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions
		{
			get
			{
				UseActionEnum useActionEnum = PrimaryAction | SecondaryAction;
				if (base.Owner != null && base.Owner.HasInventory)
				{
					useActionEnum |= UseActionEnum.OpenInventory;
				}
				return useActionEnum;
			}
		}

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.None;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound
		{
			get
			{
				if (Cockpit is MyShipController)
				{
					return (Cockpit as MyShipController).PlayDefaultUseSound;
				}
				return true;
			}
		}

		public MyUseObjectCockpitDoor(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			Cockpit = owner;
			LocalMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCockpit myCockpit = Cockpit as MyCockpit;
			MyCharacter myCharacter = entity as MyCharacter;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				myCockpit?.RequestUse(actionEnum, myCharacter);
				break;
			case UseActionEnum.OpenInventory:
				if (myCockpit == null)
				{
					break;
				}
				if (!myCockpit.GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
					{
						MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					}
				}
				else
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, base.Owner as MyEntity);
				}
				break;
			}
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyCubeBlock myCubeBlock = base.Owner as MyCubeBlock;
			string text = (myCubeBlock != null) ? myCubeBlock.DefinitionDisplayNameText : ((MyEntity)base.Owner).DisplayNameText;
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result;
			if (actionEnum == UseActionEnum.Manipulate || actionEnum != UseActionEnum.OpenInventory)
			{
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToEnterCockpit;
				result.FormatParams = new object[2]
				{
					"[" + MyGuiSandbox.GetKeyName(MyControlsSpace.USE) + "]",
					((MyEntity)Cockpit).DisplayNameText
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToEnterCockpit;
				result.JoystickFormatParams = new object[2]
				{
					"[" + MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE) + "]",
					((MyEntity)Cockpit).DisplayNameText
				};
				result.ShowForGamepad = true;
				return result;
			}
			result = default(MyActionDescription);
			result.Text = MySpaceTexts.NotificationHintPressToOpenInventory;
			result.FormatParams = new object[2]
			{
				string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY), "]"),
				text
			};
			result.IsTextControlHint = true;
			result.JoystickText = MySpaceTexts.NotificationHintJoystickPressToOpenInventory;
			result.JoystickFormatParams = new object[1]
			{
				text
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
