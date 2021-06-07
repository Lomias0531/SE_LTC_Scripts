using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ToolItemDefinition), null)]
	public class MyToolItemDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyToolItemDefinition_003C_003EActor : IActivator, IActivator<MyToolItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyToolItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyToolItemDefinition CreateInstance()
			{
				return new MyToolItemDefinition();
			}

			MyToolItemDefinition IActivator<MyToolItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyVoxelMiningDefinition[] VoxelMinings;

		public List<MyToolActionDefinition> PrimaryActions = new List<MyToolActionDefinition>();

		public List<MyToolActionDefinition> SecondaryActions = new List<MyToolActionDefinition>();

		public float HitDistance;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ToolItemDefinition myObjectBuilder_ToolItemDefinition = builder as MyObjectBuilder_ToolItemDefinition;
			if (myObjectBuilder_ToolItemDefinition.VoxelMinings != null && myObjectBuilder_ToolItemDefinition.VoxelMinings.Length != 0)
			{
				VoxelMinings = new MyVoxelMiningDefinition[myObjectBuilder_ToolItemDefinition.VoxelMinings.Length];
				for (int i = 0; i < myObjectBuilder_ToolItemDefinition.VoxelMinings.Length; i++)
				{
					VoxelMinings[i].MinedOre = myObjectBuilder_ToolItemDefinition.VoxelMinings[i].MinedOre;
					VoxelMinings[i].HitCount = myObjectBuilder_ToolItemDefinition.VoxelMinings[i].HitCount;
					VoxelMinings[i].PhysicalItemId = myObjectBuilder_ToolItemDefinition.VoxelMinings[i].PhysicalItemId;
					VoxelMinings[i].RemovedRadius = myObjectBuilder_ToolItemDefinition.VoxelMinings[i].RemovedRadius;
					VoxelMinings[i].OnlyApplyMaterial = myObjectBuilder_ToolItemDefinition.VoxelMinings[i].OnlyApplyMaterial;
				}
			}
			CopyActions(myObjectBuilder_ToolItemDefinition.PrimaryActions, PrimaryActions);
			CopyActions(myObjectBuilder_ToolItemDefinition.SecondaryActions, SecondaryActions);
			HitDistance = myObjectBuilder_ToolItemDefinition.HitDistance;
		}

		private void CopyActions(MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] sourceActions, List<MyToolActionDefinition> targetList)
		{
			if (sourceActions == null || sourceActions.Length == 0)
			{
				return;
			}
			for (int i = 0; i < sourceActions.Length; i++)
			{
				MyToolActionDefinition item = default(MyToolActionDefinition);
				item.Name = MyStringId.GetOrCompute(sourceActions[i].Name);
				item.StartTime = sourceActions[i].StartTime;
				item.EndTime = sourceActions[i].EndTime;
				item.Efficiency = sourceActions[i].Efficiency;
				item.StatsEfficiency = sourceActions[i].StatsEfficiency;
				item.SwingSound = sourceActions[i].SwingSound;
				item.SwingSoundStart = sourceActions[i].SwingSoundStart;
				item.HitStart = sourceActions[i].HitStart;
				item.HitDuration = sourceActions[i].HitDuration;
				item.HitSound = sourceActions[i].HitSound;
				item.CustomShapeRadius = sourceActions[i].CustomShapeRadius;
				item.Crosshair = sourceActions[i].Crosshair;
				if (sourceActions[i].HitConditions != null)
				{
					item.HitConditions = new MyToolHitCondition[sourceActions[i].HitConditions.Length];
					for (int j = 0; j < item.HitConditions.Length; j++)
					{
						item.HitConditions[j].EntityType = sourceActions[i].HitConditions[j].EntityType;
						item.HitConditions[j].Animation = sourceActions[i].HitConditions[j].Animation;
						item.HitConditions[j].AnimationTimeScale = sourceActions[i].HitConditions[j].AnimationTimeScale;
						item.HitConditions[j].StatsAction = sourceActions[i].HitConditions[j].StatsAction;
						item.HitConditions[j].StatsActionIfHit = sourceActions[i].HitConditions[j].StatsActionIfHit;
						item.HitConditions[j].StatsModifier = sourceActions[i].HitConditions[j].StatsModifier;
						item.HitConditions[j].StatsModifierIfHit = sourceActions[i].HitConditions[j].StatsModifierIfHit;
						item.HitConditions[j].Component = sourceActions[i].HitConditions[j].Component;
					}
				}
				targetList.Add(item);
			}
		}
	}
}
