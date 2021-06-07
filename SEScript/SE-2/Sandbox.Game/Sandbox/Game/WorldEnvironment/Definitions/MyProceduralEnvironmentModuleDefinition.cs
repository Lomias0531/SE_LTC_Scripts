using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilder;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProceduralEnvironmentModuleDefinition), null)]
	public class MyProceduralEnvironmentModuleDefinition : MyDefinitionBase
	{
		private class Sandbox_Game_WorldEnvironment_Definitions_MyProceduralEnvironmentModuleDefinition_003C_003EActor : IActivator, IActivator<MyProceduralEnvironmentModuleDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProceduralEnvironmentModuleDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProceduralEnvironmentModuleDefinition CreateInstance()
			{
				return new MyProceduralEnvironmentModuleDefinition();
			}

			MyProceduralEnvironmentModuleDefinition IActivator<MyProceduralEnvironmentModuleDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Type ModuleType;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProceduralEnvironmentModuleDefinition myObjectBuilder_ProceduralEnvironmentModuleDefinition = (MyObjectBuilder_ProceduralEnvironmentModuleDefinition)builder;
			ModuleType = MyGlobalTypeMetadata.Static.GetType(myObjectBuilder_ProceduralEnvironmentModuleDefinition.QualifiedTypeName, throwOnError: false);
			if (ModuleType == null)
			{
				MyLog.Default.Error("Could not find module type {0}!", myObjectBuilder_ProceduralEnvironmentModuleDefinition.QualifiedTypeName);
				throw new ArgumentException("Could not find module type;");
			}
		}
	}
}
