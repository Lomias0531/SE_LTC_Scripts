using System;
using System.Xml;

namespace VRageRender.Animations
{
	public interface IMyConstProperty
	{
		string Name
		{
			get;
			set;
		}

		string ValueType
		{
			get;
		}

		string BaseValueType
		{
			get;
		}

		bool Animated
		{
			get;
		}

		bool Is2D
		{
			get;
		}

		void Serialize(XmlWriter writer);

		void Deserialize(XmlReader reader);

		void DeserializeFromObjectBuilder(GenerationProperty property);

		void SerializeValue(XmlWriter writer, object value);

		void DeserializeValue(XmlReader reader, out object value);

		void SetValue(object val);

		object GetValue();

		IMyConstProperty Duplicate();

		Type GetValueType();
	}
}
