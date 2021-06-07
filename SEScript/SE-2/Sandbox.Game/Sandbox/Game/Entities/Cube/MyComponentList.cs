using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;

namespace Sandbox.Game.Entities.Cube
{
	public class MyComponentList
	{
		private List<MyTuple<MyDefinitionId, int>> m_displayList = new List<MyTuple<MyDefinitionId, int>>();

		private Dictionary<MyDefinitionId, int> m_totalMaterials = new Dictionary<MyDefinitionId, int>();

		private Dictionary<MyDefinitionId, int> m_requiredMaterials = new Dictionary<MyDefinitionId, int>();

		public DictionaryReader<MyDefinitionId, int> TotalMaterials => new DictionaryReader<MyDefinitionId, int>(m_totalMaterials);

		public DictionaryReader<MyDefinitionId, int> RequiredMaterials => new DictionaryReader<MyDefinitionId, int>(m_requiredMaterials);

		public void AddMaterial(MyDefinitionId myDefinitionId, int amount, int requiredAmount = 0, bool addToDisplayList = true)
		{
			if (requiredAmount > amount)
			{
				requiredAmount = amount;
			}
			if (addToDisplayList)
			{
				m_displayList.Add(new MyTuple<MyDefinitionId, int>(myDefinitionId, amount));
			}
			AddToDictionary(m_totalMaterials, myDefinitionId, amount);
			if (requiredAmount > 0)
			{
				AddToDictionary(m_requiredMaterials, myDefinitionId, requiredAmount);
			}
		}

		public void Clear()
		{
			m_displayList.Clear();
			m_totalMaterials.Clear();
			m_requiredMaterials.Clear();
		}

		private void AddToDictionary(Dictionary<MyDefinitionId, int> dict, MyDefinitionId myDefinitionId, int amount)
		{
			int value = 0;
			dict.TryGetValue(myDefinitionId, out value);
			value = (dict[myDefinitionId] = value + amount);
		}
	}
}
