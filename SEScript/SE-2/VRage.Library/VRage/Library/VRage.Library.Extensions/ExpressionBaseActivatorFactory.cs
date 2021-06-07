using System;
using System.Linq.Expressions;

namespace VRage.Library.Extensions
{
	/// <summary>
	/// Activator factory that uses expression trees.
	/// </summary>
	/// <remarks>This is the default activator factory.</remarks>
	public sealed class ExpressionBaseActivatorFactory : IActivatorFactory
	{
		/// <inheritdoc />
		public Func<T> CreateActivator<T>()
		{
			return CreateActivator<T>(typeof(T));
		}

		/// <inheritdoc />
		public Func<T> CreateActivator<T>(Type subtype)
		{
			return Expression.Lambda<Func<T>>(Expression.New(subtype), Array.Empty<ParameterExpression>()).Compile();
		}
	}
}
