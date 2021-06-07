using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EngineerToolBaseDefinition : MyObjectBuilder_HandItemDefinition
	{
		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ESpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				owner.SpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				value = owner.SpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EDistanceMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				owner.DistanceMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				value = owner.DistanceMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EFlare_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				owner.Flare = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				value = owner.Flare;
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELeftHandOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELeftHandOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELeftHandPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELeftHandPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ERightHandOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ERightHandOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ERightHandPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ERightHandPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemWalkingOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemWalkingPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemShootOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemShootPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemIronsightOrientation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemIronsightOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemIronsightPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemIronsightPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemOrientation3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemOrientation3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPosition3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPosition3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemWalkingOrientation3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingOrientation3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemWalkingPosition3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingPosition3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemShootOrientation3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootOrientation3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Quaternion value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Quaternion value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemShootPosition3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootPosition3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EBlendTime_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EBlendTime_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EShootBlend_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShootBlend_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EXAmplitudeOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EXAmplitudeOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EYAmplitudeOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EYAmplitudeOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EZAmplitudeOffset_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EZAmplitudeOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EXAmplitudeScale_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EXAmplitudeScale_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EYAmplitudeScale_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EYAmplitudeScale_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EZAmplitudeScale_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EZAmplitudeScale_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ERunMultiplier_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ERunMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EAmplitudeMultiplier3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EAmplitudeMultiplier3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ESimulateLeftHand_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateLeftHand_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ESimulateRightHand_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateRightHand_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ESimulateLeftHandFps_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateLeftHandFps_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool? value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool? value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ESimulateRightHandFps_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateRightHandFps_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool? value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool? value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EFingersAnimation_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EFingersAnimation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EMuzzlePosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EMuzzlePosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EShootScatter_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShootScatter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EScatterSpeed_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EScatterSpeed_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EPhysicalItemId_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EPhysicalItemId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightFalloff_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightFalloff_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightRadius_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightRadius_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightGlareSize_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightGlareSize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightGlareIntensity_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightGlareIntensity_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightIntensityLower_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightIntensityLower_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ELightIntensityUpper_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightIntensityUpper_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EShakeAmountTarget_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShakeAmountTarget_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EShakeAmountNoTarget_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShakeAmountNoTarget_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EToolSounds_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EToolSounds_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, List<ToolSound>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in List<ToolSound> value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out List<ToolSound> value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EToolMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EToolMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioning_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioning_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioning3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioning3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioningWalk_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningWalk_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioningWalk3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningWalk3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioningShoot_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningShoot_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioningShoot3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningShoot3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioningIronsight_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningIronsight_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EItemPositioningIronsight3rd_003C_003EAccessor : VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningIronsight3rd_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyItemPositioningEnum value)
			{
				Set(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyItemPositioningEnum value)
			{
				Get(ref *(MyObjectBuilder_HandItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EngineerToolBaseDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EngineerToolBaseDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EngineerToolBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EngineerToolBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EngineerToolBaseDefinition CreateInstance()
			{
				return new MyObjectBuilder_EngineerToolBaseDefinition();
			}

			MyObjectBuilder_EngineerToolBaseDefinition IActivator<MyObjectBuilder_EngineerToolBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[DefaultValue(1)]
		public float SpeedMultiplier = 1f;

		[ProtoMember(4)]
		[DefaultValue(1)]
		public float DistanceMultiplier = 1f;

		public string Flare = "Welder";
	}
}
