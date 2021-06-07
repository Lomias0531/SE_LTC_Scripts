using Sandbox.Game.Entities.Character;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.Game.GameSystems
{
	public interface IMyLifeSupportingBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity
	{
		bool RefuelAllowed
		{
			get;
		}

		bool HealingAllowed
		{
			get;
		}

		MyLifeSupportingBlockType BlockType
		{
			get;
		}

		void ShowTerminal(MyCharacter user);

		void BroadcastSupportRequest(MyCharacter user);
	}
}
