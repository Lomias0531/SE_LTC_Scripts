using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GridCreateToolDefinition), null)]
	public class MyGridCreateToolDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyGridCreateToolDefinition_003C_003EActor : IActivator, IActivator<MyGridCreateToolDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGridCreateToolDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGridCreateToolDefinition CreateInstance()
			{
				return new MyGridCreateToolDefinition();
			}

			MyGridCreateToolDefinition IActivator<MyGridCreateToolDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}
	}
}
