using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_TriggerBlockDestroyed : MyObjectBuilder_Trigger
	{
		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003EBlockIds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in List<long> value)
			{
				owner.BlockIds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out List<long> value)
			{
				value = owner.BlockIds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003ESingleMessage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value)
			{
				owner.SingleMessage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value)
			{
				value = owner.SingleMessage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003EIsTrue_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EIsTrue_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003EMessage_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EMessage_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003EWwwLink_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003EWwwLink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003ENextMission_003C_003EAccessor : VRage_Game_MyObjectBuilder_Trigger_003C_003ENextMission_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Trigger*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Trigger*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TriggerBlockDestroyed>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TriggerBlockDestroyed();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TriggerBlockDestroyed CreateInstance()
			{
				return new MyObjectBuilder_TriggerBlockDestroyed();
			}

			MyObjectBuilder_TriggerBlockDestroyed IActivator<MyObjectBuilder_TriggerBlockDestroyed>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<long> BlockIds;

		[ProtoMember(4)]
		public string SingleMessage;
	}
}
