using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
	public struct MyIntersectionResultLineTriangleEx
	{
		public MyIntersectionResultLineTriangle Triangle;

		public Vector3 IntersectionPointInObjectSpace;

		public Vector3D IntersectionPointInWorldSpace;

		public IMyEntity Entity;

		public object UserObject;

		public Vector3 NormalInWorldSpace;

		public Vector3 NormalInObjectSpace;

		public Line InputLineInObjectSpace;

		public MyIntersectionResultLineTriangleEx(MyIntersectionResultLineTriangle triangle, IMyEntity entity, ref Line line)
		{
			Triangle = triangle;
			Entity = entity;
			InputLineInObjectSpace = line;
			UserObject = null;
			NormalInObjectSpace = MyUtils.GetNormalVectorFromTriangle(ref Triangle.InputTriangle);
			if (!NormalInObjectSpace.IsValid())
			{
				NormalInObjectSpace = new Vector3(0f, 0f, 1f);
			}
			IntersectionPointInObjectSpace = line.From + line.Direction * Triangle.Distance;
			if (Entity is IMyVoxelBase)
			{
				IntersectionPointInWorldSpace = (Vector3D)IntersectionPointInObjectSpace + ((IMyVoxelBase)Entity).PositionLeftBottomCorner;
				NormalInWorldSpace = NormalInObjectSpace;
			}
			else
			{
				MatrixD matrix = Entity.WorldMatrix;
				NormalInWorldSpace = MyUtils.GetTransformNormalNormalized(NormalInObjectSpace, ref matrix);
				IntersectionPointInWorldSpace = Vector3D.Transform(IntersectionPointInObjectSpace, ref matrix);
			}
		}

		public MyIntersectionResultLineTriangleEx(MyIntersectionResultLineTriangle triangle, IMyEntity entity, ref Line line, Vector3D intersectionPointInWorldSpace, Vector3 normalInWorldSpace)
		{
			Triangle = triangle;
			Entity = entity;
			InputLineInObjectSpace = line;
			UserObject = null;
			NormalInObjectSpace = (NormalInWorldSpace = normalInWorldSpace);
			IntersectionPointInWorldSpace = intersectionPointInWorldSpace;
			IntersectionPointInObjectSpace = IntersectionPointInWorldSpace;
		}

		public VertexBoneIndicesWeights? GetAffectingBoneIndicesWeights(ref List<VertexArealBoneIndexWeight> tmpStorage)
		{
			if (!Triangle.BoneWeights.HasValue)
			{
				return null;
			}
			if (tmpStorage == null)
			{
				tmpStorage = new List<VertexArealBoneIndexWeight>(4);
			}
			tmpStorage.Clear();
			MyTriangle_BoneIndicesWeigths value = Triangle.BoneWeights.Value;
			Vector3.Barycentric(IntersectionPointInObjectSpace, Triangle.InputTriangle.Vertex0, Triangle.InputTriangle.Vertex1, Triangle.InputTriangle.Vertex2, out float u, out float v, out float w);
			FillIndicesWeightsStorage(tmpStorage, ref value.Vertex0, u);
			FillIndicesWeightsStorage(tmpStorage, ref value.Vertex1, v);
			FillIndicesWeightsStorage(tmpStorage, ref value.Vertex2, w);
			tmpStorage.Sort(Comparison);
			VertexBoneIndicesWeights indicesWeights = default(VertexBoneIndicesWeights);
			FillIndicesWeights(ref indicesWeights, 0, tmpStorage);
			FillIndicesWeights(ref indicesWeights, 1, tmpStorage);
			FillIndicesWeights(ref indicesWeights, 2, tmpStorage);
			FillIndicesWeights(ref indicesWeights, 3, tmpStorage);
			NormalizeBoneWeights(ref indicesWeights);
			return indicesWeights;
		}

		private int Comparison(VertexArealBoneIndexWeight x, VertexArealBoneIndexWeight y)
		{
			if (x.Weight > y.Weight)
			{
				return -1;
			}
			if (x.Weight == y.Weight)
			{
				return 0;
			}
			return 1;
		}

		private void FillIndicesWeights(ref VertexBoneIndicesWeights indicesWeights, int index, List<VertexArealBoneIndexWeight> tmpStorage)
		{
			if (index < tmpStorage.Count)
			{
				indicesWeights.Indices[index] = tmpStorage[index].Index;
				indicesWeights.Weights[index] = tmpStorage[index].Weight;
			}
		}

		private void FillIndicesWeightsStorage(List<VertexArealBoneIndexWeight> tmpStorage, ref MyVertex_BoneIndicesWeights indicesWeights, float arealCoord)
		{
			HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 0, arealCoord);
			HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 1, arealCoord);
			HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 2, arealCoord);
			HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 3, arealCoord);
		}

		private void HandleAddBoneIndexWeight(List<VertexArealBoneIndexWeight> tmpStorage, ref MyVertex_BoneIndicesWeights indicesWeights, int index, float arealCoord)
		{
			float num = indicesWeights.Weights[index];
			if (num != 0f)
			{
				byte b = indicesWeights.Indices[index];
				int num2 = FindExsistingBoneIndexWeight(tmpStorage, b);
				if (num2 == -1)
				{
					tmpStorage.Add(new VertexArealBoneIndexWeight
					{
						Index = b,
						Weight = num * arealCoord
					});
					return;
				}
				VertexArealBoneIndexWeight value = tmpStorage[num2];
				value.Weight += num * arealCoord;
				tmpStorage[num2] = value;
			}
		}

		private int FindExsistingBoneIndexWeight(List<VertexArealBoneIndexWeight> tmpStorage, int boneIndex)
		{
			int result = -1;
			for (int i = 0; i < tmpStorage.Count; i++)
			{
				if (tmpStorage[i].Index == boneIndex)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private void NormalizeBoneWeights(ref VertexBoneIndicesWeights indicesWeights)
		{
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				num += indicesWeights.Weights[i];
			}
			for (int j = 0; j < 4; j++)
			{
				indicesWeights.Weights[j] /= num;
			}
		}

		public static MyIntersectionResultLineTriangleEx? GetCloserIntersection(ref MyIntersectionResultLineTriangleEx? a, ref MyIntersectionResultLineTriangleEx? b)
		{
			if ((!a.HasValue && b.HasValue) || (a.HasValue && b.HasValue && b.Value.Triangle.Distance < a.Value.Triangle.Distance))
			{
				return b;
			}
			return a;
		}

		public static bool IsDistanceLessThanTolerance(ref MyIntersectionResultLineTriangleEx? a, ref MyIntersectionResultLineTriangleEx? b, float distanceTolerance)
		{
			if ((!a.HasValue && b.HasValue) || (a.HasValue && b.HasValue && Math.Abs(b.Value.Triangle.Distance - a.Value.Triangle.Distance) <= distanceTolerance))
			{
				return true;
			}
			return false;
		}
	}
}
