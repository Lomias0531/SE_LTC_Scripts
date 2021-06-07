using System;
using System.Runtime.InteropServices;
using VRage.Library.Native;

namespace VRage.XboxLive.Native
{
	public class ContextualCallback : IDisposable
	{
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void Delegate0(IntPtr context);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void Delegate1(IntPtr context, IntPtr data1);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void Delegate2(IntPtr context, IntPtr data1, IntPtr data2);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void Delegate3(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void Delegate4(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void Delegate5(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4, IntPtr data5);

		protected const CallingConvention CALLING_CONVENTION = CallingConvention.StdCall;

		protected static readonly IntPtr CallbackPtr0;

		protected static readonly IntPtr CallbackPtr1;

		protected static readonly IntPtr CallbackPtr2;

		protected static readonly IntPtr CallbackPtr3;

		protected static readonly IntPtr CallbackPtr4;

		protected static readonly IntPtr CallbackPtr5;

		private static readonly Delegate0 CallbackKeeper0;

		private static readonly Delegate1 CallbackKeeper1;

		private static readonly Delegate2 CallbackKeeper2;

		private static readonly Delegate3 CallbackKeeper3;

		private static readonly Delegate4 CallbackKeeper4;

		private static readonly Delegate5 CallbackKeeper5;

		private GCHandle? m_pinHandle;

		public IntPtr Context => (IntPtr)m_pinHandle.Value;

		[MonoPInvokeCallback(typeof(Delegate0))]
		private static void NativeCallback(IntPtr context)
		{
			ContextualCallback contextualCallback = (ContextualCallback)((GCHandle)context).Target;
			if (contextualCallback.Callback())
			{
				contextualCallback.Dispose();
			}
		}

		[MonoPInvokeCallback(typeof(Delegate1))]
		private static void NativeCallback(IntPtr context, IntPtr data1)
		{
			ContextualCallback contextualCallback = (ContextualCallback)((GCHandle)context).Target;
			if (contextualCallback.Callback(data1))
			{
				contextualCallback.Dispose();
			}
		}

		[MonoPInvokeCallback(typeof(Delegate2))]
		private static void NativeCallback(IntPtr context, IntPtr data1, IntPtr data2)
		{
			ContextualCallback contextualCallback = (ContextualCallback)((GCHandle)context).Target;
			if (contextualCallback.Callback(data1, data2))
			{
				contextualCallback.Dispose();
			}
		}

		[MonoPInvokeCallback(typeof(Delegate3))]
		private static void NativeCallback(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3)
		{
			ContextualCallback contextualCallback = (ContextualCallback)((GCHandle)context).Target;
			if (contextualCallback.Callback(data1, data2, data3))
			{
				contextualCallback.Dispose();
			}
		}

		[MonoPInvokeCallback(typeof(Delegate4))]
		private static void NativeCallback(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4)
		{
			ContextualCallback contextualCallback = (ContextualCallback)((GCHandle)context).Target;
			if (contextualCallback.Callback(data1, data2, data3, data4))
			{
				contextualCallback.Dispose();
			}
		}

		[MonoPInvokeCallback(typeof(Delegate5))]
		private static void NativeCallback(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4, IntPtr data5)
		{
			ContextualCallback contextualCallback = (ContextualCallback)((GCHandle)context).Target;
			if (contextualCallback.Callback(data1, data2, data3, data4, data5))
			{
				contextualCallback.Dispose();
			}
		}

		protected virtual bool Callback()
		{
			return true;
		}

		protected virtual bool Callback(IntPtr data1)
		{
			return true;
		}

		protected virtual bool Callback(IntPtr data1, IntPtr data2)
		{
			return true;
		}

		protected virtual bool Callback(IntPtr data1, IntPtr data2, IntPtr data3)
		{
			return true;
		}

		protected virtual bool Callback(IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4)
		{
			return true;
		}

		protected virtual bool Callback(IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4, IntPtr data5)
		{
			return true;
		}

		static ContextualCallback()
		{
			CallbackKeeper0 = NativeCallback;
			CallbackPtr0 = Marshal.GetFunctionPointerForDelegate(CallbackKeeper0);
			CallbackKeeper1 = NativeCallback;
			CallbackPtr1 = Marshal.GetFunctionPointerForDelegate(CallbackKeeper1);
			CallbackKeeper2 = NativeCallback;
			CallbackPtr2 = Marshal.GetFunctionPointerForDelegate(CallbackKeeper2);
			CallbackKeeper3 = NativeCallback;
			CallbackPtr3 = Marshal.GetFunctionPointerForDelegate(CallbackKeeper3);
			CallbackKeeper4 = NativeCallback;
			CallbackPtr4 = Marshal.GetFunctionPointerForDelegate(CallbackKeeper4);
			CallbackKeeper5 = NativeCallback;
			CallbackPtr5 = Marshal.GetFunctionPointerForDelegate(CallbackKeeper5);
		}

		protected ContextualCallback()
		{
			m_pinHandle = GCHandle.Alloc(this);
		}

		public void Dispose()
		{
			m_pinHandle?.Free();
			m_pinHandle = null;
		}
	}
}
