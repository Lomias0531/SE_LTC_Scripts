using Sandbox.Game.Entities.EnvironmentItems;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentEnvironmentItems : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentEnvironmentItems_003C_003EActor
		{
		}

		internal readonly MyEnvironmentItems EnvironmentItems;

		internal MyRenderComponentEnvironmentItems(MyEnvironmentItems environmentItems)
		{
			EnvironmentItems = environmentItems;
		}

		public override void AddRenderObjects()
		{
		}

		public override void RemoveRenderObjects()
		{
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in EnvironmentItems.Sectors)
			{
				sector.Value.UnloadRenderObjects();
			}
			foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector2 in EnvironmentItems.Sectors)
			{
				sector2.Value.ClearInstanceData();
			}
		}
	}
}
