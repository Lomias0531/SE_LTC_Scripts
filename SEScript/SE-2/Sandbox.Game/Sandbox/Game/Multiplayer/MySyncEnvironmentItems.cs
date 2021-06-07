using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.EnvironmentItems;
using System;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public static class MySyncEnvironmentItems
	{
		protected sealed class OnRemoveEnvironmentItemMessage_003C_003ESystem_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int itemInstanceId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRemoveEnvironmentItemMessage(entityId, itemInstanceId);
			}
		}

		protected sealed class OnModifyModelMessage_003C_003ESystem_Int64_0023System_Int32_0023VRage_Utils_MyStringHash : ICallSite<IMyEventOwner, long, int, MyStringHash, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int instanceId, in MyStringHash subtypeId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnModifyModelMessage(entityId, instanceId, subtypeId);
			}
		}

		protected sealed class OnBeginBatchAddMessage_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnBeginBatchAddMessage(entityId);
			}
		}

		protected sealed class OnBatchAddItemMessage_003C_003ESystem_Int64_0023VRageMath_Vector3D_0023VRage_Utils_MyStringHash : ICallSite<IMyEventOwner, long, Vector3D, MyStringHash, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3D position, in MyStringHash subtypeId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnBatchAddItemMessage(entityId, position, subtypeId);
			}
		}

		protected sealed class OnBatchModifyItemMessage_003C_003ESystem_Int64_0023System_Int32_0023VRage_Utils_MyStringHash : ICallSite<IMyEventOwner, long, int, MyStringHash, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int localId, in MyStringHash subtypeId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnBatchModifyItemMessage(entityId, localId, subtypeId);
			}
		}

		protected sealed class OnBatchRemoveItemMessage_003C_003ESystem_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in int localId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnBatchRemoveItemMessage(entityId, localId);
			}
		}

		protected sealed class OnEndBatchAddMessage_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnEndBatchAddMessage(entityId);
			}
		}

		public static Action<MyEntity, int> OnRemoveEnvironmentItem;

		public static void RemoveEnvironmentItem(long entityId, int itemInstanceId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRemoveEnvironmentItemMessage, entityId, itemInstanceId);
		}

		[Event(null, 29)]
		[Reliable]
		[Server]
		[BroadcastExcept]
		private static void OnRemoveEnvironmentItemMessage(long entityId, int itemInstanceId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
			{
				if (OnRemoveEnvironmentItem != null)
				{
					OnRemoveEnvironmentItem(entity, itemInstanceId);
				}
			}
			else
			{
				_ = MyFakes.ENABLE_FLORA_COMPONENT_DEBUG;
			}
		}

		public static void SendModifyModelMessage(long entityId, int instanceId, MyStringHash subtypeId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnModifyModelMessage, entityId, instanceId, subtypeId);
		}

		[Event(null, 51)]
		[Reliable]
		[Broadcast]
		private static void OnModifyModelMessage(long entityId, int instanceId, MyStringHash subtypeId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEnvironmentItems entity))
			{
				entity.ModifyItemModel(instanceId, subtypeId, updateSector: true, sync: false);
			}
			else
			{
				_ = MyFakes.ENABLE_FLORA_COMPONENT_DEBUG;
			}
		}

		public static void SendBeginBatchAddMessage(long entityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnBeginBatchAddMessage, entityId);
		}

		[Event(null, 72)]
		[Reliable]
		[Broadcast]
		private static void OnBeginBatchAddMessage(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEnvironmentItems entity))
			{
				entity.BeginBatch(sync: false);
			}
		}

		public static void SendBatchAddItemMessage(long entityId, Vector3D position, MyStringHash subtypeId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnBatchAddItemMessage, entityId, position, subtypeId);
		}

		[Event(null, 88)]
		[Reliable]
		[Broadcast]
		private static void OnBatchAddItemMessage(long entityId, Vector3D position, MyStringHash subtypeId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEnvironmentItems entity))
			{
				entity.BatchAddItem(position, subtypeId, sync: false);
			}
		}

		public static void SendBatchModifyItemMessage(long entityId, int localId, MyStringHash subtypeId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnBatchModifyItemMessage, entityId, localId, subtypeId);
		}

		[Event(null, 104)]
		[Reliable]
		[Broadcast]
		private static void OnBatchModifyItemMessage(long entityId, int localId, MyStringHash subtypeId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEnvironmentItems entity))
			{
				entity.BatchModifyItem(localId, subtypeId, sync: false);
			}
		}

		public static void SendBatchRemoveItemMessage(long entityId, int localId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnBatchRemoveItemMessage, entityId, localId);
		}

		[Event(null, 120)]
		[Reliable]
		[Broadcast]
		private static void OnBatchRemoveItemMessage(long entityId, int localId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEnvironmentItems entity))
			{
				entity.BatchRemoveItem(localId, sync: false);
			}
		}

		public static void SendEndBatchAddMessage(long entityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnEndBatchAddMessage, entityId);
		}

		[Event(null, 136)]
		[Reliable]
		[Broadcast]
		private static void OnEndBatchAddMessage(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEnvironmentItems entity))
			{
				entity.EndBatch(sync: false);
			}
		}
	}
}
