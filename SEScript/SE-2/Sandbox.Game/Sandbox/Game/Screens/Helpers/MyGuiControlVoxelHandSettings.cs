using Sandbox.Game.SessionComponents;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlVoxelHandSettings : MyGuiControlParent
	{
		public MyToolbarItemVoxelHand Item
		{
			get;
			set;
		}

		public MyGuiControlVoxelHandSettings()
			: base(null, new Vector2(0.263f, 0.4f))
		{
		}

		public void UpdateControls()
		{
			IMyVoxelBrush myVoxelBrush = null;
			if (Item.Definition.Id.SubtypeName == "Box")
			{
				myVoxelBrush = MyBrushBox.Static;
			}
			else if (Item.Definition.Id.SubtypeName == "Capsule")
			{
				myVoxelBrush = MyBrushCapsule.Static;
			}
			else if (Item.Definition.Id.SubtypeName == "Ramp")
			{
				myVoxelBrush = MyBrushRamp.Static;
			}
			else if (Item.Definition.Id.SubtypeName == "Sphere")
			{
				myVoxelBrush = MyBrushSphere.Static;
			}
			else if (Item.Definition.Id.SubtypeName == "AutoLevel")
			{
				myVoxelBrush = MyBrushAutoLevel.Static;
			}
			if (myVoxelBrush != null)
			{
				Elements.Clear();
				foreach (MyGuiControlBase guiControl in myVoxelBrush.GetGuiControls())
				{
					Elements.Add(guiControl);
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				myGuiControlBase = HandleInputElements();
			}
			return myGuiControlBase;
		}

		internal void UpdateFromBrush(IMyVoxelBrush shape)
		{
			Elements.Clear();
			foreach (MyGuiControlBase guiControl in shape.GetGuiControls())
			{
				Elements.Add(guiControl);
			}
		}
	}
}
