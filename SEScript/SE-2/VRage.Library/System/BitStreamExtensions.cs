using System.Collections.Generic;
using VRage.Library.Collections;

namespace System
{
	public static class BitStreamExtensions
	{
		public delegate void SerializeCallback<T>(BitStream stream, ref T item);

		public delegate T Reader<T>(BitStream bs);

		public delegate void Writer<T>(BitStream bs, T value);

		private static void Serialize<T>(this BitStream bs, T[] data, int len, SerializeCallback<T> serializer)
		{
			for (int i = 0; i < len; i++)
			{
				serializer(bs, ref data[i]);
			}
		}

		public static void SerializeList<T>(this BitStream bs, ref List<T> list, SerializeCallback<T> serializer)
		{
			if (bs.Writing)
			{
				bs.WriteVariant((uint)list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					T item = list[i];
					serializer(bs, ref item);
				}
				return;
			}
			T item2 = default(T);
			int num = (int)bs.ReadUInt32Variant();
			list = (list ?? new List<T>(num));
			list.Clear();
			for (int j = 0; j < num; j++)
			{
				serializer(bs, ref item2);
				list.Add(item2);
			}
		}

		public static void SerializeList<T>(this BitStream bs, ref List<T> list, Reader<T> reader, Writer<T> writer)
		{
			if (bs.Writing)
			{
				bs.WriteVariant((uint)list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					writer(bs, list[i]);
				}
				return;
			}
			int num = (int)bs.ReadUInt32Variant();
			list = (list ?? new List<T>(num));
			list.Clear();
			for (int j = 0; j < num; j++)
			{
				list.Add(reader(bs));
			}
		}

		public static void SerializeList(this BitStream bs, ref List<int> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadInt32(), delegate(BitStream b, int v)
			{
				b.WriteInt32(v);
			});
		}

		public static void SerializeList(this BitStream bs, ref List<uint> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadUInt32(), delegate(BitStream b, uint v)
			{
				b.WriteUInt32(v);
			});
		}

		public static void SerializeList(this BitStream bs, ref List<long> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadInt64(), delegate(BitStream b, long v)
			{
				b.WriteInt64(v);
			});
		}

		public static void SerializeList(this BitStream bs, ref List<ulong> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadUInt64(), delegate(BitStream b, ulong v)
			{
				b.WriteUInt64(v);
			});
		}

		public static void SerializeListVariant(this BitStream bs, ref List<int> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadInt32Variant(), delegate(BitStream b, int v)
			{
				b.WriteVariantSigned(v);
			});
		}

		public static void SerializeListVariant(this BitStream bs, ref List<uint> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadUInt32Variant(), delegate(BitStream b, uint v)
			{
				b.WriteVariant(v);
			});
		}

		public static void SerializeListVariant(this BitStream bs, ref List<long> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadInt64Variant(), delegate(BitStream b, long v)
			{
				b.WriteVariantSigned(v);
			});
		}

		public static void SerializeListVariant(this BitStream bs, ref List<ulong> list)
		{
			bs.SerializeList(ref list, (BitStream b) => b.ReadUInt64Variant(), delegate(BitStream b, ulong v)
			{
				b.WriteVariant(v);
			});
		}
	}
}
