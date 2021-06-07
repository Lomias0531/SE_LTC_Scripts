using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[Serializable]
	[ProtoContract]
	[XmlType("ContractChance")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyContractChancePair
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyContractChancePair_003C_003EDefinitionId_003C_003EAccessor : IMemberAccessor<MyContractChancePair, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContractChancePair owner, in SerializableDefinitionId value)
			{
				owner.DefinitionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContractChancePair owner, out SerializableDefinitionId value)
			{
				value = owner.DefinitionId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyContractChancePair_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyContractChancePair, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContractChancePair owner, in float value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContractChancePair owner, out float value)
			{
				value = owner.Value;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyContractChancePair_003C_003EActor : IActivator, IActivator<MyContractChancePair>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractChancePair();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractChancePair CreateInstance()
			{
				return new MyContractChancePair();
			}

			MyContractChancePair IActivator<MyContractChancePair>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId DefinitionId;

		[ProtoMember(3)]
		[DefaultValue(0f)]
		public float Value;
	}
}
