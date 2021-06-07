using System;

namespace VRage.Input
{
	public class MyInput
	{
		public static IMyInput Static
		{
			get;
			set;
		}

		public static void Initialize(IMyInput implementation)
		{
			if (Static != null)
			{
				throw new InvalidOperationException("Input already initialized.");
			}
			Static = implementation;
		}

		public static void UnloadData()
		{
			if (Static != null)
			{
				Static.UnloadData();
				Static = null;
			}
		}
	}
}
