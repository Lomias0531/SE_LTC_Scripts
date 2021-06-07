using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilder;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EnvironmentModuleProxyDefinition), null)]
	public class MyEnvironmentModuleProxyDefinition : MyDefinitionBase
	{
		private class Sandbox_Game_WorldEnvironment_Definitions_MyEnvironmentModuleProxyDefinition_003C_003EActor : IActivator, IActivator<MyEnvironmentModuleProxyDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEnvironmentModuleProxyDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEnvironmentModuleProxyDefinition CreateInstance()
			{
				return new MyEnvironmentModuleProxyDefinition();
			}

			MyEnvironmentModuleProxyDefinition IActivator<MyEnvironmentModuleProxyDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Type ModuleType;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EnvironmentModuleProxyDefinition myObjectBuilder_EnvironmentModuleProxyDefinition = (MyObjectBuilder_EnvironmentModuleProxyDefinition)builder;
			ModuleType = MyGlobalTypeMetadata.Static.GetType(myObjectBuilder_EnvironmentModuleProxyDefinition.QualifiedTypeName, throwOnError: false);
			if (ModuleType == null)
			{
				MyLog.Default.Error("Could not find module type {0}!", myObjectBuilder_EnvironmentModuleProxyDefinition.QualifiedTypeName);
				throw new ArgumentException("Could not find module type;");
			}
		}
	}
}
