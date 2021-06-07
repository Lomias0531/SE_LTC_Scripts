using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Graphics.GUI
{
	public static class MyGuiControlsFactory
	{
		private static MyObjectFactory<MyGuiControlTypeAttribute, MyGuiControlBase> m_objectFactory = new MyObjectFactory<MyGuiControlTypeAttribute, MyGuiControlBase>();

		public static void RegisterDescriptorsFromAssembly(Assembly assembly)
		{
			m_objectFactory.RegisterFromAssembly(assembly);
		}

		public static MyGuiControlBase CreateGuiControl(MyObjectBuilder_Base builder)
		{
			return m_objectFactory.CreateInstance(builder.TypeId);
		}

		public static MyObjectBuilder_GuiControlBase CreateObjectBuilder(MyGuiControlBase control)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_GuiControlBase>(control);
		}
	}
}
