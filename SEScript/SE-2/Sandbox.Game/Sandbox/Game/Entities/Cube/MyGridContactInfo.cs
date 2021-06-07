using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public struct MyGridContactInfo
	{
		[Flags]
		public enum ContactFlags
		{
			Known = 0x1,
			Deformation = 0x8,
			Particles = 0x10,
			RubberDeformation = 0x20,
			PredictedCollision = 0x40,
			PredictedCollision_Disabled = 0x80
		}

		public HkContactPointEvent Event;

		public readonly Vector3D ContactPosition;

		public MyCubeGrid m_currentEntity;

		public MyEntity m_collidingEntity;

		private MySlimBlock m_currentBlock;

		private MySlimBlock m_otherBlock;

		private bool m_voxelSurfaceMaterialInitialized;

		private MyVoxelMaterialDefinition m_voxelSurfaceMaterial;

		public float ImpulseMultiplier;

		public MyCubeGrid CurrentEntity => m_currentEntity;

		public MyEntity CollidingEntity => m_collidingEntity;

		public MySlimBlock OtherBlock => m_otherBlock;

		public MyVoxelMaterialDefinition VoxelSurfaceMaterial
		{
			get
			{
				if (!m_voxelSurfaceMaterialInitialized)
				{
					ReadVoxelSurfaceMaterial();
					m_voxelSurfaceMaterialInitialized = true;
				}
				return m_voxelSurfaceMaterial;
			}
		}

		private ContactFlags Flags
		{
			get
			{
				return (ContactFlags)Event.ContactProperties.UserData.AsUint;
			}
			set
			{
				HkContactPointProperties contactProperties = Event.ContactProperties;
				contactProperties.UserData = HkContactUserData.UInt((uint)value);
			}
		}

		public bool EnableDeformation
		{
			get
			{
				return (Flags & ContactFlags.Deformation) != 0;
			}
			set
			{
				SetFlag(ContactFlags.Deformation, value);
			}
		}

		public bool RubberDeformation
		{
			get
			{
				return (Flags & ContactFlags.RubberDeformation) != 0;
			}
			set
			{
				SetFlag(ContactFlags.RubberDeformation, value);
			}
		}

		public bool EnableParticles
		{
			get
			{
				return (Flags & ContactFlags.Particles) != 0;
			}
			set
			{
				SetFlag(ContactFlags.Particles, value);
			}
		}

		public bool IsKnown => (Flags & ContactFlags.Known) != 0;

		public MyGridContactInfo(ref HkContactPointEvent evnt, MyCubeGrid grid)
			: this(ref evnt, grid, evnt.GetOtherEntity(grid) as MyEntity)
		{
		}

		public MyGridContactInfo(ref HkContactPointEvent evnt, MyCubeGrid grid, MyEntity collidingEntity)
		{
			Event = evnt;
			ContactPosition = grid.Physics.ClusterToWorld(evnt.ContactPoint.Position);
			m_currentEntity = grid;
			m_collidingEntity = collidingEntity;
			m_currentBlock = null;
			m_otherBlock = null;
			ImpulseMultiplier = 1f;
			m_voxelSurfaceMaterial = null;
			m_voxelSurfaceMaterialInitialized = false;
		}

		public void HandleEvents()
		{
			if ((Flags & ContactFlags.Known) == 0)
			{
				Flags |= (ContactFlags.Known | ContactFlags.Deformation | ContactFlags.Particles);
				m_currentBlock = GetContactBlock(CurrentEntity, ContactPosition, Event.ContactPoint.NormalAndDistance.W);
				MyCubeGrid myCubeGrid = CollidingEntity as MyCubeGrid;
				if (myCubeGrid != null)
				{
					m_otherBlock = GetContactBlock(myCubeGrid, ContactPosition, Event.ContactPoint.NormalAndDistance.W);
				}
				if (m_currentBlock != null && m_currentBlock.FatBlock != null)
				{
					m_currentBlock.FatBlock.ContactPointCallback(ref this);
					ImpulseMultiplier *= m_currentBlock.BlockDefinition.PhysicalMaterial.CollisionMultiplier;
				}
				if (m_otherBlock != null && m_otherBlock.FatBlock != null)
				{
					SwapEntities();
					Event.ContactPoint.Flip();
					ImpulseMultiplier *= m_currentBlock.BlockDefinition.PhysicalMaterial.CollisionMultiplier;
					m_currentBlock.FatBlock.ContactPointCallback(ref this);
					SwapEntities();
					Event.ContactPoint.Flip();
				}
			}
		}

		private void SetFlag(ContactFlags flag, bool value)
		{
			Flags = (value ? (Flags | flag) : (Flags & ~flag));
		}

		private void SwapEntities()
		{
			MyCubeGrid currentEntity = m_currentEntity;
			m_currentEntity = (MyCubeGrid)m_collidingEntity;
			m_collidingEntity = currentEntity;
			MySlimBlock currentBlock = m_currentBlock;
			m_currentBlock = m_otherBlock;
			m_otherBlock = currentBlock;
		}

		private static MySlimBlock GetContactBlock(MyCubeGrid grid, Vector3D worldPosition, float graceDistance)
		{
			HashSet<MySlimBlock> cubeBlocks = grid.CubeBlocks;
			if (cubeBlocks.Count == 1)
			{
				return cubeBlocks.FirstElement();
			}
			MatrixD matrix = grid.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref worldPosition, ref matrix, out Vector3D result);
			MySlimBlock result2 = null;
			float num = float.MaxValue;
			if (cubeBlocks.Count < 10)
			{
				foreach (MySlimBlock item in cubeBlocks)
				{
					float num2 = (float)(item.Position * grid.GridSize - result).LengthSquared();
					if (num2 < num)
					{
						num = num2;
						result2 = item;
					}
				}
				return result2;
			}
			bool flag = false;
			Vector3 linearVelocity = grid.Physics.LinearVelocity;
			float num3 = MyGridPhysics.ShipMaxLinearVelocity();
			if (linearVelocity.LengthSquared() > num3 * num3 * 100f)
			{
				flag = true;
				linearVelocity /= linearVelocity.Length() * num3;
			}
			graceDistance = Math.Max(Math.Abs(graceDistance), grid.GridSize * 0.2f);
			graceDistance += 1f;
			Vector3D value = Vector3D.TransformNormal(linearVelocity * 0.0166666675f, matrix);
			Vector3I value2 = Vector3I.Round((result - graceDistance - value) / grid.GridSize);
			Vector3I value3 = Vector3I.Round((result + graceDistance + value) / grid.GridSize);
			Vector3I value4 = Vector3I.Round((result + graceDistance - value) / grid.GridSize);
			Vector3I value5 = Vector3I.Round((result - graceDistance + value) / grid.GridSize);
			Vector3I vector3I = Vector3I.Min(Vector3I.Min(Vector3I.Min(value2, value3), value4), value5);
			Vector3I vector3I2 = Vector3I.Max(Vector3I.Max(Vector3I.Max(value2, value3), value4), value5);
			Vector3I vector3I3 = default(Vector3I);
			vector3I3.X = vector3I.X;
			while (vector3I3.X <= vector3I2.X)
			{
				vector3I3.Y = vector3I.Y;
				while (vector3I3.Y <= vector3I2.Y)
				{
					vector3I3.Z = vector3I.Z;
					while (vector3I3.Z <= vector3I2.Z)
					{
						MySlimBlock cubeBlock = grid.GetCubeBlock(vector3I3);
						if (cubeBlock != null)
						{
							float num4 = (float)(vector3I3 * grid.GridSize - result).LengthSquared();
							if (num4 < num)
							{
								num = num4;
								result2 = cubeBlock;
								if (flag)
								{
									return result2;
								}
							}
						}
						vector3I3.Z++;
					}
					vector3I3.Y++;
				}
				vector3I3.X++;
			}
			return result2;
		}

		private void ReadVoxelSurfaceMaterial()
		{
			MyVoxelPhysicsBody myVoxelPhysicsBody = m_collidingEntity.Physics as MyVoxelPhysicsBody;
			if (myVoxelPhysicsBody != null)
			{
				int bodyIndex = (Event.GetPhysicsBody(0) != myVoxelPhysicsBody) ? 1 : 0;
				uint hitTriangleMaterial = myVoxelPhysicsBody.GetHitTriangleMaterial(ref Event, bodyIndex);
				if (hitTriangleMaterial != uint.MaxValue)
				{
					m_voxelSurfaceMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte)hitTriangleMaterial);
				}
			}
		}
	}
}
