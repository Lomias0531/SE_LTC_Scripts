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
	public class MyObjectBuilder_GuiControlCheckbox : MyObjectBuilder_GuiControlBase
	{
		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EIsChecked_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in bool value)
			{
				owner.IsChecked = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out bool value)
			{
				value = owner.IsChecked;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003ECheckedTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in string value)
			{
				owner.CheckedTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out string value)
			{
				value = owner.CheckedTexture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EVisualStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, MyGuiControlCheckboxStyleEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in MyGuiControlCheckboxStyleEnum value)
			{
				owner.VisualStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out MyGuiControlCheckboxStyleEnum value)
			{
				value = owner.VisualStyle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EBackgroundColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EBackgroundColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EControlTexture_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlTexture_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EControlAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCheckbox, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlCheckbox owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlCheckbox owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GuiControlCheckbox_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlCheckbox>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GuiControlCheckbox();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GuiControlCheckbox CreateInstance()
			{
				return new MyObjectBuilder_GuiControlCheckbox();
			}

			MyObjectBuilder_GuiControlCheckbox IActivator<MyObjectBuilder_GuiControlCheckbox>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public bool IsChecked;

		[ProtoMember(4)]
		public string CheckedTexture;

		[ProtoMember(7)]
		public MyGuiControlCheckboxStyleEnum VisualStyle;
	}
}
