using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	internal class MyProviderLeaf : IMyOctreeLeafNode, IDisposable
	{
		[ThreadStatic]
		private static MyStorageData m_filteredValueBuffer;

		private IMyStorageDataProvider m_provider;

		private MyStorageDataTypeEnum m_dataType;

		private MyCellCoord m_cell;

		private static MyStorageData FilteredValueBuffer
		{
			get
			{
				if (m_filteredValueBuffer == null)
				{
					m_filteredValueBuffer = new MyStorageData();
					m_filteredValueBuffer.Resize(Vector3I.One);
				}
				return m_filteredValueBuffer;
			}
		}

		MyOctreeStorage.ChunkTypeEnum IMyOctreeLeafNode.SerializedChunkType
		{
			get
			{
				if (m_dataType != 0)
				{
					return MyOctreeStorage.ChunkTypeEnum.MaterialLeafProvider;
				}
				return MyOctreeStorage.ChunkTypeEnum.ContentLeafProvider;
			}
		}

		int IMyOctreeLeafNode.SerializedChunkSize => 0;

		Vector3I IMyOctreeLeafNode.VoxelRangeMin => m_cell.CoordInLod << m_cell.Lod;

		bool IMyOctreeLeafNode.ReadOnly => true;

		public MyProviderLeaf(IMyStorageDataProvider provider, MyStorageDataTypeEnum dataType, ref MyCellCoord cell)
		{
			m_provider = provider;
			m_dataType = dataType;
			m_cell = cell;
		}

		[Conditional("DEBUG")]
		private void AssertRangeIsInside(int lodIndex, ref Vector3I globalMin, ref Vector3I globalMax)
		{
			int num = m_cell.Lod - lodIndex;
			int num2 = 1 << num;
			_ = (m_cell.CoordInLod << num) + (num2 - 1);
		}

		byte IMyOctreeLeafNode.GetFilteredValue()
		{
			MyStorageData filteredValueBuffer = FilteredValueBuffer;
			m_provider.ReadRange(filteredValueBuffer, m_dataType.ToFlags(), ref Vector3I.Zero, m_cell.Lod, ref m_cell.CoordInLod, ref m_cell.CoordInLod);
			if (m_dataType != MyStorageDataTypeEnum.Material)
			{
				return filteredValueBuffer.Content(0);
			}
			return filteredValueBuffer.Material(0);
		}

		void IMyOctreeLeafNode.ReadRange(MyStorageData target, MyStorageDataTypeFlags types, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, ref MyVoxelRequestFlags flags)
		{
			int shift = m_cell.Lod - lodIndex;
			Vector3I b = m_cell.CoordInLod << shift;
			Vector3I minInLod2 = minInLod + b;
			Vector3I maxInLod2 = maxInLod + b;
			MyVoxelDataRequest myVoxelDataRequest = default(MyVoxelDataRequest);
			myVoxelDataRequest.Target = target;
			myVoxelDataRequest.Offset = writeOffset;
			myVoxelDataRequest.Lod = lodIndex;
			myVoxelDataRequest.MinInLod = minInLod2;
			myVoxelDataRequest.MaxInLod = maxInLod2;
			myVoxelDataRequest.RequestFlags = flags;
			myVoxelDataRequest.RequestedData = types;
			MyVoxelDataRequest request = myVoxelDataRequest;
			m_provider.ReadRange(ref request);
			flags = request.Flags;
		}

		void IMyOctreeLeafNode.ExecuteOperation<TOperator>(ref TOperator source, ref Vector3I readOffset, ref Vector3I min, ref Vector3I max)
		{
			throw new NotSupportedException();
		}

		void IMyOctreeLeafNode.OnDataProviderChanged(IMyStorageDataProvider newProvider)
		{
			m_provider = newProvider;
		}

		void IMyOctreeLeafNode.ReplaceValues(Dictionary<byte, byte> oldToNewValueMap)
		{
		}

		public ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true)
		{
			return m_provider.Intersect(box, lod);
		}

		public bool Intersect(ref LineD line, out double startOffset, out double endOffset)
		{
			return m_provider.Intersect(ref line, out startOffset, out endOffset);
		}

		public bool TryGetUniformValue(out byte uniformValue)
		{
			_ = FilteredValueBuffer;
			MyVoxelDataRequest myVoxelDataRequest = default(MyVoxelDataRequest);
			myVoxelDataRequest.Target = null;
			myVoxelDataRequest.Offset = Vector3I.Zero;
			myVoxelDataRequest.Lod = m_cell.Lod;
			myVoxelDataRequest.MinInLod = m_cell.CoordInLod;
			myVoxelDataRequest.MaxInLod = m_cell.CoordInLod;
			myVoxelDataRequest.RequestedData = m_dataType.ToFlags();
			MyVoxelDataRequest request = myVoxelDataRequest;
			m_provider.ReadRange(ref request, detectOnly: true);
			if ((request.Flags & MyVoxelRequestFlags.EmptyData) > (MyVoxelRequestFlags)0)
			{
				uniformValue = (byte)((m_dataType == MyStorageDataTypeEnum.Material) ? byte.MaxValue : 0);
				return true;
			}
			if (m_dataType == MyStorageDataTypeEnum.Content && (request.Flags & MyVoxelRequestFlags.FullContent) > (MyVoxelRequestFlags)0)
			{
				uniformValue = byte.MaxValue;
				return true;
			}
			uniformValue = 0;
			return false;
		}

		public void Dispose()
		{
		}
	}
}
