using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.AdminMenu;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.SessionComponents;
using VRage.Input;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[PreloadRequired]
	[StaticEventOwner]
	public class MyGuiScreenAdminMenu : MyGuiScreenDebugBase
	{
		private enum TrashTab
		{
			General,
			Voxels
		}

		public enum MyPageEnum
		{
			AdminTools,
			TrashRemoval,
			CycleObjects,
			EntityList,
			SafeZones,
			GlobalSafeZone,
			ReplayTool,
			Economy
		}

		private struct MyIdNamePair
		{
			public long Id;

			public string Name;
		}

		private class MyIdNamePairComparer : IComparer<MyIdNamePair>
		{
			public int Compare(MyIdNamePair x, MyIdNamePair y)
			{
				return string.Compare(x.Name, y.Name);
			}
		}

		[Serializable]
		internal struct AdminSettings
		{
			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003Eflags_003C_003EAccessor : IMemberAccessor<AdminSettings, MyTrashRemovalFlags>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in MyTrashRemovalFlags value)
				{
					owner.flags = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out MyTrashRemovalFlags value)
				{
					value = owner.flags;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003Eenable_003C_003EAccessor : IMemberAccessor<AdminSettings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in bool value)
				{
					owner.enable = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out bool value)
				{
					value = owner.enable;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EblockCount_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.blockCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.blockCount;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EplayerDistance_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.playerDistance = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.playerDistance;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EgridCount_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.gridCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.gridCount;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EplayerInactivity_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.playerInactivity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.playerInactivity;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EcharacterRemovalThreshold_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.characterRemovalThreshold = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.characterRemovalThreshold;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EafkTimeout_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.afkTimeout = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.afkTimeout;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EstopGridsPeriod_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.stopGridsPeriod = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.stopGridsPeriod;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EremoveOldIdentities_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.removeOldIdentities = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.removeOldIdentities;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EvoxelEnable_003C_003EAccessor : IMemberAccessor<AdminSettings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in bool value)
				{
					owner.voxelEnable = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out bool value)
				{
					value = owner.voxelEnable;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EvoxelDistanceFromPlayer_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.voxelDistanceFromPlayer = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.voxelDistanceFromPlayer;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EvoxelDistanceFromGrid_003C_003EAccessor : IMemberAccessor<AdminSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in float value)
				{
					owner.voxelDistanceFromGrid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out float value)
				{
					value = owner.voxelDistanceFromGrid;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EvoxelAge_003C_003EAccessor : IMemberAccessor<AdminSettings, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in int value)
				{
					owner.voxelAge = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out int value)
				{
					value = owner.voxelAge;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings_003C_003EAdminSettingsFlags_003C_003EAccessor : IMemberAccessor<AdminSettings, AdminSettingsEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AdminSettings owner, in AdminSettingsEnum value)
				{
					owner.AdminSettingsFlags = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AdminSettings owner, out AdminSettingsEnum value)
				{
					value = owner.AdminSettingsFlags;
				}
			}

			public MyTrashRemovalFlags flags;

			public bool enable;

			public int blockCount;

			public float playerDistance;

			public int gridCount;

			public float playerInactivity;

			public int characterRemovalThreshold;

			public int afkTimeout;

			public int stopGridsPeriod;

			public int removeOldIdentities;

			public bool voxelEnable;

			public float voxelDistanceFromPlayer;

			public float voxelDistanceFromGrid;

			public int voxelAge;

			public AdminSettingsEnum AdminSettingsFlags;
		}

		public enum MyZoneAxisTypeEnum
		{
			X,
			Y,
			Z
		}

		public enum MyRestrictedTypeEnum
		{
			Player,
			Faction,
			Grid,
			FloatingObjects
		}

		private class MySafezoneNameComparer : IComparer<MySafeZone>
		{
			public int Compare(MySafeZone x, MySafeZone y)
			{
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 0;
				}
				return string.Compare(x.DisplayName, y.DisplayName);
			}
		}

		protected sealed class RequestReputation_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerIdentityId, in long factionId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestReputation(playerIdentityId, factionId);
			}
		}

		protected sealed class RequestReputationCallback_003C_003ESystem_Int64_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerIdentityId, in long factionId, in int reputation, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestReputationCallback(playerIdentityId, factionId, reputation);
			}
		}

		protected sealed class RequestChangeReputation_003C_003ESystem_Int64_0023System_Int64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, long, long, int, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in long factionId, in int reputationChange, in bool shouldPropagate, in DBNull arg5, in DBNull arg6)
			{
				RequestChangeReputation(identityId, factionId, reputationChange, shouldPropagate);
			}
		}

		protected sealed class RequestBalance_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long accountOwner, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestBalance(accountOwner);
			}
		}

		protected sealed class RequestBalanceCallback_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long accountOwner, in long balance, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestBalanceCallback(accountOwner, balance);
			}
		}

		protected sealed class RequestChange_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long accountOwner, in long balanceChange, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestChange(accountOwner, balanceChange);
			}
		}

		protected sealed class RequestSettingFromServer_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestSettingFromServer_Implementation();
			}
		}

		protected sealed class AskIsValidForEdit_Server_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AskIsValidForEdit_Server(entityId);
			}
		}

		protected sealed class AskIsValidForEdit_Reponse_003C_003ESystem_Int64_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in bool canEdit, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AskIsValidForEdit_Reponse(entityId, canEdit);
			}
		}

		protected sealed class EntityListRequest_003C_003ESandbox_Game_Entities_MyEntityList_003C_003EMyEntityTypeEnum : ICallSite<IMyEventOwner, MyEntityList.MyEntityTypeEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyEntityList.MyEntityTypeEnum selectedType, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EntityListRequest(selectedType);
			}
		}

		protected sealed class CycleRequest_Implementation_003C_003ESandbox_Game_Entities_MyEntityCyclingOrder_0023System_Boolean_0023System_Boolean_0023System_Single_0023System_Int64_0023Sandbox_Game_Entities_CyclingOptions : ICallSite<IMyEventOwner, MyEntityCyclingOrder, bool, bool, float, long, CyclingOptions>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyEntityCyclingOrder order, in bool reset, in bool findLarger, in float metricValue, in long currentEntityId, in CyclingOptions options)
			{
				CycleRequest_Implementation(order, reset, findLarger, metricValue, currentEntityId, options);
			}
		}

		protected sealed class RemoveOwner_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E_0023System_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<IMyEventOwner, List<long>, List<long>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<long> owners, in List<long> entityIds, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveOwner_Implementation(owners, entityIds);
			}
		}

		protected sealed class ProceedEntitiesAction_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E_0023Sandbox_Game_Entities_MyEntityList_003C_003EEntityListAction : ICallSite<IMyEventOwner, List<long>, MyEntityList.EntityListAction, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<long> entityIds, in MyEntityList.EntityListAction action, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProceedEntitiesAction_Implementation(entityIds, action);
			}
		}

		protected sealed class UploadSettingsToServer_003C_003ESandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings : ICallSite<IMyEventOwner, AdminSettings, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettings settings, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UploadSettingsToServer(settings);
			}
		}

		protected sealed class ProceedEntity_Implementation_003C_003ESystem_Int64_0023Sandbox_Game_Entities_MyEntityList_003C_003EEntityListAction : ICallSite<IMyEventOwner, long, MyEntityList.EntityListAction, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in MyEntityList.EntityListAction action, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProceedEntity_Implementation(entityId, action);
			}
		}

		protected sealed class ReplicateEverything_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReplicateEverything_Implementation();
			}
		}

		protected sealed class AdminSettingsChanged_003C_003ESandbox_Game_World_AdminSettingsEnum_0023System_UInt64 : ICallSite<IMyEventOwner, AdminSettingsEnum, ulong, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettingsEnum settings, in ulong steamId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AdminSettingsChanged(settings, steamId);
			}
		}

		protected sealed class AdminSettingsChangedClient_003C_003ESandbox_Game_World_AdminSettingsEnum_0023System_UInt64 : ICallSite<IMyEventOwner, AdminSettingsEnum, ulong, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettingsEnum settings, in ulong steamId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AdminSettingsChangedClient(settings, steamId);
			}
		}

		protected sealed class EntityListResponse_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003E : ICallSite<IMyEventOwner, List<MyEntityList.MyEntityListInfoItem>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyEntityList.MyEntityListInfoItem> entities, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EntityListResponse(entities);
			}
		}

		protected sealed class Cycle_Implementation_003C_003ESystem_Single_0023System_Int64_0023VRageMath_Vector3D_0023System_Boolean : ICallSite<IMyEventOwner, float, long, Vector3D, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float newMetricValue, in long newEntityId, in Vector3D position, in bool isNpcStation, in DBNull arg5, in DBNull arg6)
			{
				Cycle_Implementation(newMetricValue, newEntityId, position, isNpcStation);
			}
		}

		protected sealed class DownloadSettingFromServer_003C_003ESandbox_Game_Gui_MyGuiScreenAdminMenu_003C_003EAdminSettings : ICallSite<IMyEventOwner, AdminSettings, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AdminSettings settings, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DownloadSettingFromServer(settings);
			}
		}

		internal static readonly float TEXT_ALIGN_CONST = 0.05f;

		private static readonly Vector2 CB_OFFSET = new Vector2(-0.05f, 0f);

		private static MyGuiScreenAdminMenu m_static;

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private readonly Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		private readonly float m_textScale = 0.8f;

		protected static MyEntityCyclingOrder m_order;

		private static float m_metricValue = 0f;

		private static long m_entityId;

		private static bool m_showMedbayNotification = true;

		private long m_attachCamera;

		private bool m_attachIsNpcStation;

		private MyGuiControlLabel m_errorLabel;

		private MyGuiControlLabel m_labelCurrentIndex;

		private MyGuiControlLabel m_labelEntityName;

		private MyGuiControlLabel m_labelNumVisible;

		protected MyGuiControlButton m_removeItemButton;

		private MyGuiControlButton m_depowerItemButton;

		protected MyGuiControlButton m_stopItemButton;

		protected MyGuiControlCheckbox m_onlySmallGridsCheckbox;

		private MyGuiControlCheckbox m_onlyLargeGridsCheckbox;

		private static CyclingOptions m_cyclingOptions = default(CyclingOptions);

		protected Vector4 m_labelColor = Color.White.ToVector4();

		protected MyGuiControlCheckbox m_creativeCheckbox;

		private readonly List<IMyGps> m_gpsList = new List<IMyGps>();

		protected MyGuiControlCombobox m_modeCombo;

		protected MyGuiControlCombobox m_onlinePlayerCombo;

		protected long m_onlinePlayerCombo_SelectedPlayerIdentityId;

		protected MyGuiControlTextbox m_addCurrencyTextbox;

		protected MyGuiControlButton m_addCurrencyConfirmButton;

		protected MyGuiControlLabel m_labelCurrentBalanceValue;

		protected MyGuiControlLabel m_labelFinalBalanceValue;

		protected int m_playerCount;

		protected int m_factionCount;

		protected bool m_isPlayerSelected;

		protected bool m_isFactionSelected;

		protected long m_currentBalance;

		protected long m_finalBalance;

		protected long m_balanceDifference;

		protected MyGuiControlCombobox m_playerReputationCombo;

		protected long m_playerReputationCombo_SelectedPlayerIdentityId;

		protected MyGuiControlCombobox m_factionReputationCombo;

		protected long m_factionReputationCombo_SelectedPlayerIdentityId;

		protected MyGuiControlTextbox m_addReputationTextbox;

		protected MyGuiControlButton m_addReputationConfirmButton;

		protected MyGuiControlLabel m_labelCurrentReputationValue;

		protected MyGuiControlLabel m_labelFinalReputationValue;

		protected MyGuiControlCheckbox m_addReputationPropagate;

		protected int m_currentReputation;

		protected int m_finalReputation;

		protected int m_reputationDifference;

		protected MyGuiControlCheckbox m_invulnerableCheckbox;

		protected MyGuiControlCheckbox m_untargetableCheckbox;

		protected MyGuiControlCheckbox m_showPlayersCheckbox;

		protected MyGuiControlCheckbox m_keepOriginalOwnershipOnPasteCheckBox;

		protected MyGuiControlCheckbox m_ignoreSafeZonesCheckBox;

		protected MyGuiControlCheckbox m_ignorePcuCheckBox;

		protected MyGuiControlCheckbox m_canUseTerminals;

		protected MyGuiControlSlider m_timeDelta;

		protected MyGuiControlLabel m_timeDeltaValue;

		protected MyGuiControlListbox m_entityListbox;

		protected MyGuiControlCombobox m_entityTypeCombo;

		protected MyGuiControlCombobox m_entitySortCombo;

		private MyEntityList.MyEntityTypeEnum m_selectedType;

		private MyEntityList.MyEntitySortOrder m_selectedSort;

		private static bool m_invertOrder;

		private static bool m_damageHandler;

		private static HashSet<long> m_protectedCharacters = new HashSet<long>();

		private static MyPageEnum m_currentPage;

		private int m_currentGpsIndex;

		private bool m_unsavedTrashSettings;

		private AdminSettings m_newSettings;

		private bool m_unsavedTrashExitBoxIsOpened;

		private MyGuiControlCombobox m_trashRemovalCombo;

		private MyGuiControlStackPanel m_trashRemovalContentPanel;

		private Dictionary<MyTabControlEnum, MyTabContainer> m_tabs = new Dictionary<MyTabControlEnum, MyTabContainer>();

		protected MyGuiControlLabel m_enabledCheckboxGlobalLabel;

		protected MyGuiControlLabel m_damageCheckboxGlobalLabel;

		protected MyGuiControlLabel m_shootingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_drillingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_weldingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_grindingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_voxelHandCheckboxGlobalLabel;

		protected MyGuiControlLabel m_buildingCheckboxGlobalLabel;

		protected MyGuiControlLabel m_landingGearCheckboxGlobalLabel;

		protected MyGuiControlLabel m_convertToStationCheckboxGlobalLabel;

		protected MyGuiControlCheckbox m_enabledGlobalCheckbox;

		protected MyGuiControlCheckbox m_damageGlobalCheckbox;

		protected MyGuiControlCheckbox m_shootingGlobalCheckbox;

		protected MyGuiControlCheckbox m_drillingGlobalCheckbox;

		protected MyGuiControlCheckbox m_weldingGlobalCheckbox;

		protected MyGuiControlCheckbox m_grindingGlobalCheckbox;

		protected MyGuiControlCheckbox m_voxelHandGlobalCheckbox;

		protected MyGuiControlCheckbox m_buildingGlobalCheckbox;

		protected MyGuiControlCheckbox m_landingGearGlobalCheckbox;

		protected MyGuiControlCheckbox m_convertToStationGlobalCheckbox;

		protected MyGuiControlScrollablePanel m_optionsGroup;

		protected MyGuiControlLabel m_selectSafeZoneLabel;

		protected MyGuiControlLabel m_selectZoneShapeLabel;

		protected MyGuiControlLabel m_selectAxisLabel;

		protected MyGuiControlLabel m_zoneRadiusLabel;

		protected MyGuiControlLabel m_zoneSizeLabel;

		protected MyGuiControlLabel m_zoneRadiusValueLabel;

		protected MyGuiControlCombobox m_safeZonesCombo;

		protected MyGuiControlCombobox m_safeZonesTypeCombo;

		protected MyGuiControlCombobox m_safeZonesAxisCombo;

		protected MyGuiControlSlider m_sizeSlider;

		protected MyGuiControlSlider m_radiusSlider;

		protected MyGuiControlButton m_addSafeZoneButton;

		protected MyGuiControlButton m_repositionSafeZoneButton;

		protected MyGuiControlButton m_moveToSafeZoneButton;

		protected MyGuiControlButton m_removeSafeZoneButton;

		protected MyGuiControlButton m_renameSafeZoneButton;

		protected MyGuiControlButton m_configureFilterButton;

		protected MyGuiControlLabel m_enabledCheckboxLabel;

		protected MyGuiControlLabel m_damageCheckboxLabel;

		protected MyGuiControlLabel m_shootingCheckboxLabel;

		protected MyGuiControlLabel m_drillingCheckboxLabel;

		protected MyGuiControlLabel m_weldingCheckboxLabel;

		protected MyGuiControlLabel m_grindingCheckboxLabel;

		protected MyGuiControlLabel m_voxelHandCheckboxLabel;

		protected MyGuiControlLabel m_buildingCheckboxLabel;

		protected MyGuiControlLabel m_landingGearLockCheckboxLabel;

		protected MyGuiControlLabel m_convertToStationCheckboxLabel;

		protected MyGuiControlCheckbox m_enabledCheckbox;

		protected MyGuiControlCheckbox m_damageCheckbox;

		protected MyGuiControlCheckbox m_shootingCheckbox;

		protected MyGuiControlCheckbox m_drillingCheckbox;

		protected MyGuiControlCheckbox m_weldingCheckbox;

		protected MyGuiControlCheckbox m_grindingCheckbox;

		protected MyGuiControlCheckbox m_voxelHandCheckbox;

		protected MyGuiControlCheckbox m_buildingCheckbox;

		protected MyGuiControlCheckbox m_landingGearLockCheckbox;

		protected MyGuiControlCheckbox m_convertToStationCheckbox;

		private MyGuiControlCombobox m_textureCombo;

		private MyGuiControlColor m_colorSelector;

		private MySafeZone m_selectedSafeZone;

		private bool m_recreateInProgress;

		public MyGuiScreenAdminMenu()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
			if (!Sync.IsServer)
			{
				m_static = this;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestSettingFromServer_Implementation);
			}
			else
			{
				CreateScreen();
			}
			MySessionComponentSafeZones.OnAddSafeZone += MySafeZones_OnAddSafeZone;
			MySessionComponentSafeZones.OnRemoveSafeZone += MySafeZones_OnRemoveSafeZone;
		}

		private void CreateScreen()
		{
			m_closeOnEsc = false;
			base.CanBeHidden = true;
			base.CanHideOthers = true;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			StoreTrashSettings_RealToTmp();
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_tabs.Clear();
			Vector2 controlPadding = new Vector2(0.02f, 0.02f);
			float scale = 0.8f;
			float separatorSize = 0.01f;
			float num = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - controlPadding.X * 2f;
			float num2 = (SCREEN_SIZE.Y - 1f) / 2f;
			m_static = this;
			m_currentPosition = -m_size.Value / 2f;
			m_currentPosition += controlPadding;
			m_currentPosition.Y += num2;
			m_scale = scale;
			AddCaption(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ModeSelect).ToString(), Color.White.ToVector4(), m_controlPadding + new Vector2(0f - HIDDEN_PART_RIGHT, num2 - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.365f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			m_currentPosition.X += 0.018f;
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + controlPadding.Y - 0.012f;
			m_modeCombo = AddCombo();
			if (MySession.Static.IsUserSpaceMaster(Sync.MyId))
			{
				m_modeCombo.AddItem(0L, MySpaceTexts.ScreenDebugAdminMenu_AdminTools);
				m_modeCombo.AddItem(2L, MyCommonTexts.ScreenDebugAdminMenu_CycleObjects);
				m_modeCombo.AddItem(1L, MySpaceTexts.ScreenDebugAdminMenu_Cleanup);
				m_modeCombo.AddItem(3L, MySpaceTexts.ScreenDebugAdminMenu_EntityList);
				if (MySession.Static.IsUserAdmin(Sync.MyId))
				{
					m_modeCombo.AddItem(4L, MySpaceTexts.ScreenDebugAdminMenu_SafeZones);
					m_modeCombo.AddItem(5L, MySpaceTexts.ScreenDebugAdminMenu_GlobalSafeZone);
					m_modeCombo.AddItem(6L, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool);
					m_modeCombo.AddItem(7L, MySpaceTexts.ScreenDebugAdminMenu_Economy);
				}
				else if (m_currentPage == MyPageEnum.GlobalSafeZone || m_currentPage == MyPageEnum.SafeZones || m_currentPage == MyPageEnum.ReplayTool || m_currentPage == MyPageEnum.Economy)
				{
					m_currentPage = MyPageEnum.CycleObjects;
				}
				m_modeCombo.SelectItemByKey((long)m_currentPage);
			}
			else
			{
				m_modeCombo.AddItem(0L, MySpaceTexts.ScreenDebugAdminMenu_AdminTools);
				m_currentPage = MyPageEnum.AdminTools;
				m_modeCombo.SelectItemByKey((long)m_currentPage);
			}
			m_modeCombo.ItemSelected += OnModeComboSelect;
			switch (m_currentPage)
			{
			case MyPageEnum.CycleObjects:
			{
				m_currentPosition.Y += 0.03f;
				MyGuiControlSeparatorList myGuiControlSeparatorList4 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.19f), m_size.Value.X * 0.73f);
				myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.138f), m_size.Value.X * 0.73f);
				myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.305f), m_size.Value.X * 0.73f);
				Controls.Add(myGuiControlSeparatorList4);
				MyGuiControlLabel control23 = new MyGuiControlLabel
				{
					Position = new Vector2(-0.16f, -0.335f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SortBy) + ":"
				};
				Controls.Add(control23);
				MyGuiControlCombobox myGuiControlCombobox = AddCombo(m_order, OnOrderChanged, enabled: true, 10, null, m_labelColor);
				myGuiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				myGuiControlCombobox.PositionX = 0.122f;
				myGuiControlCombobox.Size = new Vector2(0.22f, 1f);
				m_currentPosition.Y += 0.005f;
				MyGuiControlLabel control24 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_SmallGrids)
				};
				m_onlySmallGridsCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_onlySmallGridsCheckbox.IsCheckedChanged = OnSmallGridChanged;
				m_onlySmallGridsCheckbox.IsChecked = m_cyclingOptions.OnlySmallGrids;
				Controls.Add(m_onlySmallGridsCheckbox);
				Controls.Add(control24);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control25 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_LargeGrids)
				};
				m_onlyLargeGridsCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_onlyLargeGridsCheckbox.IsCheckedChanged = OnLargeGridChanged;
				m_onlyLargeGridsCheckbox.IsChecked = m_cyclingOptions.OnlyLargeGrids;
				Controls.Add(m_onlyLargeGridsCheckbox);
				Controls.Add(control25);
				m_currentPosition.Y += 0.12f;
				float y = m_currentPosition.Y;
				MyGuiControlButton myGuiControlButton7 = CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_First, delegate
				{
					OnCycleClicked(reset: true, forward: true);
				});
				myGuiControlButton7.PositionX += 0.003f;
				myGuiControlButton7.PositionY -= 0.0435f;
				m_currentPosition.Y = y;
				CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Next, delegate
				{
					OnCycleClicked(reset: false, forward: false);
				}).PositionX = -0.088f;
				m_currentPosition.Y = y;
				CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Previous, delegate
				{
					OnCycleClicked(reset: false, forward: true);
				}).PositionX = 0.055f;
				m_labelEntityName = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + " -"
				};
				Controls.Add(m_labelEntityName);
				m_currentPosition.Y += 0.035f;
				m_labelCurrentIndex = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), (m_entityId == 0L) ? "-" : m_metricValue.ToString()).ToString()
				};
				Controls.Add(m_labelCurrentIndex);
				m_currentPosition.Y += 0.208f;
				y = m_currentPosition.Y;
				m_removeItemButton = CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_Remove, delegate
				{
					OnEntityOperationClicked(MyEntityList.EntityListAction.Remove);
				});
				m_removeItemButton.PositionX += 0.003f;
				m_currentPosition.Y = y;
				m_stopItemButton = CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_Stop, delegate
				{
					OnEntityOperationClicked(MyEntityList.EntityListAction.Stop);
				});
				m_stopItemButton.PositionX += 0.003f;
				m_stopItemButton.PositionY += 0.0435f;
				m_currentPosition.Y = y;
				m_depowerItemButton = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_Depower, delegate
				{
					OnEntityOperationClicked(MyEntityList.EntityListAction.Depower);
				});
				m_depowerItemButton.PositionX += 0.003f;
				m_depowerItemButton.PositionY += 0.087f;
				m_currentPosition.Y += 0.125f;
				y = m_currentPosition.Y;
				m_currentPosition.Y = y;
				CreateDebugButton(0.284f, MyCommonTexts.SpectatorControls_None, OnPlayerControl, enabled: true, MySpaceTexts.SpectatorControls_None_Desc).PositionX += 0.003f;
				m_currentPosition.Y = y;
				MyGuiControlButton myGuiControlButton8 = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_TeleportHere, OnTeleportButton, MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Parent == null && MySession.Static.IsUserSpaceMaster(Sync.MyId), MySpaceTexts.ScreenDebugAdminMenu_TeleportHereToolTip);
				myGuiControlButton8.PositionX += 0.003f;
				myGuiControlButton8.PositionY += 0.0435f;
				bool flag2 = !Sync.IsServer;
				CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_ReplicateEverything, OnReplicateEverything, flag2, flag2 ? MyCommonTexts.ScreenDebugAdminMenu_ReplicateEverything_Tooltip : MySpaceTexts.ScreenDebugAdminMenu_ReplicateEverythingServer_Tooltip).PositionX += 0.003f;
				myGuiControlButton8.PositionY += 0.0435f;
				OnOrderChanged(m_order);
				break;
			}
			case MyPageEnum.TrashRemoval:
			{
				m_currentPosition.Y += 0.016f;
				bool flag = false;
				if (m_trashRemovalCombo == null)
				{
					m_trashRemovalCombo = new MyGuiControlCombobox
					{
						Position = m_currentPosition,
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
					};
					m_trashRemovalCombo.AddItem(0L, MyCommonTexts.ScreenDebugAdminMenu_GeneralTabButton);
					m_trashRemovalCombo.AddItem(1L, MyCommonTexts.ScreenDebugAdminMenu_VoxelTabButton);
					m_trashRemovalCombo.AddItem(2L, MyCommonTexts.ScreenDebugAdminMenu_OtherTabButton);
					m_trashRemovalCombo.ItemSelected += OnTrashRemovalItemSelected;
					flag = true;
				}
				Controls.Add(m_trashRemovalCombo);
				m_currentPosition.Y += m_trashRemovalCombo.Size.Y + 0.016f;
				m_tabs.Add(MyTabControlEnum.General, MyAdminMenuTabFactory.CreateTab(this, MyTabControlEnum.General));
				m_tabs.Add(MyTabControlEnum.Voxel, MyAdminMenuTabFactory.CreateTab(this, MyTabControlEnum.Voxel));
				m_tabs.Add(MyTabControlEnum.Other, MyAdminMenuTabFactory.CreateTab(this, MyTabControlEnum.Other));
				m_trashRemovalContentPanel = new MyGuiControlStackPanel
				{
					Position = m_currentPosition,
					Orientation = MyGuiOrientation.Vertical,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				MyGuiControlStackPanel myGuiControlStackPanel = new MyGuiControlStackPanel
				{
					Position = Vector2.Zero,
					Orientation = MyGuiOrientation.Horizontal,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				m_currentPosition.Y += 0.06f;
				MyGuiControlButton myGuiControlButton5 = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_SubmitChangesButton, OnSubmitButtonClicked, enabled: true, MyCommonTexts.ScreenDebugAdminMenu_SubmitChangesButtonTooltip, increaseSpacing: true, addToControls: false);
				myGuiControlButton5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				myGuiControlButton5.PositionX = -0.088f;
				myGuiControlStackPanel.Add(myGuiControlButton5);
				MyGuiControlButton myGuiControlButton6 = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_CancleChangesButton, OnCancelButtonClicked, enabled: true, null, increaseSpacing: true, addToControls: false);
				myGuiControlButton6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				myGuiControlButton6.PositionX = 0.055f;
				myGuiControlButton6.PositionY -= 0.0435f;
				myGuiControlButton6.Margin = new Thickness(0.005f, 0f, 0f, 0f);
				myGuiControlStackPanel.Add(myGuiControlButton6);
				myGuiControlStackPanel.UpdateArrange();
				myGuiControlStackPanel.UpdateMeasure();
				m_trashRemovalContentPanel.Add(myGuiControlStackPanel);
				m_trashRemovalContentPanel.UpdateArrange();
				m_trashRemovalContentPanel.UpdateMeasure();
				Controls.Add(myGuiControlStackPanel);
				Controls.Add(myGuiControlButton5);
				Controls.Add(myGuiControlButton6);
				Controls.Add(m_trashRemovalContentPanel);
				if (flag)
				{
					m_trashRemovalCombo.SelectItemByKey(0L);
				}
				else
				{
					OnTrashRemovalItemSelected();
				}
				break;
			}
			case MyPageEnum.AdminTools:
			{
				m_currentPosition.Y += 0.03f;
				MyGuiControlLabel control14 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EnableAdminMode)
				};
				m_creativeCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_creativeCheckbox.IsCheckedChanged = OnEnableAdminModeChanged;
				m_creativeCheckbox.SetToolTip(MyCommonTexts.ScreenDebugAdminMenu_EnableAdminMode_Tooltip);
				m_creativeCheckbox.IsChecked = MySession.Static.CreativeToolsEnabled(Sync.MyId);
				m_creativeCheckbox.Enabled = MySession.Static.HasCreativeRights;
				Controls.Add(m_creativeCheckbox);
				Controls.Add(control14);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control15 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Invulnerable)
				};
				m_invulnerableCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_invulnerableCheckbox.IsCheckedChanged = OnInvulnerableChanged;
				m_invulnerableCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_InvulnerableToolTip);
				m_invulnerableCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.Invulnerable);
				m_invulnerableCheckbox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_invulnerableCheckbox);
				Controls.Add(control15);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control16 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Untargetable)
				};
				m_untargetableCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_untargetableCheckbox.IsCheckedChanged = OnUntargetableChanged;
				m_untargetableCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_UntargetableToolTip);
				m_untargetableCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.Untargetable);
				m_untargetableCheckbox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_untargetableCheckbox);
				Controls.Add(control16);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control17 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ShowPlayers)
				};
				m_showPlayersCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_showPlayersCheckbox.IsCheckedChanged = OnShowPlayersChanged;
				m_showPlayersCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_ShowPlayersToolTip);
				m_showPlayersCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.ShowPlayers);
				m_showPlayersCheckbox.Enabled = MySession.Static.IsUserModerator(Sync.MyId);
				Controls.Add(m_showPlayersCheckbox);
				Controls.Add(control17);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control18 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_UseTerminals)
				};
				m_canUseTerminals = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_canUseTerminals.IsCheckedChanged = OnUseTerminalsChanged;
				m_canUseTerminals.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_UseTerminalsToolTip);
				m_canUseTerminals.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals);
				m_canUseTerminals.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_canUseTerminals);
				Controls.Add(control18);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control19 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_KeepOriginalOwnershipOnPaste)
				};
				m_keepOriginalOwnershipOnPasteCheckBox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_keepOriginalOwnershipOnPasteCheckBox.IsCheckedChanged = OnKeepOwnershipChanged;
				m_keepOriginalOwnershipOnPasteCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_KeepOriginalOwnershipOnPasteTip);
				m_keepOriginalOwnershipOnPasteCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.KeepOriginalOwnershipOnPaste);
				m_keepOriginalOwnershipOnPasteCheckBox.Enabled = MySession.Static.IsUserSpaceMaster(Sync.MyId);
				Controls.Add(m_keepOriginalOwnershipOnPasteCheckBox);
				Controls.Add(control19);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control20 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_IgnoreSafeZones)
				};
				m_ignoreSafeZonesCheckBox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_ignoreSafeZonesCheckBox.IsCheckedChanged = OnIgnoreSafeZonesChanged;
				m_ignoreSafeZonesCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_IgnoreSafeZonesTip);
				m_ignoreSafeZonesCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.IgnoreSafeZones);
				m_ignoreSafeZonesCheckBox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_ignoreSafeZonesCheckBox);
				Controls.Add(control20);
				m_currentPosition.Y += 0.045f;
				MyGuiControlLabel control21 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.001f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Pcu)
				};
				m_ignorePcuCheckBox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				m_ignorePcuCheckBox.IsCheckedChanged = OnIgnorePcuChanged;
				m_ignorePcuCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_IgnorePcuTip);
				m_ignorePcuCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.IgnorePcu);
				m_ignorePcuCheckBox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
				Controls.Add(m_ignorePcuCheckBox);
				Controls.Add(control21);
				if (MySession.Static.IsUserAdmin(Sync.MyId))
				{
					m_currentPosition.Y += 0.045f;
					MyGuiControlLabel control22 = new MyGuiControlLabel
					{
						Position = m_currentPosition + new Vector2(0.001f, 0f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
						Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_TimeOfDay)
					};
					Controls.Add(control22);
					m_timeDeltaValue = new MyGuiControlLabel
					{
						Position = m_currentPosition + new Vector2(0.285f, 0f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
						Text = "0.00"
					};
					Controls.Add(m_timeDeltaValue);
					m_currentPosition.Y += 0.035f;
					m_timeDelta = new MyGuiControlSlider(m_currentPosition + new Vector2(0.001f, 0f), 0f, (MySession.Static == null) ? 1f : MySession.Static.Settings.SunRotationIntervalMinutes);
					m_timeDelta.Size = new Vector2(0.285f, 1f);
					m_timeDelta.Value = MyTimeOfDayHelper.TimeOfDay;
					m_timeDelta.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
					MyGuiControlSlider timeDelta = m_timeDelta;
					timeDelta.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(timeDelta.ValueChanged, new Action<MyGuiControlSlider>(TimeDeltaChanged));
					m_timeDeltaValue.Text = $"{m_timeDelta.Value:0.00}";
					Controls.Add(m_timeDelta);
					m_currentPosition.Y += 0.07f;
				}
				break;
			}
			case MyPageEnum.EntityList:
			{
				m_currentPosition.Y += 0.095f;
				MyGuiControlLabel control11 = new MyGuiControlLabel
				{
					Position = new Vector2(-0.16f, -0.334f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MyCommonTexts.Select) + ":"
				};
				Controls.Add(control11);
				m_currentPosition.Y -= 0.065f;
				m_entityTypeCombo = AddCombo(m_selectedType, ValueChanged, enabled: true, 10, null, m_labelColor);
				m_entityTypeCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_entityTypeCombo.PositionX = 0.122f;
				m_entityTypeCombo.Size = new Vector2(0.22f, 1f);
				MyGuiControlLabel control12 = new MyGuiControlLabel
				{
					Position = new Vector2(-0.16f, -0.284f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SortBy) + ":"
				};
				Controls.Add(control12);
				m_entitySortCombo = AddCombo(m_selectedSort, ValueChanged, enabled: true, 10, null, m_labelColor);
				m_entitySortCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_entitySortCombo.PositionX = 0.122f;
				m_entitySortCombo.Size = new Vector2(0.22f, 1f);
				MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.231f), m_size.Value.X * 0.73f);
				Controls.Add(myGuiControlSeparatorList2);
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
				{
					Position = new Vector2(-0.153f, -0.205f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities)
				};
				MyGuiControlPanel control13 = new MyGuiControlPanel(new Vector2(myGuiControlLabel.PositionX - 0.0085f, myGuiControlLabel.Position.Y - 0.005f), new Vector2(0.2865f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
				{
					BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
				};
				Controls.Add(control13);
				Controls.Add(myGuiControlLabel);
				m_currentPosition.Y += 0.065f;
				m_entityListbox = new MyGuiControlListbox(Vector2.Zero, MyGuiControlListboxStyleEnum.Blueprints);
				m_entityListbox.Size = new Vector2(num, 0f);
				m_entityListbox.Enabled = true;
				m_entityListbox.VisibleRowsCount = 12;
				m_entityListbox.Position = m_entityListbox.Size / 2f + m_currentPosition;
				m_entityListbox.ItemClicked += EntityListItemClicked;
				m_entityListbox.MultiSelect = true;
				MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.271f), m_size.Value.X * 0.73f);
				Controls.Add(myGuiControlSeparatorList3);
				m_currentPosition = m_entityListbox.GetPositionAbsoluteBottomLeft();
				m_currentPosition.Y += 0.045f;
				MyGuiControlButton myGuiControlButton = CreateDebugButton(0.14f, MyCommonTexts.SpectatorControls_None, OnPlayerControl, enabled: true, MySpaceTexts.SpectatorControls_None_Desc);
				myGuiControlButton.PositionX = -0.088f;
				MyGuiControlButton myGuiControlButton2 = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_TeleportHere, OnTeleportButton, MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Parent == null, MySpaceTexts.ScreenDebugAdminMenu_TeleportHereToolTip);
				myGuiControlButton2.PositionX = 0.055f;
				myGuiControlButton2.PositionY = myGuiControlButton.PositionY;
				float y = m_currentPosition.Y;
				m_currentPosition.Y = y;
				m_stopItemButton = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Stop, delegate
				{
					OnEntityListActionClicked(MyEntityList.EntityListAction.Stop);
				});
				m_stopItemButton.PositionX = -0.088f;
				m_stopItemButton.PositionY -= 0.0435f;
				m_currentPosition.Y = y;
				m_depowerItemButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Depower, delegate
				{
					OnEntityListActionClicked(MyEntityList.EntityListAction.Depower);
				});
				m_depowerItemButton.PositionX = 0.055f;
				m_depowerItemButton.PositionY = m_stopItemButton.PositionY;
				m_removeItemButton = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Remove, delegate
				{
					OnEntityListActionClicked(MyEntityList.EntityListAction.Remove);
				});
				m_removeItemButton.PositionX -= 0.068f;
				m_removeItemButton.PositionY -= 0.0435f;
				MyGuiControlButton myGuiControlButton3 = CreateDebugButton(0.14f, MySpaceTexts.buttonRefresh, OnRefreshButton, enabled: true, MySpaceTexts.ProgrammableBlock_ButtonRefreshScripts);
				myGuiControlButton3.PositionX += 0.075f;
				myGuiControlButton3.PositionY = m_removeItemButton.PositionY;
				MyGuiControlButton myGuiControlButton4 = CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_RemoveOwner, OnRemoveOwnerButton, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_RemoveOwnerToolTip);
				myGuiControlButton4.PositionX += 0.003f;
				myGuiControlButton4.PositionY -= 0.087f;
				Controls.Add(m_entityListbox);
				ValueChanged((MyEntityList.MyEntityTypeEnum)m_entityTypeCombo.GetSelectedKey());
				break;
			}
			case MyPageEnum.SafeZones:
				RecreateSafeZonesControls(ref controlPadding, separatorSize, num);
				break;
			case MyPageEnum.GlobalSafeZone:
				RecreateGlobalSafeZoneControls(ref controlPadding, separatorSize, num);
				break;
			case MyPageEnum.ReplayTool:
				RecreateReplayToolControls(ref controlPadding, separatorSize, num);
				break;
			case MyPageEnum.Economy:
			{
				m_currentPosition.X += 0.003f;
				m_currentPosition.Y += 0.03f;
				MyGuiControlLabel control = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.16f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_Label)
				};
				Controls.Add(control);
				m_currentPosition.Y += 0.05f;
				ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
				m_playerCount = onlinePlayers.Count;
				m_factionCount = MySession.Static.Factions.Count();
				m_onlinePlayerCombo = new MyGuiControlCombobox();
				m_onlinePlayerCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_Player_Tooltip));
				List<MyIdNamePair> list = new List<MyIdNamePair>();
				List<MyIdNamePair> list2 = new List<MyIdNamePair>();
				MyIdNamePairComparer comparer = new MyIdNamePairComparer();
				MyIdNamePair item;
				foreach (MyPlayer item2 in onlinePlayers)
				{
					item = new MyIdNamePair
					{
						Id = item2.Identity.IdentityId,
						Name = item2.DisplayName
					};
					list.Add(item);
				}
				foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
				{
					item = new MyIdNamePair
					{
						Id = faction.Key,
						Name = faction.Value.Tag
					};
					list2.Add(item);
				}
				list.Sort(comparer);
				list2.Sort(comparer);
				int num3 = 0;
				foreach (MyIdNamePair item3 in list)
				{
					m_onlinePlayerCombo.AddItem(item3.Id, item3.Name, num3);
					num3++;
				}
				foreach (MyIdNamePair item4 in list2)
				{
					m_onlinePlayerCombo.AddItem(item4.Id, item4.Name, num3);
					num3++;
				}
				m_onlinePlayerCombo.ItemSelected += OnlinePlayerCombo_ItemSelected;
				m_onlinePlayerCombo.SelectItemByIndex(-1);
				m_onlinePlayerCombo.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_onlinePlayerCombo);
				m_currentPosition.Y += 0.04f;
				string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
				string texture = (icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
				Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(1f));
				float num4 = screenSizeFromNormalizedSize.X / screenSizeFromNormalizedSize.Y;
				Vector2 value = new Vector2(0.28f, 0.0033f);
				Vector2 size = new Vector2(0.018f, num4 * 0.018f);
				float num5 = size.X + 0.01f;
				MyGuiControlLabel control2 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_CurrentBalance,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control2);
				m_labelCurrentBalanceValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f - num5, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = MyBankingSystem.GetFormatedValue(0L),
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelCurrentBalanceValue);
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage
				{
					Position = m_currentPosition + value,
					Size = size,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
				};
				myGuiControlImage.SetTexture(texture);
				Controls.Add(myGuiControlImage);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control3 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_ChangeBalance,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control3);
				m_addCurrencyTextbox = new MyGuiControlTextbox(null, "0", 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
				m_addCurrencyTextbox.Position = m_currentPosition + new Vector2(0.218f - num5 / 2f, 0f);
				m_addCurrencyTextbox.Size = new Vector2(m_addCurrencyTextbox.Size.X * 0.4f - num5, m_addCurrencyTextbox.Size.Y);
				m_addCurrencyTextbox.TextChanged += AddCurrency_TextChanged;
				Controls.Add(m_addCurrencyTextbox);
				MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
				{
					Position = m_currentPosition + value,
					Size = size,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
				};
				myGuiControlImage2.SetTexture(texture);
				Controls.Add(myGuiControlImage2);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control4 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_FinalBalance,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control4);
				m_labelFinalBalanceValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f - num5, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = MyBankingSystem.GetFormatedValue(0L),
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelFinalBalanceValue);
				MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage
				{
					Position = m_currentPosition + value,
					Size = size,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
				};
				myGuiControlImage3.SetTexture(texture);
				Controls.Add(myGuiControlImage3);
				m_currentPosition.Y += 0.06f;
				m_addCurrencyConfirmButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_CoonfirmButton), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, AddCurrency_ButtonClicked);
				m_addCurrencyConfirmButton.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_addCurrencyConfirmButton);
				m_currentBalance = 0L;
				m_finalBalance = 0L;
				m_balanceDifference = 0L;
				m_currentPosition.Y += 0.1f;
				MyGuiControlLabel control5 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_PlayerLabel)
				};
				Controls.Add(control5);
				m_currentPosition.Y += 0.05f;
				m_playerReputationCombo = new MyGuiControlCombobox();
				m_playerReputationCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_Player_Tooltip));
				List<MyIdNamePair> list3 = new List<MyIdNamePair>();
				foreach (MyPlayer item5 in onlinePlayers)
				{
					item = new MyIdNamePair
					{
						Id = item5.Identity.IdentityId,
						Name = item5.DisplayName
					};
					list3.Add(item);
				}
				list3.Sort(comparer);
				num3 = 0;
				foreach (MyIdNamePair item6 in list3)
				{
					m_playerReputationCombo.AddItem(item6.Id, item6.Name, num3);
					num3++;
				}
				m_playerReputationCombo.ItemSelected += playerReputationCombo_ItemSelected;
				m_playerReputationCombo.SelectItemByIndex(-1);
				m_playerReputationCombo.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_playerReputationCombo);
				m_currentPosition.Y += 0.03f;
				MyGuiControlLabel control6 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_FactionLabel)
				};
				Controls.Add(control6);
				m_currentPosition.Y += 0.05f;
				m_factionReputationCombo = new MyGuiControlCombobox();
				m_factionReputationCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_Faction_Tooltip));
				List<MyIdNamePair> list4 = new List<MyIdNamePair>();
				foreach (KeyValuePair<long, MyFaction> faction2 in MySession.Static.Factions)
				{
					item = new MyIdNamePair
					{
						Id = faction2.Key,
						Name = faction2.Value.Tag
					};
					list4.Add(item);
				}
				list4.Sort(comparer);
				num3 = 0;
				foreach (MyIdNamePair item7 in list4)
				{
					m_factionReputationCombo.AddItem(item7.Id, item7.Name, num3);
					num3++;
				}
				m_factionReputationCombo.ItemSelected += factionReputationCombo_ItemSelected;
				m_factionReputationCombo.SelectItemByIndex(-1);
				m_factionReputationCombo.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_factionReputationCombo);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control7 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_CurrentReputation,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control7);
				m_labelCurrentReputationValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = 0.ToString(),
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelCurrentReputationValue);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control8 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ChangeReputation,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control8);
				m_addReputationTextbox = new MyGuiControlTextbox(null, "0", 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
				m_addReputationTextbox.Position = m_currentPosition + new Vector2(0.218f, 0f);
				m_addReputationTextbox.Size = new Vector2(m_addReputationTextbox.Size.X * 0.4f, m_addCurrencyTextbox.Size.Y);
				m_addReputationTextbox.TextChanged += AddReputation_TextChanged;
				Controls.Add(m_addReputationTextbox);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control9 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_FinalReputation,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control9);
				m_labelFinalReputationValue = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0.28f, 0f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
					Text = 0.ToString(),
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(m_labelFinalReputationValue);
				m_currentPosition.Y += 0.04f;
				MyGuiControlLabel control10 = new MyGuiControlLabel
				{
					Position = m_currentPosition + new Vector2(0f, 0f),
					TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ReputationPropagate,
					AutoEllipsis = false,
					ColorMask = MyTerminalFactionController.COLOR_CUSTOM_GREY
				};
				Controls.Add(control10);
				m_addReputationPropagate = new MyGuiControlCheckbox
				{
					Position = m_currentPosition + new Vector2(0.273f, 0f),
					IsChecked = false
				};
				m_addReputationPropagate.SetToolTip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ReputationPropagate_Tooltip));
				Controls.Add(m_addReputationPropagate);
				m_currentPosition.Y += 0.06f;
				m_addReputationConfirmButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ConfirmButton), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, AddReputation_ButtonClicked);
				m_addReputationConfirmButton.Position = m_currentPosition + new Vector2(0.14f, 0f);
				Controls.Add(m_addReputationConfirmButton);
				m_currentReputation = 0;
				m_finalReputation = 0;
				m_reputationDifference = 0;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void factionReputationCombo_ItemSelected()
		{
			m_factionReputationCombo_SelectedPlayerIdentityId = m_factionReputationCombo.GetSelectedKey();
			UpdateReputation();
		}

		private void playerReputationCombo_ItemSelected()
		{
			m_playerReputationCombo_SelectedPlayerIdentityId = m_playerReputationCombo.GetSelectedKey();
			UpdateReputation();
		}

		private void UpdateReputation()
		{
			if (m_factionReputationCombo_SelectedPlayerIdentityId != 0L && m_playerReputationCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReputation, m_playerReputationCombo_SelectedPlayerIdentityId, m_factionReputationCombo_SelectedPlayerIdentityId);
			}
		}

		[Event(null, 1147)]
		[Reliable]
		[Server]
		private static void RequestReputation(long playerIdentityId, long factionId)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value))
			{
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerIdentityId, factionId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReputationCallback, playerIdentityId, factionId, relationBetweenPlayerAndFaction.Item2, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1159)]
		[Reliable]
		[Client]
		private static void RequestReputationCallback(long playerIdentityId, long factionId, int reputation)
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>()?.RequestReputationCallback_Internal(playerIdentityId, factionId, reputation);
		}

		protected void RequestReputationCallback_Internal(long playerIdentityId, long factionId, int reputation)
		{
			if (m_playerReputationCombo_SelectedPlayerIdentityId == playerIdentityId && m_factionReputationCombo_SelectedPlayerIdentityId == factionId)
			{
				m_currentReputation = ClampReputation(reputation);
				m_finalReputation = ClampReputation(reputation + m_reputationDifference);
				UpdateReputationTexts();
			}
		}

		protected void UpdateReputationTexts()
		{
			m_labelCurrentReputationValue.Text = m_currentReputation.ToString();
			m_labelFinalReputationValue.Text = m_finalReputation.ToString();
		}

		private void AddReputation_ButtonClicked(MyGuiControlButton obj)
		{
			bool isChecked = m_addReputationPropagate.IsChecked;
			if (m_playerReputationCombo_SelectedPlayerIdentityId != 0L || m_factionReputationCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestChangeReputation, m_playerReputationCombo_SelectedPlayerIdentityId, m_factionReputationCombo_SelectedPlayerIdentityId, m_reputationDifference, isChecked);
			}
		}

		[Event(null, 1187)]
		[Reliable]
		[Server]
		private static void RequestChangeReputation(long identityId, long factionId, int reputationChange, bool shouldPropagate)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value))
			{
				MySession.Static.Factions.AddFactionPlayerReputation(identityId, factionId, reputationChange, shouldPropagate);
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(identityId, factionId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReputationCallback, identityId, factionId, relationBetweenPlayerAndFaction.Item2, MyEventContext.Current.Sender);
			}
		}

		private void AddReputation_TextChanged(MyGuiControlTextbox obj)
		{
			if (int.TryParse(obj.Text, out m_reputationDifference))
			{
				m_finalReputation = ClampReputation(m_currentReputation + m_reputationDifference);
			}
			else
			{
				m_finalReputation = ClampReputation(m_currentReputation);
			}
			UpdateReputationTexts();
		}

		private int ClampReputation(int reputation)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			int hostileMax = component.GetHostileMax();
			if (reputation < hostileMax)
			{
				return hostileMax;
			}
			int friendlyMax = component.GetFriendlyMax();
			if (reputation > friendlyMax)
			{
				return friendlyMax;
			}
			return reputation;
		}

		private void OnTrashRemovalItemSelected()
		{
			MyTabControlEnum key = (MyTabControlEnum)m_trashRemovalCombo.GetSelectedKey();
			if (m_tabs.TryGetValue(key, out MyTabContainer _))
			{
				MyGuiControlParent control = m_tabs[key].Control;
				control.UpdateArrange();
				control.UpdateMeasure();
				if (m_trashRemovalContentPanel.GetControlCount() > 1)
				{
					MyGuiControlBase at = m_trashRemovalContentPanel.GetAt(0);
					at.Visible = false;
					m_trashRemovalContentPanel.Remove(at);
				}
				m_trashRemovalContentPanel.AddAt(0, control);
				control.Visible = true;
				m_trashRemovalContentPanel.UpdateArrange();
				m_trashRemovalContentPanel.UpdateMeasure();
			}
		}

		private void AddCurrency_TextChanged(MyGuiControlTextbox obj)
		{
			if (long.TryParse(obj.Text, out m_balanceDifference))
			{
				m_finalBalance = m_currentBalance + m_balanceDifference;
			}
			else
			{
				m_finalBalance = m_currentBalance;
			}
			UpdateBalanceTexts();
		}

		protected void UpdateBalanceTexts()
		{
			m_labelCurrentBalanceValue.Text = MyBankingSystem.GetFormatedValue(m_currentBalance);
			m_labelFinalBalanceValue.Text = MyBankingSystem.GetFormatedValue((m_finalBalance > 0) ? m_finalBalance : 0);
		}

		private void AddCurrency_ButtonClicked(MyGuiControlButton obj)
		{
			if (m_onlinePlayerCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestChange, m_onlinePlayerCombo_SelectedPlayerIdentityId, m_balanceDifference);
			}
		}

		private void OnlinePlayerCombo_ItemSelected()
		{
			int selectedIndex = m_onlinePlayerCombo.GetSelectedIndex();
			m_onlinePlayerCombo_SelectedPlayerIdentityId = m_onlinePlayerCombo.GetSelectedKey();
			if (selectedIndex < m_playerCount)
			{
				m_isPlayerSelected = true;
				m_isFactionSelected = false;
			}
			else if (selectedIndex - m_playerCount < m_factionCount)
			{
				m_isPlayerSelected = false;
				m_isFactionSelected = true;
			}
			if (m_onlinePlayerCombo_SelectedPlayerIdentityId != 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestBalance, m_onlinePlayerCombo_SelectedPlayerIdentityId);
			}
		}

		[Event(null, 1291)]
		[Reliable]
		[Server]
		private static void RequestBalance(long accountOwner)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value) && MyBankingSystem.Static.TryGetAccountInfo(accountOwner, out MyAccountInfo account))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestBalanceCallback, accountOwner, account.Balance, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1304)]
		[Reliable]
		[Client]
		private static void RequestBalanceCallback(long accountOwner, long balance)
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>()?.RequestBalanceCallback_Internal(accountOwner, balance);
		}

		protected void RequestBalanceCallback_Internal(long accountOwner, long balance)
		{
			if (m_onlinePlayerCombo_SelectedPlayerIdentityId == accountOwner)
			{
				m_currentBalance = balance;
				m_finalBalance = balance + m_balanceDifference;
				UpdateBalanceTexts();
			}
		}

		[Event(null, 1322)]
		[Reliable]
		[Server]
		private static void RequestChange(long accountOwner, long balanceChange)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			if (MySession.Static.IsUserAdmin(value) && MyBankingSystem.ChangeBalance(accountOwner, balanceChange) && MyBankingSystem.Static.TryGetAccountInfo(accountOwner, out MyAccountInfo account))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestBalanceCallback, accountOwner, account.Balance, MyEventContext.Current.Sender);
			}
		}

		private void CircleGps(bool reset, bool forward)
		{
			m_onlyLargeGridsCheckbox.Enabled = false;
			m_onlySmallGridsCheckbox.Enabled = false;
			m_depowerItemButton.Enabled = false;
			m_removeItemButton.Enabled = false;
			m_stopItemButton.Enabled = false;
			if (MySession.Static == null || MySession.Static.Gpss == null || MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			if (forward)
			{
				m_currentGpsIndex--;
			}
			else
			{
				m_currentGpsIndex++;
			}
			m_gpsList.Clear();
			MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, m_gpsList);
			if (m_gpsList.Count == 0)
			{
				m_currentGpsIndex = 0;
				return;
			}
			if (m_currentGpsIndex < 0)
			{
				m_currentGpsIndex = m_gpsList.Count - 1;
			}
			if (m_gpsList.Count <= m_currentGpsIndex || reset)
			{
				m_currentGpsIndex = 0;
			}
			IMyGps myGps = m_gpsList[m_currentGpsIndex];
			Vector3D coords = myGps.Coords;
			m_labelEntityName.TextToDraw.Clear();
			m_labelEntityName.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName));
			m_labelEntityName.TextToDraw.Append(string.IsNullOrEmpty(myGps.Name) ? "-" : myGps.Name);
			m_labelCurrentIndex.TextToDraw.Clear().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), m_currentGpsIndex);
			MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
			Vector3D? vector3D = MyEntities.FindFreePlace(coords + Vector3D.One, 2f, 30);
			MySpectatorCameraController.Static.Position = (vector3D.HasValue ? vector3D.Value : (coords + Vector3D.One));
			MySpectatorCameraController.Static.Target = coords;
		}

		internal static void RecalcTrash()
		{
			if (!Sync.IsServer)
			{
				AdminSettings adminSettings = default(AdminSettings);
				adminSettings.flags = MySession.Static.Settings.TrashFlags;
				adminSettings.enable = MySession.Static.Settings.TrashRemovalEnabled;
				adminSettings.blockCount = MySession.Static.Settings.BlockCountThreshold;
				adminSettings.playerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
				adminSettings.gridCount = MySession.Static.Settings.OptimalGridCount;
				adminSettings.playerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
				adminSettings.characterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
				adminSettings.stopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
				adminSettings.removeOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
				adminSettings.voxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
				adminSettings.voxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
				adminSettings.voxelAge = MySession.Static.Settings.VoxelAgeThreshold;
				adminSettings.voxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
				AdminSettings arg = adminSettings;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UploadSettingsToServer, arg);
			}
		}

		private static bool TryAttachCamera(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
			{
				BoundingSphereD worldVolume = entity.PositionComp.WorldVolume;
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
				MySpectatorCameraController.Static.Position = worldVolume.Center + Math.Max((float)worldVolume.Radius, 1f) * Vector3.One;
				MySpectatorCameraController.Static.Target = worldVolume.Center;
				MySessionComponentAnimationSystem.Static.EntitySelectedForDebug = entity;
				return true;
			}
			return false;
		}

		private void UpdateCyclingAndDepower()
		{
			bool enabled = m_cyclingOptions.Enabled = (m_order != 0 && m_order != MyEntityCyclingOrder.FloatingObjects && m_order != MyEntityCyclingOrder.Gps);
			if (m_depowerItemButton != null)
			{
				m_depowerItemButton.Enabled = enabled;
			}
		}

		private void UpdateSmallLargeGridSelection()
		{
			if (m_currentPage == MyPageEnum.CycleObjects)
			{
				bool enabled = m_order != 0 && m_order != MyEntityCyclingOrder.FloatingObjects && m_order != MyEntityCyclingOrder.Gps;
				m_removeItemButton.Enabled = true;
				m_onlySmallGridsCheckbox.Enabled = enabled;
				m_onlyLargeGridsCheckbox.Enabled = enabled;
			}
		}

		private static void UpdateRemoveAndDepowerButton(MyGuiScreenAdminMenu menu, long entityId, bool disableOverride = false)
		{
			MyEntities.TryGetEntityById(entityId, out MyEntity entity);
			bool flag = m_currentPage != MyPageEnum.CycleObjects || m_order != MyEntityCyclingOrder.Gps;
			menu.m_removeItemButton.Enabled = ((flag && !menu.m_attachIsNpcStation) || disableOverride);
			if (menu.m_depowerItemButton != null)
			{
				menu.m_depowerItemButton.Enabled = ((entity is MyCubeGrid && flag && !menu.m_attachIsNpcStation) || disableOverride);
			}
			if (menu.m_stopItemButton != null)
			{
				menu.m_stopItemButton.Enabled = ((entity != null && !(entity is MyVoxelBase) && flag && !menu.m_attachIsNpcStation) || disableOverride);
			}
			if (m_currentPage == MyPageEnum.CycleObjects)
			{
				if (!(entity is MyVoxelBase))
				{
					menu.m_labelEntityName.TextToDraw = new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + ((entity == null) ? "-" : entity.DisplayName));
				}
				else
				{
					menu.m_labelEntityName.TextToDraw = new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + ((MyVoxelBase)entity).StorageName);
				}
			}
		}

		[Event(null, 1519)]
		[Reliable]
		[Server]
		private static void RequestSettingFromServer_Implementation()
		{
			AdminSettings adminSettings = default(AdminSettings);
			adminSettings.flags = MySession.Static.Settings.TrashFlags;
			adminSettings.enable = MySession.Static.Settings.TrashRemovalEnabled;
			adminSettings.blockCount = MySession.Static.Settings.BlockCountThreshold;
			adminSettings.playerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
			adminSettings.gridCount = MySession.Static.Settings.OptimalGridCount;
			adminSettings.playerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
			adminSettings.characterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
			adminSettings.AdminSettingsFlags = MySession.Static.RemoteAdminSettings.GetValueOrDefault(MyEventContext.Current.Sender.Value, AdminSettingsEnum.None);
			adminSettings.stopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
			adminSettings.removeOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
			adminSettings.voxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
			adminSettings.voxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
			adminSettings.voxelAge = MySession.Static.Settings.VoxelAgeThreshold;
			adminSettings.voxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
			AdminSettings arg = adminSettings;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DownloadSettingFromServer, arg, MyEventContext.Current.Sender);
		}

		private void ValueChanged(MyEntityList.MyEntitySortOrder selectedOrder)
		{
			if (m_selectedSort == selectedOrder)
			{
				m_invertOrder = !m_invertOrder;
			}
			else
			{
				m_invertOrder = false;
			}
			m_selectedSort = selectedOrder;
			List<MyEntityList.MyEntityListInfoItem> items = new List<MyEntityList.MyEntityListInfoItem>(m_entityListbox.Items.Count);
			foreach (MyGuiControlListbox.Item item in m_entityListbox.Items)
			{
				items.Add((MyEntityList.MyEntityListInfoItem)item.UserData);
			}
			MyEntityList.SortEntityList(selectedOrder, ref items, m_invertOrder);
			m_entityListbox.Items.Clear();
			MyEntityList.MyEntityTypeEnum myEntityTypeEnum = (MyEntityList.MyEntityTypeEnum)m_entityTypeCombo.GetSelectedKey();
			bool isGrid = myEntityTypeEnum == MyEntityList.MyEntityTypeEnum.Grids || myEntityTypeEnum == MyEntityList.MyEntityTypeEnum.LargeGrids || myEntityTypeEnum == MyEntityList.MyEntityTypeEnum.SmallGrids;
			foreach (MyEntityList.MyEntityListInfoItem item2 in items)
			{
				StringBuilder formattedDisplayName = MyEntityList.GetFormattedDisplayName(selectedOrder, item2, isGrid);
				m_entityListbox.Add(new MyGuiControlListbox.Item(formattedDisplayName, MyEntityList.GetDescriptionText(item2, isGrid), null, item2));
			}
		}

		private void EntityListItemClicked(MyGuiControlListbox myGuiControlListbox)
		{
			if (myGuiControlListbox.SelectedItems.Count > 0)
			{
				MyEntityList.MyEntityListInfoItem myEntityListInfoItem = (MyEntityList.MyEntityListInfoItem)myGuiControlListbox.SelectedItems[myGuiControlListbox.SelectedItems.Count - 1].UserData;
				m_attachCamera = myEntityListInfoItem.EntityId;
				if (!TryAttachCamera(myEntityListInfoItem.EntityId))
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, myEntityListInfoItem.Position + Vector3.One * 50f);
				}
				if (m_attachCamera != 0L)
				{
					UpdateRemoveAndDepowerButton(this, m_attachCamera, disableOverride: true);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AskIsValidForEdit_Server, m_attachCamera);
				}
			}
		}

		[Event(null, 1597)]
		[Reliable]
		[Server]
		private static void AskIsValidForEdit_Server(long entityId)
		{
			bool flag = true;
			if (MySession.Static != null && MySession.Static.Factions.GetStationByGridId(entityId) != null)
			{
				flag = false;
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				AskIsValidForEdit_Reponse(entityId, flag);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AskIsValidForEdit_Reponse, entityId, flag, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1614)]
		[Reliable]
		[Client]
		private static void AskIsValidForEdit_Reponse(long entityId, bool canEdit)
		{
			MyGuiScreenAdminMenu firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>();
			if (firstScreenOfType != null && firstScreenOfType.m_attachCamera == entityId)
			{
				firstScreenOfType.m_attachIsNpcStation = !canEdit;
				UpdateRemoveAndDepowerButton(firstScreenOfType, firstScreenOfType.m_attachCamera);
			}
		}

		private void TimeDeltaChanged(MyGuiControlSlider slider)
		{
			MyTimeOfDayHelper.UpdateTimeOfDay(slider.Value);
			m_timeDeltaValue.Text = $"{slider.Value:0.00}";
		}

		public void ValueChanged(MyEntityList.MyEntityTypeEnum myEntityTypeEnum)
		{
			m_selectedType = myEntityTypeEnum;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityListRequest, myEntityTypeEnum);
		}

		public static void RequestEntityList(MyEntityList.MyEntityTypeEnum myEntityTypeEnum)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityListRequest, myEntityTypeEnum);
		}

		private void OnModeComboSelect()
		{
			if (m_currentPage == MyPageEnum.TrashRemoval && m_unsavedTrashSettings)
			{
				if (m_currentPage != (MyPageEnum)m_modeCombo.GetSelectedKey())
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: FinishTrashUnsavedTabChange, messageText: MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_UnsavedTrash)));
				}
			}
			else
			{
				NewTabSelected();
			}
		}

		private void NewTabSelected()
		{
			m_currentPage = (MyPageEnum)m_modeCombo.GetSelectedKey();
			RecreateControls(constructor: false);
		}

		private void FinishTrashUnsavedTabChange(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				StoreTrashSettings_RealToTmp();
				NewTabSelected();
			}
			else
			{
				m_modeCombo.SelectItemByKey((long)m_currentPage);
			}
		}

		private void StoreTrashSettings_RealToTmp()
		{
			m_newSettings.flags = MySession.Static.Settings.TrashFlags;
			m_newSettings.enable = MySession.Static.Settings.TrashRemovalEnabled;
			m_newSettings.blockCount = MySession.Static.Settings.BlockCountThreshold;
			m_newSettings.playerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
			m_newSettings.gridCount = MySession.Static.Settings.OptimalGridCount;
			m_newSettings.playerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
			m_newSettings.characterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
			m_newSettings.stopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
			m_newSettings.removeOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
			m_newSettings.voxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
			m_newSettings.voxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
			m_newSettings.voxelAge = MySession.Static.Settings.VoxelAgeThreshold;
			m_newSettings.voxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
			m_newSettings.afkTimeout = MySession.Static.Settings.AFKTimeountMin;
			m_unsavedTrashSettings = false;
		}

		private void StoreTrashSettings_TmpToReal()
		{
			MySession.Static.Settings.TrashFlags = m_newSettings.flags;
			MySession.Static.Settings.TrashRemovalEnabled = m_newSettings.enable;
			MySession.Static.Settings.BlockCountThreshold = m_newSettings.blockCount;
			MySession.Static.Settings.PlayerDistanceThreshold = m_newSettings.playerDistance;
			MySession.Static.Settings.OptimalGridCount = m_newSettings.gridCount;
			MySession.Static.Settings.PlayerInactivityThreshold = m_newSettings.playerInactivity;
			MySession.Static.Settings.PlayerCharacterRemovalThreshold = m_newSettings.characterRemovalThreshold;
			MySession.Static.Settings.StopGridsPeriodMin = m_newSettings.stopGridsPeriod;
			MySession.Static.Settings.RemoveOldIdentitiesH = m_newSettings.removeOldIdentities;
			MySession.Static.Settings.VoxelPlayerDistanceThreshold = m_newSettings.voxelDistanceFromPlayer;
			MySession.Static.Settings.VoxelGridDistanceThreshold = m_newSettings.voxelDistanceFromGrid;
			MySession.Static.Settings.VoxelAgeThreshold = m_newSettings.voxelAge;
			MySession.Static.Settings.VoxelTrashRemovalEnabled = m_newSettings.voxelEnable;
			MySession.Static.Settings.AFKTimeountMin = m_newSettings.afkTimeout;
		}

		private void OnSubmitButtonClicked(MyGuiControlButton obj)
		{
			CheckAndStoreTrashTextboxChanges();
			if (MySession.Static.Settings.OptimalGridCount == 0 && MySession.Static.Settings.OptimalGridCount != m_newSettings.gridCount)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: FinishTrashSetting, messageText: MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_GridCountWarning)));
			}
			else
			{
				FinishTrashSetting(MyGuiScreenMessageBox.ResultEnum.YES);
			}
		}

		private bool CheckAndStoreTrashTextboxChanges()
		{
			MyTabControlEnum key = (MyTabControlEnum)m_trashRemovalCombo.GetSelectedKey();
			if (!m_tabs.TryGetValue(key, out MyTabContainer _))
			{
				return false;
			}
			m_unsavedTrashSettings |= m_tabs[key].GetSettings(ref m_newSettings);
			return m_unsavedTrashSettings;
		}

		private void FinishTrashSetting(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				StoreTrashSettings_TmpToReal();
				m_unsavedTrashSettings = false;
				MySession.Static.GetComponent<MySessionComponentTrash>()?.SetPlayerAFKTimeout(m_newSettings.afkTimeout);
				RecalcTrash();
				RecreateControls(constructor: false);
			}
		}

		private void OnCancelButtonClicked(MyGuiControlButton obj)
		{
			StoreTrashSettings_RealToTmp();
			RecreateControls(constructor: false);
			m_unsavedTrashSettings = false;
		}

		private void OnCycleClicked(bool reset, bool forward)
		{
			if (m_order != MyEntityCyclingOrder.Gps)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CycleRequest_Implementation, m_order, reset, forward, m_metricValue, m_entityId, m_cyclingOptions);
			}
			else
			{
				CircleGps(reset, forward);
			}
		}

		private void OnPlayerControl(MyGuiControlButton obj)
		{
			m_attachCamera = 0L;
			m_attachIsNpcStation = false;
			MySessionComponentAnimationSystem.Static.EntitySelectedForDebug = null;
			MyGuiScreenGamePlay.SetCameraController();
		}

		private void OnTeleportButton(MyGuiControlButton obj)
		{
			if (MySession.Static.CameraController != MySession.Static.LocalCharacter)
			{
				MyMultiplayer.TeleportControlledEntity(MySpectatorCameraController.Static.Position);
			}
		}

		private void OnRefreshButton(MyGuiControlButton obj)
		{
			RecreateControls(constructor: true);
		}

		private void OnRemoveOwnerButton(MyGuiControlButton obj)
		{
			HashSet<long> hashSet = new HashSet<long>();
			List<long> list = new List<long>();
			foreach (MyGuiControlListbox.Item selectedItem in m_entityListbox.SelectedItems)
			{
				long owner = ((MyEntityList.MyEntityListInfoItem)selectedItem.UserData).Owner;
				MyPlayer.PlayerId result;
				MyPlayer player;
				if (owner == 0L)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("No owner!"), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				else if (MySession.Static != null && MySession.Static.ControlledEntity != null && owner == MySession.Static.ControlledEntity.ControllerInfo.Controller.Player.Identity.IdentityId)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Cannot remove yourself!"), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				else if (MySession.Static.Players.TryGetPlayerId(owner, out result) && MySession.Static.Players.TryGetPlayerById(result, out player) && MySession.Static.Players.GetOnlinePlayers().Contains(player))
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Cannot remove online player " + player.DisplayName + ", kick him first!"), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				else
				{
					hashSet.Add(owner);
				}
			}
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			foreach (MyGuiControlListbox.Item item in m_entityListbox.Items)
			{
				if (hashSet.Contains(((MyEntityList.MyEntityListInfoItem)item.UserData).Owner))
				{
					list2.Add(item);
					list.Add(((MyEntityList.MyEntityListInfoItem)item.UserData).EntityId);
				}
			}
			m_entityListbox.SelectedItems.Clear();
			foreach (MyGuiControlListbox.Item item2 in list2)
			{
				m_entityListbox.Items.Remove(item2);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveOwner_Implementation, hashSet.ToList(), list);
		}

		protected void OnOrderChanged(MyEntityCyclingOrder obj)
		{
			m_order = obj;
			UpdateSmallLargeGridSelection();
			UpdateCyclingAndDepower();
			OnCycleClicked(reset: true, forward: true);
		}

		private bool ValidCharacter(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null && Sync.Players.TryGetPlayerId(myCharacter.ControllerInfo.ControllingIdentityId, out MyPlayer.PlayerId result) && Sync.Players.GetPlayerById(result) != null)
				{
					MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_RemoveCharacterNotification)));
					return false;
				}
			}
			return true;
		}

		private void OnEntityListActionClicked(MyEntityList.EntityListAction action)
		{
			List<long> list = new List<long>();
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			foreach (MyGuiControlListbox.Item selectedItem in m_entityListbox.SelectedItems)
			{
				if (!ValidCharacter(((MyEntityList.MyEntityListInfoItem)selectedItem.UserData).EntityId))
				{
					return;
				}
				list.Add(((MyEntityList.MyEntityListInfoItem)selectedItem.UserData).EntityId);
				list2.Add(selectedItem);
			}
			if (action == MyEntityList.EntityListAction.Remove)
			{
				m_entityListbox.SelectedItems.Clear();
				foreach (MyGuiControlListbox.Item item in list2)
				{
					m_entityListbox.Items.Remove(item);
				}
				m_entityListbox.ScrollToolbarToTop();
				foreach (long item2 in list)
				{
					if (MyEntities.TryGetEntityById(item2, out MyEntity entity))
					{
						MyVoxelBase myVoxelBase = entity as MyVoxelBase;
						if (myVoxelBase != null && !myVoxelBase.SyncFlag)
						{
							myVoxelBase.Close();
						}
					}
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ProceedEntitiesAction_Implementation, list, action);
		}

		private void OnEntityOperationClicked(MyEntityList.EntityListAction action)
		{
			if (m_attachCamera == 0L || !ValidCharacter(m_attachCamera))
			{
				return;
			}
			if (MyEntities.TryGetEntityById(m_attachCamera, out MyEntity entity))
			{
				MyVoxelBase myVoxelBase = entity as MyVoxelBase;
				if (myVoxelBase != null)
				{
					MyEntities.SendCloseRequest(myVoxelBase);
					return;
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ProceedEntity_Implementation, m_attachCamera, action);
		}

		private void RaiseAdminSettingsChanged()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AdminSettingsChanged, MySession.Static.AdminSettings, Sync.MyId);
		}

		private void OnReplicateEverything(MyGuiControlButton button)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReplicateEverything_Implementation);
		}

		private void OnEnableAdminModeChanged(MyGuiControlCheckbox checkbox)
		{
			MySession.Static.EnableCreativeTools(Sync.MyId, checkbox.IsChecked);
		}

		private void OnInvulnerableChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.Invulnerable;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.Invulnerable;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnUntargetableChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.Untargetable;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.Untargetable;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnKeepOwnershipChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnIgnoreSafeZonesChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.IgnoreSafeZones;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.IgnoreSafeZones;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnIgnorePcuChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.IgnorePcu;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.IgnorePcu;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnUseTerminalsChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.UseTerminals;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.UseTerminals;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnShowPlayersChanged(MyGuiControlCheckbox checkbox)
		{
			if (checkbox.IsChecked)
			{
				MySession.Static.AdminSettings |= AdminSettingsEnum.ShowPlayers;
			}
			else
			{
				MySession.Static.AdminSettings &= ~AdminSettingsEnum.ShowPlayers;
			}
			RaiseAdminSettingsChanged();
		}

		private void OnSmallGridChanged(MyGuiControlCheckbox checkbox)
		{
			m_cyclingOptions.OnlySmallGrids = checkbox.IsChecked;
			if (m_cyclingOptions.OnlySmallGrids && m_onlyLargeGridsCheckbox != null)
			{
				m_onlyLargeGridsCheckbox.IsChecked = false;
			}
		}

		private void OnLargeGridChanged(MyGuiControlCheckbox checkbox)
		{
			m_cyclingOptions.OnlyLargeGrids = checkbox.IsChecked;
			if (m_cyclingOptions.OnlyLargeGrids)
			{
				m_onlySmallGridsCheckbox.IsChecked = false;
			}
		}

		[Event(null, 2081)]
		[Reliable]
		[Server]
		private static void EntityListRequest(MyEntityList.MyEntityTypeEnum selectedType)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			List<MyEntityList.MyEntityListInfoItem> entityList = MyEntityList.GetEntityList(selectedType);
			if (!MyEventContext.Current.IsLocallyInvoked)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityListResponse, entityList, MyEventContext.Current.Sender);
			}
			else
			{
				EntityListResponse(entityList);
			}
		}

		[Event(null, 2098)]
		[Reliable]
		[Server]
		private static void CycleRequest_Implementation(MyEntityCyclingOrder order, bool reset, bool findLarger, float metricValue, long currentEntityId, CyclingOptions options)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			if (reset)
			{
				metricValue = float.MinValue;
				currentEntityId = 0L;
				findLarger = false;
			}
			MyEntityCycling.FindNext(order, ref metricValue, ref currentEntityId, findLarger, options);
			Vector3D vector3D = MyEntities.GetEntityByIdOrDefault(currentEntityId)?.WorldMatrix.Translation ?? Vector3D.Zero;
			bool flag = false;
			if (MySession.Static != null)
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component != null && component.IsGridStation(currentEntityId))
				{
					flag = true;
				}
			}
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				Cycle_Implementation(metricValue, currentEntityId, vector3D, flag);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Cycle_Implementation, metricValue, currentEntityId, vector3D, flag, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 2137)]
		[Server]
		[Reliable]
		private static void RemoveOwner_Implementation(List<long> owners, List<long> entityIds)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			foreach (long entityId in entityIds)
			{
				if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
				{
					MyEntityList.ProceedEntityAction(entity, MyEntityList.EntityListAction.Remove);
				}
			}
			foreach (long owner in owners)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(owner);
				if (myIdentity.Character != null)
				{
					myIdentity.Character.Close();
				}
				foreach (long savedCharacter in myIdentity.SavedCharacters)
				{
					if (MyEntities.TryGetEntityById(savedCharacter, out MyCharacter entity2, allowClosed: true) && (!entity2.Closed || entity2.MarkedForClose))
					{
						entity2.Close();
					}
				}
				if (myIdentity != null && myIdentity.BlockLimits.BlocksBuilt == 0)
				{
					MySession.Static.Players.RemoveIdentity(owner);
				}
			}
		}

		[Event(null, 2180)]
		[Server]
		[Reliable]
		private static void ProceedEntitiesAction_Implementation(List<long> entityIds, MyEntityList.EntityListAction action)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				foreach (long entityId in entityIds)
				{
					if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
					{
						MyEntityList.ProceedEntityAction(entity, action);
					}
				}
			}
		}

		[Event(null, 2197)]
		[Reliable]
		[Server]
		private static void UploadSettingsToServer(AdminSettings settings)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			if (MySession.Static.Settings.TrashFlags != settings.flags)
			{
				MyLog.Default.Info($"Trash flags changed by {MyEventContext.Current.Sender.Value} to {settings.flags}");
			}
			MySession.Static.Settings.TrashFlags = settings.flags;
			if (MySession.Static.Settings.TrashRemovalEnabled != settings.enable)
			{
				MyLog.Default.Info($"Trash Trash Removal changed by {MyEventContext.Current.Sender.Value} to {settings.enable}");
			}
			MySession.Static.Settings.TrashRemovalEnabled = settings.enable;
			if (MySession.Static.Settings.BlockCountThreshold != settings.blockCount)
			{
				MyLog.Default.Info($"Trash Block Count changed by {MyEventContext.Current.Sender.Value} to {settings.blockCount}");
			}
			MySession.Static.Settings.BlockCountThreshold = settings.blockCount;
			if (MySession.Static.Settings.PlayerDistanceThreshold != settings.playerDistance)
			{
				MyLog.Default.Info($"Trash Player Distance Treshold changed by {MyEventContext.Current.Sender.Value} to {settings.playerDistance}");
			}
			MySession.Static.Settings.PlayerDistanceThreshold = settings.playerDistance;
			if (MySession.Static.Settings.OptimalGridCount != settings.gridCount)
			{
				MyLog.Default.Info($"Trash Optimal Grid Count changed by {MyEventContext.Current.Sender.Value} to {settings.gridCount}");
			}
			MySession.Static.Settings.OptimalGridCount = settings.gridCount;
			if (MySession.Static.Settings.PlayerInactivityThreshold != settings.playerInactivity)
			{
				MyLog.Default.Info($"Trash Player Inactivity Threshold changed by {MyEventContext.Current.Sender.Value} to {settings.playerInactivity}");
			}
			MySession.Static.Settings.PlayerInactivityThreshold = settings.playerInactivity;
			if (MySession.Static.Settings.PlayerCharacterRemovalThreshold != settings.characterRemovalThreshold)
			{
				MyLog.Default.Info($"Trash Player Character Removal Threshold changed by {MyEventContext.Current.Sender.Value} to {settings.characterRemovalThreshold}");
			}
			MySession.Static.Settings.PlayerCharacterRemovalThreshold = settings.characterRemovalThreshold;
			if (MySession.Static.Settings.StopGridsPeriodMin != settings.stopGridsPeriod)
			{
				MyLog.Default.Info($"Trash Stop Grids Period changed by {MyEventContext.Current.Sender.Value} to {settings.stopGridsPeriod}");
			}
			MySession.Static.Settings.StopGridsPeriodMin = settings.stopGridsPeriod;
			if (MySession.Static.Settings.VoxelPlayerDistanceThreshold != settings.voxelDistanceFromPlayer)
			{
				MyLog.Default.Info($"Trash Voxel Player Distance Threshold changed by {MyEventContext.Current.Sender.Value} to {settings.voxelDistanceFromPlayer}");
			}
			MySession.Static.Settings.VoxelPlayerDistanceThreshold = settings.voxelDistanceFromPlayer;
			if (MySession.Static.Settings.VoxelGridDistanceThreshold != settings.voxelDistanceFromGrid)
			{
				MyLog.Default.Info($"Trash Voxel Grid Distance Threshold changed by {MyEventContext.Current.Sender.Value} to {settings.voxelDistanceFromGrid}");
			}
			MySession.Static.Settings.VoxelGridDistanceThreshold = settings.voxelDistanceFromGrid;
			if (MySession.Static.Settings.VoxelAgeThreshold != settings.voxelAge)
			{
				MyLog.Default.Info($"Trash Voxel Age Threshold changed by {MyEventContext.Current.Sender.Value} to {settings.voxelAge}");
			}
			MySession.Static.Settings.VoxelAgeThreshold = settings.voxelAge;
			if (MySession.Static.Settings.VoxelTrashRemovalEnabled != settings.voxelEnable)
			{
				MyLog.Default.Info($"Trash Voxel Trash Removal Enabled changed by {MyEventContext.Current.Sender.Value} to {settings.voxelEnable}");
			}
			MySession.Static.Settings.VoxelTrashRemovalEnabled = settings.voxelEnable;
			if (MySession.Static.Settings.RemoveOldIdentitiesH != settings.removeOldIdentities)
			{
				MyLog.Default.Info($"Trash Identities removal time changed by {MyEventContext.Current.Sender.Value} to {settings.removeOldIdentities}");
			}
			MySession.Static.Settings.RemoveOldIdentitiesH = settings.removeOldIdentities;
		}

		[Event(null, 2259)]
		[Reliable]
		[Server]
		private static void ProceedEntity_Implementation(long entityId, MyEntityList.EntityListAction action)
		{
			MyEntity entity;
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MyEntities.TryGetEntityById(entityId, out entity))
			{
				MyEntityList.ProceedEntityAction(entity, action);
			}
		}

		[Event(null, 2273)]
		[Reliable]
		[Server]
		private static void ReplicateEverything_Implementation()
		{
			if (!MyEventContext.Current.IsLocallyInvoked)
			{
				if (!MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
				{
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				}
				else
				{
					((MyReplicationServer)MyMultiplayer.Static.ReplicationLayer).ForceEverything(new Endpoint(MyEventContext.Current.Sender, 0));
				}
			}
		}

		[Event(null, 2289)]
		[Reliable]
		[Server]
		private static void AdminSettingsChanged(AdminSettingsEnum settings, ulong steamId)
		{
			if (MySession.Static.OnlineMode != 0 && (((settings & AdminSettingsEnum.AdminOnly) > AdminSettingsEnum.None && !MySession.Static.IsUserAdmin(steamId)) || !MySession.Static.IsUserModerator(steamId)))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MySession.Static.RemoteAdminSettings[steamId] = settings;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AdminSettingsChangedClient, settings, steamId);
		}

		[Event(null, 2305)]
		[Reliable]
		[BroadcastExcept]
		private static void AdminSettingsChangedClient(AdminSettingsEnum settings, ulong steamId)
		{
			MySession.Static.RemoteAdminSettings[steamId] = settings;
		}

		[Event(null, 2315)]
		[Reliable]
		[Client]
		private static void EntityListResponse(List<MyEntityList.MyEntityListInfoItem> entities)
		{
			MyGuiScreenSafeZoneFilter firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenSafeZoneFilter>();
			if (firstScreenOfType != null)
			{
				MyGuiControlListbox entityListbox = firstScreenOfType.m_entityListbox;
				entityListbox.Items.Clear();
				MyEntityList.SortEntityList(MyEntityList.MyEntitySortOrder.DisplayName, ref entities, m_invertOrder);
				foreach (MyEntityList.MyEntityListInfoItem entity in entities)
				{
					if (!firstScreenOfType.m_selectedSafeZone.Entities.Contains(entity.EntityId))
					{
						StringBuilder formattedDisplayName = MyEntityList.GetFormattedDisplayName(MyEntityList.MyEntitySortOrder.DisplayName, entity, isGrid: true);
						entityListbox.Items.Add(new MyGuiControlListbox.Item(formattedDisplayName, null, null, entity.EntityId));
					}
				}
				return;
			}
			MyGuiScreenAdminMenu @static = m_static;
			if (@static != null)
			{
				MyGuiControlListbox entityListbox2 = @static.m_entityListbox;
				entityListbox2.Items.Clear();
				MyEntityList.SortEntityList(@static.m_selectedSort, ref entities, m_invertOrder);
				bool isGrid = @static.m_selectedType == MyEntityList.MyEntityTypeEnum.Grids || @static.m_selectedType == MyEntityList.MyEntityTypeEnum.LargeGrids || @static.m_selectedType == MyEntityList.MyEntityTypeEnum.SmallGrids;
				foreach (MyEntityList.MyEntityListInfoItem entity2 in entities)
				{
					StringBuilder formattedDisplayName2 = MyEntityList.GetFormattedDisplayName(@static.m_selectedSort, entity2, isGrid);
					entityListbox2.Items.Add(new MyGuiControlListbox.Item(formattedDisplayName2, MyEntityList.GetDescriptionText(entity2, isGrid), null, entity2));
				}
			}
		}

		[Event(null, 2357)]
		[Reliable]
		[Client]
		private static void Cycle_Implementation(float newMetricValue, long newEntityId, Vector3D position, bool isNpcStation)
		{
			m_metricValue = newMetricValue;
			m_entityId = newEntityId;
			if (m_entityId != 0L && !TryAttachCamera(m_entityId))
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, position + Vector3.One * 50f);
			}
			MyGuiScreenAdminMenu firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>();
			if (firstScreenOfType != null)
			{
				firstScreenOfType.m_attachCamera = m_entityId;
				firstScreenOfType.m_attachIsNpcStation = isNpcStation;
				UpdateRemoveAndDepowerButton(firstScreenOfType, m_entityId);
				firstScreenOfType.m_labelCurrentIndex.TextToDraw.Clear().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), (m_entityId == 0L) ? "-" : m_metricValue.ToString());
			}
		}

		[Event(null, 2393)]
		[Reliable]
		[Client]
		private static void DownloadSettingFromServer(AdminSettings settings)
		{
			MySession.Static.Settings.TrashFlags = settings.flags;
			MySession.Static.Settings.TrashRemovalEnabled = settings.enable;
			MySession.Static.Settings.BlockCountThreshold = settings.blockCount;
			MySession.Static.Settings.PlayerDistanceThreshold = settings.playerDistance;
			MySession.Static.Settings.OptimalGridCount = settings.gridCount;
			MySession.Static.Settings.PlayerInactivityThreshold = settings.playerInactivity;
			MySession.Static.Settings.PlayerCharacterRemovalThreshold = settings.characterRemovalThreshold;
			MySession.Static.Settings.StopGridsPeriodMin = settings.stopGridsPeriod;
			MySession.Static.Settings.RemoveOldIdentitiesH = settings.removeOldIdentities;
			MySession.Static.Settings.VoxelPlayerDistanceThreshold = settings.voxelDistanceFromPlayer;
			MySession.Static.Settings.VoxelGridDistanceThreshold = settings.voxelDistanceFromGrid;
			MySession.Static.Settings.VoxelAgeThreshold = settings.voxelAge;
			MySession.Static.Settings.VoxelTrashRemovalEnabled = settings.voxelEnable;
			MySession.Static.AdminSettings = settings.AdminSettingsFlags;
			if (m_static != null)
			{
				m_static.CreateScreen();
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (m_attachCamera != 0L)
			{
				TryAttachCamera(m_attachCamera);
				UpdateRemoveAndDepowerButton(this, m_attachCamera);
			}
			return base.Update(hasFocus);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenAdminMenu";
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool Draw()
		{
			if (base.Draw())
			{
				return true;
			}
			return false;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.A) && base.FocusedControl == m_entityListbox)
			{
				m_entityListbox.SelectedItems.Clear();
				m_entityListbox.SelectedItems.AddRange(m_entityListbox.Items);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MAIN_MENU) || (m_defaultJoystickCancelUse && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL)) || MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsNewKeyPressed(MyKeys.F10))
			{
				ExitButtonPressed();
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NONE))
			{
				SelectNextCharacter();
			}
		}

		public void ExitButtonPressed()
		{
			if (m_currentPage == MyPageEnum.TrashRemoval)
			{
				CheckAndStoreTrashTextboxChanges();
				if (m_unsavedTrashSettings)
				{
					if (!m_unsavedTrashExitBoxIsOpened)
					{
						m_unsavedTrashExitBoxIsOpened = true;
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: FinishTrashUnsavedExiting, messageText: MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_UnsavedTrash)));
					}
				}
				else
				{
					CloseScreen();
				}
			}
			else
			{
				CloseScreen();
			}
		}

		private void FinishTrashUnsavedExiting(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				StoreTrashSettings_RealToTmp();
				CloseScreen();
			}
			m_unsavedTrashExitBoxIsOpened = false;
		}

		public override bool CloseScreen()
		{
			m_static = null;
			MySessionComponentSafeZones.OnAddSafeZone -= MySafeZones_OnAddSafeZone;
			MySessionComponentSafeZones.OnRemoveSafeZone -= MySafeZones_OnRemoveSafeZone;
			return base.CloseScreen();
		}

		protected virtual void CreateSelectionCombo()
		{
			AddCombo(m_order, OnOrderChanged, enabled: true, 10, null, m_labelColor);
		}

		private MyGuiControlButton CreateDebugButton(float usableWidth, MyStringId text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, bool increaseSpacing = true, bool addToControls = true)
		{
			MyGuiControlButton myGuiControlButton = AddButton(MyTexts.Get(text), onClick, null, null, null, increaseSpacing, addToControls);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = m_scale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2((0f - HIDDEN_PART_RIGHT) / 2f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		private void AddSeparator()
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.Size = new Vector2(1f, 0.01f);
			myGuiControlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlSeparatorList.AddHorizontal(Vector2.Zero, 1f);
			Controls.Add(myGuiControlSeparatorList);
		}

		private MyGuiControlLabel CreateSliderWithDescription(MyGuiControlList list, float usableWidth, float min, float max, string description, ref MyGuiControlSlider slider)
		{
			MyGuiControlLabel control = AddLabel(description, Vector4.One, m_scale);
			Controls.Remove(control);
			list.Controls.Add(control);
			CreateSlider(list, usableWidth, min, max, ref slider);
			MyGuiControlLabel myGuiControlLabel = AddLabel("", Vector4.One, m_scale);
			Controls.Remove(myGuiControlLabel);
			list.Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		private void CreateSlider(MyGuiControlList list, float usableWidth, float min, float max, ref MyGuiControlSlider slider)
		{
			Vector2? position = m_currentPosition;
			float width = 400f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			slider = new MyGuiControlSlider(position, min, max, width, null, null, string.Empty, 4, 0.75f * m_scale, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			slider.DebugScale = m_sliderDebugScale;
			slider.ColorMask = Color.White.ToVector4();
			list.Controls.Add(slider);
		}

		private void RecreateGlobalSafeZoneControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.03f;
			m_damageCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage)
			};
			m_damageGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_damageCheckboxGlobalLabel);
			Controls.Add(m_damageGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_shootingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting)
			};
			m_shootingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_shootingCheckboxGlobalLabel);
			Controls.Add(m_shootingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_drillingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling)
			};
			m_drillingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_drillingCheckboxGlobalLabel);
			Controls.Add(m_drillingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_weldingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding)
			};
			m_weldingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_weldingCheckboxGlobalLabel);
			Controls.Add(m_weldingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_grindingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding)
			};
			m_grindingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_grindingCheckboxGlobalLabel);
			Controls.Add(m_grindingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_buildingCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding)
			};
			m_buildingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_buildingCheckboxGlobalLabel);
			Controls.Add(m_buildingGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_voxelHandCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands)
			};
			m_voxelHandGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_voxelHandCheckboxGlobalLabel);
			Controls.Add(m_voxelHandGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_landingGearCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear)
			};
			m_landingGearGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_landingGearGlobalCheckbox.UserData = MySafeZoneAction.LandingGearLock;
			Controls.Add(m_landingGearCheckboxGlobalLabel);
			Controls.Add(m_landingGearGlobalCheckbox);
			m_currentPosition.Y += 0.045f;
			m_convertToStationCheckboxGlobalLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation)
			};
			m_convertToStationGlobalCheckbox = new MyGuiControlCheckbox(new Vector2(m_currentPosition.X + 0.293f, m_currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_convertToStationGlobalCheckbox.UserData = MySafeZoneAction.ConvertToStation;
			Controls.Add(m_convertToStationCheckboxGlobalLabel);
			Controls.Add(m_convertToStationGlobalCheckbox);
			UpdateSelectedGlobalData();
			m_voxelHandGlobalCheckbox.IsCheckedChanged = VoxelHandCheckGlobalChanged;
			m_buildingGlobalCheckbox.IsCheckedChanged = BuildingCheckGlobalChanged;
			m_grindingGlobalCheckbox.IsCheckedChanged = GrindingCheckGlobalChanged;
			m_weldingGlobalCheckbox.IsCheckedChanged = WeldingCheckGlobalChanged;
			m_drillingGlobalCheckbox.IsCheckedChanged = DrillingCheckGlobalChanged;
			m_shootingGlobalCheckbox.IsCheckedChanged = ShootingCheckGlobalChanged;
			m_damageGlobalCheckbox.IsCheckedChanged = DamageCheckGlobalChanged;
			m_landingGearGlobalCheckbox.IsCheckedChanged = OnSettingCheckGlobalChanged;
			m_convertToStationGlobalCheckbox.IsCheckedChanged = OnSettingCheckGlobalChanged;
		}

		private void UpdateSelectedGlobalData()
		{
			m_damageGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Damage) > (MySafeZoneAction)0);
			m_shootingGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Shooting) > (MySafeZoneAction)0);
			m_drillingGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Drilling) > (MySafeZoneAction)0);
			m_weldingGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Welding) > (MySafeZoneAction)0);
			m_grindingGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Grinding) > (MySafeZoneAction)0);
			m_voxelHandGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.VoxelHand) > (MySafeZoneAction)0);
			m_buildingGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Building) > (MySafeZoneAction)0);
			m_landingGearGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.LandingGearLock) > (MySafeZoneAction)0);
			m_convertToStationGlobalCheckbox.IsChecked = ((MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.ConvertToStation) > (MySafeZoneAction)0);
		}

		private void DamageCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Damage;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Damage;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void ShootingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Shooting;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Shooting;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void DrillingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Drilling;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Drilling;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void WeldingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Welding;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Welding;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void GrindingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Grinding;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Grinding;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void VoxelHandCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.VoxelHand;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.VoxelHand;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void BuildingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Building;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Building;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void OnSettingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
		{
			if (checkBox.IsChecked)
			{
				MySessionComponentSafeZones.AllowedActions |= (MySafeZoneAction)checkBox.UserData;
			}
			else
			{
				MySessionComponentSafeZones.AllowedActions &= ~(MySafeZoneAction)checkBox.UserData;
			}
			MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
		}

		private void RecreateReplayToolControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.03f;
			if (!MySession.Static.IsServer)
			{
				MyGuiControlButton myGuiControlButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, ReloadWorld, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorldClient_Tooltip);
				myGuiControlButton.Enabled = false;
				myGuiControlButton.ShowTooltipWhenDisabled = true;
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, ReloadWorld, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld_Tooltip);
			}
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ManageCharacters)
			};
			Controls.Add(control);
			m_currentPosition.Y += 0.03f;
			Vector2 currentPosition = m_currentPosition;
			m_buttonXOffset -= 0.075f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_AddCharacter, AddCharacter, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_AddCharacter_Tooltip);
			m_currentPosition.Y = currentPosition.Y;
			m_buttonXOffset += 0.15f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RemoveCharacter, TryRemoveCurrentCharacter, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RemoveCharacter_Tooltip);
			m_buttonXOffset = 0f;
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ChangeAsset, ChangeSkin, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ChangeAsset_Tooltip);
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ManageRecordings)
			};
			Controls.Add(control2);
			m_currentPosition.Y += 0.03f;
			if (MySessionComponentReplay.Static.IsReplaying)
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopReplay, OnReplayButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopReplay_Tooltip);
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Replay, OnReplayButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Replay_Tooltip);
			}
			if (MySessionComponentReplay.Static.IsRecording)
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopRecording, OnRecordButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopRecording_Tooltip);
			}
			else
			{
				CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RecordAndReplay, OnRecordButtonPressed, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RecordAndReplay_Tooltip);
			}
			CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings, DeleteRecordings, enabled: true, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings_Tooltip);
			m_currentPosition.Y += 0.02f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				Size = new Vector2(0.7f, 0.6f),
				Font = "Blue",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlMultilineText.Text = MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial);
			Controls.Add(myGuiControlMultilineText);
		}

		private void ReloadWorld(MyGuiControlButton obj)
		{
			MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
		}

		private void AddCharacter(MyGuiControlButton obj)
		{
			MyCharacterInputComponent.SpawnCharacter();
		}

		private void TryRemoveCurrentCharacter(MyGuiControlButton obj)
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				SelectNextCharacter();
				if (MySession.Static.ControlledEntity != controlledEntity)
				{
					controlledEntity.Entity.Close();
				}
			}
		}

		private void ChangeSkin(MyGuiControlButton obj)
		{
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenAssetModifier(MySession.Static.LocalCharacter));
		}

		private void SelectNextCharacter()
		{
			MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
			if (cameraControllerEnum == MyCameraControllerEnum.Entity || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
			{
				if (MySession.Static.VirtualClients.Any() && Sync.Clients.LocalClient != null)
				{
					MyPlayer myPlayer = MySession.Static.VirtualClients.GetNextControlledPlayer(MySession.Static.LocalHumanPlayer) ?? Sync.Clients.LocalClient.GetPlayer(0);
					if (myPlayer != null)
					{
						Sync.Clients.LocalClient.ControlledPlayerSerialId = myPlayer.Id.SerialId;
					}
				}
				else
				{
					long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
					List<MyEntity> list = new List<MyEntity>();
					foreach (MyEntity entity in MyEntities.GetEntities())
					{
						MyCharacter myCharacter = entity as MyCharacter;
						if (myCharacter != null && !myCharacter.IsDead && myCharacter.GetIdentity() != null && myCharacter.GetIdentity().IdentityId == identityId)
						{
							list.Add(entity);
						}
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							foreach (MySlimBlock block in myCubeGrid.GetBlocks())
							{
								MyCockpit myCockpit = block.FatBlock as MyCockpit;
								if (myCockpit != null && myCockpit.Pilot != null && myCockpit.Pilot.GetIdentity() != null && myCockpit.Pilot.GetIdentity().IdentityId == identityId)
								{
									list.Add(myCockpit);
								}
							}
						}
					}
					int num = list.IndexOf(MySession.Static.ControlledEntity.Entity);
					List<MyEntity> list2 = new List<MyEntity>();
					if (num + 1 < list.Count)
					{
						list2.AddRange(list.GetRange(num + 1, list.Count - num - 1));
					}
					if (num != -1)
					{
						list2.AddRange(list.GetRange(0, num + 1));
					}
					IMyControllableEntity myControllableEntity = null;
					for (int i = 0; i < list2.Count; i++)
					{
						if (list2[i] is IMyControllableEntity)
						{
							myControllableEntity = (list2[i] as IMyControllableEntity);
							break;
						}
					}
					if (MySession.Static.LocalHumanPlayer != null && myControllableEntity != null)
					{
						MySession.Static.LocalHumanPlayer.Controller.TakeControl(myControllableEntity);
						MyCharacter myCharacter2 = MySession.Static.ControlledEntity as MyCharacter;
						if (myCharacter2 == null && MySession.Static.ControlledEntity is MyCockpit)
						{
							myCharacter2 = (MySession.Static.ControlledEntity as MyCockpit).Pilot;
						}
						if (myCharacter2 != null)
						{
							MySession.Static.LocalHumanPlayer.Identity.ChangeCharacter(myCharacter2);
						}
					}
				}
			}
			if (!(MySession.Static.ControlledEntity is MyCharacter))
			{
				MySession.Static.GameFocusManager.Clear();
			}
		}

		private void OnReplayButtonPressed(MyGuiControlButton obj)
		{
			if (MySessionComponentReplay.Static == null)
			{
				return;
			}
			if (!MySessionComponentReplay.Static.IsReplaying)
			{
				if (MySessionComponentReplay.Static.HasRecordedData)
				{
					MySessionComponentReplay.Static.StartReplay();
					CloseScreen();
				}
			}
			else
			{
				MySessionComponentReplay.Static.StopReplay();
				RecreateControls(constructor: false);
			}
		}

		private void OnRecordButtonPressed(MyGuiControlButton obj)
		{
			if (MySessionComponentReplay.Static != null)
			{
				if (!MySessionComponentReplay.Static.IsRecording)
				{
					MySessionComponentReplay.Static.StartRecording();
					MySessionComponentReplay.Static.StartReplay();
					CloseScreen();
				}
				else
				{
					MySessionComponentReplay.Static.StopRecording();
					MySessionComponentReplay.Static.StopReplay();
					RecreateControls(constructor: false);
				}
			}
		}

		private void DeleteRecordings(MyGuiControlButton obj)
		{
			MySessionComponentReplay.Static.DeleteRecordings();
		}

		private void RecreateSafeZonesControls(ref Vector2 controlPadding, float separatorSize, float usableWidth)
		{
			m_recreateInProgress = true;
			m_currentPosition.Y += 0.015f;
			m_selectSafeZoneLabel = new MyGuiControlLabel
			{
				Position = new Vector2(m_currentPosition.X + 0.001f, m_currentPosition.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_SelectSafeZone)
			};
			Controls.Add(m_selectSafeZoneLabel);
			m_currentPosition.Y += 0.03f;
			m_safeZonesCombo = AddCombo();
			m_currentPosition.Y += 0.001f;
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = Vector2.Zero,
				Size = new Vector2(0.32f, 0.88f)
			};
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2((0f - m_size.Value.X) * 0.83f / 2f, m_currentPosition.Y), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			m_currentPosition.Y += 0.005f;
			m_optionsGroup = new MyGuiControlScrollablePanel(myGuiControlParent)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = m_currentPosition,
				Size = new Vector2(0.32f, 0.62f)
			};
			m_optionsGroup.ScrollbarVEnabled = true;
			m_optionsGroup.ScrollBarOffset = new Vector2(-0.01f, 0f);
			Controls.Add(m_optionsGroup);
			Vector2 vector = -myGuiControlParent.Size * 0.5f;
			m_selectZoneShapeLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_SelectZoneShape)
			};
			myGuiControlParent.Controls.Add(m_selectZoneShapeLabel);
			vector.Y += 0.03f;
			m_safeZonesTypeCombo = AddCombo(null, null, null, 10, addToControls: false, vector);
			vector.Y += m_safeZonesTypeCombo.Size.Y + 0.01f + Spacing;
			m_safeZonesTypeCombo.AddItem(0L, MyTexts.GetString(MySpaceTexts.SafeZone_Spherical));
			m_safeZonesTypeCombo.AddItem(1L, MyTexts.GetString(MySpaceTexts.SafeZone_Cubical));
			myGuiControlParent.Controls.Add(m_safeZonesTypeCombo);
			vector.Y += 0.001f;
			m_zoneRadiusLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ZoneRadius)
			};
			myGuiControlParent.Controls.Add(m_zoneRadiusLabel);
			m_zoneRadiusLabel.Visible = false;
			m_zoneRadiusValueLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.285f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Text = "1"
			};
			myGuiControlParent.Controls.Add(m_zoneRadiusValueLabel);
			vector.Y += 0.03f;
			m_radiusSlider = new MyGuiControlSlider(vector, 10f, 500f, 0.285f, 1f, null, null, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_radiusSlider.Visible = false;
			MyGuiControlSlider radiusSlider = m_radiusSlider;
			radiusSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(radiusSlider.ValueChanged, new Action<MyGuiControlSlider>(OnRadiusChange));
			myGuiControlParent.Controls.Add(m_radiusSlider);
			vector.Y -= 0.03f;
			m_selectAxisLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.SafeZone_CubeAxis)
			};
			myGuiControlParent.Controls.Add(m_selectAxisLabel);
			m_zoneSizeLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.09f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.Size)
			};
			myGuiControlParent.Controls.Add(m_zoneSizeLabel);
			vector.Y += 0.03f;
			m_safeZonesAxisCombo = AddCombo(null, null, null, 10, addToControls: false, vector);
			vector.Y += m_safeZonesAxisCombo.Size.Y + 0.01f + Spacing;
			m_safeZonesAxisCombo.Size = new Vector2(0.08f, 1f);
			m_safeZonesAxisCombo.ItemSelected += m_safeZonesAxisCombo_ItemSelected;
			m_safeZonesAxisCombo.AddItem(0L, MyZoneAxisTypeEnum.X.ToString());
			m_safeZonesAxisCombo.AddItem(1L, MyZoneAxisTypeEnum.Y.ToString());
			m_safeZonesAxisCombo.AddItem(2L, MyZoneAxisTypeEnum.Z.ToString());
			m_safeZonesAxisCombo.SelectItemByIndex(0);
			myGuiControlParent.Controls.Add(m_safeZonesAxisCombo);
			m_sizeSlider = new MyGuiControlSlider(vector + new Vector2(0.09f, -0.05f), 20f, 500f, 0.195f, 1f, null, null, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlSlider sizeSlider = m_sizeSlider;
			sizeSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sizeSlider.ValueChanged, new Action<MyGuiControlSlider>(OnSizeChange));
			myGuiControlParent.Controls.Add(m_sizeSlider);
			vector.Y += 0.018f;
			m_enabledCheckboxLabel = new MyGuiControlLabel
			{
				Position = vector + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ZoneEnabled)
			};
			m_enabledCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_enabledCheckbox.IsCheckedChanged = EnabledCheckedChanged;
			myGuiControlParent.Controls.Add(m_enabledCheckboxLabel);
			myGuiControlParent.Controls.Add(m_enabledCheckbox);
			vector.Y += 0.045f;
			m_damageCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage)
			};
			m_damageCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_damageCheckbox.IsCheckedChanged = DamageCheckChanged;
			myGuiControlParent.Controls.Add(m_damageCheckboxLabel);
			myGuiControlParent.Controls.Add(m_damageCheckbox);
			vector.Y += 0.045f;
			m_shootingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting)
			};
			m_shootingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_shootingCheckbox.IsCheckedChanged = ShootingCheckChanged;
			myGuiControlParent.Controls.Add(m_shootingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_shootingCheckbox);
			vector.Y += 0.045f;
			m_drillingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling)
			};
			m_drillingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_drillingCheckbox.IsCheckedChanged = DrillingCheckChanged;
			myGuiControlParent.Controls.Add(m_drillingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_drillingCheckbox);
			vector.Y += 0.045f;
			m_weldingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding)
			};
			m_weldingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_weldingCheckbox.IsCheckedChanged = WeldingCheckChanged;
			myGuiControlParent.Controls.Add(m_weldingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_weldingCheckbox);
			vector.Y += 0.045f;
			m_grindingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding)
			};
			m_grindingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_grindingCheckbox.IsCheckedChanged = GrindingCheckChanged;
			myGuiControlParent.Controls.Add(m_grindingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_grindingCheckbox);
			vector.Y += 0.045f;
			m_buildingCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding)
			};
			m_buildingCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_buildingCheckbox.IsCheckedChanged = BuildingCheckChanged;
			myGuiControlParent.Controls.Add(m_buildingCheckboxLabel);
			myGuiControlParent.Controls.Add(m_buildingCheckbox);
			vector.Y += 0.045f;
			m_voxelHandCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands)
			};
			m_voxelHandCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_voxelHandCheckbox.IsCheckedChanged = VoxelHandCheckChanged;
			myGuiControlParent.Controls.Add(m_voxelHandCheckboxLabel);
			myGuiControlParent.Controls.Add(m_voxelHandCheckbox);
			vector.Y += 0.045f;
			m_landingGearLockCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear)
			};
			m_landingGearLockCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_landingGearLockCheckbox.IsCheckedChanged = OnSettingsCheckChanged;
			m_landingGearLockCheckbox.UserData = MySafeZoneAction.LandingGearLock;
			myGuiControlParent.Controls.Add(m_landingGearLockCheckboxLabel);
			myGuiControlParent.Controls.Add(m_landingGearLockCheckbox);
			vector.Y += 0.045f;
			m_convertToStationCheckboxLabel = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation)
			};
			m_convertToStationCheckbox = new MyGuiControlCheckbox(new Vector2(vector.X + 0.293f, vector.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_convertToStationCheckbox.IsCheckedChanged = OnSettingsCheckChanged;
			m_convertToStationCheckbox.UserData = MySafeZoneAction.ConvertToStation;
			myGuiControlParent.Controls.Add(m_convertToStationCheckboxLabel);
			myGuiControlParent.Controls.Add(m_convertToStationCheckbox);
			vector.Y += 0.04f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(vector.X + 0.001f, vector.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ScreenAdmin_Safezone_TextureColorLabel)
			};
			myGuiControlParent.Controls.Add(control);
			vector.Y += 0.03f;
			m_textureCombo = AddCombo(null, null, null, 10, addToControls: false, vector);
			IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
			if (allDefinitions != null)
			{
				foreach (MySafeZoneTexturesDefinition item in allDefinitions)
				{
					m_textureCombo.AddItem((int)item.DisplayTextId, MyStringId.GetOrCompute(item.DisplayTextId.String));
				}
			}
			else
			{
				MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
			}
			myGuiControlParent.Controls.Add(m_textureCombo);
			vector.Y += 0.055f;
			m_colorSelector = new MyGuiControlColor(MyTexts.GetString(MySpaceTexts.ScreenAdmin_Safezone_ColorLabel), 1f, vector, Color.SkyBlue, Color.Red, MyCommonTexts.DialogAmount_SetValueCaption, placeSlidersVertically: true);
			m_colorSelector.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_colorSelector.Size = new Vector2(0.285f, m_colorSelector.Size.Y);
			myGuiControlParent.Controls.Add(m_colorSelector);
			vector.Y += 0.17f;
			m_optionsGroup.RefreshInternals();
			m_currentPosition.Y += m_optionsGroup.Size.Y;
			m_currentPosition.Y += 0.005f;
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2((0f - m_size.Value.X) * 0.83f / 2f, m_currentPosition.Y), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList2);
			m_currentPosition.Y += 0.018f;
			float y = m_currentPosition.Y;
			m_addSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_NewSafeZone, delegate
			{
				OnAddSafeZone();
			});
			m_addSafeZoneButton.PositionX = -0.088f;
			m_currentPosition.Y = y;
			m_moveToSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_MoveToSafeZone, delegate
			{
				OnMoveToSafeZone();
			});
			m_moveToSafeZoneButton.PositionX = 0.055f;
			y = m_currentPosition.Y;
			m_repositionSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ChangePosition, delegate
			{
				OnRepositionSafeZone();
			});
			m_repositionSafeZoneButton.PositionX = -0.088f;
			m_currentPosition.Y = y;
			m_configureFilterButton = CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter, delegate
			{
				OnConfigureFilter();
			});
			m_configureFilterButton.PositionX = 0.055f;
			y = m_currentPosition.Y;
			m_removeSafeZoneButton = CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Remove, delegate
			{
				OnRemoveSafeZone();
			});
			m_removeSafeZoneButton.PositionX = -0.088f;
			m_currentPosition.Y = y;
			m_renameSafeZoneButton = CreateDebugButton(0.14f, MySpaceTexts.DetailScreen_Button_Rename, delegate
			{
				OnRenameSafeZone();
			});
			m_renameSafeZoneButton.PositionX = 0.055f;
			RefreshSafeZones();
			UpdateZoneType();
			UpdateSelectedData();
			m_safeZonesCombo.ItemSelected += m_safeZonesCombo_ItemSelected;
			m_safeZonesTypeCombo.ItemSelected += m_safeZonesTypeCombo_ItemSelected;
			m_textureCombo.ItemSelected += OnTextureSelected;
			m_colorSelector.OnChange += OnColorChanged;
			m_recreateInProgress = false;
		}

		private void OnColorChanged(MyGuiControlColor obj)
		{
			if (m_selectedSafeZone != null)
			{
				MyObjectBuilder_SafeZone obj2 = (MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder();
				obj2.ModelColor = obj.GetColor().ToVector3();
				MySessionComponentSafeZones.RequestUpdateSafeZone(obj2);
			}
		}

		private void OnTextureSelected()
		{
			if (m_selectedSafeZone == null)
			{
				return;
			}
			IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
			if (allDefinitions == null)
			{
				MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
				return;
			}
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder();
			MyStringHash rhs = MyStringHash.TryGet((int)m_textureCombo.GetSelectedKey());
			bool flag = false;
			foreach (MySafeZoneTexturesDefinition item in allDefinitions)
			{
				if (item.DisplayTextId == rhs)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MyLog.Default.Error("Safe zone texture not found.");
				return;
			}
			myObjectBuilder_SafeZone.Texture = rhs.String;
			MySessionComponentSafeZones.RequestUpdateSafeZone(myObjectBuilder_SafeZone);
		}

		private void UpdateSelectedData()
		{
			m_recreateInProgress = true;
			bool enabled = m_selectedSafeZone != null;
			m_enabledCheckbox.Enabled = enabled;
			m_damageCheckbox.Enabled = enabled;
			m_shootingCheckbox.Enabled = enabled;
			m_drillingCheckbox.Enabled = enabled;
			m_weldingCheckbox.Enabled = enabled;
			m_grindingCheckbox.Enabled = enabled;
			m_voxelHandCheckbox.Enabled = enabled;
			m_buildingCheckbox.Enabled = enabled;
			m_convertToStationCheckbox.Enabled = enabled;
			m_landingGearLockCheckbox.Enabled = enabled;
			m_radiusSlider.Enabled = enabled;
			m_renameSafeZoneButton.Enabled = enabled;
			m_removeSafeZoneButton.Enabled = enabled;
			m_repositionSafeZoneButton.Enabled = enabled;
			m_moveToSafeZoneButton.Enabled = enabled;
			m_configureFilterButton.Enabled = enabled;
			m_safeZonesCombo.Enabled = enabled;
			m_safeZonesTypeCombo.Enabled = enabled;
			m_safeZonesAxisCombo.Enabled = enabled;
			m_sizeSlider.Enabled = enabled;
			m_colorSelector.Enabled = enabled;
			m_textureCombo.Enabled = enabled;
			if (m_selectedSafeZone != null)
			{
				m_enabledCheckbox.IsChecked = m_selectedSafeZone.Enabled;
				if (m_selectedSafeZone.Shape == MySafeZoneShape.Sphere)
				{
					m_radiusSlider.Value = m_selectedSafeZone.Radius;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Radius.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 0)
				{
					m_sizeSlider.Value = m_selectedSafeZone.Size.X;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.X.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 1)
				{
					m_sizeSlider.Value = m_selectedSafeZone.Size.Y;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Y.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 2)
				{
					m_sizeSlider.Value = m_selectedSafeZone.Size.Z;
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Z.ToString();
				}
				m_safeZonesTypeCombo.SelectItemByKey((long)m_selectedSafeZone.Shape);
				m_damageCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.Damage) > (MySafeZoneAction)0);
				m_shootingCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.Shooting) > (MySafeZoneAction)0);
				m_drillingCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.Drilling) > (MySafeZoneAction)0);
				m_weldingCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.Welding) > (MySafeZoneAction)0);
				m_grindingCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.Grinding) > (MySafeZoneAction)0);
				m_voxelHandCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.VoxelHand) > (MySafeZoneAction)0);
				m_buildingCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.Building) > (MySafeZoneAction)0);
				m_landingGearLockCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.LandingGearLock) > (MySafeZoneAction)0);
				m_convertToStationCheckbox.IsChecked = ((m_selectedSafeZone.AllowedActions & MySafeZoneAction.ConvertToStation) > (MySafeZoneAction)0);
				m_textureCombo.SelectItemByKey((int)m_selectedSafeZone.CurrentTexture);
				m_colorSelector.SetColor(m_selectedSafeZone.ModelColor);
			}
			m_recreateInProgress = false;
		}

		private void m_safeZonesTypeCombo_ItemSelected()
		{
			if (m_selectedSafeZone.Shape != (MySafeZoneShape)m_safeZonesTypeCombo.GetSelectedKey())
			{
				m_selectedSafeZone.Shape = (MySafeZoneShape)m_safeZonesTypeCombo.GetSelectedKey();
				m_selectedSafeZone.RecreatePhysics();
				UpdateZoneType();
				RequestUpdateSafeZone();
			}
		}

		private void UpdateZoneType()
		{
			m_zoneRadiusLabel.Visible = false;
			m_radiusSlider.Visible = false;
			m_selectAxisLabel.Visible = false;
			m_zoneSizeLabel.Visible = false;
			m_safeZonesAxisCombo.Visible = false;
			m_sizeSlider.Visible = false;
			if (m_selectedSafeZone == null || m_selectedSafeZone.Shape == MySafeZoneShape.Box)
			{
				m_selectAxisLabel.Visible = true;
				m_zoneSizeLabel.Visible = true;
				m_safeZonesAxisCombo.Visible = true;
				m_sizeSlider.Visible = true;
			}
			else if (m_selectedSafeZone.Shape == MySafeZoneShape.Sphere)
			{
				m_zoneRadiusLabel.Visible = true;
				m_radiusSlider.Visible = true;
			}
		}

		private void m_safeZonesAxisCombo_ItemSelected()
		{
			if (m_selectedSafeZone != null)
			{
				if (m_safeZonesAxisCombo.GetSelectedIndex() == 0)
				{
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.X.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 1)
				{
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Y.ToString();
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 2)
				{
					m_zoneRadiusValueLabel.Text = m_selectedSafeZone.Size.Z.ToString();
				}
			}
		}

		private void m_safeZonesCombo_ItemSelected()
		{
			m_selectedSafeZone = (MySafeZone)MyEntities.GetEntityById(m_safeZonesCombo.GetItemByIndex(m_safeZonesCombo.GetSelectedIndex()).Key);
			UpdateZoneType();
			UpdateSelectedData();
		}

		private void OnAddSafeZone()
		{
			MySessionComponentSafeZones.RequestCreateSafeZone(MySector.MainCamera.Position + 2f * MySector.MainCamera.ForwardVector);
		}

		private void OnRemoveSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				MySessionComponentSafeZones.RequestDeleteSafeZone(m_selectedSafeZone.EntityId);
				RequestUpdateSafeZone();
			}
		}

		private void OnRenameSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(new Vector2(0.5f, 0.5f), delegate(string result)
				{
					if (result != null)
					{
						m_selectedSafeZone.DisplayName = result;
						RequestUpdateSafeZone();
						RefreshSafeZones();
					}
				}, "New Name", MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), 50, 0.3f));
			}
		}

		private void OnConfigureFilter()
		{
			if (m_selectedSafeZone != null)
			{
				MySafeZone selectedSafeZone = m_selectedSafeZone;
				MyScreenManager.AddScreen(new MyGuiScreenSafeZoneFilter(new Vector2(0.5f, 0.5f), selectedSafeZone));
			}
		}

		private void OnMoveToSafeZone()
		{
			if (m_selectedSafeZone != null && MySession.Static.ControlledEntity != null)
			{
				MyMultiplayer.TeleportControlledEntity(m_selectedSafeZone.PositionComp.WorldMatrix.Translation);
			}
		}

		private void OnRepositionSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				m_selectedSafeZone.PositionComp.WorldMatrix = MySector.MainCamera.WorldMatrix;
				m_selectedSafeZone.RecreatePhysics();
				RequestUpdateSafeZone();
			}
		}

		private void MySafeZones_OnAddSafeZone(object sender, EventArgs e)
		{
			m_selectedSafeZone = (MySafeZone)sender;
			if (m_currentPage == MyPageEnum.SafeZones)
			{
				m_recreateInProgress = true;
				RefreshSafeZones();
				UpdateSelectedData();
				m_recreateInProgress = false;
			}
		}

		private void MySafeZones_OnRemoveSafeZone(object sender, EventArgs e)
		{
			if (m_safeZonesCombo != null)
			{
				if (m_selectedSafeZone == sender)
				{
					m_selectedSafeZone = null;
					RefreshSafeZones();
					m_selectedSafeZone = ((m_safeZonesCombo.GetItemsCount() > 0) ? ((MySafeZone)MyEntities.GetEntityById(m_safeZonesCombo.GetItemByIndex(m_safeZonesCombo.GetItemsCount() - 1).Key)) : null);
					m_recreateInProgress = true;
					UpdateSelectedData();
					m_recreateInProgress = false;
				}
				else
				{
					m_safeZonesCombo.RemoveItem(((MySafeZone)sender).EntityId);
				}
			}
		}

		private void RequestUpdateSafeZone()
		{
			if (m_selectedSafeZone != null)
			{
				MySessionComponentSafeZones.RequestUpdateSafeZone((MyObjectBuilder_SafeZone)m_selectedSafeZone.GetObjectBuilder());
			}
		}

		private void RefreshSafeZones()
		{
			m_safeZonesCombo.ClearItems();
			List<MySafeZone> list = MySessionComponentSafeZones.SafeZones.ToList();
			list.Sort(new MySafezoneNameComparer());
			foreach (MySafeZone item in list)
			{
				if (item.SafeZoneBlockId == 0L)
				{
					m_safeZonesCombo.AddItem(item.EntityId, (item.DisplayName != null) ? item.DisplayName : item.ToString(), 1);
				}
			}
			if (m_selectedSafeZone == null)
			{
				m_selectedSafeZone = ((m_safeZonesCombo.GetItemsCount() > 0) ? ((MySafeZone)MyEntities.GetEntityById(m_safeZonesCombo.GetItemByIndex(m_safeZonesCombo.GetItemsCount() - 1).Key)) : null);
			}
			if (m_selectedSafeZone != null)
			{
				m_safeZonesCombo.SelectItemByKey(m_selectedSafeZone.EntityId);
			}
		}

		private void EnabledCheckedChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone == null || m_recreateInProgress)
			{
				return;
			}
			if (checkBox.IsChecked && MySessionComponentSafeZones.IsSafeZoneColliding(m_selectedSafeZone.EntityId, m_selectedSafeZone.WorldMatrix, m_selectedSafeZone.Shape, m_selectedSafeZone.Radius, m_selectedSafeZone.Size))
			{
				checkBox.IsChecked = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MySpaceTexts.AdminScreen_Safezone_Collision)));
				return;
			}
			if (m_selectedSafeZone.Enabled != checkBox.IsChecked)
			{
				m_selectedSafeZone.Enabled = checkBox.IsChecked;
				m_selectedSafeZone.RefreshGraphics();
			}
			RequestUpdateSafeZone();
		}

		private void OnRadiusChange(MyGuiControlSlider slider)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (m_selectedSafeZone.Enabled && MySessionComponentSafeZones.IsSafeZoneColliding(m_selectedSafeZone.EntityId, m_selectedSafeZone.WorldMatrix, m_selectedSafeZone.Shape, slider.Value))
				{
					slider.Value = m_selectedSafeZone.Radius;
					return;
				}
				m_zoneRadiusValueLabel.Text = slider.Value.ToString();
				m_selectedSafeZone.Radius = slider.Value;
				m_selectedSafeZone.RecreatePhysics();
				RequestUpdateSafeZone();
			}
		}

		private void OnSizeChange(MyGuiControlSlider slider)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				Vector3 vector = Vector3.Zero;
				float value = 0f;
				if (m_safeZonesAxisCombo.GetSelectedIndex() == 0)
				{
					value = m_selectedSafeZone.Size.X;
					vector = new Vector3(slider.Value, m_selectedSafeZone.Size.Y, m_selectedSafeZone.Size.Z);
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 1)
				{
					value = m_selectedSafeZone.Size.Y;
					vector = new Vector3(m_selectedSafeZone.Size.X, slider.Value, m_selectedSafeZone.Size.Z);
				}
				else if (m_safeZonesAxisCombo.GetSelectedIndex() == 2)
				{
					value = m_selectedSafeZone.Size.Z;
					vector = new Vector3(m_selectedSafeZone.Size.X, m_selectedSafeZone.Size.Y, slider.Value);
				}
				if (m_selectedSafeZone.Enabled && MySessionComponentSafeZones.IsSafeZoneColliding(m_selectedSafeZone.EntityId, m_selectedSafeZone.WorldMatrix, m_selectedSafeZone.Shape, 0f, vector))
				{
					slider.Value = value;
					return;
				}
				m_zoneRadiusValueLabel.Text = slider.Value.ToString();
				m_selectedSafeZone.Size = vector;
				m_selectedSafeZone.RecreatePhysics();
				RequestUpdateSafeZone();
			}
		}

		private void DamageCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Damage;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Damage;
				}
				RequestUpdateSafeZone();
			}
		}

		private void ShootingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Shooting;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Shooting;
				}
				RequestUpdateSafeZone();
			}
		}

		private void DrillingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Drilling;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Drilling;
				}
				RequestUpdateSafeZone();
			}
		}

		private void WeldingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Welding;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Welding;
				}
				RequestUpdateSafeZone();
			}
		}

		private void GrindingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Grinding;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Grinding;
				}
				RequestUpdateSafeZone();
			}
		}

		private void VoxelHandCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.VoxelHand;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.VoxelHand;
				}
				RequestUpdateSafeZone();
			}
		}

		private void OnSettingsCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= (MySafeZoneAction)checkBox.UserData;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~(MySafeZoneAction)checkBox.UserData;
				}
				RequestUpdateSafeZone();
			}
		}

		private void BuildingCheckChanged(MyGuiControlCheckbox checkBox)
		{
			if (m_selectedSafeZone != null && !m_recreateInProgress)
			{
				if (checkBox.IsChecked)
				{
					m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Building;
				}
				else
				{
					m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Building;
				}
				RequestUpdateSafeZone();
			}
		}
	}
}
