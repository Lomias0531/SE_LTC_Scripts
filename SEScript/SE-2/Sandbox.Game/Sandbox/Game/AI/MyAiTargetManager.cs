using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI
{
	[StaticEventOwner]
	[PreloadRequired]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyAiTargetManager : MySessionComponentBase
	{
		public struct ReservedEntityData
		{
			public MyReservedEntityType Type;

			public long EntityId;

			public int LocalId;

			public Vector3I GridPos;

			public long ReservationTimer;

			public MyPlayer.PlayerId ReserverId;
		}

		public struct ReservedAreaData
		{
			public Vector3D WorldPosition;

			public float Radius;

			public MyTimeSpan ReservationTimer;

			public MyPlayer.PlayerId ReserverId;
		}

		public delegate void ReservationHandler(ref ReservedEntityData entityData, bool success);

		public delegate void AreaReservationHandler(ref ReservedAreaData entityData, bool success);

		protected sealed class OnReserveEntityRequest_003C_003ESystem_Int64_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in long reservationTimeMs, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveEntityRequest(entityId, reservationTimeMs, senderSerialId);
			}
		}

		protected sealed class OnReserveEntitySuccess_003C_003ESystem_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int senderSerialId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveEntitySuccess(entityId, senderSerialId);
			}
		}

		protected sealed class OnReserveEntityFailure_003C_003ESystem_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int senderSerialId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveEntityFailure(entityId, senderSerialId);
			}
		}

		protected sealed class OnReserveEnvironmentItemRequest_003C_003ESystem_Int64_0023System_Int32_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, long, int, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int localId, in long reservationTimeMs, in int senderSerialId, in DBNull arg5, in DBNull arg6)
			{
				OnReserveEnvironmentItemRequest(entityId, localId, reservationTimeMs, senderSerialId);
			}
		}

		protected sealed class OnReserveEnvironmentItemSuccess_003C_003ESystem_Int64_0023System_Int32_0023System_Int32 : ICallSite<IMyEventOwner, long, int, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int localId, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveEnvironmentItemSuccess(entityId, localId, senderSerialId);
			}
		}

		protected sealed class OnReserveEnvironmentItemFailure_003C_003ESystem_Int64_0023System_Int32_0023System_Int32 : ICallSite<IMyEventOwner, long, int, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int localId, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveEnvironmentItemFailure(entityId, localId, senderSerialId);
			}
		}

		protected sealed class OnReserveVoxelPositionRequest_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, Vector3I, long, int, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3I voxelPosition, in long reservationTimeMs, in int senderSerialId, in DBNull arg5, in DBNull arg6)
			{
				OnReserveVoxelPositionRequest(entityId, voxelPosition, reservationTimeMs, senderSerialId);
			}
		}

		protected sealed class OnReserveVoxelPositionSuccess_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023System_Int32 : ICallSite<IMyEventOwner, long, Vector3I, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3I voxelPosition, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveVoxelPositionSuccess(entityId, voxelPosition, senderSerialId);
			}
		}

		protected sealed class OnReserveVoxelPositionFailure_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023System_Int32 : ICallSite<IMyEventOwner, long, Vector3I, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3I voxelPosition, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveVoxelPositionFailure(entityId, voxelPosition, senderSerialId);
			}
		}

		protected sealed class OnReserveAreaRequest_003C_003ESystem_String_0023VRageMath_Vector3D_0023System_Single_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, string, Vector3D, float, long, int, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string reservationName, in Vector3D position, in float radius, in long reservationTimeMs, in int senderSerialId, in DBNull arg6)
			{
				OnReserveAreaRequest(reservationName, position, radius, reservationTimeMs, senderSerialId);
			}
		}

		protected sealed class OnReserveAreaSuccess_003C_003EVRageMath_Vector3D_0023System_Single_0023System_Int32 : ICallSite<IMyEventOwner, Vector3D, float, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in float radius, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveAreaSuccess(position, radius, senderSerialId);
			}
		}

		protected sealed class OnReserveAreaFailure_003C_003EVRageMath_Vector3D_0023System_Single_0023System_Int32 : ICallSite<IMyEventOwner, Vector3D, float, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in float radius, in int senderSerialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveAreaFailure(position, radius, senderSerialId);
			}
		}

		protected sealed class OnReserveAreaAllSuccess_003C_003ESystem_Int64_0023System_String_0023VRageMath_Vector3D_0023System_Single : ICallSite<IMyEventOwner, long, string, Vector3D, float, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long id, in string reservationName, in Vector3D position, in float radius, in DBNull arg5, in DBNull arg6)
			{
				OnReserveAreaAllSuccess(id, reservationName, position, radius);
			}
		}

		protected sealed class OnReserveAreaCancel_003C_003ESystem_String_0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string reservationName, in long id, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnReserveAreaCancel(reservationName, id);
			}
		}

		private HashSet<MyAiTargetBase> m_aiTargets = new HashSet<MyAiTargetBase>();

		private static Dictionary<KeyValuePair<long, long>, ReservedEntityData> m_reservedEntities;

		private static Dictionary<string, Dictionary<long, ReservedAreaData>> m_reservedAreas;

		private static Queue<KeyValuePair<long, long>> m_removeReservedEntities;

		private static Queue<KeyValuePair<string, long>> m_removeReservedAreas;

		private static long AreaReservationCounter;

		public static MyAiTargetManager Static;

		public override bool IsRequiredByGame => true;

		public static event ReservationHandler OnReservationResult;

		public static event AreaReservationHandler OnAreaReservationResult;

		public bool IsEntityReserved(long entityId, long localId)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			ReservedEntityData value;
			return m_reservedEntities.TryGetValue(new KeyValuePair<long, long>(entityId, localId), out value);
		}

		public bool IsEntityReserved(long entityId)
		{
			return IsEntityReserved(entityId, 0L);
		}

		public void UnreserveEntity(long entityId, long localId)
		{
			if (Sync.IsServer)
			{
				m_reservedEntities.Remove(new KeyValuePair<long, long>(entityId, localId));
			}
		}

		public void UnreserveEntity(long entityId)
		{
			UnreserveEntity(entityId, 0L);
		}

		[Event(null, 94)]
		[Reliable]
		[Server]
		private static void OnReserveEntityRequest(long entityId, long reservationTimeMs, int senderSerialId)
		{
			EndpointId targetEndpoint = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
			bool flag = true;
			KeyValuePair<long, long> key = new KeyValuePair<long, long>(entityId, 0L);
			if (m_reservedEntities.TryGetValue(key, out ReservedEntityData value))
			{
				if (value.ReserverId == new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId))
				{
					value.ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				m_reservedEntities.Add(key, new ReservedEntityData
				{
					EntityId = entityId,
					ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000,
					ReserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId)
				});
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				if (flag)
				{
					OnReserveEntitySuccess(entityId, senderSerialId);
				}
				else
				{
					OnReserveEntityFailure(entityId, senderSerialId);
				}
			}
			else if (flag)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveEntitySuccess, entityId, senderSerialId, targetEndpoint);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveEntityFailure, entityId, senderSerialId, targetEndpoint);
			}
		}

		[Event(null, 137)]
		[Reliable]
		[Client]
		private static void OnReserveEntitySuccess(long entityId, int senderSerialId)
		{
			if (MyAiTargetManager.OnReservationResult != null)
			{
				ReservedEntityData reservedEntityData = default(ReservedEntityData);
				reservedEntityData.Type = MyReservedEntityType.ENTITY;
				reservedEntityData.EntityId = entityId;
				reservedEntityData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedEntityData entityData = reservedEntityData;
				MyAiTargetManager.OnReservationResult(ref entityData, success: true);
			}
		}

		[Event(null, 147)]
		[Reliable]
		[Client]
		private static void OnReserveEntityFailure(long entityId, int senderSerialId)
		{
			if (MyAiTargetManager.OnReservationResult != null)
			{
				ReservedEntityData reservedEntityData = default(ReservedEntityData);
				reservedEntityData.Type = MyReservedEntityType.ENTITY;
				reservedEntityData.EntityId = entityId;
				reservedEntityData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedEntityData entityData = reservedEntityData;
				MyAiTargetManager.OnReservationResult(ref entityData, success: false);
			}
		}

		[Event(null, 157)]
		[Reliable]
		[Server]
		private static void OnReserveEnvironmentItemRequest(long entityId, int localId, long reservationTimeMs, int senderSerialId)
		{
			EndpointId targetEndpoint = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
			bool flag = true;
			KeyValuePair<long, long> key = new KeyValuePair<long, long>(entityId, localId);
			if (m_reservedEntities.TryGetValue(key, out ReservedEntityData value))
			{
				if (value.ReserverId == new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId))
				{
					value.ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				m_reservedEntities.Add(key, new ReservedEntityData
				{
					EntityId = entityId,
					LocalId = localId,
					ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000,
					ReserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId)
				});
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				if (flag)
				{
					OnReserveEnvironmentItemSuccess(entityId, localId, senderSerialId);
				}
				else
				{
					OnReserveEnvironmentItemFailure(entityId, localId, senderSerialId);
				}
			}
			else if (flag)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveEnvironmentItemSuccess, entityId, localId, senderSerialId, targetEndpoint);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveEnvironmentItemFailure, entityId, localId, senderSerialId, targetEndpoint);
			}
		}

		[Event(null, 201)]
		[Reliable]
		[Client]
		private static void OnReserveEnvironmentItemSuccess(long entityId, int localId, int senderSerialId)
		{
			if (MyAiTargetManager.OnReservationResult != null)
			{
				ReservedEntityData reservedEntityData = default(ReservedEntityData);
				reservedEntityData.Type = MyReservedEntityType.ENVIRONMENT_ITEM;
				reservedEntityData.EntityId = entityId;
				reservedEntityData.LocalId = localId;
				reservedEntityData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedEntityData entityData = reservedEntityData;
				MyAiTargetManager.OnReservationResult(ref entityData, success: true);
			}
		}

		[Event(null, 217)]
		[Reliable]
		[Client]
		private static void OnReserveEnvironmentItemFailure(long entityId, int localId, int senderSerialId)
		{
			if (MyAiTargetManager.OnReservationResult != null)
			{
				ReservedEntityData reservedEntityData = default(ReservedEntityData);
				reservedEntityData.Type = MyReservedEntityType.ENVIRONMENT_ITEM;
				reservedEntityData.EntityId = entityId;
				reservedEntityData.LocalId = localId;
				reservedEntityData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedEntityData entityData = reservedEntityData;
				MyAiTargetManager.OnReservationResult(ref entityData, success: false);
			}
		}

		[Event(null, 233)]
		[Reliable]
		[Server]
		private static void OnReserveVoxelPositionRequest(long entityId, Vector3I voxelPosition, long reservationTimeMs, int senderSerialId)
		{
			EndpointId targetEndpoint = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
			bool flag = true;
			MyVoxelBase result = null;
			if (!MySession.Static.VoxelMaps.Instances.TryGetValue(entityId, out result))
			{
				return;
			}
			Vector3I vector3I = result.StorageMax - result.StorageMin;
			KeyValuePair<long, long> key = new KeyValuePair<long, long>(entityId, voxelPosition.X + voxelPosition.Y * vector3I.X + voxelPosition.Z * vector3I.X * vector3I.Y);
			if (m_reservedEntities.TryGetValue(key, out ReservedEntityData value))
			{
				if (value.ReserverId == new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId))
				{
					value.ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				m_reservedEntities.Add(key, new ReservedEntityData
				{
					EntityId = entityId,
					GridPos = voxelPosition,
					ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000,
					ReserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId)
				});
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				if (flag)
				{
					OnReserveVoxelPositionSuccess(entityId, voxelPosition, senderSerialId);
				}
				else
				{
					OnReserveVoxelPositionFailure(entityId, voxelPosition, senderSerialId);
				}
			}
			else if (flag)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveVoxelPositionSuccess, entityId, voxelPosition, senderSerialId, targetEndpoint);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveVoxelPositionFailure, entityId, voxelPosition, senderSerialId, targetEndpoint);
			}
		}

		[Event(null, 284)]
		[Reliable]
		[Client]
		private static void OnReserveVoxelPositionSuccess(long entityId, Vector3I voxelPosition, int senderSerialId)
		{
			if (MyAiTargetManager.OnReservationResult != null)
			{
				ReservedEntityData reservedEntityData = default(ReservedEntityData);
				reservedEntityData.Type = MyReservedEntityType.VOXEL;
				reservedEntityData.EntityId = entityId;
				reservedEntityData.GridPos = voxelPosition;
				reservedEntityData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedEntityData entityData = reservedEntityData;
				MyAiTargetManager.OnReservationResult(ref entityData, success: true);
			}
		}

		[Event(null, 300)]
		[Reliable]
		[Client]
		private static void OnReserveVoxelPositionFailure(long entityId, Vector3I voxelPosition, int senderSerialId)
		{
			if (MyAiTargetManager.OnReservationResult != null)
			{
				ReservedEntityData reservedEntityData = default(ReservedEntityData);
				reservedEntityData.Type = MyReservedEntityType.VOXEL;
				reservedEntityData.EntityId = entityId;
				reservedEntityData.GridPos = voxelPosition;
				reservedEntityData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedEntityData entityData = reservedEntityData;
				MyAiTargetManager.OnReservationResult(ref entityData, success: false);
			}
		}

		public void RequestEntityReservation(long entityId, long reservationTimeMs, int senderSerialId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveEntityRequest, entityId, reservationTimeMs, senderSerialId);
		}

		public void RequestEnvironmentItemReservation(long entityId, int localId, long reservationTimeMs, int senderSerialId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveEnvironmentItemRequest, entityId, localId, reservationTimeMs, senderSerialId);
		}

		public void RequestVoxelPositionReservation(long entityId, Vector3I voxelPosition, long reservationTimeMs, int senderSerialId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveVoxelPositionRequest, entityId, voxelPosition, reservationTimeMs, senderSerialId);
		}

		public void RequestAreaReservation(string reservationName, Vector3D position, float radius, long reservationTimeMs, int senderSerialId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveAreaRequest, reservationName, position, radius, reservationTimeMs, senderSerialId);
		}

		[Event(null, 340)]
		[Reliable]
		[Server]
		private static void OnReserveAreaRequest(string reservationName, Vector3D position, float radius, long reservationTimeMs, int senderSerialId)
		{
			EndpointId targetEndpoint = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
			if (!m_reservedAreas.ContainsKey(reservationName))
			{
				m_reservedAreas.Add(reservationName, new Dictionary<long, ReservedAreaData>());
			}
			Dictionary<long, ReservedAreaData> dictionary = m_reservedAreas[reservationName];
			bool flag = false;
			MyPlayer.PlayerId reserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId);
			foreach (KeyValuePair<long, ReservedAreaData> item in dictionary)
			{
				ReservedAreaData value = item.Value;
				if ((value.WorldPosition - position).LengthSquared() <= (double)(value.Radius * value.Radius))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				dictionary[AreaReservationCounter++] = new ReservedAreaData
				{
					WorldPosition = position,
					Radius = radius,
					ReservationTimer = MySandboxGame.Static.TotalTime + MyTimeSpan.FromMilliseconds(reservationTimeMs),
					ReserverId = reserverId
				};
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveAreaAllSuccess, AreaReservationCounter, reservationName, position, radius);
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					OnReserveAreaSuccess(position, radius, senderSerialId);
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveAreaSuccess, position, radius, senderSerialId, targetEndpoint);
				}
			}
			else if (MyEventContext.Current.IsLocallyInvoked)
			{
				OnReserveAreaFailure(position, radius, senderSerialId);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveAreaFailure, position, radius, senderSerialId, targetEndpoint);
			}
		}

		[Event(null, 395)]
		[Reliable]
		[Client]
		private static void OnReserveAreaSuccess(Vector3D position, float radius, int senderSerialId)
		{
			if (MyAiTargetManager.OnAreaReservationResult != null)
			{
				ReservedAreaData reservedAreaData = default(ReservedAreaData);
				reservedAreaData.WorldPosition = position;
				reservedAreaData.Radius = radius;
				reservedAreaData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedAreaData entityData = reservedAreaData;
				MyAiTargetManager.OnAreaReservationResult(ref entityData, success: true);
			}
		}

		[Event(null, 410)]
		[Reliable]
		[Client]
		private static void OnReserveAreaFailure(Vector3D position, float radius, int senderSerialId)
		{
			if (MyAiTargetManager.OnAreaReservationResult != null)
			{
				ReservedAreaData reservedAreaData = default(ReservedAreaData);
				reservedAreaData.WorldPosition = position;
				reservedAreaData.Radius = radius;
				reservedAreaData.ReserverId = new MyPlayer.PlayerId(0uL, senderSerialId);
				ReservedAreaData entityData = reservedAreaData;
				MyAiTargetManager.OnAreaReservationResult(ref entityData, success: false);
			}
		}

		[Event(null, 425)]
		[Reliable]
		[Broadcast]
		private static void OnReserveAreaAllSuccess(long id, string reservationName, Vector3D position, float radius)
		{
			if (!m_reservedAreas.ContainsKey(reservationName))
			{
				m_reservedAreas[reservationName] = new Dictionary<long, ReservedAreaData>();
			}
			m_reservedAreas[reservationName].Add(id, new ReservedAreaData
			{
				WorldPosition = position,
				Radius = radius
			});
		}

		[Event(null, 434)]
		[Reliable]
		[Broadcast]
		private static void OnReserveAreaCancel(string reservationName, long id)
		{
			if (m_reservedAreas.TryGetValue(reservationName, out Dictionary<long, ReservedAreaData> value))
			{
				value.Remove(id);
			}
		}

		public override void LoadData()
		{
			Static = this;
			m_reservedEntities = new Dictionary<KeyValuePair<long, long>, ReservedEntityData>();
			m_removeReservedEntities = new Queue<KeyValuePair<long, long>>();
			m_removeReservedAreas = new Queue<KeyValuePair<string, long>>();
			m_reservedAreas = new Dictionary<string, Dictionary<long, ReservedAreaData>>();
			MyEntities.OnEntityRemove += OnEntityRemoved;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_aiTargets.Clear();
			MyEntities.OnEntityRemove -= OnEntityRemoved;
			Static = null;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Sync.IsServer)
			{
				foreach (KeyValuePair<KeyValuePair<long, long>, ReservedEntityData> reservedEntity in m_reservedEntities)
				{
					if (Stopwatch.GetTimestamp() > reservedEntity.Value.ReservationTimer)
					{
						m_removeReservedEntities.Enqueue(reservedEntity.Key);
					}
				}
				foreach (KeyValuePair<long, long> removeReservedEntity in m_removeReservedEntities)
				{
					m_reservedEntities.Remove(removeReservedEntity);
				}
				m_removeReservedEntities.Clear();
				foreach (KeyValuePair<string, Dictionary<long, ReservedAreaData>> reservedArea in m_reservedAreas)
				{
					foreach (KeyValuePair<long, ReservedAreaData> item in reservedArea.Value)
					{
						if (MySandboxGame.Static.TotalTime > item.Value.ReservationTimer)
						{
							m_removeReservedAreas.Enqueue(new KeyValuePair<string, long>(reservedArea.Key, item.Key));
						}
					}
				}
				foreach (KeyValuePair<string, long> removeReservedArea in m_removeReservedAreas)
				{
					m_reservedAreas[removeReservedArea.Key].Remove(removeReservedArea.Value);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnReserveAreaCancel, removeReservedArea.Key, removeReservedArea.Value);
				}
				m_removeReservedAreas.Clear();
			}
		}

		public static void AddAiTarget(MyAiTargetBase aiTarget)
		{
			if (Static != null)
			{
				Static.m_aiTargets.Add(aiTarget);
			}
		}

		public static void RemoveAiTarget(MyAiTargetBase aiTarget)
		{
			if (Static != null)
			{
				Static.m_aiTargets.Remove(aiTarget);
			}
		}

		private void OnEntityRemoved(MyEntity entity)
		{
			foreach (MyAiTargetBase aiTarget in m_aiTargets)
			{
				if (aiTarget.TargetEntity == entity)
				{
					aiTarget.UnsetTarget();
				}
			}
		}

		public bool IsInReservedArea(string areaName, Vector3D position)
		{
			Dictionary<long, ReservedAreaData> value = null;
			if (m_reservedAreas.TryGetValue(areaName, out value))
			{
				foreach (ReservedAreaData value2 in value.Values)
				{
					if ((value2.WorldPosition - position).LengthSquared() < (double)(value2.Radius * value2.Radius))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
