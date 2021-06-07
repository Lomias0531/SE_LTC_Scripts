using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Input;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class MyResearchDebugInputComponent : MyDebugComponent
	{
		public MyResearchDebugInputComponent()
		{
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Show Your Research", ShowResearch);
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Toggle Pretty Mode", ShowResearchPretty);
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Unlock Your Research", UnlockResearch);
			AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Unlock All Research", UnlockAllResearch);
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Reset Your Research", ResetResearch);
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Reset All Research", ResetAllResearch);
		}

		public override string GetName()
		{
			return "Research";
		}

		private bool ShowResearch()
		{
			MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH = !MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH;
			return true;
		}

		private bool ShowResearchPretty()
		{
			MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH_PRETTY = !MySessionComponentResearch.Static.DEBUG_SHOW_RESEARCH_PRETTY;
			return true;
		}

		private bool ResetResearch()
		{
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				MySessionComponentResearch.Static.ResetResearch(MySession.Static.LocalCharacter);
			}
			return true;
		}

		private bool ResetAllResearch()
		{
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				MyCharacter myCharacter = onlinePlayer.Controller.ControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					MySessionComponentResearch.Static.ResetResearch(myCharacter);
				}
			}
			return true;
		}

		private bool UnlockResearch()
		{
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				MySessionComponentResearch.Static.DebugUnlockAllResearch(MySession.Static.LocalCharacter.GetPlayerIdentityId());
			}
			return true;
		}

		private bool UnlockAllResearch()
		{
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				MySessionComponentResearch.Static.DebugUnlockAllResearch(onlinePlayer.Identity.IdentityId);
			}
			return true;
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			return base.HandleInput();
		}
	}
}
