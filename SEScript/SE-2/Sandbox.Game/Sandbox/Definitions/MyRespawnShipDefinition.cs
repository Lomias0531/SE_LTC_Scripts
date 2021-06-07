using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_RespawnShipDefinition), null)]
	public class MyRespawnShipDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyRespawnShipDefinition_003C_003EActor : IActivator, IActivator<MyRespawnShipDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyRespawnShipDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRespawnShipDefinition CreateInstance()
			{
				return new MyRespawnShipDefinition();
			}

			MyRespawnShipDefinition IActivator<MyRespawnShipDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int Cooldown;

		public MyPrefabDefinition Prefab;

		public bool UseForSpace;

		public float MinimalAirDensity;

		public bool UseForPlanetsWithAtmosphere;

		public bool UseForPlanetsWithoutAtmosphere;

		public float PlanetDeployAltitude;

		public Vector3 InitialLinearVelocity;

		public Vector3 InitialAngularVelocity;

		public bool SpawnNearProceduralAsteroids;

		public string[] PlanetTypes;

		public Vector3D? SpawnPosition;

		public float SpawnPositionDispersionMin;

		public float SpawnPositionDispersionMax;

		public bool SpawnWithDefaultItems;

		public string HelpTextLocalizationId;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_RespawnShipDefinition myObjectBuilder_RespawnShipDefinition = (MyObjectBuilder_RespawnShipDefinition)builder;
			Cooldown = myObjectBuilder_RespawnShipDefinition.CooldownSeconds;
			Prefab = MyDefinitionManager.Static.GetPrefabDefinition(myObjectBuilder_RespawnShipDefinition.Prefab);
			UseForSpace = myObjectBuilder_RespawnShipDefinition.UseForSpace;
			MinimalAirDensity = myObjectBuilder_RespawnShipDefinition.MinimalAirDensity;
			SpawnWithDefaultItems = myObjectBuilder_RespawnShipDefinition.SpawnWithDefaultItems;
			InitialLinearVelocity = myObjectBuilder_RespawnShipDefinition.InitialLinearVelocity;
			InitialAngularVelocity = myObjectBuilder_RespawnShipDefinition.InitialAngularVelocity;
			UseForPlanetsWithAtmosphere = myObjectBuilder_RespawnShipDefinition.UseForPlanetsWithAtmosphere;
			UseForPlanetsWithoutAtmosphere = myObjectBuilder_RespawnShipDefinition.UseForPlanetsWithoutAtmosphere;
			PlanetDeployAltitude = (myObjectBuilder_RespawnShipDefinition.PlanetDeployAltitude ?? ((float)((myObjectBuilder_RespawnShipDefinition.UseForPlanetsWithAtmosphere || myObjectBuilder_RespawnShipDefinition.UseForPlanetsWithoutAtmosphere) ? 2000 : 10)));
			HelpTextLocalizationId = myObjectBuilder_RespawnShipDefinition.HelpTextLocalizationId;
			SpawnNearProceduralAsteroids = myObjectBuilder_RespawnShipDefinition.SpawnNearProceduralAsteroids;
			PlanetTypes = myObjectBuilder_RespawnShipDefinition.PlanetTypes;
			SpawnPosition = myObjectBuilder_RespawnShipDefinition.SpawnPosition;
			SpawnPositionDispersionMin = myObjectBuilder_RespawnShipDefinition.SpawnPositionDispersionMin;
			SpawnPositionDispersionMax = myObjectBuilder_RespawnShipDefinition.SpawnPositionDispersionMax;
			CorrectInvalidStates();
		}

		private void CorrectInvalidStates()
		{
			bool hasValue = SpawnPosition.HasValue;
			bool flag = PlanetTypes != null;
			bool num = UseForPlanetsWithAtmosphere || UseForPlanetsWithoutAtmosphere;
			if (num && hasValue && flag)
			{
				UseForPlanetsWithAtmosphere = false;
				UseForPlanetsWithoutAtmosphere = false;
				CorrectInvalidStates();
			}
			if (!num && hasValue && flag)
			{
				PlanetTypes = null;
				CorrectInvalidStates();
			}
			if (!num && !hasValue && flag)
			{
				UseForPlanetsWithAtmosphere = true;
				UseForPlanetsWithoutAtmosphere = true;
				CorrectInvalidStates();
			}
			if (num && hasValue && !flag)
			{
				UseForPlanetsWithAtmosphere = false;
				UseForPlanetsWithoutAtmosphere = false;
				CorrectInvalidStates();
			}
		}
	}
}
