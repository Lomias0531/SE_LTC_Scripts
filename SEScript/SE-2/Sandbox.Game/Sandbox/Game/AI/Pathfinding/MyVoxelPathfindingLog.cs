using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyVoxelPathfindingLog : IMyPathfindingLog
	{
		private abstract class Operation
		{
			public abstract void Perform();
		}

		private class NavMeshOp : Operation
		{
			private string m_navmeshName;

			private bool m_isAddition;

			private Vector3I m_cellCoord;

			public NavMeshOp(string navmeshName, bool addition, Vector3I cellCoord)
			{
				m_navmeshName = navmeshName;
				m_isAddition = addition;
				m_cellCoord = cellCoord;
			}

			public override void Perform()
			{
				MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart(m_navmeshName.Split(new char[1]
				{
					'-'
				})[0]);
				if (myVoxelBase != null && MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.GetVoxelMapNavmesh(myVoxelBase) != null)
				{
					_ = m_isAddition;
				}
			}
		}

		private class VoxelWriteOp : Operation
		{
			private string m_voxelName;

			private string m_data;

			private MyStorageDataTypeFlags m_dataType;

			private Vector3I m_voxelMin;

			private Vector3I m_voxelMax;

			public VoxelWriteOp(string voxelName, string data, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax)
			{
				m_voxelName = voxelName;
				m_data = data;
				m_dataType = dataToWrite;
				m_voxelMin = voxelRangeMin;
				m_voxelMax = voxelRangeMax;
			}

			public override void Perform()
			{
				MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart(m_voxelName)?.Storage.WriteRange(MyStorageData.FromBase64(m_data), m_dataType, m_voxelMin, m_voxelMax);
			}
		}

		private string m_navmeshName;

		private List<Operation> m_operations = new List<Operation>();

		private int m_ctr;

		private MyLog m_log;

		public int Counter => m_ctr;

		public MyVoxelPathfindingLog(string filename)
		{
			string text = Path.Combine(MyFileSystem.UserDataPath, filename);
			if (MyFakes.REPLAY_NAVMESH_GENERATION)
			{
				StreamReader streamReader = new StreamReader(text);
				string text2 = null;
				string pattern = "NMOP: Voxel NavMesh: (\\S+) (ADD|REM) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\]";
				string pattern2 = "VOXOP: (\\S*) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] (\\S+) (\\S+)";
				while ((text2 = streamReader.ReadLine()) != null)
				{
					text2.Split(new char[1]
					{
						'['
					});
					MatchCollection matchCollection = Regex.Matches(text2, pattern);
					if (matchCollection.Count == 1)
					{
						string value = matchCollection[0].Groups[1].Value;
						if (m_navmeshName == null)
						{
							m_navmeshName = value;
						}
						bool addition = matchCollection[0].Groups[2].Value == "ADD";
						int x = int.Parse(matchCollection[0].Groups[3].Value);
						int y = int.Parse(matchCollection[0].Groups[4].Value);
						int z = int.Parse(matchCollection[0].Groups[5].Value);
						Vector3I cellCoord = new Vector3I(x, y, z);
						m_operations.Add(new NavMeshOp(m_navmeshName, addition, cellCoord));
						continue;
					}
					matchCollection = Regex.Matches(text2, pattern2);
					if (matchCollection.Count == 1)
					{
						string value2 = matchCollection[0].Groups[1].Value;
						int x2 = int.Parse(matchCollection[0].Groups[2].Value);
						int y2 = int.Parse(matchCollection[0].Groups[3].Value);
						int z2 = int.Parse(matchCollection[0].Groups[4].Value);
						Vector3I voxelRangeMin = new Vector3I(x2, y2, z2);
						x2 = int.Parse(matchCollection[0].Groups[5].Value);
						y2 = int.Parse(matchCollection[0].Groups[6].Value);
						z2 = int.Parse(matchCollection[0].Groups[7].Value);
						Vector3I voxelRangeMax = new Vector3I(x2, y2, z2);
						MyStorageDataTypeFlags dataToWrite = (MyStorageDataTypeFlags)Enum.Parse(typeof(MyStorageDataTypeFlags), matchCollection[0].Groups[8].Value);
						string value3 = matchCollection[0].Groups[9].Value;
						m_operations.Add(new VoxelWriteOp(value2, value3, dataToWrite, voxelRangeMin, voxelRangeMax));
					}
				}
				streamReader.Close();
			}
			if (MyFakes.LOG_NAVMESH_GENERATION)
			{
				m_log = new MyLog();
				m_log.Init(text, MyFinalBuildConstants.APP_VERSION_STRING);
			}
		}

		public void Close()
		{
			if (m_log != null)
			{
				m_log.Close();
			}
		}

		public void LogCellAddition(MyVoxelNavigationMesh navMesh, Vector3I cell)
		{
			m_log.WriteLine("NMOP: " + navMesh.ToString() + " ADD " + cell.ToString());
		}

		public void LogCellRemoval(MyVoxelNavigationMesh navMesh, Vector3I cell)
		{
			m_log.WriteLine("NMOP: " + navMesh.ToString() + " REM " + cell.ToString());
		}

		public void LogStorageWrite(MyVoxelBase map, MyStorageData source, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax)
		{
			string text = source.ToBase64();
			m_log.WriteLine($"VOXOP: {map.StorageName} {voxelRangeMin} {voxelRangeMax} {dataToWrite} {text}");
		}

		public void PerformOneOperation(bool triggerPressed)
		{
			if ((triggerPressed || m_ctr <= int.MaxValue) && m_ctr < m_operations.Count)
			{
				m_operations[m_ctr].Perform();
				m_ctr++;
			}
		}

		public void DebugDraw()
		{
			if (MyFakes.REPLAY_NAVMESH_GENERATION)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(500f, 10f), $"Next operation: {m_ctr}/{m_operations.Count}", Color.Red, 1f);
			}
		}
	}
}
