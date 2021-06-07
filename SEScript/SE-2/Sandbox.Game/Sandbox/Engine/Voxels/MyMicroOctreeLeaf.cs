using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game.Voxels;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	internal class MyMicroOctreeLeaf : IMyOctreeLeafNode, IDisposable
	{
		private const bool DEBUG_WRITES = false;

		private MySparseOctree m_octree;

		private MyStorageDataTypeEnum m_dataType;

		private Vector3I m_voxelRangeMin;

		Vector3I IMyOctreeLeafNode.VoxelRangeMin => m_voxelRangeMin;

		bool IMyOctreeLeafNode.ReadOnly => false;

		MyOctreeStorage.ChunkTypeEnum IMyOctreeLeafNode.SerializedChunkType
		{
			get
			{
				if (m_dataType != 0)
				{
					return MyOctreeStorage.ChunkTypeEnum.MaterialLeafOctree;
				}
				return MyOctreeStorage.ChunkTypeEnum.ContentLeafOctree;
			}
		}

		int IMyOctreeLeafNode.SerializedChunkSize => m_octree.SerializedSize;

		public unsafe MyMicroOctreeLeaf(MyStorageDataTypeEnum dataType, int height, Vector3I voxelRangeMin)
		{
			if (dataType == MyStorageDataTypeEnum.Content)
			{
				m_octree = new MySparseOctree(height, MyOctreeNode.ContentFilter, 0);
			}
			else
			{
				m_octree = new MySparseOctree(height, MyOctreeNode.MaterialFilter, byte.MaxValue);
			}
			m_dataType = dataType;
			m_voxelRangeMin = voxelRangeMin;
		}

		internal void BuildFrom(MyStorageData source)
		{
			MyStorageData.MortonEnumerator data = new MyStorageData.MortonEnumerator(source, m_dataType);
			m_octree.Build(data);
		}

		internal void BuildFrom(byte singleValue)
		{
			m_octree.Build(singleValue);
		}

		internal void WriteTo(Stream stream)
		{
			m_octree.WriteTo(stream);
		}

		internal unsafe void ReadFrom(MyOctreeStorage.ChunkHeader header, Stream stream)
		{
			if (m_octree == null)
			{
				m_octree = new MySparseOctree(0, (header.ChunkType == MyOctreeStorage.ChunkTypeEnum.ContentLeafOctree) ? MyOctreeNode.ContentFilter : MyOctreeNode.MaterialFilter, 0);
			}
			m_octree.ReadFrom(header, stream);
		}

		public bool TryGetUniformValue(out byte uniformValue)
		{
			if (m_octree.IsAllSame)
			{
				uniformValue = m_octree.GetFilteredValue();
				return true;
			}
			uniformValue = 0;
			return false;
		}

		internal void DebugDraw(IMyDebugDrawBatchAabb batch, Vector3 worldPos, MyVoxelDebugDrawMode mode)
		{
			m_octree.DebugDraw(batch, worldPos, mode);
		}

		void IMyOctreeLeafNode.ReadRange(MyStorageData target, MyStorageDataTypeFlags types, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, ref MyVoxelRequestFlags flags)
		{
			m_octree.ReadRange(target, m_dataType, ref writeOffset, lodIndex, ref minInLod, ref maxInLod);
			flags = (MyVoxelRequestFlags)0;
		}

		void IMyOctreeLeafNode.ExecuteOperation<TOperator>(ref TOperator source, ref Vector3I readOffset, ref Vector3I min, ref Vector3I max)
		{
			MyStorageData myStorageData = null;
			m_octree.ExecuteOperation(ref source, m_dataType, ref readOffset, ref min, ref max);
		}

		byte IMyOctreeLeafNode.GetFilteredValue()
		{
			return m_octree.GetFilteredValue();
		}

		void IMyOctreeLeafNode.OnDataProviderChanged(IMyStorageDataProvider newProvider)
		{
		}

		void IMyOctreeLeafNode.ReplaceValues(Dictionary<byte, byte> oldToNewValueMap)
		{
			m_octree.ReplaceValues(oldToNewValueMap);
		}

		public ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true)
		{
			BoundingBoxI box2 = box;
			box2.Translate(-m_voxelRangeMin);
			return m_octree.Intersect(ref box2, lod, exhaustiveContainmentCheck);
		}

		public bool Intersect(ref LineD line, out double startOffset, out double endOffset)
		{
			line.From -= (Vector3D)m_voxelRangeMin;
			line.To -= (Vector3D)m_voxelRangeMin;
			if (m_octree.Intersect(ref line, out startOffset, out endOffset))
			{
				line.From += (Vector3D)m_voxelRangeMin;
				line.To += (Vector3D)m_voxelRangeMin;
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			m_octree?.Dispose();
		}
	}
}
