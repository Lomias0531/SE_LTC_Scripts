using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using SpaceEngineers.Game.Entities.Blocks;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.EntityComponents.GameLogic
{
	public class MySharedWindComponent : MyEntityComponentBase
	{
		private float m_windSpeed = -1f;

		private MyWindTurbine m_updatingTurbine;

		private readonly HashSet<MyWindTurbine> m_windTurbines = new HashSet<MyWindTurbine>();

		public new MyCubeGrid Entity => (MyCubeGrid)base.Entity;

		public Vector3D GravityNormal
		{
			get;
			private set;
		}

		public float WindSpeed
		{
			get
			{
				if (!(m_windSpeed < 0f))
				{
					return m_windSpeed;
				}
				return 0f;
			}
			private set
			{
				if (m_windSpeed != value)
				{
					bool num = value == 0f;
					bool flag = m_windSpeed <= 0f;
					if (num != flag)
					{
						UpdatingTurbine.NeedsUpdate ^= MyEntityUpdateEnum.EACH_10TH_FRAME;
					}
					m_windSpeed = value;
					Vector3D worldPoint = Entity.Physics?.CenterOfMassWorld ?? Entity.PositionComp.GetPosition();
					GravityNormal = Vector3.Normalize(MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPoint));
					foreach (MyWindTurbine windTurbine in m_windTurbines)
					{
						windTurbine.OnEnvironmentChanged();
					}
				}
			}
		}

		public bool IsEnabled => WindSpeed > 0f;

		private MyWindTurbine UpdatingTurbine
		{
			get
			{
				return m_updatingTurbine;
			}
			set
			{
				if (m_updatingTurbine != null)
				{
					m_updatingTurbine.NeedsUpdate &= ~(MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
				}
				m_updatingTurbine = value;
				if (m_updatingTurbine != null)
				{
					MyEntityUpdateEnum myEntityUpdateEnum = MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					if (IsEnabled)
					{
						myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_10TH_FRAME;
					}
					m_updatingTurbine.NeedsUpdate |= myEntityUpdateEnum;
				}
			}
		}

		public override string ComponentTypeDebugString => GetType().Name;

		public void Register(MyWindTurbine windTurbine)
		{
			m_windTurbines.Add(windTurbine);
			if (UpdatingTurbine == null)
			{
				UpdatingTurbine = windTurbine;
			}
		}

		public void Unregister(MyWindTurbine windTurbine)
		{
			m_windTurbines.Remove(windTurbine);
			if (UpdatingTurbine == windTurbine)
			{
				if (m_windTurbines.Count == 0)
				{
					UpdatingTurbine = null;
					Entity.Components.Remove(typeof(MySharedWindComponent), this);
				}
				else
				{
					UpdatingTurbine = m_windTurbines.FirstElement();
				}
			}
		}

		public void Update10()
		{
			foreach (MyWindTurbine windTurbine in m_windTurbines)
			{
				windTurbine.UpdateNextRay();
			}
		}

		public void UpdateWindSpeed()
		{
			WindSpeed = ComputeWindSpeed();
		}

		private float ComputeWindSpeed()
		{
			MyCubeGrid entity = Entity;
			if (entity.IsPreview || entity.Physics == null || !MyFixedGrids.IsRooted(entity))
			{
				return 0f;
			}
			Vector3D centerOfMassWorld = entity.Physics.CenterOfMassWorld;
			MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(centerOfMassWorld);
			if (closestPlanet == null || closestPlanet.PositionComp.WorldAABB.Contains(centerOfMassWorld) == ContainmentType.Disjoint)
			{
				return 0f;
			}
			return closestPlanet.GetWindSpeed(centerOfMassWorld);
		}
	}
}
