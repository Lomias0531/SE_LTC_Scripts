using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Animation
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationSMVariable : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMVariable, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in float value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out float value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value)
			{
				owner.SubtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value)
			{
				value = owner.SubtypeName;
			}
		}

		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMVariable>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMVariable();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationSMVariable CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMVariable();
			}

			MyObjectBuilder_AnimationSMVariable IActivator<MyObjectBuilder_AnimationSMVariable>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Description("Name of target variable.")]
		[ReadOnly(true)]
		public string Name
		{
			get;
			set;
		}

		[ProtoMember(4)]
		[Description("Float value to setup.")]
		public float Value
		{
			get;
			set;
		}

		[Browsable(false)]
		public new MyStringHash SubtypeId => base.SubtypeId;

		[Browsable(false)]
		public new string SubtypeName
		{
			get
			{
				return base.SubtypeName;
			}
			set
			{
				base.SubtypeName = value;
			}
		}

		[Browsable(false)]
		public new MyObjectBuilderType TypeId => base.TypeId;

		public override string ToString()
		{
			return Name + "=" + Value;
		}
	}
}
