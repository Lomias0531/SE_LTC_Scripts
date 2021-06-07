using System.Collections.Generic;
using System.Text;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage.Generics.StateMachine
{
	/// <summary>
	/// Implementation of generic condition. Immutable class, once set, its parameters cant be changed.
	/// </summary>
	public class MyCondition<T> : IMyCondition where T : struct
	{
		public enum MyOperation
		{
			AlwaysFalse,
			AlwaysTrue,
			NotEqual,
			Less,
			LessOrEqual,
			Equal,
			GreaterOrEqual,
			Greater
		}

		private readonly IMyVariableStorage<T> m_storage;

		private readonly MyOperation m_operation;

		private readonly MyStringId m_leftSideStorage;

		private readonly MyStringId m_rightSideStorage;

		private readonly T m_leftSideValue;

		private readonly T m_rightSideValue;

		public MyCondition(IMyVariableStorage<T> storage, MyOperation operation, string leftSideStorage, string rightSideStorage)
		{
			m_storage = storage;
			m_operation = operation;
			m_leftSideStorage = MyStringId.GetOrCompute(leftSideStorage);
			m_rightSideStorage = MyStringId.GetOrCompute(rightSideStorage);
		}

		public MyCondition(IMyVariableStorage<T> storage, MyOperation operation, string leftSideStorage, T rightSideValue)
		{
			m_storage = storage;
			m_operation = operation;
			m_leftSideStorage = MyStringId.GetOrCompute(leftSideStorage);
			m_rightSideStorage = MyStringId.NullOrEmpty;
			m_rightSideValue = rightSideValue;
		}

		public MyCondition(IMyVariableStorage<T> storage, MyOperation operation, T leftSideValue, string rightSideStorage)
		{
			m_storage = storage;
			m_operation = operation;
			m_leftSideStorage = MyStringId.NullOrEmpty;
			m_rightSideStorage = MyStringId.GetOrCompute(rightSideStorage);
			m_leftSideValue = leftSideValue;
		}

		public MyCondition(IMyVariableStorage<T> storage, MyOperation operation, T leftSideValue, T rightSideValue)
		{
			m_storage = storage;
			m_operation = operation;
			m_leftSideStorage = MyStringId.NullOrEmpty;
			m_rightSideStorage = MyStringId.NullOrEmpty;
			m_leftSideValue = leftSideValue;
			m_rightSideValue = rightSideValue;
		}

		public bool Evaluate()
		{
			T value;
			if (m_leftSideStorage != MyStringId.NullOrEmpty)
			{
				if (!m_storage.GetValue(m_leftSideStorage, out value))
				{
					return false;
				}
			}
			else
			{
				value = m_leftSideValue;
			}
			T value2;
			if (m_rightSideStorage != MyStringId.NullOrEmpty)
			{
				if (!m_storage.GetValue(m_rightSideStorage, out value2))
				{
					return false;
				}
			}
			else
			{
				value2 = m_rightSideValue;
			}
			int num = Comparer<T>.Default.Compare(value, value2);
			switch (m_operation)
			{
			case MyOperation.Less:
				return num < 0;
			case MyOperation.LessOrEqual:
				return num <= 0;
			case MyOperation.Equal:
				return num == 0;
			case MyOperation.GreaterOrEqual:
				return num >= 0;
			case MyOperation.Greater:
				return num > 0;
			case MyOperation.NotEqual:
				return num != 0;
			case MyOperation.AlwaysTrue:
				return true;
			case MyOperation.AlwaysFalse:
				return false;
			default:
				return false;
			}
		}

		public override string ToString()
		{
			if (m_operation == MyOperation.AlwaysTrue)
			{
				return "true";
			}
			if (m_operation == MyOperation.AlwaysFalse)
			{
				return "false";
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			if (m_leftSideStorage != MyStringId.NullOrEmpty)
			{
				MyStringId leftSideStorage = m_leftSideStorage;
				stringBuilder.Append(leftSideStorage.ToString());
			}
			else
			{
				stringBuilder.Append(m_leftSideValue);
			}
			stringBuilder.Append(" ");
			switch (m_operation)
			{
			case MyOperation.Less:
				stringBuilder.Append("<");
				break;
			case MyOperation.LessOrEqual:
				stringBuilder.Append("<=");
				break;
			case MyOperation.Equal:
				stringBuilder.Append("==");
				break;
			case MyOperation.GreaterOrEqual:
				stringBuilder.Append(">=");
				break;
			case MyOperation.Greater:
				stringBuilder.Append(">");
				break;
			case MyOperation.NotEqual:
				stringBuilder.Append("!=");
				break;
			default:
				stringBuilder.Append("???");
				break;
			}
			stringBuilder.Append(" ");
			if (m_rightSideStorage != MyStringId.NullOrEmpty)
			{
				MyStringId leftSideStorage = m_rightSideStorage;
				stringBuilder.Append(leftSideStorage.ToString());
			}
			else
			{
				stringBuilder.Append(m_rightSideValue);
			}
			return stringBuilder.ToString();
		}
	}
}
