using System;
using VRage.Game.Common;

namespace VRage.ObjectBuilders
{
	public class MyObjectBuilderDefinitionAttribute : MyFactoryTagAttribute
	{
		private Type ObsoleteBy;

		public readonly string LegacyName;

		public MyObjectBuilderDefinitionAttribute(Type obsoleteBy = null, string LegacyName = null)
			: base(null)
		{
			ObsoleteBy = obsoleteBy;
			this.LegacyName = LegacyName;
		}
	}
}
