using Sandbox.Engine.Utils;
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
	[MyUseObject("ladder")]
	public class MyUseObjectLadder : MyUseObjectBase
	{
		private MyLadder m_ladder;

		private Matrix m_localMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => m_localMatrix * m_ladder.WorldMatrix;

		public override MatrixD WorldMatrix => m_ladder.WorldMatrix;

		public override uint RenderObjectID
		{
			get
			{
				if (m_ladder.Render == null)
				{
					return uint.MaxValue;
				}
				uint[] renderObjectIDs = m_ladder.Render.RenderObjectIDs;
				if (renderObjectIDs.Length != 0)
				{
					return renderObjectIDs[0];
				}
				return uint.MaxValue;
			}
		}

		public override int InstanceID => -1;

		public override bool ShowOverlay => true;

		public override UseActionEnum SupportedActions => PrimaryAction | SecondaryAction;

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.None;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectLadder(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			m_ladder = (MyLadder)owner;
			m_localMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity user)
		{
			m_ladder.Use(actionEnum, user);
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result = default(MyActionDescription);
			result.Text = MySpaceTexts.NotificationHintPressToGetOnLadder;
			result.FormatParams = new object[1]
			{
				"[" + MyGuiSandbox.GetKeyName(MyControlsSpace.USE) + "]"
			};
			result.IsTextControlHint = true;
			result.JoystickFormatParams = new object[1]
			{
				MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE)
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
