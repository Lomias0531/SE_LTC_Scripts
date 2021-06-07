using System;
using System.Runtime.InteropServices;
using VRage.Library.Threading;
using VRage.Library.Utils;

namespace VRage.Stats
{
	internal class MyStat
	{
		[StructLayout(LayoutKind.Explicit)]
		internal struct Value
		{
			[FieldOffset(0)]
			public float AsFloat;

			[FieldOffset(0)]
			public long AsLong;
		}

		private int m_priority;

		public string DrawText;

		private MyStatTypeEnum Type;

		private int RefreshRate;

		private int ClearRate;

		private int NumDecimals;

		private MyTimeSpan LastRefresh;

		private MyTimeSpan LastClear;

		private Value Sum;

		private int Count;

		private Value Min;

		private Value Max;

		private Value Last;

		private Value Last2;

		private Value DrawSum;

		private int DrawCount;

		private Value DrawMin;

		private Value DrawMax;

		private Value DrawLast;

		private Value DrawLast2;

		private SpinLock Lock;

		public int Priority => m_priority;

		public MyStat(int priority)
		{
			m_priority = priority;
		}

		private int DeltaTimeToInt(MyTimeSpan delta)
		{
			return (int)(1000.0 * delta.Milliseconds + 0.5);
		}

		private MyTimeSpan IntToDeltaTime(int v)
		{
			return MyTimeSpan.FromMilliseconds((float)v / 1000f);
		}

		public void ReadAndClear(MyTimeSpan currentTime, out Value sum, out int count, out Value min, out Value max, out Value last, out MyStatTypeEnum type, out int decimals, out MyTimeSpan inactivityMs, out Value last2)
		{
			Lock.Enter();
			try
			{
				inactivityMs = MyTimeSpan.Zero;
				if (Count <= 0)
				{
					MyTimeSpan myTimeSpan = IntToDeltaTime(-Count);
					myTimeSpan += ((Count < 0) ? (currentTime - LastClear) : MyTimeSpan.FromMilliseconds(1.0));
					Count = -DeltaTimeToInt(myTimeSpan);
					inactivityMs = myTimeSpan;
					LastClear = currentTime;
				}
				else
				{
					if (currentTime >= LastRefresh + MyTimeSpan.FromMilliseconds(RefreshRate))
					{
						DrawSum = Sum;
						DrawCount = Count;
						DrawMin = Min;
						DrawMax = Max;
						DrawLast = Last;
						DrawLast2 = Last2;
						LastRefresh = currentTime;
						if (ClearRate == -1)
						{
							Count = 0;
							ClearUnsafe();
						}
					}
					if (ClearRate != -1 && currentTime >= LastClear + MyTimeSpan.FromMilliseconds(ClearRate))
					{
						Count = 0;
						ClearUnsafe();
						LastClear = currentTime;
					}
				}
				type = Type;
				decimals = NumDecimals;
			}
			finally
			{
				Lock.Exit();
			}
			sum = DrawSum;
			count = DrawCount;
			min = DrawMin;
			max = DrawMax;
			last = DrawLast;
			last2 = DrawLast2;
		}

		public void Clear()
		{
			Lock.Enter();
			try
			{
				ClearUnsafe();
				if (Count > 0)
				{
					Count = 0;
				}
				LastRefresh = MyTimeSpan.Zero;
			}
			finally
			{
				Lock.Exit();
			}
		}

		public void ChangeSettings(MyStatTypeEnum type, int refreshRate, int numDecimals, int clearRate)
		{
			Lock.Enter();
			try
			{
				ChangeSettingsUnsafe(type, refreshRate, numDecimals, clearRate);
			}
			finally
			{
				Lock.Exit();
			}
		}

		public void Write(long value, MyStatTypeEnum type, int refreshRate, int numDecimals, int clearRate)
		{
			Lock.Enter();
			try
			{
				ChangeSettingsUnsafe(type | MyStatTypeEnum.LongFlag, refreshRate, numDecimals, clearRate);
				WriteUnsafe(value);
			}
			finally
			{
				Lock.Exit();
			}
		}

		public void Write(float value, MyStatTypeEnum type, int refreshRate, int numDecimals, int clearRate, float value2 = 0f)
		{
			Lock.Enter();
			try
			{
				ChangeSettingsUnsafe(type & (MyStatTypeEnum)191, refreshRate, numDecimals, clearRate);
				WriteUnsafe(value);
				WriteUnsafe2(value2);
			}
			finally
			{
				Lock.Exit();
			}
		}

		public void Write(float value)
		{
			Lock.Enter();
			try
			{
				WriteUnsafe(value);
			}
			finally
			{
				Lock.Exit();
			}
		}

		private void ChangeSettingsUnsafe(MyStatTypeEnum type, int refreshRate, int numDecimals, int clearRate)
		{
			Type = type;
			RefreshRate = refreshRate;
			ClearRate = clearRate;
			NumDecimals = numDecimals;
		}

		private void WriteUnsafe(float value)
		{
			Last.AsFloat = value;
			Count = Math.Max(1, Count + 1);
			Sum.AsFloat += value;
			Min.AsFloat = Math.Min(Min.AsFloat, value);
			Max.AsFloat = Math.Max(Max.AsFloat, value);
		}

		private void WriteUnsafe2(float value)
		{
			Last2.AsFloat = value;
		}

		private void WriteUnsafe(long value)
		{
			Last.AsLong = value;
			Count = Math.Max(1, Count + 1);
			Sum.AsLong += value;
			Min.AsLong = Math.Min(Min.AsLong, value);
			Max.AsLong = Math.Max(Max.AsLong, value);
		}

		private void ClearUnsafe()
		{
			if ((Type & MyStatTypeEnum.LongFlag) == MyStatTypeEnum.LongFlag)
			{
				Sum.AsLong = 0L;
				Min.AsLong = long.MaxValue;
				Max.AsLong = long.MinValue;
				Last.AsLong = 0L;
			}
			else
			{
				Sum.AsFloat = 0f;
				Min.AsFloat = float.MaxValue;
				Max.AsFloat = float.MinValue;
				Last.AsFloat = 0f;
			}
		}
	}
}
