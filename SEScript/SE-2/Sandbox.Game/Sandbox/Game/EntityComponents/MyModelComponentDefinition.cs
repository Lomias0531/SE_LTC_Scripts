using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_ModelComponentDefinition), null)]
	public class MyModelComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyModelComponentDefinition_003C_003EActor : IActivator, IActivator<MyModelComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyModelComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyModelComponentDefinition CreateInstance()
			{
				return new MyModelComponentDefinition();
			}

			MyModelComponentDefinition IActivator<MyModelComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector3 Size;

		public float Mass;

		public float Volume;

		public string Model;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ModelComponentDefinition myObjectBuilder_ModelComponentDefinition = builder as MyObjectBuilder_ModelComponentDefinition;
			Size = myObjectBuilder_ModelComponentDefinition.Size;
			Mass = myObjectBuilder_ModelComponentDefinition.Mass;
			Model = myObjectBuilder_ModelComponentDefinition.Model;
			Volume = (myObjectBuilder_ModelComponentDefinition.Volume.HasValue ? (myObjectBuilder_ModelComponentDefinition.Volume.Value / 1000f) : myObjectBuilder_ModelComponentDefinition.Size.Volume);
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_ModelComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_ModelComponentDefinition;
			obj.Size = Size;
			obj.Mass = Mass;
			obj.Model = Model;
			obj.Volume = Volume * 1000f;
			return obj;
		}
	}
}
