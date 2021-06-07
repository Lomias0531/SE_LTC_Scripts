using System.Linq;
using System.Reflection;
using VRage.Network;
using VRage.Plugins;

namespace Sandbox.Engine.Multiplayer
{
	public static class MyReplicationLayerExtensions
	{
		public static void RegisterFromGameAssemblies(this MyReplicationLayerBase layer)
		{
			Assembly[] source = new Assembly[5]
			{
				typeof(MySandboxGame).Assembly,
				typeof(MyRenderProfiler).Assembly,
				MyPlugins.GameAssembly,
				MyPlugins.SandboxAssembly,
				MyPlugins.SandboxGameAssembly
			};
			layer.RegisterFromAssembly(source.Where((Assembly s) => s != null).Distinct());
		}
	}
}
