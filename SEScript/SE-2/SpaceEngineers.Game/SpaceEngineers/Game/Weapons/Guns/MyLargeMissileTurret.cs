using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using SpaceEngineers.Game.Weapons.Guns.Barrels;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LargeMissileTurret))]
	public class MyLargeMissileTurret : MyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeMissileTurret, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeMissileTurret, IMyMissileGunObject, IMyGunObject<MyGunBase>
	{
		protected new sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MyLargeMissileTurret, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeMissileTurret @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected new sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MyLargeMissileTurret, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLargeMissileTurret @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		public override IMyMissileGunObject Launcher => this;

		protected override float ForwardCameraOffset => 0.5f;

		protected override float UpCameraOffset => 1f;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_randomStandbyChangeConst_ms = 4000;
			m_rotationSpeed = 0.00157079648f;
			m_elevationSpeed = 0.00157079648f;
			if (base.BlockDefinition != null)
			{
				m_rotationSpeed = base.BlockDefinition.RotationSpeed;
				m_elevationSpeed = base.BlockDefinition.ElevationSpeed;
			}
			if (m_gunBase.HasAmmoMagazines)
			{
				m_shootingCueEnum = m_gunBase.ShootSound;
			}
			m_rotatingCueEnum.Init("WepTurretGatlingRotate");
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				m_base1 = base.Subparts["MissileTurretBase1"];
				m_base2 = m_base1.Subparts["MissileTurretBarrels"];
				m_barrel = new MyLargeMissileBarrel();
				((MyLargeMissileBarrel)m_barrel).Init(m_base2, this);
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

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction && m_barrel != null)
			{
				m_barrel.StartShooting();
			}
		}

		public new void MissileShootEffect()
		{
			if (m_barrel != null)
			{
				m_barrel.ShootEffect();
			}
		}

		public new void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeMissileTurret x) => x.OnShootMissile, builder);
		}

		[Event(null, 94)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Add(builder);
		}

		public new void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeMissileTurret x) => x.OnRemoveMissile, entityId);
		}

		[Event(null, 107)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Remove(entityId);
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

		public override void ShootFromTerminal(Vector3 direction)
		{
			if (m_barrel != null)
			{
				base.ShootFromTerminal(direction);
				m_isControlled = true;
				m_barrel.StartShooting();
				m_isControlled = false;
			}
		}
	}
}
