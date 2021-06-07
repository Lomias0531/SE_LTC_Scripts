using System.Reflection;
using VRage.Factory;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	internal static class MyRadialMenuItemFactory
	{
		private static MyObjectFactory<MyRadialMenuItemDescriptor, MyRadialMenuItem> m_objectFactory;

		static MyRadialMenuItemFactory()
		{
			m_objectFactory = new MyObjectFactory<MyRadialMenuItemDescriptor, MyRadialMenuItem>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyRadialMenuItem)));
		}

		public static MyRadialMenuItem CreateRadialMenuItem(MyObjectBuilder_RadialMenuItem data)
		{
			MyRadialMenuItem myRadialMenuItem = m_objectFactory.CreateInstance(data.TypeId);
			myRadialMenuItem.Init(data);
			return myRadialMenuItem;
		}
	}
}
