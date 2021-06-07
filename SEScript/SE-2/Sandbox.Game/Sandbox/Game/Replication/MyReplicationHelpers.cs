namespace Sandbox.Game.Replication
{
	public static class MyReplicationHelpers
	{
		public static float RampPriority(float priority, int frameCountWithoutSync, float updateOncePer, float rampAmount = 0.5f, bool alsoRampDown = true)
		{
			if ((float)frameCountWithoutSync >= updateOncePer)
			{
				float num = ((float)frameCountWithoutSync - updateOncePer) / updateOncePer;
				if (num > 1f)
				{
					float num2 = (num - 1f) * rampAmount;
					priority *= num2;
				}
				return priority;
			}
			if (!alsoRampDown)
			{
				return priority;
			}
			return 0f;
		}
	}
}
