using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.Renders;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using SpaceEngineers.Game.Weapons.Guns.Barrels;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LargeGatlingTurret))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyLargeGatlingTurret),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyLargeGatlingTurret)
	})]
	public class MyLargeGatlingTurret : MyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeGatlingTurret, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeGatlingTurret
	{
		public int Burst
		{
			get;
			private set;
		}

		protected override float ForwardCameraOffset => 0.5f;

		protected override float UpCameraOffset => 0.75f;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_randomStandbyChangeConst_ms = MyUtils.GetRandomInt(3500, 4500);
			if (m_gunBase.HasAmmoMagazines)
			{
				m_shootingCueEnum = m_gunBase.ShootSound;
			}
			m_rotatingCueEnum.Init("WepTurretGatlingRotate");
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction)
			{
				m_gunBase.Shoot(base.Parent.Physics.LinearVelocity);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				m_base1 = base.Subparts["GatlingTurretBase1"];
				m_base2 = m_base1.Subparts["GatlingTurretBase2"];
				m_barrel = new MyLargeGatlingBarrel();
				((MyLargeGatlingBarrel)m_barrel).Init(m_base2.Subparts["GatlingBarrel"], this);
				GetCameraDummy();
			}
			else
			{
				m_base1 = null;
				m_base2 = null;
				m_barrel = null;
			}
			ResetRotation();
		}

		public override void UpdateAfterSimulation()
		{
			if (!MySession.Static.WeaponsEnabled)
			{
				RotateModels();
				return;
			}
			base.UpdateAfterSimulation();
			DrawLasers();
		}

		public MyLargeGatlingTurret()
		{
			base.Render = new MyRenderComponentLargeTurret();
		}
	}
}
