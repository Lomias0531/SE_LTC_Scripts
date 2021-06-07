using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Triggers
{
	internal class MyGuiScreenTriggerLives : MyGuiScreenTrigger
	{
		private MyGuiControlLabel m_labelLives;

		protected MyGuiControlTextbox m_lives;

		private const float WINSIZEX = 0.4f;

		private const float WINSIZEY = 0.37f;

		private const float spacingH = 0.01f;

		public MyGuiScreenTriggerLives(MyTrigger trg)
			: base(trg, new Vector2(0.5f, 0.37f))
		{
			float num = m_textboxMessage.Position.X - m_textboxMessage.Size.X / 2f;
			float y = -0.185f + MyGuiScreenTrigger.MIDDLE_PART_ORIGIN.Y;
			m_labelLives = new MyGuiControlLabel(new Vector2(num, y), new Vector2(0.01f, 0.035f), MyTexts.Get(MySpaceTexts.GuiTriggersLives).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			num += m_labelLives.Size.X + 0.01f;
			m_lives = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num, y),
				Size = new Vector2(0.110000014f - m_labelLives.Size.X, 0.035f),
				Type = MyGuiControlTextboxType.DigitsOnly,
				Name = "lives"
			};
			m_lives.TextChanged += OnLivesChanged;
			AddCaption(MySpaceTexts.GuiTriggerCaptionLives);
			Controls.Add(m_labelLives);
			Controls.Add(m_lives);
			m_lives.Text = ((MyTriggerLives)trg).LivesLeft.ToString();
		}

		public void OnLivesChanged(MyGuiControlTextbox sender)
		{
			int? num = StrToInt(sender.Text);
			if (num.HasValue && num > 0)
			{
				sender.ColorMask = Vector4.One;
				m_okButton.Enabled = true;
			}
			else
			{
				sender.ColorMask = Color.Red.ToVector4();
				m_okButton.Enabled = false;
			}
		}

		protected override void OnOkButtonClick(MyGuiControlButton sender)
		{
			int? num = StrToInt(m_lives.Text);
			if (num.HasValue)
			{
				((MyTriggerLives)m_trigger).LivesLeft = num.Value;
			}
			base.OnOkButtonClick(sender);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerLives";
		}
	}
}
