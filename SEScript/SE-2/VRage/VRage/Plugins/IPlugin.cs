using System;

namespace VRage.Plugins
{
	public interface IPlugin : IDisposable
	{
		void Init(object gameInstance);

		void Update();
	}
}
