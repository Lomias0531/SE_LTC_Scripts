using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemVoxelHandBrush))]
	public class MyRadialMenuItemVoxelHandBrush : MyRadialMenuItem
	{
		private string m_brushSubtypeName;

		public override string Label
		{
			get
			{
				if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
				{
					return base.Label + "\n" + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
				}
				if (!MySession.Static.Settings.EnableVoxelHand)
				{
					return base.Label + "\n" + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_DisabledWorld);
				}
				return base.Label;
			}
			set
			{
				base.Label = value;
			}
		}

		public override bool Enabled()
		{
			if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
			{
				return false;
			}
			if (!MySession.Static.Settings.EnableVoxelHand)
			{
				return false;
			}
			return base.Enabled();
		}

		public override void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadialMenuItemVoxelHandBrush myObjectBuilder_RadialMenuItemVoxelHandBrush = (MyObjectBuilder_RadialMenuItemVoxelHandBrush)builder;
			m_brushSubtypeName = myObjectBuilder_RadialMenuItemVoxelHandBrush.BrushSubtypeName;
		}

		public override void Activate(params object[] parameters)
		{
			bool num = MySession.Static.CreativeMode || MySession.Static.IsUserAdmin(Sync.MyId);
			if (num)
			{
				MySession.Static.GameFocusManager.Clear();
			}
			if (num)
			{
				MySessionComponentVoxelHand.Static.EquipVoxelHand(m_brushSubtypeName);
			}
			if (MySessionComponentVoxelHand.Static.Enabled)
			{
				MySession.Static.ControlledEntity?.SwitchToWeapon(null);
			}
		}
	}
}
