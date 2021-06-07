using Sandbox.Game.EntityComponents;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 999, typeof(MyObjectBuilder_SessionComponentAssetModifiers), null)]
	public class MySessionComponentAssetModifiers : MySessionComponentBase
	{
		public static readonly byte[] INVALID_CHECK_DATA = new byte[1]
		{
			255
		};

		private List<MyAssetModifierComponent> m_componentListForLazyUpdates = new List<MyAssetModifierComponent>();

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			int num = 0;
			while (num < m_componentListForLazyUpdates.Count)
			{
				bool flag = true;
				if (m_componentListForLazyUpdates[num].Entity != null && !m_componentListForLazyUpdates[num].Entity.Closed && !m_componentListForLazyUpdates[num].Entity.MarkedForClose)
				{
					flag = m_componentListForLazyUpdates[num].LazyUpdate();
				}
				if (flag)
				{
					m_componentListForLazyUpdates.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}

		public void RegisterComponentForLazyUpdate(MyAssetModifierComponent comp)
		{
			lock (m_componentListForLazyUpdates)
			{
				m_componentListForLazyUpdates.Add(comp);
			}
		}
	}
}
