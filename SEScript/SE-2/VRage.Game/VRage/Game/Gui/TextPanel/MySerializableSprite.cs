using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.GUI.TextPanel
{
	[ProtoContract]
	public struct MySerializableSprite
	{
		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EType_003C_003EAccessor : IMemberAccessor<MySerializableSprite, SpriteType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in SpriteType value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out SpriteType value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MySerializableSprite, SerializableVector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in SerializableVector2? value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out SerializableVector2? value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003ESize_003C_003EAccessor : IMemberAccessor<MySerializableSprite, SerializableVector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in SerializableVector2? value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out SerializableVector2? value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EColor_003C_003EAccessor : IMemberAccessor<MySerializableSprite, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in uint? value)
			{
				owner.Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out uint? value)
			{
				value = owner.Color;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EData_003C_003EAccessor : IMemberAccessor<MySerializableSprite, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in string value)
			{
				owner.Data = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out string value)
			{
				value = owner.Data;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EFontId_003C_003EAccessor : IMemberAccessor<MySerializableSprite, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in string value)
			{
				owner.FontId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out string value)
			{
				value = owner.FontId;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EAlignment_003C_003EAccessor : IMemberAccessor<MySerializableSprite, TextAlignment>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in TextAlignment value)
			{
				owner.Alignment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out TextAlignment value)
			{
				value = owner.Alignment;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003ERotationOrScale_003C_003EAccessor : IMemberAccessor<MySerializableSprite, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in float value)
			{
				owner.RotationOrScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out float value)
			{
				value = owner.RotationOrScale;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EIndex_003C_003EAccessor : IMemberAccessor<MySerializableSprite, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializableSprite owner, in int value)
			{
				owner.Index = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializableSprite owner, out int value)
			{
				value = owner.Index;
			}
		}

		private class VRage_Game_GUI_TextPanel_MySerializableSprite_003C_003EActor : IActivator, IActivator<MySerializableSprite>
		{
			private sealed override object CreateInstance()
			{
				return default(MySerializableSprite);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySerializableSprite CreateInstance()
			{
				return (MySerializableSprite)(object)default(MySerializableSprite);
			}

			MySerializableSprite IActivator<MySerializableSprite>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Type of the render layer
		/// </summary>
		[ProtoMember(1)]
		[DefaultValue(SpriteType.TEXTURE)]
		public SpriteType Type;

		/// <summary>
		/// Render position for this layer. If not set, it will be placed in the center
		/// </summary>
		[ProtoMember(4)]
		[Nullable]
		[DefaultValue(null)]
		public SerializableVector2? Position;

		/// <summary>
		/// Render size for this layer. If not set, it will be sized to take up the whole texture
		/// </summary>
		[ProtoMember(7)]
		[Nullable]
		[DefaultValue(null)]
		public SerializableVector2? Size;

		/// <summary>
		/// Color mask to be used when rendering this layer. If not set, white will be used
		/// </summary>
		[ProtoMember(10)]
		[Nullable]
		[DefaultValue(null)]
		public uint? Color;

		/// <summary>
		/// Data to be rendered, depending on what the layer type is. This can be text or a texture path
		/// </summary>
		[ProtoMember(13)]
		[Nullable]
		[DefaultValue(null)]
		public string Data;

		/// <summary>
		/// In case we are rendering text, what font to use.
		/// </summary>
		[ProtoMember(16)]
		[Nullable]
		[DefaultValue(null)]
		public string FontId;

		/// <summary>
		/// Alignment for the text and sprites.
		/// </summary>
		[ProtoMember(19)]
		[DefaultValue(TextAlignment.CENTER)]
		public TextAlignment Alignment;

		/// <summary>
		/// Rotation of sprite in radians. Used as scale for text.
		/// </summary>
		[ProtoMember(22)]
		[DefaultValue(0f)]
		public float RotationOrScale;

		[ProtoMember(25)]
		public int Index;
	}
}
