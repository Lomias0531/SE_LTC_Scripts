using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders.Definitions;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TreeDefinition), typeof(Postprocessor))]
	public class MyTreeDefinition : MyEnvironmentItemDefinition
	{
		private class Sandbox_Definitions_MyTreeDefinition_003C_003EActor : IActivator, IActivator<MyTreeDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTreeDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTreeDefinition CreateInstance()
			{
				return new MyTreeDefinition();
			}

			MyTreeDefinition IActivator<MyTreeDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float BranchesStartHeight;

		public float HitPoints;

		public string CutEffect;

		public string FallSound;

		public string BreakSound;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TreeDefinition myObjectBuilder_TreeDefinition = builder as MyObjectBuilder_TreeDefinition;
			BranchesStartHeight = myObjectBuilder_TreeDefinition.BranchesStartHeight;
			HitPoints = myObjectBuilder_TreeDefinition.HitPoints;
			CutEffect = myObjectBuilder_TreeDefinition.CutEffect;
			FallSound = myObjectBuilder_TreeDefinition.FallSound;
			BreakSound = myObjectBuilder_TreeDefinition.BreakSound;
		}
	}
}
