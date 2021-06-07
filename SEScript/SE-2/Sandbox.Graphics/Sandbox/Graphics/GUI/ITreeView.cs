namespace Sandbox.Graphics.GUI
{
	internal interface ITreeView
	{
		int GetItemCount();

		MyTreeViewItem GetItem(int index);
	}
}
