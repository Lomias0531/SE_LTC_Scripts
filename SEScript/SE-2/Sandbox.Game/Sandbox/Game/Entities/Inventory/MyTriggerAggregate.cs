using Sandbox.Game.Components;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.Entities.Inventory
{
	[MyComponentBuilder(typeof(MyObjectBuilder_TriggerAggregate), true)]
	public class MyTriggerAggregate : MyEntityComponentBase, IMyComponentAggregate
	{
		private class Sandbox_Game_Entities_Inventory_MyTriggerAggregate_003C_003EActor : IActivator, IActivator<MyTriggerAggregate>
		{
			private sealed override object CreateInstance()
			{
				return new MyTriggerAggregate();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTriggerAggregate CreateInstance()
			{
				return new MyTriggerAggregate();
			}

			MyTriggerAggregate IActivator<MyTriggerAggregate>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_triggerCount;

		private MyAggregateComponentList m_children = new MyAggregateComponentList();

		public override string ComponentTypeDebugString => "TriggerAggregate";

		public int TriggerCount
		{
			get
			{
				return m_triggerCount;
			}
			private set
			{
				if (m_triggerCount != value)
				{
					int arg = value - m_triggerCount;
					m_triggerCount = value;
					if (this.OnTriggerCountChanged != null)
					{
						this.OnTriggerCountChanged(this, arg);
					}
				}
			}
		}

		public MyAggregateComponentList ChildList => m_children;

		public event Action<MyTriggerAggregate, int> OnTriggerCountChanged;

		public override bool IsSerialized()
		{
			return true;
		}

		public void AfterComponentAdd(MyComponentBase component)
		{
			if (component is MyTriggerComponent)
			{
				TriggerCount++;
			}
			else if (component is MyTriggerAggregate)
			{
				(component as MyTriggerAggregate).OnTriggerCountChanged += OnChildAggregateCountChanged;
				TriggerCount += (component as MyTriggerAggregate).TriggerCount;
			}
		}

		private void OnChildAggregateCountChanged(MyTriggerAggregate obj, int change)
		{
			TriggerCount += change;
		}

		public void BeforeComponentRemove(MyComponentBase component)
		{
			if (component is MyTriggerComponent)
			{
				TriggerCount--;
			}
			else if (component is MyTriggerAggregate)
			{
				(component as MyTriggerAggregate).OnTriggerCountChanged -= OnChildAggregateCountChanged;
				TriggerCount -= (component as MyTriggerAggregate).TriggerCount;
			}
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_TriggerAggregate myObjectBuilder_TriggerAggregate = base.Serialize() as MyObjectBuilder_TriggerAggregate;
			ListReader<MyComponentBase> reader = m_children.Reader;
			if (reader.Count > 0)
			{
				myObjectBuilder_TriggerAggregate.AreaTriggers = new List<MyObjectBuilder_TriggerBase>(reader.Count);
				{
					foreach (MyComponentBase item in reader)
					{
						MyObjectBuilder_TriggerBase myObjectBuilder_TriggerBase = item.Serialize() as MyObjectBuilder_TriggerBase;
						if (myObjectBuilder_TriggerBase != null)
						{
							myObjectBuilder_TriggerAggregate.AreaTriggers.Add(myObjectBuilder_TriggerBase);
						}
					}
					return myObjectBuilder_TriggerAggregate;
				}
			}
			return myObjectBuilder_TriggerAggregate;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_TriggerAggregate myObjectBuilder_TriggerAggregate = builder as MyObjectBuilder_TriggerAggregate;
			if (myObjectBuilder_TriggerAggregate != null && myObjectBuilder_TriggerAggregate.AreaTriggers != null)
			{
				foreach (MyObjectBuilder_TriggerBase areaTrigger in myObjectBuilder_TriggerAggregate.AreaTriggers)
				{
					MyComponentBase myComponentBase = MyComponentFactory.CreateInstanceByTypeId(areaTrigger.TypeId);
					myComponentBase.Deserialize(areaTrigger);
					this.AddComponent(myComponentBase);
				}
			}
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			foreach (MyComponentBase item in ChildList.Reader)
			{
				item.OnAddedToScene();
			}
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			foreach (MyComponentBase item in ChildList.Reader)
			{
				item.OnRemovedFromScene();
			}
		}

		MyComponentContainer IMyComponentAggregate.get_ContainerBase()
		{
			return base.ContainerBase;
		}
	}
}
