namespace VRage.Generics
{
	/// <summary>
	/// Pair holding transition and its starting node.
	/// </summary>
	public struct MyStateMachineTransitionWithStart
	{
		public MyStateMachineNode StartNode;

		public MyStateMachineTransition Transition;

		/// <summary>
		/// Full constructor.
		/// </summary>
		public MyStateMachineTransitionWithStart(MyStateMachineNode startNode, MyStateMachineTransition transition)
		{
			StartNode = startNode;
			Transition = transition;
		}
	}
}
