using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public class MyConveyorSegment
	{
		public MyConveyorLine ConveyorLine
		{
			get;
			private set;
		}

		public ConveyorLinePosition ConnectingPosition1
		{
			get;
			private set;
		}

		public ConveyorLinePosition ConnectingPosition2
		{
			get;
			private set;
		}

		public MyCubeBlock CubeBlock
		{
			get;
			private set;
		}

		public bool IsCorner
		{
			get
			{
				Vector3I vector = ConnectingPosition1.VectorDirection;
				Vector3I vector2 = ConnectingPosition2.VectorDirection;
				return Vector3I.Dot(ref vector, ref vector2) != -1;
			}
		}

		public void Init(MyCubeBlock myBlock, ConveyorLinePosition a, ConveyorLinePosition b, MyObjectBuilder_ConveyorLine.LineType type, MyObjectBuilder_ConveyorLine.LineConductivity conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL)
		{
			CubeBlock = myBlock;
			ConnectingPosition1 = a;
			ConnectingPosition2 = b;
			Vector3I neighbourGridPosition = (myBlock as IMyConveyorSegmentBlock).ConveyorSegment.ConnectingPosition1.NeighbourGridPosition;
			ConveyorLine = myBlock.CubeGrid.GridSystems.ConveyorSystem.GetDeserializingLine(neighbourGridPosition);
			if (ConveyorLine == null)
			{
				ConveyorLine = new MyConveyorLine();
				if (IsCorner)
				{
					ConveyorLine.Init(a, b, myBlock.CubeGrid, type, conductivity, CalculateCornerPosition());
				}
				else
				{
					ConveyorLine.Init(a, b, myBlock.CubeGrid, type, conductivity);
				}
			}
			myBlock.SlimBlock.ComponentStack.IsFunctionalChanged += CubeBlock_IsFunctionalChanged;
		}

		public void SetConveyorLine(MyConveyorLine newLine)
		{
			ConveyorLine = newLine;
		}

		public bool CanConnectTo(ConveyorLinePosition connectingPosition, MyObjectBuilder_ConveyorLine.LineType type)
		{
			if (type == ConveyorLine.Type && (connectingPosition.Equals(ConnectingPosition1.GetConnectingPosition()) || connectingPosition.Equals(ConnectingPosition2.GetConnectingPosition())))
			{
				return true;
			}
			return false;
		}

		private void CubeBlock_IsFunctionalChanged()
		{
			ConveyorLine.UpdateIsFunctional();
		}

		public Base6Directions.Direction CalculateConnectingDirection(Vector3I connectingPosition)
		{
			Vector3 value = new Vector3(CubeBlock.Max - CubeBlock.Min + Vector3I.One) * 0.5f;
			Vector3 value2 = new Vector3(CubeBlock.Max + CubeBlock.Min) * 0.5f;
			value2 -= (Vector3)connectingPosition;
			value2 = Vector3.Multiply(value2, value);
			value2 = Vector3.DominantAxisProjection(value2);
			value2.Normalize();
			return Base6Directions.GetDirection(value2);
		}

		private Vector3I CalculateCornerPosition()
		{
			Vector3I vector3I = ConnectingPosition2.LocalGridPosition - ConnectingPosition1.LocalGridPosition;
			switch (Base6Directions.GetAxis(ConnectingPosition1.Direction))
			{
			case Base6Directions.Axis.ForwardBackward:
				return ConnectingPosition1.LocalGridPosition + new Vector3I(0, 0, vector3I.Z);
			case Base6Directions.Axis.LeftRight:
				return ConnectingPosition1.LocalGridPosition + new Vector3I(vector3I.X, 0, 0);
			case Base6Directions.Axis.UpDown:
				return ConnectingPosition1.LocalGridPosition + new Vector3I(0, vector3I.Y, 0);
			default:
				return Vector3I.Zero;
			}
		}

		public void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
			{
				Vector3 position = (ConnectingPosition1.LocalGridPosition + ConnectingPosition1.VectorDirection * 0.5f) * CubeBlock.CubeGrid.GridSize;
				Vector3 position2 = (ConnectingPosition2.LocalGridPosition + ConnectingPosition2.VectorDirection * 0.5f) * CubeBlock.CubeGrid.GridSize;
				position = Vector3.Transform(position, CubeBlock.CubeGrid.WorldMatrix);
				position2 = Vector3.Transform(position2, CubeBlock.CubeGrid.WorldMatrix);
				Color color = ConveyorLine.IsFunctional ? Color.Orange : Color.DarkRed;
				color = (ConveyorLine.IsWorking ? Color.GreenYellow : color);
				MyRenderProxy.DebugDrawLine3D(position, position2, color, color, depthRead: false);
				if (ConveyorLine != null && MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS)
				{
					MyRenderProxy.DebugDrawText3D((position + position2) * 0.5f, ConveyorLine.GetHashCode().ToString(), color, 0.5f, depthRead: false);
				}
			}
		}
	}
}
