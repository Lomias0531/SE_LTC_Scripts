using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Triggers
{
	internal class MyGuiScreenTriggerTimeLimit : MyGuiScreenTriggerTime
	{
		public MyGuiScreenTriggerTimeLimit(MyTrigger trg)
			: base(trg, MySpaceTexts.GuiTriggerTimeLimit)
		{
			AddCaption(MySpaceTexts.GuiTriggerCaptionTimeLimit);
			m_textboxTime.Text = ((MyTriggerTimeLimit)trg).LimitInMinutes.ToString();
		}

		public override bool IsValid(int time)
		{
			return time > 0;
		}

		protected override void OnOkButtonClick(MyGuiControlButton sender)
		{
			int? num = StrToInt(m_textboxTime.Text);
			if (num.HasValue)
			{
				((MyTriggerTimeLimit)m_trigger).LimitInMinutes = num.Value;
			}
			base.OnOkButtonClick(sender);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerTimeLimit";
		}
	}
}
