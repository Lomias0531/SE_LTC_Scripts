using System.Collections.Generic;
using VRage;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateRenderCubeInstanceBuffer : MyRenderMessageBase
	{
		public uint ID;

		public List<MyCubeInstanceData> InstanceData = new List<MyCubeInstanceData>();

		public List<MyCubeInstanceDecalData> DecalsData = new List<MyCubeInstanceDecalData>();

		public int Capacity;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateRenderCubeInstanceBuffer;

		public override void Init()
		{
			DecalsData.Clear();
		}

		public override void Close()
		{
			InstanceData.Clear();
			base.Close();
		}
	}
}
