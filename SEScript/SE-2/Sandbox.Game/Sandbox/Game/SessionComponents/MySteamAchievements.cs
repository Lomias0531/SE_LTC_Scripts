using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Plugins;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000)]
	public class MySteamAchievements : MySessionComponentBase
	{
		public static readonly bool OFFLINE_ACHIEVEMENT_INFO = false;

		private static readonly List<MySteamAchievementBase> m_achievements = new List<MySteamAchievementBase>();

		private static bool m_initialized = false;

		private static bool m_achievementsLoaded = false;

		private double m_lastTimestamp;

		private static void Init()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
			{
				MyGameService.OnUserChanged += delegate
				{
					bool achievementsLoaded = m_achievementsLoaded;
					if (achievementsLoaded)
					{
						UnloadAchievements();
					}
					m_achievements.Clear();
					InitializeAchievements();
					if (achievementsLoaded)
					{
						LoadAchievements();
					}
				};
				MyGameService.LoadStats();
				InitializeAchievements();
				m_initialized = true;
			}
		}

		private static void InitializeAchievements()
		{
			List<MySteamAchievementBase> list = new List<MySteamAchievementBase>();
			Type[] types = MyPlugins.GameAssembly.GetTypes();
			foreach (Type type2 in types)
			{
				try
				{
					if (typeof(MySteamAchievementBase).IsAssignableFrom(type2))
					{
						list.Add((MySteamAchievementBase)Activator.CreateInstance(type2));
					}
				}
				catch (Exception e2)
				{
					ReportAchievementException(e2, type2);
				}
			}
			foreach (MySteamAchievementBase item in list)
			{
				try
				{
					item.Init();
					if (!item.IsAchieved)
					{
						item.Achieved += delegate
						{
							MyGameService.StoreStats();
						};
						m_achievements.Add(item);
					}
				}
				catch (Exception e3)
				{
					ReportAchievementException(e3, item.GetType());
				}
			}
			void ReportAchievementException(Exception e, Type type)
			{
				MySandboxGame.Log.WriteLine("Initialization of achievement failed: " + type.Name);
				MySandboxGame.Log.IncreaseIndent();
				MySandboxGame.Log.WriteLine(e);
				MySandboxGame.Log.DecreaseIndent();
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (m_initialized)
			{
				foreach (MySteamAchievementBase achievement in m_achievements)
				{
					if (achievement.NeedsUpdate && !achievement.IsAchieved)
					{
						achievement.SessionUpdate();
					}
				}
				if ((double)MySession.Static.ElapsedPlayTime.Minutes > m_lastTimestamp)
				{
					m_lastTimestamp = MySession.Static.ElapsedPlayTime.Minutes;
					MyGameService.StoreStats();
				}
			}
		}

		public override void LoadData()
		{
			if (!m_initialized)
			{
				Init();
			}
			if (m_initialized)
			{
				LoadAchievements();
			}
		}

		private static void LoadAchievements()
		{
			m_achievementsLoaded = true;
			foreach (MySteamAchievementBase achievement in m_achievements)
			{
				if (!achievement.IsAchieved)
				{
					achievement.SessionLoad();
				}
			}
		}

		public override void SaveData()
		{
			if (m_initialized)
			{
				foreach (MySteamAchievementBase achievement in m_achievements)
				{
					if (!achievement.IsAchieved)
					{
						achievement.SessionSave();
					}
				}
				MyGameService.StoreStats();
			}
		}

		protected override void UnloadData()
		{
			if (m_initialized)
			{
				UnloadAchievements();
				MyGameService.StoreStats();
			}
		}

		private static void UnloadAchievements()
		{
			m_achievementsLoaded = false;
			foreach (MySteamAchievementBase achievement in m_achievements)
			{
				if (!achievement.IsAchieved)
				{
					achievement.SessionUnload();
				}
			}
		}

		public override void BeforeStart()
		{
			if (m_initialized)
			{
				foreach (MySteamAchievementBase achievement in m_achievements)
				{
					if (!achievement.IsAchieved)
					{
						achievement.SessionBeforeStart();
					}
				}
			}
		}
	}
}
