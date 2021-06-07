using System.Collections.Generic;

namespace VRageRender.Animations
{
	/// <summary>
	/// Helper structure passed as an parameter during computation of current pose.
	/// </summary>
	public struct MyAnimationUpdateData
	{
		public double DeltaTimeInSeconds;

		public MyAnimationController Controller;

		public MyCharacterBone[] CharacterBones;

		public bool[] LayerBoneMask;

		public List<MyAnimationClip.BoneState> BonesResult;

		public int[] VisitedTreeNodesPath;

		public int VisitedTreeNodesCounter;

		public void AddVisitedTreeNodesPathPoint(int nextPoint)
		{
			if (VisitedTreeNodesPath != null && VisitedTreeNodesCounter < VisitedTreeNodesPath.Length)
			{
				VisitedTreeNodesPath[VisitedTreeNodesCounter] = nextPoint;
				VisitedTreeNodesCounter++;
			}
		}
	}
}
