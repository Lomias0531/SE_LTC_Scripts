using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlockBlueprintDefinition), null)]
	public class MyBlockBlueprintDefinition : MyBlueprintDefinition
	{
		private class Sandbox_Definitions_MyBlockBlueprintDefinition_003C_003EActor : IActivator, IActivator<MyBlockBlueprintDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlockBlueprintDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlockBlueprintDefinition CreateInstance()
			{
				return new MyBlockBlueprintDefinition();
			}

			MyBlockBlueprintDefinition IActivator<MyBlockBlueprintDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
		}

		public override void Postprocess()
		{
			Atomic = false;
			float num = 0f;
			Item[] results = Results;
			for (int i = 0; i < results.Length; i++)
			{
				Item item = results[i];
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(item.Id, out MyCubeBlockDefinition blockDefinition);
				if (blockDefinition == null)
				{
					return;
				}
				num += (float)item.Amount * blockDefinition.Mass;
			}
			OutputVolume = num;
			base.PostprocessNeeded = false;
		}
	}
}
