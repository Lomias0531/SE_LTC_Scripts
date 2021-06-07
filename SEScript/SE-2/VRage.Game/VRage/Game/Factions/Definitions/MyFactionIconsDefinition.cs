using System.Collections.Generic;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions.Factions;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Factions.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FactionIconsDefinition), null)]
	public class MyFactionIconsDefinition : MyDefinitionBase
	{
		private class VRage_Game_Factions_Definitions_MyFactionIconsDefinition_003C_003EActor : IActivator, IActivator<MyFactionIconsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFactionIconsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFactionIconsDefinition CreateInstance()
			{
				return new MyFactionIconsDefinition();
			}

			MyFactionIconsDefinition IActivator<MyFactionIconsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<Vector3> BackgroundColorRanges;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_FactionIconsDefinition myObjectBuilder_FactionIconsDefinition = (MyObjectBuilder_FactionIconsDefinition)builder;
			if (myObjectBuilder_FactionIconsDefinition.BackgroundColorRanges != null)
			{
				BackgroundColorRanges = new List<Vector3>(myObjectBuilder_FactionIconsDefinition.BackgroundColorRanges.Count);
				foreach (SerializableVector3 backgroundColorRange in myObjectBuilder_FactionIconsDefinition.BackgroundColorRanges)
				{
					BackgroundColorRanges.Add(backgroundColorRange);
				}
			}
		}
	}
}
