using System;

namespace Sandbox.Game.Gui
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MyDebugScreenAttribute : Attribute
	{
		public readonly string Group;

		public readonly string Name;

		public readonly MyDirectXSupport DirectXSupport;

		public MyDebugScreenAttribute(string group, string name, MyDirectXSupport directXSupport)
		{
			Group = group;
			Name = name;
			DirectXSupport = directXSupport;
		}

		public MyDebugScreenAttribute(string group, string name)
		{
			Group = group;
			Name = name;
			DirectXSupport = MyDirectXSupport.DX11;
		}
	}
}
