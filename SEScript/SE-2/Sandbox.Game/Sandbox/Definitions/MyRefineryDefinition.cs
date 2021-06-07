using Sandbox.Common.ObjectBuilders.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_RefineryDefinition), null)]
	public class MyRefineryDefinition : MyProductionBlockDefinition
	{
		private class Sandbox_Definitions_MyRefineryDefinition_003C_003EActor : IActivator, IActivator<MyRefineryDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyRefineryDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRefineryDefinition CreateInstance()
			{
				return new MyRefineryDefinition();
			}

			MyRefineryDefinition IActivator<MyRefineryDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float RefineSpeed;

		public float MaterialEfficiency;

		public MyFixedPoint? OreAmountPerPullRequest;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_RefineryDefinition myObjectBuilder_RefineryDefinition = builder as MyObjectBuilder_RefineryDefinition;
			RefineSpeed = myObjectBuilder_RefineryDefinition.RefineSpeed;
			MaterialEfficiency = myObjectBuilder_RefineryDefinition.MaterialEfficiency;
			OreAmountPerPullRequest = myObjectBuilder_RefineryDefinition.OreAmountPerPullRequest;
		}

		protected override bool BlueprintClassCanBeUsed(MyBlueprintClassDefinition blueprintClass)
		{
			foreach (MyBlueprintDefinitionBase item in blueprintClass)
			{
				if (item.Atomic)
				{
					MySandboxGame.Log.WriteLine("Blueprint " + item.DisplayNameText + " is atomic, but it is in a class used by refinery block");
					return false;
				}
			}
			return base.BlueprintClassCanBeUsed(blueprintClass);
		}

		protected override void InitializeLegacyBlueprintClasses(MyObjectBuilder_ProductionBlockDefinition ob)
		{
			ob.BlueprintClasses = new string[1]
			{
				"Ingots"
			};
		}
	}
}
