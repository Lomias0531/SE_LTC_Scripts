using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_DroneBehaviorDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EStrafeWidth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.StrafeWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.StrafeWidth;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EStrafeHeight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.StrafeHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.StrafeHeight;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EStrafeDepth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.StrafeDepth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.StrafeDepth;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EMinStrafeDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.MinStrafeDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.MinStrafeDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EAvoidCollisions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.AvoidCollisions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.AvoidCollisions;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ERotateToPlayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.RotateToPlayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.RotateToPlayer;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EUseStaticWeaponry_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.UseStaticWeaponry = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.UseStaticWeaponry;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EUseTools_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.UseTools = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.UseTools;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EUseRammingBehavior_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.UseRammingBehavior = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.UseRammingBehavior;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ECanBeDisabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.CanBeDisabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.CanBeDisabled;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EUsePlanetHover_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.UsePlanetHover = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.UsePlanetHover;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EAlternativeBehavior_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				owner.AlternativeBehavior = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				value = owner.AlternativeBehavior;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EPlanetHoverMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.PlanetHoverMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.PlanetHoverMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EPlanetHoverMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.PlanetHoverMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.PlanetHoverMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ESpeedLimit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.SpeedLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.SpeedLimit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EPlayerYAxisOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.PlayerYAxisOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.PlayerYAxisOffset;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ETargetDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.TargetDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.TargetDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EMaxManeuverDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.MaxManeuverDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.MaxManeuverDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EStaticWeaponryUsage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.StaticWeaponryUsage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.StaticWeaponryUsage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ERammingBehaviorDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.RammingBehaviorDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.RammingBehaviorDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EToolsUsage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.ToolsUsage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.ToolsUsage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EWaypointDelayMsMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value)
			{
				owner.WaypointDelayMsMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value)
			{
				value = owner.WaypointDelayMsMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EWaypointDelayMsMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value)
			{
				owner.WaypointDelayMsMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value)
			{
				value = owner.WaypointDelayMsMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EWaypointMaxTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value)
			{
				owner.WaypointMaxTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value)
			{
				value = owner.WaypointMaxTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EWaypointThresholdDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.WaypointThresholdDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.WaypointThresholdDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ELostTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in int value)
			{
				owner.LostTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out int value)
			{
				value = owner.LostTimeMs;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EUsesWeaponBehaviors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				owner.UsesWeaponBehaviors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				value = owner.UsesWeaponBehaviors;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EWeaponBehaviorNotFoundDelay_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in float value)
			{
				owner.WeaponBehaviorNotFoundDelay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out float value)
			{
				value = owner.WeaponBehaviorNotFoundDelay;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ESoundLoop_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				owner.SoundLoop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				value = owner.SoundLoop;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EWeaponBehaviors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, List<MyWeaponBehavior>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in List<MyWeaponBehavior> value)
			{
				owner.WeaponBehaviors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out List<MyWeaponBehavior> value)
			{
				value = owner.WeaponBehaviors;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DroneBehaviorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_DroneBehaviorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_DroneBehaviorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DroneBehaviorDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DroneBehaviorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DroneBehaviorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DroneBehaviorDefinition CreateInstance()
			{
				return new MyObjectBuilder_DroneBehaviorDefinition();
			}

			MyObjectBuilder_DroneBehaviorDefinition IActivator<MyObjectBuilder_DroneBehaviorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float StrafeWidth = 10f;

		public float StrafeHeight = 10f;

		public float StrafeDepth = 5f;

		public float MinStrafeDistance = 2f;

		public bool AvoidCollisions = true;

		public bool RotateToPlayer = true;

		public bool UseStaticWeaponry = true;

		public bool UseTools = true;

		public bool UseRammingBehavior;

		public bool CanBeDisabled = true;

		public bool UsePlanetHover;

		public string AlternativeBehavior = "";

		public float PlanetHoverMin = 2f;

		public float PlanetHoverMax = 25f;

		public float SpeedLimit = 50f;

		public float PlayerYAxisOffset = 0.9f;

		public float TargetDistance = 200f;

		public float MaxManeuverDistance = 250f;

		public float StaticWeaponryUsage = 300f;

		public float RammingBehaviorDistance = 75f;

		public float ToolsUsage = 8f;

		public int WaypointDelayMsMin = 1000;

		public int WaypointDelayMsMax = 3000;

		public int WaypointMaxTime = 15000;

		public float WaypointThresholdDistance = 0.5f;

		public int LostTimeMs = 20000;

		public bool UsesWeaponBehaviors;

		public float WeaponBehaviorNotFoundDelay = 3f;

		public string SoundLoop = "";

		[XmlArrayItem("WeaponBehavior")]
		public List<MyWeaponBehavior> WeaponBehaviors;
	}
}
