using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyBehaviorTreeDecoratorCounterLogic : IMyDecoratorLogic
	{
		public int CounterLimit
		{
			get;
			private set;
		}

		public MyBehaviorTreeDecoratorCounterLogic()
		{
			CounterLimit = 0;
		}

		public void Construct(MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData)
		{
			MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic counterLogic = logicData as MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic;
			CounterLimit = counterLogic.Count;
		}

		public void Update(MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
		{
			MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory counterLogicMemory = logicMemory as MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory;
			if (counterLogicMemory.CurrentCount == CounterLimit)
			{
				counterLogicMemory.CurrentCount = 0;
			}
			else
			{
				int num = ++counterLogicMemory.CurrentCount;
			}
		}

		public bool CanRun(MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
		{
			return (logicMemory as MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory).CurrentCount == CounterLimit;
		}

		public MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetNewMemoryObject()
		{
			return new MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory();
		}

		public override int GetHashCode()
		{
			return CounterLimit.GetHashCode();
		}

		public override string ToString()
		{
			return "Counter";
		}
	}
}
