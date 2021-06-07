using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public abstract class TypeSwitchBase<TKeyBase, TValBase> where TValBase : class
	{
		public Dictionary<Type, TValBase> Matches
		{
			get;
			private set;
		}

		protected TypeSwitchBase()
		{
			Matches = new Dictionary<Type, TValBase>();
		}

		public TypeSwitchBase<TKeyBase, TValBase> Case<TKey>(TValBase action) where TKey : class, TKeyBase
		{
			Matches.Add(typeof(TKey), action);
			return this;
		}

		protected TValBase SwitchInternal<TKey>() where TKey : class, TKeyBase
		{
			if (!Matches.TryGetValue(typeof(TKey), out TValBase value))
			{
				return null;
			}
			return value;
		}
	}
}
