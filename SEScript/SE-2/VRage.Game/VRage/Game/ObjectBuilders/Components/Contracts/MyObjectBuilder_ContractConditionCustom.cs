using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ContractConditionCustom : MyObjectBuilder_ContractCondition
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EIsFinished_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EIsFinished_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EContractId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EContractId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003ESubId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003ESubId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EStationEndId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EStationEndId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EFactionEndId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EFactionEndId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EBlockEndId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EBlockEndId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionCustom, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionCustom owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionCustom owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionCustom_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractConditionCustom>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractConditionCustom();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractConditionCustom CreateInstance()
			{
				return new MyObjectBuilder_ContractConditionCustom();
			}

			MyObjectBuilder_ContractConditionCustom IActivator<MyObjectBuilder_ContractConditionCustom>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
