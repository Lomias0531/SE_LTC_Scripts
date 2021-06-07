using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using VRage.Game.Components;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentLargeTurret : MyDebugRenderComponent
	{
		private MyLargeTurretBase m_turretBase;

		public MyDebugRenderComponentLargeTurret(MyLargeTurretBase turretBase)
			: base(turretBase)
		{
			m_turretBase = turretBase;
		}

		public override void DebugDraw()
		{
			if (m_turretBase.Render.GetModel() != null)
			{
				_ = m_turretBase.Render.GetModel().BoundingSphere;
			}
			Vector3 vector = default(Vector3);
			switch (m_turretBase.GetStatus())
			{
			case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated:
				vector = Color.Green.ToVector3();
				break;
			case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Searching:
				vector = Color.Red.ToVector3();
				break;
			case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Shooting:
				vector = Color.White.ToVector3();
				break;
			}
			Color colorFrom = new Color(vector);
			Color colorTo = new Color(vector);
			if (m_turretBase.Target != null)
			{
				MyRenderProxy.DebugDrawLine3D(m_turretBase.Barrel.Entity.PositionComp.GetPosition(), m_turretBase.Target.PositionComp.GetPosition(), colorFrom, colorTo, depthRead: false);
				MyRenderProxy.DebugDrawSphere(m_turretBase.Target.PositionComp.GetPosition(), m_turretBase.Target.PositionComp.LocalVolume.Radius, Color.White, 1f, depthRead: false);
			}
			m_turretBase.Components.Get<MyResourceSinkComponent>()?.DebugDraw(m_turretBase.PositionComp.WorldMatrix);
			base.DebugDraw();
		}
	}
}
