using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerEnergy : MyStatBase
	{
		public MyStatPlayerEnergy()
		{
			base.Id = MyStringHash.GetOrCompute("player_energy");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				base.CurrentValue = localCharacter.SuitEnergyLevel;
			}
		}

		public override string ToString()
		{
			return $"{base.CurrentValue * 100f:0}";
		}
	}
}
