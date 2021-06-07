using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace VRage.Voxels
{
	[Serializable]
	public class MyStorageData
	{
		public interface IOperator
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void Op(ref byte target, byte source);
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct CopyOperator : IOperator
		{
			public void Op(ref byte target, byte source)
			{
				target = source;
			}
		}

		public struct MortonEnumerator : IEnumerator<byte>, IEnumerator, IDisposable
		{
			private MyStorageDataTypeEnum m_type;

			private MyStorageData m_source;

			private int m_maxMortonCode;

			private int m_mortonCode;

			private Vector3I m_pos;

			private byte m_current;

			public byte Current => m_current;

			object IEnumerator.Current => m_current;

			public MortonEnumerator(MyStorageData source, MyStorageDataTypeEnum type)
			{
				m_type = type;
				m_source = source;
				m_maxMortonCode = source.Size3D.Size;
				m_mortonCode = -1;
				m_pos = default(Vector3I);
				m_current = 0;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				m_mortonCode++;
				if (m_mortonCode < m_maxMortonCode)
				{
					MyMortonCode3D.Decode(m_mortonCode, out m_pos);
					m_current = m_source.Get(m_type, ref m_pos);
					return true;
				}
				return false;
			}

			public void Reset()
			{
				m_mortonCode = -1;
				m_current = 0;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_dataByType_003C_003EAccessor : IMemberAccessor<MyStorageData, byte[][]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in byte[][] value)
			{
				owner.m_dataByType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out byte[][] value)
			{
				value = owner.m_dataByType;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_sZ_003C_003EAccessor : IMemberAccessor<MyStorageData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in int value)
			{
				owner.m_sZ = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out int value)
			{
				value = owner.m_sZ;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_sY_003C_003EAccessor : IMemberAccessor<MyStorageData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in int value)
			{
				owner.m_sY = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out int value)
			{
				value = owner.m_sY;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_size3d_003C_003EAccessor : IMemberAccessor<MyStorageData, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in Vector3I value)
			{
				owner.m_size3d = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out Vector3I value)
			{
				value = owner.m_size3d;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_sizeLinear_003C_003EAccessor : IMemberAccessor<MyStorageData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in int value)
			{
				owner.m_sizeLinear = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out int value)
			{
				value = owner.m_sizeLinear;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_dataSizeLinear_003C_003EAccessor : IMemberAccessor<MyStorageData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in int value)
			{
				owner.m_dataSizeLinear = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out int value)
			{
				value = owner.m_dataSizeLinear;
			}
		}

		protected class VRage_Voxels_MyStorageData_003C_003Em_storedTypes_003C_003EAccessor : IMemberAccessor<MyStorageData, MyStorageDataTypeFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyStorageData owner, in MyStorageDataTypeFlags value)
			{
				owner.m_storedTypes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyStorageData owner, out MyStorageDataTypeFlags value)
			{
				value = owner.m_storedTypes;
			}
		}

		private byte[][] m_dataByType;

		private int m_sZ;

		private int m_sY;

		private Vector3I m_size3d;

		private int m_sizeLinear;

		private int m_dataSizeLinear = -1;

		private MyStorageDataTypeFlags m_storedTypes;

		public byte[] this[MyStorageDataTypeEnum type]
		{
			get
			{
				return m_dataByType[(int)type];
			}
			set
			{
				if (m_dataSizeLinear == -1)
				{
					m_dataSizeLinear = value.Length;
				}
				m_dataByType[(int)type] = value;
			}
		}

		public int SizeLinear => m_sizeLinear;

		public int StepLinear => 1;

		public int StepX => 1;

		public int StepY => m_sY;

		public int StepZ => m_sZ;

		public Vector3I Step => new Vector3I(1, m_sY, m_sZ);

		public Vector3I Size3D => m_size3d;

		public MyStorageData()
		{
			m_storedTypes = MyStorageDataTypeFlags.ContentAndMaterial;
			m_dataByType = new byte[2][];
		}

		public MyStorageData(MyStorageDataTypeFlags typesToStore)
		{
			m_storedTypes = typesToStore;
			m_dataByType = new byte[2][];
		}

		public MyStorageData(Vector3I size, byte[] content = null, byte[] material = null)
		{
			m_dataByType = new byte[2][];
			Resize(size);
			if (content != null)
			{
				m_storedTypes |= MyStorageDataTypeFlags.Content;
				this[MyStorageDataTypeEnum.Content] = content;
			}
			if (material != null)
			{
				m_storedTypes |= MyStorageDataTypeFlags.Material;
				this[MyStorageDataTypeEnum.Material] = material;
			}
		}

		/// <param name="start">Inclusive.</param>
		/// <param name="end">Inclusive.</param>
		public void Resize(Vector3I start, Vector3I end)
		{
			Resize(end - start + 1);
		}

		public void Resize(Vector3I size3D)
		{
			m_size3d = size3D;
			int size = size3D.Size;
			m_sY = size3D.X;
			m_sZ = size3D.Y * m_sY;
			m_sizeLinear = size * StepLinear;
			for (int i = 0; i < m_dataByType.Length; i++)
			{
				if ((m_dataByType[i] == null || m_dataByType[i].Length < m_sizeLinear) && m_storedTypes.Requests((MyStorageDataTypeEnum)i))
				{
					m_dataByType[i] = new byte[m_sizeLinear];
				}
			}
		}

		public byte Get(MyStorageDataTypeEnum type, ref Vector3I p)
		{
			return this[type][p.X + p.Y * m_sY + p.Z * m_sZ];
		}

		public byte Get(MyStorageDataTypeEnum type, int linearIdx)
		{
			return this[type][linearIdx];
		}

		public byte Get(MyStorageDataTypeEnum type, int x, int y, int z)
		{
			return this[type][x + y * m_sY + z * m_sZ];
		}

		public void Set(MyStorageDataTypeEnum type, ref Vector3I p, byte value)
		{
			this[type][p.X + p.Y * m_sY + p.Z * m_sZ] = value;
		}

		public void Content(ref Vector3I p, byte content)
		{
			this[MyStorageDataTypeEnum.Content][p.X + p.Y * m_sY + p.Z * m_sZ] = content;
		}

		public void Content(int linearIdx, byte content)
		{
			this[MyStorageDataTypeEnum.Content][linearIdx] = content;
		}

		public byte Content(ref Vector3I p)
		{
			return this[MyStorageDataTypeEnum.Content][p.X + p.Y * m_sY + p.Z * m_sZ];
		}

		public byte Content(int x, int y, int z)
		{
			return this[MyStorageDataTypeEnum.Content][x + y * m_sY + z * m_sZ];
		}

		public byte Content(int linearIdx)
		{
			return this[MyStorageDataTypeEnum.Content][linearIdx];
		}

		public void Material(ref Vector3I p, byte materialIdx)
		{
			this[MyStorageDataTypeEnum.Material][p.X + p.Y * m_sY + p.Z * m_sZ] = materialIdx;
		}

		public byte Material(ref Vector3I p)
		{
			return this[MyStorageDataTypeEnum.Material][p.X + p.Y * m_sY + p.Z * m_sZ];
		}

		public byte Material(int linearIdx)
		{
			return this[MyStorageDataTypeEnum.Material][linearIdx];
		}

		public void Material(int linearIdx, byte materialIdx)
		{
			this[MyStorageDataTypeEnum.Material][linearIdx] = materialIdx;
		}

		public int ComputeLinear(ref Vector3I p)
		{
			return p.X + p.Y * m_sY + p.Z * m_sZ;
		}

		public void ComputePosition(int linear, out Vector3I p)
		{
			int num = linear % m_sY;
			int num2 = (linear - num) % m_sZ / m_sY;
			int z = (linear - num - num2 * m_sY) / m_sZ;
			p = new Vector3I(num, num2, z);
		}

		public bool WrinkleVoxelContent(ref Vector3I p, float wrinkleWeightAdd, float wrinkleWeightRemove)
		{
			int num = int.MinValue;
			int num2 = int.MaxValue;
			int num3 = (int)(wrinkleWeightAdd * 255f);
			int num4 = (int)(wrinkleWeightRemove * 255f);
			Vector3I p2 = default(Vector3I);
			for (int i = -1; i <= 1; i++)
			{
				p2.Z = i + p.Z;
				if ((uint)p2.Z >= (uint)m_size3d.Z)
				{
					continue;
				}
				for (int j = -1; j <= 1; j++)
				{
					p2.Y = j + p.Y;
					if ((uint)p2.Y >= (uint)m_size3d.Y)
					{
						continue;
					}
					for (int k = -1; k <= 1; k++)
					{
						p2.X = k + p.X;
						if ((uint)p2.X < (uint)m_size3d.X)
						{
							byte val = Content(ref p2);
							num = Math.Max(num, val);
							num2 = Math.Min(num2, val);
						}
					}
				}
			}
			if (num2 == num)
			{
				return false;
			}
			int num5 = Content(ref p);
			byte b = (byte)MyUtils.GetClampInt(num5 + MyUtils.GetRandomInt(num3 + num4) - num4, num2, num);
			if (b != num5)
			{
				Content(ref p, b);
				return true;
			}
			return false;
		}

		public unsafe void BlockFill(MyStorageDataTypeEnum type, Vector3I min, Vector3I max, byte content)
		{
			min.Z *= m_sZ;
			max.Z *= m_sZ;
			min.Y *= m_sY;
			max.Y *= m_sY;
			ref int x = ref min.X;
			x = x;
			ref int x2 = ref max.X;
			x2 = x2;
			fixed (byte* ptr = &this[type][0])
			{
				Vector3I vector3I = default(Vector3I);
				vector3I.Z = min.Z;
				while (vector3I.Z <= max.Z)
				{
					int z = vector3I.Z;
					vector3I.Y = min.Y;
					while (vector3I.Y <= max.Y)
					{
						int num = z + vector3I.Y;
						vector3I.X = min.X;
						while (vector3I.X <= max.X)
						{
							ptr[vector3I.X + num] = content;
							vector3I.X++;
						}
						vector3I.Y += m_sY;
					}
					vector3I.Z += m_sZ;
				}
			}
		}

		public void BlockFillContent(Vector3I min, Vector3I max, byte content)
		{
			BlockFill(MyStorageDataTypeEnum.Content, min, max, content);
		}

		public unsafe void BlockFillMaterialConsiderContent(Vector3I min, Vector3I max, byte materialIdx)
		{
			min.Z *= m_sZ;
			max.Z *= m_sZ;
			min.Y *= m_sY;
			max.Y *= m_sY;
			ref int x = ref min.X;
			x = x;
			ref int x2 = ref max.X;
			x2 = x2;
			fixed (byte* ptr = &this[MyStorageDataTypeEnum.Content][0])
			{
				fixed (byte* ptr2 = &this[MyStorageDataTypeEnum.Material][0])
				{
					Vector3I vector3I = default(Vector3I);
					vector3I.Z = min.Z;
					while (vector3I.Z <= max.Z)
					{
						int z = vector3I.Z;
						vector3I.Y = min.Y;
						while (vector3I.Y <= max.Y)
						{
							int num = z + vector3I.Y;
							vector3I.X = min.X;
							while (vector3I.X <= max.X)
							{
								if (ptr[vector3I.X + num] == 0)
								{
									ptr2[vector3I.X + num] = byte.MaxValue;
								}
								else
								{
									ptr2[vector3I.X + num] = materialIdx;
								}
								vector3I.X++;
							}
							vector3I.Y += m_sY;
						}
						vector3I.Z += m_sZ;
					}
				}
			}
		}

		public void BlockFillMaterial(Vector3I min, Vector3I max, byte materialIdx)
		{
			BlockFill(MyStorageDataTypeEnum.Material, min, max, materialIdx);
		}

		public void CopyRange(MyStorageData src, Vector3I min, Vector3I max, Vector3I offset, MyStorageDataTypeEnum dataType)
		{
			OpRange<CopyOperator>(src, min, max, offset, dataType);
		}

		public void OpRange<Op>(MyStorageData src, Vector3I min, Vector3I max, Vector3I offset, MyStorageDataTypeEnum dataType) where Op : struct, IOperator
		{
			byte[] array = this[dataType];
			Vector3I step = Step;
			Vector3I step2 = src.Step;
			byte[] array2 = src[dataType];
			min *= step2;
			max *= step2;
			offset *= step;
			Op val = default(Op);
			int num = min.Z;
			int num2 = offset.X;
			while (num <= max.Z)
			{
				int num3 = min.Y;
				int num4 = offset.Y;
				while (num3 <= max.Y)
				{
					int num5 = num3 + num;
					int num6 = num4 + num2;
					int num7 = min.X;
					int num8 = offset.Z;
					while (num7 <= max.X)
					{
						val.Op(ref array[num8 + num6], array2[num7 + num5]);
						num7 += step2.X;
						num8 += step.X;
					}
					num3 += step2.Y;
					num4 += step.Y;
				}
				num += step2.Z;
				num2 += step.Z;
			}
		}

		public bool ContainsIsoSurface()
		{
			try
			{
				byte[] array = this[MyStorageDataTypeEnum.Content];
				bool flag = array[0] < 127;
				for (int i = 1; i < m_sizeLinear; i += StepLinear)
				{
					bool flag2 = array[i] < 127;
					if (flag != flag2)
					{
						return true;
					}
				}
				return false;
			}
			finally
			{
			}
		}

		public MyVoxelContentConstitution ComputeContentConstitution()
		{
			try
			{
				byte[] array = this[MyStorageDataTypeEnum.Content];
				bool flag = array[0] < 127;
				for (int i = 1; i < m_sizeLinear; i += StepLinear)
				{
					bool flag2 = array[i] < 127;
					if (flag != flag2)
					{
						return MyVoxelContentConstitution.Mixed;
					}
				}
				return (!flag) ? MyVoxelContentConstitution.Full : MyVoxelContentConstitution.Empty;
			}
			finally
			{
			}
		}

		public bool ContainsVoxelsAboveIsoLevel()
		{
			byte[] array = this[MyStorageDataTypeEnum.Content];
			try
			{
				for (int i = 0; i < m_sizeLinear; i += StepLinear)
				{
					if (array[i] > 127)
					{
						return true;
					}
				}
				return false;
			}
			finally
			{
			}
		}

		public int ValueWhenAllEqual(MyStorageDataTypeEnum dataType)
		{
			byte[] array = this[dataType];
			byte b = array[0];
			for (int i = 1; i < m_sizeLinear; i += StepLinear)
			{
				if (b != array[i])
				{
					return -1;
				}
			}
			return b;
		}

		[Conditional("DEBUG")]
		private void AssertPosition(ref Vector3I position)
		{
		}

		[Conditional("DEBUG")]
		private void AssertPosition(int x, int y, int z)
		{
		}

		public void ClearContent(byte p)
		{
			Clear(MyStorageDataTypeEnum.Content, p);
		}

		public void ClearMaterials(byte p)
		{
			Clear(MyStorageDataTypeEnum.Material, p);
		}

		public unsafe void Clear(MyStorageDataTypeEnum type, byte p)
		{
			fixed (byte* ptr = this[type])
			{
				for (int i = 0; i < m_sizeLinear; i++)
				{
					ptr[i] = p;
				}
			}
		}

		public string ToBase64()
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, this);
			return Convert.ToBase64String(memoryStream.GetBuffer());
		}

		public static MyStorageData FromBase64(string str)
		{
			MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(str));
			return (MyStorageData)new BinaryFormatter().Deserialize(serializationStream);
		}
	}
}
