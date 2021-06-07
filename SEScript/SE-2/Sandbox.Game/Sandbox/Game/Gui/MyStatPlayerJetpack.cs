using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerJetpack : MyStatBase
	{
		public MyStatPlayerJetpack()
		{
			base.Id = MyStringHash.GetOrCompute("player_jetpack");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.JetpackComp != null)
			{
				base.CurrentValue = (localCharacter.JetpackComp.TurnedOn ? 1f : 0f);
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
