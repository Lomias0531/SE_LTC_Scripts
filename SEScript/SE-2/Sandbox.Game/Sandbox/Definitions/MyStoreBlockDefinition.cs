using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_StoreBlockDefinition), null)]
	public class MyStoreBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyStoreBlockDefinition_003C_003EActor : IActivator, IActivator<MyStoreBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyStoreBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyStoreBlockDefinition CreateInstance()
			{
				return new MyStoreBlockDefinition();
			}

			MyStoreBlockDefinition IActivator<MyStoreBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_StoreBlockDefinition myObjectBuilder_StoreBlockDefinition = builder as MyObjectBuilder_StoreBlockDefinition;
			ScreenAreas = ((myObjectBuilder_StoreBlockDefinition.ScreenAreas != null) ? myObjectBuilder_StoreBlockDefinition.ScreenAreas.ToList() : null);
		}
	}
}
