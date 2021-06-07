using Sandbox.Game.Entities.Character.Components;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerRefillingHydrogen : MyStatPlayerGasRefillingBase
	{
		public MyStatPlayerRefillingHydrogen()
		{
			base.Id = MyStringHash.GetOrCompute("player_refilling_hydrogen");
		}

		protected override float GetGassLevel(MyCharacterOxygenComponent oxygenComp)
		{
			return oxygenComp.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId);
		}
	}
}
