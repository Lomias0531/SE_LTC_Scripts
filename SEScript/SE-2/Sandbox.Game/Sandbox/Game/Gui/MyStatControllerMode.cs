using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControllerMode : MyStatBase
	{
		public MyStatControllerMode()
		{
			base.Id = MyStringHash.GetOrCompute("controller_mode");
		}

		public override void Update()
		{
			base.CurrentValue = (MyInput.Static.IsJoystickLastUsed ? 1 : 0);
		}
	}
}
