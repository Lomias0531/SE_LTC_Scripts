using Sandbox.Game.Entities.Character.Components;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerRefillingOxygen : MyStatPlayerGasRefillingBase
	{
		public MyStatPlayerRefillingOxygen()
		{
			base.Id = MyStringHash.GetOrCompute("player_refilling_oxygen");
		}

		protected override float GetGassLevel(MyCharacterOxygenComponent oxygenComp)
		{
			return oxygenComp.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId);
		}
	}
}
