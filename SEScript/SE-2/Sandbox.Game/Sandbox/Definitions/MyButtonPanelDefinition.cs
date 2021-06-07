using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ButtonPanelDefinition), null)]
	public class MyButtonPanelDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyButtonPanelDefinition_003C_003EActor : IActivator, IActivator<MyButtonPanelDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyButtonPanelDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyButtonPanelDefinition CreateInstance()
			{
				return new MyButtonPanelDefinition();
			}

			MyButtonPanelDefinition IActivator<MyButtonPanelDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public int ButtonCount;

		public string[] ButtonSymbols;

		public Vector4[] ButtonColors;

		public Vector4 UnassignedButtonColor;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ButtonPanelDefinition myObjectBuilder_ButtonPanelDefinition = builder as MyObjectBuilder_ButtonPanelDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ButtonPanelDefinition.ResourceSinkGroup);
			ButtonCount = myObjectBuilder_ButtonPanelDefinition.ButtonCount;
			ButtonSymbols = myObjectBuilder_ButtonPanelDefinition.ButtonSymbols;
			ButtonColors = myObjectBuilder_ButtonPanelDefinition.ButtonColors;
			UnassignedButtonColor = myObjectBuilder_ButtonPanelDefinition.UnassignedButtonColor;
		}
	}
}
