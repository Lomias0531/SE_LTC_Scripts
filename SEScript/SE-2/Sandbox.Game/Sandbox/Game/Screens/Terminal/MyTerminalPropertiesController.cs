using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
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

namespace Sandbox.Game.Screens.Terminal
{
	internal class MyTerminalPropertiesController
	{
		private enum MyCubeGridConnectionStatus
		{
			PhysicallyConnected,
			Connected,
			OutOfBroadcastingRange,
			OutOfReceivingRange,
			Me,
			IsPreviewGrid
		}

		private enum MyRefuseReason
		{
			NoRemoteControl,
			NoMainRemoteControl,
			NoStableConnection,
			NoOwner,
			NoProblem,
			PlayerBroadcastOff,
			Forbidden
		}

		private struct UserData
		{
			public long GridEntityId;

			public long? RemoteEntityId;

			public bool IsSelectable;
		}

		private class CubeGridInfo
		{
			public long EntityId;

			public double Distance;

			public string Name;

			public StringBuilder AppendedDistance;

			public MyCubeGridConnectionStatus Status;

			public bool Owned;

			public MyRefuseReason RemoteStatus;

			public long? RemoteId;

			public override bool Equals(object obj)
			{
				if (!(obj is CubeGridInfo))
				{
					return false;
				}
				CubeGridInfo cubeGridInfo = obj as CubeGridInfo;
				string text = (Name == null) ? "" : Name;
				string value = (cubeGridInfo.Name == null) ? "" : cubeGridInfo.Name;
				if (EntityId.Equals(cubeGridInfo.EntityId) && text.Equals(value) && AppendedDistance.Equals(cubeGridInfo.AppendedDistance))
				{
					return Status == cubeGridInfo.Status;
				}
				return false;
			}

			public override int GetHashCode()
			{
				int hashCode = EntityId.GetHashCode();
				string text = (Name == null) ? "" : Name;
				return (((((hashCode * 397) ^ text.GetHashCode()) * 397) ^ AppendedDistance.GetHashCode()) * 397) ^ (int)Status;
			}
		}

		private MyGuiControlCombobox m_shipsInRange;

		private MyGuiControlButton m_button;

		private MyGuiControlTable m_shipsData;

		private MyEntity m_interactedEntityRepresentative;

		private MyEntity m_openInventoryInteractedEntityRepresentative;

		private MyEntity m_interactedEntity;

		private bool m_isRemote;

		private int m_columnToSort;

		private HashSet<MyDataReceiver> m_tmpAntennas = new HashSet<MyDataReceiver>();

		private Dictionary<long, CubeGridInfo> m_tmpGridInfoOutput = new Dictionary<long, CubeGridInfo>();

		private HashSet<MyDataBroadcaster> m_tmpBroadcasters = new HashSet<MyDataBroadcaster>();

		private List<MyDataBroadcaster> m_tempBroadcasters = new List<MyDataBroadcaster>();

		private List<MyDataBroadcaster> m_tempSendingToGrid = new List<MyDataBroadcaster>();

		private List<MyDataBroadcaster> m_tempReceivingFromGrid = new List<MyDataBroadcaster>();

		private HashSet<MyAntennaSystem.BroadcasterInfo> previousMutualConnectionGrids;

		private HashSet<CubeGridInfo> previousShipInfo;

		private int cnt;

		public event Action ButtonClicked;

		public void Init(MyGuiControlParent menuParent, MyGuiControlParent panelParent, MyEntity interactedEntity, MyEntity openInventoryInteractedEntity, bool isRemote)
		{
			m_interactedEntityRepresentative = GetInteractedEntityRepresentative(interactedEntity);
			m_openInventoryInteractedEntityRepresentative = GetInteractedEntityRepresentative(openInventoryInteractedEntity);
			m_interactedEntity = (interactedEntity ?? MySession.Static.LocalCharacter);
			m_isRemote = isRemote;
			if (menuParent == null)
			{
				MySandboxGame.Log.WriteLine("menuParent is null");
			}
			if (panelParent == null)
			{
				MySandboxGame.Log.WriteLine("panelParent is null");
			}
			if (menuParent != null && panelParent != null)
			{
				m_shipsInRange = (MyGuiControlCombobox)menuParent.Controls.GetControlByName("ShipsInRange");
				m_button = (MyGuiControlButton)menuParent.Controls.GetControlByName("SelectShip");
				m_shipsData = (MyGuiControlTable)panelParent.Controls.GetControlByName("ShipsData");
				m_columnToSort = 1;
				m_button.ButtonClicked += Menu_ButtonClicked;
				m_shipsData.ColumnClicked += shipsData_ColumnClicked;
				m_shipsInRange.ItemSelected += shipsInRange_ItemSelected;
				Refresh();
			}
		}

		public void Refresh()
		{
			PopulateMutuallyConnectedCubeGrids(MyAntennaSystem.Static.GetConnectedGridsInfo(m_openInventoryInteractedEntityRepresentative, null, mutual: true, accessible: true));
			PopulateOwnedCubeGrids(GetAllCubeGridsInfo());
		}

		private void PopulateMutuallyConnectedCubeGrids(HashSet<MyAntennaSystem.BroadcasterInfo> playerMutualConnection)
		{
			m_shipsInRange.ClearItems();
			_ = m_openInventoryInteractedEntityRepresentative;
			m_shipsInRange.AddItem(m_openInventoryInteractedEntityRepresentative.EntityId, new StringBuilder(m_openInventoryInteractedEntityRepresentative.DisplayName));
			foreach (MyAntennaSystem.BroadcasterInfo item in playerMutualConnection)
			{
				if (m_shipsInRange.TryGetItemByKey(item.EntityId) == null)
				{
					m_shipsInRange.AddItem(item.EntityId, new StringBuilder(item.Name));
				}
			}
			m_shipsInRange.Visible = true;
			m_button.Visible = true;
			m_shipsInRange.SortItemsByValueText();
			if (m_shipsInRange.TryGetItemByKey(m_interactedEntityRepresentative.EntityId) == null && m_interactedEntityRepresentative is MyCubeGrid)
			{
				m_shipsInRange.AddItem(m_interactedEntityRepresentative.EntityId, new StringBuilder((m_interactedEntityRepresentative as MyCubeGrid).DisplayName));
			}
			m_shipsInRange.SelectItemByKey(m_interactedEntityRepresentative.EntityId);
		}

		private void PopulateOwnedCubeGrids(HashSet<CubeGridInfo> gridInfoList)
		{
			float value = m_shipsData.ScrollBar.Value;
			m_shipsData.Clear();
			m_shipsData.Controls.Clear();
			foreach (CubeGridInfo gridInfo in gridInfoList)
			{
				UserData userData = default(UserData);
				userData.GridEntityId = gridInfo.EntityId;
				userData.RemoteEntityId = gridInfo.RemoteId;
				MyGuiControlTable.Cell cell;
				MyGuiControlTable.Cell cell2;
				MyGuiControlTable.Cell cell3;
				MyGuiControlTable.Cell cell4;
				MyGuiControlTable.Cell cell5;
				if (gridInfo.Status == MyCubeGridConnectionStatus.Connected || gridInfo.Status == MyCubeGridConnectionStatus.PhysicallyConnected || gridInfo.Status == MyCubeGridConnectionStatus.Me)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (gridInfo.Status == MyCubeGridConnectionStatus.Connected)
					{
						stringBuilder = gridInfo.AppendedDistance;
					}
					userData.IsSelectable = true;
					cell = new MyGuiControlTable.Cell(new StringBuilder(gridInfo.Name), null, textColor: Color.White, toolTip: gridInfo.Name);
					cell2 = CreateControlCell(gridInfo, isActive: true);
					cell3 = new MyGuiControlTable.Cell(stringBuilder, toolTip: stringBuilder.ToString(), userData: gridInfo.Distance, textColor: Color.White);
					cell4 = CreateStatusIcons(gridInfo, isActive: true);
					cell5 = CreateTerminalCell(gridInfo, isActive: true);
				}
				else
				{
					userData.IsSelectable = false;
					cell = new MyGuiControlTable.Cell(new StringBuilder(gridInfo.Name), null, textColor: Color.Gray, toolTip: gridInfo.Name);
					cell2 = CreateControlCell(gridInfo, isActive: false);
					cell3 = new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.NotAvailable), double.MaxValue, MyTexts.GetString(MySpaceTexts.NotAvailable), Color.Gray);
					cell4 = CreateStatusIcons(gridInfo, isActive: true);
					cell5 = CreateTerminalCell(gridInfo, isActive: false);
				}
				MyGuiControlTable.Row row = new MyGuiControlTable.Row(userData);
				row.AddCell(cell);
				row.AddCell(cell3);
				row.AddCell(cell4);
				row.AddCell(cell2);
				row.AddCell(cell5);
				m_shipsData.Add(row);
				m_shipsData.SortByColumn(m_columnToSort, MyGuiControlTable.SortStateEnum.Ascending, switchSort: false);
			}
			m_shipsData.ScrollBar.ChangeValue(value);
		}

		private MyGuiControlTable.Cell CreateControlCell(CubeGridInfo gridInfo, bool isActive)
		{
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
			Vector2 value = new Vector2(0.1f, m_shipsData.RowHeight * 0.8f);
			MyRefuseReason remoteStatus = gridInfo.RemoteStatus;
			if ((uint)remoteStatus <= 1u || remoteStatus == MyRefuseReason.Forbidden)
			{
				isActive = false;
			}
			isActive &= CanTakeTerminalOuter(gridInfo);
			cell.Control = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MySpaceTexts.BroadcastScreen_TakeControlButton), size: value, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnButtonClicked_TakeControl);
			cell.Control.ShowTooltipWhenDisabled = true;
			cell.Control.Enabled = isActive;
			if (cell.Control.Enabled)
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TakeControlButton_ToolTip);
			}
			else
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TakeControlButtonDisabled_ToolTip);
			}
			m_shipsData.Controls.Add(cell.Control);
			return cell;
		}

		private bool CanTakeTerminalOuter(CubeGridInfo gridInfo)
		{
			bool result = true;
			MyRefuseReason myRefuseReason = CanTakeTerminal(gridInfo);
			if ((uint)(myRefuseReason - 2) <= 1u || (uint)(myRefuseReason - 5) <= 1u)
			{
				result = false;
			}
			return result;
		}

		private MyGuiControlTable.Cell CreateTerminalCell(CubeGridInfo gridInfo, bool isActive)
		{
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
			Vector2 value = new Vector2(0.1f, m_shipsData.RowHeight * 0.8f);
			isActive &= CanTakeTerminalOuter(gridInfo);
			cell.Control = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MySpaceTexts.BroadcastScreen_TerminalButton), size: value, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnButtonClicked_OpenTerminal);
			cell.Control.ShowTooltipWhenDisabled = true;
			cell.Control.Enabled = isActive;
			if (cell.Control.Enabled)
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TerminalButton_ToolTip);
			}
			else
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TerminalButtonDisabled_ToolTip);
			}
			m_shipsData.Controls.Add(cell.Control);
			return cell;
		}

		private MyGuiControlTable.Cell CreateStatusIcons(CubeGridInfo gridInfo, bool isActive)
		{
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
			float num = m_shipsData.RowHeight * 0.7f;
			bool flag2;
			bool flag;
			bool flag3 = flag2 = (flag = isActive);
			MyStringId toolTip;
			MyStringId toolTip2 = toolTip = MyStringId.NullOrEmpty;
			StringBuilder stringBuilder = new StringBuilder();
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			myGuiControlParent.CanPlaySoundOnMouseOver = false;
			MyRefuseReason myRefuseReason = CanTakeTerminal(gridInfo);
			MyRefuseReason remoteStatus = gridInfo.RemoteStatus;
			switch (myRefuseReason)
			{
			case MyRefuseReason.PlayerBroadcastOff:
				flag3 = false;
				toolTip2 = MySpaceTexts.BroadcastScreen_TerminalButton_PlayerBroadcastOffToolTip;
				break;
			case MyRefuseReason.NoStableConnection:
				flag3 = false;
				toolTip2 = MySpaceTexts.BroadcastScreen_TerminalButton_NoStableConnectionToolTip;
				break;
			case MyRefuseReason.Forbidden:
				flag3 = false;
				toolTip2 = MySpaceTexts.BroadcastScreen_NoOwnership;
				break;
			case MyRefuseReason.NoProblem:
				toolTip2 = MySpaceTexts.BroadcastScreen_TerminalButton_StableConnectionToolTip;
				break;
			}
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2(-1.25f * num, 0f), new Vector2(num * 0.78f, num), null, flag3 ? "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOff.png", null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlImage.SetToolTip(toolTip2);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			switch (remoteStatus)
			{
			case MyRefuseReason.NoRemoteControl:
				toolTip = MySpaceTexts.BroadcastScreen_TakeControlButton_NoRemoteToolTip;
				flag = false;
				break;
			case MyRefuseReason.NoMainRemoteControl:
				toolTip = MySpaceTexts.BroadcastScreen_TakeControlButton_NoMainRemoteControl;
				flag = false;
				break;
			case MyRefuseReason.NoOwner:
			case MyRefuseReason.NoProblem:
				toolTip = MySpaceTexts.BroadcastScreen_TakeControlButton_RemoteToolTip;
				break;
			}
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2(-0.25f * num, 0f), new Vector2(num * 0.78f, num), null, flag ? "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOff.png", null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlImage2.SetToolTip(toolTip);
			myGuiControlParent.Controls.Add(myGuiControlImage2);
			if ((myRefuseReason == MyRefuseReason.NoStableConnection || myRefuseReason == MyRefuseReason.PlayerBroadcastOff) && remoteStatus == MyRefuseReason.NoRemoteControl)
			{
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_UnavailableControlButton));
				flag2 = false;
			}
			if (flag2 && (myRefuseReason == MyRefuseReason.NoOwner || remoteStatus == MyRefuseReason.Forbidden || myRefuseReason == MyRefuseReason.NoStableConnection || myRefuseReason == MyRefuseReason.PlayerBroadcastOff))
			{
				flag2 = false;
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_NoOwnership));
			}
			if (myRefuseReason == MyRefuseReason.NoOwner)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.DisplayName_Block_Antenna));
			}
			if (remoteStatus == MyRefuseReason.Forbidden)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.DisplayName_Block_RemoteControl));
			}
			if (flag2)
			{
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_Ownership));
			}
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(new Vector2(0.75f * num, 0f), new Vector2(num * 0.78f, num), null, flag2 ? "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOff.png", null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlImage3.SetToolTip(stringBuilder.ToString());
			myGuiControlParent.Controls.Add(myGuiControlImage3);
			cell.Control = myGuiControlParent;
			m_shipsData.Controls.Add(myGuiControlParent);
			return cell;
		}

		private HashSet<CubeGridInfo> GetAllCubeGridsInfo()
		{
			HashSet<CubeGridInfo> hashSet = new HashSet<CubeGridInfo>();
			m_tmpGridInfoOutput.Clear();
			m_tmpBroadcasters.Clear();
			if (MySession.Static.LocalCharacter == null)
			{
				return hashSet;
			}
			foreach (MyDataBroadcaster allRelayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters(m_interactedEntityRepresentative, MySession.Static.LocalPlayerId, mutual: false, m_tmpBroadcasters))
			{
				if (allRelayedBroadcaster != MySession.Static.LocalCharacter.RadioBroadcaster && allRelayedBroadcaster.ShowInTerminal)
				{
					double playerBroadcasterDistance = GetPlayerBroadcasterDistance(allRelayedBroadcaster);
					MyCubeGridConnectionStatus broadcasterStatus = GetBroadcasterStatus(allRelayedBroadcaster);
					if (m_tmpGridInfoOutput.TryGetValue(allRelayedBroadcaster.Info.EntityId, out CubeGridInfo value))
					{
						if (value.Status > broadcasterStatus)
						{
							value.Status = broadcasterStatus;
						}
						if (value.Distance > playerBroadcasterDistance)
						{
							value.Distance = playerBroadcasterDistance;
							value.AppendedDistance = new StringBuilder().AppendDecimal(playerBroadcasterDistance, 0).Append(" m");
						}
						if (!value.Owned && allRelayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId))
						{
							value.Owned = true;
						}
					}
					else
					{
						m_tmpGridInfoOutput.Add(allRelayedBroadcaster.Info.EntityId, new CubeGridInfo
						{
							EntityId = allRelayedBroadcaster.Info.EntityId,
							Distance = playerBroadcasterDistance,
							AppendedDistance = new StringBuilder().AppendDecimal(playerBroadcasterDistance, 0).Append(" m"),
							Name = allRelayedBroadcaster.Info.Name,
							Status = broadcasterStatus,
							Owned = allRelayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId),
							RemoteStatus = GetRemoteStatus(allRelayedBroadcaster),
							RemoteId = allRelayedBroadcaster.MainRemoteControlId
						});
					}
				}
			}
			foreach (CubeGridInfo value2 in m_tmpGridInfoOutput.Values)
			{
				hashSet.Add(value2);
			}
			return hashSet;
		}

		private MyCubeGridConnectionStatus GetBroadcasterStatus(MyDataBroadcaster broadcaster)
		{
			if (!MyAntennaSystem.Static.CheckConnection(broadcaster.Receiver, m_openInventoryInteractedEntityRepresentative, MySession.Static.LocalPlayerId, mutual: false))
			{
				return MyCubeGridConnectionStatus.OutOfBroadcastingRange;
			}
			if (!MyAntennaSystem.Static.CheckConnection(m_openInventoryInteractedEntityRepresentative, broadcaster, MySession.Static.LocalPlayerId, mutual: false))
			{
				return MyCubeGridConnectionStatus.OutOfReceivingRange;
			}
			return MyCubeGridConnectionStatus.Connected;
		}

		private MyCubeGridConnectionStatus GetShipStatus(MyCubeGrid grid)
		{
			HashSet<MyDataBroadcaster> output = new HashSet<MyDataBroadcaster>();
			MyAntennaSystem.Static.GetEntityBroadcasters(grid, ref output, MySession.Static.LocalPlayerId);
			MyCubeGridConnectionStatus result = MyCubeGridConnectionStatus.OutOfReceivingRange;
			foreach (MyDataBroadcaster item in output)
			{
				MyCubeGridConnectionStatus broadcasterStatus = GetBroadcasterStatus(item);
				switch (broadcasterStatus)
				{
				case MyCubeGridConnectionStatus.Connected:
					return broadcasterStatus;
				case MyCubeGridConnectionStatus.OutOfBroadcastingRange:
					result = broadcasterStatus;
					break;
				}
			}
			return result;
		}

		private MyRefuseReason GetRemoteStatus(MyDataBroadcaster broadcaster)
		{
			if (!broadcaster.HasRemoteControl)
			{
				return MyRefuseReason.NoRemoteControl;
			}
			long? mainRemoteControlOwner = broadcaster.MainRemoteControlOwner;
			if (!mainRemoteControlOwner.HasValue)
			{
				return MyRefuseReason.NoMainRemoteControl;
			}
			MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(mainRemoteControlOwner.Value, MySession.Static.LocalHumanPlayer.Identity.IdentityId);
			if (relationPlayerPlayer == MyRelationsBetweenPlayers.Self)
			{
				return MyRefuseReason.NoProblem;
			}
			MyOwnershipShareModeEnum mainRemoteControlSharing = broadcaster.MainRemoteControlSharing;
			if (mainRemoteControlSharing == MyOwnershipShareModeEnum.All || (mainRemoteControlSharing == MyOwnershipShareModeEnum.Faction && relationPlayerPlayer == MyRelationsBetweenPlayers.Allies))
			{
				return MyRefuseReason.NoProblem;
			}
			if (mainRemoteControlOwner.Value == 0L)
			{
				return MyRefuseReason.NoOwner;
			}
			return MyRefuseReason.Forbidden;
		}

		private MyEntity GetInteractedEntityRepresentative(MyEntity controlledEntity)
		{
			if (controlledEntity is MyCubeBlock)
			{
				return MyAntennaSystem.Static.GetLogicalGroupRepresentative((controlledEntity as MyCubeBlock).CubeGrid);
			}
			return MySession.Static.LocalCharacter;
		}

		private double GetPlayerBroadcasterDistance(MyDataBroadcaster broadcaster)
		{
			if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null)
			{
				return Vector3D.Distance(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), broadcaster.BroadcastPosition);
			}
			return double.MaxValue;
		}

		private MyRefuseReason CanTakeTerminal(CubeGridInfo gridInfo)
		{
			if (!gridInfo.Owned)
			{
				return MyRefuseReason.NoOwner;
			}
			if (gridInfo.Status == MyCubeGridConnectionStatus.OutOfBroadcastingRange && MySession.Static.ControlledEntity.Entity is MyCharacter && !(MySession.Static.ControlledEntity.Entity as MyCharacter).RadioBroadcaster.Enabled)
			{
				return MyRefuseReason.PlayerBroadcastOff;
			}
			if (gridInfo.Status == MyCubeGridConnectionStatus.OutOfBroadcastingRange || gridInfo.Status == MyCubeGridConnectionStatus.OutOfReceivingRange)
			{
				return MyRefuseReason.NoStableConnection;
			}
			return MyRefuseReason.NoProblem;
		}

		private void OnButtonClicked_TakeControl(MyGuiControlButton obj)
		{
			if (m_shipsData.SelectedRow != null)
			{
				UserData userData = (UserData)m_shipsData.SelectedRow.UserData;
				if (userData.IsSelectable && userData.RemoteEntityId.HasValue)
				{
					FindRemoteControlAndTakeControl(userData.GridEntityId, userData.RemoteEntityId.Value);
				}
			}
		}

		private void Menu_ButtonClicked(MyGuiControlButton button)
		{
			if (this.ButtonClicked != null)
			{
				this.ButtonClicked();
			}
		}

		private void OnButtonClicked_OpenTerminal(MyGuiControlButton obj)
		{
			MyGuiControlTable.EventArgs args = default(MyGuiControlTable.EventArgs);
			args.MouseButton = MyMouseButtonsEnum.None;
			args.RowIndex = -1;
			shipsData_ItemDoubleClicked(null, args);
		}

		private void shipsData_ItemDoubleClicked(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			if (m_shipsData.SelectedRow != null)
			{
				UserData userData = (UserData)m_shipsData.SelectedRow.UserData;
				if (userData.IsSelectable)
				{
					OpenPropertiesByEntityId(userData.GridEntityId);
				}
			}
		}

		private void shipsData_ColumnClicked(MyGuiControlTable sender, int column)
		{
			m_columnToSort = column;
		}

		private void shipsInRange_ItemSelected()
		{
			if ((m_shipsInRange.IsMouseOver || m_shipsInRange.HasFocus) && m_shipsInRange.GetSelectedKey() != m_interactedEntityRepresentative.EntityId)
			{
				OpenPropertiesByEntityId(m_shipsInRange.GetSelectedKey());
			}
		}

		private void OpenPropertiesByEntityId(long entityId)
		{
			MyEntities.TryGetEntityById(entityId, out MyEntity entity);
			if (entity == null && !Sync.IsServer)
			{
				MyGuiScreenTerminal.RequestReplicable(entityId, entityId, OpenPropertiesByEntityId);
			}
			else if (entity is MyCharacter)
			{
				MyGuiScreenTerminal.ChangeInteractedEntity(null, isRemote: false);
			}
			else
			{
				if (entity == null || !(entity is MyCubeGrid))
				{
					return;
				}
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (m_openInventoryInteractedEntityRepresentative == myCubeGrid && MySession.Static.LocalCharacter?.Parent != null)
				{
					MyGuiScreenTerminal.ChangeInteractedEntity(MySession.Static.LocalCharacter?.Parent, isRemote: false);
				}
				else if (MyAntennaSystem.Static.CheckConnection(myCubeGrid, m_openInventoryInteractedEntityRepresentative, MySession.Static.LocalHumanPlayer))
				{
					m_tmpAntennas.Clear();
					MyAntennaSystem.Static.GetEntityReceivers(myCubeGrid, ref m_tmpAntennas, MySession.Static.LocalPlayerId);
					foreach (MyDataReceiver tmpAntenna in m_tmpAntennas)
					{
						tmpAntenna.UpdateBroadcastersInRange();
					}
					if (m_tmpAntennas.Count <= 0)
					{
						MyGuiScreenTerminal.ChangeInteractedEntity(MySession.Static.LocalCharacter?.Parent, isRemote: false);
					}
					else
					{
						MyGuiScreenTerminal.ChangeInteractedEntity(m_tmpAntennas.ElementAt(0).Entity as MyTerminalBlock, isRemote: true);
					}
				}
			}
		}

		private void FindRemoteControlAndTakeControl(long gridEntityId, long remoteEntityId)
		{
			MyEntities.TryGetEntityById(remoteEntityId, out MyRemoteControl entity);
			if (entity == null)
			{
				if (!Sync.IsServer)
				{
					MyGuiScreenTerminal.RequestReplicable(gridEntityId, remoteEntityId, delegate(long x)
					{
						FindRemoteControlAndTakeControl(gridEntityId, x);
					});
				}
			}
			else
			{
				m_tmpAntennas.Clear();
				MyAntennaSystem.Static.GetEntityReceivers(entity, ref m_tmpAntennas, MySession.Static.LocalPlayerId);
				foreach (MyDataReceiver tmpAntenna in m_tmpAntennas)
				{
					tmpAntenna.UpdateBroadcastersInRange();
				}
				entity.RequestControl();
			}
		}

		public bool TestConnection()
		{
			if (m_openInventoryInteractedEntityRepresentative.EntityId == m_interactedEntityRepresentative.EntityId && !m_isRemote)
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (m_interactedEntity != null && localCharacter != null)
				{
					return m_interactedEntity.PositionComp.WorldAABB.DistanceSquared(localCharacter.PositionComp.GetPosition()) < (double)(MyConstants.DEFAULT_INTERACTIVE_DISTANCE * MyConstants.DEFAULT_INTERACTIVE_DISTANCE);
				}
			}
			if (m_interactedEntityRepresentative is MyCubeGrid)
			{
				return GetShipStatus(m_interactedEntityRepresentative as MyCubeGrid) == MyCubeGridConnectionStatus.Connected;
			}
			return true;
		}

		public void Close()
		{
			if (m_shipsInRange != null)
			{
				m_shipsInRange.ItemSelected -= shipsInRange_ItemSelected;
				m_shipsInRange.ClearItems();
				m_shipsInRange = null;
			}
			if (m_shipsData != null)
			{
				m_shipsData.ColumnClicked -= shipsData_ColumnClicked;
				m_shipsData.Clear();
				m_shipsData = null;
			}
			if (m_button != null)
			{
				m_button.ButtonClicked -= Menu_ButtonClicked;
				m_button = null;
			}
		}

		public void Update()
		{
			cnt = ++cnt % 30;
			if (cnt == 0)
			{
				if (previousMutualConnectionGrids == null)
				{
					previousMutualConnectionGrids = MyAntennaSystem.Static.GetConnectedGridsInfo(m_openInventoryInteractedEntityRepresentative);
				}
				if (previousShipInfo == null)
				{
					previousShipInfo = GetAllCubeGridsInfo();
				}
				HashSet<MyAntennaSystem.BroadcasterInfo> connectedGridsInfo = MyAntennaSystem.Static.GetConnectedGridsInfo(m_openInventoryInteractedEntityRepresentative);
				HashSet<CubeGridInfo> allCubeGridsInfo = GetAllCubeGridsInfo();
				if (!previousMutualConnectionGrids.SetEquals(connectedGridsInfo))
				{
					PopulateMutuallyConnectedCubeGrids(connectedGridsInfo);
				}
				if (!previousShipInfo.SequenceEqual(allCubeGridsInfo))
				{
					PopulateOwnedCubeGrids(allCubeGridsInfo);
				}
				previousMutualConnectionGrids = connectedGridsInfo;
				previousShipInfo = allCubeGridsInfo;
			}
		}

		public void HandleInput()
		{
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION1))
			{
				(m_shipsData.GetInnerControlsFromCurrentCell(3) as MyGuiControlButton)?.PressButton();
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
			{
				(m_shipsData.GetInnerControlsFromCurrentCell(4) as MyGuiControlButton)?.PressButton();
			}
		}
	}
}
