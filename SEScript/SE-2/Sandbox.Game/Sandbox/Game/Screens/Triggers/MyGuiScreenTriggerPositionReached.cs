using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Triggers
{
	public class MyGuiScreenTriggerPositionReached : MyGuiScreenTriggerPosition
	{
		public MyGuiScreenTriggerPositionReached(MyTrigger trg)
			: base(trg)
		{
			AddCaption(MySpaceTexts.GuiTriggerCaptionPositionReached);
			m_xCoord.Text = ((MyTriggerPositionReached)trg).TargetPos.X.ToString();
			m_yCoord.Text = ((MyTriggerPositionReached)trg).TargetPos.Y.ToString();
			m_zCoord.Text = ((MyTriggerPositionReached)trg).TargetPos.Z.ToString();
			m_radius.Text = ((MyTriggerPositionReached)trg).Radius.ToString();
		}

		protected override void OnOkButtonClick(MyGuiControlButton sender)
		{
			double? num = StrToDouble(m_radius.Text);
			if (num.HasValue)
			{
				((MyTriggerPositionReached)m_trigger).Radius = num.Value;
			}
			if (m_coordsChanged)
			{
				((MyTriggerPositionReached)m_trigger).TargetPos = m_coords;
			}
			base.OnOkButtonClick(sender);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerPositionReached";
		}
	}
}
