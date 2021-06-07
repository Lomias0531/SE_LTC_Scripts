using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public abstract class MyGuiScreenScenarioBase : MyGuiScreenBase
	{
		protected enum StateEnum
		{
			ListNeedsReload,
			ListLoading,
			ListLoaded
		}

		protected StateEnum m_state;

		protected MyGuiControlTextbox m_nameTextbox;

		protected MyGuiControlTextbox m_descriptionTextbox;

		protected MyGuiControlButton m_okButton;

		protected MyGuiControlButton m_cancelButton;

		protected MyGuiControlTable m_scenarioTable;

		protected MyGuiControlMultilineText m_descriptionBox;

		protected MyLayoutTable m_sideMenuLayout;

		protected MyLayoutTable m_buttonsLayout;

		protected int m_selectedRow;

		protected const float MARGIN_TOP = 0.1f;

		protected const float MARGIN_LEFT = 0.42f;

		protected const string WORKSHOP_PATH_TAG = "workshop";

		private List<Tuple<string, MyWorldInfo>> m_availableSaves = new List<Tuple<string, MyWorldInfo>>();

		protected abstract MyStringId ScreenCaption
		{
			get;
		}

		protected abstract bool IsOnlineMode
		{
			get;
		}

		public MyGuiScreenScenarioBase()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, CalcSize(null))
		{
		}

		private static Vector2 CalcSize(MyObjectBuilder_Checkpoint checkpoint)
		{
			float x = (checkpoint == null) ? 0.9f : 0.65f;
			float num = (checkpoint == null) ? 1.24f : 0.97f;
			if (checkpoint != null)
			{
				num -= 0.05f;
			}
			num -= 0.27f;
			return new Vector2(x, num);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			BuildControls();
			SetDefaultValues();
		}

		protected virtual void BuildControls()
		{
			AddCaption(ScreenCaption);
			MyGuiControlLabel control = MakeLabel(MyCommonTexts.Name);
			MyGuiControlLabel control2 = MakeLabel(MyCommonTexts.Description);
			m_nameTextbox = new MyGuiControlTextbox(null, null, 128);
			m_nameTextbox.Enabled = false;
			m_descriptionTextbox = new MyGuiControlTextbox(null, null, 7999);
			m_descriptionTextbox.Enabled = false;
			new Vector2(0f, 0.052f);
			Vector2 vector = -m_size.Value / 2f + new Vector2(0.42f, 0.1f);
			Vector2 size = m_size.Value / 2f - vector;
			float num = size.X * 0.25f;
			float num2 = size.X - num;
			float num3 = 0.052f;
			size.Y = num3 * 5f;
			m_sideMenuLayout = new MyLayoutTable(this, vector, size);
			m_sideMenuLayout.SetColumnWidthsNormalized(num, num2);
			m_sideMenuLayout.SetRowHeightsNormalized(num3, num3, num3, num3, num3);
			m_sideMenuLayout.Add(control, MyAlignH.Left, MyAlignV.Top, 0, 0);
			m_sideMenuLayout.Add(m_nameTextbox, MyAlignH.Left, MyAlignV.Top, 0, 1);
			m_sideMenuLayout.Add(control2, MyAlignH.Left, MyAlignV.Top, 1, 0);
			m_sideMenuLayout.Add(m_descriptionTextbox, MyAlignH.Left, MyAlignV.Top, 1, 1);
			MyGuiControlPanel control3 = new MyGuiControlPanel
			{
				Name = "BriefingPanel",
				Position = new Vector2(-0.02f, -0.12f),
				Size = new Vector2(0.43f, 0.422f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST
			};
			Controls.Add(control3);
			m_descriptionBox = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, selectable: false, showTextShadow: false, null, null)
			{
				Name = "BriefingMultilineText",
				Position = new Vector2(-0.009f, -0.115f),
				Size = new Vector2(0.419f, 0.412f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			Controls.Add(m_descriptionBox);
			int num4 = 2;
			int num5 = 4;
			Vector2 vector2 = new Vector2(0.1875f, 7f / 120f);
			Vector2 topLeft = m_size.Value / 2f - new Vector2(0.83f, 0.16f);
			Vector2 vector3 = new Vector2(0.01f, 0.01f);
			Vector2 size2 = new Vector2((vector2.X + vector3.X) * (float)num5, (vector2.Y + vector3.Y) * (float)num4);
			m_buttonsLayout = new MyLayoutTable(this, topLeft, size2);
			float[] columnWidthsNormalized = Enumerable.Repeat(vector2.X + vector3.X, num5).ToArray();
			m_buttonsLayout.SetColumnWidthsNormalized(columnWidthsNormalized);
			float[] rowHeightsNormalized = Enumerable.Repeat(vector2.Y + vector3.Y, num4).ToArray();
			m_buttonsLayout.SetRowHeightsNormalized(rowHeightsNormalized);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelButtonClick);
			m_buttonsLayout.Add(m_okButton, MyAlignH.Left, MyAlignV.Top, 1, 2);
			m_buttonsLayout.Add(m_cancelButton, MyAlignH.Left, MyAlignV.Top, 1, 3);
			m_scenarioTable = CreateScenarioTable();
			Controls.Add(m_scenarioTable);
		}

		protected virtual MyGuiControlTable CreateScenarioTable()
		{
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable();
			myGuiControlTable.Position = new Vector2(-0.42f, -0.4f);
			myGuiControlTable.Size = new Vector2(0.38f, 1.8f);
			myGuiControlTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlTable.VisibleRowsCount = 20;
			myGuiControlTable.ColumnsCount = 2;
			myGuiControlTable.SetCustomColumnWidths(new float[2]
			{
				0.085f,
				0.905f
			});
			myGuiControlTable.SetColumnName(1, MyTexts.Get(MyCommonTexts.Name));
			myGuiControlTable.ItemSelected += OnTableItemSelected;
			return myGuiControlTable;
		}

		protected MyGuiControlLabel MakeLabel(MyStringId textEnum)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(textEnum));
		}

		protected virtual void SetDefaultValues()
		{
			FillRight();
		}

		protected void OnOkButtonClick(object sender)
		{
			if (m_nameTextbox.Text.Length < 5 || m_nameTextbox.Text.Length > 128)
			{
				MyStringId id = (m_nameTextbox.Text.Length >= 5) ? MyCommonTexts.ErrorNameTooLong : MyCommonTexts.ErrorNameTooShort;
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(id), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox.SkipTransition = true;
				myGuiScreenMessageBox.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
			}
			else if (m_descriptionTextbox.Text.Length > 7999)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ErrorDescriptionTooLong), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox2.SkipTransition = true;
				myGuiScreenMessageBox2.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
			}
			else
			{
				CloseScreen();
				LoadSandbox(IsOnlineMode);
			}
		}

		private void OnCancelButtonClick(object sender)
		{
			CloseScreen();
		}

		protected virtual void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			m_selectedRow = eventArgs.RowIndex;
			FillRight();
		}

		public override bool Update(bool hasFocus)
		{
			if (m_state == StateEnum.ListNeedsReload)
			{
				FillList();
			}
			if (m_scenarioTable.SelectedRow != null)
			{
				m_okButton.Enabled = true;
			}
			else
			{
				m_okButton.Enabled = false;
			}
			return base.Update(hasFocus);
		}

		public override bool Draw()
		{
			if (m_state != StateEnum.ListLoaded)
			{
				return false;
			}
			return base.Draw();
		}

		protected override void OnShow()
		{
			base.OnShow();
			if (m_state == StateEnum.ListNeedsReload)
			{
				FillList();
			}
		}

		protected void FillRight()
		{
			if (m_scenarioTable == null || m_scenarioTable.SelectedRow == null)
			{
				m_nameTextbox.SetText(new StringBuilder(""));
				m_descriptionTextbox.SetText(new StringBuilder(""));
				return;
			}
			Tuple<string, MyWorldInfo> tuple = FindSave(m_scenarioTable.SelectedRow);
			m_nameTextbox.SetText(new StringBuilder(MyTexts.GetString(tuple.Item2.SessionName)));
			m_descriptionTextbox.SetText(new StringBuilder(tuple.Item2.Description));
			m_descriptionBox.Text = new StringBuilder(MyTexts.GetString(tuple.Item2.Briefing));
		}

		protected virtual void FillList()
		{
			m_state = StateEnum.ListLoading;
		}

		protected void AddSave(Tuple<string, MyWorldInfo> save)
		{
			m_availableSaves.Add(save);
		}

		protected void AddSaves(List<Tuple<string, MyWorldInfo>> saves)
		{
			m_availableSaves.AddRange(saves);
		}

		protected void ClearSaves()
		{
			m_availableSaves.Clear();
		}

		protected void RefreshGameList()
		{
			int num = m_scenarioTable.SelectedRowIndex ?? (-1);
			m_scenarioTable.Clear();
			Color? color = null;
			for (int i = 0; i < m_availableSaves.Count; i++)
			{
				StringBuilder stringBuilder = new StringBuilder(m_availableSaves[i].Item2.SessionName);
				MyGuiControlTable.Row row = new MyGuiControlTable.Row(m_availableSaves[i]);
				row.AddCell(new MyGuiControlTable.Cell(string.Empty, null, null, color, GetIcon(m_availableSaves[i])));
				Color? textColor = color;
				row.AddCell(new MyGuiControlTable.Cell(stringBuilder, stringBuilder, null, textColor));
				m_scenarioTable.Add(row);
				if (i == num)
				{
					m_selectedRow = i;
					m_scenarioTable.SelectedRow = row;
				}
			}
			m_scenarioTable.SelectedRowIndex = m_selectedRow;
			m_scenarioTable.ScrollToSelection();
			FillRight();
		}

		protected Tuple<string, MyWorldInfo> FindSave(MyGuiControlTable.Row row)
		{
			return (Tuple<string, MyWorldInfo>)row.UserData;
		}

		protected virtual MyGuiHighlightTexture GetIcon(Tuple<string, MyWorldInfo> save)
		{
			return MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL;
		}

		private void LoadSandbox(bool MP)
		{
			MyLog.Default.WriteLine("LoadSandbox() - Start");
			MyGuiControlTable.Row selectedRow = m_scenarioTable.SelectedRow;
			if (selectedRow != null)
			{
				Tuple<string, MyWorldInfo> tuple = FindSave(selectedRow);
				if (tuple != null)
				{
					LoadSandboxInternal(tuple, MP);
				}
			}
			MyLog.Default.WriteLine("LoadSandbox() - End");
		}

		protected virtual void LoadSandboxInternal(Tuple<string, MyWorldInfo> save, bool MP)
		{
		}
	}
}
