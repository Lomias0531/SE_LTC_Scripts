using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRageMath;

namespace VRageRender.Messages
{
	[ProtoContract]
	public class MyCloudLayerSettings
	{
		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EModel_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in string value)
			{
				owner.Model = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out string value)
			{
				value = owner.Model;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003ETextures_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in List<string> value)
			{
				owner.Textures = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out List<string> value)
			{
				value = owner.Textures;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003ERelativeAltitude_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in float value)
			{
				owner.RelativeAltitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out float value)
			{
				value = owner.RelativeAltitude;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003ERotationAxis_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in Vector3D value)
			{
				owner.RotationAxis = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out Vector3D value)
			{
				value = owner.RotationAxis;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EAngularVelocity_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in float value)
			{
				owner.AngularVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out float value)
			{
				value = owner.AngularVelocity;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EInitialRotation_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in float value)
			{
				owner.InitialRotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out float value)
			{
				value = owner.InitialRotation;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EScalingEnabled_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in bool value)
			{
				owner.ScalingEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out bool value)
			{
				value = owner.ScalingEnabled;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EFadeOutRelativeAltitudeStart_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in float value)
			{
				owner.FadeOutRelativeAltitudeStart = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out float value)
			{
				value = owner.FadeOutRelativeAltitudeStart;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EFadeOutRelativeAltitudeEnd_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in float value)
			{
				owner.FadeOutRelativeAltitudeEnd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out float value)
			{
				value = owner.FadeOutRelativeAltitudeEnd;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EApplyFogRelativeDistance_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in float value)
			{
				owner.ApplyFogRelativeDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out float value)
			{
				value = owner.ApplyFogRelativeDistance;
			}
		}

		protected class VRageRender_Messages_MyCloudLayerSettings_003C_003EColor_003C_003EAccessor : IMemberAccessor<MyCloudLayerSettings, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCloudLayerSettings owner, in Vector4 value)
			{
				owner.Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCloudLayerSettings owner, out Vector4 value)
			{
				value = owner.Color;
			}
		}

		[ProtoMember(1)]
		[ModdableContentFile("mwm")]
		public string Model;

		[ProtoMember(4)]
		[XmlArrayItem("Texture")]
		[ModdableContentFile("dds")]
		public List<string> Textures;

		[ProtoMember(7)]
		public float RelativeAltitude;

		[ProtoMember(10)]
		public Vector3D RotationAxis;

		[ProtoMember(13)]
		public float AngularVelocity;

		[ProtoMember(16)]
		public float InitialRotation;

		[ProtoMember(19)]
		public bool ScalingEnabled;

		[ProtoMember(22)]
		public float FadeOutRelativeAltitudeStart;

		[ProtoMember(25)]
		public float FadeOutRelativeAltitudeEnd;

		[ProtoMember(28)]
		public float ApplyFogRelativeDistance;

		[ProtoMember(31)]
		public Vector4 Color = Vector4.One;
	}
}
