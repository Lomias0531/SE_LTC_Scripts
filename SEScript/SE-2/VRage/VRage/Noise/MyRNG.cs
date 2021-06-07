namespace VRage.Noise
{
	public struct MyRNG
	{
		private const uint MAX_MASK = 2147483647u;

		private const float MAX_MASK_FLOAT = 2.14748365E+09f;

		public uint Seed;

		public MyRNG(int seed = 1)
		{
			Seed = (uint)seed;
		}

		public uint NextInt()
		{
			return Gen();
		}

		public float NextFloat()
		{
			return (float)Gen() / 2.14748365E+09f;
		}

		public int NextIntRange(float min, float max)
		{
			return (int)(min + (max - min) * NextFloat() + 0.5f);
		}

		public float NextFloatRange(float min, float max)
		{
			return min + (max - min) * NextFloat();
		}

		private uint Gen()
		{
			return Seed = ((Seed * 16807) & int.MaxValue);
		}
	}
}
