using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Components.Session
{
	[MyDefinitionType(typeof(MyObjectBuilder_SessionComponentDefinition), null)]
	public class MySessionComponentDefinition : MyDefinitionBase
	{
		private class VRage_Game_Components_Session_MySessionComponentDefinition_003C_003EActor : IActivator, IActivator<MySessionComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySessionComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySessionComponentDefinition CreateInstance()
			{
				return new MySessionComponentDefinition();
			}

			MySessionComponentDefinition IActivator<MySessionComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
