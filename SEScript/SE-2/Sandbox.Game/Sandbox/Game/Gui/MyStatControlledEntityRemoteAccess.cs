using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityRemoteAccess : MyStatBase
	{
		public MyStatControlledEntityRemoteAccess()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_remote_access");
		}

		public override void Update()
		{
			base.CurrentValue = 0f;
		}
	}
}
