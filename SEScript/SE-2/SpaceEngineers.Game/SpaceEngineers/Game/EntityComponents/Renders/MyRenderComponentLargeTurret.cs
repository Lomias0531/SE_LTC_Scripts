using Sandbox.Game.Components;
using Sandbox.Game.Weapons;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
	internal class MyRenderComponentLargeTurret : MyRenderComponentCubeBlock
	{
		private MyLargeTurretBase m_turretBase;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_turretBase = (base.Container.Entity as MyLargeTurretBase);
		}

		public override void Draw()
		{
			if (m_turretBase.IsWorking)
			{
				base.Draw();
				if (m_turretBase.Barrel != null)
				{
					m_turretBase.Barrel.Draw();
				}
			}
		}
	}
}
