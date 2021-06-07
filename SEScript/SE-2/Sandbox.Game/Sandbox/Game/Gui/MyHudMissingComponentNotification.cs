using Sandbox.Definitions;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyHudMissingComponentNotification : MyHudNotificationBase
	{
		private MyStringId m_originalText;

		public MyHudMissingComponentNotification(MyStringId text, int disapearTimeMs = 2500, string font = "White", MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, int priority = 0, MyNotificationLevel level = MyNotificationLevel.Normal)
			: base(disapearTimeMs, font, textAlign, priority, level)
		{
			m_originalText = text;
		}

		protected override string GetOriginalText()
		{
			return MyTexts.GetString(m_originalText);
		}

		public void SetBlockDefinition(MyCubeBlockDefinition definition)
		{
			SetTextFormatArguments(definition.Components[0].Definition.DisplayNameText.ToString(), definition.DisplayNameText.ToString());
		}

		public override void BeforeAdd()
		{
			MyHud.BlockInfo.MissingComponentIndex = 0;
		}

		public override void BeforeRemove()
		{
			MyHud.BlockInfo.MissingComponentIndex = -1;
		}
	}
}
