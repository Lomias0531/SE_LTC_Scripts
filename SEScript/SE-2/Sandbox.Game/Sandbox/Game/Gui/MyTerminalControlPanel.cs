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
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalControlPanel : MyTerminalController
	{
		private static readonly MyTerminalComparer m_nameComparer = new MyTerminalComparer();

		private IMyGuiControlsParent m_controlsParent;

		private MyGuiControlListbox m_blockListbox;

		private MyGuiControlLabel m_blockNameLabel;

		private MyGuiControlBase m_blockControl;

		private MyGridTerminalSystem m_terminalSystem;

		private List<MyBlockGroup> m_currentGroups = new List<MyBlockGroup>();

		private MyBlockGroup m_tmpGroup;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlTextbox m_groupName;

		private MyGuiControlButton m_groupSave;

		private MyGuiControlButton m_showAll;

		private MyGuiControlButton m_groupDelete;

		private List<MyBlockGroup> m_oldGroups = new List<MyBlockGroup>();

		private MyTerminalBlock m_originalBlock;

		private static bool m_showAllTerminalBlocks = false;

		private MyGridColorHelper m_colorHelper;

		private MyPlayer m_controller;

		private static HashSet<Type> tmpBlockTypes = new HashSet<Type>();

		private HashSet<MyTerminalBlock> CurrentBlocks => m_tmpGroup.Blocks;

		public MyGridTerminalSystem TerminalSystem => m_terminalSystem;

		public void Init(IMyGuiControlsParent controlsParent, MyPlayer controller, MyCubeGrid grid, MyTerminalBlock currentBlock, MyGridColorHelper colorHelper)
		{
			m_controlsParent = controlsParent;
			m_controller = controller;
			m_colorHelper = colorHelper;
			if (grid == null)
			{
				foreach (MyGuiControlBase control2 in controlsParent.Controls)
				{
					control2.Visible = false;
				}
				MyGuiControlLabel control = MyGuiScreenTerminal.CreateErrorLabel(MySpaceTexts.ScreenTerminalError_ShipNotConnected, "ErrorMessage");
				controlsParent.Controls.Add(control);
				return;
			}
			m_terminalSystem = grid.GridSystems.TerminalSystem;
			m_tmpGroup = new MyBlockGroup();
			m_searchBox = (MyGuiControlSearchBox)m_controlsParent.Controls.GetControlByName("FunctionalBlockSearch");
			m_searchBox.OnTextChanged += blockSearch_TextChanged;
			m_blockListbox = (MyGuiControlListbox)m_controlsParent.Controls.GetControlByName("FunctionalBlockListbox");
			m_blockNameLabel = (MyGuiControlLabel)m_controlsParent.Controls.GetControlByName("BlockNameLabel");
			m_blockNameLabel.Text = "";
			m_groupName = (MyGuiControlTextbox)m_controlsParent.Controls.GetControlByName("GroupName");
			m_groupName.TextChanged += m_groupName_TextChanged;
			m_groupName.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroup));
			m_groupName.ShowTooltipWhenDisabled = true;
			m_showAll = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("ShowAll");
			m_showAll.Selected = m_showAllTerminalBlocks;
			m_showAll.ButtonClicked += showAll_Clicked;
			m_showAll.SetToolTip(MySpaceTexts.Terminal_ShowAllInTerminal);
			m_showAll.IconRotation = 0f;
			m_showAll.Icon = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\button_hide.dds",
				Highlight = "Textures\\GUI\\Controls\\button_unhide.dds",
				SizePx = new Vector2(40f, 40f)
			};
			m_showAll.Size = new Vector2(0f, 0f);
			m_showAll.HighlightType = MyGuiControlHighlightType.FORCED;
			m_groupSave = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("GroupSave");
			m_groupSave.TextEnum = MySpaceTexts.TerminalButton_GroupSave;
			m_groupSave.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_groupSave.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			m_groupSave.ButtonClicked += groupSave_ButtonClicked;
			m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSave));
			m_groupSave.ShowTooltipWhenDisabled = true;
			m_groupDelete = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("GroupDelete");
			m_groupDelete.ButtonClicked += groupDelete_ButtonClicked;
			m_groupDelete.ShowTooltipWhenDisabled = true;
			m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDeleteDisabled));
			m_groupDelete.Enabled = false;
			m_blockListbox.ItemsSelected += blockListbox_ItemSelected;
			m_originalBlock = currentBlock;
			MyTerminalBlock[] selectedBlocks = null;
			if (m_originalBlock != null)
			{
				selectedBlocks = new MyTerminalBlock[1]
				{
					m_originalBlock
				};
			}
			RefreshBlockList(selectedBlocks);
			m_terminalSystem.BlockAdded += TerminalSystem_BlockAdded;
			m_terminalSystem.BlockRemoved += TerminalSystem_BlockRemoved;
			m_terminalSystem.BlockManipulationFinished += TerminalSystem_BlockManipulationFinished;
			m_terminalSystem.GroupAdded += TerminalSystem_GroupAdded;
			m_terminalSystem.GroupRemoved += TerminalSystem_GroupRemoved;
			blockSearch_TextChanged(m_searchBox.SearchText);
			m_blockListbox.ScrollToFirstSelection();
		}

		private void m_groupName_TextChanged(MyGuiControlTextbox obj)
		{
			if (string.IsNullOrEmpty(obj.Text) || CurrentBlocks.Count == 0)
			{
				m_groupSave.Enabled = false;
				m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSaveDisabled));
			}
			else
			{
				m_groupSave.Enabled = true;
				m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSave));
			}
		}

		private void TerminalSystem_GroupRemoved(MyBlockGroup group)
		{
			if (m_blockListbox != null)
			{
				foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
				{
					if (item.UserData == group)
					{
						m_blockListbox.Items.Remove(item);
						break;
					}
				}
			}
		}

		private void TerminalSystem_GroupAdded(MyBlockGroup group)
		{
			if (m_blockListbox != null)
			{
				AddGroupToList(group, 0);
			}
		}

		private void groupDelete_ButtonClicked(MyGuiControlButton obj)
		{
			bool flag = false;
			foreach (MyBlockGroup currentGroup in m_currentGroups)
			{
				foreach (MyTerminalBlock block in currentGroup.Blocks)
				{
					if (!block.HasLocalPlayerAccess())
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCannotDeleteGroup)));
				return;
			}
			while (m_currentGroups.Count > 0)
			{
				m_terminalSystem.RemoveGroup(m_currentGroups[0], fireEvent: true);
			}
		}

		private void showAll_Clicked(MyGuiControlButton obj)
		{
			m_showAllTerminalBlocks = !m_showAllTerminalBlocks;
			m_showAll.Selected = m_showAllTerminalBlocks;
			List<MyGuiControlListbox.Item> selectedItems = m_blockListbox.SelectedItems;
			MyTerminalBlock[] array = new MyTerminalBlock[selectedItems.Count];
			for (int i = 0; i < selectedItems.Count; i++)
			{
				if (selectedItems[i].UserData is MyTerminalBlock)
				{
					array[i] = (MyTerminalBlock)selectedItems[i].UserData;
				}
			}
			ClearBlockList();
			PopulateBlockList(array);
			m_blockListbox.ScrollToolbarToTop();
			blockSearch_TextChanged(m_searchBox.SearchText);
		}

		private void groupSave_ButtonClicked(MyGuiControlButton obj)
		{
			bool flag = false;
			foreach (MyTerminalBlock block in m_tmpGroup.Blocks)
			{
				if (!block.HasLocalPlayerAccess())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCannotCreateGroup)));
			}
			else if (m_groupName.Text != "")
			{
				m_currentGroups.Clear();
				m_tmpGroup.Name.Clear().Append(m_groupName.Text);
				m_tmpGroup = m_terminalSystem.AddUpdateGroup(m_tmpGroup, fireEvent: true, modify: true);
				m_currentGroups.Add(m_tmpGroup);
				m_tmpGroup = new MyBlockGroup();
				CurrentBlocks.UnionWith(m_currentGroups[0].Blocks);
				SelectBlocks();
			}
		}

		private void blockSearch_TextChanged(string text)
		{
			if (m_blockListbox != null)
			{
				if (text != "")
				{
					string[] array = text.Split(new char[1]
					{
						' '
					});
					foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
					{
						bool flag = true;
						if (item.UserData is MyTerminalBlock)
						{
							flag = (((MyTerminalBlock)item.UserData).ShowInTerminal || m_showAllTerminalBlocks || item.UserData == m_originalBlock);
						}
						if (flag)
						{
							string text2 = item.Text.ToString().ToLower();
							string[] array2 = array;
							foreach (string text3 in array2)
							{
								if (!text2.Contains(text3.ToLower()))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								item.Visible = true;
							}
							else
							{
								item.Visible = false;
							}
						}
					}
				}
				else
				{
					foreach (MyGuiControlListbox.Item item2 in m_blockListbox.Items)
					{
						if (item2.UserData is MyTerminalBlock)
						{
							MyTerminalBlock myTerminalBlock = (MyTerminalBlock)item2.UserData;
							item2.Visible = (myTerminalBlock.ShowInTerminal || m_showAllTerminalBlocks || myTerminalBlock == m_originalBlock);
						}
						else
						{
							item2.Visible = true;
						}
					}
				}
				m_blockListbox.ScrollToolbarToTop();
			}
		}

		private void TerminalSystem_BlockAdded(MyTerminalBlock obj)
		{
			AddBlockToList(obj);
		}

		private void TerminalSystem_BlockRemoved(MyTerminalBlock obj)
		{
			obj.CustomNameChanged -= block_CustomNameChanged;
			obj.PropertiesChanged -= block_CustomNameChanged;
			if (m_blockListbox != null && (obj.ShowInTerminal || m_showAllTerminalBlocks))
			{
				m_blockListbox.Remove((MyGuiControlListbox.Item item) => item.UserData == obj);
			}
		}

		private void TerminalSystem_BlockManipulationFinished()
		{
			blockSearch_TextChanged(m_searchBox.SearchText);
		}

		public void Close()
		{
			if (m_terminalSystem != null)
			{
				if (m_blockListbox != null)
				{
					ClearBlockList();
					m_blockListbox.ItemsSelected -= blockListbox_ItemSelected;
				}
				m_terminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
				m_terminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
				m_terminalSystem.BlockManipulationFinished -= TerminalSystem_BlockManipulationFinished;
				m_terminalSystem.GroupAdded -= TerminalSystem_GroupAdded;
				m_terminalSystem.GroupRemoved -= TerminalSystem_GroupRemoved;
			}
			if (m_tmpGroup != null)
			{
				m_tmpGroup.Blocks.Clear();
			}
			if (m_showAll != null)
			{
				m_showAll.ButtonClicked -= showAll_Clicked;
			}
			m_controlsParent = null;
			m_blockListbox = null;
			m_blockNameLabel = null;
			m_terminalSystem = null;
			m_currentGroups.Clear();
		}

		public void RefreshBlockList(MyTerminalBlock[] selectedBlocks = null)
		{
			if (m_blockListbox != null)
			{
				ClearBlockList();
				PopulateBlockList(selectedBlocks);
			}
		}

		public void ClearBlockList()
		{
			if (m_blockListbox != null)
			{
				foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
				{
					if (item.UserData is MyTerminalBlock)
					{
						MyTerminalBlock obj = (MyTerminalBlock)item.UserData;
						obj.CustomNameChanged -= block_CustomNameChanged;
						obj.PropertiesChanged -= block_CustomNameChanged;
						obj.ShowInTerminalChanged -= block_ShowInTerminalChanged;
					}
				}
				m_blockListbox.Items.Clear();
			}
		}

		public void PopulateBlockList(MyTerminalBlock[] selectedBlocks = null)
		{
			if (m_terminalSystem == null)
			{
				return;
			}
			if (m_terminalSystem.BlockGroups == null)
			{
				MySandboxGame.Log.WriteLine("m_terminalSystem.BlockGroups is null");
			}
			if (!m_terminalSystem.Blocks.IsValid)
			{
				MySandboxGame.Log.WriteLine("m_terminalSystem.Blocks.IsValid is false");
			}
			if (CurrentBlocks == null)
			{
				MySandboxGame.Log.WriteLine("CurrentBlocks is null");
			}
			if (m_blockListbox == null)
			{
				MySandboxGame.Log.WriteLine("m_blockListbox is null");
			}
			MyBlockGroup[] array = m_terminalSystem.BlockGroups.ToArray();
			Array.Sort(array, MyTerminalComparer.Static);
			MyBlockGroup[] array2 = array;
			foreach (MyBlockGroup group in array2)
			{
				AddGroupToList(group);
			}
			MyTerminalBlock[] array3 = m_terminalSystem.Blocks.ToArray();
			Array.Sort(array3, MyTerminalComparer.Static);
			m_blockListbox.SelectedItems.Clear();
			MyTerminalBlock[] array4 = array3;
			foreach (MyTerminalBlock myTerminalBlock in array4)
			{
				AddBlockToList(myTerminalBlock, myTerminalBlock == m_originalBlock || myTerminalBlock.ShowInTerminal || m_showAllTerminalBlocks);
			}
			if (selectedBlocks == null)
			{
				if (CurrentBlocks.Count > 0)
				{
					SelectBlocks();
				}
				else
				{
					foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
					{
						if (item.UserData is MyTerminalBlock)
						{
							SelectBlocks(new MyTerminalBlock[1]
							{
								(MyTerminalBlock)item.UserData
							});
							break;
						}
					}
				}
			}
			else
			{
				SelectBlocks(selectedBlocks);
			}
		}

		private bool IsGeneric(MyBlockGroup group)
		{
			try
			{
				bool flag = true;
				foreach (MyTerminalBlock block in group.Blocks)
				{
					flag = (flag && block is MyFunctionalBlock);
					tmpBlockTypes.Add(block.GetType());
				}
				if (tmpBlockTypes.Count == 1)
				{
					return false;
				}
				return true;
			}
			finally
			{
				tmpBlockTypes.Clear();
			}
		}

		private void AddGroupToList(MyBlockGroup group, int? position = null)
		{
			foreach (MyGuiControlListbox.Item item2 in m_blockListbox.Items)
			{
				MyBlockGroup myBlockGroup = item2.UserData as MyBlockGroup;
				if (myBlockGroup != null && myBlockGroup.Name.CompareTo(group.Name) == 0)
				{
					m_blockListbox.Items.Remove(item2);
					break;
				}
			}
			MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(null, group.Name.ToString(), GetIconForGroup(group), group);
			item.Text.Clear().Append("*").AppendStringBuilder(group.Name)
				.Append("*");
			m_blockListbox.Add(item, position);
		}

		private string GetIconForBlock(MyTerminalBlock block)
		{
			if (block.BlockDefinition == null || block.BlockDefinition.Icons.IsNullOrEmpty())
			{
				return MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
			}
			return block.BlockDefinition.Icons[0];
		}

		private string GetIconForGroup(MyBlockGroup group)
		{
			if (group == null || IsGeneric(group))
			{
				return MyGuiConstants.TEXTURE_TERMINAL_GROUP;
			}
			MyTerminalBlock myTerminalBlock = group.Blocks.First();
			if (myTerminalBlock.BlockDefinition == null || myTerminalBlock.BlockDefinition.Icons.IsNullOrEmpty())
			{
				return MyGuiConstants.TEXTURE_TERMINAL_GROUP;
			}
			return myTerminalBlock.BlockDefinition.Icons[0];
		}

		private MyGuiControlListbox.Item AddBlockToList(MyTerminalBlock block, bool? visibility = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			block.GetTerminalName(stringBuilder);
			MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(null, stringBuilder.ToString(), GetIconForBlock(block), block);
			UpdateItemAppearance(block, item);
			block.CustomNameChanged += block_CustomNameChanged;
			block.PropertiesChanged += block_CustomNameChanged;
			block.ShowInTerminalChanged += block_ShowInTerminalChanged;
			if (visibility.HasValue)
			{
				item.Visible = visibility.Value;
			}
			m_blockListbox.Add(item);
			return item;
		}

		private void UpdateItemAppearance(MyTerminalBlock block, MyGuiControlListbox.Item item)
		{
			item.Text.Clear();
			block.GetTerminalName(item.Text);
			if (!block.IsFunctional)
			{
				item.ColorMask = Vector4.One;
				item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockIncomplete));
				item.FontOverride = "Red";
				return;
			}
			MyTerminalBlock.AccessRightsResult accessRightsResult;
			if ((accessRightsResult = block.HasPlayerAccessReason(m_controller.Identity.IdentityId)) != 0)
			{
				item.ColorMask = Vector4.One;
				if (accessRightsResult == MyTerminalBlock.AccessRightsResult.MissingDLC)
				{
					string[] dLCs = block.BlockDefinition.DLCs;
					for (int i = 0; i < dLCs.Length; i++)
					{
						if (MyDLCs.TryGetDLC(dLCs[i], out MyDLCs.MyDLC _))
						{
							item.Text.Append(" (").Append((object)MyTexts.Get(MyCommonTexts.RequiresAnyDlc)).Append(")");
						}
					}
				}
				else
				{
					item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
				}
				item.FontOverride = "Red";
				return;
			}
			if (!block.ShowInTerminal)
			{
				item.ColorMask = 0.6f * m_colorHelper.GetGridColor(block.CubeGrid).ToVector4();
				item.FontOverride = null;
				return;
			}
			if (block.IDModule == null && block.CubeGrid != null)
			{
				List<long> bigOwners = block.CubeGrid.BigOwners;
				if (bigOwners == null || bigOwners.Count != 0)
				{
					List<long> smallOwners = block.CubeGrid.SmallOwners;
					if ((smallOwners == null || smallOwners.Count != 0) && !block.HasLocalPlayerAdminUseTerminals() && !block.CubeGrid.SmallOwners.Contains(m_controller.Identity.IdentityId))
					{
						item.ColorMask = Vector4.One;
						item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
						item.FontOverride = "Red";
						return;
					}
				}
			}
			item.ColorMask = m_colorHelper.GetGridColor(block.CubeGrid).ToVector4();
			item.FontOverride = null;
		}

		private void block_CustomNameChanged(MyTerminalBlock obj)
		{
			if (m_blockListbox != null)
			{
				foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
				{
					if (item.UserData == obj)
					{
						UpdateItemAppearance(obj, item);
						break;
					}
				}
				if (CurrentBlocks.Count > 0 && CurrentBlocks.FirstElement() == obj)
				{
					m_blockNameLabel.Text = obj.CustomName.ToString();
				}
			}
		}

		public void SelectBlocks(MyTerminalBlock[] blocks)
		{
			m_tmpGroup.Blocks.Clear();
			m_tmpGroup.Blocks.UnionWith(blocks);
			m_currentGroups.Clear();
			CurrentBlocks.Clear();
			foreach (MyTerminalBlock myTerminalBlock in blocks)
			{
				if (myTerminalBlock != null)
				{
					CurrentBlocks.Add(myTerminalBlock);
				}
			}
			SelectBlocks();
		}

		private void SelectBlocks()
		{
			if (m_blockControl != null)
			{
				m_controlsParent.Controls.Remove(m_blockControl);
				m_blockControl = null;
			}
			m_blockNameLabel.Text = "";
			m_groupName.Text = "";
			if (m_currentGroups.Count == 1)
			{
				m_blockNameLabel.Text = m_currentGroups[0].Name.ToString();
				m_groupName.Text = m_blockNameLabel.Text;
			}
			if (CurrentBlocks.Count > 0)
			{
				if (CurrentBlocks.Count == 1)
				{
					m_blockNameLabel.Text = CurrentBlocks.FirstElement().CustomName.ToString();
				}
				m_blockControl = new MyGuiControlGenericFunctionalBlock(CurrentBlocks.ToArray());
				m_controlsParent.Controls.Add(m_blockControl);
				m_blockControl.Size = new Vector2(0.595f, 0.64f);
				m_blockControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_blockControl.Position = new Vector2(-0.1415f, -0.3f);
			}
			UpdateGroupControl();
			m_blockListbox.SelectedItems.Clear();
			foreach (MyTerminalBlock currentBlock in CurrentBlocks)
			{
				foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
				{
					if (item.UserData == currentBlock)
					{
						m_blockListbox.SelectedItems.Add(item);
						break;
					}
				}
			}
			foreach (MyBlockGroup currentGroup in m_currentGroups)
			{
				foreach (MyGuiControlListbox.Item item2 in m_blockListbox.Items)
				{
					if (item2.UserData == currentGroup)
					{
						m_blockListbox.SelectedItems.Add(item2);
						break;
					}
				}
			}
		}

		public void SelectAllBlocks()
		{
			if (m_blockListbox != null)
			{
				m_blockListbox.SelectAllVisible();
			}
		}

		private void UpdateGroupControl()
		{
			if (m_currentGroups.Count > 0)
			{
				m_groupDelete.Enabled = true;
				m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDelete));
			}
			else
			{
				m_groupDelete.Enabled = false;
				m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDeleteDisabled));
			}
		}

		public void UpdateCubeBlock(MyTerminalBlock block)
		{
			if (block != null)
			{
				if (m_terminalSystem != null)
				{
					m_terminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
					m_terminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
					m_terminalSystem.BlockManipulationFinished -= TerminalSystem_BlockManipulationFinished;
					m_terminalSystem.GroupAdded -= TerminalSystem_GroupAdded;
					m_terminalSystem.GroupRemoved -= TerminalSystem_GroupRemoved;
				}
				MyCubeGrid cubeGrid = block.CubeGrid;
				m_terminalSystem = cubeGrid.GridSystems.TerminalSystem;
				m_tmpGroup = new MyBlockGroup();
				m_terminalSystem.BlockAdded += TerminalSystem_BlockAdded;
				m_terminalSystem.BlockRemoved += TerminalSystem_BlockRemoved;
				m_terminalSystem.BlockManipulationFinished += TerminalSystem_BlockManipulationFinished;
				m_terminalSystem.GroupAdded += TerminalSystem_GroupAdded;
				m_terminalSystem.GroupRemoved += TerminalSystem_GroupRemoved;
				SelectBlocks(new MyTerminalBlock[1]
				{
					block
				});
			}
		}

		private void blockListbox_ItemSelected(MyGuiControlListbox sender)
		{
			m_oldGroups.Clear();
			m_oldGroups.AddRange(m_currentGroups);
			m_currentGroups.Clear();
			m_tmpGroup.Blocks.Clear();
			foreach (MyGuiControlListbox.Item selectedItem in sender.SelectedItems)
			{
				if (selectedItem.UserData is MyBlockGroup)
				{
					m_currentGroups.Add((MyBlockGroup)selectedItem.UserData);
				}
				else if (selectedItem.UserData is MyTerminalBlock)
				{
					CurrentBlocks.Add(selectedItem.UserData as MyTerminalBlock);
				}
			}
			for (int i = 0; i < m_currentGroups.Count; i++)
			{
				if (!m_oldGroups.Contains(m_currentGroups[i]) || m_currentGroups[i].Blocks.Intersect(CurrentBlocks).Count() == 0)
				{
					foreach (MyTerminalBlock block in m_currentGroups[i].Blocks)
					{
						if (!CurrentBlocks.Contains(block))
						{
							CurrentBlocks.Add(block);
						}
					}
				}
			}
			SelectBlocks();
		}

		private void block_ShowInTerminalChanged(MyTerminalBlock obj)
		{
			MyTerminalBlock[] array = null;
			if (m_blockListbox != null)
			{
				List<MyGuiControlListbox.Item> selectedItems = m_blockListbox.SelectedItems;
				array = new MyTerminalBlock[selectedItems.Count];
				for (int i = 0; i < selectedItems.Count; i++)
				{
					if (selectedItems[i].UserData is MyTerminalBlock)
					{
						array[i] = (MyTerminalBlock)selectedItems[i].UserData;
					}
				}
			}
			ClearBlockList();
			PopulateBlockList(array);
			if (m_blockListbox != null)
			{
				m_blockListbox.ScrollToolbarToTop();
			}
			blockSearch_TextChanged(m_searchBox.SearchText);
		}
	}
}
