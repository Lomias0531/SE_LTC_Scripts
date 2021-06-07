namespace VRage.Serialization
{
    using System;
    using System.Reflection;
    using VRage.Library.Collections;
    using VRage.Network;

    public sealed class MyMemberSerializer<TOwner, TMember> : MyMemberSerializer<TOwner>
    {
        private Getter<TOwner, TMember> m_getter;
        private Setter<TOwner, TMember> m_setter;
        private MySerializer<TMember> m_serializer;
        private MemberInfo m_memberInfo;

        public override unsafe void Clone(ref TOwner original, ref TOwner clone)
        {
            this.m_getter(ref original, out TMember local);
            this.m_serializer.Clone(ref local);
            this.m_setter(ref clone, &local);
        }

        public override bool Equals(ref TOwner a, ref TOwner b)
        {
            this.m_getter(ref a, out TMember local);
            this.m_getter(ref b, out TMember local2);
            return this.m_serializer.Equals(ref local, ref local2);
        }

        public sealed override void Init(MemberInfo memberInfo, MySerializeInfo info)
        {
            if (this.m_serializer != null)
            {
                throw new InvalidOperationException("Already initialized");
            }
            IMemberAccessor memberAccessor = CodegenUtils.GetMemberAccessor((Type) typeof(TOwner), memberInfo);
            if (memberAccessor != null)
            {
                IMemberAccessor<TOwner, TMember> accessor2 = (IMemberAccessor<TOwner, TMember>) memberAccessor;
                this.m_getter = new Getter<TOwner, TMember>(accessor2.Get);
                this.m_setter = new Setter<TOwner, TMember>(accessor2.Set);
            }
            else
            {
                this.m_getter = memberInfo.CreateGetterRef<TOwner, TMember>();
                this.m_setter = memberInfo.CreateSetterRef<TOwner, TMember>();
            }
            this.m_serializer = MyFactory.GetSerializer<TMember>();
            base.m_info = info;
            this.m_memberInfo = memberInfo;
        }

        public sealed override unsafe void Read(BitStream stream, ref TOwner obj, MySerializeInfo info)
        {
            if (MySerializationHelpers.CreateAndRead<TMember>(stream, out TMember local, this.m_serializer, info ?? base.m_info))
            {
                this.m_setter(ref obj, &local);
            }
        }

        public override string ToString() => 
            string.Format("{2} {0}.{1}", this.m_memberInfo.get_DeclaringType().Name, this.m_memberInfo.Name, this.m_memberInfo.GetMemberType().Name);

        public sealed override void Write(BitStream stream, ref TOwner obj, MySerializeInfo info)
        {
            try
            {
                this.m_getter(ref obj, out TMember local);
                MySerializationHelpers.Write<TMember>(stream, ref local, this.m_serializer, info ?? base.m_info);
            }
            catch (MySerializeException exception)
            {
                string str;
                MySerializeErrorEnum error = exception.Error;
                if (error != MySerializeErrorEnum.NullNotAllowed)
                {
                    if (error != MySerializeErrorEnum.DynamicNotAllowed)
                    {
                        goto Label_0088;
                    }
                    str = $"Error serializing {this.m_memberInfo.get_DeclaringType().Name}.{this.m_memberInfo.Name}, member contains inherited type, but it's not allowed, consider adding attribute [Serialize(MyObjectFlags.Dynamic)]";
                }
                else
                {
                    str = $"Error serializing {this.m_memberInfo.get_DeclaringType().Name}.{this.m_memberInfo.Name}, member contains null, but it's not allowed, consider adding attribute [Serialize(MyObjectFlags.Nullable)]";
                }
                goto Label_008E;
            Label_0088:
                str = "Unknown serialization error";
            Label_008E:
                throw new InvalidOperationException(str, exception);
            }
        }
    }
}

