using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Systems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Components
{
	[MyComponentType(typeof(MyTimerComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_TimerComponent), true)]
	public class MyTimerComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_Components_MyTimerComponent_003C_003EActor : IActivator, IActivator<MyTimerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyTimerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTimerComponent CreateInstance()
			{
				return new MyTimerComponent();
			}

			MyTimerComponent IActivator<MyTimerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool Repeat;

		public float TimeToEvent;

		public Action<MyEntityComponentContainer> EventToTrigger;

		private float m_setTimeMin;

		private float m_originTimeMin;

		public bool TimerEnabled = true;

		public bool RemoveEntityOnTimer;

		private bool m_resetOrigin;

		public override string ComponentTypeDebugString => "Timer";

		public void SetRemoveEntityTimer(float timeMin)
		{
			RemoveEntityOnTimer = true;
			SetTimer(timeMin, GetRemoveEntityOnTimerEvent());
		}

		public void SetTimer(float timeMin, Action<MyEntityComponentContainer> triggerEvent, bool start = true, bool repeat = false)
		{
			TimeToEvent = -1f;
			m_setTimeMin = timeMin;
			Repeat = repeat;
			EventToTrigger = triggerEvent;
			TimerEnabled = false;
			if (start)
			{
				StartTiming();
			}
		}

		public void ClearEvent()
		{
			EventToTrigger = null;
		}

		private void StartTiming()
		{
			TimeToEvent = m_setTimeMin;
			TimerEnabled = true;
			m_originTimeMin = (float)MySession.Static.ElapsedGameTime.TotalMinutes;
		}

		public void Update()
		{
			if (!TimerEnabled)
			{
				return;
			}
			float num = (float)MySession.Static.ElapsedGameTime.TotalMinutes;
			if (m_resetOrigin)
			{
				m_originTimeMin = num - m_setTimeMin + TimeToEvent;
				m_resetOrigin = false;
			}
			TimeToEvent = m_originTimeMin + m_setTimeMin - num;
			if (TimeToEvent <= 0f)
			{
				if (EventToTrigger != null)
				{
					EventToTrigger(base.Container);
				}
				if (Repeat)
				{
					m_originTimeMin = (float)MySession.Static.ElapsedGameTime.TotalMinutes;
				}
				else
				{
					TimerEnabled = false;
				}
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (TimerEnabled)
			{
				m_resetOrigin = true;
			}
			MyTimerComponentSystem.Static.Register(this);
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (MyTimerComponentSystem.Static != null)
			{
				MyTimerComponentSystem.Static.Unregister(this);
			}
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_TimerComponent obj = MyComponentFactory.CreateObjectBuilder(this) as MyObjectBuilder_TimerComponent;
			obj.Repeat = Repeat;
			obj.TimeToEvent = TimeToEvent;
			obj.SetTimeMinutes = m_setTimeMin;
			obj.TimerEnabled = TimerEnabled;
			obj.RemoveEntityOnTimer = RemoveEntityOnTimer;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
		{
			MyObjectBuilder_TimerComponent myObjectBuilder_TimerComponent = baseBuilder as MyObjectBuilder_TimerComponent;
			Repeat = myObjectBuilder_TimerComponent.Repeat;
			TimeToEvent = myObjectBuilder_TimerComponent.TimeToEvent;
			m_setTimeMin = myObjectBuilder_TimerComponent.SetTimeMinutes;
			TimerEnabled = myObjectBuilder_TimerComponent.TimerEnabled;
			RemoveEntityOnTimer = myObjectBuilder_TimerComponent.RemoveEntityOnTimer;
			if (RemoveEntityOnTimer && Sync.IsServer)
			{
				EventToTrigger = GetRemoveEntityOnTimerEvent();
			}
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			MyTimerComponentDefinition myTimerComponentDefinition = definition as MyTimerComponentDefinition;
			if (myTimerComponentDefinition != null)
			{
				TimerEnabled = (myTimerComponentDefinition.TimeToRemoveMin > 0f);
				m_setTimeMin = myTimerComponentDefinition.TimeToRemoveMin;
				TimeToEvent = m_setTimeMin;
				RemoveEntityOnTimer = (myTimerComponentDefinition.TimeToRemoveMin > 0f);
				if (RemoveEntityOnTimer && Sync.IsServer)
				{
					EventToTrigger = GetRemoveEntityOnTimerEvent();
				}
			}
		}

		private static Action<MyEntityComponentContainer> GetRemoveEntityOnTimerEvent()
		{
			return delegate(MyEntityComponentContainer container)
			{
				MyLog.Default.Info($"MyTimerComponent removed entity '{container.Entity.Name}:{container.Entity.DisplayName}' with entity id '{container.Entity.EntityId}'");
				if (!container.Entity.MarkedForClose)
				{
					container.Entity.Close();
				}
			};
		}
	}
}
