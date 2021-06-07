using VRage.Game.ObjectBuilders.Campaign;
using VRage.Generics;

namespace VRage.Game.VisualScripting.Campaign
{
	public class MyCampaignStateMachine : MySingleStateMachine
	{
		private MyObjectBuilder_CampaignSM m_objectBuilder;

		public bool Initialized => m_objectBuilder != null;

		public bool Finished => ((MyCampaignStateMachineNode)base.CurrentNode).Finished;

		public void Deserialize(MyObjectBuilder_CampaignSM ob)
		{
			if (m_objectBuilder == null)
			{
				m_objectBuilder = ob;
				MyObjectBuilder_CampaignSMNode[] nodes = m_objectBuilder.Nodes;
				foreach (MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode in nodes)
				{
					MyCampaignStateMachineNode newNode = new MyCampaignStateMachineNode(myObjectBuilder_CampaignSMNode.Name)
					{
						SavePath = myObjectBuilder_CampaignSMNode.SaveFilePath
					};
					AddNode(newNode);
				}
				MyObjectBuilder_CampaignSMTransition[] transitions = m_objectBuilder.Transitions;
				foreach (MyObjectBuilder_CampaignSMTransition myObjectBuilder_CampaignSMTransition in transitions)
				{
					AddTransition(myObjectBuilder_CampaignSMTransition.From, myObjectBuilder_CampaignSMTransition.To, null, myObjectBuilder_CampaignSMTransition.Name);
				}
			}
		}

		public void ResetToStart()
		{
			foreach (MyStateMachineNode value in m_nodes.Values)
			{
				MyCampaignStateMachineNode myCampaignStateMachineNode = value as MyCampaignStateMachineNode;
				if (myCampaignStateMachineNode != null && myCampaignStateMachineNode.InTransitionCount == 0)
				{
					SetState(myCampaignStateMachineNode.Name);
					break;
				}
			}
		}
	}
}
