using Sandbox.Game.Entities.Blocks;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentTextPanel : MyRenderComponentScreenAreas
	{
		private class Sandbox_Game_Components_MyRenderComponentTextPanel_003C_003EActor
		{
		}

		private MyTextPanel m_textPanel;

		public MyRenderComponentTextPanel(MyTextPanel textPanel)
			: base(textPanel)
		{
			m_textPanel = textPanel;
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			AddScreenArea(base.RenderObjectIDs, m_textPanel.BlockDefinition.PanelMaterialName);
		}
	}
}
