using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders;
using VRage.Network;

namespace Sandbox.Game.Screens.Helpers
{
	[MyDefinitionType(typeof(MyObjectBuilder_RadialMenu), null)]
	public class MyRadialMenu : MyDefinitionBase
	{
		private class Sandbox_Game_Screens_Helpers_MyRadialMenu_003C_003EActor : IActivator, IActivator<MyRadialMenu>
		{
			private sealed override object CreateInstance()
			{
				return new MyRadialMenu();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRadialMenu CreateInstance()
			{
				return new MyRadialMenu();
			}

			MyRadialMenu IActivator<MyRadialMenu>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyRadialMenuSection> Sections;

		public MyRadialMenu()
		{
		}

		public MyRadialMenu(List<MyRadialMenuSection> sections)
		{
			Sections = sections;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadialMenu myObjectBuilder_RadialMenu = builder as MyObjectBuilder_RadialMenu;
			if (myObjectBuilder_RadialMenu != null)
			{
				Sections = new List<MyRadialMenuSection>();
				MyObjectBuilder_RadialMenuSection[] sections = myObjectBuilder_RadialMenu.Sections;
				foreach (MyObjectBuilder_RadialMenuSection builder2 in sections)
				{
					MyRadialMenuSection myRadialMenuSection = new MyRadialMenuSection();
					myRadialMenuSection.Init(builder2);
					Sections.Add(myRadialMenuSection);
				}
			}
		}
	}
}
