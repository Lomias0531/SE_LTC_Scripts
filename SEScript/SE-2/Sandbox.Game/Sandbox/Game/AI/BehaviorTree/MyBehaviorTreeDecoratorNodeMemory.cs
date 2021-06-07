using System.Diagnostics;
using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeMemoryType(typeof(MyObjectBuilder_BehaviorTreeDecoratorNodeMemory))]
	public class MyBehaviorTreeDecoratorNodeMemory : MyBehaviorTreeNodeMemory
	{
		public abstract class LogicMemory
		{
			public abstract void ClearMemory();

			public abstract void Init(MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder logicMemoryBuilder);

			public abstract MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder GetObjectBuilder();

			public abstract void PostTickMemory();
		}

		public class TimerLogicMemory : LogicMemory
		{
			public bool TimeLimitReached
			{
				get;
				set;
			}

			public long CurrentTime
			{
				get;
				set;
			}

			public override void ClearMemory()
			{
				TimeLimitReached = true;
				CurrentTime = Stopwatch.GetTimestamp();
			}

			public override void Init(MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder logicMemoryBuilder)
			{
				MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder timerLogicMemoryBuilder = logicMemoryBuilder as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder;
				CurrentTime = Stopwatch.GetTimestamp() - timerLogicMemoryBuilder.CurrentTime;
				TimeLimitReached = timerLogicMemoryBuilder.TimeLimitReached;
			}

			public override MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder GetObjectBuilder()
			{
				return new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder
				{
					CurrentTime = Stopwatch.GetTimestamp() - CurrentTime,
					TimeLimitReached = TimeLimitReached
				};
			}

			public override void PostTickMemory()
			{
				TimeLimitReached = false;
				CurrentTime = Stopwatch.GetTimestamp();
			}
		}

		public class CounterLogicMemory : LogicMemory
		{
			public int CurrentCount
			{
				get;
				set;
			}

			public override void ClearMemory()
			{
				CurrentCount = 0;
			}

			public override void Init(MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder logicMemoryBuilder)
			{
				MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder counterLogicMemoryBuilder = logicMemoryBuilder as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder;
				CurrentCount = counterLogicMemoryBuilder.CurrentCount;
			}

			public override MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder GetObjectBuilder()
			{
				return new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder
				{
					CurrentCount = CurrentCount
				};
			}

			public override void PostTickMemory()
			{
				CurrentCount = 0;
			}
		}

		public MyBehaviorTreeState ChildState
		{
			get;
			set;
		}

		public LogicMemory DecoratorLogicMemory
		{
			get;
			set;
		}

		public override void Init(MyObjectBuilder_BehaviorTreeNodeMemory builder)
		{
			base.Init(builder);
			MyObjectBuilder_BehaviorTreeDecoratorNodeMemory myObjectBuilder_BehaviorTreeDecoratorNodeMemory = builder as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory;
			ChildState = myObjectBuilder_BehaviorTreeDecoratorNodeMemory.ChildState;
			DecoratorLogicMemory = GetLogicMemoryByBuilder(myObjectBuilder_BehaviorTreeDecoratorNodeMemory.Logic);
		}

		public override MyObjectBuilder_BehaviorTreeNodeMemory GetObjectBuilder()
		{
			MyObjectBuilder_BehaviorTreeDecoratorNodeMemory obj = base.GetObjectBuilder() as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory;
			obj.ChildState = ChildState;
			obj.Logic = DecoratorLogicMemory.GetObjectBuilder();
			return obj;
		}

		public override void ClearMemory()
		{
			base.ClearMemory();
			ChildState = MyBehaviorTreeState.NOT_TICKED;
			DecoratorLogicMemory.ClearMemory();
		}

		public override void PostTickMemory()
		{
			base.PostTickMemory();
			ChildState = MyBehaviorTreeState.NOT_TICKED;
			DecoratorLogicMemory.PostTickMemory();
		}

		private static LogicMemory GetLogicMemoryByBuilder(MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder builder)
		{
			if (builder is MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder)
			{
				return new TimerLogicMemory();
			}
			if (builder is MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder)
			{
				return new CounterLogicMemory();
			}
			return null;
		}
	}
}
