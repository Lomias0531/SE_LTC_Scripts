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
	public class MyObjectBuilder_HudIcon : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudIcon owner, in Vector2 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudIcon owner, out Vector2 value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003EOriginAlign_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, MyGuiDrawAlignEnum?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudIcon owner, in MyGuiDrawAlignEnum? value)
			{
				owner.OriginAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudIcon owner, out MyGuiDrawAlignEnum? value)
			{
				value = owner.OriginAlign;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, Vector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudIcon owner, in Vector2? value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudIcon owner, out Vector2? value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudIcon owner, in MyStringHash value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudIcon owner, out MyStringHash value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003EBlink_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, MyAlphaBlinkBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HudIcon owner, in MyAlphaBlinkBehavior value)
			{
				owner.Blink = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HudIcon owner, out MyAlphaBlinkBehavior value)
			{
				value = owner.Blink;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudIcon owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudIcon owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudIcon owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudIcon owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudIcon owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudIcon owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HudIcon owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HudIcon owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon_003C_003EActor : IActivator, IActivator<MyObjectBuilder_HudIcon>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_HudIcon();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_HudIcon CreateInstance()
			{
				return new MyObjectBuilder_HudIcon();
			}

			MyObjectBuilder_HudIcon IActivator<MyObjectBuilder_HudIcon>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector2 Position;

		public MyGuiDrawAlignEnum? OriginAlign;

		public Vector2? Size;

		public MyStringHash Texture;

		public MyAlphaBlinkBehavior Blink;
	}
}
