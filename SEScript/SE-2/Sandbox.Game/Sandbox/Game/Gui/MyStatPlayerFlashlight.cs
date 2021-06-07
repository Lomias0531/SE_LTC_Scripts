using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerFlashlight : MyStatBase
	{
		public MyStatPlayerFlashlight()
		{
			base.Id = MyStringHash.GetOrCompute("player_flashlight");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				base.CurrentValue = (localCharacter.LightEnabled ? 1f : 0f);
			}
		}
	}
}
