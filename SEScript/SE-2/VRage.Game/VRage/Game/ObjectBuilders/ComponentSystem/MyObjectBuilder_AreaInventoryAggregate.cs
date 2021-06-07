using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AreaInventoryAggregate : MyObjectBuilder_InventoryAggregate
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003ERadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in float value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out float value)
			{
				value = owner.Radius;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003EInventories_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate_003C_003EInventories_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, List<MyObjectBuilder_InventoryBase>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in List<MyObjectBuilder_InventoryBase> value)
			{
				Set(ref *(MyObjectBuilder_InventoryAggregate*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out List<MyObjectBuilder_InventoryBase> value)
			{
				Get(ref *(MyObjectBuilder_InventoryAggregate*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003EInventoryId_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryBase_003C_003EInventoryId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in string value)
			{
				Set(ref *(MyObjectBuilder_InventoryBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out string value)
			{
				Get(ref *(MyObjectBuilder_InventoryBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AreaInventoryAggregate>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AreaInventoryAggregate();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AreaInventoryAggregate CreateInstance()
			{
				return new MyObjectBuilder_AreaInventoryAggregate();
			}

			MyObjectBuilder_AreaInventoryAggregate IActivator<MyObjectBuilder_AreaInventoryAggregate>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float Radius;
	}
}
