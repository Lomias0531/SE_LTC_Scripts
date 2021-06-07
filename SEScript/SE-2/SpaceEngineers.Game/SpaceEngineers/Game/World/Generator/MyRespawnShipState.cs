using Sandbox.Definitions;
using Sandbox.Game.World;
using VRage.Game;
using VRageMath;

namespace SpaceEngineers.Game.World.Generator
{
	[MyWorldGenerator.StartingStateType(typeof(MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip))]
	public class MyRespawnShipState : MyWorldGeneratorStartingStateBase
	{
		private string m_respawnShipId;

		public override void Init(MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
		{
			base.Init(builder);
			MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip myObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip = builder as MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip;
			m_respawnShipId = myObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip.RespawnShip;
		}

		public override MyObjectBuilder_WorldGeneratorPlayerStartingState GetObjectBuilder()
		{
			MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip obj = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip;
			obj.RespawnShip = m_respawnShipId;
			return obj;
		}

		public override void SetupCharacter(MyWorldGenerator.Args generatorArgs)
		{
			string respawnShipId = m_respawnShipId;
			if (!MyDefinitionManager.Static.HasRespawnShip(m_respawnShipId))
			{
				respawnShipId = MyDefinitionManager.Static.GetFirstRespawnShip();
			}
			if (MySession.Static.LocalHumanPlayer != null)
			{
				CreateAndSetPlayerFaction();
				MySpaceRespawnComponent.Static.SpawnAtShip(MySession.Static.LocalHumanPlayer, respawnShipId, null, null, null);
			}
		}

		public override Vector3D? GetStartingLocation()
		{
			return null;
		}
	}
}
