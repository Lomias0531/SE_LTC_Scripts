using VRage.Utils;
using VRageMath;

namespace VRage
{
	public class MySpectator
	{
		public static MySpectator Static;

		public const float DEFAULT_SPECTATOR_LINEAR_SPEED = 0.1f;

		public const float MIN_SPECTATOR_LINEAR_SPEED = 0.0001f;

		public const float MAX_SPECTATOR_LINEAR_SPEED = 8000f;

		public const float DEFAULT_SPECTATOR_ANGULAR_SPEED = 1f;

		public const float MIN_SPECTATOR_ANGULAR_SPEED = 0.0001f;

		public const float MAX_SPECTATOR_ANGULAR_SPEED = 6f;

		public Vector3D ThirdPersonCameraDelta = new Vector3D(-10.0, 10.0, -10.0);

		private MySpectatorCameraMovementEnum m_spectatorCameraMovement;

		private Vector3D m_position;

		private Vector3D m_targetDelta = Vector3D.Forward;

		private Vector3D? m_up;

		protected float m_speedModeLinear = 0.1f;

		protected float m_speedModeAngular = 1f;

		protected MatrixD m_orientation = MatrixD.Identity;

		protected bool m_orientationDirty = true;

		private float m_orbitY;

		private float m_orbitX;

		public MySpectatorCameraMovementEnum SpectatorCameraMovement
		{
			get
			{
				return m_spectatorCameraMovement;
			}
			set
			{
				if (m_spectatorCameraMovement != value)
				{
					OnChangingMode(m_spectatorCameraMovement, value);
				}
				m_spectatorCameraMovement = value;
			}
		}

		public bool IsInFirstPersonView
		{
			get;
			set;
		}

		public bool ForceFirstPersonCamera
		{
			get;
			set;
		}

		public bool Initialized
		{
			get;
			set;
		}

		public Vector3D Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_position = value;
			}
		}

		public float SpeedModeLinear
		{
			get
			{
				return m_speedModeLinear;
			}
			set
			{
				m_speedModeLinear = value;
			}
		}

		public float SpeedModeAngular
		{
			get
			{
				return m_speedModeAngular;
			}
			set
			{
				m_speedModeAngular = value;
			}
		}

		public Vector3D Target
		{
			get
			{
				return Position + m_targetDelta;
			}
			set
			{
				Vector3D vector3D = value - Position;
				m_orientationDirty = (m_targetDelta != vector3D);
				m_targetDelta = vector3D;
				m_up = null;
			}
		}

		public MatrixD Orientation
		{
			get
			{
				if (m_orientationDirty)
				{
					UpdateOrientation();
					m_orientationDirty = false;
				}
				return m_orientation;
			}
		}

		protected virtual void OnChangingMode(MySpectatorCameraMovementEnum oldMode, MySpectatorCameraMovementEnum newMode)
		{
		}

		public MySpectator()
		{
			Static = this;
		}

		public void SetTarget(Vector3D target, Vector3D? up)
		{
			Target = target;
			m_orientationDirty |= (m_up != up);
			m_up = up;
		}

		public void UpdateOrientation()
		{
			Vector3D vector3D = MyUtils.Normalize(m_targetDelta);
			vector3D = ((vector3D.LengthSquared() > 0.0) ? vector3D : Vector3D.Forward);
			m_orientation = MatrixD.CreateFromDir(vector3D, m_up.HasValue ? m_up.Value : Vector3D.Up);
		}

		public void Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(Vector3.Zero, rotationIndicator, rollIndicator);
		}

		public void RotateStopped()
		{
			MoveAndRotateStopped();
		}

		public virtual void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			_ = Position;
			moveIndicator *= m_speedModeLinear;
			float num = 0.1f;
			float num2 = 0.0025f * m_speedModeAngular;
			Vector3D position = (Vector3D)moveIndicator * (double)num;
			switch (SpectatorCameraMovement)
			{
			case MySpectatorCameraMovementEnum.FreeMouse:
			case MySpectatorCameraMovementEnum.None:
				break;
			case MySpectatorCameraMovementEnum.UserControlled:
				if (rollIndicator != 0f)
				{
					float value3 = rollIndicator * m_speedModeLinear * 0.1f;
					value3 = MathHelper.Clamp(value3, -0.02f, 0.02f);
					MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Right, out Vector3D xOut, out Vector3D yOut, value3);
					m_orientation.Right = yOut;
					m_orientation.Up = xOut;
				}
				if (rotationIndicator.X != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Up, m_orientation.Forward, out Vector3D xOut2, out Vector3D yOut2, rotationIndicator.X * num2);
					m_orientation.Up = xOut2;
					m_orientation.Forward = yOut2;
				}
				if (rotationIndicator.Y != 0f)
				{
					MyUtils.VectorPlaneRotation(m_orientation.Right, m_orientation.Forward, out Vector3D xOut3, out Vector3D yOut3, (0f - rotationIndicator.Y) * num2);
					m_orientation.Right = xOut3;
					m_orientation.Forward = yOut3;
				}
				Position += Vector3D.Transform(position, m_orientation);
				break;
			case MySpectatorCameraMovementEnum.Orbit:
			{
				m_orbitY += rotationIndicator.Y * 0.01f;
				m_orbitX += rotationIndicator.X * 0.01f;
				Vector3D position4 = -m_targetDelta;
				Vector3D value2 = Position + m_targetDelta;
				MatrixD matrix2 = Matrix.Invert(Orientation);
				Vector3D position5 = Vector3D.Transform(position4, matrix2);
				rotationIndicator *= 0.01f;
				MatrixD matrixD2 = MatrixD.CreateRotationX(m_orbitX) * MatrixD.CreateRotationY(m_orbitY) * MatrixD.CreateRotationZ(rollIndicator);
				position4 = Vector3D.Transform(position5, matrixD2);
				Position = value2 + position4;
				m_targetDelta = -position4;
				Vector3D vector3D = m_orientation.Right * position.X + m_orientation.Up * position.Y;
				Position += vector3D;
				Vector3D vector3D2 = m_orientation.Forward * (0.0 - position.Z);
				Position += vector3D2;
				m_targetDelta -= vector3D2;
				m_orientation = matrixD2;
				break;
			}
			case MySpectatorCameraMovementEnum.ConstantDelta:
			{
				m_orbitY += rotationIndicator.Y * 0.01f;
				m_orbitX += rotationIndicator.X * 0.01f;
				Vector3D position2 = -m_targetDelta;
				Vector3D value = Position + m_targetDelta;
				MatrixD matrix = Matrix.Invert(Orientation);
				Vector3D position3 = Vector3D.Transform(position2, matrix);
				rotationIndicator *= 0.01f;
				MatrixD matrixD = MatrixD.CreateRotationX(m_orbitX) * MatrixD.CreateRotationY(m_orbitY) * MatrixD.CreateRotationZ(rollIndicator);
				position2 = Vector3D.Transform(position3, matrixD);
				Position = value + position2;
				m_targetDelta = -position2;
				m_orientation = matrixD;
				break;
			}
			}
		}

		public virtual void Update()
		{
		}

		public virtual void MoveAndRotateStopped()
		{
		}

		public MatrixD GetViewMatrix()
		{
			return MatrixD.Invert(MatrixD.CreateWorld(Position, Orientation.Forward, Orientation.Up));
		}

		public void SetViewMatrix(MatrixD viewMatrix)
		{
			MatrixD matrixD = MatrixD.Invert(viewMatrix);
			Position = matrixD.Translation;
			m_orientation = MatrixD.Identity;
			m_orientation.Right = matrixD.Right;
			m_orientation.Up = matrixD.Up;
			m_orientation.Forward = matrixD.Forward;
			m_orientationDirty = false;
		}

		public void Reset()
		{
			m_position = Vector3.Zero;
			m_targetDelta = Vector3.Forward;
			ThirdPersonCameraDelta = new Vector3D(-10.0, 10.0, -10.0);
			m_orientationDirty = true;
			m_orbitX = 0f;
			m_orbitY = 0f;
		}
	}
}
