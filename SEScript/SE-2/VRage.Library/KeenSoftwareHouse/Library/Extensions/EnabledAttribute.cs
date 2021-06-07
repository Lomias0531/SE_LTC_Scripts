using System;

namespace KeenSoftwareHouse.Library.Extensions
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class EnabledAttribute : Attribute
	{
		public bool Enabled
		{
			get;
			set;
		}

		public EnabledAttribute(bool enabled = true)
		{
			Enabled = enabled;
		}
	}
}
