using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Game;
using VRage.Game.Models;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public class MyMultilineConveyorEndpoint : IMyConveyorEndpoint, IMyPathVertex<IMyConveyorEndpoint>, IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>, IEnumerable
	{
		protected MyConveyorLine[] m_conveyorLines;

		protected static Dictionary<MyDefinitionId, ConveyorLinePosition[]> m_linePositions = new Dictionary<MyDefinitionId, ConveyorLinePosition[]>();

		private MyCubeBlock m_block;

		private MyPathfindingData m_pathfindingData;

		public MyCubeBlock CubeBlock => m_block;

		MyPathfindingData IMyPathVertex<IMyConveyorEndpoint>.PathfindingData => m_pathfindingData;

		public MyMultilineConveyorEndpoint(MyCubeBlock myBlock)
		{
			m_block = myBlock;
			MyConveyorLine.BlockLinePositionInformation[] blockLinePositions = MyConveyorLine.GetBlockLinePositions(myBlock);
			m_conveyorLines = new MyConveyorLine[blockLinePositions.Length];
			MyGridConveyorSystem conveyorSystem = myBlock.CubeGrid.GridSystems.ConveyorSystem;
			int num = 0;
			MyConveyorLine.BlockLinePositionInformation[] array = blockLinePositions;
			for (int i = 0; i < array.Length; i++)
			{
				MyConveyorLine.BlockLinePositionInformation blockLinePositionInformation = array[i];
				ConveyorLinePosition conveyorLinePosition = PositionToGridCoords(blockLinePositionInformation.Position);
				MyConveyorLine myConveyorLine = conveyorSystem.GetDeserializingLine(conveyorLinePosition);
				if (myConveyorLine == null)
				{
					myConveyorLine = new MyConveyorLine();
					myConveyorLine.Init(conveyorLinePosition, conveyorLinePosition.GetConnectingPosition(), myBlock.CubeGrid, blockLinePositionInformation.LineType, blockLinePositionInformation.LineConductivity);
					myConveyorLine.InitEndpoints(this, null);
				}
				else if (myConveyorLine.GetEndpointPosition(0).Equals(conveyorLinePosition))
				{
					myConveyorLine.SetEndpoint(0, this);
				}
				else if (myConveyorLine.GetEndpointPosition(1).Equals(conveyorLinePosition))
				{
					myConveyorLine.SetEndpoint(1, this);
				}
				m_conveyorLines[num] = myConveyorLine;
				num++;
			}
			myBlock.SlimBlock.ComponentStack.IsFunctionalChanged += UpdateLineFunctionality;
			myBlock.CubeGrid.GridSystems.ConveyorSystem.ResourceSink.IsPoweredChanged += UpdateLineFunctionality;
			m_pathfindingData = new MyPathfindingData(this);
		}

		public ConveyorLinePosition PositionToGridCoords(ConveyorLinePosition position)
		{
			return PositionToGridCoords(position, CubeBlock);
		}

		public static ConveyorLinePosition PositionToGridCoords(ConveyorLinePosition position, MyCubeBlock cubeBlock)
		{
			ConveyorLinePosition result = default(ConveyorLinePosition);
			Matrix result2 = default(Matrix);
			cubeBlock.Orientation.GetMatrix(out result2);
			Vector3 value = Vector3.Transform(new Vector3(position.LocalGridPosition), result2);
			result.LocalGridPosition = Vector3I.Round(value) + cubeBlock.Position;
			result.Direction = cubeBlock.Orientation.TransformDirection(position.Direction);
			return result;
		}

		public MyConveyorLine GetConveyorLine(ConveyorLinePosition position)
		{
			ConveyorLinePosition[] linePositions = GetLinePositions();
			for (int i = 0; i < linePositions.Length; i++)
			{
				if (PositionToGridCoords(linePositions[i]).Equals(position))
				{
					return m_conveyorLines[i];
				}
			}
			return null;
		}

		public ConveyorLinePosition GetPosition(int index)
		{
			ConveyorLinePosition[] linePositions = GetLinePositions();
			return PositionToGridCoords(linePositions[index]);
		}

		public MyConveyorLine GetConveyorLine(int index)
		{
			if (index >= m_conveyorLines.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return m_conveyorLines[index];
		}

		public void SetConveyorLine(ConveyorLinePosition position, MyConveyorLine newLine)
		{
			ConveyorLinePosition[] linePositions = GetLinePositions();
			int num = 0;
			while (true)
			{
				if (num < linePositions.Length)
				{
					if (PositionToGridCoords(linePositions[num]).Equals(position))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_conveyorLines[num] = newLine;
		}

		public int GetLineCount()
		{
			return m_conveyorLines.Length;
		}

		protected ConveyorLinePosition[] GetLinePositions()
		{
			ConveyorLinePosition[] value = null;
			lock (m_linePositions)
			{
				if (m_linePositions.TryGetValue(CubeBlock.BlockDefinition.Id, out value))
				{
					return value;
				}
				value = GetLinePositions(CubeBlock, "detector_conveyor");
				m_linePositions.Add(CubeBlock.BlockDefinition.Id, value);
				return value;
			}
		}

		public static ConveyorLinePosition[] GetLinePositions(MyCubeBlock cubeBlock, string dummyName)
		{
			return GetLinePositions(cubeBlock, MyModels.GetModelOnlyDummies(cubeBlock.BlockDefinition.Model).Dummies, dummyName);
		}

		public static ConveyorLinePosition[] GetLinePositions(MyCubeBlock cubeBlock, IDictionary<string, MyModelDummy> dummies, string dummyName)
		{
			MyCubeBlockDefinition blockDefinition = cubeBlock.BlockDefinition;
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize);
			Vector3 value = new Vector3(blockDefinition.Size) * 0.5f * cubeSize;
			int num = 0;
			foreach (KeyValuePair<string, MyModelDummy> dummy in dummies)
			{
				if (dummy.Key.ToLower().Contains(dummyName))
				{
					num++;
				}
			}
			ConveyorLinePosition[] array = new ConveyorLinePosition[num];
			int num2 = 0;
			foreach (KeyValuePair<string, MyModelDummy> dummy2 in dummies)
			{
				if (dummy2.Key.ToLower().Contains(dummyName))
				{
					Matrix matrix = dummy2.Value.Matrix;
					ConveyorLinePosition conveyorLinePosition = default(ConveyorLinePosition);
					Vector3 vector = matrix.Translation + blockDefinition.ModelOffset + value;
					Vector3I vector3I = Vector3I.Min(value2: Vector3I.Max(value2: Vector3I.Floor(vector / cubeSize), value1: Vector3I.Zero), value1: blockDefinition.Size - Vector3I.One);
					Vector3 value4 = (new Vector3(vector3I) + Vector3.Half) * cubeSize;
					Vector3 vec = Vector3.Normalize(Vector3.DominantAxisProjection(vector - value4));
					conveyorLinePosition.LocalGridPosition = vector3I - blockDefinition.Center;
					conveyorLinePosition.Direction = Base6Directions.GetDirection(vec);
					array[num2] = conveyorLinePosition;
					num2++;
				}
			}
			return array;
		}

		protected void UpdateLineFunctionality()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				for (int i = 0; i < m_conveyorLines.Length; i++)
				{
					m_conveyorLines[i].UpdateIsFunctional();
				}
			}, "MyMultilineConveyorEndpoint::UpdateLineFunctionality");
		}

		public ConveyorLineEnumerator GetEnumeratorInternal()
		{
			return new ConveyorLineEnumerator(this);
		}

		IEnumerator<IMyPathEdge<IMyConveyorEndpoint>> IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>.GetEnumerator()
		{
			return GetEnumeratorInternal();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorInternal();
		}

		float IMyPathVertex<IMyConveyorEndpoint>.EstimateDistanceTo(IMyPathVertex<IMyConveyorEndpoint> other)
		{
			return Vector3.RectangularDistance((other as IMyConveyorEndpoint).CubeBlock.WorldMatrix.Translation, CubeBlock.WorldMatrix.Translation);
		}

		int IMyPathVertex<IMyConveyorEndpoint>.GetNeighborCount()
		{
			return GetNeighborCount();
		}

		protected virtual int GetNeighborCount()
		{
			return m_conveyorLines.Length;
		}

		IMyPathVertex<IMyConveyorEndpoint> IMyPathVertex<IMyConveyorEndpoint>.GetNeighbor(int index)
		{
			return GetNeighbor(index);
		}

		protected virtual IMyPathVertex<IMyConveyorEndpoint> GetNeighbor(int index)
		{
			return m_conveyorLines[index].GetOtherVertex(this);
		}

		IMyPathEdge<IMyConveyorEndpoint> IMyPathVertex<IMyConveyorEndpoint>.GetEdge(int index)
		{
			return GetEdge(index);
		}

		protected virtual IMyPathEdge<IMyConveyorEndpoint> GetEdge(int index)
		{
			return m_conveyorLines[index];
		}

		public void DebugDraw()
		{
		}
	}
}
