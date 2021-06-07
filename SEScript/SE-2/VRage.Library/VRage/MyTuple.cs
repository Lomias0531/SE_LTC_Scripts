using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Network;

namespace VRage
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct MyTuple
	{
		public static MyTuple<T1> Create<T1>(T1 arg1)
		{
			return new MyTuple<T1>(arg1);
		}

		public static MyTuple<T1, T2> Create<T1, T2>(T1 arg1, T2 arg2)
		{
			return new MyTuple<T1, T2>(arg1, arg2);
		}

		public static MyTuple<T1, T2, T3> Create<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
		{
			return new MyTuple<T1, T2, T3>(arg1, arg2, arg3);
		}

		public static MyTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			return new MyTuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
		}

		public static MyTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			return new MyTuple<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
		}

		public static MyTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			return new MyTuple<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		public static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		public static int CombineHashCodes(int h1, int h2, int h3)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2), h3);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), h5);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), CombineHashCodes(h5, h6));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), CombineHashCodes(h5, h6, h7));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), CombineHashCodes(h5, h6, h7, h8));
		}
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MyTuple<T1>
	{
		protected class VRage_MyTuple_00601_003C_003EItem1_003C_003EAccessor : IMemberAccessor<MyTuple<T1>, T1>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1> owner, in T1 value)
			{
				owner.Item1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1> owner, out T1 value)
			{
				value = owner.Item1;
			}
		}

		public T1 Item1;

		public MyTuple(T1 item1)
		{
			Item1 = item1;
		}
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MyTuple<T1, T2>
	{
		protected class VRage_MyTuple_00602_003C_003EItem1_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2>, T1>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2> owner, in T1 value)
			{
				owner.Item1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2> owner, out T1 value)
			{
				value = owner.Item1;
			}
		}

		protected class VRage_MyTuple_00602_003C_003EItem2_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2>, T2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2> owner, in T2 value)
			{
				owner.Item2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2> owner, out T2 value)
			{
				value = owner.Item2;
			}
		}

		public T1 Item1;

		public T2 Item2;

		public MyTuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MyTuple<T1, T2, T3>
	{
		protected class VRage_MyTuple_00603_003C_003EItem1_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3>, T1>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3> owner, in T1 value)
			{
				owner.Item1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3> owner, out T1 value)
			{
				value = owner.Item1;
			}
		}

		protected class VRage_MyTuple_00603_003C_003EItem2_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3>, T2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3> owner, in T2 value)
			{
				owner.Item2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3> owner, out T2 value)
			{
				value = owner.Item2;
			}
		}

		protected class VRage_MyTuple_00603_003C_003EItem3_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3>, T3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3> owner, in T3 value)
			{
				owner.Item3 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3> owner, out T3 value)
			{
				value = owner.Item3;
			}
		}

		public T1 Item1;

		public T2 Item2;

		public T3 Item3;

		public MyTuple(T1 item1, T2 item2, T3 item3)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MyTuple<T1, T2, T3, T4>
	{
		protected class VRage_MyTuple_00604_003C_003EItem1_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4>, T1>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4> owner, in T1 value)
			{
				owner.Item1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4> owner, out T1 value)
			{
				value = owner.Item1;
			}
		}

		protected class VRage_MyTuple_00604_003C_003EItem2_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4>, T2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4> owner, in T2 value)
			{
				owner.Item2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4> owner, out T2 value)
			{
				value = owner.Item2;
			}
		}

		protected class VRage_MyTuple_00604_003C_003EItem3_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4>, T3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4> owner, in T3 value)
			{
				owner.Item3 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4> owner, out T3 value)
			{
				value = owner.Item3;
			}
		}

		protected class VRage_MyTuple_00604_003C_003EItem4_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4>, T4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4> owner, in T4 value)
			{
				owner.Item4 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4> owner, out T4 value)
			{
				value = owner.Item4;
			}
		}

		public T1 Item1;

		public T2 Item2;

		public T3 Item3;

		public T4 Item4;

		public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MyTuple<T1, T2, T3, T4, T5>
	{
		protected class VRage_MyTuple_00605_003C_003EItem1_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5>, T1>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5> owner, in T1 value)
			{
				owner.Item1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5> owner, out T1 value)
			{
				value = owner.Item1;
			}
		}

		protected class VRage_MyTuple_00605_003C_003EItem2_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5>, T2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5> owner, in T2 value)
			{
				owner.Item2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5> owner, out T2 value)
			{
				value = owner.Item2;
			}
		}

		protected class VRage_MyTuple_00605_003C_003EItem3_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5>, T3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5> owner, in T3 value)
			{
				owner.Item3 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5> owner, out T3 value)
			{
				value = owner.Item3;
			}
		}

		protected class VRage_MyTuple_00605_003C_003EItem4_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5>, T4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5> owner, in T4 value)
			{
				owner.Item4 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5> owner, out T4 value)
			{
				value = owner.Item4;
			}
		}

		protected class VRage_MyTuple_00605_003C_003EItem5_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5>, T5>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5> owner, in T5 value)
			{
				owner.Item5 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5> owner, out T5 value)
			{
				value = owner.Item5;
			}
		}

		public T1 Item1;

		public T2 Item2;

		public T3 Item3;

		public T4 Item4;

		public T5 Item5;

		public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
		}
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MyTuple<T1, T2, T3, T4, T5, T6>
	{
		protected class VRage_MyTuple_00606_003C_003EItem1_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T1>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T1 value)
			{
				owner.Item1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T1 value)
			{
				value = owner.Item1;
			}
		}

		protected class VRage_MyTuple_00606_003C_003EItem2_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T2 value)
			{
				owner.Item2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T2 value)
			{
				value = owner.Item2;
			}
		}

		protected class VRage_MyTuple_00606_003C_003EItem3_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T3 value)
			{
				owner.Item3 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T3 value)
			{
				value = owner.Item3;
			}
		}

		protected class VRage_MyTuple_00606_003C_003EItem4_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T4 value)
			{
				owner.Item4 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T4 value)
			{
				value = owner.Item4;
			}
		}

		protected class VRage_MyTuple_00606_003C_003EItem5_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T5>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T5 value)
			{
				owner.Item5 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T5 value)
			{
				value = owner.Item5;
			}
		}

		protected class VRage_MyTuple_00606_003C_003EItem6_003C_003EAccessor : IMemberAccessor<MyTuple<T1, T2, T3, T4, T5, T6>, T6>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, in T6 value)
			{
				owner.Item6 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTuple<T1, T2, T3, T4, T5, T6> owner, out T6 value)
			{
				value = owner.Item6;
			}
		}

		public T1 Item1;

		public T2 Item2;

		public T3 Item3;

		public T4 Item4;

		public T5 Item5;

		public T6 Item6;

		public MyTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
			Item5 = item5;
			Item6 = item6;
		}
	}
}
