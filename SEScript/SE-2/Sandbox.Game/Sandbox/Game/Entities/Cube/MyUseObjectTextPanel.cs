using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyUseObject("textpanel")]
	public class MyUseObjectTextPanel : MyUseObjectBase
	{
		private MyTextPanel m_textPanel;

		private Matrix m_localMatrix;

		public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public override MatrixD ActivationMatrix => m_localMatrix * m_textPanel.WorldMatrix;

		public override MatrixD WorldMatrix => m_textPanel.WorldMatrix;

		public override uint RenderObjectID
		{
			get
			{
				if (m_textPanel.Render == null)
				{
					return uint.MaxValue;
				}
				uint[] renderObjectIDs = m_textPanel.Render.RenderObjectIDs;
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
				UseActionEnum result = UseActionEnum.None;
				if (m_textPanel.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies)
				{
					result = (PrimaryAction | SecondaryAction);
				}
				return result;
			}
		}

		public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

		public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

		public override bool ContinuousUsage => false;

		public override bool PlayIndicatorSound => true;

		public MyUseObjectTextPanel(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
			: base(owner, dummyData)
		{
			m_textPanel = (MyTextPanel)owner;
			m_localMatrix = dummyData.Matrix;
		}

		public override void Use(UseActionEnum actionEnum, IMyEntity user)
		{
			m_textPanel.Use(actionEnum, user);
		}

		public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				result = default(MyActionDescription);
				result.Text = MySpaceTexts.NotificationHintPressToShowScreen;
				result.FormatParams = new object[1]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]")
				};
				result.IsTextControlHint = true;
				result.JoystickText = MySpaceTexts.NotificationHintPressToShowScreen;
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
