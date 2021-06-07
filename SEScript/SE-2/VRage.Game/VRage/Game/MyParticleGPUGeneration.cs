using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Messages;
using VRageRender.Utils;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyParticleGPUGeneration : IComparable, IMyParticleGeneration
	{
		private enum MyRotationReference
		{
			Camera,
			Local,
			LocalAndCamera
		}

		private enum MyGPUGenerationPropertiesEnum
		{
			ArraySize,
			ArrayOffset,
			ArrayModulo,
			Color,
			ColorIntensity,
			Bounciness,
			EmitterSize,
			EmitterSizeMin,
			Direction,
			Velocity,
			VelocityVar,
			DirectionInnerCone,
			DirectionConeVar,
			Acceleration,
			AccelerationFactor,
			RotationVelocity,
			Radius,
			Life,
			SoftParticleDistanceScale,
			StreakMultiplier,
			AnimationFrameTime,
			Enabled,
			ParticlesPerSecond,
			Material,
			OITWeightFactor,
			Collide,
			SleepState,
			Light,
			VolumetricLight,
			TargetCoverage,
			Gravity,
			Offset,
			RotationVelocityVar,
			HueVar,
			RotationEnabled,
			MotionInheritance,
			LifeVar,
			Streaks,
			RotationReference,
			Angle,
			AngleVar,
			Thickness,
			ParticlesPerFrame,
			CameraBias,
			Emissivity,
			ShadowAlphaMultiplier,
			UseEmissivityChannel,
			UseAlphaAnisotropy,
			AmbientFactor,
			RadiusVar,
			RotationVelocityCollisionMultiplier,
			CollisionCountToKill,
			DistanceScalingFactor,
			NumMembers
		}

		private class VRage_Game_MyParticleGPUGeneration_003C_003EActor : IActivator, IActivator<MyParticleGPUGeneration>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleGPUGeneration();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleGPUGeneration CreateInstance()
			{
				return new MyParticleGPUGeneration();
			}

			MyParticleGPUGeneration IActivator<MyParticleGPUGeneration>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyStringId ID_WHITE_BLOCK = MyStringId.GetOrCompute("WhiteBlock");

		private static readonly int m_version = 2;

		private string m_name;

		private MyParticleEffect m_effect;

		private bool m_dirty;

		private bool m_animDirty;

		private uint m_renderId = uint.MaxValue;

		private MyGPUEmitter m_emitter;

		private static readonly string[] m_myRotationReferenceStrings = new string[3]
		{
			"Camera",
			"Local",
			"Local and camera"
		};

		private static readonly List<string> m_rotationReferenceStrings = m_myRotationReferenceStrings.ToList();

		private readonly IMyConstProperty[] m_properties = new IMyConstProperty[53];

		private bool m_show = true;

		private bool m_animatedTimeValues;

		private float m_lastFramePPS;

		private readonly MyAnimatedPropertyVector4 m_vec4Tmp = new MyAnimatedPropertyVector4();

		private readonly MyAnimatedPropertyFloat m_floatTmp = new MyAnimatedPropertyFloat();

		/// <summary>
		/// Public members to easy access
		/// </summary>
		public MyConstPropertyVector3 ArraySize
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[0];
			}
			private set
			{
				m_properties[0] = value;
			}
		}

		public MyConstPropertyInt ArrayOffset
		{
			get
			{
				return (MyConstPropertyInt)m_properties[1];
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyConstPropertyInt ArrayModulo
		{
			get
			{
				return (MyConstPropertyInt)m_properties[2];
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyAnimatedProperty2DVector4 Color
		{
			get
			{
				return (MyAnimatedProperty2DVector4)m_properties[3];
			}
			private set
			{
				m_properties[3] = value;
			}
		}

		public MyAnimatedProperty2DFloat ColorIntensity
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[4];
			}
			private set
			{
				m_properties[4] = value;
			}
		}

		public MyConstPropertyFloat HueVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[33];
			}
			private set
			{
				m_properties[33] = value;
			}
		}

		public MyConstPropertyFloat Bounciness
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[5];
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public MyConstPropertyFloat RotationVelocityCollisionMultiplier
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[50];
			}
			private set
			{
				m_properties[50] = value;
			}
		}

		public MyConstPropertyFloat DistanceScalingFactor
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[52];
			}
			private set
			{
				m_properties[52] = value;
			}
		}

		public MyConstPropertyInt CollisionCountToKill
		{
			get
			{
				return (MyConstPropertyInt)m_properties[51];
			}
			private set
			{
				m_properties[51] = value;
			}
		}

		public MyAnimatedPropertyVector3 EmitterSize
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[6];
			}
			private set
			{
				m_properties[6] = value;
			}
		}

		public MyAnimatedPropertyFloat EmitterSizeMin
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[7];
			}
			private set
			{
				m_properties[7] = value;
			}
		}

		public MyConstPropertyVector3 Offset
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[31];
			}
			private set
			{
				m_properties[31] = value;
			}
		}

		public MyConstPropertyVector3 Direction
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[8];
			}
			private set
			{
				m_properties[8] = value;
			}
		}

		public MyAnimatedPropertyFloat Velocity
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[9];
			}
			private set
			{
				m_properties[9] = value;
			}
		}

		public MyAnimatedPropertyFloat VelocityVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[10];
			}
			private set
			{
				m_properties[10] = value;
			}
		}

		public MyAnimatedPropertyFloat DirectionInnerCone
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[11];
			}
			private set
			{
				m_properties[11] = value;
			}
		}

		public MyAnimatedPropertyFloat DirectionConeVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[12];
			}
			private set
			{
				m_properties[12] = value;
			}
		}

		public MyConstPropertyVector3 Acceleration
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[13];
			}
			private set
			{
				m_properties[13] = value;
			}
		}

		public MyAnimatedProperty2DFloat AccelerationFactor
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[14];
			}
			private set
			{
				m_properties[14] = value;
			}
		}

		public MyConstPropertyFloat RotationVelocity
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[15];
			}
			private set
			{
				m_properties[15] = value;
			}
		}

		public MyConstPropertyFloat RotationVelocityVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[32];
			}
			private set
			{
				m_properties[32] = value;
			}
		}

		public MyAnimatedProperty2DFloat Radius
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[16];
			}
			private set
			{
				m_properties[16] = value;
			}
		}

		public MyConstPropertyFloat RadiusVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[49];
			}
			private set
			{
				m_properties[49] = value;
			}
		}

		public MyConstPropertyFloat Life
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[17];
			}
			private set
			{
				m_properties[17] = value;
			}
		}

		public MyConstPropertyFloat LifeVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[36];
			}
			private set
			{
				m_properties[36] = value;
			}
		}

		public MyConstPropertyFloat SoftParticleDistanceScale
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[18];
			}
			private set
			{
				m_properties[18] = value;
			}
		}

		public MyConstPropertyFloat StreakMultiplier
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[19];
			}
			private set
			{
				m_properties[19] = value;
			}
		}

		public MyConstPropertyFloat AnimationFrameTime
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[20];
			}
			private set
			{
				m_properties[20] = value;
			}
		}

		public MyConstPropertyBool Enabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[21];
			}
			private set
			{
				m_properties[21] = value;
			}
		}

		public MyAnimatedPropertyFloat ParticlesPerSecond
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[22];
			}
			private set
			{
				m_properties[22] = value;
			}
		}

		public MyAnimatedPropertyFloat ParticlesPerFrame
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[42];
			}
			private set
			{
				m_properties[42] = value;
			}
		}

		public MyConstPropertyTransparentMaterial Material
		{
			get
			{
				return (MyConstPropertyTransparentMaterial)m_properties[23];
			}
			private set
			{
				m_properties[23] = value;
			}
		}

		public MyConstPropertyBool Streaks
		{
			get
			{
				return (MyConstPropertyBool)m_properties[37];
			}
			private set
			{
				m_properties[37] = value;
			}
		}

		public MyConstPropertyBool RotationEnabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[34];
			}
			private set
			{
				m_properties[34] = value;
			}
		}

		public MyConstPropertyBool Collide
		{
			get
			{
				return (MyConstPropertyBool)m_properties[25];
			}
			private set
			{
				m_properties[25] = value;
			}
		}

		public MyConstPropertyBool UseEmissivityChannel
		{
			get
			{
				return (MyConstPropertyBool)m_properties[46];
			}
			private set
			{
				m_properties[46] = value;
			}
		}

		public MyConstPropertyBool UseAlphaAnisotropy
		{
			get
			{
				return (MyConstPropertyBool)m_properties[47];
			}
			private set
			{
				m_properties[47] = value;
			}
		}

		public MyConstPropertyBool SleepState
		{
			get
			{
				return (MyConstPropertyBool)m_properties[26];
			}
			private set
			{
				m_properties[26] = value;
			}
		}

		public MyConstPropertyBool Light
		{
			get
			{
				return (MyConstPropertyBool)m_properties[27];
			}
			private set
			{
				m_properties[27] = value;
			}
		}

		public MyConstPropertyBool VolumetricLight
		{
			get
			{
				return (MyConstPropertyBool)m_properties[28];
			}
			private set
			{
				m_properties[28] = value;
			}
		}

		public MyConstPropertyFloat OITWeightFactor
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[24];
			}
			private set
			{
				m_properties[24] = value;
			}
		}

		public MyConstPropertyFloat TargetCoverage
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[29];
			}
			private set
			{
				m_properties[29] = value;
			}
		}

		public MyConstPropertyFloat Gravity
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[30];
			}
			private set
			{
				m_properties[30] = value;
			}
		}

		public MyConstPropertyFloat MotionInheritance
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[35];
			}
			private set
			{
				m_properties[35] = value;
			}
		}

		private MyRotationReference RotationReference
		{
			get
			{
				return (MyRotationReference)(int)(MyConstPropertyInt)m_properties[38];
			}
			set
			{
				m_properties[38].SetValue((int)value);
			}
		}

		public MyConstPropertyVector3 Angle
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[39];
			}
			private set
			{
				m_properties[39] = value;
			}
		}

		public MyConstPropertyVector3 AngleVar
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[40];
			}
			private set
			{
				m_properties[40] = value;
			}
		}

		public MyAnimatedProperty2DFloat Thickness
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[41];
			}
			private set
			{
				m_properties[41] = value;
			}
		}

		public MyConstPropertyFloat CameraBias
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[43];
			}
			private set
			{
				m_properties[43] = value;
			}
		}

		public MyAnimatedProperty2DFloat Emissivity
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[44];
			}
			private set
			{
				m_properties[44] = value;
			}
		}

		public MyConstPropertyFloat ShadowAlphaMultiplier
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[45];
			}
			private set
			{
				m_properties[45] = value;
			}
		}

		public MyConstPropertyFloat AmbientFactor
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[48];
			}
			private set
			{
				m_properties[48] = value;
			}
		}

		public bool Show
		{
			get
			{
				return m_show;
			}
			set
			{
				m_show = value;
				SetDirty();
			}
		}

		private bool IsDirty => m_dirty;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public void Init()
		{
			AddProperty(MyGPUGenerationPropertiesEnum.ArraySize, new MyConstPropertyVector3("Array size"));
			AddProperty(MyGPUGenerationPropertiesEnum.ArrayOffset, new MyConstPropertyInt("Array offset"));
			AddProperty(MyGPUGenerationPropertiesEnum.ArrayModulo, new MyConstPropertyInt("Array modulo"));
			AddProperty(MyGPUGenerationPropertiesEnum.Color, new MyAnimatedProperty2DVector4("Color"));
			AddProperty(MyGPUGenerationPropertiesEnum.ColorIntensity, new MyAnimatedProperty2DFloat("Color intensity"));
			AddProperty(MyGPUGenerationPropertiesEnum.HueVar, new MyConstPropertyFloat("Hue var"));
			AddProperty(MyGPUGenerationPropertiesEnum.Bounciness, new MyConstPropertyFloat("Bounciness"));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationVelocityCollisionMultiplier, new MyConstPropertyFloat("Rotation velocity collision multiplier"));
			AddProperty(MyGPUGenerationPropertiesEnum.DistanceScalingFactor, new MyConstPropertyFloat("Distance scaling factor"));
			AddProperty(MyGPUGenerationPropertiesEnum.CollisionCountToKill, new MyConstPropertyInt("Collision count to kill particle"));
			AddProperty(MyGPUGenerationPropertiesEnum.EmitterSize, new MyAnimatedPropertyVector3("Emitter size"));
			AddProperty(MyGPUGenerationPropertiesEnum.EmitterSizeMin, new MyAnimatedPropertyFloat("Emitter inner size"));
			AddProperty(MyGPUGenerationPropertiesEnum.Offset, new MyConstPropertyVector3("Offset"));
			AddProperty(MyGPUGenerationPropertiesEnum.Direction, new MyConstPropertyVector3("Direction"));
			AddProperty(MyGPUGenerationPropertiesEnum.Velocity, new MyAnimatedPropertyFloat("Velocity"));
			AddProperty(MyGPUGenerationPropertiesEnum.VelocityVar, new MyAnimatedPropertyFloat("Velocity var"));
			AddProperty(MyGPUGenerationPropertiesEnum.DirectionInnerCone, new MyAnimatedPropertyFloat("Direction inner cone"));
			AddProperty(MyGPUGenerationPropertiesEnum.DirectionConeVar, new MyAnimatedPropertyFloat("Direction cone"));
			AddProperty(MyGPUGenerationPropertiesEnum.Acceleration, new MyConstPropertyVector3("Acceleration"));
			AddProperty(MyGPUGenerationPropertiesEnum.AccelerationFactor, new MyAnimatedProperty2DFloat("Acceleration factor [m/s^2]"));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationVelocity, new MyConstPropertyFloat("Rotation velocity"));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationVelocityVar, new MyConstPropertyFloat("Rotation velocity var"));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationEnabled, new MyConstPropertyBool("Rotation enabled"));
			AddProperty(MyGPUGenerationPropertiesEnum.Radius, new MyAnimatedProperty2DFloat("Radius"));
			AddProperty(MyGPUGenerationPropertiesEnum.RadiusVar, new MyConstPropertyFloat("Radius var"));
			AddProperty(MyGPUGenerationPropertiesEnum.Life, new MyConstPropertyFloat("Life"));
			AddProperty(MyGPUGenerationPropertiesEnum.LifeVar, new MyConstPropertyFloat("Life var"));
			AddProperty(MyGPUGenerationPropertiesEnum.SoftParticleDistanceScale, new MyConstPropertyFloat("Soft particle distance scale"));
			AddProperty(MyGPUGenerationPropertiesEnum.StreakMultiplier, new MyConstPropertyFloat("Streak multiplier"));
			AddProperty(MyGPUGenerationPropertiesEnum.AnimationFrameTime, new MyConstPropertyFloat("Animation frame time"));
			AddProperty(MyGPUGenerationPropertiesEnum.Enabled, new MyConstPropertyBool("Enabled"));
			AddProperty(MyGPUGenerationPropertiesEnum.ParticlesPerSecond, new MyAnimatedPropertyFloat("Particles per second"));
			AddProperty(MyGPUGenerationPropertiesEnum.ParticlesPerFrame, new MyAnimatedPropertyFloat("Particles per frame"));
			AddProperty(MyGPUGenerationPropertiesEnum.Material, new MyConstPropertyTransparentMaterial("Material"));
			AddProperty(MyGPUGenerationPropertiesEnum.OITWeightFactor, new MyConstPropertyFloat("OIT weight factor"));
			AddProperty(MyGPUGenerationPropertiesEnum.TargetCoverage, new MyConstPropertyFloat("Target coverage"));
			AddProperty(MyGPUGenerationPropertiesEnum.Streaks, new MyConstPropertyBool("Streaks"));
			AddProperty(MyGPUGenerationPropertiesEnum.Collide, new MyConstPropertyBool("Collide"));
			AddProperty(MyGPUGenerationPropertiesEnum.UseEmissivityChannel, new MyConstPropertyBool("Use Emissivity Channel"));
			AddProperty(MyGPUGenerationPropertiesEnum.UseAlphaAnisotropy, new MyConstPropertyBool("Use Alpha Anisotropy"));
			AddProperty(MyGPUGenerationPropertiesEnum.SleepState, new MyConstPropertyBool("SleepState"));
			AddProperty(MyGPUGenerationPropertiesEnum.Light, new MyConstPropertyBool("Light"));
			AddProperty(MyGPUGenerationPropertiesEnum.VolumetricLight, new MyConstPropertyBool("VolumetricLight"));
			AddProperty(MyGPUGenerationPropertiesEnum.Gravity, new MyConstPropertyFloat("Gravity"));
			AddProperty(MyGPUGenerationPropertiesEnum.MotionInheritance, new MyConstPropertyFloat("Motion inheritance"));
			AddProperty(MyGPUGenerationPropertiesEnum.RotationReference, new MyConstPropertyEnum("Rotation reference", typeof(MyRotationReference), m_rotationReferenceStrings));
			AddProperty(MyGPUGenerationPropertiesEnum.Angle, new MyConstPropertyVector3("Angle"));
			AddProperty(MyGPUGenerationPropertiesEnum.AngleVar, new MyConstPropertyVector3("Angle var"));
			AddProperty(MyGPUGenerationPropertiesEnum.Thickness, new MyAnimatedProperty2DFloat("Thickness"));
			AddProperty(MyGPUGenerationPropertiesEnum.CameraBias, new MyConstPropertyFloat("Camera bias"));
			AddProperty(MyGPUGenerationPropertiesEnum.Emissivity, new MyAnimatedProperty2DFloat("Emissivity"));
			AddProperty(MyGPUGenerationPropertiesEnum.ShadowAlphaMultiplier, new MyConstPropertyFloat("Shadow alpha multiplier"));
			AddProperty(MyGPUGenerationPropertiesEnum.AmbientFactor, new MyConstPropertyFloat("Ambient light factor"));
			InitDefault();
		}

		public void Done()
		{
			Stop(instant: true);
		}

		public void Start(MyParticleEffect effect)
		{
			m_effect = effect;
			m_name = "ParticleGeneration GPU";
			m_dirty = true;
		}

		public void Close()
		{
			Stop(instant: false);
		}

		private void Stop(bool instant)
		{
			Clear();
			for (int i = 0; i < m_properties.Length; i++)
			{
				m_properties[i] = null;
			}
			m_effect = null;
			if (m_renderId != uint.MaxValue)
			{
				MyRenderProxy.RemoveGPUEmitter(m_renderId, instant);
				m_renderId = uint.MaxValue;
			}
		}

		public void InitDefault()
		{
			ArraySize.SetValue(Vector3.One);
			ArrayModulo.SetValue(1);
			MyAnimatedPropertyVector4 myAnimatedPropertyVector = new MyAnimatedPropertyVector4();
			myAnimatedPropertyVector.AddKey(0f, Vector4.One);
			myAnimatedPropertyVector.AddKey(0.33f, Vector4.One);
			myAnimatedPropertyVector.AddKey(0.66f, Vector4.One);
			myAnimatedPropertyVector.AddKey(1f, Vector4.One);
			Color.AddKey(0f, myAnimatedPropertyVector);
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat.AddKey(0f, 1f);
			myAnimatedPropertyFloat.AddKey(0.33f, 1f);
			myAnimatedPropertyFloat.AddKey(0.66f, 1f);
			myAnimatedPropertyFloat.AddKey(1f, 1f);
			ColorIntensity.AddKey(0f, myAnimatedPropertyFloat);
			MyAnimatedPropertyFloat myAnimatedPropertyFloat2 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat2.AddKey(0f, 0f);
			myAnimatedPropertyFloat2.AddKey(0.33f, 0f);
			myAnimatedPropertyFloat2.AddKey(0.66f, 0f);
			myAnimatedPropertyFloat2.AddKey(1f, 0f);
			AccelerationFactor.AddKey(0f, myAnimatedPropertyFloat2);
			Offset.SetValue(new Vector3(0f, 0f, 0f));
			Direction.SetValue(new Vector3(0f, 0f, -1f));
			MyAnimatedPropertyFloat myAnimatedPropertyFloat3 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat3.AddKey(0f, 0.1f);
			myAnimatedPropertyFloat3.AddKey(0.33f, 0.1f);
			myAnimatedPropertyFloat3.AddKey(0.66f, 0.1f);
			myAnimatedPropertyFloat3.AddKey(1f, 0.1f);
			Radius.AddKey(0f, myAnimatedPropertyFloat3);
			MyAnimatedPropertyFloat myAnimatedPropertyFloat4 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat4.AddKey(0f, 1f);
			myAnimatedPropertyFloat4.AddKey(0.33f, 1f);
			myAnimatedPropertyFloat4.AddKey(0.66f, 1f);
			myAnimatedPropertyFloat4.AddKey(1f, 1f);
			Thickness.AddKey(0f, myAnimatedPropertyFloat4);
			MyAnimatedPropertyFloat myAnimatedPropertyFloat5 = new MyAnimatedPropertyFloat();
			myAnimatedPropertyFloat5.AddKey(0f, 0f);
			myAnimatedPropertyFloat5.AddKey(0.33f, 0f);
			myAnimatedPropertyFloat5.AddKey(0.66f, 0f);
			myAnimatedPropertyFloat5.AddKey(1f, 0f);
			Emissivity.AddKey(0f, myAnimatedPropertyFloat5);
			Life.SetValue(1f);
			LifeVar.SetValue(0f);
			StreakMultiplier.SetValue(4f);
			AnimationFrameTime.SetValue(1f);
			Enabled.SetValue(val: true);
			EmitterSize.AddKey(0f, new Vector3(0f, 0f, 0f));
			EmitterSizeMin.AddKey(0f, 0f);
			DirectionInnerCone.AddKey(0f, 0f);
			DirectionConeVar.AddKey(0f, 0f);
			Velocity.AddKey(0f, 1f);
			VelocityVar.AddKey(0f, 0f);
			ParticlesPerSecond.AddKey(0f, 1000f);
			Material.SetValue(MyTransparentMaterials.GetMaterial(ID_WHITE_BLOCK));
			SoftParticleDistanceScale.SetValue(1f);
			Bounciness.SetValue(0.5f);
			RotationVelocityCollisionMultiplier.SetValue(1f);
			DistanceScalingFactor.SetValue(0f);
			CollisionCountToKill.SetValue(0);
			HueVar.SetValue(0f);
			RotationEnabled.SetValue(val: true);
			MotionInheritance.SetValue(0f);
			OITWeightFactor.SetValue(1f);
			TargetCoverage.SetValue(1f);
			CameraBias.SetValue(0f);
			ShadowAlphaMultiplier.SetValue(5f);
			AmbientFactor.SetValue(1f);
		}

		private T AddProperty<T>(MyGPUGenerationPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public IEnumerable<IMyConstProperty> GetProperties()
		{
			return m_properties;
		}

		public void Update()
		{
		}

		public void SetDirty()
		{
			m_dirty = true;
		}

		public void SetAnimDirty()
		{
			m_animDirty = true;
		}

		private MatrixD CalculateWorldMatrix()
		{
			return MatrixD.CreateTranslation((Vector3)Offset * m_effect.GetEmitterScale()) * GetEffect().WorldMatrix;
		}

		private void FillDataComplete(ref MyGPUEmitter emitter)
		{
			m_animatedTimeValues = (Velocity.GetKeysCount() > 1 || VelocityVar.GetKeysCount() > 1 || DirectionInnerCone.GetKeysCount() > 1 || DirectionConeVar.GetKeysCount() > 1 || EmitterSize.GetKeysCount() > 1 || EmitterSizeMin.GetKeysCount() > 1 || Color.GetKeysCount() > 1 || ColorIntensity.GetKeysCount() > 1 || AccelerationFactor.GetKeysCount() > 1 || Radius.GetKeysCount() > 1 || Emissivity.GetKeysCount() > 1 || Thickness.GetKeysCount() > 1);
			emitter.Data.HueVar = HueVar;
			if (emitter.Data.HueVar > 1f)
			{
				emitter.Data.HueVar = 1f;
			}
			else if (emitter.Data.HueVar < 0f)
			{
				emitter.Data.HueVar = 0f;
			}
			emitter.DistanceMaxSqr = (MyParticlesManager.DISTANCE_CHECK_ENABLE ? (m_effect.DistanceMax * m_effect.DistanceMax) : float.MaxValue);
			emitter.Data.MotionInheritance = MotionInheritance;
			emitter.Data.Bounciness = Bounciness;
			emitter.Data.RotationVelocityCollisionMultiplier = RotationVelocityCollisionMultiplier;
			emitter.Data.DistanceScalingFactor = DistanceScalingFactor;
			emitter.Data.CollisionCountToKill = (uint)(int)CollisionCountToKill;
			emitter.Data.ParticleLifeSpan = Life;
			emitter.Data.ParticleLifeSpanVar = LifeVar;
			emitter.Data.Direction = Direction;
			emitter.Data.RotationVelocity = RotationVelocity;
			emitter.Data.RotationVelocityVar = RotationVelocityVar;
			emitter.Data.AccelerationVector = Acceleration;
			emitter.Data.RadiusVar = RadiusVar;
			emitter.Data.StreakMultiplier = StreakMultiplier;
			emitter.Data.SoftParticleDistanceScale = SoftParticleDistanceScale;
			emitter.Data.AnimationFrameTime = AnimationFrameTime;
			emitter.Data.OITWeightFactor = OITWeightFactor;
			emitter.Data.ShadowAlphaMultiplier = ShadowAlphaMultiplier;
			emitter.Data.AmbientFactor = AmbientFactor;
			emitter.AtlasTexture = Material.GetValue<MyTransparentMaterial>().Texture;
			emitter.AtlasDimension = new Vector2I((int)ArraySize.GetValue<Vector3>().X, (int)ArraySize.GetValue<Vector3>().Y);
			emitter.AtlasFrameOffset = ArrayOffset;
			emitter.AtlasFrameModulo = ArrayModulo;
			emitter.Data.Angle = MathHelper.ToRadians(Angle);
			emitter.Data.AngleVar = MathHelper.ToRadians(AngleVar);
			emitter.GravityFactor = Gravity;
			GPUEmitterFlags gPUEmitterFlags = (GPUEmitterFlags)0u;
			switch (RotationReference)
			{
			case MyRotationReference.Local:
				gPUEmitterFlags |= GPUEmitterFlags.LocalRotation;
				break;
			case MyRotationReference.LocalAndCamera:
				gPUEmitterFlags |= GPUEmitterFlags.LocalAndCameraRotation;
				break;
			default:
				gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (Streaks ? 1 : 0));
				break;
			}
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (Collide ? 2 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (UseEmissivityChannel ? 4096 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (UseAlphaAnisotropy ? 8192 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (SleepState ? 4 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (Light ? 16 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (VolumetricLight ? 32 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | ((m_effect.IsSimulationPaused || MyParticlesManager.Paused) ? 128 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (MyParticlesManager.Paused ? 256 : 0));
			gPUEmitterFlags = (GPUEmitterFlags)((int)gPUEmitterFlags | (RotationEnabled ? 512 : 0));
			emitter.Data.Flags = gPUEmitterFlags;
			emitter.GID = m_renderId;
			FillData(ref emitter);
		}

		private void FillData(ref MyGPUEmitter emitter)
		{
			MatrixD matrixD = CalculateWorldMatrix();
			emitter.ParentID = m_effect.ParentID;
			emitter.Data.Rotation = matrixD.Rotation;
			emitter.WorldPosition = matrixD.Translation;
			emitter.Data.Scale = m_effect.GetEmitterScale();
			emitter.ParticlesPerSecond = GetParticlesPerSecond();
			emitter.ParticlesPerFrame = GetParticlesPerFrame();
			emitter.CameraBias = (float)CameraBias * m_effect.GetEmitterScale();
			Velocity.GetInterpolatedValue(m_effect.GetElapsedTime(), out emitter.Data.Velocity);
			VelocityVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out emitter.Data.VelocityVar);
			DirectionInnerCone.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value);
			emitter.Data.DirectionInnerCone = value;
			DirectionConeVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out value);
			emitter.Data.DirectionConeVar = MathHelper.ToRadians(value);
			EmitterSize.GetInterpolatedValue(m_effect.GetElapsedTime(), out emitter.Data.EmitterSize);
			EmitterSizeMin.GetInterpolatedValue(m_effect.GetElapsedTime(), out emitter.Data.EmitterSizeMin);
			Color.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_vec4Tmp);
			m_vec4Tmp.GetKey(0, out float time, out emitter.Data.Color0);
			m_vec4Tmp.GetKey(1, out emitter.Data.ColorKey1, out emitter.Data.Color1);
			m_vec4Tmp.GetKey(2, out emitter.Data.ColorKey2, out emitter.Data.Color2);
			m_vec4Tmp.GetKey(3, out time, out emitter.Data.Color3);
			emitter.Data.Color0 *= m_effect.UserColorMultiplier;
			emitter.Data.Color1 *= m_effect.UserColorMultiplier;
			emitter.Data.Color2 *= m_effect.UserColorMultiplier;
			emitter.Data.Color3 *= m_effect.UserColorMultiplier;
			ColorIntensity.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_floatTmp);
			m_floatTmp.GetKey(0, out time, out emitter.Data.Intensity0);
			m_floatTmp.GetKey(1, out emitter.Data.IntensityKey1, out emitter.Data.Intensity1);
			m_floatTmp.GetKey(2, out emitter.Data.IntensityKey2, out emitter.Data.Intensity2);
			m_floatTmp.GetKey(3, out time, out emitter.Data.Intensity3);
			AccelerationFactor.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_floatTmp);
			m_floatTmp.GetKey(0, out time, out emitter.Data.Acceleration0);
			m_floatTmp.GetKey(1, out emitter.Data.AccelerationKey1, out emitter.Data.Acceleration1);
			m_floatTmp.GetKey(2, out emitter.Data.AccelerationKey2, out emitter.Data.Acceleration2);
			m_floatTmp.GetKey(3, out time, out emitter.Data.Acceleration3);
			Radius.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_floatTmp);
			m_floatTmp.GetKey(0, out time, out emitter.Data.ParticleSize0);
			m_floatTmp.GetKey(1, out emitter.Data.ParticleSizeKeys1, out emitter.Data.ParticleSize1);
			m_floatTmp.GetKey(2, out emitter.Data.ParticleSizeKeys2, out emitter.Data.ParticleSize2);
			m_floatTmp.GetKey(3, out time, out emitter.Data.ParticleSize3);
			emitter.Data.ParticleSize0 *= m_effect.UserRadiusMultiplier;
			emitter.Data.ParticleSize1 *= m_effect.UserRadiusMultiplier;
			emitter.Data.ParticleSize2 *= m_effect.UserRadiusMultiplier;
			emitter.Data.ParticleSize3 *= m_effect.UserRadiusMultiplier;
			Thickness.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_floatTmp);
			m_floatTmp.GetKey(0, out time, out emitter.Data.ParticleThickness0);
			m_floatTmp.GetKey(1, out emitter.Data.ParticleThicknessKeys1, out emitter.Data.ParticleThickness1);
			m_floatTmp.GetKey(2, out emitter.Data.ParticleThicknessKeys2, out emitter.Data.ParticleThickness2);
			m_floatTmp.GetKey(3, out time, out emitter.Data.ParticleThickness3);
			Emissivity.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_floatTmp);
			m_floatTmp.GetKey(0, out time, out emitter.Data.Emissivity0);
			m_floatTmp.GetKey(1, out emitter.Data.EmissivityKeys1, out emitter.Data.Emissivity1);
			m_floatTmp.GetKey(2, out emitter.Data.EmissivityKeys2, out emitter.Data.Emissivity2);
			m_floatTmp.GetKey(3, out time, out emitter.Data.Emissivity3);
		}

		private float GetParticlesPerSecond()
		{
			if (Enabled.GetValue<bool>() && m_show && !m_effect.IsEmittingStopped)
			{
				ParticlesPerSecond.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value);
				return value * m_effect.UserBirthMultiplier;
			}
			return 0f;
		}

		private float GetParticlesPerFrame()
		{
			if (ParticlesPerFrame.GetKeysCount() > 0 && Enabled.GetValue<bool>() && m_show && !m_effect.IsEmittingStopped)
			{
				ParticlesPerFrame.GetNextValue(m_effect.GetElapsedTime() - 0.0166666675f, out float nextValue, out float nextTime, out float _);
				if (nextTime < m_effect.GetElapsedTime() - 0.0166666675f || nextTime >= m_effect.GetElapsedTime())
				{
					return 0f;
				}
				return nextValue * m_effect.UserBirthMultiplier;
			}
			return 0f;
		}

		public int CompareTo(object compareToObject)
		{
			return 0;
		}

		public void Clear()
		{
		}

		public void Deallocate()
		{
			MyParticlesManager.GPUGenerationsPool.Deallocate(this);
		}

		public IMyParticleGeneration CreateInstance(MyParticleEffect effect)
		{
			MyParticlesManager.GPUGenerationsPool.AllocateOrCreate(out MyParticleGPUGeneration item);
			item.Start(effect);
			item.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				item.m_properties[i] = m_properties[i];
			}
			return item;
		}

		public IMyParticleGeneration Duplicate(MyParticleEffect effect)
		{
			MyParticlesManager.GPUGenerationsPool.AllocateOrCreate(out MyParticleGPUGeneration item);
			item.Start(effect);
			item.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				item.m_properties[i] = m_properties[i].Duplicate();
			}
			return item;
		}

		public MyParticleEffect GetEffect()
		{
			return m_effect;
		}

		public MyParticleEmitter GetEmitter()
		{
			return null;
		}

		public void MergeAABB(ref BoundingBoxD aabb)
		{
		}

		public float GetBirthRate()
		{
			return 0f;
		}

		private void ConvertAlphaColors()
		{
			MyAnimatedProperty2DVector4 color = Color;
			for (int i = 0; i < color.GetKeysCount(); i++)
			{
				color.GetKey(i, out float time, out MyAnimatedPropertyVector4 value);
				IMyAnimatedProperty myAnimatedProperty = value;
				for (int j = 0; j < value.GetKeysCount(); j++)
				{
					value.GetKey(j, out time, out Vector4 value2);
					value2 = value2.UnmultiplyColor();
					value2.W = ColorExtensions.ToLinearRGBComponent(value2.W);
					value2 = value2.PremultiplyColor();
					value2 = Vector4.Clamp(value2, new Vector4(0f, 0f, 0f, 0f), new Vector4(1f, 1f, 1f, 1f));
					myAnimatedProperty.SetKey(j, time, value2);
				}
			}
		}

		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ParticleGeneration");
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("Version", ((int)m_version).ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("GenerationType", "GPU");
			writer.WriteStartElement("Properties");
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				writer.WriteStartElement("Property");
				writer.WriteAttributeString("Name", myConstProperty.Name);
				writer.WriteAttributeString("Type", myConstProperty.BaseValueType);
				PropertyAnimationType propertyAnimationType = PropertyAnimationType.Const;
				if (myConstProperty.Animated)
				{
					propertyAnimationType = ((!myConstProperty.Is2D) ? PropertyAnimationType.Animated : PropertyAnimationType.Animated2D);
				}
				writer.WriteAttributeString("AnimationType", propertyAnimationType.ToString());
				myConstProperty.Serialize(writer);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public void DeserializeFromObjectBuilder(ParticleGeneration generation)
		{
			m_name = generation.Name;
			foreach (GenerationProperty property in generation.Properties)
			{
				for (int i = 0; i < m_properties.Length; i++)
				{
					if (m_properties[i].Name.Equals(property.Name))
					{
						m_properties[i].DeserializeFromObjectBuilder(property);
					}
				}
			}
			Emissivity.GetInterpolatedKeys(m_effect.GetElapsedTime(), 1f, m_floatTmp);
			if (m_floatTmp.GetKeysCount() < 4)
			{
				MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat();
				myAnimatedPropertyFloat.AddKey(0f, 0f);
				myAnimatedPropertyFloat.AddKey(0.33f, 0f);
				myAnimatedPropertyFloat.AddKey(0.66f, 0f);
				myAnimatedPropertyFloat.AddKey(1f, 0f);
				Emissivity.AddKey(0f, myAnimatedPropertyFloat);
			}
		}

		public void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			int num = Convert.ToInt32(reader.GetAttribute("version"), CultureInfo.InvariantCulture);
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (reader.GetAttribute("name") == null)
				{
					break;
				}
				myConstProperty.Deserialize(reader);
			}
			reader.ReadEndElement();
			if (num == 1)
			{
				ConvertAlphaColors();
			}
		}

		public void Draw(List<MyBillboard> collectedBillboards)
		{
			if (m_renderId == uint.MaxValue)
			{
				m_renderId = MyRenderProxy.CreateGPUEmitter(m_effect.Name + "_" + Name);
			}
			if (IsDirty)
			{
				m_emitter = default(MyGPUEmitter);
				FillDataComplete(ref m_emitter);
				m_lastFramePPS = m_emitter.ParticlesPerSecond;
				MyParticlesManager.GPUEmitters.Add(m_emitter);
				m_dirty = (m_animDirty = false);
			}
			else if (m_animatedTimeValues || m_animDirty)
			{
				FillData(ref m_emitter);
				m_lastFramePPS = m_emitter.ParticlesPerSecond;
				MyParticlesManager.GPUEmitters.Add(m_emitter);
				m_animDirty = false;
			}
			else if (m_effect.TransformDirty)
			{
				float particlesPerSecond = GetParticlesPerSecond();
				float particlesPerFrame = GetParticlesPerFrame();
				m_lastFramePPS = particlesPerSecond;
				MatrixD matrixD = CalculateWorldMatrix();
				MyGPUEmitterTransformUpdate myGPUEmitterTransformUpdate = default(MyGPUEmitterTransformUpdate);
				myGPUEmitterTransformUpdate.GID = m_renderId;
				myGPUEmitterTransformUpdate.Rotation = matrixD.Rotation;
				myGPUEmitterTransformUpdate.Position = matrixD.Translation;
				myGPUEmitterTransformUpdate.Scale = m_effect.GetEmitterScale();
				myGPUEmitterTransformUpdate.ParticlesPerSecond = particlesPerSecond;
				myGPUEmitterTransformUpdate.ParticlesPerFrame = particlesPerFrame;
				MyGPUEmitterTransformUpdate item = myGPUEmitterTransformUpdate;
				MyParticlesManager.GPUEmitterTransforms.Add(item);
			}
			else if (ParticlesPerSecond.GetKeysCount() > 1 || ParticlesPerFrame.GetKeysCount() > 0)
			{
				float particlesPerSecond2 = GetParticlesPerSecond();
				float particlesPerFrame2 = GetParticlesPerFrame();
				if (Math.Abs(m_lastFramePPS - particlesPerSecond2) > 0.5f || particlesPerFrame2 > 0f)
				{
					m_lastFramePPS = particlesPerSecond2;
					MyParticlesManager.GPUEmittersLite.Add(new MyGPUEmitterLite
					{
						GID = m_renderId,
						ParticlesPerSecond = particlesPerSecond2,
						ParticlesPerFrame = particlesPerFrame2
					});
				}
			}
		}

		public void DebugDraw()
		{
		}
	}
}
