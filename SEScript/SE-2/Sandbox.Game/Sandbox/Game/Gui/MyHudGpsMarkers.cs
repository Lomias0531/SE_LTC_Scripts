using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudGpsMarkers
	{
		public class DistanceFromCameraComparer : IComparer<MyGps>
		{
			public int Compare(MyGps first, MyGps second)
			{
				return Vector3D.DistanceSquared(MySector.MainCamera.Position, second.Coords).CompareTo(Vector3D.DistanceSquared(MySector.MainCamera.Position, first.Coords));
			}
		}

		private List<MyGps> m_Inss = new List<MyGps>();

		private DistanceFromCameraComparer m_distFromCamComparer = new DistanceFromCameraComparer();

		public bool Visible
		{
			get;
			set;
		}

		internal List<MyGps> MarkerEntities => m_Inss;

		public MyHudGpsMarkers()
		{
			Visible = true;
		}

		public void RegisterMarker(MyGps ins)
		{
			if (!m_Inss.Contains(ins))
			{
				m_Inss.Add(ins);
			}
		}

		public void UnregisterMarker(MyGps ins)
		{
			m_Inss.Remove(ins);
		}

		public void Clear()
		{
			m_Inss.Clear();
		}

		internal void Sort(DistanceFromCameraComparer distComparer)
		{
			m_Inss.Sort(distComparer);
		}

		internal void Sort()
		{
			Sort(m_distFromCamComparer);
		}
	}
}
