using System;

namespace VRage.Library.Extensions
{
	/// <summary>
	/// Interface describing a factory for activators.
	/// </summary>
	public interface IActivatorFactory
	{
		Func<T> CreateActivator<T>();

		Func<T> CreateActivator<T>(Type subtype);
	}
}
