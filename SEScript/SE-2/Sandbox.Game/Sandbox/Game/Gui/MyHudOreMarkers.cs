using Sandbox.Game.Entities.Cube;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
	public class MyHudOreMarkers : IEnumerable<MyEntityOreDeposit>, IEnumerable
	{
		private readonly HashSet<MyEntityOreDeposit> m_markers = new HashSet<MyEntityOreDeposit>(MyEntityOreDeposit.Comparer);

		public bool Visible
		{
			get;
			set;
		}

		public MyHudOreMarkers()
		{
			Visible = true;
		}

		internal void RegisterMarker(MyEntityOreDeposit deposit)
		{
			m_markers.Add(deposit);
		}

		internal void UnregisterMarker(MyEntityOreDeposit deposit)
		{
			m_markers.Remove(deposit);
		}

		internal void Clear()
		{
			m_markers.Clear();
		}

		public HashSet<MyEntityOreDeposit>.Enumerator GetEnumerator()
		{
			return m_markers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<MyEntityOreDeposit> IEnumerable<MyEntityOreDeposit>.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
