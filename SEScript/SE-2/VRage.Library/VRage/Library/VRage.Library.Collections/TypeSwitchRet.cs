using System;

namespace VRage.Library.Collections
{
	public sealed class TypeSwitchRet<TKeyBase, TRetBase> : TypeSwitchBase<TKeyBase, Func<TRetBase>>
	{
		public TRet Switch<TKey, TRet>() where TKey : class, TKeyBase where TRet : class, TRetBase
		{
			Func<TRetBase> func = SwitchInternal<TKey>();
			if (func != null)
			{
				return (TRet)(object)func();
			}
			return null;
		}
	}
}
