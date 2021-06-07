using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlueprintDefinition), null)]
	public class MyBlueprintDefinition : MyBlueprintDefinitionBase
	{
		private class Sandbox_Definitions_MyBlueprintDefinition_003C_003EActor : IActivator, IActivator<MyBlueprintDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlueprintDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlueprintDefinition CreateInstance()
			{
				return new MyBlueprintDefinition();
			}

			MyBlueprintDefinition IActivator<MyBlueprintDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_BlueprintDefinition myObjectBuilder_BlueprintDefinition = (MyObjectBuilder_BlueprintDefinition)ob;
			Prerequisites = new Item[myObjectBuilder_BlueprintDefinition.Prerequisites.Length];
			for (int i = 0; i < Prerequisites.Length; i++)
			{
				Prerequisites[i] = Item.FromObjectBuilder(myObjectBuilder_BlueprintDefinition.Prerequisites[i]);
			}
			if (myObjectBuilder_BlueprintDefinition.Result != null)
			{
				Results = new Item[1];
				Results[0] = Item.FromObjectBuilder(myObjectBuilder_BlueprintDefinition.Result);
			}
			else
			{
				Results = new Item[myObjectBuilder_BlueprintDefinition.Results.Length];
				for (int j = 0; j < Results.Length; j++)
				{
					Results[j] = Item.FromObjectBuilder(myObjectBuilder_BlueprintDefinition.Results[j]);
				}
			}
			BaseProductionTimeInSeconds = myObjectBuilder_BlueprintDefinition.BaseProductionTimeInSeconds;
			base.PostprocessNeeded = true;
			ProgressBarSoundCue = myObjectBuilder_BlueprintDefinition.ProgressBarSoundCue;
			IsPrimary = myObjectBuilder_BlueprintDefinition.IsPrimary;
			Priority = myObjectBuilder_BlueprintDefinition.Priority;
		}

		public override void Postprocess()
		{
			bool atomic = false;
			float num = 0f;
			Item[] results = Results;
			for (int i = 0; i < results.Length; i++)
			{
				Item item = results[i];
				if (item.Id.TypeId != typeof(MyObjectBuilder_Ore) && item.Id.TypeId != typeof(MyObjectBuilder_Ingot))
				{
					atomic = true;
				}
				MyDefinitionManager.Static.TryGetPhysicalItemDefinition(item.Id, out MyPhysicalItemDefinition definition);
				if (definition == null)
				{
					return;
				}
				num += (float)item.Amount * definition.Volume;
			}
			Atomic = atomic;
			OutputVolume = num;
			base.PostprocessNeeded = false;
		}

		public override int GetBlueprints(List<ProductionInfo> blueprints)
		{
			blueprints.Add(new ProductionInfo
			{
				Blueprint = this,
				Amount = 1
			});
			return 1;
		}
	}
}
