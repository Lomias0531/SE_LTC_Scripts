using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Debris
{
	internal class MyDebrisTree : MyDebrisBase
	{
		protected class MyDebrisTreeLogic : MyDebrisBaseLogic
		{
			private class Sandbox_Game_Entities_Debris_MyDebrisTree_003C_003EMyDebrisTreeLogic_003C_003EActor : IActivator, IActivator<MyDebrisTreeLogic>
			{
				private sealed override object CreateInstance()
				{
					return new MyDebrisTreeLogic();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyDebrisTreeLogic CreateInstance()
				{
					return new MyDebrisTreeLogic();
				}

				MyDebrisTreeLogic IActivator<MyDebrisTreeLogic>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private const float AVERAGE_WOOD_DENSITY = 800f;

			protected override MyPhysicsComponentBase GetPhysics(RigidBodyFlag rigidBodyFlag)
			{
				return new MyDebrisTreePhysics(base.Entity, rigidBodyFlag);
			}

			protected override float CalculateMass(float radius)
			{
				MyDebrisTreePhysics.ComputeShapeDimensions(base.Entity.Model.BoundingBox, out float height, out float radius2);
				return 3.141593f * (radius2 * radius2) * (1.33333337f * radius2 + height) * 800f;
			}

			public override void Init(MyDebrisBaseDescription desc)
			{
				base.Init(desc);
				HkRigidBody rigidBody = base.Entity.Physics.RigidBody;
				rigidBody.EnableDeactivation = false;
				rigidBody.MaxAngularVelocity = 2f;
			}
		}

		protected class MyDebrisTreePhysics : MyDebrisPhysics
		{
			private class Sandbox_Game_Entities_Debris_MyDebrisTree_003C_003EMyDebrisTreePhysics_003C_003EActor
			{
			}

			public MyDebrisTreePhysics(IMyEntity entity, RigidBodyFlag rigidBodyFlags)
				: base(entity, rigidBodyFlags)
			{
			}

			public override void CreatePhysicsShape(out HkShape shape, out HkMassProperties massProperties, float mass)
			{
				shape = HkShape.Empty;
				MyModel model = base.Entity.Render.GetModel();
				BoundingBox boundingBox = model.BoundingBox;
				ComputeShapeDimensions(boundingBox, out float height, out float radius);
				Vector3 value = Vector3.Up * height;
				Vector3 value2 = (boundingBox.Min + boundingBox.Max) * 0.5f;
				Vector3 vector = value2 + value * 0.2f;
				Vector3 vector2 = value2 - value * 0.45f;
				bool flag = true;
				if (MyFakes.TREE_MESH_FROM_MODEL)
				{
					HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
					if (havokCollisionShapes != null && havokCollisionShapes.Length != 0)
					{
						if (havokCollisionShapes.Length == 1)
						{
							shape = havokCollisionShapes[0];
							shape.AddReference();
						}
						else
						{
							shape = new HkListShape(havokCollisionShapes, HkReferencePolicy.None);
						}
						flag = false;
					}
				}
				if (flag)
				{
					shape = new HkCapsuleShape(vector, vector2, radius);
				}
				massProperties = HkInertiaTensorComputer.ComputeCapsuleVolumeMassProperties(vector, vector2, radius, mass);
			}

			public static void ComputeShapeDimensions(BoundingBox bBox, out float height, out float radius)
			{
				height = bBox.Height;
				radius = height / 20f;
				height -= 2f * radius;
			}
		}

		private class Sandbox_Game_Entities_Debris_MyDebrisTree_003C_003EActor : IActivator, IActivator<MyDebrisTree>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebrisTree();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebrisTree CreateInstance()
			{
				return new MyDebrisTree();
			}

			MyDebrisTree IActivator<MyDebrisTree>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDebrisTree()
		{
			base.GameLogic = new MyDebrisTreeLogic();
			if (MyDebugDrawSettings.DEBUG_DRAW_TREE_COLLISION_SHAPES)
			{
				base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_TREE_COLLISION_SHAPES)
			{
				return;
			}
			MyPhysicsComponentBase physics = base.Physics;
			if (physics != null)
			{
				HkRigidBody rigidBody = physics.RigidBody;
				if (!(rigidBody == null))
				{
					int shapeIndex = 0;
					MyPhysicsDebugDraw.DrawCollisionShape(rigidBody.GetShape(), physics.GetWorldMatrix(), 1f, ref shapeIndex, physics.IsActive ? "A" : "I");
				}
			}
		}
	}
}
