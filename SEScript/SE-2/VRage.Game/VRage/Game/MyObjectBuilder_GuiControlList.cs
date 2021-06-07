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
	public class MyObjectBuilder_GuiControlList : MyObjectBuilder_GuiControlParent
	{
		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EVisualStyle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlList, MyGuiControlListStyleEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in MyGuiControlListStyleEnum value)
			{
				owner.VisualStyle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out MyGuiControlListStyleEnum value)
			{
				value = owner.VisualStyle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EControls_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlParent_003C_003EControls_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, MyObjectBuilder_GuiControls>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in MyObjectBuilder_GuiControls value)
			{
				Set(ref *(MyObjectBuilder_GuiControlParent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out MyObjectBuilder_GuiControls value)
			{
				Get(ref *(MyObjectBuilder_GuiControlParent*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in Vector2 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out Vector2 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EBackgroundColor_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EBackgroundColor_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in Vector4 value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out Vector4 value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EControlTexture_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlTexture_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in string value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out string value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EOriginAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EOriginAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in MyGuiDrawAlignEnum value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out MyGuiDrawAlignEnum value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EControlAlign_003C_003EAccessor : VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlAlign_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in int value)
			{
				Set(ref *(MyObjectBuilder_GuiControlBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out int value)
			{
				Get(ref *(MyObjectBuilder_GuiControlBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlList_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlList, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlList owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlList owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GuiControlList_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlList>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GuiControlList();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GuiControlList CreateInstance()
			{
				return new MyObjectBuilder_GuiControlList();
			}

			MyObjectBuilder_GuiControlList IActivator<MyObjectBuilder_GuiControlList>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyGuiControlListStyleEnum VisualStyle;
	}
}
