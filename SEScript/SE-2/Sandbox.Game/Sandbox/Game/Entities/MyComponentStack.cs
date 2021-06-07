using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
	public class MyComponentStack
	{
		public struct GroupInfo
		{
			public int MountedCount;

			public int TotalCount;

			public int AvailableCount;

			public float Integrity;

			public float MaxIntegrity;

			public MyComponentDefinition Component;
		}

		public const float MOUNT_THRESHOLD = 1.52590219E-05f;

		private readonly MyCubeBlockDefinition m_blockDefinition;

		private float m_buildIntegrity;

		private float m_integrity;

		private bool m_yieldLastComponent = true;

		private ushort m_topGroupIndex;

		private ushort m_topComponentIndex;

		public int LastChangeStamp
		{
			get;
			private set;
		}

		public bool YieldLastComponent => m_yieldLastComponent;

		public bool IsFullIntegrity => m_integrity >= MaxIntegrity;

		public bool IsFullyDismounted => m_integrity < 1.52590219E-05f;

		public bool IsDestroyed => m_integrity < 1.52590219E-05f;

		public float Integrity
		{
			get
			{
				return m_integrity;
			}
			private set
			{
				if (m_integrity != value)
				{
					bool isFunctional = IsFunctional;
					m_integrity = value;
					CheckFunctionalState(isFunctional);
				}
			}
		}

		public float IntegrityRatio => Integrity / MaxIntegrity;

		public float MaxIntegrity => m_blockDefinition.MaxIntegrity;

		public float BuildRatio => m_buildIntegrity / MaxIntegrity;

		public float BuildIntegrity
		{
			get
			{
				return m_buildIntegrity;
			}
			private set
			{
				if (m_buildIntegrity != value)
				{
					bool isFunctional = IsFunctional;
					m_buildIntegrity = value;
					CheckFunctionalState(isFunctional);
				}
			}
		}

		public static float NewBlockIntegrity
		{
			get
			{
				if (!MySession.Static.SurvivalMode)
				{
					return 1f;
				}
				return 1.52590219E-05f;
			}
		}

		public bool IsFunctional
		{
			get
			{
				if (IsBuilt)
				{
					return Integrity > MaxIntegrity * m_blockDefinition.CriticalIntegrityRatio;
				}
				return false;
			}
		}

		public bool IsBuilt => BuildIntegrity >= MaxIntegrity * m_blockDefinition.FinalModelThreshold();

		public int GroupCount => m_blockDefinition.Components.Length;

		public event Action IsFunctionalChanged;

		public bool? WillFunctionalityRise(float mountAmount)
		{
			bool num = Integrity > MaxIntegrity * m_blockDefinition.CriticalIntegrityRatio;
			bool flag = Integrity + mountAmount * m_blockDefinition.IntegrityPointsPerSec > MaxIntegrity * m_blockDefinition.CriticalIntegrityRatio;
			if (num == flag)
			{
				return null;
			}
			return flag;
		}

		private void CheckFunctionalState(bool oldFunctionalState)
		{
			if (IsFunctional != oldFunctionalState && this.IsFunctionalChanged != null)
			{
				this.IsFunctionalChanged();
			}
		}

		public MyComponentStack(MyCubeBlockDefinition BlockDefinition, float integrityPercent, float buildPercent)
		{
			m_blockDefinition = BlockDefinition;
			float maxIntegrity = BlockDefinition.MaxIntegrity;
			BuildIntegrity = maxIntegrity * buildPercent;
			Integrity = maxIntegrity * integrityPercent;
			UpdateIndices();
			if (Integrity != 0f)
			{
				float topComponentIntegrity = GetTopComponentIntegrity();
				if (topComponentIntegrity < 1.52590219E-05f)
				{
					Integrity += 1.52590219E-05f - topComponentIntegrity;
				}
				if (topComponentIntegrity > (float)BlockDefinition.Components[m_topGroupIndex].Definition.MaxIntegrity)
				{
					Integrity -= topComponentIntegrity - (float)BlockDefinition.Components[m_topGroupIndex].Definition.MaxIntegrity;
				}
			}
			LastChangeStamp = 1;
		}

		private float GetTopComponentIntegrity()
		{
			float num = Integrity;
			MyCubeBlockDefinition.Component[] components = m_blockDefinition.Components;
			for (int i = 0; i < m_topGroupIndex; i++)
			{
				num -= (float)(components[i].Definition.MaxIntegrity * components[i].Count);
			}
			return num - (float)(components[m_topGroupIndex].Definition.MaxIntegrity * m_topComponentIndex);
		}

		private void SetTopIndex(int newTopGroupIndex, int newTopComponentIndex)
		{
			m_topGroupIndex = (ushort)newTopGroupIndex;
			m_topComponentIndex = (ushort)newTopComponentIndex;
		}

		private void UpdateIndices()
		{
			float integrity = Integrity;
			MyCubeBlockDefinition blockDefinition = m_blockDefinition;
			int topGroupIndex = 0;
			int topComponentIndex = 0;
			CalculateIndicesInternal(integrity, blockDefinition, ref topGroupIndex, ref topComponentIndex);
			SetTopIndex(topGroupIndex, topComponentIndex);
		}

		private static void CalculateIndicesInternal(float integrity, MyCubeBlockDefinition blockDef, ref int topGroupIndex, ref int topComponentIndex)
		{
			float num = integrity;
			MyCubeBlockDefinition.Component[] components = blockDef.Components;
			int num2 = 0;
			num2 = 0;
			while (true)
			{
				if (num2 < components.Length)
				{
					float num3 = components[num2].Definition.MaxIntegrity * components[num2].Count;
					if (!(num >= num3))
					{
						break;
					}
					num -= num3;
					if (num <= 1.52590219E-05f)
					{
						topGroupIndex = num2;
						topComponentIndex = components[num2].Count - 1;
						return;
					}
					num2++;
					continue;
				}
				return;
			}
			int num4 = (int)(num / (float)components[num2].Definition.MaxIntegrity);
			if (num - (float)(components[num2].Definition.MaxIntegrity * num4) < 7.629511E-06f && num4 != 0)
			{
				topGroupIndex = num2;
				topComponentIndex = num4 - 1;
			}
			else
			{
				topGroupIndex = num2;
				topComponentIndex = num4;
			}
		}

		public void UpdateBuildIntegrityUp()
		{
			if (BuildIntegrity < Integrity)
			{
				BuildIntegrity = Integrity;
				LastChangeStamp++;
			}
		}

		public void UpdateBuildIntegrityDown(float ratio)
		{
			if (BuildIntegrity > Integrity * ratio)
			{
				BuildIntegrity = Integrity * ratio;
				LastChangeStamp++;
			}
		}

		public bool CanContinueBuild(MyInventoryBase inventory, MyConstructionStockpile stockpile)
		{
			if (IsFullIntegrity)
			{
				return false;
			}
			if (GetTopComponentIntegrity() < (float)m_blockDefinition.Components[m_topGroupIndex].Definition.MaxIntegrity)
			{
				return true;
			}
			int num = m_topGroupIndex;
			if (m_topComponentIndex == m_blockDefinition.Components[num].Count - 1)
			{
				num++;
			}
			MyComponentDefinition definition = m_blockDefinition.Components[num].Definition;
			if (stockpile != null && stockpile.GetItemAmount(definition.Id) > 0)
			{
				return true;
			}
			if (inventory != null && MyCubeBuilder.BuildComponent.GetItemAmountCombined(inventory, definition.Id) > 0)
			{
				return true;
			}
			return false;
		}

		public void GetMissingInfo(out int groupIndex, out int componentCount)
		{
			if (IsFullIntegrity)
			{
				groupIndex = 0;
				componentCount = 0;
				return;
			}
			if (GetTopComponentIntegrity() < (float)m_blockDefinition.Components[m_topGroupIndex].Definition.MaxIntegrity)
			{
				groupIndex = 0;
				componentCount = 0;
				return;
			}
			int num = m_topComponentIndex + 1;
			groupIndex = m_topGroupIndex;
			if (num == m_blockDefinition.Components[groupIndex].Count)
			{
				groupIndex++;
				num = 0;
			}
			componentCount = m_blockDefinition.Components[groupIndex].Count - num;
		}

		public void DestroyCompletely()
		{
			BuildIntegrity = 0f;
			Integrity = 0f;
			UpdateIndices();
		}

		private bool CheckOrMountFirstComponent(MyConstructionStockpile stockpile = null)
		{
			if (Integrity > 7.629511E-06f)
			{
				return true;
			}
			MyComponentDefinition definition = m_blockDefinition.Components[0].Definition;
			if (stockpile == null || stockpile.RemoveItems(1, definition.Id))
			{
				Integrity = 1.52590219E-05f;
				UpdateBuildIntegrityUp();
				return true;
			}
			return false;
		}

		public void GetMissingComponents(Dictionary<string, int> addToDictionary, MyConstructionStockpile availableItems = null)
		{
			int topGroupIndex = m_topGroupIndex;
			MyCubeBlockDefinition.Component component = m_blockDefinition.Components[topGroupIndex];
			int num = m_topComponentIndex + 1;
			if (IsFullyDismounted)
			{
				num--;
			}
			if (num < component.Count)
			{
				string subtypeName = component.Definition.Id.SubtypeName;
				if (addToDictionary.ContainsKey(subtypeName))
				{
					addToDictionary[subtypeName] += component.Count - num;
				}
				else
				{
					addToDictionary[subtypeName] = component.Count - num;
				}
			}
			for (topGroupIndex++; topGroupIndex < m_blockDefinition.Components.Length; topGroupIndex++)
			{
				component = m_blockDefinition.Components[topGroupIndex];
				string subtypeName2 = component.Definition.Id.SubtypeName;
				if (addToDictionary.ContainsKey(subtypeName2))
				{
					addToDictionary[subtypeName2] += component.Count;
				}
				else
				{
					addToDictionary[subtypeName2] = component.Count;
				}
			}
			if (availableItems == null)
			{
				return;
			}
			for (topGroupIndex = 0; topGroupIndex < addToDictionary.Keys.Count; topGroupIndex++)
			{
				string text = addToDictionary.Keys.ElementAt(topGroupIndex);
				addToDictionary[text] -= availableItems.GetItemAmount(new MyDefinitionId(typeof(MyObjectBuilder_Component), text));
				if (addToDictionary[text] <= 0)
				{
					addToDictionary.Remove(text);
					topGroupIndex--;
				}
			}
		}

		public static void GetMountedComponents(MyComponentList addToList, MyObjectBuilder_CubeBlock block)
		{
			int topGroupIndex = 0;
			int topComponentIndex = 0;
			MyCubeBlockDefinition blockDefinition = null;
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(block.GetId(), out blockDefinition);
			if (blockDefinition == null || block == null)
			{
				return;
			}
			float num = block.IntegrityPercent * blockDefinition.MaxIntegrity;
			CalculateIndicesInternal(num, blockDefinition, ref topGroupIndex, ref topComponentIndex);
			if (topGroupIndex < blockDefinition.Components.Length && topComponentIndex < blockDefinition.Components[topGroupIndex].Count)
			{
				int num2 = topComponentIndex;
				if (num >= 1.52590219E-05f)
				{
					num2++;
				}
				for (int i = 0; i < topGroupIndex; i++)
				{
					MyCubeBlockDefinition.Component component = blockDefinition.Components[i];
					addToList.AddMaterial(component.Definition.Id, component.Count, component.Count, addToDisplayList: false);
				}
				MyDefinitionId id = blockDefinition.Components[topGroupIndex].Definition.Id;
				addToList.AddMaterial(id, num2, num2, addToDisplayList: false);
			}
		}

		public void IncreaseMountLevel(float mountAmount, MyConstructionStockpile stockpile = null)
		{
			_ = IsFunctional;
			IncreaseMountLevelInternal(mountAmount, stockpile);
			LastChangeStamp++;
		}

		private void IncreaseMountLevelInternal(float mountAmount, MyConstructionStockpile stockpile = null)
		{
			if (!CheckOrMountFirstComponent(stockpile))
			{
				return;
			}
			float num = GetTopComponentIntegrity();
			float num2 = m_blockDefinition.Components[m_topGroupIndex].Definition.MaxIntegrity;
			int num3 = m_topGroupIndex;
			int num4 = m_topComponentIndex;
			while (true)
			{
				if (mountAmount > 0f)
				{
					float num5 = num2 - num;
					if (mountAmount < num5)
					{
						Integrity += mountAmount;
						UpdateBuildIntegrityUp();
						return;
					}
					Integrity += num5 + 1.52590219E-05f;
					mountAmount -= num5 + 1.52590219E-05f;
					num4++;
					if (num4 >= m_blockDefinition.Components[m_topGroupIndex].Count)
					{
						num3++;
						num4 = 0;
					}
					if (num3 == m_blockDefinition.Components.Length)
					{
						Integrity = MaxIntegrity;
						UpdateBuildIntegrityUp();
						return;
					}
					MyComponentDefinition definition = m_blockDefinition.Components[num3].Definition;
					if (stockpile != null && !stockpile.RemoveItems(1, definition.Id))
					{
						break;
					}
					UpdateBuildIntegrityUp();
					SetTopIndex(num3, num4);
					num = 1.52590219E-05f;
					num2 = m_blockDefinition.Components[num3].Definition.MaxIntegrity;
					continue;
				}
				return;
			}
			Integrity -= 1.52590219E-05f;
			UpdateBuildIntegrityUp();
		}

		public void DecreaseMountLevel(float unmountAmount, MyConstructionStockpile outputStockpile = null, bool useDefaultDeconstructEfficiency = false)
		{
			float ratio = BuildIntegrity / Integrity;
			UnmountInternal(unmountAmount, outputStockpile, damageItems: false, useDefaultDeconstructEfficiency);
			UpdateBuildIntegrityDown(ratio);
			LastChangeStamp++;
		}

		public void ApplyDamage(float damage, MyConstructionStockpile outputStockpile = null)
		{
			UnmountInternal(damage, outputStockpile, damageItems: true);
			float ratio = BuildIntegrity / Integrity;
			UpdateBuildIntegrityDown(ratio);
			LastChangeStamp++;
		}

		private float GetDeconstructionEfficiency(int groupIndex, bool useDefault)
		{
			if (!useDefault)
			{
				return m_blockDefinition.Components[groupIndex].Definition.DeconstructionEfficiency;
			}
			return 1f;
		}

		private void UnmountInternal(float unmountAmount, MyConstructionStockpile outputStockpile = null, bool damageItems = false, bool useDefaultDeconstructEfficiency = false)
		{
			float num = GetTopComponentIntegrity();
			int num2 = m_topGroupIndex;
			int num3 = m_topComponentIndex;
			MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = null;
			MyObjectBuilder_Ore scrapBuilder = MyFloatingObject.ScrapBuilder;
			while (unmountAmount * GetDeconstructionEfficiency(num2, damageItems || useDefaultDeconstructEfficiency) >= num)
			{
				Integrity -= num;
				unmountAmount -= num;
				if (outputStockpile != null && MySession.Static.SurvivalMode)
				{
					bool flag = damageItems && MyFakes.ENABLE_DAMAGED_COMPONENTS;
					if (!damageItems || (flag && MyRandom.Instance.NextFloat() <= m_blockDefinition.Components[num2].Definition.DropProbability))
					{
						myObjectBuilder_PhysicalObject = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(m_blockDefinition.Components[num2].DeconstructItem.Id);
						if (flag)
						{
							myObjectBuilder_PhysicalObject.Flags |= MyItemFlags.Damaged;
						}
						if (Integrity > 0f || m_yieldLastComponent)
						{
							outputStockpile.AddItems(1, myObjectBuilder_PhysicalObject);
						}
					}
					MyComponentDefinition definition = m_blockDefinition.Components[num2].Definition;
					if (MyFakes.ENABLE_SCRAP && damageItems && MyRandom.Instance.NextFloat() < definition.DropProbability && (Integrity > 0f || m_yieldLastComponent))
					{
						outputStockpile.AddItems((int)(0.8f * definition.Mass), scrapBuilder);
					}
				}
				num3--;
				if (num3 < 0)
				{
					num2--;
					if (num2 < 0)
					{
						SetTopIndex(0, 0);
						Integrity = 0f;
						return;
					}
					num3 = m_blockDefinition.Components[num2].Count - 1;
				}
				num = m_blockDefinition.Components[num2].Definition.MaxIntegrity;
				SetTopIndex(num2, num3);
			}
			Integrity -= unmountAmount * GetDeconstructionEfficiency(num2, damageItems || useDefaultDeconstructEfficiency);
			num -= unmountAmount * GetDeconstructionEfficiency(num2, damageItems || useDefaultDeconstructEfficiency);
			if (num < 1.52590219E-05f)
			{
				Integrity += 1.52590219E-05f - num;
				num = 1.52590219E-05f;
			}
		}

		internal void SetIntegrity(float buildIntegrity, float integrity)
		{
			Integrity = integrity;
			BuildIntegrity = buildIntegrity;
			UpdateIndices();
			LastChangeStamp++;
		}

		public GroupInfo GetGroupInfo(int index)
		{
			MyCubeBlockDefinition.Component component = m_blockDefinition.Components[index];
			GroupInfo groupInfo = default(GroupInfo);
			groupInfo.Component = component.Definition;
			groupInfo.TotalCount = component.Count;
			groupInfo.MountedCount = 0;
			groupInfo.AvailableCount = 0;
			groupInfo.Integrity = 0f;
			groupInfo.MaxIntegrity = component.Count * component.Definition.MaxIntegrity;
			GroupInfo result = groupInfo;
			if (index < m_topGroupIndex)
			{
				result.MountedCount = component.Count;
				result.Integrity = component.Count * component.Definition.MaxIntegrity;
			}
			else if (index == m_topGroupIndex)
			{
				result.MountedCount = m_topComponentIndex + 1;
				result.Integrity = GetTopComponentIntegrity() + (float)(m_topComponentIndex * component.Definition.MaxIntegrity);
			}
			return result;
		}

		public void DisableLastComponentYield()
		{
			m_yieldLastComponent = false;
		}
	}
}
