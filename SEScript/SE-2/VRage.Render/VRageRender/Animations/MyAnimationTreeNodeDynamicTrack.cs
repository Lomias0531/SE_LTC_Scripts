using System;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyAnimationTreeNodeDynamicTrack : MyAnimationTreeNodeTrack
	{
		public struct DynamicTrackData
		{
			public MyAnimationClip Clip;

			public bool Loop;
		}

		public static Func<MyStringId, DynamicTrackData> OnAction;

		public MyStringId DefaultAnimation;

		public override void SetAction(MyStringId action)
		{
			DynamicTrackData dynamicTrackData = default(DynamicTrackData);
			if (OnAction != null)
			{
				dynamicTrackData = OnAction(action);
			}
			if (dynamicTrackData.Clip == null)
			{
				dynamicTrackData = OnAction(DefaultAnimation);
			}
			if (dynamicTrackData.Clip != null)
			{
				SetClip(dynamicTrackData.Clip);
				base.Loop = dynamicTrackData.Loop;
			}
		}
	}
}
