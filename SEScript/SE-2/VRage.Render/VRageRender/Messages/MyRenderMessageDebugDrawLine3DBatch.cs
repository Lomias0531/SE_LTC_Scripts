using System;
using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawLine3DBatch : MyDebugRenderMessage, IDisposable
	{
		public MatrixD WorldMatrix;

		public List<MyFormatPositionColor> Lines = new List<MyFormatPositionColor>();

		public bool DepthRead;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawLine3DBatch;

		public void AddLine(Vector3D pointFrom, Color colorFrom, Vector3D pointTo, Color colorTo)
		{
			List<MyFormatPositionColor> lines = Lines;
			MyFormatPositionColor item = new MyFormatPositionColor
			{
				Color = colorFrom,
				Position = pointFrom
			};
			lines.Add(item);
			List<MyFormatPositionColor> lines2 = Lines;
			item = new MyFormatPositionColor
			{
				Color = colorTo,
				Position = pointTo
			};
			lines2.Add(item);
		}

		public new void Dispose()
		{
			MyRenderProxy.DebugDrawLine3DSubmitBatch(this);
		}

		public override void Close()
		{
			Lines.Clear();
		}
	}
}
