using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractConditionModelFactory
	{
		private static MyObjectFactory<MyContractConditionModelDescriptor, MyContractConditionModel> m_objectFactory;

		static MyContractConditionModelFactory()
		{
			m_objectFactory = new MyObjectFactory<MyContractConditionModelDescriptor, MyContractConditionModel>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyContractConditionModel)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyContractConditionModel CreateInstance(MyObjectBuilder_ContractCondition data)
		{
			MyContractConditionModel myContractConditionModel = m_objectFactory.CreateInstance(data.TypeId);
			myContractConditionModel.Init(data);
			return myContractConditionModel;
		}
	}
}
