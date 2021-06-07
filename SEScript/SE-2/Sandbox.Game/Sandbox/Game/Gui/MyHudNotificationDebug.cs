using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudNotificationDebug : MyHudNotificationBase
	{
		private string m_originalText;

		public MyHudNotificationDebug(string text, int disapearTimeMs = 2500, string font = "White", MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, int priority = 0, MyNotificationLevel level = MyNotificationLevel.Debug)
			: base(disapearTimeMs, font, textAlign, priority, level)
		{
			m_originalText = text;
		}

		protected override string GetOriginalText()
		{
			return m_originalText;
		}
	}
}
