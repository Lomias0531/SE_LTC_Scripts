namespace Sandbox.Game.Entities
{
	public static class MyGunBaseUserExtension
	{
		public static bool PutConstraint(this IMyGunBaseUser obj)
		{
			return !string.IsNullOrEmpty(obj.ConstraintDisplayName);
		}
	}
}
