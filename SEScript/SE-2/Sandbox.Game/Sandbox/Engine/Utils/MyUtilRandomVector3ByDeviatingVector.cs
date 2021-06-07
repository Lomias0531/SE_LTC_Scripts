using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	internal struct MyUtilRandomVector3ByDeviatingVector
	{
		private Matrix m_matrix;

		public MyUtilRandomVector3ByDeviatingVector(Vector3 originalVector)
		{
			m_matrix = Matrix.CreateFromDir(originalVector);
		}

		public Vector3 GetNext(float maxAngle)
		{
			float randomFloat = MyUtils.GetRandomFloat(0f - maxAngle, maxAngle);
			float randomFloat2 = MyUtils.GetRandomFloat(0f, MathF.PI * 2f);
			return Vector3.TransformNormal(-new Vector3(MyMath.FastSin(randomFloat) * MyMath.FastCos(randomFloat2), MyMath.FastSin(randomFloat) * MyMath.FastSin(randomFloat2), MyMath.FastCos(randomFloat)), m_matrix);
		}

		public static Vector3 GetRandom(Vector3 originalVector, float maxAngle)
		{
			if (maxAngle == 0f)
			{
				return originalVector;
			}
			return new MyUtilRandomVector3ByDeviatingVector(originalVector).GetNext(maxAngle);
		}
	}
}
