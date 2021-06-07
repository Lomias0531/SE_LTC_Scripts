using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MissileAmmoDefinition), null)]
	public class MyMissileAmmoDefinition : MyAmmoDefinition
	{
		private class Sandbox_Definitions_MyMissileAmmoDefinition_003C_003EActor : IActivator, IActivator<MyMissileAmmoDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMissileAmmoDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMissileAmmoDefinition CreateInstance()
			{
				return new MyMissileAmmoDefinition();
			}

			MyMissileAmmoDefinition IActivator<MyMissileAmmoDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const float MINIMAL_EXPLOSION_RADIUS = 0.6f;

		public float MissileMass;

		public float MissileExplosionRadius;

		public string MissileModelName;

		public float MissileAcceleration;

		public float MissileInitialSpeed;

		public bool MissileSkipAcceleration;

		public float MissileExplosionDamage;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MissileAmmoDefinition obj = builder as MyObjectBuilder_MissileAmmoDefinition;
			AmmoType = MyAmmoType.Missile;
			MyObjectBuilder_MissileAmmoDefinition.AmmoMissileProperties missileProperties = obj.MissileProperties;
			MissileAcceleration = missileProperties.MissileAcceleration;
			MissileExplosionDamage = missileProperties.MissileExplosionDamage;
			MissileExplosionRadius = missileProperties.MissileExplosionRadius;
			MissileInitialSpeed = missileProperties.MissileInitialSpeed;
			MissileMass = missileProperties.MissileMass;
			MissileModelName = missileProperties.MissileModelName;
			MissileSkipAcceleration = missileProperties.MissileSkipAcceleration;
		}

		public override float GetDamageForMechanicalObjects()
		{
			return MissileExplosionDamage;
		}
	}
}
