using Sandbox.Engine.Networking;
using Sandbox.ModAPI;
using System;
using VRage.GameServices;

namespace Sandbox.Game.SessionComponents
{
	public abstract class MySteamAchievementBase
	{
		protected IMyAchievement m_remoteAchievement;

		public bool IsAchieved
		{
			get;
			protected set;
		}

		public abstract bool NeedsUpdate
		{
			get;
		}

		public event Action<MySteamAchievementBase> Achieved;

		public virtual void SessionLoad()
		{
		}

		public virtual void SessionUpdate()
		{
		}

		public virtual void SessionSave()
		{
		}

		public virtual void SessionUnload()
		{
		}

		public virtual void SessionBeforeStart()
		{
		}

		protected MySteamAchievementBase()
		{
			(string achievementId, string statTag, float statMaxValue) achievementInfo = GetAchievementInfo();
			string item = achievementInfo.achievementId;
			string item2 = achievementInfo.statTag;
			float item3 = achievementInfo.statMaxValue;
			m_remoteAchievement = MyGameService.GetAchievement(item, item2, item3);
			if (!string.IsNullOrEmpty(item2))
			{
				m_remoteAchievement.OnStatValueChanged += LoadStatValue;
			}
		}

		protected void NotifyAchieved()
		{
			m_remoteAchievement.Unlock();
			if (MySteamAchievements.OFFLINE_ACHIEVEMENT_INFO)
			{
				string item = GetAchievementInfo().achievementId;
				MyAPIGateway.Utilities.ShowNotification("Achievement Unlocked: " + item, 10000, "Red");
			}
			IsAchieved = true;
			this.Achieved.InvokeIfNotNull(this);
			this.Achieved = null;
		}

		public virtual void Init()
		{
			IsAchieved = m_remoteAchievement.IsUnlocked;
			if (!IsAchieved)
			{
				LoadStatValue();
			}
		}

		protected abstract (string achievementId, string statTag, float statMaxValue) GetAchievementInfo();

		protected virtual void LoadStatValue()
		{
		}
	}
}
