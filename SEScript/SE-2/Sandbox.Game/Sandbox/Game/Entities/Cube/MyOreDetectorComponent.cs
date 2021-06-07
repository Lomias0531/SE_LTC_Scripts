using Sandbox.Game.Gui;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyOreDetectorComponent
	{
		public delegate bool CheckControlDelegate();

		public const int QUERY_LOD = 2;

		public const int CELL_SIZE_IN_VOXELS_BITS = 3;

		public const int CELL_SIZE_IN_LOD_VOXELS = 8;

		public const float CELL_SIZE_IN_METERS = 32f;

		public const float CELL_SIZE_IN_METERS_HALF = 16f;

		private static readonly List<MyVoxelBase> m_inRangeCache = new List<MyVoxelBase>();

		private static readonly List<MyVoxelBase> m_notInRangeCache = new List<MyVoxelBase>();

		public CheckControlDelegate OnCheckControl;

		private readonly Dictionary<MyVoxelBase, MyOreDepositGroup> m_depositGroupsByEntity = new Dictionary<MyVoxelBase, MyOreDepositGroup>();

		private bool m_discardQueryResult;

		public float DetectionRadius
		{
			get;
			set;
		}

		public bool BroadcastUsingAntennas
		{
			get;
			set;
		}

		public bool SetRelayedRequest
		{
			get;
			set;
		}

		public bool WillDiscardNextQuery => m_discardQueryResult;

		public MyOreDetectorComponent()
		{
			DetectionRadius = 50f;
			SetRelayedRequest = false;
			BroadcastUsingAntennas = false;
		}

		public void Update(Vector3D position, long detectorId, bool checkControl = true)
		{
			if (!SetRelayedRequest && checkControl && !OnCheckControl())
			{
				Clear();
				return;
			}
			SetRelayedRequest = false;
			BoundingSphereD sphere = new BoundingSphereD(position, DetectionRadius);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, m_inRangeCache);
			RemoveVoxelMapsOutOfRange();
			AddVoxelMapsInRange();
			UpdateDeposits(ref sphere, detectorId);
			m_inRangeCache.Clear();
		}

		private void UpdateDeposits(ref BoundingSphereD sphere, long detectorId)
		{
			foreach (MyOreDepositGroup value in m_depositGroupsByEntity.Values)
			{
				value.UpdateDeposits(ref sphere, detectorId, this);
			}
		}

		private void AddVoxelMapsInRange()
		{
			foreach (MyVoxelBase item in m_inRangeCache)
			{
				if (!(item is MyVoxelPhysics) && !(item.GetTopMostParent() is MyVoxelPhysics) && !m_depositGroupsByEntity.ContainsKey(item.GetTopMostParent() as MyVoxelBase))
				{
					m_depositGroupsByEntity.Add(item, new MyOreDepositGroup(item));
				}
			}
			m_inRangeCache.Clear();
		}

		private void RemoveVoxelMapsOutOfRange()
		{
			foreach (MyVoxelBase key in m_depositGroupsByEntity.Keys)
			{
				if (!m_inRangeCache.Contains(key.GetTopMostParent() as MyVoxelBase))
				{
					m_notInRangeCache.Add(key);
				}
			}
			foreach (MyVoxelBase item in m_notInRangeCache)
			{
				if (m_depositGroupsByEntity.TryGetValue(item, out MyOreDepositGroup value))
				{
					value.RemoveMarks();
				}
				m_depositGroupsByEntity.Remove(item);
			}
			m_notInRangeCache.Clear();
		}

		public void DiscardNextQuery()
		{
			m_discardQueryResult = true;
		}

		public void EnableNextQuery()
		{
			m_discardQueryResult = false;
		}

		public void Clear()
		{
			foreach (MyOreDepositGroup value in m_depositGroupsByEntity.Values)
			{
				value.ClearMinMax();
				foreach (MyEntityOreDeposit deposit in value.Deposits)
				{
					MyHud.OreMarkers.UnregisterMarker(deposit);
				}
			}
		}
	}
}
