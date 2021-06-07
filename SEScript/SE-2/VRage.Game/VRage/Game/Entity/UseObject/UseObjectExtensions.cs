namespace VRage.Game.Entity.UseObject
{
	public static class UseObjectExtensions
	{
		public static bool IsActionSupported(this IMyUseObject useObject, UseActionEnum action)
		{
			return (useObject.SupportedActions & action) == action;
		}
	}
}
