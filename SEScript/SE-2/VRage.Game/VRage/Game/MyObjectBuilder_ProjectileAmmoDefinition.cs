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
	public class MyObjectBuilder_ProjectileAmmoDefinition : MyObjectBuilder_AmmoDefinition
	{
		[ProtoContract]
		public class AmmoProjectileProperties
		{
			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileHitImpulse_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in float value)
				{
					owner.ProjectileHitImpulse = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out float value)
				{
					value = owner.ProjectileHitImpulse;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileTrailScale_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in float value)
				{
					owner.ProjectileTrailScale = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out float value)
				{
					value = owner.ProjectileTrailScale;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileTrailColor_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, SerializableVector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in SerializableVector3 value)
				{
					owner.ProjectileTrailColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out SerializableVector3 value)
				{
					value = owner.ProjectileTrailColor;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileTrailMaterial_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in string value)
				{
					owner.ProjectileTrailMaterial = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out string value)
				{
					value = owner.ProjectileTrailMaterial;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileTrailProbability_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in float value)
				{
					owner.ProjectileTrailProbability = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out float value)
				{
					value = owner.ProjectileTrailProbability;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileOnHitEffectName_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in string value)
				{
					owner.ProjectileOnHitEffectName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out string value)
				{
					value = owner.ProjectileOnHitEffectName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileMassDamage_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in float value)
				{
					owner.ProjectileMassDamage = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out float value)
				{
					value = owner.ProjectileMassDamage;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileHealthDamage_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in float value)
				{
					owner.ProjectileHealthDamage = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out float value)
				{
					value = owner.ProjectileHealthDamage;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EHeadShot_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in bool value)
				{
					owner.HeadShot = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out bool value)
				{
					value = owner.HeadShot;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileHeadShotDamage_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in float value)
				{
					owner.ProjectileHeadShotDamage = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out float value)
				{
					value = owner.ProjectileHeadShotDamage;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EProjectileCount_003C_003EAccessor : IMemberAccessor<AmmoProjectileProperties, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AmmoProjectileProperties owner, in int value)
				{
					owner.ProjectileCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AmmoProjectileProperties owner, out int value)
				{
					value = owner.ProjectileCount;
				}
			}

			private class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAmmoProjectileProperties_003C_003EActor : IActivator, IActivator<AmmoProjectileProperties>
			{
				private sealed override object CreateInstance()
				{
					return new AmmoProjectileProperties();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override AmmoProjectileProperties CreateInstance()
				{
					return new AmmoProjectileProperties();
				}

				AmmoProjectileProperties IActivator<AmmoProjectileProperties>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public float ProjectileHitImpulse;

			[ProtoMember(4)]
			[DefaultValue(0.1f)]
			public float ProjectileTrailScale = 0.1f;

			[ProtoMember(7)]
			public SerializableVector3 ProjectileTrailColor = new SerializableVector3(1f, 1f, 1f);

			[ProtoMember(10)]
			[DefaultValue(null)]
			public string ProjectileTrailMaterial;

			[ProtoMember(13)]
			[DefaultValue(0.5f)]
			public float ProjectileTrailProbability = 0.5f;

			[ProtoMember(16)]
			public string ProjectileOnHitEffectName = "Hit_BasicAmmoSmall";

			[ProtoMember(19)]
			public float ProjectileMassDamage;

			[ProtoMember(22)]
			public float ProjectileHealthDamage;

			[ProtoMember(25)]
			public bool HeadShot;

			[ProtoMember(28)]
			[DefaultValue(120)]
			public float ProjectileHeadShotDamage = 120f;

			[ProtoMember(31)]
			[DefaultValue(1)]
			public int ProjectileCount = 1;
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EProjectileProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, AmmoProjectileProperties>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in AmmoProjectileProperties value)
			{
				owner.ProjectileProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out AmmoProjectileProperties value)
			{
				value = owner.ProjectileProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EBasicProperties_003C_003EAccessor : VRage_Game_MyObjectBuilder_AmmoDefinition_003C_003EBasicProperties_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, AmmoBasicProperties>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in AmmoBasicProperties value)
			{
				Set(ref *(MyObjectBuilder_AmmoDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out AmmoBasicProperties value)
			{
				Get(ref *(MyObjectBuilder_AmmoDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProjectileAmmoDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProjectileAmmoDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProjectileAmmoDefinition CreateInstance()
			{
				return new MyObjectBuilder_ProjectileAmmoDefinition();
			}

			MyObjectBuilder_ProjectileAmmoDefinition IActivator<MyObjectBuilder_ProjectileAmmoDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(34)]
		[DefaultValue(null)]
		public AmmoProjectileProperties ProjectileProperties;
	}
}
