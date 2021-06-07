using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGridColorHelper
	{
		private Dictionary<MyCubeGrid, Color> m_colors = new Dictionary<MyCubeGrid, Color>();

		private int m_lastColorIndex;

		public void Init(MyCubeGrid mainGrid = null)
		{
			m_lastColorIndex = 0;
			m_colors.Clear();
			if (mainGrid != null)
			{
				m_colors.Add(mainGrid, Color.White);
			}
		}

		public Color GetGridColor(MyCubeGrid grid)
		{
			if (!m_colors.TryGetValue(grid, out Color value))
			{
				do
				{
					value = new Vector3((float)(m_lastColorIndex++ % 20) / 20f, 0.75f, 1f).HSVtoColor();
				}
				while (value.HueDistance(Color.Red) < 0.04f || value.HueDistance(0.65f) < 0.07f);
				m_colors[grid] = value;
			}
			return value;
		}
	}
}
