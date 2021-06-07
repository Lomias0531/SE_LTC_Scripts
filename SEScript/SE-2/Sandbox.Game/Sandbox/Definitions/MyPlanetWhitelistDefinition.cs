using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PlanetWhitelistDefinition), null)]
	public class MyPlanetWhitelistDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPlanetWhitelistDefinition_003C_003EActor : IActivator, IActivator<MyPlanetWhitelistDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetWhitelistDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetWhitelistDefinition CreateInstance()
			{
				return new MyPlanetWhitelistDefinition();
			}

			MyPlanetWhitelistDefinition IActivator<MyPlanetWhitelistDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public HashSetReader<MyPlanetGeneratorDefinition> WhitelistedPlanets
		{
			get;
			private set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PlanetWhitelistDefinition myObjectBuilder_PlanetWhitelistDefinition = (MyObjectBuilder_PlanetWhitelistDefinition)builder;
			HashSet<MyPlanetGeneratorDefinition> hashSet = new HashSet<MyPlanetGeneratorDefinition>();
			foreach (MyPlanetGeneratorDefinition planetsGeneratorsDefinition in MyDefinitionManager.Static.GetPlanetsGeneratorsDefinitions())
			{
				string @string = planetsGeneratorsDefinition.Id.SubtypeId.String;
				if (myObjectBuilder_PlanetWhitelistDefinition.WhitelistedPlanets.Contains(@string))
				{
					hashSet.Add(planetsGeneratorsDefinition);
				}
			}
			WhitelistedPlanets = hashSet;
		}
	}
}
