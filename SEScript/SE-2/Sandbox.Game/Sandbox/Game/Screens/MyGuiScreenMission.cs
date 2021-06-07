using Sandbox.Game.Gui;
using System;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenMission : MyGuiScreenText
	{
		public MyGuiScreenMission(string missionTitle = null, string currentObjectivePrefix = null, string currentObjective = null, string description = null, Action<ResultEnum> resultCallback = null, string okButtonCaption = null, Vector2? windowSize = null, Vector2? descSize = null, bool editEnabled = false, bool canHideOthers = true, bool enableBackgroundFade = false, MyMissionScreenStyleEnum style = MyMissionScreenStyleEnum.BLUE)
			: base(missionTitle, currentObjectivePrefix, currentObjective, description, resultCallback, okButtonCaption, windowSize, descSize, editEnabled, canHideOthers, enableBackgroundFade, style)
		{
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
		}

		public override bool Draw()
		{
			return base.Draw();
		}
	}
}
