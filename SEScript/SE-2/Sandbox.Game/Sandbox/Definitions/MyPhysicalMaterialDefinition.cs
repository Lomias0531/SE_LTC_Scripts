using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PhysicalMaterialDefinition), null)]
	public class MyPhysicalMaterialDefinition : MyDefinitionBase
	{
		public struct CollisionProperty
		{
			public MySoundPair Sound;

			public string ParticleEffect;

			public List<ImpactSounds> ImpactSoundCues;

			public CollisionProperty(string soundCue, string particleEffectName, List<AlternativeImpactSounds> impactsounds)
			{
				Sound = new MySoundPair(soundCue);
				ParticleEffect = particleEffectName;
				if (impactsounds == null || impactsounds.Count == 0)
				{
					ImpactSoundCues = null;
					return;
				}
				ImpactSoundCues = new List<ImpactSounds>();
				foreach (AlternativeImpactSounds impactsound in impactsounds)
				{
					ImpactSoundCues.Add(new ImpactSounds(impactsound.mass, impactsound.soundCue, impactsound.minVelocity, impactsound.maxVolumeVelocity));
				}
			}
		}

		public struct ImpactSounds
		{
			public float Mass;

			public MySoundPair SoundCue;

			public float minVelocity;

			public float maxVolumeVelocity;

			public ImpactSounds(float mass, string soundCue, float minVelocity, float maxVolumeVelocity)
			{
				Mass = mass;
				SoundCue = new MySoundPair(soundCue);
				this.minVelocity = minVelocity;
				this.maxVolumeVelocity = maxVolumeVelocity;
			}
		}

		private class Sandbox_Definitions_MyPhysicalMaterialDefinition_003C_003EActor : IActivator, IActivator<MyPhysicalMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicalMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicalMaterialDefinition CreateInstance()
			{
				return new MyPhysicalMaterialDefinition();
			}

			MyPhysicalMaterialDefinition IActivator<MyPhysicalMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Density;

		public float HorisontalTransmissionMultiplier;

		public float HorisontalFragility;

		public float SupportMultiplier;

		public float CollisionMultiplier;

		public Dictionary<MyStringId, Dictionary<MyStringHash, CollisionProperty>> CollisionProperties = new Dictionary<MyStringId, Dictionary<MyStringHash, CollisionProperty>>(MyStringId.Comparer);

		public Dictionary<MyStringId, MySoundPair> GeneralSounds = new Dictionary<MyStringId, MySoundPair>(MyStringId.Comparer);

		public MyStringHash InheritFrom = MyStringHash.NullOrEmpty;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_PhysicalMaterialDefinition myObjectBuilder_PhysicalMaterialDefinition = builder as MyObjectBuilder_PhysicalMaterialDefinition;
			if (myObjectBuilder_PhysicalMaterialDefinition != null)
			{
				Density = myObjectBuilder_PhysicalMaterialDefinition.Density;
				HorisontalTransmissionMultiplier = myObjectBuilder_PhysicalMaterialDefinition.HorisontalTransmissionMultiplier;
				HorisontalFragility = myObjectBuilder_PhysicalMaterialDefinition.HorisontalFragility;
				SupportMultiplier = myObjectBuilder_PhysicalMaterialDefinition.SupportMultiplier;
				CollisionMultiplier = myObjectBuilder_PhysicalMaterialDefinition.CollisionMultiplier;
			}
			MyObjectBuilder_MaterialPropertiesDefinition myObjectBuilder_MaterialPropertiesDefinition = builder as MyObjectBuilder_MaterialPropertiesDefinition;
			if (myObjectBuilder_MaterialPropertiesDefinition != null)
			{
				InheritFrom = MyStringHash.GetOrCompute(myObjectBuilder_MaterialPropertiesDefinition.InheritFrom);
				foreach (MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty contactProperty in myObjectBuilder_MaterialPropertiesDefinition.ContactProperties)
				{
					MyStringId orCompute = MyStringId.GetOrCompute(contactProperty.Type);
					if (!CollisionProperties.ContainsKey(orCompute))
					{
						CollisionProperties[orCompute] = new Dictionary<MyStringHash, CollisionProperty>(MyStringHash.Comparer);
					}
					MyStringHash orCompute2 = MyStringHash.GetOrCompute(contactProperty.Material);
					CollisionProperties[orCompute][orCompute2] = new CollisionProperty(contactProperty.SoundCue, contactProperty.ParticleEffect, contactProperty.AlternativeImpactSounds);
				}
				foreach (MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty generalProperty in myObjectBuilder_MaterialPropertiesDefinition.GeneralProperties)
				{
					GeneralSounds[MyStringId.GetOrCompute(generalProperty.Type)] = new MySoundPair(generalProperty.SoundCue);
				}
			}
		}
	}
}
