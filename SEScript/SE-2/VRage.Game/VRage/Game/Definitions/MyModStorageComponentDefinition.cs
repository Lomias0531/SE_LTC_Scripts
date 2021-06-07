using System;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace VRage.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ModStorageComponentDefinition), null)]
	public class MyModStorageComponentDefinition : MyComponentDefinitionBase
	{
		private class VRage_Game_Definitions_MyModStorageComponentDefinition_003C_003EActor : IActivator, IActivator<MyModStorageComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyModStorageComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyModStorageComponentDefinition CreateInstance()
			{
				return new MyModStorageComponentDefinition();
			}

			MyModStorageComponentDefinition IActivator<MyModStorageComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Guid[] RegisteredStorageGuids;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ModStorageComponentDefinition myObjectBuilder_ModStorageComponentDefinition = builder as MyObjectBuilder_ModStorageComponentDefinition;
			RegisteredStorageGuids = myObjectBuilder_ModStorageComponentDefinition.RegisteredStorageGuids;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_ModStorageComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_ModStorageComponentDefinition;
			obj.RegisteredStorageGuids = RegisteredStorageGuids;
			return obj;
		}
	}
}
