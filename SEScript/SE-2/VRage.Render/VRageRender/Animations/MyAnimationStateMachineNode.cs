using System.Collections.Generic;
using VRage.Generics;
using VRage.Utils;

namespace VRageRender.Animations
{
	/// <summary>
	/// Animation state machine node is a representation of one state inside MyAnimationStateMachine.
	/// </summary>
	public class MyAnimationStateMachineNode : MyStateMachineNode
	{
		public struct VarAssignmentData
		{
			public MyStringId VariableId;

			public float Value;
		}

		private MyAnimationTreeNode m_rootAnimationNode;

		public List<VarAssignmentData> VariableAssignments;

		public MyAnimationTreeNode RootAnimationNode
		{
			get
			{
				return m_rootAnimationNode;
			}
			set
			{
				m_rootAnimationNode = value;
			}
		}

		public MyAnimationStateMachineNode(string name)
			: base(name)
		{
		}

		public MyAnimationStateMachineNode(string name, MyAnimationClip animationClip)
			: base(name)
		{
			if (animationClip != null)
			{
				MyAnimationTreeNodeTrack myAnimationTreeNodeTrack = new MyAnimationTreeNodeTrack();
				myAnimationTreeNodeTrack.SetClip(animationClip);
				m_rootAnimationNode = myAnimationTreeNodeTrack;
			}
		}

		protected override MyStateMachineTransition QueryNextTransition()
		{
			for (int i = 0; i < OutTransitions.Count; i++)
			{
				if (OutTransitions[i].Name == MyStringId.NullOrEmpty && OutTransitions[i].Evaluate())
				{
					return OutTransitions[i];
				}
			}
			return null;
		}

		public override void OnUpdate(MyStateMachine stateMachine)
		{
			MyAnimationStateMachine myAnimationStateMachine = stateMachine as MyAnimationStateMachine;
			if (myAnimationStateMachine != null)
			{
				if (m_rootAnimationNode != null)
				{
					myAnimationStateMachine.CurrentUpdateData.AddVisitedTreeNodesPathPoint(1);
					m_rootAnimationNode.Update(ref myAnimationStateMachine.CurrentUpdateData);
				}
				else
				{
					myAnimationStateMachine.CurrentUpdateData.BonesResult = myAnimationStateMachine.CurrentUpdateData.Controller.ResultBonesPool.Alloc();
				}
				myAnimationStateMachine.CurrentUpdateData.AddVisitedTreeNodesPathPoint(0);
			}
		}
	}
}
