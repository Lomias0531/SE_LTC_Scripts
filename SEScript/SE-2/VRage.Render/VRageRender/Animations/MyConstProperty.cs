using System;
using System.Xml;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyConstProperty<T> : IMyConstProperty
	{
		private string m_name;

		private T m_value;

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

		public virtual string ValueType => typeof(T).Name;

		public virtual string BaseValueType => ValueType;

		public virtual bool Animated => false;

		public virtual bool Is2D => false;

		public MyConstProperty()
		{
			Init();
		}

		public MyConstProperty(string name)
			: this()
		{
			m_name = name;
		}

		protected virtual void Init()
		{
		}

		object IMyConstProperty.GetValue()
		{
			return m_value;
		}

		public U GetValue<U>() where U : T
		{
			return (U)(object)m_value;
		}

		public virtual void SetValue(object val)
		{
			SetValue((T)val);
		}

		public void SetValue(T val)
		{
			m_value = val;
		}

		public virtual IMyConstProperty Duplicate()
		{
			return null;
		}

		protected virtual void Duplicate(IMyConstProperty targetProp)
		{
			targetProp.SetValue(GetValue<T>());
		}

		Type IMyConstProperty.GetValueType()
		{
			return GetValueTypeInternal();
		}

		protected virtual Type GetValueTypeInternal()
		{
			return typeof(T);
		}

		public virtual void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Value" + ValueType);
			SerializeValue(writer, m_value);
			writer.WriteEndElement();
		}

		public virtual void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			reader.ReadStartElement();
			DeserializeValue(reader, out object value);
			m_value = (T)value;
			reader.ReadEndElement();
		}

		public virtual void DeserializeFromObjectBuilder(GenerationProperty property)
		{
			m_name = property.Name;
			object obj;
			switch (property.Type)
			{
			case "Float":
				obj = property.ValueFloat;
				break;
			case "Vector3":
				obj = property.ValueVector3;
				break;
			case "Vector4":
				obj = property.ValueVector4;
				break;
			default:
				obj = property.ValueInt;
				break;
			case "Bool":
				obj = property.ValueBool;
				break;
			case "String":
				obj = property.ValueString;
				break;
			case "MyTransparentMaterial":
				obj = MyTransparentMaterials.GetMaterial(MyStringId.GetOrCompute(property.ValueString));
				break;
			}
			m_value = (T)obj;
		}

		public virtual void SerializeValue(XmlWriter writer, object value)
		{
		}

		public virtual void DeserializeValue(XmlReader reader, out object value)
		{
			value = reader.Value;
			reader.Read();
		}
	}
}
