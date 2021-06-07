using VRage.Utils;

namespace VRage.Generics
{
	/// <summary>
	/// Implementation of generic state machine. Inherit from this class to create your own state machine.
	/// Transitions are performed automatically on each update (if conditions of transition are fulfilled).
	/// </summary>
	public class MySingleStateMachine : MyStateMachine
	{
		public delegate void StateChangedHandler(MyStateMachineTransitionWithStart transition, MyStringId action);

		public MyStateMachineNode CurrentNode
		{
			get
			{
				if (m_activeCursors.Count == 0)
				{
					return null;
				}
				return m_activeCursors[0].Node;
			}
		}

		public event StateChangedHandler OnStateChanged;

		protected void NotifyStateChanged(MyStateMachineTransitionWithStart transitionWithStart, MyStringId action)
		{
			if (this.OnStateChanged != null)
			{
				this.OnStateChanged(transitionWithStart, action);
			}
		}

		public override bool DeleteCursor(int id)
		{
			return false;
		}

		public override MyStateMachineCursor CreateCursor(string nodeName)
		{
			return null;
		}

		public bool SetState(string nameOfNewState)
		{
			if (m_activeCursors.Count == 0)
			{
				if (base.CreateCursor(nameOfNewState) == null)
				{
					return false;
				}
				m_activeCursors.ApplyChanges();
				m_activeCursors[0].OnCursorStateChanged += CursorStateChanged;
			}
			else
			{
				MyStateMachineNode node = FindNode(nameOfNewState);
				m_activeCursors[0].Node = node;
			}
			return true;
		}

		private void CursorStateChanged(int transitionId, MyStringId action, MyStateMachineNode node, MyStateMachine stateMachine)
		{
			NotifyStateChanged(m_transitions[transitionId], action);
		}
	}
}
