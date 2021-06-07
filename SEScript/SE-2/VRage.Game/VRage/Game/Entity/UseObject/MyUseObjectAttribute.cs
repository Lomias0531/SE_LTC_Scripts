using System;

namespace VRage.Game.Entity.UseObject
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class MyUseObjectAttribute : Attribute
	{
		public readonly string DummyName;

		public MyUseObjectAttribute(string dummyName)
		{
			DummyName = dummyName;
		}
	}
}
