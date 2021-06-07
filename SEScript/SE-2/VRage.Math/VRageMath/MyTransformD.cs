using VRage.Serialization;

namespace VRageMath
{
	public struct MyTransformD
	{
		[Serialize(MyPrimitiveFlags.Normalized)]
		public Quaternion Rotation;

		public Vector3D Position;

		public MatrixD TransformMatrix
		{
			get
			{
				MatrixD result = MatrixD.CreateFromQuaternion(Rotation);
				result.Translation = Position;
				return result;
			}
		}

		public MyTransformD(Vector3D position)
			: this(ref position)
		{
		}

		public MyTransformD(MatrixD matrix)
			: this(ref matrix)
		{
		}

		public MyTransformD(ref Vector3D position)
		{
			Rotation = Quaternion.Identity;
			Position = position;
		}

		public MyTransformD(ref MatrixD matrix)
		{
			Quaternion.CreateFromRotationMatrix(ref matrix, out Rotation);
			Position = matrix.Translation;
		}

		public static MyTransformD Transform(ref MyTransformD t1, ref MyTransformD t2)
		{
			Transform(ref t1, ref t2, out MyTransformD result);
			return result;
		}

		public static void Transform(ref MyTransformD t1, ref MyTransformD t2, out MyTransformD result)
		{
			Vector3D.Transform(ref t1.Position, ref t2.Rotation, out Vector3D result2);
			result2 += t2.Position;
			Quaternion.Multiply(ref t1.Rotation, ref t2.Rotation, out Quaternion result3);
			result.Position = result2;
			result.Rotation = result3;
		}

		public static Vector3D Transform(ref Vector3D v, ref MyTransformD t2)
		{
			Transform(ref v, ref t2, out Vector3D result);
			return result;
		}

		public static void Transform(ref Vector3D v, ref MyTransformD t2, out Vector3D result)
		{
			Vector3D.Transform(ref v, ref t2.Rotation, out result);
			result += t2.Position;
		}
	}
}
