using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SessionComponentContainerDropSystem : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EPlayerData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, List<PlayerContainerData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in List<PlayerContainerData> value)
			{
				owner.PlayerData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out List<PlayerContainerData> value)
			{
				value = owner.PlayerData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EGPSForRemoval_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, List<MyContainerGPS>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in List<MyContainerGPS> value)
			{
				owner.GPSForRemoval = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out List<MyContainerGPS> value)
			{
				value = owner.GPSForRemoval;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EEntitiesForRemoval_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, List<MyEntityForRemoval>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in List<MyEntityForRemoval> value)
			{
				owner.EntitiesForRemoval = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out List<MyEntityForRemoval> value)
			{
				value = owner.EntitiesForRemoval;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EContainerIdSmall_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in uint value)
			{
				owner.ContainerIdSmall = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out uint value)
			{
				value = owner.ContainerIdSmall;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EContainerIdLarge_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in uint value)
			{
				owner.ContainerIdLarge = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out uint value)
			{
				value = owner.ContainerIdLarge;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContainerDropSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContainerDropSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContainerDropSystem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentContainerDropSystem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentContainerDropSystem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SessionComponentContainerDropSystem CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentContainerDropSystem();
			}

			MyObjectBuilder_SessionComponentContainerDropSystem IActivator<MyObjectBuilder_SessionComponentContainerDropSystem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(28)]
		public List<PlayerContainerData> PlayerData;

		[ProtoMember(31)]
		public List<MyContainerGPS> GPSForRemoval;

		[ProtoMember(34)]
		public List<MyEntityForRemoval> EntitiesForRemoval;

		[ProtoMember(37)]
		public uint ContainerIdSmall = 1u;

		[ProtoMember(40)]
		public uint ContainerIdLarge = 1u;

		public MyObjectBuilder_SessionComponentContainerDropSystem()
		{
			PlayerData = new List<PlayerContainerData>();
			GPSForRemoval = new List<MyContainerGPS>();
			EntitiesForRemoval = new List<MyEntityForRemoval>();
		}
	}
}
