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
	public class MyObjectBuilder_GuiControlMultilineEditableLabel : MyObjectBuilder_GuiControlMultilineLabel
	{
		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003ETextScale_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlMultilineLabel_003C_003ETextScale_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in float value)
			{
				Set(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out float value)
			{
				Get(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003ETextAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlMultilineLabel_003C_003ETextAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003ETextColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlMultilineLabel_003C_003ETextColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EText_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlMultilineLabel_003C_003EText_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003ETextBoxAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlMultilineLabel_003C_003ETextBoxAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EFont_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlMultilineLabel_003C_003EFont_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlMultilineLabel*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EBackgroundColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EBackgroundColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EControlTexture_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlTexture_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EControlAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlMultilineEditableLabel, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlMultilineEditableLabel owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GuiControlMultilineEditableLabel_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlMultilineEditableLabel>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GuiControlMultilineEditableLabel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GuiControlMultilineEditableLabel CreateInstance()
			{
				return new MyObjectBuilder_GuiControlMultilineEditableLabel();
			}

			MyObjectBuilder_GuiControlMultilineEditableLabel IActivator<MyObjectBuilder_GuiControlMultilineEditableLabel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
