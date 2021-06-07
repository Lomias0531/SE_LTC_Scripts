using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageSetCharacterTransforms : MyRenderMessageBase
	{
		public uint CharacterID;

		public Matrix[] BoneAbsoluteTransforms;

		public List<MyBoneDecalUpdate> BoneDecalUpdates;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeEvery;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.SetCharacterTransforms;

		public MyRenderMessageSetCharacterTransforms()
		{
			BoneDecalUpdates = new List<MyBoneDecalUpdate>();
		}

		public override void Init()
		{
			BoneDecalUpdates.Clear();
		}

		public override void Close()
		{
			base.Close();
			CharacterID = uint.MaxValue;
		}
	}
}
