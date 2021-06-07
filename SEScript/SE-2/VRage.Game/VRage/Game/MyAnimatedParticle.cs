using System;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Utils;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyAnimatedParticle
	{
		private class VRage_Game_MyAnimatedParticle_003C_003EActor : IActivator, IActivator<MyAnimatedParticle>
		{
			private sealed override object CreateInstance()
			{
				return new MyAnimatedParticle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAnimatedParticle CreateInstance()
			{
				return new MyAnimatedParticle();
			}

			MyAnimatedParticle IActivator<MyAnimatedParticle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private float m_elapsedTime;

		private MyParticleGeneration m_generation;

		public MyParticleTypeEnum Type;

		public MyBillboard.BlendTypeEnum BlendType;

		public MyQuadD Quad;

		public Vector3D StartPosition;

		private Vector3 m_velocity;

		public float Life;

		public Vector3 Angle;

		public MyAnimatedPropertyVector3 RotationSpeed;

		public float Thickness;

		public float ColorIntensity;

		public float SoftParticleDistanceScale;

		public MyAnimatedPropertyVector3 Pivot;

		public MyAnimatedPropertyVector3 PivotRotation;

		private Vector3 m_actualPivot = Vector3.Zero;

		private Vector3 m_actualPivotRotation;

		public MyAnimatedPropertyFloat AlphaCutout;

		public MyAnimatedPropertyInt ArrayIndex;

		private int m_arrayIndex = -1;

		public MyAnimatedPropertyVector3 Acceleration;

		private Vector3 m_actualAcceleration = Vector3.Zero;

		public MyAnimatedPropertyFloat Radius = new MyAnimatedPropertyFloat();

		public MyAnimatedPropertyVector4 Color = new MyAnimatedPropertyVector4();

		public MyAnimatedPropertyTransparentMaterial Material = new MyAnimatedPropertyTransparentMaterial();

		private Vector3D m_actualPosition;

		private Vector3D m_previousPosition;

		private Vector3 m_actualAngle;

		private float m_elapsedTimeDivider;

		private float m_normalizedTime;

		public Vector3 Velocity
		{
			get
			{
				return m_velocity;
			}
			set
			{
				m_velocity = value;
			}
		}

		public float NormalizedTime => m_normalizedTime;

		public Vector3D ActualPosition => m_actualPosition;

		public void Start(MyParticleGeneration generation)
		{
			m_elapsedTime = 0f;
			m_normalizedTime = 0f;
			m_elapsedTimeDivider = 0.0166666675f / Life;
			m_generation = generation;
			m_actualPosition = StartPosition;
			m_previousPosition = m_actualPosition;
			m_actualAngle = new Vector3(MathHelper.ToRadians(Angle.X), MathHelper.ToRadians(Angle.Y), MathHelper.ToRadians(Angle.Z));
			if (Pivot != null)
			{
				Pivot.GetInterpolatedValue(0f, out m_actualPivot);
			}
			if (PivotRotation != null)
			{
				PivotRotation.GetInterpolatedValue(0f, out m_actualPivotRotation);
			}
			m_arrayIndex = -1;
			if (ArrayIndex != null)
			{
				ArrayIndex.GetInterpolatedValue(m_normalizedTime, out m_arrayIndex);
				int num = m_generation.ArrayOffset;
				Vector3 vector = m_generation.ArraySize;
				if (vector.X > 0f && vector.Y > 0f)
				{
					int num2 = ((int)m_generation.ArrayModulo == 0) ? ((int)vector.X * (int)vector.Y) : ((int)m_generation.ArrayModulo);
					m_arrayIndex = num + m_arrayIndex % num2;
				}
			}
		}

		public bool Update()
		{
			m_elapsedTime += 0.0166666675f;
			if (m_elapsedTime >= Life)
			{
				return false;
			}
			m_normalizedTime += m_elapsedTimeDivider;
			m_velocity += m_generation.GetEffect().Gravity * m_generation.Gravity * 0.0166666675f;
			m_previousPosition = m_actualPosition;
			m_actualPosition.X += Velocity.X * 0.0166666675f;
			m_actualPosition.Y += Velocity.Y * 0.0166666675f;
			m_actualPosition.Z += Velocity.Z * 0.0166666675f;
			if (Pivot != null)
			{
				Pivot.GetInterpolatedValue(m_normalizedTime, out m_actualPivot);
			}
			if (Acceleration != null)
			{
				Acceleration.GetInterpolatedValue(m_normalizedTime, out m_actualAcceleration);
				Matrix matrix = Matrix.Identity;
				if (m_generation.AccelerationReference == MyAccelerationReference.Camera)
				{
					matrix = MyTransparentGeometry.Camera;
				}
				else if (m_generation.AccelerationReference != 0)
				{
					if (m_generation.AccelerationReference == MyAccelerationReference.Velocity)
					{
						Vector3 value = m_actualPosition - m_previousPosition;
						if (value.LengthSquared() < 1E-05f)
						{
							m_actualAcceleration = Vector3.Zero;
						}
						else
						{
							matrix = Matrix.CreateFromDir(Vector3.Normalize(value));
						}
					}
					else if (m_generation.AccelerationReference == MyAccelerationReference.Gravity)
					{
						if (m_generation.GetEffect().Gravity.LengthSquared() < 1E-05f)
						{
							m_actualAcceleration = Vector3.Zero;
						}
						else
						{
							matrix = Matrix.CreateFromDir(Vector3.Normalize(m_generation.GetEffect().Gravity));
						}
					}
				}
				m_actualAcceleration = Vector3.TransformNormal(m_actualAcceleration, matrix);
				Velocity += m_actualAcceleration * 0.0166666675f;
			}
			if (RotationSpeed != null)
			{
				RotationSpeed.GetInterpolatedValue(m_normalizedTime, out Vector3 value2);
				m_actualAngle += new Vector3(MathHelper.ToRadians(value2.X), MathHelper.ToRadians(value2.Y), MathHelper.ToRadians(value2.Z)) * 0.0166666675f;
			}
			if (PivotRotation != null)
			{
				PivotRotation.GetInterpolatedValue(m_normalizedTime, out Vector3 value3);
				m_actualPivotRotation += value3;
			}
			if (ArrayIndex != null)
			{
				ArrayIndex.GetInterpolatedValue(m_normalizedTime, out m_arrayIndex);
			}
			return true;
		}

		public bool Draw(MyBillboard billboard)
		{
			if (Pivot != null && !MyParticlesManager.Paused)
			{
				if (PivotRotation != null)
				{
					Matrix matrix = Matrix.CreateRotationX(MathHelper.ToRadians(m_actualPivotRotation.X) * 0.0166666675f) * Matrix.CreateRotationY(MathHelper.ToRadians(m_actualPivotRotation.Y) * 0.0166666675f) * Matrix.CreateRotationZ(MathHelper.ToRadians(m_actualPivotRotation.Z) * 0.0166666675f);
					m_actualPivot = Vector3.TransformNormal(m_actualPivot, matrix);
				}
				m_actualPivot = Vector3D.TransformNormal(m_actualPivot, m_generation.GetEffect().WorldMatrix);
			}
			Vector3D position = m_actualPosition + m_actualPivot;
			billboard.DistanceSquared = (float)Vector3D.DistanceSquared(MyTransparentGeometry.Camera.Translation, position);
			if (billboard.DistanceSquared <= 0.1f)
			{
				return false;
			}
			float value = 1f;
			Radius.GetInterpolatedValue(m_normalizedTime, out value);
			float value2 = 0f;
			if (AlphaCutout != null)
			{
				AlphaCutout.GetInterpolatedValue(m_normalizedTime, out value2);
			}
			billboard.CustomViewProjection = -1;
			billboard.ParentID = uint.MaxValue;
			billboard.AlphaCutout = value2;
			billboard.UVOffset = Vector2.Zero;
			billboard.UVSize = Vector2.One;
			billboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
			billboard.BlendType = BlendType;
			float scaleFactor = 1f;
			Matrix identity = Matrix.Identity;
			Vector3 forward = Vector3.Forward;
			Vector3 vector = m_actualPosition - m_previousPosition;
			if ((float)m_generation.RadiusBySpeed > 0f)
			{
				float num = vector.Length();
				value = Math.Max(value, value * (float)m_generation.RadiusBySpeed * num);
			}
			if (Type == MyParticleTypeEnum.Point)
			{
				Vector2 radius = new Vector2(value, value);
				if (Thickness > 0f)
				{
					radius.Y = Thickness;
				}
				if (m_generation.RotationReference == MyRotationReference.Camera)
				{
					identity = Matrix.CreateFromAxisAngle(MyTransparentGeometry.Camera.Right, m_actualAngle.X) * Matrix.CreateFromAxisAngle(MyTransparentGeometry.Camera.Up, m_actualAngle.Y) * Matrix.CreateFromAxisAngle(MyTransparentGeometry.Camera.Forward, m_actualAngle.Z);
					GetBillboardQuadRotated(billboard, ref position, radius, ref identity, MyTransparentGeometry.Camera.Left, MyTransparentGeometry.Camera.Up);
				}
				else if (m_generation.RotationReference == MyRotationReference.Local)
				{
					identity = Matrix.CreateFromAxisAngle(m_generation.GetEffect().WorldMatrix.Right, m_actualAngle.X) * Matrix.CreateFromAxisAngle(m_generation.GetEffect().WorldMatrix.Up, m_actualAngle.Y) * Matrix.CreateFromAxisAngle(m_generation.GetEffect().WorldMatrix.Forward, m_actualAngle.Z);
					GetBillboardQuadRotated(billboard, ref position, radius, ref identity, m_generation.GetEffect().WorldMatrix.Left, m_generation.GetEffect().WorldMatrix.Up);
				}
				else if (m_generation.RotationReference == MyRotationReference.Velocity)
				{
					if (vector.LengthSquared() < 1E-05f)
					{
						return false;
					}
					Matrix matrix2 = Matrix.CreateFromDir(Vector3.Normalize(vector));
					identity = Matrix.CreateFromAxisAngle(matrix2.Right, m_actualAngle.X) * Matrix.CreateFromAxisAngle(matrix2.Up, m_actualAngle.Y) * Matrix.CreateFromAxisAngle(matrix2.Forward, m_actualAngle.Z);
					GetBillboardQuadRotated(billboard, ref position, radius, ref identity, matrix2.Left, matrix2.Up);
				}
				else if (m_generation.RotationReference == MyRotationReference.VelocityAndCamera)
				{
					if (vector.LengthSquared() < 0.0001f)
					{
						return false;
					}
					Vector3 vector2 = Vector3.Normalize(m_actualPosition - MyTransparentGeometry.Camera.Translation);
					Vector3 vector3 = Vector3.Normalize(vector);
					Vector3 up = Vector3.Cross(Vector3.Cross(vector2, vector3), vector3);
					Matrix matrix3 = Matrix.CreateWorld(m_actualPosition, vector3, up);
					identity = Matrix.CreateFromAxisAngle(matrix3.Right, m_actualAngle.X) * Matrix.CreateFromAxisAngle(matrix3.Up, m_actualAngle.Y) * Matrix.CreateFromAxisAngle(matrix3.Forward, m_actualAngle.Z);
					GetBillboardQuadRotated(billboard, ref position, radius, ref identity, matrix3.Left, matrix3.Up);
				}
				else if (m_generation.RotationReference == MyRotationReference.LocalAndCamera)
				{
					Vector3 vector4 = Vector3.Normalize(m_actualPosition - MyTransparentGeometry.Camera.Translation);
					Vector3 vector5 = m_generation.GetEffect().WorldMatrix.Forward;
					Matrix matrix4;
					if (vector4.Dot(vector5) >= 0.9999f)
					{
						matrix4 = Matrix.CreateTranslation(m_actualPosition);
					}
					else
					{
						Vector3 up2 = Vector3.Cross(Vector3.Cross(vector4, vector5), vector5);
						matrix4 = Matrix.CreateWorld(m_actualPosition, vector5, up2);
					}
					identity = Matrix.CreateFromAxisAngle(matrix4.Right, m_actualAngle.X) * Matrix.CreateFromAxisAngle(matrix4.Up, m_actualAngle.Y) * Matrix.CreateFromAxisAngle(matrix4.Forward, m_actualAngle.Z);
					GetBillboardQuadRotated(billboard, ref position, radius, ref identity, matrix4.Left, matrix4.Up);
				}
			}
			else if (Type == MyParticleTypeEnum.Line)
			{
				if (MyUtils.IsZero(Velocity.LengthSquared()))
				{
					Velocity = MyUtils.GetRandomVector3Normalized();
				}
				MyQuadD retQuad = default(MyQuadD);
				MyPolyLineD polyLine = default(MyPolyLineD);
				if (vector.LengthSquared() > 0f)
				{
					polyLine.LineDirectionNormalized = MyUtils.Normalize(vector);
				}
				else
				{
					polyLine.LineDirectionNormalized = MyUtils.Normalize(Velocity);
				}
				if (m_actualAngle.Z != 0f)
				{
					polyLine.LineDirectionNormalized = Vector3.TransformNormal(polyLine.LineDirectionNormalized, Matrix.CreateRotationY(m_actualAngle.Z));
				}
				polyLine.Point0 = position;
				polyLine.Point1.X = position.X - (double)(polyLine.LineDirectionNormalized.X * value);
				polyLine.Point1.Y = position.Y - (double)(polyLine.LineDirectionNormalized.Y * value);
				polyLine.Point1.Z = position.Z - (double)(polyLine.LineDirectionNormalized.Z * value);
				if (m_actualAngle.LengthSquared() > 0f)
				{
					polyLine.Point0.X = polyLine.Point0.X - (double)(polyLine.LineDirectionNormalized.X * value * 0.5f);
					polyLine.Point0.Y = polyLine.Point0.Y - (double)(polyLine.LineDirectionNormalized.Y * value * 0.5f);
					polyLine.Point0.Z = polyLine.Point0.Z - (double)(polyLine.LineDirectionNormalized.Z * value * 0.5f);
					polyLine.Point1.X = polyLine.Point1.X - (double)(polyLine.LineDirectionNormalized.X * value * 0.5f);
					polyLine.Point1.Y = polyLine.Point1.Y - (double)(polyLine.LineDirectionNormalized.Y * value * 0.5f);
					polyLine.Point1.Z = polyLine.Point1.Z - (double)(polyLine.LineDirectionNormalized.Z * value * 0.5f);
				}
				polyLine.Thickness = Thickness;
				Vector3D translation = MyTransparentGeometry.Camera.Translation;
				MyUtils.GetPolyLineQuad(out retQuad, ref polyLine, translation);
				identity.Forward = polyLine.LineDirectionNormalized;
				billboard.Position0 = retQuad.Point0;
				billboard.Position1 = retQuad.Point1;
				billboard.Position2 = retQuad.Point2;
				billboard.Position3 = retQuad.Point3;
			}
			else
			{
				if (Type != MyParticleTypeEnum.Trail)
				{
					throw new NotSupportedException(string.Concat(Type, " is not supported particle type"));
				}
				if (Quad.Point0 == Quad.Point2)
				{
					return false;
				}
				if (Quad.Point1 == Quad.Point3)
				{
					return false;
				}
				if (Quad.Point0 == Quad.Point3)
				{
					return false;
				}
				billboard.Position0 = Quad.Point0;
				billboard.Position1 = Quad.Point1;
				billboard.Position2 = Quad.Point2;
				billboard.Position3 = Quad.Point3;
			}
			if ((bool)m_generation.AlphaAnisotropic)
			{
				forward = Vector3.Normalize(Vector3.Cross(billboard.Position0 - billboard.Position1, billboard.Position0 - billboard.Position2));
				float num2 = Math.Abs(Vector3.Dot(Vector3.Normalize((Vector3)((Vector3)((billboard.Position0 + billboard.Position1 + billboard.Position2 + billboard.Position3) / 4.0) - MyTransparentGeometry.Camera.Translation)), forward)) * 2f;
				float num3 = num2 * num2;
				scaleFactor = Math.Min(num3 * num3, 1f);
			}
			Vector4 value3 = Vector4.One;
			if (Color.GetKeysCount() > 0)
			{
				Color.GetInterpolatedValue(m_normalizedTime, out value3);
			}
			if (m_arrayIndex != -1)
			{
				Vector3 vector6 = m_generation.ArraySize;
				if (vector6.X > 0f && vector6.Y > 0f)
				{
					int num4 = m_generation.ArrayOffset;
					int num5 = ((int)m_generation.ArrayModulo == 0) ? ((int)vector6.X * (int)vector6.Y) : ((int)m_generation.ArrayModulo);
					m_arrayIndex = m_arrayIndex % num5 + num4;
					float num6 = 1f / vector6.X;
					float num7 = 1f / vector6.Y;
					int num8 = m_arrayIndex % (int)vector6.X;
					int num9 = m_arrayIndex / (int)vector6.X;
					billboard.UVOffset = new Vector2(num6 * (float)num8, num7 * (float)num9);
					billboard.UVSize = new Vector2(num6, num7);
				}
			}
			MyTransparentMaterial value4 = MyTransparentMaterials.ErrorMaterial;
			Material.GetInterpolatedValue(m_normalizedTime, out value4);
			if (value4 != null)
			{
				billboard.Material = value4.Id;
			}
			billboard.Color = value3 * scaleFactor * m_generation.GetEffect().UserColorMultiplier;
			billboard.ColorIntensity = ColorIntensity;
			billboard.SoftParticleDistanceScale = SoftParticleDistanceScale;
			return true;
		}

		public void AddMotionInheritance(ref float motionInheritance, ref MatrixD deltaMatrix)
		{
			Vector3D value = Vector3D.Transform(m_actualPosition, deltaMatrix);
			m_actualPosition += (value - m_actualPosition) * motionInheritance;
			Velocity = Vector3.TransformNormal(Velocity, deltaMatrix);
		}

		/// <summary>
		/// Return quad whos face is always looking to the camera. 
		/// IMPORTANT: This bilboard looks same as point vertexes (point sprites) - horizontal and vertical axes of billboard are always parallel to screen
		/// That means, if billboard is in the left-up corner of screen, it won't be distorted by perspective. If will look as 2D quad on screen. As I said, it's same as GPU points.
		/// </summary>
		private static void GetBillboardQuadRotated(MyBillboard billboard, ref Vector3D position, float radius, float angle)
		{
			float num = radius * (float)Math.Cos(angle);
			float num2 = radius * (float)Math.Sin(angle);
			Vector3D vector3D = default(Vector3D);
			vector3D.X = (double)num * MyTransparentGeometry.Camera.Left.X + (double)num2 * MyTransparentGeometry.Camera.Up.X;
			vector3D.Y = (double)num * MyTransparentGeometry.Camera.Left.Y + (double)num2 * MyTransparentGeometry.Camera.Up.Y;
			vector3D.Z = (double)num * MyTransparentGeometry.Camera.Left.Z + (double)num2 * MyTransparentGeometry.Camera.Up.Z;
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = (double)(0f - num2) * MyTransparentGeometry.Camera.Left.X + (double)num * MyTransparentGeometry.Camera.Up.X;
			vector3D2.Y = (double)(0f - num2) * MyTransparentGeometry.Camera.Left.Y + (double)num * MyTransparentGeometry.Camera.Up.Y;
			vector3D2.Z = (double)(0f - num2) * MyTransparentGeometry.Camera.Left.Z + (double)num * MyTransparentGeometry.Camera.Up.Z;
			billboard.Position0.X = position.X + vector3D.X + vector3D2.X;
			billboard.Position0.Y = position.Y + vector3D.Y + vector3D2.Y;
			billboard.Position0.Z = position.Z + vector3D.Z + vector3D2.Z;
			billboard.Position1.X = position.X - vector3D.X + vector3D2.X;
			billboard.Position1.Y = position.Y - vector3D.Y + vector3D2.Y;
			billboard.Position1.Z = position.Z - vector3D.Z + vector3D2.Z;
			billboard.Position2.X = position.X - vector3D.X - vector3D2.X;
			billboard.Position2.Y = position.Y - vector3D.Y - vector3D2.Y;
			billboard.Position2.Z = position.Z - vector3D.Z - vector3D2.Z;
			billboard.Position3.X = position.X + vector3D.X - vector3D2.X;
			billboard.Position3.Y = position.Y + vector3D.Y - vector3D2.Y;
			billboard.Position3.Z = position.Z + vector3D.Z - vector3D2.Z;
		}

		private static void GetBillboardQuadRotated(MyBillboard billboard, ref Vector3D position, Vector2 radius, ref Matrix transform)
		{
			GetBillboardQuadRotated(billboard, ref position, radius, ref transform, MyTransparentGeometry.Camera.Left, MyTransparentGeometry.Camera.Up);
		}

		private static void GetBillboardQuadRotated(MyBillboard billboard, ref Vector3D position, Vector2 radius, ref Matrix transform, Vector3 left, Vector3 up)
		{
			Vector3 value = default(Vector3);
			value.X = radius.X * left.X;
			value.Y = radius.X * left.Y;
			value.Z = radius.X * left.Z;
			Vector3 value2 = default(Vector3);
			value2.X = radius.Y * up.X;
			value2.Y = radius.Y * up.Y;
			value2.Z = radius.Y * up.Z;
			Vector3D vector3D = Vector3.TransformNormal(value + value2, transform);
			Vector3D vector3D2 = Vector3.TransformNormal(value - value2, transform);
			billboard.Position0.X = position.X + vector3D.X;
			billboard.Position0.Y = position.Y + vector3D.Y;
			billboard.Position0.Z = position.Z + vector3D.Z;
			billboard.Position1.X = position.X - vector3D2.X;
			billboard.Position1.Y = position.Y - vector3D2.Y;
			billboard.Position1.Z = position.Z - vector3D2.Z;
			billboard.Position2.X = position.X - vector3D.X;
			billboard.Position2.Y = position.Y - vector3D.Y;
			billboard.Position2.Z = position.Z - vector3D.Z;
			billboard.Position3.X = position.X + vector3D2.X;
			billboard.Position3.Y = position.Y + vector3D2.Y;
			billboard.Position3.Z = position.Z + vector3D2.Z;
		}

		public bool IsValid()
		{
			if (Life <= 0f)
			{
				return false;
			}
			if (MyUtils.IsValid(StartPosition) && MyUtils.IsValid(Angle) && MyUtils.IsValid(Velocity) && MyUtils.IsValid(m_actualPosition))
			{
				return MyUtils.IsValid(m_actualAngle);
			}
			return false;
		}
	}
}
