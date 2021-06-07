using System;
using VRage.Game;

namespace Sandbox.Game.Screens
{
	public class MyFilterBool : IMyFilterOption
	{
		public bool? Value
		{
			get;
			set;
		}

		public CheckStateEnum CheckValue
		{
			get
			{
				switch (Value)
				{
				case true:
					return CheckStateEnum.Checked;
				case false:
					return CheckStateEnum.Unchecked;
				case null:
					return CheckStateEnum.Indeterminate;
				default:
					throw new InvalidBranchException();
				}
			}
			set
			{
				switch (value)
				{
				case CheckStateEnum.Checked:
					Value = true;
					break;
				case CheckStateEnum.Unchecked:
					Value = false;
					break;
				case CheckStateEnum.Indeterminate:
					Value = null;
					break;
				default:
					throw new ArgumentOutOfRangeException("value", value, null);
				}
			}
		}

		public string SerializedValue
		{
			get
			{
				switch (Value)
				{
				case false:
					return "0";
				case true:
					return "1";
				case null:
					return "2";
				default:
					throw new InvalidBranchException();
				}
			}
		}

		public MyFilterBool(bool? value = null)
		{
			Value = value;
		}

		public void Configure(string value)
		{
			switch (value)
			{
			case "0":
				Value = false;
				break;
			case "1":
				Value = true;
				break;
			case "2":
				Value = null;
				break;
			default:
				throw new InvalidBranchException();
			}
		}

		public bool IsMatch(object value)
		{
			if (Value.HasValue)
			{
				return Value == (bool?)value;
			}
			return true;
		}
	}
}
