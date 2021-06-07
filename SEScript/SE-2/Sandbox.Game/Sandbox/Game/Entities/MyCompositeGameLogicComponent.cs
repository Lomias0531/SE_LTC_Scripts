using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	public class MyCompositeGameLogicComponent : MyGameLogicComponent, IMyGameLogicComponent
	{
		private class Sandbox_Game_Entities_MyCompositeGameLogicComponent_003C_003EActor
		{
		}

		private ICollection<MyGameLogicComponent> m_logicComponents;

		private MyCompositeGameLogicComponent(ICollection<MyGameLogicComponent> logicComponents)
		{
			m_logicComponents = logicComponents;
		}

		public static MyGameLogicComponent Create(ICollection<MyGameLogicComponent> logicComponents, MyEntity entity)
		{
			foreach (MyGameLogicComponent logicComponent in logicComponents)
			{
				logicComponent.SetContainer(entity.Components);
			}
			switch (logicComponents.Count)
			{
			case 0:
				return null;
			case 1:
				return logicComponents.First();
			default:
				return new MyCompositeGameLogicComponent(logicComponents);
			}
		}

		void IMyGameLogicComponent.UpdateOnceBeforeFrame(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateOnceBeforeFrame(entityUpdate);
			}
		}

		void IMyGameLogicComponent.UpdateBeforeSimulation(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateBeforeSimulation(entityUpdate);
			}
		}

		void IMyGameLogicComponent.UpdateBeforeSimulation10(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateBeforeSimulation10(entityUpdate);
			}
		}

		void IMyGameLogicComponent.UpdateBeforeSimulation100(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateBeforeSimulation100(entityUpdate);
			}
		}

		void IMyGameLogicComponent.UpdateAfterSimulation(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateAfterSimulation(entityUpdate);
			}
		}

		void IMyGameLogicComponent.UpdateAfterSimulation10(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateAfterSimulation10(entityUpdate);
			}
		}

		void IMyGameLogicComponent.UpdateAfterSimulation100(bool entityUpdate)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UpdateAfterSimulation100(entityUpdate);
			}
		}

		void IMyGameLogicComponent.Close()
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).Close();
			}
		}

		void IMyGameLogicComponent.RegisterForUpdate()
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).RegisterForUpdate();
			}
		}

		void IMyGameLogicComponent.UnregisterForUpdate()
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				((IMyGameLogicComponent)logicComponent).UnregisterForUpdate();
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				logicComponent.Init(objectBuilder);
			}
		}

		public override void MarkForClose()
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				logicComponent.MarkForClose();
			}
		}

		public override void Close()
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				logicComponent.Close();
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				MyObjectBuilder_EntityBase objectBuilder = logicComponent.GetObjectBuilder(copy);
				if (objectBuilder != null)
				{
					return objectBuilder;
				}
			}
			return null;
		}

		public override T GetAs<T>()
		{
			foreach (MyGameLogicComponent logicComponent in m_logicComponents)
			{
				if (logicComponent is T)
				{
					return logicComponent as T;
				}
			}
			return null;
		}

		public MyGameLogicComponent GetAs(string typeName)
		{
			return m_logicComponents.FirstOrDefault((MyGameLogicComponent c) => c.GetType().FullName == typeName);
		}
	}
}
