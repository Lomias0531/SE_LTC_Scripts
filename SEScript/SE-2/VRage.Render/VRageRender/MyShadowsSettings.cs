using System;
using System.Xml.Serialization;
using VRage;

namespace VRageRender
{
	public class MyShadowsSettings
	{
		[XmlType("MyShadowSettings.Struct")]
		public struct Struct
		{
			[StructDefault]
			public static readonly Struct Default;

			public bool UpdateCascadesEveryFrame;

			public bool EnableShadowBlur;

			public float ShadowCascadeMaxDistance;

			public float ShadowCascadeMaxDistanceMultiplierMedium;

			public float ShadowCascadeMaxDistanceMultiplierHigh;

			public float ShadowCascadeMaxDistanceMultiplierExtreme;

			public float ShadowCascadeSpreadFactor;

			public float ShadowCascadeZOffset;

			public float ReflectorShadowDistanceLow;

			public float ReflectorShadowDistanceMedium;

			public float ReflectorShadowDistanceHigh;

			public float ReflectorShadowDistanceExtreme;

			public float LightDirectionDifferenceThreshold;

			public float LightDirectionChangeDelayMultiplier;

			public float ZBias;

			public int CascadesCount;

			static Struct()
			{
				Default = new Struct
				{
					UpdateCascadesEveryFrame = false,
					EnableShadowBlur = true,
					ShadowCascadeMaxDistance = 300f,
					ShadowCascadeMaxDistanceMultiplierMedium = 2f,
					ShadowCascadeMaxDistanceMultiplierHigh = 3.5f,
					ShadowCascadeSpreadFactor = 0.5f,
					ShadowCascadeZOffset = 400f,
					ReflectorShadowDistanceLow = 0.2f,
					ReflectorShadowDistanceMedium = 0.4f,
					ReflectorShadowDistanceHigh = 0.8f,
					LightDirectionDifferenceThreshold = 0.0175f,
					LightDirectionChangeDelayMultiplier = 18f,
					ZBias = 0.01f,
					CascadesCount = 6
				};
			}
		}

		[XmlType("MyShadowSettings.Cascade")]
		public struct Cascade
		{
			public float FullCoverageDepth;

			public float ExtendedCoverageDepth;

			public float ShadowNormalOffset;

			public float SkippingSmallObjectThreshold;
		}

		private float[] m_shadowCascadeSmallSkipThresholds;

		private bool[] m_shadowCascadeFrozen;

		[XmlElement(Type = typeof(MyStructXmlSerializer<Struct>))]
		public Struct Data = Struct.Default;

		private Cascade[] m_cascades;

		[XmlArrayItem("Value")]
		public float[] ShadowCascadeSmallSkipThresholds
		{
			get
			{
				return m_shadowCascadeSmallSkipThresholds;
			}
			set
			{
				if (ShadowCascadeSmallSkipThresholds.Length != value.Length)
				{
					ShadowCascadeSmallSkipThresholds = new float[value.Length];
				}
				value.CopyTo(ShadowCascadeSmallSkipThresholds, 0);
			}
		}

		[XmlIgnore]
		public bool[] ShadowCascadeFrozen
		{
			get
			{
				return m_shadowCascadeFrozen;
			}
			set
			{
				if (ShadowCascadeFrozen.Length != value.Length)
				{
					ShadowCascadeFrozen = new bool[value.Length];
				}
				value.CopyTo(ShadowCascadeFrozen, 0);
			}
		}

		[XmlArrayItem("Cascade")]
		public Cascade[] Cascades
		{
			get
			{
				return m_cascades;
			}
			set
			{
				if (m_cascades.Length != value.Length)
				{
					m_cascades = new Cascade[value.Length];
				}
				value.CopyTo(m_cascades, 0);
			}
		}

		public MyShadowsSettings()
		{
			m_shadowCascadeSmallSkipThresholds = new float[6]
			{
				1000f,
				5000f,
				200f,
				1000f,
				1000f,
				1000f
			};
			m_shadowCascadeFrozen = new bool[6];
			m_cascades = new Cascade[8];
			float num = 5f;
			float num2 = 5f;
			float num3 = 2f;
			float num4 = 758f;
			float skippingSmallObjectThreshold = 0f;
			for (int i = 0; i < 8; i++)
			{
				float num5 = num * (float)Math.Pow(num2, i);
				m_cascades[i].FullCoverageDepth = num5;
				m_cascades[i].ExtendedCoverageDepth = num5 * num3;
				m_cascades[i].ShadowNormalOffset = (num5 + num3) / num4;
				m_cascades[i].SkippingSmallObjectThreshold = skippingSmallObjectThreshold;
			}
		}

		public void CopyFrom(MyShadowsSettings settings)
		{
			ShadowCascadeSmallSkipThresholds = (settings.ShadowCascadeSmallSkipThresholds.Clone() as float[]);
			ShadowCascadeFrozen = (settings.ShadowCascadeFrozen.Clone() as bool[]);
			Data = settings.Data;
			Cascades = (settings.Cascades.Clone() as Cascade[]);
		}
	}
}
