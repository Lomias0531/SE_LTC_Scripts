using System.Xml.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Messages
{
	public struct MySubGlare
	{
		public struct KeyPoint
		{
			public float Occlusion;

			public float Intensity;
		}

		[XmlIgnore]
		public MyStringId Material;

		public SubGlareType Type;

		public Vector4 Color;

		public bool FixedSize;

		public Vector2 Size;

		public float ScreenIntensityMultiplierCenter;

		public float ScreenIntensityMultiplierEdge;

		public Vector2 ScreenCenterDistance;

		public KeyPoint[] OcclusionToIntensityCurve;

		[XmlElement(ElementName = "Material")]
		public string MaterialName
		{
			get
			{
				return Material.String;
			}
			set
			{
				Material = MyStringId.GetOrCompute(value);
			}
		}
	}
}
