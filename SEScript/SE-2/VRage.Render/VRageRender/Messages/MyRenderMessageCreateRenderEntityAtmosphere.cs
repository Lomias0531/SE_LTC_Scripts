using VRageMath;
using VRageRender.Import;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateRenderEntityAtmosphere : MyRenderMessageBase
	{
		public uint ID;

		public string DebugName;

		public string Model;

		public MatrixD WorldMatrix;

		public MyMeshDrawTechnique Technique;

		public RenderFlags Flags;

		public CullingOptions CullingOptions;

		public float MaxViewDistance;

		public float AtmosphereRadius;

		public float PlanetRadius;

		public Vector3 AtmosphereWavelengths;

		public bool FadeIn;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateRenderEntityAtmosphere;

		public override string ToString()
		{
			return DebugName ?? (string.Empty + ", " + Model);
		}
	}
}
