using Sandbox.Game.Entities.Character;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyUseTerminalControlHelper : MyAbstractControlMenuItem
	{
		private MyCharacter m_character;

		private string m_label;

		public override string Label => m_label;

		public MyUseTerminalControlHelper()
			: base(MyControlsSpace.TERMINAL)
		{
		}

		public void SetCharacter(MyCharacter character)
		{
			m_character = character;
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			m_character.UseTerminal();
		}

		public void SetLabel(MyStringId id)
		{
			m_label = MyTexts.GetString(id);
		}
	}
}
