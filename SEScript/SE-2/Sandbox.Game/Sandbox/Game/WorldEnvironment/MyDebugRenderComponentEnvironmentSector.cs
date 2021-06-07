using Sandbox.Game.Components;
using Sandbox.Game.Entities.Planet;
using VRage.ModAPI;

namespace Sandbox.Game.WorldEnvironment
{
	internal class MyDebugRenderComponentEnvironmentSector : MyDebugRenderComponent
	{
		public override void DebugDraw()
		{
			if (MyPlanetEnvironmentSessionComponent.DebugDrawSectors)
			{
				((MyEnvironmentSector)Entity).DebugDraw();
			}
		}

		public MyDebugRenderComponentEnvironmentSector(IMyEntity entity)
			: base(entity)
		{
		}
	}
}
