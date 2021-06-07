using VRage.GameServices;

namespace VRage.Mod.Io
{
	public static class MyModIoService
	{
		public static IMyUGCService Create(bool isDedicated, IMyGameService service, string gameId, string gameName, string apiKey)
		{
			return new MyModIoServiceInternal(isDedicated, service, gameId, gameName, apiKey);
		}
	}
}
