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
	[TriggerType(typeof(MyObjectBuilder_TriggerAllOthersLost))]
	internal class MyTriggerAllOthersLost : MyTrigger, ICloneable
	{
		private StringBuilder m_progress = new StringBuilder();

		public MyTriggerAllOthersLost()
		{
		}

		public MyTriggerAllOthersLost(MyTriggerAllOthersLost trg)
			: base(trg)
		{
		}

		public override object Clone()
		{
			return new MyTriggerAllOthersLost(this);
		}

		public override bool RaiseSignal(Signal signal)
		{
			if (signal == Signal.ALL_OTHERS_LOST)
			{
				m_IsTrue = true;
			}
			return IsTrue;
		}

		public override StringBuilder GetProgress()
		{
			m_progress.Clear().Append((object)MyTexts.Get(MySpaceTexts.ScenarioProgressOthersLost));
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			if (onlinePlayers.Count == 1)
			{
				return null;
			}
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
			MyGuiSandbox.AddScreen(new MyGuiScreenTriggerAllOthersLost(this));
		}

		public new static MyStringId GetCaption()
		{
			return MySpaceTexts.GuiTriggerCaptionAllOthersLost;
		}
	}
}
