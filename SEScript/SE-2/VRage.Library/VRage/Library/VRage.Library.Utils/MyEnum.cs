using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Compiler;

namespace VRage.Library.Utils
{
	public static class MyEnum<T> where T : struct, IComparable, IFormattable, IConvertible
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Range
		{
			public static readonly T Min;

			public static readonly T Max;

			static Range()
			{
				T[] values = MyEnum<T>.Values;
				Comparer<T> @default = Comparer<T>.Default;
				if (values.Length == 0)
				{
					return;
				}
				Max = values[0];
				Min = values[0];
				for (int i = 1; i < values.Length; i++)
				{
					T val = values[i];
					if (@default.Compare(Max, val) < 0)
					{
						Max = val;
					}
					if (@default.Compare(Min, val) > 0)
					{
						Min = val;
					}
				}
			}
		}

		public static readonly T[] Values = (T[])Enum.GetValues(typeof(T));

		public static readonly Type UnderlyingType = typeof(T).UnderlyingSystemType;

		/// <summary>
		/// Cached strings to avoid ToString() calls. These values are not readable in obfuscated builds!
		/// </summary>
		private static readonly Dictionary<int, string> m_names = new Dictionary<int, string>();

		public static string Name => TypeNameHelper<T>.Name;

		public static string GetName(T value)
		{
			int key = Array.IndexOf(Values, value);
			if (!m_names.TryGetValue(key, out string value2))
			{
				value2 = value.ToString();
				m_names[key] = value2;
			}
			return value2;
		}

		public unsafe static ulong GetValue(T value)
		{
			void* source = Unsafe.AsPointer(ref value);
			switch (TypeExtensions.SizeOf<T>())
			{
			case 1:
				return Unsafe.ReadUnaligned<byte>(source);
			case 2:
				return Unsafe.ReadUnaligned<ushort>(source);
			case 4:
				return Unsafe.ReadUnaligned<uint>(source);
			default:
				return Unsafe.ReadUnaligned<ulong>(source);
			}
		}

		public unsafe static T SetValue(ulong value)
		{
			T value2 = default(T);
			void* destination = Unsafe.AsPointer(ref value2);
			switch (TypeExtensions.SizeOf<T>())
			{
			case 1:
				Unsafe.WriteUnaligned(destination, (byte)value);
				break;
			case 2:
				Unsafe.WriteUnaligned(destination, (ushort)value);
				break;
			case 4:
				Unsafe.WriteUnaligned(destination, (uint)value);
				break;
			default:
				Unsafe.WriteUnaligned(destination, value);
				break;
			}
			return value2;
		}
	}
}
