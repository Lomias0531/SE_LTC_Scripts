using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Triggers
{
	internal class MyGuiScreenTriggerNoSpawn : MyGuiScreenTriggerTime
	{
		public MyGuiScreenTriggerNoSpawn(MyTrigger trg)
			: base(trg, MySpaceTexts.GuiTriggerNoSpawnTimeLimit)
		{
			AddCaption(MySpaceTexts.GuiTriggerCaptionNoSpawn);
			m_textboxTime.Text = ((MyTriggerNoSpawn)trg).LimitInSeconds.ToString();
		}

		public override bool IsValid(int time)
		{
			return time >= 15;
		}

		protected override void OnOkButtonClick(MyGuiControlButton sender)
		{
			int? num = StrToInt(m_textboxTime.Text);
			if (num.HasValue)
			{
				((MyTriggerNoSpawn)m_trigger).LimitInSeconds = num.Value;
			}
			base.OnOkButtonClick(sender);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerNoSpawn";
		}
	}
}
