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
	public class MyObjectBuilder_ProgressBarStatVisualStyle : MyObjectBuilder_StatVisualStyle
	{
		public struct SimpleBarData
		{
			public MyStringHash BackgroundTexture;

			public MyStringHash ProgressTexture;

			public Vector4? BackgroundColorMask;

			public Vector4? ProgressColorMask;

			public Vector2I ProgressTextureOffsetPx;
		}

		public struct NineTiledData
		{
			public MyStringHash Texture;

			public Vector4? ColorMask;
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003ESimpleStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, SimpleBarData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in SimpleBarData? value)
			{
				owner.SimpleStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out SimpleBarData? value)
			{
				value = owner.SimpleStyle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003ENineTiledStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, NineTiledData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in NineTiledData? value)
			{
				owner.NineTiledStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out NineTiledData? value)
			{
				value = owner.NineTiledStyle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EInverted_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in bool? value)
			{
				owner.Inverted = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out bool? value)
			{
				value = owner.Inverted;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EStatId_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EStatId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003ESizePx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESizePx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EOffsetPx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EOffsetPx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EBlink_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in MyAlphaBlinkBehavior value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out MyAlphaBlinkBehavior value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003ECategory_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ECategory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, VisualStyleCategory?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in VisualStyleCategory? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out VisualStyleCategory? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProgressBarStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProgressBarStatVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProgressBarStatVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProgressBarStatVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_ProgressBarStatVisualStyle();
			}

			MyObjectBuilder_ProgressBarStatVisualStyle IActivator<MyObjectBuilder_ProgressBarStatVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SimpleBarData? SimpleStyle;

		public NineTiledData? NineTiledStyle;

		public bool? Inverted;
	}
}
