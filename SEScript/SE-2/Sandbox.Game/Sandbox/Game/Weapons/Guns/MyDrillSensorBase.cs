using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Weapons.Guns
{
	public abstract class MyDrillSensorBase
	{
		public struct DetectionInfo
		{
			public readonly MyEntity Entity;

			public readonly Vector3D DetectionPoint;

			public readonly int ItemId;

			public DetectionInfo(MyEntity entity, Vector3D detectionPoint)
			{
				Entity = entity;
				DetectionPoint = detectionPoint;
				ItemId = 0;
			}

			public DetectionInfo(MyEntity entity, Vector3D detectionPoint, int itemid)
			{
				Entity = entity;
				DetectionPoint = detectionPoint;
				ItemId = itemid;
			}
		}

		private const int CacheExpirationFrames = 10;

		protected MyDefinitionBase m_drillDefinition;

		public HashSet<MyEntity> IgnoredEntities;

		private ulong m_cacheValidTill;

		protected readonly Dictionary<long, DetectionInfo> m_entitiesInRange;

		private Vector3D m_center;

		private Vector3D m_frontPoint;

		public MyDefinitionBase DrillDefinition => m_drillDefinition;

		public Dictionary<long, DetectionInfo> CachedEntitiesInRange
		{
			get
			{
				if (MySandboxGame.Static.SimulationFrameCounter >= m_cacheValidTill)
				{
					return EntitiesInRange;
				}
				return m_entitiesInRange;
			}
		}

		public Dictionary<long, DetectionInfo> EntitiesInRange
		{
			get
			{
				m_cacheValidTill = MySandboxGame.Static.SimulationFrameCounter + 10;
				ReadEntitiesInRange();
				return m_entitiesInRange;
			}
		}

		public Vector3D Center
		{
			get
			{
				return m_center;
			}
			protected set
			{
				m_center = value;
			}
		}

		public Vector3D FrontPoint
		{
			get
			{
				return m_frontPoint;
			}
			protected set
			{
				m_frontPoint = value;
			}
		}

		public MyDrillSensorBase()
		{
			IgnoredEntities = new HashSet<MyEntity>();
			m_entitiesInRange = new Dictionary<long, DetectionInfo>();
		}

		protected abstract void ReadEntitiesInRange();

		public abstract void OnWorldPositionChanged(ref MatrixD worldMatrix);

		public abstract void DebugDraw();
	}
}
