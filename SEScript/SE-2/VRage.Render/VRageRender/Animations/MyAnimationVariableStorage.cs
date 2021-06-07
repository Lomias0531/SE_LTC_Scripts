using System.Collections;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Generics;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRageRender.Animations
{
	/// <summary>
	/// Key-value storage of float values, other types are implicitly converted.
	/// </summary>
	public class MyAnimationVariableStorage : IMyVariableStorage<float>, IEnumerable<KeyValuePair<MyStringId, float>>, IEnumerable
	{
		private readonly Dictionary<MyStringId, float> m_storage = new Dictionary<MyStringId, float>(MyStringId.Comparer);

		private readonly MyRandom m_random = new MyRandom();

		private readonly FastResourceLock m_lock = new FastResourceLock();

		public DictionaryReader<MyStringId, float> AllVariables => m_storage;

		public void SetValue(MyStringId key, float newValue)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_storage[key] = newValue;
			}
		}

		public bool GetValue(MyStringId key, out float value)
		{
			if (key == MyAnimationVariableStorageHints.StrIdRandom)
			{
				value = m_random.NextFloat();
				return true;
			}
			using (m_lock.AcquireSharedUsing())
			{
				return m_storage.TryGetValue(key, out value);
			}
		}

		public void Clear()
		{
			m_storage.Clear();
		}

		public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<MyStringId, float>, Dictionary<MyStringId, float>.Enumerator> GetEnumerator()
		{
			return ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, KeyValuePair<MyStringId, float>, Dictionary<MyStringId, float>.Enumerator>(m_lock.AcquireSharedUsing(), m_storage.GetEnumerator());
		}

		IEnumerator<KeyValuePair<MyStringId, float>> IEnumerable<KeyValuePair<MyStringId, float>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
