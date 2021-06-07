using System;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyWrappedCubemap<TFaceFormat> : IDisposable where TFaceFormat : IMyWrappedCubemapFace
	{
		protected TFaceFormat[] m_faces;

		protected readonly int m_resolution;

		public string Name
		{
			get;
		}

		public TFaceFormat[] Faces => m_faces;

		public int Resolution => m_resolution;

		public TFaceFormat this[int i] => m_faces[i];

		public TFaceFormat Top => m_faces[4];

		public TFaceFormat Back => m_faces[1];

		public TFaceFormat Left => m_faces[2];

		public TFaceFormat Right => m_faces[3];

		public TFaceFormat Bottom => m_faces[5];

		public TFaceFormat Front => m_faces[0];

		public MyWrappedCubemap(string name, int resolution, TFaceFormat[] faces)
		{
			Name = name;
			m_faces = faces;
			m_resolution = resolution;
			PrepareSides();
		}

		private void PrepareSides()
		{
			int num = m_resolution - 1;
			Front.CopyRange(new Vector2I(0, -1), new Vector2I(num, -1), Top, new Vector2I(0, num), new Vector2I(num, num));
			Front.CopyRange(new Vector2I(0, m_resolution), new Vector2I(num, m_resolution), Bottom, new Vector2I(num, num), new Vector2I(0, num));
			Front.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), Left, new Vector2I(num, 0), new Vector2I(num, num));
			Front.CopyRange(new Vector2I(m_resolution, 0), new Vector2I(m_resolution, num), Right, new Vector2I(0, 0), new Vector2I(0, num));
			Back.CopyRange(new Vector2I(num, -1), new Vector2I(0, -1), Top, new Vector2I(0, 0), new Vector2I(num, 0));
			Back.CopyRange(new Vector2I(num, m_resolution), new Vector2I(0, m_resolution), Bottom, new Vector2I(num, 0), new Vector2I(0, 0));
			Back.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), Right, new Vector2I(num, 0), new Vector2I(num, num));
			Back.CopyRange(new Vector2I(m_resolution, 0), new Vector2I(m_resolution, num), Left, new Vector2I(0, 0), new Vector2I(0, num));
			Left.CopyRange(new Vector2I(num, -1), new Vector2I(0, -1), Top, new Vector2I(0, num), new Vector2I(0, 0));
			Left.CopyRange(new Vector2I(num, m_resolution), new Vector2I(0, m_resolution), Bottom, new Vector2I(num, num), new Vector2I(num, 0));
			Left.CopyRange(new Vector2I(m_resolution, 0), new Vector2I(m_resolution, num), Front, new Vector2I(0, 0), new Vector2I(0, num));
			Left.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), Back, new Vector2I(num, 0), new Vector2I(num, num));
			Right.CopyRange(new Vector2I(num, -1), new Vector2I(0, -1), Top, new Vector2I(num, 0), new Vector2I(num, num));
			Right.CopyRange(new Vector2I(num, m_resolution), new Vector2I(0, m_resolution), Bottom, new Vector2I(0, 0), new Vector2I(0, num));
			Right.CopyRange(new Vector2I(m_resolution, 0), new Vector2I(m_resolution, num), Back, new Vector2I(0, 0), new Vector2I(0, num));
			Right.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), Front, new Vector2I(num, 0), new Vector2I(num, num));
			Top.CopyRange(new Vector2I(0, m_resolution), new Vector2I(num, m_resolution), Front, new Vector2I(0, 0), new Vector2I(num, 0));
			Top.CopyRange(new Vector2I(0, -1), new Vector2I(num, -1), Back, new Vector2I(num, 0), new Vector2I(0, 0));
			Top.CopyRange(new Vector2I(m_resolution, 0), new Vector2I(m_resolution, num), Right, new Vector2I(num, 0), new Vector2I(0, 0));
			Top.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), Left, new Vector2I(0, 0), new Vector2I(num, 0));
			Bottom.CopyRange(new Vector2I(0, m_resolution), new Vector2I(num, m_resolution), Front, new Vector2I(num, num), new Vector2I(0, num));
			Bottom.CopyRange(new Vector2I(0, -1), new Vector2I(num, -1), Back, new Vector2I(0, num), new Vector2I(num, num));
			Bottom.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), Right, new Vector2I(num, num), new Vector2I(0, num));
			Bottom.CopyRange(new Vector2I(m_resolution, 0), new Vector2I(m_resolution, num), Left, new Vector2I(0, num), new Vector2I(num, num));
			for (int i = 0; i < 6; i++)
			{
				m_faces[i].FinishFace(Name + "_" + MyCubemapHelpers.GetNameForFace(i));
			}
		}

		~MyWrappedCubemap()
		{
		}

		public void Dispose()
		{
			TFaceFormat[] faces = m_faces;
			for (int i = 0; i < faces.Length; i++)
			{
				TFaceFormat val = faces[i];
				val.Dispose();
			}
			m_faces = null;
			GC.SuppressFinalize(this);
		}
	}
}
