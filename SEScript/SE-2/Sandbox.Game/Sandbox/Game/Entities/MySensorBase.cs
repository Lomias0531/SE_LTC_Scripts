using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal class MySensorBase : MyEntity
	{
		public enum EventType : byte
		{
			None,
			Add,
			Delete
		}

		private class DetectedEntityInfo
		{
			public bool Moved;

			public EventType EventType;
		}

		private class Sandbox_Game_Entities_MySensorBase_003C_003EActor : IActivator, IActivator<MySensorBase>
		{
			private sealed override object CreateInstance()
			{
				return new MySensorBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySensorBase CreateInstance()
			{
				return new MySensorBase();
			}

			MySensorBase IActivator<MySensorBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Stack<DetectedEntityInfo> m_unusedInfos = new Stack<DetectedEntityInfo>();

		private Dictionary<MyEntity, DetectedEntityInfo> m_detectedEntities = new Dictionary<MyEntity, DetectedEntityInfo>(new InstanceComparer<MyEntity>());

		private List<MyEntity> m_deleteList = new List<MyEntity>();

		private Action<MyPositionComponentBase> m_entityPositionChanged;

		private Action<MyEntity> m_entityClosed;

		public event SensorFilterHandler Filter;

		public event EntitySensorHandler EntityEntered;

		public event EntitySensorHandler EntityMoved;

		public event EntitySensorHandler EntityLeft;

		public MySensorBase()
		{
			base.Save = false;
			m_entityPositionChanged = entity_OnPositionChanged;
			m_entityClosed = entity_OnClose;
		}

		public MyEntity GetClosestEntity(Vector3 position)
		{
			MyEntity result = null;
			double num = double.MaxValue;
			foreach (KeyValuePair<MyEntity, DetectedEntityInfo> detectedEntity in m_detectedEntities)
			{
				double num2 = (position - detectedEntity.Key.PositionComp.GetPosition()).LengthSquared();
				if (num2 < num)
				{
					num = num2;
					result = detectedEntity.Key;
				}
			}
			return result;
		}

		private DetectedEntityInfo GetInfo()
		{
			if (m_unusedInfos.Count == 0)
			{
				return new DetectedEntityInfo();
			}
			return m_unusedInfos.Pop();
		}

		protected void TrackEntity(MyEntity entity)
		{
			if (!FilterEntity(entity))
			{
				if (!m_detectedEntities.TryGetValue(entity, out DetectedEntityInfo value))
				{
					entity.PositionComp.OnPositionChanged += m_entityPositionChanged;
					entity.OnClose += m_entityClosed;
					value = GetInfo();
					value.Moved = false;
					value.EventType = EventType.Add;
					m_detectedEntities[entity] = value;
				}
				else if (value.EventType == EventType.Delete)
				{
					value.EventType = EventType.None;
				}
			}
		}

		protected bool FilterEntity(MyEntity entity)
		{
			SensorFilterHandler filter = this.Filter;
			if (filter != null)
			{
				bool processEntity = true;
				filter(this, entity, ref processEntity);
				if (!processEntity)
				{
					return true;
				}
			}
			return false;
		}

		public bool AnyEntityWithState(EventType type)
		{
			return m_detectedEntities.Any((KeyValuePair<MyEntity, DetectedEntityInfo> s) => s.Value.EventType == type);
		}

		public bool HasAnyMoved()
		{
			return m_detectedEntities.Any((KeyValuePair<MyEntity, DetectedEntityInfo> s) => s.Value.Moved);
		}

		private void UntrackEntity(MyEntity entity)
		{
			entity.PositionComp.OnPositionChanged -= m_entityPositionChanged;
			entity.OnClose -= m_entityClosed;
		}

		private void entity_OnClose(MyEntity obj)
		{
			if (m_detectedEntities.TryGetValue(obj, out DetectedEntityInfo value))
			{
				value.EventType = EventType.Delete;
			}
		}

		private void entity_OnPositionChanged(MyPositionComponentBase entity)
		{
			if (m_detectedEntities.TryGetValue(entity.Container.Entity as MyEntity, out DetectedEntityInfo value))
			{
				value.Moved = true;
			}
		}

		private void raise_EntityEntered(MyEntity entity)
		{
			this.EntityEntered?.Invoke(this, entity);
		}

		private void raise_EntityMoved(MyEntity entity)
		{
			this.EntityMoved?.Invoke(this, entity);
		}

		private void raise_EntityLeft(MyEntity entity)
		{
			this.EntityLeft?.Invoke(this, entity);
		}

		public void RaiseAllMove()
		{
			EntitySensorHandler entityMoved = this.EntityMoved;
			if (entityMoved != null)
			{
				foreach (KeyValuePair<MyEntity, DetectedEntityInfo> detectedEntity in m_detectedEntities)
				{
					entityMoved(this, detectedEntity.Key);
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			foreach (KeyValuePair<MyEntity, DetectedEntityInfo> detectedEntity in m_detectedEntities)
			{
				if (detectedEntity.Value.EventType == EventType.Delete)
				{
					UntrackEntity(detectedEntity.Key);
					raise_EntityLeft(detectedEntity.Key);
					m_deleteList.Add(detectedEntity.Key);
					m_unusedInfos.Push(detectedEntity.Value);
				}
				else
				{
					if (detectedEntity.Value.EventType == EventType.Add)
					{
						raise_EntityEntered(detectedEntity.Key);
					}
					else if (detectedEntity.Value.Moved)
					{
						raise_EntityMoved(detectedEntity.Key);
					}
					detectedEntity.Value.Moved = false;
					detectedEntity.Value.EventType = EventType.Delete;
				}
			}
			foreach (MyEntity delete in m_deleteList)
			{
				m_detectedEntities.Remove(delete);
			}
			m_deleteList.Clear();
		}
	}
}
