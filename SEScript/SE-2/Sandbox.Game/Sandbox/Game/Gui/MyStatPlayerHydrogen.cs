using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerHydrogen : MyStatBase
	{
		public MyStatPlayerHydrogen()
		{
			base.Id = MyStringHash.GetOrCompute("player_hydrogen");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.OxygenComponent != null)
			{
				base.CurrentValue = localCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId);
			}
		}

		public override string ToString()
		{
			return $"{base.CurrentValue * 100f:0}";
		}
	}
}
