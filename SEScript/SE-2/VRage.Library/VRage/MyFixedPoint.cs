using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage
{
	/// <summary>
	/// Fixed point number represented as 64-bit integer with 6 decimal places (one millionts)
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct MyFixedPoint : IXmlSerializable
	{
		protected class VRage_MyFixedPoint_003C_003ERawValue_003C_003EAccessor : IMemberAccessor<MyFixedPoint, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyFixedPoint owner, in long value)
			{
				owner.RawValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyFixedPoint owner, out long value)
			{
				value = owner.RawValue;
			}
		}

		private const int Places = 6;

		private const int Divider = 1000000;

		private static readonly string FormatSpecifier = "D" + 7;

		private static readonly char[] TrimChars = new char[1]
		{
			'0'
		};

		public static readonly MyFixedPoint MinValue = new MyFixedPoint(long.MinValue);

		public static readonly MyFixedPoint MaxValue = new MyFixedPoint(long.MaxValue);

		public static readonly MyFixedPoint Zero = new MyFixedPoint(0L);

		public static readonly MyFixedPoint SmallestPossibleValue = new MyFixedPoint(1L);

		public static readonly MyFixedPoint MaxIntValue = int.MaxValue;

		public static readonly MyFixedPoint MinIntValue = int.MinValue;

		[ProtoMember(1)]
		public long RawValue;

		private MyFixedPoint(long rawValue)
		{
			RawValue = rawValue;
		}

		/// <summary>
		/// For XmlSerialization, format is 123.456789
		/// </summary>
		public string SerializeString()
		{
			string text = RawValue.ToString(FormatSpecifier);
			string text2 = text.Substring(0, text.Length - 6);
			string text3 = text.Substring(text.Length - 6).TrimEnd(TrimChars);
			if (text3.Length > 0)
			{
				return text2 + "." + text3;
			}
			return text2;
		}

		/// <summary>
		/// For XmlSerialization, format is 123.456789
		/// Handles double and decimal formats too.
		/// </summary>
		public static MyFixedPoint DeserializeStringSafe(string text)
		{
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if ((c < '0' || c > '9') && c != '.' && (c != '-' || i != 0))
				{
					return (MyFixedPoint)double.Parse(text);
				}
			}
			try
			{
				return DeserializeString(text);
			}
			catch
			{
				return (MyFixedPoint)double.Parse(text);
			}
		}

		public static MyFixedPoint DeserializeString(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return default(MyFixedPoint);
			}
			int num = text.IndexOf('.');
			if (num == -1)
			{
				return new MyFixedPoint(long.Parse(text) * 1000000);
			}
			text = text.Replace(".", "");
			text = text.PadRight(num + 1 + 6, '0');
			text = text.Substring(0, num + 6);
			return new MyFixedPoint(long.Parse(text));
		}

		public static explicit operator MyFixedPoint(float d)
		{
			if (d * 1000000f + 0.5f >= 9.223372E+18f)
			{
				return MaxValue;
			}
			if (d * 1000000f + 0.5f <= -9.223372E+18f)
			{
				return MinValue;
			}
			return new MyFixedPoint((long)(d * 1000000f + 0.5f));
		}

		public static explicit operator MyFixedPoint(double d)
		{
			if (d * 1000000.0 + 0.5 >= 9.2233720368547758E+18)
			{
				return MaxValue;
			}
			if (d * 1000000.0 + 0.5 <= -9.2233720368547758E+18)
			{
				return MinValue;
			}
			return new MyFixedPoint((long)(d * 1000000.0 + 0.5));
		}

		public static explicit operator MyFixedPoint(decimal d)
		{
			return new MyFixedPoint((long)(d * 1000000m + 0.5m));
		}

		public static implicit operator MyFixedPoint(int i)
		{
			return new MyFixedPoint((long)i * 1000000L);
		}

		public static explicit operator decimal(MyFixedPoint fp)
		{
			return (decimal)fp.RawValue / 1000000m;
		}

		public static explicit operator float(MyFixedPoint fp)
		{
			return (float)fp.RawValue / 1000000f;
		}

		public static explicit operator double(MyFixedPoint fp)
		{
			return (double)fp.RawValue / 1000000.0;
		}

		public static explicit operator int(MyFixedPoint fp)
		{
			return (int)(fp.RawValue / 1000000);
		}

		public static bool IsIntegral(MyFixedPoint fp)
		{
			return fp.RawValue % 1000000 == 0;
		}

		public static MyFixedPoint Ceiling(MyFixedPoint a)
		{
			a.RawValue = (a.RawValue + 1000000 - 1) / 1000000 * 1000000;
			return a;
		}

		public static MyFixedPoint Floor(MyFixedPoint a)
		{
			a.RawValue = a.RawValue / 1000000 * 1000000;
			return a;
		}

		public static MyFixedPoint Min(MyFixedPoint a, MyFixedPoint b)
		{
			if (!(a < b))
			{
				return b;
			}
			return a;
		}

		public static MyFixedPoint Max(MyFixedPoint a, MyFixedPoint b)
		{
			if (!(a > b))
			{
				return b;
			}
			return a;
		}

		public static MyFixedPoint Round(MyFixedPoint a)
		{
			a.RawValue = (a.RawValue + 500000) / 1000000;
			return a;
		}

		public static MyFixedPoint operator -(MyFixedPoint a)
		{
			return new MyFixedPoint(-a.RawValue);
		}

		public static bool operator <(MyFixedPoint a, MyFixedPoint b)
		{
			return a.RawValue < b.RawValue;
		}

		public static bool operator >(MyFixedPoint a, MyFixedPoint b)
		{
			return a.RawValue > b.RawValue;
		}

		public static bool operator <=(MyFixedPoint a, MyFixedPoint b)
		{
			return a.RawValue <= b.RawValue;
		}

		public static bool operator >=(MyFixedPoint a, MyFixedPoint b)
		{
			return a.RawValue >= b.RawValue;
		}

		public static bool operator ==(MyFixedPoint a, MyFixedPoint b)
		{
			return a.RawValue == b.RawValue;
		}

		public static bool operator !=(MyFixedPoint a, MyFixedPoint b)
		{
			return a.RawValue != b.RawValue;
		}

		public static MyFixedPoint operator +(MyFixedPoint a, MyFixedPoint b)
		{
			a.RawValue += b.RawValue;
			return a;
		}

		public static MyFixedPoint operator -(MyFixedPoint a, MyFixedPoint b)
		{
			a.RawValue -= b.RawValue;
			return a;
		}

		public static MyFixedPoint operator *(MyFixedPoint a, MyFixedPoint b)
		{
			long num = a.RawValue / 1000000;
			long num2 = b.RawValue / 1000000;
			long num3 = a.RawValue % 1000000;
			long num4 = b.RawValue % 1000000;
			return new MyFixedPoint(num * num2 * 1000000 + num3 * num4 / 1000000 + num * num4 + num2 * num3);
		}

		public static MyFixedPoint operator *(MyFixedPoint a, float b)
		{
			return a * (MyFixedPoint)b;
		}

		public static MyFixedPoint operator *(float a, MyFixedPoint b)
		{
			return (MyFixedPoint)a * b;
		}

		public static MyFixedPoint operator *(MyFixedPoint a, int b)
		{
			return a * (MyFixedPoint)b;
		}

		public static MyFixedPoint operator *(int a, MyFixedPoint b)
		{
			return (MyFixedPoint)a * b;
		}

		public static MyFixedPoint AddSafe(MyFixedPoint a, MyFixedPoint b)
		{
			return new MyFixedPoint(AddSafeInternal(a.RawValue, b.RawValue));
		}

		public static MyFixedPoint MultiplySafe(MyFixedPoint a, float b)
		{
			return MultiplySafe(a, (MyFixedPoint)b);
		}

		public static MyFixedPoint MultiplySafe(MyFixedPoint a, int b)
		{
			return MultiplySafe(a, (MyFixedPoint)b);
		}

		public static MyFixedPoint MultiplySafe(float a, MyFixedPoint b)
		{
			return MultiplySafe((MyFixedPoint)a, b);
		}

		public static MyFixedPoint MultiplySafe(int a, MyFixedPoint b)
		{
			return MultiplySafe((MyFixedPoint)a, b);
		}

		public static MyFixedPoint MultiplySafe(MyFixedPoint a, MyFixedPoint b)
		{
			long a2 = a.RawValue / 1000000;
			long num = b.RawValue / 1000000;
			long num2 = a.RawValue % 1000000;
			long num3 = b.RawValue % 1000000;
			long a3 = num2 * num3 / 1000000;
			long b2 = MultiplySafeInternal(a2, num * 1000000);
			long b3 = MultiplySafeInternal(a2, num3);
			long b4 = MultiplySafeInternal(num, num2);
			return new MyFixedPoint(AddSafeInternal(AddSafeInternal(AddSafeInternal(a3, b2), b3), b4));
		}

		private static long MultiplySafeInternal(long a, long b)
		{
			long num = a * b;
			if (b == 0L || num / b == a)
			{
				return num;
			}
			if (Math.Sign(a) * Math.Sign(b) != 1)
			{
				return long.MinValue;
			}
			return long.MaxValue;
		}

		private static long AddSafeInternal(long a, long b)
		{
			int num = Math.Sign(a);
			if (num * Math.Sign(b) != 1)
			{
				return a + b;
			}
			long num2 = a + b;
			if (Math.Sign(num2) == num)
			{
				return num2;
			}
			if (num >= 0)
			{
				return long.MaxValue;
			}
			return long.MinValue;
		}

		public int ToIntSafe()
		{
			if (RawValue > MaxIntValue.RawValue)
			{
				return (int)MaxIntValue;
			}
			if (RawValue < MinIntValue.RawValue)
			{
				return (int)MinIntValue;
			}
			return (int)this;
		}

		public override string ToString()
		{
			return SerializeString();
		}

		public override int GetHashCode()
		{
			return (int)RawValue;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				MyFixedPoint? myFixedPoint = obj as MyFixedPoint?;
				if (myFixedPoint.HasValue)
				{
					return this == myFixedPoint.Value;
				}
			}
			return false;
		}

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string text = reader.ReadInnerXml();
			RawValue = DeserializeStringSafe(text).RawValue;
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(SerializeString());
		}
	}
}
