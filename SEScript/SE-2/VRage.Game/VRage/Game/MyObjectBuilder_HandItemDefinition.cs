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
	public class MyObjectBuilder_HandItemDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELeftHandOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.LeftHandOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.LeftHandOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELeftHandPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.LeftHandPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.LeftHandPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ERightHandOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.RightHandOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.RightHandOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ERightHandPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.RightHandPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.RightHandPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemWalkingOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemWalkingOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemWalkingPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemWalkingPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemShootOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemShootOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemShootPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemShootPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemIronsightOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemIronsightOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemIronsightOrientation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemIronsightPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemIronsightPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemIronsightPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemOrientation3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemOrientation3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemOrientation3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPosition3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemPosition3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemPosition3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingOrientation3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemWalkingOrientation3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemWalkingOrientation3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemWalkingPosition3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemWalkingPosition3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemWalkingPosition3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootOrientation3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value)
			{
				owner.ItemShootOrientation3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value)
			{
				value = owner.ItemShootOrientation3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemShootPosition3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ItemShootPosition3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ItemShootPosition3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EBlendTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.BlendTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.BlendTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShootBlend_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.ShootBlend = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.ShootBlend;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EXAmplitudeOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.XAmplitudeOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.XAmplitudeOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EYAmplitudeOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.YAmplitudeOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.YAmplitudeOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EZAmplitudeOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.ZAmplitudeOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.ZAmplitudeOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EXAmplitudeScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.XAmplitudeScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.XAmplitudeScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EYAmplitudeScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.YAmplitudeScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.YAmplitudeScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EZAmplitudeScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.ZAmplitudeScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.ZAmplitudeScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ERunMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.RunMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.RunMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EAmplitudeMultiplier3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.AmplitudeMultiplier3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.AmplitudeMultiplier3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateLeftHand_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value)
			{
				owner.SimulateLeftHand = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value)
			{
				value = owner.SimulateLeftHand;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateRightHand_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value)
			{
				owner.SimulateRightHand = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value)
			{
				value = owner.SimulateRightHand;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateLeftHandFps_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool? value)
			{
				owner.SimulateLeftHandFps = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool? value)
			{
				value = owner.SimulateLeftHandFps;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESimulateRightHandFps_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool? value)
			{
				owner.SimulateRightHandFps = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool? value)
			{
				value = owner.SimulateRightHandFps;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EFingersAnimation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				owner.FingersAnimation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				value = owner.FingersAnimation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EMuzzlePosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.MuzzlePosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.MuzzlePosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShootScatter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value)
			{
				owner.ShootScatter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value)
			{
				value = owner.ShootScatter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EScatterSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.ScatterSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.ScatterSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EPhysicalItemId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in SerializableDefinitionId value)
			{
				owner.PhysicalItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.PhysicalItemId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector4 value)
			{
				owner.LightColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector4 value)
			{
				value = owner.LightColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightFalloff_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.LightFalloff = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.LightFalloff;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.LightRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.LightRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightGlareSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.LightGlareSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.LightGlareSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightGlareIntensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.LightGlareIntensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.LightGlareIntensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightIntensityLower_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.LightIntensityLower = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.LightIntensityLower;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ELightIntensityUpper_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.LightIntensityUpper = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.LightIntensityUpper;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShakeAmountTarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.ShakeAmountTarget = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.ShakeAmountTarget;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EShakeAmountNoTarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value)
			{
				owner.ShakeAmountNoTarget = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value)
			{
				value = owner.ShakeAmountNoTarget;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EToolSounds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, List<ToolSound>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in List<ToolSound> value)
			{
				owner.ToolSounds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out List<ToolSound> value)
			{
				value = owner.ToolSounds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EToolMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				owner.ToolMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				value = owner.ToolMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioning_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioning = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioning;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioning3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioning3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioning3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningWalk_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioningWalk = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioningWalk;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningWalk3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioningWalk3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioningWalk3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningShoot_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioningShoot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioningShoot;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningShoot3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioningShoot3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioningShoot3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningIronsight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioningIronsight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioningIronsight;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EItemPositioningIronsight3rd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyItemPositioningEnum value)
			{
				owner.ItemPositioningIronsight3rd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyItemPositioningEnum value)
			{
				value = owner.ItemPositioningIronsight3rd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_HandItemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_HandItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_HandItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_HandItemDefinition CreateInstance()
			{
				return new MyObjectBuilder_HandItemDefinition();
			}

			MyObjectBuilder_HandItemDefinition IActivator<MyObjectBuilder_HandItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public Quaternion LeftHandOrientation = Quaternion.Identity;

		[ProtoMember(4)]
		public Vector3 LeftHandPosition;

		[ProtoMember(7)]
		public Quaternion RightHandOrientation = Quaternion.Identity;

		[ProtoMember(10)]
		public Vector3 RightHandPosition;

		[ProtoMember(13)]
		public Quaternion ItemOrientation = Quaternion.Identity;

		[ProtoMember(16)]
		public Vector3 ItemPosition;

		[ProtoMember(19)]
		public Quaternion ItemWalkingOrientation = Quaternion.Identity;

		[ProtoMember(22)]
		public Vector3 ItemWalkingPosition;

		[ProtoMember(25)]
		public Quaternion ItemShootOrientation = Quaternion.Identity;

		[ProtoMember(28)]
		public Vector3 ItemShootPosition;

		[ProtoMember(31)]
		public Quaternion ItemIronsightOrientation = Quaternion.Identity;

		[ProtoMember(34)]
		public Vector3 ItemIronsightPosition;

		[ProtoMember(37)]
		public Quaternion ItemOrientation3rd = Quaternion.Identity;

		[ProtoMember(40)]
		public Vector3 ItemPosition3rd;

		[ProtoMember(43)]
		public Quaternion ItemWalkingOrientation3rd = Quaternion.Identity;

		[ProtoMember(46)]
		public Vector3 ItemWalkingPosition3rd;

		[ProtoMember(49)]
		public Quaternion ItemShootOrientation3rd = Quaternion.Identity;

		[ProtoMember(52)]
		public Vector3 ItemShootPosition3rd;

		[ProtoMember(55)]
		public float BlendTime;

		[ProtoMember(58)]
		public float ShootBlend;

		[ProtoMember(61)]
		public float XAmplitudeOffset;

		[ProtoMember(64)]
		public float YAmplitudeOffset;

		[ProtoMember(67)]
		public float ZAmplitudeOffset;

		[ProtoMember(70)]
		public float XAmplitudeScale;

		[ProtoMember(73)]
		public float YAmplitudeScale;

		[ProtoMember(76)]
		public float ZAmplitudeScale;

		[ProtoMember(79)]
		public float RunMultiplier;

		[ProtoMember(82)]
		public float AmplitudeMultiplier3rd;

		[ProtoMember(85)]
		[DefaultValue(true)]
		public bool SimulateLeftHand = true;

		[ProtoMember(88)]
		[DefaultValue(true)]
		public bool SimulateRightHand = true;

		[ProtoMember(91)]
		public bool? SimulateLeftHandFps;

		[ProtoMember(94)]
		public bool? SimulateRightHandFps;

		[ProtoMember(97)]
		public string FingersAnimation;

		[ProtoMember(100)]
		public Vector3 MuzzlePosition;

		[ProtoMember(103)]
		public Vector3 ShootScatter;

		[ProtoMember(106)]
		public float ScatterSpeed;

		[ProtoMember(109)]
		public SerializableDefinitionId PhysicalItemId;

		[ProtoMember(112)]
		public Vector4 LightColor;

		[ProtoMember(115)]
		public float LightFalloff;

		[ProtoMember(118)]
		public float LightRadius;

		[ProtoMember(121)]
		public float LightGlareSize;

		[ProtoMember(124)]
		public float LightGlareIntensity = 1f;

		[ProtoMember(127)]
		public float LightIntensityLower;

		[ProtoMember(130)]
		public float LightIntensityUpper;

		[ProtoMember(133)]
		public float ShakeAmountTarget;

		[ProtoMember(136)]
		public float ShakeAmountNoTarget;

		[ProtoMember(139)]
		public List<ToolSound> ToolSounds;

		[ProtoMember(142)]
		public string ToolMaterial = "Grinder";

		[ProtoMember(145)]
		public MyItemPositioningEnum ItemPositioning;

		[ProtoMember(148)]
		public MyItemPositioningEnum ItemPositioning3rd;

		[ProtoMember(151)]
		public MyItemPositioningEnum ItemPositioningWalk;

		[ProtoMember(154)]
		public MyItemPositioningEnum ItemPositioningWalk3rd;

		[ProtoMember(157)]
		public MyItemPositioningEnum ItemPositioningShoot;

		[ProtoMember(160)]
		public MyItemPositioningEnum ItemPositioningShoot3rd;

		[ProtoMember(163)]
		public MyItemPositioningEnum ItemPositioningIronsight;

		[ProtoMember(166)]
		public MyItemPositioningEnum ItemPositioningIronsight3rd;
	}
}
