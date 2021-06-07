using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalInventoryController : MyTerminalController
	{
		private struct QueueComponent
		{
			public MyDefinitionId Id;

			public int Count;
		}

		private enum MyBuildPlannerAction
		{
			None,
			DefaultWithdraw,
			WithdrawKeep1,
			WithdrawKeep10,
			AddProduction1,
			AddProcuction10
		}

		private MyGuiControlList m_leftOwnersControl;

		private MyGuiControlRadioButton m_leftSuitButton;

		private MyGuiControlRadioButton m_leftGridButton;

		private MyGuiControlRadioButton m_leftFilterShipButton;

		private MyGuiControlRadioButton m_leftFilterStorageButton;

		private MyGuiControlRadioButton m_leftFilterSystemButton;

		private MyGuiControlRadioButton m_leftFilterEnergyButton;

		private MyGuiControlRadioButton m_leftFilterAllButton;

		private MyGuiControlRadioButtonGroup m_leftTypeGroup;

		private MyGuiControlRadioButtonGroup m_leftFilterGroup;

		private MyGuiControlList m_rightOwnersControl;

		private MyGuiControlRadioButton m_rightSuitButton;

		private MyGuiControlRadioButton m_rightGridButton;

		private MyGuiControlRadioButton m_rightFilterShipButton;

		private MyGuiControlRadioButton m_rightFilterStorageButton;

		private MyGuiControlRadioButton m_rightFilterSystemButton;

		private MyGuiControlRadioButton m_rightFilterEnergyButton;

		private MyGuiControlRadioButton m_rightFilterAllButton;

		private MyGuiControlRadioButtonGroup m_rightTypeGroup;

		private MyGuiControlRadioButtonGroup m_rightFilterGroup;

		private MyGuiControlButton m_throwOutButton;

		private MyGuiControlButton m_withdrawButton;

		private MyGuiControlButton m_depositAllButton;

		private MyGuiControlButton m_addToProductionButton;

		private MyGuiControlButton m_selectedToProductionButton;

		private MyDragAndDropInfo m_dragAndDropInfo;

		private MyGuiControlGridDragAndDrop m_dragAndDrop;

		private List<MyGuiControlGrid> m_controlsDisabledWhileDragged;

		private MyEntity m_userAsEntity;

		private MyEntity m_interactedAsEntity;

		private MyEntity m_openInventoryInteractedAsEntity;

		private MyEntity m_userAsOwner;

		private MyEntity m_interactedAsOwner;

		private List<MyEntity> m_interactedGridOwners = new List<MyEntity>();

		private List<MyEntity> m_interactedGridOwnersMechanical = new List<MyEntity>();

		private List<IMyConveyorEndpoint> m_reachableInventoryOwners = new List<IMyConveyorEndpoint>();

		private List<MyGridConveyorSystem> m_registeredConveyorSystems = new List<MyGridConveyorSystem>();

		private List<MyGridConveyorSystem> m_registeredConveyorMechanicalSystems = new List<MyGridConveyorSystem>();

		private MyGuiControlInventoryOwner m_focusedOwnerControl;

		private MyGuiControlGrid m_focusedGridControl;

		private MyPhysicalInventoryItem? m_selectedInventoryItem;

		private MyInventory m_selectedInventory;

		private bool m_leftShowsGrid;

		private bool m_rightShowsGrid;

		private bool m_rightFilterCurrentShipOnly;

		private bool m_leftFilterCurrentShipOnly;

		private MyInventoryOwnerTypeEnum? m_leftFilterType;

		private MyInventoryOwnerTypeEnum? m_rightFilterType;

		private MyGridColorHelper m_colorHelper;

		private MyGuiControlSearchBox m_searchBoxLeft;

		private MyGuiControlSearchBox m_searchBoxRight;

		private static int m_persistentRadioSelectionLeft = 0;

		private static int m_persistentRadioSelectionRight = 0;

		private static readonly Vector2 m_controlListFullSize = new Vector2(0.437f, 0.618f);

		private static readonly Vector2 m_controlListSizeWithSearch = new Vector2(0.437f, 0.569f);

		private static readonly Vector2 m_leftControlListPosition = new Vector2(-0.452f, -0.276f);

		private static readonly Vector2 m_rightControlListPosition = new Vector2(0.4555f, -0.276f);

		private static readonly Vector2 m_leftControlListPosWithSearch = new Vector2(-0.452f, -0.227f);

		private static readonly Vector2 m_rightControlListPosWithSearch = new Vector2(0.4555f, -0.227f);

		private MyGuiControlCheckbox m_hideEmptyLeft;

		private MyGuiControlLabel m_hideEmptyLeftLabel;

		private MyGuiControlCheckbox m_hideEmptyRight;

		private MyGuiControlLabel m_hideEmptyRightLabel;

		private MyGuiControlGrid m_leftFocusedInventory;

		private MyGuiControlGrid m_rightFocusedInventory;

		private Predicate<IMyConveyorEndpoint> m_endpointPredicate;

		private IMyConveyorEndpointBlock m_interactedEndpointBlock;

		private bool m_selectionDirty;

		private MyGuiControlGrid LeftFocusedInventory
		{
			get
			{
				return m_leftFocusedInventory;
			}
			set
			{
				if (m_leftFocusedInventory != value)
				{
					if (m_leftFocusedInventory != null)
					{
						m_leftFocusedInventory.BorderSize = 1;
					}
					m_leftFocusedInventory = value;
					if (m_leftFocusedInventory != null)
					{
						m_leftFocusedInventory.BorderSize = 3;
					}
				}
			}
		}

		private MyGuiControlGrid RightFocusedInventory
		{
			get
			{
				return m_rightFocusedInventory;
			}
			set
			{
				if (m_rightFocusedInventory != value)
				{
					if (m_rightFocusedInventory != null)
					{
						m_rightFocusedInventory.BorderSize = 1;
					}
					m_rightFocusedInventory = value;
					if (m_rightFocusedInventory != null)
					{
						m_rightFocusedInventory.BorderSize = 3;
					}
				}
			}
		}

		public MyTerminalInventoryController()
		{
			m_leftTypeGroup = new MyGuiControlRadioButtonGroup();
			m_leftFilterGroup = new MyGuiControlRadioButtonGroup();
			m_rightTypeGroup = new MyGuiControlRadioButtonGroup();
			m_rightFilterGroup = new MyGuiControlRadioButtonGroup();
			m_controlsDisabledWhileDragged = new List<MyGuiControlGrid>();
			m_endpointPredicate = EndpointPredicate;
		}

		public void Refresh()
		{
			MyCubeGrid myCubeGrid = (m_interactedAsEntity != null) ? (m_interactedAsEntity.Parent as MyCubeGrid) : null;
			m_interactedGridOwners.Clear();
			if (myCubeGrid != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid).Nodes)
				{
					GetGridInventories(node.NodeData, m_interactedGridOwners, m_interactedAsEntity, MySession.Static.LocalPlayerId);
					node.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystem_BlockAdded;
					node.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystem_BlockRemoved;
					m_registeredConveyorSystems.Add(node.NodeData.GridSystems.ConveyorSystem);
				}
			}
			m_interactedGridOwnersMechanical.Clear();
			if (myCubeGrid != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node2 in MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeGrid).Nodes)
				{
					GetGridInventories(node2.NodeData, m_interactedGridOwnersMechanical, m_interactedAsEntity, MySession.Static.LocalPlayerId);
					node2.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystemMechanical_BlockAdded;
					node2.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystemMechanical_BlockRemoved;
					m_registeredConveyorMechanicalSystems.Add(node2.NodeData.GridSystems.ConveyorSystem);
				}
			}
			m_leftTypeGroup.SelectedIndex = m_persistentRadioSelectionLeft;
			m_rightTypeGroup.SelectedIndex = m_persistentRadioSelectionRight;
			m_leftFilterGroup.SelectedIndex = 0;
			m_rightFilterGroup.SelectedIndex = 0;
			LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			SetLeftFilter(m_leftFilterType);
			SetRightFilter(m_rightFilterType);
		}

		public void Init(IMyGuiControlsParent controlsParent, MyEntity thisEntity, MyEntity interactedEntity, MyGridColorHelper colorHelper)
		{
			m_userAsEntity = thisEntity;
			m_interactedAsEntity = interactedEntity;
			m_colorHelper = colorHelper;
			m_leftOwnersControl = (MyGuiControlList)controlsParent.Controls.GetControlByName("LeftInventory");
			m_rightOwnersControl = (MyGuiControlList)controlsParent.Controls.GetControlByName("RightInventory");
			m_leftOwnersControl.FocusScrollingEnabled = false;
			m_rightOwnersControl.FocusScrollingEnabled = false;
			m_leftSuitButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftSuitButton");
			m_leftGridButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftGridButton");
			m_leftFilterShipButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterShipButton");
			m_leftFilterStorageButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterStorageButton");
			m_leftFilterSystemButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterSystemButton");
			m_leftFilterEnergyButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterEnergyButton");
			m_leftFilterAllButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterAllButton");
			m_rightSuitButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightSuitButton");
			m_rightGridButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightGridButton");
			m_rightFilterShipButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterShipButton");
			m_rightFilterStorageButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterStorageButton");
			m_rightFilterSystemButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterSystemButton");
			m_rightFilterEnergyButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterEnergyButton");
			m_rightFilterAllButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterAllButton");
			m_throwOutButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("ThrowOutButton");
			m_withdrawButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("WithdrawButton");
			m_depositAllButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("DepositAllButton");
			m_addToProductionButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("AddToProductionButton");
			m_selectedToProductionButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("SelectedToProductionButton");
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.BuildPlanner != null)
			{
				m_withdrawButton.Enabled = (MySession.Static.LocalCharacter.BuildPlanner.Count > 0);
				m_addToProductionButton.Enabled = (MySession.Static.LocalCharacter.BuildPlanner.Count > 0);
			}
			else
			{
				m_withdrawButton.Enabled = false;
				m_addToProductionButton.Enabled = false;
			}
			m_selectedToProductionButton.Enabled = false;
			m_hideEmptyLeft = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("CheckboxHideEmptyLeft");
			m_hideEmptyLeftLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("LabelHideEmptyLeft");
			m_hideEmptyRight = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("CheckboxHideEmptyRight");
			m_hideEmptyRightLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("LabelHideEmptyRight");
			m_searchBoxLeft = (MyGuiControlSearchBox)controlsParent.Controls.GetControlByName("BlockSearchLeft");
			m_searchBoxRight = (MyGuiControlSearchBox)controlsParent.Controls.GetControlByName("BlockSearchRight");
			m_hideEmptyLeft.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_HideEmpty);
			m_hideEmptyRight.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_HideEmpty);
			m_hideEmptyLeft.Visible = false;
			m_hideEmptyLeftLabel.Visible = false;
			m_hideEmptyRight.Visible = true;
			m_hideEmptyRightLabel.Visible = true;
			m_searchBoxLeft.Visible = false;
			m_searchBoxRight.Visible = false;
			MyGuiControlCheckbox hideEmptyLeft = m_hideEmptyLeft;
			hideEmptyLeft.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(hideEmptyLeft.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyLeft_Checked));
			MyGuiControlCheckbox hideEmptyRight = m_hideEmptyRight;
			hideEmptyRight.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(hideEmptyRight.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyRight_Checked));
			m_searchBoxLeft.OnTextChanged += BlockSearchLeft_TextChanged;
			m_searchBoxRight.OnTextChanged += BlockSearchRight_TextChanged;
			m_leftSuitButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowCharacter);
			m_leftGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnected);
			m_leftGridButton.ShowTooltipWhenDisabled = true;
			m_rightSuitButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowInteracted);
			m_rightGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnected);
			m_rightGridButton.ShowTooltipWhenDisabled = true;
			m_leftFilterAllButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterAll);
			m_leftFilterEnergyButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterEnergy);
			m_leftFilterShipButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterShip);
			m_leftFilterStorageButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterStorage);
			m_leftFilterSystemButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterSystem);
			m_rightFilterAllButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterAll);
			m_rightFilterEnergyButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterEnergy);
			m_rightFilterShipButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterShip);
			m_rightFilterStorageButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterStorage);
			m_rightFilterSystemButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterSystem);
			m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOut);
			m_throwOutButton.ShowTooltipWhenDisabled = true;
			m_throwOutButton.CueEnum = GuiSounds.None;
			MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER);
			m_withdrawButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Withdraw).ToString(), gameControl));
			m_withdrawButton.ShowTooltipWhenDisabled = true;
			m_withdrawButton.CueEnum = GuiSounds.None;
			m_withdrawButton.DrawCrossTextureWhenDisabled = false;
			m_depositAllButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Deposit).ToString(), gameControl));
			m_depositAllButton.ShowTooltipWhenDisabled = true;
			m_depositAllButton.CueEnum = GuiSounds.None;
			m_depositAllButton.DrawCrossTextureWhenDisabled = false;
			m_addToProductionButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_AddComponents).ToString(), gameControl));
			m_addToProductionButton.ShowTooltipWhenDisabled = true;
			m_addToProductionButton.CueEnum = GuiSounds.None;
			m_addToProductionButton.DrawCrossTextureWhenDisabled = false;
			m_selectedToProductionButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_AddSelectedComponent);
			m_selectedToProductionButton.ShowTooltipWhenDisabled = true;
			m_selectedToProductionButton.CueEnum = GuiSounds.None;
			m_selectedToProductionButton.DrawCrossTextureWhenDisabled = false;
			m_leftTypeGroup.Add(m_leftSuitButton);
			m_leftTypeGroup.Add(m_leftGridButton);
			m_rightTypeGroup.Add(m_rightSuitButton);
			m_rightTypeGroup.Add(m_rightGridButton);
			m_leftFilterGroup.Add(m_leftFilterAllButton);
			m_leftFilterGroup.Add(m_leftFilterEnergyButton);
			m_leftFilterGroup.Add(m_leftFilterShipButton);
			m_leftFilterGroup.Add(m_leftFilterStorageButton);
			m_leftFilterGroup.Add(m_leftFilterSystemButton);
			m_rightFilterGroup.Add(m_rightFilterAllButton);
			m_rightFilterGroup.Add(m_rightFilterEnergyButton);
			m_rightFilterGroup.Add(m_rightFilterShipButton);
			m_rightFilterGroup.Add(m_rightFilterStorageButton);
			m_rightFilterGroup.Add(m_rightFilterSystemButton);
			m_throwOutButton.DrawCrossTextureWhenDisabled = false;
			m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, supportIcon: true);
			controlsParent.Controls.Add(m_dragAndDrop);
			m_dragAndDrop.DrawBackgroundTexture = false;
			m_throwOutButton.ButtonClicked += throwOutButton_OnButtonClick;
			m_withdrawButton.ButtonClicked += WithdrawButton_ButtonClicked;
			m_depositAllButton.ButtonClicked += DepositAllButton_ButtonClicked;
			m_addToProductionButton.ButtonClicked += addToProductionButton_ButtonClicked;
			m_selectedToProductionButton.ButtonClicked += selectedToProductionButton_ButtonClicked;
			m_dragAndDrop.ItemDropped += dragDrop_OnItemDropped;
			MyEntity myEntity = (m_userAsEntity != null && m_userAsEntity.HasInventory) ? m_userAsEntity : null;
			if (myEntity != null)
			{
				m_userAsOwner = myEntity;
			}
			MyEntity myEntity2 = (m_interactedAsEntity != null && m_interactedAsEntity.HasInventory) ? m_interactedAsEntity : null;
			if (myEntity2 != null)
			{
				m_interactedAsOwner = myEntity2;
			}
			MyCubeGrid myCubeGrid = (m_interactedAsEntity != null) ? (m_interactedAsEntity.Parent as MyCubeGrid) : null;
			m_interactedGridOwners.Clear();
			if (myCubeGrid != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid).Nodes)
				{
					GetGridInventories(node.NodeData, m_interactedGridOwners, m_interactedAsEntity, MySession.Static.LocalPlayerId);
					node.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystem_BlockAdded;
					node.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystem_BlockRemoved;
					m_registeredConveyorSystems.Add(node.NodeData.GridSystems.ConveyorSystem);
				}
			}
			m_interactedGridOwnersMechanical.Clear();
			if (myCubeGrid != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node2 in MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeGrid).Nodes)
				{
					GetGridInventories(node2.NodeData, m_interactedGridOwnersMechanical, m_interactedAsEntity, MySession.Static.LocalPlayerId);
					node2.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystemMechanical_BlockAdded;
					node2.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystemMechanical_BlockRemoved;
					m_registeredConveyorMechanicalSystems.Add(node2.NodeData.GridSystems.ConveyorSystem);
				}
			}
			if (m_interactedAsEntity is MyCharacter || m_interactedAsEntity is MyInventoryBagEntity)
			{
				m_persistentRadioSelectionRight = 0;
			}
			m_leftTypeGroup.SelectedIndex = m_persistentRadioSelectionLeft;
			m_rightTypeGroup.SelectedIndex = m_persistentRadioSelectionRight;
			m_leftFilterGroup.SelectedIndex = 0;
			m_rightFilterGroup.SelectedIndex = 0;
			LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			SetLeftFilter(null);
			SetRightFilter(null);
			m_leftTypeGroup.SelectedChanged += LeftTypeGroup_SelectedChanged;
			m_rightTypeGroup.SelectedChanged += RightTypeGroup_SelectedChanged;
			m_leftFilterAllButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					m_leftFilterCurrentShipOnly = false;
					SetLeftFilter(null);
				}
			};
			m_leftFilterEnergyButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					SetLeftFilter(MyInventoryOwnerTypeEnum.Energy);
				}
			};
			m_leftFilterStorageButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					SetLeftFilter(MyInventoryOwnerTypeEnum.Storage);
				}
			};
			m_leftFilterSystemButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					SetLeftFilter(MyInventoryOwnerTypeEnum.System);
				}
			};
			m_leftFilterShipButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					m_leftFilterCurrentShipOnly = true;
					SetLeftFilter(null);
				}
			};
			m_rightFilterAllButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					m_rightFilterCurrentShipOnly = false;
					SetRightFilter(null);
				}
			};
			m_rightFilterEnergyButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					SetRightFilter(MyInventoryOwnerTypeEnum.Energy);
				}
			};
			m_rightFilterStorageButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					SetRightFilter(MyInventoryOwnerTypeEnum.Storage);
				}
			};
			m_rightFilterSystemButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					SetRightFilter(MyInventoryOwnerTypeEnum.System);
				}
			};
			m_rightFilterShipButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					m_rightFilterCurrentShipOnly = true;
					SetRightFilter(null);
				}
			};
			if (m_interactedAsEntity == null)
			{
				m_leftGridButton.Enabled = false;
				m_leftGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnectedDisabled);
				m_rightGridButton.Enabled = false;
				m_rightGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnectedDisabled);
				m_rightTypeGroup.SelectedIndex = 0;
			}
			RefreshSelectedInventoryItem();
		}

		private void selectedToProductionButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (m_selectedInventoryItem.HasValue && m_interactedAsEntity != null)
			{
				Queue<QueueComponent> queue = new Queue<QueueComponent>();
				int count = 1;
				if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
				{
					count = 100;
				}
				else if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					count = 10;
				}
				QueueComponent item = new QueueComponent
				{
					Id = m_selectedInventoryItem.Value.Content.GetId(),
					Count = count
				};
				queue.Enqueue(item);
				AddComponentsToProduction(queue, m_interactedAsEntity);
			}
		}

		private static bool FilterAssemblerFunc(Sandbox.ModAPI.IMyTerminalBlock block)
		{
			if (!(block is Sandbox.ModAPI.IMyAssembler))
			{
				return false;
			}
			if (block != null && !block.IsWorking)
			{
				return false;
			}
			MyEntity myEntity = block as MyEntity;
			if (myEntity == null || !myEntity.HasInventory)
			{
				return false;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = block.GetUserRelationToOwner(MySession.Static.LocalPlayerId);
			if (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Owner && userRelationToOwner != MyRelationsBetweenPlayerAndBlock.FactionShare && userRelationToOwner != 0)
			{
				return false;
			}
			return true;
		}

		private static int SortAssemberBlockFunc(Sandbox.ModAPI.IMyTerminalBlock x, Sandbox.ModAPI.IMyTerminalBlock y)
		{
			MyAssemblerDefinition myAssemblerDefinition = x.SlimBlock?.BlockDefinition as MyAssemblerDefinition;
			if (myAssemblerDefinition == null)
			{
				return 0;
			}
			return (y.SlimBlock?.BlockDefinition as MyAssemblerDefinition)?.AssemblySpeed.CompareTo(myAssemblerDefinition.AssemblySpeed) ?? 1;
		}

		private void addToProductionButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyBuildPlannerAction myBuildPlannerAction = MyBuildPlannerAction.None;
				switch ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? 4 : 5)
				{
				case 4:
				{
					int value2 = 1;
					if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
					{
						int num2 = AddComponentsToProduction(m_interactedAsEntity, value2);
						if (num2 > 0)
						{
							MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionFailed).SetTextFormatArguments(num2);
						}
						else
						{
							MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionSuccessful);
						}
					}
					else
					{
						MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
					}
					break;
				}
				case 5:
				{
					int value = 10;
					if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
					{
						int num = AddComponentsToProduction(m_interactedAsEntity, value);
						if (num > 0)
						{
							MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionFailed).SetTextFormatArguments(num);
						}
						else
						{
							MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionSuccessful);
						}
					}
					else
					{
						MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
					}
					break;
				}
				}
			}
			else
			{
				int num3 = 1;
				if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
				{
					num3 = 100;
				}
				else if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					num3 = 10;
				}
				Queue<QueueComponent> queue = new Queue<QueueComponent>();
				while (num3 > 0)
				{
					foreach (MyIdentity.BuildPlanItem item in MySession.Static.LocalCharacter.BuildPlanner)
					{
						foreach (MyIdentity.BuildPlanItem.Component component in item.Components)
						{
							queue.Enqueue(new QueueComponent
							{
								Id = component.ComponentDefinition.Id,
								Count = component.Count
							});
						}
					}
					num3--;
				}
				int num4 = AddComponentsToProduction(queue, m_interactedAsEntity);
				if (num4 > 0)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationPutToProductionFailed), num4))));
				}
			}
		}

		public static int AddComponentsToProduction(MyEntity interactedEntity, int? persistentMultiple)
		{
			Queue<QueueComponent> queue = new Queue<QueueComponent>();
			foreach (MyIdentity.BuildPlanItem item in MySession.Static.LocalCharacter.BuildPlanner)
			{
				for (int num = (!persistentMultiple.HasValue) ? 1 : persistentMultiple.Value; num > 0; num--)
				{
					foreach (MyIdentity.BuildPlanItem.Component component in item.Components)
					{
						queue.Enqueue(new QueueComponent
						{
							Id = component.ComponentDefinition.Id,
							Count = component.Count
						});
					}
				}
			}
			return AddComponentsToProduction(queue, interactedEntity);
		}

		private static int AddComponentsToProduction(Queue<QueueComponent> queuedComponents, MyEntity interactedEntity)
		{
			if (interactedEntity == null)
			{
				return 0;
			}
			MyCubeGrid myCubeGrid = interactedEntity.Parent as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return 0;
			}
			MyGridTerminalSystem terminalSystem = myCubeGrid.GridSystems.TerminalSystem;
			int num = 0;
			List<Sandbox.ModAPI.IMyTerminalBlock> list = new List<Sandbox.ModAPI.IMyTerminalBlock>();
			((Sandbox.ModAPI.IMyGridTerminalSystem)terminalSystem).GetBlocksOfType<Sandbox.ModAPI.IMyTerminalBlock>(list, (Func<Sandbox.ModAPI.IMyTerminalBlock, bool>)FilterAssemblerFunc);
			List<Sandbox.ModAPI.IMyAssembler> list2 = list.Cast<Sandbox.ModAPI.IMyAssembler>().ToList();
			list2.SortNoAlloc(SortAssemberBlockFunc);
			_ = queuedComponents.Count;
			while (queuedComponents.Count > 0)
			{
				QueueComponent queueComponent = queuedComponents.Dequeue();
				bool flag = false;
				foreach (Sandbox.ModAPI.IMyAssembler item in list2)
				{
					if (item.Mode != MyAssemblerMode.Disassembly && item.UseConveyorSystem && !item.CooperativeMode)
					{
						Sandbox.ModAPI.IMyProductionBlock myProductionBlock = item;
						MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(queueComponent.Id);
						if (myBlueprintDefinitionBase != null && myProductionBlock.CanUseBlueprint(myBlueprintDefinitionBase))
						{
							myProductionBlock.AddQueueItem(myBlueprintDefinitionBase, queueComponent.Count);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					num++;
				}
			}
			return num;
		}

		private MyInventory[] GetSourceInventories()
		{
			ObservableCollection<MyGuiControlBase>.Enumerator enumerator = m_leftOwnersControl.Controls.GetEnumerator();
			List<MyInventory> list = new List<MyInventory>();
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Visible)
				{
					continue;
				}
				myGuiControlInventoryOwner = (enumerator.Current as MyGuiControlInventoryOwner);
				if (myGuiControlInventoryOwner == null || !myGuiControlInventoryOwner.Enabled)
				{
					continue;
				}
				MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
				for (int i = 0; i < inventoryOwner.InventoryCount; i++)
				{
					MyInventory inventory = inventoryOwner.GetInventory(i);
					if (inventory != null)
					{
						list.Add(inventory);
					}
				}
			}
			return list.ToArray();
		}

		private void DepositAllButton_ButtonClicked(MyGuiControlButton obj)
		{
			int num = depositAllFrom(GetSourceInventories(), m_interactedAsEntity, GetAvailableInventories);
			if (num > 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationDepositFailed), num))));
			}
		}

		public static int DepositAll(MyInventory srcInventory, MyEntity interactedEntity)
		{
			return depositAllFrom(new MyInventory[1]
			{
				srcInventory
			}, interactedEntity, GetAvailableInventoriesStatic);
		}

		private static int depositAllFrom(MyInventory[] srcInventories, MyEntity interactedEntity, Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod)
		{
			int num = 0;
			Dictionary<MyInventory, Dictionary<MyDefinitionId, MyFixedPoint>> dictionary = new Dictionary<MyInventory, Dictionary<MyDefinitionId, MyFixedPoint>>();
			MyInventory[] array = srcInventories;
			foreach (MyInventory myInventory in array)
			{
				foreach (MyPhysicalInventoryItem item in myInventory.GetItems())
				{
					if ((!(item.Content is MyObjectBuilder_AmmoMagazine) || !(item.Content.SubtypeName == "NATO_5p56x45mm")) && !(item.Content is MyObjectBuilder_PhysicalGunObject) && !(item.Content is MyObjectBuilder_GasContainerObject))
					{
						if (!dictionary.ContainsKey(myInventory))
						{
							dictionary[myInventory] = new Dictionary<MyDefinitionId, MyFixedPoint>();
						}
						if (!dictionary[myInventory].ContainsKey(item.Content.GetId()))
						{
							dictionary[myInventory][item.Content.GetId()] = 0;
						}
						dictionary[myInventory][item.Content.GetId()] += item.Amount;
					}
				}
			}
			array = srcInventories;
			foreach (MyInventory myInventory2 in array)
			{
				if (dictionary.ContainsKey(myInventory2))
				{
					Dictionary<MyInventory, MyFixedPoint> dictionary2 = new Dictionary<MyInventory, MyFixedPoint>();
					List<MyInventory> list = new List<MyInventory>();
					foreach (MyDefinitionId item2 in dictionary[myInventory2].Keys.ToList())
					{
						getInventoriesMethod(myInventory2.Owner, item2, list, interactedEntity, arg5: false);
						if (list.Count == 0)
						{
							num++;
						}
						else
						{
							MyFixedPoint a = 0;
							foreach (MyInventory item3 in list)
							{
								if (myInventory2 != item3)
								{
									if (!dictionary2.ContainsKey(item3))
									{
										dictionary2.Add(item3, item3.MaxVolume - item3.CurrentVolume);
									}
									MyFixedPoint myFixedPoint = dictionary2[item3];
									if (!(myFixedPoint == 0))
									{
										MyInventory.GetItemVolumeAndMass(item2, out float _, out float itemVolume);
										MyFixedPoint myFixedPoint2 = dictionary[myInventory2][item2] * itemVolume;
										MyFixedPoint myFixedPoint3 = myFixedPoint2;
										MyFixedPoint myFixedPoint4 = dictionary[myInventory2][item2];
										if (myFixedPoint < myFixedPoint2)
										{
											myFixedPoint3 = myFixedPoint;
											MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
											@static.Adapt(item2);
											if (@static.HasIntegralAmounts)
											{
												myFixedPoint4 = (MyFixedPoint)(Math.Round((double)myFixedPoint3 * 1000.0 / (double)itemVolume) / 1000.0);
												myFixedPoint4 = MyFixedPoint.Floor(myFixedPoint4);
											}
											else
											{
												MyFixedPoint myFixedPoint5 = (MyFixedPoint)((double)myFixedPoint3 / (double)itemVolume);
												if (Math.Abs((float)myFixedPoint5 - (float)myFixedPoint4) > 0.001f)
												{
													myFixedPoint4 = myFixedPoint5;
												}
											}
										}
										if (myFixedPoint4 > 0)
										{
											MyInventory.TransferByPlanner(myInventory2, item3, item2, MyItemFlags.None, myFixedPoint4);
											dictionary[myInventory2][item2] -= myFixedPoint4;
											dictionary2[item3] -= myFixedPoint3;
										}
										a += myFixedPoint4;
									}
								}
							}
							if (a == 0)
							{
								num++;
							}
						}
					}
				}
			}
			return num;
		}

		private void WithdrawButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyBuildPlannerAction myBuildPlannerAction = MyBuildPlannerAction.None;
				switch (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) ? 2 : ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? 1 : 3))
				{
				case 0:
					break;
				case 1:
					ProcessWithdraw(m_interactedAsEntity, MySession.Static.LocalCharacter.GetInventory(), null);
					MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
					break;
				case 2:
					ProcessWithdraw(multiplier: 1, owner: m_interactedAsEntity, inventory: MySession.Static.LocalCharacter.GetInventory());
					break;
				case 3:
					ProcessWithdraw(multiplier: 10, owner: m_interactedAsEntity, inventory: MySession.Static.LocalCharacter.GetInventory());
					break;
				}
				return;
			}
			MyInventory[] sourceInventories = GetSourceInventories();
			int? persistentMultiple = null;
			if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
			{
				persistentMultiple = 100;
			}
			else if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				persistentMultiple = 10;
			}
			else if (MyInput.Static.IsAnyShiftKeyPressed())
			{
				persistentMultiple = 1;
			}
			List<MyIdentity.BuildPlanItem.Component> missingComponents = WithdrawToInventories(sourceInventories, GetAvailableInventories, m_interactedAsEntity, persistentMultiple);
			if (!persistentMultiple.HasValue)
			{
				if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
				{
					m_withdrawButton.Enabled = true;
					m_addToProductionButton.Enabled = true;
					string missingComponentsText = GetMissingComponentsText(missingComponents);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(missingComponentsText)));
				}
				else
				{
					m_withdrawButton.Enabled = false;
					m_addToProductionButton.Enabled = false;
				}
			}
		}

		private static void ProcessWithdraw(MyEntity owner, MyInventory inventory, int? multiplier)
		{
			if (MySession.Static.LocalCharacter.BuildPlanner.Count == 0)
			{
				MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
				return;
			}
			List<MyIdentity.BuildPlanItem.Component> list = Withdraw(owner, inventory, multiplier);
			if (list.Count == 0)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.WithdrawSuccessful);
				return;
			}
			string missingComponentsText = GetMissingComponentsText(list);
			MyHud.Notifications.Add(MyNotificationSingletons.WithdrawFailed).SetTextFormatArguments(missingComponentsText);
		}

		public static string GetMissingComponentsText(List<MyIdentity.BuildPlanItem.Component> missingComponents)
		{
			string text = "";
			switch (missingComponents.Count)
			{
			case 1:
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed1).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText);
			case 2:
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed2).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText, missingComponents[1].Count, missingComponents[1].ComponentDefinition.DisplayNameText);
			case 3:
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed3).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText, missingComponents[1].Count, missingComponents[1].ComponentDefinition.DisplayNameText, missingComponents[2].Count, missingComponents[2].ComponentDefinition.DisplayNameText);
			default:
			{
				int num = 0;
				for (int i = 3; i < missingComponents.Count; i++)
				{
					num += missingComponents[i].Count;
				}
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed4More).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText, missingComponents[1].Count, missingComponents[1].ComponentDefinition.DisplayNameText, missingComponents[2].Count, missingComponents[2].ComponentDefinition.DisplayNameText, num);
			}
			}
		}

		public static List<MyIdentity.BuildPlanItem.Component> Withdraw(MyEntity interactedEntity, MyInventory toInventory, int? persistentMultiple)
		{
			return WithdrawToInventories(new MyInventory[1]
			{
				toInventory
			}, GetAvailableInventoriesStatic, interactedEntity, persistentMultiple);
		}

		private static void GetAvailableInventoriesStatic(MyEntity inventoryOwner, MyDefinitionId id, List<MyInventory> availableInventories, MyEntity interactedEntity, bool requireAmount)
		{
			availableInventories.Clear();
			List<MyEntity> list = new List<MyEntity>();
			MyCubeGrid myCubeGrid = (interactedEntity as MyCubeBlock)?.CubeGrid;
			MyInventoryBagEntity myInventoryBagEntity = interactedEntity as MyInventoryBagEntity;
			if (myCubeGrid != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeGrid).Nodes)
				{
					GetGridInventories(node.NodeData, list, interactedEntity, MySession.Static.LocalPlayerId);
				}
			}
			if (myInventoryBagEntity != null)
			{
				list.Add(myInventoryBagEntity);
			}
			foreach (MyEntity item in list)
			{
				IMyConveyorEndpointBlock myConveyorEndpointBlock = item as IMyConveyorEndpointBlock;
				IMyConveyorEndpointBlock myConveyorEndpointBlock2 = interactedEntity as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null && myConveyorEndpointBlock2 != null && (myConveyorEndpointBlock == myConveyorEndpointBlock2 || MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint, MySession.Static.LocalPlayerId, id, EndpointPredicateStatic)))
				{
					for (int i = 0; i < item.InventoryCount; i++)
					{
						MyInventory inventory = item.GetInventory(i);
						if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id) > 0))
						{
							availableInventories.Add(inventory);
						}
					}
				}
			}
		}

		private static List<MyIdentity.BuildPlanItem.Component> WithdrawToInventories(MyInventory[] toInventories, Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod, MyEntity interactedEntity, int? persistentMultiple = null)
		{
			Dictionary<MyInventory, Dictionary<MyDefinitionId, int>> dictionary = new Dictionary<MyInventory, Dictionary<MyDefinitionId, int>>();
			List<MyInventory> list = new List<MyInventory>();
			IReadOnlyList<MyIdentity.BuildPlanItem> readOnlyList = MySession.Static.LocalCharacter.BuildPlanner;
			if (persistentMultiple.HasValue)
			{
				List<MyIdentity.BuildPlanItem> list2 = new List<MyIdentity.BuildPlanItem>();
				for (int num = persistentMultiple.Value; num > 0; num--)
				{
					foreach (MyIdentity.BuildPlanItem item2 in MySession.Static.LocalCharacter.BuildPlanner)
					{
						MyIdentity.BuildPlanItem item = item2.Clone();
						list2.Add(item);
					}
				}
				readOnlyList = list2;
			}
			foreach (MyInventory myInventory in toInventories)
			{
				MyFixedPoint fp = myInventory.MaxVolume - myInventory.CurrentVolume;
				foreach (MyIdentity.BuildPlanItem item3 in readOnlyList)
				{
					foreach (MyIdentity.BuildPlanItem.Component component in item3.Components)
					{
						if (!(component.ComponentDefinition.Volume <= 0f) && myInventory.CheckConstraint(component.ComponentDefinition.Id))
						{
							getInventoriesMethod(myInventory.Owner, component.ComponentDefinition.Id, list, interactedEntity, arg5: true);
							foreach (MyInventory item4 in list)
							{
								MyFixedPoint itemAmount = item4.GetItemAmount(component.ComponentDefinition.Id);
								if (!Sync.IsServer)
								{
									if (dictionary.ContainsKey(item4) && dictionary[item4].ContainsKey(component.ComponentDefinition.Id))
									{
										int i2 = dictionary[item4][component.ComponentDefinition.Id];
										itemAmount -= (MyFixedPoint)i2;
									}
									if (itemAmount == 0)
									{
										continue;
									}
								}
								MyFixedPoint myFixedPoint = itemAmount;
								if (itemAmount > component.Count)
								{
									myFixedPoint = component.Count;
								}
								float num2 = (float)myFixedPoint * component.ComponentDefinition.Volume;
								if (num2 > (float)fp)
								{
									num2 = (float)fp;
									myFixedPoint = (int)(num2 / component.ComponentDefinition.Volume);
								}
								if (!Sync.IsServer)
								{
									if (!dictionary.ContainsKey(item4))
									{
										dictionary.Add(item4, new Dictionary<MyDefinitionId, int>());
									}
									if (!dictionary[item4].ContainsKey(component.ComponentDefinition.Id))
									{
										dictionary[item4].Add(component.ComponentDefinition.Id, 0);
									}
									dictionary[item4][component.ComponentDefinition.Id] += (int)myFixedPoint;
								}
								MyInventory.TransferByPlanner(item4, myInventory, component.ComponentDefinition.Id, MyItemFlags.None, myFixedPoint);
								fp -= (MyFixedPoint)num2;
								item3.IsInProgress = true;
								component.Count -= (int)myFixedPoint;
								break;
							}
						}
					}
					item3.Components.RemoveAll((MyIdentity.BuildPlanItem.Component x) => x.Count == 0);
				}
				if (!persistentMultiple.HasValue)
				{
					MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
				}
			}
			List<MyIdentity.BuildPlanItem.Component> list3 = new List<MyIdentity.BuildPlanItem.Component>();
			foreach (MyIdentity.BuildPlanItem item5 in readOnlyList)
			{
				foreach (MyIdentity.BuildPlanItem.Component component2 in item5.Components)
				{
					list3.Add(component2);
				}
			}
			return list3;
		}

		private void GetAvailableInventories(MyEntity inventoryOwner, MyDefinitionId id, List<MyInventory> availableInventories, MyEntity interactedEntity, bool requireAmount)
		{
			ObservableCollection<MyGuiControlBase>.Enumerator enumerator = m_rightOwnersControl.Controls.GetEnumerator();
			availableInventories.Clear();
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Visible)
				{
					continue;
				}
				myGuiControlInventoryOwner = (enumerator.Current as MyGuiControlInventoryOwner);
				if (myGuiControlInventoryOwner == null || !myGuiControlInventoryOwner.Enabled)
				{
					continue;
				}
				if ((inventoryOwner != m_userAsOwner && inventoryOwner != m_interactedAsOwner) || (myGuiControlInventoryOwner.InventoryOwner != m_userAsOwner && myGuiControlInventoryOwner.InventoryOwner != m_interactedAsOwner))
				{
					bool flag = inventoryOwner is MyCharacter;
					bool flag2 = myGuiControlInventoryOwner.InventoryOwner is MyCharacter;
					IMyConveyorEndpointBlock myConveyorEndpointBlock = (inventoryOwner == null) ? null : ((flag ? m_interactedAsOwner : inventoryOwner) as IMyConveyorEndpointBlock);
					IMyConveyorEndpointBlock myConveyorEndpointBlock2 = (myGuiControlInventoryOwner.InventoryOwner == null) ? null : ((flag2 ? m_interactedAsOwner : myGuiControlInventoryOwner.InventoryOwner) as IMyConveyorEndpointBlock);
					if (myConveyorEndpointBlock == null || myConveyorEndpointBlock2 == null)
					{
						continue;
					}
					try
					{
						MyGridConveyorSystem.AppendReachableEndpoints(myConveyorEndpointBlock.ConveyorEndpoint, MySession.Static.LocalPlayerId, m_reachableInventoryOwners, id, m_endpointPredicate);
						if (!m_reachableInventoryOwners.Contains(myConveyorEndpointBlock2.ConveyorEndpoint))
						{
							continue;
						}
					}
					finally
					{
						m_reachableInventoryOwners.Clear();
					}
					if (!MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint))
					{
						continue;
					}
				}
				MyEntity inventoryOwner2 = myGuiControlInventoryOwner.InventoryOwner;
				for (int i = 0; i < inventoryOwner2.InventoryCount; i++)
				{
					MyInventory inventory = inventoryOwner2.GetInventory(i);
					if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id) > 0))
					{
						availableInventories.Add(inventory);
					}
				}
			}
		}

		public void Close()
		{
			foreach (MyGridConveyorSystem registeredConveyorSystem in m_registeredConveyorSystems)
			{
				registeredConveyorSystem.BlockAdded -= ConveyorSystem_BlockAdded;
				registeredConveyorSystem.BlockRemoved -= ConveyorSystem_BlockRemoved;
			}
			m_registeredConveyorSystems.Clear();
			foreach (MyGridConveyorSystem registeredConveyorMechanicalSystem in m_registeredConveyorMechanicalSystems)
			{
				registeredConveyorMechanicalSystem.BlockAdded -= ConveyorSystemMechanical_BlockAdded;
				registeredConveyorMechanicalSystem.BlockRemoved -= ConveyorSystemMechanical_BlockRemoved;
			}
			m_registeredConveyorMechanicalSystems.Clear();
			m_leftTypeGroup.Clear();
			m_leftFilterGroup.Clear();
			m_rightTypeGroup.Clear();
			m_rightFilterGroup.Clear();
			m_controlsDisabledWhileDragged.Clear();
			m_leftOwnersControl = null;
			m_leftSuitButton = null;
			m_leftGridButton = null;
			m_leftFilterStorageButton = null;
			m_leftFilterSystemButton = null;
			m_leftFilterEnergyButton = null;
			m_leftFilterAllButton = null;
			m_leftFilterShipButton = null;
			m_rightOwnersControl = null;
			m_rightSuitButton = null;
			m_rightGridButton = null;
			m_rightFilterShipButton = null;
			m_rightFilterStorageButton = null;
			m_rightFilterSystemButton = null;
			m_rightFilterEnergyButton = null;
			m_rightFilterAllButton = null;
			m_throwOutButton = null;
			m_dragAndDrop = null;
			m_dragAndDropInfo = null;
			m_focusedOwnerControl = null;
			m_focusedGridControl = null;
			m_selectedInventory = null;
			MyGuiControlCheckbox hideEmptyLeft = m_hideEmptyLeft;
			hideEmptyLeft.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(hideEmptyLeft.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyLeft_Checked));
			MyGuiControlCheckbox hideEmptyRight = m_hideEmptyRight;
			hideEmptyRight.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(hideEmptyRight.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyRight_Checked));
			m_searchBoxLeft.OnTextChanged -= BlockSearchLeft_TextChanged;
			m_searchBoxRight.OnTextChanged -= BlockSearchRight_TextChanged;
			m_hideEmptyLeft = null;
			m_hideEmptyLeftLabel = null;
			m_hideEmptyRight = null;
			m_hideEmptyRightLabel = null;
			m_searchBoxLeft = null;
			m_searchBoxRight = null;
		}

		public void SetSearch(string text, bool interactedSide = true)
		{
			MyGuiControlSearchBox myGuiControlSearchBox = interactedSide ? m_searchBoxRight : m_searchBoxLeft;
			if (myGuiControlSearchBox != null)
			{
				myGuiControlSearchBox.SearchText = text;
			}
			if (interactedSide)
			{
				SetRightFilter(null);
			}
			else
			{
				SetLeftFilter(null);
			}
		}

		private void StartDragging(MyDropHandleType dropHandlingType, MyGuiControlGrid gridControl, ref MyGuiControlGrid.EventArgs args)
		{
			m_dragAndDropInfo = new MyDragAndDropInfo();
			m_dragAndDropInfo.Grid = gridControl;
			m_dragAndDropInfo.ItemIndex = args.ItemIndex;
			DisableInvalidWhileDragging();
			MyGuiGridItem itemAt = m_dragAndDropInfo.Grid.GetItemAt(m_dragAndDropInfo.ItemIndex);
			m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, m_dragAndDropInfo, includeTooltip: false);
		}

		private void DisableInvalidWhileDragging()
		{
			MyGuiGridItem itemAt = m_dragAndDropInfo.Grid.GetItemAt(m_dragAndDropInfo.ItemIndex);
			if (itemAt != null)
			{
				MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)itemAt.UserData;
				MyInventory srcInventory = (MyInventory)m_dragAndDropInfo.Grid.UserData;
				DisableUnacceptingInventoryControls(item, m_leftOwnersControl);
				DisableUnacceptingInventoryControls(item, m_rightOwnersControl);
				DisableUnreachableInventoryControls(srcInventory, item, m_leftOwnersControl);
				DisableUnreachableInventoryControls(srcInventory, item, m_rightOwnersControl);
			}
		}

		private void DisableUnacceptingInventoryControls(MyPhysicalInventoryItem item, MyGuiControlList list)
		{
			foreach (MyGuiControlBase visibleControl in list.Controls.GetVisibleControls())
			{
				if (visibleControl.Enabled)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner = (MyGuiControlInventoryOwner)visibleControl;
					MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
					for (int i = 0; i < inventoryOwner.InventoryCount; i++)
					{
						if (!inventoryOwner.GetInventory(i).CanItemsBeAdded(0, item.Content.GetId()))
						{
							myGuiControlInventoryOwner.ContentGrids[i].Enabled = false;
							m_controlsDisabledWhileDragged.Add(myGuiControlInventoryOwner.ContentGrids[i]);
						}
					}
				}
			}
		}

		private bool EndpointPredicate(IMyConveyorEndpoint endpoint)
		{
			if (endpoint.CubeBlock == null || !endpoint.CubeBlock.HasInventory)
			{
				return endpoint.CubeBlock == m_interactedEndpointBlock;
			}
			return true;
		}

		private static bool EndpointPredicateStatic(IMyConveyorEndpoint endpoint)
		{
			if (endpoint.CubeBlock != null)
			{
				return endpoint.CubeBlock.HasInventory;
			}
			return false;
		}

		private void DisableUnreachableInventoryControls(MyInventory srcInventory, MyPhysicalInventoryItem item, MyGuiControlList list)
		{
			bool flag = srcInventory.Owner == m_userAsOwner;
			bool flag2 = srcInventory.Owner == m_interactedAsOwner;
			MyEntity owner = srcInventory.Owner;
			IMyConveyorEndpointBlock myConveyorEndpointBlock = null;
			if (flag)
			{
				myConveyorEndpointBlock = (m_interactedAsEntity as IMyConveyorEndpointBlock);
			}
			else if (owner != null)
			{
				myConveyorEndpointBlock = (owner as IMyConveyorEndpointBlock);
			}
			IMyConveyorEndpointBlock myConveyorEndpointBlock2 = null;
			if (m_interactedAsEntity != null)
			{
				myConveyorEndpointBlock2 = (m_interactedAsEntity as IMyConveyorEndpointBlock);
			}
			if (myConveyorEndpointBlock != null)
			{
				long localPlayerId = MySession.Static.LocalPlayerId;
				m_interactedEndpointBlock = myConveyorEndpointBlock2;
				MyGridConveyorSystem.AppendReachableEndpoints(myConveyorEndpointBlock.ConveyorEndpoint, localPlayerId, m_reachableInventoryOwners, item.Content.GetId(), m_endpointPredicate);
			}
			foreach (MyGuiControlBase visibleControl in list.Controls.GetVisibleControls())
			{
				if (visibleControl.Enabled)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner = (MyGuiControlInventoryOwner)visibleControl;
					MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
					IMyConveyorEndpoint item2 = null;
					IMyConveyorEndpointBlock myConveyorEndpointBlock3 = inventoryOwner as IMyConveyorEndpointBlock;
					if (myConveyorEndpointBlock3 != null)
					{
						item2 = myConveyorEndpointBlock3.ConveyorEndpoint;
					}
					bool num = inventoryOwner == owner;
					bool flag3 = (flag && inventoryOwner == m_interactedAsOwner) || (flag2 && inventoryOwner == m_userAsOwner);
					bool num2 = !num && !flag3;
					bool flag4 = !m_reachableInventoryOwners.Contains(item2);
					bool flag5 = myConveyorEndpointBlock2 != null && m_reachableInventoryOwners.Contains(myConveyorEndpointBlock2.ConveyorEndpoint);
					bool flag6 = inventoryOwner == m_userAsOwner && flag5;
					if (num2 && flag4 && !flag6)
					{
						for (int i = 0; i < inventoryOwner.InventoryCount; i++)
						{
							if (myGuiControlInventoryOwner.ContentGrids[i].Enabled)
							{
								myGuiControlInventoryOwner.ContentGrids[i].Enabled = false;
								m_controlsDisabledWhileDragged.Add(myGuiControlInventoryOwner.ContentGrids[i]);
							}
						}
					}
				}
			}
			m_reachableInventoryOwners.Clear();
		}

		private static void GetGridInventories(MyCubeGrid grid, List<MyEntity> outputInventories, MyEntity interactedEntity, long identityId)
		{
			grid?.GridSystems.ConveyorSystem.GetGridInventories(interactedEntity, outputInventories, identityId);
		}

		private void CreateInventoryControlInList(MyEntity owner, MyGuiControlList listControl)
		{
			List<MyEntity> list = new List<MyEntity>();
			if (owner != null)
			{
				list.Add(owner);
			}
			CreateInventoryControlsInList(list, listControl);
		}

		private void CreateInventoryControlsInList(List<MyEntity> owners, MyGuiControlList listControl, MyInventoryOwnerTypeEnum? filterType = null)
		{
			if (listControl.Controls.Contains(m_focusedOwnerControl))
			{
				m_focusedOwnerControl = null;
			}
			List<MyGuiControlBase> list = new List<MyGuiControlBase>();
			foreach (MyEntity owner in owners)
			{
				if (owner != null && owner.HasInventory && (!filterType.HasValue || owner.InventoryOwnerType() == filterType))
				{
					Vector4 labelColorMask = Color.White.ToVector4();
					if (owner is MyCubeBlock)
					{
						labelColorMask = m_colorHelper.GetGridColor((owner as MyCubeBlock).CubeGrid).ToVector4();
					}
					MyGuiControlInventoryOwner myGuiControlInventoryOwner = new MyGuiControlInventoryOwner(owner, labelColorMask);
					myGuiControlInventoryOwner.Size = new Vector2(listControl.Size.X - 0.05f, myGuiControlInventoryOwner.Size.Y);
					myGuiControlInventoryOwner.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
					foreach (MyGuiControlGrid contentGrid in myGuiControlInventoryOwner.ContentGrids)
					{
						contentGrid.ItemSelected += grid_ItemSelected;
						contentGrid.ItemDragged += grid_ItemDragged;
						contentGrid.ItemDoubleClicked += grid_ItemDoubleClicked;
						contentGrid.ItemClicked += grid_ItemClicked;
						contentGrid.ItemAccepted += grid_ItemDoubleClicked;
						contentGrid.ItemReleased += grid_ItemReleased;
						contentGrid.ReleasedWithoutItem += grid_ReleasedWithoutItem;
					}
					myGuiControlInventoryOwner.SizeChanged += inventoryControl_SizeChanged;
					myGuiControlInventoryOwner.InventoryContentsChanged += ownerControl_InventoryContentsChanged;
					if (owner is MyCubeBlock)
					{
						myGuiControlInventoryOwner.Enabled = (owner as MyCubeBlock).IsFunctional;
					}
					if (owner == m_interactedAsOwner || owner == m_userAsOwner)
					{
						list.Insert(0, myGuiControlInventoryOwner);
					}
					else
					{
						list.Add(myGuiControlInventoryOwner);
					}
				}
			}
			listControl.InitControls(list);
		}

		private void grid_ReleasedWithoutItem(MyGuiControlGrid obj)
		{
			m_focusedGridControl = obj;
			m_focusedOwnerControl = (MyGuiControlInventoryOwner)obj.Owner;
			RefreshSelectedInventoryItem();
		}

		private void ShowAmountTransferDialog(MyPhysicalInventoryItem inventoryItem, Action<float> onConfirmed)
		{
			MyFixedPoint amount = inventoryItem.Amount;
			MyObjectBuilderType typeId = inventoryItem.Content.TypeId;
			int minMaxDecimalDigits = 0;
			bool parseAsInteger = true;
			if (typeId == typeof(MyObjectBuilder_Ore) || typeId == typeof(MyObjectBuilder_Ingot))
			{
				minMaxDecimalDigits = 2;
				parseAsInteger = false;
			}
			MyGuiScreenDialogAmount dialog = new MyGuiScreenDialogAmount(0f, (float)amount, MyCommonTexts.DialogAmount_AddAmountCaption, minMaxDecimalDigits, parseAsInteger, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity);
			dialog.OnConfirmed += onConfirmed;
			if (m_interactedAsEntity != null)
			{
				Action<MyEntity> entityCloseAction = null;
				entityCloseAction = delegate
				{
					dialog.CloseScreen();
				};
				m_interactedAsEntity.OnClose += entityCloseAction;
				dialog.Closed += delegate
				{
					m_interactedAsEntity.OnClose -= entityCloseAction;
				};
			}
			MyGuiSandbox.AddScreen(dialog);
		}

		private bool TransferToOppositeFirst(MyPhysicalInventoryItem item, MyGuiControlGrid sender)
		{
			if (sender == null || sender.Owner == null)
			{
				return false;
			}
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = sender.Owner as MyGuiControlInventoryOwner;
			ObservableCollection<MyGuiControlBase>.Enumerator enumerator = ((myGuiControlInventoryOwner.Owner == m_leftOwnersControl) ? m_rightOwnersControl : m_leftOwnersControl).Controls.GetEnumerator();
			MyGuiControlInventoryOwner myGuiControlInventoryOwner2 = null;
			myGuiControlInventoryOwner2 = ((myGuiControlInventoryOwner.Owner == m_leftOwnersControl) ? (RightFocusedInventory?.Owner as MyGuiControlInventoryOwner) : (LeftFocusedInventory?.Owner as MyGuiControlInventoryOwner));
			if (myGuiControlInventoryOwner2 == null)
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Visible)
					{
						myGuiControlInventoryOwner2 = (enumerator.Current as MyGuiControlInventoryOwner);
						break;
					}
				}
			}
			if (myGuiControlInventoryOwner2 == null || !myGuiControlInventoryOwner2.Enabled)
			{
				return false;
			}
			if ((myGuiControlInventoryOwner.InventoryOwner != m_userAsOwner && myGuiControlInventoryOwner.InventoryOwner != m_interactedAsOwner) || (myGuiControlInventoryOwner2.InventoryOwner != m_userAsOwner && myGuiControlInventoryOwner2.InventoryOwner != m_interactedAsOwner))
			{
				bool flag = myGuiControlInventoryOwner.InventoryOwner is MyCharacter;
				bool flag2 = myGuiControlInventoryOwner2.InventoryOwner is MyCharacter;
				IMyConveyorEndpointBlock myConveyorEndpointBlock = (myGuiControlInventoryOwner.InventoryOwner == null) ? null : ((flag ? m_interactedAsOwner : myGuiControlInventoryOwner.InventoryOwner) as IMyConveyorEndpointBlock);
				IMyConveyorEndpointBlock myConveyorEndpointBlock2 = (myGuiControlInventoryOwner2.InventoryOwner == null) ? null : ((flag2 ? m_interactedAsOwner : myGuiControlInventoryOwner2.InventoryOwner) as IMyConveyorEndpointBlock);
				if (myConveyorEndpointBlock == null || myConveyorEndpointBlock2 == null)
				{
					return false;
				}
				try
				{
					MyGridConveyorSystem.AppendReachableEndpoints(myConveyorEndpointBlock.ConveyorEndpoint, MySession.Static.LocalPlayerId, m_reachableInventoryOwners, item.Content.GetId(), m_endpointPredicate);
					if (!m_reachableInventoryOwners.Contains(myConveyorEndpointBlock2.ConveyorEndpoint))
					{
						return false;
					}
				}
				finally
				{
					m_reachableInventoryOwners.Clear();
				}
				if (!MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint))
				{
					return false;
				}
			}
			MyEntity inventoryOwner = myGuiControlInventoryOwner2.InventoryOwner;
			_ = myGuiControlInventoryOwner.InventoryOwner;
			MyInventory myInventory = (MyInventory)sender.UserData;
			MyInventory myInventory2 = (myGuiControlInventoryOwner.Owner == m_leftOwnersControl) ? (RightFocusedInventory?.UserData as MyInventory) : (LeftFocusedInventory?.UserData as MyInventory);
			if (myInventory2 == null)
			{
				for (int i = 0; i < inventoryOwner.InventoryCount; i++)
				{
					MyInventory inventory = inventoryOwner.GetInventory(i);
					if (inventory.CheckConstraint(item.Content.GetId()))
					{
						myInventory2 = inventory;
						break;
					}
				}
			}
			else if (!myInventory2.CheckConstraint(item.Content.GetId()))
			{
				return false;
			}
			if (myInventory2 == null)
			{
				return false;
			}
			MyFixedPoint value = item.Amount;
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
			{
				value = ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? ((MyFixedPoint)1) : ((MyFixedPoint)100));
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
			{
				value = 10;
			}
			MyInventory.TransferByUser(myInventory, myInventory2, myInventory.GetItems()[sender.SelectedIndex.Value].ItemId, -1, value);
			return true;
		}

		private void SetLeftFilter(MyInventoryOwnerTypeEnum? filterType)
		{
			m_leftFilterType = filterType;
			if (m_leftFilterType.HasValue)
			{
				m_leftFilterCurrentShipOnly = false;
			}
			if (m_leftShowsGrid)
			{
				CreateInventoryControlsInList(m_leftFilterCurrentShipOnly ? m_interactedGridOwnersMechanical : m_interactedGridOwners, m_leftOwnersControl, m_leftFilterType);
				m_searchBoxLeft.SearchText = m_searchBoxLeft.SearchText;
			}
			LeftFocusedInventory = ((m_leftOwnersControl.Controls.Count > 0) ? (m_leftOwnersControl.Controls[0] as MyGuiControlInventoryOwner).ContentGrids[0] : null);
			RefreshSelectedInventoryItem();
		}

		private void SetRightFilter(MyInventoryOwnerTypeEnum? filterType)
		{
			m_rightFilterType = filterType;
			if (m_rightFilterType.HasValue)
			{
				m_rightFilterCurrentShipOnly = false;
			}
			if (m_rightShowsGrid)
			{
				CreateInventoryControlsInList(m_rightFilterCurrentShipOnly ? m_interactedGridOwnersMechanical : m_interactedGridOwners, m_rightOwnersControl, m_rightFilterType);
				m_searchBoxRight.SearchText = m_searchBoxRight.SearchText;
			}
			RightFocusedInventory = ((m_rightOwnersControl.Controls.Count > 0) ? (m_rightOwnersControl.Controls[0] as MyGuiControlInventoryOwner).ContentGrids[0] : null);
			RefreshSelectedInventoryItem();
		}

		private void RefreshSelectedInventoryItem()
		{
			if (m_focusedGridControl != null)
			{
				m_selectedInventory = (MyInventory)m_focusedGridControl.UserData;
				MyGuiGridItem selectedItem = m_focusedGridControl.SelectedItem;
				m_selectedInventoryItem = ((selectedItem != null) ? ((MyPhysicalInventoryItem?)selectedItem.UserData) : null);
				if (m_focusedGridControl?.Owner?.Owner == m_leftOwnersControl)
				{
					LeftFocusedInventory = m_focusedGridControl;
				}
				else if (m_focusedGridControl?.Owner?.Owner == m_rightOwnersControl)
				{
					RightFocusedInventory = m_focusedGridControl;
				}
			}
			else
			{
				m_selectedInventory = null;
				m_selectedInventoryItem = null;
			}
			if (m_throwOutButton != null)
			{
				m_throwOutButton.Enabled = (m_selectedInventoryItem.HasValue && m_selectedInventoryItem.HasValue && m_focusedOwnerControl != null && m_focusedOwnerControl.InventoryOwner == m_userAsOwner);
				if (m_throwOutButton.Enabled)
				{
					m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOut);
				}
				else
				{
					m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOutDisabled);
				}
			}
			if (m_selectedToProductionButton != null)
			{
				if (m_selectedInventoryItem.HasValue && m_selectedInventoryItem.Value.Content != null && m_interactedAsEntity != null)
				{
					MyDefinitionId id = m_selectedInventoryItem.Value.Content.GetId();
					MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(id);
					m_selectedToProductionButton.Enabled = (myBlueprintDefinitionBase != null);
				}
				else
				{
					m_selectedToProductionButton.Enabled = false;
				}
			}
			m_depositAllButton.Enabled = (m_interactedAsEntity != null);
			m_addToProductionButton.Enabled = (m_interactedAsEntity != null && m_interactedAsEntity.Parent is MyCubeGrid && MySession.Static.LocalCharacter.BuildPlanner.Count > 0);
			m_withdrawButton.Enabled = (m_interactedAsEntity != null && MySession.Static.LocalCharacter.BuildPlanner.Count > 0);
		}

		private MyCubeGrid GetInteractedGrid()
		{
			if (m_interactedAsEntity == null)
			{
				return null;
			}
			return m_interactedAsEntity.Parent as MyCubeGrid;
		}

		private void ApplyTypeGroupSelectionChange(MyGuiControlRadioButtonGroup obj, ref bool showsGrid, MyGuiControlList targetControlList, MyInventoryOwnerTypeEnum? filterType, MyGuiControlRadioButtonGroup filterButtonGroup, MyGuiControlCheckbox showEmpty, MyGuiControlLabel showEmptyLabel, MyGuiControlSearchBox searchBox, bool isLeftControllist)
		{
			switch (obj.SelectedButton.VisualStyle)
			{
			case MyGuiControlRadioButtonStyleEnum.FilterCharacter:
				showsGrid = false;
				showEmpty.Visible = false;
				showEmptyLabel.Visible = false;
				searchBox.Visible = false;
				targetControlList.Position = (isLeftControllist ? new Vector2(-0.46f, -0.276f) : new Vector2(0.4595f, -0.276f));
				targetControlList.Size = m_controlListFullSize;
				if (targetControlList == m_leftOwnersControl)
				{
					CreateInventoryControlInList(m_userAsOwner, targetControlList);
				}
				else
				{
					CreateInventoryControlInList(m_interactedAsOwner, targetControlList);
				}
				break;
			case MyGuiControlRadioButtonStyleEnum.FilterGrid:
			{
				showsGrid = true;
				bool flag = (targetControlList == m_leftOwnersControl) ? m_leftFilterCurrentShipOnly : m_rightFilterCurrentShipOnly;
				CreateInventoryControlsInList(flag ? m_interactedGridOwnersMechanical : m_interactedGridOwners, targetControlList, filterType);
				showEmpty.Visible = true;
				showEmptyLabel.Visible = true;
				searchBox.Visible = true;
				searchBox.SearchText = searchBox.SearchText;
				targetControlList.Position = (isLeftControllist ? new Vector2(-0.46f, -0.227f) : new Vector2(0.4595f, -0.227f));
				targetControlList.Size = m_controlListSizeWithSearch;
				break;
			}
			}
			foreach (MyGuiControlRadioButton item in filterButtonGroup)
			{
				bool visible = item.Enabled = showsGrid;
				item.Visible = visible;
			}
			RefreshSelectedInventoryItem();
		}

		private void ConveyorSystem_BlockAdded(MyCubeBlock obj)
		{
			m_interactedGridOwners.Add(obj);
			if (m_leftShowsGrid)
			{
				LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			}
			if (m_rightShowsGrid)
			{
				RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			}
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void ConveyorSystem_BlockRemoved(MyCubeBlock obj)
		{
			m_interactedGridOwners.Remove(obj);
			UpdateSelection();
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void ConveyorSystemMechanical_BlockAdded(MyCubeBlock obj)
		{
			m_interactedGridOwnersMechanical.Add(obj);
			if (m_leftShowsGrid)
			{
				LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			}
			if (m_rightShowsGrid)
			{
				RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			}
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void ConveyorSystemMechanical_BlockRemoved(MyCubeBlock obj)
		{
			m_interactedGridOwnersMechanical.Remove(obj);
			UpdateSelection();
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void UpdateSelection()
		{
			m_selectionDirty = (m_leftShowsGrid || m_rightShowsGrid);
		}

		public void UpdateBeforeDraw()
		{
			if (m_selectionDirty)
			{
				m_selectionDirty = false;
				if (m_leftShowsGrid)
				{
					LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
				}
				if (m_rightShowsGrid)
				{
					RightTypeGroup_SelectedChanged(m_rightTypeGroup);
				}
			}
		}

		private void LeftTypeGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			ApplyTypeGroupSelectionChange(obj, ref m_leftShowsGrid, m_leftOwnersControl, m_leftFilterType, m_leftFilterGroup, m_hideEmptyLeft, m_hideEmptyLeftLabel, m_searchBoxLeft, isLeftControllist: true);
			m_leftOwnersControl.SetScrollBarPage();
			if (obj.SelectedIndex.HasValue)
			{
				m_persistentRadioSelectionLeft = obj.SelectedIndex.Value;
			}
			CheckFocusedInventoryVisibilityLeft();
		}

		private void RightTypeGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			ApplyTypeGroupSelectionChange(obj, ref m_rightShowsGrid, m_rightOwnersControl, m_rightFilterType, m_rightFilterGroup, m_hideEmptyRight, m_hideEmptyRightLabel, m_searchBoxRight, isLeftControllist: false);
			m_rightOwnersControl.SetScrollBarPage();
			if (obj.SelectedIndex.HasValue)
			{
				m_persistentRadioSelectionRight = obj.SelectedIndex.Value;
			}
			CheckFocusedInventoryVisibilityRight();
		}

		private void throwOutButton_OnButtonClick(MyGuiControlButton sender)
		{
			MyEntity inventoryOwner = m_focusedOwnerControl.InventoryOwner;
			if (m_selectedInventoryItem.HasValue && inventoryOwner != null && m_focusedOwnerControl.InventoryOwner == m_userAsOwner)
			{
				MyPhysicalInventoryItem value = m_selectedInventoryItem.Value;
				if (m_focusedGridControl.SelectedIndex.HasValue)
				{
					m_selectedInventory.DropItem(m_focusedGridControl.SelectedIndex.Value, value.Amount);
				}
			}
			RefreshSelectedInventoryItem();
		}

		private void interactedObjectButton_OnButtonClick(MyGuiControlButton sender)
		{
			CreateInventoryControlInList(m_interactedAsOwner, m_rightOwnersControl);
		}

		private void grid_ItemSelected(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (m_focusedGridControl != null && m_focusedGridControl != sender)
			{
				m_focusedGridControl.SelectedIndex = null;
			}
			m_focusedGridControl = sender;
			m_focusedOwnerControl = (MyGuiControlInventoryOwner)sender.Owner;
			RefreshSelectedInventoryItem();
		}

		private void grid_ItemDragged(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
			{
				StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);
			}
		}

		private void grid_ItemDoubleClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData;
				TransferToOppositeFirst(item, sender);
				RefreshSelectedInventoryItem();
			}
		}

		private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			bool flag = MyInput.Static.IsAnyCtrlKeyPressed();
			bool flag2 = MyInput.Static.IsAnyShiftKeyPressed();
			if (flag || flag2)
			{
				MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData;
				item.Amount = MyFixedPoint.Min(((!flag2) ? 1 : 100) * ((!flag) ? 1 : 10), item.Amount);
				TransferToOppositeFirst(item, sender);
				RefreshSelectedInventoryItem();
			}
			else if (((MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData).Content != null)
			{
				MyInventory myInventory = m_focusedGridControl.UserData as MyInventory;
				MyCharacter myCharacter = null;
				if (myInventory != null)
				{
					myCharacter = (myInventory.Owner as MyCharacter);
				}
				MyShipController myShipController;
				if (myCharacter == null && myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = (myInventory.Owner as MyShipController)) != null)
				{
					myCharacter = myShipController.Pilot;
				}
			}
		}

		private void grid_ItemReleased(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData;
			if (item.Content != null)
			{
				MyInventory myInventory = m_focusedGridControl.UserData as MyInventory;
				MyCharacter myCharacter = null;
				if (myInventory != null)
				{
					myCharacter = (myInventory.Owner as MyCharacter);
				}
				MyShipController myShipController;
				if (myCharacter == null && myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = (myInventory.Owner as MyShipController)) != null)
				{
					myCharacter = myShipController.Pilot;
				}
				if (myCharacter != null)
				{
					MyUsableItemHelper.ItemClicked(item, myInventory, myCharacter, eventArgs.Button);
				}
			}
		}

		private void dragDrop_OnItemDropped(object sender, MyDragAndDropEventArgs eventArgs)
		{
			if (eventArgs.DropTo != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				MyPhysicalInventoryItem inventoryItem = (MyPhysicalInventoryItem)eventArgs.Item.UserData;
				MyGuiControlGrid grid = eventArgs.DragFrom.Grid;
				MyGuiControlGrid dstGrid = eventArgs.DropTo.Grid;
				_ = (MyGuiControlInventoryOwner)grid.Owner;
				if (!(dstGrid.Owner is MyGuiControlInventoryOwner))
				{
					return;
				}
				MyInventory srcInventory = (MyInventory)grid.UserData;
				MyInventory dstInventory = (MyInventory)dstGrid.UserData;
				if (grid == dstGrid)
				{
					if (eventArgs.DragButton == MySharedButtonsEnum.Secondary)
					{
						ShowAmountTransferDialog(inventoryItem, delegate(float amount)
						{
							if (amount != 0f && srcInventory.IsItemAt(eventArgs.DragFrom.ItemIndex))
							{
								inventoryItem.Amount = (MyFixedPoint)amount;
								CorrectItemAmount(ref inventoryItem);
								MyInventory.TransferByUser(srcInventory, srcInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex, inventoryItem.Amount);
								if (dstGrid.IsValidIndex(eventArgs.DropTo.ItemIndex))
								{
									dstGrid.SelectedIndex = eventArgs.DropTo.ItemIndex;
								}
								else
								{
									dstGrid.SelectLastItem();
								}
								RefreshSelectedInventoryItem();
							}
						});
					}
					else
					{
						MyInventory.TransferByUser(srcInventory, srcInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex);
						if (dstGrid.IsValidIndex(eventArgs.DropTo.ItemIndex))
						{
							dstGrid.SelectedIndex = eventArgs.DropTo.ItemIndex;
						}
						else
						{
							dstGrid.SelectLastItem();
						}
						RefreshSelectedInventoryItem();
					}
				}
				else if (eventArgs.DragButton == MySharedButtonsEnum.Secondary)
				{
					ShowAmountTransferDialog(inventoryItem, delegate(float amount)
					{
						if (amount != 0f && srcInventory.IsItemAt(eventArgs.DragFrom.ItemIndex))
						{
							inventoryItem.Amount = (MyFixedPoint)amount;
							CorrectItemAmount(ref inventoryItem);
							MyInventory.TransferByUser(srcInventory, dstInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex, inventoryItem.Amount);
							RefreshSelectedInventoryItem();
						}
					});
				}
				else
				{
					MyInventory.TransferByUser(srcInventory, dstInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex);
					RefreshSelectedInventoryItem();
				}
			}
			else if (((MyGuiControlGridDragAndDrop)sender).IsEmptySpace() && m_throwOutButton.Enabled)
			{
				throwOutButton_OnButtonClick(m_throwOutButton);
			}
			ClearDisabledControls();
			m_dragAndDropInfo = null;
		}

		private void ClearDisabledControls()
		{
			foreach (MyGuiControlGrid item in m_controlsDisabledWhileDragged)
			{
				item.Enabled = true;
			}
			m_controlsDisabledWhileDragged.Clear();
		}

		private static void CorrectItemAmount(ref MyPhysicalInventoryItem dragItem)
		{
			_ = dragItem.Content.TypeId;
		}

		private void inventoryControl_SizeChanged(MyGuiControlBase obj)
		{
			((MyGuiControlList)obj.Owner).Recalculate();
		}

		private void ownerControl_InventoryContentsChanged(MyGuiControlInventoryOwner control)
		{
			if (control == m_focusedOwnerControl)
			{
				RefreshSelectedInventoryItem();
			}
			UpdateDisabledControlsWhileDragging(control);
		}

		private void UpdateDisabledControlsWhileDragging(MyGuiControlInventoryOwner control)
		{
			if (m_controlsDisabledWhileDragged.Count == 0)
			{
				return;
			}
			MyEntity inventoryOwner = control.InventoryOwner;
			for (int i = 0; i < inventoryOwner.InventoryCount; i++)
			{
				MyGuiControlGrid myGuiControlGrid = control.ContentGrids[i];
				if (m_controlsDisabledWhileDragged.Contains(myGuiControlGrid) && myGuiControlGrid.Enabled)
				{
					myGuiControlGrid.Enabled = false;
				}
			}
		}

		private void HideEmptyLeft_Checked(MyGuiControlCheckbox obj)
		{
			if (m_leftFilterType != MyInventoryOwnerTypeEnum.Character)
			{
				SearchInList(m_searchBoxLeft.TextBox, m_leftOwnersControl, obj.IsChecked);
				CheckFocusedInventoryVisibilityLeft();
			}
		}

		private void CheckFocusedInventoryVisibilityLeft()
		{
			if (LeftFocusedInventory == null || LeftFocusedInventory.Owner == null || !LeftFocusedInventory.Owner.Visible)
			{
				LeftFocusedInventory = null;
			}
		}

		private void HideEmptyRight_Checked(MyGuiControlCheckbox obj)
		{
			if (m_rightFilterType != MyInventoryOwnerTypeEnum.Character)
			{
				SearchInList(m_searchBoxRight.TextBox, m_rightOwnersControl, obj.IsChecked);
				CheckFocusedInventoryVisibilityRight();
			}
		}

		private void CheckFocusedInventoryVisibilityRight()
		{
			if (RightFocusedInventory == null || RightFocusedInventory.Owner == null || !RightFocusedInventory.Owner.Visible)
			{
				RightFocusedInventory = null;
			}
		}

		private void BlockSearchLeft_TextChanged(string obj)
		{
			if (m_leftFilterType != MyInventoryOwnerTypeEnum.Character)
			{
				SearchInList(m_searchBoxLeft.TextBox, m_leftOwnersControl, m_hideEmptyLeft.IsChecked);
				MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
				foreach (MyGuiControlBase control in m_leftOwnersControl.Controls)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner2;
					if (control.Visible && (myGuiControlInventoryOwner2 = (control as MyGuiControlInventoryOwner)) != null)
					{
						myGuiControlInventoryOwner = myGuiControlInventoryOwner2;
						break;
					}
				}
				if (myGuiControlInventoryOwner != null && myGuiControlInventoryOwner.ContentGrids.Count > 0)
				{
					m_focusedGridControl = myGuiControlInventoryOwner.ContentGrids[0];
					m_focusedOwnerControl = myGuiControlInventoryOwner;
					RefreshSelectedInventoryItem();
				}
			}
		}

		private void BlockSearchRight_TextChanged(string obj)
		{
			if (m_rightFilterType != MyInventoryOwnerTypeEnum.Character)
			{
				SearchInList(m_searchBoxRight.TextBox, m_rightOwnersControl, m_hideEmptyRight.IsChecked);
				MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
				foreach (MyGuiControlBase control in m_rightOwnersControl.Controls)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner2;
					if (control.Visible && (myGuiControlInventoryOwner2 = (control as MyGuiControlInventoryOwner)) != null)
					{
						myGuiControlInventoryOwner = myGuiControlInventoryOwner2;
						break;
					}
				}
				if (myGuiControlInventoryOwner != null && myGuiControlInventoryOwner.ContentGrids.Count > 0)
				{
					m_focusedGridControl = myGuiControlInventoryOwner.ContentGrids[0];
					m_focusedOwnerControl = myGuiControlInventoryOwner;
					RefreshSelectedInventoryItem();
				}
			}
		}

		private void SearchInList(MyGuiControlTextbox searchText, MyGuiControlList list, bool hideEmpty)
		{
			if (searchText.Text != "")
			{
				string[] array = searchText.Text.ToLower().Split(new char[1]
				{
					' '
				});
				foreach (MyGuiControlBase control in list.Controls)
				{
					MyEntity inventoryOwner = (control as MyGuiControlInventoryOwner).InventoryOwner;
					string text = inventoryOwner.DisplayNameText.ToString().ToLower();
					bool flag = true;
					bool flag2 = true;
					string[] array2 = array;
					foreach (string value in array2)
					{
						if (!text.Contains(value))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						for (int j = 0; j < inventoryOwner.InventoryCount; j++)
						{
							foreach (MyPhysicalInventoryItem item in inventoryOwner.GetInventory(j).GetItems())
							{
								bool flag3 = true;
								string text2 = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content).DisplayNameText.ToString().ToLower();
								array2 = array;
								foreach (string value2 in array2)
								{
									if (!text2.Contains(value2))
									{
										flag3 = false;
										break;
									}
								}
								if (flag3)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
					if (flag)
					{
						for (int k = 0; k < inventoryOwner.InventoryCount; k++)
						{
							if (inventoryOwner.GetInventory(k).CurrentMass != 0)
							{
								flag2 = false;
								break;
							}
						}
						control.Visible = ((!(hideEmpty && flag2)) ? true : false);
					}
					else
					{
						control.Visible = false;
					}
				}
			}
			else
			{
				foreach (MyGuiControlBase control2 in list.Controls)
				{
					bool flag4 = true;
					MyEntity inventoryOwner2 = (control2 as MyGuiControlInventoryOwner).InventoryOwner;
					for (int l = 0; l < inventoryOwner2.InventoryCount; l++)
					{
						if (inventoryOwner2.GetInventory(l).CurrentMass != 0)
						{
							flag4 = false;
							break;
						}
					}
					if (hideEmpty && flag4)
					{
						control2.Visible = false;
					}
					else
					{
						control2.Visible = true;
					}
				}
			}
			list.SetScrollBarPage();
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1))
			{
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
				{
					throwOutButton_OnButtonClick(null);
				}
				else
				{
					DepositAllButton_ButtonClicked(null);
				}
			}
		}

		public MyGuiControlGrid GetDefaultFocus()
		{
			return LeftFocusedInventory;
		}
	}
}
