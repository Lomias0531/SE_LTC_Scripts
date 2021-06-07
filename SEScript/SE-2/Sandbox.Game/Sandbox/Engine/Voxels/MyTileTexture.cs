using Sandbox.Game.Entities;
using System;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Library;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyTileTexture<TPixel> : IDisposable where TPixel : unmanaged
	{
		private static NativeArrayAllocator BufferAllocator;

		private int m_stride;

		private Vector2I m_cellSize;

		private unsafe TPixel* m_data;

		private NativeArray m_nativeBuffer;

		private static readonly Vector2B[] s_baseCellCoords;

		private Vector2I[] m_cellCoords = new Vector2I[16];

		public static readonly MyTileTexture<TPixel> Default;

		static MyTileTexture()
		{
			BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("TileTextures"));
			s_baseCellCoords = new Vector2B[16]
			{
				new Vector2B(0, 0),
				new Vector2B(1, 0),
				new Vector2B(2, 0),
				new Vector2B(3, 0),
				new Vector2B(0, 1),
				new Vector2B(1, 1),
				new Vector2B(2, 1),
				new Vector2B(3, 1),
				new Vector2B(0, 2),
				new Vector2B(1, 2),
				new Vector2B(2, 2),
				new Vector2B(3, 2),
				new Vector2B(0, 3),
				new Vector2B(1, 3),
				new Vector2B(2, 3),
				new Vector2B(3, 3)
			};
			Default = new MyTileTexture<TPixel>();
			MyVRage.RegisterExitCallback(delegate
			{
				Default.Dispose();
			});
		}

		private MyTileTexture()
			: this(4, new TPixel[16], 1)
		{
		}

		public MyTileTexture(Vector2I size, int stride, TPixel[] data, int cellSize)
			: this(stride, data, cellSize)
		{
		}

		private unsafe MyTileTexture(int stride, TPixel[] data, int cellSize)
		{
			m_stride = stride;
			m_cellSize = new Vector2I(cellSize);
			m_nativeBuffer = BufferAllocator.Allocate(data.Length * sizeof(TPixel));
			m_data = (TPixel*)(void*)m_nativeBuffer.Ptr;
			fixed (TPixel* source = data)
			{
				Unsafe.CopyBlockUnaligned(m_data, source, (uint)m_nativeBuffer.Size);
			}
			PrepareCellCoords();
		}

		private void PrepareCellCoords()
		{
			for (int i = 0; i < 16; i++)
			{
				m_cellCoords[i] = s_baseCellCoords[i] * m_cellSize.X;
			}
		}

		public unsafe void GetValue(int corners, Vector2I coords, out TPixel value)
		{
			if (corners > 15)
			{
				value = default(TPixel);
				return;
			}
			coords += m_cellCoords[corners];
			value = m_data[coords.X + coords.Y * m_stride];
		}

		public unsafe void GetValue(int corners, Vector2 coords, out TPixel value)
		{
			if (corners > 15)
			{
				value = default(TPixel);
				return;
			}
			Vector2I vector2I = new Vector2I(coords * m_cellSize.X);
			vector2I += m_cellCoords[corners];
			value = m_data[vector2I.X + vector2I.Y * m_stride];
		}

		public unsafe void Dispose()
		{
			BufferAllocator.Dispose(m_nativeBuffer);
			m_nativeBuffer = null;
			m_data = null;
		}
	}
}
