using System;

namespace VRage.Library.Collections
{
	public sealed class TypeSwitch<TKeyBase> : TypeSwitchBase<TKeyBase, Func<TKeyBase>>
	{
		public TRet Switch<TRet>() where TRet : class, TKeyBase
		{
			Func<TKeyBase> func = SwitchInternal<TRet>();
			if (func != null)
			{
				return (TRet)(object)func();
			}
			return null;
		}
	}
}
