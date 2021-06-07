using ProtoBuf;
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
	public class MyObjectBuilder_GuiControlButton : MyObjectBuilder_GuiControlBase
	{
		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003ETextEnum_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in string value)
			{
				owner.TextEnum = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out string value)
			{
				value = owner.TextEnum;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003ETextScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in float value)
			{
				owner.TextScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out float value)
			{
				value = owner.TextScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003ETextAlignment_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in int value)
			{
				owner.TextAlignment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out int value)
			{
				value = owner.TextAlignment;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EDrawCrossTextureWhenDisabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in bool value)
			{
				owner.DrawCrossTextureWhenDisabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out bool value)
			{
				value = owner.DrawCrossTextureWhenDisabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EDrawRedTextureWhenDisabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in bool value)
			{
				owner.DrawRedTextureWhenDisabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out bool value)
			{
				value = owner.DrawRedTextureWhenDisabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EVisualStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlButton, MyGuiControlButtonStyleEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in MyGuiControlButtonStyleEnum value)
			{
				owner.VisualStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out MyGuiControlButtonStyleEnum value)
			{
				value = owner.VisualStyle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EBackgroundColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EBackgroundColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EControlTexture_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlTexture_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EControlAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlButton, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlButton owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlButton owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GuiControlButton_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlButton>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GuiControlButton();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GuiControlButton CreateInstance()
			{
				return new MyObjectBuilder_GuiControlButton();
			}

			MyObjectBuilder_GuiControlButton IActivator<MyObjectBuilder_GuiControlButton>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Text;

		[ProtoMember(4)]
		public string TextEnum;

		[ProtoMember(7)]
		public float TextScale;

		[ProtoMember(10)]
		public int TextAlignment;

		[ProtoMember(13)]
		public bool DrawCrossTextureWhenDisabled;

		[ProtoMember(16)]
		public bool DrawRedTextureWhenDisabled;

		[ProtoMember(19)]
		public MyGuiControlButtonStyleEnum VisualStyle;
	}
}
