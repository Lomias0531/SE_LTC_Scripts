using Sandbox.Engine.Physics;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyDefaultPlacementProvider : IMyPlacementProvider
	{
		private int m_lastUpdate;

		private MyPhysics.HitInfo? m_hitInfo;

		private MyCubeGrid m_closestGrid;

		private MySlimBlock m_closestBlock;

		private MyVoxelBase m_closestVoxelMap;

		private readonly List<MyPhysics.HitInfo> m_tmpHitList = new List<MyPhysics.HitInfo>();

		public Vector3D RayStart
		{
			get
			{
				MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
				if (cameraControllerEnum == MyCameraControllerEnum.Entity || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
				{
					if (MySession.Static.ControlledEntity != null)
					{
						return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: false).Translation;
					}
					if (MySector.MainCamera != null)
					{
						return MySector.MainCamera.Position;
					}
				}
				else if (MySector.MainCamera != null)
				{
					return MySector.MainCamera.Position;
				}
				return Vector3.Zero;
			}
		}

		public Vector3D RayDirection
		{
			get
			{
				MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
				if (cameraControllerEnum == MyCameraControllerEnum.Entity || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
				{
					if (MySession.Static.ControlledEntity != null)
					{
						return MySession.Static.ControlledEntity.GetHeadMatrix(includeY: false).Forward;
					}
					if (MySector.MainCamera != null)
					{
						return MySector.MainCamera.ForwardVector;
					}
				}
				else if (MySector.MainCamera != null)
				{
					return MySector.MainCamera.ForwardVector;
				}
				return Vector3.Forward;
			}
		}

		public MyPhysics.HitInfo? HitInfo
		{
			get
			{
				if (MySession.Static.GameplayFrameCounter != m_lastUpdate)
				{
					UpdatePlacement();
				}
				return m_hitInfo;
			}
		}

		public MyCubeGrid ClosestGrid
		{
			get
			{
				if (MySession.Static.GameplayFrameCounter != m_lastUpdate)
				{
					UpdatePlacement();
				}
				return m_closestGrid;
			}
		}

		public MyVoxelBase ClosestVoxelMap
		{
			get
			{
				if (MySession.Static.GameplayFrameCounter != m_lastUpdate)
				{
					UpdatePlacement();
				}
				return m_closestVoxelMap;
			}
		}

		public bool CanChangePlacementObjectSize => false;

		public float IntersectionDistance
		{
			get;
			set;
		}

		public MyDefaultPlacementProvider(float intersectionDistance)
		{
			IntersectionDistance = intersectionDistance;
		}

		public void RayCastGridCells(MyCubeGrid grid, List<Vector3I> outHitPositions, Vector3I gridSizeInflate, float maxDist)
		{
			grid.RayCastCells(RayStart, RayStart + RayDirection * maxDist, outHitPositions, gridSizeInflate);
		}

		public void UpdatePlacement()
		{
			m_lastUpdate = MySession.Static.GameplayFrameCounter;
			m_hitInfo = null;
			m_closestGrid = null;
			m_closestVoxelMap = null;
			LineD lineD = new LineD(RayStart, RayStart + RayDirection * IntersectionDistance);
			MyPhysics.CastRay(lineD.From, lineD.To, m_tmpHitList, 24);
			if (MySession.Static.ControlledEntity != null)
			{
				m_tmpHitList.RemoveAll((MyPhysics.HitInfo hitInfo) => hitInfo.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity);
			}
			if (m_tmpHitList.Count == 0)
			{
				return;
			}
			MyPhysics.HitInfo value = m_tmpHitList[0];
			if (value.HkHitInfo.GetHitEntity() != null)
			{
				m_closestGrid = (value.HkHitInfo.GetHitEntity().GetTopMostParent() as MyCubeGrid);
			}
			if (m_closestGrid != null)
			{
				m_hitInfo = value;
				if (!ClosestGrid.Editable)
				{
					m_closestGrid = null;
				}
			}
			else
			{
				m_closestVoxelMap = (value.HkHitInfo.GetHitEntity() as MyVoxelBase);
				if (m_closestVoxelMap != null)
				{
					m_hitInfo = value;
				}
			}
		}
	}
}
