using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Models;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public class MyConveyorLine : IEnumerable<Vector3I>, IEnumerable, IMyPathEdge<IMyConveyorEndpoint>
	{
		public struct LinePositionEnumerator : IEnumerator<Vector3I>, IEnumerator, IDisposable
		{
			private MyConveyorLine m_line;

			private Vector3I m_currentPosition;

			private Vector3I m_direction;

			private int m_index;

			private int m_sectionIndex;

			private int m_sectionLength;

			public Vector3I Current => m_currentPosition;

			object IEnumerator.Current => Current;

			public LinePositionEnumerator(MyConveyorLine line)
			{
				m_line = line;
				m_currentPosition = line.m_endpointPosition1.LocalGridPosition;
				m_direction = line.m_endpointPosition1.VectorDirection;
				m_index = 0;
				m_sectionIndex = 0;
				m_sectionLength = m_line.m_length;
				UpdateSectionLength();
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				m_index++;
				m_currentPosition += m_direction;
				if (m_index >= m_sectionLength)
				{
					m_index = 0;
					m_sectionIndex++;
					if (m_line.m_sections == null || m_sectionIndex >= m_line.m_sections.Count)
					{
						return false;
					}
					m_direction = Base6Directions.GetIntVector(m_line.m_sections[m_sectionIndex].Direction);
					UpdateSectionLength();
				}
				return true;
			}

			public void Reset()
			{
				m_currentPosition = m_line.m_endpointPosition1.LocalGridPosition;
				m_direction = m_line.m_endpointPosition1.VectorDirection;
				m_index = 0;
				m_sectionIndex = 0;
				m_sectionLength = m_line.m_length;
				UpdateSectionLength();
			}

			private void UpdateSectionLength()
			{
				if (m_line.m_sections != null && m_line.m_sections.Count != 0)
				{
					m_sectionLength = m_line.m_sections[m_sectionIndex].Length;
				}
			}
		}

		private struct SectionInformation
		{
			public Base6Directions.Direction Direction;

			public int Length;

			public void Reverse()
			{
				Direction = Base6Directions.GetFlippedDirection(Direction);
			}

			public override string ToString()
			{
				return Length + " -> " + Direction;
			}
		}

		public struct BlockLinePositionInformation
		{
			public ConveyorLinePosition Position;

			public MyObjectBuilder_ConveyorLine.LineType LineType;

			public MyObjectBuilder_ConveyorLine.LineConductivity LineConductivity;
		}

		public class InvertedConductivity : IDisposable
		{
			public InvertedConductivity()
			{
				m_invertedConductivity = !m_invertedConductivity;
			}

			public void Dispose()
			{
				m_invertedConductivity = !m_invertedConductivity;
			}
		}

		private static ConcurrentDictionary<MyDefinitionId, BlockLinePositionInformation[]> m_blockLinePositions = new ConcurrentDictionary<MyDefinitionId, BlockLinePositionInformation[]>();

		private static readonly float CONVEYOR_PER_LINE_PENALTY = 1f;

		private const int FRAMES_PER_BIG_UPDATE = 64;

		private const float BIG_UPDATE_FRACTION = 0.015625f;

		private ConveyorLinePosition m_endpointPosition1;

		private ConveyorLinePosition m_endpointPosition2;

		private IMyConveyorEndpoint m_endpoint1;

		private IMyConveyorEndpoint m_endpoint2;

		private MyObjectBuilder_ConveyorLine.LineType m_type;

		private MyObjectBuilder_ConveyorLine.LineConductivity m_conductivity;

		private int m_length;

		private MyCubeGrid m_cubeGrid;

		[ThreadStatic]
		private static bool m_invertedConductivity = false;

		private MySinglyLinkedList<MyConveyorPacket> m_queue1;

		private MySinglyLinkedList<MyConveyorPacket> m_queue2;

		private List<SectionInformation> m_sections;

		private static List<SectionInformation> m_tmpSections1 = new List<SectionInformation>();

		private static List<SectionInformation> m_tmpSections2 = new List<SectionInformation>();

		private bool m_stopped1;

		private bool m_stopped2;

		private float m_queuePosition;

		private bool m_isFunctional;

		private bool m_isWorking;

		private LinePositionEnumerator m_enumerator;

		public bool IsFunctional => m_isFunctional;

		public bool IsWorking => m_isWorking;

		public int Length => m_length;

		public bool IsDegenerate
		{
			get
			{
				if (Length == 1)
				{
					return HasNullEndpoints;
				}
				return false;
			}
		}

		public bool IsCircular
		{
			get
			{
				if (Length != 1)
				{
					return m_endpointPosition1.GetConnectingPosition().Equals(m_endpointPosition2);
				}
				return false;
			}
		}

		public bool HasNullEndpoints
		{
			get
			{
				if (m_endpoint1 == null)
				{
					return m_endpoint2 == null;
				}
				return false;
			}
		}

		public bool IsDisconnected
		{
			get
			{
				if (m_endpoint1 != null)
				{
					return m_endpoint2 == null;
				}
				return true;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (m_queue1.Count == 0)
				{
					return m_queue2.Count == 0;
				}
				return false;
			}
		}

		public MyObjectBuilder_ConveyorLine.LineType Type => m_type;

		public MyObjectBuilder_ConveyorLine.LineConductivity Conductivity => m_conductivity;

		public MyConveyorLine()
		{
			m_queue1 = new MySinglyLinkedList<MyConveyorPacket>();
			m_queue2 = new MySinglyLinkedList<MyConveyorPacket>();
			m_length = 0;
			m_queuePosition = 0f;
			m_stopped1 = false;
			m_stopped2 = false;
			m_sections = null;
			m_isFunctional = false;
			m_isWorking = false;
		}

		public MyObjectBuilder_ConveyorLine GetObjectBuilder()
		{
			MyObjectBuilder_ConveyorLine myObjectBuilder_ConveyorLine = new MyObjectBuilder_ConveyorLine();
			foreach (MyConveyorPacket item2 in m_queue1)
			{
				MyObjectBuilder_ConveyorPacket myObjectBuilder_ConveyorPacket = new MyObjectBuilder_ConveyorPacket();
				myObjectBuilder_ConveyorPacket.Item = item2.Item.GetObjectBuilder();
				myObjectBuilder_ConveyorPacket.LinePosition = item2.LinePosition;
				myObjectBuilder_ConveyorLine.PacketsForward.Add(myObjectBuilder_ConveyorPacket);
			}
			foreach (MyConveyorPacket item3 in m_queue2)
			{
				MyObjectBuilder_ConveyorPacket myObjectBuilder_ConveyorPacket2 = new MyObjectBuilder_ConveyorPacket();
				myObjectBuilder_ConveyorPacket2.Item = item3.Item.GetObjectBuilder();
				myObjectBuilder_ConveyorPacket2.LinePosition = item3.LinePosition;
				myObjectBuilder_ConveyorLine.PacketsBackward.Add(myObjectBuilder_ConveyorPacket2);
			}
			myObjectBuilder_ConveyorLine.StartPosition = m_endpointPosition1.LocalGridPosition;
			myObjectBuilder_ConveyorLine.StartDirection = m_endpointPosition1.Direction;
			myObjectBuilder_ConveyorLine.EndPosition = m_endpointPosition2.LocalGridPosition;
			myObjectBuilder_ConveyorLine.EndDirection = m_endpointPosition2.Direction;
			if (m_sections != null)
			{
				myObjectBuilder_ConveyorLine.Sections = new List<SerializableLineSectionInformation>(m_sections.Count);
				foreach (SectionInformation section in m_sections)
				{
					SerializableLineSectionInformation item = default(SerializableLineSectionInformation);
					item.Direction = section.Direction;
					item.Length = section.Length;
					myObjectBuilder_ConveyorLine.Sections.Add(item);
				}
			}
			myObjectBuilder_ConveyorLine.ConveyorLineType = m_type;
			myObjectBuilder_ConveyorLine.ConveyorLineConductivity = m_conductivity;
			return myObjectBuilder_ConveyorLine;
		}

		private void InitializeSectionList(int size = -1)
		{
			if (m_sections != null)
			{
				m_sections.Clear();
				if (size != -1)
				{
					m_sections.Capacity = size;
				}
			}
			else if (size != -1)
			{
				m_sections = new List<SectionInformation>(size);
			}
			else
			{
				m_sections = new List<SectionInformation>();
			}
		}

		public void Init(MyObjectBuilder_ConveyorLine objectBuilder, MyCubeGrid cubeGrid)
		{
			m_cubeGrid = cubeGrid;
			foreach (MyObjectBuilder_ConveyorPacket item2 in objectBuilder.PacketsForward)
			{
				MyConveyorPacket myConveyorPacket = new MyConveyorPacket();
				myConveyorPacket.Init(item2, m_cubeGrid);
				m_queue1.Append(myConveyorPacket);
			}
			foreach (MyObjectBuilder_ConveyorPacket item3 in objectBuilder.PacketsBackward)
			{
				MyConveyorPacket myConveyorPacket2 = new MyConveyorPacket();
				myConveyorPacket2.Init(item3, m_cubeGrid);
				m_queue2.Append(myConveyorPacket2);
			}
			m_endpointPosition1 = new ConveyorLinePosition(objectBuilder.StartPosition, objectBuilder.StartDirection);
			m_endpointPosition2 = new ConveyorLinePosition(objectBuilder.EndPosition, objectBuilder.EndDirection);
			m_length = 0;
			if (objectBuilder.Sections != null && objectBuilder.Sections.Count != 0)
			{
				InitializeSectionList(objectBuilder.Sections.Count);
				foreach (SerializableLineSectionInformation section in objectBuilder.Sections)
				{
					SectionInformation item = default(SectionInformation);
					item.Direction = section.Direction;
					item.Length = section.Length;
					m_sections.Add(item);
					m_length += item.Length;
				}
			}
			if (m_length == 0)
			{
				m_length = m_endpointPosition2.LocalGridPosition.RectangularDistance(m_endpointPosition1.LocalGridPosition);
			}
			m_type = objectBuilder.ConveyorLineType;
			if (m_type == MyObjectBuilder_ConveyorLine.LineType.DEFAULT_LINE)
			{
				if (cubeGrid.GridSizeEnum == MyCubeSize.Small)
				{
					m_type = MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;
				}
				else if (cubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					m_type = MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE;
				}
			}
			m_conductivity = objectBuilder.ConveyorLineConductivity;
			StopQueuesIfNeeded();
			RecalculatePacketPositions();
		}

		public void Init(ConveyorLinePosition endpoint1, ConveyorLinePosition endpoint2, MyCubeGrid cubeGrid, MyObjectBuilder_ConveyorLine.LineType type, MyObjectBuilder_ConveyorLine.LineConductivity conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL, Vector3I? corner = null)
		{
			m_cubeGrid = cubeGrid;
			m_type = type;
			m_conductivity = conductivity;
			m_endpointPosition1 = endpoint1;
			m_endpointPosition2 = endpoint2;
			m_isFunctional = false;
			if (corner.HasValue)
			{
				InitializeSectionList(2);
				Vector3I vec = corner.Value - endpoint1.LocalGridPosition;
				int num = vec.RectangularLength();
				vec /= num;
				Vector3I vec2 = endpoint2.LocalGridPosition - corner.Value;
				int num2 = vec2.RectangularLength();
				vec2 /= num2;
				Base6Directions.Direction direction = Base6Directions.GetDirection(vec);
				Base6Directions.Direction direction2 = Base6Directions.GetDirection(vec2);
				SectionInformation sectionInformation = default(SectionInformation);
				sectionInformation.Direction = direction;
				sectionInformation.Length = num;
				SectionInformation item = sectionInformation;
				sectionInformation = default(SectionInformation);
				sectionInformation.Direction = direction2;
				sectionInformation.Length = num2;
				SectionInformation item2 = sectionInformation;
				m_sections.Add(item);
				m_sections.Add(item2);
			}
			m_length = endpoint1.LocalGridPosition.RectangularDistance(endpoint2.LocalGridPosition);
		}

		private void InitAfterSplit(ConveyorLinePosition endpoint1, ConveyorLinePosition endpoint2, List<SectionInformation> sections, int newLength, MyCubeGrid cubeGrid, MyObjectBuilder_ConveyorLine.LineType lineType)
		{
			m_endpointPosition1 = endpoint1;
			m_endpointPosition2 = endpoint2;
			m_sections = sections;
			m_length = newLength;
			m_cubeGrid = cubeGrid;
			m_type = lineType;
		}

		public void InitEndpoints(IMyConveyorEndpoint endpoint1, IMyConveyorEndpoint endpoint2)
		{
			m_endpoint1 = endpoint1;
			m_endpoint2 = endpoint2;
			UpdateIsFunctional();
		}

		private void RecalculatePacketPositions()
		{
			int num = 0;
			Vector3I sectionStart = m_endpointPosition1.LocalGridPosition;
			Base6Directions.Direction direction = m_endpointPosition1.Direction;
			int num2 = 0;
			int length = Length;
			if (m_sections != null)
			{
				num2 = m_sections.Count - 1;
				num = Length - m_sections[num2].Length;
				sectionStart = m_endpointPosition2.LocalGridPosition - Base6Directions.GetIntVector(direction) * m_sections[num2].Length;
				direction = m_sections[num2].Direction;
				length = m_sections[num2].Length;
			}
			Base6Directions.Direction perpendicular = Base6Directions.GetPerpendicular(direction);
			MySinglyLinkedList<MyConveyorPacket>.Enumerator enumerator = m_queue1.GetEnumerator();
			bool flag = enumerator.MoveNext();
			while (num >= 0)
			{
				while (flag && enumerator.Current.LinePosition >= num)
				{
					enumerator.Current.SetLocalPosition(sectionStart, num, m_cubeGrid.GridSize, direction, perpendicular);
					enumerator.Current.SetSegmentLength(m_cubeGrid.GridSize);
					flag = enumerator.MoveNext();
				}
				if (m_sections == null || !flag)
				{
					break;
				}
				num2--;
				if (num2 < 0)
				{
					break;
				}
				direction = m_sections[num2].Direction;
				length = m_sections[num2].Length;
				num -= length;
				sectionStart -= Base6Directions.GetIntVector(direction) * length;
			}
			num = 0;
			sectionStart = m_endpointPosition2.LocalGridPosition;
			direction = m_endpointPosition2.Direction;
			perpendicular = Base6Directions.GetFlippedDirection(perpendicular);
			num2 = 0;
			length = Length;
			if (m_sections != null)
			{
				length = m_sections[num2].Length;
				num = Length - length;
				direction = Base6Directions.GetFlippedDirection(m_sections[num2].Direction);
				sectionStart = m_endpointPosition1.LocalGridPosition - Base6Directions.GetIntVector(direction) * length;
			}
			MySinglyLinkedList<MyConveyorPacket>.Enumerator enumerator2 = m_queue2.GetEnumerator();
			bool flag2 = enumerator2.MoveNext();
			while (num >= 0)
			{
				while (flag2 && enumerator2.Current.LinePosition >= num)
				{
					enumerator2.Current.SetLocalPosition(sectionStart, num, m_cubeGrid.GridSize, direction, perpendicular);
					enumerator2.Current.SetSegmentLength(m_cubeGrid.GridSize);
					flag2 = enumerator2.MoveNext();
				}
				if (m_sections != null && flag2)
				{
					num2++;
					if (num2 < m_sections.Count)
					{
						length = m_sections[num2].Length;
						num -= length;
						direction = Base6Directions.GetFlippedDirection(m_sections[num2].Direction);
						sectionStart -= Base6Directions.GetIntVector(direction) * length;
						continue;
					}
					break;
				}
				break;
			}
		}

		public IMyConveyorEndpoint GetEndpoint(int index)
		{
			switch (index)
			{
			case 0:
				return m_endpoint1;
			case 1:
				return m_endpoint2;
			default:
				throw new IndexOutOfRangeException();
			}
		}

		public void SetEndpoint(int index, IMyConveyorEndpoint endpoint)
		{
			switch (index)
			{
			case 0:
				m_endpoint1 = endpoint;
				break;
			case 1:
				m_endpoint2 = endpoint;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}

		public ConveyorLinePosition GetEndpointPosition(int index)
		{
			switch (index)
			{
			case 0:
				return m_endpointPosition1;
			case 1:
				return m_endpointPosition2;
			default:
				throw new IndexOutOfRangeException();
			}
		}

		public static BlockLinePositionInformation[] GetBlockLinePositions(MyCubeBlock block)
		{
			if (m_blockLinePositions.TryGetValue(block.BlockDefinition.Id, out BlockLinePositionInformation[] value))
			{
				return value;
			}
			MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize);
			Vector3 value2 = new Vector3(blockDefinition.Size) * 0.5f * cubeSize;
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(block.BlockDefinition.Model);
			int num = 0;
			foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
			{
				string[] array = dummy.Key.ToLower().Split(new char[1]
				{
					'_'
				});
				if (array.Length >= 2 && array[0] == "detector" && array[1].StartsWith("conveyor"))
				{
					num++;
				}
			}
			value = new BlockLinePositionInformation[num];
			int num2 = 0;
			foreach (KeyValuePair<string, MyModelDummy> dummy2 in modelOnlyDummies.Dummies)
			{
				string[] array2 = dummy2.Key.ToLower().Split(new char[1]
				{
					'_'
				});
				if (array2.Length >= 2 && !(array2[0] != "detector") && array2[1].StartsWith("conveyor"))
				{
					if (array2.Length > 2 && array2[2] == "small")
					{
						value[num2].LineType = MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;
					}
					else
					{
						value[num2].LineType = MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE;
					}
					value[num2].LineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
					if ((array2.Length > 2 && array2[2] == "in") || (array2.Length > 3 && array2[3] == "in"))
					{
						value[num2].LineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
					}
					if ((array2.Length > 2 && array2[2] == "out") || (array2.Length > 3 && array2[3] == "out"))
					{
						value[num2].LineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
					}
					Matrix matrix = dummy2.Value.Matrix;
					ConveyorLinePosition position = default(ConveyorLinePosition);
					Vector3 vector = matrix.Translation + blockDefinition.ModelOffset + value2;
					Vector3I vector3I = Vector3I.Min(value2: Vector3I.Max(value2: Vector3I.Floor(vector / cubeSize), value1: Vector3I.Zero), value1: blockDefinition.Size - Vector3I.One);
					Vector3 value5 = (new Vector3(vector3I) + Vector3.Half) * cubeSize;
					Vector3 vec = Vector3.Normalize(Vector3.DominantAxisProjection(Vector3.Divide(vector - value5, cubeSize)));
					position.LocalGridPosition = vector3I - blockDefinition.Center;
					position.Direction = Base6Directions.GetDirection(vec);
					value[num2].Position = position;
					num2++;
				}
			}
			m_blockLinePositions.TryAdd(blockDefinition.Id, value);
			return value;
		}

		public void RecalculateConductivity()
		{
			m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
			MyObjectBuilder_ConveyorLine.LineConductivity lineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
			MyObjectBuilder_ConveyorLine.LineConductivity lineConductivity2 = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
			if (m_endpoint1 != null && m_endpoint1 is MyMultilineConveyorEndpoint)
			{
				MyMultilineConveyorEndpoint myMultilineConveyorEndpoint = m_endpoint1 as MyMultilineConveyorEndpoint;
				BlockLinePositionInformation[] blockLinePositions = GetBlockLinePositions(myMultilineConveyorEndpoint.CubeBlock);
				for (int i = 0; i < blockLinePositions.Length; i++)
				{
					BlockLinePositionInformation blockLinePositionInformation = blockLinePositions[i];
					if (myMultilineConveyorEndpoint.PositionToGridCoords(blockLinePositionInformation.Position).Equals(m_endpointPosition1))
					{
						lineConductivity = blockLinePositionInformation.LineConductivity;
						break;
					}
				}
			}
			if (m_endpoint2 != null && m_endpoint2 is MyMultilineConveyorEndpoint)
			{
				MyMultilineConveyorEndpoint myMultilineConveyorEndpoint2 = m_endpoint2 as MyMultilineConveyorEndpoint;
				BlockLinePositionInformation[] blockLinePositions = GetBlockLinePositions(myMultilineConveyorEndpoint2.CubeBlock);
				for (int i = 0; i < blockLinePositions.Length; i++)
				{
					BlockLinePositionInformation blockLinePositionInformation2 = blockLinePositions[i];
					if (myMultilineConveyorEndpoint2.PositionToGridCoords(blockLinePositionInformation2.Position).Equals(m_endpointPosition2))
					{
						lineConductivity2 = blockLinePositionInformation2.LineConductivity;
						switch (lineConductivity2)
						{
						case MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD:
							lineConductivity2 = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
							break;
						case MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD:
							lineConductivity2 = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
							break;
						}
						break;
					}
				}
			}
			if (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.NONE || lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.NONE || (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD) || (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD))
			{
				m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.NONE;
			}
			else if ((lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FULL && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD) || (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FULL) || (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD))
			{
				m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
			}
			else if ((lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FULL && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD) || (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FULL) || (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD))
			{
				m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
			}
			else
			{
				m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
			}
		}

		public void Merge(MyConveyorLine mergingLine, IMyConveyorSegmentBlock newlyAddedBlock = null)
		{
			ConveyorLinePosition connectingPosition = m_endpointPosition2.GetConnectingPosition();
			if (mergingLine.m_endpointPosition1.Equals(connectingPosition))
			{
				MergeInternal(mergingLine, newlyAddedBlock);
			}
			else if (mergingLine.m_endpointPosition2.Equals(connectingPosition))
			{
				mergingLine.Reverse();
				MergeInternal(mergingLine, newlyAddedBlock);
			}
			else
			{
				Reverse();
				connectingPosition = m_endpointPosition2.GetConnectingPosition();
				if (mergingLine.m_endpointPosition1.Equals(connectingPosition))
				{
					MergeInternal(mergingLine, newlyAddedBlock);
				}
				else if (mergingLine.m_endpointPosition2.Equals(connectingPosition))
				{
					mergingLine.Reverse();
					MergeInternal(mergingLine, newlyAddedBlock);
				}
			}
			mergingLine.RecalculateConductivity();
		}

		public void MergeInternal(MyConveyorLine mergingLine, IMyConveyorSegmentBlock newlyAddedBlock = null)
		{
			m_endpointPosition2 = mergingLine.m_endpointPosition2;
			m_endpoint2 = mergingLine.m_endpoint2;
			if (mergingLine.m_sections != null)
			{
				if (m_sections == null)
				{
					InitializeSectionList(mergingLine.m_sections.Count);
					m_sections.AddRange(mergingLine.m_sections);
					SectionInformation value = m_sections[0];
					value.Length += m_length - 1;
					m_sections[0] = value;
				}
				else
				{
					m_sections.Capacity = m_sections.Count + mergingLine.m_sections.Count - 1;
					SectionInformation value2 = m_sections[m_sections.Count - 1];
					value2.Length += mergingLine.m_sections[0].Length - 1;
					m_sections[m_sections.Count - 1] = value2;
					for (int i = 1; i < mergingLine.m_sections.Count; i++)
					{
						m_sections.Add(mergingLine.m_sections[i]);
					}
				}
			}
			else if (m_sections != null)
			{
				SectionInformation value3 = m_sections[m_sections.Count - 1];
				value3.Length += mergingLine.m_length - 1;
				m_sections[m_sections.Count - 1] = value3;
			}
			m_length = m_length + mergingLine.m_length - 1;
			UpdateIsFunctional();
			if (newlyAddedBlock != null)
			{
				m_isFunctional &= newlyAddedBlock.ConveyorSegment.CubeBlock.IsFunctional;
				m_isWorking &= m_isFunctional;
			}
		}

		public bool CheckSectionConsistency()
		{
			if (m_sections == null)
			{
				return true;
			}
			Base6Directions.Direction? direction = null;
			foreach (SectionInformation section in m_sections)
			{
				if (direction.HasValue && direction.Value == section.Direction)
				{
					return false;
				}
				direction = section.Direction;
			}
			return true;
		}

		public void Reverse()
		{
			ConveyorLinePosition endpointPosition = m_endpointPosition1;
			m_endpointPosition1 = m_endpointPosition2;
			m_endpointPosition2 = endpointPosition;
			IMyConveyorEndpoint endpoint = m_endpoint1;
			m_endpoint1 = m_endpoint2;
			m_endpoint2 = endpoint;
			if (m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
			{
				m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
			}
			else if (m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD)
			{
				m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
			}
			if (m_sections != null)
			{
				for (int i = 0; i < (m_sections.Count + 1) / 2; i++)
				{
					int index = m_sections.Count - i - 1;
					SectionInformation value = m_sections[i];
					value.Direction = Base6Directions.GetFlippedDirection(value.Direction);
					SectionInformation value2 = m_sections[index];
					value2.Direction = Base6Directions.GetFlippedDirection(value2.Direction);
					m_sections[i] = value2;
					m_sections[index] = value;
				}
			}
		}

		public void DisconnectEndpoint(IMyConveyorEndpoint endpoint)
		{
			if (endpoint == m_endpoint1)
			{
				m_endpoint1 = null;
			}
			if (endpoint == m_endpoint2)
			{
				m_endpoint2 = null;
			}
			UpdateIsFunctional();
		}

		public IEnumerator<Vector3I> GetEnumerator()
		{
			return new LinePositionEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new LinePositionEnumerator(this);
		}

		public float GetWeight()
		{
			return (float)Length + CONVEYOR_PER_LINE_PENALTY;
		}

		public IMyConveyorEndpoint GetOtherVertex(IMyConveyorEndpoint endpoint)
		{
			if (!m_isWorking)
			{
				return null;
			}
			MyObjectBuilder_ConveyorLine.LineConductivity lineConductivity = m_conductivity;
			if (m_invertedConductivity)
			{
				if (m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
				{
					lineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
				}
				else if (m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD)
				{
					lineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
				}
			}
			if (endpoint == m_endpoint1)
			{
				if (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FULL || lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD)
				{
					return m_endpoint2;
				}
				return null;
			}
			if (endpoint == m_endpoint2)
			{
				if (lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FULL || lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
				{
					return m_endpoint1;
				}
				return null;
			}
			return null;
		}

		public override string ToString()
		{
			return m_endpointPosition1.LocalGridPosition.ToString() + " <-> " + m_endpointPosition2.LocalGridPosition.ToString();
		}

		public MyConveyorLine RemovePortion(Vector3I startPosition, Vector3I endPosition)
		{
			if (IsCircular)
			{
				RotateCircularLine(startPosition);
			}
			if (startPosition != endPosition)
			{
				bool flag = false;
				if (m_sections != null)
				{
					Vector3I localGridPosition = m_endpointPosition1.LocalGridPosition;
					foreach (SectionInformation section in m_sections)
					{
						int sectionLength;
						bool flag2 = PositionIsInSection(startPosition, localGridPosition, section, out sectionLength);
						int sectionLength2;
						bool flag3 = PositionIsInSection(endPosition, localGridPosition, section, out sectionLength2);
						if (flag2 && flag3)
						{
							if (sectionLength2 < sectionLength)
							{
								flag = true;
							}
							break;
						}
						if (flag3)
						{
							flag = true;
							break;
						}
						if (flag2)
						{
							break;
						}
						localGridPosition += Base6Directions.GetIntVector(section.Direction) * section.Length;
					}
				}
				else if (Vector3I.DistanceManhattan(m_endpointPosition1.LocalGridPosition, endPosition) < Vector3I.DistanceManhattan(m_endpointPosition1.LocalGridPosition, startPosition))
				{
					flag = true;
				}
				if (flag)
				{
					Vector3I vector3I = startPosition;
					startPosition = endPosition;
					endPosition = vector3I;
				}
			}
			List<SectionInformation> list = null;
			List<SectionInformation> list2 = null;
			ConveyorLinePosition newPosition = new ConveyorLinePosition(startPosition, m_endpointPosition2.Direction);
			ConveyorLinePosition newPosition2 = new ConveyorLinePosition(endPosition, m_endpointPosition1.Direction);
			ConveyorLinePosition newPosition3 = default(ConveyorLinePosition);
			int line1Length = 0;
			int num = 0;
			if (m_sections != null)
			{
				m_tmpSections1.Clear();
				m_tmpSections2.Clear();
				SplitSections(m_sections, Length, m_endpointPosition1.LocalGridPosition, startPosition, m_tmpSections1, m_tmpSections2, out newPosition, out newPosition2, out line1Length);
				num = Length - line1Length;
				if (m_tmpSections1.Count > 1)
				{
					list = new List<SectionInformation>();
					list.AddRange(m_tmpSections1);
				}
				if (startPosition != endPosition)
				{
					m_tmpSections1.Clear();
					SplitSections(m_tmpSections2, num, newPosition2.LocalGridPosition, endPosition, null, m_tmpSections1, out newPosition3, out newPosition2, out int line1Length2);
					num -= line1Length2;
					if (m_tmpSections1.Count > 1)
					{
						list2 = new List<SectionInformation>();
						list2.AddRange(m_tmpSections1);
					}
				}
				else if (m_tmpSections2.Count > 1)
				{
					list2 = new List<SectionInformation>();
					list2.AddRange(m_tmpSections2);
				}
				m_tmpSections1.Clear();
				m_tmpSections2.Clear();
			}
			else
			{
				line1Length = startPosition.RectangularDistance(m_endpointPosition1.LocalGridPosition);
				num = endPosition.RectangularDistance(m_endpointPosition2.LocalGridPosition);
			}
			MyConveyorLine myConveyorLine = null;
			if (line1Length <= 1 || line1Length < num)
			{
				if (line1Length > 1 || (line1Length > 0 && m_endpoint1 != null))
				{
					myConveyorLine = new MyConveyorLine();
					myConveyorLine.InitAfterSplit(m_endpointPosition1, newPosition, list, line1Length, m_cubeGrid, m_type);
					myConveyorLine.InitEndpoints(m_endpoint1, null);
				}
				InitAfterSplit(newPosition2, m_endpointPosition2, list2, num, m_cubeGrid, m_type);
				InitEndpoints(null, m_endpoint2);
			}
			else
			{
				if (num > 1 || (num > 0 && m_endpoint2 != null))
				{
					myConveyorLine = new MyConveyorLine();
					myConveyorLine.InitAfterSplit(newPosition2, m_endpointPosition2, list2, num, m_cubeGrid, m_type);
					myConveyorLine.InitEndpoints(null, m_endpoint2);
				}
				InitAfterSplit(m_endpointPosition1, newPosition, list, line1Length, m_cubeGrid, m_type);
				InitEndpoints(m_endpoint1, null);
			}
			RecalculateConductivity();
			myConveyorLine?.RecalculateConductivity();
			return myConveyorLine;
		}

		private static void SplitSections(List<SectionInformation> sections, int lengthLimit, Vector3I startPosition, Vector3I splittingPosition, List<SectionInformation> sections1, List<SectionInformation> sections2, out ConveyorLinePosition newPosition1, out ConveyorLinePosition newPosition2, out int line1Length)
		{
			bool flag = false;
			int num = 0;
			line1Length = 0;
			Vector3I sectionStart = startPosition;
			SectionInformation section = default(SectionInformation);
			int sectionLength = 0;
			for (num = 0; num < sections.Count; num++)
			{
				section = sections[num];
				if (PositionIsInSection(splittingPosition, sectionStart, section, out sectionLength))
				{
					line1Length += sectionLength;
					if (sectionLength == 0)
					{
						flag = true;
					}
					break;
				}
				line1Length += section.Length;
				sectionStart += Base6Directions.GetIntVector(section.Direction) * section.Length;
			}
			newPosition2 = new ConveyorLinePosition(splittingPosition, section.Direction);
			if (flag)
			{
				newPosition1 = new ConveyorLinePosition(splittingPosition, Base6Directions.GetFlippedDirection(sections[num - 1].Direction));
			}
			else
			{
				newPosition1 = new ConveyorLinePosition(splittingPosition, Base6Directions.GetFlippedDirection(section.Direction));
			}
			int num2 = flag ? num : (num + 1);
			int num3 = sections.Count - num;
			SectionInformation item = default(SectionInformation);
			if (sections1 != null)
			{
				for (int i = 0; i < num2 - 1; i++)
				{
					sections1.Add(sections[i]);
				}
				if (flag)
				{
					sections1.Add(sections[num2 - 1]);
				}
				else
				{
					item.Direction = sections[num2 - 1].Direction;
					item.Length = sectionLength;
					sections1.Add(item);
				}
			}
			item.Direction = sections[num].Direction;
			item.Length = sections[num].Length - sectionLength;
			sections2.Add(item);
			for (int j = 1; j < num3; j++)
			{
				sections2.Add(sections[num + j]);
			}
		}

		private static bool PositionIsInSection(Vector3I position, Vector3I sectionStart, SectionInformation section, out int sectionLength)
		{
			sectionLength = 0;
			Vector3I intVector = Base6Directions.GetIntVector(section.Direction);
			Vector3I vector3I = position - sectionStart;
			switch (Base6Directions.GetAxis(section.Direction))
			{
			case Base6Directions.Axis.ForwardBackward:
				sectionLength = intVector.Z * vector3I.Z;
				break;
			case Base6Directions.Axis.LeftRight:
				sectionLength = intVector.X * vector3I.X;
				break;
			case Base6Directions.Axis.UpDown:
				sectionLength = intVector.Y * vector3I.Y;
				break;
			}
			if (sectionLength >= 0 && sectionLength < section.Length && vector3I.RectangularLength() == sectionLength)
			{
				return true;
			}
			return false;
		}

		private void RotateCircularLine(Vector3I position)
		{
			List<SectionInformation> list = new List<SectionInformation>(m_sections.Count + 1);
			Vector3I localGridPosition = m_endpointPosition1.LocalGridPosition;
			for (int i = 0; i < m_sections.Count; i++)
			{
				SectionInformation sectionInformation = m_sections[i];
				int num = 0;
				Vector3I intVector = Base6Directions.GetIntVector(sectionInformation.Direction);
				Vector3I vector3I = position - localGridPosition;
				switch (Base6Directions.GetAxis(sectionInformation.Direction))
				{
				case Base6Directions.Axis.ForwardBackward:
					num = intVector.Z * vector3I.Z;
					break;
				case Base6Directions.Axis.LeftRight:
					num = intVector.X * vector3I.X;
					break;
				case Base6Directions.Axis.UpDown:
					num = intVector.Y * vector3I.Y;
					break;
				}
				if (num > 0 && num <= sectionInformation.Length && vector3I.RectangularLength() == num)
				{
					SectionInformation item = default(SectionInformation);
					item.Direction = m_sections[i].Direction;
					item.Length = m_sections[i].Length - num + 1;
					list.Add(item);
					for (int j = i + 1; j < m_sections.Count - 1; j++)
					{
						list.Add(m_sections[j]);
					}
					SectionInformation item2 = default(SectionInformation);
					item2.Direction = m_sections[0].Direction;
					item2.Length = m_sections[0].Length + m_sections[m_sections.Count - 1].Length - 1;
					list.Add(item2);
					for (int k = 1; k < i; k++)
					{
						list.Add(m_sections[k]);
					}
					SectionInformation item3 = default(SectionInformation);
					item3.Direction = m_sections[i].Direction;
					item3.Length = num;
					list.Add(item3);
					break;
				}
				localGridPosition += Base6Directions.GetIntVector(sectionInformation.Direction) * sectionInformation.Length;
			}
			m_sections = list;
			m_endpointPosition2 = new ConveyorLinePosition(position, Base6Directions.GetFlippedDirection(m_sections[0].Direction));
			m_endpointPosition1 = m_endpointPosition2.GetConnectingPosition();
		}

		private MyCubeGrid GetGrid()
		{
			if (m_endpoint1 == null || m_endpoint2 == null)
			{
				return null;
			}
			return m_endpoint1.CubeBlock.CubeGrid;
		}

		public void StopQueuesIfNeeded()
		{
			if (m_queuePosition == 0f)
			{
				if (!m_stopped1 && m_queue1.Count != 0 && m_queue1.First().LinePosition >= Length - 1)
				{
					m_stopped1 = true;
				}
				if (!m_stopped2 && m_queue2.Count != 0 && m_queue2.First().LinePosition >= Length - 1)
				{
					m_stopped2 = true;
				}
			}
		}

		public void Update()
		{
			m_queuePosition += 0.015625f;
			if (m_queuePosition >= 1f)
			{
				BigUpdate();
			}
		}

		public void BigUpdate()
		{
			StopQueuesIfNeeded();
			if (!m_stopped1)
			{
				foreach (MyConveyorPacket item in m_queue1)
				{
					item.LinePosition++;
				}
			}
			if (!m_stopped2)
			{
				foreach (MyConveyorPacket item2 in m_queue2)
				{
					item2.LinePosition++;
				}
			}
			if (!m_isWorking)
			{
				m_stopped1 = true;
				m_stopped2 = true;
			}
			m_queuePosition = 0f;
			if (!m_stopped1 || !m_stopped2)
			{
				RecalculatePacketPositions();
			}
		}

		public void UpdateIsFunctional()
		{
			m_isFunctional = UpdateIsFunctionalInternal();
			UpdateIsWorking();
		}

		public MyResourceStateEnum UpdateIsWorking()
		{
			MyResourceStateEnum myResourceStateEnum = MyResourceStateEnum.NoPower;
			if (!m_isFunctional)
			{
				m_isWorking = false;
				return myResourceStateEnum;
			}
			if (IsDisconnected)
			{
				m_isWorking = false;
				return myResourceStateEnum;
			}
			if (MySession.Static == null)
			{
				m_isWorking = false;
				return myResourceStateEnum;
			}
			MyCubeGrid grid = GetGrid();
			if (grid.GridSystems.ResourceDistributor != null)
			{
				myResourceStateEnum = grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
				bool flag = myResourceStateEnum != MyResourceStateEnum.NoPower;
				if (m_isWorking != flag)
				{
					m_isWorking = flag;
					grid.GridSystems.ConveyorSystem.FlagForRecomputation();
				}
			}
			return myResourceStateEnum;
		}

		private bool UpdateIsFunctionalInternal()
		{
			if (m_endpoint1 == null || m_endpoint2 == null || !m_endpoint1.CubeBlock.IsFunctional || !m_endpoint2.CubeBlock.IsFunctional)
			{
				return false;
			}
			MyCubeGrid cubeGrid = m_endpoint1.CubeBlock.CubeGrid;
			using (IEnumerator<Vector3I> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector3I current = enumerator.Current;
					MySlimBlock cubeBlock = cubeGrid.GetCubeBlock(current);
					if (cubeBlock != null && cubeBlock.FatBlock != null && !cubeBlock.FatBlock.IsFunctional)
					{
						return false;
					}
				}
			}
			return true;
		}

		public void PrepareForDraw(MyCubeGrid grid)
		{
			if (m_queue1.Count != 0 || m_queue2.Count != 0)
			{
				if (!m_stopped1)
				{
					foreach (MyConveyorPacket item in m_queue1)
					{
						item.MoveRelative(0.015625f);
					}
				}
				if (!m_stopped2)
				{
					foreach (MyConveyorPacket item2 in m_queue2)
					{
						item2.MoveRelative(0.015625f);
					}
				}
			}
		}

		public void DebugDraw(MyCubeGrid grid)
		{
			Vector3 position = new Vector3(m_endpointPosition1.LocalGridPosition) * grid.GridSize;
			Vector3 position2 = new Vector3(m_endpointPosition2.LocalGridPosition) * grid.GridSize;
			Vector3 vector = Vector3.Transform(position, grid.WorldMatrix);
			position2 = Vector3.Transform(position2, grid.WorldMatrix);
			string str = (m_endpoint1 == null) ? "- " : "# ";
			str += m_length;
			str += " ";
			str += m_type;
			str += ((m_endpoint2 == null) ? " -" : " #");
			str += " ";
			MyRenderProxy.DebugDrawText3D(text: str + m_conductivity, worldCoord: (vector + position2) * 0.5f, color: Color.Blue, scale: 1f, depthRead: false);
			Color color = IsFunctional ? Color.Green : Color.Red;
			MyRenderProxy.DebugDrawLine3D(vector, position2, color, color, depthRead: false);
		}

		public void DebugDrawPackets()
		{
			foreach (MyConveyorPacket item in m_queue1)
			{
				MyRenderProxy.DebugDrawSphere(item.WorldMatrix.Translation, 0.2f, Color.Red.ToVector3(), 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(item.WorldMatrix.Translation, item.LinePosition.ToString(), Color.White, 1f, depthRead: false);
			}
			foreach (MyConveyorPacket item2 in m_queue2)
			{
				MyRenderProxy.DebugDrawSphere(item2.WorldMatrix.Translation, 0.2f, Color.Red.ToVector3(), 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(item2.WorldMatrix.Translation, item2.LinePosition.ToString(), Color.White, 1f, depthRead: false);
			}
		}
	}
}
