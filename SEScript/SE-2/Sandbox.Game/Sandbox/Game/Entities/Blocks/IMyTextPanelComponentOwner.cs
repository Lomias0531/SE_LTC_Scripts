namespace Sandbox.Game.Entities.Blocks
{
	public interface IMyTextPanelComponentOwner
	{
		MyTextPanelComponent PanelComponent
		{
			get;
		}

		bool IsTextPanelOpen
		{
			get;
		}

		void OpenWindow(bool isEditable, bool sync, bool isPublic);
	}
}
