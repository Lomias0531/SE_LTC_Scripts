using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.CoordinateSystem
{
	public class MyLocalCoordSys
	{
		private static readonly MyStringId ID_SQUARE = MyStringId.GetOrCompute("Square");

		private const float COLOR_ALPHA = 0.4f;

		private const int LOCAL_COORD_SIZE = 1000;

		private const float BBOX_BORDER_THICKNESS_MODIF = 0.0015f;

		private MyTransformD m_origin;

		private MyOrientedBoundingBoxD m_boundingBox;

		private Vector3D[] m_corners = new Vector3D[8];

		internal Color DebugColor;

		public MyTransformD Origin => m_origin;

		public long EntityCounter
		{
			get;
			set;
		}

		internal MyOrientedBoundingBoxD BoundingBox => m_boundingBox;

		public Color RenderColor
		{
			get;
			set;
		}

		public long Id
		{
			get;
			set;
		}

		public MyLocalCoordSys(int size = 1000)
		{
			m_origin = new MyTransformD(MatrixD.Identity);
			float num = (float)size / 2f;
			Vector3 vector = new Vector3(num, num, num);
			m_boundingBox = new MyOrientedBoundingBoxD(new BoundingBoxD(-vector, vector), m_origin.TransformMatrix);
			m_boundingBox.GetCorners(m_corners, 0);
			RenderColor = GenerateRandomColor();
			DebugColor = GenerateDebugColor(RenderColor);
		}

		public MyLocalCoordSys(MyTransformD origin, int size = 1000)
		{
			m_origin = origin;
			Vector3 vector = new Vector3(size / 2, size / 2, size / 2);
			m_boundingBox = new MyOrientedBoundingBoxD(new BoundingBoxD(-vector, vector), m_origin.TransformMatrix);
			m_boundingBox.GetCorners(m_corners, 0);
			RenderColor = GenerateRandomColor();
			DebugColor = GenerateDebugColor(RenderColor);
		}

		private Color GenerateRandomColor()
		{
			float x = (float)MyRandom.Instance.Next(0, 100) / 100f * 0.4f;
			float y = (float)MyRandom.Instance.Next(0, 100) / 100f * 0.4f;
			float z = (float)MyRandom.Instance.Next(0, 100) / 100f * 0.4f;
			return new Vector4(x, y, z, 0.4f);
		}

		private Color GenerateDebugColor(Color original)
		{
			Vector3 hSV = new Color(original, 1f).ColorToHSV();
			hSV.Y = 0.8f;
			hSV.Z = 0.8f;
			return hSV.HSVtoColor();
		}

		public bool Contains(ref Vector3D vec)
		{
			return m_boundingBox.Contains(ref vec);
		}

		public void Draw()
		{
			MatrixD worldMatrix = Origin.TransformMatrix;
			Vector3D vector3D = Vector3D.One;
			Vector3D value = Vector3D.Zero;
			for (int i = 0; i < 8; i++)
			{
				Vector3D value2 = MySector.MainCamera.WorldToScreen(ref m_corners[i]);
				vector3D = Vector3D.Min(vector3D, value2);
				value = Vector3D.Max(value, value2);
			}
			float lineWidth = 0.0015f / (float)MathHelper.Clamp((value - vector3D).Length(), 0.01, 1.0);
			Color color = MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS ? DebugColor : RenderColor;
			BoundingBoxD localbox = new BoundingBoxD(-m_boundingBox.HalfExtent, m_boundingBox.HalfExtent);
			MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 1, lineWidth, ID_SQUARE, ID_SQUARE);
			if (MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS)
			{
				Vector3D vector3D2 = worldMatrix.Translation - MySector.MainCamera.Position;
				MyRenderProxy.DebugDrawText3D(Origin.Position, $"LCS Id:{Id} Distance:{vector3D2.Length():###.00}m", color, 1f, depthRead: true);
				for (int j = -10; j < 11; j++)
				{
					Vector3D pointFrom = Origin.Position + worldMatrix.Forward * 20.0 + worldMatrix.Right * ((double)j * 2.5);
					Vector3D pointTo = Origin.Position - worldMatrix.Forward * 20.0 + worldMatrix.Right * ((double)j * 2.5);
					MyRenderProxy.DebugDrawLine3D(pointFrom, pointTo, color, color, depthRead: false);
				}
				for (int k = -10; k < 11; k++)
				{
					Vector3D pointFrom2 = Origin.Position + worldMatrix.Right * 20.0 + worldMatrix.Forward * ((double)k * 2.5);
					Vector3D pointTo2 = Origin.Position - worldMatrix.Right * 20.0 + worldMatrix.Forward * ((double)k * 2.5);
					MyRenderProxy.DebugDrawLine3D(pointFrom2, pointTo2, color, color, depthRead: false);
				}
			}
		}
	}
}
