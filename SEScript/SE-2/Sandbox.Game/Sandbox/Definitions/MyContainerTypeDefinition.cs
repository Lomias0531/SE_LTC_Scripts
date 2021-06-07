using System;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContainerTypeDefinition), null)]
	public class MyContainerTypeDefinition : MyDefinitionBase
	{
		public struct ContainerTypeItem
		{
			public MyFixedPoint AmountMin;

			public MyFixedPoint AmountMax;

			public float Frequency;

			public MyDefinitionId DefinitionId;

			public bool HasIntegralAmount;
		}

		private class Sandbox_Definitions_MyContainerTypeDefinition_003C_003EActor : IActivator, IActivator<MyContainerTypeDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContainerTypeDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContainerTypeDefinition CreateInstance()
			{
				return new MyContainerTypeDefinition();
			}

			MyContainerTypeDefinition IActivator<MyContainerTypeDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int CountMin;

		public int CountMax;

		public float ItemsCumulativeFrequency;

		private float m_tempCumulativeFreq;

		public ContainerTypeItem[] Items;

		private bool[] m_itemSelection;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContainerTypeDefinition myObjectBuilder_ContainerTypeDefinition = builder as MyObjectBuilder_ContainerTypeDefinition;
			CountMin = myObjectBuilder_ContainerTypeDefinition.CountMin;
			CountMax = myObjectBuilder_ContainerTypeDefinition.CountMax;
			ItemsCumulativeFrequency = 0f;
			int num = 0;
			Items = new ContainerTypeItem[myObjectBuilder_ContainerTypeDefinition.Items.Length];
			m_itemSelection = new bool[myObjectBuilder_ContainerTypeDefinition.Items.Length];
			MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem[] items = myObjectBuilder_ContainerTypeDefinition.Items;
			foreach (MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem containerTypeItem in items)
			{
				ContainerTypeItem containerTypeItem2 = default(ContainerTypeItem);
				containerTypeItem2.AmountMax = MyFixedPoint.DeserializeStringSafe(containerTypeItem.AmountMax);
				containerTypeItem2.AmountMin = MyFixedPoint.DeserializeStringSafe(containerTypeItem.AmountMin);
				containerTypeItem2.Frequency = Math.Max(containerTypeItem.Frequency, 0f);
				containerTypeItem2.DefinitionId = containerTypeItem.Id;
				ItemsCumulativeFrequency += containerTypeItem2.Frequency;
				Items[num] = containerTypeItem2;
				m_itemSelection[num] = false;
				num++;
			}
			m_tempCumulativeFreq = ItemsCumulativeFrequency;
		}

		public void DeselectAll()
		{
			for (int i = 0; i < Items.Length; i++)
			{
				m_itemSelection[i] = false;
			}
			m_tempCumulativeFreq = ItemsCumulativeFrequency;
		}

		public ContainerTypeItem SelectNextRandomItem()
		{
			float num = MyRandom.Instance.NextFloat(0f, m_tempCumulativeFreq);
			int num2 = 0;
			while (num2 < Items.Length - 1)
			{
				if (m_itemSelection[num2])
				{
					num2++;
					continue;
				}
				num -= Items[num2].Frequency;
				if (num < 0f)
				{
					break;
				}
				num2++;
			}
			m_tempCumulativeFreq -= Items[num2].Frequency;
			m_itemSelection[num2] = true;
			return Items[num2];
		}
	}
}
