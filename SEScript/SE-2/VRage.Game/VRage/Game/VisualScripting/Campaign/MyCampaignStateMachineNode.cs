using VRage.Generics;

namespace VRage.Game.VisualScripting.Campaign
{
	public class MyCampaignStateMachineNode : MyStateMachineNode
	{
		public string SavePath
		{
			get;
			set;
		}

		public bool Finished
		{
			get;
			private set;
		}

		public int InTransitionCount => InTransitions.Count;

		public MyCampaignStateMachineNode(string name)
			: base(name)
		{
		}

		public override void OnUpdate(MyStateMachine stateMachine)
		{
			if (OutTransitions.Count == 0)
			{
				Finished = true;
			}
		}
	}
}
