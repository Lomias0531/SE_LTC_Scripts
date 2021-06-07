using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Trading
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TradingManager : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_TradingManager_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TradingManager, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TradingManager owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TradingManager owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_TradingManager_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TradingManager, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TradingManager owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TradingManager owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_TradingManager_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TradingManager, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TradingManager owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TradingManager owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_TradingManager_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TradingManager, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TradingManager owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TradingManager owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_TradingManager_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TradingManager, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TradingManager owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TradingManager owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_TradingManager_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TradingManager>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TradingManager();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TradingManager CreateInstance()
			{
				return new MyObjectBuilder_TradingManager();
			}

			MyObjectBuilder_TradingManager IActivator<MyObjectBuilder_TradingManager>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
