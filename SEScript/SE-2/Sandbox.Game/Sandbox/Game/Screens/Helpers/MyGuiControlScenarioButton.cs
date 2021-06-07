using Sandbox.Definitions;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlScenarioButton : MyGuiControlRadioButton
	{
		private MyGuiControlLabel m_titleLabel;

		private MyGuiControlImage m_previewImage;

		public string Title => m_titleLabel.Text.ToString();

		public MyScenarioDefinition Scenario
		{
			get;
			private set;
		}

		public MyGuiControlScenarioButton(MyScenarioDefinition scenario)
			: base(null, null, MyDefinitionManager.Static.GetScenarioDefinitions().IndexOf(scenario))
		{
			base.VisualStyle = MyGuiControlRadioButtonStyleEnum.ScenarioButton;
			base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Scenario = scenario;
			m_titleLabel = new MyGuiControlLabel(null, null, scenario.DisplayNameText, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_previewImage = new MyGuiControlImage(null, null, null, null, scenario.Icons, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiSizedTexture myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(229f, 128f)
			};
			m_previewImage.Size = myGuiSizedTexture.SizeGui;
			m_previewImage.BorderEnabled = true;
			m_previewImage.BorderColor = MyGuiConstants.THEMED_GUI_LINE_BORDER.ToVector4();
			SetToolTip(scenario.DescriptionText);
			base.Size = new Vector2(Math.Max(m_titleLabel.Size.X, m_previewImage.Size.X), m_titleLabel.Size.Y + m_previewImage.Size.Y);
			Elements.Add(m_titleLabel);
			Elements.Add(m_previewImage);
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			UpdatePositions();
		}

		private void UpdatePositions()
		{
			m_titleLabel.Position = base.Size * -0.5f;
			m_previewImage.Position = m_titleLabel.Position + new Vector2(0f, m_titleLabel.Size.Y);
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			if (base.HasHighlight)
			{
				m_titleLabel.Font = "White";
				m_previewImage.BorderColor = Vector4.One;
			}
			else
			{
				m_titleLabel.Font = "Blue";
				m_previewImage.BorderColor = MyGuiConstants.THEMED_GUI_LINE_BORDER.ToVector4();
			}
		}
	}
}
