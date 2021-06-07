using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudSinkGroupInfo
	{
		private bool m_needsRefresh = true;

		private float[] m_missingPowerByGroup;

		private MyStringId[] m_groupNames;

		private float m_missingTotal;

		public int GroupCount;

		private int m_workingGroupCount;

		private bool m_visible;

		private MyHudNameValueData m_data;

		public int WorkingGroupCount
		{
			get
			{
				return m_workingGroupCount;
			}
			set
			{
				if (m_workingGroupCount != value)
				{
					m_workingGroupCount = value;
					m_needsRefresh = true;
				}
			}
		}

		public bool Visible
		{
			get
			{
				if (m_visible)
				{
					return WorkingGroupCount != GroupCount;
				}
				return false;
			}
			set
			{
				m_visible = value;
			}
		}

		public MyHudNameValueData Data
		{
			get
			{
				if (m_needsRefresh)
				{
					Refresh();
				}
				return m_data;
			}
		}

		public MyHudSinkGroupInfo()
		{
			Reload();
		}

		public void Reload()
		{
			MyResourceDistributorComponent.InitializeMappings();
			if (MyResourceDistributorComponent.SinkGroupPrioritiesTotal != -1 && (m_groupNames == null || m_groupNames.Length < MyResourceDistributorComponent.SinkGroupPrioritiesTotal))
			{
				GroupCount = MyResourceDistributorComponent.SinkGroupPrioritiesTotal;
				WorkingGroupCount = GroupCount;
				m_groupNames = new MyStringId[GroupCount];
				m_missingPowerByGroup = new float[GroupCount];
				m_data = new MyHudNameValueData(GroupCount + 1, "Blue", "White", 0.025f, showBackgroundFog: true);
			}
			if (m_groupNames != null)
			{
				ListReader<MyResourceDistributionGroupDefinition> definitionsOfType = MyDefinitionManager.Static.GetDefinitionsOfType<MyResourceDistributionGroupDefinition>();
				DictionaryReader<MyStringHash, int> sinkSubtypesToPriority = MyResourceDistributorComponent.SinkSubtypesToPriority;
				foreach (MyResourceDistributionGroupDefinition item in definitionsOfType)
				{
					if (!item.IsSource && sinkSubtypesToPriority.TryGetValue(item.Id.SubtypeId, out int value) && value < GroupCount)
					{
						m_groupNames[value] = MyStringId.GetOrCompute(item.Id.SubtypeName);
					}
				}
				Data[GroupCount].NameFont = "Red";
				Data[GroupCount].ValueFont = "Red";
			}
		}

		internal void SetGroupDeficit(int groupIndex, float missingPower)
		{
			if (m_missingPowerByGroup == null)
			{
				Reload();
			}
			m_missingTotal += missingPower - m_missingPowerByGroup[groupIndex];
			m_missingPowerByGroup[groupIndex] = missingPower;
			m_needsRefresh = true;
		}

		private void Refresh()
		{
			m_needsRefresh = false;
			MyHudNameValueData data = Data;
			for (int i = 0; i < data.Count - 1; i++)
			{
				data[i].Name.Clear().AppendStringBuilder(MyTexts.Get(m_groupNames[i]));
			}
			data[GroupCount].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudEnergyMissingTotal));
			MyHudNameValueData.Data data2 = data[GroupCount];
			data2.Value.Clear();
			MyValueFormatter.AppendWorkInBestUnit(0f - m_missingTotal, data2.Value);
			for (int j = 0; j < GroupCount; j++)
			{
				data2 = data[j];
				if (j < m_workingGroupCount)
				{
					data2.NameFont = (data2.ValueFont = null);
				}
				else
				{
					data2.NameFont = (data2.ValueFont = "Red");
				}
				data2.Value.Clear();
				MyValueFormatter.AppendWorkInBestUnit(0f - m_missingPowerByGroup[j], data2.Value);
			}
		}
	}
}
