using VRageMath;

namespace VRage.Render.Image
{
	public interface IMyImage
	{
		Vector2I Size
		{
			get;
		}

		int Stride
		{
			get;
		}

		int BitsPerPixel
		{
			get;
		}
	}
	public interface IMyImage<TData> : IMyImage where TData : unmanaged
	{
		TData[] Data
		{
			get;
		}
	}
}
