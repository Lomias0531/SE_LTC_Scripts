using System;
using System.Collections.Generic;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlRadialMenuSystem : MyGuiControlRadialMenuBase
	{
		public MyGuiControlRadialMenuSystem(MyRadialMenu data, MyStringId closingControl, Func<bool> handleInputCallback)
			: base(data, closingControl, handleInputCallback)
		{
			SwitchSection(MyGuiControlRadialMenuBase.m_lastSelectedSection.GetValueOrDefault(data.Id, 0));
		}

		protected override void UpdateTooltip()
		{
			List<MyRadialMenuItem> items = m_data.Sections[m_currentSection].Items;
			if (m_selectedButton >= 0 && m_selectedButton < items.Count)
			{
				MyRadialMenuItem myRadialMenuItem = items[m_selectedButton];
				m_tooltip.Text = MyTexts.GetString(myRadialMenuItem.Label);
				m_tooltip.Position = m_icons[m_selectedButton].Position * 2f;
				m_tooltip.Visible = true;
				Vector2 position = m_tooltip.Position;
				int num = (!((double)Math.Abs(position.X) < 0.05)) ? Math.Sign(position.X) : 0;
				int num2 = (!((double)Math.Abs(position.Y) < 0.05)) ? Math.Sign(position.Y) : 0;
				m_tooltip.OriginAlign = (MyGuiDrawAlignEnum)(3 * (-num + 1) + 1 + num2);
			}
			else
			{
				m_tooltip.Visible = false;
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL_MOD1))
			{
				Cancel();
			}
		}
	}
}
