using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyUseObject("cryopod")]
	internal class MyUseObjectCryoChamberDoor : MyUseObjectBase
	{
		public readonly MyCryoChamber CryoChamber;

		public readonly Matrix LocalMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => LocalMatrix * CryoChamber.WorldMatrix;

		public override MatrixD WorldMatrix => CryoChamber.WorldMatrix;

		public override uint RenderObjectID => CryoChamber.Render.GetRenderObjectID();

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions => PrimaryAction | SecondaryAction;

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.None;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectCryoChamberDoor(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			CryoChamber = (owner as MyCryoChamber);
			LocalMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter user = entity as MyCharacter;
			CryoChamber.RequestUse(actionEnum, user);
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result = default(MyActionDescription);
			result.Text = MySpaceTexts.NotificationHintPressToEnterCryochamber;
			result.FormatParams = new object[2]
			{
				"[" + MyGuiSandbox.GetKeyName(MyControlsSpace.USE) + "]",
				CryoChamber.DisplayNameText
			};
			result.IsTextControlHint = true;
			result.JoystickFormatParams = new object[2]
			{
				MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
				CryoChamber.DisplayNameText
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
