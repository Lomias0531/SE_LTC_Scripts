using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
	public class MyHudLocationMarkers
	{
		private SortedList<long, MyHudEntityParams> m_markerEntities = new SortedList<long, MyHudEntityParams>();

		public bool Visible
		{
			get;
			set;
		}

		public SortedList<long, MyHudEntityParams> MarkerEntities => m_markerEntities;

		public MyHudLocationMarkers()
		{
			Visible = true;
		}

		public void RegisterMarker(MyEntity entity, MyHudEntityParams hudParams)
		{
			if (hudParams.Entity == null)
			{
				hudParams.Entity = entity;
			}
			RegisterMarker(entity.EntityId, hudParams);
		}

		public void RegisterMarker(long entityId, MyHudEntityParams hudParams)
		{
			m_markerEntities[entityId] = hudParams;
		}

		public void UnregisterMarker(MyEntity entity)
		{
			UnregisterMarker(entity.EntityId);
		}

		public void UnregisterMarker(long entityId)
		{
			m_markerEntities.Remove(entityId);
		}

		public void Clear()
		{
			m_markerEntities.Clear();
		}
	}
}
