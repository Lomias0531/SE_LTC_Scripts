using System.Reflection;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.World
{
	public class MyEnvironmentalParticleLogicFactory
	{
		private static MyObjectFactory<MyEnvironmentalParticleLogicTypeAttribute, MyEnvironmentalParticleLogic> m_objectFactory;

		static MyEnvironmentalParticleLogicFactory()
		{
			m_objectFactory = new MyObjectFactory<MyEnvironmentalParticleLogicTypeAttribute, MyEnvironmentalParticleLogic>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyEnvironmentalParticleLogic)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyEnvironmentalParticleLogic CreateEnvironmentalParticleLogic(MyObjectBuilder_EnvironmentalParticleLogic builder)
		{
			return m_objectFactory.CreateInstance(builder.TypeId);
		}
	}
}
