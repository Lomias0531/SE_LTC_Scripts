namespace System.Reflection
{
	public static class MemberAccess
	{
		public static bool IsMemberPublic(this MemberInfo memberInfo)
		{
			switch (memberInfo.MemberType)
			{
			case MemberTypes.Field:
				return (((FieldInfo)memberInfo).Attributes & FieldAttributes.Public) == FieldAttributes.Public;
			case MemberTypes.Property:
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				MethodInfo setMethod = propertyInfo.GetSetMethod();
				if (getMethod != null && setMethod != null && (getMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
				{
					return (setMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public;
				}
				return false;
			}
			default:
				throw new NotImplementedException();
			}
		}

		public static Type GetMemberType(this MemberInfo memberInfo)
		{
			if (memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)memberInfo).PropertyType;
			}
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).FieldType;
			}
			if (memberInfo is MethodInfo)
			{
				return ((MethodInfo)memberInfo).ReturnType;
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo or MethodInfo");
		}

		public static object GetValue(this MemberInfo memberInfo, object forObject)
		{
			switch (memberInfo.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)memberInfo).GetValue(forObject);
			case MemberTypes.Property:
				return ((PropertyInfo)memberInfo).GetValue(forObject);
			default:
				throw new NotImplementedException();
			}
		}

		public static void SetValue(this MemberInfo memberInfo, object forObject, object value)
		{
			switch (memberInfo.MemberType)
			{
			case MemberTypes.Field:
				((FieldInfo)memberInfo).SetValue(forObject, value);
				break;
			case MemberTypes.Property:
				((PropertyInfo)memberInfo).SetValue(forObject, value);
				break;
			default:
				throw new NotImplementedException();
			}
		}

		public static Func<T, TMember> CreateGetter<T, TMember>(this MemberInfo memberInfo)
		{
			if (memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)memberInfo).CreateGetter<T, TMember>();
			}
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).CreateGetter<T, TMember>();
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
		}

		public static Action<T, TMember> CreateSetter<T, TMember>(this MemberInfo memberInfo)
		{
			if (memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)memberInfo).CreateSetter<T, TMember>();
			}
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).CreateSetter<T, TMember>();
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
		}

		public static Getter<T, TMember> CreateGetterRef<T, TMember>(this MemberInfo memberInfo)
		{
			if (memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)memberInfo).CreateGetterRef<T, TMember>();
			}
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).CreateGetterRef<T, TMember>();
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
		}

		public static Setter<T, TMember> CreateSetterRef<T, TMember>(this MemberInfo memberInfo)
		{
			if (memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)memberInfo).CreateSetterRef<T, TMember>();
			}
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).CreateSetterRef<T, TMember>();
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
		}

		public static bool CheckGetterSignature<T, TMember>(this MemberInfo memberInfo)
		{
			if (!typeof(T).IsAssignableFrom(memberInfo.DeclaringType))
			{
				return false;
			}
			if (memberInfo is PropertyInfo)
			{
				return typeof(TMember).IsAssignableFrom(((PropertyInfo)memberInfo).PropertyType);
			}
			if (memberInfo is FieldInfo)
			{
				return typeof(TMember).IsAssignableFrom(((FieldInfo)memberInfo).FieldType);
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
		}

		public static bool CheckSetterSignature<T, TMember>(this MemberInfo memberInfo)
		{
			if (!typeof(T).IsAssignableFrom(memberInfo.DeclaringType))
			{
				return false;
			}
			if (memberInfo is PropertyInfo)
			{
				return typeof(TMember).IsAssignableFrom(((PropertyInfo)memberInfo).PropertyType);
			}
			if (memberInfo is FieldInfo)
			{
				return typeof(TMember).IsAssignableFrom(((FieldInfo)memberInfo).FieldType);
			}
			throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
		}
	}
}
