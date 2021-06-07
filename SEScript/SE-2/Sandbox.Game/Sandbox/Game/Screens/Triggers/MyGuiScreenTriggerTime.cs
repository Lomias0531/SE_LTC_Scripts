using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Triggers
{
	public abstract class MyGuiScreenTriggerTime : MyGuiScreenTrigger
	{
		private MyGuiControlLabel m_labelTime;

		protected MyGuiControlTextbox m_textboxTime;

		private const float WINSIZEX = 0.4f;

		private const float WINSIZEY = 0.37f;

		private const float spacingH = 0.01f;

		public MyGuiScreenTriggerTime(MyTrigger trg, MyStringId labelText)
			: base(trg, new Vector2(0.5f, 0.37f))
		{
			float num = m_textboxMessage.Position.X - m_textboxMessage.Size.X / 2f;
			float y = -0.185f + MyGuiScreenTrigger.MIDDLE_PART_ORIGIN.Y;
			m_labelTime = new MyGuiControlLabel(new Vector2(num, y), new Vector2(0.013f, 0.035f), MyTexts.Get(labelText).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			num += m_labelTime.Size.X + 0.01f;
			m_textboxTime = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num, y),
				Size = new Vector2(0.05f, 0.035f),
				Type = MyGuiControlTextboxType.DigitsOnly,
				Name = "time"
			};
			m_textboxTime.TextChanged += OnTimeChanged;
			Controls.Add(m_labelTime);
			Controls.Add(m_textboxTime);
		}

		public void OnTimeChanged(MyGuiControlTextbox sender)
		{
			int? num = StrToInt(sender.Text);
			if (num.HasValue && IsValid(num.Value))
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

		public virtual bool IsValid(int time)
		{
			return true;
		}
	}
}
