using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.GUI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CircularProgressBarStatVisualStyle : MyObjectBuilder_StatVisualStyle
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ESegmentSizePx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector2 value)
			{
				owner.SegmentSizePx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector2 value)
			{
				value = owner.SegmentSizePx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ESegmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				owner.SegmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				value = owner.SegmentTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EBackgroudTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.BackgroudTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.BackgroudTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EFirstSegmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.FirstSegmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.FirstSegmentTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ELastSegmentTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash? value)
			{
				owner.LastSegmentTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash? value)
			{
				value = owner.LastSegmentTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ESegmentOrigin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector2? value)
			{
				owner.SegmentOrigin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector2? value)
			{
				value = owner.SegmentOrigin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ESpacingAngle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in float? value)
			{
				owner.SpacingAngle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out float? value)
			{
				value = owner.SpacingAngle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EAngleOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in float? value)
			{
				owner.AngleOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out float? value)
			{
				value = owner.AngleOffset;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EAnimate_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in bool? value)
			{
				owner.Animate = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out bool? value)
			{
				value = owner.Animate;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ENumberOfSegments_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in int? value)
			{
				owner.NumberOfSegments = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out int? value)
			{
				value = owner.NumberOfSegments;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EShowEmptySegments_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in bool? value)
			{
				owner.ShowEmptySegments = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out bool? value)
			{
				value = owner.ShowEmptySegments;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EEmptySegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.EmptySegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.EmptySegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EFullSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.FullSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.FullSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EAnimatedSegmentColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector4? value)
			{
				owner.AnimatedSegmentColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector4? value)
			{
				value = owner.AnimatedSegmentColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EAnimationDelayMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in double? value)
			{
				owner.AnimationDelayMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out double? value)
			{
				value = owner.AnimationDelayMs;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EAnimationSegmentDelayMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in double? value)
			{
				owner.AnimationSegmentDelayMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out double? value)
			{
				value = owner.AnimationSegmentDelayMs;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EStatId_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EStatId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ESizePx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESizePx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EOffsetPx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EOffsetPx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EBlink_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyAlphaBlinkBehavior value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyAlphaBlinkBehavior value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ECategory_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ECategory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, VisualStyleCategory?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in VisualStyleCategory? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out VisualStyleCategory? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CircularProgressBarStatVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CircularProgressBarStatVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CircularProgressBarStatVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_CircularProgressBarStatVisualStyle();
			}

			MyObjectBuilder_CircularProgressBarStatVisualStyle IActivator<MyObjectBuilder_CircularProgressBarStatVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector2 SegmentSizePx;

		public MyStringHash SegmentTexture;

		public MyStringHash? BackgroudTexture;

		public MyStringHash? FirstSegmentTexture;

		public MyStringHash? LastSegmentTexture;

		public Vector2? SegmentOrigin;

		public float? SpacingAngle;

		public float? AngleOffset;

		public bool? Animate;

		public int? NumberOfSegments;

		public bool? ShowEmptySegments;

		public Vector4? EmptySegmentColorMask;

		public Vector4? FullSegmentColorMask;

		public Vector4? AnimatedSegmentColorMask;

		public double? AnimationDelayMs;

		public double? AnimationSegmentDelayMs;
	}
}
