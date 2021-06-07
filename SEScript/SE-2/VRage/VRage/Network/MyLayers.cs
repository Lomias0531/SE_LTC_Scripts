using System;
using System.Collections.Generic;

namespace VRage.Network
{
	public static class MyLayers
	{
		public struct UpdateLayerDesc
		{
			public int Radius;

			public int UpdateInterval;

			public int SendInterval;
		}

		public static readonly List<UpdateLayerDesc> UpdateLayerDescriptors = new List<UpdateLayerDesc>();

		public static void SetSyncDistance(int distance)
		{
			UpdateLayerDescriptors.Clear();
			List<UpdateLayerDesc> updateLayerDescriptors = UpdateLayerDescriptors;
			UpdateLayerDesc item = new UpdateLayerDesc
			{
				Radius = 20,
				UpdateInterval = 60,
				SendInterval = 4
			};
			updateLayerDescriptors.Add(item);
			UpdateLayerDesc updateLayerDesc = UpdateLayerDescriptors[UpdateLayerDescriptors.Count - 1];
			while (updateLayerDesc.Radius < distance)
			{
				List<UpdateLayerDesc> updateLayerDescriptors2 = UpdateLayerDescriptors;
				item = new UpdateLayerDesc
				{
					Radius = Math.Min(updateLayerDesc.Radius * 4, distance),
					UpdateInterval = updateLayerDesc.UpdateInterval * 2,
					SendInterval = updateLayerDesc.SendInterval * 2
				};
				updateLayerDescriptors2.Add(item);
				updateLayerDesc = UpdateLayerDescriptors[UpdateLayerDescriptors.Count - 1];
			}
		}

		public static int GetSyncDistance()
		{
			if (UpdateLayerDescriptors.Count == 0)
			{
				return 0;
			}
			return UpdateLayerDescriptors[UpdateLayerDescriptors.Count - 1].Radius;
		}
	}
}
