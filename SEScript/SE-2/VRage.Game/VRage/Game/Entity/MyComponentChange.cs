namespace VRage.Game.Entity
{
	public struct MyComponentChange
	{
		private const int OPERATION_REMOVAL = 0;

		private const int OPERATION_ADDITION = 1;

		private const int OPERATION_CHANGE = 2;

		private byte m_operation;

		private MyDefinitionId m_toRemove;

		private MyDefinitionId m_toAdd;

		public int Amount;

		public MyDefinitionId ToRemove
		{
			get
			{
				return m_toRemove;
			}
			set
			{
				m_toRemove = value;
			}
		}

		public MyDefinitionId ToAdd
		{
			get
			{
				return m_toAdd;
			}
			set
			{
				m_toAdd = value;
			}
		}

		public bool IsRemoval()
		{
			return m_operation == 0;
		}

		public bool IsAddition()
		{
			return m_operation == 1;
		}

		public bool IsChange()
		{
			return m_operation == 2;
		}

		public static MyComponentChange CreateRemoval(MyDefinitionId toRemove, int amount)
		{
			MyComponentChange result = default(MyComponentChange);
			result.ToRemove = toRemove;
			result.Amount = amount;
			result.m_operation = 0;
			return result;
		}

		public static MyComponentChange CreateAddition(MyDefinitionId toAdd, int amount)
		{
			MyComponentChange result = default(MyComponentChange);
			result.ToAdd = toAdd;
			result.Amount = amount;
			result.m_operation = 1;
			return result;
		}

		public static MyComponentChange CreateChange(MyDefinitionId toRemove, MyDefinitionId toAdd, int amount)
		{
			MyComponentChange result = default(MyComponentChange);
			result.ToRemove = toRemove;
			result.ToAdd = toAdd;
			result.Amount = amount;
			result.m_operation = 2;
			return result;
		}
	}
}
