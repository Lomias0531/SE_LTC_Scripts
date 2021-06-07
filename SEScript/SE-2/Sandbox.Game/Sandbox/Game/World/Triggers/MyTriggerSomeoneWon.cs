using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Triggers;
using Sandbox.Game.SessionComponents;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library;
using VRage.Utils;

namespace Sandbox.Game.World.Triggers
{
	[TriggerType(typeof(MyObjectBuilder_TriggerSomeoneWon))]
	public class MyTriggerSomeoneWon : MyTrigger, ICloneable
	{
		private StringBuilder m_progress = new StringBuilder();

		public MyTriggerSomeoneWon()
		{
		}

		public MyTriggerSomeoneWon(MyTriggerSomeoneWon trg)
			: base(trg)
		{
		}

		public override object Clone()
		{
			return new MyTriggerSomeoneWon(this);
		}

		public override bool RaiseSignal(Signal signal)
		{
			if (signal == Signal.OTHER_WON)
			{
				m_IsTrue = true;
			}
			return IsTrue;
		}

		public override StringBuilder GetProgress()
		{
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers.Count == 1)
			{
				return null;
			}
			m_progress.Clear().Append((object)MyTexts.Get(MySpaceTexts.ScenarioProgressSomeoneWon));
			foreach (MyPlayer item in onlinePlayers)
			{
				if (item != MySession.Static.LocalHumanPlayer && MySessionComponentMissionTriggers.Static.MissionTriggers.TryGetValue(item.Id, out MyMissionTriggers value) && !value.Lost && !value.Won)
				{
					m_progress.Append(MyEnvironment.NewLine).Append("   ").Append(item.DisplayName);
				}
			}
			return m_progress;
		}

		public override void DisplayGUI()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerSomeoneWon(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionSomeoneWon;
		}
	}
}
