using Sandbox.Game.Entities;
using System;
using System.Runtime.CompilerServices;
using VRage.Library;

namespace Sandbox.Engine.Voxels
{
	public class MyHeightDetailTexture : IDisposable
	{
		private static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("HeightDetailTexture"));

		private NativeArray m_nativeBuffer;

		public uint Resolution
		{
			get;
		}

		public unsafe byte* Data => (byte*)(void*)m_nativeBuffer.Ptr;

		public unsafe MyHeightDetailTexture(byte[] data, uint resolution)
		{
			Resolution = resolution;
			m_nativeBuffer = BufferAllocator.Allocate(data.Length);
			fixed (byte* source = data)
			{
				Unsafe.CopyBlockUnaligned((void*)m_nativeBuffer.Ptr, source, (uint)data.Length);
			}
		}

		public unsafe float GetValue(float x, float y)
		{
			return (float)(int)Data[(int)(y * (float)Resolution) * Resolution + (int)(x * (float)Resolution)] * 0.003921569f;
		}

		public unsafe byte GetValue(int x, int y)
		{
			return Data[y * Resolution + x];
		}

		public void Dispose()
		{
			BufferAllocator.Dispose(m_nativeBuffer);
			m_nativeBuffer = null;
		}
	}
}
