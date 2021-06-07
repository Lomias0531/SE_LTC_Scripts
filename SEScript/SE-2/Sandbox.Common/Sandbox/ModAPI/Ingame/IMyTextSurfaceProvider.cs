namespace Sandbox.ModAPI.Ingame
{
	public interface IMyTextSurfaceProvider
	{
		int SurfaceCount
		{
			get;
		}

		IMyTextSurface GetSurface(int index);
	}
}
