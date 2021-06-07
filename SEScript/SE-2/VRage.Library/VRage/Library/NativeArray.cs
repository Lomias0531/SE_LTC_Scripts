using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VRage.Library
{
	public abstract class NativeArray : IDisposable
	{
		public readonly int Size;

		public readonly IntPtr Ptr;

		protected NativeArray(int size)
		{
			Size = size;
			Ptr = Marshal.AllocHGlobal(size);
		}

		[Conditional("DEBUG")]
		public void UpdateAllocationTrace()
		{
		}

		/// <summary>
		/// Get the contents of this array as a span.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public unsafe Span<T> AsSpan<T>(int length)
		{
			if (length * Unsafe.SizeOf<T>() > Size)
			{
				throw new ArgumentException("Requested length is too long for the native array.");
			}
			return new Span<T>(Ptr.ToPointer(), length);
		}

		public virtual void Dispose()
		{
			Marshal.FreeHGlobal(Ptr);
		}
	}
}
