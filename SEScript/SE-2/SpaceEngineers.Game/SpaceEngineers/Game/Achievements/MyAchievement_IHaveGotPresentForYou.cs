using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_IHaveGotPresentForYou : MySteamAchievementBase
	{
		private bool m_someoneIsDead;

		private bool m_imDead;

		private long m_lastAttackerID;

		private List<long> m_warheadList = new List<long>();

		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_IHaveGotPresentForYou", null, 0f);
		}

		public override void SessionBeforeStart()
		{
			base.SessionBeforeStart();
			if (!base.IsAchieved)
			{
				MyCharacter.OnCharacterDied += MyCharacter_OnCharacterDied;
				MyWarhead.OnCreated = (Action<MyWarhead>)Delegate.Combine(MyWarhead.OnCreated, new Action<MyWarhead>(MyWarhead_OnCreated));
				MyWarhead.OnDeleted = (Action<MyWarhead>)Delegate.Combine(MyWarhead.OnDeleted, new Action<MyWarhead>(MyWarhead_OnDeleted));
			}
		}

		private void MyWarhead_OnCreated(MyWarhead obj)
		{
			if (obj.BuiltBy == MySession.Static.LocalPlayerId && !m_warheadList.Contains(obj.CubeGrid.EntityId))
			{
				m_warheadList.Add(obj.CubeGrid.EntityId);
			}
		}

		private void MyWarhead_OnDeleted(MyWarhead obj)
		{
			m_warheadList.Remove(obj.CubeGrid.EntityId);
		}

		private void MyCharacter_OnCharacterDied(MyCharacter character)
		{
			if (!(character.StatComp.LastDamage.Type != MyDamageType.Explosion))
			{
				long attackerId = character.StatComp.LastDamage.AttackerId;
				if (attackerId != m_lastAttackerID)
				{
					m_someoneIsDead = false;
					m_imDead = false;
					m_lastAttackerID = attackerId;
				}
				if (character.GetPlayerIdentityId() == MySession.Static.LocalHumanPlayer.Identity.IdentityId)
				{
					m_imDead = true;
				}
				else if (character.IsPlayer)
				{
					m_someoneIsDead = true;
				}
				if (m_imDead && m_someoneIsDead && m_lastAttackerID == attackerId && m_warheadList.Contains(m_lastAttackerID))
				{
					NotifyAchieved();
					MyCharacter.OnCharacterDied -= MyCharacter_OnCharacterDied;
					MyWarhead.OnCreated = (Action<MyWarhead>)Delegate.Remove(MyWarhead.OnCreated, new Action<MyWarhead>(MyWarhead_OnCreated));
					MyWarhead.OnDeleted = (Action<MyWarhead>)Delegate.Remove(MyWarhead.OnDeleted, new Action<MyWarhead>(MyWarhead_OnDeleted));
				}
			}
		}
	}
}
