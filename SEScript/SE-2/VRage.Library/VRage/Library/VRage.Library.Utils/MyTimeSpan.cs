using System;

namespace VRage.Library.Utils
{
	/// <summary>
	/// Hi-resolution time span. Beware: the resolution can be different on different systems!
	/// </summary>
	public struct MyTimeSpan
	{
		public static readonly MyTimeSpan Zero = default(MyTimeSpan);

		public static readonly MyTimeSpan MaxValue = new MyTimeSpan(long.MaxValue);

		public readonly long Ticks;

		public double Nanoseconds => (double)Ticks / ((double)MyGameTimer.Frequency / 1000000000.0);

		public double Microseconds => (double)Ticks / ((double)MyGameTimer.Frequency / 1000000.0);

		public double Milliseconds => (double)Ticks / ((double)MyGameTimer.Frequency / 1000.0);

		public double Seconds => (double)Ticks / (double)MyGameTimer.Frequency;

		public double Minutes => (double)Ticks / (double)((float)MyGameTimer.Frequency * 60f);

		/// <summary>
		/// This may not be accurate for large values - double accuracy
		/// </summary>
		public TimeSpan TimeSpan => TimeSpan.FromTicks((long)Math.Round((double)Ticks * (10000000.0 / (double)MyGameTimer.Frequency)));

		public MyTimeSpan(long stopwatchTicks)
		{
			Ticks = stopwatchTicks;
		}

		public override bool Equals(object obj)
		{
			return Ticks == ((MyTimeSpan)obj).Ticks;
		}

		public override int GetHashCode()
		{
			return Ticks.GetHashCode();
		}

		public static MyTimeSpan FromTicks(long ticks)
		{
			return new MyTimeSpan(ticks);
		}

		public static MyTimeSpan FromSeconds(double seconds)
		{
			return new MyTimeSpan((long)(seconds * (double)MyGameTimer.Frequency));
		}

		public static MyTimeSpan FromMinutes(double minutes)
		{
			return FromSeconds(minutes * 60.0);
		}

		public static MyTimeSpan FromMilliseconds(double milliseconds)
		{
			return FromSeconds(milliseconds * 0.001);
		}

		public static MyTimeSpan operator +(MyTimeSpan a, MyTimeSpan b)
		{
			return new MyTimeSpan(a.Ticks + b.Ticks);
		}

		public static MyTimeSpan operator -(MyTimeSpan a, MyTimeSpan b)
		{
			return new MyTimeSpan(a.Ticks - b.Ticks);
		}

		public static bool operator !=(MyTimeSpan a, MyTimeSpan b)
		{
			return a.Ticks != b.Ticks;
		}

		public static bool operator ==(MyTimeSpan a, MyTimeSpan b)
		{
			return a.Ticks == b.Ticks;
		}

		public static bool operator >(MyTimeSpan a, MyTimeSpan b)
		{
			return a.Ticks > b.Ticks;
		}

		public static bool operator <(MyTimeSpan a, MyTimeSpan b)
		{
			return a.Ticks < b.Ticks;
		}

		public static bool operator >=(MyTimeSpan a, MyTimeSpan b)
		{
			return a.Ticks >= b.Ticks;
		}

		public static bool operator <=(MyTimeSpan a, MyTimeSpan b)
		{
			return a.Ticks <= b.Ticks;
		}

		public override string ToString()
		{
			return ((int)Math.Round(Milliseconds)).ToString();
		}
	}
}
