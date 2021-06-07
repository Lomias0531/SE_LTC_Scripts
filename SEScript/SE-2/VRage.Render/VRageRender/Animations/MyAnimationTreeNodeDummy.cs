namespace VRageRender.Animations
{
	/// <summary>
	/// Node of animation tree: single track. Contains reference to animation clip.
	/// </summary>
	public class MyAnimationTreeNodeDummy : MyAnimationTreeNode
	{
		private float m_localNormalizedTime;

		public override void Update(ref MyAnimationUpdateData data)
		{
			data.BonesResult = data.Controller.ResultBonesPool.Alloc();
			data.AddVisitedTreeNodesPathPoint(-1);
		}

		public override float GetLocalTimeNormalized()
		{
			return m_localNormalizedTime;
		}

		public override void SetLocalTimeNormalized(float normalizedTime)
		{
			m_localNormalizedTime = normalizedTime;
		}
	}
}
