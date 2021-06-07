using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerBroadcasting : MyStatBase
	{
		public MyStatPlayerBroadcasting()
		{
			base.Id = MyStringHash.GetOrCompute("player_broadcasting");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.RadioBroadcaster != null)
			{
				base.CurrentValue = (localCharacter.RadioBroadcaster.Enabled ? 1f : 0f);
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
