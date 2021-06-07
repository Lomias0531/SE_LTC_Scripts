using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SmallMissileLauncherReload))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMySmallMissileLauncherReload),
		typeof(Sandbox.ModAPI.Ingame.IMySmallMissileLauncherReload)
	})]
	public class MySmallMissileLauncherReload : MySmallMissileLauncher, Sandbox.ModAPI.IMySmallMissileLauncherReload, Sandbox.ModAPI.IMySmallMissileLauncher, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallMissileLauncher, Sandbox.ModAPI.Ingame.IMySmallMissileLauncherReload
	{
		private class Sandbox_Game_Weapons_MySmallMissileLauncherReload_003C_003EActor : IActivator, IActivator<MySmallMissileLauncherReload>
		{
			private sealed override object CreateInstance()
			{
				return new MySmallMissileLauncherReload();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySmallMissileLauncherReload CreateInstance()
			{
				return new MySmallMissileLauncherReload();
			}

			MySmallMissileLauncherReload IActivator<MySmallMissileLauncherReload>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const int COOLDOWN_TIME_MILISECONDS = 5000;

		private int m_numRocketsShot;

		private static readonly MyHudNotification MISSILE_RELOAD_NOTIFICATION = new MyHudNotification(MySpaceTexts.MissileLauncherReloadingNotification, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);

		public int BurstFireRate => base.GunBase.ShotsInBurst;

		public MySmallMissileLauncherReload()
		{
			CreateTerminalControls();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySmallMissileLauncherReload>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MySmallMissileLauncherReload> obj = new MyTerminalControlOnOffSwitch<MySmallMissileLauncherReload>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MySmallMissileLauncherReload x) => x.UseConveyorSystem),
					Setter = delegate(MySmallMissileLauncherReload x, bool v)
					{
						x.UseConveyorSystem = v;
					},
					Visible = ((MySmallMissileLauncherReload x) => true)
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (BurstFireRate != m_numRocketsShot || MySandboxGame.TotalGamePlayTimeInMilliseconds >= m_nextShootTime)
			{
				if (BurstFireRate == m_numRocketsShot)
				{
					m_numRocketsShot = 0;
				}
				m_numRocketsShot++;
				base.Shoot(action, direction, overrideWeaponPos, gunAction);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			MyObjectBuilder_SmallMissileLauncherReload myObjectBuilder_SmallMissileLauncherReload = (MyObjectBuilder_SmallMissileLauncherReload)builder;
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_SmallMissileLauncherReload.UseConveyorSystem);
		}
	}
}
