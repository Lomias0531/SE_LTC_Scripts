using System;

namespace VRage.Game.VisualScripting
{
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
	public class VisualScriptingEvent : Attribute
	{
		public readonly bool[] IsKey;

		public bool HasKeys
		{
			get
			{
				bool[] isKey = IsKey;
				for (int i = 0; i < isKey.Length; i++)
				{
					if (isKey[i])
					{
						return true;
					}
				}
				return false;
			}
		}

		public VisualScriptingEvent(bool firstParam = false)
		{
			IsKey = new bool[1]
			{
				firstParam
			};
		}

		public VisualScriptingEvent(bool[] @params)
		{
			IsKey = @params;
		}
	}
}
