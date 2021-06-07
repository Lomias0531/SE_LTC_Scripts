using System;

namespace VRage.Game.VisualScripting
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public class VisualScriptingMiscData : Attribute
	{
		private const int m_cadetBlue = -10510688;

		public readonly string Group;

		public readonly string Comment;

		public readonly int BackgroundColor;

		public VisualScriptingMiscData(string group, string comment = null, int backgroundColor = -10510688)
		{
			Group = group;
			Comment = comment;
			BackgroundColor = backgroundColor;
		}
	}
}
