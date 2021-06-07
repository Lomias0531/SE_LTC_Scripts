using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Serialization
{
	public class MySerializerEnum<TEnum> : MySerializer<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		private static readonly int m_valueCount;

		private static readonly TEnum m_firstValue;

		private static readonly TEnum m_secondValue;

		private static readonly ulong m_firstUlong;

		private static readonly int m_bitCount;

		public static readonly bool HasNegativeValues;

		static MySerializerEnum()
		{
			m_valueCount = MyEnum<TEnum>.Values.Length;
			m_firstValue = MyEnum<TEnum>.Values.FirstOrDefault();
			m_secondValue = MyEnum<TEnum>.Values.Skip(1).FirstOrDefault();
			m_firstUlong = MyEnum<TEnum>.GetValue(m_firstValue);
			m_bitCount = (int)Math.Log(MyEnum<TEnum>.GetValue(MyEnum<TEnum>.Range.Max), 2.0) + 1;
			HasNegativeValues = (Comparer<TEnum>.Default.Compare(MyEnum<TEnum>.Range.Min, default(TEnum)) < 0);
		}

		public override void Clone(ref TEnum value)
		{
		}

		public override bool Equals(ref TEnum a, ref TEnum b)
		{
			return MyEnum<TEnum>.GetValue(a) == MyEnum<TEnum>.GetValue(b);
		}

		public override void Read(BitStream stream, out TEnum value, MySerializeInfo info)
		{
			if (m_valueCount == 1)
			{
				value = m_firstValue;
			}
			else if (m_valueCount == 2)
			{
				value = (stream.ReadBool() ? m_firstValue : m_secondValue);
			}
			else if (m_valueCount > 2)
			{
				if (HasNegativeValues)
				{
					value = MyEnum<TEnum>.SetValue((ulong)stream.ReadInt64Variant());
				}
				else
				{
					value = MyEnum<TEnum>.SetValue(stream.ReadUInt64(m_bitCount));
				}
			}
			else
			{
				value = default(TEnum);
			}
		}

		public override void Write(BitStream stream, ref TEnum value, MySerializeInfo info)
		{
			ulong value2 = MyEnum<TEnum>.GetValue(value);
			if (m_valueCount == 2)
			{
				stream.WriteBool(value2 == m_firstUlong);
			}
			else if (m_valueCount > 2)
			{
				if (HasNegativeValues)
				{
					stream.WriteVariantSigned((long)value2);
				}
				else
				{
					stream.WriteUInt64(value2, m_bitCount);
				}
			}
		}
	}
}
