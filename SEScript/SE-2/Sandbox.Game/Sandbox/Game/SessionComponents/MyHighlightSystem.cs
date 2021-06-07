using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyHighlightSystem : MySessionComponentBase
	{
		[Serializable]
		public struct MyHighlightData
		{
			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyHighlightData, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003EOutlineColor_003C_003EAccessor : IMemberAccessor<MyHighlightData, Color?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in Color? value)
				{
					owner.OutlineColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out Color? value)
				{
					value = owner.OutlineColor;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003EThickness_003C_003EAccessor : IMemberAccessor<MyHighlightData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in int value)
				{
					owner.Thickness = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out int value)
				{
					value = owner.Thickness;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003EPulseTimeInFrames_003C_003EAccessor : IMemberAccessor<MyHighlightData, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in ulong value)
				{
					owner.PulseTimeInFrames = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out ulong value)
				{
					value = owner.PulseTimeInFrames;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyHighlightData, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in long value)
				{
					owner.PlayerId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out long value)
				{
					value = owner.PlayerId;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003EIgnoreUseObjectData_003C_003EAccessor : IMemberAccessor<MyHighlightData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in bool value)
				{
					owner.IgnoreUseObjectData = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out bool value)
				{
					value = owner.IgnoreUseObjectData;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EMyHighlightData_003C_003ESubPartNames_003C_003EAccessor : IMemberAccessor<MyHighlightData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyHighlightData owner, in string value)
				{
					owner.SubPartNames = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyHighlightData owner, out string value)
				{
					value = owner.SubPartNames;
				}
			}

			public long EntityId;

			public Color? OutlineColor;

			public int Thickness;

			public ulong PulseTimeInFrames;

			public long PlayerId;

			public bool IgnoreUseObjectData;

			public string SubPartNames;

			public MyHighlightData(long entityId = 0L, int thickness = -1, ulong pulseTimeInFrames = 0uL, Color? outlineColor = null, bool ignoreUseObjectData = false, long playerId = -1L, string subPartNames = null)
			{
				EntityId = entityId;
				Thickness = thickness;
				OutlineColor = outlineColor;
				PulseTimeInFrames = pulseTimeInFrames;
				PlayerId = playerId;
				IgnoreUseObjectData = ignoreUseObjectData;
				SubPartNames = subPartNames;
			}
		}

		[Serializable]
		private struct HighlightMsg
		{
			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg_003C_003EData_003C_003EAccessor : IMemberAccessor<HighlightMsg, MyHighlightData>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref HighlightMsg owner, in MyHighlightData value)
				{
					owner.Data = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref HighlightMsg owner, out MyHighlightData value)
				{
					value = owner.Data;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg_003C_003EExclusiveKey_003C_003EAccessor : IMemberAccessor<HighlightMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref HighlightMsg owner, in int value)
				{
					owner.ExclusiveKey = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref HighlightMsg owner, out int value)
				{
					value = owner.ExclusiveKey;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg_003C_003EIsExclusive_003C_003EAccessor : IMemberAccessor<HighlightMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref HighlightMsg owner, in bool value)
				{
					owner.IsExclusive = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref HighlightMsg owner, out bool value)
				{
					value = owner.IsExclusive;
				}
			}

			public MyHighlightData Data;

			public int ExclusiveKey;

			public bool IsExclusive;
		}

		protected sealed class OnHighlightOnClient_003C_003ESandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg : ICallSite<IMyEventOwner, HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in HighlightMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnHighlightOnClient(msg);
			}
		}

		protected sealed class OnRequestRejected_003C_003ESandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg : ICallSite<IMyEventOwner, HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in HighlightMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRequestRejected(msg);
			}
		}

		protected sealed class OnRequestAccepted_003C_003ESandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg : ICallSite<IMyEventOwner, HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in HighlightMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRequestAccepted(msg);
			}
		}

		private static MyHighlightSystem m_static;

		private static int m_exclusiveKeyCounter = 10;

		private readonly Dictionary<int, long> m_exclusiveKeysToIds = new Dictionary<int, long>();

		private readonly HashSet<long> m_highlightedIds = new HashSet<long>();

		private readonly MyHudSelectedObject m_highlightCalculationHelper = new MyHudSelectedObject();

		private readonly List<uint> m_subPartIndicies = new List<uint>();

		private readonly HashSet<uint> m_highlightOverlappingIds = new HashSet<uint>();

		private StringBuilder m_highlightAttributeBuilder = new StringBuilder();

		public HashSetReader<uint> HighlightOverlappingRenderIds => new HashSetReader<uint>(m_highlightOverlappingIds);

		public event Action<MyHighlightData> HighlightRejected;

		public event Action<MyHighlightData> HighlightAccepted;

		public event Action<MyHighlightData, int> ExclusiveHighlightRejected;

		public event Action<MyHighlightData, int> ExclusiveHighlightAccepted;

		public MyHighlightSystem()
		{
			m_static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_static = null;
		}

		public void RequestHighlightChange(MyHighlightData data)
		{
			ProcessRequest(data, -1, isExclusive: false);
		}

		public void RequestHighlightChangeExclusive(MyHighlightData data, int exclusiveKey = -1)
		{
			ProcessRequest(data, exclusiveKey, isExclusive: true);
		}

		public bool IsHighlighted(long entityId)
		{
			return m_highlightedIds.Contains(entityId);
		}

		public bool IsReserved(long entityId)
		{
			return m_exclusiveKeysToIds.ContainsValue(entityId);
		}

		public void AddHighlightOverlappingModel(uint modelRenderId)
		{
			if (modelRenderId != uint.MaxValue && !m_highlightOverlappingIds.Contains(modelRenderId))
			{
				m_highlightOverlappingIds.Add(modelRenderId);
				MyRenderProxy.UpdateHighlightOverlappingModel(modelRenderId);
			}
		}

		public void RemoveHighlightOverlappingModel(uint modelRenderId)
		{
			if (modelRenderId != uint.MaxValue && m_highlightOverlappingIds.Contains(modelRenderId))
			{
				m_highlightOverlappingIds.Remove(modelRenderId);
				MyRenderProxy.UpdateHighlightOverlappingModel(modelRenderId, enable: false);
			}
		}

		private void ProcessRequest(MyHighlightData data, int exclusiveKey, bool isExclusive)
		{
			if (data.PlayerId == -1)
			{
				data.PlayerId = MySession.Static.LocalPlayerId;
			}
			if ((MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer) && data.PlayerId != MySession.Static.LocalPlayerId)
			{
				if (MySession.Static.Players.TryGetPlayerId(data.PlayerId, out MyPlayer.PlayerId result))
				{
					HighlightMsg highlightMsg = default(HighlightMsg);
					highlightMsg.Data = data;
					highlightMsg.ExclusiveKey = exclusiveKey;
					highlightMsg.IsExclusive = isExclusive;
					HighlightMsg arg = highlightMsg;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnHighlightOnClient, arg, new EndpointId(result.SteamId));
				}
				return;
			}
			bool flag = data.Thickness > -1;
			if (m_exclusiveKeysToIds.ContainsValue(data.EntityId) && (!m_exclusiveKeysToIds.TryGetValue(exclusiveKey, out long value) || value != data.EntityId))
			{
				if (this.HighlightRejected != null)
				{
					this.HighlightRejected(data);
				}
			}
			else if (isExclusive)
			{
				if (exclusiveKey == -1)
				{
					exclusiveKey = m_exclusiveKeyCounter++;
				}
				if (flag)
				{
					if (!m_exclusiveKeysToIds.ContainsKey(exclusiveKey))
					{
						m_exclusiveKeysToIds.Add(exclusiveKey, data.EntityId);
					}
				}
				else
				{
					m_exclusiveKeysToIds.Remove(exclusiveKey);
				}
				MakeLocalHighlightChange(data);
				if (this.ExclusiveHighlightAccepted != null)
				{
					this.ExclusiveHighlightAccepted(data, exclusiveKey);
				}
			}
			else
			{
				MakeLocalHighlightChange(data);
				if (this.HighlightAccepted != null)
				{
					this.HighlightAccepted(data);
				}
			}
		}

		private void MakeLocalHighlightChange(MyHighlightData data)
		{
			if (data.Thickness > -1)
			{
				m_highlightedIds.Add(data.EntityId);
			}
			else
			{
				m_highlightedIds.Remove(data.EntityId);
			}
			if (!MyEntities.TryGetEntityById(data.EntityId, out MyEntity entity))
			{
				return;
			}
			if (!data.IgnoreUseObjectData)
			{
				IMyUseObject myUseObject = entity as IMyUseObject;
				MyUseObjectsComponentBase myUseObjectsComponentBase = entity.Components.Get<MyUseObjectsComponentBase>();
				if (myUseObject != null || myUseObjectsComponentBase != null)
				{
					if (myUseObjectsComponentBase == null)
					{
						HighlightUseObject(myUseObject, data);
						if (this.HighlightAccepted != null)
						{
							this.HighlightAccepted(data);
						}
						return;
					}
					List<IMyUseObject> list = new List<IMyUseObject>();
					myUseObjectsComponentBase.GetInteractiveObjects(list);
					for (int i = 0; i < list.Count; i++)
					{
						HighlightUseObject(list[i], data);
					}
					if (list.Count > 0)
					{
						if (this.HighlightAccepted != null)
						{
							this.HighlightAccepted(data);
						}
						return;
					}
				}
			}
			m_subPartIndicies.Clear();
			CollectSubPartIndicies(entity);
			uint[] renderObjectIDs = entity.Render.RenderObjectIDs;
			for (int j = 0; j < renderObjectIDs.Length; j++)
			{
				MyRenderProxy.UpdateModelHighlight(renderObjectIDs[j], null, m_subPartIndicies.ToArray(), data.OutlineColor, data.Thickness, data.PulseTimeInFrames);
			}
			if (this.HighlightAccepted != null)
			{
				this.HighlightAccepted(data);
			}
		}

		private void CollectSubPartIndicies(MyEntity currentEntity)
		{
			if (currentEntity.Subparts != null && currentEntity.Render != null)
			{
				foreach (MyEntitySubpart value in currentEntity.Subparts.Values)
				{
					CollectSubPartIndicies(value);
					m_subPartIndicies.AddRange(value.Render.RenderObjectIDs);
				}
			}
		}

		private void HighlightUseObject(IMyUseObject useObject, MyHighlightData data)
		{
			m_highlightCalculationHelper.HighlightAttribute = null;
			if (useObject.Dummy != null)
			{
				useObject.Dummy.CustomData.TryGetValue("highlight", out object value);
				string text = value as string;
				if (text == null)
				{
					return;
				}
				if (data.SubPartNames != null)
				{
					m_highlightAttributeBuilder.Clear();
					string[] array = data.SubPartNames.Split(new char[1]
					{
						';'
					});
					foreach (string value2 in array)
					{
						if (text.Contains(value2))
						{
							m_highlightAttributeBuilder.Append(value2).Append(';');
						}
					}
					if (m_highlightAttributeBuilder.Length > 0)
					{
						m_highlightAttributeBuilder.TrimEnd(1);
					}
					m_highlightCalculationHelper.HighlightAttribute = m_highlightAttributeBuilder.ToString();
				}
				else
				{
					m_highlightCalculationHelper.HighlightAttribute = text;
				}
				if (string.IsNullOrEmpty(m_highlightCalculationHelper.HighlightAttribute))
				{
					return;
				}
			}
			m_highlightCalculationHelper.Highlight(useObject);
			MyRenderProxy.UpdateModelHighlight(m_highlightCalculationHelper.InteractiveObject.RenderObjectID, m_highlightCalculationHelper.SectionNames, m_highlightCalculationHelper.SubpartIndices, data.OutlineColor, data.Thickness, data.PulseTimeInFrames, m_highlightCalculationHelper.InteractiveObject.InstanceID);
		}

		[Event(null, 446)]
		[Reliable]
		[Client]
		private static void OnHighlightOnClient(HighlightMsg msg)
		{
			if (m_static.m_exclusiveKeysToIds.ContainsValue(msg.Data.EntityId) && (!m_static.m_exclusiveKeysToIds.TryGetValue(msg.ExclusiveKey, out long value) || value != msg.Data.EntityId))
			{
				if (m_static.HighlightRejected != null)
				{
					m_static.HighlightRejected(msg.Data);
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRequestRejected, msg, MyEventContext.Current.Sender);
				return;
			}
			m_static.MakeLocalHighlightChange(msg.Data);
			if (msg.IsExclusive)
			{
				bool flag = msg.Data.Thickness > -1;
				if (msg.ExclusiveKey == -1)
				{
					msg.ExclusiveKey = m_exclusiveKeyCounter++;
					if (flag && !m_static.m_exclusiveKeysToIds.ContainsKey(msg.ExclusiveKey))
					{
						m_static.m_exclusiveKeysToIds.Add(msg.ExclusiveKey, msg.Data.EntityId);
					}
				}
				if (!flag)
				{
					m_static.m_exclusiveKeysToIds.Remove(msg.ExclusiveKey);
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRequestAccepted, msg, MyEventContext.Current.Sender);
		}

		[Event(null, 492)]
		[Reliable]
		[Server]
		private static void OnRequestRejected(HighlightMsg msg)
		{
			if (msg.IsExclusive)
			{
				m_static.NotifyExclusiveHighlightRejected(msg.Data, msg.ExclusiveKey);
			}
			else if (m_static.HighlightRejected != null)
			{
				m_static.HighlightRejected(msg.Data);
			}
		}

		[Event(null, 507)]
		[Reliable]
		[Server]
		private static void OnRequestAccepted(HighlightMsg msg)
		{
			if (msg.IsExclusive)
			{
				m_static.NotifyExclusiveHighlightAccepted(msg.Data, msg.ExclusiveKey);
			}
			else
			{
				m_static.NotifyHighlightAccepted(msg.Data);
			}
		}

		private void NotifyHighlightAccepted(MyHighlightData data)
		{
			if (this.HighlightAccepted != null)
			{
				this.HighlightAccepted(data);
				Delegate[] invocationList = this.HighlightAccepted.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					HighlightAccepted -= (Action<MyHighlightData>)@delegate;
				}
			}
		}

		private void NotifyExclusiveHighlightAccepted(MyHighlightData data, int exclusiveKey)
		{
			if (this.ExclusiveHighlightAccepted != null)
			{
				this.ExclusiveHighlightAccepted(data, exclusiveKey);
				Delegate[] invocationList = this.ExclusiveHighlightAccepted.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					ExclusiveHighlightAccepted -= (Action<MyHighlightData, int>)@delegate;
				}
			}
		}

		private void NotifyExclusiveHighlightRejected(MyHighlightData data, int exclusiveKey)
		{
			if (this.ExclusiveHighlightRejected != null)
			{
				this.ExclusiveHighlightRejected(data, exclusiveKey);
				Delegate[] invocationList = this.ExclusiveHighlightRejected.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					ExclusiveHighlightRejected -= (Action<MyHighlightData, int>)@delegate;
				}
			}
		}
	}
}
