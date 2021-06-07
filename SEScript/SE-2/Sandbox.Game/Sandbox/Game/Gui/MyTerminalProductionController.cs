using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalProductionController : MyTerminalController
	{
		private enum AssemblerMode
		{
			Assembling,
			Disassembling
		}

		public static readonly int BLUEPRINT_GRID_ROWS = 7;

		public static readonly int QUEUE_GRID_ROWS = 2;

		public static readonly int INVENTORY_GRID_ROWS = 3;

		private static readonly Vector4 ERROR_ICON_COLOR_MASK = new Vector4(1f, 0.5f, 0.5f, 1f);

		private static StringBuilder m_textCache = new StringBuilder();

		private static Dictionary<MyDefinitionId, MyFixedPoint> m_requiredCountCache = new Dictionary<MyDefinitionId, MyFixedPoint>(MyDefinitionId.Comparer);

		private static List<MyBlueprintDefinitionBase.ProductionInfo> m_blueprintCache = new List<MyBlueprintDefinitionBase.ProductionInfo>();

		private IMyGuiControlsParent m_controlsParent;

		private MyGridTerminalSystem m_terminalSystem;

		private Dictionary<int, MyAssembler> m_assemblersByKey = new Dictionary<int, MyAssembler>();

		private int m_assemblerKeyCounter;

		private MyGuiControlSearchBox m_blueprintsSearchBox;

		private MyGuiControlCombobox m_comboboxAssemblers;

		private MyGuiControlGrid m_blueprintsGrid;

		private MyAssembler m_selectedAssembler;

		private MyGuiControlRadioButtonGroup m_blueprintButtonGroup = new MyGuiControlRadioButtonGroup();

		private MyGuiControlRadioButtonGroup m_modeButtonGroup = new MyGuiControlRadioButtonGroup();

		private MyGuiControlGrid m_queueGrid;

		private MyGuiControlGrid m_inventoryGrid;

		private MyGuiControlComponentList m_materialsList;

		private MyGuiControlScrollablePanel m_blueprintsArea;

		private MyGuiControlScrollablePanel m_queueArea;

		private MyGuiControlScrollablePanel m_inventoryArea;

		private MyGuiControlBase m_blueprintsBgPanel;

		private MyGuiControlBase m_blueprintsLabel;

		private MyGuiControlCheckbox m_repeatCheckbox;

		private MyGuiControlCheckbox m_slaveCheckbox;

		private MyGuiControlButton m_disassembleAllButton;

		private MyGuiControlButton m_controlPanelButton;

		private MyGuiControlButton m_inventoryButton;

		private MyGuiControlLabel m_materialsLabel;

		private MyDragAndDropInfo m_dragAndDropInfo;

		private MyGuiControlGridDragAndDrop m_dragAndDrop;

		private StringBuilder m_incompleteAssemblerName = new StringBuilder();

		private AssemblerMode CurrentAssemblerMode => (AssemblerMode)m_modeButtonGroup.SelectedButton.Key;

		public void Init(IMyGuiControlsParent controlsParent, MyCubeGrid grid, MyCubeBlock currentBlock)
		{
			if (grid == null)
			{
				ShowError(MySpaceTexts.ScreenTerminalError_ShipNotConnected, controlsParent);
				return;
			}
			grid.OnTerminalOpened();
			m_assemblerKeyCounter = 0;
			m_assemblersByKey.Clear();
			foreach (MyTerminalBlock block in grid.GridSystems.TerminalSystem.Blocks)
			{
				MyAssembler myAssembler = block as MyAssembler;
				if (myAssembler != null && myAssembler.HasLocalPlayerAccess())
				{
					m_assemblersByKey.Add(m_assemblerKeyCounter++, myAssembler);
				}
			}
			m_controlsParent = controlsParent;
			m_terminalSystem = grid.GridSystems.TerminalSystem;
			m_blueprintsArea = (MyGuiControlScrollablePanel)controlsParent.Controls.GetControlByName("BlueprintsScrollableArea");
			m_blueprintsSearchBox = (MyGuiControlSearchBox)controlsParent.Controls.GetControlByName("BlueprintsSearchBox");
			m_queueArea = (MyGuiControlScrollablePanel)controlsParent.Controls.GetControlByName("QueueScrollableArea");
			m_inventoryArea = (MyGuiControlScrollablePanel)controlsParent.Controls.GetControlByName("InventoryScrollableArea");
			m_blueprintsBgPanel = controlsParent.Controls.GetControlByName("BlueprintsBackgroundPanel");
			m_blueprintsLabel = controlsParent.Controls.GetControlByName("BlueprintsLabel");
			m_comboboxAssemblers = (MyGuiControlCombobox)controlsParent.Controls.GetControlByName("AssemblersCombobox");
			m_blueprintsGrid = (MyGuiControlGrid)m_blueprintsArea.ScrolledControl;
			m_queueGrid = (MyGuiControlGrid)m_queueArea.ScrolledControl;
			m_inventoryGrid = (MyGuiControlGrid)m_inventoryArea.ScrolledControl;
			m_materialsList = (MyGuiControlComponentList)controlsParent.Controls.GetControlByName("MaterialsList");
			m_repeatCheckbox = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("RepeatCheckbox");
			m_slaveCheckbox = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("SlaveCheckbox");
			m_disassembleAllButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("DisassembleAllButton");
			m_controlPanelButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("ControlPanelButton");
			m_inventoryButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("InventoryButton");
			m_materialsLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("RequiredLabel");
			m_controlPanelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_TerminalControlScreen));
			m_inventoryButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_TerminalInventoryScreen));
			MyGuiControlRadioButton myGuiControlRadioButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("AssemblingButton");
			myGuiControlRadioButton.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
			MyGuiControlRadioButton myGuiControlRadioButton2 = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("DisassemblingButton");
			myGuiControlRadioButton2.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
			myGuiControlRadioButton.Key = 0;
			myGuiControlRadioButton2.Key = 1;
			m_modeButtonGroup.Add(myGuiControlRadioButton);
			m_modeButtonGroup.Add(myGuiControlRadioButton2);
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey.OrderBy(delegate(KeyValuePair<int, MyAssembler> x)
			{
				MyAssembler value2 = x.Value;
				return (value2 == currentBlock) ? (-1) : (((!value2.IsFunctional) ? 10000 : 0) + value2.GUIPriority);
			}))
			{
				MyAssembler value = item.Value;
				if (!value.IsFunctional)
				{
					m_incompleteAssemblerName.Clear();
					m_incompleteAssemblerName.AppendStringBuilder(value.CustomName);
					m_incompleteAssemblerName.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockIncomplete));
					m_comboboxAssemblers.AddItem(item.Key, m_incompleteAssemblerName);
				}
				else
				{
					m_comboboxAssemblers.AddItem(item.Key, value.CustomName);
				}
			}
			m_comboboxAssemblers.ItemSelected += Assemblers_ItemSelected;
			m_comboboxAssemblers.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_AssemblerList));
			m_comboboxAssemblers.SelectItemByIndex(0);
			m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, supportIcon: true);
			controlsParent.Controls.Add(m_dragAndDrop);
			m_dragAndDrop.DrawBackgroundTexture = false;
			m_dragAndDrop.ItemDropped += dragDrop_OnItemDropped;
			RefreshBlueprints();
			Assemblers_ItemSelected();
			RegisterEvents();
			if (m_assemblersByKey.Count == 0)
			{
				ShowError(MySpaceTexts.ScreenTerminalError_NoAssemblers, controlsParent);
			}
		}

		private void UpdateBlueprintClassGui()
		{
			foreach (MyGuiControlRadioButton item in m_blueprintButtonGroup)
			{
				m_controlsParent.Controls.Remove(item);
			}
			m_blueprintButtonGroup.Clear();
			float xOffset = 0f;
			if (m_selectedAssembler.BlockDefinition is MyProductionBlockDefinition)
			{
				List<MyBlueprintClassDefinition> blueprintClasses = (m_selectedAssembler.BlockDefinition as MyProductionBlockDefinition).BlueprintClasses;
				for (int i = 0; i < blueprintClasses.Count; i++)
				{
					bool selected = i == 0 || blueprintClasses[i].Id.SubtypeName == "Components" || blueprintClasses[i].Id.SubtypeName == "BasicComponents";
					AddBlueprintClassButton(blueprintClasses[i], ref xOffset, selected);
				}
			}
		}

		private void AddBlueprintClassButton(MyBlueprintClassDefinition classDef, ref float xOffset, bool selected = false)
		{
			if (classDef != null)
			{
				MyGuiControlRadioButton myGuiControlRadioButton = new MyGuiControlRadioButton(m_blueprintsLabel.Position + new Vector2(xOffset, m_blueprintsLabel.Size.Y + 0.012f), new Vector2(46f, 46f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
				xOffset += myGuiControlRadioButton.Size.X;
				myGuiControlRadioButton.Icon = new MyGuiHighlightTexture
				{
					Normal = classDef.Icons[0],
					Highlight = classDef.HighlightIcon,
					SizePx = new Vector2(46f, 46f)
				};
				myGuiControlRadioButton.UserData = classDef;
				myGuiControlRadioButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				if (classDef.DisplayNameEnum.HasValue)
				{
					myGuiControlRadioButton.SetToolTip(classDef.DisplayNameEnum.Value);
				}
				else
				{
					myGuiControlRadioButton.SetToolTip(classDef.DisplayNameString);
				}
				m_blueprintButtonGroup.Add(myGuiControlRadioButton);
				m_controlsParent.Controls.Add(myGuiControlRadioButton);
				myGuiControlRadioButton.Selected = selected;
			}
		}

		private static void ShowError(MyStringId errorText, IMyGuiControlsParent controlsParent)
		{
			foreach (MyGuiControlBase control in controlsParent.Controls)
			{
				control.Visible = false;
			}
			MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("ErrorMessage");
			if (myGuiControlLabel == null)
			{
				myGuiControlLabel = MyGuiScreenTerminal.CreateErrorLabel(errorText, "ErrorMessage");
			}
			myGuiControlLabel.TextEnum = errorText;
			if (!controlsParent.Controls.Contains(myGuiControlLabel))
			{
				controlsParent.Controls.Add(myGuiControlLabel);
			}
		}

		private static void HideError(IMyGuiControlsParent controlsParent)
		{
			controlsParent.Controls.RemoveControlByName("ErrorMessage");
			foreach (MyGuiControlBase control in controlsParent.Controls)
			{
				control.Visible = true;
			}
		}

		private void RegisterEvents()
		{
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				item.Value.CustomNameChanged += assembler_CustomNameChanged;
			}
			m_terminalSystem.BlockAdded += TerminalSystem_BlockAdded;
			m_terminalSystem.BlockRemoved += TerminalSystem_BlockRemoved;
			m_blueprintButtonGroup.SelectedChanged += blueprintButtonGroup_SelectedChanged;
			m_modeButtonGroup.SelectedChanged += modeButtonGroup_SelectedChanged;
			m_blueprintsSearchBox.OnTextChanged += OnSearchTextChanged;
			m_blueprintsGrid.ItemClicked += blueprintsGrid_ItemClicked;
			m_blueprintsGrid.MouseOverIndexChanged += blueprintsGrid_MouseOverIndexChanged;
			m_blueprintsGrid.ItemAccepted += blueprintsGrid_ItemClicked;
			m_inventoryGrid.ItemClicked += inventoryGrid_ItemClicked;
			m_inventoryGrid.MouseOverIndexChanged += inventoryGrid_MouseOverIndexChanged;
			m_inventoryGrid.ItemAccepted += inventoryGrid_ItemClicked;
			m_repeatCheckbox.IsCheckedChanged = repeatCheckbox_IsCheckedChanged;
			m_slaveCheckbox.IsCheckedChanged = slaveCheckbox_IsCheckedChanged;
			m_queueGrid.ItemClicked += queueGrid_ItemClicked;
			m_queueGrid.ItemDragged += queueGrid_ItemDragged;
			m_queueGrid.ItemAccepted += queueGrid_ItemClicked;
			m_queueGrid.MouseOverIndexChanged += queueGrid_MouseOverIndexChanged;
			m_controlPanelButton.ButtonClicked += controlPanelButton_ButtonClicked;
			m_inventoryButton.ButtonClicked += inventoryButton_ButtonClicked;
			m_disassembleAllButton.ButtonClicked += disassembleAllButton_ButtonClicked;
		}

		private void UnregisterEvents()
		{
			if (m_controlsParent != null)
			{
				foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
				{
					item.Value.CustomNameChanged -= assembler_CustomNameChanged;
				}
				if (m_terminalSystem != null)
				{
					m_terminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
					m_terminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
				}
				m_blueprintButtonGroup.SelectedChanged -= blueprintButtonGroup_SelectedChanged;
				m_modeButtonGroup.SelectedChanged -= modeButtonGroup_SelectedChanged;
				m_blueprintsSearchBox.OnTextChanged -= OnSearchTextChanged;
				m_blueprintsGrid.ItemClicked -= blueprintsGrid_ItemClicked;
				m_blueprintsGrid.MouseOverIndexChanged -= blueprintsGrid_MouseOverIndexChanged;
				m_blueprintsGrid.ItemAccepted -= blueprintsGrid_ItemClicked;
				m_inventoryGrid.ItemClicked -= inventoryGrid_ItemClicked;
				m_inventoryGrid.MouseOverIndexChanged -= inventoryGrid_MouseOverIndexChanged;
				m_inventoryGrid.ItemAccepted -= inventoryGrid_ItemClicked;
				m_repeatCheckbox.IsCheckedChanged = null;
				m_slaveCheckbox.IsCheckedChanged = null;
				m_queueGrid.ItemClicked -= queueGrid_ItemClicked;
				m_queueGrid.ItemDragged -= queueGrid_ItemDragged;
				m_queueGrid.MouseOverIndexChanged -= queueGrid_MouseOverIndexChanged;
				m_queueGrid.ItemAccepted -= queueGrid_ItemClicked;
				m_controlPanelButton.ButtonClicked -= controlPanelButton_ButtonClicked;
				m_inventoryButton.ButtonClicked -= inventoryButton_ButtonClicked;
				m_disassembleAllButton.ButtonClicked -= disassembleAllButton_ButtonClicked;
			}
		}

		private void RegisterAssemblerEvents(MyAssembler assembler)
		{
			if (assembler != null)
			{
				assembler.CurrentModeChanged += assembler_CurrentModeChanged;
				assembler.QueueChanged += assembler_QueueChanged;
				assembler.CurrentProgressChanged += assembler_CurrentProgressChanged;
				assembler.CurrentStateChanged += assembler_CurrentStateChanged;
				assembler.InputInventory.ContentsChanged += InputInventory_ContentsChanged;
				assembler.OutputInventory.ContentsChanged += OutputInventory_ContentsChanged;
			}
		}

		private void UnregisterAssemblerEvents(MyAssembler assembler)
		{
			if (assembler != null)
			{
				m_selectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
				m_selectedAssembler.QueueChanged -= assembler_QueueChanged;
				m_selectedAssembler.CurrentProgressChanged -= assembler_CurrentProgressChanged;
				m_selectedAssembler.CurrentStateChanged -= assembler_CurrentStateChanged;
				if (assembler.InputInventory != null)
				{
					assembler.InputInventory.ContentsChanged -= InputInventory_ContentsChanged;
				}
				if (m_selectedAssembler.OutputInventory != null)
				{
					m_selectedAssembler.OutputInventory.ContentsChanged -= OutputInventory_ContentsChanged;
				}
			}
		}

		internal void Close()
		{
			UnregisterEvents();
			UnregisterAssemblerEvents(m_selectedAssembler);
			m_assemblersByKey.Clear();
			m_blueprintButtonGroup.Clear();
			m_modeButtonGroup.Clear();
			m_selectedAssembler = null;
			m_controlsParent = null;
			m_terminalSystem = null;
			m_comboboxAssemblers = null;
			m_dragAndDrop = null;
			m_dragAndDropInfo = null;
		}

		private void SelectAndShowAssembler(MyAssembler assembler)
		{
			UnregisterAssemblerEvents(m_selectedAssembler);
			m_selectedAssembler = assembler;
			RegisterAssemblerEvents(assembler);
			RefreshRepeatMode(assembler.RepeatEnabled);
			RefreshSlaveMode(assembler.IsSlave);
			SelectModeButton(assembler);
			UpdateBlueprintClassGui();
			m_blueprintsSearchBox.SearchText = string.Empty;
			RefreshQueue();
			RefreshInventory();
			RefreshProgress();
			RefreshAssemblerModeView();
		}

		private void RefreshInventory()
		{
			m_inventoryGrid.Clear();
			foreach (MyPhysicalInventoryItem item in m_selectedAssembler.OutputInventory.GetItems())
			{
				m_inventoryGrid.Add(MyGuiControlInventoryOwner.CreateInventoryGridItem(item));
			}
			int count = m_selectedAssembler.OutputInventory.GetItems().Count;
			m_inventoryGrid.RowsCount = Math.Max(1 + count / m_inventoryGrid.ColumnsCount, INVENTORY_GRID_ROWS);
		}

		private void RefreshQueue()
		{
			m_queueGrid.Clear();
			int num = 0;
			foreach (MyProductionBlock.QueueItem item in m_selectedAssembler.Queue)
			{
				m_textCache.Clear().Append((int)item.Amount).Append('x');
				MyGuiGridItem myGuiGridItem = new MyGuiGridItem(item.Blueprint.Icons, null, item.Blueprint.DisplayNameText, item);
				myGuiGridItem.AddText(m_textCache, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				if (MyFakes.SHOW_PRODUCTION_QUEUE_ITEM_IDS)
				{
					m_textCache.Clear().Append((int)item.ItemId);
					myGuiGridItem.AddText(m_textCache, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				}
				m_queueGrid.Add(myGuiGridItem);
				num++;
			}
			m_queueGrid.RowsCount = Math.Max(1 + num / m_queueGrid.ColumnsCount, QUEUE_GRID_ROWS);
			RefreshProgress();
		}

		private void RefreshBlueprints()
		{
			if (m_blueprintButtonGroup.SelectedButton != null)
			{
				MyBlueprintClassDefinition myBlueprintClassDefinition = m_blueprintButtonGroup.SelectedButton.UserData as MyBlueprintClassDefinition;
				if (myBlueprintClassDefinition != null)
				{
					m_blueprintsGrid.Clear();
					bool flag = !string.IsNullOrEmpty(m_blueprintsSearchBox.SearchText);
					int num = 0;
					foreach (MyBlueprintDefinitionBase item2 in myBlueprintClassDefinition)
					{
						if (item2.Public && (!flag || StringExtensions.Contains(item2.DisplayNameText, m_blueprintsSearchBox.SearchText, StringComparison.OrdinalIgnoreCase)))
						{
							MyGuiGridItem item = new MyGuiGridItem(item2.Icons, null, item2.DisplayNameText, item2);
							m_blueprintsGrid.Add(item);
							num++;
						}
					}
					m_blueprintsGrid.RowsCount = Math.Max(1 + num / m_blueprintsGrid.ColumnsCount, BLUEPRINT_GRID_ROWS);
					RefreshBlueprintGridColors();
				}
			}
		}

		private void RefreshBlueprintGridColors()
		{
			m_selectedAssembler.InventoryOwnersDirty = true;
			for (int i = 0; i < m_blueprintsGrid.RowsCount; i++)
			{
				for (int j = 0; j < m_blueprintsGrid.ColumnsCount; j++)
				{
					MyGuiGridItem myGuiGridItem = m_blueprintsGrid.TryGetItemAt(i, j);
					if (myGuiGridItem == null)
					{
						continue;
					}
					MyBlueprintDefinitionBase myBlueprintDefinitionBase = myGuiGridItem.UserData as MyBlueprintDefinitionBase;
					if (myBlueprintDefinitionBase == null)
					{
						continue;
					}
					myGuiGridItem.IconColorMask = Vector4.One;
					if (m_selectedAssembler != null)
					{
						AddComponentPrerequisites(myBlueprintDefinitionBase, 1, m_requiredCountCache);
						if (CurrentAssemblerMode == AssemblerMode.Assembling)
						{
							foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> item in m_requiredCountCache)
							{
								if (!m_selectedAssembler.CheckConveyorResources(item.Value, item.Key))
								{
									myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
									break;
								}
							}
						}
						else if (CurrentAssemblerMode == AssemblerMode.Disassembling && !m_selectedAssembler.CheckConveyorResources(null, myBlueprintDefinitionBase.Results[0].Id))
						{
							myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
						}
						m_requiredCountCache.Clear();
					}
				}
			}
		}

		private void RefreshProgress()
		{
			int currentItemIndex = m_selectedAssembler.CurrentItemIndex;
			MyGuiGridItem myGuiGridItem = m_queueGrid.TryGetItemAt(currentItemIndex);
			if (myGuiGridItem != null)
			{
				MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)myGuiGridItem.UserData;
				myGuiGridItem.OverlayPercent = MathHelper.Clamp(m_selectedAssembler.CurrentProgress, 0f, 1f);
				myGuiGridItem.ToolTip.ToolTips.Clear();
				m_textCache.Clear().AppendFormat("{0}: {1}%", queueItem.Blueprint.DisplayNameText, (int)(m_selectedAssembler.CurrentProgress * 100f));
				myGuiGridItem.ToolTip.AddToolTip(m_textCache.ToString());
			}
			for (int i = 0; i < m_queueGrid.GetItemsCount(); i++)
			{
				myGuiGridItem = m_queueGrid.TryGetItemAt(i);
				if (myGuiGridItem == null)
				{
					break;
				}
				if (i < currentItemIndex)
				{
					myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
					myGuiGridItem.OverlayColorMask = Color.Red.ToVector4();
					myGuiGridItem.ToolTip.ToolTips.Clear();
					myGuiGridItem.ToolTip.AddToolTip(GetAssemblerStateText(MyAssembler.StateEnum.MissingItems), 0.7f, "Red");
					continue;
				}
				myGuiGridItem.IconColorMask = ((m_selectedAssembler.CurrentState == MyAssembler.StateEnum.Ok) ? Color.White.ToVector4() : ERROR_ICON_COLOR_MASK);
				myGuiGridItem.OverlayColorMask = ((m_selectedAssembler.CurrentState == MyAssembler.StateEnum.Ok) ? Color.White.ToVector4() : Color.Red.ToVector4());
				if (i != currentItemIndex)
				{
					myGuiGridItem.ToolTip.ToolTips.Clear();
				}
				if (m_selectedAssembler.CurrentState != 0)
				{
					myGuiGridItem.ToolTip.AddToolTip(GetAssemblerStateText(m_selectedAssembler.CurrentState), 0.7f, (m_selectedAssembler.CurrentState == MyAssembler.StateEnum.Ok) ? "White" : "Red");
				}
			}
		}

		private void RefreshAssemblerModeView()
		{
			bool flag = CurrentAssemblerMode == AssemblerMode.Assembling;
			bool repeatEnabled = m_selectedAssembler.RepeatEnabled;
			m_blueprintsArea.Enabled = true;
			m_blueprintsBgPanel.Enabled = true;
			m_blueprintsLabel.Enabled = true;
			foreach (MyGuiControlRadioButton item in m_blueprintButtonGroup)
			{
				item.Enabled = true;
			}
			m_materialsLabel.Text = (flag ? MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_RequiredAndAvailable) : MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_GainedAndAvailable));
			m_queueGrid.Enabled = (flag || !repeatEnabled);
			m_disassembleAllButton.Visible = (!flag && !repeatEnabled);
			RefreshBlueprintGridColors();
		}

		private void RefreshRepeatMode(bool repeatModeEnabled)
		{
			if (m_selectedAssembler.IsSlave && repeatModeEnabled)
			{
				RefreshSlaveMode(slaveModeEnabled: false);
			}
			m_selectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
			m_selectedAssembler.RequestRepeatEnabled(repeatModeEnabled);
			m_selectedAssembler.CurrentModeChanged += assembler_CurrentModeChanged;
			m_repeatCheckbox.IsCheckedChanged = null;
			m_repeatCheckbox.IsChecked = m_selectedAssembler.RepeatEnabled;
			m_repeatCheckbox.IsCheckedChanged = repeatCheckbox_IsCheckedChanged;
			m_repeatCheckbox.Visible = m_selectedAssembler.SupportsAdvancedFunctions;
		}

		private void RefreshSlaveMode(bool slaveModeEnabled)
		{
			if (m_selectedAssembler.RepeatEnabled && slaveModeEnabled)
			{
				RefreshRepeatMode(repeatModeEnabled: false);
			}
			if (m_selectedAssembler.DisassembleEnabled)
			{
				m_slaveCheckbox.Enabled = false;
				m_slaveCheckbox.Visible = false;
			}
			if (!m_selectedAssembler.DisassembleEnabled)
			{
				m_slaveCheckbox.Enabled = true;
				m_slaveCheckbox.Visible = true;
			}
			m_selectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
			m_selectedAssembler.IsSlave = slaveModeEnabled;
			m_selectedAssembler.CurrentModeChanged += assembler_CurrentModeChanged;
			m_slaveCheckbox.IsCheckedChanged = null;
			m_slaveCheckbox.IsChecked = m_selectedAssembler.IsSlave;
			m_slaveCheckbox.IsCheckedChanged = slaveCheckbox_IsCheckedChanged;
			if (!m_selectedAssembler.SupportsAdvancedFunctions)
			{
				m_slaveCheckbox.Visible = false;
			}
		}

		private void EnqueueBlueprint(MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			m_blueprintCache.Clear();
			blueprint.GetBlueprints(m_blueprintCache);
			foreach (MyBlueprintDefinitionBase.ProductionInfo item in m_blueprintCache)
			{
				m_selectedAssembler.InsertQueueItemRequest(-1, item.Blueprint, item.Amount * amount);
			}
			m_blueprintCache.Clear();
		}

		private void ShowBlueprintComponents(MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			m_materialsList.Clear();
			if (blueprint != null)
			{
				AddComponentPrerequisites(blueprint, amount, m_requiredCountCache);
				FillMaterialList(m_requiredCountCache);
				m_requiredCountCache.Clear();
			}
		}

		private void FillMaterialList(Dictionary<MyDefinitionId, MyFixedPoint> materials)
		{
			bool flag = CurrentAssemblerMode == AssemblerMode.Disassembling;
			foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> material in materials)
			{
				MyFixedPoint itemAmount = m_selectedAssembler.InventoryAggregate.GetItemAmount(material.Key);
				string font = (flag || material.Value <= itemAmount) ? "White" : "Red";
				m_materialsList.Add(material.Key, (double)material.Value, (double)itemAmount, font);
			}
		}

		private void AddComponentPrerequisites(MyBlueprintDefinitionBase blueprint, MyFixedPoint multiplier, Dictionary<MyDefinitionId, MyFixedPoint> outputAmounts)
		{
			MyFixedPoint b = (MyFixedPoint)(1f / ((m_selectedAssembler != null) ? m_selectedAssembler.GetEfficiencyMultiplierForBlueprint(blueprint) : MySession.Static.AssemblerEfficiencyMultiplier));
			MyBlueprintDefinitionBase.Item[] prerequisites = blueprint.Prerequisites;
			for (int i = 0; i < prerequisites.Length; i++)
			{
				MyBlueprintDefinitionBase.Item item = prerequisites[i];
				if (!outputAmounts.ContainsKey(item.Id))
				{
					outputAmounts[item.Id] = 0;
				}
				outputAmounts[item.Id] += item.Amount * multiplier * b;
			}
		}

		private void StartDragging(MyDropHandleType dropHandlingType, MyGuiControlGrid gridControl, ref MyGuiControlGrid.EventArgs args)
		{
			m_dragAndDropInfo = new MyDragAndDropInfo();
			m_dragAndDropInfo.Grid = gridControl;
			m_dragAndDropInfo.ItemIndex = args.ItemIndex;
			MyGuiGridItem itemAt = m_dragAndDropInfo.Grid.GetItemAt(m_dragAndDropInfo.ItemIndex);
			m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, m_dragAndDropInfo, includeTooltip: false);
		}

		private void SelectModeButton(MyAssembler assembler)
		{
			bool supportsAdvancedFunctions = assembler.SupportsAdvancedFunctions;
			foreach (MyGuiControlRadioButton item in m_modeButtonGroup)
			{
				item.Visible = supportsAdvancedFunctions;
			}
			AssemblerMode key = assembler.DisassembleEnabled ? AssemblerMode.Disassembling : AssemblerMode.Assembling;
			m_modeButtonGroup.SelectByKey((int)key);
		}

		private void RefreshMaterialsPreview()
		{
			m_materialsList.Clear();
			if (m_blueprintsGrid.MouseOverItem != null)
			{
				ShowBlueprintComponents((MyBlueprintDefinitionBase)m_blueprintsGrid.MouseOverItem.UserData, 1);
			}
			else if (m_inventoryGrid.MouseOverItem != null && CurrentAssemblerMode == AssemblerMode.Disassembling)
			{
				MyPhysicalInventoryItem myPhysicalInventoryItem = (MyPhysicalInventoryItem)m_inventoryGrid.MouseOverItem.UserData;
				if (MyDefinitionManager.Static.HasBlueprint(myPhysicalInventoryItem.Content.GetId()))
				{
					ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(myPhysicalInventoryItem.Content.GetId()), 1);
				}
			}
			else if (m_queueGrid.MouseOverItem != null)
			{
				MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)m_queueGrid.MouseOverItem.UserData;
				ShowBlueprintComponents(queueItem.Blueprint, queueItem.Amount);
			}
			else if (m_selectedAssembler != null)
			{
				foreach (MyProductionBlock.QueueItem item in m_selectedAssembler.Queue)
				{
					AddComponentPrerequisites(item.Blueprint, item.Amount, m_requiredCountCache);
				}
				FillMaterialList(m_requiredCountCache);
			}
			m_requiredCountCache.Clear();
		}

		private static string GetAssemblerStateText(MyAssembler.StateEnum state)
		{
			MyStringId id = MySpaceTexts.Blank;
			switch (state)
			{
			case MyAssembler.StateEnum.Ok:
				id = MySpaceTexts.Blank;
				break;
			case MyAssembler.StateEnum.Disabled:
				id = MySpaceTexts.AssemblerState_Disabled;
				break;
			case MyAssembler.StateEnum.NotWorking:
				id = MySpaceTexts.AssemblerState_NotWorking;
				break;
			case MyAssembler.StateEnum.MissingItems:
				id = MySpaceTexts.AssemblerState_MissingItems;
				break;
			case MyAssembler.StateEnum.NotEnoughPower:
				id = MySpaceTexts.AssemblerState_NotEnoughPower;
				break;
			case MyAssembler.StateEnum.InventoryFull:
				id = MySpaceTexts.AssemblerState_InventoryFull;
				break;
			}
			return MyTexts.GetString(id);
		}

		private void blueprintButtonGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			RefreshBlueprints();
		}

		private void Assemblers_ItemSelected()
		{
			if (m_assemblersByKey.Count > 0 && m_assemblersByKey.ContainsKey((int)m_comboboxAssemblers.GetSelectedKey()))
			{
				SelectAndShowAssembler(m_assemblersByKey[(int)m_comboboxAssemblers.GetSelectedKey()]);
			}
		}

		private void assembler_CurrentModeChanged(MyAssembler assembler)
		{
			SelectModeButton(assembler);
			RefreshRepeatMode(assembler.RepeatEnabled);
			RefreshSlaveMode(assembler.IsSlave);
			RefreshProgress();
			RefreshAssemblerModeView();
			RefreshMaterialsPreview();
		}

		private void assembler_QueueChanged(MyProductionBlock block)
		{
			RefreshQueue();
			RefreshMaterialsPreview();
		}

		private void assembler_CurrentProgressChanged(MyAssembler assembler)
		{
			RefreshProgress();
		}

		private void assembler_CurrentStateChanged(MyAssembler obj)
		{
			RefreshProgress();
		}

		private void InputInventory_ContentsChanged(MyInventoryBase obj)
		{
			if (CurrentAssemblerMode == AssemblerMode.Assembling)
			{
				RefreshBlueprintGridColors();
			}
			RefreshMaterialsPreview();
		}

		private void OutputInventory_ContentsChanged(MyInventoryBase obj)
		{
			RefreshInventory();
			RefreshMaterialsPreview();
		}

		private void OnSearchTextChanged(string text)
		{
			RefreshBlueprints();
		}

		private void blueprintsGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			MyGuiGridItem itemAt = control.GetItemAt(args.ItemIndex);
			if (itemAt != null)
			{
				MyBlueprintDefinitionBase blueprint = (MyBlueprintDefinitionBase)itemAt.UserData;
				int num = 1;
				if (MyInput.Static.IsAnyCtrlKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
				{
					num *= 10;
				}
				if (MyInput.Static.IsAnyShiftKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
				{
					num *= 100;
				}
				EnqueueBlueprint(blueprint, num);
			}
		}

		private void inventoryGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if (CurrentAssemblerMode == AssemblerMode.Assembling)
			{
				return;
			}
			MyGuiGridItem itemAt = control.GetItemAt(args.ItemIndex);
			if (itemAt != null)
			{
				MyPhysicalInventoryItem myPhysicalInventoryItem = (MyPhysicalInventoryItem)itemAt.UserData;
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(myPhysicalInventoryItem.Content.GetId());
				if (myBlueprintDefinitionBase != null)
				{
					int i = ((!MyInput.Static.IsAnyShiftKeyPressed()) ? 1 : 100) * ((!MyInput.Static.IsAnyCtrlKeyPressed()) ? 1 : 10);
					EnqueueBlueprint(myBlueprintDefinitionBase, i);
				}
			}
		}

		private void queueGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if ((CurrentAssemblerMode != AssemblerMode.Disassembling || !m_selectedAssembler.RepeatEnabled) && args.Button == MySharedButtonsEnum.Secondary)
			{
				m_selectedAssembler.RemoveQueueItemRequest(args.ItemIndex, -1);
			}
		}

		private void queueGrid_ItemDragged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			StartDragging(MyDropHandleType.MouseRelease, control, ref args);
		}

		private void dragDrop_OnItemDropped(object sender, MyDragAndDropEventArgs eventArgs)
		{
			if (m_selectedAssembler != null && eventArgs.DropTo != null)
			{
				MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)eventArgs.Item.UserData;
				m_selectedAssembler.MoveQueueItemRequest(queueItem.ItemId, eventArgs.DropTo.ItemIndex);
			}
			m_dragAndDropInfo = null;
		}

		private void blueprintsGrid_MouseOverIndexChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			RefreshMaterialsPreview();
		}

		private void inventoryGrid_MouseOverIndexChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if (CurrentAssemblerMode != 0)
			{
				RefreshMaterialsPreview();
			}
		}

		private void queueGrid_MouseOverIndexChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			RefreshMaterialsPreview();
		}

		private void modeButtonGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			m_selectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
			bool flag = obj.SelectedButton.Key == 1;
			m_selectedAssembler.RequestDisassembleEnabled(flag);
			if (flag)
			{
				m_slaveCheckbox.Enabled = false;
				m_slaveCheckbox.Visible = false;
			}
			if (!flag && m_selectedAssembler.SupportsAdvancedFunctions)
			{
				m_slaveCheckbox.Enabled = true;
				m_slaveCheckbox.Visible = true;
			}
			m_selectedAssembler.CurrentModeChanged += assembler_CurrentModeChanged;
			m_repeatCheckbox.IsCheckedChanged = null;
			m_repeatCheckbox.IsChecked = m_selectedAssembler.RepeatEnabled;
			m_repeatCheckbox.IsCheckedChanged = repeatCheckbox_IsCheckedChanged;
			m_slaveCheckbox.IsCheckedChanged = null;
			m_slaveCheckbox.IsChecked = m_selectedAssembler.IsSlave;
			m_slaveCheckbox.IsCheckedChanged = slaveCheckbox_IsCheckedChanged;
			RefreshProgress();
			RefreshAssemblerModeView();
		}

		private void repeatCheckbox_IsCheckedChanged(MyGuiControlCheckbox control)
		{
			RefreshRepeatMode(control.IsChecked);
			RefreshAssemblerModeView();
		}

		private void slaveCheckbox_IsCheckedChanged(MyGuiControlCheckbox control)
		{
			RefreshSlaveMode(control.IsChecked);
			RefreshAssemblerModeView();
		}

		private void controlPanelButton_ButtonClicked(MyGuiControlButton control)
		{
			MyGuiScreenTerminal.SwitchToControlPanelBlock(m_selectedAssembler);
		}

		private void inventoryButton_ButtonClicked(MyGuiControlButton control)
		{
			MyGuiScreenTerminal.SwitchToInventory(m_selectedAssembler);
		}

		private void TerminalSystem_BlockAdded(MyTerminalBlock obj)
		{
			MyAssembler myAssembler = obj as MyAssembler;
			if (myAssembler != null)
			{
				if (m_assemblersByKey.Count == 0)
				{
					HideError(m_controlsParent);
				}
				int num = m_assemblerKeyCounter++;
				m_assemblersByKey.Add(num, myAssembler);
				m_comboboxAssemblers.AddItem(num, myAssembler.CustomName);
				if (m_assemblersByKey.Count == 1)
				{
					m_comboboxAssemblers.SelectItemByIndex(0);
				}
				myAssembler.CustomNameChanged += assembler_CustomNameChanged;
			}
		}

		private void TerminalSystem_BlockRemoved(MyTerminalBlock obj)
		{
			MyAssembler myAssembler = obj as MyAssembler;
			if (myAssembler == null)
			{
				return;
			}
			myAssembler.CustomNameChanged -= assembler_CustomNameChanged;
			int? num = null;
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				if (item.Value == myAssembler)
				{
					num = item.Key;
					break;
				}
			}
			if (num.HasValue)
			{
				m_assemblersByKey.Remove(num.Value);
				m_comboboxAssemblers.RemoveItem(num.Value);
			}
			if (myAssembler == m_selectedAssembler)
			{
				if (m_assemblersByKey.Count > 0)
				{
					m_comboboxAssemblers.SelectItemByIndex(0);
				}
				else
				{
					ShowError(MySpaceTexts.ScreenTerminalError_NoAssemblers, m_controlsParent);
				}
			}
		}

		private void assembler_CustomNameChanged(MyTerminalBlock block)
		{
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				if (item.Value == block)
				{
					m_comboboxAssemblers.TryGetItemByKey(item.Key).Value.Clear().AppendStringBuilder(block.CustomName);
				}
			}
		}

		private void disassembleAllButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (CurrentAssemblerMode == AssemblerMode.Disassembling && !m_selectedAssembler.RepeatEnabled)
			{
				m_selectedAssembler.RequestDisassembleAll();
			}
		}
	}
}
