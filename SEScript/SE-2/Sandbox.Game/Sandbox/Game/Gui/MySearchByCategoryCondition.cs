using Sandbox.Definitions;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public class MySearchByCategoryCondition : IMySearchCondition
	{
		public List<MyGuiBlockCategoryDefinition> SelectedCategories;

		private MyGuiBlockCategoryDefinition m_lastCategory;

		private HashSet<MyCubeBlockDefinitionGroup> m_sortedBlocks = new HashSet<MyCubeBlockDefinitionGroup>();

		private Dictionary<string, List<MyCubeBlockDefinitionGroup>> m_blocksByCategories = new Dictionary<string, List<MyCubeBlockDefinitionGroup>>();

		public bool StrictSearch
		{
			get;
			set;
		}

		public bool MatchesCondition(string itemId)
		{
			return IsItemInAnySelectedCategory(itemId);
		}

		public bool MatchesCondition(MyDefinitionBase itemId)
		{
			return IsItemInAnySelectedCategory(itemId.Id.ToString());
		}

		public void AddDefinitionGroup(MyCubeBlockDefinitionGroup definitionGruop)
		{
			if (m_lastCategory != null)
			{
				List<MyCubeBlockDefinitionGroup> value = null;
				if (!m_blocksByCategories.TryGetValue(m_lastCategory.Name, out value))
				{
					value = new List<MyCubeBlockDefinitionGroup>();
					m_blocksByCategories.Add(m_lastCategory.Name, value);
				}
				value.Add(definitionGruop);
			}
		}

		public HashSet<MyCubeBlockDefinitionGroup> GetSortedBlocks()
		{
			foreach (KeyValuePair<string, List<MyCubeBlockDefinitionGroup>> blocksByCategory in m_blocksByCategories)
			{
				foreach (MyCubeBlockDefinitionGroup item in blocksByCategory.Value)
				{
					m_sortedBlocks.Add(item);
				}
			}
			return m_sortedBlocks;
		}

		public void CleanDefinitionGroups()
		{
			m_sortedBlocks.Clear();
			m_blocksByCategories.Clear();
		}

		private bool IsItemInAnySelectedCategory(string itemId)
		{
			m_lastCategory = null;
			if (SelectedCategories == null)
			{
				return true;
			}
			foreach (MyGuiBlockCategoryDefinition selectedCategory in SelectedCategories)
			{
				if (selectedCategory.HasItem(itemId) || (selectedCategory.ShowAnimations && itemId.Contains("AnimationDefinition")))
				{
					m_lastCategory = selectedCategory;
					return true;
				}
			}
			return false;
		}
	}
}
