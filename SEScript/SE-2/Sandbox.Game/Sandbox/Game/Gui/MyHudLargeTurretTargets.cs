using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
	public class MyHudLargeTurretTargets
	{
		private Dictionary<MyEntity, MyHudEntityParams> m_markers = new Dictionary<MyEntity, MyHudEntityParams>();

		public bool Visible
		{
			get;
			set;
		}

		internal Dictionary<MyEntity, MyHudEntityParams> Targets => m_markers;

		public MyHudLargeTurretTargets()
		{
			Visible = true;
		}

		internal void RegisterMarker(MyEntity target, MyHudEntityParams hudParams)
		{
			m_markers[target] = hudParams;
		}

		internal void UnregisterMarker(MyEntity target)
		{
			m_markers.Remove(target);
		}
	}
}
