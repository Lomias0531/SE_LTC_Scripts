using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public class MyItemTypeDefinition
	{
		public struct Module
		{
			public Type Type;

			public MyDefinitionId Definition;
		}

		public string Name;

		public int LodFrom;

		public int LodTo;

		public Module StorageModule;

		public Module[] ProxyModules;

		public MyItemTypeDefinition(MyEnvironmentItemTypeDefinition def)
		{
			Name = def.Name;
			LodFrom = ((def.LodFrom == -1) ? 15 : def.LodFrom);
			LodTo = def.LodTo;
			if (def.Provider.HasValue)
			{
				MyProceduralEnvironmentModuleDefinition definition = MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>(def.Provider.Value);
				if (definition == null)
				{
					MyLog.Default.Error("Could not find module definition for type {0}.", def.Provider.Value);
				}
				else
				{
					StorageModule.Type = definition.ModuleType;
					StorageModule.Definition = def.Provider.Value;
				}
			}
			if (def.Proxies == null)
			{
				return;
			}
			List<Module> list = new List<Module>();
			SerializableDefinitionId[] proxies = def.Proxies;
			foreach (SerializableDefinitionId serializableDefinitionId in proxies)
			{
				MyEnvironmentModuleProxyDefinition definition2 = MyDefinitionManager.Static.GetDefinition<MyEnvironmentModuleProxyDefinition>(serializableDefinitionId);
				if (definition2 == null)
				{
					MyLog.Default.Error("Could not find proxy module definition for type {0}.", serializableDefinitionId);
				}
				else
				{
					list.Add(new Module
					{
						Type = definition2.ModuleType,
						Definition = serializableDefinitionId
					});
				}
			}
			list.Capacity = list.Count;
			ProxyModules = list.ToArray();
		}
	}
}
