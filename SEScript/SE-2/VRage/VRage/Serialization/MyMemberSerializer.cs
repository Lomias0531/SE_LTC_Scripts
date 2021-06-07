using System;
using System.Reflection;
using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
	public abstract class MyMemberSerializer
	{
		protected MySerializeInfo m_info;

		public MySerializeInfo Info => m_info;

		public abstract void Init(MemberInfo memberInfo, MySerializeInfo info);

		/// <summary>
		/// Makes clone of object member.
		/// </summary>
		public abstract void Clone(object original, object clone);

		/// <summary>
		/// Tests equality of object members.
		/// </summary>
		public new abstract bool Equals(object a, object b);

		public abstract void Read(BitStream stream, object obj, MySerializeInfo info);

		public abstract void Write(BitStream stream, object obj, MySerializeInfo info);
	}
	public abstract class MyMemberSerializer<TOwner> : MyMemberSerializer
	{
		/// <summary>
		/// Makes clone of object member.
		/// </summary>
		public abstract void Clone(ref TOwner original, ref TOwner clone);

		/// <summary>
		/// Tests equality of object members.
		/// </summary>
		public abstract bool Equals(ref TOwner a, ref TOwner b);

		public abstract void Read(BitStream stream, ref TOwner obj, MySerializeInfo info);

		public abstract void Write(BitStream stream, ref TOwner obj, MySerializeInfo info);

		public sealed override void Clone(object original, object clone)
		{
			TOwner val = (TOwner)original;
			TOwner val2 = (TOwner)clone;
			Clone(val, val2);
		}

		public sealed override bool Equals(object a, object b)
		{
			TOwner a2 = (TOwner)a;
			TOwner b2 = (TOwner)b;
			return Equals(ref a2, ref b2);
		}

		public sealed override void Read(BitStream stream, object obj, MySerializeInfo info)
		{
			TOwner obj2 = (TOwner)obj;
			Read(stream, ref obj2, info);
		}

		public sealed override void Write(BitStream stream, object obj, MySerializeInfo info)
		{
			TOwner obj2 = (TOwner)obj;
			Write(stream, ref obj2, info);
		}
	}
	public sealed class MyMemberSerializer<TOwner, TMember> : MyMemberSerializer<TOwner>
	{
		private Getter<TOwner, TMember> m_getter;

		private Setter<TOwner, TMember> m_setter;

		private MySerializer<TMember> m_serializer;

		private MemberInfo m_memberInfo;

		public sealed override void Init(MemberInfo memberInfo, MySerializeInfo info)
		{
			if (m_serializer != null)
			{
				throw new InvalidOperationException("Already initialized");
			}
			IMemberAccessor memberAccessor;
			if ((memberAccessor = CodegenUtils.GetMemberAccessor(typeof(TOwner), memberInfo)) != null)
			{
				IMemberAccessor<TOwner, TMember> memberAccessor2 = (IMemberAccessor<TOwner, TMember>)memberAccessor;
				m_getter = memberAccessor2.Get;
				m_setter = memberAccessor2.Set;
			}
			else
			{
				m_getter = memberInfo.CreateGetterRef<TOwner, TMember>();
				m_setter = memberInfo.CreateSetterRef<TOwner, TMember>();
			}
			m_serializer = MyFactory.GetSerializer<TMember>();
			m_info = info;
			m_memberInfo = memberInfo;
		}

		public override string ToString()
		{
			return string.Format("{2} {0}.{1}", m_memberInfo.DeclaringType.Name, m_memberInfo.Name, m_memberInfo.GetMemberType().Name);
		}

		public override void Clone(ref TOwner original, ref TOwner clone)
		{
			m_getter(ref original, out TMember value);
			m_serializer.Clone(ref value);
			m_setter(ref clone, in value);
		}

		public override bool Equals(ref TOwner a, ref TOwner b)
		{
			m_getter(ref a, out TMember value);
			m_getter(ref b, out TMember value2);
			return m_serializer.Equals(ref value, ref value2);
		}

		public sealed override void Read(BitStream stream, ref TOwner obj, MySerializeInfo info)
		{
			if (MySerializationHelpers.CreateAndRead(stream, out TMember result, m_serializer, info ?? m_info))
			{
				m_setter(ref obj, in result);
			}
		}

		public sealed override void Write(BitStream stream, ref TOwner obj, MySerializeInfo info)
		{
			try
			{
				m_getter(ref obj, out TMember value);
				MySerializationHelpers.Write(stream, ref value, m_serializer, info ?? m_info);
			}
			catch (MySerializeException ex)
			{
				string message;
				switch (ex.Error)
				{
				case MySerializeErrorEnum.DynamicNotAllowed:
					message = $"Error serializing {m_memberInfo.DeclaringType.Name}.{m_memberInfo.Name}, member contains inherited type, but it's not allowed, consider adding attribute [Serialize(MyObjectFlags.Dynamic)]";
					break;
				case MySerializeErrorEnum.NullNotAllowed:
					message = $"Error serializing {m_memberInfo.DeclaringType.Name}.{m_memberInfo.Name}, member contains null, but it's not allowed, consider adding attribute [Serialize(MyObjectFlags.Nullable)]";
					break;
				default:
					message = "Unknown serialization error";
					break;
				}
				throw new InvalidOperationException(message, ex);
			}
		}
	}
}
