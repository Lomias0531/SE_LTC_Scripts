using System;
using System.Text;
using VRage.Library;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlCountdownWheel : MyGuiControlRotatingWheel
	{
		private StringBuilder m_sb;

		private int m_time;

		private int m_shownTime;

		public MyGuiControlCountdownWheel(Vector2? position = null, string texture = "Textures\\GUI\\screens\\screen_loading_wheel.dds", Vector2? textureResolution = null, int seconds = 10, float radiansPerSecond = MathF.PI * 2f, float scale = 0.36f)
			: base(position, null, scale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, texture, manualRotationUpdate: true, multipleSpinningWheels: false, textureResolution, radiansPerSecond)
		{
			m_sb = new StringBuilder();
			m_sb.Append(seconds);
			m_time = MyEnvironment.TickCount + seconds * 1000 + 999;
			m_shownTime = seconds;
		}

		public override void Update()
		{
			base.Update();
			int num = (m_time - MyEnvironment.TickCount) / 1000;
			if (num < 0)
			{
				num = 0;
			}
			if (num != m_shownTime)
			{
				m_shownTime = num;
				m_sb.Clear();
				if (m_shownTime > 0)
				{
					m_sb.Append(m_shownTime);
				}
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			MyGuiManager.DrawString("White", m_sb, GetPositionAbsoluteCenter(), 1f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}
	}
}
