using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemEmpty))]
	internal class MyRadialMenuItemEmpty : MyRadialMenuItem
	{
		public override void Activate(params object[] parameters)
		{
		}
	}
}
