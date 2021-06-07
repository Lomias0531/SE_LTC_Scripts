using Sandbox.Game.SessionComponents;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemVoxelHand))]
	internal class MyRadialMenuItemVoxelHand : MyRadialMenuItem
	{
		public SerializableDefinitionId Material;

		public override void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadialMenuItemVoxelHand myObjectBuilder_RadialMenuItemVoxelHand = (MyObjectBuilder_RadialMenuItemVoxelHand)builder;
			Material = myObjectBuilder_RadialMenuItemVoxelHand.Material;
		}

		public override void Activate(params object[] parameters)
		{
			MySessionComponentVoxelHand.Static.SetMaterial(Material);
		}
	}
}
