using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatHudShowStates : MyStatBase
	{
		public MyStatHudShowStates()
		{
			base.Id = MyStringHash.GetOrCompute("hud_show_states");
		}

		public override void Update()
		{
		}
	}
}
