using System;
using System.Reflection;
using VRage.Library.Collections;

namespace VRage.Network
{
	public abstract class CallSite
	{
		public readonly MySynchronizedTypeInfo OwnerType;

		public readonly uint Id;

		public readonly MethodInfo MethodInfo;

		public readonly CallSiteFlags CallSiteFlags;

		public readonly ValidationType ValidationFlags;

		public bool HasClientFlag => (CallSiteFlags & CallSiteFlags.Client) == CallSiteFlags.Client;

		public bool HasServerFlag => (CallSiteFlags & CallSiteFlags.Server) == CallSiteFlags.Server;

		public bool HasServerInvokedFlag => (CallSiteFlags & CallSiteFlags.ServerInvoked) == CallSiteFlags.ServerInvoked;

		public bool HasBroadcastFlag => (CallSiteFlags & CallSiteFlags.Broadcast) == CallSiteFlags.Broadcast;

		public bool HasBroadcastExceptFlag => (CallSiteFlags & CallSiteFlags.BroadcastExcept) == CallSiteFlags.BroadcastExcept;

		public bool HasRefreshReplicableFlag => (CallSiteFlags & CallSiteFlags.RefreshReplicable) == CallSiteFlags.RefreshReplicable;

		public bool IsReliable => (CallSiteFlags & CallSiteFlags.Reliable) == CallSiteFlags.Reliable;

		public bool IsBlocking => (CallSiteFlags & CallSiteFlags.Blocking) == CallSiteFlags.Blocking;

		public CallSite(MySynchronizedTypeInfo owner, uint id, MethodInfo info, CallSiteFlags flags, ValidationType validationFlags)
		{
			OwnerType = owner;
			Id = id;
			MethodInfo = info;
			CallSiteFlags = flags;
			ValidationFlags = validationFlags;
		}

		public abstract bool Invoke(BitStream stream, object obj, bool validate);

		public override string ToString()
		{
			return $"{MethodInfo.DeclaringType.Name}.{MethodInfo.Name}";
		}
	}
	internal class CallSite<T1, T2, T3, T4, T5, T6, T7> : CallSite
	{
		public readonly CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7> Handler;

		public readonly SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> Serializer;

		public readonly Func<T1, T2, T3, T4, T5, T6, T7, bool> Validator;

		public CallSite(MySynchronizedTypeInfo owner, uint id, MethodInfo info, CallSiteFlags flags, CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7> handler, SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> serializer, Func<T1, T2, T3, T4, T5, T6, T7, bool> validator, ValidationType validationFlags)
			: base(owner, id, info, flags, validationFlags)
		{
			Handler = handler;
			Serializer = serializer;
			Validator = validator;
		}

		public override bool Invoke(BitStream stream, object obj, bool validate)
		{
			T1 arg = (T1)obj;
			T2 arg2 = default(T2);
			T3 arg3 = default(T3);
			T4 arg4 = default(T4);
			T5 arg5 = default(T5);
			T6 arg6 = default(T6);
			T7 arg7 = default(T7);
			Serializer(arg, stream, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7);
			if (validate && !Validator(arg, arg2, arg3, arg4, arg5, arg6, arg7))
			{
				return false;
			}
			Handler(in arg, in arg2, in arg3, in arg4, in arg5, in arg6, in arg7);
			return true;
		}

		public void Invoke(in T1 arg1, in T2 arg2, in T3 arg3, in T4 arg4, in T5 arg5, in T6 arg6, in T7 arg7)
		{
			Handler(in arg1, in arg2, in arg3, in arg4, in arg5, in arg6, in arg7);
		}
	}
}
