using Sandbox.Game.World;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	public class MyEntityCameraSettings
	{
		public double Distance;

		public Vector2? HeadAngle;

		private bool m_isFirstPerson;

		public bool IsFirstPerson
		{
			get
			{
				if (!m_isFirstPerson)
				{
					return !MySession.Static.Settings.Enable3rdPersonView;
				}
				return true;
			}
			set
			{
				m_isFirstPerson = value;
			}
		}
	}
}
