using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath.PackedVector;

namespace VRageMath
{
	/// <summary>
	/// Represents a four-component color using red, green, blue, and alpha data.
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct Color : IPackedVector<uint>, IPackedVector, IEquatable<Color>
	{
		protected class VRageMath_Color_003C_003EPackedValue_003C_003EAccessor : IMemberAccessor<Color, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in uint value)
			{
				owner.PackedValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out uint value)
			{
				value = owner.PackedValue;
			}
		}

		protected class VRageMath_Color_003C_003EX_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Color_003C_003EY_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Color_003C_003EZ_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_Color_003C_003ER_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.R = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.R;
			}
		}

		protected class VRageMath_Color_003C_003EG_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.G = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.G;
			}
		}

		protected class VRageMath_Color_003C_003EB_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.B = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.B;
			}
		}

		protected class VRageMath_Color_003C_003EA_003C_003EAccessor : IMemberAccessor<Color, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in byte value)
			{
				owner.A = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out byte value)
			{
				value = owner.A;
			}
		}

		protected class VRageMath_Color_003C_003EVRageMath_002EPackedVector_002EIPackedVector_003CSystem_002EUInt32_003E_002EPackedValue_003C_003EAccessor : IMemberAccessor<Color, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Color owner, in uint value)
			{
				owner.VRageMath_002EPackedVector_002EIPackedVector_003CSystem_002EUInt32_003E_002EPackedValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Color owner, out uint value)
			{
				value = owner.VRageMath_002EPackedVector_002EIPackedVector_003CSystem_002EUInt32_003E_002EPackedValue;
			}
		}

		/// <summary>
		/// Gets the current color as a packed value.
		/// </summary>
		[ProtoMember(1)]
		public uint PackedValue;

		/// <summary>
		/// Gets or sets the red component value of this Color.
		/// </summary>
		public byte X
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

		/// <summary>
		/// Gets or sets the green component value of this Color.
		/// </summary>
		public byte Y
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

		/// <summary>
		/// Gets or sets the blue component value of this Color.
		/// </summary>
		public byte Z
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

		/// <summary>
		/// Gets or sets the red component value of this Color.
		/// </summary>
		public byte R
		{
			get
			{
				return (byte)PackedValue;
			}
			set
			{
				PackedValue = (uint)(((int)PackedValue & -256) | value);
			}
		}

		/// <summary>
		/// Gets or sets the green component value of this Color.
		/// </summary>
		public byte G
		{
			get
			{
				return (byte)(PackedValue >> 8);
			}
			set
			{
				PackedValue = (uint)(((int)PackedValue & -65281) | (value << 8));
			}
		}

		/// <summary>
		/// Gets or sets the blue component value of this Color.
		/// </summary>
		public byte B
		{
			get
			{
				return (byte)(PackedValue >> 16);
			}
			set
			{
				PackedValue = (uint)(((int)PackedValue & -16711681) | (value << 16));
			}
		}

		/// <summary>
		/// Gets or sets the alpha component value.
		/// </summary>
		public byte A
		{
			get
			{
				return (byte)(PackedValue >> 24);
			}
			set
			{
				PackedValue = (uint)((int)(PackedValue & 0xFFFFFF) | (value << 24));
			}
		}

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:0 B:0 A:0.
		/// </summary>
		public static Color Transparent => new Color(0u);

		/// <summary>
		/// Gets a system-defined color with the value R:240 G:248 B:255 A:255.
		/// </summary>
		public static Color AliceBlue => new Color(4294965488u);

		/// <summary>
		/// Gets a system-defined color with the value R:250 G:235 B:215 A:255.
		/// </summary>
		public static Color AntiqueWhite => new Color(4292340730u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:255 B:255 A:255.
		/// </summary>
		public static Color Aqua => new Color(4294967040u);

		/// <summary>
		/// Gets a system-defined color with the value R:127 G:255 B:212 A:255.
		/// </summary>
		public static Color Aquamarine => new Color(4292149119u);

		/// <summary>
		/// Gets a system-defined color with the value R:240 G:255 B:255 A:255.
		/// </summary>
		public static Color Azure => new Color(4294967280u);

		/// <summary>
		/// Gets a system-defined color with the value R:245 G:245 B:220 A:255.
		/// </summary>
		public static Color Beige => new Color(4292670965u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:228 B:196 A:255.
		/// </summary>
		public static Color Bisque => new Color(4291093759u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:0 B:0 A:255.
		/// </summary>
		public static Color Black => new Color(4278190080u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:235 B:205 A:255.
		/// </summary>
		public static Color BlanchedAlmond => new Color(4291685375u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:0 B:255 A:255.
		/// </summary>
		public static Color Blue => new Color(4294901760u);

		/// <summary>
		/// Gets a system-defined color with the value R:138 G:43 B:226 A:255.
		/// </summary>
		public static Color BlueViolet => new Color(4293012362u);

		/// <summary>
		/// Gets a system-defined color with the value R:165 G:42 B:42 A:255.
		/// </summary>
		public static Color Brown => new Color(4280953509u);

		/// <summary>
		/// Gets a system-defined color with the value R:222 G:184 B:135 A:255.
		/// </summary>
		public static Color BurlyWood => new Color(4287084766u);

		/// <summary>
		/// Gets a system-defined color with the value R:95 G:158 B:160 A:255.
		/// </summary>
		public static Color CadetBlue => new Color(4288716383u);

		/// <summary>
		/// Gets a system-defined color with the value R:127 G:255 B:0 A:255.
		/// </summary>
		public static Color Chartreuse => new Color(4278255487u);

		/// <summary>
		/// Gets a system-defined color with the value R:210 G:105 B:30 A:255.
		/// </summary>
		public static Color Chocolate => new Color(4280183250u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:127 B:80 A:255.
		/// </summary>
		public static Color Coral => new Color(4283465727u);

		/// <summary>
		/// Gets a system-defined color with the value R:100 G:149 B:237 A:255.
		/// </summary>
		public static Color CornflowerBlue => new Color(4293760356u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:248 B:220 A:255.
		/// </summary>
		public static Color Cornsilk => new Color(4292671743u);

		/// <summary>
		/// Gets a system-defined color with the value R:220 G:20 B:60 A:255.
		/// </summary>
		public static Color Crimson => new Color(4282127580u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:255 B:255 A:255.
		/// </summary>
		public static Color Cyan => new Color(4294967040u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:0 B:139 A:255.
		/// </summary>
		public static Color DarkBlue => new Color(4287299584u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:139 B:139 A:255.
		/// </summary>
		public static Color DarkCyan => new Color(4287335168u);

		/// <summary>
		/// Gets a system-defined color with the value R:184 G:134 B:11 A:255.
		/// </summary>
		public static Color DarkGoldenrod => new Color(4278945464u);

		/// <summary>
		/// Gets a system-defined color with the value R:169 G:169 B:169 A:255.
		/// </summary>
		public static Color DarkGray => new Color(4289309097u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:100 B:0 A:255.
		/// </summary>
		public static Color DarkGreen => new Color(4278215680u);

		/// <summary>
		/// Gets a system-defined color with the value R:189 G:183 B:107 A:255.
		/// </summary>
		public static Color DarkKhaki => new Color(4285249469u);

		/// <summary>
		/// Gets a system-defined color with the value R:139 G:0 B:139 A:255.
		/// </summary>
		public static Color DarkMagenta => new Color(4287299723u);

		/// <summary>
		/// Gets a system-defined color with the value R:85 G:107 B:47 A:255.
		/// </summary>
		public static Color DarkOliveGreen => new Color(4281297749u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:140 B:0 A:255.
		/// </summary>
		public static Color DarkOrange => new Color(4278226175u);

		/// <summary>
		/// Gets a system-defined color with the value R:153 G:50 B:204 A:255.
		/// </summary>
		public static Color DarkOrchid => new Color(4291572377u);

		/// <summary>
		/// Gets a system-defined color with the value R:139 G:0 B:0 A:255.
		/// </summary>
		public static Color DarkRed => new Color(4278190219u);

		/// <summary>
		/// Gets a system-defined color with the value R:233 G:150 B:122 A:255.
		/// </summary>
		public static Color DarkSalmon => new Color(4286224105u);

		/// <summary>
		/// Gets a system-defined color with the value R:143 G:188 B:139 A:255.
		/// </summary>
		public static Color DarkSeaGreen => new Color(4287347855u);

		/// <summary>
		/// Gets a system-defined color with the value R:72 G:61 B:139 A:255.
		/// </summary>
		public static Color DarkSlateBlue => new Color(4287315272u);

		/// <summary>
		/// Gets a system-defined color with the value R:47 G:79 B:79 A:255.
		/// </summary>
		public static Color DarkSlateGray => new Color(4283387695u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:206 B:209 A:255.
		/// </summary>
		public static Color DarkTurquoise => new Color(4291939840u);

		/// <summary>
		/// Gets a system-defined color with the value R:148 G:0 B:211 A:255.
		/// </summary>
		public static Color DarkViolet => new Color(4292018324u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:20 B:147 A:255.
		/// </summary>
		public static Color DeepPink => new Color(4287829247u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:191 B:255 A:255.
		/// </summary>
		public static Color DeepSkyBlue => new Color(4294950656u);

		/// <summary>
		/// Gets a system-defined color with the value R:105 G:105 B:105 A:255.
		/// </summary>
		public static Color DimGray => new Color(4285098345u);

		/// <summary>
		/// Gets a system-defined color with the value R:30 G:144 B:255 A:255.
		/// </summary>
		public static Color DodgerBlue => new Color(4294938654u);

		/// <summary>
		/// Gets a system-defined color with the value R:178 G:34 B:34 A:255.
		/// </summary>
		public static Color Firebrick => new Color(4280427186u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:250 B:240 A:255.
		/// </summary>
		public static Color FloralWhite => new Color(4293982975u);

		/// <summary>
		/// Gets a system-defined color with the value R:34 G:139 B:34 A:255.
		/// </summary>
		public static Color ForestGreen => new Color(4280453922u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:0 B:255 A:255.
		/// </summary>
		public static Color Fuchsia => new Color(4294902015u);

		/// <summary>
		/// Gets a system-defined color with the value R:220 G:220 B:220 A:255.
		/// </summary>
		public static Color Gainsboro => new Color(4292664540u);

		/// <summary>
		/// Gets a system-defined color with the value R:248 G:248 B:255 A:255.
		/// </summary>
		public static Color GhostWhite => new Color(4294965496u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:215 B:0 A:255.
		/// </summary>
		public static Color Gold => new Color(4278245375u);

		/// <summary>
		/// Gets a system-defined color with the value R:218 G:165 B:32 A:255.
		/// </summary>
		public static Color Goldenrod => new Color(4280329690u);

		/// <summary>
		/// Gets a system-defined color with the value R:128 G:128 B:128 A:255.
		/// </summary>
		public static Color Gray => new Color(4286611584u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:128 B:0 A:255.
		/// </summary>
		public static Color Green => new Color(4278222848u);

		/// <summary>
		/// Gets a system-defined color with the value R:173 G:255 B:47 A:255.
		/// </summary>
		public static Color GreenYellow => new Color(4281335725u);

		/// <summary>
		/// Gets a system-defined color with the value R:240 G:255 B:240 A:255.
		/// </summary>
		public static Color Honeydew => new Color(4293984240u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:105 B:180 A:255.
		/// </summary>
		public static Color HotPink => new Color(4290013695u);

		/// <summary>
		/// Gets a system-defined color with the value R:205 G:92 B:92 A:255.
		/// </summary>
		public static Color IndianRed => new Color(4284243149u);

		/// <summary>
		/// Gets a system-defined color with the value R:75 G:0 B:130 A:255.
		/// </summary>
		public static Color Indigo => new Color(4286709835u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:255 B:240 A:255.
		/// </summary>
		public static Color Ivory => new Color(4293984255u);

		/// <summary>
		/// Gets a system-defined color with the value R:240 G:230 B:140 A:255.
		/// </summary>
		public static Color Khaki => new Color(4287424240u);

		/// <summary>
		/// Gets a system-defined color with the value R:230 G:230 B:250 A:255.
		/// </summary>
		public static Color Lavender => new Color(4294633190u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:240 B:245 A:255.
		/// </summary>
		public static Color LavenderBlush => new Color(4294308095u);

		/// <summary>
		/// Gets a system-defined color with the value R:124 G:252 B:0 A:255.
		/// </summary>
		public static Color LawnGreen => new Color(4278254716u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:250 B:205 A:255.
		/// </summary>
		public static Color LemonChiffon => new Color(4291689215u);

		/// <summary>
		/// Gets a system-defined color with the value R:173 G:216 B:230 A:255.
		/// </summary>
		public static Color LightBlue => new Color(4293318829u);

		/// <summary>
		/// Gets a system-defined color with the value R:240 G:128 B:128 A:255.
		/// </summary>
		public static Color LightCoral => new Color(4286611696u);

		/// <summary>
		/// Gets a system-defined color with the value R:224 G:255 B:255 A:255.
		/// </summary>
		public static Color LightCyan => new Color(4294967264u);

		/// <summary>
		/// Gets a system-defined color with the value R:250 G:250 B:210 A:255.
		/// </summary>
		public static Color LightGoldenrodYellow => new Color(4292016890u);

		/// <summary>
		/// Gets a system-defined color with the value R:144 G:238 B:144 A:255.
		/// </summary>
		public static Color LightGreen => new Color(4287688336u);

		/// <summary>
		/// Gets a system-defined color with the value R:211 G:211 B:211 A:255.
		/// </summary>
		public static Color LightGray => new Color(4292072403u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:182 B:193 A:255.
		/// </summary>
		public static Color LightPink => new Color(4290885375u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:160 B:122 A:255.
		/// </summary>
		public static Color LightSalmon => new Color(4286226687u);

		/// <summary>
		/// Gets a system-defined color with the value R:32 G:178 B:170 A:255.
		/// </summary>
		public static Color LightSeaGreen => new Color(4289376800u);

		/// <summary>
		/// Gets a system-defined color with the value R:135 G:206 B:250 A:255.
		/// </summary>
		public static Color LightSkyBlue => new Color(4294626951u);

		/// <summary>
		/// Gets a system-defined color with the value R:119 G:136 B:153 A:255.
		/// </summary>
		public static Color LightSlateGray => new Color(4288252023u);

		/// <summary>
		/// Gets a system-defined color with the value R:176 G:196 B:222 A:255.
		/// </summary>
		public static Color LightSteelBlue => new Color(4292789424u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:255 B:224 A:255.
		/// </summary>
		public static Color LightYellow => new Color(4292935679u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:255 B:0 A:255.
		/// </summary>
		public static Color Lime => new Color(4278255360u);

		/// <summary>
		/// Gets a system-defined color with the value R:50 G:205 B:50 A:255.
		/// </summary>
		public static Color LimeGreen => new Color(4281519410u);

		/// <summary>
		/// Gets a system-defined color with the value R:250 G:240 B:230 A:255.
		/// </summary>
		public static Color Linen => new Color(4293325050u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:0 B:255 A:255.
		/// </summary>
		public static Color Magenta => new Color(4294902015u);

		/// <summary>
		/// Gets a system-defined color with the value R:128 G:0 B:0 A:255.
		/// </summary>
		public static Color Maroon => new Color(4278190208u);

		/// <summary>
		/// Gets a system-defined color with the value R:102 G:205 B:170 A:255.
		/// </summary>
		public static Color MediumAquamarine => new Color(4289383782u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:0 B:205 A:255.
		/// </summary>
		public static Color MediumBlue => new Color(4291624960u);

		/// <summary>
		/// Gets a system-defined color with the value R:186 G:85 B:211 A:255.
		/// </summary>
		public static Color MediumOrchid => new Color(4292040122u);

		/// <summary>
		/// Gets a system-defined color with the value R:147 G:112 B:219 A:255.
		/// </summary>
		public static Color MediumPurple => new Color(4292571283u);

		/// <summary>
		/// Gets a system-defined color with the value R:60 G:179 B:113 A:255.
		/// </summary>
		public static Color MediumSeaGreen => new Color(4285641532u);

		/// <summary>
		/// Gets a system-defined color with the value R:123 G:104 B:238 A:255.
		/// </summary>
		public static Color MediumSlateBlue => new Color(4293814395u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:250 B:154 A:255.
		/// </summary>
		public static Color MediumSpringGreen => new Color(4288346624u);

		/// <summary>
		/// Gets a system-defined color with the value R:72 G:209 B:204 A:255.
		/// </summary>
		public static Color MediumTurquoise => new Color(4291613000u);

		/// <summary>
		/// Gets a system-defined color with the value R:199 G:21 B:133 A:255.
		/// </summary>
		public static Color MediumVioletRed => new Color(4286911943u);

		/// <summary>
		/// Gets a system-defined color with the value R:25 G:25 B:112 A:255.
		/// </summary>
		public static Color MidnightBlue => new Color(4285536537u);

		/// <summary>
		/// Gets a system-defined color with the value R:245 G:255 B:250 A:255.
		/// </summary>
		public static Color MintCream => new Color(4294639605u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:228 B:225 A:255.
		/// </summary>
		public static Color MistyRose => new Color(4292994303u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:228 B:181 A:255.
		/// </summary>
		public static Color Moccasin => new Color(4290110719u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:222 B:173 A:255.
		/// </summary>
		public static Color NavajoWhite => new Color(4289584895u);

		/// <summary>
		/// Gets a system-defined color R:0 G:0 B:128 A:255.
		/// </summary>
		public static Color Navy => new Color(4286578688u);

		/// <summary>
		/// Gets a system-defined color with the value R:253 G:245 B:230 A:255.
		/// </summary>
		public static Color OldLace => new Color(4293326333u);

		/// <summary>
		/// Gets a system-defined color with the value R:128 G:128 B:0 A:255.
		/// </summary>
		public static Color Olive => new Color(4278222976u);

		/// <summary>
		/// Gets a system-defined color with the value R:107 G:142 B:35 A:255.
		/// </summary>
		public static Color OliveDrab => new Color(4280520299u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:165 B:0 A:255.
		/// </summary>
		public static Color Orange => new Color(4278232575u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:69 B:0 A:255.
		/// </summary>
		public static Color OrangeRed => new Color(4278207999u);

		/// <summary>
		/// Gets a system-defined color with the value R:218 G:112 B:214 A:255.
		/// </summary>
		public static Color Orchid => new Color(4292243674u);

		/// <summary>
		/// Gets a system-defined color with the value R:238 G:232 B:170 A:255.
		/// </summary>
		public static Color PaleGoldenrod => new Color(4289390830u);

		/// <summary>
		/// Gets a system-defined color with the value R:152 G:251 B:152 A:255.
		/// </summary>
		public static Color PaleGreen => new Color(4288215960u);

		/// <summary>
		/// Gets a system-defined color with the value R:175 G:238 B:238 A:255.
		/// </summary>
		public static Color PaleTurquoise => new Color(4293848751u);

		/// <summary>
		/// Gets a system-defined color with the value R:219 G:112 B:147 A:255.
		/// </summary>
		public static Color PaleVioletRed => new Color(4287852763u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:239 B:213 A:255.
		/// </summary>
		public static Color PapayaWhip => new Color(4292210687u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:218 B:185 A:255.
		/// </summary>
		public static Color PeachPuff => new Color(4290370303u);

		/// <summary>
		/// Gets a system-defined color with the value R:205 G:133 B:63 A:255.
		/// </summary>
		public static Color Peru => new Color(4282353101u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:192 B:203 A:255.
		/// </summary>
		public static Color Pink => new Color(4291543295u);

		/// <summary>
		/// Gets a system-defined color with the value R:221 G:160 B:221 A:255.
		/// </summary>
		public static Color Plum => new Color(4292714717u);

		/// <summary>
		/// Gets a system-defined color with the value R:176 G:224 B:230 A:255.
		/// </summary>
		public static Color PowderBlue => new Color(4293320880u);

		/// <summary>
		/// Gets a system-defined color with the value R:128 G:0 B:128 A:255.
		/// </summary>
		public static Color Purple => new Color(4286578816u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:0 B:0 A:255.
		/// </summary>
		public static Color Red => new Color(4278190335u);

		/// <summary>
		/// Gets a system-defined color with the value R:188 G:143 B:143 A:255.
		/// </summary>
		public static Color RosyBrown => new Color(4287598524u);

		/// <summary>
		/// Gets a system-defined color with the value R:65 G:105 B:225 A:255.
		/// </summary>
		public static Color RoyalBlue => new Color(4292962625u);

		/// <summary>
		/// Gets a system-defined color with the value R:139 G:69 B:19 A:255.
		/// </summary>
		public static Color SaddleBrown => new Color(4279453067u);

		/// <summary>
		/// Gets a system-defined color with the value R:250 G:128 B:114 A:255.
		/// </summary>
		public static Color Salmon => new Color(4285694202u);

		/// <summary>
		/// Gets a system-defined color with the value R:244 G:164 B:96 A:255.
		/// </summary>
		public static Color SandyBrown => new Color(4284523764u);

		/// <summary>
		/// Gets a system-defined color with the value R:46 G:139 B:87 A:255.
		/// </summary>
		public static Color SeaGreen => new Color(4283927342u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:245 B:238 A:255.
		/// </summary>
		public static Color SeaShell => new Color(4293850623u);

		/// <summary>
		/// Gets a system-defined color with the value R:160 G:82 B:45 A:255.
		/// </summary>
		public static Color Sienna => new Color(4281160352u);

		/// <summary>
		/// Gets a system-defined color with the value R:192 G:192 B:192 A:255.
		/// </summary>
		public static Color Silver => new Color(4290822336u);

		/// <summary>
		/// Gets a system-defined color with the value R:135 G:206 B:235 A:255.
		/// </summary>
		public static Color SkyBlue => new Color(4293643911u);

		/// <summary>
		/// Gets a system-defined color with the value R:106 G:90 B:205 A:255.
		/// </summary>
		public static Color SlateBlue => new Color(4291648106u);

		/// <summary>
		/// Gets a system-defined color with the value R:112 G:128 B:144 A:255.
		/// </summary>
		public static Color SlateGray => new Color(4287660144u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:250 B:250 A:255.
		/// </summary>
		public static Color Snow => new Color(4294638335u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:255 B:127 A:255.
		/// </summary>
		public static Color SpringGreen => new Color(4286578432u);

		/// <summary>
		/// Gets a system-defined color with the value R:70 G:130 B:180 A:255.
		/// </summary>
		public static Color SteelBlue => new Color(4290019910u);

		/// <summary>
		/// Gets a system-defined color with the value R:210 G:180 B:140 A:255.
		/// </summary>
		public static Color Tan => new Color(4287411410u);

		/// <summary>
		/// Gets a system-defined color with the value R:0 G:128 B:128 A:255.
		/// </summary>
		public static Color Teal => new Color(4286611456u);

		/// <summary>
		/// Gets a system-defined color with the value R:216 G:191 B:216 A:255.
		/// </summary>
		public static Color Thistle => new Color(4292394968u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:99 B:71 A:255.
		/// </summary>
		public static Color Tomato => new Color(4282868735u);

		/// <summary>
		/// Gets a system-defined color with the value R:64 G:224 B:208 A:255.
		/// </summary>
		public static Color Turquoise => new Color(4291878976u);

		/// <summary>
		/// Gets a system-defined color with the value R:238 G:130 B:238 A:255.
		/// </summary>
		public static Color Violet => new Color(4293821166u);

		/// <summary>
		/// Gets a system-defined color with the value R:245 G:222 B:179 A:255.
		/// </summary>
		public static Color Wheat => new Color(4289978101u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:255 B:255 A:255.
		/// </summary>
		public static Color White => new Color(uint.MaxValue);

		/// <summary>
		/// Gets a system-defined color with the value R:245 G:245 B:245 A:255.
		/// </summary>
		public static Color WhiteSmoke => new Color(4294309365u);

		/// <summary>
		/// Gets a system-defined color with the value R:255 G:255 B:0 A:255.
		/// </summary>
		public static Color Yellow => new Color(4278255615u);

		/// <summary>
		/// Gets a system-defined color with the value R:154 G:205 B:50 A:255.
		/// </summary>
		public static Color YellowGreen => new Color(4281519514u);

		uint IPackedVector<uint>.PackedValue
		{
			get
			{
				return PackedValue;
			}
			set
			{
				PackedValue = value;
			}
		}

		public Color(uint packedValue)
		{
			PackedValue = packedValue;
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="r">Red component.</param><param name="g">Green component.</param><param name="b">Blue component.</param>
		public Color(int r, int g, int b)
		{
			if (((r | g | b) & -256) != 0)
			{
				r = ClampToByte64(r);
				g = ClampToByte64(g);
				b = ClampToByte64(b);
			}
			g <<= 8;
			b <<= 16;
			PackedValue = (uint)(r | g | b | -16777216);
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="r">Red component.</param><param name="g">Green component.</param><param name="b">Blue component.</param><param name="a">Alpha component.</param>
		public Color(int r, int g, int b, int a)
		{
			if (((r | g | b | a) & -256) != 0)
			{
				r = ClampToByte32(r);
				g = ClampToByte32(g);
				b = ClampToByte32(b);
				a = ClampToByte32(a);
			}
			g <<= 8;
			b <<= 16;
			a <<= 24;
			PackedValue = (uint)(r | g | b | a);
		}

		public Color(float rgb)
		{
			PackedValue = PackHelper(rgb, rgb, rgb, 1f);
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="r">Red component.</param><param name="g">Green component.</param><param name="b">Blue component.</param>
		public Color(float r, float g, float b)
		{
			PackedValue = PackHelper(r, g, b, 1f);
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="r">Red component.</param><param name="g">Green component.</param><param name="b">Blue component.</param><param name="a">Alpha component.</param>
		public Color(float r, float g, float b, float a)
		{
			PackedValue = PackHelper(r, g, b, a);
		}

		public Color(Color color, float a)
		{
			PackedValue = PackHelper((float)(int)color.R / 255f, (float)(int)color.G / 255f, (float)(int)color.B / 255f, a);
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="vector">A three-component color.</param>
		public Color(Vector3 vector)
		{
			PackedValue = PackHelper(vector.X, vector.Y, vector.Z, 1f);
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="vector">A four-component color.</param>
		public Color(Vector4 vector)
		{
			PackedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
		}

		/// <summary>
		/// Multiply operator.
		/// </summary>
		/// <param name="value">A four-component color</param><param name="scale">Scale factor.</param>
		public static Color operator *(Color value, float scale)
		{
			uint packedValue = value.PackedValue;
			uint num = (byte)packedValue;
			uint num2 = (byte)(packedValue >> 8);
			uint num3 = (byte)(packedValue >> 16);
			byte num4 = (byte)(packedValue >> 24);
			scale *= 65536f;
			uint num5 = ((double)scale >= 0.0) ? (((double)scale <= 16777215.0) ? ((uint)scale) : 16777215u) : 0u;
			uint num6 = num * num5 >> 16;
			uint num7 = num2 * num5 >> 16;
			uint num8 = num3 * num5 >> 16;
			uint num9 = num4 * num5 >> 16;
			if (num6 > 255)
			{
				num6 = 255u;
			}
			if (num7 > 255)
			{
				num7 = 255u;
			}
			if (num8 > 255)
			{
				num8 = 255u;
			}
			if (num9 > 255)
			{
				num9 = 255u;
			}
			Color result = default(Color);
			result.PackedValue = (num6 | (num7 << 8) | (num8 << 16) | (num9 << 24));
			return result;
		}

		public static Color operator +(Color value, Color other)
		{
			return new Color(value.R + other.R, value.G + other.G, value.B + other.B, value.A + other.A);
		}

		/// <summary>
		/// Multiply operator.
		/// </summary>
		/// <param name="value">A four-component color</param><param name="other">Multiplied color.</param>
		public static Color operator *(Color value, Color other)
		{
			Vector4 vector = value.ToVector4();
			Vector4 vector2 = other.ToVector4();
			return new Color(vector.X * vector2.X, vector.Y * vector2.Y, vector.Z * vector2.Z, vector.W * vector2.W);
		}

		/// <summary>
		/// Equality operator.
		/// </summary>
		/// <param name="a">A four-component color.</param><param name="b">A four-component color.</param>
		public static bool operator ==(Color a, Color b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Equality operator for Testing two color objects to see if they are equal.
		/// </summary>
		/// <param name="a">A four-component color.</param><param name="b">A four-component color.</param>
		public static bool operator !=(Color a, Color b)
		{
			return !a.Equals(b);
		}

		void IPackedVector.PackFromVector4(Vector4 vector)
		{
			PackedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
		}

		/// <summary>
		/// Convert a non premultipled color into color data that contains alpha.
		/// </summary>
		/// <param name="vector">A four-component color.</param>
		public static Color FromNonPremultiplied(Vector4 vector)
		{
			Color result = default(Color);
			result.PackedValue = PackHelper(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W, vector.W);
			return result;
		}

		/// <summary>
		/// Converts a non-premultipled alpha color to a color that contains premultiplied alpha.
		/// </summary>
		/// <param name="r">Red component.</param><param name="g">Green component.</param><param name="b">Blue component.</param><param name="a">Alpha component.</param>
		public static Color FromNonPremultiplied(int r, int g, int b, int a)
		{
			r = ClampToByte64((long)r * (long)a / 255);
			g = ClampToByte64((long)g * (long)a / 255);
			b = ClampToByte64((long)b * (long)a / 255);
			a = ClampToByte32(a);
			g <<= 8;
			b <<= 16;
			a <<= 24;
			Color result = default(Color);
			result.PackedValue = (uint)(r | g | b | a);
			return result;
		}

		private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
		{
			return PackUtils.PackUNorm(255f, vectorX) | (PackUtils.PackUNorm(255f, vectorY) << 8) | (PackUtils.PackUNorm(255f, vectorZ) << 16) | (PackUtils.PackUNorm(255f, vectorW) << 24);
		}

		private static int ClampToByte32(int value)
		{
			if (value < 0)
			{
				return 0;
			}
			if (value > 255)
			{
				return 255;
			}
			return value;
		}

		private static int ClampToByte64(long value)
		{
			if (value < 0)
			{
				return 0;
			}
			if (value > 255)
			{
				return 255;
			}
			return (int)value;
		}

		/// <summary>
		/// Gets a three-component vector representation for this object.
		/// </summary>
		public Vector3 ToVector3()
		{
			Vector3 result = default(Vector3);
			result.X = PackUtils.UnpackUNorm(255u, PackedValue);
			result.Y = PackUtils.UnpackUNorm(255u, PackedValue >> 8);
			result.Z = PackUtils.UnpackUNorm(255u, PackedValue >> 16);
			return result;
		}

		/// <summary>
		/// Gets a four-component vector representation for this object.
		/// </summary>
		public Vector4 ToVector4()
		{
			Vector4 result = default(Vector4);
			result.X = PackUtils.UnpackUNorm(255u, PackedValue);
			result.Y = PackUtils.UnpackUNorm(255u, PackedValue >> 8);
			result.Z = PackUtils.UnpackUNorm(255u, PackedValue >> 16);
			result.W = PackUtils.UnpackUNorm(255u, PackedValue >> 24);
			return result;
		}

		/// <summary>
		/// Linearly interpolate a color.
		/// </summary>
		/// <param name="value1">A four-component color.</param><param name="value2">A four-component color.</param><param name="amount">Interpolation factor.</param>
		public static Color Lerp(Color value1, Color value2, float amount)
		{
			uint packedValue = value1.PackedValue;
			uint packedValue2 = value2.PackedValue;
			int num = (byte)packedValue;
			int num2 = (byte)(packedValue >> 8);
			int num3 = (byte)(packedValue >> 16);
			int num4 = (byte)(packedValue >> 24);
			int num5 = (byte)packedValue2;
			int num6 = (byte)(packedValue2 >> 8);
			int num7 = (byte)(packedValue2 >> 16);
			int num8 = (byte)(packedValue2 >> 24);
			int num9 = (int)PackUtils.PackUNorm(65536f, amount);
			int num10 = num + ((num5 - num) * num9 >> 16);
			int num11 = num2 + ((num6 - num2) * num9 >> 16);
			int num12 = num3 + ((num7 - num3) * num9 >> 16);
			int num13 = num4 + ((num8 - num4) * num9 >> 16);
			Color result = default(Color);
			result.PackedValue = (uint)(num10 | (num11 << 8) | (num12 << 16) | (num13 << 24));
			return result;
		}

		/// <summary>
		/// Multiply each color component by the scale factor.
		/// </summary>
		/// <param name="value">The source, four-component color.</param><param name="scale">The scale factor.</param>
		public static Color Multiply(Color value, float scale)
		{
			uint packedValue = value.PackedValue;
			uint num = (byte)packedValue;
			uint num2 = (byte)(packedValue >> 8);
			uint num3 = (byte)(packedValue >> 16);
			byte num4 = (byte)(packedValue >> 24);
			scale *= 65536f;
			uint num5 = ((double)scale >= 0.0) ? (((double)scale <= 16777215.0) ? ((uint)scale) : 16777215u) : 0u;
			uint num6 = num * num5 >> 16;
			uint num7 = num2 * num5 >> 16;
			uint num8 = num3 * num5 >> 16;
			uint num9 = num4 * num5 >> 16;
			if (num6 > 255)
			{
				num6 = 255u;
			}
			if (num7 > 255)
			{
				num7 = 255u;
			}
			if (num8 > 255)
			{
				num8 = 255u;
			}
			if (num9 > 255)
			{
				num9 = 255u;
			}
			Color result = default(Color);
			result.PackedValue = (num6 | (num7 << 8) | (num8 << 16) | (num9 << 24));
			return result;
		}

		/// <summary>
		/// Gets a string representation of this object.
		/// </summary>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", R, G, B, A);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		public override int GetHashCode()
		{
			return PackedValue.GetHashCode();
		}

		/// <summary>
		/// Test an instance of a color object to see if it is equal to this object.
		/// </summary>
		/// <param name="obj">A color object.</param>
		public override bool Equals(object obj)
		{
			if (obj is Color)
			{
				return Equals((Color)obj);
			}
			return false;
		}

		/// <summary>
		/// Test a color to see if it is equal to the color in this instance.
		/// </summary>
		/// <param name="other">A four-component color.</param>
		public bool Equals(Color other)
		{
			return PackedValue.Equals(other.PackedValue);
		}

		public static implicit operator Color(Vector3 v)
		{
			return new Color(v.X, v.Y, v.Z, 1f);
		}

		public static implicit operator Vector3(Color v)
		{
			return v.ToVector3();
		}

		public static implicit operator Color(Vector4 v)
		{
			return new Color(v.X, v.Y, v.Z, v.W);
		}

		public static implicit operator Vector4(Color v)
		{
			return v.ToVector4();
		}

		public static Color Lighten(Color inColor, double inAmount)
		{
			return new Color((int)Math.Min(255.0, (double)(int)inColor.R + 255.0 * inAmount), (int)Math.Min(255.0, (double)(int)inColor.G + 255.0 * inAmount), (int)Math.Min(255.0, (double)(int)inColor.B + 255.0 * inAmount), inColor.A);
		}

		public static Color Darken(Color inColor, double inAmount)
		{
			return new Color((int)Math.Max(0.0, (double)(int)inColor.R - 255.0 * inAmount), (int)Math.Max(0.0, (double)(int)inColor.G - 255.0 * inAmount), (int)Math.Max(0.0, (double)(int)inColor.B - 255.0 * inAmount), inColor.A);
		}

		public Color ToGray()
		{
			Vector4 vector = this;
			float num = 0.2989f * vector.X + 0.587f * vector.Y + 0.114f * vector.Z;
			return new Color(num, num, num, vector.W);
		}
	}
}
