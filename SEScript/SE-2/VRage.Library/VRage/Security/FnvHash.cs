namespace VRage.Security
{
	public static class FnvHash
	{
		private const uint InitialFNV = 2166136261u;

		private const uint FNVMultiple = 16777619u;

		public static uint Compute(string s)
		{
			uint num = 2166136261u;
			for (int i = 0; i < s.Length; i++)
			{
				num ^= s[i];
				num *= 16777619;
			}
			return num;
		}

		public static uint ComputeAscii(string s)
		{
			uint num = 2166136261u;
			for (int i = 0; i < s.Length; i++)
			{
				num ^= (byte)s[i];
				num *= 16777619;
			}
			return num;
		}
	}
}
