using Sandbox.Graphics.GUI;
using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenTextPanel : MyGuiScreenText
	{
		public MyGuiScreenTextPanel(string missionTitle = null, string currentObjectivePrefix = null, string currentObjective = null, string description = null, Action<ResultEnum> resultCallback = null, Action saveCodeCallback = null, string okButtonCaption = null, bool editable = false, MyGuiScreenBase previousScreen = null)
			: base(missionTitle, currentObjectivePrefix, currentObjective, description, resultCallback, okButtonCaption, null, null, editable)
		{
			base.CanHideOthers = editable;
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}
	}
}
