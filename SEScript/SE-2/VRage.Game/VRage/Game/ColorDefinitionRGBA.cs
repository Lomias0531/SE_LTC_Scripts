using ProtoBuf;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public struct ColorDefinitionRGBA
	{
		protected class VRage_Game_ColorDefinitionRGBA_003C_003ER_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.R = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.R;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EG_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.G = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.G;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EB_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.B = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.B;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EA_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.A = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.A;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003ERed_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.Red = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.Red;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EGreen_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.Green = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.Green;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EBlue_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.Blue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.Blue;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EAlpha_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in byte value)
			{
				owner.Alpha = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out byte value)
			{
				value = owner.Alpha;
			}
		}

		protected class VRage_Game_ColorDefinitionRGBA_003C_003EHex_003C_003EAccessor : IMemberAccessor<ColorDefinitionRGBA, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ColorDefinitionRGBA owner, in string value)
			{
				owner.Hex = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ColorDefinitionRGBA owner, out string value)
			{
				value = owner.Hex;
			}
		}

		private class VRage_Game_ColorDefinitionRGBA_003C_003EActor : IActivator, IActivator<ColorDefinitionRGBA>
		{
			private sealed override object CreateInstance()
			{
				return default(ColorDefinitionRGBA);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ColorDefinitionRGBA CreateInstance()
			{
				return (ColorDefinitionRGBA)(object)default(ColorDefinitionRGBA);
			}

			ColorDefinitionRGBA IActivator<ColorDefinitionRGBA>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlAttribute]
		[ProtoMember(1)]
		public byte R;

		[ProtoMember(4)]
		[XmlAttribute]
		public byte G;

		[ProtoMember(7)]
		[XmlAttribute]
		public byte B;

		[ProtoMember(10)]
		[XmlAttribute]
		public byte A;

		[XmlAttribute]
		public byte Red
		{
			get
			{
				return R;
			}
			set
			{
				R = value;
			}
		}

		[XmlAttribute]
		public byte Green
		{
			get
			{
				return G;
			}
			set
			{
				G = value;
			}
		}

		[XmlAttribute]
		public byte Blue
		{
			get
			{
				return B;
			}
			set
			{
				B = value;
			}
		}

		[XmlAttribute]
		public byte Alpha
		{
			get
			{
				return A;
			}
			set
			{
				A = value;
			}
		}

		[XmlAttribute]
		public string Hex
		{
			get
			{
				return GetHex();
			}
			set
			{
				SetHex(value);
			}
		}

		public bool ShouldSerializeRed()
		{
			return false;
		}

		public bool ShouldSerializeGreen()
		{
			return false;
		}

		public bool ShouldSerializeBlue()
		{
			return false;
		}

		public bool ShouldSerializeAlpha()
		{
			return false;
		}

		public bool ShouldSerializeHex()
		{
			return false;
		}

		private string GetHex()
		{
			return $"#{A:X2}{R:X2}{G:X2}{B:X2}";
		}

		private void SetHex(string hex)
		{
			if (!string.IsNullOrEmpty(hex))
			{
				hex = hex.Trim(' ', '#');
				uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint result);
				if (hex.Length < 8)
				{
					result = (uint)((int)result | -16777216);
				}
				A = (byte)((uint)(-16777216 & (int)result) >> 24);
				R = (byte)((0xFF0000 & result) >> 16);
				G = (byte)((0xFF00 & result) >> 8);
				B = (byte)(0xFF & result);
			}
		}

		public ColorDefinitionRGBA(string hex)
			: this(0, 0, 0)
		{
			Hex = hex;
		}

		public ColorDefinitionRGBA(byte red, byte green, byte blue, byte alpha = byte.MaxValue)
		{
			R = red;
			G = green;
			B = blue;
			A = alpha;
		}

		public static implicit operator Color(ColorDefinitionRGBA definitionRgba)
		{
			return new Color(definitionRgba.R, definitionRgba.G, definitionRgba.B, definitionRgba.A);
		}

		public static implicit operator ColorDefinitionRGBA(Color color)
		{
			ColorDefinitionRGBA result = default(ColorDefinitionRGBA);
			result.A = color.A;
			result.B = color.B;
			result.G = color.G;
			result.R = color.R;
			return result;
		}

		public static implicit operator Vector4(ColorDefinitionRGBA definitionRgba)
		{
			return new Vector4((float)(int)definitionRgba.R / 255f, (float)(int)definitionRgba.G / 255f, (float)(int)definitionRgba.B / 255f, (float)(int)definitionRgba.A / 255f);
		}

		public static implicit operator ColorDefinitionRGBA(Vector4 vector)
		{
			ColorDefinitionRGBA result = default(ColorDefinitionRGBA);
			result.A = (byte)(vector.W * 255f);
			result.B = (byte)(vector.Z * 255f);
			result.G = (byte)(vector.Y * 255f);
			result.R = (byte)(vector.X * 255f);
			return result;
		}

		public override string ToString()
		{
			return $"R:{R} G:{G} B:{B} A:{A}";
		}
	}
}
