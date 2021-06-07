using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sandbox.Engine.Voxels
{
	internal struct MyOctreeNode
	{
		public unsafe delegate byte FilterFunction(byte* pData, int lod);

		public const int CHILD_COUNT = 8;

		public const int SERIALIZED_SIZE = 9;

		[ThreadStatic]
		private static Dictionary<byte, int> Histogram;

		public static readonly FilterFunction ContentFilter;

		public static readonly FilterFunction MaterialFilter;

		public byte ChildMask;

		public unsafe fixed byte Data[8];

		public bool HasChildren => ChildMask != 0;

		unsafe static MyOctreeNode()
		{
			ContentFilter = SignedDistanceFilterInternal;
			MaterialFilter = HistogramFilterInternal;
		}

		public MyOctreeNode(byte allContent)
		{
			ChildMask = 0;
			SetAllData(allContent);
		}

		public void ClearChildren()
		{
			ChildMask = 0;
		}

		public void SetChildren()
		{
			ChildMask = byte.MaxValue;
		}

		public bool HasChild(int childIndex)
		{
			return (ChildMask & (1 << childIndex)) != 0;
		}

		public void SetChild(int childIndex, bool childPresent)
		{
			int num = 1 << childIndex;
			if (childPresent)
			{
				ChildMask |= (byte)num;
			}
			else
			{
				ChildMask &= (byte)(~num);
			}
		}

		public unsafe void SetAllData(byte value)
		{
			fixed (byte* dst = Data)
			{
				SetAllData(dst, value);
			}
		}

		public unsafe static void SetAllData(byte* dst, byte value)
		{
			for (int i = 0; i < 8; i++)
			{
				dst[i] = value;
			}
		}

		public unsafe void SetData(int childIndex, byte data)
		{
			fixed (byte* ptr = Data)
			{
				ptr[childIndex] = data;
			}
		}

		public unsafe byte GetData(int cellIndex)
		{
			fixed (byte* ptr = Data)
			{
				return ptr[cellIndex];
			}
		}

		public unsafe byte ComputeFilteredValue(FilterFunction filter, int lod)
		{
			fixed (byte* pData = Data)
			{
				return filter(pData, lod);
			}
		}

		public unsafe bool AllDataSame()
		{
			fixed (byte* pData = Data)
			{
				return AllDataSame(pData);
			}
		}

		public unsafe static bool AllDataSame(byte* pData)
		{
			byte b = *pData;
			for (int i = 1; i < 8; i++)
			{
				if (pData[i] != b)
				{
					return false;
				}
			}
			return true;
		}

		public unsafe bool AllDataSame(byte value)
		{
			fixed (byte* pData = Data)
			{
				return AllDataSame(pData, value);
			}
		}

		public unsafe static bool AllDataSame(byte* pData, byte value)
		{
			for (int i = 1; i < 8; i++)
			{
				if (pData[i] != value)
				{
					return false;
				}
			}
			return true;
		}

		public unsafe override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(20);
			stringBuilder.Append("0x").Append(ChildMask.ToString("X2")).Append(": ");
			fixed (byte* ptr = Data)
			{
				for (int i = 0; i < 8; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(ptr[i]);
				}
			}
			return stringBuilder.ToString();
		}

		[Conditional("DEBUG")]
		private void AssertChildIndex(int cellIndex)
		{
		}

		private unsafe static byte AverageFilter(byte* pData, int lod)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				num += pData[i];
			}
			num /= 8;
			return (byte)num;
		}

		private static float ToSignedDistance(byte value)
		{
			return (float)(int)value / 255f * 2f - 1f;
		}

		private static byte FromSignedDistance(float value)
		{
			return (byte)((value * 0.5f + 0.5f) * 255f + 0.5f);
		}

		private unsafe static byte SignedDistanceFilterInternal(byte* pData, int lod)
		{
			float num = ToSignedDistance(*pData);
			if (ToSignedDistance(AverageValueFilterInternal(pData, lod)) != num || (num != 1f && num != -1f))
			{
				num *= 0.5f;
			}
			return FromSignedDistance(num);
		}

		private unsafe static byte AverageValueFilterInternal(byte* pData, int lod)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += ToSignedDistance(pData[i]);
			}
			num /= 8f;
			if (num != 1f && num != -1f)
			{
				num *= 0.5f;
			}
			return FromSignedDistance(num);
		}

		private unsafe static byte IsoSurfaceFilterInternal(byte* pData, int lod)
		{
			byte b = 0;
			byte b2 = byte.MaxValue;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 8; i++)
			{
				byte b3 = pData[i];
				if (b3 < 127)
				{
					num2++;
					if (b3 > b)
					{
						b = b3;
					}
				}
				else
				{
					num++;
					if (b3 < b2)
					{
						b2 = b3;
					}
				}
			}
			float num3 = (float)(int)((num2 > num) ? b : b2) / 255f * 2f - 1f;
			if (num3 != 1f && num3 != -1f)
			{
				num3 *= 0.5f;
			}
			num3 = num3 * 0.5f + 0.5f;
			return (byte)(num3 * 255f);
		}

		private unsafe static byte HistogramFilterInternal(byte* pdata, int lod)
		{
			if (Histogram == null)
			{
				Histogram = new Dictionary<byte, int>(8);
			}
			for (int i = 0; i < 8; i++)
			{
				byte b = pdata[i];
				if (b != byte.MaxValue)
				{
					Histogram.TryGetValue(b, out int value);
					value++;
					Histogram[b] = value;
				}
			}
			if (Histogram.Count == 0)
			{
				return byte.MaxValue;
			}
			byte result = 0;
			int num = 0;
			foreach (KeyValuePair<byte, int> item in Histogram)
			{
				if (item.Value > num)
				{
					num = item.Value;
					result = item.Key;
				}
			}
			Histogram.Clear();
			return result;
		}

		public unsafe bool AnyAboveIso()
		{
			fixed (byte* ptr = Data)
			{
				for (int i = 0; i < 8; i++)
				{
					if (ptr[i] > 127)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
