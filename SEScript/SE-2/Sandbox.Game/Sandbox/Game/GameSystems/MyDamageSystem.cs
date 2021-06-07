using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyDamageSystem : MySessionComponentBase, IMyDamageSystem
	{
		private List<Tuple<int, Action<object, MyDamageInformation>>> m_destroyHandlers = new List<Tuple<int, Action<object, MyDamageInformation>>>();

		private List<Tuple<int, BeforeDamageApplied>> m_beforeDamageHandlers = new List<Tuple<int, BeforeDamageApplied>>();

		private List<Tuple<int, Action<object, MyDamageInformation>>> m_afterDamageHandlers = new List<Tuple<int, Action<object, MyDamageInformation>>>();

		public static MyDamageSystem Static
		{
			get;
			private set;
		}

		public bool HasAnyBeforeHandler => m_beforeDamageHandlers.Count > 0;

		public override void LoadData()
		{
			Static = this;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_destroyHandlers.Clear();
			m_beforeDamageHandlers.Clear();
			m_afterDamageHandlers.Clear();
		}

		public void RaiseDestroyed(object target, MyDamageInformation info)
		{
			foreach (Tuple<int, Action<object, MyDamageInformation>> destroyHandler in m_destroyHandlers)
			{
				destroyHandler.Item2(target, info);
			}
		}

		public void RaiseBeforeDamageApplied(object target, ref MyDamageInformation info)
		{
			if (m_beforeDamageHandlers.Count > 0)
			{
				RaiseBeforeDamageAppliedIntenal(target, ref info);
			}
		}

		private void RaiseBeforeDamageAppliedIntenal(object target, ref MyDamageInformation info)
		{
			foreach (Tuple<int, BeforeDamageApplied> beforeDamageHandler in m_beforeDamageHandlers)
			{
				beforeDamageHandler.Item2(target, ref info);
			}
		}

		public void RaiseAfterDamageApplied(object target, MyDamageInformation info)
		{
			foreach (Tuple<int, Action<object, MyDamageInformation>> afterDamageHandler in m_afterDamageHandlers)
			{
				afterDamageHandler.Item2(target, info);
			}
		}

		public void RegisterDestroyHandler(int priority, Action<object, MyDamageInformation> handler)
		{
			Tuple<int, Action<object, MyDamageInformation>> item = new Tuple<int, Action<object, MyDamageInformation>>(priority, handler);
			m_destroyHandlers.Add(item);
			m_destroyHandlers.Sort((Tuple<int, Action<object, MyDamageInformation>> x, Tuple<int, Action<object, MyDamageInformation>> y) => x.Item1 - y.Item1);
		}

		public void RegisterBeforeDamageHandler(int priority, BeforeDamageApplied handler)
		{
			Tuple<int, BeforeDamageApplied> item = new Tuple<int, BeforeDamageApplied>(priority, handler);
			m_beforeDamageHandlers.Add(item);
			m_beforeDamageHandlers.Sort((Tuple<int, BeforeDamageApplied> x, Tuple<int, BeforeDamageApplied> y) => x.Item1 - y.Item1);
		}

		public void RegisterAfterDamageHandler(int priority, Action<object, MyDamageInformation> handler)
		{
			Tuple<int, Action<object, MyDamageInformation>> item = new Tuple<int, Action<object, MyDamageInformation>>(priority, handler);
			m_afterDamageHandlers.Add(item);
			m_afterDamageHandlers.Sort((Tuple<int, Action<object, MyDamageInformation>> x, Tuple<int, Action<object, MyDamageInformation>> y) => x.Item1 - y.Item1);
		}
	}
}
