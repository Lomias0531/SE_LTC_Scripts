using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CubeBuilderDefinition : MyObjectBuilder_SessionComponentDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EDefaultBlockBuildingDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in float value)
			{
				owner.DefaultBlockBuildingDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out float value)
			{
				value = owner.DefaultBlockBuildingDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EMaxBlockBuildingDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in float value)
			{
				owner.MaxBlockBuildingDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out float value)
			{
				value = owner.MaxBlockBuildingDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EMinBlockBuildingDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in float value)
			{
				owner.MinBlockBuildingDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out float value)
			{
				value = owner.MinBlockBuildingDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EBuildingDistSmallSurvivalCharacter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value)
			{
				owner.BuildingDistSmallSurvivalCharacter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value)
			{
				value = owner.BuildingDistSmallSurvivalCharacter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EBuildingDistLargeSurvivalCharacter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value)
			{
				owner.BuildingDistLargeSurvivalCharacter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value)
			{
				value = owner.BuildingDistLargeSurvivalCharacter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EBuildingDistSmallSurvivalShip_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value)
			{
				owner.BuildingDistSmallSurvivalShip = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value)
			{
				value = owner.BuildingDistSmallSurvivalShip;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EBuildingDistLargeSurvivalShip_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value)
			{
				owner.BuildingDistLargeSurvivalShip = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value)
			{
				value = owner.BuildingDistLargeSurvivalShip;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EBuildingSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, MyPlacementSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in MyPlacementSettings value)
			{
				owner.BuildingSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out MyPlacementSettings value)
			{
				value = owner.BuildingSettings;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CubeBuilderDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CubeBuilderDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CubeBuilderDefinition CreateInstance()
			{
				return new MyObjectBuilder_CubeBuilderDefinition();
			}

			MyObjectBuilder_CubeBuilderDefinition IActivator<MyObjectBuilder_CubeBuilderDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Default block building distance for creative mode.
		/// </summary>
		public float DefaultBlockBuildingDistance = 20f;

		/// <summary>
		/// Max building distance for creative mode.
		/// </summary>
		public float MaxBlockBuildingDistance = 20f;

		/// <summary>
		/// Min building distnace for creative mode.
		/// </summary>
		public float MinBlockBuildingDistance = 1f;

		/// <summary>
		/// Building distance for small grid in survival mode when controlling character.
		/// </summary>
		public double BuildingDistSmallSurvivalCharacter = 5.0;

		/// <summary>
		/// Building distance for large grid in survival mode when controlling character.
		/// </summary>
		public double BuildingDistLargeSurvivalCharacter = 10.0;

		/// <summary>
		/// Building distance for small grid in survival mode when controlling ship.
		/// </summary>
		public double BuildingDistSmallSurvivalShip = 12.5;

		/// <summary>
		/// Building distance for large grid in survival mode when controlling ship.
		/// </summary>
		public double BuildingDistLargeSurvivalShip = 12.5;

		/// <summary>
		/// Defines placement settings for building mode.
		/// </summary>
		public MyPlacementSettings BuildingSettings;
	}
}
