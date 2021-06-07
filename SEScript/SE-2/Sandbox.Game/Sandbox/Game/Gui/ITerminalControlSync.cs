using Sandbox.Game.Entities.Cube;
using VRage.Library.Collections;

namespace Sandbox.Game.Gui
{
	public interface ITerminalControlSync
	{
		void Serialize(BitStream stream, MyTerminalBlock block);
	}
}
