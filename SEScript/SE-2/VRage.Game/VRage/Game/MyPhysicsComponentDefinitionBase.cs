using System.Xml.Serialization;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_PhysicsComponentDefinitionBase), null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyPhysicsComponentDefinitionBase : MyComponentDefinitionBase
	{
		private class VRage_Game_MyPhysicsComponentDefinitionBase_003C_003EActor : IActivator, IActivator<MyPhysicsComponentDefinitionBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicsComponentDefinitionBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicsComponentDefinitionBase CreateInstance()
			{
				return new MyPhysicsComponentDefinitionBase();
			}

			MyPhysicsComponentDefinitionBase IActivator<MyPhysicsComponentDefinitionBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType MassPropertiesComputation;

		public RigidBodyFlag RigidBodyFlags;

		public string CollisionLayer;

		public float? LinearDamping;

		public float? AngularDamping;

		public bool ForceActivate;

		public MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags UpdateFlags;

		public bool Serialize;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PhysicsComponentDefinitionBase myObjectBuilder_PhysicsComponentDefinitionBase = builder as MyObjectBuilder_PhysicsComponentDefinitionBase;
			MassPropertiesComputation = myObjectBuilder_PhysicsComponentDefinitionBase.MassPropertiesComputation;
			RigidBodyFlags = myObjectBuilder_PhysicsComponentDefinitionBase.RigidBodyFlags;
			CollisionLayer = myObjectBuilder_PhysicsComponentDefinitionBase.CollisionLayer;
			LinearDamping = myObjectBuilder_PhysicsComponentDefinitionBase.LinearDamping;
			AngularDamping = myObjectBuilder_PhysicsComponentDefinitionBase.AngularDamping;
			ForceActivate = myObjectBuilder_PhysicsComponentDefinitionBase.ForceActivate;
			UpdateFlags = myObjectBuilder_PhysicsComponentDefinitionBase.UpdateFlags;
			Serialize = myObjectBuilder_PhysicsComponentDefinitionBase.Serialize;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_PhysicsComponentDefinitionBase obj = base.GetObjectBuilder() as MyObjectBuilder_PhysicsComponentDefinitionBase;
			obj.MassPropertiesComputation = MassPropertiesComputation;
			obj.RigidBodyFlags = RigidBodyFlags;
			obj.CollisionLayer = CollisionLayer;
			obj.LinearDamping = LinearDamping;
			obj.AngularDamping = AngularDamping;
			obj.ForceActivate = ForceActivate;
			obj.UpdateFlags = UpdateFlags;
			obj.Serialize = Serialize;
			return obj;
		}
	}
}
