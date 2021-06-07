using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_CubeBuilderDefinition), null)]
	public class MyCubeBuilderDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MyCubeBuilderDefinition_003C_003EActor : IActivator, IActivator<MyCubeBuilderDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCubeBuilderDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCubeBuilderDefinition CreateInstance()
			{
				return new MyCubeBuilderDefinition();
			}

			MyCubeBuilderDefinition IActivator<MyCubeBuilderDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float DefaultBlockBuildingDistance;

		public float MaxBlockBuildingDistance;

		public float MinBlockBuildingDistance;

		public double BuildingDistSmallSurvivalCharacter;

		public double BuildingDistLargeSurvivalCharacter;

		public double BuildingDistSmallSurvivalShip;

		public double BuildingDistLargeSurvivalShip;

		/// <summary>
		/// Defines settings for building mode.
		/// </summary>
		public MyPlacementSettings BuildingSettings;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CubeBuilderDefinition myObjectBuilder_CubeBuilderDefinition = (MyObjectBuilder_CubeBuilderDefinition)builder;
			DefaultBlockBuildingDistance = myObjectBuilder_CubeBuilderDefinition.DefaultBlockBuildingDistance;
			MaxBlockBuildingDistance = myObjectBuilder_CubeBuilderDefinition.MaxBlockBuildingDistance;
			MinBlockBuildingDistance = myObjectBuilder_CubeBuilderDefinition.MinBlockBuildingDistance;
			BuildingDistSmallSurvivalCharacter = myObjectBuilder_CubeBuilderDefinition.BuildingDistSmallSurvivalCharacter;
			BuildingDistLargeSurvivalCharacter = myObjectBuilder_CubeBuilderDefinition.BuildingDistLargeSurvivalCharacter;
			BuildingDistSmallSurvivalShip = myObjectBuilder_CubeBuilderDefinition.BuildingDistSmallSurvivalShip;
			BuildingDistLargeSurvivalShip = myObjectBuilder_CubeBuilderDefinition.BuildingDistLargeSurvivalShip;
			BuildingSettings = myObjectBuilder_CubeBuilderDefinition.BuildingSettings;
		}
	}
}
