using VRage.Game;

namespace Sandbox.Game.EntityComponents
{
	public delegate void MyResourceOutputChangedDelegate(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source);
}
