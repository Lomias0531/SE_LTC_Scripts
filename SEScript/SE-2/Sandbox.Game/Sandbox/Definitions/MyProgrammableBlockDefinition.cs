using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProgrammableBlockDefinition), null)]
	public class MyProgrammableBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyProgrammableBlockDefinition_003C_003EActor : IActivator, IActivator<MyProgrammableBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProgrammableBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProgrammableBlockDefinition CreateInstance()
			{
				return new MyProgrammableBlockDefinition();
			}

			MyProgrammableBlockDefinition IActivator<MyProgrammableBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProgrammableBlockDefinition myObjectBuilder_ProgrammableBlockDefinition = (MyObjectBuilder_ProgrammableBlockDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ProgrammableBlockDefinition.ResourceSinkGroup);
			ScreenAreas = ((myObjectBuilder_ProgrammableBlockDefinition.ScreenAreas != null) ? myObjectBuilder_ProgrammableBlockDefinition.ScreenAreas.ToList() : null);
		}
	}
}
