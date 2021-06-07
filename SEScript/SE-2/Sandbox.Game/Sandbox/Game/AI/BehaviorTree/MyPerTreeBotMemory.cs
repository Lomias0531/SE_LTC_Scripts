using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyPerTreeBotMemory
	{
		private List<MyBehaviorTreeNodeMemory> m_nodesMemory;

		private Dictionary<MyStringId, MyBBMemoryValue> m_blackboardMemory;

		public int NodesMemoryCount => m_nodesMemory.Count;

		public ListReader<MyBehaviorTreeNodeMemory> NodesMemory => new ListReader<MyBehaviorTreeNodeMemory>(m_nodesMemory);

		public IEnumerable<KeyValuePair<MyStringId, MyBBMemoryValue>> BBMemory => m_blackboardMemory;

		public MyPerTreeBotMemory()
		{
			m_nodesMemory = new List<MyBehaviorTreeNodeMemory>(20);
			m_blackboardMemory = new Dictionary<MyStringId, MyBBMemoryValue>(20, MyStringId.Comparer);
		}

		public void AddNodeMemory(MyBehaviorTreeNodeMemory nodeMemory)
		{
			m_nodesMemory.Add(nodeMemory);
		}

		public void AddBlackboardMemoryInstance(string name, MyBBMemoryValue obj)
		{
			MyStringId orCompute = MyStringId.GetOrCompute(name);
			m_blackboardMemory.Add(orCompute, obj);
		}

		public void RemoveBlackboardMemoryInstance(MyStringId name)
		{
			m_blackboardMemory.Remove(name);
		}

		public MyBehaviorTreeNodeMemory GetNodeMemoryByIndex(int index)
		{
			return m_nodesMemory[index];
		}

		public void ClearNodesData()
		{
			foreach (MyBehaviorTreeNodeMemory item in m_nodesMemory)
			{
				item.ClearNodeState();
			}
		}

		public void Clear()
		{
			m_nodesMemory.Clear();
			m_blackboardMemory.Clear();
		}

		public bool TryGetFromBlackboard<T>(MyStringId id, out T value) where T : MyBBMemoryValue
		{
			MyBBMemoryValue value2 = null;
			bool result = m_blackboardMemory.TryGetValue(id, out value2);
			value = (value2 as T);
			return result;
		}

		public void SaveToBlackboard(MyStringId id, MyBBMemoryValue value)
		{
			if (id != MyStringId.NullOrEmpty)
			{
				m_blackboardMemory[id] = value;
			}
		}

		public MyBBMemoryValue TrySaveToBlackboard(MyStringId id, Type type)
		{
			if (!type.IsSubclassOf(typeof(MyBBMemoryValue)) && !(type == typeof(MyBBMemoryValue)))
			{
				return null;
			}
			if (type.GetConstructor(Type.EmptyTypes) == null)
			{
				return null;
			}
			MyBBMemoryValue myBBMemoryValue = Activator.CreateInstance(type) as MyBBMemoryValue;
			m_blackboardMemory[id] = myBBMemoryValue;
			return myBBMemoryValue;
		}
	}
}
