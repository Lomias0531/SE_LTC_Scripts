using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProjectileAmmoDefinition), null)]
	public class MyProjectileAmmoDefinition : MyAmmoDefinition
	{
		private class Sandbox_Definitions_MyProjectileAmmoDefinition_003C_003EActor : IActivator, IActivator<MyProjectileAmmoDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProjectileAmmoDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProjectileAmmoDefinition CreateInstance()
			{
				return new MyProjectileAmmoDefinition();
			}

			MyProjectileAmmoDefinition IActivator<MyProjectileAmmoDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float ProjectileHitImpulse;

		public float ProjectileTrailScale;

		public Vector3 ProjectileTrailColor;

		public string ProjectileTrailMaterial;

		public float ProjectileTrailProbability;

		public string ProjectileOnHitEffectName;

		public float ProjectileMassDamage;

		public float ProjectileHealthDamage;

		public bool HeadShot;

		public float ProjectileHeadShotDamage;

		public int ProjectileCount;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProjectileAmmoDefinition obj = builder as MyObjectBuilder_ProjectileAmmoDefinition;
			AmmoType = MyAmmoType.HighSpeed;
			MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties projectileProperties = obj.ProjectileProperties;
			ProjectileHealthDamage = projectileProperties.ProjectileHealthDamage;
			ProjectileHitImpulse = projectileProperties.ProjectileHitImpulse;
			ProjectileMassDamage = projectileProperties.ProjectileMassDamage;
			ProjectileOnHitEffectName = projectileProperties.ProjectileOnHitEffectName;
			ProjectileTrailColor = projectileProperties.ProjectileTrailColor;
			ProjectileTrailMaterial = projectileProperties.ProjectileTrailMaterial;
			ProjectileTrailProbability = projectileProperties.ProjectileTrailProbability;
			ProjectileTrailScale = projectileProperties.ProjectileTrailScale;
			HeadShot = projectileProperties.HeadShot;
			ProjectileHeadShotDamage = projectileProperties.ProjectileHeadShotDamage;
			ProjectileCount = projectileProperties.ProjectileCount;
		}

		public override float GetDamageForMechanicalObjects()
		{
			return ProjectileMassDamage;
		}
	}
}
