using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawTriangles : MyDebugRenderMessage, IDrawTrianglesMessage
	{
		public Color Color;

		public MatrixD WorldMatrix;

		public bool DepthRead;

		public bool Shaded;

		public bool Edges;

		public List<int> Indices = new List<int>();

		public List<MyFormatPositionColor> Vertices = new List<MyFormatPositionColor>();

		public int VertexCount => Vertices.Count;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawTriangles;

		public void AddIndex(int index)
		{
			Indices.Add(index);
		}

		public void AddVertex(Vector3D position)
		{
			Vertices.Add(new MyFormatPositionColor(position, Color));
		}

		public void AddVertex(Vector3D position, Color color)
		{
			Vertices.Add(new MyFormatPositionColor(position, color));
		}

		public void AddTriangle(ref Vector3D v0, ref Vector3D v1, ref Vector3D v2)
		{
			int count = Vertices.Count;
			Indices.Add(count);
			Indices.Add(count + 1);
			Indices.Add(count + 2);
			Vertices.Add(new MyFormatPositionColor(v0, Color));
			Vertices.Add(new MyFormatPositionColor(v1, Color));
			Vertices.Add(new MyFormatPositionColor(v2, Color));
		}

		public void AddTriangle(Vector3D v0, Vector3D v1, Vector3D v2)
		{
			int count = Vertices.Count;
			Indices.Add(count);
			Indices.Add(count + 1);
			Indices.Add(count + 2);
			Vertices.Add(new MyFormatPositionColor(v0, Color));
			Vertices.Add(new MyFormatPositionColor(v1, Color));
			Vertices.Add(new MyFormatPositionColor(v2, Color));
		}

		public void AddTriangle(Vector3D v0, Color c0, Vector3D v1, Color c1, Vector3D v2, Color c2)
		{
			int count = Vertices.Count;
			Indices.Add(count);
			Indices.Add(count + 1);
			Indices.Add(count + 2);
			Vertices.Add(new MyFormatPositionColor(v0, c0));
			Vertices.Add(new MyFormatPositionColor(v1, c1));
			Vertices.Add(new MyFormatPositionColor(v2, c2));
		}
	}
}
