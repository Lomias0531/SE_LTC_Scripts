using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ComponentGroupDefinition), null)]
	public class MyComponentGroupDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyComponentGroupDefinition_003C_003EActor : IActivator, IActivator<MyComponentGroupDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyComponentGroupDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyComponentGroupDefinition CreateInstance()
			{
				return new MyComponentGroupDefinition();
			}

			MyComponentGroupDefinition IActivator<MyComponentGroupDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyObjectBuilder_ComponentGroupDefinition m_postprocessBuilder;

		private List<MyComponentDefinition> m_components;

		public bool IsValid => m_components.Count != 0;

		public MyComponentGroupDefinition()
		{
			m_components = new List<MyComponentDefinition>();
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			m_postprocessBuilder = (builder as MyObjectBuilder_ComponentGroupDefinition);
		}

		public new void Postprocess()
		{
			bool flag = true;
			int num = 0;
			MyObjectBuilder_ComponentGroupDefinition.Component[] components = m_postprocessBuilder.Components;
			for (int i = 0; i < components.Length; i++)
			{
				MyObjectBuilder_ComponentGroupDefinition.Component component = components[i];
				if (component.Amount > num)
				{
					num = component.Amount;
				}
			}
			for (int j = 0; j < num; j++)
			{
				m_components.Add(null);
			}
			components = m_postprocessBuilder.Components;
			for (int i = 0; i < components.Length; i++)
			{
				MyObjectBuilder_ComponentGroupDefinition.Component component2 = components[i];
				MyDefinitionId defId = new MyDefinitionId(typeof(MyObjectBuilder_Component), component2.SubtypeId);
				MyDefinitionManager.Static.TryGetDefinition(defId, out MyComponentDefinition definition);
				if (definition == null)
				{
					flag = false;
				}
				SetComponentDefinition(component2.Amount, definition);
			}
			for (int k = 0; k < m_components.Count; k++)
			{
				if (m_components[k] == null)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				m_components.Clear();
			}
		}

		public void SetComponentDefinition(int amount, MyComponentDefinition definition)
		{
			if (amount > 0 && amount <= m_components.Count)
			{
				m_components[amount - 1] = definition;
			}
		}

		public MyComponentDefinition GetComponentDefinition(int amount)
		{
			if (amount > 0 && amount <= m_components.Count)
			{
				return m_components[amount - 1];
			}
			return null;
		}

		public int GetComponentNumber()
		{
			return m_components.Count;
		}
	}
}
