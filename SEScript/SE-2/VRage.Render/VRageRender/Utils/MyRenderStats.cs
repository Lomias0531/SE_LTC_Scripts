using System.Collections.Generic;
using VRage.Stats;

namespace VRageRender.Utils
{
	/// <summary>
	/// Draws statistics
	/// </summary>
	public static class MyRenderStats
	{
		public enum ColumnEnum
		{
			Left,
			Right
		}

		public static Dictionary<ColumnEnum, List<MyStats>> m_stats;

		public static readonly MyStats Generic;

		static MyRenderStats()
		{
			Generic = new MyStats();
			Generic = new MyStats();
			m_stats = new Dictionary<ColumnEnum, List<MyStats>>(EqualityComparer<ColumnEnum>.Default)
			{
				{
					ColumnEnum.Left,
					new List<MyStats>
					{
						Generic
					}
				},
				{
					ColumnEnum.Right,
					new List<MyStats>()
				}
			};
		}

		public static void SetColumn(ColumnEnum column, params MyStats[] stats)
		{
			if (!m_stats.TryGetValue(column, out List<MyStats> value))
			{
				value = new List<MyStats>();
				m_stats[column] = value;
			}
			value.Clear();
			value.AddRange(stats);
		}
	}
}
