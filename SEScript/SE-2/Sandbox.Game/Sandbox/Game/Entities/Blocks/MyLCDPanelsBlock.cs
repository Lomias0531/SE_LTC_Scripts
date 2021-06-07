using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LCDPanelsBlock))]
	public class MyLCDPanelsBlock : MyFunctionalBlock, IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner
	{
		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyLCDPanelsBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyLCDPanelsBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyLCDPanelsBlock, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyLCDPanelsBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyLCDPanelsBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyLCDPanelsBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyLCDPanelsBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLCDPanelsBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyLCDPanelsBlock_003C_003EActor : IActivator, IActivator<MyLCDPanelsBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyLCDPanelsBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLCDPanelsBlock CreateInstance()
			{
				return new MyLCDPanelsBlock();
			}

			MyLCDPanelsBlock IActivator<MyLCDPanelsBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBox;

		private bool m_isTextPanelOpen;

		public new MyLCDPanelsBlockDefinition BlockDefinition => (MyLCDPanelsBlockDefinition)base.BlockDefinition;

		int IMyTextSurfaceProvider.SurfaceCount
		{
			get
			{
				if (m_multiPanel == null)
				{
					return 0;
				}
				return m_multiPanel.SurfaceCount;
			}
		}

		MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => m_multiPanel;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_multiPanel == null)
				{
					return null;
				}
				return m_multiPanel.PanelComponent;
			}
		}

		public bool IsTextPanelOpen
		{
			get
			{
				return m_isTextPanelOpen;
			}
			set
			{
				if (m_isTextPanelOpen != value)
				{
					m_isTextPanelOpen = value;
					RaisePropertiesChanged();
				}
			}
		}

		public MyLCDPanelsBlock()
		{
			CreateTerminalControls();
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLCDPanelsBlock>())
			{
				base.CreateTerminalControls();
				MyMultiTextPanelComponent.CreateTerminalControls<MyLCDPanelsBlock>();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyLCDPanelsBlockDefinition blockDefinition = BlockDefinition;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, blockDefinition.RequiredPowerInput, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			myResourceSinkComponent.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			MyObjectBuilder_LCDPanelsBlock myObjectBuilder_LCDPanelsBlock = (MyObjectBuilder_LCDPanelsBlock)objectBuilder;
			if (blockDefinition.ScreenAreas != null && blockDefinition.ScreenAreas.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_multiPanel = new MyMultiTextPanelComponent(this, blockDefinition.ScreenAreas, myObjectBuilder_LCDPanelsBlock.TextPanels);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_multiPanel != null)
			{
				m_multiPanel.Reset();
			}
			if (base.ResourceSink != null)
			{
				UpdateScreen();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_LCDPanelsBlock myObjectBuilder_LCDPanelsBlock = (MyObjectBuilder_LCDPanelsBlock)base.GetObjectBuilderCubeBlock(copy);
			if (m_multiPanel != null)
			{
				myObjectBuilder_LCDPanelsBlock.TextPanels = m_multiPanel.Serialize();
			}
			return myObjectBuilder_LCDPanelsBlock;
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(CheckIsWorking());
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene();
			}
		}

		private void UpdateScreen()
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateScreen(CheckIsWorking());
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 223)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.RemoveItems(panelIndex, selection);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		[Event(null, 234)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.SelectItems(panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 245)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 259)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			}
			RaisePropertiesChanged();
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text.Clear().Append((object)description);
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 302)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			PanelComponent.Text.Clear().Append((object)stringBuilder);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			CreateTextBox(isEditable, new StringBuilder(PanelComponent.Text.ToString()), isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBox);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 329)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyLCDPanelsBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		[Event(null, 340)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBox = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBox != null)
			{
				if (m_textBox.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(isPublic: true);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBox.Description.Text.Remove(100000, m_textBox.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBox.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBox);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					SendChangeDescriptionMessage(m_textBox.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}
	}
}
