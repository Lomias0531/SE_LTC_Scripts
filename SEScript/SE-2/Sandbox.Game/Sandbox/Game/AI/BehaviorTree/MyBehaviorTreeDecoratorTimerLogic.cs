using System.Diagnostics;
using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyBehaviorTreeDecoratorTimerLogic : IMyDecoratorLogic
	{
		public long TimeInMs
		{
			get;
			private set;
		}

		public MyBehaviorTreeDecoratorTimerLogic()
		{
			TimeInMs = 0L;
		}

		public void Construct(MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData)
		{
			MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic timerLogic = logicData as MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic;
			TimeInMs = timerLogic.TimeInMs;
		}

		public void Update(MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
		{
			MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory timerLogicMemory = logicMemory as MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory;
			if ((Stopwatch.GetTimestamp() - timerLogicMemory.CurrentTime) / Stopwatch.Frequency * 1000 > TimeInMs)
			{
				timerLogicMemory.CurrentTime = Stopwatch.GetTimestamp();
				timerLogicMemory.TimeLimitReached = true;
			}
			else
			{
				timerLogicMemory.TimeLimitReached = false;
			}
		}

		public bool CanRun(MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
		{
			return (logicMemory as MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory).TimeLimitReached;
		}

		public MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetNewMemoryObject()
		{
			return new MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory();
		}

		public override int GetHashCode()
		{
			return ((int)TimeInMs).GetHashCode();
		}

		public override string ToString()
		{
			return "Timer";
		}
	}
}
