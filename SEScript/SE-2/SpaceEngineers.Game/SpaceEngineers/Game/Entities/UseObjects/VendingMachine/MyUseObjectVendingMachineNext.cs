using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
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
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects.VendingMachine
{
	[MyUseObject("vendingMachineNext")]
	public class MyUseObjectVendingMachineNext : MyUseObjectBase
	{
		public override MatrixD ActivationMatrix => base.Dummy.Matrix * base.Owner.WorldMatrix;

		public override MatrixD WorldMatrix => base.Owner.WorldMatrix;

		public override uint RenderObjectID => base.Owner.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions
		{
			get
			{
				MyVendingMachine myVendingMachine;
				if ((myVendingMachine = (base.Owner as MyVendingMachine)) == null)
				{
					return UseActionEnum.None;
				}
				if (!myVendingMachine.IsWorking)
				{
					return UseActionEnum.None;
				}
				return PrimaryAction | SecondaryAction;
			}
		}

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.None;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public MyUseObjectVendingMachineNext(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyCubeBlock myCubeBlock = base.Owner as MyCubeBlock;
			string text = (myCubeBlock != null) ? myCubeBlock.DefinitionDisplayNameText : ((MyEntity)base.Owner).DisplayNameText;
			MyActionDescription result;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintNextItem;
				result.FormatParams = new object[1]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]")
				};
				result.IsTextControlHint = true;
				result.JoystickFormatParams = new object[1]
				{
					"[" + MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.USE) + "]"
				};
				result.ShowForGamepad = true;
				return result;
			case UseActionEnum.OpenInventory:
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
			default:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenTerminal;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), "]"),
					text
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintJoystickPressToOpenControlPanel;
				result.JoystickFormatParams = new object[1]
				{
					text
				};
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

		public override void Use(UseActionEnum actionEnum, IMyEntity userEntity)
		{
			MyCharacter myCharacter;
			if ((myCharacter = (userEntity as MyCharacter)) == null)
			{
				return;
			}
			MyPlayer.GetPlayerFromCharacter(myCharacter);
			MyVendingMachine myVendingMachine;
			if ((myVendingMachine = (base.Owner as MyVendingMachine)) == null)
			{
				return;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = myVendingMachine.GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId);
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate | UseActionEnum.OpenTerminal:
				break;
			case UseActionEnum.Manipulate:
				if (!userRelationToOwner.IsFriendly() && (!myVendingMachine.AnyoneCanUse || userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies) && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
				else
				{
					myVendingMachine.SelectNextItem();
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
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, myCharacter, (MyEntity)base.Owner);
				}
				break;
			case UseActionEnum.OpenInventory:
				if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
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

		private void Screen_Closed(MyGuiScreenBase source)
		{
		}
	}
}
