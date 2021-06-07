using System;

namespace Sandbox.Game.SessionComponents
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public class IngameObjectiveAttribute : Attribute
	{
		private string m_id;

		private int m_priority;

		public string Id => m_id;

		public int Priority => m_priority;

		public IngameObjectiveAttribute(string id, int priority)
		{
			m_id = id;
			m_priority = priority;
		}
	}
}
