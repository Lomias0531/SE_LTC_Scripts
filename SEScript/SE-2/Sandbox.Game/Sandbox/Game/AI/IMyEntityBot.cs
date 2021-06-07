using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI
{
	public interface IMyEntityBot : IMyBot
	{
		MyEntity BotEntity
		{
			get;
		}

		bool ShouldFollowPlayer
		{
			get;
			set;
		}

		void Spawn(Vector3D? spawnPosition, bool spawnedByPlayer);
	}
}
