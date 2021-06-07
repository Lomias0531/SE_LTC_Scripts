using VRageMath;

namespace VRage.Game
{
	public static class MyConstants
	{
		public static readonly float FIELD_OF_VIEW_CONFIG_MIN;

		public static readonly float FIELD_OF_VIEW_CONFIG_MAX;

		public static readonly float FIELD_OF_VIEW_CONFIG_MAX_SAFE;

		public static readonly float FIELD_OF_VIEW_CONFIG_DEFAULT;

		public static readonly float FIELD_OF_VIEW_CONFIG_MAX_DUAL_HEAD;

		public static readonly float FIELD_OF_VIEW_CONFIG_MAX_TRIPLE_HEAD;

		public const int FAREST_TIME_IN_PAST = -60000;

		public static readonly Vector3D GAME_PRUNING_STRUCTURE_AABB_EXTENSION;

		public static float DEFAULT_INTERACTIVE_DISTANCE;

		public static float DEFAULT_GROUND_SEARCH_DISTANCE;

		public static float FLOATING_OBJ_INTERACTIVE_DISTANCE;

		public static float MAX_THRUST;

		public const float MAX_PARTICLE_DISTANCE_DEFAULT = 1f;

		public const float MAX_PARTICLE_DISTANCE_EXTENSION = 0.25f;

		static MyConstants()
		{
			FIELD_OF_VIEW_CONFIG_MIN = MathHelper.ToRadians(40f);
			FIELD_OF_VIEW_CONFIG_MAX = MathHelper.ToRadians(120f);
			FIELD_OF_VIEW_CONFIG_MAX_SAFE = MathHelper.ToRadians(120f);
			FIELD_OF_VIEW_CONFIG_DEFAULT = MathHelper.ToRadians(70f);
			FIELD_OF_VIEW_CONFIG_MAX_DUAL_HEAD = MathHelper.ToRadians(80f);
			FIELD_OF_VIEW_CONFIG_MAX_TRIPLE_HEAD = MathHelper.ToRadians(70f);
			GAME_PRUNING_STRUCTURE_AABB_EXTENSION = new Vector3D(3.0);
			DEFAULT_INTERACTIVE_DISTANCE = 5f;
			DEFAULT_GROUND_SEARCH_DISTANCE = 2f;
			FLOATING_OBJ_INTERACTIVE_DISTANCE = 3f;
			MAX_THRUST = 1.5f;
		}
	}
}
