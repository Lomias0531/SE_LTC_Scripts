using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[Serializable]
	public struct MatrixI
	{
		protected class VRageMath_MatrixI_003C_003ERight_003C_003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Base6Directions.Direction value)
			{
				owner.Right = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Base6Directions.Direction value)
			{
				value = owner.Right;
			}
		}

		protected class VRageMath_MatrixI_003C_003EUp_003C_003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Base6Directions.Direction value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Base6Directions.Direction value)
			{
				value = owner.Up;
			}
		}

		protected class VRageMath_MatrixI_003C_003EBackward_003C_003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Base6Directions.Direction value)
			{
				owner.Backward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Base6Directions.Direction value)
			{
				value = owner.Backward;
			}
		}

		protected class VRageMath_MatrixI_003C_003ETranslation_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.Translation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.Translation;
			}
		}

		protected class VRageMath_MatrixI_003C_003ELeft_003C_003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Base6Directions.Direction value)
			{
				owner.Left = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Base6Directions.Direction value)
			{
				value = owner.Left;
			}
		}

		protected class VRageMath_MatrixI_003C_003EDown_003C_003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Base6Directions.Direction value)
			{
				owner.Down = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Base6Directions.Direction value)
			{
				value = owner.Down;
			}
		}

		protected class VRageMath_MatrixI_003C_003EForward_003C_003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Base6Directions.Direction value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Base6Directions.Direction value)
			{
				value = owner.Forward;
			}
		}

		protected class VRageMath_MatrixI_003C_003ERightVector_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.RightVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.RightVector;
			}
		}

		protected class VRageMath_MatrixI_003C_003ELeftVector_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.LeftVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.LeftVector;
			}
		}

		protected class VRageMath_MatrixI_003C_003EUpVector_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.UpVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.UpVector;
			}
		}

		protected class VRageMath_MatrixI_003C_003EDownVector_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.DownVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.DownVector;
			}
		}

		protected class VRageMath_MatrixI_003C_003EBackwardVector_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.BackwardVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.BackwardVector;
			}
		}

		protected class VRageMath_MatrixI_003C_003EForwardVector_003C_003EAccessor : IMemberAccessor<MatrixI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixI owner, in Vector3I value)
			{
				owner.ForwardVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixI owner, out Vector3I value)
			{
				value = owner.ForwardVector;
			}
		}

		public Base6Directions.Direction Right;

		public Base6Directions.Direction Up;

		public Base6Directions.Direction Backward;

		public Vector3I Translation;

		public Base6Directions.Direction Left
		{
			get
			{
				return Base6Directions.GetFlippedDirection(Right);
			}
			set
			{
				Right = Base6Directions.GetFlippedDirection(value);
			}
		}

		public Base6Directions.Direction Down
		{
			get
			{
				return Base6Directions.GetFlippedDirection(Up);
			}
			set
			{
				Up = Base6Directions.GetFlippedDirection(value);
			}
		}

		public Base6Directions.Direction Forward
		{
			get
			{
				return Base6Directions.GetFlippedDirection(Backward);
			}
			set
			{
				Backward = Base6Directions.GetFlippedDirection(value);
			}
		}

		public Vector3I RightVector
		{
			get
			{
				return Base6Directions.GetIntVector(Right);
			}
			set
			{
				Right = Base6Directions.GetDirection(value);
			}
		}

		public Vector3I LeftVector
		{
			get
			{
				return Base6Directions.GetIntVector(Left);
			}
			set
			{
				Left = Base6Directions.GetDirection(value);
			}
		}

		public Vector3I UpVector
		{
			get
			{
				return Base6Directions.GetIntVector(Up);
			}
			set
			{
				Up = Base6Directions.GetDirection(value);
			}
		}

		public Vector3I DownVector
		{
			get
			{
				return Base6Directions.GetIntVector(Down);
			}
			set
			{
				Down = Base6Directions.GetDirection(value);
			}
		}

		public Vector3I BackwardVector
		{
			get
			{
				return Base6Directions.GetIntVector(Backward);
			}
			set
			{
				Backward = Base6Directions.GetDirection(value);
			}
		}

		public Vector3I ForwardVector
		{
			get
			{
				return Base6Directions.GetIntVector(Forward);
			}
			set
			{
				Forward = Base6Directions.GetDirection(value);
			}
		}

		public Base6Directions.Direction GetDirection(Base6Directions.Direction direction)
		{
			switch (direction)
			{
			case Base6Directions.Direction.Right:
				return Right;
			case Base6Directions.Direction.Left:
				return Left;
			case Base6Directions.Direction.Up:
				return Up;
			case Base6Directions.Direction.Down:
				return Down;
			case Base6Directions.Direction.Backward:
				return Backward;
			default:
				return Forward;
			}
		}

		public void SetDirection(Base6Directions.Direction dirToSet, Base6Directions.Direction newDirection)
		{
			switch (dirToSet)
			{
			case Base6Directions.Direction.Right:
				Right = newDirection;
				break;
			case Base6Directions.Direction.Left:
				Left = newDirection;
				break;
			case Base6Directions.Direction.Up:
				Up = newDirection;
				break;
			case Base6Directions.Direction.Down:
				Down = newDirection;
				break;
			case Base6Directions.Direction.Backward:
				Backward = newDirection;
				break;
			case Base6Directions.Direction.Forward:
				Forward = newDirection;
				break;
			}
		}

		public MatrixI(ref Vector3I position, Base6Directions.Direction forward, Base6Directions.Direction up)
		{
			Translation = position;
			Right = Base6Directions.GetFlippedDirection(Base6Directions.GetLeft(up, forward));
			Up = up;
			Backward = Base6Directions.GetFlippedDirection(forward);
		}

		public MatrixI(Vector3I position, Base6Directions.Direction forward, Base6Directions.Direction up)
		{
			Translation = position;
			Right = Base6Directions.GetFlippedDirection(Base6Directions.GetLeft(up, forward));
			Up = up;
			Backward = Base6Directions.GetFlippedDirection(forward);
		}

		public MatrixI(Base6Directions.Direction forward, Base6Directions.Direction up)
			: this(Vector3I.Zero, forward, up)
		{
		}

		public MatrixI(ref Vector3I position, ref Vector3I forward, ref Vector3I up)
			: this(ref position, Base6Directions.GetDirection(ref forward), Base6Directions.GetDirection(ref up))
		{
		}

		public MatrixI(ref Vector3I position, ref Vector3 forward, ref Vector3 up)
			: this(ref position, Base6Directions.GetDirection(ref forward), Base6Directions.GetDirection(ref up))
		{
		}

		public MatrixI(MyBlockOrientation orientation)
			: this(Vector3I.Zero, orientation.Forward, orientation.Up)
		{
		}

		public MyBlockOrientation GetBlockOrientation()
		{
			return new MyBlockOrientation(Forward, Up);
		}

		public Matrix GetFloatMatrix()
		{
			return Matrix.CreateWorld(new Vector3(Translation), Base6Directions.GetVector(Forward), Base6Directions.GetVector(Up));
		}

		public static MatrixI CreateRotation(Base6Directions.Direction oldA, Base6Directions.Direction oldB, Base6Directions.Direction newA, Base6Directions.Direction newB)
		{
			MatrixI result = default(MatrixI);
			result.Translation = Vector3I.Zero;
			Base6Directions.Direction cross = Base6Directions.GetCross(oldA, oldB);
			Base6Directions.Direction cross2 = Base6Directions.GetCross(newA, newB);
			result.SetDirection(oldA, newA);
			result.SetDirection(oldB, newB);
			result.SetDirection(cross, cross2);
			return result;
		}

		public static void Invert(ref MatrixI matrix, out MatrixI result)
		{
			result = default(MatrixI);
			switch (matrix.Right)
			{
			case Base6Directions.Direction.Up:
				result.Up = Base6Directions.Direction.Right;
				break;
			case Base6Directions.Direction.Down:
				result.Up = Base6Directions.Direction.Left;
				break;
			case Base6Directions.Direction.Backward:
				result.Backward = Base6Directions.Direction.Right;
				break;
			case Base6Directions.Direction.Forward:
				result.Backward = Base6Directions.Direction.Left;
				break;
			default:
				result.Right = matrix.Right;
				break;
			}
			switch (matrix.Up)
			{
			case Base6Directions.Direction.Right:
				result.Right = Base6Directions.Direction.Up;
				break;
			case Base6Directions.Direction.Left:
				result.Right = Base6Directions.Direction.Down;
				break;
			case Base6Directions.Direction.Backward:
				result.Backward = Base6Directions.Direction.Up;
				break;
			case Base6Directions.Direction.Forward:
				result.Backward = Base6Directions.Direction.Down;
				break;
			default:
				result.Up = matrix.Up;
				break;
			}
			switch (matrix.Backward)
			{
			case Base6Directions.Direction.Right:
				result.Right = Base6Directions.Direction.Backward;
				break;
			case Base6Directions.Direction.Left:
				result.Right = Base6Directions.Direction.Forward;
				break;
			case Base6Directions.Direction.Up:
				result.Up = Base6Directions.Direction.Backward;
				break;
			case Base6Directions.Direction.Down:
				result.Up = Base6Directions.Direction.Forward;
				break;
			default:
				result.Backward = matrix.Backward;
				break;
			}
			Vector3I.TransformNormal(ref matrix.Translation, ref result, out result.Translation);
			result.Translation = -result.Translation;
		}

		public static void Multiply(ref MatrixI leftMatrix, ref MatrixI rightMatrix, out MatrixI result)
		{
			result = default(MatrixI);
			Vector3I normal = leftMatrix.RightVector;
			Vector3I normal2 = leftMatrix.UpVector;
			Vector3I normal3 = leftMatrix.BackwardVector;
			Vector3I.TransformNormal(ref normal, ref rightMatrix, out Vector3I result2);
			Vector3I.TransformNormal(ref normal2, ref rightMatrix, out Vector3I result3);
			Vector3I.TransformNormal(ref normal3, ref rightMatrix, out Vector3I result4);
			Vector3I.Transform(ref leftMatrix.Translation, ref rightMatrix, out result.Translation);
			result.RightVector = result2;
			result.UpVector = result3;
			result.BackwardVector = result4;
		}

		public static MyBlockOrientation Transform(ref MyBlockOrientation orientation, ref MatrixI transform)
		{
			Base6Directions.Direction direction = transform.GetDirection(orientation.Forward);
			Base6Directions.Direction direction2 = transform.GetDirection(orientation.Up);
			return new MyBlockOrientation(direction, direction2);
		}
	}
}
