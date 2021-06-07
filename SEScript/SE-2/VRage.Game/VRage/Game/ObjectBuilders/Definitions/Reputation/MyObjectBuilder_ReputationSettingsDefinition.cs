using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions.Reputation
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ReputationSettingsDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public struct MyReputationDamageSettings
		{
			protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EMyReputationDamageSettings_003C_003EGrindingWelding_003C_003EAccessor : IMemberAccessor<MyReputationDamageSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationDamageSettings owner, in int value)
				{
					owner.GrindingWelding = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationDamageSettings owner, out int value)
				{
					value = owner.GrindingWelding;
				}
			}

			protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EMyReputationDamageSettings_003C_003EDamaging_003C_003EAccessor : IMemberAccessor<MyReputationDamageSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationDamageSettings owner, in int value)
				{
					owner.Damaging = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationDamageSettings owner, out int value)
				{
					value = owner.Damaging;
				}
			}

			protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EMyReputationDamageSettings_003C_003EStealing_003C_003EAccessor : IMemberAccessor<MyReputationDamageSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationDamageSettings owner, in int value)
				{
					owner.Stealing = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationDamageSettings owner, out int value)
				{
					value = owner.Stealing;
				}
			}

			protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EMyReputationDamageSettings_003C_003EKilling_003C_003EAccessor : IMemberAccessor<MyReputationDamageSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationDamageSettings owner, in int value)
				{
					owner.Killing = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationDamageSettings owner, out int value)
				{
					value = owner.Killing;
				}
			}

			private class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EMyReputationDamageSettings_003C_003EActor : IActivator, IActivator<MyReputationDamageSettings>
			{
				private sealed override object CreateInstance()
				{
					return default(MyReputationDamageSettings);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyReputationDamageSettings CreateInstance()
				{
					return (MyReputationDamageSettings)(object)default(MyReputationDamageSettings);
				}

				MyReputationDamageSettings IActivator<MyReputationDamageSettings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public int GrindingWelding;

			[ProtoMember(3)]
			public int Damaging;

			[ProtoMember(5)]
			public int Stealing;

			[ProtoMember(7)]
			public int Killing;
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EDamageSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyReputationDamageSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in MyReputationDamageSettings value)
			{
				owner.DamageSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out MyReputationDamageSettings value)
			{
				value = owner.DamageSettings;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EPirateDamageSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyReputationDamageSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in MyReputationDamageSettings value)
			{
				owner.PirateDamageSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out MyReputationDamageSettings value)
			{
				value = owner.PirateDamageSettings;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EMaxReputationGainInTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in int value)
			{
				owner.MaxReputationGainInTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out int value)
			{
				value = owner.MaxReputationGainInTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EResetTimeMinForRepGain_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in int value)
			{
				owner.ResetTimeMinForRepGain = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out int value)
			{
				value = owner.ResetTimeMinForRepGain;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReputationSettingsDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReputationSettingsDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ReputationSettingsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ReputationSettingsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ReputationSettingsDefinition CreateInstance()
			{
				return new MyObjectBuilder_ReputationSettingsDefinition();
			}

			MyObjectBuilder_ReputationSettingsDefinition IActivator<MyObjectBuilder_ReputationSettingsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyReputationDamageSettings DamageSettings;

		[ProtoMember(3)]
		public MyReputationDamageSettings PirateDamageSettings;

		[ProtoMember(5)]
		public int MaxReputationGainInTime;

		[ProtoMember(7)]
		public int ResetTimeMinForRepGain;
	}
}
