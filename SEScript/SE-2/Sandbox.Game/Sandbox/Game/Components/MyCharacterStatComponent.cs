using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using VRage;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_CharacterStatComponent), true)]
	public class MyCharacterStatComponent : MyEntityStatComponent
	{
		private class Sandbox_Game_Components_MyCharacterStatComponent_003C_003EActor : IActivator, IActivator<MyCharacterStatComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterStatComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterStatComponent CreateInstance()
			{
				return new MyCharacterStatComponent();
			}

			MyCharacterStatComponent IActivator<MyCharacterStatComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static MyStringHash HealthId = MyStringHash.GetOrCompute("Health");

		public MyDamageInformation LastDamage;

		public static readonly float HEALTH_RATIO_CRITICAL = 0.2f;

		public static readonly float HEALTH_RATIO_LOW = 0.4f;

		private MyCharacter m_character;

		public MyEntityStat Health
		{
			get
			{
				if (base.Stats.TryGetValue(HealthId, out MyEntityStat result))
				{
					return result;
				}
				return null;
			}
		}

		public float HealthRatio
		{
			get
			{
				float result = 1f;
				MyEntityStat health = Health;
				if (health != null)
				{
					result = health.Value / health.MaxValue;
				}
				return result;
			}
		}

		public override void Update()
		{
			if (m_character != null && m_character.IsDead)
			{
				foreach (MyEntityStat stat in base.Stats)
				{
					stat.ClearEffects();
				}
				m_scripts.Clear();
			}
			base.Update();
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_character = (base.Container.Entity as MyCharacter);
		}

		public override void OnBeforeRemovedFromContainer()
		{
			m_character = null;
			base.OnBeforeRemovedFromContainer();
		}

		public void OnHealthChanged(float newHealth, float oldHealth, object statChangeData)
		{
			if (m_character != null && m_character.CharacterCanDie)
			{
				m_character.ForceUpdateBreath();
				if (newHealth < oldHealth)
				{
					OnDamage(newHealth, oldHealth);
				}
			}
		}

		private void OnDamage(float newHealth, float oldHealth)
		{
			if (m_character != null && !m_character.IsDead)
			{
				m_character.SoundComp.PlayDamageSound(oldHealth);
				m_character.Render.Damage();
			}
		}

		public void DoDamage(float damage, object statChangeData = null)
		{
			MyEntityStat health = Health;
			if (health != null)
			{
				if (m_character != null)
				{
					m_character.CharacterAccumulatedDamage += damage;
				}
				if (statChangeData is MyDamageInformation)
				{
					LastDamage = (MyDamageInformation)statChangeData;
				}
				health.Decrease(damage, statChangeData);
			}
		}

		public void Consume(MyFixedPoint amount, MyConsumableItemDefinition definition)
		{
			if (definition != null)
			{
				MyObjectBuilder_EntityStatRegenEffect myObjectBuilder_EntityStatRegenEffect = new MyObjectBuilder_EntityStatRegenEffect();
				myObjectBuilder_EntityStatRegenEffect.Interval = 1f;
				myObjectBuilder_EntityStatRegenEffect.MaxRegenRatio = 1f;
				myObjectBuilder_EntityStatRegenEffect.MinRegenRatio = 0f;
				foreach (MyConsumableItemDefinition.StatValue stat in definition.Stats)
				{
					if (base.Stats.TryGetValue(MyStringHash.GetOrCompute(stat.Name), out MyEntityStat result))
					{
						myObjectBuilder_EntityStatRegenEffect.TickAmount = stat.Value * (float)amount;
						myObjectBuilder_EntityStatRegenEffect.Duration = stat.Time;
						result.AddEffect(myObjectBuilder_EntityStatRegenEffect);
					}
				}
			}
		}
	}
}
