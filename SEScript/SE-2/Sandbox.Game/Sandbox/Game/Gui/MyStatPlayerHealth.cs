using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerHealth : MyStatBase
	{
		public MyStatPlayerHealth()
		{
			base.Id = MyStringHash.GetOrCompute("player_health");
		}

		public override void Update()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.StatComp != null)
			{
				base.CurrentValue = myCharacter.StatComp.HealthRatio;
			}
		}

		public override string ToString()
		{
			return $"{base.CurrentValue * 100f:0}";
		}
	}
}
