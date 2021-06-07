using Havok;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;

namespace Sandbox.Engine.Physics
{
	internal abstract class PhysicsStepOptimizerBase : IPhysicsStepOptimizer
	{
		public abstract void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings);

		public abstract void DisableOptimizations();

		public abstract void Unload();

		protected static void ForEverySignificantWorld(List<MyTuple<HkWorld, MyTimeSpan>> timings, Action<HkWorld> action)
		{
			double num = 0.0;
			foreach (MyTuple<HkWorld, MyTimeSpan> timing in timings)
			{
				double num2 = num;
				MyTimeSpan item = timing.Item2;
				num = num2 + item.Milliseconds;
			}
			double num3 = num / (double)timings.Count;
			foreach (MyTuple<HkWorld, MyTimeSpan> timing2 in timings)
			{
				MyTimeSpan item = timing2.Item2;
				if (item.Milliseconds >= num3)
				{
					action(timing2.Item1);
				}
			}
		}

		protected static void ForEveryActivePhysicsBodyOfType<TBody>(HkWorld world, Action<TBody> action) where TBody : class
		{
			foreach (HkRigidBody activeRigidBody in world.ActiveRigidBodies)
			{
				TBody val = activeRigidBody.UserObject as TBody;
				if (val != null)
				{
					action(val);
				}
			}
		}
	}
}
