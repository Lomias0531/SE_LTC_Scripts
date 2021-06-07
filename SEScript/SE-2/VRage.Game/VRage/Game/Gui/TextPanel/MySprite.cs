using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.GUI.TextPanel
{
	[Serializable]
	[ProtoContract]
	public struct MySprite : IEquatable<MySprite>
	{
		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003EType_003C_003EAccessor : IMemberAccessor<MySprite, SpriteType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in SpriteType value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out SpriteType value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MySprite, Vector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in Vector2? value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out Vector2? value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003ESize_003C_003EAccessor : IMemberAccessor<MySprite, Vector2?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in Vector2? value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out Vector2? value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003EColor_003C_003EAccessor : IMemberAccessor<MySprite, Color?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in Color? value)
			{
				owner.Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out Color? value)
			{
				value = owner.Color;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003EData_003C_003EAccessor : IMemberAccessor<MySprite, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in string value)
			{
				owner.Data = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out string value)
			{
				value = owner.Data;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003EFontId_003C_003EAccessor : IMemberAccessor<MySprite, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in string value)
			{
				owner.FontId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out string value)
			{
				value = owner.FontId;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003EAlignment_003C_003EAccessor : IMemberAccessor<MySprite, TextAlignment>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in TextAlignment value)
			{
				owner.Alignment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out TextAlignment value)
			{
				value = owner.Alignment;
			}
		}

		protected class VRage_Game_GUI_TextPanel_MySprite_003C_003ERotationOrScale_003C_003EAccessor : IMemberAccessor<MySprite, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySprite owner, in float value)
			{
				owner.RotationOrScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySprite owner, out float value)
			{
				value = owner.RotationOrScale;
			}
		}

		private class VRage_Game_GUI_TextPanel_MySprite_003C_003EActor : IActivator, IActivator<MySprite>
		{
			private sealed override object CreateInstance()
			{
				return default(MySprite);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySprite CreateInstance()
			{
				return (MySprite)(object)default(MySprite);
			}

			MySprite IActivator<MySprite>.CreateInstance()
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
		public Vector2? Position;

		/// <summary>
		/// Render size for this layer. If not set, it will be sized to take up the whole texture
		/// </summary>
		[ProtoMember(7)]
		[Nullable]
		[DefaultValue(null)]
		public Vector2? Size;

		/// <summary>
		/// Color mask to be used when rendering this layer. If not set, white will be used
		/// </summary>
		[ProtoMember(10)]
		[Nullable]
		[DefaultValue(null)]
		public Color? Color;

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

		public MySprite(SpriteType type = SpriteType.TEXTURE, string data = null, Vector2? position = null, Vector2? size = null, Color? color = null, string fontId = null, TextAlignment alignment = TextAlignment.CENTER, float rotation = 0f)
		{
			Type = type;
			Data = data;
			Position = position;
			Size = size;
			Color = color;
			FontId = fontId;
			Alignment = alignment;
			RotationOrScale = rotation;
		}

		public static MySprite CreateSprite(string sprite, Vector2 position, Vector2 size)
		{
			return new MySprite(SpriteType.TEXTURE, sprite, position, size);
		}

		public static MySprite CreateText(string text, string fontId, Color color, float scale = 1f, TextAlignment alignment = TextAlignment.CENTER)
		{
			return new MySprite(SpriteType.TEXT, text, null, null, color, fontId, alignment, scale);
		}

		public static implicit operator MySerializableSprite(MySprite sprite)
		{
			MySerializableSprite result = default(MySerializableSprite);
			result.Type = sprite.Type;
			result.Position = sprite.Position;
			result.Size = sprite.Size;
			result.Color = sprite.Color?.PackedValue;
			result.Data = sprite.Data;
			result.Alignment = sprite.Alignment;
			result.FontId = sprite.FontId;
			result.RotationOrScale = sprite.RotationOrScale;
			return result;
		}

		public static implicit operator MySprite(MySerializableSprite sprite)
		{
			MySprite result = default(MySprite);
			result.Type = sprite.Type;
			result.Position = sprite.Position;
			result.Size = sprite.Size;
			result.Color = (sprite.Color.HasValue ? new Color?(new Color(sprite.Color.Value)) : null);
			result.Data = sprite.Data;
			result.Alignment = sprite.Alignment;
			result.FontId = sprite.FontId;
			result.RotationOrScale = sprite.RotationOrScale;
			return result;
		}

		public bool Equals(MySprite other)
		{
			if (Type == other.Type && Alignment == other.Alignment && RotationOrScale.Equals(other.RotationOrScale) && Position.Equals(other.Position) && Size.Equals(other.Size) && Color.Equals(other.Color) && AreStringsEqual(Data, other.Data))
			{
				return AreStringsEqual(FontId, other.FontId);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			object obj2;
			if ((obj2 = obj) is MySprite)
			{
				MySprite other = (MySprite)obj2;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((((((((((((((int)Type * 397) ^ Position.GetHashCode()) * 397) ^ Size.GetHashCode()) * 397) ^ Color.GetHashCode()) * 397) ^ ((Data != null) ? StringComparer.InvariantCulture.GetHashCode(Data) : 0)) * 397) ^ ((FontId != null) ? StringComparer.InvariantCulture.GetHashCode(FontId) : 0)) * 397) ^ (int)Alignment) * 397) ^ RotationOrScale.GetHashCode();
		}

		private bool AreStringsEqual(string lhs, string rhs)
		{
			if (string.IsNullOrEmpty(lhs) && string.IsNullOrEmpty(rhs))
			{
				return true;
			}
			return string.Equals(lhs, rhs, StringComparison.InvariantCulture);
		}
	}
}
