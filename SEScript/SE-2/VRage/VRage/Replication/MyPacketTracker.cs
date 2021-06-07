using System.Collections.Generic;

namespace VRage.Replication
{
	public class MyPacketTracker
	{
		public enum OrderType
		{
			InOrder,
			OutOfOrder,
			Duplicate,
			Drop1,
			Drop2,
			Drop3,
			Drop4,
			DropX
		}

		private const int BUFFER_LENGTH = 5;

		private readonly List<byte> m_ids = new List<byte>();

		public MyPacketStatistics Statistics
		{
			get;
			set;
		}

		public OrderType Add(byte id)
		{
			if (m_ids.Count == 1 && id == (byte)(m_ids[0] + 1))
			{
				m_ids[0] = id;
				return OrderType.InOrder;
			}
			if (m_ids.FindIndex((byte x) => x == id) != -1)
			{
				return OrderType.Duplicate;
			}
			m_ids.Add(id);
			for (int i = 2; i < m_ids.Count; i++)
			{
				if ((byte)(m_ids[0] + 1) == m_ids[i])
				{
					m_ids.RemoveAt(i);
					m_ids.RemoveAt(0);
					CleanUp();
					return OrderType.OutOfOrder;
				}
			}
			if (m_ids.Count >= 5)
			{
				int num = m_ids[0];
				m_ids.RemoveAt(0);
				byte num2 = m_ids[0];
				CleanUp();
				int num3 = (byte)(num2 - num) - 2;
				return (OrderType)(3 + num3);
			}
			return OrderType.InOrder;
		}

		private void CleanUp()
		{
			byte b = 0;
			bool flag = true;
			bool flag2 = true;
			foreach (byte id in m_ids)
			{
				flag2 = (flag2 && (flag || (byte)(b + 1) == id));
				b = id;
				flag = false;
			}
			if (flag2)
			{
				m_ids.RemoveRange(0, m_ids.Count - 1);
			}
		}
	}
}
