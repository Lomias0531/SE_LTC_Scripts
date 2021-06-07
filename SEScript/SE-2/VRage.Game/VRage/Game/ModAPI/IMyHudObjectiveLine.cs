using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	public interface IMyHudObjectiveLine
	{
		bool Visible
		{
			get;
		}

		string Title
		{
			get;
			set;
		}

		string CurrentObjective
		{
			get;
		}

		List<string> Objectives
		{
			get;
			set;
		}

		void Show();

		void Hide();

		void AdvanceObjective();
	}
}
