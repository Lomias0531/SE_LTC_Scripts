using System;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	public class MyPlanetEnvironmentMapping
	{
		public MyMaterialEnvironmentItem[] Items;

		public MyPlanetSurfaceRule Rule;

		private float[] CumulativeIntervals;

		public float TotalFrequency;

		public bool Valid
		{
			get
			{
				if (Items != null)
				{
					return Items.Length != 0;
				}
				return false;
			}
		}

		public MyPlanetEnvironmentMapping(PlanetEnvironmentItemMapping map)
		{
			Rule = map.Rule;
			Items = new MyMaterialEnvironmentItem[map.Items.Length];
			if (Items.Length == 0)
			{
				CumulativeIntervals = null;
				TotalFrequency = 0f;
				return;
			}
			TotalFrequency = 0f;
			for (int i = 0; i < map.Items.Length; i++)
			{
				MyPlanetEnvironmentItemDef myPlanetEnvironmentItemDef = map.Items[i];
				if (myPlanetEnvironmentItemDef.TypeId != null && MyObjectBuilderType.TryParse(myPlanetEnvironmentItemDef.TypeId, out MyObjectBuilderType result))
				{
					if (!typeof(MyObjectBuilder_BotDefinition).IsAssignableFrom(result) && !typeof(MyObjectBuilder_VoxelMapStorageDefinition).IsAssignableFrom(result) && !typeof(MyObjectBuilder_EnvironmentItems).IsAssignableFrom(result))
					{
						MyLog.Default.WriteLine($"Object builder type {myPlanetEnvironmentItemDef.TypeId} is not supported for environment items.");
						Items[i].Frequency = 0f;
					}
					else
					{
						Items[i] = new MyMaterialEnvironmentItem
						{
							Definition = new MyDefinitionId(result, myPlanetEnvironmentItemDef.SubtypeId),
							Frequency = map.Items[i].Density,
							IsDetail = map.Items[i].IsDetail,
							IsBot = typeof(MyObjectBuilder_BotDefinition).IsAssignableFrom(result),
							IsVoxel = typeof(MyObjectBuilder_VoxelMapStorageDefinition).IsAssignableFrom(result),
							IsEnvironemntItem = typeof(MyObjectBuilder_EnvironmentItems).IsAssignableFrom(result),
							BaseColor = map.Items[i].BaseColor,
							ColorSpread = map.Items[i].ColorSpread,
							MaxRoll = (float)Math.Cos(MathHelper.ToDegrees(map.Items[i].MaxRoll)),
							Offset = map.Items[i].Offset,
							GroupId = map.Items[i].GroupId,
							GroupIndex = map.Items[i].GroupIndex,
							ModifierId = map.Items[i].ModifierId,
							ModifierIndex = map.Items[i].ModifierIndex
						};
					}
				}
				else
				{
					MyLog.Default.WriteLine($"Object builder type {myPlanetEnvironmentItemDef.TypeId} does not exist.");
					Items[i].Frequency = 0f;
				}
			}
			ComputeDistribution();
		}

		public void ComputeDistribution()
		{
			if (!Valid)
			{
				TotalFrequency = 0f;
				CumulativeIntervals = null;
				return;
			}
			TotalFrequency = 0f;
			for (int i = 0; i < Items.Length; i++)
			{
				TotalFrequency += Items[i].Frequency;
			}
			CumulativeIntervals = new float[Items.Length - 1];
			float num = 0f;
			for (int j = 0; j < CumulativeIntervals.Length; j++)
			{
				CumulativeIntervals[j] = num + Items[j].Frequency / TotalFrequency;
				num = CumulativeIntervals[j];
			}
		}

		public int GetItemRated(float rate)
		{
			return CumulativeIntervals.BinaryIntervalSearch(rate);
		}
	}
}
