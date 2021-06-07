using System;
using System.Collections.Generic;

namespace VRage.Serialization
{
	/// <summary>
	/// This is not optimal in terms of allocations, but works fine
	/// </summary>
	public class BlitCollectionSerializer<T, TData> : ISerializer<T> where T : ICollection<TData>, new()where TData : unmanaged
	{
		public static readonly BlitCollectionSerializer<T, TData> Default = new BlitCollectionSerializer<T, TData>();

		public static readonly BlitSerializer<TData> InnerSerializer = BlitSerializer<TData>.Default;

		public void Serialize(ByteStream destination, ref T data)
		{
			destination.Write7BitEncodedInt(data.Count);
			foreach (TData datum in data)
			{
				TData data2 = datum;
				InnerSerializer.Serialize(destination, ref data2);
			}
		}

		public void Deserialize(ByteStream source, out T data)
		{
			data = new T();
			int num = source.Read7BitEncodedInt();
			for (int i = 0; i < num; i++)
			{
				InnerSerializer.Deserialize(source, out TData data2);
				data.Add(data2);
			}
		}
	}
}
