using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenBriefing : MyGuiScreenBase
	{
		public static MyGuiScreenBriefing Static;

		private MyGuiControlLabel m_mainLabel;

		private MyGuiControlMultilineText m_descriptionBox;

		protected MyGuiControlButton m_okButton;

		public string Briefing
		{
			set
			{
				m_descriptionBox.Text = new StringBuilder(value);
			}
		}

		public MyGuiScreenBriefing()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1620f, 1125f) / MyGuiConstants.GUI_OPTIMAL_SIZE)
		{
			Static = this;
			RecreateControls(constructor: true);
			FillData();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyLayoutTable myLayoutTable = new MyLayoutTable(this);
			myLayoutTable.SetColumnWidthsNormalized(50f, 250f, 150f, 250f, 50f);
			myLayoutTable.SetRowHeightsNormalized(50f, 450f, 30f, 50f);
			m_mainLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.GuiScenarioDescription));
			myLayoutTable.AddWithSize(m_mainLabel, MyAlignH.Left, MyAlignV.Center, 0, 1, 1, 3);
			m_descriptionBox = new MyGuiControlMultilineText(new Vector2(0f, 0f), new Vector2(0.2f, 0.2f), null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myLayoutTable.AddWithSize(m_descriptionBox, MyAlignH.Left, MyAlignV.Top, 1, 1, 1, 3);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MyCommonTexts.Ok), size: new Vector2(200f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnOkClicked);
			myLayoutTable.AddWithSize(m_okButton, MyAlignH.Left, MyAlignV.Top, 2, 2);
		}

		private void FillData()
		{
			m_descriptionBox.Text.Clear().Append(MySession.Static.GetWorld().Checkpoint.Briefing).Append(MyEnvironment.NewLine)
				.Append(MyEnvironment.NewLine);
			m_descriptionBox.Text.Append(MyEnvironment.NewLine).Append((object)MySessionComponentMissionTriggers.GetProgress(MySession.Static.LocalHumanPlayer));
			m_descriptionBox.RefreshText(useEnum: false);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenBriefing";
		}

		public override bool Update(bool hasFocus)
		{
			return base.Update(hasFocus);
		}

		protected virtual void OnOkClicked(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		protected override void OnClosed()
		{
			base.OnClosed();
		}
	}
}
