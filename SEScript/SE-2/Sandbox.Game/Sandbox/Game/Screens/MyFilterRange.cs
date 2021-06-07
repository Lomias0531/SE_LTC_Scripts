using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyFilterRange : IMyFilterOption
	{
		public SerializableRange Value
		{
			get;
			set;
		}

		public bool Active
		{
			get;
			set;
		}

		public string SerializedValue => Value.Min + ":" + Value.Max + ":" + Active.ToString();

		public MyFilterRange()
		{
			Value = default(SerializableRange);
			Active = false;
		}

		public MyFilterRange(SerializableRange value, bool active = false)
		{
			Value = value;
			Active = active;
		}

		public void Configure(string value)
		{
			SerializableRange value2;
			if (string.IsNullOrEmpty(value))
			{
				value2 = (Value = default(SerializableRange));
				return;
			}
			string[] array = value.Split(new char[1]
			{
				':'
			});
			value2 = new SerializableRange
			{
				Min = float.Parse(array[0]),
				Max = float.Parse(array[1])
			};
			Value = value2;
			Active = bool.Parse(array[2]);
		}

		public bool IsMatch(float value)
		{
			if (Active)
			{
				return Value.ValueBetween(value);
			}
			return true;
		}
	}
}
