using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.Entities.EnvironmentItems
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500)]
	public class MyEnvironmentItemsCoordinator : MySessionComponentBase
	{
		private struct TransferData
		{
			public MyEnvironmentItems From;

			public MyEnvironmentItems To;

			public int LocalId;

			public MyStringHash SubtypeId;
		}

		private static MyEnvironmentItemsCoordinator Static;

		private HashSet<MyEnvironmentItems> m_tmpItems;

		private List<TransferData> m_transferList;

		private float? m_transferTime;

		public override bool IsRequiredByGame => false;

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			if (m_transferTime.HasValue)
			{
				FinalizeTransfers();
			}
			return base.GetObjectBuilder();
		}

		public override void LoadData()
		{
			base.LoadData();
			m_transferList = new List<TransferData>();
			m_tmpItems = new HashSet<MyEnvironmentItems>();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_transferTime.HasValue)
			{
				m_transferTime = m_transferTime.Value - 0.0166666675f;
				if (m_transferTime < 0f)
				{
					FinalizeTransfers();
				}
			}
		}

		private void FinalizeTransfers()
		{
			foreach (TransferData transfer in m_transferList)
			{
				if (MakeTransfer(transfer))
				{
					m_tmpItems.Add(transfer.To);
				}
			}
			m_transferList.Clear();
			m_transferTime = null;
			foreach (MyEnvironmentItems tmpItem in m_tmpItems)
			{
				tmpItem.EndBatch(sync: true);
			}
			m_tmpItems.Clear();
		}

		private bool MakeTransfer(TransferData data)
		{
			if (!data.From.TryGetItemInfoById(data.LocalId, out MyEnvironmentItems.ItemInfo result))
			{
				return false;
			}
			data.From.RemoveItem(data.LocalId, sync: true);
			if (!data.To.IsBatching)
			{
				data.To.BeginBatch(sync: true);
			}
			data.To.BatchAddItem(result.Transform.Position, data.SubtypeId, sync: true);
			return true;
		}

		private void StartTimer(int updateTimeS)
		{
			if (!m_transferTime.HasValue)
			{
				m_transferTime = updateTimeS;
			}
		}

		public static void TransferItems(MyEnvironmentItems from, MyEnvironmentItems to, int localId, MyStringHash subtypeId, int timeS = 10)
		{
			Static.AddTransferData(from, to, localId, subtypeId);
			Static.StartTimer(timeS);
		}

		private void AddTransferData(MyEnvironmentItems from, MyEnvironmentItems to, int localId, MyStringHash subtypeId)
		{
			m_transferList.Add(new TransferData
			{
				From = from,
				To = to,
				LocalId = localId,
				SubtypeId = subtypeId
			});
		}
	}
}
