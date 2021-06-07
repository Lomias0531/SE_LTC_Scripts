using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TerminalBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyTerminalBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyTerminalBlock)
	})]
	public class MyTerminalBlock : MySyncedBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock
	{
		public enum AccessRightsResult
		{
			Granted,
			Enemies,
			MissingDLC,
			Other,
			None
		}

		protected sealed class SetCustomNameEvent_003C_003ESystem_String : ICallSite<MyTerminalBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in string name, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetCustomNameEvent(name);
			}
		}

		protected sealed class OnCustomDataChanged_003C_003ESystem_String : ICallSite<MyTerminalBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in string data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnCustomDataChanged(data);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64 : ICallSite<MyTerminalBlock, bool, bool, ulong, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in bool isOpen, in bool editable, in ulong user, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64 : ICallSite<MyTerminalBlock, bool, bool, ulong, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in bool isOpen, in bool editable, in ulong user, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user);
			}
		}

		protected class m_showOnHUD_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showOnHUD;
				ISyncType result = showOnHUD = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyTerminalBlock)P_0).m_showOnHUD = (Sync<bool, SyncDirection.BothWays>)showOnHUD;
				return result;
			}
		}

		protected class m_showInTerminal_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showInTerminal;
				ISyncType result = showInTerminal = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyTerminalBlock)P_0).m_showInTerminal = (Sync<bool, SyncDirection.BothWays>)showInTerminal;
				return result;
			}
		}

		protected class m_showInToolbarConfig_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showInToolbarConfig;
				ISyncType result = showInToolbarConfig = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyTerminalBlock)P_0).m_showInToolbarConfig = (Sync<bool, SyncDirection.BothWays>)showInToolbarConfig;
				return result;
			}
		}

		protected class m_showInInventory_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showInInventory;
				ISyncType result = showInInventory = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyTerminalBlock)P_0).m_showInInventory = (Sync<bool, SyncDirection.BothWays>)showInInventory;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyTerminalBlock_003C_003EActor : IActivator, IActivator<MyTerminalBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyTerminalBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTerminalBlock CreateInstance()
			{
				return new MyTerminalBlock();
			}

			MyTerminalBlock IActivator<MyTerminalBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly Guid m_storageGuid = new Guid("74DE02B3-27F9-4960-B1C4-27351F2B06D1");

		private const int DATA_CHARACTER_LIMIT = 64000;

		private Sync<bool, SyncDirection.BothWays> m_showOnHUD;

		private Sync<bool, SyncDirection.BothWays> m_showInTerminal;

		private Sync<bool, SyncDirection.BothWays> m_showInToolbarConfig;

		private Sync<bool, SyncDirection.BothWays> m_showInInventory;

		private bool m_isBeingHackedPrevValue;

		private MyGuiScreenTextPanel m_textBox;

		protected bool m_textboxOpen;

		private ulong m_currentUser;

		public int? HackAttemptTime;

		public bool IsAccessibleForProgrammableBlock = true;

		private bool m_detailedInfoDirty;

		private readonly StringBuilder m_detailedInfo = new StringBuilder();

		private static FastResourceLock m_createControlsLock = new FastResourceLock();

		public StringBuilder CustomName
		{
			get;
			private set;
		}

		public StringBuilder CustomNameWithFaction
		{
			get;
			private set;
		}

		public string CustomData
		{
			get
			{
				if (base.Storage == null || !base.Storage.TryGetValue(m_storageGuid, out string value))
				{
					return string.Empty;
				}
				return value;
			}
			set
			{
				SetCustomData_Internal(value, sync: true);
			}
		}

		public bool ShowOnHUD
		{
			get
			{
				return m_showOnHUD;
			}
			set
			{
				if ((bool)m_showOnHUD != value && CanShowOnHud)
				{
					m_showOnHUD.Value = value;
					RaiseShowOnHUDChanged();
				}
			}
		}

		public bool ShowInTerminal
		{
			get
			{
				return m_showInTerminal;
			}
			set
			{
				if ((bool)m_showInTerminal != value)
				{
					m_showInTerminal.Value = value;
					RaiseShowInTerminalChanged();
				}
			}
		}

		public bool ShowInInventory
		{
			get
			{
				return m_showInInventory;
			}
			set
			{
				if ((bool)m_showInInventory != value)
				{
					m_showInInventory.Value = value;
					RaiseShowInInventoryChanged();
				}
			}
		}

		public bool ShowInToolbarConfig
		{
			get
			{
				return m_showInToolbarConfig;
			}
			set
			{
				if ((bool)m_showInToolbarConfig != value)
				{
					m_showInToolbarConfig.Value = value;
					RaiseShowInToolbarConfigChanged();
				}
			}
		}

		public new bool IsBeingHacked
		{
			get
			{
				if (!HackAttemptTime.HasValue)
				{
					return false;
				}
				bool flag = MySandboxGame.TotalSimulationTimeInMilliseconds - HackAttemptTime.Value < 1000;
				if (flag != m_isBeingHackedPrevValue)
				{
					m_isBeingHackedPrevValue = flag;
					RaiseIsBeingHackedChanged();
				}
				return flag;
			}
		}

		public StringBuilder DetailedInfo
		{
			get
			{
				if (m_detailedInfoDirty)
				{
					m_detailedInfoDirty = false;
					UpdateDetailedInfo(m_detailedInfo);
				}
				return m_detailedInfo;
			}
		}

		public StringBuilder CustomInfo
		{
			get;
			private set;
		}

		public bool HasUnsafeValues
		{
			get;
			private set;
		}

		public bool IsOpenedInTerminal
		{
			get;
			set;
		}

		protected virtual bool CanShowOnHud => true;

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomName
		{
			get
			{
				return CustomName.ToString();
			}
			set
			{
				SetCustomName(value);
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomNameWithFaction => CustomNameWithFaction.ToString();

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.DetailedInfo => DetailedInfo.ToString();

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomInfo => CustomInfo.ToString();

		public event Action<MyTerminalBlock> CustomDataChanged;

		public event Action<MyTerminalBlock> CustomNameChanged;

		public event Action<MyTerminalBlock> PropertiesChanged;

		public event Action<MyTerminalBlock> OwnershipChanged;

		public event Action<MyTerminalBlock> VisibilityChanged;

		public event Action<MyTerminalBlock> ShowOnHUDChanged;

		public event Action<MyTerminalBlock> ShowInTerminalChanged;

		public event Action<MyTerminalBlock> ShowInIventoryChanged;

		public event Action<MyTerminalBlock> ShowInToolbarConfigChanged;

		public event Action<MyTerminalBlock> IsBeingHackedChanged;

		public event Action<MyTerminalBlock, StringBuilder> AppendingCustomInfo;

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.CustomNameChanged
		{
			add
			{
				CustomNameChanged += GetDelegate(value);
			}
			remove
			{
				CustomNameChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.OwnershipChanged
		{
			add
			{
				OwnershipChanged += GetDelegate(value);
			}
			remove
			{
				OwnershipChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.PropertiesChanged
		{
			add
			{
				PropertiesChanged += GetDelegate(value);
			}
			remove
			{
				PropertiesChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.ShowOnHUDChanged
		{
			add
			{
				ShowOnHUDChanged += GetDelegate(value);
			}
			remove
			{
				ShowOnHUDChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.VisibilityChanged
		{
			add
			{
				VisibilityChanged += GetDelegate(value);
			}
			remove
			{
				VisibilityChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> Sandbox.ModAPI.IMyTerminalBlock.AppendingCustomInfo
		{
			add
			{
				AppendingCustomInfo += GetDelegate(value);
			}
			remove
			{
				AppendingCustomInfo -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.CustomDataChanged
		{
			add
			{
				CustomDataChanged += GetDelegate(value);
			}
			remove
			{
				CustomDataChanged -= GetDelegate(value);
			}
		}

		private void SetCustomData_Internal(string value, bool sync)
		{
			if (base.Storage == null)
			{
				base.Storage = new MyModStorageComponent();
				base.Components.Add(base.Storage);
			}
			if (value.Length <= 64000)
			{
				base.Storage[m_storageGuid] = value;
			}
			else
			{
				base.Storage[m_storageGuid] = value.Substring(0, 64000);
			}
			if (sync)
			{
				RaiseCustomDataChanged();
			}
			else
			{
				this.CustomDataChanged?.Invoke(this);
			}
		}

		public MyTerminalBlock()
		{
			using (m_createControlsLock.AcquireExclusiveUsing())
			{
				CreateTerminalControls();
			}
			CustomInfo = new StringBuilder();
			CustomNameWithFaction = new StringBuilder();
			CustomName = new StringBuilder();
			base.SyncType.PropertyChanged += delegate
			{
				RaisePropertiesChanged();
			};
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_TerminalBlock myObjectBuilder_TerminalBlock = (MyObjectBuilder_TerminalBlock)objectBuilder;
			if (myObjectBuilder_TerminalBlock.CustomName != null)
			{
				CustomName.Clear().Append(myObjectBuilder_TerminalBlock.CustomName);
				DisplayNameText = myObjectBuilder_TerminalBlock.CustomName;
			}
			else
			{
				CustomName.Append(DisplayNameText);
			}
			if (Sync.IsServer && Sync.Clients != null)
			{
				MyClientCollection clients = Sync.Clients;
				clients.ClientRemoved = (Action<ulong>)Delegate.Combine(clients.ClientRemoved, new Action<ulong>(ClientRemoved));
			}
			m_showOnHUD.ValueChanged += m_showOnHUD_ValueChanged;
			m_showOnHUD.SetLocalValue(myObjectBuilder_TerminalBlock.ShowOnHUD);
			m_showInTerminal.SetLocalValue(myObjectBuilder_TerminalBlock.ShowInTerminal);
			m_showInInventory.SetLocalValue(myObjectBuilder_TerminalBlock.ShowInInventory);
			m_showInToolbarConfig.SetLocalValue(myObjectBuilder_TerminalBlock.ShowInToolbarConfig);
			AddDebugRenderComponent(new MyDebugRenderComponentTerminal(this));
		}

		private void m_showOnHUD_ValueChanged(SyncBase obj)
		{
			if (base.CubeGrid != null)
			{
				base.CubeGrid.MarkForUpdate();
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (this is IMyControllableEntity)
			{
				MyPlayerCollection.UpdateControl(this);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (HasUnsafeValues)
			{
				base.CubeGrid.UnregisterUnsafeBlock(this);
			}
			base.OnRemovedFromScene(source);
		}

		protected override void Closing()
		{
			base.Closing();
			if (Sync.IsServer && Sync.Clients != null)
			{
				MyClientCollection clients = Sync.Clients;
				clients.ClientRemoved = (Action<ulong>)Delegate.Remove(clients.ClientRemoved, new Action<ulong>(ClientRemoved));
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TerminalBlock obj = (MyObjectBuilder_TerminalBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.CustomName = DisplayNameText.ToString();
			obj.ShowOnHUD = ShowOnHUD;
			obj.ShowInTerminal = ShowInTerminal;
			obj.ShowInInventory = ShowInInventory;
			obj.ShowInToolbarConfig = ShowInToolbarConfig;
			return obj;
		}

		public void NotifyTerminalValueChanged(ITerminalControl control)
		{
		}

		public void RefreshCustomInfo()
		{
			CustomInfo.Clear();
			this.AppendingCustomInfo?.Invoke(this, CustomInfo);
		}

		public void SetCustomName(string text)
		{
			UpdateCustomName(text);
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.SetCustomNameEvent, text);
		}

		public void UpdateCustomName(string text)
		{
			if (CustomName.CompareUpdate(text))
			{
				RaiseCustomNameChanged();
				RaiseShowOnHUDChanged();
				DisplayNameText = text;
			}
		}

		public void SetCustomName(StringBuilder text)
		{
			UpdateCustomName(text);
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.SetCustomNameEvent, text.ToString());
		}

		[Event(null, 362)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		public void SetCustomNameEvent(string name)
		{
			UpdateCustomName(name);
		}

		public void UpdateCustomName(StringBuilder text)
		{
			if (CustomName.CompareUpdate(text))
			{
				DisplayNameText = text.ToString();
				RaiseCustomNameChanged();
				RaiseShowOnHUDChanged();
			}
		}

		private void RaiseCustomNameChanged()
		{
			this.CustomNameChanged?.Invoke(this);
		}

		public void RaisePropertiesChanged()
		{
			this.PropertiesChanged?.Invoke(this);
		}

		protected void SetDetailedInfoDirty()
		{
			m_detailedInfoDirty = true;
		}

		protected void RaiseVisibilityChanged()
		{
			this.VisibilityChanged?.Invoke(this);
		}

		protected void RaiseShowOnHUDChanged()
		{
			this.ShowOnHUDChanged?.Invoke(this);
		}

		protected void RaiseShowInTerminalChanged()
		{
			this.ShowInTerminalChanged?.Invoke(this);
		}

		protected void RaiseShowInInventoryChanged()
		{
			this.ShowInIventoryChanged?.Invoke(this);
		}

		protected void RaiseShowInToolbarConfigChanged()
		{
			this.ShowInToolbarConfigChanged?.Invoke(this);
		}

		protected void RaiseIsBeingHackedChanged()
		{
			this.IsBeingHackedChanged?.Invoke(this);
		}

		public bool HasLocalPlayerAccess()
		{
			return HasPlayerAccess(MySession.Static.LocalPlayerId);
		}

		public bool HasPlayerAccess(long identityId)
		{
			return HasPlayerAccessReason(identityId) == AccessRightsResult.Granted;
		}

		public AccessRightsResult HasPlayerAccessReason(long identityId)
		{
			if (!MyFakes.SHOW_FACTIONS_GUI)
			{
				return AccessRightsResult.Other;
			}
			if (HasAdminUseTerminals(identityId))
			{
				return AccessRightsResult.Granted;
			}
			if (!GetUserRelationToOwner(identityId).IsFriendly())
			{
				return AccessRightsResult.Enemies;
			}
			return AccessRightsResult.Granted;
		}

		internal bool HasLocalPlayerAdminUseTerminals()
		{
			return HasAdminUseTerminals(MySession.Static.LocalPlayerId);
		}

		internal bool HasAdminUseTerminals(long identityId)
		{
			ulong key = MySession.Static.Players.TryGetSteamId(identityId);
			AdminSettingsEnum value2;
			if (Sync.IsServer)
			{
				if (MySession.Static.RemoteAdminSettings.TryGetValue(key, out AdminSettingsEnum value) && value.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					return true;
				}
			}
			else if (identityId == MySession.Static.LocalPlayerId)
			{
				if (MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					return true;
				}
			}
			else if (MySession.Static.RemoteAdminSettings.TryGetValue(key, out value2) && value2.HasFlag(AdminSettingsEnum.UseTerminals))
			{
				return true;
			}
			return false;
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			CustomNameWithFaction.Clear();
			if (!string.IsNullOrEmpty(GetOwnerFactionTag()))
			{
				CustomNameWithFaction.Append(GetOwnerFactionTag());
				CustomNameWithFaction.Append(".");
			}
			CustomNameWithFaction.AppendStringBuilder(CustomName);
			m_hudParams.Clear();
			m_hudParams.Add(new MyHudEntityParams
			{
				FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL,
				Text = CustomNameWithFaction,
				Owner = ((base.IDModule != null) ? base.IDModule.Owner : 0),
				Share = ((base.IDModule != null) ? base.IDModule.ShareMode : MyOwnershipShareModeEnum.None),
				Entity = this,
				BlinkingTime = ((allowBlink && IsBeingHacked) ? 10 : 0)
			});
			return m_hudParams;
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			RaiseOwnershipChanged();
			RaiseShowOnHUDChanged();
			RaisePropertiesChanged();
		}

		private void RaiseOwnershipChanged()
		{
			if (this.OwnershipChanged != null)
			{
				this.OwnershipChanged(this);
			}
		}

		public virtual void GetTerminalName(StringBuilder result)
		{
			result.AppendStringBuilder(CustomName);
		}

		protected void PrintUpgradeModuleInfo(StringBuilder output)
		{
			if (GetComponent().ConnectionPositions.Count == 0)
			{
				return;
			}
			int num = 0;
			if (CurrentAttachedUpgradeModules != null)
			{
				foreach (AttachedUpgradeModule value in CurrentAttachedUpgradeModules.Values)
				{
					num += value.SlotCount;
				}
			}
			output.Append(MyTexts.Get(MyCommonTexts.Module_UsedSlots).ToString() + num + " / " + GetComponent().ConnectionPositions.Count + "\n");
			if (CurrentAttachedUpgradeModules != null)
			{
				int num2 = 0;
				foreach (AttachedUpgradeModule value2 in CurrentAttachedUpgradeModules.Values)
				{
					num2 += ((value2.Block != null && value2.Block.IsWorking) ? 1 : 0);
				}
				output.Append(MyTexts.Get(MyCommonTexts.Module_Attached).ToString() + CurrentAttachedUpgradeModules.Count);
				if (num2 != CurrentAttachedUpgradeModules.Count)
				{
					output.Append(" (" + num2 + MyTexts.Get(MyCommonTexts.Module_Functioning).ToString());
				}
				output.Append("\n");
				foreach (AttachedUpgradeModule value3 in CurrentAttachedUpgradeModules.Values)
				{
					if (value3.Block != null)
					{
						output.Append(" - " + value3.Block.DisplayNameText + ((!value3.Block.IsFunctional) ? MyTexts.Get(MyCommonTexts.Module_Damaged).ToString() : ((!value3.Compatible) ? MyTexts.Get(MyCommonTexts.Module_Incompatible).ToString() : (value3.Block.Enabled ? "" : MyTexts.Get(MyCommonTexts.Module_Off).ToString()))));
					}
					else
					{
						output.Append(MyTexts.Get(MyCommonTexts.Module_Unknown).ToString());
					}
					output.Append("\n");
				}
			}
			output.AppendFormat("\n");
		}

		protected void FixSingleInventory()
		{
			if (base.Components.TryGet(out MyInventoryBase component))
			{
				MyInventoryAggregate myInventoryAggregate = component as MyInventoryAggregate;
				MyInventory myInventory = null;
				if (myInventoryAggregate != null)
				{
					foreach (MyComponentBase item in myInventoryAggregate.ChildList.Reader)
					{
						MyInventory myInventory2 = item as MyInventory;
						if (myInventory2 != null)
						{
							if (myInventory == null)
							{
								myInventory = myInventory2;
							}
							else if (myInventory.GetItemsCount() < myInventory2.GetItemsCount())
							{
								myInventory = myInventory2;
							}
						}
					}
				}
				if (myInventory != null)
				{
					base.Components.Remove<MyInventoryBase>();
					base.Components.Add((MyInventoryBase)myInventory);
				}
			}
		}

		protected virtual void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyTerminalBlock>())
			{
				MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInTerminal", MySpaceTexts.Terminal_ShowInTerminal, MySpaceTexts.Terminal_ShowInTerminalToolTip)
				{
					Getter = ((MyTerminalBlock x) => x.m_showInTerminal),
					Setter = delegate(MyTerminalBlock x, bool v)
					{
						x.ShowInTerminal = v;
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInInventory", MySpaceTexts.Terminal_ShowInInventory, MySpaceTexts.Terminal_ShowInInventoryToolTip)
				{
					Getter = ((MyTerminalBlock x) => x.m_showInInventory),
					Setter = delegate(MyTerminalBlock x, bool v)
					{
						x.ShowInInventory = v;
					},
					Visible = ((MyTerminalBlock x) => x.HasInventory)
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInToolbarConfig", MySpaceTexts.Terminal_ShowInToolbarConfig, MySpaceTexts.Terminal_ShowInToolbarConfigToolTip)
				{
					Getter = ((MyTerminalBlock x) => x.m_showInToolbarConfig),
					Setter = delegate(MyTerminalBlock x, bool v)
					{
						x.ShowInToolbarConfig = v;
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyTerminalBlock>("Name", MyCommonTexts.Name, MySpaceTexts.Blank)
				{
					Getter = ((MyTerminalBlock x) => x.CustomName),
					Setter = delegate(MyTerminalBlock x, StringBuilder v)
					{
						x.SetCustomName(v);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlOnOffSwitch<MyTerminalBlock> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowOnHUD", MySpaceTexts.Terminal_ShowOnHUD, MySpaceTexts.Terminal_ShowOnHUDToolTip);
				myTerminalControlOnOffSwitch.Getter = ((MyTerminalBlock x) => x.ShowOnHUD);
				myTerminalControlOnOffSwitch.Setter = delegate(MyTerminalBlock x, bool v)
				{
					x.ShowOnHUD = v;
				};
				myTerminalControlOnOffSwitch.EnableToggleAction();
				myTerminalControlOnOffSwitch.EnableOnOffActions();
				myTerminalControlOnOffSwitch.Visible = ((MyTerminalBlock x) => x.CanShowOnHud);
				myTerminalControlOnOffSwitch.Enabled = ((MyTerminalBlock x) => x.CanShowOnHud);
				MyTerminalAction<MyTerminalBlock>[] actions = myTerminalControlOnOffSwitch.Actions;
				for (int i = 0; i < actions.Length; i++)
				{
					actions[i].Enabled = ((MyTerminalBlock x) => x.CanShowOnHud);
				}
				MyTerminalControlFactory.AddControl(myTerminalControlOnOffSwitch);
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTerminalBlock>("CustomData", MySpaceTexts.Terminal_CustomData, MySpaceTexts.Terminal_CustomDataTooltip, CustomDataClicked)
				{
					Enabled = ((MyTerminalBlock x) => !x.m_textboxOpen),
					SupportsMultipleBlocks = false
				});
			}
		}

		protected void CustomDataClicked(MyTerminalBlock myTerminalBlock)
		{
			myTerminalBlock.OpenWindow(isEditable: true, sync: true);
		}

		private void RaiseCustomDataChanged()
		{
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.OnCustomDataChanged, CustomData);
		}

		[Event(null, 692)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void OnCustomDataChanged(string data)
		{
			SetCustomData_Internal(data, sync: false);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL)
		{
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.OnChangeOpenRequest, isOpen, editable, user);
		}

		[Event(null, 703)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user)
		{
			if (!(Sync.IsServer && m_textboxOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user);
				MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user);
			}
		}

		[Event(null, 714)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user)
		{
			OnChangeOpen(isOpen, editable, user);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user)
		{
			m_textboxOpen = isOpen;
			m_currentUser = user;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false);
			}
		}

		private void CreateTextBox(bool isEditable, string description)
		{
			string missionTitle = CustomName.ToString();
			string @string = MyTexts.GetString(MySpaceTexts.Terminal_CustomData);
			bool editable = isEditable;
			m_textBox = new MyGuiScreenTextPanel(missionTitle, "", @string, description, OnClosedTextBox, null, null, editable);
		}

		public void OpenWindow(bool isEditable, bool sync)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId);
				return;
			}
			CreateTextBox(isEditable, CustomData);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBox);
		}

		public void OnClosedTextBox(ResultEnum result)
		{
			if (m_textBox != null)
			{
				CloseWindow();
			}
		}

		public void OnClosedMessageBox(ResultEnum result)
		{
			if (result == ResultEnum.OK)
			{
				CloseWindow();
				return;
			}
			CreateTextBox(isEditable: true, m_textBox.Description.Text.ToString());
			MyScreenManager.AddScreen(m_textBox);
		}

		private void CloseWindow()
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					CustomData = m_textBox.Description.Text.ToString();
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		private void ClientRemoved(ulong steamId)
		{
			if (steamId == m_currentUser)
			{
				SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
			}
		}

		protected void OnUnsafeSettingsChanged()
		{
			MySandboxGame.Static.Invoke("", this, delegate(object x)
			{
				OnUnsafeSettingsChangedInternal(x);
			});
		}

		private static void OnUnsafeSettingsChangedInternal(object o)
		{
			MyTerminalBlock myTerminalBlock = (MyTerminalBlock)o;
			if (myTerminalBlock.MarkedForClose)
			{
				return;
			}
			bool flag = myTerminalBlock.HasUnsafeSettingsCollector();
			if (myTerminalBlock.HasUnsafeValues != flag)
			{
				myTerminalBlock.HasUnsafeValues = flag;
				if (flag)
				{
					myTerminalBlock.CubeGrid.RegisterUnsafeBlock(myTerminalBlock);
				}
				else
				{
					myTerminalBlock.CubeGrid.UnregisterUnsafeBlock(myTerminalBlock);
				}
			}
		}

		protected virtual bool HasUnsafeSettingsCollector()
		{
			return false;
		}

		protected virtual void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			detailedInfo.Clear();
		}

		public override string ToString()
		{
			return base.ToString() + " " + CustomName;
		}

		private Action<MyTerminalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock> value)
		{
			return (Action<MyTerminalBlock>)Delegate.CreateDelegate(typeof(Action<MyTerminalBlock>), value.Target, value.Method);
		}

		private Action<MyTerminalBlock, StringBuilder> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> value)
		{
			return (Action<MyTerminalBlock, StringBuilder>)Delegate.CreateDelegate(typeof(Action<MyTerminalBlock, StringBuilder>), value.Target, value.Method);
		}

		bool Sandbox.ModAPI.IMyTerminalBlock.IsInSameLogicalGroupAs(Sandbox.ModAPI.IMyTerminalBlock other)
		{
			return base.CubeGrid.IsInSameLogicalGroupAs(other.CubeGrid);
		}

		bool Sandbox.ModAPI.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.IMyTerminalBlock other)
		{
			return base.CubeGrid.IsSameConstructAs(other.CubeGrid);
		}

		void Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActions(List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect)
		{
			((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetActions(GetType(), resultList, collect);
		}

		void Sandbox.ModAPI.Ingame.IMyTerminalBlock.SearchActionsOfName(string name, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
		{
			((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).SearchActionsOfName(name, GetType(), resultList, collect);
		}

		Sandbox.ModAPI.Interfaces.ITerminalAction Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActionWithName(string name)
		{
			return ((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetActionWithName(name, GetType());
		}

		public ITerminalProperty GetProperty(string id)
		{
			return ((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetProperty(id, GetType());
		}

		public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
		{
			((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetProperties(GetType(), resultList, collect);
		}

		bool Sandbox.ModAPI.Ingame.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.Ingame.IMyTerminalBlock other)
		{
			return ((VRage.Game.ModAPI.Ingame.IMyCubeGrid)base.CubeGrid).IsSameConstructAs(other.CubeGrid);
		}
	}
}
