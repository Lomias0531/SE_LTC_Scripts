using Sandbox.Game.Entities;
using System;
using VRage.Library;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyCubemapData<TPixel> : IMyWrappedCubemapFace, IDisposable where TPixel : unmanaged
	{
		private static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("CubemapDataBuffers"));

		public unsafe TPixel* Data;

		private NativeArray m_dataBuffer;

		private readonly int m_realResolution;

		public int Resolution
		{
			get;
			set;
		}

		public int ResolutionMinusOne
		{
			get;
			set;
		}

		public int RowStride => m_realResolution;

		public unsafe void SetMaterial(int x, int y, TPixel value)
		{
			Data[(y + 1) * m_realResolution + (x + 1)] = value;
		}

		public unsafe void SetValue(int x, int y, ref TPixel value)
		{
			int num = (y + 1) * m_realResolution + (x + 1);
			Data[num] = value;
		}

		public unsafe void GetValue(int x, int y, out TPixel value)
		{
			value = Data[(y + 1) * m_realResolution + (x + 1)];
		}

		public unsafe TPixel GetValue(float x, float y)
		{
			int num = (int)((float)Resolution * x);
			int num2 = (int)((float)Resolution * y);
			return Data[(num2 + 1) * m_realResolution + (num + 1)];
		}

		public unsafe MyCubemapData(int resolution)
		{
			m_realResolution = resolution + 2;
			Resolution = resolution;
			ResolutionMinusOne = resolution - 1;
			m_dataBuffer = BufferAllocator.Allocate(m_realResolution * m_realResolution * sizeof(TPixel));
			Data = (TPixel*)(void*)m_dataBuffer.Ptr;
		}

		public int GetRowStart(int y)
		{
			return (y + 1) * m_realResolution + 1;
		}

		public void CopyRange(Vector2I start, Vector2I end, MyCubemapData<TPixel> other, Vector2I oStart, Vector2I oEnd)
		{
			Vector2I step = MyCubemapHelpers.GetStep(ref start, ref end);
			Vector2I step2 = MyCubemapHelpers.GetStep(ref oStart, ref oEnd);
			TPixel value;
			while (start != end)
			{
				other.GetValue(oStart.X, oStart.Y, out value);
				SetValue(start.X, start.Y, ref value);
				start += step;
				oStart += step2;
			}
			other.GetValue(oStart.X, oStart.Y, out value);
			SetValue(start.X, start.Y, ref value);
		}

		public void CopyRange(Vector2I start, Vector2I end, IMyWrappedCubemapFace other, Vector2I oStart, Vector2I oEnd)
		{
			MyCubemapData<TPixel> myCubemapData = other as MyCubemapData<TPixel>;
			if (myCubemapData != null)
			{
				CopyRange(start, end, myCubemapData, oStart, oEnd);
			}
		}

		public void FinishFace(string name)
		{
			TPixel pixel = default(TPixel);
			SetPixel(-1, -1, ref pixel);
			SetPixel(Resolution, -1, ref pixel);
			SetPixel(-1, Resolution, ref pixel);
			SetPixel(Resolution, Resolution, ref pixel);
		}

		internal unsafe void SetPixel(int y, int x, ref TPixel pixel)
		{
			Data[(y + 1) * m_realResolution + (x + 1)] = pixel;
		}

		public unsafe void Dispose()
		{
			m_dataBuffer.Dispose();
			m_dataBuffer = null;
			Data = null;
		}
	}
}
