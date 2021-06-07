using System.Collections;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Generics
{
	/// <summary>
	/// Interface of variable storage (key-value principle).
	/// </summary>
	public interface IMyVariableStorage<T> : IEnumerable<KeyValuePair<MyStringId, T>>, IEnumerable
	{
		void SetValue(MyStringId key, T newValue);

		bool GetValue(MyStringId key, out T value);
	}
}
