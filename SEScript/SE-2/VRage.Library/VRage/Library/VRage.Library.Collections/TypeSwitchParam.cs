using System;

namespace VRage.Library.Collections
{
	public sealed class TypeSwitchParam<TKeyBase, TParam> : TypeSwitchBase<TKeyBase, Func<TParam, TKeyBase>>
	{
		public TRet Switch<TRet>(TParam par) where TRet : class, TKeyBase
		{
			Func<TParam, TKeyBase> func = SwitchInternal<TRet>();
			if (func != null)
			{
				return (TRet)(object)func(par);
			}
			return null;
		}
	}
	public sealed class TypeSwitchParam<TKeyBase, TParam1, TParam2> : TypeSwitchBase<TKeyBase, Func<TParam1, TParam2, TKeyBase>>
	{
		public TRet Switch<TRet>(TParam1 par1, TParam2 par2) where TRet : class, TKeyBase
		{
			Func<TParam1, TParam2, TKeyBase> func = SwitchInternal<TRet>();
			if (func != null)
			{
				return (TRet)(object)func(par1, par2);
			}
			return null;
		}
	}
}
