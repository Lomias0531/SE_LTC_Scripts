using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Localization;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using VRage;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	public class MyGridLandingSystem
	{
		private static readonly int GEAR_MODE_COUNT = MyUtils.GetMaxValueFromEnum<LandingGearMode>() + 1;

		private static readonly List<IMyLandingGear> m_gearTmpList = new List<IMyLandingGear>();

		private HashSet<IMyLandingGear>[] m_gearStates;

		private LockModeChangedHandler m_onStateChanged;

		public MyStringId HudMessage = MyStringId.NullOrEmpty;

		public MyMultipleEnabledEnum Locked
		{
			get
			{
				int totalGearCount = TotalGearCount;
				if (totalGearCount == 0)
				{
					return MyMultipleEnabledEnum.NoObjects;
				}
				if (totalGearCount == this[LandingGearMode.Locked])
				{
					return MyMultipleEnabledEnum.AllEnabled;
				}
				if (totalGearCount == this[LandingGearMode.ReadyToLock] + this[LandingGearMode.Unlocked])
				{
					return MyMultipleEnabledEnum.AllDisabled;
				}
				return MyMultipleEnabledEnum.Mixed;
			}
		}

		public int TotalGearCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < GEAR_MODE_COUNT; i++)
				{
					num += m_gearStates[i].Count;
				}
				return num;
			}
		}

		public int this[LandingGearMode mode] => m_gearStates[(int)mode].Count;

		public MyGridLandingSystem()
		{
			m_gearStates = new HashSet<IMyLandingGear>[GEAR_MODE_COUNT];
			for (int i = 0; i < GEAR_MODE_COUNT; i++)
			{
				m_gearStates[i] = new HashSet<IMyLandingGear>();
			}
			m_onStateChanged = StateChanged;
		}

		private void StateChanged(IMyLandingGear gear, LandingGearMode oldMode)
		{
			if (oldMode == LandingGearMode.ReadyToLock && gear.LockMode == LandingGearMode.Locked)
			{
				HudMessage = MySpaceTexts.NotificationLandingGearSwitchLocked;
			}
			else if (oldMode == LandingGearMode.Locked && gear.LockMode == LandingGearMode.Unlocked)
			{
				HudMessage = MySpaceTexts.NotificationLandingGearSwitchUnlocked;
			}
			else
			{
				HudMessage = MyStringId.NullOrEmpty;
			}
			m_gearStates[(int)oldMode].Remove(gear);
			m_gearStates[(int)gear.LockMode].Add(gear);
		}

		public void Switch()
		{
			if (Locked == MyMultipleEnabledEnum.AllEnabled || Locked == MyMultipleEnabledEnum.Mixed)
			{
				Switch(enabled: false);
			}
			else if (Locked == MyMultipleEnabledEnum.AllDisabled)
			{
				Switch(enabled: true);
			}
		}

		public List<IMyEntity> GetAttachedEntities()
		{
			List<IMyEntity> list = new List<IMyEntity>();
			foreach (IMyLandingGear item in m_gearStates[2])
			{
				IMyEntity attachedEntity = item.GetAttachedEntity();
				if (attachedEntity != null)
				{
					list.Add(attachedEntity);
				}
			}
			return list;
		}

		public void Switch(bool enabled)
		{
			int num = enabled ? 1 : 2;
			bool flag = !enabled && m_gearStates[2].Count > 0;
			foreach (IMyLandingGear item in m_gearStates[num])
			{
				m_gearTmpList.Add(item);
			}
			if (enabled)
			{
				foreach (IMyLandingGear item2 in m_gearStates[0])
				{
					m_gearTmpList.Add(item2);
				}
			}
			foreach (IMyLandingGear gearTmp in m_gearTmpList)
			{
				gearTmp.RequestLock(enabled);
			}
			m_gearTmpList.Clear();
			if (flag)
			{
				HashSet<IMyLandingGear>[] gearStates = m_gearStates;
				for (int i = 0; i < gearStates.Length; i++)
				{
					foreach (IMyLandingGear item3 in gearStates[i])
					{
						if (item3.AutoLock)
						{
							item3.ResetAutolock();
						}
					}
				}
			}
		}

		public void Register(IMyLandingGear gear)
		{
			gear.LockModeChanged += m_onStateChanged;
			m_gearStates[(int)gear.LockMode].Add(gear);
		}

		public void Unregister(IMyLandingGear gear)
		{
			m_gearStates[(int)gear.LockMode].Remove(gear);
			gear.LockModeChanged -= m_onStateChanged;
		}
	}
}
