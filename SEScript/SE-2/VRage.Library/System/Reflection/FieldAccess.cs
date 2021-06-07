using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Reflection
{
	public static class FieldAccess
	{
		private struct FieldKey
		{
			private sealed class ReflectedTypeFieldEqualityComparer : IEqualityComparer<FieldKey>
			{
				public bool Equals(FieldKey x, FieldKey y)
				{
					if (object.Equals(x.ReflectedType, y.ReflectedType))
					{
						return object.Equals(x.Field, y.Field);
					}
					return false;
				}

				public int GetHashCode(FieldKey obj)
				{
					return (((obj.ReflectedType != null) ? obj.ReflectedType.GetHashCode() : 0) * 397) ^ ((obj.Field != null) ? obj.Field.GetHashCode() : 0);
				}
			}

			public readonly Type ReflectedType;

			public readonly FieldInfo Field;

			public static IEqualityComparer<FieldKey> Comparer
			{
				get;
			} = new ReflectedTypeFieldEqualityComparer();


			public FieldKey(Type reflectedType, FieldInfo field)
			{
				ReflectedType = reflectedType;
				Field = field;
			}
		}

		private class FieldHelper
		{
		}

		private class FieldHelper<TType, TMember> : FieldHelper
		{
			private readonly FieldInfo m_info;

			public readonly Func<TType, TMember> Getter;

			public readonly Action<TType, TMember> Setter;

			public readonly Getter<TType, TMember> GetterRef;

			public readonly Setter<TType, TMember> SetterRef;

			/// <inheritdoc />
			public FieldHelper(FieldInfo info)
			{
				m_info = info;
				Getter = Get;
				Setter = Set;
				GetterRef = Get;
				SetterRef = Set;
			}

			private TMember Get(TType instance)
			{
				return (TMember)m_info.GetValue(instance);
			}

			private void Get(ref TType instance, out TMember memberValue)
			{
				memberValue = (TMember)m_info.GetValue(instance);
			}

			private void Set(TType instance, TMember memberValue)
			{
				m_info.SetValue(instance, memberValue);
			}

			private void Set(ref TType instance, in TMember memberValue)
			{
				object obj = instance;
				m_info.SetValue(obj, memberValue);
				instance = (TType)obj;
			}
		}

		private static readonly ConcurrentDictionary<FieldKey, FieldHelper> Helpers = new ConcurrentDictionary<FieldKey, FieldHelper>();

		private static FieldHelper<TInstance, TMember> GetHelper<TInstance, TMember>(FieldInfo info)
		{
			FieldKey key = new FieldKey(typeof(TInstance), info);
			if (!Helpers.TryGetValue(key, out FieldHelper value))
			{
				value = new FieldHelper<TInstance, TMember>(info);
				Helpers.TryAdd(key, value);
			}
			return (FieldHelper<TInstance, TMember>)value;
		}

		public static Func<TType, TMember> CreateGetter<TType, TMember>(this FieldInfo field)
		{
			return GetHelper<TType, TMember>(field).Getter;
		}

		public static Action<TType, TMember> CreateSetter<TType, TMember>(this FieldInfo field)
		{
			return GetHelper<TType, TMember>(field).Setter;
		}

		public static Getter<TType, TMember> CreateGetterRef<TType, TMember>(this FieldInfo field)
		{
			return GetHelper<TType, TMember>(field).GetterRef;
		}

		public static Setter<TType, TMember> CreateSetterRef<TType, TMember>(this FieldInfo field)
		{
			return GetHelper<TType, TMember>(field).SetterRef;
		}
	}
}
