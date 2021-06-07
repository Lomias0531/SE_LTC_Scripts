using System;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Lights;
using VRageRender.Messages;

namespace Sandbox.Game.Lights
{
	[GenerateActivator]
	public class MyLight
	{
		private class Sandbox_Game_Lights_MyLight_003C_003EActor : IActivator, IActivator<MyLight>
		{
			private sealed override object CreateInstance()
			{
				return new MyLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLight CreateInstance()
			{
				return new MyLight();
			}

			MyLight IActivator<MyLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyLightType m_lightType;

		private uint m_renderObjectID = uint.MaxValue;

		private bool m_propertiesDirty;

		private bool m_positionDirty;

		private bool m_parentDirty;

		private Vector3D m_position;

		private uint m_parentID = uint.MaxValue;

		private float m_glareMaxDistance;

		private MySubGlare[] m_subGlares;

		private float m_glareIntensity;

		private Vector2 m_glareSize;

		private float m_glareQuerySize;

		private float m_glareQueryFreqMinMs;

		private float m_glareQueryFreqRndMs;

		private float m_glareQueryShift;

		private MyGlareTypeEnum m_glareType;

		private bool m_glareOn;

		private Color m_color = Color.White;

		private float m_falloff;

		private float m_glossFactor;

		private float m_diffuseFactor;

		private float m_range;

		private float m_intensity;

		private bool m_lightOn;

		private float m_reflectorIntensity;

		private bool m_reflectorOn;

		private Vector3 m_reflectorDirection;

		private Vector3 m_reflectorUp;

		private float m_reflectorConeMaxAngleCos;

		private Color m_reflectorColor;

		private float m_reflectorRange;

		private float m_reflectorFalloff;

		private float m_reflectorGlossFactor;

		private float m_reflectorDiffuseFactor;

		private string m_reflectorTexture;

		private bool m_castShadows;

		private float m_pointLightOffset;

		private MatrixD m_matrix;

		private Vector3 m_colorLinear = Vector3.One;

		private Vector3 m_reflectorColorLinear = Vector3.One;

		private bool m_aabbDirty;

		private string m_debugName;

		public Vector3D Position
		{
			get
			{
				return m_position;
			}
			set
			{
				if (Vector3D.DistanceSquared(m_position, value) > 0.0001)
				{
					m_position = value;
					m_propertiesDirty = true;
					m_positionDirty = true;
				}
			}
		}

		public uint ParentID
		{
			get
			{
				return m_parentID;
			}
			set
			{
				if (m_parentID != value)
				{
					m_parentID = value;
					m_parentDirty = true;
				}
			}
		}

		public float PointLightOffset
		{
			get
			{
				return m_pointLightOffset;
			}
			set
			{
				if (m_pointLightOffset != value)
				{
					m_pointLightOffset = value;
					m_propertiesDirty = true;
					m_aabbDirty = true;
				}
			}
		}

		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				if (m_color != value)
				{
					m_color = value;
					m_colorLinear = m_color.ToVector3().ToLinearRGB();
					m_propertiesDirty = true;
				}
			}
		}

		public float Falloff
		{
			get
			{
				return m_falloff;
			}
			set
			{
				if (m_falloff != value)
				{
					m_falloff = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float GlossFactor
		{
			get
			{
				return m_glossFactor;
			}
			set
			{
				if (m_glossFactor != value)
				{
					m_glossFactor = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float DiffuseFactor
		{
			get
			{
				return m_diffuseFactor;
			}
			set
			{
				if (m_diffuseFactor != value)
				{
					m_diffuseFactor = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float Range
		{
			get
			{
				return m_range;
			}
			set
			{
				if (m_range != value)
				{
					if (value <= 0f)
					{
						value = 0.5f;
					}
					m_range = value;
					m_aabbDirty = true;
					m_propertiesDirty = true;
				}
			}
		}

		public float Intensity
		{
			get
			{
				return m_intensity;
			}
			set
			{
				if (m_intensity != value)
				{
					m_intensity = value;
					m_propertiesDirty = true;
				}
			}
		}

		public bool LightOn
		{
			get
			{
				return m_lightOn;
			}
			set
			{
				if (m_lightOn != value)
				{
					m_lightOn = value;
					m_propertiesDirty = true;
					m_positionDirty = true;
				}
			}
		}

		public MyLightType LightType
		{
			get
			{
				return m_lightType;
			}
			set
			{
				m_lightType = value;
			}
		}

		public float ReflectorIntensity
		{
			get
			{
				return m_reflectorIntensity;
			}
			set
			{
				if (m_reflectorIntensity != value)
				{
					m_reflectorIntensity = value;
					m_propertiesDirty = true;
				}
			}
		}

		public bool ReflectorOn
		{
			get
			{
				return m_reflectorOn;
			}
			set
			{
				if (m_reflectorOn != value)
				{
					m_reflectorOn = value;
					m_propertiesDirty = true;
					m_positionDirty = true;
				}
			}
		}

		public Vector3 ReflectorDirection
		{
			get
			{
				return m_reflectorDirection;
			}
			set
			{
				if (Vector3.DistanceSquared(m_reflectorDirection, value) > 1E-05f)
				{
					m_reflectorDirection = value;
					m_propertiesDirty = true;
					m_positionDirty = true;
				}
			}
		}

		public Vector3 ReflectorUp
		{
			get
			{
				return m_reflectorUp;
			}
			set
			{
				if (Vector3.DistanceSquared(m_reflectorUp, value) > 1E-05f)
				{
					m_reflectorUp = MyUtils.Normalize(value);
					m_propertiesDirty = true;
					m_positionDirty = true;
				}
			}
		}

		public float ReflectorConeMaxAngleCos
		{
			get
			{
				return m_reflectorConeMaxAngleCos;
			}
			set
			{
				if (m_reflectorConeMaxAngleCos != value)
				{
					m_reflectorConeMaxAngleCos = value;
					m_propertiesDirty = true;
					m_aabbDirty = true;
				}
			}
		}

		public Color ReflectorColor
		{
			get
			{
				return m_reflectorColor;
			}
			set
			{
				if (m_reflectorColor != value)
				{
					m_reflectorColor = value;
					m_reflectorColorLinear = m_reflectorColor.ToVector3().ToLinearRGB();
					m_propertiesDirty = true;
				}
			}
		}

		public float ReflectorRange
		{
			get
			{
				return m_reflectorRange;
			}
			set
			{
				if (m_reflectorRange != value)
				{
					m_reflectorRange = value;
					m_propertiesDirty = true;
					m_aabbDirty = true;
				}
			}
		}

		public float ReflectorFalloff
		{
			get
			{
				return m_reflectorFalloff;
			}
			set
			{
				if (m_reflectorFalloff != value)
				{
					m_reflectorFalloff = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float ReflectorGlossFactor
		{
			get
			{
				return m_reflectorGlossFactor;
			}
			set
			{
				if (m_reflectorGlossFactor != value)
				{
					m_reflectorGlossFactor = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float ReflectorDiffuseFactor
		{
			get
			{
				return m_reflectorDiffuseFactor;
			}
			set
			{
				if (m_reflectorDiffuseFactor != value)
				{
					m_reflectorDiffuseFactor = value;
					m_propertiesDirty = true;
				}
			}
		}

		public string ReflectorTexture
		{
			get
			{
				return m_reflectorTexture;
			}
			set
			{
				if (m_reflectorTexture != value)
				{
					m_reflectorTexture = value;
					m_propertiesDirty = true;
				}
			}
		}

		public bool CastShadows
		{
			get
			{
				return m_castShadows;
			}
			set
			{
				if (m_castShadows != value)
				{
					m_castShadows = value;
					m_propertiesDirty = true;
				}
			}
		}

		public bool GlareOn
		{
			get
			{
				return m_glareOn;
			}
			set
			{
				if (m_glareOn != value)
				{
					m_glareOn = value;
					m_propertiesDirty = true;
					m_positionDirty = true;
				}
			}
		}

		public MyGlareTypeEnum GlareType
		{
			get
			{
				return m_glareType;
			}
			set
			{
				if (m_glareType != value)
				{
					m_glareType = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float GlareQuerySize
		{
			get
			{
				return m_glareQuerySize;
			}
			set
			{
				if (m_glareQuerySize != value)
				{
					m_glareQuerySize = value;
					m_propertiesDirty = true;
					m_aabbDirty = true;
				}
			}
		}

		public float GlareQueryShift
		{
			get
			{
				return m_glareQueryShift;
			}
			set
			{
				if (m_glareQueryShift != value)
				{
					m_glareQueryShift = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float GlareQueryFreqMinMs
		{
			get
			{
				return m_glareQueryFreqMinMs;
			}
			set
			{
				if (m_glareQueryFreqMinMs != value)
				{
					m_glareQueryFreqMinMs = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float GlareQueryFreqRndMs
		{
			get
			{
				return m_glareQueryFreqRndMs;
			}
			set
			{
				if (m_glareQueryFreqRndMs != value)
				{
					m_glareQueryFreqRndMs = value;
					m_propertiesDirty = true;
				}
			}
		}

		public MySubGlare[] SubGlares
		{
			get
			{
				return m_subGlares;
			}
			set
			{
				m_subGlares = value;
				m_propertiesDirty = true;
			}
		}

		public float GlareIntensity
		{
			get
			{
				return m_glareIntensity;
			}
			set
			{
				if (m_glareIntensity != value)
				{
					m_glareIntensity = value;
					m_propertiesDirty = true;
				}
			}
		}

		public Vector2 GlareSize
		{
			get
			{
				return m_glareSize;
			}
			set
			{
				if (m_glareSize != value)
				{
					m_glareSize = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float GlareMaxDistance
		{
			get
			{
				return m_glareMaxDistance;
			}
			set
			{
				if (m_glareMaxDistance != value)
				{
					m_glareMaxDistance = value;
					m_propertiesDirty = true;
				}
			}
		}

		public float ReflectorConeRadians
		{
			get
			{
				return ConeMaxAngleCosToRadians(ReflectorConeMaxAngleCos);
			}
			set
			{
				ReflectorConeMaxAngleCos = ConeRadiansToConeMaxAngleCos(value);
			}
		}

		public float ReflectorConeDegrees
		{
			get
			{
				return ConeMaxAngleCosToDegrees(ReflectorConeMaxAngleCos);
			}
			set
			{
				ReflectorConeMaxAngleCos = ConeDegreesToConeMaxAngleCos(value);
			}
		}

		public uint RenderObjectID => m_renderObjectID;

		private static float ConeRadiansToConeMaxAngleCos(float value)
		{
			return 1f - (float)Math.Cos(value / 2f);
		}

		private static float ConeDegreesToConeMaxAngleCos(float value)
		{
			return ConeRadiansToConeMaxAngleCos(MathHelper.ToRadians(value));
		}

		private static float ConeMaxAngleCosToRadians(float reflectorConeMaxAngleCos)
		{
			return (float)Math.Acos(1f - reflectorConeMaxAngleCos) * 2f;
		}

		private static float ConeMaxAngleCosToDegrees(float reflectorConeMaxAngleCos)
		{
			return MathHelper.ToDegrees(ConeMaxAngleCosToRadians(reflectorConeMaxAngleCos));
		}

		public void Start(Vector3D position, Vector4 color, float range, string debugName)
		{
			Start(color, range, debugName);
			Position = position;
		}

		public void Start(Vector4 color, float range, string debugName)
		{
			Start(debugName);
			Color = color;
			Range = range;
		}

		public void UpdateLight()
		{
			if (m_positionDirty)
			{
				m_matrix = (ReflectorOn ? MatrixD.CreateWorld(Position, ReflectorDirection, ReflectorUp) : MatrixD.CreateTranslation(Position));
			}
			if (m_parentDirty)
			{
				m_parentDirty = false;
				MyRenderProxy.SetParentCullObject(m_renderObjectID, ParentID, m_matrix);
			}
			if (m_propertiesDirty || m_positionDirty)
			{
				MyLightLayout myLightLayout = default(MyLightLayout);
				myLightLayout.Range = Range;
				myLightLayout.Color = m_colorLinear * Intensity;
				myLightLayout.Falloff = m_falloff;
				myLightLayout.GlossFactor = GlossFactor;
				myLightLayout.DiffuseFactor = DiffuseFactor;
				MyLightLayout pointLight = myLightLayout;
				MySpotLightLayout mySpotLightLayout = default(MySpotLightLayout);
				myLightLayout = new MyLightLayout
				{
					Range = ReflectorRange,
					Color = m_reflectorColorLinear * ReflectorIntensity,
					Falloff = m_reflectorFalloff,
					GlossFactor = ReflectorGlossFactor,
					DiffuseFactor = ReflectorDiffuseFactor
				};
				mySpotLightLayout.Light = myLightLayout;
				mySpotLightLayout.Up = ReflectorUp;
				mySpotLightLayout.Direction = ReflectorDirection;
				mySpotLightLayout.ApertureCos = (float)Math.Min(Math.Max(1f - ReflectorConeMaxAngleCos, 0.01), 0.99000000953674316);
				MySpotLightLayout spotLight = mySpotLightLayout;
				MyFlareDesc myFlareDesc = default(MyFlareDesc);
				myFlareDesc.Enabled = (GlareOn && GlareIntensity > 0.01f && GlareSize.X > 0.01f && GlareSize.Y > 0.01f && SubGlares != null && SubGlares.Length != 0);
				myFlareDesc.Type = GlareType;
				myFlareDesc.MaxDistance = GlareMaxDistance;
				myFlareDesc.QuerySize = GlareQuerySize;
				myFlareDesc.QueryShift = GlareQueryShift;
				myFlareDesc.QueryFreqMinMs = GlareQueryFreqMinMs;
				myFlareDesc.QueryFreqRndMs = GlareQueryFreqRndMs;
				myFlareDesc.Intensity = GlareIntensity;
				myFlareDesc.SizeMultiplier = GlareSize;
				myFlareDesc.Glares = SubGlares;
				MyFlareDesc glare = myFlareDesc;
				UpdateRenderLightData updateRenderLightData = default(UpdateRenderLightData);
				updateRenderLightData.ID = RenderObjectID;
				updateRenderLightData.Position = Position;
				updateRenderLightData.CastShadows = CastShadows;
				updateRenderLightData.PointLightOn = LightOn;
				updateRenderLightData.PointLight = pointLight;
				updateRenderLightData.PointIntensity = Intensity;
				updateRenderLightData.PointOffset = PointLightOffset;
				updateRenderLightData.SpotLightOn = ReflectorOn;
				updateRenderLightData.SpotLight = spotLight;
				updateRenderLightData.SpotIntensity = ReflectorIntensity;
				updateRenderLightData.ReflectorTexture = ReflectorTexture;
				updateRenderLightData.Glare = glare;
				updateRenderLightData.PositionChanged = m_positionDirty;
				updateRenderLightData.AabbChanged = m_aabbDirty;
				UpdateRenderLightData data = updateRenderLightData;
				MyRenderProxy.UpdateRenderLight(ref data);
				m_propertiesDirty = (m_positionDirty = (m_aabbDirty = false));
			}
		}

		public void Clear()
		{
			MyRenderProxy.RemoveRenderObject(RenderObjectID, MyRenderProxy.ObjectType.Light);
			m_renderObjectID = uint.MaxValue;
		}

		public void MarkPositionDirty()
		{
			m_positionDirty = true;
		}

		public bool SpotlightNotTooLarge(float reflectorConeMaxAngleCos, float reflectorRange)
		{
			if (reflectorConeMaxAngleCos <= MyLightsConstants.MAX_SPOTLIGHT_ANGLE_COS)
			{
				return reflectorRange <= 1200f;
			}
			return false;
		}

		public void UpdateReflectorRangeAndAngle(float reflectorConeMaxAngleCos, float reflectorRange)
		{
			m_reflectorRange = reflectorRange;
			m_reflectorConeMaxAngleCos = reflectorConeMaxAngleCos;
		}

		public void Start(string debugName)
		{
			m_debugName = debugName;
			m_positionDirty = true;
			m_propertiesDirty = true;
			m_aabbDirty = true;
			ReflectorOn = false;
			ReflectorRange = 1f;
			ReflectorUp = Vector3.Up;
			ReflectorDirection = Vector3.Forward;
			ReflectorGlossFactor = 1f;
			ReflectorDiffuseFactor = 3.14f;
			ReflectorFalloff = 2f;
			LightOn = true;
			Intensity = 1f;
			GlareOn = false;
			GlareQueryFreqMinMs = 150f;
			GlareQueryFreqRndMs = 100f;
			GlareMaxDistance = 100f;
			GlareSize = new Vector2(1f, 1f);
			GlareIntensity = 1f;
			PointLightOffset = 0f;
			CastShadows = true;
			Range = 0.5f;
			GlossFactor = 1f;
			DiffuseFactor = 3.14f;
			Falloff = 1f;
			ParentID = uint.MaxValue;
			GlareQueryShift = 0f;
			GlareQuerySize = 0f;
			GlareType = MyGlareTypeEnum.Normal;
			ReflectorIntensity = 0f;
			ReflectorTexture = null;
			m_renderObjectID = MyRenderProxy.CreateRenderLight(debugName);
		}
	}
}
