using Sandbox.Game.AI.BehaviorTree;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.AI
{
	public class MyBotMemory
	{
		private IMyBot m_memoryUser;

		private MyBehaviorTree m_behaviorTree;

		private MyPerTreeBotMemory m_treeBotMemory;

		private Stack<int> m_newNodePath;

		private HashSet<int> m_oldNodePath;

		public MyPerTreeBotMemory CurrentTreeBotMemory
		{
			get
			{
				return m_treeBotMemory;
			}
			private set
			{
				m_treeBotMemory = value;
			}
		}

		public bool HasOldPath => m_oldNodePath.Count > 0;

		public int LastRunningNodeIndex
		{
			get;
			private set;
		}

		public bool HasPathToSave => m_newNodePath.Count > 0;

		public int TickCounter
		{
			get;
			private set;
		}

		public MyBotMemory Clone()
		{
			MyBotMemory myBotMemory = new MyBotMemory(m_memoryUser);
			myBotMemory.m_behaviorTree = m_behaviorTree;
			MyObjectBuilder_BotMemory myObjectBuilder_BotMemory = new MyObjectBuilder_BotMemory();
			myObjectBuilder_BotMemory = GetObjectBuilder();
			myBotMemory.Init(myObjectBuilder_BotMemory);
			return myBotMemory;
		}

		public MyBotMemory(IMyBot bot)
		{
			LastRunningNodeIndex = -1;
			m_memoryUser = bot;
			m_newNodePath = new Stack<int>(20);
			m_oldNodePath = new HashSet<int>();
		}

		public void Init(MyObjectBuilder_BotMemory builder)
		{
			if (builder.BehaviorTreeMemory != null)
			{
				MyPerTreeBotMemory myPerTreeBotMemory = new MyPerTreeBotMemory();
				foreach (MyObjectBuilder_BehaviorTreeNodeMemory item in builder.BehaviorTreeMemory.Memory)
				{
					MyBehaviorTreeNodeMemory myBehaviorTreeNodeMemory = MyBehaviorTreeNodeMemoryFactory.CreateNodeMemory(item);
					myBehaviorTreeNodeMemory.Init(item);
					myPerTreeBotMemory.AddNodeMemory(myBehaviorTreeNodeMemory);
				}
				if (builder.BehaviorTreeMemory.BlackboardMemory != null)
				{
					foreach (MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory item2 in builder.BehaviorTreeMemory.BlackboardMemory)
					{
						myPerTreeBotMemory.AddBlackboardMemoryInstance(item2.MemberName, item2.Value);
					}
				}
				CurrentTreeBotMemory = myPerTreeBotMemory;
			}
			if (builder.OldPath != null)
			{
				for (int i = 0; i < builder.OldPath.Count; i++)
				{
					m_oldNodePath.Add(i);
				}
			}
			if (builder.NewPath != null)
			{
				for (int j = 0; j < builder.NewPath.Count; j++)
				{
					m_newNodePath.Push(builder.NewPath[j]);
				}
			}
			LastRunningNodeIndex = builder.LastRunningNodeIndex;
			TickCounter = 0;
		}

		public MyObjectBuilder_BotMemory GetObjectBuilder()
		{
			MyObjectBuilder_BotMemory myObjectBuilder_BotMemory = new MyObjectBuilder_BotMemory();
			myObjectBuilder_BotMemory.LastRunningNodeIndex = LastRunningNodeIndex;
			myObjectBuilder_BotMemory.NewPath = m_newNodePath.ToList();
			myObjectBuilder_BotMemory.OldPath = m_oldNodePath.ToList();
			MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory behaviorTreeNodesMemory = new MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory();
			behaviorTreeNodesMemory.BehaviorName = m_behaviorTree.BehaviorTreeName;
			behaviorTreeNodesMemory.Memory = new List<MyObjectBuilder_BehaviorTreeNodeMemory>(CurrentTreeBotMemory.NodesMemoryCount);
			foreach (MyBehaviorTreeNodeMemory item in CurrentTreeBotMemory.NodesMemory)
			{
				behaviorTreeNodesMemory.Memory.Add(item.GetObjectBuilder());
			}
			behaviorTreeNodesMemory.BlackboardMemory = new List<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory>();
			foreach (KeyValuePair<MyStringId, MyBBMemoryValue> item2 in CurrentTreeBotMemory.BBMemory)
			{
				MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory behaviorTreeBlackboardMemory = new MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory();
				behaviorTreeBlackboardMemory.MemberName = item2.Key.ToString();
				behaviorTreeBlackboardMemory.Value = item2.Value;
				behaviorTreeNodesMemory.BlackboardMemory.Add(behaviorTreeBlackboardMemory);
			}
			myObjectBuilder_BotMemory.BehaviorTreeMemory = behaviorTreeNodesMemory;
			return myObjectBuilder_BotMemory;
		}

		public void AssignBehaviorTree(MyBehaviorTree behaviorTree)
		{
			if (CurrentTreeBotMemory == null && (m_behaviorTree == null || behaviorTree.BehaviorTreeId == m_behaviorTree.BehaviorTreeId))
			{
				CurrentTreeBotMemory = CreateBehaviorTreeMemory(behaviorTree);
			}
			else if (!ValidateMemoryForBehavior(behaviorTree))
			{
				CurrentTreeBotMemory.Clear();
				ClearPathMemory(postTick: false);
				ResetMemoryInternal(behaviorTree, CurrentTreeBotMemory);
			}
			m_behaviorTree = behaviorTree;
		}

		private MyPerTreeBotMemory CreateBehaviorTreeMemory(MyBehaviorTree behaviorTree)
		{
			MyPerTreeBotMemory myPerTreeBotMemory = new MyPerTreeBotMemory();
			ResetMemoryInternal(behaviorTree, myPerTreeBotMemory);
			return myPerTreeBotMemory;
		}

		public bool ValidateMemoryForBehavior(MyBehaviorTree behaviorTree)
		{
			bool result = true;
			if (CurrentTreeBotMemory.NodesMemoryCount != behaviorTree.TotalNodeCount)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < CurrentTreeBotMemory.NodesMemoryCount; i++)
				{
					if (CurrentTreeBotMemory.GetNodeMemoryByIndex(i).GetType() != behaviorTree.GetNodeByIndex(i).MemoryType)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		public void PreTickClear()
		{
			if (HasPathToSave)
			{
				PrepareForNewNodePath();
			}
			CurrentTreeBotMemory.ClearNodesData();
			TickCounter++;
		}

		public void ClearPathMemory(bool postTick)
		{
			if (postTick)
			{
				PostTickPaths();
			}
			m_newNodePath.Clear();
			m_oldNodePath.Clear();
			LastRunningNodeIndex = -1;
		}

		public void ResetMemory(bool clearMemory = false)
		{
			if (m_behaviorTree != null)
			{
				if (clearMemory)
				{
					ClearPathMemory(postTick: true);
				}
				CurrentTreeBotMemory.Clear();
				ResetMemoryInternal(m_behaviorTree, CurrentTreeBotMemory);
			}
		}

		public void UnassignCurrentBehaviorTree()
		{
			ClearPathMemory(postTick: true);
			CurrentTreeBotMemory = null;
			m_behaviorTree = null;
		}

		private void ResetMemoryInternal(MyBehaviorTree behaviorTree, MyPerTreeBotMemory treeMemory)
		{
			for (int i = 0; i < behaviorTree.TotalNodeCount; i++)
			{
				treeMemory.AddNodeMemory(behaviorTree.GetNodeByIndex(i).GetNewMemoryObject());
			}
		}

		private void ClearOldPath()
		{
			m_oldNodePath.Clear();
			LastRunningNodeIndex = -1;
		}

		private void PostTickPaths()
		{
			if (m_behaviorTree != null)
			{
				m_behaviorTree.CallPostTickOnPath(m_memoryUser, CurrentTreeBotMemory, m_oldNodePath);
				m_behaviorTree.CallPostTickOnPath(m_memoryUser, CurrentTreeBotMemory, m_newNodePath);
			}
		}

		private void PostTickOldPath()
		{
			if (HasOldPath)
			{
				m_oldNodePath.ExceptWith(m_newNodePath);
				m_behaviorTree.CallPostTickOnPath(m_memoryUser, CurrentTreeBotMemory, m_oldNodePath);
				ClearOldPath();
			}
		}

		public void RememberNode(int nodeIndex)
		{
			m_newNodePath.Push(nodeIndex);
		}

		public void ForgetNode()
		{
			m_newNodePath.Pop();
		}

		public void PrepareForNewNodePath()
		{
			m_oldNodePath.Clear();
			m_oldNodePath.UnionWith(m_newNodePath);
			LastRunningNodeIndex = m_newNodePath.Peek();
			m_newNodePath.Clear();
		}

		public void ProcessLastRunningNode(MyBehaviorTreeNode node)
		{
			if (LastRunningNodeIndex != -1)
			{
				if (LastRunningNodeIndex != node.MemoryIndex)
				{
					PostTickOldPath();
				}
				else
				{
					ClearOldPath();
				}
			}
		}
	}
}
