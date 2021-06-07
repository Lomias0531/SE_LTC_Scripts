using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions.GUI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions.GUI
{
	[MyDefinitionType(typeof(MyObjectBuilder_ButtonListStyleDefinition), null)]
	public class MyButtonListStyleDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_GUI_MyButtonListStyleDefinition_003C_003EActor : IActivator, IActivator<MyButtonListStyleDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyButtonListStyleDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyButtonListStyleDefinition CreateInstance()
			{
				return new MyButtonListStyleDefinition();
			}

			MyButtonListStyleDefinition IActivator<MyButtonListStyleDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Vector2 ButtonSize;

		public Vector2 ButtonMargin;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}
	}
}
