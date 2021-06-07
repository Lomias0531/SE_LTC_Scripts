using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("RespawnShip")]
	public class MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip : MyObjectBuilder_WorldGeneratorPlayerStartingState
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003EDampenersEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in bool value)
			{
				owner.DampenersEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out bool value)
			{
				value = owner.DampenersEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003ERespawnShip_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in string value)
			{
				owner.RespawnShip = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out string value)
			{
				value = owner.RespawnShip;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorPlayerStartingState*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorPlayerStartingState*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip();
			}

			MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(115)]
		[XmlAttribute]
		public bool DampenersEnabled;

		[ProtoMember(118)]
		[XmlAttribute]
		public string RespawnShip;
	}
}
