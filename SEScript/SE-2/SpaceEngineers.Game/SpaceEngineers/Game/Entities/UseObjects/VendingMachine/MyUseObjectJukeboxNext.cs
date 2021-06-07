using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects.VendingMachine
{
	[MyUseObject("jukeboxNext")]
	public class MyUseObjectJukeboxNext : MyUseObjectBase
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
				MyJukebox myJukebox;
				if ((myJukebox = (base.Owner as MyJukebox)) == null)
				{
					return UseActionEnum.None;
				}
				if (!myJukebox.IsWorking)
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

		public MyUseObjectJukeboxNext(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyActionDescription result;
			if (actionEnum == UseActionEnum.Manipulate)
			{
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintJukeboxPlayNext;
				result.FormatParams = new object[1]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]")
				};
				result.IsTextControlHint = true;
				result.JoystickFormatParams = new object[1]
				{
					"[" + MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.USE) + "]"
				};
				return result;
			}
			result = default(MyActionDescription);
			return result;
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
			MyCharacter character;
			if ((character = (userEntity as MyCharacter)) != null)
			{
				MyPlayer.GetPlayerFromCharacter(character);
				MyJukebox myJukebox;
				if ((myJukebox = (base.Owner as MyJukebox)) != null && actionEnum == UseActionEnum.Manipulate)
				{
					myJukebox.PlayNext();
				}
			}
		}
	}
}
