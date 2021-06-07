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
	public class MyObjectBuilder_ToolbarControlVisualStyle : MyObjectBuilder_Base
	{
		public class ToolbarItemStyle
		{
			public MyStringHash Texture = MyStringHash.GetOrCompute("Textures\\GUI\\Controls\\grid_item.dds");

			public MyStringHash TextureHighlight = MyStringHash.GetOrCompute("Textures\\GUI\\Controls\\grid_item_highlight.dds");

			public MyStringHash VariantTexture = MyStringHash.GetOrCompute("Textures\\GUI\\Icons\\VariantsAvailable.dds");

			public Vector2? VariantOffset;

			public string FontNormal = "Blue";

			public string FontHighlight = "White";

			public float TextScale = 0.75f;

			public Vector2 ItemTextureScale = Vector2.Zero;

			public MyGuiOffset? Margin;
		}

		public class ToolbarPageStyle
		{
			public MyStringHash PageCompositeTexture;

			public MyStringHash PageHighlightCompositeTexture;

			public Vector2 PagesOffset;

			public float? NumberSize;
		}

		public class ColorStyle
		{
			public Vector2 Offset;

			public Vector2 VoxelHandPosition;

			public Vector2 Size;

			public MyStringHash Texture;
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EVisibleCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, ConditionBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in ConditionBase value)
			{
				owner.VisibleCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out ConditionBase value)
			{
				value = owner.VisibleCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EColorPanelStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, ColorStyle>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in ColorStyle value)
			{
				owner.ColorPanelStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out ColorStyle value)
			{
				value = owner.ColorPanelStyle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003ECenterPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in Vector2 value)
			{
				owner.CenterPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out Vector2 value)
			{
				value = owner.CenterPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003ESelectedItemPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in Vector2 value)
			{
				owner.SelectedItemPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out Vector2 value)
			{
				value = owner.SelectedItemPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003ESelectedItemTextScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in float? value)
			{
				owner.SelectedItemTextScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out float? value)
			{
				value = owner.SelectedItemTextScale;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EOriginAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in MyGuiDrawAlignEnum value)
			{
				owner.OriginAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out MyGuiDrawAlignEnum value)
			{
				value = owner.OriginAlign;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EItemStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, ToolbarItemStyle>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in ToolbarItemStyle value)
			{
				owner.ItemStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out ToolbarItemStyle value)
			{
				value = owner.ItemStyle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EPageStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, ToolbarPageStyle>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in ToolbarPageStyle value)
			{
				owner.PageStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out ToolbarPageStyle value)
			{
				value = owner.PageStyle;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EStatControls_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyObjectBuilder_StatControls[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in MyObjectBuilder_StatControls[] value)
			{
				owner.StatControls = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out MyObjectBuilder_StatControls[] value)
			{
				value = owner.StatControls;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolbarControlVisualStyle owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolbarControlVisualStyle owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ToolbarControlVisualStyle>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ToolbarControlVisualStyle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ToolbarControlVisualStyle CreateInstance()
			{
				return new MyObjectBuilder_ToolbarControlVisualStyle();
			}

			MyObjectBuilder_ToolbarControlVisualStyle IActivator<MyObjectBuilder_ToolbarControlVisualStyle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlElement(typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase VisibleCondition;

		public ColorStyle ColorPanelStyle;

		public Vector2 CenterPosition;

		public Vector2 SelectedItemPosition;

		public float? SelectedItemTextScale;

		public MyGuiDrawAlignEnum OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;

		public ToolbarItemStyle ItemStyle;

		public ToolbarPageStyle PageStyle;

		[XmlArrayItem("StatControl")]
		public MyObjectBuilder_StatControls[] StatControls;
	}
}
