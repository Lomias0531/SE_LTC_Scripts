using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudBlockInfo
	{
		public struct ComponentInfo
		{
			public MyDefinitionId DefinitionId;

			public string[] Icons;

			public string ComponentName;

			public int MountedCount;

			public int StockpileCount;

			public int TotalCount;

			public int AvailableAmount;

			public int InstalledCount => MountedCount + StockpileCount;

			public override string ToString()
			{
				return $"{MountedCount}/{StockpileCount}/{TotalCount} {ComponentName}";
			}
		}

		public bool ShowDetails;

		public List<ComponentInfo> Components = new List<ComponentInfo>(12);

		public string BlockName;

		private string m_contextHelp;

		public string[] BlockIcons;

		public float BlockIntegrity;

		public float CriticalIntegrity;

		public float OwnershipIntegrity;

		public bool ShowAvailable;

		public int CriticalComponentIndex = -1;

		public int MissingComponentIndex = -1;

		public int PCUCost;

		public long BlockBuiltBy;

		public MyCubeSize GridSize;

		public bool Visible;

		public string ContextHelp => m_contextHelp;

		public event Action<string> ContextHelpChanged;

		public void SetContextHelp(MyDefinitionBase definition)
		{
			if (!string.IsNullOrEmpty(definition.DescriptionText))
			{
				if (string.IsNullOrEmpty(definition.DescriptionArgs))
				{
					m_contextHelp = definition.DescriptionText;
				}
				else
				{
					string[] array = definition.DescriptionArgs.Split(new char[1]
					{
						','
					});
					object[] array2 = new object[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = MyIngameHelpObjective.GetHighlightedControl(MyStringId.GetOrCompute(array[i]));
					}
					m_contextHelp = string.Format(definition.DescriptionText, array2);
				}
			}
			else
			{
				m_contextHelp = MyTexts.GetString(MySpaceTexts.Description_NotAvailable);
			}
			this.ContextHelpChanged?.Invoke(m_contextHelp);
		}
	}
}
