using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.WorldEnvironment;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	public class MyCasterComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyCasterComponent_003C_003EActor
		{
		}

		private MySlimBlock m_hitBlock;

		private MyCubeGrid m_hitCubeGrid;

		private MyCharacter m_hitCharacter;

		private IMyDestroyableObject m_hitDestroaybleObj;

		private MyFloatingObject m_hitFloatingObject;

		private MyEnvironmentSector m_hitEnvironmentSector;

		private int m_environmentItem;

		private Vector3D m_hitPosition;

		private double m_distanceToHitSq;

		private MyDrillSensorBase m_caster;

		private Vector3D m_pointOfReference;

		private bool m_isPointOfRefSet;

		public override string ComponentTypeDebugString => "MyBlockInfoComponent";

		public MySlimBlock HitBlock => m_hitBlock;

		public MyCubeGrid HitCubeGrid => m_hitCubeGrid;

		public Vector3D HitPosition => m_hitPosition;

		public IMyDestroyableObject HitDestroyableObj => m_hitDestroaybleObj;

		public MyFloatingObject HitFloatingObject => m_hitFloatingObject;

		public MyEnvironmentSector HitEnvironmentSector => m_hitEnvironmentSector;

		public int EnvironmentItem => m_environmentItem;

		public MyCharacter HitCharacter => m_hitCharacter;

		public double DistanceToHitSq => m_distanceToHitSq;

		public Vector3D PointOfReference => m_pointOfReference;

		public MyDrillSensorBase Caster => m_caster;

		public MyCasterComponent(MyDrillSensorBase caster)
		{
			m_caster = caster;
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
		}

		public void OnWorldPosChanged(ref MatrixD newTransform)
		{
			MatrixD worldMatrix = newTransform;
			m_caster.OnWorldPositionChanged(ref worldMatrix);
			Dictionary<long, MyDrillSensorBase.DetectionInfo> entitiesInRange = m_caster.EntitiesInRange;
			float num = float.MaxValue;
			MyEntity myEntity = null;
			int environmentItem = 0;
			if (!m_isPointOfRefSet)
			{
				m_pointOfReference = worldMatrix.Translation;
			}
			if (entitiesInRange != null && entitiesInRange.Count > 0)
			{
				foreach (MyDrillSensorBase.DetectionInfo value2 in entitiesInRange.Values)
				{
					float num2 = (float)Vector3D.DistanceSquared(value2.DetectionPoint, m_pointOfReference);
					if (value2.Entity.Physics != null && value2.Entity.Physics.Enabled && num2 < num)
					{
						myEntity = value2.Entity;
						environmentItem = value2.ItemId;
						m_distanceToHitSq = num2;
						m_hitPosition = value2.DetectionPoint;
						num = num2;
					}
				}
			}
			m_hitCubeGrid = (myEntity as MyCubeGrid);
			m_hitBlock = null;
			m_hitDestroaybleObj = (myEntity as IMyDestroyableObject);
			m_hitFloatingObject = (myEntity as MyFloatingObject);
			m_hitCharacter = (myEntity as MyCharacter);
			m_hitEnvironmentSector = (myEntity as MyEnvironmentSector);
			m_environmentItem = environmentItem;
			if (m_hitCubeGrid != null)
			{
				MatrixD worldMatrixNormalizedInv = m_hitCubeGrid.PositionComp.WorldMatrixNormalizedInv;
				Vector3D value = Vector3D.Transform(m_hitPosition, worldMatrixNormalizedInv);
				m_hitCubeGrid.FixTargetCube(out Vector3I cube, value / m_hitCubeGrid.GridSize);
				m_hitBlock = m_hitCubeGrid.GetCubeBlock(cube);
			}
		}

		public void SetPointOfReference(Vector3D pointOfRef)
		{
			m_pointOfReference = pointOfRef;
			m_isPointOfRefSet = true;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
		}
	}
}
