using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Generics;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Inventory
{
	public class MyComponentCombiner
	{
		private MyDynamicObjectPool<List<int>> m_listAllocator = new MyDynamicObjectPool<List<int>>(2);

		private Dictionary<MyDefinitionId, List<int>> m_groups = new Dictionary<MyDefinitionId, List<int>>();

		private Dictionary<int, int> m_presentItems = new Dictionary<int, int>();

		private int m_totalItemCounter;

		private int m_solvedItemCounter;

		private List<MyComponentChange> m_solution = new List<MyComponentChange>();

		private static Dictionary<MyDefinitionId, MyFixedPoint> m_componentCounts = new Dictionary<MyDefinitionId, MyFixedPoint>();

		public MyFixedPoint GetItemAmountCombined(MyInventoryBase inventory, MyDefinitionId contentId)
		{
			if (inventory == null)
			{
				return 0;
			}
			int amount = 0;
			MyComponentGroupDefinition groupForComponent = MyDefinitionManager.Static.GetGroupForComponent(contentId, out amount);
			if (groupForComponent == null)
			{
				return amount + inventory.GetItemAmount(contentId, MyItemFlags.None, substitute: true);
			}
			Clear();
			inventory.CountItems(m_componentCounts);
			AddItem(groupForComponent.Id, amount, int.MaxValue);
			Solve(m_componentCounts);
			return GetSolvedItemCount();
		}

		public bool CanCombineItems(MyInventoryBase inventory, DictionaryReader<MyDefinitionId, int> items)
		{
			bool flag = true;
			Clear();
			inventory.CountItems(m_componentCounts);
			foreach (KeyValuePair<MyDefinitionId, int> item in items)
			{
				int amount = 0;
				int value = item.Value;
				MyComponentGroupDefinition myComponentGroupDefinition = null;
				myComponentGroupDefinition = MyDefinitionManager.Static.GetGroupForComponent(item.Key, out amount);
				if (myComponentGroupDefinition == null)
				{
					if (!m_componentCounts.TryGetValue(item.Key, out MyFixedPoint value2))
					{
						flag = false;
						break;
					}
					if (value2 < value)
					{
						flag = false;
						break;
					}
				}
				else
				{
					AddItem(myComponentGroupDefinition.Id, amount, value);
				}
			}
			if (flag)
			{
				flag &= Solve(m_componentCounts);
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				if (flag)
				{
					List<MyComponentChange> changes = null;
					GetSolution(out changes);
					float num = 0f;
					{
						foreach (MyComponentChange item2 in changes)
						{
							string text = "";
							if (item2.IsAddition())
							{
								string[] obj = new string[5]
								{
									text,
									"+ ",
									null,
									null,
									null
								};
								int amount2 = item2.Amount;
								obj[2] = amount2.ToString();
								obj[3] = "x";
								obj[4] = item2.ToAdd.ToString();
								text = string.Concat(obj);
								MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), text, Color.Green, 1f);
								num += 20f;
							}
							else if (item2.IsRemoval())
							{
								string[] obj2 = new string[5]
								{
									text,
									"- ",
									null,
									null,
									null
								};
								int amount2 = item2.Amount;
								obj2[2] = amount2.ToString();
								obj2[3] = "x";
								obj2[4] = item2.ToRemove.ToString();
								text = string.Concat(obj2);
								MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), text, Color.Red, 1f);
								num += 20f;
							}
							else
							{
								string[] obj3 = new string[5]
								{
									text,
									"- ",
									null,
									null,
									null
								};
								int amount2 = item2.Amount;
								obj3[2] = amount2.ToString();
								obj3[3] = "x";
								obj3[4] = item2.ToRemove.ToString();
								text = string.Concat(obj3);
								MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), text, Color.Orange, 1f);
								num += 20f;
								text = "";
								string[] obj4 = new string[5]
								{
									text,
									"+ ",
									null,
									null,
									null
								};
								amount2 = item2.Amount;
								obj4[2] = amount2.ToString();
								obj4[3] = "x";
								obj4[4] = item2.ToAdd.ToString();
								text = string.Concat(obj4);
								MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), text, Color.Orange, 1f);
								num += 20f;
							}
						}
						return flag;
					}
				}
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 0f), "Can not build", Color.Red, 1f);
			}
			return flag;
		}

		public void RemoveItemsCombined(MyInventoryBase inventory, DictionaryReader<MyDefinitionId, int> toRemove)
		{
			Clear();
			foreach (KeyValuePair<MyDefinitionId, int> item in toRemove)
			{
				int amount = 0;
				MyComponentGroupDefinition groupForComponent = MyDefinitionManager.Static.GetGroupForComponent(item.Key, out amount);
				if (groupForComponent == null)
				{
					inventory.RemoveItemsOfType(item.Value, item.Key);
				}
				else
				{
					AddItem(groupForComponent.Id, amount, item.Value);
				}
			}
			inventory.CountItems(m_componentCounts);
			Solve(m_componentCounts);
			inventory.ApplyChanges(m_solution);
		}

		public void Clear()
		{
			foreach (KeyValuePair<MyDefinitionId, List<int>> group in m_groups)
			{
				group.Value.Clear();
				m_listAllocator.Deallocate(group.Value);
			}
			m_groups.Clear();
			m_totalItemCounter = 0;
			m_solvedItemCounter = 0;
			m_componentCounts.Clear();
		}

		public void AddItem(MyDefinitionId groupId, int itemValue, int amount)
		{
			List<int> value = null;
			MyComponentGroupDefinition componentGroup = MyDefinitionManager.Static.GetComponentGroup(groupId);
			if (componentGroup == null)
			{
				return;
			}
			if (!m_groups.TryGetValue(groupId, out value))
			{
				value = m_listAllocator.Allocate();
				value.Clear();
				for (int i = 0; i <= componentGroup.GetComponentNumber(); i++)
				{
					value.Add(0);
				}
				m_groups.Add(groupId, value);
			}
			value[itemValue] += amount;
			m_totalItemCounter += amount;
		}

		public bool Solve(Dictionary<MyDefinitionId, MyFixedPoint> componentCounts)
		{
			m_solution.Clear();
			m_solvedItemCounter = 0;
			foreach (KeyValuePair<MyDefinitionId, List<int>> group in m_groups)
			{
				MyComponentGroupDefinition componentGroup = MyDefinitionManager.Static.GetComponentGroup(group.Key);
				List<int> value = group.Value;
				UpdatePresentItems(componentGroup, componentCounts);
				for (int i = 1; i <= componentGroup.GetComponentNumber(); i++)
				{
					int num = value[i];
					int num2 = TryRemovePresentItems(i, num);
					if (num2 > 0)
					{
						AddRemovalToSolution(componentGroup.GetComponentDefinition(i).Id, num2);
						value[i] = Math.Max(0, num - num2);
					}
					m_solvedItemCounter += num2;
				}
				for (int num3 = componentGroup.GetComponentNumber(); num3 >= 1; num3--)
				{
					int num4 = value[num3];
					int num5 = TryCreatingItemsBySplit(componentGroup, num3, num4);
					value[num3] = num4 - num5;
					m_solvedItemCounter += num5;
				}
				for (int j = 1; j <= componentGroup.GetComponentNumber(); j++)
				{
					int num6 = value[j];
					if (num6 > 0)
					{
						int num7 = TryCreatingItemsByMerge(componentGroup, j, num6);
						value[j] = num6 - num7;
						m_solvedItemCounter += num7;
					}
				}
			}
			return m_totalItemCounter == m_solvedItemCounter;
		}

		private int GetSolvedItemCount()
		{
			return m_solvedItemCounter;
		}

		public void GetSolution(out List<MyComponentChange> changes)
		{
			changes = m_solution;
		}

		private int TryCreatingItemsBySplit(MyComponentGroupDefinition group, int itemValue, int itemCount)
		{
			int num = 0;
			for (int i = itemValue + 1; i <= group.GetComponentNumber(); i++)
			{
				int num2 = i / itemValue;
				int num3 = itemCount / num2;
				int num4 = itemCount % num2;
				int num5 = (num4 != 0) ? 1 : 0;
				int num6 = TryRemovePresentItems(i, num3 + num5);
				if (num6 > 0)
				{
					int num7 = Math.Min(num6, num3);
					if (num7 != 0)
					{
						int num8 = SplitHelper(group, i, itemValue, num7, num2);
						num += num8;
						itemCount -= num8;
					}
					if (num6 - num3 > 0)
					{
						int num9 = SplitHelper(group, i, itemValue, 1, num4);
						num += num9;
						itemCount -= num9;
					}
				}
			}
			return num;
		}

		private int SplitHelper(MyComponentGroupDefinition group, int splitItemValue, int resultItemValue, int numItemsToSplit, int splitCount)
		{
			int num = splitItemValue - splitCount * resultItemValue;
			MyDefinitionId id = group.GetComponentDefinition(splitItemValue).Id;
			if (num != 0)
			{
				MyDefinitionId id2 = group.GetComponentDefinition(num).Id;
				AddPresentItems(num, numItemsToSplit);
				AddChangeToSolution(id, id2, numItemsToSplit);
			}
			else
			{
				AddRemovalToSolution(id, numItemsToSplit);
			}
			return splitCount * numItemsToSplit;
		}

		private int TryCreatingItemsByMerge(MyComponentGroupDefinition group, int itemValue, int itemCount)
		{
			List<int> list = m_listAllocator.Allocate();
			list.Clear();
			for (int i = 0; i <= group.GetComponentNumber(); i++)
			{
				list.Add(0);
			}
			int num = 0;
			for (int j = 0; j < itemCount; j++)
			{
				int num2 = itemValue;
				for (int num3 = itemValue - 1; num3 >= 1; num3--)
				{
					int value = 0;
					if (m_presentItems.TryGetValue(num3, out value))
					{
						int num4 = Math.Min(num2 / num3, value);
						if (num4 > 0)
						{
							num2 -= num3 * num4;
							value -= num4;
							list[num3] += num4;
						}
					}
				}
				if (num2 == itemValue)
				{
					break;
				}
				if (num2 != 0)
				{
					for (int k = num2 + 1; k <= group.GetComponentNumber(); k++)
					{
						int value2 = 0;
						m_presentItems.TryGetValue(k, out value2);
						if (value2 > list[k])
						{
							MyDefinitionId id = group.GetComponentDefinition(k).Id;
							MyDefinitionId id2 = group.GetComponentDefinition(k - num2).Id;
							AddChangeToSolution(id, id2, 1);
							TryRemovePresentItems(k, 1);
							AddPresentItems(k - num2, 1);
							num2 = 0;
							break;
						}
					}
				}
				if (num2 == 0)
				{
					num++;
					for (int l = 1; l <= group.GetComponentNumber(); l++)
					{
						if (list[l] > 0)
						{
							MyDefinitionId id3 = group.GetComponentDefinition(l).Id;
							TryRemovePresentItems(l, list[l]);
							AddRemovalToSolution(id3, list[l]);
							list[l] = 0;
						}
					}
				}
				else if (num2 > 0)
				{
					break;
				}
			}
			m_listAllocator.Deallocate(list);
			return num;
		}

		private void AddRemovalToSolution(MyDefinitionId removedComponentId, int removeCount)
		{
			for (int i = 0; i < m_solution.Count; i++)
			{
				MyComponentChange value = m_solution[i];
				if ((value.IsChange() || value.IsAddition()) && value.ToAdd == removedComponentId)
				{
					int num = value.Amount - removeCount;
					int amount = Math.Min(removeCount, value.Amount);
					removeCount -= value.Amount;
					if (num > 0)
					{
						value.Amount = num;
						m_solution[i] = value;
					}
					else
					{
						m_solution.RemoveAtFast(i);
					}
					if (value.IsChange())
					{
						m_solution.Add(MyComponentChange.CreateRemoval(value.ToRemove, amount));
					}
					if (removeCount <= 0)
					{
						break;
					}
				}
			}
			if (removeCount > 0)
			{
				m_solution.Add(MyComponentChange.CreateRemoval(removedComponentId, removeCount));
			}
		}

		private void AddChangeToSolution(MyDefinitionId removedComponentId, MyDefinitionId addedComponentId, int numChanged)
		{
			for (int i = 0; i < m_solution.Count; i++)
			{
				MyComponentChange value = m_solution[i];
				if ((value.IsChange() || value.IsAddition()) && value.ToAdd == removedComponentId)
				{
					int num = value.Amount - numChanged;
					int amount = Math.Min(numChanged, value.Amount);
					numChanged -= value.Amount;
					if (num > 0)
					{
						value.Amount = num;
						m_solution[i] = value;
					}
					else
					{
						m_solution.RemoveAtFast(i);
					}
					if (value.IsChange())
					{
						m_solution.Add(MyComponentChange.CreateChange(value.ToRemove, addedComponentId, amount));
					}
					else
					{
						m_solution.Add(MyComponentChange.CreateAddition(addedComponentId, amount));
					}
					if (numChanged <= 0)
					{
						break;
					}
				}
			}
			if (numChanged > 0)
			{
				m_solution.Add(MyComponentChange.CreateChange(removedComponentId, addedComponentId, numChanged));
			}
		}

		private void UpdatePresentItems(MyComponentGroupDefinition group, Dictionary<MyDefinitionId, MyFixedPoint> componentCounts)
		{
			m_presentItems.Clear();
			for (int i = 1; i <= group.GetComponentNumber(); i++)
			{
				MyComponentDefinition componentDefinition = group.GetComponentDefinition(i);
				MyFixedPoint value = 0;
				componentCounts.TryGetValue(componentDefinition.Id, out value);
				m_presentItems[i] = (int)value;
			}
		}

		private int TryRemovePresentItems(int itemValue, int removeCount)
		{
			int value = 0;
			m_presentItems.TryGetValue(itemValue, out value);
			if (value > removeCount)
			{
				m_presentItems[itemValue] = value - removeCount;
				return removeCount;
			}
			m_presentItems.Remove(itemValue);
			return value;
		}

		private void AddPresentItems(int itemValue, int addCount)
		{
			int value = 0;
			m_presentItems.TryGetValue(itemValue, out value);
			value += addCount;
			m_presentItems[itemValue] = value;
		}
	}
}
