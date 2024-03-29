using Sandbox.Definitions;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyBehaviorTree
	{
		public class MyBehaviorTreeDesc
		{
			public List<MyBehaviorTreeNode> Nodes
			{
				get;
				private set;
			}

			public HashSet<MyStringId> ActionIds
			{
				get;
				private set;
			}

			public int MemorableNodesCounter
			{
				get;
				set;
			}

			public MyBehaviorTreeDesc()
			{
				Nodes = new List<MyBehaviorTreeNode>(20);
				ActionIds = new HashSet<MyStringId>(MyStringId.Comparer);
				MemorableNodesCounter = 0;
			}
		}

		private static List<MyStringId> m_tmpHelper = new List<MyStringId>();

		private MyBehaviorTreeNode m_root;

		private MyBehaviorTreeDesc m_treeDesc;

		private MyBehaviorDefinition m_behaviorDefinition;

		public int TotalNodeCount => m_treeDesc.Nodes.Count;

		public MyBehaviorDefinition BehaviorDefinition => m_behaviorDefinition;

		public string BehaviorTreeName => m_behaviorDefinition.Id.SubtypeName;

		public MyStringHash BehaviorTreeId => m_behaviorDefinition.Id.SubtypeId;

		public MyBehaviorTree(MyBehaviorDefinition def)
		{
			m_behaviorDefinition = def;
			m_treeDesc = new MyBehaviorTreeDesc();
		}

		public void ReconstructTree(MyBehaviorDefinition def)
		{
			m_behaviorDefinition = def;
			Construct();
		}

		public void Construct()
		{
			ClearData();
			m_root = new MyBehaviorTreeRoot();
			m_root.Construct(m_behaviorDefinition.FirstNode, m_treeDesc);
		}

		public void ClearData()
		{
			m_treeDesc.MemorableNodesCounter = 0;
			m_treeDesc.ActionIds.Clear();
			m_treeDesc.Nodes.Clear();
		}

		public void Tick(IMyBot bot)
		{
			m_root.Tick(bot, bot.BotMemory.CurrentTreeBotMemory);
		}

		public void CallPostTickOnPath(IMyBot bot, MyPerTreeBotMemory botTreeMemory, IEnumerable<int> postTickNodes)
		{
			foreach (int postTickNode in postTickNodes)
			{
				m_treeDesc.Nodes[postTickNode].PostTick(bot, botTreeMemory);
			}
		}

		public bool IsCompatibleWithBot(ActionCollection botActions)
		{
			foreach (MyStringId actionId in m_treeDesc.ActionIds)
			{
				if (!botActions.ContainsActionDesc(actionId))
				{
					m_tmpHelper.Add(actionId);
				}
			}
			if (m_tmpHelper.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder("Error! The behavior tree is not compatible with the bot. Missing bot actions: ");
				foreach (MyStringId item in m_tmpHelper)
				{
					stringBuilder.Append(item.ToString());
					stringBuilder.Append(", ");
				}
				m_tmpHelper.Clear();
				return false;
			}
			return true;
		}

		public MyBehaviorTreeNode GetNodeByIndex(int index)
		{
			if (index >= m_treeDesc.Nodes.Count)
			{
				return null;
			}
			return m_treeDesc.Nodes[index];
		}

		public override int GetHashCode()
		{
			return m_root.GetHashCode();
		}
	}
}
