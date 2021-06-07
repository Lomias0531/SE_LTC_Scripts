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
using VRageRender.Utils;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyParticleGeneration : IMyParticleGeneration
	{
		private class VRage_Game_MyParticleGeneration_003C_003EActor : IActivator, IActivator<MyParticleGeneration>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleGeneration();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleGeneration CreateInstance()
			{
				return new MyParticleGeneration();
			}

			MyParticleGeneration IActivator<MyParticleGeneration>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyStringId m_smokeStringId = MyStringId.GetOrCompute("Smoke");

		private static readonly string[] MyVelocityDirStrings = new string[2]
		{
			"Default",
			"FromEmitterCenter"
		};

		private static readonly string[] MyParticleTypeStrings = new string[3]
		{
			"Point",
			"Line",
			"Trail"
		};

		private static readonly string[] MyBlendTypeStrings = new string[3]
		{
			MyBillboard.BlendTypeEnum.Standard.ToString(),
			MyBillboard.BlendTypeEnum.AdditiveBottom.ToString(),
			MyBillboard.BlendTypeEnum.AdditiveTop.ToString()
		};

		private static readonly string[] MyRotationReferenceStrings = new string[5]
		{
			"Camera",
			"Local",
			"Velocity",
			"Velocity and camera",
			"Local and camera"
		};

		private static readonly string[] MyAccelerationReferenceStrings = new string[4]
		{
			"Local",
			"Camera",
			"Velocity",
			"Gravity"
		};

		private static readonly List<string> s_velocityDirStrings = MyVelocityDirStrings.ToList();

		private static readonly List<string> s_particleTypeStrings = MyParticleTypeStrings.ToList();

		private static readonly List<string> s_blendTypeStrings = MyBlendTypeStrings.ToList();

		private static readonly List<string> s_rotationReferenceStrings = MyRotationReferenceStrings.ToList();

		private static readonly List<string> s_accelerationReferenceStrings = MyAccelerationReferenceStrings.ToList();

		private static readonly int Version = 4;

		private string m_name;

		private MyParticleEffect m_effect;

		private readonly MyParticleEmitter m_emitter;

		private float m_particlesToCreate;

		private float m_birthRate;

		private float m_birthPerFrame;

		private readonly List<MyAnimatedParticle> m_particles = new List<MyAnimatedParticle>(64);

		private Vector3D? m_lastEffectPosition;

		private BoundingBoxD m_AABB;

		private readonly IMyConstProperty[] m_properties = new IMyConstProperty[Enum.GetValues(typeof(MyGenerationPropertiesEnum)).Length];

		private bool m_show = true;

		private readonly List<MyBillboard> m_billboards = new List<MyBillboard>();

		/// <summary>
		/// Public members to easy access
		/// </summary>
		public MyAnimatedPropertyFloat Birth
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[0];
			}
			private set
			{
				m_properties[0] = value;
			}
		}

		public MyAnimatedPropertyFloat BirthPerFrame
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[31];
			}
			private set
			{
				m_properties[31] = value;
			}
		}

		public MyAnimatedPropertyFloat Life
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[1];
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyConstPropertyFloat LifeVar
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[2];
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyAnimatedPropertyVector3 Velocity
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[3];
			}
			private set
			{
				m_properties[3] = value;
			}
		}

		public MyVelocityDirEnum VelocityDir
		{
			get
			{
				return (MyVelocityDirEnum)(int)(MyConstPropertyInt)m_properties[4];
			}
			private set
			{
				m_properties[4].SetValue((int)value);
			}
		}

		public MyAnimatedPropertyFloat VelocityVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[45];
			}
			private set
			{
				m_properties[45] = value;
			}
		}

		public MyAnimatedPropertyVector3 Angle
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[5];
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public MyConstPropertyVector3 AngleVar
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[6];
			}
			private set
			{
				m_properties[6] = value;
			}
		}

		public MyAnimatedProperty2DVector3 RotationSpeed
		{
			get
			{
				return (MyAnimatedProperty2DVector3)m_properties[7];
			}
			private set
			{
				m_properties[7] = value;
			}
		}

		public MyConstPropertyVector3 RotationSpeedVar
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

		public MyAnimatedProperty2DFloat Radius
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[9];
			}
			private set
			{
				m_properties[9] = value;
			}
		}

		public MyAnimatedPropertyFloat RadiusVar
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

		public MyConstPropertyFloat RadiusBySpeed
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

		public MyAnimatedProperty2DVector4 Color
		{
			get
			{
				return (MyAnimatedProperty2DVector4)m_properties[11];
			}
			private set
			{
				m_properties[11] = value;
			}
		}

		public MyAnimatedPropertyFloat ColorVar
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

		public MyAnimatedProperty2DTransparentMaterial Material
		{
			get
			{
				return (MyAnimatedProperty2DTransparentMaterial)m_properties[13];
			}
			private set
			{
				m_properties[13] = value;
			}
		}

		public MyConstPropertyEnum ParticleType
		{
			get
			{
				return (MyConstPropertyEnum)m_properties[14];
			}
			private set
			{
				m_properties[14] = value;
			}
		}

		public MyConstPropertyEnum BlendType
		{
			get
			{
				return (MyConstPropertyEnum)m_properties[44];
			}
			private set
			{
				m_properties[44] = value;
			}
		}

		public MyAnimatedPropertyFloat Thickness
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[15];
			}
			private set
			{
				m_properties[15] = value;
			}
		}

		public MyConstPropertyBool Enabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[16];
			}
			private set
			{
				m_properties[16] = value;
			}
		}

		public MyConstPropertyBool EnableCustomRadius
		{
			get
			{
				return (MyConstPropertyBool)m_properties[17];
			}
			private set
			{
				m_properties[17] = value;
			}
		}

		public MyConstPropertyBool EnableCustomBirth
		{
			get
			{
				return (MyConstPropertyBool)m_properties[19];
			}
			private set
			{
				m_properties[19] = value;
			}
		}

		public MyConstPropertyGenerationIndex OnDie
		{
			get
			{
				return (MyConstPropertyGenerationIndex)m_properties[20];
			}
			private set
			{
				m_properties[20] = value;
			}
		}

		public MyConstPropertyGenerationIndex OnLife
		{
			get
			{
				return (MyConstPropertyGenerationIndex)m_properties[21];
			}
			private set
			{
				m_properties[21] = value;
			}
		}

		public MyAnimatedPropertyFloat LODBirth
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

		public MyAnimatedPropertyFloat LODRadius
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[23];
			}
			private set
			{
				m_properties[23] = value;
			}
		}

		public MyAnimatedPropertyFloat MotionInheritance
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[24];
			}
			private set
			{
				m_properties[24] = value;
			}
		}

		public MyConstPropertyBool AlphaAnisotropic
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

		public MyConstPropertyFloat Gravity
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[26];
			}
			private set
			{
				m_properties[26] = value;
			}
		}

		public MyAnimatedProperty2DVector3 PivotRotation
		{
			get
			{
				return (MyAnimatedProperty2DVector3)m_properties[27];
			}
			private set
			{
				m_properties[27] = value;
			}
		}

		public MyAnimatedProperty2DVector3 Acceleration
		{
			get
			{
				return (MyAnimatedProperty2DVector3)m_properties[28];
			}
			private set
			{
				m_properties[28] = value;
			}
		}

		public MyConstPropertyFloat AccelerationVar
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

		public MyAnimatedProperty2DFloat AlphaCutout
		{
			get
			{
				return (MyAnimatedProperty2DFloat)m_properties[30];
			}
			private set
			{
				m_properties[30] = value;
			}
		}

		public MyAnimatedPropertyFloat ColorIntensity
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[33];
			}
			private set
			{
				m_properties[33] = value;
			}
		}

		public MyConstPropertyFloat SoftParticleDistanceScale
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

		public MyAnimatedProperty2DVector3 Pivot
		{
			get
			{
				return (MyAnimatedProperty2DVector3)m_properties[34];
			}
			private set
			{
				m_properties[34] = value;
			}
		}

		public MyConstPropertyVector3 PivotVar
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[35];
			}
			private set
			{
				m_properties[35] = value;
			}
		}

		public MyConstPropertyVector3 PivotRotationVar
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[36];
			}
			private set
			{
				m_properties[36] = value;
			}
		}

		public MyRotationReference RotationReference
		{
			get
			{
				return (MyRotationReference)(int)(MyConstPropertyInt)m_properties[37];
			}
			set
			{
				m_properties[37].SetValue((int)value);
			}
		}

		public MyConstPropertyVector3 ArraySize
		{
			get
			{
				return (MyConstPropertyVector3)m_properties[38];
			}
			private set
			{
				m_properties[38] = value;
			}
		}

		public MyAnimatedProperty2DInt ArrayIndex
		{
			get
			{
				return (MyAnimatedProperty2DInt)m_properties[39];
			}
			private set
			{
				m_properties[39] = value;
			}
		}

		public MyConstPropertyInt ArrayOffset
		{
			get
			{
				return (MyConstPropertyInt)m_properties[40];
			}
			private set
			{
				m_properties[40] = value;
			}
		}

		public MyConstPropertyInt ArrayModulo
		{
			get
			{
				return (MyConstPropertyInt)m_properties[41];
			}
			private set
			{
				m_properties[41] = value;
			}
		}

		public MyAccelerationReference AccelerationReference
		{
			get
			{
				return (MyAccelerationReference)(int)(MyConstPropertyInt)m_properties[42];
			}
			set
			{
				m_properties[42].SetValue((int)value);
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
			}
		}

		private bool IsDirty => true;

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

		public MatrixD EffectMatrix
		{
			get;
			set;
		}

		public bool IsInherited
		{
			get;
			set;
		}

		public MyParticleGeneration()
		{
			m_emitter = new MyParticleEmitter(MyParticleEmitterType.Point);
		}

		public void SetVelocityDir(MyVelocityDirEnum val)
		{
			VelocityDir = val;
		}

		public void Init()
		{
			AddProperty(MyGenerationPropertiesEnum.Birth, new MyAnimatedPropertyFloat("Birth"));
			AddProperty(MyGenerationPropertiesEnum.Life, new MyAnimatedPropertyFloat("Life"));
			AddProperty(MyGenerationPropertiesEnum.LifeVar, new MyConstPropertyFloat("Life var"));
			AddProperty(MyGenerationPropertiesEnum.Velocity, new MyAnimatedPropertyVector3("Velocity"));
			AddProperty(MyGenerationPropertiesEnum.VelocityDir, new MyConstPropertyEnum("Velocity dir", typeof(MyVelocityDirEnum), s_velocityDirStrings));
			AddProperty(MyGenerationPropertiesEnum.VelocityVar, new MyAnimatedPropertyFloat("Velocity var"));
			AddProperty(MyGenerationPropertiesEnum.Angle, new MyAnimatedPropertyVector3("Angle"));
			AddProperty(MyGenerationPropertiesEnum.AngleVar, new MyConstPropertyVector3("Angle var"));
			AddProperty(MyGenerationPropertiesEnum.RotationSpeed, new MyAnimatedProperty2DVector3("Rotation speed"));
			AddProperty(MyGenerationPropertiesEnum.RotationSpeedVar, new MyConstPropertyVector3("Rotation speed var"));
			AddProperty(MyGenerationPropertiesEnum.Radius, new MyAnimatedProperty2DFloat("Radius"));
			AddProperty(MyGenerationPropertiesEnum.RadiusVar, new MyAnimatedPropertyFloat("Radius var"));
			AddProperty(MyGenerationPropertiesEnum.Color, new MyAnimatedProperty2DVector4("Color"));
			AddProperty(MyGenerationPropertiesEnum.ColorVar, new MyAnimatedPropertyFloat("Color var"));
			AddProperty(MyGenerationPropertiesEnum.Material, new MyAnimatedProperty2DTransparentMaterial("Material", MyTransparentMaterialInterpolator.Switch));
			AddProperty(MyGenerationPropertiesEnum.ParticleType, new MyConstPropertyEnum("Particle type", typeof(MyParticleTypeEnum), s_particleTypeStrings));
			AddProperty(MyGenerationPropertiesEnum.BlendType, new MyConstPropertyEnum("Blend type", typeof(MyBillboard.BlendTypeEnum), s_blendTypeStrings));
			AddProperty(MyGenerationPropertiesEnum.Thickness, new MyAnimatedPropertyFloat("Thickness"));
			AddProperty(MyGenerationPropertiesEnum.Enabled, new MyConstPropertyBool("Enabled"));
			Enabled.SetValue(val: true);
			AddProperty(MyGenerationPropertiesEnum.EnableCustomRadius, new MyConstPropertyBool("Enable custom radius"));
			AddProperty(MyGenerationPropertiesEnum.EnableCustomVelocity, new MyConstPropertyBool("Enable custom velocity"));
			AddProperty(MyGenerationPropertiesEnum.EnableCustomBirth, new MyConstPropertyBool("Enable custom birth"));
			AddProperty(MyGenerationPropertiesEnum.OnDie, new MyConstPropertyGenerationIndex("OnDie"));
			OnDie.SetValue(-1);
			AddProperty(MyGenerationPropertiesEnum.OnLife, new MyConstPropertyGenerationIndex("OnLife"));
			OnLife.SetValue(-1);
			AddProperty(MyGenerationPropertiesEnum.LODBirth, new MyAnimatedPropertyFloat("LODBirth"));
			AddProperty(MyGenerationPropertiesEnum.LODRadius, new MyAnimatedPropertyFloat("LODRadius"));
			AddProperty(MyGenerationPropertiesEnum.MotionInheritance, new MyAnimatedPropertyFloat("Motion inheritance"));
			AddProperty(MyGenerationPropertiesEnum.AlphaAnisotropic, new MyConstPropertyBool("Alpha anisotropic"));
			AddProperty(MyGenerationPropertiesEnum.Gravity, new MyConstPropertyFloat("Gravity"));
			AddProperty(MyGenerationPropertiesEnum.PivotRotation, new MyAnimatedProperty2DVector3("Pivot rotation"));
			AddProperty(MyGenerationPropertiesEnum.Acceleration, new MyAnimatedProperty2DVector3("Acceleration"));
			AddProperty(MyGenerationPropertiesEnum.AccelerationVar, new MyConstPropertyFloat("Acceleration var"));
			AddProperty(MyGenerationPropertiesEnum.AlphaCutout, new MyAnimatedProperty2DFloat("Alpha cutout"));
			AddProperty(MyGenerationPropertiesEnum.BirthPerFrame, new MyAnimatedPropertyFloat("Birth per frame"));
			AddProperty(MyGenerationPropertiesEnum.RadiusBySpeed, new MyConstPropertyFloat("Radius by speed"));
			AddProperty(MyGenerationPropertiesEnum.SoftParticleDistanceScale, new MyConstPropertyFloat("Soft particle distance scale"));
			AddProperty(MyGenerationPropertiesEnum.ColorIntensity, new MyAnimatedPropertyFloat("Color intensity"));
			AddProperty(MyGenerationPropertiesEnum.Pivot, new MyAnimatedProperty2DVector3("Pivot"));
			AddProperty(MyGenerationPropertiesEnum.PivotVar, new MyConstPropertyVector3("Pivot var"));
			AddProperty(MyGenerationPropertiesEnum.PivotRotationVar, new MyConstPropertyVector3("Pivot rotation var"));
			AddProperty(MyGenerationPropertiesEnum.RotationReference, new MyConstPropertyEnum("Rotation reference", typeof(MyRotationReference), s_rotationReferenceStrings));
			AddProperty(MyGenerationPropertiesEnum.ArraySize, new MyConstPropertyVector3("Array size"));
			AddProperty(MyGenerationPropertiesEnum.ArrayIndex, new MyAnimatedProperty2DInt("Array index"));
			AddProperty(MyGenerationPropertiesEnum.ArrayOffset, new MyConstPropertyInt("Array offset"));
			AddProperty(MyGenerationPropertiesEnum.ArrayModulo, new MyConstPropertyInt("Array modulo"));
			AddProperty(MyGenerationPropertiesEnum.AccelerationReference, new MyConstPropertyEnum("Acceleration reference", typeof(MyAccelerationReference), s_accelerationReferenceStrings));
			LODBirth.AddKey(0f, 1f);
			LODRadius.AddKey(0f, 1f);
			MyAnimatedPropertyVector3 myAnimatedPropertyVector = new MyAnimatedPropertyVector3(Pivot.Name);
			myAnimatedPropertyVector.AddKey(0f, new Vector3(0f, 0f, 0f));
			Pivot.AddKey(0f, myAnimatedPropertyVector);
			AccelerationVar.SetValue(0f);
			ColorIntensity.AddKey(0f, 1f);
			SoftParticleDistanceScale.SetValue(1f);
			m_emitter.Init();
		}

		public void Done()
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				if (m_properties[i] is IMyAnimatedProperty)
				{
					(m_properties[i] as IMyAnimatedProperty).ClearKeys();
				}
			}
			m_emitter.Done();
			Close();
		}

		public void Start(MyParticleEffect effect)
		{
			m_effect = effect;
			m_name = "ParticleGeneration";
			m_emitter.Start();
			m_lastEffectPosition = null;
			IsInherited = false;
			m_birthRate = 0f;
			m_particlesToCreate = 0f;
			m_AABB = BoundingBoxD.CreateInvalid();
		}

		public void Close()
		{
			Clear();
			for (int i = 0; i < m_properties.Length; i++)
			{
				m_properties[i] = null;
			}
			m_emitter.Close();
			m_effect = null;
		}

		public void Deallocate()
		{
			MyParticlesManager.GenerationsPool.Deallocate(this);
		}

		public void InitDefault()
		{
			Birth.AddKey(0f, 1f);
			Life.AddKey(0f, 10f);
			Velocity.AddKey(0f, new Vector3(0f, 1f, 0f));
			MyAnimatedPropertyVector4 myAnimatedPropertyVector = new MyAnimatedPropertyVector4(Color.Name);
			myAnimatedPropertyVector.AddKey(0f, new Vector4(1f, 0f, 0f, 1f));
			myAnimatedPropertyVector.AddKey(1f, new Vector4(0f, 0f, 1f, 1f));
			Color.AddKey(0f, myAnimatedPropertyVector);
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat(Radius.Name);
			myAnimatedPropertyFloat.AddKey(0f, 1f);
			Radius.AddKey(0f, myAnimatedPropertyFloat);
			MyAnimatedPropertyTransparentMaterial myAnimatedPropertyTransparentMaterial = new MyAnimatedPropertyTransparentMaterial(Material.Name);
			myAnimatedPropertyTransparentMaterial.AddKey(0f, MyTransparentMaterials.GetMaterial(m_smokeStringId));
			Material.AddKey(0f, myAnimatedPropertyTransparentMaterial);
			LODBirth.AddKey(0f, 1f);
			LODBirth.AddKey(0.5f, 1f);
			LODBirth.AddKey(1f, 0f);
			LODRadius.AddKey(0f, 1f);
			MyAnimatedPropertyVector3 myAnimatedPropertyVector2 = new MyAnimatedPropertyVector3(Pivot.Name);
			myAnimatedPropertyVector2.AddKey(0f, new Vector3(0f, 0f, 0f));
			Pivot.AddKey(0f, myAnimatedPropertyVector2);
			AccelerationVar.SetValue(0f);
			SoftParticleDistanceScale.SetValue(1f);
			BlendType.SetValue(MyBillboard.BlendTypeEnum.Standard);
		}

		private T AddProperty<T>(MyGenerationPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public IEnumerable<IMyConstProperty> GetProperties()
		{
			return m_properties;
		}

		private void UpdateParticlesLife()
		{
			int num = 0;
			MyParticleGeneration myParticleGeneration = null;
			Vector3D point = m_effect.WorldMatrix.Translation;
			float particlesToCreate = 0f;
			m_AABB = BoundingBoxD.CreateInvalid();
			m_AABB = m_AABB.Include(ref point);
			if (OnDie.GetValue<int>() != -1)
			{
				myParticleGeneration = (GetInheritedGeneration(OnDie.GetValue<int>()) as MyParticleGeneration);
				if (myParticleGeneration == null)
				{
					OnDie.SetValue(-1);
				}
				else
				{
					myParticleGeneration.IsInherited = true;
					particlesToCreate = myParticleGeneration.m_particlesToCreate;
				}
			}
			Vector3D point2 = point;
			Vector3D point3 = point;
			while (num < m_particles.Count)
			{
				MotionInheritance.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value);
				MyAnimatedParticle myAnimatedParticle = m_particles[num];
				if (myAnimatedParticle.Update())
				{
					if (value > 0f)
					{
						MatrixD deltaMatrix = m_effect.GetDeltaMatrix();
						myAnimatedParticle.AddMotionInheritance(ref value, ref deltaMatrix);
					}
					if (num == 0)
					{
						point = myAnimatedParticle.ActualPosition;
						point2 = myAnimatedParticle.Quad.Point1;
						point3 = myAnimatedParticle.Quad.Point2;
						myAnimatedParticle.Quad.Point0 = myAnimatedParticle.ActualPosition;
						myAnimatedParticle.Quad.Point2 = myAnimatedParticle.ActualPosition;
					}
					num++;
					if (myAnimatedParticle.Type == MyParticleTypeEnum.Trail)
					{
						if (myAnimatedParticle.ActualPosition == point)
						{
							myAnimatedParticle.Quad.Point0 = myAnimatedParticle.ActualPosition;
							myAnimatedParticle.Quad.Point1 = myAnimatedParticle.ActualPosition;
							myAnimatedParticle.Quad.Point2 = myAnimatedParticle.ActualPosition;
							myAnimatedParticle.Quad.Point3 = myAnimatedParticle.ActualPosition;
						}
						else
						{
							MyPolyLineD polyLine = default(MyPolyLineD);
							myAnimatedParticle.Radius.GetInterpolatedValue(myAnimatedParticle.NormalizedTime, out float value2);
							polyLine.Thickness = value2;
							polyLine.Point0 = myAnimatedParticle.ActualPosition;
							polyLine.Point1 = point;
							_ = polyLine.Point1 - polyLine.Point0;
							Vector3 vector = polyLine.LineDirectionNormalized = MyUtils.Normalize((Vector3)(polyLine.Point1 - polyLine.Point0));
							Vector3D translation = MyTransparentGeometry.Camera.Translation;
							MyUtils.GetPolyLineQuad(out myAnimatedParticle.Quad, ref polyLine, translation);
							myAnimatedParticle.Quad.Point0 = point2;
							myAnimatedParticle.Quad.Point3 = point3;
							point2 = myAnimatedParticle.Quad.Point1;
							point3 = myAnimatedParticle.Quad.Point2;
						}
					}
					point = myAnimatedParticle.ActualPosition;
					m_AABB = m_AABB.Include(ref point);
				}
				else
				{
					if (myParticleGeneration != null)
					{
						myParticleGeneration.m_particlesToCreate = particlesToCreate;
						myParticleGeneration.EffectMatrix = MatrixD.CreateWorld(myAnimatedParticle.ActualPosition, Vector3D.Normalize(myAnimatedParticle.Velocity), Vector3D.Cross(Vector3D.Left, myAnimatedParticle.Velocity));
						myParticleGeneration.UpdateParticlesCreation();
					}
					m_particles.Remove(myAnimatedParticle);
					MyTransparentGeometry.DeallocateAnimatedParticle(myAnimatedParticle);
				}
			}
		}

		private void UpdateParticlesCreation()
		{
			if (!Enabled.GetValue<bool>() || !m_show)
			{
				return;
			}
			if (!m_effect.CalculateDeltaMatrix)
			{
				MotionInheritance.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value);
				if (value > 0f)
				{
					m_effect.CalculateDeltaMatrix = true;
				}
			}
			if (!m_effect.IsEmittingStopped)
			{
				float value2 = 1f;
				if (GetEffect().EnableLods && LODBirth.GetKeysCount() > 0)
				{
					LODBirth.GetInterpolatedValue(GetEffect().Distance, out value2);
				}
				Birth.GetInterpolatedValue(m_effect.GetElapsedTime(), out m_birthRate);
				m_birthRate *= 0.0166666675f * (EnableCustomBirth ? m_effect.UserBirthMultiplier : 1f) * value2;
				if (BirthPerFrame.GetKeysCount() > 0)
				{
					BirthPerFrame.GetNextValue(m_effect.GetElapsedTime() - 0.0166666675f, out m_birthPerFrame, out float nextTime, out float _);
					if (nextTime >= m_effect.GetElapsedTime() - 0.0166666675f && nextTime < m_effect.GetElapsedTime())
					{
						m_birthPerFrame *= (EnableCustomBirth ? m_effect.UserBirthMultiplier : 1f) * value2;
					}
					else
					{
						m_birthPerFrame = 0f;
					}
				}
				m_particlesToCreate += m_birthRate;
			}
			Vector3 value3 = Vector3.Zero;
			if (!m_lastEffectPosition.HasValue)
			{
				m_lastEffectPosition = EffectMatrix.Translation;
			}
			if (m_particlesToCreate > 1f && !m_effect.CalculateDeltaMatrix)
			{
				value3 = (Vector3)(EffectMatrix.Translation - m_lastEffectPosition.Value) / (float)(int)m_particlesToCreate;
			}
			int num = 40;
			while (m_particlesToCreate >= 1f && num-- > 0)
			{
				if (m_effect.CalculateDeltaMatrix)
				{
					CreateParticle(EffectMatrix.Translation);
				}
				else
				{
					CreateParticle(m_lastEffectPosition.Value + value3 * (int)m_particlesToCreate);
				}
				m_particlesToCreate -= 1f;
			}
			while (m_birthPerFrame >= 1f && num-- > 0)
			{
				if (m_effect.CalculateDeltaMatrix)
				{
					CreateParticle(EffectMatrix.Translation);
				}
				else
				{
					CreateParticle(m_lastEffectPosition.Value + value3 * (int)m_birthPerFrame);
				}
				m_birthPerFrame -= 1f;
			}
			if (OnLife.GetValue<int>() != -1)
			{
				MyParticleGeneration myParticleGeneration = GetInheritedGeneration(OnLife.GetValue<int>()) as MyParticleGeneration;
				if (myParticleGeneration == null)
				{
					OnLife.SetValue(-1);
				}
				else
				{
					myParticleGeneration.IsInherited = true;
					float particlesToCreate = myParticleGeneration.m_particlesToCreate;
					foreach (MyAnimatedParticle particle in m_particles)
					{
						myParticleGeneration.m_particlesToCreate = particlesToCreate;
						myParticleGeneration.EffectMatrix = MatrixD.CreateWorld(particle.ActualPosition, (Vector3D)particle.Velocity, Vector3D.Cross(Vector3D.Left, particle.Velocity));
						myParticleGeneration.UpdateParticlesCreation();
					}
				}
			}
			m_lastEffectPosition = EffectMatrix.Translation;
		}

		public void Update()
		{
			EffectMatrix = m_effect.WorldMatrix;
			m_birthRate = 0f;
			UpdateParticlesLife();
			if (!IsInherited)
			{
				UpdateParticlesCreation();
			}
			m_effect.ParticlesCount += m_particles.Count;
		}

		private IMyParticleGeneration GetInheritedGeneration(int generationIndex)
		{
			if (generationIndex >= m_effect.GetGenerations().Count || generationIndex == m_effect.GetGenerations().IndexOf(this))
			{
				return null;
			}
			return m_effect.GetGenerations()[generationIndex];
		}

		public void SetDirty()
		{
		}

		public void SetAnimDirty()
		{
		}

		public void Clear()
		{
			int num = 0;
			while (num < m_particles.Count)
			{
				MyAnimatedParticle myAnimatedParticle = m_particles[num];
				m_particles.Remove(myAnimatedParticle);
				MyTransparentGeometry.DeallocateAnimatedParticle(myAnimatedParticle);
			}
			m_particlesToCreate = 0f;
			m_lastEffectPosition = m_effect.WorldMatrix.Translation;
		}

		private void CreateParticle(Vector3D interpolatedEffectPosition)
		{
			MyAnimatedParticle myAnimatedParticle = MyTransparentGeometry.AddAnimatedParticle();
			if (myAnimatedParticle == null)
			{
				return;
			}
			myAnimatedParticle.Type = (MyParticleTypeEnum)ParticleType.GetValue<int>();
			myAnimatedParticle.BlendType = (MyBillboard.BlendTypeEnum)BlendType.GetValue<int>();
			m_emitter.CalculateStartPosition(m_effect.GetElapsedTime(), MatrixD.CreateWorld(interpolatedEffectPosition, m_effect.WorldMatrix.Forward, m_effect.WorldMatrix.Up), m_effect.GetEmitterAxisScale(), m_effect.GetEmitterScale(), out Vector3D startOffset, out myAnimatedParticle.StartPosition);
			Vector3D point = myAnimatedParticle.StartPosition;
			m_AABB = m_AABB.Include(ref point);
			Life.GetInterpolatedValue(m_effect.GetElapsedTime(), out myAnimatedParticle.Life);
			float num = LifeVar;
			if (num > 0f)
			{
				myAnimatedParticle.Life = MathHelper.Max(MyUtils.GetRandomFloat(myAnimatedParticle.Life - num, myAnimatedParticle.Life + num), 0.1f);
			}
			Velocity.GetInterpolatedValue(m_effect.GetElapsedTime(), out Vector3 value);
			if (VelocityVar.GetKeysCount() > 0)
			{
				VelocityVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value2);
				if (value2 != 0f)
				{
					float minValue = 1f / value2;
					float maxValue = value2;
					value2 = MyUtils.GetRandomFloat(minValue, maxValue);
					value *= m_effect.GetScale() * value2;
				}
				else
				{
					value *= m_effect.GetScale();
				}
			}
			else
			{
				value *= m_effect.GetScale();
			}
			myAnimatedParticle.Velocity = value;
			myAnimatedParticle.Velocity = Vector3D.TransformNormal(myAnimatedParticle.Velocity, GetEffect().WorldMatrix);
			if (VelocityDir == MyVelocityDirEnum.FromEmitterCenter && !MyUtils.IsZero(startOffset - myAnimatedParticle.StartPosition))
			{
				float scaleFactor = myAnimatedParticle.Velocity.Length();
				myAnimatedParticle.Velocity = (Vector3)MyUtils.Normalize(myAnimatedParticle.StartPosition - startOffset) * scaleFactor;
			}
			Angle.GetInterpolatedValue(m_effect.GetElapsedTime(), out myAnimatedParticle.Angle);
			Vector3 vector = AngleVar;
			if (vector.LengthSquared() > 0f)
			{
				myAnimatedParticle.Angle = new Vector3(MyUtils.GetRandomFloat(myAnimatedParticle.Angle.X - vector.X, myAnimatedParticle.Angle.X + vector.X), MyUtils.GetRandomFloat(myAnimatedParticle.Angle.Y - vector.Y, myAnimatedParticle.Angle.Y + vector.Y), MyUtils.GetRandomFloat(myAnimatedParticle.Angle.Z - vector.Z, myAnimatedParticle.Angle.Z + vector.Z));
			}
			if (RotationSpeed.GetKeysCount() > 0)
			{
				myAnimatedParticle.RotationSpeed = new MyAnimatedPropertyVector3(RotationSpeed.Name);
				Vector3 variance = RotationSpeedVar;
				RotationSpeed.GetInterpolatedKeys(m_effect.GetElapsedTime(), variance, 1f, myAnimatedParticle.RotationSpeed);
			}
			else
			{
				myAnimatedParticle.RotationSpeed = null;
			}
			RadiusVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value3);
			float value4 = 1f;
			if (GetEffect().EnableLods)
			{
				LODRadius.GetInterpolatedValue(GetEffect().Distance, out value4);
			}
			Radius.GetInterpolatedKeys(m_effect.GetElapsedTime(), value3, (EnableCustomRadius.GetValue<bool>() ? m_effect.UserRadiusMultiplier : 1f) * value4 * m_effect.GetScale(), myAnimatedParticle.Radius);
			Thickness.GetInterpolatedValue(m_effect.GetElapsedTime(), out myAnimatedParticle.Thickness);
			myAnimatedParticle.Thickness *= value4;
			ColorVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value5);
			Color.GetInterpolatedKeys(m_effect.GetElapsedTime(), value5, 1f, myAnimatedParticle.Color);
			ColorIntensity.GetInterpolatedValue(m_effect.GetElapsedTime(), out myAnimatedParticle.ColorIntensity);
			myAnimatedParticle.SoftParticleDistanceScale = SoftParticleDistanceScale;
			Material.GetInterpolatedKeys(m_effect.GetElapsedTime(), 0, 1f, myAnimatedParticle.Material);
			if (Pivot.GetKeysCount() > 0)
			{
				myAnimatedParticle.Pivot = new MyAnimatedPropertyVector3(Pivot.Name);
				Pivot.GetInterpolatedKeys(m_effect.GetElapsedTime(), PivotVar, 1f, myAnimatedParticle.Pivot);
			}
			else
			{
				myAnimatedParticle.Pivot = null;
			}
			if (PivotRotation.GetKeysCount() > 0)
			{
				myAnimatedParticle.PivotRotation = new MyAnimatedPropertyVector3(PivotRotation.Name);
				PivotRotation.GetInterpolatedKeys(m_effect.GetElapsedTime(), PivotRotationVar, 1f, myAnimatedParticle.PivotRotation);
			}
			else
			{
				myAnimatedParticle.PivotRotation = null;
			}
			if (Acceleration.GetKeysCount() > 0)
			{
				myAnimatedParticle.Acceleration = new MyAnimatedPropertyVector3(Acceleration.Name);
				float randomFloat = MyUtils.GetRandomFloat(1f - (float)AccelerationVar, 1f + (float)AccelerationVar);
				Acceleration.GetInterpolatedKeys(m_effect.GetElapsedTime(), randomFloat, myAnimatedParticle.Acceleration);
			}
			else
			{
				myAnimatedParticle.Acceleration = null;
			}
			if (AlphaCutout.GetKeysCount() > 0)
			{
				myAnimatedParticle.AlphaCutout = new MyAnimatedPropertyFloat(AlphaCutout.Name);
				AlphaCutout.GetInterpolatedKeys(m_effect.GetElapsedTime(), 0f, 1f, myAnimatedParticle.AlphaCutout);
			}
			else
			{
				myAnimatedParticle.AlphaCutout = null;
			}
			if (ArrayIndex.GetKeysCount() > 0)
			{
				myAnimatedParticle.ArrayIndex = new MyAnimatedPropertyInt(ArrayIndex.Name);
				ArrayIndex.GetInterpolatedKeys(m_effect.GetElapsedTime(), 0, 1f, myAnimatedParticle.ArrayIndex);
			}
			else
			{
				myAnimatedParticle.ArrayIndex = null;
			}
			myAnimatedParticle.Start(this);
			m_particles.Add(myAnimatedParticle);
		}

		public IMyParticleGeneration CreateInstance(MyParticleEffect effect)
		{
			MyParticlesManager.GenerationsPool.AllocateOrCreate(out MyParticleGeneration item);
			item.Start(effect);
			item.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				item.m_properties[i] = m_properties[i];
			}
			item.m_emitter.CreateInstance(m_emitter);
			return item;
		}

		public IMyParticleGeneration Duplicate(MyParticleEffect effect)
		{
			MyParticlesManager.GenerationsPool.AllocateOrCreate(out MyParticleGeneration item);
			item.Start(effect);
			item.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				item.m_properties[i] = m_properties[i].Duplicate();
			}
			m_emitter.Duplicate(item.m_emitter);
			return item;
		}

		public MyParticleEmitter GetEmitter()
		{
			return m_emitter;
		}

		public MyParticleEffect GetEffect()
		{
			return m_effect;
		}

		public float GetBirthRate()
		{
			return m_birthRate;
		}

		public void MergeAABB(ref BoundingBoxD aabb)
		{
			aabb.Include(ref m_AABB);
		}

		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ParticleGeneration");
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("Version", ((int)Version).ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("GenerationType", "CPU");
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
			writer.WriteStartElement("Emitter");
			m_emitter.Serialize(writer);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public void DeserializeV0(XmlReader reader)
		{
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (reader.Name == "Emitter")
				{
					break;
				}
				IMyConstProperty myConstProperty2 = myConstProperty;
				if (myConstProperty2.Name == "Angle" || myConstProperty2.Name == "Rotation speed")
				{
					myConstProperty2 = new MyAnimatedPropertyFloat();
				}
				if (myConstProperty2.Name == "Angle var" || myConstProperty2.Name == "Rotation speed var")
				{
					myConstProperty2 = new MyConstPropertyFloat();
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "PivotDistance")
				{
					new MyAnimatedProperty2DFloat("temp").Deserialize(reader);
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "PivotDistVar")
				{
					new MyConstPropertyFloat().Deserialize(reader);
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "Pivot distance")
				{
					new MyAnimatedProperty2DFloat("temp").Deserialize(reader);
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "Pivot distance var")
				{
					new MyConstPropertyFloat().Deserialize(reader);
				}
				myConstProperty2.Deserialize(reader);
			}
			reader.ReadStartElement();
			m_emitter.Deserialize(reader);
			reader.ReadEndElement();
			reader.ReadEndElement();
			if (LODBirth.GetKeysCount() > 0)
			{
				LODBirth.GetKey(LODBirth.GetKeysCount() - 1, out float time, out float value);
				if (value > 0f)
				{
					LODBirth.AddKey(Math.Max(time + 0.25f, 1f), 0f);
				}
			}
			if ((int)ParticleType != 1)
			{
				Thickness.ClearKeys();
			}
		}

		public void DeserializeV1(XmlReader reader)
		{
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (reader.Name == "Emitter")
				{
					break;
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "PivotDistance")
				{
					new MyAnimatedProperty2DFloat("temp").Deserialize(reader);
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "PivotDistVar")
				{
					new MyConstPropertyFloat().Deserialize(reader);
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "Pivot distance")
				{
					new MyAnimatedProperty2DFloat("temp").Deserialize(reader);
				}
				if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "Pivot distance var")
				{
					new MyConstPropertyFloat().Deserialize(reader);
				}
				myConstProperty.Deserialize(reader);
			}
			reader.ReadStartElement();
			m_emitter.Deserialize(reader);
			reader.ReadEndElement();
			reader.ReadEndElement();
			if (LODBirth.GetKeysCount() > 0)
			{
				LODBirth.GetKey(LODBirth.GetKeysCount() - 1, out float time, out float value);
				if (value > 0f)
				{
					LODBirth.AddKey(Math.Max(time + 0.25f, 1f), 0f);
				}
			}
			if ((int)ParticleType != 1)
			{
				Thickness.ClearKeys();
			}
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
			m_emitter.DeserializeFromObjectBuilder(generation.Emitter);
			if (LODBirth.GetKeysCount() > 0)
			{
				LODBirth.GetKey(LODBirth.GetKeysCount() - 1, out float time, out float value);
				if (value > 0f)
				{
					LODBirth.AddKey(Math.Max(time + 0.25f, 1f), 0f);
				}
			}
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

		private void ConvertSRGBColors()
		{
			MyAnimatedProperty2DVector4 color = Color;
			for (int i = 0; i < color.GetKeysCount(); i++)
			{
				color.GetKey(i, out float time, out MyAnimatedPropertyVector4 value);
				IMyAnimatedProperty myAnimatedProperty = value;
				for (int j = 0; j < value.GetKeysCount(); j++)
				{
					value.GetKey(j, out time, out Vector4 value2);
					value2 = value2.UnmultiplyColor().ToLinearRGB().PremultiplyColor();
					value2 = Vector4.Clamp(value2, new Vector4(0f, 0f, 0f, 0f), new Vector4(1f, 1f, 1f, 1f));
					myAnimatedProperty.SetKey(j, time, value2);
				}
			}
		}

		public void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			int num = Convert.ToInt32(reader.GetAttribute("version"), CultureInfo.InvariantCulture);
			switch (num)
			{
			case 0:
				DeserializeV0(reader);
				ConvertSRGBColors();
				return;
			case 1:
				DeserializeV1(reader);
				ConvertSRGBColors();
				return;
			}
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (reader.Name == "Emitter")
				{
					break;
				}
				myConstProperty.Deserialize(reader);
				if (myConstProperty.Name == "Target coverage")
				{
					myConstProperty.Name = "Soft particle distance scale";
				}
			}
			reader.ReadStartElement();
			m_emitter.Deserialize(reader);
			reader.ReadEndElement();
			reader.ReadEndElement();
			if (LODBirth.GetKeysCount() > 0)
			{
				LODBirth.GetKey(LODBirth.GetKeysCount() - 1, out float time, out float value);
				if (value > 0f)
				{
					LODBirth.AddKey(Math.Max(time + 0.25f, 1f), 0f);
				}
			}
			if (num == 2)
			{
				ConvertSRGBColors();
			}
			if (num == 3)
			{
				ConvertAlphaColors();
			}
		}

		private void PrepareForDraw()
		{
			m_billboards.Clear();
			if (m_particles.Count != 0)
			{
				foreach (MyAnimatedParticle particle in m_particles)
				{
					MyBillboard myBillboard = MyTransparentGeometry.AddBillboardParticle(particle);
					if (myBillboard != null)
					{
						m_billboards.Add(myBillboard);
					}
				}
			}
		}

		public void Draw(List<MyBillboard> collectedBillboards)
		{
			PrepareForDraw();
			foreach (MyBillboard billboard in m_billboards)
			{
				collectedBillboards.Add(billboard);
			}
			m_billboards.Clear();
		}

		public void DebugDraw()
		{
			m_emitter.DebugDraw(m_effect.GetElapsedTime(), m_effect.WorldMatrix);
		}
	}
}
