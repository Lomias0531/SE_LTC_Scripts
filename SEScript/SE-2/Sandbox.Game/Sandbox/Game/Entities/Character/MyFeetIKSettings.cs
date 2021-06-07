using VRageMath;

namespace Sandbox.Game.Entities.Character
{
	public struct MyFeetIKSettings
	{
		public bool Enabled;

		public float BelowReachableDistance;

		public float AboveReachableDistance;

		public float VerticalShiftUpGain;

		public float VerticalShiftDownGain;

		public Vector3 FootSize;
	}
}
