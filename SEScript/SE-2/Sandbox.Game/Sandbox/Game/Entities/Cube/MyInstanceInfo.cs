using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	internal struct MyInstanceInfo
	{
		public MyInstanceFlagsEnum Flags;

		public float MaxViewDistance;

		public MyInstanceInfo(MyInstanceFlagsEnum flags, float maxViewDistance)
		{
			Flags = flags;
			MaxViewDistance = maxViewDistance;
		}
	}
}
