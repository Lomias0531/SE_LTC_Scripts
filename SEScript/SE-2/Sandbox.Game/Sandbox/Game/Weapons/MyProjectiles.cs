using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Generics;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	internal class MyProjectiles : MySessionComponentBase
	{
		private static MyObjectsPool<MyProjectile> m_projectiles;

		static MyProjectiles()
		{
		}

		public override void LoadData()
		{
			if (m_projectiles == null)
			{
				m_projectiles = new MyObjectsPool<MyProjectile>(8192);
			}
		}

		protected override void UnloadData()
		{
			if (m_projectiles != null)
			{
				m_projectiles.DeallocateAll();
			}
			m_projectiles = null;
		}

		public static void Add(MyWeaponPropertiesWrapper props, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, IMyGunBaseUser user, MyEntity owner)
		{
			m_projectiles.AllocateOrCreate(out MyProjectile item);
			item.Start(props.GetCurrentAmmoDefinitionAs<MyProjectileAmmoDefinition>(), props.WeaponDefinition, user.IgnoreEntities, origin, initialVelocity, directionNormalized, user.Weapon);
			item.OwnerEntity = (user.Owner ?? ((user.IgnoreEntities != null && user.IgnoreEntities.Length != 0) ? user.IgnoreEntities[0] : null));
			item.OwnerEntityAbsolute = owner;
		}

		public static void AddShrapnel(MyProjectileAmmoDefinition ammoDefinition, MyEntity[] ignoreEntities, Vector3 origin, Vector3 initialVelocity, Vector3 directionNormalized, bool groupStart, float thicknessMultiplier, float trailProbability, MyEntity weapon, MyEntity ownerEntity = null, float projectileCountMultiplier = 1f)
		{
			m_projectiles.AllocateOrCreate(out MyProjectile item);
			item.Start(ammoDefinition, null, ignoreEntities, origin, initialVelocity, directionNormalized, weapon);
			item.OwnerEntity = (ownerEntity ?? ((ignoreEntities != null && ignoreEntities.Length != 0) ? ignoreEntities[0] : null));
		}

		public static void AddShotgun(MyProjectileAmmoDefinition ammoDefinition, MyEntity ignorePhysObject, Vector3 origin, Vector3 initialVelocity, Vector3 directionNormalized, bool groupStart, float thicknessMultiplier, MyEntity weapon, float frontBillboardSize, MyEntity ownerEntity = null, float projectileCountMultiplier = 1f)
		{
			m_projectiles.Allocate();
		}

		public override void UpdateBeforeSimulation()
		{
			foreach (MyProjectile item in m_projectiles.Active)
			{
				if (!item.Update())
				{
					item.Close();
					m_projectiles.MarkForDeallocate(item);
				}
			}
			m_projectiles.DeallocateAllMarked();
		}

		public override void Draw()
		{
			foreach (MyProjectile item in m_projectiles.Active)
			{
				item.Draw();
			}
		}
	}
}
