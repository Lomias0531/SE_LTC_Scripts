using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Library;
using VRage.Library.Algorithms;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
	public abstract class MyReplicationLayer : MyReplicationLayerBase, IDisposable, INetObjectResolver
	{
		[Flags]
		private enum NetworkObjectGroup
		{
			None = 0x0,
			Replicable = 0x1,
			StatGroup = 0x2,
			Unknown = 0x4
		}

		private struct NetworkObjectStat
		{
			public int Count;

			public NetworkObjectGroup Group;
		}

		private readonly SequenceIdGenerator m_networkIdGenerator = SequenceIdGenerator.CreateWithStopwatch(TimeSpan.FromSeconds(1800.0), 100000);

		protected readonly bool m_isNetworkAuthority;

		private readonly Dictionary<NetworkId, IMyNetObject> m_networkIDToObject = new Dictionary<NetworkId, IMyNetObject>();

		private readonly Dictionary<IMyNetObject, NetworkId> m_objectToNetworkID = new Dictionary<IMyNetObject, NetworkId>();

		private readonly Dictionary<IMyEventProxy, IMyProxyTarget> m_proxyToTarget = new Dictionary<IMyEventProxy, IMyProxyTarget>();

		private readonly Dictionary<Type, Boxed<NetworkObjectStat>> m_networkObjectStats = new Dictionary<Type, Boxed<NetworkObjectStat>>();

		protected MyReplicablesBase m_replicables;

		private const int TIMESTAMP_CORRECTION_MINIMUM = 10;

		private const float SMOOTH_TIMESTAMP_CORRECTION_AMPLITUDE = 1f;

		public float PingSmoothFactor = 3f;

		private readonly FastResourceLock m_networkObjectLock = new FastResourceLock();

		/// <summary>
		/// Synchonized time. Server frame number. It is increased by one every frame tick.
		/// </summary>
		protected long SyncFrameCounter;

		private readonly Thread m_mainThread;

		private readonly Queue<CallSite> m_lastFiveSites = new Queue<CallSite>(5);

		public bool UseSmoothPing
		{
			get;
			set;
		}

		public bool UseSmoothCorrection
		{
			get;
			set;
		}

		public float SmoothCorrectionAmplitude
		{
			get;
			set;
		}

		public int TimestampCorrectionMinimum
		{
			get;
			set;
		}

		protected MyReplicationLayer(bool isNetworkAuthority, EndpointId localEndpoint, Thread mainThread)
		{
			m_mainThread = mainThread;
			TimestampCorrectionMinimum = 10;
			SmoothCorrectionAmplitude = 1f;
			m_isNetworkAuthority = isNetworkAuthority;
			SetLocalEndpoint(localEndpoint);
		}

		public virtual void Dispose()
		{
			using (m_networkObjectLock.AcquireExclusiveUsing())
			{
				m_networkObjectStats.Clear();
				m_networkIDToObject.Clear();
				m_objectToNetworkID.Clear();
				m_proxyToTarget.Clear();
			}
		}

		/// <summary>
		/// Advance local synchronized time.
		/// </summary>
		public override void AdvanceSyncTime()
		{
			SyncFrameCounter++;
		}

		public virtual void SetPriorityMultiplier(EndpointId id, float priority)
		{
		}

		protected Type GetTypeByTypeId(TypeId typeId)
		{
			return m_typeTable.Get(typeId).Type;
		}

		protected TypeId GetTypeIdByType(Type type)
		{
			return m_typeTable.Get(type).TypeId;
		}

		public bool IsTypeReplicated(Type type)
		{
			if (m_typeTable.TryGet(type, out MySynchronizedTypeInfo typeInfo))
			{
				return typeInfo.IsReplicated;
			}
			return false;
		}

		protected void AddNetworkObjectServer(IMyNetObject obj)
		{
			NetworkId networkID = new NetworkId(m_networkIdGenerator.NextId());
			AddNetworkObject(networkID, obj);
		}

		protected virtual void AddNetworkObject(NetworkId networkID, IMyNetObject obj)
		{
			using (m_networkObjectLock.AcquireExclusiveUsing())
			{
				if (!m_networkIDToObject.TryGetValue(networkID, out IMyNetObject value))
				{
					m_networkIDToObject.Add(networkID, obj);
					m_objectToNetworkID.Add(obj, networkID);
					Type type = obj.GetType();
					if (!m_networkObjectStats.TryGetValue(type, out Boxed<NetworkObjectStat> value2))
					{
						NetworkObjectStat value3 = default(NetworkObjectStat);
						value3.Group = GetNetworkObjectGroup(obj);
						value2 = new Boxed<NetworkObjectStat>(value3);
						m_networkObjectStats[type] = value2;
					}
					value2.BoxedValue.Count++;
					IMyProxyTarget myProxyTarget = obj as IMyProxyTarget;
					if (myProxyTarget != null && myProxyTarget.Target != null && !m_proxyToTarget.ContainsKey(myProxyTarget.Target))
					{
						m_proxyToTarget.Add(myProxyTarget.Target, myProxyTarget);
					}
				}
				else if (obj != null && value != null)
				{
					MyLog.Default.WriteLine("Replicated object already exists adding : " + obj.ToString() + " existing : " + value.ToString() + " id : " + networkID.ToString());
				}
			}
		}

		protected NetworkId RemoveNetworkedObject(IMyNetObject obj)
		{
			using (m_networkObjectLock.AcquireExclusiveUsing())
			{
				if (m_objectToNetworkID.TryGetValue(obj, out NetworkId value))
				{
					RemoveNetworkedObjectInternal(value, obj);
				}
				return value;
			}
		}

		protected virtual void RemoveNetworkedObjectInternal(NetworkId networkID, IMyNetObject obj)
		{
			m_objectToNetworkID.Remove(obj);
			m_networkIDToObject.Remove(networkID);
			IMyProxyTarget myProxyTarget = obj as IMyProxyTarget;
			if (myProxyTarget != null && myProxyTarget.Target != null)
			{
				m_proxyToTarget.Remove(myProxyTarget.Target);
			}
			m_networkObjectStats[obj.GetType()].BoxedValue.Count--;
			m_networkIdGenerator.Return(networkID.Value);
		}

		public bool TryGetNetworkIdByObject(IMyNetObject obj, out NetworkId networkId)
		{
			if (obj == null)
			{
				networkId = NetworkId.Invalid;
				return false;
			}
			using (m_networkObjectLock.AcquireSharedUsing())
			{
				return m_objectToNetworkID.TryGetValue(obj, out networkId);
			}
		}

		public NetworkId GetNetworkIdByObject(IMyNetObject obj)
		{
			if (obj == null)
			{
				return NetworkId.Invalid;
			}
			using (m_networkObjectLock.AcquireSharedUsing())
			{
				return m_objectToNetworkID.GetValueOrDefault(obj, NetworkId.Invalid);
			}
		}

		protected IMyNetObject GetObjectByNetworkId(NetworkId id)
		{
			using (m_networkObjectLock.AcquireSharedUsing())
			{
				return m_networkIDToObject.GetValueOrDefault(id);
			}
		}

		public IMyProxyTarget GetProxyTarget(IMyEventProxy proxy)
		{
			using (m_networkObjectLock.AcquireSharedUsing())
			{
				return m_proxyToTarget.GetValueOrDefault(proxy);
			}
		}

		public abstract void UpdateBefore();

		public abstract void UpdateAfter();

		public abstract void UpdateClientStateGroups();

		public abstract void Simulate();

		public abstract void SendUpdate();

		public abstract MyTimeSpan GetSimulationUpdateTime();

		protected abstract MyPacketDataBitStreamBase GetBitStreamPacketData();

		public abstract void Disconnect();

		public void ReportReplicatedObjects()
		{
			MyStatsGraph.ProfileAdvanced(begin: true);
			MyStatsGraph.Begin("ReportObjects", int.MaxValue, "ReportReplicatedObjects", 253, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
			ReportObjects("Replicable objects", NetworkObjectGroup.Replicable);
			ReportObjects("State groups", NetworkObjectGroup.StatGroup);
			ReportObjects("Unknown net objects", NetworkObjectGroup.Unknown);
			MyStatsGraph.End(null, 0f, "", "{0} B", null, "ReportReplicatedObjects", 257, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
			MyStatsGraph.ProfileAdvanced(begin: false);
		}

		private static NetworkObjectGroup GetNetworkObjectGroup(IMyNetObject obj)
		{
			NetworkObjectGroup networkObjectGroup = NetworkObjectGroup.None;
			if (obj is IMyReplicable)
			{
				networkObjectGroup |= NetworkObjectGroup.Replicable;
			}
			if (obj is IMyStateGroup)
			{
				networkObjectGroup |= NetworkObjectGroup.StatGroup;
			}
			if (networkObjectGroup == NetworkObjectGroup.None)
			{
				networkObjectGroup |= NetworkObjectGroup.Unknown;
			}
			return networkObjectGroup;
		}

		private void ReportObjects(string name, NetworkObjectGroup group)
		{
			using (m_networkObjectLock.AcquireSharedUsing())
			{
				int num = 0;
				MyStatsGraph.Begin(name, int.MaxValue, "ReportObjects", 278, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
				foreach (KeyValuePair<Type, Boxed<NetworkObjectStat>> networkObjectStat in m_networkObjectStats)
				{
					Boxed<NetworkObjectStat> value = networkObjectStat.Value;
					if (value.BoxedValue.Count > 0 && value.BoxedValue.Group.HasFlag(group))
					{
						num += value.BoxedValue.Count;
						MyStatsGraph.Begin(networkObjectStat.Key.Name, int.MaxValue, "ReportObjects", 285, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
						MyStatsGraph.End(value.BoxedValue.Count, 0f, "", "{0:.} x", "", "ReportObjects", 286, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
					}
				}
				MyStatsGraph.End(num, 0f, "", "{0:.} x", "", "ReportObjects", 289, "E:\\Repo1\\Sources\\VRage\\Replication\\MyReplicationLayer.cs");
			}
		}

		public virtual MyClientStateBase GetClientData(Endpoint endpointId)
		{
			return null;
		}

		internal void SerializeTypeTable(BitStream stream)
		{
			m_typeTable.Serialize(stream);
		}

		public void RefreshReplicableHierarchy(IMyReplicable replicable)
		{
			m_replicables.Refresh(replicable);
		}

		/// <summary>
		/// Returns string with current multiplayer status. Use only for debugging.
		/// </summary>
		/// <returns>Already formatted string with current multiplayer status.</returns>
		public virtual string GetMultiplayerStat()
		{
			return "Multiplayer Statistics:" + MyEnvironment.NewLine;
		}

		public abstract void UpdateStatisticsData(int outgoing, int incoming, int tamperred, float gcMemory, float processMemory);

		public abstract MyPacketStatistics ClearClientStatistics();

		public abstract MyPacketStatistics ClearServerStatistics();

		[Conditional("DEBUG")]
		protected void CheckThread()
		{
		}

		/// <summary>
		/// Called when event is raised locally to send it to other peer(s).
		/// Return true to invoke event locally.
		/// </summary>
		/// <remarks>
		/// Invoking event locally is important to be done AFTER event is sent to other peers, 
		/// because invocation can raise another event and order must be preserved.
		/// Local event invocation is done in optimized way without unnecessary deserialization.
		/// </remarks>
		protected abstract bool DispatchEvent(IPacketData stream, CallSite site, EndpointId recipient, IMyNetObject eventInstance, Vector3D? position);

		protected abstract bool DispatchBlockingEvent(IPacketData stream, CallSite site, EndpointId recipient, IMyNetObject eventInstance, Vector3D? position, IMyNetObject blockedNetObject);

		/// <summary>
		/// Called when event is received over network.
		/// Event can be validated, invoked and/or transferred to other peers.
		/// </summary>
		protected abstract void OnEvent(MyPacketDataBitStreamBase data, CallSite site, object obj, IMyNetObject sendAs, Vector3D? position, EndpointId source);

		protected sealed override void DispatchEvent<T1, T2, T3, T4, T5, T6, T7, T8>(CallSite callSite, EndpointId recipient, Vector3D? position, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7, ref T8 arg8)
		{
			uint num = callSite.Id;
			IMyNetObject myNetObject;
			NetworkId networkId;
			if (callSite.MethodInfo.IsStatic)
			{
				myNetObject = null;
				networkId = NetworkId.Invalid;
			}
			else
			{
				if (arg1 == null)
				{
					throw new InvalidOperationException("First argument (the instance on which is event invoked) cannot be null for non-static events");
				}
				if (arg1 is IMyEventProxy)
				{
					myNetObject = GetProxyTarget((IMyEventProxy)(object)arg1);
					if (myNetObject == null)
					{
						string msg = "Raising event on object which is not recognized by replication: " + arg1;
						MyLog.Default.WriteLine(msg);
						return;
					}
					num = (uint)((int)num + m_typeTable.Get(myNetObject.GetType()).EventTable.Count);
					networkId = GetNetworkIdByObject(myNetObject);
				}
				else
				{
					if (!(arg1 is IMyNetObject))
					{
						throw new InvalidOperationException("Instance events may be called only on IMyNetObject or IMyEventProxy");
					}
					myNetObject = (IMyNetObject)(object)arg1;
					networkId = GetNetworkIdByObject(myNetObject);
				}
			}
			NetworkId networkId2 = NetworkId.Invalid;
			IMyNetObject myNetObject2 = null;
			if (arg8 is IMyEventProxy && callSite.IsBlocking)
			{
				myNetObject2 = GetProxyTarget((IMyEventProxy)(object)arg8);
				networkId2 = GetNetworkIdByObject(myNetObject2);
			}
			else
			{
				if (arg8 is IMyEventProxy && !callSite.IsBlocking)
				{
					throw new InvalidOperationException("Rising blocking event but event itself does not have Blocking attribute");
				}
				if (!(arg8 is IMyEventProxy) && callSite.IsBlocking)
				{
					throw new InvalidOperationException("Event contain Blocking attribute but blocked event proxy is not set or raised event is not blocking one");
				}
			}
			CallSite<T1, T2, T3, T4, T5, T6, T7> callSite2 = (CallSite<T1, T2, T3, T4, T5, T6, T7>)callSite;
			MyPacketDataBitStreamBase bitStreamPacketData = GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteNetworkId(networkId);
			bitStreamPacketData.Stream.WriteNetworkId(networkId2);
			bitStreamPacketData.Stream.WriteUInt16((ushort)num);
			bitStreamPacketData.Stream.WriteBool(position.HasValue);
			if (position.HasValue)
			{
				bitStreamPacketData.Stream.Write(position.Value);
			}
			using (MySerializerNetObject.Using(this))
			{
				callSite2.Serializer(arg1, bitStreamPacketData.Stream, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7);
			}
			bitStreamPacketData.Stream.Terminate();
			if (networkId2.IsInvalid ? DispatchEvent(bitStreamPacketData, callSite, recipient, myNetObject, position) : DispatchBlockingEvent(bitStreamPacketData, callSite, recipient, myNetObject, position, myNetObject2))
			{
				InvokeLocally(callSite2, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
		}

		/// <summary>
		/// Reads arguments from stream and invokes event. Returns false when validation failed, otherwise true.
		/// </summary>
		public bool Invoke(CallSite callSite, BitStream stream, object obj, EndpointId source, MyClientStateBase clientState, bool validate)
		{
			using (MySerializerNetObject.Using(this))
			{
				using (MyEventContext.Set(source, clientState, isInvokedLocally: false))
				{
					return callSite.Invoke(stream, obj, validate) && (!validate || !MyEventContext.Current.HasValidationFailed);
				}
			}
		}

		public void OnEvent(MyPacket packet)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = GetBitStreamPacketData();
			bitStreamPacketData.Stream.ResetRead(packet.BitStream);
			ProcessEvent(bitStreamPacketData, packet.Sender.Id);
			packet.Return();
		}

		private void ProcessEvent(MyPacketDataBitStreamBase data, EndpointId sender)
		{
			NetworkId networkId = data.Stream.ReadNetworkId();
			NetworkId blockedNetId = data.Stream.ReadNetworkId();
			uint eventId = data.Stream.ReadUInt16();
			bool num = data.Stream.ReadBool();
			Vector3D? position = null;
			if (num)
			{
				position = data.Stream.ReadVector3D();
			}
			OnEvent(data, networkId, blockedNetId, eventId, sender, position);
		}

		protected virtual void OnEvent(MyPacketDataBitStreamBase data, NetworkId networkId, NetworkId blockedNetId, uint eventId, EndpointId sender, Vector3D? position)
		{
			CallSite callSite;
			IMyNetObject myNetObject;
			object obj;
			try
			{
				if (networkId.IsInvalid)
				{
					callSite = m_typeTable.StaticEventTable.Get(eventId);
					myNetObject = null;
					obj = null;
				}
				else
				{
					myNetObject = GetObjectByNetworkId(networkId);
					if (myNetObject == null || !myNetObject.IsValid)
					{
						return;
					}
					MySynchronizedTypeInfo mySynchronizedTypeInfo = m_typeTable.Get(myNetObject.GetType());
					int count = mySynchronizedTypeInfo.EventTable.Count;
					if (eventId < count)
					{
						obj = myNetObject;
						callSite = mySynchronizedTypeInfo.EventTable.Get(eventId);
					}
					else
					{
						obj = ((IMyProxyTarget)myNetObject).Target;
						mySynchronizedTypeInfo = m_typeTable.Get(obj.GetType());
						callSite = mySynchronizedTypeInfo.EventTable.Get((uint)((int)eventId - count));
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Static: " + !networkId.IsInvalid);
				MyLog.Default.WriteLine("EventId: " + eventId);
				MyLog.Default.WriteLine("Last five sites:");
				foreach (CallSite lastFiveSite in m_lastFiveSites)
				{
					MyLog.Default.WriteLine(lastFiveSite.ToString());
				}
				throw ex;
			}
			if (m_lastFiveSites.Count >= 5)
			{
				m_lastFiveSites.Dequeue();
			}
			m_lastFiveSites.Enqueue(callSite);
			OnEvent(data, callSite, obj, myNetObject, position, sender);
		}

		void INetObjectResolver.Resolve<T>(BitStream stream, ref T obj)
		{
			if (stream.Reading)
			{
				obj = (T)GetObjectByNetworkId(stream.ReadNetworkId());
			}
			else
			{
				stream.WriteNetworkId(TryGetNetworkIdByObject(obj, out NetworkId networkId) ? networkId : NetworkId.Invalid);
			}
		}
	}
}
