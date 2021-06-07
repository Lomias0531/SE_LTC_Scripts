using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Medieval.ObjectBuilders
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[XmlType("Range")]
	public class MyObjectBuilder_WorldGeneratorPlayerStartingState_Range : MyObjectBuilder_WorldGeneratorPlayerStartingState
	{
		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003EMinPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in SerializableVector3 value)
			{
				owner.MinPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out SerializableVector3 value)
			{
				value = owner.MinPosition;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003EMaxPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in SerializableVector3 value)
			{
				owner.MaxPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out SerializableVector3 value)
			{
				value = owner.MaxPosition;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorPlayerStartingState*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorPlayerStartingState*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorPlayerStartingState_Range();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorPlayerStartingState_Range CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorPlayerStartingState_Range();
			}

			MyObjectBuilder_WorldGeneratorPlayerStartingState_Range IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableVector3 MinPosition;

		[ProtoMember(4)]
		public SerializableVector3 MaxPosition;
	}
}
