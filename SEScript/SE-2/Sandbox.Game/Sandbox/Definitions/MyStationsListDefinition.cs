using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_StationsListDefinition), null)]
	public class MyStationsListDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyStationsListDefinition_003C_003EActor : IActivator, IActivator<MyStationsListDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyStationsListDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyStationsListDefinition CreateInstance()
			{
				return new MyStationsListDefinition();
			}

			MyStationsListDefinition IActivator<MyStationsListDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyStringId> StationNames
		{
			get;
			set;
		}

		public int SpawnDistance
		{
			get;
			set;
		}

		public MyDefinitionId? GeneratedItemsContainerType
		{
			get;
			set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_StationsListDefinition myObjectBuilder_StationsListDefinition = (MyObjectBuilder_StationsListDefinition)builder;
			if (myObjectBuilder_StationsListDefinition.StationNames != null)
			{
				StationNames = new List<MyStringId>(myObjectBuilder_StationsListDefinition.StationNames.Count);
				foreach (string stationName in myObjectBuilder_StationsListDefinition.StationNames)
				{
					StationNames.Add(MyStringId.GetOrCompute(stationName));
				}
			}
			else
			{
				StationNames = new List<MyStringId>();
			}
			SpawnDistance = myObjectBuilder_StationsListDefinition.SpawnDistance;
			GeneratedItemsContainerType = myObjectBuilder_StationsListDefinition.GeneratedItemsContainerType;
		}
	}
}
