using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty<T> : IMyAnimatedProperty<T>, IMyAnimatedProperty, IMyConstProperty
	{
		public struct ValueHolder
		{
			public T Value;

			public float PrecomputedDiff;

			public float Time;

			public int ID;

			public ValueHolder(int id, float time, T value, float diff)
			{
				ID = id;
				Time = time;
				Value = value;
				PrecomputedDiff = diff;
			}

			public ValueHolder Duplicate()
			{
				ValueHolder result = default(ValueHolder);
				result.Time = Time;
				result.PrecomputedDiff = PrecomputedDiff;
				result.ID = ID;
				if (Value is IMyConstProperty)
				{
					result.Value = (T)((IMyConstProperty)(object)Value).Duplicate();
				}
				else
				{
					result.Value = Value;
				}
				return result;
			}
		}

		private class MyKeysComparer : IComparer<ValueHolder>
		{
			public int Compare(ValueHolder x, ValueHolder y)
			{
				return x.Time.CompareTo(y.Time);
			}
		}

		public delegate void InterpolatorDelegate(ref T previousValue, ref T nextValue, float time, out T value);

		protected List<ValueHolder> m_keys = new List<ValueHolder>();

		public InterpolatorDelegate Interpolator;

		protected string m_name;

		private bool m_interpolateAfterEnd;

		private static MyKeysComparer m_keysComparer = new MyKeysComparer();

		private static int m_globalKeyCounter = 0;

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

		public virtual bool Animated => true;

		public virtual bool Is2D => false;

		public MyAnimatedProperty()
		{
			Init();
		}

		public MyAnimatedProperty(string name, bool interpolateAfterEnd, InterpolatorDelegate interpolator)
			: this()
		{
			m_name = name;
			m_interpolateAfterEnd = interpolateAfterEnd;
			if (interpolator != null)
			{
				Interpolator = interpolator;
			}
		}

		protected virtual void Init()
		{
		}

		public void SetValue(object val)
		{
		}

		public void SetValue(T val)
		{
		}

		object IMyConstProperty.GetValue()
		{
			return null;
		}

		public U GetValue<U>()
		{
			return default(U);
		}

		void IMyAnimatedProperty.GetInterpolatedValue(float time, out object value)
		{
			GetInterpolatedValue(time, out T value2);
			value = value2;
		}

		public void GetInterpolatedValue<U>(float time, out U value) where U : T
		{
			if (m_keys.Count == 0)
			{
				value = default(U);
			}
			else if (m_keys.Count == 1)
			{
				value = (U)(object)m_keys[0].Value;
			}
			else if (time > m_keys[m_keys.Count - 1].Time)
			{
				if (m_interpolateAfterEnd)
				{
					GetPreviousValue(m_keys[m_keys.Count - 1].Time, out T previousValue, out float previousTime);
					GetNextValue(time, out T nextValue, out float _, out float difference);
					if (Interpolator != null)
					{
						Interpolator(ref previousValue, ref nextValue, (time - previousTime) * difference, out T value2);
						value = (U)(object)value2;
					}
					else
					{
						value = default(U);
					}
				}
				else
				{
					value = (U)(object)m_keys[m_keys.Count - 1].Value;
				}
			}
			else
			{
				GetPreviousValue(time, out T previousValue2, out float previousTime2);
				GetNextValue(time, out T nextValue2, out float nextTime2, out float difference2);
				if (nextTime2 == previousTime2)
				{
					value = (U)(object)previousValue2;
				}
				else if (Interpolator != null)
				{
					Interpolator(ref previousValue2, ref nextValue2, (time - previousTime2) * difference2, out T value3);
					value = (U)(object)value3;
				}
				else
				{
					value = default(U);
				}
			}
		}

		public void GetPreviousValue(float time, out T previousValue, out float previousTime)
		{
			previousValue = default(T);
			previousTime = 0f;
			if (m_keys.Count > 0)
			{
				previousTime = m_keys[0].Time;
				previousValue = m_keys[0].Value;
			}
			for (int i = 1; i < m_keys.Count && !(m_keys[i].Time >= time); i++)
			{
				previousTime = m_keys[i].Time;
				previousValue = m_keys[i].Value;
			}
		}

		void IMyAnimatedProperty.GetKey(int index, out float time, out object value)
		{
			GetKey(index, out time, out T value2);
			value = value2;
		}

		void IMyAnimatedProperty.GetKey(int index, out int id, out float time, out object value)
		{
			GetKey(index, out id, out time, out T value2);
			value = value2;
		}

		void IMyAnimatedProperty.GetKeyByID(int id, out float time, out object value)
		{
			GetKeyByID(id, out time, out T value2);
			value = value2;
		}

		void IMyAnimatedProperty.SetKey(int index, float time, object value)
		{
			ValueHolder value2 = m_keys[index];
			value2.Time = time;
			value2.Value = (T)value;
			m_keys[index] = value2;
			UpdateDiff(index - 1);
			UpdateDiff(index);
			UpdateDiff(index + 1);
			m_keys.Sort(m_keysComparer);
		}

		void IMyAnimatedProperty.SetKey(int index, float time)
		{
			ValueHolder value = m_keys[index];
			value.Time = time;
			m_keys[index] = value;
			UpdateDiff(index - 1);
			UpdateDiff(index);
			UpdateDiff(index + 1);
			m_keys.Sort(m_keysComparer);
		}

		void IMyAnimatedProperty.SetKeyByID(int id, float time, object value)
		{
			int num = -1;
			ValueHolder valueHolder = default(ValueHolder);
			for (int i = 0; i < m_keys.Count; i++)
			{
				if (m_keys[i].ID == id)
				{
					valueHolder = m_keys[i];
					num = i;
					break;
				}
			}
			valueHolder.Time = time;
			valueHolder.Value = (T)value;
			if (num == -1)
			{
				valueHolder.ID = id;
				num = m_keys.Count;
				m_keys.Add(valueHolder);
			}
			else
			{
				m_keys[num] = valueHolder;
			}
			UpdateDiff(num - 1);
			UpdateDiff(num);
			UpdateDiff(num + 1);
			m_keys.Sort(m_keysComparer);
		}

		void IMyAnimatedProperty.SetKeyByID(int id, float time)
		{
			ValueHolder valueHolder = m_keys.Find((ValueHolder x) => x.ID == id);
			int num = m_keys.IndexOf(valueHolder);
			valueHolder.Time = time;
			m_keys[num] = valueHolder;
			UpdateDiff(num - 1);
			UpdateDiff(num);
			UpdateDiff(num + 1);
			m_keys.Sort(m_keysComparer);
		}

		public void GetNextValue(float time, out T nextValue, out float nextTime, out float difference)
		{
			nextValue = default(T);
			nextTime = -1f;
			difference = 0f;
			for (int i = 0; i < m_keys.Count; i++)
			{
				nextTime = m_keys[i].Time;
				nextValue = m_keys[i].Value;
				difference = m_keys[i].PrecomputedDiff;
				if (nextTime >= time)
				{
					break;
				}
			}
		}

		public void AddKey(ValueHolder val)
		{
			m_keys.Add(val);
		}

		public int AddKey<U>(float time, U val) where U : T
		{
			ValueHolder item = new ValueHolder(m_globalKeyCounter++, time, (T)(object)val, 0f);
			m_keys.Add(item);
			m_keys.Sort(m_keysComparer);
			int num = 0;
			for (num = 0; num < m_keys.Count && m_keys[num].Time != time; num++)
			{
			}
			if (num > 0)
			{
				UpdateDiff(num);
			}
			return item.ID;
		}

		private void UpdateDiff(int index)
		{
			if (index >= 1 && index < m_keys.Count)
			{
				float time = m_keys[index].Time;
				float time2 = m_keys[index - 1].Time;
				m_keys[index] = new ValueHolder(m_keys[index].ID, time, m_keys[index].Value, 1f / (time - time2));
			}
		}

		int IMyAnimatedProperty.AddKey(float time, object val)
		{
			return AddKey(time, (T)val);
		}

		public void RemoveKey(float time)
		{
			int num = 0;
			while (true)
			{
				if (num < m_keys.Count)
				{
					if (m_keys[num].Time == time)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			RemoveKey(num);
		}

		void IMyAnimatedProperty.RemoveKey(int index)
		{
			RemoveKey(index);
		}

		void IMyAnimatedProperty.RemoveKeyByID(int id)
		{
			ValueHolder item = m_keys.Find((ValueHolder x) => x.ID == id);
			int index = m_keys.IndexOf(item);
			RemoveKey(index);
		}

		private void RemoveKey(int index)
		{
			m_keys.RemoveAt(index);
			UpdateDiff(index);
		}

		public void ClearKeys()
		{
			m_keys.Clear();
		}

		public void GetKey(int index, out float time, out T value)
		{
			time = m_keys[index].Time;
			value = m_keys[index].Value;
		}

		public void GetKey(int index, out int id, out float time, out T value)
		{
			id = m_keys[index].ID;
			time = m_keys[index].Time;
			value = m_keys[index].Value;
		}

		public void GetKeyByID(int id, out float time, out T value)
		{
			ValueHolder valueHolder = m_keys.Find((ValueHolder x) => x.ID == id);
			time = valueHolder.Time;
			value = valueHolder.Value;
		}

		public int GetKeysCount()
		{
			return m_keys.Count;
		}

		public virtual IMyConstProperty Duplicate()
		{
			return null;
		}

		protected virtual void Duplicate(IMyConstProperty targetProp)
		{
			MyAnimatedProperty<T> myAnimatedProperty = targetProp as MyAnimatedProperty<T>;
			myAnimatedProperty.Interpolator = Interpolator;
			myAnimatedProperty.ClearKeys();
			foreach (ValueHolder key in m_keys)
			{
				myAnimatedProperty.AddKey(key.Duplicate());
			}
		}

		Type IMyConstProperty.GetValueType()
		{
			return typeof(T);
		}

		public virtual void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Keys");
			foreach (ValueHolder key in m_keys)
			{
				writer.WriteStartElement("Key");
				float time = key.Time;
				writer.WriteElementString("Time", time.ToString(CultureInfo.InvariantCulture));
				if (Is2D)
				{
					writer.WriteStartElement("Value2D");
				}
				else
				{
					writer.WriteStartElement("Value" + ValueType);
				}
				SerializeValue(writer, key.Value);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public virtual void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			reader.ReadStartElement();
			m_keys.Clear();
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement();
				float time = reader.ReadElementContentAsFloat();
				reader.ReadStartElement();
				DeserializeValue(reader, out object value);
				reader.ReadEndElement();
				AddKey(time, (T)value);
				reader.ReadEndElement();
			}
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
			reader.ReadEndElement();
		}

		public void DeserializeFromObjectBuilder_Animation(Generation2DProperty property, string type)
		{
			DeserializeKeys(property.Keys, type);
		}

		public virtual void DeserializeFromObjectBuilder(GenerationProperty property)
		{
			m_name = property.Name;
			DeserializeKeys(property.Keys, property.Type);
		}

		public void DeserializeKeys(List<AnimationKey> keys, string type)
		{
			m_keys.Clear();
			foreach (AnimationKey key in keys)
			{
				object obj;
				switch (type)
				{
				case "Float":
					obj = key.ValueFloat;
					break;
				case "Vector3":
					obj = key.ValueVector3;
					break;
				case "Vector4":
					obj = key.ValueVector4;
					break;
				default:
					obj = key.ValueInt;
					break;
				case "Bool":
					obj = key.ValueBool;
					break;
				case "MyTransparentMaterial":
					obj = MyTransparentMaterials.GetMaterial(MyStringId.GetOrCompute(key.ValueString));
					break;
				case "String":
					obj = key.ValueString;
					break;
				}
				AddKey(key.Time, (T)obj);
			}
		}

		private void RemoveRedundantKeys()
		{
			int num = 0;
			bool flag = true;
			while (num < m_keys.Count - 1)
			{
				object value = m_keys[num].Value;
				object value2 = m_keys[num + 1].Value;
				bool flag2 = EqualsValues(value, value2);
				if (flag2 && !flag)
				{
					RemoveKey(num);
					continue;
				}
				flag = !flag2;
				num++;
			}
			if (m_keys.Count == 2)
			{
				object value3 = m_keys[0].Value;
				object value4 = m_keys[1].Value;
				if (EqualsValues(value3, value4))
				{
					RemoveKey(num);
				}
			}
		}

		public virtual void SerializeValue(XmlWriter writer, object value)
		{
		}

		public virtual void DeserializeValue(XmlReader reader, out object value)
		{
			value = reader.Value;
			reader.Read();
		}

		protected virtual bool EqualsValues(object value1, object value2)
		{
			return false;
		}
	}
}
