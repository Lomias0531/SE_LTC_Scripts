using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.World
{
	[GenerateActivator]
	public abstract class MyWorldGeneratorStartingStateBase
	{
		public string FactionTag;

		public abstract Vector3D? GetStartingLocation();

		public abstract void SetupCharacter(MyWorldGenerator.Args generatorArgs);

		public virtual void Init(MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
		{
			FactionTag = builder.FactionTag;
		}

		public virtual MyObjectBuilder_WorldGeneratorPlayerStartingState GetObjectBuilder()
		{
			MyObjectBuilder_WorldGeneratorPlayerStartingState myObjectBuilder_WorldGeneratorPlayerStartingState = MyWorldGenerator.StartingStateFactory.CreateObjectBuilder(this);
			myObjectBuilder_WorldGeneratorPlayerStartingState.FactionTag = FactionTag;
			return myObjectBuilder_WorldGeneratorPlayerStartingState;
		}

		protected Vector3D FixPositionToVoxel(Vector3D position)
		{
			MyVoxelMap myVoxelMap = null;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				myVoxelMap = (entity as MyVoxelMap);
				if (myVoxelMap != null)
				{
					break;
				}
			}
			float maxVertDistance = 2048f;
			if (myVoxelMap != null)
			{
				position = myVoxelMap.GetPositionOnVoxel(position, maxVertDistance);
			}
			return position;
		}

		protected virtual void CreateAndSetPlayerFaction()
		{
			if (Sync.IsServer && FactionTag != null && MySession.Static.LocalHumanPlayer != null)
			{
				MySession.Static.Factions.TryGetOrCreateFactionByTag(FactionTag).AcceptJoin(MySession.Static.LocalHumanPlayer.Identity.IdentityId);
			}
		}
	}
}
