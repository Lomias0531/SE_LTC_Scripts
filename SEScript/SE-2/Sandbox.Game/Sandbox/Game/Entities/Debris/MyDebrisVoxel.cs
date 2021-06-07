using Havok;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Debris
{
	internal class MyDebrisVoxel : MyDebrisBase
	{
		internal class MyDebrisVoxelPhysics : MyDebrisPhysics
		{
			private class Sandbox_Game_Entities_Debris_MyDebrisVoxel_003C_003EMyDebrisVoxelPhysics_003C_003EActor
			{
			}

			private const float VoxelDensity = 260f;

			public MyDebrisVoxelPhysics(IMyEntity entity, RigidBodyFlag rigidBodyFlag)
				: base(entity, rigidBodyFlag)
			{
			}

			public override void CreatePhysicsShape(out HkShape shape, out HkMassProperties massProperties, float mass)
			{
				HkSphereShape shape2 = new HkSphereShape(0.5f * ((MyEntity)base.Entity).Render.GetModel().BoundingSphere.Radius * base.Entity.PositionComp.Scale.Value);
				shape = shape2;
				massProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(shape2.Radius * 0.5f, mass);
			}

			public override void ScalePhysicsShape(ref HkMassProperties massProperties)
			{
				HkSphereShape shape = (HkSphereShape)RigidBody.GetShape();
				shape.Radius = ((MyEntity)base.Entity).Render.GetModel().BoundingSphere.Radius * base.Entity.PositionComp.Scale.Value;
				float mass = SphereMass(shape.Radius, 260f);
				massProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(shape.Radius, mass);
				RigidBody.SetShape(shape);
				RigidBody.SetMassProperties(ref massProperties);
				RigidBody.UpdateShape();
			}

			private float SphereMass(float radius, float density)
			{
				return radius * radius * radius * 3.141593f * 4f * 0.333f * density;
			}
		}

		internal class MyDebrisVoxelLogic : MyDebrisBaseLogic
		{
			private class Sandbox_Game_Entities_Debris_MyDebrisVoxel_003C_003EMyDebrisVoxelLogic_003C_003EActor : IActivator, IActivator<MyDebrisVoxelLogic>
			{
				private sealed override object CreateInstance()
				{
					return new MyDebrisVoxelLogic();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyDebrisVoxelLogic CreateInstance()
				{
					return new MyDebrisVoxelLogic();
				}

				MyDebrisVoxelLogic IActivator<MyDebrisVoxelLogic>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			protected override MyPhysicsComponentBase GetPhysics(RigidBodyFlag rigidBodyFlag)
			{
				return new MyDebrisVoxelPhysics(base.Container.Entity, rigidBodyFlag);
			}

			public override void Start(Vector3D position, Vector3D initialVelocity)
			{
				Start(position, initialVelocity, MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition());
			}

			public void Start(Vector3D position, Vector3D initialVelocity, MyVoxelMaterialDefinition mat)
			{
				MyRenderComponentDebrisVoxel obj = base.Container.Entity.Render as MyRenderComponentDebrisVoxel;
				obj.TexCoordOffset = MyUtils.GetRandomFloat(5f, 15f);
				obj.TexCoordScale = MyUtils.GetRandomFloat(8f, 12f);
				obj.VoxelMaterialIndex = mat.Index;
				base.Start(position, initialVelocity);
				base.Container.Entity.Render.NeedsResolveCastShadow = true;
				base.Container.Entity.Render.FastCastShadowResolve = true;
			}
		}

		private class Sandbox_Game_Entities_Debris_MyDebrisVoxel_003C_003EActor : IActivator, IActivator<MyDebrisVoxel>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebrisVoxel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebrisVoxel CreateInstance()
			{
				return new MyDebrisVoxel();
			}

			MyDebrisVoxel IActivator<MyDebrisVoxel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void InitComponents()
		{
			base.GameLogic = new MyDebrisVoxelLogic();
			base.Render = new MyRenderComponentDebrisVoxel();
			base.InitComponents();
		}
	}
}
