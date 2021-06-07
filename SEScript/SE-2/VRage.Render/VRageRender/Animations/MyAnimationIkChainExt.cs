using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Tiny structure describing IK chain + remembering last state.
	/// </summary>
	public class MyAnimationIkChainExt : MyAnimationIkChain
	{
		public float LastTerrainHeight;

		public Vector3 LastTerrainNormal = Vector3.Up;

		public Vector3 LastPoleVector = Vector3.Left;

		public Matrix LastAligningRotationMatrix = Matrix.Identity;

		public float AligningSmoothness = 0.2f;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public MyAnimationIkChainExt()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		public MyAnimationIkChainExt(MyAnimationIkChain initFromChain)
		{
			BoneIndex = initFromChain.BoneIndex;
			BoneName = initFromChain.BoneName;
			ChainLength = initFromChain.ChainLength;
			AlignBoneWithTerrain = initFromChain.AlignBoneWithTerrain;
			EndBoneTransform = initFromChain.EndBoneTransform;
			MinEndPointRotation = initFromChain.MinEndPointRotation;
			MaxEndPointRotation = initFromChain.MaxEndPointRotation;
		}
	}
}
