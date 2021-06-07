using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Network;

namespace VRage.Library.Utils
{
	/// <summary>
	/// Original C# implementation which allows settings the seed.
	/// </summary>
	[Serializable]
	public class MyRandom
	{
		public struct State
		{
			public int Inext;

			public int Inextp;

			public unsafe fixed int Seed[56];
		}

		public struct StateToken : IDisposable
		{
			private MyRandom m_random;

			private State m_state;

			public StateToken(MyRandom random)
			{
				m_random = random;
				random.GetState(out m_state);
			}

			public StateToken(MyRandom random, int newSeed)
			{
				m_random = random;
				random.GetState(out m_state);
				random.SetSeed(newSeed);
			}

			public void Dispose()
			{
				if (m_random != null)
				{
					m_random.SetState(ref m_state);
				}
			}
		}

		protected class VRage_Library_Utils_MyRandom_003C_003Einext_003C_003EAccessor : IMemberAccessor<MyRandom, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRandom owner, in int value)
			{
				owner.inext = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRandom owner, out int value)
			{
				value = owner.inext;
			}
		}

		protected class VRage_Library_Utils_MyRandom_003C_003Einextp_003C_003EAccessor : IMemberAccessor<MyRandom, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRandom owner, in int value)
			{
				owner.inextp = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRandom owner, out int value)
			{
				value = owner.inextp;
			}
		}

		protected class VRage_Library_Utils_MyRandom_003C_003ESeedArray_003C_003EAccessor : IMemberAccessor<MyRandom, int[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRandom owner, in int[] value)
			{
				owner.SeedArray = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRandom owner, out int[] value)
			{
				value = owner.SeedArray;
			}
		}

		protected class VRage_Library_Utils_MyRandom_003C_003Em_tmpLongArray_003C_003EAccessor : IMemberAccessor<MyRandom, byte[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRandom owner, in byte[] value)
			{
				owner.m_tmpLongArray = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRandom owner, out byte[] value)
			{
				value = owner.m_tmpLongArray;
			}
		}

		[ThreadStatic]
		private static MyRandom m_instance;

		private int inext;

		private int inextp;

		private const int MBIG = int.MaxValue;

		private const int MSEED = 161803398;

		private const int MZ = 0;

		private int[] SeedArray;

		private byte[] m_tmpLongArray = new byte[8];

		internal static bool EnableDeterminism;

		public static MyRandom Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyRandom();
				}
				return m_instance;
			}
		}

		public MyRandom()
			: this(MyEnvironment.TickCount + Thread.CurrentThread.ManagedThreadId)
		{
		}

		public MyRandom(int Seed)
		{
			SeedArray = new int[56];
			SetSeed(Seed);
		}

		public StateToken PushSeed(int newSeed)
		{
			return new StateToken(this, newSeed);
		}

		public unsafe void GetState(out State state)
		{
			state.Inext = inext;
			state.Inextp = inextp;
			fixed (int* value = state.Seed)
			{
				Marshal.Copy(SeedArray, 0, new IntPtr(value), 56);
			}
		}

		public unsafe void SetState(ref State state)
		{
			inext = state.Inext;
			inextp = state.Inextp;
			fixed (int* value = state.Seed)
			{
				Marshal.Copy(new IntPtr(value), SeedArray, 0, 56);
			}
		}

		public int CreateRandomSeed()
		{
			return MyEnvironment.TickCount ^ Next();
		}

		/// <summary>
		/// Sets new seed, only use this method when you have separate instance of MyRandom.
		/// Setting seed for RNG used for EntityId without reverting to previous state is dangerous.
		/// Use PushSeed for EntityId random generator.
		/// </summary>
		public void SetSeed(int Seed)
		{
			int num = (Seed == int.MinValue) ? int.MaxValue : Math.Abs(Seed);
			int num2 = 161803398 - num;
			SeedArray[55] = num2;
			int num3 = 1;
			for (int i = 1; i < 55; i++)
			{
				int num4 = 21 * i % 55;
				SeedArray[num4] = num3;
				num3 = num2 - num3;
				if (num3 < 0)
				{
					num3 += int.MaxValue;
				}
				num2 = SeedArray[num4];
			}
			for (int j = 1; j < 5; j++)
			{
				for (int k = 1; k < 56; k++)
				{
					SeedArray[k] -= SeedArray[1 + (k + 30) % 55];
					if (SeedArray[k] < 0)
					{
						SeedArray[k] += int.MaxValue;
					}
				}
			}
			inext = 0;
			inextp = 21;
			Seed = 1;
		}

		private double GetSampleForLargeRange()
		{
			int num = InternalSample();
			if (InternalSample() % 2 == 0)
			{
				num = -num;
			}
			double num2 = num;
			num2 += 2147483646.0;
			return num2 / 4294967293.0;
		}

		private int InternalSample()
		{
			int num = inext;
			int num2 = inextp;
			if (++num >= 56)
			{
				num = 1;
			}
			if (++num2 >= 56)
			{
				num2 = 1;
			}
			int num3 = SeedArray[num] - SeedArray[num2];
			if (num3 == int.MaxValue)
			{
				num3--;
			}
			if (num3 < 0)
			{
				num3 += int.MaxValue;
			}
			SeedArray[num] = num3;
			inext = num;
			inextp = num2;
			return num3;
		}

		public int Next()
		{
			return InternalSample();
		}

		public int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException("maxValue");
			}
			return (int)(Sample() * (double)maxValue);
		}

		public int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue");
			}
			long num = maxValue - minValue;
			if (num <= int.MaxValue)
			{
				return (int)(Sample() * (double)num) + minValue;
			}
			return (int)(long)(GetSampleForLargeRange() * (double)num) + minValue;
		}

		public long NextLong()
		{
			NextBytes(m_tmpLongArray);
			return BitConverter.ToInt64(m_tmpLongArray, 0);
		}

		public void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(InternalSample() % 256);
			}
		}

		/// Returns random number between 0 and 1.
		public float NextFloat()
		{
			return (float)NextDouble();
		}

		/// Returns random number between 0 and 1.
		public double NextDouble()
		{
			return Sample();
		}

		protected double Sample()
		{
			return (double)InternalSample() * 4.6566128752457969E-10;
		}

		public float GetRandomSign()
		{
			return Math.Sign((float)NextDouble() - 0.5f);
		}

		public float GetRandomFloat(float minValue, float maxValue)
		{
			return NextFloat() * (maxValue - minValue) + minValue;
		}
	}
}
