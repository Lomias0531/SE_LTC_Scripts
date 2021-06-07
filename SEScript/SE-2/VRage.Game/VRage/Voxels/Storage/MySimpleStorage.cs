using System;
using System.Threading;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Voxels.Storage
{
	public class MySimpleStorage : VRage.Game.Voxels.IMyStorage, VRage.ModAPI.IMyStorage
	{
		private static int m_storegeIds;

		private MyStorageData m_data;

		public MyStorageData Data => m_data;

		public bool AreCompressedDataCached => true;

		public bool MarkedForClose => false;

		public Vector3I Size
		{
			get;
			private set;
		}

		public uint StorageId
		{
			get;
			private set;
		}

		public bool Shared
		{
			get;
			private set;
		}

		public bool Closed
		{
			get;
			private set;
		}

		public bool DeleteSupported => false;

		public IMyStorageDataProvider DataProvider => null;

		public event Action<Vector3I, Vector3I, MyStorageDataTypeFlags> RangeChanged;

		public MySimpleStorage(int size)
		{
			Size = new Vector3I(size);
			m_data = new MyStorageData();
			m_data.Resize(Size);
			StorageId = (uint)Interlocked.Increment(ref m_storegeIds);
		}

		public ContainmentType Intersect(ref BoundingBox box, bool lazy)
		{
			return ContainmentType.Disjoint;
		}

		public void PinAndExecute(Action action)
		{
		}

		public void PinAndExecute(Action<VRage.ModAPI.IMyStorage> action)
		{
		}

		public void Reset(MyStorageDataTypeFlags dataToReset)
		{
		}

		public unsafe void Save(out byte[] outCompressedData)
		{
			outCompressedData = new byte[m_data.SizeLinear * 2 + 4];
			int num = 0;
			fixed (byte* ptr = outCompressedData)
			{
				*(int*)ptr = m_data.Size3D.X;
				num += 4;
				for (int i = 0; i < m_data.SizeLinear; i++)
				{
					ptr[num++] = m_data.Content(i);
					ptr[num++] = m_data.Material(i);
				}
			}
		}

		public byte[] GetVoxelData()
		{
			Save(out byte[] outCompressedData);
			return outCompressedData;
		}

		public void OverwriteAllMaterials(byte materialIndex)
		{
		}

		public void Close()
		{
			Closed = true;
		}

		public VRage.Game.Voxels.IMyStorage Copy()
		{
			throw new NotImplementedException();
		}

		public StoragePin Pin()
		{
			return new StoragePin(this);
		}

		public void Unpin()
		{
		}

		public void ReadRange(MyStorageData target, MyStorageDataTypeFlags dataToRead, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax)
		{
			MyVoxelRequestFlags requestFlags = (MyVoxelRequestFlags)0;
			ReadRange(target, dataToRead, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
		}

		public void ReadRange(MyStorageData target, MyStorageDataTypeFlags dataToRead, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax, ref MyVoxelRequestFlags requestFlags)
		{
			lodVoxelRangeMax = Vector3I.Min(lodVoxelRangeMax, (Size >> lodIndex) - 1);
			if ((dataToRead & MyStorageDataTypeFlags.Content) != 0)
			{
				target.CopyRange(m_data, lodVoxelRangeMin, lodVoxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Content);
			}
			if ((dataToRead & MyStorageDataTypeFlags.Material) != 0)
			{
				target.CopyRange(m_data, lodVoxelRangeMin, lodVoxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Material);
			}
		}

		public void WriteRange(MyStorageData source, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax, bool notify, bool skipCache)
		{
			if ((dataToWrite & MyStorageDataTypeFlags.Content) != 0)
			{
				m_data.CopyRange(source, voxelRangeMin, voxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Content);
			}
			if ((dataToWrite & MyStorageDataTypeFlags.Material) != 0)
			{
				m_data.CopyRange(source, voxelRangeMin, voxelRangeMax, Vector3I.Zero, MyStorageDataTypeEnum.Material);
			}
		}

		public void DeleteRange(MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax, bool notify)
		{
		}

		public void ExecuteOperationFast<TVoxelOperator>(ref TVoxelOperator voxelOperator, MyStorageDataTypeFlags dataToWrite, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax, bool notifyRangeChanged) where TVoxelOperator : struct, IVoxelOperator
		{
			if ((dataToWrite & MyStorageDataTypeFlags.Content) != 0)
			{
				ExecuteOperationInternal(ref voxelOperator, MyStorageDataTypeEnum.Content, ref voxelRangeMin, ref voxelRangeMax);
			}
			if ((dataToWrite & MyStorageDataTypeFlags.Material) != 0)
			{
				ExecuteOperationInternal(ref voxelOperator, MyStorageDataTypeEnum.Material, ref voxelRangeMin, ref voxelRangeMax);
			}
		}

		private void ExecuteOperationInternal<TVoxelOperator>(ref TVoxelOperator voxelOperator, MyStorageDataTypeEnum dataType, ref Vector3I min, ref Vector3I max) where TVoxelOperator : struct, IVoxelOperator
		{
			Vector3I step = m_data.Step;
			byte[] array = m_data[dataType];
			Vector3I position = default(Vector3I);
			position.Z = min.Z;
			int num = 0;
			while (position.Z <= max.Z)
			{
				position.Y = min.Y;
				int num2 = 0;
				while (position.Y <= max.Y)
				{
					int num3 = num2 + num;
					position.X = min.X;
					int num4 = 0;
					while (position.X <= max.X)
					{
						voxelOperator.Op(ref position, dataType, ref array[num4 + num3]);
						position.X++;
						num4 += step.X;
					}
					position.Y++;
					num2 += step.Y;
				}
				position.Z++;
				num += step.Z;
			}
		}

		public void NotifyRangeChanged(ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax, MyStorageDataTypeFlags dataChanged)
		{
		}

		public ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true)
		{
			return ContainmentType.Intersects;
		}

		public bool Intersect(ref LineD line)
		{
			return true;
		}

		public void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode)
		{
		}

		public void SetCompressedDataCache(byte[] data)
		{
		}

		public void NotifyChanged(Vector3I voxelRangeMin, Vector3I voxelRangeMax, MyStorageDataTypeFlags changedData)
		{
		}
	}
}
