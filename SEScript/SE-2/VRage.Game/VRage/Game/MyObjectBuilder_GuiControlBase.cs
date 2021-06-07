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
	public abstract class MyObjectBuilder_GuiControlBase : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in Vector2 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out Vector2 value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in Vector2 value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out Vector2 value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in Vector4 value)
			{
				owner.BackgroundColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out Vector4 value)
			{
				value = owner.BackgroundColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in string value)
			{
				owner.ControlTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out string value)
			{
				value = owner.ControlTexture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EOriginAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in MyGuiDrawAlignEnum value)
			{
				owner.OriginAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out MyGuiDrawAlignEnum value)
			{
				value = owner.OriginAlign;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003EControlAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in int value)
			{
				owner.ControlAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out int value)
			{
				value = owner.ControlAlign;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiControlBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiControlBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiControlBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		[ProtoMember(1)]
		public Vector2 Position;

		[ProtoMember(4)]
		public Vector2 Size;

		[ProtoMember(7)]
		public string Name;

		[ProtoMember(10)]
		public Vector4 BackgroundColor = Vector4.One;

		[ProtoMember(13)]
		public string ControlTexture;

		[ProtoMember(16)]
		public MyGuiDrawAlignEnum OriginAlign;

		public int ControlAlign
		{
			get
			{
				return (int)OriginAlign;
			}
			set
			{
				OriginAlign = (MyGuiDrawAlignEnum)value;
			}
		}

		public bool ShouldSerializeControlAlign()
		{
			return false;
		}
	}
}
