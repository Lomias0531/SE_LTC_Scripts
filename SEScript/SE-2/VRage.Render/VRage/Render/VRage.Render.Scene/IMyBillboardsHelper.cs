using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Render.Scene
{
	public interface IMyBillboardsHelper
	{
		void AddPointBillboard(MyStringId material, Vector4 color, Vector3D origin, float radius, float angle, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f);

		void AddLineBillboard(MyStringId material, Vector4 color, Vector3D origin, Vector3 directionNormalized, float length, float thickness, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f);

		void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float radiusX, float radiusY, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float softParticleDistanceScale = 1f, int customViewProjection = -1);
	}
}
