using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal class MyTerminalInfoController : MyTerminalController
	{
		[Serializable]
		private struct GridBuiltByIdInfo
		{
			protected class Sandbox_Game_Gui_MyTerminalInfoController_003C_003EGridBuiltByIdInfo_003C_003EGridName_003C_003EAccessor : IMemberAccessor<GridBuiltByIdInfo, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GridBuiltByIdInfo owner, in string value)
				{
					owner.GridName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GridBuiltByIdInfo owner, out string value)
				{
					value = owner.GridName;
				}
			}

			protected class Sandbox_Game_Gui_MyTerminalInfoController_003C_003EGridBuiltByIdInfo_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<GridBuiltByIdInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GridBuiltByIdInfo owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GridBuiltByIdInfo owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Gui_MyTerminalInfoController_003C_003EGridBuiltByIdInfo_003C_003EPCUBuilt_003C_003EAccessor : IMemberAccessor<GridBuiltByIdInfo, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GridBuiltByIdInfo owner, in int value)
				{
					owner.PCUBuilt = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GridBuiltByIdInfo owner, out int value)
				{
					value = owner.PCUBuilt;
				}
			}

			protected class Sandbox_Game_Gui_MyTerminalInfoController_003C_003EGridBuiltByIdInfo_003C_003EBlockCount_003C_003EAccessor : IMemberAccessor<GridBuiltByIdInfo, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GridBuiltByIdInfo owner, in int value)
				{
					owner.BlockCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GridBuiltByIdInfo owner, out int value)
				{
					value = owner.BlockCount;
				}
			}

			protected class Sandbox_Game_Gui_MyTerminalInfoController_003C_003EGridBuiltByIdInfo_003C_003EUnsafeBlocks_003C_003EAccessor : IMemberAccessor<GridBuiltByIdInfo, List<string>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref GridBuiltByIdInfo owner, in List<string> value)
				{
					owner.UnsafeBlocks = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref GridBuiltByIdInfo owner, out List<string> value)
				{
					value = owner.UnsafeBlocks;
				}
			}

			public string GridName;

			public long EntityId;

			public int PCUBuilt;

			public int BlockCount;

			public List<string> UnsafeBlocks;
		}

		protected sealed class ServerLimitInfo_Implementation_003C_003ESystem_Int64_0023System_UInt64 : ICallSite<IMyEventOwner, long, ulong, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in ulong clientId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ServerLimitInfo_Implementation(identityId, clientId);
			}
		}

		protected sealed class ServerLimitInfo_Received_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Gui_MyTerminalInfoController_003C_003EGridBuiltByIdInfo_003E : ICallSite<IMyEventOwner, List<GridBuiltByIdInfo>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<GridBuiltByIdInfo> gridsWithBuiltById, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ServerLimitInfo_Received(gridsWithBuiltById);
			}
		}

		private static MyGuiControlTabPage m_infoPage;

		private static MyCubeGrid m_grid;

		private static List<MyBlockLimits.MyGridLimitData> m_infoGrids = new List<MyBlockLimits.MyGridLimitData>();

		private static List<MyPlayer.PlayerId> m_playerIds = new List<MyPlayer.PlayerId>();

		private static bool m_controlsDirty;

		internal void Close()
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
			if (myIdentity != null)
			{
				myIdentity.BlockLimits.BlockLimitsChanged -= grid_OnAuthorshipChanged;
			}
			MyEntities.OnEntityDelete -= grid_OnClose;
			if (m_grid != null && m_infoPage != null)
			{
				MySessionComponentSafeZones.OnSafeZoneUpdated -= OnSafeZoneUpdated;
				MyGuiControlButton myGuiControlButton = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertBtn");
				if (myGuiControlButton != null)
				{
					myGuiControlButton.ButtonClicked -= convertBtn_ButtonClicked;
				}
				MyGuiControlButton myGuiControlButton2 = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
				if (myGuiControlButton2 != null)
				{
					myGuiControlButton2.ButtonClicked -= convertToStationBtn_ButtonClicked;
				}
				m_grid.OnBlockAdded -= grid_OnBlockAdded;
				m_grid.OnBlockRemoved -= grid_OnBlockRemoved;
				m_grid.OnPhysicsChanged -= grid_OnPhysicsChanged;
				m_grid.OnBlockOwnershipChanged -= grid_OnBlockOwnershipChanged;
				m_grid.PositionComp.OnPositionChanged -= OnGridPositionChanged;
				m_grid = null;
				m_infoPage = null;
			}
		}

		internal void Init(MyGuiControlTabPage infoPage, MyCubeGrid grid)
		{
			m_grid = grid;
			m_infoPage = infoPage;
			m_playerIds.Clear();
			m_controlsDirty = false;
			MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId).BlockLimits.BlockLimitsChanged += grid_OnAuthorshipChanged;
			RecreateControls();
			MyEntities.OnEntityDelete += grid_OnClose;
			if (grid == null)
			{
				return;
			}
			grid.OnBlockAdded += grid_OnBlockAdded;
			grid.OnBlockRemoved += grid_OnBlockRemoved;
			grid.OnPhysicsChanged += grid_OnPhysicsChanged;
			grid.OnBlockOwnershipChanged += grid_OnBlockOwnershipChanged;
			grid.PositionComp.OnPositionChanged += OnGridPositionChanged;
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				MyGuiControlButton myGuiControlButton = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("RenameShipButton");
				if (myGuiControlButton != null)
				{
					myGuiControlButton.ButtonClicked += renameBtn_ButtonClicked;
				}
			}
			MyGuiControlButton myGuiControlButton2 = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertBtn");
			if (myGuiControlButton2 != null)
			{
				myGuiControlButton2.ButtonClicked += convertBtn_ButtonClicked;
			}
			MyGuiControlButton myGuiControlButton3 = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
			if (myGuiControlButton3 != null)
			{
				myGuiControlButton3.ButtonClicked += convertToStationBtn_ButtonClicked;
				myGuiControlButton3.Enabled = MySessionComponentSafeZones.IsActionAllowed(m_grid, MySafeZoneAction.ConvertToStation, 0L, 0uL);
			}
			MySessionComponentSafeZones.OnSafeZoneUpdated += OnSafeZoneUpdated;
		}

		private static void RecreateControls()
		{
			if (m_infoPage != null)
			{
				m_controlsDirty = true;
			}
		}

		public void UpdateBeforeDraw()
		{
			if (!m_controlsDirty)
			{
				return;
			}
			m_controlsDirty = false;
			if (MyFakes.ENABLE_CENTER_OF_MASS)
			{
				MyGuiControlCheckbox obj = (MyGuiControlCheckbox)m_infoPage.Controls.GetControlByName("CenterBtn");
				obj.IsChecked = MyCubeGrid.ShowCenterOfMass;
				obj.IsCheckedChanged = centerBtn_IsCheckedChanged;
				MyGuiControlCheckbox obj2 = (MyGuiControlCheckbox)m_infoPage.Controls.GetControlByName("PivotBtn");
				obj2.IsChecked = MyCubeGrid.ShowGridPivot;
				obj2.IsCheckedChanged = pivotBtn_IsCheckedChanged;
			}
			MyGuiControlCheckbox obj3 = (MyGuiControlCheckbox)m_infoPage.Controls.GetControlByName("ShowGravityGizmo");
			obj3.IsChecked = MyCubeGrid.ShowGravityGizmos;
			obj3.IsCheckedChanged = showGravityGizmos_IsCheckedChanged;
			MyGuiControlCheckbox obj4 = (MyGuiControlCheckbox)m_infoPage.Controls.GetControlByName("ShowSenzorGizmo");
			obj4.IsChecked = MyCubeGrid.ShowSenzorGizmos;
			obj4.IsCheckedChanged = showSenzorGizmos_IsCheckedChanged;
			MyGuiControlCheckbox obj5 = (MyGuiControlCheckbox)m_infoPage.Controls.GetControlByName("ShowAntenaGizmo");
			obj5.IsChecked = MyCubeGrid.ShowAntennaGizmos;
			obj5.IsCheckedChanged = showAntenaGizmos_IsCheckedChanged;
			MyGuiControlSlider obj6 = (MyGuiControlSlider)m_infoPage.Controls.GetControlByName("FriendAntennaRange");
			obj6.Value = MyHudMarkerRender.FriendAntennaRange;
			obj6.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(obj6.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				MyHudMarkerRender.FriendAntennaRange = s.Value;
			});
			obj6.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_FriendlyAntennaRange_ToolTip));
			MyGuiControlSlider obj7 = (MyGuiControlSlider)m_infoPage.Controls.GetControlByName("EnemyAntennaRange");
			obj7.Value = MyHudMarkerRender.EnemyAntennaRange;
			obj7.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(obj7.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				MyHudMarkerRender.EnemyAntennaRange = s.Value;
			});
			obj7.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_EnemyAntennaRange_ToolTip));
			MyGuiControlSlider obj8 = (MyGuiControlSlider)m_infoPage.Controls.GetControlByName("OwnedAntennaRange");
			obj8.Value = MyHudMarkerRender.OwnerAntennaRange;
			obj8.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(obj8.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				MyHudMarkerRender.OwnerAntennaRange = s.Value;
			});
			obj8.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_OwnedAntennaRange_ToolTip));
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				bool enabled = IsPlayerOwner(m_grid);
				MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel)m_infoPage.Controls.GetControlByName("RenameShipLabel");
				MyGuiControlButton obj9 = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("RenameShipButton");
				MyGuiControlTextbox myGuiControlTextbox = (MyGuiControlTextbox)m_infoPage.Controls.GetControlByName("RenameShipText");
				if (myGuiControlTextbox != null)
				{
					if (m_grid != null)
					{
						myGuiControlTextbox.Text = m_grid.DisplayName;
					}
					myGuiControlTextbox.Enabled = enabled;
				}
				myGuiControlLabel.Enabled = enabled;
				obj9.Enabled = enabled;
			}
			MyGuiControlButton myGuiControlButton = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertBtn");
			MyGuiControlButton myGuiControlButton2 = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
			MyGuiControlList myGuiControlList = (MyGuiControlList)m_infoPage.Controls.GetControlByName("InfoList");
			myGuiControlList.Controls.Clear();
			MyGuiControlCheckbox myGuiControlCheckbox = (MyGuiControlCheckbox)m_infoPage.Controls.GetControlByName("SetDestructibleBlocks");
			myGuiControlCheckbox.Visible = (MySession.Static.Settings.ScenarioEditMode || MySession.Static.IsScenario);
			myGuiControlCheckbox.Enabled = MySession.Static.Settings.ScenarioEditMode;
			if (m_grid == null || m_grid.Physics == null)
			{
				myGuiControlButton.Enabled = false;
				myGuiControlButton2.Enabled = false;
				((MyGuiControlLabel)m_infoPage.Controls.GetControlByName("Infolabel")).Text = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_Overview);
				RequestServerLimitInfo(MySession.Static.LocalPlayerId);
				return;
			}
			if (!m_grid.IsStatic)
			{
				myGuiControlButton.Enabled = false;
				myGuiControlButton2.Enabled = MySessionComponentSafeZones.IsActionAllowed(m_grid, MySafeZoneAction.ConvertToStation, 0L, Sync.MyId);
			}
			else
			{
				myGuiControlButton.Enabled = true;
				myGuiControlButton2.Enabled = false;
			}
			if (m_grid.GridSizeEnum == MyCubeSize.Small)
			{
				myGuiControlButton2.Enabled = false;
			}
			if (!m_grid.BigOwners.Contains(MySession.Static.LocalPlayerId) && !MySession.Static.IsUserSpaceMaster(Sync.MyId))
			{
				myGuiControlButton.Enabled = false;
				myGuiControlButton2.Enabled = false;
			}
			myGuiControlCheckbox.IsChecked = m_grid.DestructibleBlocks;
			myGuiControlCheckbox.IsCheckedChanged = setDestructibleBlocks_IsCheckedChanged;
			int number = 0;
			if (m_grid.BlocksCounters.ContainsKey(typeof(MyObjectBuilder_GravityGenerator)))
			{
				number = m_grid.BlocksCounters[typeof(MyObjectBuilder_GravityGenerator)];
			}
			int number2 = 0;
			if (m_grid.BlocksCounters.ContainsKey(typeof(MyObjectBuilder_VirtualMass)))
			{
				number2 = m_grid.BlocksCounters[typeof(MyObjectBuilder_VirtualMass)];
			}
			int number3 = 0;
			if (m_grid.BlocksCounters.ContainsKey(typeof(MyObjectBuilder_InteriorLight)))
			{
				number3 = m_grid.BlocksCounters[typeof(MyObjectBuilder_InteriorLight)];
			}
			int num = 0;
			foreach (MyObjectBuilderType key in m_grid.BlocksCounters.Keys)
			{
				Type producedType = MyCubeBlockFactory.GetProducedType(key);
				if (typeof(IMyConveyorSegmentBlock).IsAssignableFrom(producedType) || typeof(IMyConveyorEndpointBlock).IsAssignableFrom(producedType))
				{
					num += m_grid.BlocksCounters[key];
				}
			}
			int num2 = 0;
			foreach (MySlimBlock block in m_grid.GetBlocks())
			{
				if (block.FatBlock != null)
				{
					num2 += block.FatBlock.Model.GetTrianglesCount();
				}
			}
			foreach (MyCubeGridRenderCell value in m_grid.RenderData.Cells.Values)
			{
				foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in value.CubeParts)
				{
					num2 += cubePart.Key.Model.GetTrianglesCount();
				}
			}
			int number4 = 0;
			MyEntityThrustComponent myEntityThrustComponent = m_grid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null)
			{
				number4 = myEntityThrustComponent.ThrustCount;
			}
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Thrusters)).AppendInt32(number4).ToString());
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Triangles)).AppendInt32(num2).ToString());
			myGuiControlLabel3.SetToolTip(MySpaceTexts.TerminalTab_Info_TrianglesTooltip);
			MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Blocks)).AppendInt32(m_grid.GetBlocks().Count).ToString());
			myGuiControlLabel4.SetToolTip(MySpaceTexts.TerminalTab_Info_BlocksTooltip);
			MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(new StringBuilder("PCU: ")).AppendInt32(m_grid.BlocksPCU).ToString());
			myGuiControlLabel4.SetToolTip(MySpaceTexts.TerminalTab_Info_BlocksTooltip);
			MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_NonArmor)).AppendInt32(m_grid.Hierarchy.Children.Count).ToString());
			MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(null, null, new StringBuilder().Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Lights)).AppendInt32(number3)
				.ToString());
			MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Reflectors)).AppendInt32(m_grid.GridSystems.ReflectorLightSystem.ReflectorCount).ToString());
			MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_GravGens)).AppendInt32(number).ToString());
			MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_VirtualMass)).AppendInt32(number2).ToString());
			MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Conveyors)).AppendInt32(num).ToString());
			MyGuiControlLabel myGuiControlLabel12 = new MyGuiControlLabel(null, null, new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_GridMass)).AppendInt32(m_grid.GetCurrentMass()).ToString());
			MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel(null, null, string.Format(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Shapes).ToString(), m_grid.ShapeCount, 65536));
			myGuiControlList.InitControls(new MyGuiControlBase[12]
			{
				myGuiControlLabel4,
				myGuiControlLabel6,
				myGuiControlLabel5,
				myGuiControlLabel11,
				myGuiControlLabel2,
				myGuiControlLabel7,
				myGuiControlLabel8,
				myGuiControlLabel9,
				myGuiControlLabel10,
				myGuiControlLabel3,
				myGuiControlLabel12,
				myGuiControlLabel13
			});
		}

		private void setDestructibleBlocks_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			m_grid.DestructibleBlocks = obj.IsChecked;
		}

		public void MarkControlsDirty()
		{
			m_controlsDirty = true;
		}

		public static void RequestServerLimitInfo(long identityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ServerLimitInfo_Implementation, identityId, MySession.Static.LocalHumanPlayer.Id.SteamId);
		}

		[Event(null, 326)]
		[Reliable]
		[Server]
		public static void ServerLimitInfo_Implementation(long identityId, ulong clientId)
		{
			if (MySession.Static != null)
			{
				List<GridBuiltByIdInfo> list = new List<GridBuiltByIdInfo>();
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (myIdentity != null)
				{
					foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> item2 in myIdentity.BlockLimits.BlocksBuiltByGrid)
					{
						HashSet<MySlimBlock> hashSet = null;
						if (MyEntities.TryGetEntityById(item2.Key, out MyCubeGrid entity))
						{
							hashSet = entity.FindBlocksBuiltByID(myIdentity.IdentityId);
							if (!hashSet.Any())
							{
								continue;
							}
						}
						GridBuiltByIdInfo gridBuiltByIdInfo = default(GridBuiltByIdInfo);
						gridBuiltByIdInfo.GridName = item2.Value.GridName;
						gridBuiltByIdInfo.EntityId = item2.Key;
						gridBuiltByIdInfo.UnsafeBlocks = new List<string>();
						GridBuiltByIdInfo item = gridBuiltByIdInfo;
						if (hashSet != null)
						{
							item.BlockCount = hashSet.Count;
							item.PCUBuilt = hashSet.Sum((MySlimBlock x) => x.BlockDefinition.PCU);
						}
						if (MyUnsafeGridsSessionComponent.UnsafeGrids.TryGetValue(item2.Key, out entity))
						{
							foreach (MyCubeBlock unsafeBlock in entity.UnsafeBlocks)
							{
								item.UnsafeBlocks.Add(unsafeBlock.DisplayNameText);
							}
						}
						list.Add(item);
					}
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ServerLimitInfo_Received, list, new EndpointId(clientId));
			}
		}

		[Event(null, 377)]
		[Reliable]
		[Client]
		private static void ServerLimitInfo_Received(List<GridBuiltByIdInfo> gridsWithBuiltById)
		{
			if (m_infoPage == null)
			{
				return;
			}
			MyGuiControlList myGuiControlList = (MyGuiControlList)m_infoPage.Controls.GetControlByName("InfoList");
			if (myGuiControlList == null)
			{
				return;
			}
			myGuiControlList.Controls.Clear();
			MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
			if (identity != null)
			{
				if (MySession.Static.MaxBlocksPerPlayer > 0)
				{
					MyGuiControlLabel control = new MyGuiControlLabel(null, null, $"{MyTexts.Get(MySpaceTexts.TerminalTab_Info_YouBuilt)} {identity.BlockLimits.BlocksBuilt}/{identity.BlockLimits.MaxBlocks} {MyTexts.Get(MySpaceTexts.TerminalTab_Info_BlocksLower)}");
					myGuiControlList.Controls.Add(control);
				}
				foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
				{
					identity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out MyBlockLimits.MyTypeLimitData value);
					MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = MyDefinitionManager.Static.TryGetDefinitionGroup(blockTypeLimit.Key);
					if (myCubeBlockDefinitionGroup != null && value != null)
					{
						MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, $"{MyTexts.Get(MySpaceTexts.TerminalTab_Info_YouBuilt)} {value.BlocksBuilt}/{MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key)} {myCubeBlockDefinitionGroup.Any.DisplayNameText}");
						myGuiControlList.Controls.Add(control2);
					}
				}
				m_infoGrids.Clear();
				if (gridsWithBuiltById != null)
				{
					foreach (GridBuiltByIdInfo item in gridsWithBuiltById)
					{
						MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
						bool flag = item.UnsafeBlocks.Count > 0;
						myGuiControlParent.Size = new Vector2(myGuiControlParent.Size.X, 0.1f);
						if (m_infoGrids.Count == 0)
						{
							MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
							myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.15f, -0.052f), 0.3f, 0.002f);
							myGuiControlParent.Controls.Add(myGuiControlSeparatorList);
						}
						string text = item.GridName;
						if (text != null && text.Length >= 16)
						{
							text = text.Substring(0, 15);
							text = text.Insert(text.Length, "...");
						}
						MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, text, null, 0.7005405f);
						MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, $"{item.BlockCount} {MyTexts.Get(MySpaceTexts.TerminalTab_Info_BlocksLower)} ({item.PCUBuilt} PCU)", null, 0.7005405f);
						MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_Assign), null, 0.7005405f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
						MyGuiControlCombobox assignCombobox = new MyGuiControlCombobox(null, new Vector2(0.11f, 0.008f), null, null, 10, null, useScrollBarOffset: false, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
						MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
						myGuiControlLabel.Position = new Vector2(-0.12f, -0.025f);
						myGuiControlLabel2.Position = new Vector2(-0.12f, 0f);
						myGuiControlLabel3.Position = new Vector2(0f, 0.035f);
						assignCombobox.Position = new Vector2(0.121f, 0.055f);
						GridBuiltByIdInfo gridSelected = item;
						assignCombobox.ItemSelected += delegate
						{
							assignCombobox_ItemSelected(identity, gridSelected.EntityId, m_playerIds[(int)assignCombobox.GetSelectedKey()]);
						};
						m_playerIds.Clear();
						foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
						{
							if (MySession.Static.LocalHumanPlayer != onlinePlayer)
							{
								assignCombobox.AddItem(m_playerIds.Count, onlinePlayer.DisplayName);
								m_playerIds.Add(onlinePlayer.Id);
							}
						}
						if (MySession.Static.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
						{
							assignCombobox.Enabled = false;
							assignCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.Terminal_AuthorshipNotAvailable));
							assignCombobox.ShowTooltipWhenDisabled = true;
						}
						else if (assignCombobox.GetItemsCount() == 0)
						{
							assignCombobox.Enabled = false;
						}
						myGuiControlSeparatorList2.AddHorizontal(new Vector2(-0.15f, 0.063f), 0.3f, flag ? 0.002f : 0.003f);
						myGuiControlParent.Controls.Add(myGuiControlLabel);
						myGuiControlParent.Controls.Add(myGuiControlLabel2);
						myGuiControlParent.Controls.Add(myGuiControlLabel3);
						myGuiControlParent.Controls.Add(assignCombobox);
						myGuiControlParent.Controls.Add(myGuiControlSeparatorList2);
						if (MySession.Static.EnableRemoteBlockRemoval)
						{
							MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.buttonRemove), null, 0.7005405f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
							MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.SquareSmall, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: new StringBuilder("X"), onButtonClick: deleteBuiltByIdBlocksButton_ButtonClicked, buttonIndex: m_infoGrids.Count, toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_RemoveGrid));
							myGuiControlLabel4.Position = new Vector2(0.082f, -0.02f);
							myGuiControlButton.Position = new Vector2(0.1215f, -0.02f);
							myGuiControlParent.Controls.Add(myGuiControlLabel4);
							myGuiControlParent.Controls.Add(myGuiControlButton);
						}
						if (identity.BlockLimits.BlocksBuiltByGrid.ContainsKey(item.EntityId))
						{
							m_infoGrids.Add(identity.BlockLimits.BlocksBuiltByGrid[item.EntityId]);
						}
						else if (MySession.Static.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
						{
							m_infoGrids.Add(new MyBlockLimits.MyGridLimitData
							{
								EntityId = item.EntityId,
								GridName = item.GridName
							});
						}
						myGuiControlList.Controls.Add(myGuiControlParent);
						if (flag)
						{
							MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText
							{
								OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
								TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
								TextScale = 0.7f,
								TextColor = Color.Red
							};
							myGuiControlLabel.ColorMask = Color.Red;
							myGuiControlLabel2.ColorMask = Color.Red;
							StringBuilder text3 = myGuiControlMultilineText.Text;
							text3.AppendLine(MyTexts.GetString(MyCommonTexts.ScreenTerminalInfo_UnsafeBlocks));
							foreach (string unsafeBlock in item.UnsafeBlocks)
							{
								text3.AppendLine(unsafeBlock);
							}
							myGuiControlMultilineText.RefreshText(useEnum: false);
							myGuiControlMultilineText.Size = new Vector2(1f, myGuiControlMultilineText.TextSize.Y);
							MyGuiControlParent myGuiControlParent2 = new MyGuiControlParent
							{
								Size = new Vector2(1f, myGuiControlMultilineText.TextSize.Y - 0.01f)
							};
							myGuiControlParent2.Controls.Add(myGuiControlMultilineText);
							myGuiControlList.Controls.Add(myGuiControlParent2);
							myGuiControlMultilineText.PositionX -= 0.12f;
							myGuiControlMultilineText.PositionY -= myGuiControlParent2.Size.Y / 2f - 0.012f;
							MyGuiControlParent myGuiControlParent3 = new MyGuiControlParent
							{
								Size = new Vector2(1f, 0.02f)
							};
							MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
							myGuiControlSeparatorList3.AddHorizontal(new Vector2(-0.15f, 0f), 0.3f, 0.003f);
							myGuiControlParent3.Controls.Add(myGuiControlSeparatorList3);
							myGuiControlList.Controls.Add(myGuiControlParent3);
						}
					}
				}
			}
		}

		private bool IsPlayerOwner(MyCubeGrid grid)
		{
			return grid?.BigOwners.Contains(MySession.Static.LocalPlayerId) ?? false;
		}

		private void showAntenaGizmos_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			MyCubeGrid.ShowAntennaGizmos = obj.IsChecked;
			(from x in MyEntities.GetEntities()
				where x is MyCubeGrid
				select x).Cast<MyCubeGrid>().ForEach(delegate(MyCubeGrid x)
			{
				x.MarkForDraw();
			});
		}

		private void showSenzorGizmos_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			MyCubeGrid.ShowSenzorGizmos = obj.IsChecked;
			(from x in MyEntities.GetEntities()
				where x is MyCubeGrid
				select x).Cast<MyCubeGrid>().ForEach(delegate(MyCubeGrid x)
			{
				x.MarkForDraw();
			});
		}

		private void showGravityGizmos_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			MyCubeGrid.ShowGravityGizmos = obj.IsChecked;
			(from x in MyEntities.GetEntities()
				where x is MyCubeGrid
				select x).Cast<MyCubeGrid>().ForEach(delegate(MyCubeGrid x)
			{
				x.MarkForDraw();
			});
		}

		private void centerBtn_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			MyCubeGrid.ShowCenterOfMass = obj.IsChecked;
			(from x in MyEntities.GetEntities()
				where x is MyCubeGrid
				select x).Cast<MyCubeGrid>().ForEach(delegate(MyCubeGrid x)
			{
				x.MarkForDraw();
			});
		}

		private void pivotBtn_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			MyCubeGrid.ShowGridPivot = obj.IsChecked;
			(from x in MyEntities.GetEntities()
				where x is MyCubeGrid
				select x).Cast<MyCubeGrid>().ForEach(delegate(MyCubeGrid x)
			{
				x.MarkForDraw();
			});
		}

		private void setDestructibleBlocksBtn_IsCheckedChanged(MyGuiControlCheckbox obj)
		{
			m_grid.DestructibleBlocks = obj.IsChecked;
		}

		private void convertBtn_Fail()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextConvertToShipFail), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void convertBtn_ButtonClicked(MyGuiControlButton obj)
		{
			m_grid.RequestConversionToShip(convertBtn_Fail);
		}

		private void convertToStationBtn_ButtonClicked(MyGuiControlButton obj)
		{
			m_grid.RequestConversionToStation();
		}

		private void OnSafeZoneUpdated(MySafeZone obj)
		{
			MyGuiControlButton myGuiControlButton = (MyGuiControlButton)m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
			if (myGuiControlButton != null)
			{
				myGuiControlButton.Enabled = MySessionComponentSafeZones.IsActionAllowed(m_grid, MySafeZoneAction.ConvertToStation, 0L, Sync.MyId);
			}
		}

		private void OnGridPositionChanged(MyPositionComponentBase obj)
		{
			OnSafeZoneUpdated(null);
		}

		private void renameBtn_Update(MyGuiControlTextbox obj)
		{
			if (obj.Enabled)
			{
				MyGuiControlTextbox myGuiControlTextbox = (MyGuiControlTextbox)m_infoPage.Controls.GetControlByName("RenameShipText");
				m_grid.ChangeDisplayNameRequest(myGuiControlTextbox.Text);
			}
		}

		private static void deleteBuiltByIdBlocksButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (obj.Index < m_infoGrids.Count)
			{
				MyBlockLimits.MyGridLimitData gridInfo = m_infoGrids[obj.Index];
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextConfirmDeleteGrid, gridInfo.GridName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyBlockLimits.RemoveBlocksBuiltByID, gridInfo.EntityId, MySession.Static.LocalPlayerId);
					}
				}, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
			}
		}

		private static void assignCombobox_ItemSelected(MyIdentity locallIdentity, long entityId, MyPlayer.PlayerId playerId)
		{
			if (locallIdentity.BlockLimits.BlocksBuiltByGrid.TryGetValue(entityId, out MyBlockLimits.MyGridLimitData gridLimitData))
			{
				ulong steamId = playerId.SteamId;
				MyIdentity identity = MySession.Static.Players.TryGetPlayerIdentity(playerId);
				if (identity != null)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmTransferGrid), new object[2]
					{
						gridLimitData.GridName,
						identity.DisplayName
					}), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							if (MySession.Static.Players.GetOnlinePlayers().Contains(MySession.Static.Players.GetPlayerById(playerId)))
							{
								MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyBlockLimits.SendTransferRequestMessage, gridLimitData, MySession.Static.LocalPlayerId, identity.IdentityId, steamId);
							}
							else
							{
								ShowPlayerNotOnlineMessage(identity);
							}
						}
					}, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
				}
			}
		}

		private static void ShowPlayerNotOnlineMessage(MyIdentity identity)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextPlayerNotOnline, identity.DisplayName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), null, null, null, null, delegate
			{
				RecreateControls();
			}, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void renameBtn_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTextbox myGuiControlTextbox = (MyGuiControlTextbox)m_infoPage.Controls.GetControlByName("RenameShipText");
			m_grid.ChangeDisplayNameRequest(myGuiControlTextbox.Text);
		}

		private void grid_OnClose(MyEntity obj)
		{
			if (obj is MyCubeGrid)
			{
				foreach (MyBlockLimits.MyGridLimitData infoGrid in m_infoGrids)
				{
					if (infoGrid.EntityId == obj.EntityId)
					{
						RecreateControls();
						break;
					}
				}
			}
		}

		private void grid_OnBlockRemoved(MySlimBlock obj)
		{
			RecreateControls();
		}

		private void grid_OnBlockAdded(MySlimBlock obj)
		{
			RecreateControls();
		}

		private void grid_OnPhysicsChanged(MyEntity obj)
		{
			RecreateControls();
		}

		private void grid_OnBlockOwnershipChanged(MyEntity obj)
		{
			RecreateControls();
		}

		private void grid_OnAuthorshipChanged()
		{
			RecreateControls();
		}
	}
}
