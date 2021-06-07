using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlOnOffSwitch))]
	public class MyGuiControlOnOffSwitch : MyGuiControlBase
	{
		private MyGuiControlCheckbox m_onButton;

		private MyGuiControlLabel m_onLabel;

		private MyGuiControlCheckbox m_offButton;

		private MyGuiControlLabel m_offLabel;

		private bool m_value;

		public bool Value
		{
			get
			{
				return m_value;
			}
			set
			{
				if (m_value != value)
				{
					m_value = value;
					UpdateButtonState();
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this);
					}
				}
			}
		}

		public event Action<MyGuiControlOnOffSwitch> ValueChanged;

		public MyGuiControlOnOffSwitch(bool initialValue = false, string onText = null, string offText = null)
			: base(null, null, null, null, null, isActiveControl: true, canHaveFocus: true)
		{
			m_onButton = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.SwitchOnOffLeft, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_offButton = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.SwitchOnOffRight, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_onLabel = new MyGuiControlLabel(new Vector2(m_onButton.Size.X * -0.5f, 0f), null, onText ?? MyTexts.GetString(MySpaceTexts.SwitchText_On), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_offLabel = new MyGuiControlLabel(new Vector2(m_onButton.Size.X * 0.5f, 0f), null, offText ?? MyTexts.GetString(MySpaceTexts.SwitchText_Off), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			base.Size = new Vector2(m_onButton.Size.X + m_offButton.Size.X, Math.Max(m_onButton.Size.Y, m_offButton.Size.Y));
			Elements.Add(m_onButton);
			Elements.Add(m_offButton);
			Elements.Add(m_onLabel);
			Elements.Add(m_offLabel);
			m_value = initialValue;
			UpdateButtonState();
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			base.Size = new Vector2(m_onButton.Size.X + m_offButton.Size.X, Math.Max(m_onButton.Size.Y, m_offButton.Size.Y));
			UpdateButtonState();
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase != null)
			{
				return myGuiControlBase;
			}
			if (base.IsMouseOver || base.HasFocus)
			{
				m_onButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
				m_offButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
				m_onButton.HasHighlight = true;
				m_offButton.HasHighlight = true;
			}
			else
			{
				m_onButton.HighlightType = MyGuiControlHighlightType.WHEN_ACTIVE;
				m_offButton.HighlightType = MyGuiControlHighlightType.WHEN_ACTIVE;
			}
			bool flag = MyInput.Static.IsNewLeftMouseReleased() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT, MyControlStateType.NEW_RELEASED);
			if ((base.Enabled && base.IsMouseOver && flag) || (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))))
			{
				Value = !Value;
				myGuiControlBase = this;
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			return myGuiControlBase;
		}

		private void UpdateButtonState()
		{
			m_onButton.IsChecked = Value;
			m_offButton.IsChecked = !Value;
			m_onLabel.Font = (Value ? "White" : "Blue");
			m_offLabel.Font = (Value ? "Blue" : "White");
		}

		protected override void OnVisibleChanged()
		{
			if (m_onButton != null)
			{
				m_onButton.Visible = base.Visible;
			}
			if (m_offButton != null)
			{
				m_offButton.Visible = base.Visible;
			}
			base.OnVisibleChanged();
		}
	}
}
