using System.Linq.Expressions;

namespace System.Reflection
{
	public static class PropertyAccess
	{
		public static Func<T, TProperty> CreateGetter<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "value");
			Expression expression = (propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType));
			Expression expression2 = Expression.Property(expression, propertyInfo);
			if (typeFromHandle2 != propertyInfo.PropertyType)
			{
				expression2 = Expression.Convert(expression2, typeFromHandle2);
			}
			return Expression.Lambda<Func<T, TProperty>>(expression2, new ParameterExpression[1]
			{
				parameterExpression
			}).Compile();
		}

		public static Action<T, TProperty> CreateSetter<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle);
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2);
			Expression expression = (propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType));
			Expression right = (propertyInfo.PropertyType == typeFromHandle2) ? ((Expression)parameterExpression2) : ((Expression)Expression.Convert(parameterExpression2, propertyInfo.PropertyType));
			MemberExpression left = Expression.Property(expression, propertyInfo);
			return Expression.Lambda<Action<T, TProperty>>(Expression.Assign(left, right), new ParameterExpression[2]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		public static Getter<T, TProperty> CreateGetterRef<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle.MakeByRefType(), "value");
			Expression expression = (propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType));
			Expression expression2 = Expression.Property(expression, propertyInfo);
			if (typeFromHandle2 != propertyInfo.PropertyType)
			{
				expression2 = Expression.Convert(expression2, typeFromHandle2);
			}
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2.MakeByRefType(), "out");
			BinaryExpression body = Expression.Assign(parameterExpression2, expression2);
			return Expression.Lambda<Getter<T, TProperty>>(body, new ParameterExpression[2]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		public static Setter<T, TProperty> CreateSetterRef<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle.MakeByRefType());
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2.MakeByRefType());
			Expression expression = (propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType));
			Expression right = (propertyInfo.PropertyType == typeFromHandle2) ? ((Expression)parameterExpression2) : ((Expression)Expression.Convert(parameterExpression2, propertyInfo.PropertyType));
			MemberExpression left = Expression.Property(expression, propertyInfo);
			return Expression.Lambda<Setter<T, TProperty>>(Expression.Assign(left, right), new ParameterExpression[2]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}
	}
}
