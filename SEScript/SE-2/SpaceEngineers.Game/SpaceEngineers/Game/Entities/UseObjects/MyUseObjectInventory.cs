using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
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

namespace SpaceEngineers.Game.Entities.UseObjects
{
	[MyUseObject("inventory")]
	[MyUseObject("conveyor")]
	internal class MyUseObjectInventory : MyUseObjectBase
	{
		public readonly MyEntity Entity;

		public readonly Matrix LocalMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => LocalMatrix * Entity.WorldMatrix;

		public override MatrixD WorldMatrix => Entity.WorldMatrix;

		public override uint RenderObjectID => Entity.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions => PrimaryAction | SecondaryAction | UseActionEnum.BuildPlanner;

		public override UseActionEnum PrimaryAction => UseActionEnum.OpenInventory;

		public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectInventory(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			Entity = (owner as MyEntity);
			LocalMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			MyCubeBlock myCubeBlock = Entity as MyCubeBlock;
			MyContainerDropComponent component;
			if (myCubeBlock != null && !myCubeBlock.GetUserRelationToOwner(myCharacter.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
			{
				if (myCharacter.ControllerInfo.IsLocallyHumanControlled())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				}
			}
			else if (Entity.Components.TryGet(out component))
			{
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenClaimGameItem(component, myCharacter.GetPlayerIdentityId()));
			}
			else
			{
				MyGuiScreenTerminal.Show((actionEnum == UseActionEnum.OpenTerminal) ? MyTerminalPageEnum.ControlPanel : MyTerminalPageEnum.Inventory, myCharacter, Entity);
			}
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyCubeBlock myCubeBlock = Entity as MyCubeBlock;
			string text = (myCubeBlock != null) ? myCubeBlock.DefinitionDisplayNameText : Entity.DisplayNameText;
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result;
			switch (actionEnum)
			{
			case UseActionEnum.OpenInventory:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToOpenInventory;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY), "]"),
					text
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenInventory;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
					text
				};
				result.ShowForGamepad = true;
				return result;
			case UseActionEnum.BuildPlanner:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToWithdraw;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER), "]"),
					text
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToWithdraw;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS),
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
				result.JoystickText = MySpaceTexts.NotificationHintPressToOpenTerminal;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL),
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
	}
}
