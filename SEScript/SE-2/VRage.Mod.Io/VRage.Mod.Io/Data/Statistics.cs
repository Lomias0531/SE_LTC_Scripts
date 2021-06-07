using System;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class Statistics
	{
		public int mod_id;

		public int popularity_rank_position;

		public int popularity_rank_total_mods;

		public int downloads_total;

		public int subscribers_total;

		public int ratings_total;

		public int ratings_positive;

		public int ratings_negative;

		public int ratings_percentage_positive;

		public double ratings_weighted_aggregate;

		public string ratings_display_text;

		public int date_expires;
	}
}
