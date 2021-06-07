using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Contracts
{
	public static class MyContractFactory
	{
		private static MyObjectFactory<MyContractDescriptor, MyContract> m_objectFactory;

		static MyContractFactory()
		{
			m_objectFactory = new MyObjectFactory<MyContractDescriptor, MyContract>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyContract)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyContract CreateInstance(MyObjectBuilder_Contract data)
		{
			MyContract myContract = m_objectFactory.CreateInstance(data.TypeId);
			myContract.Init(data);
			return myContract;
		}

		public static MyObjectBuilder_Contract CreateObjectBuilder(MyContract cont)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Contract>(cont);
		}
	}
}
