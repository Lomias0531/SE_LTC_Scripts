using Sandbox.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyEdgeInfo
	{
		public Vector4 LocalOrthoMatrix;

		private Color m_packedColor;

		public MyStringHash EdgeModel;

		public Base27Directions.Direction PackedNormal0;

		public Base27Directions.Direction PackedNormal1;

		public Color Color
		{
			get
			{
				Color packedColor = m_packedColor;
				packedColor.A = 0;
				return packedColor;
			}
			set
			{
				byte a = m_packedColor.A;
				m_packedColor = value;
				m_packedColor.A = a;
			}
		}

		public MyCubeEdgeType EdgeType
		{
			get
			{
				return (MyCubeEdgeType)m_packedColor.A;
			}
			set
			{
				m_packedColor.A = (byte)value;
			}
		}

		public MyEdgeInfo()
		{
		}

		public MyEdgeInfo(ref Vector3 pos, ref Vector3I edgeDirection, ref Vector3 normal0, ref Vector3 normal1, ref Color color, MyStringHash edgeModel)
		{
			MyEdgeOrientationInfo myEdgeOrientationInfo = MyCubeGridDefinitions.EdgeOrientations[edgeDirection];
			PackedNormal0 = Base27Directions.GetDirection(normal0);
			PackedNormal1 = Base27Directions.GetDirection(normal1);
			m_packedColor = color;
			EdgeType = myEdgeOrientationInfo.EdgeType;
			LocalOrthoMatrix = Vector4.PackOrthoMatrix(pos, myEdgeOrientationInfo.Orientation.Forward, myEdgeOrientationInfo.Orientation.Up);
			EdgeModel = edgeModel;
		}
	}
}
