using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MySuicideControlHelper : MyAbstractControlMenuItem
	{
		private MyCharacter m_character;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CommitSuicide);

		public MySuicideControlHelper()
			: base(MyControlsSpace.SUICIDE)
		{
		}

		public override void Activate()
		{
			m_character.Die();
		}

		public void SetCharacter(MyCharacter character)
		{
			m_character = character;
		}
	}
}
