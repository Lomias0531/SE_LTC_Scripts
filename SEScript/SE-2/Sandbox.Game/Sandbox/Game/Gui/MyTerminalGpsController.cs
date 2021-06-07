using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using VRage;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalGpsController : MyTerminalController
	{
		public static readonly Color ITEM_SHOWN_COLOR = Color.CornflowerBlue;

		private IMyGuiControlsParent m_controlsParent;

		private MyGuiControlSearchBox m_searchBox;

		private StringBuilder m_NameBuilder = new StringBuilder();

		private MyGuiControlTable m_tableIns;

		private MyGuiControlLabel m_labelInsName;

		private MyGuiControlTextbox m_panelInsName;

		private MyGuiControlLabel m_labelInsDesc;

		private MyGuiControlMultilineEditableText m_panelInsDesc;

		private MyGuiControlLabel m_labelInsX;

		private MyGuiControlTextbox m_xCoord;

		private MyGuiControlLabel m_labelInsY;

		private MyGuiControlTextbox m_yCoord;

		private MyGuiControlLabel m_labelInsZ;

		private MyGuiControlTextbox m_zCoord;

		private MyGuiControlLabel m_labelInsShowOnHud;

		private MyGuiControlCheckbox m_checkInsShowOnHud;

		private MyGuiControlLabel m_labelInsAlwaysVisible;

		private MyGuiControlCheckbox m_checkInsAlwaysVisible;

		private MyGuiControlButton m_buttonAdd;

		private MyGuiControlButton m_buttonAddFromClipboard;

		private MyGuiControlButton m_buttonAddCurrent;

		private MyGuiControlButton m_buttonDelete;

		private MyGuiControlButton m_buttonCopy;

		private MyGuiControlLabel m_labelSaveWarning;

		private int? m_previousHash;

		private bool m_needsSyncName;

		private bool m_needsSyncDesc;

		private bool m_needsSyncX;

		private bool m_needsSyncY;

		private bool m_needsSyncZ;

		private string m_clipboardText;

		private bool m_nameOk;

		private bool m_xOk;

		private bool m_yOk;

		private bool m_zOk;

		private MyGps m_syncedGps;

		public void Init(IMyGuiControlsParent controlsParent)
		{
			m_controlsParent = controlsParent;
			m_searchBox = (MyGuiControlSearchBox)m_controlsParent.Controls.GetControlByName("SearchIns");
			m_searchBox.OnTextChanged += SearchIns_TextChanged;
			m_tableIns = (MyGuiControlTable)controlsParent.Controls.GetControlByName("TableINS");
			m_tableIns.SetColumnComparison(0, TableSortingComparison);
			m_tableIns.ItemSelected += OnTableItemSelected;
			m_tableIns.ItemDoubleClicked += OnTableDoubleclick;
			m_buttonAdd = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("buttonAdd");
			m_buttonAddCurrent = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("buttonFromCurrent");
			m_buttonAddFromClipboard = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("buttonFromClipboard");
			m_buttonDelete = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("buttonDelete");
			m_buttonAdd.ButtonClicked += OnButtonPressedNew;
			m_buttonAddFromClipboard.ButtonClicked += OnButtonPressedNewFromClipboard;
			m_buttonAddCurrent.ButtonClicked += OnButtonPressedNewFromCurrent;
			m_buttonDelete.ButtonClicked += OnButtonPressedDelete;
			m_labelInsName = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsName");
			m_panelInsName = (MyGuiControlTextbox)controlsParent.Controls.GetControlByName("panelInsName");
			m_labelInsDesc = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsDesc");
			m_panelInsDesc = (MyGuiControlMultilineEditableText)controlsParent.Controls.GetControlByName("textInsDesc");
			m_labelInsX = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsX");
			m_xCoord = (MyGuiControlTextbox)controlsParent.Controls.GetControlByName("textInsX");
			m_labelInsY = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsY");
			m_yCoord = (MyGuiControlTextbox)controlsParent.Controls.GetControlByName("textInsY");
			m_labelInsZ = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsZ");
			m_zCoord = (MyGuiControlTextbox)controlsParent.Controls.GetControlByName("textInsZ");
			m_labelInsShowOnHud = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsShowOnHud");
			m_checkInsShowOnHud = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("checkInsShowOnHud");
			m_checkInsShowOnHud.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_ShowOnHud_ToolTip));
			MyGuiControlCheckbox checkInsShowOnHud = m_checkInsShowOnHud;
			checkInsShowOnHud.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkInsShowOnHud.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
			m_labelInsAlwaysVisible = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelInsAlwaysVisible");
			m_checkInsAlwaysVisible = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("checkInsAlwaysVisible");
			MyGuiControlCheckbox checkInsAlwaysVisible = m_checkInsAlwaysVisible;
			checkInsAlwaysVisible.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkInsAlwaysVisible.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAlwaysVisibleChecked));
			m_buttonCopy = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("buttonToClipboard");
			m_buttonCopy.ButtonClicked += OnButtonPressedCopy;
			m_labelSaveWarning = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("TerminalTab_GPS_SaveWarning");
			m_labelSaveWarning.Visible = false;
			m_panelInsName.ShowTooltipWhenDisabled = true;
			m_panelInsDesc.ShowTooltipWhenDisabled = true;
			m_xCoord.ShowTooltipWhenDisabled = true;
			m_yCoord.ShowTooltipWhenDisabled = true;
			m_zCoord.ShowTooltipWhenDisabled = true;
			m_checkInsShowOnHud.ShowTooltipWhenDisabled = true;
			m_checkInsAlwaysVisible.ShowTooltipWhenDisabled = true;
			m_buttonCopy.ShowTooltipWhenDisabled = true;
			HookSyncEvents();
			MySession.Static.Gpss.GpsAdded += OnGpsAdded;
			MySession.Static.Gpss.GpsChanged += OnInsChanged;
			MySession.Static.Gpss.ListChanged += OnListChanged;
			MySession.Static.Gpss.DiscardOld();
			PopulateList();
			m_previousHash = null;
			EnableEditBoxes(enable: false);
			SetDeleteButtonEnabled(enabled: false);
		}

		private int TableSortingComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			if ((((MyGps)a.UserData).DiscardAt.HasValue && ((MyGps)b.UserData).DiscardAt.HasValue) || (!((MyGps)a.UserData).DiscardAt.HasValue && !((MyGps)b.UserData).DiscardAt.HasValue))
			{
				return a.Text.CompareToIgnoreCase(b.Text);
			}
			if (!((MyGps)a.UserData).DiscardAt.HasValue)
			{
				return -1;
			}
			return 1;
		}

		public void PopulateList()
		{
			PopulateList(null);
		}

		public void PopulateList(string searchString)
		{
			object obj = m_tableIns.SelectedRow?.UserData;
			int? num = m_tableIns.SelectedRowIndex;
			ClearList();
			if (MySession.Static.Gpss.ExistsForPlayer(MySession.Static.LocalPlayerId))
			{
				foreach (KeyValuePair<int, MyGps> item in MySession.Static.Gpss[MySession.Static.LocalPlayerId])
				{
					if (searchString != null)
					{
						string[] array = searchString.ToLower().Split(new char[1]
						{
							' '
						});
						string text = item.Value.Name.ToString().ToLower();
						bool flag = true;
						string[] array2 = array;
						foreach (string text2 in array2)
						{
							if (!text.Contains(text2.ToLower()))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							AddToList(item.Value);
						}
					}
					else
					{
						AddToList(item.Value);
					}
				}
			}
			m_tableIns.SortByColumn(0, MyGuiControlTable.SortStateEnum.Ascending);
			EnableEditBoxes(enable: false);
			if (obj != null)
			{
				for (int j = 0; j < m_tableIns.RowsCount; j++)
				{
					if (obj == m_tableIns.GetRow(j).UserData)
					{
						m_tableIns.SelectedRowIndex = j;
						EnableEditBoxes(enable: true);
						SetDeleteButtonEnabled(enabled: true);
						break;
					}
				}
				if (m_tableIns.SelectedRow == null && obj != null)
				{
					if (num >= m_tableIns.RowsCount)
					{
						num = m_tableIns.RowsCount - 1;
					}
					m_tableIns.SelectedRowIndex = num;
					if (m_tableIns.SelectedRow != null)
					{
						EnableEditBoxes(enable: true);
						SetDeleteButtonEnabled(enabled: true);
						FillRight((MyGps)m_tableIns.SelectedRow.UserData);
					}
				}
			}
			m_tableIns.ScrollToSelection();
			if (obj == null)
			{
				FillRight();
			}
		}

		private MyGuiControlTable.Row AddToList(MyGps ins)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(ins);
			StringBuilder stringBuilder = new StringBuilder(ins.Name);
			string toolTip = stringBuilder.ToString();
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder, ins, toolTip, ins.DiscardAt.HasValue ? Color.Gray : (ins.ShowOnHud ? ITEM_SHOWN_COLOR : Color.White)));
			m_tableIns.Add(row);
			return row;
		}

		public void ClearList()
		{
			if (m_tableIns != null)
			{
				m_tableIns.Clear();
			}
		}

		private void SearchIns_TextChanged(string text)
		{
			PopulateList(text);
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			trySync();
			if (sender.SelectedRow != null)
			{
				EnableEditBoxes(enable: true);
				SetDeleteButtonEnabled(enabled: true);
				FillRight((MyGps)sender.SelectedRow.UserData);
			}
			else
			{
				EnableEditBoxes(enable: false);
				SetDeleteButtonEnabled(enabled: false);
				ClearRight();
			}
		}

		private void SetDeleteButtonEnabled(bool enabled)
		{
			if (enabled)
			{
				m_buttonDelete.Enabled = true;
				m_buttonDelete.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Delete_ToolTip));
			}
			else
			{
				m_buttonDelete.Enabled = false;
				m_buttonDelete.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Delete_Disabled_ToolTip));
			}
		}

		private void EnableEditBoxes(bool enable)
		{
			m_panelInsName.Enabled = enable;
			m_panelInsDesc.Enabled = enable;
			m_xCoord.Enabled = enable;
			m_yCoord.Enabled = enable;
			m_zCoord.Enabled = enable;
			m_checkInsShowOnHud.Enabled = enable;
			m_checkInsAlwaysVisible.Enabled = enable;
			m_buttonCopy.Enabled = enable;
			if (enable)
			{
				m_panelInsName.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Name_ToolTip));
				m_panelInsDesc.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Desc_ToolTip));
				m_xCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_X_ToolTip));
				m_yCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Y_ToolTip));
				m_zCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Z_ToolTip));
				m_checkInsShowOnHud.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_ShowOnHud_ToolTip));
				m_checkInsAlwaysVisible.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_AlwaysVisible_Tooltip));
				m_buttonCopy.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_CopyToClipboard_ToolTip));
			}
			else
			{
				m_checkInsShowOnHud.ShowTooltipWhenDisabled = true;
				m_checkInsAlwaysVisible.ShowTooltipWhenDisabled = true;
				m_panelInsName.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_panelInsDesc.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_xCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_yCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_zCoord.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_checkInsShowOnHud.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_checkInsAlwaysVisible.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
				m_buttonCopy.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_SelectGpsEntry));
			}
		}

		private void OnTableDoubleclick(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			if (sender.SelectedRow != null)
			{
				MyGps obj = (MyGps)sender.SelectedRow.UserData;
				obj.ShowOnHud = !obj.ShowOnHud;
				MySession.Static.Gpss.ChangeShowOnHud(MySession.Static.LocalPlayerId, ((MyGps)sender.SelectedRow.UserData).Hash, ((MyGps)sender.SelectedRow.UserData).ShowOnHud);
			}
		}

		private void HookSyncEvents()
		{
			m_panelInsName.TextChanged += OnNameChanged;
			m_panelInsDesc.TextChanged += OnDescChanged;
			m_xCoord.TextChanged += OnXChanged;
			m_yCoord.TextChanged += OnYChanged;
			m_zCoord.TextChanged += OnZChanged;
		}

		private void UnhookSyncEvents()
		{
			m_panelInsName.TextChanged -= OnNameChanged;
			m_panelInsDesc.TextChanged -= OnDescChanged;
			m_xCoord.TextChanged -= OnXChanged;
			m_yCoord.TextChanged -= OnYChanged;
			m_zCoord.TextChanged -= OnZChanged;
		}

		public void OnNameChanged(MyGuiControlTextbox sender)
		{
			if (m_tableIns.SelectedRow == null)
			{
				return;
			}
			m_needsSyncName = true;
			if (IsNameOk(sender.Text))
			{
				m_nameOk = true;
				sender.ColorMask = Vector4.One;
				MyGuiControlTable.Row selectedRow = m_tableIns.SelectedRow;
				MyGuiControlTable.Cell cell = selectedRow.GetCell(0);
				if (cell != null)
				{
					cell.Text.Clear().Append(sender.Text);
					cell.ToolTip.ToolTips[0] = new MyColoredText(sender.Text);
				}
				m_tableIns.SortByColumn(0, MyGuiControlTable.SortStateEnum.Ascending);
				for (int i = 0; i < m_tableIns.RowsCount; i++)
				{
					if (selectedRow == m_tableIns.GetRow(i))
					{
						m_tableIns.SelectedRowIndex = i;
						break;
					}
				}
				m_tableIns.ScrollToSelection();
			}
			else
			{
				m_nameOk = false;
				sender.ColorMask = Color.Red.ToVector4();
			}
			UpdateWarningLabel();
		}

		public bool IsNameOk(string str)
		{
			return !str.Contains(":");
		}

		public void OnDescChanged(MyGuiControlMultilineEditableText sender)
		{
			m_needsSyncDesc = true;
		}

		private bool IsCoordOk(string str)
		{
			double result;
			return double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		public void OnXChanged(MyGuiControlTextbox sender)
		{
			m_needsSyncX = true;
			if (IsCoordOk(sender.Text))
			{
				m_xOk = true;
				sender.ColorMask = Vector4.One;
			}
			else
			{
				m_xOk = false;
				sender.ColorMask = Color.Red.ToVector4();
			}
			UpdateWarningLabel();
		}

		public void OnYChanged(MyGuiControlTextbox sender)
		{
			m_needsSyncY = true;
			if (IsCoordOk(sender.Text))
			{
				m_yOk = true;
				sender.ColorMask = Vector4.One;
			}
			else
			{
				m_yOk = false;
				sender.ColorMask = Color.Red.ToVector4();
			}
			UpdateWarningLabel();
		}

		public void OnZChanged(MyGuiControlTextbox sender)
		{
			m_needsSyncZ = true;
			if (IsCoordOk(sender.Text))
			{
				m_zOk = true;
				sender.ColorMask = Vector4.One;
			}
			else
			{
				m_zOk = false;
				sender.ColorMask = Color.Red.ToVector4();
			}
			UpdateWarningLabel();
		}

		private void UpdateWarningLabel()
		{
			if (m_nameOk && m_xOk && m_yOk && m_zOk)
			{
				m_labelSaveWarning.Visible = false;
				if (m_panelInsName.Enabled)
				{
					m_buttonCopy.Enabled = true;
				}
			}
			else
			{
				m_labelSaveWarning.Visible = true;
				m_buttonCopy.Enabled = false;
			}
		}

		private bool trySync()
		{
			if (m_previousHash.HasValue && (m_needsSyncName || m_needsSyncDesc || m_needsSyncX || m_needsSyncY || m_needsSyncZ) && MySession.Static.Gpss.ExistsForPlayer(MySession.Static.LocalPlayerId) && IsNameOk(m_panelInsName.Text) && IsCoordOk(m_xCoord.Text) && IsCoordOk(m_yCoord.Text) && IsCoordOk(m_zCoord.Text) && MySession.Static.Gpss[MySession.Static.LocalPlayerId].TryGetValue(m_previousHash.Value, out MyGps value))
			{
				if (m_needsSyncName)
				{
					value.Name = m_panelInsName.Text;
				}
				if (m_needsSyncDesc)
				{
					value.Description = m_panelInsDesc.Text.ToString();
				}
				StringBuilder stringBuilder = new StringBuilder();
				Vector3D coords = value.Coords;
				if (m_needsSyncX)
				{
					m_xCoord.GetText(stringBuilder);
					coords.X = Math.Round(double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture), 2);
				}
				stringBuilder.Clear();
				if (m_needsSyncY)
				{
					m_yCoord.GetText(stringBuilder);
					coords.Y = Math.Round(double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture), 2);
				}
				stringBuilder.Clear();
				if (m_needsSyncZ)
				{
					m_zCoord.GetText(stringBuilder);
					coords.Z = Math.Round(double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture), 2);
				}
				value.Coords = coords;
				m_syncedGps = value;
				MySession.Static.Gpss.SendModifyGps(MySession.Static.LocalPlayerId, value);
				return true;
			}
			return false;
		}

		private void OnButtonPressedNew(MyGuiControlButton sender)
		{
			trySync();
			MyGps gps = new MyGps();
			gps.Name = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewCoord_Name).ToString();
			gps.Description = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewCoord_Desc).ToString();
			gps.Coords = new Vector3D(0.0, 0.0, 0.0);
			gps.ShowOnHud = true;
			gps.DiscardAt = null;
			EnableEditBoxes(enable: false);
			MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
			m_searchBox.SearchText = string.Empty;
		}

		private void OnButtonPressedNewFromCurrent(MyGuiControlButton sender)
		{
			trySync();
			MyGps gps = new MyGps();
			MySession.Static.Gpss.GetNameForNewCurrent(m_NameBuilder);
			gps.Name = m_NameBuilder.ToString();
			gps.Description = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromCurrent_Desc).ToString();
			Vector3D position = MySession.Static.LocalHumanPlayer.GetPosition();
			position.X = Math.Round(position.X, 2);
			position.Y = Math.Round(position.Y, 2);
			position.Z = Math.Round(position.Z, 2);
			gps.Coords = position;
			gps.ShowOnHud = true;
			gps.DiscardAt = null;
			EnableEditBoxes(enable: false);
			MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
			m_searchBox.SearchText = string.Empty;
		}

		private void PasteFromClipboard()
		{
			m_clipboardText = MyVRage.Platform.Clipboard;
		}

		private void OnButtonPressedNewFromClipboard(MyGuiControlButton sender)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				PasteFromClipboard();
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			MySession.Static.Gpss.ScanText(m_clipboardText, MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromClipboard_Desc));
			m_searchBox.SearchText = string.Empty;
		}

		private void OnButtonPressedDelete(MyGuiControlButton sender)
		{
			if (m_tableIns.SelectedRow != null)
			{
				Delete();
			}
		}

		private void Delete()
		{
			MySession.Static.Gpss.SendDelete(MySession.Static.LocalPlayerId, ((MyGps)m_tableIns.SelectedRow.UserData).GetHashCode());
			EnableEditBoxes(enable: false);
			SetDeleteButtonEnabled(enabled: false);
			PopulateList();
		}

		public void OnDelKeyPressed()
		{
			if (m_tableIns.SelectedRow != null && m_tableIns.HasFocus)
			{
				Delete();
			}
		}

		private void OnButtonPressedCopy(MyGuiControlButton sender)
		{
			if (m_tableIns.SelectedRow != null)
			{
				if (trySync())
				{
					m_syncedGps.ToClipboard();
				}
				else
				{
					((MyGps)m_tableIns.SelectedRow.UserData).ToClipboard();
				}
			}
		}

		private void OnGpsAdded(long id, int hash)
		{
			if (id != MySession.Static.LocalPlayerId)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < m_tableIns.RowsCount)
				{
					if (((MyGps)m_tableIns.GetRow(num).UserData).GetHashCode() == hash)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_tableIns.SelectedRowIndex = num;
			if (m_tableIns.SelectedRow != null)
			{
				EnableEditBoxes(enable: true);
				SetDeleteButtonEnabled(enabled: true);
				FillRight((MyGps)m_tableIns.SelectedRow.UserData);
			}
		}

		private void OnInsChanged(long id, int hash)
		{
			if (id != MySession.Static.LocalPlayerId)
			{
				return;
			}
			FillRight();
			int num = 0;
			while (true)
			{
				if (num < m_tableIns.RowsCount)
				{
					if (((MyGps)m_tableIns.GetRow(num).UserData).GetHashCode() == hash)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			MyGuiControlTable.Cell cell = m_tableIns.GetRow(num).GetCell(0);
			if (cell != null)
			{
				MyGps myGps = (MyGps)m_tableIns.GetRow(num).UserData;
				cell.TextColor = (myGps.DiscardAt.HasValue ? Color.Gray : (myGps.ShowOnHud ? ITEM_SHOWN_COLOR : Color.White));
				cell.Text.Clear().Append(((MyGps)m_tableIns.GetRow(num).UserData).Name);
			}
		}

		private void OnListChanged(long id)
		{
			if (id == MySession.Static.LocalPlayerId)
			{
				PopulateList();
			}
		}

		private void OnShowOnHudChecked(MyGuiControlCheckbox sender)
		{
			if (m_tableIns.SelectedRow != null)
			{
				MyGps myGps = m_tableIns.SelectedRow.UserData as MyGps;
				myGps.ShowOnHud = sender.IsChecked;
				if (!sender.IsChecked && myGps.AlwaysVisible)
				{
					myGps.AlwaysVisible = false;
					MyGuiControlCheckbox checkInsShowOnHud = m_checkInsShowOnHud;
					checkInsShowOnHud.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkInsShowOnHud.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
					m_checkInsShowOnHud.IsChecked = false;
					MyGuiControlCheckbox checkInsShowOnHud2 = m_checkInsShowOnHud;
					checkInsShowOnHud2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkInsShowOnHud2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
				}
				if (!trySync())
				{
					MySession.Static.Gpss.ChangeShowOnHud(MySession.Static.LocalPlayerId, myGps.Hash, sender.IsChecked);
				}
			}
		}

		private void OnAlwaysVisibleChecked(MyGuiControlCheckbox sender)
		{
			if (m_tableIns.SelectedRow != null)
			{
				MyGps myGps = m_tableIns.SelectedRow.UserData as MyGps;
				myGps.AlwaysVisible = sender.IsChecked;
				myGps.ShowOnHud = (myGps.ShowOnHud || myGps.AlwaysVisible);
				MyGuiControlCheckbox checkInsShowOnHud = m_checkInsShowOnHud;
				checkInsShowOnHud.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkInsShowOnHud.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
				m_checkInsShowOnHud.IsChecked = (m_checkInsShowOnHud.IsChecked || sender.IsChecked);
				MyGuiControlCheckbox checkInsShowOnHud2 = m_checkInsShowOnHud;
				checkInsShowOnHud2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkInsShowOnHud2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
				if (!trySync())
				{
					MySession.Static.Gpss.ChangeAlwaysVisible(MySession.Static.LocalPlayerId, myGps.Hash, sender.IsChecked);
				}
			}
		}

		private void FillRight()
		{
			if (m_tableIns.SelectedRow != null)
			{
				FillRight((MyGps)m_tableIns.SelectedRow.UserData);
			}
			else
			{
				ClearRight();
			}
			m_nameOk = (m_xOk = (m_yOk = (m_zOk = true)));
			UpdateWarningLabel();
		}

		private void FillRight(MyGps ins)
		{
			UnhookSyncEvents();
			m_panelInsName.SetText(new StringBuilder(ins.Name));
			m_panelInsName.Enabled = !ins.IsContainerGPS;
			m_panelInsDesc.Text = new StringBuilder(ins.Description);
			m_xCoord.SetText(new StringBuilder(ins.Coords.X.ToString("F2", CultureInfo.InvariantCulture)));
			m_xCoord.Enabled = !ins.IsContainerGPS;
			m_yCoord.SetText(new StringBuilder(ins.Coords.Y.ToString("F2", CultureInfo.InvariantCulture)));
			m_yCoord.Enabled = !ins.IsContainerGPS;
			m_zCoord.SetText(new StringBuilder(ins.Coords.Z.ToString("F2", CultureInfo.InvariantCulture)));
			m_zCoord.Enabled = !ins.IsContainerGPS;
			MyGuiControlCheckbox checkInsShowOnHud = m_checkInsShowOnHud;
			checkInsShowOnHud.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkInsShowOnHud.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
			m_checkInsShowOnHud.IsChecked = ins.ShowOnHud;
			MyGuiControlCheckbox checkInsShowOnHud2 = m_checkInsShowOnHud;
			checkInsShowOnHud2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkInsShowOnHud2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
			MyGuiControlCheckbox checkInsAlwaysVisible = m_checkInsAlwaysVisible;
			checkInsAlwaysVisible.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkInsAlwaysVisible.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAlwaysVisibleChecked));
			m_checkInsAlwaysVisible.IsChecked = ins.AlwaysVisible;
			MyGuiControlCheckbox checkInsAlwaysVisible2 = m_checkInsAlwaysVisible;
			checkInsAlwaysVisible2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkInsAlwaysVisible2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAlwaysVisibleChecked));
			m_previousHash = ins.Hash;
			HookSyncEvents();
			m_needsSyncName = false;
			m_needsSyncDesc = false;
			m_needsSyncX = false;
			m_needsSyncY = false;
			m_needsSyncZ = false;
			m_panelInsName.ColorMask = Vector4.One;
			m_xCoord.ColorMask = Vector4.One;
			m_yCoord.ColorMask = Vector4.One;
			m_zCoord.ColorMask = Vector4.One;
			m_nameOk = (m_xOk = (m_yOk = (m_zOk = true)));
			UpdateWarningLabel();
		}

		private void ClearRight()
		{
			UnhookSyncEvents();
			StringBuilder text = new StringBuilder("");
			m_panelInsName.SetText(text);
			m_panelInsDesc.Clear();
			m_xCoord.SetText(text);
			m_yCoord.SetText(text);
			m_zCoord.SetText(text);
			m_checkInsShowOnHud.IsChecked = false;
			m_checkInsAlwaysVisible.IsChecked = false;
			m_previousHash = null;
			HookSyncEvents();
			m_needsSyncName = false;
			m_needsSyncDesc = false;
			m_needsSyncX = false;
			m_needsSyncY = false;
			m_needsSyncZ = false;
		}

		public void Close()
		{
			trySync();
			if (m_tableIns != null)
			{
				ClearList();
				m_tableIns.ItemSelected -= OnTableItemSelected;
				m_tableIns.ItemDoubleClicked -= OnTableDoubleclick;
			}
			m_syncedGps = null;
			MySession.Static.Gpss.GpsAdded -= OnGpsAdded;
			MySession.Static.Gpss.GpsChanged -= OnInsChanged;
			MySession.Static.Gpss.ListChanged -= OnListChanged;
			UnhookSyncEvents();
			MyGuiControlCheckbox checkInsShowOnHud = m_checkInsShowOnHud;
			checkInsShowOnHud.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkInsShowOnHud.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnShowOnHudChecked));
			MyGuiControlCheckbox checkInsAlwaysVisible = m_checkInsAlwaysVisible;
			checkInsAlwaysVisible.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkInsAlwaysVisible.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAlwaysVisibleChecked));
			m_buttonAdd.ButtonClicked -= OnButtonPressedNew;
			m_buttonAddFromClipboard.ButtonClicked -= OnButtonPressedNewFromClipboard;
			m_buttonAddCurrent.ButtonClicked -= OnButtonPressedNewFromCurrent;
			m_buttonDelete.ButtonClicked -= OnButtonPressedDelete;
			m_buttonCopy.ButtonClicked -= OnButtonPressedCopy;
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Delete))
			{
				OnDelKeyPressed();
			}
		}
	}
}
