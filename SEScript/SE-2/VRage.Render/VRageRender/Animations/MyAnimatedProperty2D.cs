using System.Xml;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2D<T, V, W> : MyAnimatedProperty<T>, IMyAnimatedProperty2D<T, V, W>, IMyAnimatedProperty2D, IMyAnimatedProperty, IMyConstProperty where T : MyAnimatedProperty<V>, new()
	{
		protected MyAnimatedProperty<V>.InterpolatorDelegate m_interpolator2;

		public override bool Is2D => true;

		public MyAnimatedProperty2D()
		{
		}

		public MyAnimatedProperty2D(string name, MyAnimatedProperty<V>.InterpolatorDelegate interpolator)
			: base(name, interpolateAfterEnd: false, (InterpolatorDelegate)null)
		{
			m_interpolator2 = interpolator;
		}

		public X GetInterpolatedValue<X>(float overallTime, float time) where X : V
		{
			GetPreviousValue(overallTime, out T previousValue, out float previousTime);
			GetNextValue(overallTime, out T nextValue, out float _, out float difference);
			previousValue.GetInterpolatedValue(time, out V value);
			nextValue.GetInterpolatedValue(time, out V value2);
			previousValue.Interpolator(ref value, ref value2, (overallTime - previousTime) * difference, out V value3);
			return (X)(object)value3;
		}

		public void GetInterpolatedKeys(float overallTime, float multiplier, IMyAnimatedProperty interpolatedKeys)
		{
			GetInterpolatedKeys(overallTime, default(W), multiplier, interpolatedKeys);
		}

		public void GetInterpolatedKeys(float overallTime, W variance, float multiplier, IMyAnimatedProperty interpolatedKeysOb)
		{
			GetPreviousValue(overallTime, out T previousValue, out float previousTime);
			GetNextValue(overallTime, out T nextValue, out float nextTime, out float difference);
			T val = interpolatedKeysOb as T;
			val.ClearKeys();
			if (previousValue == null)
			{
				return;
			}
			if (m_interpolator2 != null)
			{
				val.Interpolator = m_interpolator2;
			}
			for (int i = 0; i < previousValue.GetKeysCount(); i++)
			{
				previousValue.GetKey(i, out float time, out V _);
				previousValue.GetInterpolatedValue(time, out V value2);
				nextValue.GetInterpolatedValue(time, out V value3);
				V value4 = value2;
				if (nextTime != previousTime)
				{
					val.Interpolator(ref value2, ref value3, (overallTime - previousTime) * difference, out value4);
				}
				ApplyVariance(ref value4, ref variance, multiplier, out value4);
				val.AddKey(time, value4);
			}
		}

		public virtual void ApplyVariance(ref V interpolatedValue, ref W variance, float multiplier, out V value)
		{
			value = default(V);
		}

		public IMyAnimatedProperty CreateEmptyKeys()
		{
			return new T();
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			(value as IMyAnimatedProperty).Serialize(writer);
		}

		public override IMyConstProperty Duplicate()
		{
			return null;
		}

		protected override void Duplicate(IMyConstProperty targetProp)
		{
			MyAnimatedProperty2D<T, V, W> myAnimatedProperty2D = targetProp as MyAnimatedProperty2D<T, V, W>;
			myAnimatedProperty2D.Interpolator = Interpolator;
			myAnimatedProperty2D.m_interpolator2 = m_interpolator2;
			myAnimatedProperty2D.ClearKeys();
			foreach (ValueHolder key in m_keys)
			{
				myAnimatedProperty2D.AddKey(key.Duplicate());
			}
		}

		public override void DeserializeFromObjectBuilder(GenerationProperty property)
		{
			m_name = property.Name;
			m_keys.Clear();
			foreach (AnimationKey key in property.Keys)
			{
				T val = new T();
				val.DeserializeFromObjectBuilder_Animation(key.Value2D, property.Type);
				AddKey(key.Time, val);
			}
		}
	}
}
