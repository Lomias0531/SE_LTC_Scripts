using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyHudStatManager
	{
		private readonly Dictionary<MyStringHash, IMyHudStat> m_stats = new Dictionary<MyStringHash, IMyHudStat>();

		private readonly Dictionary<Type, IMyHudStat> m_statsByType = new Dictionary<Type, IMyHudStat>();

		private void RegisterModStats()
		{
			if (MyScriptManager.Static != null && MyScriptManager.Static.Scripts != null)
			{
				MyScriptManager.Static.Scripts.ForEach(delegate(KeyValuePair<MyStringId, Assembly> pair)
				{
					RegisterFromAssembly(pair.Value);
				});
			}
		}

		public MyHudStatManager()
		{
			RegisterFromAssembly(GetType().Assembly);
			RegisterModStats();
		}

		public void RegisterFromAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
				Type derivedType = typeof(IMyHudStat);
				(from t in assembly.GetTypes()
					where t != derivedType && derivedType.IsAssignableFrom(t) && !t.IsAbstract
					select t).ForEach(delegate(Type stat)
				{
					IMyHudStat myHudStat = (IMyHudStat)Activator.CreateInstance(stat);
					m_stats[myHudStat.Id] = myHudStat;
					m_statsByType[stat] = myHudStat;
				});
			}
		}

		public bool Register(IMyHudStat stat)
		{
			Type type = stat.GetType();
			if (m_stats.ContainsKey(stat.Id))
			{
				return false;
			}
			if (m_statsByType.ContainsKey(type))
			{
				return false;
			}
			m_stats[stat.Id] = stat;
			m_statsByType[type] = stat;
			return true;
		}

		public IMyHudStat GetStat(MyStringHash id)
		{
			IMyHudStat value = null;
			m_stats.TryGetValue(id, out value);
			return value;
		}

		public T GetStat<T>() where T : IMyHudStat
		{
			IMyHudStat value = null;
			m_statsByType.TryGetValue(typeof(T), out value);
			return (T)value;
		}

		public void Update()
		{
			if (MySession.Static != null)
			{
				foreach (IMyHudStat value in m_stats.Values)
				{
					value.Update();
				}
			}
		}
	}
}
