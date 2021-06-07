using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Triggers;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library;
using VRage.Utils;

namespace Sandbox.Game.World.Triggers
{
	[TriggerType(typeof(MyObjectBuilder_TriggerLives))]
	internal class MyTriggerLives : MyTrigger, ICloneable
	{
		public int LivesLeft = 1;

		private StringBuilder m_progress = new StringBuilder();

		public override bool IsTrue
		{
			get
			{
				return m_IsTrue;
			}
			set
			{
				m_IsTrue = value;
				if (value)
				{
					LivesLeft = 0;
				}
			}
		}

		public MyTriggerLives()
		{
		}

		public MyTriggerLives(MyTriggerLives trg)
			: base(trg)
		{
			LivesLeft = trg.LivesLeft;
		}

		public override object Clone()
		{
			return new MyTriggerLives(this);
		}

		public override bool RaiseSignal(Signal signal)
		{
			if (signal == Signal.PLAYER_DIED)
			{
				LivesLeft--;
				if (LivesLeft <= 0)
				{
					m_IsTrue = true;
				}
			}
			return IsTrue;
		}

		public override void DisplayHints(MyPlayer player, MyEntity me)
		{
			if (MySession.Static.IsScenario)
			{
				MyHud.ScenarioInfo.LivesLeft = LivesLeft;
			}
		}

		public override StringBuilder GetProgress()
		{
			m_progress.Clear().AppendFormat(MySpaceTexts.ScenarioProgressLimitedLives, LivesLeft).Append(MyEnvironment.NewLine);
			return m_progress;
		}

		public override void Init(MyObjectBuilder_Trigger ob)
		{
			base.Init(ob);
			LivesLeft = ((MyObjectBuilder_TriggerLives)ob).Lives;
		}

		public override MyObjectBuilder_Trigger GetObjectBuilder()
		{
			MyObjectBuilder_TriggerLives obj = (MyObjectBuilder_TriggerLives)base.GetObjectBuilder();
			obj.Lives = LivesLeft;
			return obj;
		}

		public override void DisplayGUI()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerLives(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionLives;
		}
	}
}
