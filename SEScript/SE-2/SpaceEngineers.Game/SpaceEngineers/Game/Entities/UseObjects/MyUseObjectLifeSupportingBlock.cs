using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects
{
	[MyUseObject("block")]
	internal class MyUseObjectLifeSupportingBlock : MyUseObjectBase
	{
		private Matrix m_localMatrix;

		public new IMyLifeSupportingBlock Owner
		{
			get
			{
				IMyLifeSupportingBlock myLifeSupportingBlock = base.Owner as IMyLifeSupportingBlock;
				if (myLifeSupportingBlock == null)
				{
					return null;
				}
				return myLifeSupportingBlock;
			}
		}

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => m_localMatrix * Owner.WorldMatrix;

		public override MatrixD WorldMatrix => Owner.WorldMatrix;

		public override uint RenderObjectID
		{
			get
			{
				IMyLifeSupportingBlock owner = Owner;
				if (owner == null)
				{
					return uint.MaxValue;
				}
				uint[] renderObjectIDs = owner.Render.RenderObjectIDs;
				if (renderObjectIDs.Length != 0)
				{
					return renderObjectIDs[0];
				}
				return uint.MaxValue;
			}
		}

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions
		{
			get
			{
				UseActionEnum useActionEnum = PrimaryAction | SecondaryAction;
				IMyLifeSupportingBlock owner = Owner;
				if (owner != null && owner.HasInventory)
				{
					useActionEnum |= UseActionEnum.OpenInventory;
				}
				return useActionEnum;
			}
		}

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

		public override bool ContinuousUsage => true;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectLifeSupportingBlock(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			m_localMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter == null)
			{
				return;
			}
			MyPlayer playerFromCharacter = MyPlayer.GetPlayerFromCharacter(myCharacter);
			IMyLifeSupportingBlock owner = Owner;
			if (owner == null)
			{
				return;
			}
			if (!owner.GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId).IsFriendly() && (playerFromCharacter == null || !MySession.Static.IsUserSpaceMaster(playerFromCharacter.Client.SteamUserId)))
			{
				if (myCharacter.ControllerInfo.Controller.Player == MySession.Static.LocalHumanPlayer)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
				return;
			}
			switch (actionEnum)
			{
			case UseActionEnum.OpenTerminal:
				Owner.ShowTerminal(myCharacter);
				break;
			case UseActionEnum.Manipulate:
				Owner.Components.Get<MyLifeSupportingComponent>().OnSupportRequested(myCharacter);
				break;
			case UseActionEnum.OpenInventory:
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, Owner as MyEntity);
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
				result.Text = MySpaceTexts.NotificationHintPressToRechargeInMedicalRoom;
				result.FormatParams = new object[1]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]")
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToRechargeInMedicalRoom;
				result.JoystickFormatParams = new object[1]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE)
				};
				result.ShowForGamepad = true;
				return result;
			case UseActionEnum.OpenTerminal:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenTerminal;
				result.FormatParams = new object[1]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), "]")
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenTerminal;
				result.JoystickFormatParams = new object[1]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL)
				};
				result.ShowForGamepad = true;
				return result;
			case UseActionEnum.OpenInventory:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenInventory;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY), "]"),
					(Owner is MyCubeBlock) ? ((MyCubeBlock)Owner).DefinitionDisplayNameText : Owner.DisplayNameText
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenInventory;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
					(Owner is MyCubeBlock) ? ((MyCubeBlock)Owner).DefinitionDisplayNameText : Owner.DisplayNameText
				};
				result.ShowForGamepad = true;
				return result;
			default:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenTerminal;
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

		public override void OnSelectionLost()
		{
		}
	}
}
