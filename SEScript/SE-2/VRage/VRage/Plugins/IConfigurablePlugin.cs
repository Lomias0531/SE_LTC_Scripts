using System;

namespace VRage.Plugins
{
	public interface IConfigurablePlugin : IPlugin, IDisposable
	{
		string GetPluginTitle();

		IPluginConfiguration GetConfiguration(string userDataPath);
	}
}
