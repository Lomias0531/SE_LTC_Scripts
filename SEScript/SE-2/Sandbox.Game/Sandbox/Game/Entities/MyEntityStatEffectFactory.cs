using System;
using System.Reflection;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	internal static class MyEntityStatEffectFactory
	{
		private static MyObjectFactory<MyEntityStatEffectTypeAttribute, MyEntityStatRegenEffect> m_objectFactory;

		static MyEntityStatEffectFactory()
		{
			m_objectFactory = new MyObjectFactory<MyEntityStatEffectTypeAttribute, MyEntityStatRegenEffect>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyEntityStatRegenEffect)));
		}

		public static MyEntityStatRegenEffect CreateInstance(MyObjectBuilder_EntityStatRegenEffect builder)
		{
			return m_objectFactory.CreateInstance(builder.TypeId);
		}

		public static MyObjectBuilder_EntityStatRegenEffect CreateObjectBuilder(MyEntityStatRegenEffect effect)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_EntityStatRegenEffect>(effect);
		}

		public static Type GetProducedType(MyObjectBuilderType objectBuilderType)
		{
			return m_objectFactory.GetProducedType(objectBuilderType);
		}
	}
}
