using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using VRage.Network;
using VRageMath;
using VRageRender.Animations;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyParticleSound
	{
		private enum MySoundPropertiesEnum
		{
			Volume,
			VolumeVar,
			Range,
			RangeVar,
			SoundName,
			Enabled
		}

		private class VRage_Game_MyParticleSound_003C_003EActor : IActivator, IActivator<MyParticleSound>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleSound();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleSound CreateInstance()
			{
				return new MyParticleSound();
			}

			MyParticleSound IActivator<MyParticleSound>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int Version = 0;

		private string m_name;

		private MyParticleEffect m_effect;

		private float m_range;

		private float m_volume;

		private Vector3 m_position = Vector3.Zero;

		private uint m_particleSoundId;

		private static uint m_particleSoundIdGlobal = 1u;

		private bool m_newLoop;

		private IMyConstProperty[] m_properties = new IMyConstProperty[Enum.GetValues(typeof(MySoundPropertiesEnum)).Length];

		public float CurrentVolume => m_volume;

		public float CurrentRange => m_range;

		public Vector3 Position => m_position;

		public uint ParticleSoundId => m_particleSoundId;

		public bool NewLoop
		{
			get
			{
				return m_newLoop;
			}
			set
			{
				m_newLoop = value;
			}
		}

		/// <summary>
		/// Public members to easy access
		/// </summary>
		public MyAnimatedPropertyFloat Range
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[2];
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyAnimatedPropertyFloat RangeVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[3];
			}
			private set
			{
				m_properties[3] = value;
			}
		}

		public MyAnimatedPropertyFloat Volume
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[0];
			}
			private set
			{
				m_properties[0] = value;
			}
		}

		public MyAnimatedPropertyFloat VolumeVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[1];
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyConstPropertyString SoundName
		{
			get
			{
				return (MyConstPropertyString)m_properties[4];
			}
			private set
			{
				m_properties[4] = value;
			}
		}

		public MyConstPropertyBool Enabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[5];
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public void Init()
		{
			AddProperty(MySoundPropertiesEnum.Range, new MyAnimatedPropertyFloat("Range"));
			AddProperty(MySoundPropertiesEnum.RangeVar, new MyAnimatedPropertyFloat("Range var"));
			AddProperty(MySoundPropertiesEnum.Volume, new MyAnimatedPropertyFloat("Volume"));
			AddProperty(MySoundPropertiesEnum.VolumeVar, new MyAnimatedPropertyFloat("Volume var"));
			AddProperty(MySoundPropertiesEnum.SoundName, new MyConstPropertyString("Sound name"));
			AddProperty(MySoundPropertiesEnum.Enabled, new MyConstPropertyBool("Enabled"));
			Enabled.SetValue(val: true);
		}

		public void Done()
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				if (m_properties[i] is IMyAnimatedProperty)
				{
					(m_properties[i] as IMyAnimatedProperty).ClearKeys();
				}
			}
			Close();
		}

		public void Start(MyParticleEffect effect)
		{
			m_effect = effect;
			m_name = "ParticleSound";
			m_particleSoundId = m_particleSoundIdGlobal++;
		}

		private void InitSound()
		{
		}

		public void Close()
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				m_properties[i] = null;
			}
			m_effect = null;
			CloseSound();
		}

		private void CloseSound()
		{
		}

		private T AddProperty<T>(MySoundPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public IEnumerable<IMyConstProperty> GetProperties()
		{
			return m_properties;
		}

		public void Update(bool newLoop = false)
		{
			m_newLoop |= newLoop;
			if ((bool)Enabled)
			{
				Range.GetInterpolatedValue(m_effect.GetElapsedTime() / m_effect.Duration, out m_range);
				Volume.GetInterpolatedValue(m_effect.GetElapsedTime() / m_effect.Duration, out m_volume);
				m_position = m_effect.WorldMatrix.Translation;
			}
			_ = (bool)Enabled;
		}

		public MyParticleSound CreateInstance(MyParticleEffect effect)
		{
			MyParticlesManager.SoundsPool.AllocateOrCreate(out MyParticleSound item);
			item.Start(effect);
			item.Name = Name;
			for (int i = 0; i < m_properties.Length; i++)
			{
				item.m_properties[i] = m_properties[i].Duplicate();
			}
			return item;
		}

		public void InitDefault()
		{
			Range.AddKey(0f, 30f);
			Volume.AddKey(0f, 1f);
		}

		public MyParticleSound Duplicate(MyParticleEffect effect)
		{
			return CreateInstance(effect);
		}

		public MyParticleEffect GetEffect()
		{
			return m_effect;
		}

		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ParticleSound");
			writer.WriteAttributeString("Name", Name);
			writer.WriteAttributeString("Version", ((int)Version).ToString(CultureInfo.InvariantCulture));
			writer.WriteStartElement("Properties");
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				writer.WriteStartElement("Property");
				writer.WriteAttributeString("Name", myConstProperty.Name);
				writer.WriteAttributeString("Type", myConstProperty.BaseValueType);
				PropertyAnimationType propertyAnimationType = PropertyAnimationType.Const;
				if (myConstProperty.Animated)
				{
					propertyAnimationType = ((!myConstProperty.Is2D) ? PropertyAnimationType.Animated : PropertyAnimationType.Animated2D);
				}
				writer.WriteAttributeString("AnimationType", propertyAnimationType.ToString());
				myConstProperty.Serialize(writer);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public void DeserializeFromObjectBuilder(ParticleSound sound)
		{
			m_name = sound.Name;
			foreach (GenerationProperty property in sound.Properties)
			{
				for (int i = 0; i < m_properties.Length; i++)
				{
					if (m_properties[i].Name.Equals(property.Name))
					{
						m_properties[i].DeserializeFromObjectBuilder(property);
					}
				}
			}
		}

		public void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			Convert.ToInt32(reader.GetAttribute("version"), CultureInfo.InvariantCulture);
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			for (int i = 0; i < properties.Length; i++)
			{
				properties[i].Deserialize(reader);
			}
			reader.ReadEndElement();
		}

		public void DebugDraw()
		{
		}
	}
}
