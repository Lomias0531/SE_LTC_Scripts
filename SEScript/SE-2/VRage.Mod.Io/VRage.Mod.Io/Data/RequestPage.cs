using System;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class RequestPage<T>
	{
		public int result_limit;

		public int result_offset;

		public int result_total;

		public T[] data;
	}
}
