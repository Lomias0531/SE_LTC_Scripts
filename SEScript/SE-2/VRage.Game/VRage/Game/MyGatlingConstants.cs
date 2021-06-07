using System;
using VRageMath;

namespace VRage.Game
{
	public static class MyGatlingConstants
	{
		public const float ROTATION_SPEED_PER_SECOND = MathF.PI * 4f;

		public const int ROTATION_TIMEOUT = 2000;

		public const int SHOT_INTERVAL_IN_MILISECONDS = 95;

		public const int REAL_SHOTS_PER_SECOND = 45;

		public const int MIN_TIME_RELEASE_INTERVAL_IN_MILISECONDS = 204;

		public static readonly float SHOT_PROJECTILE_DEBRIS_MAX_DEVIATION_ANGLE = MathHelper.ToRadians(30f);

		public static readonly float COCKPIT_GLASS_PROJECTILE_DEBRIS_MAX_DEVIATION_ANGLE = MathHelper.ToRadians(10f);

		public const int SMOKE_INCREASE_PER_SHOT = 19;

		public const int SMOKE_DECREASE = 1;

		public const int SMOKES_MAX = 50;

		public const int SMOKES_MIN = 40;

		public const int SMOKES_INTERVAL_IN_MILISECONDS = 10;
	}
}
