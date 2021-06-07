using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyComponentsDebugInputComponent : MyDebugComponent
	{
		public static List<BoundingBoxD> Boxes = null;

		public static List<MyEntity> DetectedEntities = new List<MyEntity>();

		public MyComponentsDebugInputComponent()
		{
			AddShortcut(MyKeys.G, newPress: true, control: false, shift: false, alt: false, () => "Show components Config Screen.", ShowComponentsConfigScreen);
			AddShortcut(MyKeys.H, newPress: true, control: false, shift: false, alt: false, () => "Show entity spawn screen.", ShowEntitySpawnScreen);
			AddShortcut(MyKeys.J, newPress: true, control: false, shift: false, alt: false, () => "Show defined entites spawn screen.", ShowDefinedEntitySpawnScreen);
		}

		private bool ShowComponentsConfigScreen()
		{
			if (DetectedEntities.Count == 0)
			{
				return false;
			}
			MyGuiSandbox.AddScreen(new MyGuiScreenConfigComponents(DetectedEntities));
			return true;
		}

		public override void Draw()
		{
			base.Draw();
			_ = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
		}

		public override string GetName()
		{
			return "Components config";
		}

		private bool ShowEntitySpawnScreen()
		{
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity != null)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenSpawnEntity(myEntity.WorldMatrix.Translation + myEntity.WorldMatrix.Forward + myEntity.WorldMatrix.Up));
			}
			return true;
		}

		private bool ShowDefinedEntitySpawnScreen()
		{
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity != null)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenSpawnDefinedEntity(myEntity.WorldMatrix.Translation + myEntity.WorldMatrix.Forward + myEntity.WorldMatrix.Up));
			}
			return true;
		}
	}
}
