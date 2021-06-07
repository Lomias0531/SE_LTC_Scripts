using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
	public static class MethodInfoExtensions
	{
		public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object instance) where TDelegate : Delegate
		{
			return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, method);
		}

		public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method) where TDelegate : Delegate
		{
			return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), method);
		}

		public static ParameterExpression[] ExtractParameterExpressionsFrom<TDelegate>()
		{
			return (from s in typeof(TDelegate).GetMethod("Invoke").GetParameters()
				select Expression.Parameter(s.ParameterType)).ToArray();
		}
	}
}
