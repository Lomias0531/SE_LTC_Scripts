using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public class MyEnvironmentItemMapping
	{
		public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>[] Samplers;

		public int[] Keys;

		public MyEnvironmentRule Rule;

		public bool Valid => Samplers != null;

		public MyEnvironmentItemMapping(MyRuntimeEnvironmentItemInfo[] map, MyEnvironmentRule rule, MyProceduralEnvironmentDefinition env)
		{
			Rule = rule;
			SortedDictionary<int, List<MyRuntimeEnvironmentItemInfo>> sortedDictionary = new SortedDictionary<int, List<MyRuntimeEnvironmentItemInfo>>();
			foreach (MyRuntimeEnvironmentItemInfo myRuntimeEnvironmentItemInfo in map)
			{
				MyItemTypeDefinition type = myRuntimeEnvironmentItemInfo.Type;
				if (!sortedDictionary.TryGetValue(type.LodFrom + 1, out List<MyRuntimeEnvironmentItemInfo> value))
				{
					value = new List<MyRuntimeEnvironmentItemInfo>();
					sortedDictionary[type.LodFrom + 1] = value;
				}
				value.Add(myRuntimeEnvironmentItemInfo);
			}
			Keys = sortedDictionary.Keys.ToArray();
			List<MyRuntimeEnvironmentItemInfo>[] array = sortedDictionary.Values.ToArray();
			Samplers = new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>[Keys.Length];
			for (int j = 0; j < Keys.Length; j++)
			{
				Samplers[j] = PrepareSampler(array.Range(j, array.Length).SelectMany((List<MyRuntimeEnvironmentItemInfo> x) => x));
			}
		}

		public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> PrepareSampler(IEnumerable<MyRuntimeEnvironmentItemInfo> items)
		{
			float num = 0f;
			foreach (MyRuntimeEnvironmentItemInfo item in items)
			{
				num += item.Density;
			}
			if (num < 1f)
			{
				return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(items.Concat(new MyRuntimeEnvironmentItemInfo[1]), items.Select((MyRuntimeEnvironmentItemInfo x) => x.Density).Concat(new float[1]
				{
					1f - num
				}));
			}
			return new MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>(items, items.Select((MyRuntimeEnvironmentItemInfo x) => x.Density));
		}

		public MyRuntimeEnvironmentItemInfo GetItemRated(int lod, float rate)
		{
			int num = Keys.BinaryIntervalSearch(lod);
			if (num > Samplers.Length)
			{
				return null;
			}
			return Samplers[num].Sample(rate);
		}

		public bool ValidForLod(int lod)
		{
			if (Keys.BinaryIntervalSearch(lod) > Samplers.Length)
			{
				return false;
			}
			return true;
		}

		public MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> Sampler(int lod)
		{
			int num = Keys.BinaryIntervalSearch(lod);
			if (num >= Samplers.Length)
			{
				return null;
			}
			return Samplers[num];
		}
	}
}
