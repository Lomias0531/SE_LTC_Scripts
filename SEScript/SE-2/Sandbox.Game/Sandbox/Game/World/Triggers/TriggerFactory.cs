using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Triggers
{
	public static class TriggerFactory
	{
		private static MyObjectFactory<TriggerTypeAttribute, MyTrigger> m_objectFactory;

		static TriggerFactory()
		{
			m_objectFactory = new MyObjectFactory<TriggerTypeAttribute, MyTrigger>();
			m_objectFactory.RegisterFromCreatedObjectAssembly();
		}

		public static MyTrigger CreateInstance(MyObjectBuilder_Trigger builder)
		{
			MyTrigger myTrigger = m_objectFactory.CreateInstance(builder.TypeId);
			myTrigger.Init(builder);
			return myTrigger;
		}

		public static MyObjectBuilder_Trigger CreateObjectBuilder(MyTrigger instance)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Trigger>(instance);
		}
	}
}
