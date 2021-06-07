using VRage.Generics;
using VRage.Utils;

namespace VRageRender.Animations
{
	/// <summary>
	/// Description of transition to another state (MyAnimationStateMachineNode) in the state machine (MyAnimationStateMachine).
	/// </summary>
	public class MyAnimationStateMachineTransition : MyStateMachineTransition
	{
		public double TransitionTimeInSec;

		public MyAnimationTransitionSyncType Sync = MyAnimationTransitionSyncType.NoSynchonization;

		public MyAnimationTransitionCurve Curve;

		/// <summary>
		/// Animation transition evaluation - different behavior from default transition. 
		/// If no conditions are given and it has a name, it must be triggered manually.
		///
		/// </summary>
		public override bool Evaluate()
		{
			if (Conditions.Count > 0)
			{
				return base.Evaluate();
			}
			return Name == MyStringId.NullOrEmpty;
		}
	}
}
