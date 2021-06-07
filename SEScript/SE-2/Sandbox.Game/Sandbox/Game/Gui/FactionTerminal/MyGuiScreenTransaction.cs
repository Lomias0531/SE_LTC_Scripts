using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui.FactionTerminal
{
	public class MyGuiScreenTransaction : MyGuiScreenBase
	{
		private MyTransactionType m_transactionType;

		private long m_currentBalance;

		private long m_finalBalance;

		private long m_fromIdentifier;

		private long m_toIdentifier;

		private bool m_invalidInput;

		private MyGuiControlLabel m_labelFinalBalanceValue;

		private MyGuiControlButton m_btnOk;

		private MyGuiControlTextbox m_textboxAmount;

		public override string GetFriendlyName()
		{
			return "Transaction Form";
		}

		public MyGuiScreenTransaction(MyTransactionType transactionType, long fromIdentifier, long toIdentifier)
		{
			m_transactionType = transactionType;
			base.BackgroundColor = MyGuiConstants.SCREEN_BACKGROUND_COLOR;
			base.CanHideOthers = false;
			MyBankingSystem.Static.TryGetAccountInfo(fromIdentifier, out MyAccountInfo account);
			m_currentBalance = account.Balance;
			m_finalBalance = account.Balance;
			m_fromIdentifier = fromIdentifier;
			m_toIdentifier = toIdentifier;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.Size = new Vector2(0.5f, 0.712f);
			Vector2 value = new Vector2((0f - m_size.Value.X) * 0.5f, (0f - m_size.Value.Y) * 0.5f);
			Vector2 value2 = new Vector2((0f - m_size.Value.X) * 0.5f, m_size.Value.Y * 0.5f);
			Vector2 value3 = new Vector2(m_size.Value.X * 0.5f, m_size.Value.Y * 0.5f);
			new Vector2(m_size.Value.X * 0.5f, (0f - m_size.Value.Y) * 0.5f);
			base.CloseButtonEnabled = true;
			base.CloseButtonStyle = MyGuiControlButtonStyleEnum.Close;
			if (m_transactionType == MyTransactionType.Withdraw)
			{
				AddCaption(MySpaceTexts.FactionTerminal_Withdraw_Currency, null, new Vector2(0f, -0.01f));
			}
			else if (m_transactionType == MyTransactionType.Deposit)
			{
				AddCaption(MySpaceTexts.FactionTerminal_Deposit_Currency, null, new Vector2(0f, -0.01f));
			}
			float num = 0.05f;
			float num2 = 0.02f;
			Vector2 vector = value + new Vector2(num, 0.062f);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(vector, m_size.Value.X - 2f * num);
			Controls.Add(myGuiControlSeparatorList);
			Vector2 size = new Vector2(0.13f, 0.04f);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(value2 + new Vector2(num, 0f - size.Y - 2f * num2), m_size.Value.X - 2f * num);
			Controls.Add(myGuiControlSeparatorList2);
			float num3 = -0.002f;
			m_btnOk = new MyGuiControlButton
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
				Position = value3 + new Vector2(0f - num - num3, 0f - num2),
				VisualStyle = MyGuiControlButtonStyleEnum.Rectangular,
				Size = size,
				TextEnum = MySpaceTexts.Transaction_Form_Ok_Btn
			};
			m_btnOk.SetTooltip(MyTexts.GetString(MySpaceTexts.Transaction_Form_Ok_Btn_TTIP));
			m_btnOk.ButtonClicked += OnButtonOkPressed;
			Controls.Add(m_btnOk);
			float num4 = -0.002f;
			float num5 = 0.0018f;
			Vector2 value4 = new Vector2(0f, 0.002f);
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			string texture = (icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
			Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(1f));
			float num6 = screenSizeFromNormalizedSize.X / screenSizeFromNormalizedSize.Y;
			Vector2 size2 = new Vector2(0.018f, num6 * 0.018f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(num3 + num5, 2.2f * num2),
				TextEnum = ((m_transactionType == MyTransactionType.Withdraw) ? MySpaceTexts.Transaction_Form_FactionBalance : MySpaceTexts.Transaction_Form_PersonalBalance),
				AutoEllipsis = false
			};
			Controls.Add(myGuiControlLabel);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Position = new Vector2(value3.X - num - num3 + num4, myGuiControlLabel.PositionY) + value4,
				Size = size2,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			myGuiControlImage.SetTexture(texture);
			Controls.Add(myGuiControlImage);
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(myGuiControlImage.PositionX - myGuiControlImage.Size.X * 1.2f, myGuiControlLabel.PositionY),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Text = MyBankingSystem.GetFormatedValue(m_currentBalance),
				AutoEllipsis = false,
				ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
			};
			Controls.Add(control);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel
			{
				Position = myGuiControlLabel.Position + new Vector2(0f, 2f * num2),
				TextEnum = MySpaceTexts.Transaction_Form_Amount,
				AutoEllipsis = false
			};
			Controls.Add(myGuiControlLabel2);
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
			{
				Position = new Vector2(value3.X - num - num3 + num4, myGuiControlLabel2.PositionY) + value4,
				Size = size2,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			myGuiControlImage2.SetTexture(texture);
			Controls.Add(myGuiControlImage2);
			Color cOLOR_CUSTOM_GREY = MyTerminalFactionController.COLOR_CUSTOM_GREY;
			m_textboxAmount = new MyGuiControlTextbox(null, null, 512, cOLOR_CUSTOM_GREY, 0.8f, MyGuiControlTextboxType.DigitsOnly)
			{
				Position = new Vector2(myGuiControlImage2.PositionX - myGuiControlImage2.Size.X * 1.2f, myGuiControlLabel2.PositionY),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				MaxLength = 19,
				TextAlignment = TextAlingmentMode.Right
			};
			m_textboxAmount.Size = new Vector2(0.2f, m_textboxAmount.Size.Y);
			m_textboxAmount.ColorMask = new Vector4(m_textboxAmount.ColorMask.X, m_textboxAmount.ColorMask.Y, m_textboxAmount.ColorMask.Z, 0.5f);
			m_textboxAmount.TextChanged += OnAmountChanged;
			m_textboxAmount.HighlightType = MyGuiControlHighlightType.NEVER;
			Controls.Add(m_textboxAmount);
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel
			{
				Position = myGuiControlLabel2.Position + new Vector2(0f, 2f * num2),
				TextEnum = MySpaceTexts.Transaction_Form_FinalBalance,
				AutoEllipsis = false
			};
			Controls.Add(myGuiControlLabel3);
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage
			{
				Position = new Vector2(value3.X - num - num3 + num4, myGuiControlLabel3.PositionY) + value4,
				Size = size2,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			myGuiControlImage3.SetTexture(texture);
			Controls.Add(myGuiControlImage3);
			m_labelFinalBalanceValue = new MyGuiControlLabel
			{
				Position = new Vector2(myGuiControlImage3.PositionX - myGuiControlImage3.Size.X * 1.2f, myGuiControlLabel3.PositionY),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Text = MyBankingSystem.GetFormatedValue((m_finalBalance > 0) ? m_finalBalance : 0),
				AutoEllipsis = false,
				ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
			};
			Controls.Add(m_labelFinalBalanceValue);
			Vector2 vector2 = new Vector2(vector.X, myGuiControlImage3.PositionY + 3f * num2);
			float num7 = m_size.Value.X - 2f * num;
			myGuiControlSeparatorList.AddHorizontal(vector2, num7);
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = vector2 + new Vector2(num7 * 0.5f, 0f - num2),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Text = MyTexts.GetString(MySpaceTexts.Transaction_Form_ActivityLogLabel),
				Font = "ScreenCaption"
			};
			Controls.Add(control2);
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable
			{
				Position = vector2 + new Vector2(0f, num2),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = new Vector2(num7, 0.35f),
				VisualStyle = MyGuiControlTableStyleEnum.Default
			};
			myGuiControlTable.ColumnsCount = 3;
			myGuiControlTable.SetCustomColumnWidths(new float[3]
			{
				0.38f,
				0.27f,
				0.3f
			});
			myGuiControlTable.VisibleRowsCount = 9;
			myGuiControlTable.ColumnLinesVisible = true;
			myGuiControlTable.RowLinesVisible = true;
			myGuiControlTable.SetColumnName(0, MyTexts.Get(MySpaceTexts.Transaction_Form_Log_DateHeader));
			myGuiControlTable.SetColumnName(1, MyTexts.Get(MySpaceTexts.Transaction_Form_Log_NameHeader));
			myGuiControlTable.SetColumnName(2, MyTexts.Get(MySpaceTexts.Transaction_Form_Log_AmountHeader));
			myGuiControlTable.SetColumnAlign(2, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			myGuiControlTable.SetHeaderColumnMargin(0, new Thickness(0.01f));
			myGuiControlTable.SetHeaderColumnMargin(1, new Thickness(0.01f));
			myGuiControlTable.SetHeaderColumnMargin(2, new Thickness(0.01f));
			UpdateActivityLog(myGuiControlTable);
			Controls.Add(myGuiControlTable);
			base.FocusedControl = m_textboxAmount;
			m_invalidInput = true;
			UpdateControls();
		}

		private void UpdateActivityLog(MyGuiControlTable listHistory)
		{
			MyAccountInfo account;
			if (m_transactionType == MyTransactionType.Deposit)
			{
				MyBankingSystem.Static.TryGetAccountInfo(m_toIdentifier, out account);
			}
			else
			{
				MyBankingSystem.Static.TryGetAccountInfo(m_fromIdentifier, out account);
			}
			for (int num = account.Log.Length - 1; num >= 0; num--)
			{
				MyAccountLogEntry myAccountLogEntry = account.Log[num];
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(myAccountLogEntry.ChangeIdentifier);
				string accountOwnerName;
				if (myIdentity != null)
				{
					accountOwnerName = myIdentity.DisplayName;
				}
				else
				{
					IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(myAccountLogEntry.ChangeIdentifier);
					accountOwnerName = ((myFaction == null) ? myAccountLogEntry.ChangeIdentifier.ToString() : myFaction.Name);
				}
				listHistory.Add(CreateRow(myAccountLogEntry.DateTime, accountOwnerName, myAccountLogEntry.Amount));
			}
		}

		private MyGuiControlTable.Row CreateRow(DateTime dateTime, string accountOwnerName, long amount)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row();
			row.AddCell(new MyGuiControlTable.Cell(dateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm")));
			row.AddCell(new MyGuiControlTable.Cell(accountOwnerName));
			row.AddCell(new MyGuiControlTable.Cell(MyBankingSystem.GetFormatedValue(amount))
			{
				TextColor = ((amount > 0) ? Color.Green : Color.Red)
			});
			return row;
		}

		private void OnButtonOkPressed(MyGuiControlButton obj)
		{
			if (long.TryParse(m_textboxAmount.Text, out long result))
			{
				MyBankingSystem.RequestTransfer(m_fromIdentifier, m_toIdentifier, result);
				CloseScreen();
			}
		}

		private void OnAmountChanged(MyGuiControlTextbox obj)
		{
			if (long.TryParse(obj.Text, out long result) && result > 0)
			{
				m_finalBalance = m_currentBalance - result;
			}
			else
			{
				m_finalBalance = m_currentBalance;
			}
			m_invalidInput = (!IsFinalBalanceValid() || result <= 0);
			UpdateControls();
		}

		private bool IsFinalBalanceValid()
		{
			if (m_finalBalance < 0 || m_finalBalance == m_currentBalance)
			{
				return false;
			}
			return true;
		}

		private void UpdateControls()
		{
			if (m_invalidInput)
			{
				m_labelFinalBalanceValue.ColorMask = MyTerminalFactionController.COLOR_CUSTOM_RED;
				m_btnOk.Enabled = false;
			}
			else
			{
				m_labelFinalBalanceValue.ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY;
				m_btnOk.Enabled = true;
			}
			m_labelFinalBalanceValue.Text = MyBankingSystem.GetFormatedValue((m_finalBalance > 0) ? m_finalBalance : 0);
		}
	}
}
