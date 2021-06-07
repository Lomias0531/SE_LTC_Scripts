using System.Threading;
using VRage.Utils;

namespace VRage.Generics
{
	public class MyStateMachineCursor
	{
		public delegate void CursorStateChanged(int transitionId, MyStringId action, MyStateMachineNode node, MyStateMachine stateMachine);

		private static int m_idCounter;

		private readonly MyStateMachine m_stateMachine;

		private MyStateMachineNode m_node;

		public readonly int Id;

		public MyStateMachineNode Node
		{
			get
			{
				return m_node;
			}
			internal set
			{
				m_node = value;
			}
		}

		public int LastTransitionTakenId
		{
			get;
			private set;
		}

		public MyStateMachine StateMachine => m_stateMachine;

		public event CursorStateChanged OnCursorStateChanged;

		public MyStateMachineCursor(MyStateMachineNode node, MyStateMachine stateMachine)
		{
			m_stateMachine = stateMachine;
			Id = Interlocked.Increment(ref m_idCounter);
			m_node = node;
			m_node.Cursors.Add(this);
			this.OnCursorStateChanged = null;
		}

		private void NotifyCursorChanged(MyStateMachineTransition transition, MyStringId action)
		{
			if (this.OnCursorStateChanged != null)
			{
				this.OnCursorStateChanged(transition.Id, action, Node, StateMachine);
			}
		}

		public void FollowTransition(MyStateMachineTransition transition, MyStringId action)
		{
			Node.Cursors.Remove(this);
			transition.TargetNode.Cursors.Add(this);
			Node = transition.TargetNode;
			LastTransitionTakenId = transition.Id;
			NotifyCursorChanged(transition, action);
		}
	}
}
