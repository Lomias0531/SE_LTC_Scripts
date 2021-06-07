using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenText : MyGuiScreenBase
	{
		public class Style
		{
			public string BackgroundTextureName;

			public string CaptionFont;

			public string TextFont;

			public MyGuiControlButtonStyleEnum ButtonStyle;

			public bool ShowBackgroundPanel;
		}

		private static readonly Style[] m_styles;

		private static Vector2 m_defaultWindowSize;

		private static Vector2 m_defaultDescSize;

		private Vector2 m_windowSize;

		protected Vector2 m_descSize;

		private string m_currentObjectivePrefix = "Current objective: ";

		private StringBuilder m_okButtonCaption;

		private string m_missionTitle = "Mission Title";

		private string m_currentObjective = "";

		protected string m_description = "";

		protected bool m_enableEdit;

		protected MyGuiControlLabel m_titleLabel;

		private MyGuiControlLabel m_currentObjectiveLabel;

		protected MyGuiControlMultilineText m_descriptionBox;

		protected MyGuiControlButton m_okButton;

		protected MyGuiControlCompositePanel m_descriptionBackgroundPanel;

		private Action<ResultEnum> m_resultCallback;

		private ResultEnum m_screenResult = ResultEnum.CANCEL;

		private Style m_style;

		public MyGuiControlMultilineText Description => m_descriptionBox;

		static MyGuiScreenText()
		{
			m_defaultWindowSize = new Vector2(0.6f, 0.7f);
			m_defaultDescSize = new Vector2(0.5f, 0.44f);
			m_styles = new Style[MyUtils.GetMaxValueFromEnum<MyMessageBoxStyleEnum>() + 1];
			m_styles[0] = new Style
			{
				BackgroundTextureName = MyGuiConstants.TEXTURE_SCREEN_BACKGROUND_RED.Texture,
				CaptionFont = "White",
				TextFont = "White",
				ButtonStyle = MyGuiControlButtonStyleEnum.Red,
				ShowBackgroundPanel = false
			};
			m_styles[1] = new Style
			{
				BackgroundTextureName = MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture,
				CaptionFont = "White",
				TextFont = "Blue",
				ButtonStyle = MyGuiControlButtonStyleEnum.Default,
				ShowBackgroundPanel = true
			};
		}

		public MyGuiScreenText(string missionTitle = null, string currentObjectivePrefix = null, string currentObjective = null, string description = null, Action<ResultEnum> resultCallback = null, string okButtonCaption = null, Vector2? windowSize = null, Vector2? descSize = null, bool editEnabled = false, bool canHideOthers = true, bool enableBackgroundFade = false, MyMissionScreenStyleEnum style = MyMissionScreenStyleEnum.BLUE)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, windowSize.HasValue ? windowSize.Value : m_defaultWindowSize)
		{
			m_style = m_styles[(int)style];
			m_enableEdit = editEnabled;
			m_descSize = (descSize.HasValue ? descSize.Value : m_defaultDescSize);
			m_windowSize = (windowSize.HasValue ? windowSize.Value : m_defaultWindowSize);
			m_missionTitle = (missionTitle ?? m_missionTitle);
			m_currentObjectivePrefix = (currentObjectivePrefix ?? m_currentObjectivePrefix);
			m_currentObjective = (currentObjective ?? m_currentObjective);
			m_description = (description ?? m_description);
			m_resultCallback = resultCallback;
			m_okButtonCaption = ((okButtonCaption != null) ? new StringBuilder(okButtonCaption) : MyTexts.Get(MyCommonTexts.Ok));
			m_closeOnEsc = true;
			RecreateControls(constructor: true);
			m_titleLabel.Font = m_style.CaptionFont;
			m_currentObjectiveLabel.Font = m_style.CaptionFont;
			m_descriptionBox.Font = m_style.TextFont;
			m_backgroundTexture = m_style.BackgroundTextureName;
			m_okButton.VisualStyle = m_style.ButtonStyle;
			m_descriptionBackgroundPanel.Visible = m_style.ShowBackgroundPanel;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			base.CanHideOthers = canHideOthers;
			base.EnabledBackgroundFade = enableBackgroundFade;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMission";
		}

		public override void RecreateControls(bool constructor)
		{
			Vector2 value = new Vector2(0f, -0.3f);
			Vector2 descSize = m_descSize;
			Vector2 vector = new Vector2((0f - descSize.X) / 2f, value.Y + 0.1f);
			new Vector2(0.2f, 0.3f);
			new Vector2(0.32f, 0f);
			Vector2 value2 = new Vector2(0.005f, 0f);
			Vector2 value3 = new Vector2(0f, value.Y + 0.05f);
			base.RecreateControls(constructor);
			base.CloseButtonEnabled = true;
			m_okButton = new MyGuiControlButton(new Vector2(0f, 0.29f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, m_okButtonCaption, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OkButtonClicked);
			Controls.Add(m_okButton);
			m_titleLabel = new MyGuiControlLabel(text: m_missionTitle, position: value, size: null, colorMask: null, textScale: 1.5f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			Controls.Add(m_titleLabel);
			m_currentObjectiveLabel = new MyGuiControlLabel(value3, null, null, null, 1f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			Controls.Add(m_currentObjectiveLabel);
			SetCurrentObjective(m_currentObjective);
			m_descriptionBackgroundPanel = AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK, vector, descSize, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_descriptionBox = AddMultilineText(offset: vector + value2, size: descSize);
			m_descriptionBox.Text = new StringBuilder(m_description);
		}

		protected MyGuiControlCompositePanel AddCompositePanel(MyGuiCompositeTexture texture, Vector2 position, Vector2 size, MyGuiDrawAlignEnum panelAlign)
		{
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = texture
			};
			myGuiControlCompositePanel.Position = position;
			myGuiControlCompositePanel.Size = size;
			myGuiControlCompositePanel.OriginAlign = panelAlign;
			Controls.Add(myGuiControlCompositePanel);
			return myGuiControlCompositePanel;
		}

		protected virtual MyGuiControlMultilineText AddMultilineText(Vector2? size = null, Vector2? offset = null, float textScale = 1f, bool selectable = false, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2 vector = size ?? base.Size ?? new Vector2(1.2f, 0.5f);
			MyGuiControlMultilineText myGuiControlMultilineText = null;
			myGuiControlMultilineText = ((!m_enableEdit) ? new MyGuiControlMultilineText(vector / 2f + (offset ?? Vector2.Zero), vector, Color.White.ToVector4(), "White", 0.8f, textAlign, null, drawScrollbarV: true, drawScrollbarH: true, textBoxAlign, null, m_enableEdit) : new MyGuiControlMultilineEditableText(vector / 2f + (offset ?? Vector2.Zero), vector, Color.White.ToVector4(), "White", 0.8f, textAlign, null, drawScrollbarV: true, drawScrollbarH: true, textBoxAlign));
			Controls.Add(myGuiControlMultilineText);
			return myGuiControlMultilineText;
		}

		private void OkButtonClicked(MyGuiControlButton button)
		{
			m_screenResult = ResultEnum.OK;
			CloseScreen();
		}

		public void SetTitle(string title)
		{
			m_missionTitle = title;
			m_titleLabel.Text = title;
		}

		public void SetCurrentObjective(string objective)
		{
			m_currentObjective = objective;
			m_currentObjectiveLabel.Text = m_currentObjectivePrefix + m_currentObjective;
		}

		public void SetDescription(string desc)
		{
			m_description = desc;
			m_descriptionBox.Clear();
			m_descriptionBox.Text = new StringBuilder(m_description);
		}

		public void AppendTextToDescription(string text, Vector4 color, string font = "White", float scale = 1f)
		{
			m_description += text;
			m_descriptionBox.AppendText(text, font, scale, color);
		}

		public void AppendTextToDescription(string text, string font = "White", float scale = 1f)
		{
			m_description += text;
			m_descriptionBox.AppendText(text, font, scale, Vector4.One);
		}

		public void SetCurrentObjectivePrefix(string prefix)
		{
			m_currentObjectivePrefix = prefix;
		}

		public void SetOkButtonCaption(string caption)
		{
			m_okButtonCaption = new StringBuilder(caption);
		}

		protected override void Canceling()
		{
			base.Canceling();
			m_screenResult = ResultEnum.CANCEL;
		}

		public override bool CloseScreen()
		{
			CallResultCallback(m_screenResult);
			return base.CloseScreen();
		}

		protected void CallResultCallback(ResultEnum result)
		{
			if (m_resultCallback != null)
			{
				m_resultCallback(result);
			}
		}
	}
}
