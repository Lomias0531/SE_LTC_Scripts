using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("Transform")]
	public class MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform : MyObjectBuilder_WorldGeneratorPlayerStartingState
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003ETransform_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in MyPositionAndOrientation? value)
			{
				owner.Transform = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out MyPositionAndOrientation? value)
			{
				value = owner.Transform;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003EJetpackEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in bool value)
			{
				owner.JetpackEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out bool value)
			{
				value = owner.JetpackEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003EDampenersEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in bool value)
			{
				owner.DampenersEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out bool value)
			{
				value = owner.DampenersEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorPlayerStartingState*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorPlayerStartingState*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform();
			}

			MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(106)]
		public MyPositionAndOrientation? Transform;

		[ProtoMember(109)]
		[XmlAttribute]
		public bool JetpackEnabled;

		[ProtoMember(112)]
		[XmlAttribute]
		public bool DampenersEnabled;

		public bool ShouldSerializeTransform()
		{
			return Transform.HasValue;
		}
	}
}
