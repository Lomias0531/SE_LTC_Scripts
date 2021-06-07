using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Messages;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyParticleLight
	{
		private enum MyLightPropertiesEnum
		{
			Position,
			PositionVar,
			Color,
			ColorVar,
			Range,
			RangeVar,
			Intensity,
			IntensityVar,
			Enabled,
			GravityDisplacement,
			Falloff,
			VarianceTimeout
		}

		private class VRage_Game_MyParticleLight_003C_003EActor : IActivator, IActivator<MyParticleLight>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleLight CreateInstance()
			{
				return new MyParticleLight();
			}

			MyParticleLight IActivator<MyParticleLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int Version;

		private string m_name;

		private MyParticleEffect m_effect;

		private uint m_renderObjectID = uint.MaxValue;

		private Vector3D m_position;

		private Vector4 m_color;

		private float m_range;

		private float m_intensity;

		private float m_falloff;

		private Vector3 m_localPositionVarRnd;

		private float m_colorVarRnd;

		private float m_lastVarianceTime;

		private float m_rangeVarRnd;

		private float m_intensityRnd;

		private IMyConstProperty[] m_properties = new IMyConstProperty[Enum.GetValues(typeof(MyLightPropertiesEnum)).Length];

		private uint m_parentID;

		/// <summary>
		/// Public members to easy access
		/// </summary>
		public MyAnimatedPropertyVector3 Position
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[0];
			}
			private set
			{
				m_properties[0] = value;
			}
		}

		public MyAnimatedPropertyVector3 PositionVar
		{
			get
			{
				return (MyAnimatedPropertyVector3)m_properties[1];
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyAnimatedPropertyVector4 Color
		{
			get
			{
				return (MyAnimatedPropertyVector4)m_properties[2];
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyAnimatedPropertyFloat ColorVar
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

		public MyAnimatedPropertyFloat Range
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[4];
			}
			private set
			{
				m_properties[4] = value;
			}
		}

		public MyAnimatedPropertyFloat RangeVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[5];
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public MyAnimatedPropertyFloat Intensity
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[6];
			}
			private set
			{
				m_properties[6] = value;
			}
		}

		public MyAnimatedPropertyFloat IntensityVar
		{
			get
			{
				return (MyAnimatedPropertyFloat)m_properties[7];
			}
			private set
			{
				m_properties[7] = value;
			}
		}

		public MyConstPropertyBool Enabled
		{
			get
			{
				return (MyConstPropertyBool)m_properties[8];
			}
			private set
			{
				m_properties[8] = value;
			}
		}

		public MyConstPropertyFloat GravityDisplacement
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[9];
			}
			private set
			{
				m_properties[9] = value;
			}
		}

		public MyConstPropertyFloat Falloff
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[10];
			}
			private set
			{
				m_properties[10] = value;
			}
		}

		public MyConstPropertyFloat VarianceTimeout
		{
			get
			{
				return (MyConstPropertyFloat)m_properties[11];
			}
			private set
			{
				m_properties[11] = value;
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
			AddProperty(MyLightPropertiesEnum.Position, new MyAnimatedPropertyVector3("Position"));
			AddProperty(MyLightPropertiesEnum.PositionVar, new MyAnimatedPropertyVector3("Position var"));
			AddProperty(MyLightPropertiesEnum.Color, new MyAnimatedPropertyVector4("Color"));
			AddProperty(MyLightPropertiesEnum.ColorVar, new MyAnimatedPropertyFloat("Color var"));
			AddProperty(MyLightPropertiesEnum.Range, new MyAnimatedPropertyFloat("Range"));
			AddProperty(MyLightPropertiesEnum.RangeVar, new MyAnimatedPropertyFloat("Range var"));
			AddProperty(MyLightPropertiesEnum.Intensity, new MyAnimatedPropertyFloat("Intensity"));
			AddProperty(MyLightPropertiesEnum.IntensityVar, new MyAnimatedPropertyFloat("Intensity var"));
			AddProperty(MyLightPropertiesEnum.GravityDisplacement, new MyConstPropertyFloat("Gravity Displacement"));
			AddProperty(MyLightPropertiesEnum.Falloff, new MyConstPropertyFloat("Falloff"));
			AddProperty(MyLightPropertiesEnum.VarianceTimeout, new MyConstPropertyFloat("Variance Timeout"));
			AddProperty(MyLightPropertiesEnum.Enabled, new MyConstPropertyBool("Enabled"));
			InitDefault();
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
			m_name = "ParticleLight";
			m_parentID = uint.MaxValue;
		}

		private void InitLight()
		{
			m_renderObjectID = MyRenderProxy.CreateRenderLight("ParticleLight");
		}

		public void Close()
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				m_properties[i] = null;
			}
			m_effect = null;
			CloseLight();
		}

		private void CloseLight()
		{
			if (m_renderObjectID != uint.MaxValue)
			{
				MyRenderProxy.RemoveRenderObject(m_renderObjectID, MyRenderProxy.ObjectType.Light);
				m_renderObjectID = uint.MaxValue;
			}
		}

		private T AddProperty<T>(MyLightPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public IEnumerable<IMyConstProperty> GetProperties()
		{
			return m_properties;
		}

		public void Update()
		{
			bool flag = false;
			if ((bool)Enabled)
			{
				if (m_renderObjectID == uint.MaxValue)
				{
					InitLight();
					flag = true;
				}
				float num = m_effect.GetElapsedTime() - m_lastVarianceTime;
				bool num2 = num > (float)VarianceTimeout || num < 0f;
				if (num2)
				{
					m_lastVarianceTime = m_effect.GetElapsedTime();
				}
				Position.GetInterpolatedValue(m_effect.GetElapsedTime(), out Vector3 value);
				if (num2)
				{
					PositionVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out Vector3 value2);
					m_localPositionVarRnd = new Vector3(MyUtils.GetRandomFloat(0f - value2.X, value2.X), MyUtils.GetRandomFloat(0f - value2.Y, value2.Y), MyUtils.GetRandomFloat(0f - value2.Z, value2.Z));
				}
				value += m_localPositionVarRnd;
				Color.GetInterpolatedValue(m_effect.GetElapsedTime(), out Vector4 value3);
				if (num2)
				{
					ColorVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value4);
					m_colorVarRnd = MyUtils.GetRandomFloat(1f - value4, 1f + value4);
				}
				value3.X = MathHelper.Clamp(value3.X * m_colorVarRnd, 0f, 1f);
				value3.Y = MathHelper.Clamp(value3.Y * m_colorVarRnd, 0f, 1f);
				value3.Z = MathHelper.Clamp(value3.Z * m_colorVarRnd, 0f, 1f);
				Range.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value5);
				if (num2)
				{
					RangeVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value6);
					m_rangeVarRnd = MyUtils.GetRandomFloat(0f - value6, value6);
				}
				value5 += m_rangeVarRnd;
				Intensity.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value7);
				if (num2)
				{
					IntensityVar.GetInterpolatedValue(m_effect.GetElapsedTime(), out float value8);
					m_intensityRnd = MyUtils.GetRandomFloat(0f - value8, value8);
				}
				value7 += m_intensityRnd;
				if (m_effect.IsStopped)
				{
					value7 = 0f;
				}
				Vector3D vector3D = Vector3D.Transform(value * m_effect.GetEmitterScale(), m_effect.WorldMatrix);
				if (m_effect.Gravity.LengthSquared() > 0.0001f)
				{
					Vector3 gravity = m_effect.Gravity;
					gravity.Normalize();
					vector3D += gravity * GravityDisplacement;
				}
				if (m_parentID != m_effect.ParentID)
				{
					m_parentID = m_effect.ParentID;
					MyRenderProxy.SetParentCullObject(m_renderObjectID, m_parentID);
				}
				bool flag2 = m_position != vector3D;
				bool flag3 = m_range != value5;
				bool flag4 = m_falloff != (float)Falloff;
				if (flag2 || m_color != value3 || flag3 || m_intensity != value7 || flag || flag4)
				{
					m_color = value3;
					m_intensity = value7;
					m_range = value5;
					m_position = vector3D;
					m_falloff = Falloff;
					MyLightLayout myLightLayout = default(MyLightLayout);
					myLightLayout.Range = m_range * m_effect.GetEmitterScale();
					myLightLayout.Color = new Vector3(m_color) * m_intensity;
					myLightLayout.Falloff = m_falloff;
					myLightLayout.GlossFactor = 1f;
					myLightLayout.DiffuseFactor = 3.14f;
					MyLightLayout pointLight = myLightLayout;
					MySpotLightLayout mySpotLightLayout = default(MySpotLightLayout);
					mySpotLightLayout.Up = Vector3.Up;
					mySpotLightLayout.Direction = Vector3.Forward;
					MySpotLightLayout spotLight = mySpotLightLayout;
					UpdateRenderLightData updateRenderLightData = default(UpdateRenderLightData);
					updateRenderLightData.ID = m_renderObjectID;
					updateRenderLightData.Position = m_position;
					updateRenderLightData.PointLightOn = true;
					updateRenderLightData.PointLight = pointLight;
					updateRenderLightData.PositionChanged = flag2;
					updateRenderLightData.SpotLight = spotLight;
					updateRenderLightData.AabbChanged = flag3;
					updateRenderLightData.PointIntensity = m_intensity;
					UpdateRenderLightData data = updateRenderLightData;
					MyRenderProxy.UpdateRenderLight(ref data);
				}
			}
			else if (m_renderObjectID != uint.MaxValue)
			{
				CloseLight();
			}
		}

		public MyParticleLight CreateInstance(MyParticleEffect effect)
		{
			MyParticlesManager.LightsPool.AllocateOrCreate(out MyParticleLight item);
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
			Color.AddKey(0f, Vector4.One);
			Range.AddKey(0f, 2.5f);
			Intensity.AddKey(0f, 10f);
			Falloff.SetValue(1f);
			VarianceTimeout.SetValue(0.1f);
			Enabled.SetValue(val: true);
		}

		public MyParticleLight Duplicate(MyParticleEffect effect)
		{
			return CreateInstance(effect);
		}

		public MyParticleEffect GetEffect()
		{
			return m_effect;
		}

		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ParticleLight");
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

		public void DeserializeFromObjectBuilder(ParticleLight light)
		{
			m_name = light.Name;
			foreach (GenerationProperty property in light.Properties)
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
