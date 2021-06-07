using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.Entities.Cube;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ButtonPanel))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyButtonPanel),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel)
	})]
	public class MyButtonPanel : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyButtonPanel, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel
	{
		protected sealed class ActivateButton_003C_003ESystem_Int32 : ICallSite<MyButtonPanel, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int index, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ActivateButton(index);
			}
		}

		protected sealed class NotifyActivationFailed_003C_003E : ICallSite<MyButtonPanel, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.NotifyActivationFailed();
			}
		}

		protected sealed class SetButtonName_003C_003ESystem_String_0023System_Int32 : ICallSite<MyButtonPanel, string, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in string name, in int position, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetButtonName(name, position);
			}
		}

		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MyButtonPanel, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected class m_anyoneCanUse_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType anyoneCanUse;
				ISyncType result = anyoneCanUse = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyButtonPanel)P_0).m_anyoneCanUse = (Sync<bool, SyncDirection.BothWays>)anyoneCanUse;
				return result;
			}
		}

		private const string DETECTOR_NAME = "panel";

		private new List<string> m_emissiveNames;

		private readonly Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

		private int m_selectedButton = -1;

		private MyHudNotification m_activationFailedNotification = new MyHudNotification(MySpaceTexts.Notification_ActivationFailed, 2500, "Red");

		private static List<MyToolbar> m_openedToolbars;

		private static bool m_shouldSetOtherToolbars;

		private SerializableDictionary<int, string> m_customButtonNames = new SerializableDictionary<int, string>();

		private List<MyUseObjectPanelButton> m_buttonsUseObjects = new List<MyUseObjectPanelButton>();

		private StringBuilder m_emptyName = new StringBuilder("");

		private bool m_syncing;

		private static StringBuilder m_helperSB = new StringBuilder();

		public MyToolbar Toolbar
		{
			get;
			set;
		}

		public new MyButtonPanelDefinition BlockDefinition => base.BlockDefinition as MyButtonPanelDefinition;

		public bool AnyoneCanUse
		{
			get
			{
				return m_anyoneCanUse;
			}
			set
			{
				m_anyoneCanUse.Value = value;
			}
		}

		private event Action<int> ButtonPressed;

		event Action<int> SpaceEngineers.Game.ModAPI.IMyButtonPanel.ButtonPressed
		{
			add
			{
				ButtonPressed += value;
			}
			remove
			{
				ButtonPressed -= value;
			}
		}

		public MyButtonPanel()
		{
			CreateTerminalControls();
			m_openedToolbars = new List<MyToolbar>();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyButtonPanel>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyButtonPanel> obj = new MyTerminalControlCheckbox<MyButtonPanel>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
				{
					Getter = ((MyButtonPanel x) => x.AnyoneCanUse),
					Setter = delegate(MyButtonPanel x, bool v)
					{
						x.AnyoneCanUse = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyButtonPanel>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, delegate(MyButtonPanel self)
				{
					m_openedToolbars.Add(self.Toolbar);
					if (MyGuiScreenToolbarConfigBase.Static == null)
					{
						m_shouldSetOtherToolbars = true;
						MyToolbarComponent.CurrentToolbar = self.Toolbar;
						MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
						MyToolbarComponent.AutoUpdate = false;
						myGuiScreenBase.Closed += delegate
						{
							MyToolbarComponent.AutoUpdate = true;
							m_openedToolbars.Clear();
						};
						MyGuiSandbox.AddScreen(myGuiScreenBase);
					}
				}));
				MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyButtonPanel>("ButtonText", MySpaceTexts.BlockPropertyText_ButtonList, MySpaceTexts.Blank)
				{
					ListContent = delegate(MyButtonPanel x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
					{
						x.FillListContent(list1, list2);
					},
					ItemSelected = delegate(MyButtonPanel x, List<MyGuiControlListbox.Item> y)
					{
						x.SelectButtonToName(y);
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyButtonPanel>("ButtonName", MySpaceTexts.BlockPropertyText_ButtonName, MySpaceTexts.Blank)
				{
					Getter = ((MyButtonPanel x) => x.GetButtonName()),
					Setter = delegate(MyButtonPanel x, StringBuilder v)
					{
						x.SetCustomButtonName(v);
					},
					SupportsMultipleBlocks = false
				});
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, 0.0001f, () => (!base.Enabled || !base.IsFunctional) ? 0f : 0.0001f);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.IsPoweredChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(builder, cubeGrid);
			m_emissiveNames = new List<string>(BlockDefinition.ButtonCount);
			for (int i = 1; i <= BlockDefinition.ButtonCount; i++)
			{
				m_emissiveNames.Add($"Emissive{i}");
			}
			MyObjectBuilder_ButtonPanel myObjectBuilder_ButtonPanel = builder as MyObjectBuilder_ButtonPanel;
			Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, Math.Min(BlockDefinition.ButtonCount, 9), BlockDefinition.ButtonCount / 9 + 1);
			Toolbar.DrawNumbers = false;
			Toolbar.GetSymbol = delegate(int slot)
			{
				ColoredIcon result = default(ColoredIcon);
				if (Toolbar.SlotToIndex(slot) < BlockDefinition.ButtonCount)
				{
					result.Icon = BlockDefinition.ButtonSymbols[Toolbar.SlotToIndex(slot) % BlockDefinition.ButtonSymbols.Length];
					Vector4 color = BlockDefinition.ButtonColors[Toolbar.SlotToIndex(slot) % BlockDefinition.ButtonColors.Length];
					color.W = 1f;
					result.Color = color;
				}
				return result;
			};
			Toolbar.Init(myObjectBuilder_ButtonPanel.Toolbar, this);
			Toolbar.ItemChanged += Toolbar_ItemChanged;
			m_anyoneCanUse.SetLocalValue(myObjectBuilder_ButtonPanel.AnyoneCanUse);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
			if (myObjectBuilder_ButtonPanel.CustomButtonNames != null)
			{
				foreach (int key in myObjectBuilder_ButtonPanel.CustomButtonNames.Dictionary.Keys)
				{
					m_customButtonNames.Dictionary.Add(key, MyStatControlText.SubstituteTexts(myObjectBuilder_ButtonPanel.CustomButtonNames[key]));
				}
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.UseObjectsComponent.GetInteractiveObjects(m_buttonsUseObjects);
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateEmissivity();
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (base.Components.TryGet(out MyContainerDropComponent component))
			{
				component.UpdateSound();
			}
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index)
		{
			if (!m_syncing)
			{
				ToolbarItem arg = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
				UpdateButtonEmissivity(index.ItemIndex);
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SendToolbarItemChanged, arg, index.ItemIndex);
				if (m_shouldSetOtherToolbars)
				{
					m_shouldSetOtherToolbars = false;
					foreach (MyToolbar openedToolbar in m_openedToolbars)
					{
						if (openedToolbar != self)
						{
							openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
						}
					}
					m_shouldSetOtherToolbars = true;
				}
				MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(index.ItemIndex);
				if (itemAtIndex != null)
				{
					string arg2 = itemAtIndex.DisplayName.ToString();
					MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, arg2, index.ItemIndex);
				}
				else
				{
					MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, MyTexts.GetString(MySpaceTexts.NotificationHintNoAction), index.ItemIndex);
				}
			}
		}

		private void UpdateEmissivity()
		{
			for (int i = 0; i < BlockDefinition.ButtonCount; i++)
			{
				UpdateButtonEmissivity(i);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.InScene)
			{
				UpdateEmissivity();
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
			m_buttonsUseObjects.Clear();
			base.UseObjectsComponent.GetInteractiveObjects(m_buttonsUseObjects);
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			UpdateEmissivity();
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void UpdateButtonEmissivity(int index)
		{
			if (!base.InScene)
			{
				return;
			}
			Vector4 vector = BlockDefinition.ButtonColors[index % BlockDefinition.ButtonColors.Length];
			if (Toolbar.GetItemAtIndex(index) == null)
			{
				vector = BlockDefinition.UnassignedButtonColor;
			}
			float emissivity = vector.W;
			if (!base.IsWorking)
			{
				if (base.IsFunctional)
				{
					vector = Color.Red.ToVector4();
					emissivity = 0f;
				}
				else
				{
					vector = Color.Black.ToVector4();
					emissivity = 0f;
				}
			}
			MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveNames[index], new Color(vector.X, vector.Y, vector.Z), emissivity);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ButtonPanel obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ButtonPanel;
			obj.Toolbar = Toolbar.GetObjectBuilder();
			obj.AnyoneCanUse = AnyoneCanUse;
			obj.CustomButtonNames = m_customButtonNames;
			return obj;
		}

		public void PressButton(int i)
		{
			if (this.ButtonPressed != null)
			{
				this.ButtonPressed(i);
			}
		}

		[Event(null, 322)]
		[Reliable]
		[Server(ValidationType.Access)]
		public void ActivateButton(int index)
		{
			Toolbar.UpdateItem(index);
			bool num = Toolbar.ActivateItemAtIndex(index);
			PressButton(index);
			if (!num)
			{
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.NotifyActivationFailed, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 332)]
		[Reliable]
		[Client]
		private void NotifyActivationFailed()
		{
			MyHud.Notifications.Add(m_activationFailedNotification);
		}

		protected override void Closing()
		{
			base.Closing();
			foreach (MyUseObjectPanelButton buttonsUseObject in m_buttonsUseObjects)
			{
				buttonsUseObject.RemoveButtonMarker();
			}
		}

		public void FillListContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			string @string = MyTexts.GetString(MySpaceTexts.BlockPropertyText_Button);
			for (int i = 0; i < m_buttonsUseObjects.Count; i++)
			{
				m_helperSB.Clear().Append(@string + " " + (i + 1));
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(m_helperSB, null, null, i);
				listBoxContent.Add(item);
				if (i == m_selectedButton)
				{
					listBoxSelectedItems.Add(item);
				}
			}
		}

		public void SelectButtonToName(List<MyGuiControlListbox.Item> imageIds)
		{
			m_selectedButton = (int)imageIds[0].UserData;
			RaisePropertiesChanged();
		}

		public StringBuilder GetButtonName()
		{
			if (m_selectedButton == -1)
			{
				return m_emptyName;
			}
			string value = null;
			if (!m_customButtonNames.Dictionary.TryGetValue(m_selectedButton, out value))
			{
				MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(m_selectedButton);
				if (itemAtIndex != null)
				{
					return itemAtIndex.DisplayName;
				}
				return m_emptyName;
			}
			return new StringBuilder(value);
		}

		[Event(null, 392)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void SetButtonName(string name, int position)
		{
			string value = null;
			if (name == null)
			{
				m_customButtonNames.Dictionary.Remove(position);
			}
			else if (m_customButtonNames.Dictionary.TryGetValue(position, out value))
			{
				m_customButtonNames.Dictionary[position] = name.ToString();
			}
			else
			{
				m_customButtonNames.Dictionary.Add(position, name.ToString());
			}
		}

		public bool IsButtonAssigned(int pos)
		{
			return Toolbar.GetItemAtIndex(pos) != null;
		}

		public bool HasCustomButtonName(int pos)
		{
			return m_customButtonNames.Dictionary.ContainsKey(pos);
		}

		public void SetCustomButtonName(string name, int pos)
		{
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, name, pos);
		}

		public void SetCustomButtonName(StringBuilder name)
		{
			if (m_selectedButton != -1)
			{
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, name.ToString(), m_selectedButton);
			}
		}

		public string GetCustomButtonName(int pos)
		{
			string value = null;
			if (!m_customButtonNames.Dictionary.TryGetValue(pos, out value))
			{
				MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(pos);
				if (itemAtIndex != null)
				{
					return itemAtIndex.DisplayName.ToString();
				}
				return MyTexts.GetString(MySpaceTexts.NotificationHintNoAction);
			}
			return value;
		}

		[Event(null, 449)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
		{
			m_syncing = true;
			MyToolbarItem item = null;
			if (sentItem.EntityID != 0L)
			{
				item = ToolbarItem.ToItem(sentItem);
			}
			Toolbar.SetItemAtIndex(index, item);
			UpdateButtonEmissivity(index);
			m_syncing = false;
		}

		string SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.GetButtonName(int index)
		{
			return GetCustomButtonName(index);
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.SetCustomButtonName(int index, string name)
		{
			SetCustomButtonName(name, index);
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.ClearCustomButtonName(int index)
		{
			SetCustomButtonName(null, index);
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.HasCustomButtonName(int index)
		{
			return HasCustomButtonName(index);
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.IsButtonAssigned(int index)
		{
			return IsButtonAssigned(index);
		}
	}
}
