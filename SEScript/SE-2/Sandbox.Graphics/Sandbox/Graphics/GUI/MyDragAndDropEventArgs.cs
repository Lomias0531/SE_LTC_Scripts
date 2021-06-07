using VRage.Input;

namespace Sandbox.Graphics.GUI
{
	public class MyDragAndDropEventArgs
	{
		public MySharedButtonsEnum DragButton;

		public MyDragAndDropInfo DragFrom
		{
			get;
			set;
		}

		public MyDragAndDropInfo DropTo
		{
			get;
			set;
		}

		public MyGuiGridItem Item
		{
			get;
			set;
		}
	}
}
