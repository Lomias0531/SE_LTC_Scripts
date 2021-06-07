using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_JukeboxDefinition), null)]
	public class MyJukeboxDefinition : MySoundBlockDefinition
	{
		private class Sandbox_Definitions_MyJukeboxDefinition_003C_003EActor : IActivator, IActivator<MyJukeboxDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyJukeboxDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJukeboxDefinition CreateInstance()
			{
				return new MyJukeboxDefinition();
			}

			MyJukeboxDefinition IActivator<MyJukeboxDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_JukeboxDefinition myObjectBuilder_JukeboxDefinition = (MyObjectBuilder_JukeboxDefinition)builder;
			ScreenAreas = ((myObjectBuilder_JukeboxDefinition.ScreenAreas != null) ? myObjectBuilder_JukeboxDefinition.ScreenAreas.ToList() : null);
		}
	}
}
