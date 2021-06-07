using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Triggers;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.World.Triggers
{
	[TriggerType(typeof(MyObjectBuilder_TriggerNoSpawn))]
	internal class MyTriggerNoSpawn : MyTrigger, ICloneable
	{
		private bool m_isDead;

		private bool m_couldRespawn = true;

		private DateTime m_begin;

		private int m_limitInSeconds = 60;

		private TimeSpan m_limit = new TimeSpan(0, 1, 0);

		private int m_lastSeconds;

		private StringBuilder m_guiText = new StringBuilder();

		private StringBuilder m_progress = new StringBuilder();

		public int LimitInSeconds
		{
			get
			{
				return m_limitInSeconds;
			}
			set
			{
				m_limitInSeconds = value;
				m_limit = new TimeSpan(0, 0, value);
			}
		}

		public MyTriggerNoSpawn()
		{
		}

		public MyTriggerNoSpawn(MyTriggerNoSpawn trg)
			: base(trg)
		{
			LimitInSeconds = trg.LimitInSeconds;
		}

		public override object Clone()
		{
			return new MyTriggerNoSpawn(this);
		}

		public override bool Update(MyPlayer player, MyEntity me)
		{
			if (player.Identity.IsDead)
			{
				if (m_begin == DateTime.MinValue)
				{
					m_begin = DateTime.UtcNow;
				}
				if (DateTime.UtcNow - m_begin > m_limit)
				{
					m_IsTrue = true;
				}
			}
			else
			{
				m_begin = DateTime.MinValue;
			}
			return IsTrue;
		}

		public override void DisplayHints(MyPlayer player, MyEntity me)
		{
			if (!MySession.Static.IsScenario)
			{
				return;
			}
			if (m_IsTrue)
			{
				m_begin = DateTime.MinValue;
				return;
			}
			if (Sync.Players.RespawnComponent.IsInRespawnScreen())
			{
				if (m_begin == DateTime.MinValue)
				{
					m_begin = DateTime.UtcNow;
				}
			}
			else
			{
				m_begin = DateTime.MinValue;
			}
			if (!(m_begin == DateTime.MinValue))
			{
				TimeSpan timeSpan = m_limit - (DateTime.UtcNow - m_begin);
				int seconds = timeSpan.Seconds;
				if (m_lastSeconds != seconds)
				{
					m_lastSeconds = seconds;
					m_guiText.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScreenMedicals_NoRespawnPlace), (int)timeSpan.TotalMinutes, seconds);
					Sync.Players.RespawnComponent.SetNoRespawnText(m_guiText, (int)timeSpan.TotalSeconds);
				}
			}
		}

		public override StringBuilder GetProgress()
		{
			m_progress.Clear().AppendFormat(MySpaceTexts.ScenarioProgressNoSpawn, LimitInSeconds);
			return m_progress;
		}

		public override void Init(MyObjectBuilder_Trigger ob)
		{
			base.Init(ob);
			LimitInSeconds = ((MyObjectBuilder_TriggerNoSpawn)ob).Limit;
		}

		public override MyObjectBuilder_Trigger GetObjectBuilder()
		{
			MyObjectBuilder_TriggerNoSpawn obj = (MyObjectBuilder_TriggerNoSpawn)base.GetObjectBuilder();
			obj.Limit = LimitInSeconds;
			return obj;
		}

		public override void DisplayGUI()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerNoSpawn(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionNoSpawn;
		}
	}
}
