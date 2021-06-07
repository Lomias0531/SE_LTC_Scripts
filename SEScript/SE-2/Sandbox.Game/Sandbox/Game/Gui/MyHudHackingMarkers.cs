using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
	public class MyHudHackingMarkers
	{
		private Dictionary<long, MyHudEntityParams> m_markerEntities = new Dictionary<long, MyHudEntityParams>();

		private Dictionary<long, float> m_blinkingTimes = new Dictionary<long, float>();

		private List<long> m_removeList = new List<long>();

		public bool Visible
		{
			get;
			set;
		}

		internal Dictionary<long, MyHudEntityParams> MarkerEntities => m_markerEntities;

		public MyHudHackingMarkers()
		{
			Visible = true;
		}

		internal void UpdateMarkers()
		{
			m_removeList.Clear();
			foreach (KeyValuePair<long, MyHudEntityParams> markerEntity in m_markerEntities)
			{
				if (m_blinkingTimes[markerEntity.Key] <= 0.0166666675f)
				{
					m_removeList.Add(markerEntity.Key);
				}
				else
				{
					m_blinkingTimes[markerEntity.Key] -= 0.0166666675f;
				}
			}
			foreach (long remove in m_removeList)
			{
				UnregisterMarker(remove);
			}
			m_removeList.Clear();
		}

		internal void RegisterMarker(MyEntity entity, MyHudEntityParams hudParams)
		{
			RegisterMarker(entity.EntityId, hudParams);
		}

		internal void RegisterMarker(long entityId, MyHudEntityParams hudParams)
		{
			m_markerEntities[entityId] = hudParams;
			m_blinkingTimes[entityId] = hudParams.BlinkingTime;
		}

		internal void UnregisterMarker(MyEntity entity)
		{
			UnregisterMarker(entity.EntityId);
		}

		internal void UnregisterMarker(long entityId)
		{
			m_markerEntities.Remove(entityId);
			m_blinkingTimes.Remove(entityId);
		}

		public void Clear()
		{
			m_markerEntities.Clear();
			m_blinkingTimes.Clear();
		}
	}
}
