using System.Collections.Generic;
using VRage.Collections;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyNavmeshComponents
	{
		public struct CellInfo
		{
			public int StartingIndex;

			public ushort ComponentNum;

			public Base6Directions.DirectionFlags ExploredDirections;

			public override string ToString()
			{
				return ComponentNum.ToString() + " components: " + StartingIndex + " - " + (StartingIndex + ComponentNum - 1) + ", Expl.: " + ExploredDirections.ToString();
			}
		}

		public struct ClosedCellInfo
		{
			public bool NewCell;

			public int OldStartingIndex;

			public ushort OldComponentNum;

			public int StartingIndex;

			public ushort ComponentNum;

			public Base6Directions.DirectionFlags ExploredDirections;
		}

		private Dictionary<ulong, CellInfo> m_cellInfos;

		private Dictionary<int, ulong> m_componentCells;

		private bool m_cellOpen;

		private bool m_componentOpen;

		private Dictionary<ushort, List<int>> m_componentIndicesAvailable;

		private int m_nextComponentIndex;

		private List<Vector3> m_lastCellComponentCenters;

		private ulong m_cellCoord;

		private ushort m_componentNum;

		private List<MyIntervalList> m_components;

		public MyNavmeshComponents()
		{
			m_cellOpen = false;
			m_componentOpen = false;
			m_cellInfos = new Dictionary<ulong, CellInfo>();
			m_componentCells = new Dictionary<int, ulong>();
			m_componentIndicesAvailable = new Dictionary<ushort, List<int>>();
			m_nextComponentIndex = 0;
			m_components = null;
			m_lastCellComponentCenters = new List<Vector3>();
		}

		public void OpenCell(ulong cellCoord)
		{
			m_cellOpen = true;
			m_cellCoord = cellCoord;
			m_componentNum = 0;
			m_components = new List<MyIntervalList>();
			m_lastCellComponentCenters.Clear();
		}

		public void CloseAndCacheCell(ref ClosedCellInfo output)
		{
			CellInfo value = default(CellInfo);
			bool flag = true;
			if (m_cellInfos.TryGetValue(m_cellCoord, out value))
			{
				output.NewCell = false;
				output.OldComponentNum = value.ComponentNum;
				output.OldStartingIndex = value.StartingIndex;
				if (value.ComponentNum == m_componentNum)
				{
					flag = false;
					value.ComponentNum = output.OldComponentNum;
					value.StartingIndex = output.OldStartingIndex;
				}
			}
			else
			{
				output.NewCell = true;
			}
			if (flag)
			{
				value.ComponentNum = m_componentNum;
				value.StartingIndex = AllocateComponentStartingIndex(m_componentNum);
				if (!output.NewCell)
				{
					DeallocateComponentStartingIndex(output.OldStartingIndex, output.OldComponentNum);
					for (int i = 0; i < output.OldComponentNum; i++)
					{
						m_componentCells.Remove(output.OldStartingIndex + i);
					}
				}
				for (int j = 0; j < value.ComponentNum; j++)
				{
					m_componentCells[value.StartingIndex + j] = m_cellCoord;
				}
			}
			m_cellInfos[m_cellCoord] = value;
			output.ComponentNum = value.ComponentNum;
			output.ExploredDirections = value.ExploredDirections;
			output.StartingIndex = value.StartingIndex;
			m_components = null;
			m_componentNum = 0;
			m_cellOpen = false;
		}

		public void OpenComponent()
		{
			m_componentOpen = true;
			m_lastCellComponentCenters.Add(Vector3.Zero);
			m_components.Add(new MyIntervalList());
		}

		public void AddComponentTriangle(MyNavigationTriangle triangle, Vector3 center)
		{
			int index = triangle.Index;
			MyIntervalList myIntervalList = m_components[m_componentNum];
			myIntervalList.Add(index);
			float num = 1f / (float)myIntervalList.Count;
			m_lastCellComponentCenters[m_componentNum] = center * num + m_lastCellComponentCenters[m_componentNum] * (1f - num);
		}

		public void CloseComponent()
		{
			m_componentNum++;
			m_componentOpen = false;
		}

		public Vector3 GetComponentCenter(int index)
		{
			return m_lastCellComponentCenters[index];
		}

		public bool TryGetComponentCell(int componentIndex, out ulong cellIndex)
		{
			return m_componentCells.TryGetValue(componentIndex, out cellIndex);
		}

		public bool GetComponentCell(int componentIndex, out ulong cellIndex)
		{
			return m_componentCells.TryGetValue(componentIndex, out cellIndex);
		}

		public bool GetComponentInfo(int componentIndex, ulong cellIndex, out Base6Directions.DirectionFlags exploredDirections)
		{
			exploredDirections = (Base6Directions.DirectionFlags)0;
			TryGetCell(cellIndex, out CellInfo cellInfo);
			int num = componentIndex - cellInfo.StartingIndex;
			if (num < 0 || num >= cellInfo.ComponentNum)
			{
				return false;
			}
			exploredDirections = cellInfo.ExploredDirections;
			return true;
		}

		public void MarkExplored(ulong otherCell, Base6Directions.Direction direction)
		{
			CellInfo value = default(CellInfo);
			if (m_cellInfos.TryGetValue(otherCell, out value))
			{
				value.ExploredDirections |= Base6Directions.GetDirectionFlag(direction);
				m_cellInfos[otherCell] = value;
			}
		}

		public void SetExplored(ulong packedCoord, Base6Directions.DirectionFlags directionFlags)
		{
			CellInfo value = default(CellInfo);
			if (m_cellInfos.TryGetValue(packedCoord, out value))
			{
				value.ExploredDirections = directionFlags;
				m_cellInfos[packedCoord] = value;
			}
		}

		public bool TryGetCell(ulong packedCoord, out CellInfo cellInfo)
		{
			return m_cellInfos.TryGetValue(packedCoord, out cellInfo);
		}

		public ICollection<ulong> GetPresentCells()
		{
			return m_cellInfos.Keys;
		}

		public void ClearCell(ulong packedCoord, ref CellInfo cellInfo)
		{
			for (int i = 0; i < cellInfo.ComponentNum; i++)
			{
				m_componentCells.Remove(cellInfo.StartingIndex + i);
			}
			m_cellInfos.Remove(packedCoord);
			DeallocateComponentStartingIndex(cellInfo.StartingIndex, cellInfo.ComponentNum);
		}

		private int AllocateComponentStartingIndex(ushort componentNum)
		{
			List<int> value = null;
			if (m_componentIndicesAvailable.TryGetValue(componentNum, out value) && value.Count > 0)
			{
				int result = value[value.Count - 1];
				value.RemoveAt(value.Count - 1);
				return result;
			}
			int nextComponentIndex = m_nextComponentIndex;
			m_nextComponentIndex += componentNum;
			return nextComponentIndex;
		}

		private void DeallocateComponentStartingIndex(int index, ushort componentNum)
		{
			List<int> value = null;
			if (!m_componentIndicesAvailable.TryGetValue(componentNum, out value))
			{
				value = new List<int>();
				m_componentIndicesAvailable[componentNum] = value;
			}
			value.Add(index);
		}
	}
}
