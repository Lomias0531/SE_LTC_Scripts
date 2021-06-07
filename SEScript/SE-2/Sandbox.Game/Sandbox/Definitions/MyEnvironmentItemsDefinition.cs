using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EnvironmentItemsDefinition), null)]
	public class MyEnvironmentItemsDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyEnvironmentItemsDefinition_003C_003EActor : IActivator, IActivator<MyEnvironmentItemsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEnvironmentItemsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEnvironmentItemsDefinition CreateInstance()
			{
				return new MyEnvironmentItemsDefinition();
			}

			MyEnvironmentItemsDefinition IActivator<MyEnvironmentItemsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private HashSet<MyStringHash> m_itemDefinitions;

		private List<MyStringHash> m_definitionList;

		private List<float> Frequencies;

		private float[] Intervals;

		private MyObjectBuilderType m_itemDefinitionType = MyObjectBuilderType.Invalid;

		public MyObjectBuilderType ItemDefinitionType => m_itemDefinitionType;

		public int Channel
		{
			get;
			private set;
		}

		public float MaxViewDistance
		{
			get;
			private set;
		}

		public float SectorSize
		{
			get;
			private set;
		}

		public float ItemSize
		{
			get;
			private set;
		}

		public MyStringHash Material
		{
			get;
			private set;
		}

		public int ItemDefinitionCount => m_definitionList.Count;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EnvironmentItemsDefinition myObjectBuilder_EnvironmentItemsDefinition = builder as MyObjectBuilder_EnvironmentItemsDefinition;
			m_itemDefinitions = new HashSet<MyStringHash>(MyStringHash.Comparer);
			m_definitionList = new List<MyStringHash>();
			object[] customAttributes = ((Type)builder.Id.TypeId).GetCustomAttributes(typeof(MyEnvironmentItemsAttribute), inherit: false);
			if (customAttributes.Length == 1)
			{
				MyEnvironmentItemsAttribute myEnvironmentItemsAttribute = customAttributes[0] as MyEnvironmentItemsAttribute;
				m_itemDefinitionType = myEnvironmentItemsAttribute.ItemDefinitionType;
			}
			else
			{
				m_itemDefinitionType = typeof(MyObjectBuilder_EnvironmentItemDefinition);
			}
			Channel = myObjectBuilder_EnvironmentItemsDefinition.Channel;
			MaxViewDistance = myObjectBuilder_EnvironmentItemsDefinition.MaxViewDistance;
			SectorSize = myObjectBuilder_EnvironmentItemsDefinition.SectorSize;
			ItemSize = myObjectBuilder_EnvironmentItemsDefinition.ItemSize;
			Material = MyStringHash.GetOrCompute(myObjectBuilder_EnvironmentItemsDefinition.PhysicalMaterial);
			Frequencies = new List<float>();
		}

		public void AddItemDefinition(MyStringHash definition, float frequency, bool recompute = true)
		{
			if (!m_itemDefinitions.Contains(definition))
			{
				m_itemDefinitions.Add(definition);
				m_definitionList.Add(definition);
				Frequencies.Add(frequency);
				if (recompute)
				{
					RecomputeFrequencies();
				}
			}
		}

		public void RecomputeFrequencies()
		{
			if (m_definitionList.Count == 0)
			{
				Intervals = null;
				return;
			}
			Intervals = new float[m_definitionList.Count - 1];
			float num = 0f;
			foreach (float frequency in Frequencies)
			{
				num += frequency;
			}
			float num2 = 0f;
			for (int i = 0; i < Intervals.Length; i++)
			{
				num2 += Frequencies[i];
				Intervals[i] = num2 / num;
			}
		}

		public MyEnvironmentItemDefinition GetItemDefinition(MyStringHash subtypeId)
		{
			MyEnvironmentItemDefinition definition = null;
			MyDefinitionId defId = new MyDefinitionId(m_itemDefinitionType, subtypeId);
			MyDefinitionManager.Static.TryGetDefinition(defId, out definition);
			return definition;
		}

		public MyEnvironmentItemDefinition GetItemDefinition(int index)
		{
			if (index < 0 || index >= m_definitionList.Count)
			{
				return null;
			}
			return GetItemDefinition(m_definitionList[index]);
		}

		public MyEnvironmentItemDefinition GetRandomItemDefinition()
		{
			if (m_definitionList.Count == 0)
			{
				return null;
			}
			float value = (float)MyRandom.Instance.Next(0, 65536) / 65536f;
			return GetItemDefinition(m_definitionList[Intervals.BinaryIntervalSearch(value)]);
		}

		public MyEnvironmentItemDefinition GetRandomItemDefinition(MyRandom instance)
		{
			if (m_definitionList.Count == 0)
			{
				return null;
			}
			float value = (float)instance.Next(0, 65536) / 65536f;
			return GetItemDefinition(m_definitionList[Intervals.BinaryIntervalSearch(value)]);
		}

		public bool ContainsItemDefinition(MyStringHash subtypeId)
		{
			return m_itemDefinitions.Contains(subtypeId);
		}

		public bool ContainsItemDefinition(MyDefinitionId definitionId)
		{
			if (definitionId.TypeId == m_itemDefinitionType)
			{
				return m_itemDefinitions.Contains(definitionId.SubtypeId);
			}
			return false;
		}

		public bool ContainsItemDefinition(MyEnvironmentItemDefinition itemDefinition)
		{
			return ContainsItemDefinition(itemDefinition.Id);
		}
	}
}
