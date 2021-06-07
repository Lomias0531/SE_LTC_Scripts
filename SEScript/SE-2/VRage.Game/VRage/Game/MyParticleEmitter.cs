using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace VRage.Game
{
	public class MyParticleEmitter
	{
		private enum MyEmitterPropertiesEnum
		{
			Type,
			Offset,
			Rotation,
			AxisScale,
			Size,
			RadiusMin,
			RadiusMax,
			DirToCamera,
			LimitAngle
		}

		private static string[] MyParticleEmitterTypeStrings = new string[6]
		{
			"Point",
			"Line",
			"Box",
			"Sphere",
			"Hemisphere",
			"Circle"
		};

		private static List<string> s_emitterTypeStrings = MyParticleEmitterTypeStrings.ToList();

		private static readonly int Version = 5;

		[ThreadStatic]
		private IMyConstProperty[] m_propertiesInternal;

		private IMyConstProperty[] m_properties
		{
			get
			{
				if (m_propertiesInternal == null)
				{
					m_propertiesInternal = new IMyConstProperty[Enum.GetValues(typeof(MyEmitterPropertiesEnum)).Length];
				}
				return m_propertiesInternal;
			}
		}

		public MyParticleEmitterType Type
		{
			get
			{
				return (MyParticleEmitterType)(int)(m_properties[0] as MyConstPropertyEnum);
			}
			private set
			{
				m_properties[0].SetValue((int)value);
			}
		}

		public MyAnimatedPropertyVector3 Offset
		{
			get
			{
				return m_properties[1] as MyAnimatedPropertyVector3;
			}
			private set
			{
				m_properties[1] = value;
			}
		}

		public MyAnimatedPropertyVector3 Rotation
		{
			get
			{
				return m_properties[2] as MyAnimatedPropertyVector3;
			}
			private set
			{
				m_properties[2] = value;
			}
		}

		public MyConstPropertyVector3 AxisScale
		{
			get
			{
				return m_properties[3] as MyConstPropertyVector3;
			}
			private set
			{
				m_properties[3] = value;
			}
		}

		public MyAnimatedPropertyFloat Size
		{
			get
			{
				return m_properties[4] as MyAnimatedPropertyFloat;
			}
			private set
			{
				m_properties[4] = value;
			}
		}

		public MyConstPropertyFloat RadiusMin
		{
			get
			{
				return m_properties[5] as MyConstPropertyFloat;
			}
			private set
			{
				m_properties[5] = value;
			}
		}

		public MyConstPropertyFloat RadiusMax
		{
			get
			{
				return m_properties[6] as MyConstPropertyFloat;
			}
			private set
			{
				m_properties[6] = value;
			}
		}

		public MyConstPropertyBool DirToCamera
		{
			get
			{
				return m_properties[7] as MyConstPropertyBool;
			}
			private set
			{
				m_properties[7] = value;
			}
		}

		public MyAnimatedPropertyFloat LimitAngle
		{
			get
			{
				return m_properties[8] as MyAnimatedPropertyFloat;
			}
			private set
			{
				m_properties[8] = value;
			}
		}

		public MyParticleEmitter(MyParticleEmitterType type)
		{
		}

		public void Init()
		{
			AddProperty(MyEmitterPropertiesEnum.Type, new MyConstPropertyEnum("Type", typeof(MyParticleEmitterType), s_emitterTypeStrings));
			AddProperty(MyEmitterPropertiesEnum.Offset, new MyAnimatedPropertyVector3("Offset"));
			AddProperty(MyEmitterPropertiesEnum.Rotation, new MyAnimatedPropertyVector3("Rotation", interpolateAfterEnd: true, null));
			AddProperty(MyEmitterPropertiesEnum.AxisScale, new MyConstPropertyVector3("AxisScale"));
			AddProperty(MyEmitterPropertiesEnum.Size, new MyAnimatedPropertyFloat("Size"));
			AddProperty(MyEmitterPropertiesEnum.RadiusMin, new MyConstPropertyFloat("RadiusMin"));
			AddProperty(MyEmitterPropertiesEnum.RadiusMax, new MyConstPropertyFloat("RadiusMax"));
			AddProperty(MyEmitterPropertiesEnum.DirToCamera, new MyConstPropertyBool("DirToCamera"));
			AddProperty(MyEmitterPropertiesEnum.LimitAngle, new MyAnimatedPropertyFloat("LimitAngle"));
			Offset.AddKey(0f, new Vector3(0f, 0f, 0f));
			Rotation.AddKey(0f, new Vector3(0f, 0f, 0f));
			AxisScale.SetValue(Vector3.One);
			Size.AddKey(0f, 1f);
			RadiusMin.SetValue(1f);
			RadiusMax.SetValue(1f);
			DirToCamera.SetValue(val: false);
		}

		public void Done()
		{
			for (int i = 0; i < GetProperties().Length; i++)
			{
				if (m_properties[i] is IMyAnimatedProperty)
				{
					(m_properties[i] as IMyAnimatedProperty).ClearKeys();
				}
			}
			Close();
		}

		public void Start()
		{
		}

		public void Close()
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				m_properties[i] = null;
			}
		}

		private T AddProperty<T>(MyEmitterPropertiesEnum e, T property) where T : IMyConstProperty
		{
			m_properties[(int)e] = property;
			return property;
		}

		public void CalculateStartPosition(float elapsedTime, MatrixD worldMatrix, Vector3 userAxisScale, float userScale, out Vector3D startOffset, out Vector3D startPosition)
		{
			Offset.GetInterpolatedValue(elapsedTime, out Vector3 value);
			Rotation.GetInterpolatedValue(elapsedTime, out Vector3 value2);
			Size.GetInterpolatedValue(elapsedTime, out float value3);
			value3 *= MyUtils.GetRandomFloat(RadiusMin, RadiusMax) * userScale;
			Vector3 value4 = userAxisScale * AxisScale;
			Vector3 position = Vector3.Zero;
			Vector3D.Transform(ref value, ref worldMatrix, out Vector3D result);
			switch (Type)
			{
			case MyParticleEmitterType.Point:
				position = Vector3.Zero;
				break;
			case MyParticleEmitterType.Line:
				position = Vector3.Forward * MyUtils.GetRandomFloat(0f, value3) * value4;
				break;
			case MyParticleEmitterType.Sphere:
				if (LimitAngle.GetKeysCount() > 0)
				{
					LimitAngle.GetInterpolatedValue(elapsedTime, out float value5);
					value5 = MathHelper.ToRadians(value5);
					position = MyUtils.GetRandomVector3MaxAngle(value5) * value3 * value4;
				}
				else
				{
					position = MyUtils.GetRandomVector3Normalized() * value3 * value4;
				}
				break;
			case MyParticleEmitterType.Box:
			{
				float num = value3 * 0.5f;
				position = new Vector3(MyUtils.GetRandomFloat(0f - num, num), MyUtils.GetRandomFloat(0f - num, num), MyUtils.GetRandomFloat(0f - num, num)) * value4;
				break;
			}
			case MyParticleEmitterType.Hemisphere:
				position = MyUtils.GetRandomVector3HemisphereNormalized(Vector3.Forward) * value3 * value4;
				break;
			case MyParticleEmitterType.Circle:
				position = MyUtils.GetRandomVector3CircleNormalized() * value3 * value4;
				break;
			}
			if (value2.LengthSquared() > 0f)
			{
				Matrix matrix = Matrix.CreateRotationX(MathHelper.ToRadians(value2.X));
				matrix *= Matrix.CreateRotationY(MathHelper.ToRadians(value2.Y));
				matrix *= Matrix.CreateRotationZ(MathHelper.ToRadians(value2.Z));
				Vector3.TransformNormal(ref position, ref matrix, out position);
			}
			Vector3D result2;
			if ((bool)DirToCamera)
			{
				if (MyUtils.IsZero(MyTransparentGeometry.Camera.Forward))
				{
					startPosition = Vector3.Zero;
					startOffset = Vector3.Zero;
					return;
				}
				MatrixD matrix2 = worldMatrix * MyTransparentGeometry.CameraView;
				matrix2.Translation += value;
				MatrixD matrixD = matrix2 * MatrixD.Invert(MyTransparentGeometry.CameraView);
				Vector3D dir = MyTransparentGeometry.Camera.Translation - matrixD.Translation;
				dir.Normalize();
				MatrixD matrix3 = MatrixD.CreateFromDir(dir);
				matrix3.Translation = matrixD.Translation;
				Vector3D.Transform(ref position, ref matrix3, out result2);
				startOffset = matrixD.Translation;
				startPosition = result2;
			}
			else
			{
				Vector3D.TransformNormal(ref position, ref worldMatrix, out result2);
				startOffset = result;
				startPosition = result + result2;
			}
		}

		public void CreateInstance(MyParticleEmitter emitter)
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				m_properties[i] = emitter.m_properties[i];
			}
		}

		public IMyConstProperty[] GetProperties()
		{
			return m_properties;
		}

		public void Duplicate(MyParticleEmitter targetEmitter)
		{
			for (int i = 0; i < m_properties.Length; i++)
			{
				targetEmitter.m_properties[i] = m_properties[i].Duplicate();
			}
		}

		public void Serialize(XmlWriter writer)
		{
			writer.WriteElementString("Version", ((int)Version).ToString(CultureInfo.InvariantCulture));
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
		}

		public void Deserialize(XmlReader reader)
		{
			switch (Convert.ToInt32(reader.GetAttribute("version"), CultureInfo.InvariantCulture))
			{
			case 0:
				DeserializeV0(reader);
				return;
			case 1:
				DeserializeV1(reader);
				return;
			case 2:
				DeserializeV2(reader);
				return;
			case 3:
				DeserializeV2(reader);
				return;
			case 4:
				DeserializeV4(reader);
				return;
			}
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			for (int i = 0; i < properties.Length; i++)
			{
				properties[i].Deserialize(reader);
			}
			reader.ReadEndElement();
		}

		public void DeserializeFromObjectBuilder(ParticleEmitter emitter)
		{
			foreach (GenerationProperty property in emitter.Properties)
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

		private void DeserializeV0(XmlReader reader)
		{
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (!(myConstProperty.Name == "Rotation") && !(myConstProperty.Name == "AxisScale"))
				{
					myConstProperty.Deserialize(reader);
				}
			}
			reader.ReadEndElement();
		}

		private void DeserializeV1(XmlReader reader)
		{
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (!(myConstProperty.Name == "AxisScale") && !(myConstProperty.Name == "LimitAngle"))
				{
					myConstProperty.Deserialize(reader);
				}
			}
			reader.ReadEndElement();
		}

		private void DeserializeV2(XmlReader reader)
		{
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (!(myConstProperty.Name == "LimitAngle"))
				{
					myConstProperty.Deserialize(reader);
				}
			}
			if (reader.AttributeCount > 0 && reader.GetAttribute(0) == "LimitAngle")
			{
				reader.Skip();
			}
			reader.ReadEndElement();
		}

		private void DeserializeV4(XmlReader reader)
		{
			reader.ReadStartElement();
			IMyConstProperty[] properties = m_properties;
			foreach (IMyConstProperty myConstProperty in properties)
			{
				if (!(myConstProperty.Name == "LimitAngle"))
				{
					myConstProperty.Deserialize(reader);
				}
			}
			reader.ReadEndElement();
		}

		public void DebugDraw(float elapsedTime, Matrix worldMatrix)
		{
		}
	}
}
