using Sandbox.Engine.Voxels;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyOreDepositGroup
	{
		private readonly MyVoxelBase m_voxelMap;

		private readonly Action<List<MyEntityOreDeposit>, List<Vector3I>, MyOreDetectorComponent> m_onDepositQueryComplete;

		private Dictionary<Vector3I, MyEntityOreDeposit> m_depositsByCellCoord_Main = new Dictionary<Vector3I, MyEntityOreDeposit>(Vector3I.Comparer);

		private Dictionary<Vector3I, MyEntityOreDeposit> m_depositsByCellCoord_Swap = new Dictionary<Vector3I, MyEntityOreDeposit>(Vector3I.Comparer);

		private Vector3I m_lastDetectionMin;

		private Vector3I m_lastDetectionMax;

		private int m_tasksRunning;

		public ICollection<MyEntityOreDeposit> Deposits => m_depositsByCellCoord_Main.Values;

		public void ClearMinMax()
		{
			m_lastDetectionMin = (m_lastDetectionMax = Vector3I.Zero);
		}

		public MyOreDepositGroup(MyVoxelBase voxelMap)
		{
			m_voxelMap = voxelMap;
			m_onDepositQueryComplete = OnDepositQueryComplete;
			m_lastDetectionMax = new Vector3I(int.MinValue);
			m_lastDetectionMin = new Vector3I(int.MaxValue);
		}

		private void OnDepositQueryComplete(List<MyEntityOreDeposit> deposits, List<Vector3I> emptyCells, MyOreDetectorComponent detectorComponent)
		{
			foreach (MyEntityOreDeposit deposit in deposits)
			{
				Vector3I cellCoord = deposit.CellCoord;
				m_depositsByCellCoord_Swap[cellCoord] = deposit;
			}
			m_tasksRunning--;
			if (m_tasksRunning == 0)
			{
				if (detectorComponent == null || detectorComponent.WillDiscardNextQuery)
				{
					foreach (MyEntityOreDeposit value in m_depositsByCellCoord_Main.Values)
					{
						MyHud.OreMarkers.UnregisterMarker(value);
					}
					foreach (MyEntityOreDeposit value2 in m_depositsByCellCoord_Swap.Values)
					{
						MyHud.OreMarkers.UnregisterMarker(value2);
					}
					m_depositsByCellCoord_Main.Clear();
					m_depositsByCellCoord_Swap.Clear();
				}
				else
				{
					Dictionary<Vector3I, MyEntityOreDeposit> depositsByCellCoord_Main = m_depositsByCellCoord_Main;
					m_depositsByCellCoord_Main = m_depositsByCellCoord_Swap;
					m_depositsByCellCoord_Swap = depositsByCellCoord_Main;
					foreach (MyEntityOreDeposit value3 in m_depositsByCellCoord_Swap.Values)
					{
						MyHud.OreMarkers.UnregisterMarker(value3);
					}
					m_depositsByCellCoord_Swap.Clear();
					foreach (MyEntityOreDeposit value4 in m_depositsByCellCoord_Main.Values)
					{
						MyHud.OreMarkers.RegisterMarker(value4);
					}
				}
			}
		}

		public void UpdateDeposits(ref BoundingSphereD worldDetectionSphere, long detectorId, MyOreDetectorComponent detectorComponent)
		{
			if (m_tasksRunning != 0)
			{
				return;
			}
			MySession @static = MySession.Static;
			if (@static == null || !@static.Ready)
			{
				return;
			}
			Vector3D worldPosition = worldDetectionSphere.Center - worldDetectionSphere.Radius;
			Vector3D worldPosition2 = worldDetectionSphere.Center + worldDetectionSphere.Radius;
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_voxelMap.PositionLeftBottomCorner, ref worldPosition, out Vector3I voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_voxelMap.PositionLeftBottomCorner, ref worldPosition2, out Vector3I voxelCoord2);
			voxelCoord += m_voxelMap.StorageMin;
			voxelCoord2 += m_voxelMap.StorageMin;
			m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
			m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
			voxelCoord >>= 5;
			voxelCoord2 >>= 5;
			if (voxelCoord == m_lastDetectionMin && voxelCoord2 == m_lastDetectionMax)
			{
				return;
			}
			m_lastDetectionMin = voxelCoord;
			m_lastDetectionMax = voxelCoord2;
			int num = Math.Max((voxelCoord2.X - voxelCoord.X) / 2, 1);
			int num2 = Math.Max((voxelCoord2.Y - voxelCoord.Y) / 2, 1);
			Vector3I min = default(Vector3I);
			min.Z = voxelCoord.Z;
			Vector3I max = default(Vector3I);
			max.Z = voxelCoord2.Z;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					min.X = voxelCoord.X + i * num;
					min.Y = voxelCoord.Y + j * num2;
					max.X = min.X + num;
					max.Y = min.Y + num2;
					MyDepositQuery.Start(min, max, detectorId, m_voxelMap, m_onDepositQueryComplete, detectorComponent);
					m_tasksRunning++;
				}
			}
		}

		internal void RemoveMarks()
		{
			foreach (MyEntityOreDeposit value in m_depositsByCellCoord_Main.Values)
			{
				MyHud.OreMarkers.UnregisterMarker(value);
			}
		}
	}
}
