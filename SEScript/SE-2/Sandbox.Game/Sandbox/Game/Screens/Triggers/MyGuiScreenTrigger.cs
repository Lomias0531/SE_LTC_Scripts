using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;
using System;
using System.Globalization;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Triggers
{
	public abstract class MyGuiScreenTrigger : MyGuiScreenBase
	{
		private MyGuiControlLabel m_textboxName;

		protected MyGuiControlTextbox m_textboxMessage;

		private MyGuiControlLabel m_wwwLabel;

		protected MyGuiControlTextbox m_wwwTextbox;

		private MyGuiControlLabel m_nextMisLabel;

		protected MyGuiControlTextbox m_nextMisTextbox;

		protected MyGuiControlButton m_okButton;

		protected MyGuiControlButton m_cancelButton;

		protected MyTrigger m_trigger;

		protected const float VERTICAL_OFFSET = 0.005f;

		protected static readonly Vector2 RESERVED_SIZE = new Vector2(0f, 0.196f);

		protected static readonly Vector2 MIDDLE_PART_ORIGIN = -RESERVED_SIZE / 2f + new Vector2(0f, 0.17f);

		public MyGuiScreenTrigger(MyTrigger trg, Vector2 size)
			: base(null, MyGuiConstants.SCREEN_BACKGROUND_COLOR, size + RESERVED_SIZE)
		{
			size += RESERVED_SIZE;
			Vector2 value = new Vector2
			{
				Y = (0f - size.Y) / 2f + 0.1f
			};
			m_textboxName = new MyGuiControlLabel(value, null, MyTexts.Get(MySpaceTexts.GuiTriggerMessage).ToString());
			value.Y += m_textboxName.Size.Y + 0.005f;
			m_trigger = trg;
			m_textboxMessage = new MyGuiControlTextbox(value, trg.Message, 85);
			m_textboxName.Position -= new Vector2(m_textboxMessage.Size.X / 2f, 0f);
			Controls.Add(m_textboxName);
			Controls.Add(m_textboxMessage);
			value.Y = base.Size.Value.Y * 0.5f - 0.3f;
			m_wwwLabel = new MyGuiControlLabel(value, null, string.Format(MyTexts.GetString(MySpaceTexts.GuiTriggerWwwLink), MySession.GameServiceName));
			value.Y += m_wwwLabel.Size.Y + 0.005f;
			m_wwwTextbox = new MyGuiControlTextbox(value, trg.WwwLink, 300);
			value.Y += m_wwwTextbox.Size.Y + 0.005f;
			m_wwwLabel.Position -= new Vector2(m_wwwTextbox.Size.X / 2f, 0f);
			m_wwwTextbox.TextChanged += OnWwwTextChanged;
			Controls.Add(m_wwwLabel);
			Controls.Add(m_wwwTextbox);
			m_nextMisLabel = new MyGuiControlLabel(value, null, MyTexts.Get(MySpaceTexts.GuiTriggerNextMission).ToString());
			value.Y += m_wwwLabel.Size.Y + 0.005f;
			m_nextMisTextbox = new MyGuiControlTextbox(value, m_trigger.NextMission, 300);
			value.Y += m_wwwTextbox.Size.Y + 0.005f;
			m_nextMisLabel.Position -= new Vector2(m_nextMisTextbox.Size.X / 2f, 0f);
			m_nextMisTextbox.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.GuiTriggerNextMissionTooltip), MySession.GameServiceName));
			Controls.Add(m_nextMisLabel);
			Controls.Add(m_nextMisTextbox);
			Vector2 value2 = new Vector2(0f, base.Size.Value.Y * 0.5f - 0.05f);
			Vector2 value3 = new Vector2(0.01f, 0f);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
			m_okButton.Position = value2 - value3;
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelButtonClick);
			m_cancelButton.Position = value2 + value3;
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			OnWwwTextChanged(m_wwwTextbox);
		}

		private void OnWwwTextChanged(MyGuiControlTextbox source)
		{
			if (source.Text.Length == 0 || MyGuiSandbox.IsUrlWhitelisted(source.Text))
			{
				source.ColorMask = Vector4.One;
				source.SetToolTip((MyToolTips)null);
				m_okButton.Enabled = true;
			}
			else
			{
				MyStringId toolTip = (!MySession.GameServiceName.Equals("Steam")) ? MySpaceTexts.WwwLinkNotAllowed : MySpaceTexts.WwwLinkNotAllowed_Steam;
				m_wwwTextbox.SetToolTip(toolTip);
				source.ColorMask = Color.Red.ToVector4();
				m_okButton.Enabled = false;
			}
		}

		private void OnCancelButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		protected virtual void OnOkButtonClick(MyGuiControlButton sender)
		{
			m_trigger.Message = m_textboxMessage.Text;
			m_trigger.WwwLink = m_wwwTextbox.Text;
			m_trigger.NextMission = m_nextMisTextbox.Text;
			CloseScreen();
		}

		public override bool CloseScreen()
		{
			m_wwwTextbox.TextChanged -= OnWwwTextChanged;
			return base.CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTrigger";
		}

		protected double? StrToDouble(string str)
		{
			double value;
			try
			{
				value = double.Parse(str, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				return null;
			}
			return value;
		}

		protected int? StrToInt(string str)
		{
			int value;
			try
			{
				value = int.Parse(str, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				return null;
			}
			return value;
		}
	}
}
