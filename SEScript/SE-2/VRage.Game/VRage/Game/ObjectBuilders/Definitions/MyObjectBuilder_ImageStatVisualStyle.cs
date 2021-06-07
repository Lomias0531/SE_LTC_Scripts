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
	public class MyObjectBuilder_ImageStatVisualStyle : MyObjectBuilder_StatVisualStyle
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out MyStringHash value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in Vector4? value)
			{
				owner.ColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out Vector4? value)
			{
				value = owner.ColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EStatId_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EStatId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003ESizePx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESizePx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EOffsetPx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EOffsetPx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EBlink_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyAlphaBlinkBehavior value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out MyAlphaBlinkBehavior value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003ECategory_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ECategory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, VisualStyleCategory?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in VisualStyleCategory? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out VisualStyleCategory? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ImageStatVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ImageStatVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ImageStatVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_ImageStatVisualStyle();
			}

			MyObjectBuilder_ImageStatVisualStyle IActivator<MyObjectBuilder_ImageStatVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash Texture;

		public Vector4? ColorMask;
	}
}
