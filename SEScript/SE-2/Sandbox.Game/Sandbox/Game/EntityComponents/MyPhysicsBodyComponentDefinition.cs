using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_PhysicsBodyComponentDefinition), null)]
	public class MyPhysicsBodyComponentDefinition : MyPhysicsComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyPhysicsBodyComponentDefinition_003C_003EActor : IActivator, IActivator<MyPhysicsBodyComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicsBodyComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicsBodyComponentDefinition CreateInstance()
			{
				return new MyPhysicsBodyComponentDefinition();
			}

			MyPhysicsBodyComponentDefinition IActivator<MyPhysicsBodyComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool CreateFromCollisionObject;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PhysicsBodyComponentDefinition myObjectBuilder_PhysicsBodyComponentDefinition = builder as MyObjectBuilder_PhysicsBodyComponentDefinition;
			CreateFromCollisionObject = myObjectBuilder_PhysicsBodyComponentDefinition.CreateFromCollisionObject;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_PhysicsBodyComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_PhysicsBodyComponentDefinition;
			obj.CreateFromCollisionObject = CreateFromCollisionObject;
			return obj;
		}
	}
}
