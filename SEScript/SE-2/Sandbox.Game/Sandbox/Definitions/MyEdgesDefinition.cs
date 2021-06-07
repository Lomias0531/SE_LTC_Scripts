using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EdgesDefinition), null)]
	public class MyEdgesDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyEdgesDefinition_003C_003EActor : IActivator, IActivator<MyEdgesDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEdgesDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEdgesDefinition CreateInstance()
			{
				return new MyEdgesDefinition();
			}

			MyEdgesDefinition IActivator<MyEdgesDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyEdgesModelSet Large;

		public MyEdgesModelSet Small;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EdgesDefinition myObjectBuilder_EdgesDefinition = builder as MyObjectBuilder_EdgesDefinition;
			Large = myObjectBuilder_EdgesDefinition.Large;
			Small = myObjectBuilder_EdgesDefinition.Small;
		}
	}
}
