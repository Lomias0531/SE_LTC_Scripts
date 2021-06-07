using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractModelFactory
	{
		private static MyObjectFactory<MyContractModelDescriptor, MyContractModel> m_objectFactory;

		static MyContractModelFactory()
		{
			m_objectFactory = new MyObjectFactory<MyContractModelDescriptor, MyContractModel>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyContractModel)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyContractModel CreateInstance(MyObjectBuilder_Contract data, bool showFactionIcons = true)
		{
			MyContractModel myContractModel = m_objectFactory.CreateInstance(data.TypeId);
			myContractModel.Init(data, showFactionIcons);
			return myContractModel;
		}
	}
}
