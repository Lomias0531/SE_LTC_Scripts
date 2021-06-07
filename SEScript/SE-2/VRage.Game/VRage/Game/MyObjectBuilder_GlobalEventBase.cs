using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_GlobalEventBase : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003EDefinitionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in SerializableDefinitionId? value)
			{
				owner.DefinitionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out SerializableDefinitionId? value)
			{
				value = owner.DefinitionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in bool value)
			{
				owner.Enabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out bool value)
			{
				value = owner.Enabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003EActivationTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in long value)
			{
				owner.ActivationTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out long value)
			{
				value = owner.ActivationTimeMs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003EEventType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, MyGlobalEventTypeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in MyGlobalEventTypeEnum value)
			{
				owner.EventType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out MyGlobalEventTypeEnum value)
			{
				value = owner.EventType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GlobalEventBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GlobalEventBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GlobalEventBase_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GlobalEventBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GlobalEventBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GlobalEventBase CreateInstance()
			{
				return new MyObjectBuilder_GlobalEventBase();
			}

			MyObjectBuilder_GlobalEventBase IActivator<MyObjectBuilder_GlobalEventBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId? DefinitionId;

		[ProtoMember(4)]
		public bool Enabled;

		[ProtoMember(7)]
		public long ActivationTimeMs;

		[ProtoMember(10)]
		public MyGlobalEventTypeEnum EventType;

		public bool ShouldSerializeDefinitionId()
		{
			return false;
		}

		public bool ShouldSerializeEventType()
		{
			return false;
		}
	}
}
