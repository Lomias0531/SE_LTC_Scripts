using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
	public abstract class MyBlueprintDefinitionBase : MyDefinitionBase
	{
		public struct Item
		{
			public MyDefinitionId Id;

			public MyFixedPoint Amount;

			public override string ToString()
			{
				return $"{Amount}x {Id}";
			}

			public static Item FromObjectBuilder(BlueprintItem obItem)
			{
				Item result = default(Item);
				result.Id = obItem.Id;
				result.Amount = MyFixedPoint.DeserializeStringSafe(obItem.Amount);
				return result;
			}
		}

		public struct ProductionInfo
		{
			public MyBlueprintDefinitionBase Blueprint;

			public MyFixedPoint Amount;
		}

		public Item[] Prerequisites;

		public Item[] Results;

		public string ProgressBarSoundCue;

		public float BaseProductionTimeInSeconds = 1f;

		public float OutputVolume;

		public bool Atomic;

		public bool IsPrimary;

		public int Priority;

		public MyObjectBuilderType InputItemType => Prerequisites[0].Id.TypeId;

		public bool PostprocessNeeded
		{
			get;
			protected set;
		}

		public new abstract void Postprocess();

		[Conditional("DEBUG")]
		private void VerifyInputItemType(MyObjectBuilderType inputType)
		{
			Item[] prerequisites = Prerequisites;
			for (int i = 0; i < prerequisites.Length; i++)
			{
				_ = ref prerequisites[i];
			}
		}

		public override string ToString()
		{
			return string.Format("(" + DisplayNameEnum.GetValueOrDefault(MyStringId.NullOrEmpty).String + "){{{0}}}->{{{1}}}", string.Join(" ", Prerequisites), string.Join(" ", Results));
		}

		public abstract int GetBlueprints(List<ProductionInfo> blueprints);
	}
}
