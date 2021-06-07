using System;
using System.Linq.Expressions;
using System.Reflection;

namespace VRage
{
	public static class MemberHelper
	{
		/// <summary>
		/// Gets the memberinfo of field/property on static class.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="selector">The selector.</param>
		/// <returns></returns>
		public static MemberInfo GetMember<TValue>(Expression<Func<TValue>> selector)
		{
			Exceptions.ThrowIf<ArgumentNullException>(selector == null, "selector");
			MemberExpression memberExpression = selector.Body as MemberExpression;
			Exceptions.ThrowIf<ArgumentNullException>(memberExpression == null, "Selector must be a member access expression", "selector");
			return memberExpression.Member;
		}
	}
	public static class MemberHelper<T>
	{
		/// <summary>
		///  Gets the memberinfo of field/property on instance class.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="selector">The selector.</param>
		/// <returns></returns>
		public static MemberInfo GetMember<TValue>(Expression<Func<T, TValue>> selector)
		{
			Exceptions.ThrowIf<ArgumentNullException>(selector == null, "selector");
			MemberExpression memberExpression = selector.Body as MemberExpression;
			Exceptions.ThrowIf<ArgumentNullException>(memberExpression == null, "Selector must be a member access expression", "selector");
			return memberExpression.Member;
		}
	}
}
