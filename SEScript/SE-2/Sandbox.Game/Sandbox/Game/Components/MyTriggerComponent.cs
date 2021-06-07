using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_TriggerBase), true)]
	public class MyTriggerComponent : MyEntityComponentBase
	{
		public enum TriggerType
		{
			AABB,
			Sphere
		}

		private class Sandbox_Game_Components_MyTriggerComponent_003C_003EActor : IActivator, IActivator<MyTriggerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyTriggerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTriggerComponent CreateInstance()
			{
				return new MyTriggerComponent();
			}

			MyTriggerComponent IActivator<MyTriggerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static uint m_triggerCounter;

		private const uint PRIME = 31u;

		private readonly uint m_updateOffset;

		private readonly List<MyEntity> m_queryResult = new List<MyEntity>();

		protected TriggerType m_triggerType;

		protected BoundingBoxD m_AABB;

		protected BoundingSphereD m_boundingSphere;

		public Vector3D DefaultTranslation = Vector3D.Zero;

		protected bool DoQuery
		{
			get;
			set;
		}

		protected List<MyEntity> QueryResult => m_queryResult;

		public uint UpdateFrequency
		{
			get;
			set;
		}

		public virtual bool Enabled
		{
			get;
			protected set;
		}

		public override string ComponentTypeDebugString => "Trigger";

		public Color? CustomDebugColor
		{
			get;
			set;
		}

		public Vector3D Center
		{
			get
			{
				switch (m_triggerType)
				{
				case TriggerType.AABB:
					return m_AABB.Center;
				case TriggerType.Sphere:
					return m_boundingSphere.Center;
				default:
					return Vector3D.Zero;
				}
			}
		}

		public MyTriggerComponent(TriggerType type, uint updateFrequency = 300u)
		{
			m_triggerType = type;
			UpdateFrequency = updateFrequency;
			m_updateOffset = m_triggerCounter++ * 31 % UpdateFrequency;
			DoQuery = true;
		}

		public MyTriggerComponent()
		{
			m_triggerType = TriggerType.AABB;
			UpdateFrequency = 300u;
			m_updateOffset = m_triggerCounter++ * 31 % UpdateFrequency;
			DoQuery = true;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_TriggerBase myObjectBuilder_TriggerBase = base.Serialize() as MyObjectBuilder_TriggerBase;
			if (myObjectBuilder_TriggerBase != null)
			{
				myObjectBuilder_TriggerBase.AABB = m_AABB;
				myObjectBuilder_TriggerBase.BoundingSphere = m_boundingSphere;
				myObjectBuilder_TriggerBase.Type = (int)m_triggerType;
				myObjectBuilder_TriggerBase.Offset = DefaultTranslation;
			}
			return myObjectBuilder_TriggerBase;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_TriggerBase myObjectBuilder_TriggerBase = builder as MyObjectBuilder_TriggerBase;
			if (myObjectBuilder_TriggerBase != null)
			{
				m_AABB = myObjectBuilder_TriggerBase.AABB;
				m_boundingSphere = myObjectBuilder_TriggerBase.BoundingSphere;
				m_triggerType = (TriggerType)((myObjectBuilder_TriggerBase.Type != -1) ? myObjectBuilder_TriggerBase.Type : 0);
				DefaultTranslation = myObjectBuilder_TriggerBase.Offset;
			}
		}

		public override void OnAddedToScene()
		{
			MySessionComponentTriggerSystem.Static.AddTrigger(this);
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			MySessionComponentTriggerSystem.RemoveTrigger((MyEntity)base.Entity, this);
			base.Entity.PositionComp.OnPositionChanged -= OnEntityPositionCompPositionChanged;
			Dispose();
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			base.Entity.PositionComp.OnPositionChanged += OnEntityPositionCompPositionChanged;
			base.Entity.NeedsWorldMatrix = true;
			if (base.Entity.InScene)
			{
				MySessionComponentTriggerSystem.Static.AddTrigger(this);
			}
		}

		private void OnEntityPositionCompPositionChanged(MyPositionComponentBase myPositionComponentBase)
		{
			switch (m_triggerType)
			{
			case TriggerType.AABB:
			{
				Vector3D vctTranlsation = base.Entity.PositionComp.GetPosition() - m_AABB.Matrix.Translation + DefaultTranslation;
				m_AABB.Translate(vctTranlsation);
				break;
			}
			case TriggerType.Sphere:
				m_boundingSphere.Center = base.Entity.PositionComp.GetPosition() + DefaultTranslation;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void Update()
		{
			if ((long)MySession.Static.GameplayFrameCounter % (long)UpdateFrequency == m_updateOffset)
			{
				UpdateInternal();
			}
		}

		protected virtual void UpdateInternal()
		{
			if (!DoQuery)
			{
				return;
			}
			m_queryResult.Clear();
			switch (m_triggerType)
			{
			case TriggerType.AABB:
				MyGamePruningStructure.GetTopMostEntitiesInBox(ref m_AABB, m_queryResult);
				break;
			case TriggerType.Sphere:
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref m_boundingSphere, m_queryResult);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			int num = 0;
			while (num < m_queryResult.Count)
			{
				MyEntity entity = m_queryResult[num];
				if (!QueryEvaluator(entity))
				{
					m_queryResult.RemoveAtFast(num);
					continue;
				}
				switch (m_triggerType)
				{
				case TriggerType.AABB:
					if (!m_AABB.Intersects(m_queryResult[num].PositionComp.WorldAABB))
					{
						m_queryResult.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
					break;
				case TriggerType.Sphere:
					if (!m_boundingSphere.Intersects(m_queryResult[num].PositionComp.WorldAABB))
					{
						m_queryResult.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
					break;
				default:
					num++;
					break;
				}
			}
		}

		public virtual void Dispose()
		{
			m_queryResult.Clear();
		}

		public virtual void DebugDraw()
		{
			Color color = Color.Red;
			if (CustomDebugColor.HasValue)
			{
				color = CustomDebugColor.Value;
			}
			if (m_triggerType == TriggerType.AABB)
			{
				MyRenderProxy.DebugDrawAABB(m_AABB, (m_queryResult.Count == 0) ? color : Color.Green, 1f, 1f, depthRead: false);
			}
			else
			{
				MyRenderProxy.DebugDrawSphere(m_boundingSphere.Center, (float)m_boundingSphere.Radius, (m_queryResult.Count == 0) ? color : Color.Green, 1f, depthRead: false);
			}
			if (base.Entity.Parent != null)
			{
				MyRenderProxy.DebugDrawLine3D(Center, base.Entity.Parent.PositionComp.GetPosition(), Color.Yellow, Color.Green, depthRead: false);
			}
			foreach (MyEntity item in m_queryResult)
			{
				MyRenderProxy.DebugDrawAABB(item.PositionComp.WorldAABB, Color.Yellow, 1f, 1f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(item.WorldMatrix.Translation, base.Entity.WorldMatrix.Translation, Color.Yellow, Color.Green, depthRead: false);
			}
		}

		protected virtual bool QueryEvaluator(MyEntity entity)
		{
			return true;
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public bool Contains(Vector3D point)
		{
			switch (m_triggerType)
			{
			case TriggerType.AABB:
				return m_AABB.Contains(point) == ContainmentType.Contains;
			case TriggerType.Sphere:
				return m_boundingSphere.Contains(point) == ContainmentType.Contains;
			default:
				return false;
			}
		}
	}
}
