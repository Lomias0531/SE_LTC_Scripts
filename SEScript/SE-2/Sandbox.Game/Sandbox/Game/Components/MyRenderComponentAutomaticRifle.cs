using Sandbox.Game.Weapons;
using System;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentAutomaticRifle : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentAutomaticRifle_003C_003EActor : IActivator, IActivator<MyRenderComponentAutomaticRifle>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentAutomaticRifle();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentAutomaticRifle CreateInstance()
			{
				return new MyRenderComponentAutomaticRifle();
			}

			MyRenderComponentAutomaticRifle IActivator<MyRenderComponentAutomaticRifle>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyStringId ID_MUZZLE_FLASH_SIDE = MyStringId.GetOrCompute("MuzzleFlashMachineGunSide");

		private static readonly MyStringId ID_MUZZLE_FLASH_FRONT = MyStringId.GetOrCompute("MuzzleFlashMachineGunFront");

		private MyAutomaticRifleGun m_rifleGun;

		public static void GenerateMuzzleFlash(Vector3D position, Vector3 dir, float radius, float length)
		{
			GenerateMuzzleFlash(position, dir, uint.MaxValue, ref MatrixD.Zero, radius, length);
		}

		public static void GenerateMuzzleFlash(Vector3D position, Vector3 dir, uint renderObjectID, ref MatrixD worldToLocal, float radius, float length)
		{
			float angle = MyParticlesManager.Paused ? 0f : MyUtils.GetRandomFloat(0f, MathF.E * 449f / 777f);
			float num = 10f;
			Vector4 color = new Vector4(num, num, num, 1f);
			MyTransparentGeometry.AddLineBillboard(ID_MUZZLE_FLASH_SIDE, color, position, renderObjectID, ref worldToLocal, dir, length, 0.15f, MyBillboard.BlendTypeEnum.AdditiveBottom);
			MyTransparentGeometry.AddPointBillboard(ID_MUZZLE_FLASH_FRONT, color, position, renderObjectID, ref worldToLocal, radius, angle, -1, MyBillboard.BlendTypeEnum.AdditiveBottom);
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_rifleGun = (base.Container.Entity as MyAutomaticRifleGun);
		}

		public override void Draw()
		{
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_rifleGun.LastTimeShoot;
			MyGunBase gunBase = m_rifleGun.GunBase;
			if (gunBase.UseDefaultMuzzleFlash && num <= gunBase.MuzzleFlashLifeSpan)
			{
				GenerateMuzzleFlash(gunBase.GetMuzzleWorldPosition(), gunBase.GetMuzzleWorldMatrix().Forward, 0.1f, 0.3f);
			}
		}
	}
}
