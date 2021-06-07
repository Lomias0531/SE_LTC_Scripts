using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Game.Entities.Character
{
	public class MyCharacterHitInfo
	{
		public int CapsuleIndex
		{
			get;
			set;
		}

		public int BoneIndex
		{
			get;
			set;
		}

		public CapsuleD Capsule
		{
			get;
			set;
		}

		public Vector3 HitNormalBindingPose
		{
			get;
			set;
		}

		public Vector3 HitPositionBindingPose
		{
			get;
			set;
		}

		public Matrix BindingTransformation
		{
			get;
			set;
		}

		public MyIntersectionResultLineTriangleEx Triangle
		{
			get;
			set;
		}

		public bool HitHead
		{
			get;
			set;
		}

		public MyCharacterHitInfo()
		{
			CapsuleIndex = -1;
		}

		public void Reset()
		{
			CapsuleIndex = -1;
			BoneIndex = -1;
			Capsule = default(CapsuleD);
			HitNormalBindingPose = default(Vector3);
			HitPositionBindingPose = default(Vector3);
			BindingTransformation = default(Matrix);
			Triangle = default(MyIntersectionResultLineTriangleEx);
			HitHead = false;
		}
	}
}
