using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyGuiScreenProgress : MyGuiScreenProgressBase
	{
		public StringBuilder Text
		{
			get
			{
				return m_progressTextLabel.TextToDraw;
			}
			set
			{
				m_progressTextLabel.TextToDraw = value;
			}
		}

		public event Action Tick;

		public MyGuiScreenProgress(StringBuilder text, MyStringId? cancelText = null, bool isTopMostScreen = true, bool enableBackgroundFade = true)
			: base(MySpaceTexts.Blank, cancelText, isTopMostScreen, enableBackgroundFade)
		{
			Text = new StringBuilder(text.Length);
			Text.AppendStringBuilder(text);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_rotatingWheel.MultipleSpinningWheels = MyPerGameSettings.GUI.MultipleSpinningWheels;
		}

		protected override void ProgressStart()
		{
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenProgress";
		}

		public override bool Update(bool hasFocus)
		{
			Action tick = this.Tick;
			if (tick != null && !base.Cancelled)
			{
				tick();
			}
			return base.Update(hasFocus);
		}
	}
}
