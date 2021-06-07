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
	public class MyObjectBuilder_TextStatVisualStyle : MyObjectBuilder_StatVisualStyle
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EFont_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value)
			{
				owner.Font = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value)
			{
				value = owner.Font;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in Vector4? value)
			{
				owner.ColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out Vector4? value)
			{
				value = owner.ColorMask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003ETextAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyGuiDrawAlignEnum?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyGuiDrawAlignEnum? value)
			{
				owner.TextAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyGuiDrawAlignEnum? value)
			{
				value = owner.TextAlign;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EStatId_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EStatId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EVisibleCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlinkCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in ConditionBase value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out ConditionBase value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003ESizePx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ESizePx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EOffsetPx_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EOffsetPx_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeInTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EFadeOutTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EMaxOnScreenTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EBlink_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003EBlink_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyAlphaBlinkBehavior value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyAlphaBlinkBehavior value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003ECategory_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle_003C_003ECategory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, VisualStyleCategory?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in VisualStyleCategory? value)
			{
				Set(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out VisualStyleCategory? value)
			{
				Get(ref *(MyObjectBuilder_StatVisualStyle*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TextStatVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TextStatVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TextStatVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_TextStatVisualStyle();
			}

			MyObjectBuilder_TextStatVisualStyle IActivator<MyObjectBuilder_TextStatVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Text;

		public float Scale;

		public string Font;

		public Vector4? ColorMask;

		public MyGuiDrawAlignEnum? TextAlign;
	}
}
