using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Contracts
{
	public static class MyContractConditionFactory
	{
		private static MyObjectFactory<MyContractConditionDescriptor, MyContractCondition> m_objectFactory;

		static MyContractConditionFactory()
		{
			m_objectFactory = new MyObjectFactory<MyContractConditionDescriptor, MyContractCondition>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyContractCondition)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyContractCondition CreateInstance(MyObjectBuilder_ContractCondition data)
		{
			MyContractCondition myContractCondition = m_objectFactory.CreateInstance(data.TypeId);
			myContractCondition.Init(data);
			return myContractCondition;
		}

		public static MyObjectBuilder_ContractCondition CreateObjectBuilder(MyContractCondition cont)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_ContractCondition>(cont);
		}
	}
}
