using VRage.Library.Extensions;

namespace System.Linq.Expressions
{
	public static class ExpressionExtension
	{
		/// <summary>
		/// The factory used by this class.
		/// </summary>
		public static IActivatorFactory Factory = new ExpressionBaseActivatorFactory();

		public static Func<T> CreateActivator<T>() where T : new()
		{
			return Factory.CreateActivator<T>();
		}

		public static Func<T> CreateActivator<T>(Type t)
		{
			return Factory.CreateActivator<T>(t);
		}
	}
}
