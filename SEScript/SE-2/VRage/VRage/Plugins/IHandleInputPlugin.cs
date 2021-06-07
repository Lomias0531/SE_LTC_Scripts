using System;

namespace VRage.Plugins
{
	public interface IHandleInputPlugin : IPlugin, IDisposable
	{
		void HandleInput();
	}
}
