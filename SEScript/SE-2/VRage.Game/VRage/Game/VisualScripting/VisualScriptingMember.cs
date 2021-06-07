using System;

namespace VRage.Game.VisualScripting
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public class VisualScriptingMember : Attribute
	{
		public readonly bool Sequential;

		public readonly bool Reserved;

		public VisualScriptingMember(bool isSequenceDependent = false, bool reserved = false)
		{
			Sequential = isSequenceDependent;
			Reserved = reserved;
		}
	}
}
