using System.Xml.Serialization;
using VRageMath;

namespace VRage.Game
{
	public struct ColorDefinitionHSV
	{
		[XmlAttribute]
		public int H;

		[XmlAttribute]
		public int S;

		[XmlAttribute]
		public int V;

		[XmlAttribute]
		public int Hue
		{
			get
			{
				return H;
			}
			set
			{
				H = value;
			}
		}

		[XmlAttribute]
		public int Saturation
		{
			get
			{
				return S;
			}
			set
			{
				S = value;
			}
		}

		[XmlAttribute]
		public int Value
		{
			get
			{
				return V;
			}
			set
			{
				V = value;
			}
		}

		public bool ShouldSerializeHue()
		{
			return false;
		}

		public bool ShouldSerializeSaturation()
		{
			return false;
		}

		public bool ShouldSerializeValue()
		{
			return false;
		}

		public bool IsValid()
		{
			if (H >= 0 && H <= 360 && S >= -100 && S <= 100 && V >= -100)
			{
				return V <= 100;
			}
			return false;
		}

		public static implicit operator Vector3(ColorDefinitionHSV definition)
		{
			definition.H %= 360;
			if (definition.H < 0)
			{
				definition.H += 360;
			}
			return new Vector3((float)definition.H / 360f, MathHelper.Clamp((float)definition.S / 100f, -1f, 1f), MathHelper.Clamp((float)definition.V / 100f, -1f, 1f));
		}

		public static implicit operator ColorDefinitionHSV(Vector3 vector)
		{
			ColorDefinitionHSV result = default(ColorDefinitionHSV);
			result.H = (int)MathHelper.Clamp(vector.Z * 100f, -100f, 100f);
			result.S = (int)MathHelper.Clamp(vector.Y * 100f, -100f, 100f);
			result.V = (int)MathHelper.Clamp(vector.Z * 360f, 0f, 360f);
			return result;
		}

		public override string ToString()
		{
			return $"H:{(int)H} S:{(int)S} V:{(int)V}";
		}
	}
}
