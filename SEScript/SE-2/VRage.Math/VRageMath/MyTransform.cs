using VRage.Serialization;

namespace VRageMath
{
	public struct MyTransform
	{
		[Serialize(MyPrimitiveFlags.Normalized)]
		public Quaternion Rotation;

		public Vector3 Position;

		public Matrix TransformMatrix
		{
			get
			{
				Matrix result = Matrix.CreateFromQuaternion(Rotation);
				result.Translation = Position;
				return result;
			}
		}

		public MyTransform(Vector3 position)
			: this(ref position)
		{
		}

		public MyTransform(Matrix matrix)
			: this(ref matrix)
		{
		}

		public MyTransform(ref Vector3 position)
		{
			Rotation = Quaternion.Identity;
			Position = position;
		}

		public MyTransform(ref Matrix matrix)
		{
			Quaternion.CreateFromRotationMatrix(ref matrix, out Rotation);
			Position = matrix.Translation;
		}

		public static MyTransform Transform(ref MyTransform t1, ref MyTransform t2)
		{
			Transform(ref t1, ref t2, out MyTransform result);
			return result;
		}

		public static void Transform(ref MyTransform t1, ref MyTransform t2, out MyTransform result)
		{
			Vector3.Transform(ref t1.Position, ref t2.Rotation, out Vector3 result2);
			result2 += t2.Position;
			Quaternion.Multiply(ref t1.Rotation, ref t2.Rotation, out Quaternion result3);
			result.Position = result2;
			result.Rotation = result3;
		}

		public static Vector3 Transform(ref Vector3 v, ref MyTransform t2)
		{
			Transform(ref v, ref t2, out Vector3 result);
			return result;
		}

		public static void Transform(ref Vector3 v, ref MyTransform t2, out Vector3 result)
		{
			Vector3.Transform(ref v, ref t2.Rotation, out result);
			result += t2.Position;
		}
	}
}
