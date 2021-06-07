using System;
using System.Reflection;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Entities.Cube
{
	internal static class MyCubeBlockFactory
	{
		private static MyObjectFactory<MyCubeBlockTypeAttribute, object> m_objectFactory;

		static MyCubeBlockFactory()
		{
			m_objectFactory = new MyObjectFactory<MyCubeBlockTypeAttribute, object>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyCubeBlock)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static object CreateCubeBlock(MyObjectBuilder_CubeBlock builder)
		{
			object obj = m_objectFactory.CreateInstance(builder.TypeId);
			MyEntity myEntity = obj as MyEntity;
			if (myEntity != null)
			{
				MyEntityFactory.AddScriptGameLogic(myEntity, builder.TypeId, builder.SubtypeName);
			}
			return obj;
		}

		public static MyObjectBuilder_CubeBlock CreateObjectBuilder(MyCubeBlock cubeBlock)
		{
			return (MyObjectBuilder_CubeBlock)MyObjectBuilderSerializer.CreateNewObject(cubeBlock.BlockDefinition.Id);
		}

		public static Type GetProducedType(MyObjectBuilderType objectBuilderType)
		{
			return m_objectFactory.GetProducedType(objectBuilderType);
		}
	}
}
