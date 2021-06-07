using VRage.Game;

namespace Sandbox.Game.EntityComponents
{
	public delegate void MyRequiredResourceChangeDelegate(MyDefinitionId changedResourceTypeId, MyResourceSinkComponent sink, float oldRequirement, float newRequirement);
}
