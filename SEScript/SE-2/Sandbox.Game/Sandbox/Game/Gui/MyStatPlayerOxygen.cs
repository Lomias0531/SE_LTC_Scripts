using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerOxygen : MyStatBase
	{
		public MyStatPlayerOxygen()
		{
			base.Id = MyStringHash.GetOrCompute("player_oxygen");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.OxygenComponent != null)
			{
				base.CurrentValue = localCharacter.OxygenComponent.SuitOxygenLevel;
			}
		}

		public override string ToString()
		{
			return $"{base.CurrentValue * 100f:0}";
		}
	}
}
