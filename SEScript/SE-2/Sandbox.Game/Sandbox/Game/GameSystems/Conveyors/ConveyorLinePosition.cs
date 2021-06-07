using System;
using VRageMath;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public struct ConveyorLinePosition : IEquatable<ConveyorLinePosition>
	{
		public Vector3I LocalGridPosition;

		public Base6Directions.Direction Direction;

		public Vector3I VectorDirection => Base6Directions.GetIntVector(Direction);

		public Vector3I NeighbourGridPosition => LocalGridPosition + Base6Directions.GetIntVector(Direction);

		public ConveyorLinePosition(Vector3I gridPosition, Base6Directions.Direction direction)
		{
			LocalGridPosition = gridPosition;
			Direction = direction;
		}

		public ConveyorLinePosition GetConnectingPosition()
		{
			return new ConveyorLinePosition(LocalGridPosition + VectorDirection, Base6Directions.GetFlippedDirection(Direction));
		}

		public ConveyorLinePosition GetFlippedPosition()
		{
			return new ConveyorLinePosition(LocalGridPosition, Base6Directions.GetFlippedDirection(Direction));
		}

		public bool Equals(ConveyorLinePosition other)
		{
			if (LocalGridPosition == other.LocalGridPosition)
			{
				return Direction == other.Direction;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((((((int)Direction * 397) ^ LocalGridPosition.X) * 397) ^ LocalGridPosition.Y) * 397) ^ LocalGridPosition.Z;
		}

		public override string ToString()
		{
			return LocalGridPosition.ToString() + " -> " + Direction;
		}
	}
}
