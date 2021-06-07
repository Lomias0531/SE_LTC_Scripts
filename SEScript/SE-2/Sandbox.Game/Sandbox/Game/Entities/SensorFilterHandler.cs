using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
	internal delegate void SensorFilterHandler(MySensorBase sender, MyEntity detectedEntity, ref bool processEntity);
}
