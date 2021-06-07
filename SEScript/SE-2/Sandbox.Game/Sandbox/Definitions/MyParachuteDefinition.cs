using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ParachuteDefinition), null)]
	public class MyParachuteDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyParachuteDefinition_003C_003EActor : IActivator, IActivator<MyParachuteDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyParachuteDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParachuteDefinition CreateInstance()
			{
				return new MyParachuteDefinition();
			}

			MyParachuteDefinition IActivator<MyParachuteDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float PowerConsumptionIdle;

		public float PowerConsumptionMoving;

		public MyObjectBuilder_ParachuteDefinition.SubpartDefinition[] Subparts;

		public MyObjectBuilder_ParachuteDefinition.Opening[] OpeningSequence;

		public string ParachuteSubpartName;

		public float DragCoefficient;

		public int MaterialDeployCost;

		public MyDefinitionId MaterialDefinitionId;

		public float ReefAtmosphereLevel;

		public float MinimumAtmosphereLevel;

		public float RadiusMultiplier;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ParachuteDefinition myObjectBuilder_ParachuteDefinition = builder as MyObjectBuilder_ParachuteDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ParachuteDefinition.ResourceSinkGroup);
			PowerConsumptionIdle = myObjectBuilder_ParachuteDefinition.PowerConsumptionIdle;
			PowerConsumptionMoving = myObjectBuilder_ParachuteDefinition.PowerConsumptionMoving;
			Subparts = myObjectBuilder_ParachuteDefinition.Subparts;
			OpeningSequence = myObjectBuilder_ParachuteDefinition.OpeningSequence;
			ParachuteSubpartName = myObjectBuilder_ParachuteDefinition.ParachuteSubpartName;
			DragCoefficient = myObjectBuilder_ParachuteDefinition.DragCoefficient;
			MaterialDeployCost = myObjectBuilder_ParachuteDefinition.MaterialDeployCost;
			ReefAtmosphereLevel = myObjectBuilder_ParachuteDefinition.ReefAtmosphereLevel;
			MinimumAtmosphereLevel = myObjectBuilder_ParachuteDefinition.MinimumAtmosphereLevel;
			RadiusMultiplier = myObjectBuilder_ParachuteDefinition.RadiusMultiplier;
			MaterialDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_Component), myObjectBuilder_ParachuteDefinition.MaterialSubtype);
		}
	}
}
