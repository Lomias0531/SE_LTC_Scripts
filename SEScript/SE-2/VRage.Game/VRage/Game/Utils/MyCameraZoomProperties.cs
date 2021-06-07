using VRageMath;

namespace VRage.Game.Utils
{
	public class MyCameraZoomProperties
	{
		private static readonly float FIELD_OF_VIEW_MIN = MathHelper.ToRadians(40f);

		private float ZoomTime = 0.075f;

		private float m_currentZoomTime;

		private MyCameraZoomOperationType m_zoomType;

		private float m_FOV;

		private float m_zoomLevel;

		private MyCamera m_camera;

		public bool ApplyToFov
		{
			get;
			set;
		}

		public MyCameraZoomProperties(MyCamera camera)
		{
			m_camera = camera;
			Update(0f);
		}

		public void Update(float updateStepSize)
		{
			switch (m_zoomType)
			{
			case MyCameraZoomOperationType.ZoomingIn:
				if (m_currentZoomTime <= ZoomTime)
				{
					m_currentZoomTime += updateStepSize;
					if (m_currentZoomTime >= ZoomTime)
					{
						m_currentZoomTime = ZoomTime;
						m_zoomType = MyCameraZoomOperationType.Zoomed;
					}
				}
				break;
			case MyCameraZoomOperationType.ZoomingOut:
				if (m_currentZoomTime >= 0f)
				{
					m_currentZoomTime -= updateStepSize;
					if (m_currentZoomTime <= 0f)
					{
						m_currentZoomTime = 0f;
						m_zoomType = MyCameraZoomOperationType.NoZoom;
					}
				}
				break;
			}
			m_zoomLevel = 1f - m_currentZoomTime / ZoomTime;
			m_FOV = (ApplyToFov ? MathHelper.Lerp(FIELD_OF_VIEW_MIN, m_camera.FieldOfView, m_zoomLevel) : m_camera.FieldOfView);
		}

		public void ResetZoom()
		{
			m_zoomType = MyCameraZoomOperationType.NoZoom;
			m_currentZoomTime = 0f;
		}

		public void SetZoom(MyCameraZoomOperationType inZoomType)
		{
			m_zoomType = inZoomType;
		}

		public float GetZoomLevel()
		{
			return m_zoomLevel;
		}

		public float GetFOV()
		{
			return MyMath.Clamp(m_FOV, 1E-05f, 3.14158273f);
		}

		public bool IsZooming()
		{
			return m_zoomType != MyCameraZoomOperationType.NoZoom;
		}
	}
}
