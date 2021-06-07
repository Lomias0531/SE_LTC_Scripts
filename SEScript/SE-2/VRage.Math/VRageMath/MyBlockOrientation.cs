using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct MyBlockOrientation
	{
		protected class VRageMath_MyBlockOrientation_003C_003EForward_003C_003EAccessor : IMemberAccessor<MyBlockOrientation, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBlockOrientation owner, in Base6Directions.Direction value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBlockOrientation owner, out Base6Directions.Direction value)
			{
				value = owner.Forward;
			}
		}

		protected class VRageMath_MyBlockOrientation_003C_003EUp_003C_003EAccessor : IMemberAccessor<MyBlockOrientation, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyBlockOrientation owner, in Base6Directions.Direction value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyBlockOrientation owner, out Base6Directions.Direction value)
			{
				value = owner.Up;
			}
		}

		public static readonly MyBlockOrientation Identity = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

		[ProtoMember(1)]
		public Base6Directions.Direction Forward;

		[ProtoMember(4)]
		public Base6Directions.Direction Up;

		public Base6Directions.Direction Left => Base6Directions.GetLeft(Up, Forward);

		public bool IsValid => Base6Directions.IsValidBlockOrientation(Forward, Up);

		public MyBlockOrientation(Base6Directions.Direction forward, Base6Directions.Direction up)
		{
			Forward = forward;
			Up = up;
		}

		public MyBlockOrientation(ref Quaternion q)
		{
			Forward = Base6Directions.GetForward(q);
			Up = Base6Directions.GetUp(q);
		}

		public MyBlockOrientation(ref Matrix m)
		{
			Forward = Base6Directions.GetForward(ref m);
			Up = Base6Directions.GetUp(ref m);
		}

		public void GetQuaternion(out Quaternion result)
		{
			GetMatrix(out Matrix result2);
			Quaternion.CreateFromRotationMatrix(ref result2, out result);
		}

		public void GetMatrix(out Matrix result)
		{
			Base6Directions.GetVector(Forward, out Vector3 result2);
			Base6Directions.GetVector(Up, out Vector3 result3);
			Matrix.CreateWorld(ref Vector3.Zero, ref result2, ref result3, out result);
		}

		public override int GetHashCode()
		{
			return (int)((uint)Forward << 16) | (int)Up;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				MyBlockOrientation? myBlockOrientation = obj as MyBlockOrientation?;
				if (myBlockOrientation.HasValue)
				{
					return this == myBlockOrientation.Value;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return $"[Forward:{Forward}, Up:{Up}]";
		}

		/// <summary>
		/// Returns the direction baseDirection will point to after transformation
		/// </summary>
		public Base6Directions.Direction TransformDirection(Base6Directions.Direction baseDirection)
		{
			Base6Directions.Axis axis = Base6Directions.GetAxis(baseDirection);
			int num = (int)baseDirection % 2;
			switch (axis)
			{
			case Base6Directions.Axis.ForwardBackward:
				if (num != 1)
				{
					return Forward;
				}
				return Base6Directions.GetFlippedDirection(Forward);
			case Base6Directions.Axis.LeftRight:
				if (num != 1)
				{
					return Left;
				}
				return Base6Directions.GetFlippedDirection(Left);
			default:
				if (num != 1)
				{
					return Up;
				}
				return Base6Directions.GetFlippedDirection(Up);
			}
		}

		/// <summary>
		/// Returns the direction that this orientation transforms to baseDirection
		/// </summary>
		public Base6Directions.Direction TransformDirectionInverse(Base6Directions.Direction baseDirection)
		{
			Base6Directions.Axis axis = Base6Directions.GetAxis(baseDirection);
			if (axis == Base6Directions.GetAxis(Forward))
			{
				if (baseDirection != Forward)
				{
					return Base6Directions.Direction.Backward;
				}
				return Base6Directions.Direction.Forward;
			}
			if (axis == Base6Directions.GetAxis(Left))
			{
				if (baseDirection != Left)
				{
					return Base6Directions.Direction.Right;
				}
				return Base6Directions.Direction.Left;
			}
			if (baseDirection != Up)
			{
				return Base6Directions.Direction.Down;
			}
			return Base6Directions.Direction.Up;
		}

		public static bool operator ==(MyBlockOrientation orientation1, MyBlockOrientation orientation2)
		{
			if (orientation1.Forward == orientation2.Forward)
			{
				return orientation1.Up == orientation2.Up;
			}
			return false;
		}

		public static bool operator !=(MyBlockOrientation orientation1, MyBlockOrientation orientation2)
		{
			if (orientation1.Forward == orientation2.Forward)
			{
				return orientation1.Up != orientation2.Up;
			}
			return true;
		}
	}
}
