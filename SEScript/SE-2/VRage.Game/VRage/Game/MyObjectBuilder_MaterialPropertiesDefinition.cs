using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_MaterialPropertiesDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public struct ContactProperty
		{
			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperty_003C_003EType_003C_003EAccessor : IMemberAccessor<ContactProperty, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactProperty owner, in string value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactProperty owner, out string value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperty_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<ContactProperty, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactProperty owner, in string value)
				{
					owner.Material = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactProperty owner, out string value)
				{
					value = owner.Material;
				}
			}

			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperty_003C_003ESoundCue_003C_003EAccessor : IMemberAccessor<ContactProperty, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactProperty owner, in string value)
				{
					owner.SoundCue = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactProperty owner, out string value)
				{
					value = owner.SoundCue;
				}
			}

			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperty_003C_003EParticleEffect_003C_003EAccessor : IMemberAccessor<ContactProperty, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactProperty owner, in string value)
				{
					owner.ParticleEffect = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactProperty owner, out string value)
				{
					value = owner.ParticleEffect;
				}
			}

			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperty_003C_003EAlternativeImpactSounds_003C_003EAccessor : IMemberAccessor<ContactProperty, List<AlternativeImpactSounds>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ContactProperty owner, in List<AlternativeImpactSounds> value)
				{
					owner.AlternativeImpactSounds = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ContactProperty owner, out List<AlternativeImpactSounds> value)
				{
					value = owner.AlternativeImpactSounds;
				}
			}

			private class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperty_003C_003EActor : IActivator, IActivator<ContactProperty>
			{
				private sealed override object CreateInstance()
				{
					return default(ContactProperty);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ContactProperty CreateInstance()
				{
					return (ContactProperty)(object)default(ContactProperty);
				}

				ContactProperty IActivator<ContactProperty>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string Type;

			[ProtoMember(4)]
			public string Material;

			[ProtoMember(7)]
			public string SoundCue;

			[ProtoMember(10)]
			public string ParticleEffect;

			[ProtoMember(13)]
			public List<AlternativeImpactSounds> AlternativeImpactSounds;
		}

		[ProtoContract]
		public struct GeneralProperty
		{
			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EGeneralProperty_003C_003EType_003C_003EAccessor : IMemberAccessor<GeneralProperty, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GeneralProperty owner, in string value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GeneralProperty owner, out string value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EGeneralProperty_003C_003ESoundCue_003C_003EAccessor : IMemberAccessor<GeneralProperty, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GeneralProperty owner, in string value)
				{
					owner.SoundCue = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GeneralProperty owner, out string value)
				{
					value = owner.SoundCue;
				}
			}

			private class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EGeneralProperty_003C_003EActor : IActivator, IActivator<GeneralProperty>
			{
				private sealed override object CreateInstance()
				{
					return default(GeneralProperty);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override GeneralProperty CreateInstance()
				{
					return (GeneralProperty)(object)default(GeneralProperty);
				}

				GeneralProperty IActivator<GeneralProperty>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(16)]
			public string Type;

			[ProtoMember(19)]
			public string SoundCue;
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EContactProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, List<ContactProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in List<ContactProperty> value)
			{
				owner.ContactProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out List<ContactProperty> value)
			{
				value = owner.ContactProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EGeneralProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, List<GeneralProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in List<GeneralProperty> value)
			{
				owner.GeneralProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out List<GeneralProperty> value)
			{
				value = owner.GeneralProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EInheritFrom_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string value)
			{
				owner.InheritFrom = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string value)
			{
				value = owner.InheritFrom;
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_MaterialPropertiesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_MaterialPropertiesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_MaterialPropertiesDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_MaterialPropertiesDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_MaterialPropertiesDefinition CreateInstance()
			{
				return new MyObjectBuilder_MaterialPropertiesDefinition();
			}

			MyObjectBuilder_MaterialPropertiesDefinition IActivator<MyObjectBuilder_MaterialPropertiesDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(22)]
		public List<ContactProperty> ContactProperties;

		[XmlArrayItem("Property")]
		[ProtoMember(25)]
		public List<GeneralProperty> GeneralProperties;

		[ProtoMember(28)]
		public string InheritFrom;
	}
}
