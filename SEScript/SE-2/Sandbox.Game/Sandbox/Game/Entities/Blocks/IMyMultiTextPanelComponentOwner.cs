using Sandbox.Graphics.GUI;
using System.Collections.Generic;

namespace Sandbox.Game.Entities.Blocks
{
	public interface IMyMultiTextPanelComponentOwner : IMyTextPanelComponentOwner
	{
		MyMultiTextPanelComponent MultiTextPanel
		{
			get;
		}

		void SelectPanel(List<MyGuiControlListbox.Item> selectedItems);
	}
}
