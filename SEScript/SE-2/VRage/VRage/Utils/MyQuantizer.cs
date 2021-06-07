using System;

namespace VRage.Utils
{
	internal class MyQuantizer
	{
		private int m_quantizationBits;

		private int m_throwawayBits;

		private int m_minValue;

		private byte[] m_smearBits;

		private uint[] m_bitmask;

		public MyQuantizer(int quantizationBits)
		{
			m_quantizationBits = quantizationBits;
			m_throwawayBits = 8 - m_quantizationBits;
			m_smearBits = new byte[1 << m_quantizationBits];
			for (uint num = 0u; num < 1 << m_quantizationBits; num++)
			{
				uint num2 = num << m_throwawayBits;
				num2 += num2 >> m_quantizationBits;
				if (m_quantizationBits < 4)
				{
					num2 += num2 >> m_quantizationBits * 2;
					if (m_quantizationBits < 2)
					{
						num2 += num2 >> m_quantizationBits * 4;
					}
				}
				m_smearBits[num] = (byte)num2;
			}
			m_bitmask = new uint[8]
			{
				~(255u >> m_throwawayBits),
				~(255u >> m_throwawayBits << 1),
				~(255u >> m_throwawayBits << 2),
				~(255u >> m_throwawayBits << 3),
				~(255u >> m_throwawayBits << 4),
				~(255u >> m_throwawayBits << 5),
				~(255u >> m_throwawayBits << 6),
				~(255u >> m_throwawayBits << 7)
			};
			m_minValue = 1 << m_throwawayBits;
		}

		public byte QuantizeValue(byte val)
		{
			return m_smearBits[val >> m_throwawayBits];
		}

		public void SetAllFromUnpacked(byte[] dstPacked, int dstSize, byte[] srcUnpacked)
		{
			Array.Clear(dstPacked, 0, dstPacked.Length);
			int num = 0;
			int num2 = 0;
			while (num < dstSize * m_quantizationBits)
			{
				int num3 = num >> 3;
				uint num4 = (uint)srcUnpacked[num2] >> m_throwawayBits << (num & 7);
				dstPacked[num3] |= (byte)num4;
				dstPacked[num3 + 1] |= (byte)(num4 >> 8);
				num += m_quantizationBits;
				num2++;
			}
		}

		public void WriteVal(byte[] packed, int idx, byte val)
		{
			int num = idx * m_quantizationBits;
			int num2 = num & 7;
			int num3 = num >> 3;
			uint num4 = (uint)val >> m_throwawayBits << num2;
			packed[num3] = (byte)((packed[num3] & m_bitmask[num2]) | num4);
			packed[num3 + 1] = (byte)((packed[num3 + 1] & (m_bitmask[num2] >> 8)) | (num4 >> 8));
		}

		public byte ReadVal(byte[] packed, int idx)
		{
			int num = idx * m_quantizationBits;
			int num2 = num >> 3;
			uint num3 = (uint)(packed[num2] + (packed[num2 + 1] << 8));
			return m_smearBits[(num3 >> (num & 7)) & (255 >> m_throwawayBits)];
		}

		public int ComputeRequiredPackedSize(int unpackedSize)
		{
			return (unpackedSize * m_quantizationBits + 7) / 8 + 1;
		}

		public int GetMinimumQuantizableValue()
		{
			return m_minValue;
		}
	}
}
