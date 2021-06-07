using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Temperature", 63)]
	internal class MyIngameHelpTemperature : MyIngameHelpObjective
	{
		public MyIngameHelpTemperature()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Temperature_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Camera"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Temperature_Detail1
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			DelayToAppear = (float)TimeSpan.FromMinutes(10.0).TotalSeconds;
		}
	}
}
