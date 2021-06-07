using Sandbox.Game.Entities;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WeaponDefinition), null)]
	public class MyWeaponDefinition : MyDefinitionBase
	{
		public class MyWeaponAmmoData
		{
			public int RateOfFire;

			public int ShotsInBurst;

			public MySoundPair ShootSound;

			public int ShootIntervalInMiliseconds;

			public MyWeaponAmmoData(MyObjectBuilder_WeaponDefinition.WeaponAmmoData data)
				: this(data.RateOfFire, data.ShootSoundName, data.ShotsInBurst)
			{
			}

			public MyWeaponAmmoData(int rateOfFire, string soundName, int shotsInBurst)
			{
				RateOfFire = rateOfFire;
				ShotsInBurst = shotsInBurst;
				ShootSound = new MySoundPair(soundName);
				ShootIntervalInMiliseconds = (int)(1000f / ((float)RateOfFire / 60f));
			}
		}

		public enum WeaponEffectAction
		{
			Unknown,
			Shoot
		}

		public class MyWeaponEffect
		{
			public WeaponEffectAction Action;

			public string Dummy = "";

			public string Particle = "";

			public bool Loop;

			public bool InstantStop;

			public MyWeaponEffect(string action, string dummy, string particle, bool loop, bool instantStop)
			{
				Dummy = dummy;
				Particle = particle;
				Loop = loop;
				InstantStop = instantStop;
				foreach (WeaponEffectAction value in Enum.GetValues(typeof(WeaponEffectAction)))
				{
					if (value.ToString().Equals(action))
					{
						Action = value;
						break;
					}
				}
			}
		}

		private class Sandbox_Definitions_MyWeaponDefinition_003C_003EActor : IActivator, IActivator<MyWeaponDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeaponDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeaponDefinition CreateInstance()
			{
				return new MyWeaponDefinition();
			}

			MyWeaponDefinition IActivator<MyWeaponDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly string ErrorMessageTemplate = "No weapon ammo data specified for {0} ammo (<{1}AmmoData> tag is missing in weapon definition)";

		public MySoundPair NoAmmoSound;

		public MySoundPair ReloadSound;

		public MySoundPair SecondarySound;

		public float DeviateShotAngle;

		public float ReleaseTimeAfterFire;

		public int MuzzleFlashLifeSpan;

		public MyDefinitionId[] AmmoMagazinesId;

		public MyWeaponAmmoData[] WeaponAmmoDatas;

		public MyWeaponEffect[] WeaponEffects;

		public MyStringHash PhysicalMaterial;

		public bool UseDefaultMuzzleFlash;

		public int ReloadTime = 2000;

		public float DamageMultiplier = 1f;

		public float RangeMultiplier = 1f;

		public bool UseRandomizedRange = true;

		public bool HasProjectileAmmoDefined => WeaponAmmoDatas[0] != null;

		public bool HasMissileAmmoDefined => WeaponAmmoDatas[1] != null;

		public bool HasSpecificAmmoData(MyAmmoDefinition ammoDefinition)
		{
			return WeaponAmmoDatas[(int)ammoDefinition.AmmoType] != null;
		}

		public bool HasAmmoMagazines()
		{
			if (AmmoMagazinesId != null)
			{
				return AmmoMagazinesId.Length != 0;
			}
			return false;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WeaponDefinition myObjectBuilder_WeaponDefinition = builder as MyObjectBuilder_WeaponDefinition;
			WeaponAmmoDatas = new MyWeaponAmmoData[Enum.GetValues(typeof(MyAmmoType)).Length];
			WeaponEffects = new MyWeaponEffect[(myObjectBuilder_WeaponDefinition.Effects != null) ? myObjectBuilder_WeaponDefinition.Effects.Length : 0];
			if (myObjectBuilder_WeaponDefinition.Effects != null)
			{
				for (int i = 0; i < myObjectBuilder_WeaponDefinition.Effects.Length; i++)
				{
					WeaponEffects[i] = new MyWeaponEffect(myObjectBuilder_WeaponDefinition.Effects[i].Action, myObjectBuilder_WeaponDefinition.Effects[i].Dummy, myObjectBuilder_WeaponDefinition.Effects[i].Particle, myObjectBuilder_WeaponDefinition.Effects[i].Loop, myObjectBuilder_WeaponDefinition.Effects[i].InstantStop);
				}
			}
			PhysicalMaterial = MyStringHash.GetOrCompute(myObjectBuilder_WeaponDefinition.PhysicalMaterial);
			UseDefaultMuzzleFlash = myObjectBuilder_WeaponDefinition.UseDefaultMuzzleFlash;
			NoAmmoSound = new MySoundPair(myObjectBuilder_WeaponDefinition.NoAmmoSoundName);
			ReloadSound = new MySoundPair(myObjectBuilder_WeaponDefinition.ReloadSoundName);
			SecondarySound = new MySoundPair(myObjectBuilder_WeaponDefinition.SecondarySoundName);
			DeviateShotAngle = MathHelper.ToRadians(myObjectBuilder_WeaponDefinition.DeviateShotAngle);
			ReleaseTimeAfterFire = myObjectBuilder_WeaponDefinition.ReleaseTimeAfterFire;
			MuzzleFlashLifeSpan = myObjectBuilder_WeaponDefinition.MuzzleFlashLifeSpan;
			ReloadTime = myObjectBuilder_WeaponDefinition.ReloadTime;
			DamageMultiplier = myObjectBuilder_WeaponDefinition.DamageMultiplier;
			RangeMultiplier = myObjectBuilder_WeaponDefinition.RangeMultiplier;
			UseRandomizedRange = myObjectBuilder_WeaponDefinition.UseRandomizedRange;
			AmmoMagazinesId = new MyDefinitionId[myObjectBuilder_WeaponDefinition.AmmoMagazines.Length];
			for (int j = 0; j < AmmoMagazinesId.Length; j++)
			{
				MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine weaponAmmoMagazine = myObjectBuilder_WeaponDefinition.AmmoMagazines[j];
				AmmoMagazinesId[j] = new MyDefinitionId(weaponAmmoMagazine.Type, weaponAmmoMagazine.Subtype);
				MyAmmoMagazineDefinition ammoMagazineDefinition = MyDefinitionManager.Static.GetAmmoMagazineDefinition(AmmoMagazinesId[j]);
				MyAmmoType ammoType = MyDefinitionManager.Static.GetAmmoDefinition(ammoMagazineDefinition.AmmoDefinitionId).AmmoType;
				string text = null;
				switch (ammoType)
				{
				case MyAmmoType.HighSpeed:
					if (myObjectBuilder_WeaponDefinition.ProjectileAmmoData != null)
					{
						WeaponAmmoDatas[0] = new MyWeaponAmmoData(myObjectBuilder_WeaponDefinition.ProjectileAmmoData);
					}
					else
					{
						text = string.Format(ErrorMessageTemplate, "projectile", "Projectile");
					}
					break;
				case MyAmmoType.Missile:
					if (myObjectBuilder_WeaponDefinition.MissileAmmoData != null)
					{
						WeaponAmmoDatas[1] = new MyWeaponAmmoData(myObjectBuilder_WeaponDefinition.MissileAmmoData);
					}
					else
					{
						text = string.Format(ErrorMessageTemplate, "missile", "Missile");
					}
					break;
				default:
					throw new NotImplementedException();
				}
				if (!string.IsNullOrEmpty(text))
				{
					MyDefinitionErrors.Add(Context, text, TErrorSeverity.Critical);
				}
			}
		}

		public bool IsAmmoMagazineCompatible(MyDefinitionId ammoMagazineDefinitionId)
		{
			for (int i = 0; i < AmmoMagazinesId.Length; i++)
			{
				if (ammoMagazineDefinitionId.SubtypeId == AmmoMagazinesId[i].SubtypeId)
				{
					return true;
				}
			}
			return false;
		}

		public int GetAmmoMagazineIdArrayIndex(MyDefinitionId ammoMagazineId)
		{
			for (int i = 0; i < AmmoMagazinesId.Length; i++)
			{
				if (ammoMagazineId.SubtypeId == AmmoMagazinesId[i].SubtypeId)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
