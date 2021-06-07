using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeMemoryType(typeof(MyObjectBuilder_BehaviorTreeControlNodeMemory))]
	public class MyBehaviorTreeControlNodeMemory : MyBehaviorTreeNodeMemory
	{
		public int InitialIndex
		{
			get;
			set;
		}

		public MyBehaviorTreeControlNodeMemory()
		{
			InitialIndex = 0;
		}

		public override void Init(MyObjectBuilder_BehaviorTreeNodeMemory builder)
		{
			base.Init(builder);
			MyObjectBuilder_BehaviorTreeControlNodeMemory myObjectBuilder_BehaviorTreeControlNodeMemory = builder as MyObjectBuilder_BehaviorTreeControlNodeMemory;
			InitialIndex = myObjectBuilder_BehaviorTreeControlNodeMemory.InitialIndex;
		}

		public override MyObjectBuilder_BehaviorTreeNodeMemory GetObjectBuilder()
		{
			MyObjectBuilder_BehaviorTreeControlNodeMemory obj = base.GetObjectBuilder() as MyObjectBuilder_BehaviorTreeControlNodeMemory;
			obj.InitialIndex = InitialIndex;
			return obj;
		}

		public override void ClearMemory()
		{
			base.ClearMemory();
			InitialIndex = 0;
		}

		public override void PostTickMemory()
		{
			base.PostTickMemory();
			InitialIndex = 0;
		}
	}
}
