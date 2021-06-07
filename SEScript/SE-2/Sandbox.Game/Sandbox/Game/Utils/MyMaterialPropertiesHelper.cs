using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Utils
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyMaterialPropertiesHelper : MySessionComponentBase
	{
		public static class CollisionType
		{
			public static MyStringId Start = MyStringId.GetOrCompute("Start");

			public static MyStringId Hit = MyStringId.GetOrCompute("Hit");

			public static MyStringId Walk = MyStringId.GetOrCompute("Walk");

			public static MyStringId Run = MyStringId.GetOrCompute("Run");

			public static MyStringId Sprint = MyStringId.GetOrCompute("Sprint");
		}

		private struct MaterialProperties
		{
			public MySoundPair Sound;

			public string ParticleEffectName;

			public List<MyPhysicalMaterialDefinition.ImpactSounds> ImpactSoundCues;

			public MaterialProperties(MySoundPair soundCue, string particleEffectName, List<MyPhysicalMaterialDefinition.ImpactSounds> impactSounds)
			{
				Sound = soundCue;
				ParticleEffectName = particleEffectName;
				ImpactSoundCues = impactSounds;
			}
		}

		public static MyMaterialPropertiesHelper Static;

		private Dictionary<MyStringId, Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>>> MaterialDictionary = new Dictionary<MyStringId, Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>>>(MyStringId.Comparer);

		private HashSet<MyStringHash> m_loaded = new HashSet<MyStringHash>(MyStringHash.Comparer);

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			foreach (MyPhysicalMaterialDefinition physicalMaterialDefinition in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
			{
				LoadMaterialProperties(physicalMaterialDefinition);
			}
			foreach (MyPhysicalMaterialDefinition physicalMaterialDefinition2 in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
			{
				LoadMaterialSoundsInheritance(physicalMaterialDefinition2);
			}
		}

		private void LoadMaterialSoundsInheritance(MyPhysicalMaterialDefinition material)
		{
			MyStringHash subtypeId = material.Id.SubtypeId;
			if (!m_loaded.Add(subtypeId) || !(material.InheritFrom != MyStringHash.NullOrEmpty))
			{
				return;
			}
			if (MyDefinitionManager.Static.TryGetDefinition(new MyDefinitionId(typeof(MyObjectBuilder_PhysicalMaterialDefinition), material.InheritFrom), out MyPhysicalMaterialDefinition definition))
			{
				if (!m_loaded.Contains(material.InheritFrom))
				{
					LoadMaterialSoundsInheritance(definition);
				}
				foreach (KeyValuePair<MyStringId, MySoundPair> generalSound in definition.GeneralSounds)
				{
					material.GeneralSounds[generalSound.Key] = generalSound.Value;
				}
			}
			foreach (MyStringId key in MaterialDictionary.Keys)
			{
				if (!MaterialDictionary[key].ContainsKey(subtypeId))
				{
					MaterialDictionary[key][subtypeId] = new Dictionary<MyStringHash, MaterialProperties>(MyStringHash.Comparer);
				}
				MaterialProperties? materialProperties = null;
				if (MaterialDictionary[key].ContainsKey(material.InheritFrom))
				{
					foreach (KeyValuePair<MyStringHash, MaterialProperties> item in MaterialDictionary[key][material.InheritFrom])
					{
						if (item.Key == material.InheritFrom)
						{
							materialProperties = item.Value;
						}
						else if (MaterialDictionary[key][subtypeId].ContainsKey(item.Key))
						{
							if (!MaterialDictionary[key][item.Key].ContainsKey(subtypeId))
							{
								MaterialDictionary[key][item.Key][subtypeId] = item.Value;
							}
						}
						else
						{
							MaterialDictionary[key][subtypeId][item.Key] = item.Value;
							MaterialDictionary[key][item.Key][subtypeId] = item.Value;
						}
					}
					if (materialProperties.HasValue)
					{
						MaterialDictionary[key][subtypeId][subtypeId] = materialProperties.Value;
						MaterialDictionary[key][subtypeId][material.InheritFrom] = materialProperties.Value;
						MaterialDictionary[key][material.InheritFrom][subtypeId] = materialProperties.Value;
					}
				}
			}
		}

		private void LoadMaterialProperties(MyPhysicalMaterialDefinition material)
		{
			MyStringHash subtypeId = material.Id.SubtypeId;
			foreach (KeyValuePair<MyStringId, Dictionary<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty>> collisionProperty in material.CollisionProperties)
			{
				MyStringId key = collisionProperty.Key;
				if (!MaterialDictionary.ContainsKey(key))
				{
					MaterialDictionary[key] = new Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>>(MyStringHash.Comparer);
				}
				if (!MaterialDictionary[key].ContainsKey(subtypeId))
				{
					MaterialDictionary[key][subtypeId] = new Dictionary<MyStringHash, MaterialProperties>(MyStringHash.Comparer);
				}
				foreach (KeyValuePair<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty> item in collisionProperty.Value)
				{
					MaterialDictionary[key][subtypeId][item.Key] = new MaterialProperties(item.Value.Sound, item.Value.ParticleEffect, item.Value.ImpactSoundCues);
					if (!MaterialDictionary[key].ContainsKey(item.Key))
					{
						MaterialDictionary[key][item.Key] = new Dictionary<MyStringHash, MaterialProperties>(MyStringHash.Comparer);
					}
					if (!MaterialDictionary[key][item.Key].ContainsKey(subtypeId))
					{
						MaterialDictionary[key][item.Key][subtypeId] = new MaterialProperties(item.Value.Sound, item.Value.ParticleEffect, item.Value.ImpactSoundCues);
					}
				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public bool TryCreateCollisionEffect(MyStringId type, Vector3D position, Vector3 normal, MyStringHash material1, MyStringHash material2, IMyEntity entity)
		{
			string collisionEffect = GetCollisionEffect(type, material1, material2);
			if (collisionEffect != null)
			{
				MatrixD effectMatrix = MatrixD.CreateWorld(position, normal, Vector3.CalculatePerpendicularVector(normal));
				MyParticleEffect effect;
				if (entity != null && !(entity is MyVoxelBase) && !(entity is MySafeZone))
				{
					MyEntity myEntity = entity as MyEntity;
					effectMatrix *= myEntity.PositionComp.WorldMatrixNormalizedInv;
					return MyParticlesManager.TryCreateParticleEffect(collisionEffect, ref effectMatrix, ref position, myEntity.Render.RenderObjectIDs[0], out effect);
				}
				return MyParticlesManager.TryCreateParticleEffect(collisionEffect, effectMatrix, out effect);
			}
			return false;
		}

		public string GetCollisionEffect(MyStringId type, MyStringHash materialType1, MyStringHash materialType2)
		{
			string result = null;
			if (MaterialDictionary.TryGetValue(type, out Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>> value) && value.TryGetValue(materialType1, out Dictionary<MyStringHash, MaterialProperties> value2) && value2.TryGetValue(materialType2, out MaterialProperties value3))
			{
				result = value3.ParticleEffectName;
			}
			return result;
		}

		public MySoundPair GetCollisionCue(MyStringId type, MyStringHash materialType1, MyStringHash materialType2)
		{
			if (MaterialDictionary.TryGetValue(type, out Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>> value) && value.TryGetValue(materialType1, out Dictionary<MyStringHash, MaterialProperties> value2) && value2.TryGetValue(materialType2, out MaterialProperties value3))
			{
				return value3.Sound;
			}
			return MySoundPair.Empty;
		}

		public MySoundPair GetCollisionCueWithMass(MyStringId type, MyStringHash materialType1, MyStringHash materialType2, ref float volume, float? mass = null, float velocity = 0f)
		{
			if (MaterialDictionary.TryGetValue(type, out Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>> value) && value.TryGetValue(materialType1, out Dictionary<MyStringHash, MaterialProperties> value2) && value2.TryGetValue(materialType2, out MaterialProperties value3))
			{
				if (!mass.HasValue || value3.ImpactSoundCues == null || value3.ImpactSoundCues.Count == 0)
				{
					return value3.Sound;
				}
				int num = -1;
				float num2 = -1f;
				for (int i = 0; i < value3.ImpactSoundCues.Count; i++)
				{
					if (mass >= value3.ImpactSoundCues[i].Mass && value3.ImpactSoundCues[i].Mass > num2 && velocity >= value3.ImpactSoundCues[i].minVelocity)
					{
						num = i;
						num2 = value3.ImpactSoundCues[i].Mass;
					}
				}
				if (num >= 0)
				{
					volume = 0.25f + 0.75f * MyMath.Clamp((velocity - value3.ImpactSoundCues[num].minVelocity) / (value3.ImpactSoundCues[num].maxVolumeVelocity - value3.ImpactSoundCues[num].minVelocity), 0f, 1f);
					return value3.ImpactSoundCues[num].SoundCue;
				}
				return value3.Sound;
			}
			return MySoundPair.Empty;
		}
	}
}
