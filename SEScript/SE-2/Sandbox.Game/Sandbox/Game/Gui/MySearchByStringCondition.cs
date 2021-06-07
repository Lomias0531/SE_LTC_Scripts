using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public class MySearchByStringCondition : IMySearchCondition
	{
		private string[] m_searchItems;

		private HashSet<MyCubeBlockDefinitionGroup> m_sortedBlocks = new HashSet<MyCubeBlockDefinitionGroup>();

		public string SearchName
		{
			set
			{
				m_searchItems = value.Split(new char[1]
				{
					' '
				});
			}
		}

		public bool IsValid => m_searchItems != null;

		public void Clean()
		{
			m_searchItems = null;
			CleanDefinitionGroups();
		}

		public bool MatchesCondition(string itemId)
		{
			string[] searchItems = m_searchItems;
			foreach (string testSequence in searchItems)
			{
				if (!StringExtensions.Contains(itemId, testSequence, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			return true;
		}

		public bool MatchesCondition(MyDefinitionBase itemId)
		{
			if (itemId != null && itemId.DisplayNameText != null)
			{
				return MatchesCondition(itemId.DisplayNameText);
			}
			return false;
		}

		public void AddDefinitionGroup(MyCubeBlockDefinitionGroup definitionGruop)
		{
			m_sortedBlocks.Add(definitionGruop);
		}

		public HashSet<MyCubeBlockDefinitionGroup> GetSortedBlocks()
		{
			return m_sortedBlocks;
		}

		public void CleanDefinitionGroups()
		{
			m_sortedBlocks.Clear();
		}
	}
}
