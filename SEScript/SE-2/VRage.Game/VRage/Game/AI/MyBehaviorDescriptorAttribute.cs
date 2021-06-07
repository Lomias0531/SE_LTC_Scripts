using System;

namespace VRage.Game.AI
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MyBehaviorDescriptorAttribute : Attribute
	{
		public readonly string DescriptorCategory;

		public MyBehaviorDescriptorAttribute(string category)
		{
			DescriptorCategory = category;
		}
	}
}
