using System;
using System.Xml.Serialization;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
	public struct MySunProperties
	{
		private static class Defaults
		{
			public const float SunIntensity = 1f;

			public static readonly Vector3 SunDirectionNormalized = new Vector3(0.339467347f, 0.709795356f, -0.617213368f);

			public static readonly Vector3 BaseSunDirectionNormalized = new Vector3(0.339467347f, 0.709795356f, -0.617213368f);

			public const int EnvMapResolution = 512;

			public const int EnvMapFilteredResolution = 256;
		}

		[StructDefault]
		public static readonly MySunProperties Default;

		public float SunIntensity;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyEnvironmentLightData>))]
		public MyEnvironmentLightData EnvironmentLight;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyEnvironmentProbeData>))]
		public MyEnvironmentProbeData EnvironmentProbe;

		[XmlElement(Type = typeof(MyStructXmlSerializer<MyTextureDebugMultipliers>))]
		public MyTextureDebugMultipliers TextureMultipliers;

		/// <summary>Direction TO sun</summary>
		public Vector3 BaseSunDirectionNormalized;

		/// <summary>Direction TO sun</summary>
		public Vector3 SunDirectionNormalized;

		public int EnvMapResolution;

		public int EnvMapFilteredResolution;

		public MyEnvironmentData EnvironmentData
		{
			get
			{
				MyEnvironmentData myEnvironmentData = default(MyEnvironmentData);
				myEnvironmentData.EnvironmentLight = EnvironmentLight;
				myEnvironmentData.EnvironmentProbe = EnvironmentProbe;
				myEnvironmentData.TextureMultipliers = TextureMultipliers;
				myEnvironmentData.EnvMapResolution = EnvMapResolution;
				myEnvironmentData.EnvMapFilteredResolution = EnvMapFilteredResolution;
				MyEnvironmentData result = myEnvironmentData;
				result.EnvironmentLight.SunColorRaw = EnvironmentLight.SunColorRaw * SunIntensity;
				return result;
			}
		}

		public Vector3 SunRotationAxis
		{
			get
			{
				Vector3 result = (!(Math.Abs(Vector3.Dot(BaseSunDirectionNormalized, Vector3.Up)) > 0.95f)) ? Vector3.Cross(Vector3.Cross(BaseSunDirectionNormalized, Vector3.Up), BaseSunDirectionNormalized) : Vector3.Cross(Vector3.Cross(BaseSunDirectionNormalized, Vector3.Left), BaseSunDirectionNormalized);
				result.Normalize();
				return result;
			}
		}

		static MySunProperties()
		{
			Default = new MySunProperties
			{
				SunIntensity = 1f,
				EnvironmentLight = MyEnvironmentLightData.Default,
				EnvironmentProbe = MyEnvironmentProbeData.Default,
				SunDirectionNormalized = Defaults.SunDirectionNormalized,
				BaseSunDirectionNormalized = Defaults.BaseSunDirectionNormalized,
				TextureMultipliers = MyTextureDebugMultipliers.Defaults,
				EnvMapResolution = 512,
				EnvMapFilteredResolution = 256
			};
		}
	}
}
