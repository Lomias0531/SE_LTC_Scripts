using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WeaponDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public class WeaponAmmoData
		{
			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoData_003C_003ERateOfFire_003C_003EAccessor : IMemberAccessor<WeaponAmmoData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponAmmoData owner, in int value)
				{
					owner.RateOfFire = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponAmmoData owner, out int value)
				{
					value = owner.RateOfFire;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoData_003C_003EShootSoundName_003C_003EAccessor : IMemberAccessor<WeaponAmmoData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponAmmoData owner, in string value)
				{
					owner.ShootSoundName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponAmmoData owner, out string value)
				{
					value = owner.ShootSoundName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoData_003C_003EShotsInBurst_003C_003EAccessor : IMemberAccessor<WeaponAmmoData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponAmmoData owner, in int value)
				{
					owner.ShotsInBurst = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponAmmoData owner, out int value)
				{
					value = owner.ShotsInBurst;
				}
			}

			private class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoData_003C_003EActor : IActivator, IActivator<WeaponAmmoData>
			{
				private sealed override object CreateInstance()
				{
					return new WeaponAmmoData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override WeaponAmmoData CreateInstance()
				{
					return new WeaponAmmoData();
				}

				WeaponAmmoData IActivator<WeaponAmmoData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			public int RateOfFire;

			[XmlAttribute]
			public string ShootSoundName;

			[XmlAttribute]
			public int ShotsInBurst;
		}

		[ProtoContract]
		public class WeaponAmmoMagazine
		{
			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoMagazine_003C_003EType_003C_003EAccessor : IMemberAccessor<WeaponAmmoMagazine, MyObjectBuilderType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponAmmoMagazine owner, in MyObjectBuilderType value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponAmmoMagazine owner, out MyObjectBuilderType value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoMagazine_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<WeaponAmmoMagazine, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponAmmoMagazine owner, in string value)
				{
					owner.Subtype = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponAmmoMagazine owner, out string value)
				{
					value = owner.Subtype;
				}
			}

			private class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponAmmoMagazine_003C_003EActor : IActivator, IActivator<WeaponAmmoMagazine>
			{
				private sealed override object CreateInstance()
				{
					return new WeaponAmmoMagazine();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override WeaponAmmoMagazine CreateInstance()
				{
					return new WeaponAmmoMagazine();
				}

				WeaponAmmoMagazine IActivator<WeaponAmmoMagazine>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlIgnore]
			public MyObjectBuilderType Type = typeof(MyObjectBuilder_AmmoMagazine);

			[XmlAttribute]
			[ProtoMember(1)]
			public string Subtype;
		}

		[ProtoContract]
		public class WeaponEffect
		{
			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponEffect_003C_003EAction_003C_003EAccessor : IMemberAccessor<WeaponEffect, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponEffect owner, in string value)
				{
					owner.Action = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponEffect owner, out string value)
				{
					value = owner.Action;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponEffect_003C_003EDummy_003C_003EAccessor : IMemberAccessor<WeaponEffect, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponEffect owner, in string value)
				{
					owner.Dummy = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponEffect owner, out string value)
				{
					value = owner.Dummy;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponEffect_003C_003EParticle_003C_003EAccessor : IMemberAccessor<WeaponEffect, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponEffect owner, in string value)
				{
					owner.Particle = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponEffect owner, out string value)
				{
					value = owner.Particle;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponEffect_003C_003ELoop_003C_003EAccessor : IMemberAccessor<WeaponEffect, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponEffect owner, in bool value)
				{
					owner.Loop = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponEffect owner, out bool value)
				{
					value = owner.Loop;
				}
			}

			protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponEffect_003C_003EInstantStop_003C_003EAccessor : IMemberAccessor<WeaponEffect, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WeaponEffect owner, in bool value)
				{
					owner.InstantStop = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WeaponEffect owner, out bool value)
				{
					value = owner.InstantStop;
				}
			}

			private class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EWeaponEffect_003C_003EActor : IActivator, IActivator<WeaponEffect>
			{
				private sealed override object CreateInstance()
				{
					return new WeaponEffect();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override WeaponEffect CreateInstance()
				{
					return new WeaponEffect();
				}

				WeaponEffect IActivator<WeaponEffect>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(43)]
			public string Action = "";

			[XmlAttribute]
			[ProtoMember(46)]
			public string Dummy = "";

			[XmlAttribute]
			[ProtoMember(49)]
			public string Particle = "";

			[XmlAttribute]
			[ProtoMember(52)]
			public bool Loop;

			[XmlAttribute]
			[ProtoMember(55, IsRequired = false)]
			public bool InstantStop = true;
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EProjectileAmmoData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, WeaponAmmoData>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in WeaponAmmoData value)
			{
				owner.ProjectileAmmoData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out WeaponAmmoData value)
			{
				value = owner.ProjectileAmmoData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EMissileAmmoData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, WeaponAmmoData>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in WeaponAmmoData value)
			{
				owner.MissileAmmoData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out WeaponAmmoData value)
			{
				value = owner.MissileAmmoData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003ENoAmmoSoundName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				owner.NoAmmoSoundName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				value = owner.NoAmmoSoundName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EReloadSoundName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				owner.ReloadSoundName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				value = owner.ReloadSoundName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003ESecondarySoundName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				owner.SecondarySoundName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				value = owner.SecondarySoundName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				owner.PhysicalMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				value = owner.PhysicalMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EDeviateShotAngle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value)
			{
				owner.DeviateShotAngle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value)
			{
				value = owner.DeviateShotAngle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EReleaseTimeAfterFire_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value)
			{
				owner.ReleaseTimeAfterFire = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value)
			{
				value = owner.ReleaseTimeAfterFire;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EMuzzleFlashLifeSpan_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in int value)
			{
				owner.MuzzleFlashLifeSpan = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out int value)
			{
				value = owner.MuzzleFlashLifeSpan;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EReloadTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in int value)
			{
				owner.ReloadTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out int value)
			{
				value = owner.ReloadTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EAmmoMagazines_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, WeaponAmmoMagazine[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in WeaponAmmoMagazine[] value)
			{
				owner.AmmoMagazines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out WeaponAmmoMagazine[] value)
			{
				value = owner.AmmoMagazines;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EEffects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, WeaponEffect[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in WeaponEffect[] value)
			{
				owner.Effects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out WeaponEffect[] value)
			{
				value = owner.Effects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EUseDefaultMuzzleFlash_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value)
			{
				owner.UseDefaultMuzzleFlash = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value)
			{
				value = owner.UseDefaultMuzzleFlash;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EDamageMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value)
			{
				owner.DamageMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value)
			{
				value = owner.DamageMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003ERangeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value)
			{
				owner.RangeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value)
			{
				value = owner.RangeMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EUseRandomizedRange_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value)
			{
				owner.UseRandomizedRange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value)
			{
				value = owner.UseRandomizedRange;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WeaponDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WeaponDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WeaponDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WeaponDefinition CreateInstance()
			{
				return new MyObjectBuilder_WeaponDefinition();
			}

			MyObjectBuilder_WeaponDefinition IActivator<MyObjectBuilder_WeaponDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public WeaponAmmoData ProjectileAmmoData;

		[ProtoMember(7)]
		public WeaponAmmoData MissileAmmoData;

		[ProtoMember(10)]
		public string NoAmmoSoundName;

		[ProtoMember(13)]
		public string ReloadSoundName;

		[ProtoMember(16)]
		public string SecondarySoundName;

		[ProtoMember(19)]
		public string PhysicalMaterial = "Metal";

		[ProtoMember(22)]
		public float DeviateShotAngle;

		[ProtoMember(25)]
		public float ReleaseTimeAfterFire;

		[ProtoMember(28)]
		public int MuzzleFlashLifeSpan;

		[ProtoMember(31)]
		public int ReloadTime = 2000;

		[XmlArrayItem("AmmoMagazine")]
		[ProtoMember(34)]
		public WeaponAmmoMagazine[] AmmoMagazines;

		[XmlArrayItem("Effect")]
		[ProtoMember(37)]
		public WeaponEffect[] Effects;

		[ProtoMember(40)]
		public bool UseDefaultMuzzleFlash = true;

		[ProtoMember(58)]
		[DefaultValue(1)]
		public float DamageMultiplier = 1f;

		[ProtoMember(61, IsRequired = false)]
		[DefaultValue(1)]
		public float RangeMultiplier = 1f;

		[ProtoMember(64, IsRequired = false)]
		[DefaultValue(true)]
		public bool UseRandomizedRange = true;
	}
}
