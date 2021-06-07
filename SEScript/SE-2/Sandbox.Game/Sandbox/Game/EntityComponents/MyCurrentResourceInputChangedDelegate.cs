using VRage.Game;

namespace Sandbox.Game.EntityComponents
{
	public delegate void MyCurrentResourceInputChangedDelegate(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink);
}
