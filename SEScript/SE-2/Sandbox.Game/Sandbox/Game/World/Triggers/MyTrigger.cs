using Sandbox.Game.GameSystems;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.World.Triggers
{
	public class MyTrigger : ICloneable
	{
		protected bool m_IsTrue;

		public string Message;

		public string WwwLink;

		public string NextMission;

		public virtual bool IsTrue
		{
			get
			{
				return m_IsTrue;
			}
			set
			{
				m_IsTrue = value;
			}
		}

		public void SetTrue()
		{
			IsTrue = true;
			if (WwwLink != null && WwwLink.Length > 0)
			{
				MyGuiSandbox.OpenUrlWithFallback(WwwLink, "Scenario info", useWhitelist: true);
			}
			if (NextMission != null && NextMission.Length > 0 && MySession.Static.IsScenario)
			{
				MyScenarioSystem.LoadNextScenario(NextMission);
			}
		}

		public MyTrigger()
		{
		}

		public MyTrigger(MyTrigger trg)
		{
			m_IsTrue = trg.m_IsTrue;
			if (trg.Message != null)
			{
				Message = string.Copy(trg.Message);
			}
			if (trg.WwwLink != null)
			{
				WwwLink = string.Copy(trg.WwwLink);
			}
			if (trg.NextMission != null)
			{
				NextMission = string.Copy(trg.NextMission);
			}
		}

		public virtual object Clone()
		{
			return new MyTrigger(this);
		}

		public virtual bool Update(MyPlayer player, MyEntity me)
		{
			return IsTrue;
		}

		public virtual bool RaiseSignal(Signal signal)
		{
			return IsTrue;
		}

		public virtual void DisplayHints(MyPlayer player, MyEntity me)
		{
		}

		public virtual StringBuilder GetProgress()
		{
			return null;
		}

		public virtual void Init(MyObjectBuilder_Trigger ob)
		{
			m_IsTrue = ob.IsTrue;
			Message = ob.Message;
			WwwLink = ob.WwwLink;
			NextMission = ob.NextMission;
		}

		public virtual MyObjectBuilder_Trigger GetObjectBuilder()
		{
			MyObjectBuilder_Trigger myObjectBuilder_Trigger = TriggerFactory.CreateObjectBuilder(this);
			myObjectBuilder_Trigger.IsTrue = m_IsTrue;
			myObjectBuilder_Trigger.Message = Message;
			myObjectBuilder_Trigger.WwwLink = WwwLink;
			myObjectBuilder_Trigger.NextMission = NextMission;
			return myObjectBuilder_Trigger;
		}

		public virtual void DisplayGUI()
		{
		}

		public static MyStringId GetCaption()
		{
			return MyCommonTexts.MessageBoxCaptionError;
		}
	}
}
