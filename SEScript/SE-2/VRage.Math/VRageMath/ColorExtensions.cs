using System;
using System.Drawing;

namespace VRageMath
{
	public static class ColorExtensions
	{
		public static Vector3 ColorToHSV(this Color rgb)
		{
			System.Drawing.Color color = System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
			int num = Math.Max(color.R, Math.Max(color.G, color.B));
			int num2 = Math.Min(color.R, Math.Min(color.G, color.B));
			float x = color.GetHue() / 360f;
			float y = (num == 0) ? 0f : (1f - 1f * (float)num2 / (float)num);
			float z = (float)num / 255f;
			return new Vector3(x, y, z);
		}

		/// <summary>
		/// Use this for HSV in DX11 Renderer, X = Hue 0..1, Y = Saturation -1..1, Z = Value -1..1
		/// </summary>
		public static Vector3 ColorToHSVDX11(this Color rgb)
		{
			System.Drawing.Color color = System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
			int num = Math.Max(color.R, Math.Max(color.G, color.B));
			int num2 = Math.Min(color.R, Math.Min(color.G, color.B));
			float x = color.GetHue() / 360f;
			float y = (num == 0) ? (-1f) : (1f - 2f * (float)num2 / (float)num);
			float z = -1f + 2f * (float)num / 255f;
			return new Vector3(x, y, z);
		}

		public static Color HexToColor(string hex)
		{
			if (hex.Length > 0 && !hex.StartsWith("#"))
			{
				hex = "#" + hex;
			}
			Color? color = FromHtml(hex);
			if (!color.HasValue)
			{
				return Color.Pink;
			}
			return color.Value;
		}

		public static Vector4 HexToVector4(string hex)
		{
			return HexToColor(hex).ToVector4();
		}

		public static Color? FromHtml(string htmlColor)
		{
			if (!string.IsNullOrEmpty(htmlColor) && htmlColor[0] == '#')
			{
				if (htmlColor.Length == 7)
				{
					return new Color(Convert.ToInt32(htmlColor.Substring(1, 2), 16), Convert.ToInt32(htmlColor.Substring(3, 2), 16), Convert.ToInt32(htmlColor.Substring(5, 2), 16));
				}
				if (htmlColor.Length == 4)
				{
					string text = char.ToString(htmlColor[1]);
					string text2 = char.ToString(htmlColor[2]);
					string text3 = char.ToString(htmlColor[3]);
					return new Color(Convert.ToInt32(text + text, 16), Convert.ToInt32(text2 + text2, 16), Convert.ToInt32(text3 + text3, 16));
				}
			}
			return null;
		}

		private static Vector3 Hue(float H)
		{
			float value = Math.Abs(H * 6f - 3f) - 1f;
			float value2 = 2f - Math.Abs(H * 6f - 2f);
			return new Vector3(z: MathHelper.Clamp(2f - Math.Abs(H * 6f - 4f), 0f, 1f), x: MathHelper.Clamp(value, 0f, 1f), y: MathHelper.Clamp(value2, 0f, 1f));
		}

		public static Color HSVtoColor(this Vector3 HSV)
		{
			return new Color(((Hue(HSV.X) - 1f) * HSV.Y + 1f) * HSV.Z);
		}

		public static uint PackHSVToUint(this Vector3 HSV)
		{
			int num = (int)Math.Round(HSV.X * 360f);
			int num2 = (int)Math.Round(HSV.Y * 100f + 100f);
			int num3 = (int)Math.Round(HSV.Z * 100f + 100f);
			num2 <<= 16;
			num3 <<= 24;
			return (uint)(num | num2 | num3);
		}

		public static Vector3 UnpackHSVFromUint(uint packed)
		{
			ushort num = (ushort)packed;
			byte b = (byte)(packed >> 16);
			byte b2 = (byte)(packed >> 24);
			return new Vector3((float)(int)num / 360f, (float)(b - 100) / 100f, (float)(b2 - 100) / 100f);
		}

		public static float HueDistance(this Color color, float hue)
		{
			float num = Math.Abs(color.ColorToHSV().X - hue);
			return Math.Min(num, 1f - num);
		}

		public static float HueDistance(this Color color, Color otherColor)
		{
			return color.HueDistance(otherColor.ColorToHSV().X);
		}

		public static Vector3 TemperatureToRGB(float temperature)
		{
			Vector3 result = default(Vector3);
			temperature /= 100f;
			if (temperature <= 66f)
			{
				result.X = 1f;
				result.Y = (float)MathHelper.Saturate(0.390081579 * Math.Log(temperature) - 0.631841444);
			}
			else
			{
				float num = temperature - 60f;
				result.X = (float)MathHelper.Saturate(1.292936186 * Math.Pow(num, -0.1332047592));
				result.Y = (float)MathHelper.Saturate(1.129890861 * Math.Pow(num, -0.0755148492));
			}
			if (temperature >= 66f)
			{
				result.Z = 1f;
			}
			else if (temperature <= 19f)
			{
				result.Z = 0f;
			}
			else
			{
				result.Z = (float)MathHelper.Saturate(0.543206789 * Math.Log(temperature - 10f) - 1.196254089);
			}
			return result;
		}

		public static Vector4 UnmultiplyColor(this Vector4 c)
		{
			if (c.W == 0f)
			{
				return Vector4.Zero;
			}
			return new Vector4(c.X / c.W, c.Y / c.W, c.Z / c.W, c.W);
		}

		public static Vector4 PremultiplyColor(this Vector4 c)
		{
			return new Vector4(c.X * c.W, c.Y * c.W, c.Z * c.W, c.W);
		}

		public static Vector4 ToSRGB(this Vector4 c)
		{
			return new Vector4(ToSRGBComponent(c.X), ToSRGBComponent(c.Y), ToSRGBComponent(c.Z), c.W);
		}

		public static Vector4 ToLinearRGB(this Vector4 c)
		{
			return new Vector4(ToLinearRGBComponent(c.X), ToLinearRGBComponent(c.Y), ToLinearRGBComponent(c.Z), c.W);
		}

		public static Vector3 ToLinearRGB(this Vector3 c)
		{
			return new Vector3(ToLinearRGBComponent(c.X), ToLinearRGBComponent(c.Y), ToLinearRGBComponent(c.Z));
		}

		public static Vector3 ToSRGB(this Vector3 c)
		{
			return new Vector3(ToSRGBComponent(c.X), ToSRGBComponent(c.Y), ToSRGBComponent(c.Z));
		}

		public static Vector3 ToGray(this Vector3 c)
		{
			return new Vector3(0.2126f * c.X + 0.7152f * c.Y + 0.0722f * c.Z);
		}

		public static Vector4 ToGray(this Vector4 c)
		{
			float num = 0.2126f * c.X + 0.7152f * c.Y + 0.0722f * c.Z;
			return new Vector4(num, num, num, c.W);
		}

		public static float ToLinearRGBComponent(float c)
		{
			if (!(c <= 0.04045f))
			{
				return (float)Math.Pow((c + 0.055f) / 1.055f, 2.4000000953674316);
			}
			return c / 12.92f;
		}

		public static float ToSRGBComponent(float c)
		{
			if (!(c <= 0.0031308f))
			{
				return (float)Math.Pow(c, 0.4166666567325592) * 1.055f - 0.055f;
			}
			return c * 12.92f;
		}

		public static Color Shade(this Color c, float r)
		{
			return new Color((int)((float)(int)c.R * r), (int)((float)(int)c.G * r), (int)((float)(int)c.B * r), c.A);
		}

		public static Color Tint(this Color c, float r)
		{
			return new Color((int)((float)(int)c.R + (float)(255 - c.R) * r), (int)((float)(int)c.G + (float)(255 - c.G) * r), (int)((float)(int)c.B + (float)(255 - c.B) * r), c.A);
		}

		public static Color Alpha(this Color c, float a)
		{
			return new Color(c, a);
		}
	}
}
