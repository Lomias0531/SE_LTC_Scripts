using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.ModAPI;

namespace SpaceEngineers.Game.Achievements
{
	/// <summary>
	/// Implements achievement Declare War - declare war with some faction
	/// </summary>
	internal class MyAchievement_DeclareWar : MySteamAchievementBase
	{
		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievment_DeclareWar", null, 0f);
		}

		public override void SessionBeforeStart()
		{
			if (!base.IsAchieved)
			{
				MySession.Static.Factions.FactionStateChanged += Factions_FactionStateChanged;
			}
		}

		private void Factions_FactionStateChanged(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			if (MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(identityId);
			if (myFaction != null)
			{
				MyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(toFactionId) as MyFaction;
				if (myFaction2 != null && myFaction2.FactionType == MyFactionTypes.PlayerMade && (myFaction.IsFounder(identityId) || myFaction.IsLeader(identityId)) && myFaction.FactionId == fromFactionId && action == MyFactionStateChange.DeclareWar)
				{
					NotifyAchieved();
					MySession.Static.Factions.FactionStateChanged -= Factions_FactionStateChanged;
				}
			}
		}
	}
}
