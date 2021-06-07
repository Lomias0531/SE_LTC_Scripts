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
	public class MyObjectBuilder_GuiScreen : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003EControls_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, MyObjectBuilder_GuiControls>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in MyObjectBuilder_GuiControls value)
			{
				owner.Controls = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out MyObjectBuilder_GuiControls value)
			{
				value = owner.Controls;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, Vector4?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in Vector4? value)
			{
				owner.BackgroundColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out Vector4? value)
			{
				value = owner.BackgroundColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003EBackgroundTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in string value)
			{
				owner.BackgroundTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out string value)
			{
				value = owner.BackgroundTexture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, Vector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in Vector2? value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out Vector2? value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003ECloseButtonEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in bool value)
			{
				owner.CloseButtonEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out bool value)
			{
				value = owner.CloseButtonEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003ECloseButtonOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in Vector2 value)
			{
				owner.CloseButtonOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out Vector2 value)
			{
				value = owner.CloseButtonOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GuiScreen_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GuiScreen owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GuiScreen owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GuiScreen_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GuiScreen>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GuiScreen();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GuiScreen CreateInstance()
			{
				return new MyObjectBuilder_GuiScreen();
			}

			MyObjectBuilder_GuiScreen IActivator<MyObjectBuilder_GuiScreen>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyObjectBuilder_GuiControls Controls;

		[ProtoMember(4)]
		public Vector4? BackgroundColor;

		[ProtoMember(7)]
		public string BackgroundTexture;

		[ProtoMember(10)]
		public Vector2? Size;

		[ProtoMember(13)]
		public bool CloseButtonEnabled;

		[ProtoMember(16)]
		public Vector2 CloseButtonOffset;

		public bool ShouldSerializeCloseButtonOffset()
		{
			return CloseButtonEnabled;
		}
	}
}
