using System;
using System.Runtime.CompilerServices;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Serialization
{
	public class BlitSerializer<T> : ISerializer<T> where T : unmanaged
	{
		public unsafe static int StructSize = sizeof(T);

		public static readonly BlitSerializer<T> Default = new BlitSerializer<T>();

		public BlitSerializer()
		{
			MyLibraryUtils.ThrowNonBlittable<T>();
		}

		public unsafe void Serialize(ByteStream destination, ref T data)
		{
			destination.EnsureCapacity(destination.Position + StructSize);
			fixed (byte* destination2 = &destination.Data[destination.Position])
			{
				Unsafe.Copy(destination2, ref data);
			}
			destination.Position += StructSize;
		}

		public unsafe void Deserialize(ByteStream source, out T data)
		{
			source.CheckCapacity(source.Position + StructSize);
			fixed (byte* source2 = &source.Data[source.Position])
			{
				data = default(T);
				Unsafe.Copy(ref data, source2);
			}
			source.Position += StructSize;
		}

		public void SerializeList(ByteStream destination, MyList<T> data)
		{
			int count = data.Count;
			destination.Write7BitEncodedInt(count);
			for (int i = 0; i < count; i++)
			{
				T[] internalArray = data.GetInternalArray();
				Serialize(destination, ref internalArray[i]);
			}
		}

		public void DeserializeList(ByteStream source, MyList<T> resultList)
		{
			int num = source.Read7BitEncodedInt();
			if (resultList.Capacity < num)
			{
				resultList.Capacity = num;
			}
			for (int i = 0; i < num; i++)
			{
				Deserialize(source, out T data);
				resultList.Add(data);
			}
		}
	}
}
