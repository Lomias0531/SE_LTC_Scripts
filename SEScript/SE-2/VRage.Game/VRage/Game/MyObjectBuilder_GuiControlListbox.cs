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
	public class MyObjectBuilder_GuiControlListbox : MyObjectBuilder_GuiControlBase
	{
		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EVisualStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlListbox, MyGuiControlListboxStyleEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in MyGuiControlListboxStyleEnum value)
			{
				owner.VisualStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out MyGuiControlListboxStyleEnum value)
			{
				value = owner.VisualStyle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EVisibleRows_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlListbox, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in int value)
			{
				owner.VisibleRows = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out int value)
			{
				value = owner.VisibleRows;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EBackgroundColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EBackgroundColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EControlTexture_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlTexture_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EControlAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlListbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlListbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlListbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GuiControlListbox_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlListbox>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GuiControlListbox();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GuiControlListbox CreateInstance()
			{
				return new MyObjectBuilder_GuiControlListbox();
			}

			MyObjectBuilder_GuiControlListbox IActivator<MyObjectBuilder_GuiControlListbox>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyGuiControlListboxStyleEnum VisualStyle;

		[ProtoMember(4)]
		public int VisibleRows;
	}
}
