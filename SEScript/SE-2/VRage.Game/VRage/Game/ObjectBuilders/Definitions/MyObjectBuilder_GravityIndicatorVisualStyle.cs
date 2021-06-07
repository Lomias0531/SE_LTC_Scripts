using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_GravityIndicatorVisualStyle : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EOffsetPx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in Vector2 value)
			{
				owner.OffsetPx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out Vector2 value)
			{
				value = owner.OffsetPx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003ESizePx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in Vector2 value)
			{
				owner.SizePx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out Vector2 value)
			{
				value = owner.SizePx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EVelocitySizePx_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in Vector2 value)
			{
				owner.VelocitySizePx = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out Vector2 value)
			{
				value = owner.VelocitySizePx;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EFillTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in MyStringHash value)
			{
				owner.FillTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out MyStringHash value)
			{
				value = owner.FillTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EOverlayTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in MyStringHash value)
			{
				owner.OverlayTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out MyStringHash value)
			{
				value = owner.OverlayTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EVelocityTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in MyStringHash value)
			{
				owner.VelocityTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out MyStringHash value)
			{
				value = owner.VelocityTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EOriginAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in MyGuiDrawAlignEnum value)
			{
				owner.OriginAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out MyGuiDrawAlignEnum value)
			{
				value = owner.OriginAlign;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in ConditionBase value)
			{
				owner.VisibleCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out ConditionBase value)
			{
				value = owner.VisibleCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GravityIndicatorVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GravityIndicatorVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GravityIndicatorVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GravityIndicatorVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_GravityIndicatorVisualStyle();
			}

			MyObjectBuilder_GravityIndicatorVisualStyle IActivator<MyObjectBuilder_GravityIndicatorVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector2 OffsetPx;

		public Vector2 SizePx;

		public Vector2 VelocitySizePx;

		public MyStringHash FillTexture;

		public MyStringHash OverlayTexture;

		public MyStringHash VelocityTexture;

		public MyGuiDrawAlignEnum OriginAlign;

		[XmlElement(typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase VisibleCondition;
	}
}
