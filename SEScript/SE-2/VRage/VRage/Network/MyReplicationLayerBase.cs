using System;
using System.Collections.Generic;
using System.Reflection;
using VRageMath;

namespace VRage.Network
{
	public abstract class MyReplicationLayerBase
	{
		private static DBNull e = DBNull.Value;

		protected readonly MyTypeTable m_typeTable = new MyTypeTable();

		protected EndpointId m_localEndpoint;

		public DateTime LastMessageFromServer
		{
			get;
			protected set;
		}

		public void SetLocalEndpoint(EndpointId localEndpoint)
		{
			m_localEndpoint = localEndpoint;
		}

		public Type GetType(TypeId id)
		{
			return m_typeTable.Get(id).Type;
		}

		public TypeId GetTypeId(Type id)
		{
			return m_typeTable.Get(id).TypeId;
		}

		protected static bool ShouldServerInvokeLocally(CallSite site, EndpointId localClientEndpoint, EndpointId recipient)
		{
			if (!site.HasServerFlag)
			{
				if (recipient == localClientEndpoint)
				{
					if (!site.HasClientFlag)
					{
						return site.HasBroadcastFlag;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private bool TryGetInstanceCallSite<T>(Func<T, Delegate> callSiteGetter, T arg, out CallSite site)
		{
			return m_typeTable.Get(arg.GetType()).EventTable.TryGet(callSiteGetter, callSiteGetter, arg, out site);
		}

		private bool TryGetStaticCallSite<T>(Func<T, Delegate> callSiteGetter, out CallSite site)
		{
			return m_typeTable.StaticEventTable.TryGet(callSiteGetter, callSiteGetter, default(T), out site);
		}

		private CallSite GetCallSite<T>(Func<T, Delegate> callSiteGetter, T arg)
		{
			CallSite site;
			if (arg == null)
			{
				TryGetStaticCallSite(callSiteGetter, out site);
			}
			else
			{
				TryGetInstanceCallSite(callSiteGetter, arg, out site);
			}
			if (site == null)
			{
				MethodInfo method = callSiteGetter(arg).Method;
				if (!method.HasAttribute<EventAttribute>())
				{
					throw new InvalidOperationException($"Event '{method.Name}' in type '{method.DeclaringType.Name}' is missing attribute '{typeof(EventAttribute).Name}'");
				}
				if (!method.DeclaringType.HasAttribute<StaticEventOwnerAttribute>() && !typeof(IMyEventProxy).IsAssignableFrom(method.DeclaringType) && !typeof(IMyNetObject).IsAssignableFrom(method.DeclaringType))
				{
					throw new InvalidOperationException($"Event '{method.Name}' is defined in type '{method.DeclaringType.Name}', which does not implement '{typeof(IMyEventOwner).Name}' or '{typeof(IMyNetObject).Name}' or has attribute '{typeof(StaticEventOwnerAttribute).Name}'");
				}
				throw new InvalidOperationException($"Event '{method.Name}' not found, is declaring type '{method.DeclaringType.Name}' registered within replication layer?");
			}
			return site;
		}

		public void RaiseEvent<T1, T2>(T1 arg1, T2 arg2, Func<T1, Action> action, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T2 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref e, ref e, ref e, ref e, ref e, ref e, ref arg2);
		}

		public void RaiseEvent<T1, T2, T3>(T1 arg1, T3 arg3, Func<T1, Action<T2>> action, T2 arg2, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T3 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref arg2, ref e, ref e, ref e, ref e, ref e, ref arg3);
		}

		public void RaiseEvent<T1, T2, T3, T4>(T1 arg1, T4 arg4, Func<T1, Action<T2, T3>> action, T2 arg2, T3 arg3, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T4 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref e, ref e, ref e, ref e, ref arg4);
		}

		public void RaiseEvent<T1, T2, T3, T4, T5>(T1 arg1, T5 arg5, Func<T1, Action<T2, T3, T4>> action, T2 arg2, T3 arg3, T4 arg4, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T5 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref e, ref e, ref e, ref arg5);
		}

		public void RaiseEvent<T1, T2, T3, T4, T5, T6>(T1 arg1, T6 arg6, Func<T1, Action<T2, T3, T4, T5>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T6 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref e, ref e, ref arg6);
		}

		public void RaiseEvent<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T7 arg7, Func<T1, Action<T2, T3, T4, T5, T6>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T7 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref e, ref arg7);
		}

		public void RaiseEvent<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T8 arg8, Func<T1, Action<T2, T3, T4, T5, T6, T7>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, EndpointId endpointId = default(EndpointId), Vector3D? position = null) where T1 : IMyEventOwner where T8 : IMyEventOwner
		{
			DispatchEvent(GetCallSite(action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8);
		}

		protected abstract void DispatchEvent<T1, T2, T3, T4, T5, T6, T7, T8>(CallSite callSite, EndpointId recipient, Vector3D? position, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7, ref T8 arg8) where T1 : IMyEventOwner where T8 : IMyEventOwner;

		/// <summary>
		/// Invokes event locally without validation and with empty Sender and ClientData.
		/// </summary>
		internal void InvokeLocally<T1, T2, T3, T4, T5, T6, T7>(CallSite<T1, T2, T3, T4, T5, T6, T7> site, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			using (MyEventContext.Set(m_localEndpoint, null, isInvokedLocally: true))
			{
				site.Invoke(in arg1, in arg2, in arg3, in arg4, in arg5, in arg6, in arg7);
			}
		}

		public void RegisterFromAssembly(IEnumerable<Assembly> assemblies)
		{
			foreach (Assembly assembly in assemblies)
			{
				RegisterFromAssembly(assembly);
			}
		}

		public void RegisterFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (MyTypeTable.ShouldRegister(type))
				{
					m_typeTable.Register(type);
				}
			}
		}

		public abstract void AdvanceSyncTime();
	}
}
