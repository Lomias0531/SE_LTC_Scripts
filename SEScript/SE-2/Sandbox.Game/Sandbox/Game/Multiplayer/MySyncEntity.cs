using System;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace Sandbox.Game.Multiplayer
{
	[PreloadRequired]
	public class MySyncEntity : MySyncComponentBase
	{
		private class Sandbox_Game_Multiplayer_MySyncEntity_003C_003EActor
		{
		}

		public new readonly MyEntity Entity;

		public MySyncEntity(MyEntity entity)
		{
			Entity = entity;
		}
	}
}
