using System;
using VRage.Game.Entity;

namespace Sandbox.Game.GameSystems
{
	public class DroneTarget : IComparable<DroneTarget>
	{
		public MyEntity Target;

		public int Priority;

		public DroneTarget(MyEntity target)
		{
			Target = target;
			Priority = 1;
		}

		public DroneTarget(MyEntity target, int priority)
		{
			Target = target;
			Priority = priority;
		}

		public int CompareTo(DroneTarget other)
		{
			return Priority.CompareTo(other.Priority);
		}
	}
}
