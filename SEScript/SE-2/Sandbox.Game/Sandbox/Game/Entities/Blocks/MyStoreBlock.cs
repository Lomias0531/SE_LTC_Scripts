using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_StoreBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyStoreBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyStoreBlock)
	})]
	public class MyStoreBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, IMyInventoryOwner, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, Sandbox.ModAPI.IMyStoreBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
	{
		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyStoreBlock, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyStoreBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyStoreBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyStoreBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyStoreBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyStoreBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyStoreBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class GetStoreItems_003C_003E : ICallSite<MyStoreBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetStoreItems();
			}
		}

		protected sealed class OnGetStoreItemsResult_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_Blocks_MyStoreItem_003E_0023System_Int64_0023System_Single_0023System_Single : ICallSite<MyStoreBlock, List<MyStoreItem>, long, float, float, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in List<MyStoreItem> storeItems, in long lastEconomyTick, in float offersBonus, in float ordersBonus, in DBNull arg5, in DBNull arg6)
			{
				@this.OnGetStoreItemsResult(storeItems, lastEconomyTick, offersBonus, ordersBonus);
			}
		}

		protected sealed class OnPlayerStoreItemsChanged_Broacast_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_Blocks_MyStoreItem_003E_0023System_Int64 : ICallSite<MyStoreBlock, List<MyStoreItem>, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in List<MyStoreItem> storeItems, in long lastEconomyTick, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnPlayerStoreItemsChanged_Broacast(storeItems, lastEconomyTick);
			}
		}

		protected sealed class BuyItem_003C_003ESystem_Int64_0023System_Int32_0023System_Int64_0023System_Int64 : ICallSite<MyStoreBlock, long, int, long, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in long id, in int amount, in long targetEntityId, in long lastEconomyTick, in DBNull arg5, in DBNull arg6)
			{
				@this.BuyItem(id, amount, targetEntityId, lastEconomyTick);
			}
		}

		protected sealed class OnBuyItemResult_003C_003ESandbox_Game_Entities_Blocks_MyStoreBuyItemResult : ICallSite<MyStoreBlock, MyStoreBuyItemResult, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in MyStoreBuyItemResult result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnBuyItemResult(result);
			}
		}

		protected sealed class SellItem_003C_003ESystem_Int64_0023System_Int32_0023System_Int64_0023System_Int64 : ICallSite<MyStoreBlock, long, int, long, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in long id, in int amount, in long sourceEntityId, in long lastEconomyTick, in DBNull arg5, in DBNull arg6)
			{
				@this.SellItem(id, amount, sourceEntityId, lastEconomyTick);
			}
		}

		protected sealed class OnSellItemResult_003C_003ESandbox_Game_Entities_Blocks_MyStoreSellItemResult : ICallSite<MyStoreBlock, MyStoreSellItemResult, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in MyStoreSellItemResult result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSellItemResult(result);
			}
		}

		protected sealed class GetConnectedGridInventories_003C_003E : ICallSite<MyStoreBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetConnectedGridInventories();
			}
		}

		protected sealed class OnGetConnectedGridInventoriesResult_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MyStoreBlock, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in List<long> inventories, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnGetConnectedGridInventoriesResult(inventories);
			}
		}

		protected sealed class GetGridInventories_003C_003E : ICallSite<MyStoreBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetGridInventories();
			}
		}

		protected sealed class OnGetGridInventoriesResult_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MyStoreBlock, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in List<long> inventories, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnGetGridInventoriesResult(inventories);
			}
		}

		protected sealed class CreateNewOffer_003C_003EVRage_ObjectBuilders_SerializableDefinitionId_0023System_Int32_0023System_Int32 : ICallSite<MyStoreBlock, SerializableDefinitionId, int, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in SerializableDefinitionId itemId, in int amount, in int pricePerUnit, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateNewOffer(itemId, amount, pricePerUnit);
			}
		}

		protected sealed class OnCreateNewOfferResult_003C_003ESandbox_Game_Entities_Blocks_MyStoreCreationResult : ICallSite<MyStoreBlock, MyStoreCreationResult, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in MyStoreCreationResult result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnCreateNewOfferResult(result);
			}
		}

		protected sealed class CreateNewOrder_003C_003EVRage_ObjectBuilders_SerializableDefinitionId_0023System_Int32_0023System_Int32 : ICallSite<MyStoreBlock, SerializableDefinitionId, int, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in SerializableDefinitionId itemId, in int amount, in int pricePerUnit, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateNewOrder(itemId, amount, pricePerUnit);
			}
		}

		protected sealed class OnCreateNewOrderResult_003C_003ESandbox_Game_Entities_Blocks_MyStoreCreationResult : ICallSite<MyStoreBlock, MyStoreCreationResult, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in MyStoreCreationResult result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnCreateNewOrderResult(result);
			}
		}

		protected sealed class CancelStoreItemServer_003C_003ESystem_Int64 : ICallSite<MyStoreBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in long id, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CancelStoreItemServer(id);
			}
		}

		protected sealed class OnCancelStoreItemResult_003C_003ESystem_Boolean : ICallSite<MyStoreBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in bool result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnCancelStoreItemResult(result);
			}
		}

		protected sealed class ChangeBalance_003C_003ESystem_Int32_0023System_Int64 : ICallSite<MyStoreBlock, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in int amount, in long targetEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ChangeBalance(amount, targetEntityId);
			}
		}

		protected sealed class OnChangeBalanceResult_003C_003ESandbox_Game_Entities_Blocks_MyStoreBuyItemResults : ICallSite<MyStoreBlock, MyStoreBuyItemResults, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in MyStoreBuyItemResults result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeBalanceResult(result);
			}
		}

		protected sealed class ShowPreviewImplementation_003C_003ESystem_Int64 : ICallSite<MyStoreBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyStoreBlock @this, in long storeItemId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShowPreviewImplementation(storeItemId);
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyStoreBlock)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		protected class m_anyoneCanUse_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType anyoneCanUse;
				ISyncType result = anyoneCanUse = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyStoreBlock)P_0).m_anyoneCanUse = (Sync<bool, SyncDirection.BothWays>)anyoneCanUse;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyStoreBlock_003C_003EActor : IActivator, IActivator<MyStoreBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyStoreBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyStoreBlock CreateInstance()
			{
				return new MyStoreBlock();
			}

			MyStoreBlock IActivator<MyStoreBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private Action<List<MyStoreItem>, long, float, float> m_localRequestStoreItemsCallback;

		private Action<MyStoreBuyItemResult> m_localRequestBuyCallback;

		private readonly Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private Action<MyStoreSellItemResult> m_localRequestSellItemCallback;

		private Action<List<long>> m_localRequestConnectedInventoriesItemCallback;

		private Action<List<long>> m_localRequestInventoriesItemCallback;

		private Action<MyStoreCreationResult> m_localRequestCreateOfferCallback;

		private Action<MyStoreCreationResult> m_localRequestCreateOrderCallback;

		private Action<bool> m_localRequestCancelStoreItemCallback;

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBoxMultiPanel;

		private bool m_isTextPanelOpen;

		private MyResourceStateEnum m_currentState = MyResourceStateEnum.NoPower;

		private Action<MyStoreBuyItemResults> m_localRequestChangeBalanceCallback;

		int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount
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

		protected virtual bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

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

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public List<MyStoreItem> PlayerItems
		{
			get;
			private set;
		}

		public new MyStoreBlockDefinition BlockDefinition => (MyStoreBlockDefinition)base.BlockDefinition;

		public override MyCubeBlockHighlightModes HighlightMode
		{
			get
			{
				if (AnyoneCanUse)
				{
					return MyCubeBlockHighlightModes.AlwaysCanUse;
				}
				return MyCubeBlockHighlightModes.Default;
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

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		public MyStoreBlock()
		{
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_useConveyorSystem.SetLocalValue(newValue: true);
			MyObjectBuilder_StoreBlock myObjectBuilder_StoreBlock = objectBuilder as MyObjectBuilder_StoreBlock;
			m_anyoneCanUse.SetLocalValue(myObjectBuilder_StoreBlock.AnyoneCanUse);
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_StoreBlock.UseConveyorSystem);
			if (myObjectBuilder_StoreBlock.PlayerItems != null)
			{
				PlayerItems = new List<MyStoreItem>(myObjectBuilder_StoreBlock.PlayerItems.Count);
				foreach (MyObjectBuilder_StoreItem playerItem in myObjectBuilder_StoreBlock.PlayerItems)
				{
					PlayerItems.Add(new MyStoreItem(playerItem));
				}
			}
			else
			{
				PlayerItems = new List<MyStoreItem>();
			}
			InitializeConveyorEndpoint();
			if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_multiPanel = new MyMultiTextPanelComponent(this, BlockDefinition.ScreenAreas, myObjectBuilder_StoreBlock.TextPanels);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return m_currentState == MyResourceStateEnum.Ok;
			}
			return false;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_StoreBlock myObjectBuilder_StoreBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_StoreBlock;
			myObjectBuilder_StoreBlock.AnyoneCanUse = AnyoneCanUse;
			if (PlayerItems != null)
			{
				myObjectBuilder_StoreBlock.PlayerItems = new List<MyObjectBuilder_StoreItem>(PlayerItems.Count);
				foreach (MyStoreItem playerItem in PlayerItems)
				{
					myObjectBuilder_StoreBlock.PlayerItems.Add(playerItem.GetObjectBuilder());
				}
			}
			else
			{
				myObjectBuilder_StoreBlock.PlayerItems = new List<MyObjectBuilder_StoreItem>();
			}
			if (m_multiPanel != null)
			{
				myObjectBuilder_StoreBlock.TextPanels = m_multiPanel.Serialize();
			}
			myObjectBuilder_StoreBlock.UseConveyorSystem = m_useConveyorSystem;
			return myObjectBuilder_StoreBlock;
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
			UpdateScreen();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(CheckIsWorking());
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				MyResourceStateEnum currentState = m_currentState;
				m_currentState = base.CubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
				if (currentState != m_currentState)
				{
					UpdateIsWorking();
				}
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null && m_multiPanel.SurfaceCount > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void UpdateScreen()
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateScreen(CheckIsWorking());
			}
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 317)]
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
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 333)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 349)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.RemoveItems(panelIndex, selection);
			}
		}

		[Event(null, 360)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectItems(panelIndex, selection);
			}
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			}
			RaisePropertiesChanged();
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
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBoxMultiPanel);
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBoxMultiPanel != null)
			{
				if (m_textBoxMultiPanel.Description.Text.Length > 100000)
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
				m_textBoxMultiPanel.Description.Text.Remove(100000, m_textBoxMultiPanel.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBoxMultiPanel.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBoxMultiPanel);
			}
		}

		[Event(null, 448)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 459)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
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
					SendChangeDescriptionMessage(m_textBoxMultiPanel.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text.Clear().Append((object)description);
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 516)]
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

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyStoreBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyStoreBlock> obj = new MyTerminalControlCheckbox<MyStoreBlock>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
				{
					Getter = ((MyStoreBlock x) => x.AnyoneCanUse),
					Setter = delegate(MyStoreBlock x, bool v)
					{
						x.AnyoneCanUse = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlOnOffSwitch<MyStoreBlock> obj2 = new MyTerminalControlOnOffSwitch<MyStoreBlock>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MyStoreBlock x) => x.UseConveyorSystem),
					Setter = delegate(MyStoreBlock x, bool v)
					{
						x.UseConveyorSystem = v;
					},
					Visible = ((MyStoreBlock x) => x.HasInventory)
				};
				obj2.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj2);
				MyMultiTextPanelComponent.CreateTerminalControls<MyStoreBlock>();
			}
		}

		public void CreateGetStoreItemsRequest(long identityId, Action<List<MyStoreItem>, long, float, float> resultCallback)
		{
			m_localRequestStoreItemsCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.GetStoreItems);
		}

		[Event(null, 556)]
		[Reliable]
		[Server]
		private void GetStoreItems()
		{
			if (!HasAccess())
			{
				return;
			}
			MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
			if (stationByGridId != null)
			{
				GetStoreItemsForStation(stationByGridId);
				return;
			}
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				string message = "GetStoreItems - Economy session component not found.";
				MyLog.Default.WriteToLogAndAssert(message);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnGetStoreItemsResult, PlayerItems, component.LastEconomyTick.Ticks, 1f, 1f, MyEventContext.Current.Sender);
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnPlayerStoreItemsChanged_Broacast, PlayerItems, component.LastEconomyTick.Ticks, MyEventContext.Current.Sender);
			}
		}

		private void GetStoreItemsForStation(MyStation station)
		{
			if (MySession.Static.Factions.TryGetFactionById(station.FactionId) == null)
			{
				string message = "GetStoreItemsForStation - Faction not found.";
				MyLog.Default.WriteToLogAndAssert(message);
				return;
			}
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				MyLog.Default.WriteToLogAndAssert("GetStoreItemsForStation - Economy session component not found.");
				return;
			}
			float arg = 1f;
			float arg2 = 1f;
			long playerId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, station.FactionId);
			if (relationBetweenPlayerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
			{
				int item = relationBetweenPlayerAndFaction.Item2;
				arg2 = component.GetOffersFriendlyBonus(item);
				arg = component.GetOrdersFriendlyBonus(item);
			}
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnGetStoreItemsResult, station.StoreItems, component.LastEconomyTick.Ticks, arg2, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 621)]
		[Reliable]
		[Client]
		private void OnGetStoreItemsResult(List<MyStoreItem> storeItems, long lastEconomyTick, float offersBonus, float ordersBonus)
		{
			m_localRequestStoreItemsCallback?.Invoke(storeItems, lastEconomyTick, offersBonus, ordersBonus);
			m_localRequestStoreItemsCallback = null;
		}

		[Event(null, 628)]
		[Reliable]
		[BroadcastExcept]
		private void OnPlayerStoreItemsChanged_Broacast(List<MyStoreItem> storeItems, long lastEconomyTick)
		{
			OnPlayerStoreItemsChanged(storeItems, lastEconomyTick);
		}

		protected virtual void OnPlayerStoreItemsChanged(List<MyStoreItem> storeItems, long lastEconomyTick)
		{
		}

		internal void CreateBuyRequest(long id, int amount, long targetEntityId, long lastEconomyTick, Action<MyStoreBuyItemResult> resultCallback)
		{
			m_localRequestBuyCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.BuyItem, id, amount, targetEntityId, lastEconomyTick);
		}

		[Event(null, 645)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void BuyItem(long id, int amount, long targetEntityId, long lastEconomyTick)
		{
			if (!HasAccess() || amount <= 0)
			{
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MyPlayer player = GetPlayer(num);
			if (player == null || player.Character == null)
			{
				MyLog.Default.WriteToLogAndAssert("BuyItem - Player not found.");
				return;
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(num, out MyAccountInfo account))
			{
				string message = "BuyItem - Player does not have account.";
				MyLog.Default.WriteToLogAndAssert(message);
				return;
			}
			MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
			if (stationByGridId == null)
			{
				BuyFromPlayer(id, amount, targetEntityId, player, account);
			}
			else
			{
				BuyFromStation(id, amount, player, account, stationByGridId, targetEntityId, lastEconomyTick);
			}
		}

		private void BuyFromPlayer(long id, int amount, long targetEntityId, MyPlayer player, MyAccountInfo playerAccountInfo)
		{
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(targetEntityId, out entity))
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Entity not found.");
				return;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = (entity as MyCubeBlock)) != null && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Player is not big owner of the grid.");
				return;
			}
			if (entity is MyCharacter && player.Character != entity)
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Player entity and inventory entity is different.");
				return;
			}
			if (!entity.TryGetInventory(out MyInventory inventory))
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Inventory not found.");
				return;
			}
			MyStoreItem myStoreItem = null;
			foreach (MyStoreItem playerItem in PlayerItems)
			{
				if (playerItem.Id == id)
				{
					myStoreItem = playerItem;
					break;
				}
			}
			if (myStoreItem == null)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.ItemNotFound);
				return;
			}
			if (amount > myStoreItem.Amount)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.WrongAmount);
				return;
			}
			long num = (long)myStoreItem.PricePerUnit * (long)amount;
			if (num > playerAccountInfo.Balance)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughMoney);
				return;
			}
			if (num < 0)
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Wrong price for the item.");
				return;
			}
			switch (myStoreItem.ItemType)
			{
			case ItemTypes.Oxygen:
			case ItemTypes.Hydrogen:
			case ItemTypes.Grid:
				break;
			case ItemTypes.PhysicalItem:
				BuyPhysicalItemFromPlayer(id, amount, player, inventory, myStoreItem, num);
				break;
			}
		}

		private void BuyPhysicalItemFromPlayer(long id, int amount, MyPlayer player, MyInventory inventory, MyStoreItem storeItem, long totalPrice)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				MyLog.Default.WriteToLogAndAssert("BuyPhysicalItemFromPlayer - Economy session component not found.");
				return;
			}
			if (!inventory.CheckConstraint(storeItem.Item.Value))
			{
				MyLog.Default.WriteToLogAndAssert("BuyPhysicalItemFromPlayer - Item can not be transfered to this inventory.");
				return;
			}
			if (!inventory.CanItemsBeAdded(amount, storeItem.Item.Value))
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughInventorySpace);
				return;
			}
			bool flag = false;
			if (UseConveyorSystem)
			{
				flag = base.CubeGrid.GridSystems.ConveyorSystem.PullItem(storeItem.Item.Value, amount, this, inventory, MyFakes.CONV_PULL_CACL_IMMIDIATLY_STORE_SAFEZONE);
			}
			bool flag2 = true;
			if (!flag)
			{
				MyInventory inventory2 = this.GetInventory();
				if (inventory2 != null)
				{
					MyFixedPoint itemAmount = inventory2.GetItemAmount(storeItem.Item.Value);
					if (amount > itemAmount)
					{
						SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughAmount);
						return;
					}
					inventory2.RemoveItemsOfType(amount, storeItem.Item.Value);
				}
				else
				{
					flag2 = false;
				}
				MyObjectBuilder_Base objectBuilder = MyObjectBuilderSerializer.CreateNewObject(storeItem.Item.Value);
				inventory.AddItems(amount, objectBuilder);
			}
			if (flag2)
			{
				storeItem.Amount -= amount;
			}
			if (storeItem.OnTransaction != null)
			{
				storeItem.OnTransaction(amount, storeItem.Amount, totalPrice, base.OwnerId, player.Identity.IdentityId);
			}
			if (storeItem.Amount == 0)
			{
				PlayerItems.Remove(storeItem);
			}
			MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddCurrencyDestroyed((long)((float)totalPrice * component.EconomyDefinition.TransactionFee));
			MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
			totalPrice = (long)((float)totalPrice * (1f - component.EconomyDefinition.TransactionFee));
			if (flag2 && base.OwnerId != 0L)
			{
				MyBankingSystem.ChangeBalance(base.OwnerId, totalPrice);
			}
			SendBuyItemResult(id, storeItem.Item.Value.SubtypeName, totalPrice, amount, MyStoreBuyItemResults.Success);
			OnItemBought(inventory, storeItem.Item.Value, totalPrice, amount);
		}

		protected virtual void OnItemBought(MyInventory inventory, MyDefinitionId definitionId, long totalPrice, int amount)
		{
		}

		private void BuyFromStation(long id, int amount, MyPlayer player, MyAccountInfo playerAccountInfo, MyStation station, long targetEntityId, long lastEconomyTick)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromStation - Economy session component not found.");
				return;
			}
			if (lastEconomyTick != component.LastEconomyTick.Ticks)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.ItemsTimeout);
				return;
			}
			Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(player.Identity.IdentityId, station.FactionId);
			float num = 1f;
			if (relationBetweenPlayerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
			{
				num = component.GetOffersFriendlyBonus(relationBetweenPlayerAndFaction.Item2);
			}
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(targetEntityId, out entity))
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromStation - Entity not found.");
				return;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = (entity as MyCubeBlock)) != null && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromStation - Player is not big owner of the grid.");
				return;
			}
			if (entity is MyCharacter && player.Character != entity)
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromStation - Player entity and inventory entity is different.");
				return;
			}
			if (!entity.TryGetInventory(out MyInventory inventory))
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromStation - Inventory not found.");
				return;
			}
			MyStoreItem storeItemById = station.GetStoreItemById(id);
			if (storeItemById == null)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.ItemNotFound);
				return;
			}
			if (amount > storeItemById.Amount)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.WrongAmount);
				return;
			}
			long num2 = (long)((float)((long)storeItemById.PricePerUnit * (long)amount) * num);
			if (num2 > playerAccountInfo.Balance)
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughMoney);
				return;
			}
			if (num2 < 0)
			{
				MyLog.Default.WriteToLogAndAssert("BuyFromStation - Wrong price for the item.");
				return;
			}
			switch (storeItemById.ItemType)
			{
			case ItemTypes.PhysicalItem:
				BuyPhysicalItem(id, amount, player, station, inventory, storeItemById, num2);
				break;
			case ItemTypes.Oxygen:
				BuyGas(storeItemById, amount, player, station, entity, num2, MyCharacterOxygenComponent.OxygenId);
				break;
			case ItemTypes.Hydrogen:
				BuyGas(storeItemById, amount, player, station, entity, num2, MyCharacterOxygenComponent.HydrogenId);
				break;
			case ItemTypes.Grid:
				BuyPrefab(storeItemById, amount, player, station, entity, num2);
				break;
			}
		}

		private void BuyPhysicalItem(long id, int amount, MyPlayer player, MyStation station, MyInventory inventory, MyStoreItem storeItem, long totalPrice)
		{
			if (!inventory.CheckConstraint(storeItem.Item.Value))
			{
				MyLog.Default.WriteToLogAndAssert("BuyPhysicalItem - Item can not be transfered to this inventory.");
				return;
			}
			if (!inventory.CanItemsBeAdded(amount, storeItem.Item.Value))
			{
				SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughInventorySpace);
				return;
			}
			for (int i = 0; i < amount; i++)
			{
				MyObjectBuilder_Base myObjectBuilder_Base = MyObjectBuilderSerializer.CreateNewObject(storeItem.Item.Value);
				MyObjectBuilder_Datapad datapad;
				if ((datapad = (myObjectBuilder_Base as MyObjectBuilder_Datapad)) != null)
				{
					IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(station.FactionId);
					if (myFaction == null)
					{
						MyLog.Default.WriteToLogAndAssert("BuyPhysicalItem - Faction not found.");
						return;
					}
					if (!MySession.Static.Factions.GetRandomFriendlyStation(myFaction.FactionId, station.Id, out MyFaction friendlyFaction, out MyStation friendlyStation, includeSameFaction: true))
					{
						MyLog.Default.WriteToLogAndAssert("BuyPhysicalItem - Friendly Station not found.");
						return;
					}
					MySessionComponentEconomy.PrepareDatapad(ref datapad, friendlyFaction, friendlyStation);
				}
				inventory.AddItems(1, myObjectBuilder_Base);
			}
			storeItem.Amount -= amount;
			MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
			MyBankingSystem.ChangeBalance(station.FactionId, totalPrice);
			SendBuyItemResult(id, storeItem.Item.Value.SubtypeName, totalPrice, amount, MyStoreBuyItemResults.Success);
		}

		private void BuyPrefab(MyStoreItem storeItem, int amount, MyPlayer player, MyStation station, MyEntity entity, long totalPrice)
		{
			bool flag = true;
			switch (MySession.Static.BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.GLOBALLY:
			case MyBlockLimitsEnabledEnum.PER_FACTION:
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				flag = (player.Identity.BlockLimits.PCU >= storeItem.PrefabTotalPcu || MySession.Static.TotalPCU == 0);
				break;
			default:
				flag = false;
				break;
			case MyBlockLimitsEnabledEnum.NONE:
				break;
			}
			if (!flag)
			{
				SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.NotEnoughPCU);
				return;
			}
			MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(storeItem.PrefabName);
			MyEntities.TryGetEntityById(station.SafeZoneEntityId, out MyEntity entity2);
			Vector3D vector3D = Vector3D.Forward;
			Vector3D vector3D2 = Vector3D.Up;
			Vector3D? vector3D3 = null;
			BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
			BoundingBox box = BoundingBox.CreateInvalid();
			MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
			for (int i = 0; i < cubeGrids.Length; i++)
			{
				BoundingBox box2 = cubeGrids[i].CalculateBoundingBox();
				box.Include(box2);
			}
			boundingSphere = BoundingSphere.CreateFromBoundingBox(box);
			if (station.Type != MyStationTypeEnum.Outpost)
			{
				vector3D3 = MyEntities.FindFreePlace(station.Position, 60f, 20, 5, 1f, entity2);
			}
			else
			{
				MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(station.Position);
				SpawnInfo spawnInfo = default(SpawnInfo);
				spawnInfo.CollisionRadius = boundingSphere.Radius;
				spawnInfo.Planet = closestPlanet;
				spawnInfo.PlanetDeployAltitude = boundingSphere.Radius * 1.05f;
				SpawnInfo info = spawnInfo;
				vector3D3 = MyRespawnComponentBase.FindPositionAbovePlanet(station.Position, ref info, testFreeZone: false, 1, 30, 60f, entity2);
				vector3D2 = station.Position - closestPlanet.PositionComp.GetPosition();
				vector3D2.Normalize();
				vector3D = Vector3D.CalculatePerpendicularVector(vector3D2);
			}
			if (!vector3D3.HasValue)
			{
				SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.FreePositionNotFound);
				return;
			}
			Vector3D vector3D4 = Vector3D.TransformNormal(boundingSphere.Center, MatrixD.CreateWorld(vector3D3.Value, vector3D, vector3D2));
			Vector3D position = vector3D3.Value - vector3D4;
			MyRenderProxy.DebugDrawSphere(vector3D4, 0.1f, Color.Green, 1f, depthRead: false, smooth: false, cull: true, persistent: true);
			MySpawnPrefabProperties spawnPrefabProperties = new MySpawnPrefabProperties
			{
				Position = position,
				Forward = vector3D,
				Up = vector3D2,
				SpawningOptions = (SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SetAuthorship | SpawningOptions.UseOnlyWorldMatrix),
				OwnerId = player.Identity.IdentityId,
				UpdateSync = true,
				PrefabName = storeItem.PrefabName
			};
			EndpointId targetEndpoint = MyEventContext.Current.Sender;
			MyPrefabManager.Static.SpawnPrefabInternal(spawnPrefabProperties, delegate
			{
				storeItem.Amount -= amount;
				MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
				MyBankingSystem.ChangeBalance(station.FactionId, totalPrice);
				SendBuyItemResult(storeItem.Id, storeItem.PrefabName, totalPrice, amount, MyStoreBuyItemResults.Success, targetEndpoint);
			}, delegate
			{
				SendBuyItemResult(storeItem.Id, storeItem.PrefabName, totalPrice, amount, MyStoreBuyItemResults.SpawnFailed, targetEndpoint);
			});
		}

		private void BuyGas(MyStoreItem storeItem, int amount, MyPlayer player, MyStation station, MyEntity entity, long totalPrice, MyDefinitionId gasId)
		{
			float num = (float)amount * 1000f;
			if (entity == player.Character && player.Character.OxygenComponent != null)
			{
				player.Character.OxygenComponent.TransferSuitGas(ref gasId, num, 0f);
			}
			else
			{
				MyGasTank myGasTank;
				if ((myGasTank = (entity as MyGasTank)) == null || !(myGasTank.BlockDefinition.StoredGasId == gasId))
				{
					SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.WrongInventory);
					return;
				}
				double num2 = (1.0 - myGasTank.FilledRatio) * (double)myGasTank.Capacity;
				if ((double)num > num2)
				{
					SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.NotEnoughSpaceInTank);
					return;
				}
				myGasTank.Transfer(num);
			}
			storeItem.Amount -= amount;
			MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
			MyBankingSystem.ChangeBalance(station.FactionId, totalPrice);
			SendBuyItemResult(storeItem.Id, gasId.SubtypeName, totalPrice, amount, MyStoreBuyItemResults.Success);
		}

		private void SendBuyItemResult(long id, int amount, MyStoreBuyItemResults result)
		{
			SendBuyItemResult(id, string.Empty, 0L, amount, result, MyEventContext.Current.Sender);
		}

		private void SendBuyItemResult(long id, string name, long price, int amount, MyStoreBuyItemResults result)
		{
			SendBuyItemResult(id, name, price, amount, result, MyEventContext.Current.Sender);
		}

		private void SendBuyItemResult(long id, string name, long price, int amount, MyStoreBuyItemResults result, EndpointId targetEndpoint)
		{
			MyStoreBuyItemResult arg = new MyStoreBuyItemResult
			{
				Result = result,
				ItemId = id,
				Amount = amount
			};
			MyLog.Default.WriteLine($"SendBuyItemResult - {result}, {id}, {name}, {price}, {amount}, {targetEndpoint.Value}");
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnBuyItemResult, arg, targetEndpoint);
		}

		[Event(null, 1161)]
		[Reliable]
		[Client]
		private void OnBuyItemResult(MyStoreBuyItemResult result)
		{
			m_localRequestBuyCallback?.Invoke(result);
			m_localRequestBuyCallback = null;
		}

		public void CreateSellItemRequest(long id, int amount, long sourceEntityId, long lastEconomyTick, Action<MyStoreSellItemResult> resultCallback)
		{
			m_localRequestSellItemCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.SellItem, id, amount, sourceEntityId, lastEconomyTick);
		}

		[Event(null, 1174)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void SellItem(long id, int amount, long sourceEntityId, long lastEconomyTick)
		{
			if (!HasAccess() || amount <= 0)
			{
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MyPlayer player = GetPlayer(num);
			if (!MyBankingSystem.Static.TryGetAccountInfo(num, out MyAccountInfo _))
			{
				MyLog.Default.WriteToLogAndAssert("SellItem - Player does not have account.");
				return;
			}
			MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
			if (stationByGridId == null)
			{
				SellToPlayer(id, amount, sourceEntityId, player);
			}
			else
			{
				SellToStation(id, amount, player, stationByGridId, sourceEntityId, lastEconomyTick);
			}
		}

		private void SellToPlayer(long id, int amount, long sourceEntityId, MyPlayer player)
		{
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(sourceEntityId, out entity))
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Entity not found.");
				return;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = (entity as MyCubeBlock)) != null && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Player is not big owner of the grid.");
				return;
			}
			if (entity is MyCharacter && player.Character != entity)
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Player entity and inventory entity is different.");
				return;
			}
			if (!entity.TryGetInventory(out MyInventory inventory))
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Inventory not found.");
				return;
			}
			if (player == null || player.Character == null)
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Player not found.");
				return;
			}
			MyStoreItem myStoreItem = null;
			foreach (MyStoreItem playerItem in PlayerItems)
			{
				if (playerItem.Id == id)
				{
					myStoreItem = playerItem;
					break;
				}
			}
			if (myStoreItem == null)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.ItemNotFound);
				return;
			}
			if (amount > myStoreItem.Amount)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.WrongAmount);
				return;
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(base.OwnerId, out MyAccountInfo account))
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Owner does not have an account.");
				return;
			}
			long num = (long)myStoreItem.PricePerUnit * (long)amount;
			if (num > account.Balance)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughMoney);
				return;
			}
			if (num < 0)
			{
				MyLog.Default.WriteToLogAndAssert("SellToPlayer - Wrong price for the item.");
				return;
			}
			switch (myStoreItem.ItemType)
			{
			case ItemTypes.Oxygen:
			case ItemTypes.Hydrogen:
			case ItemTypes.Grid:
				break;
			case ItemTypes.PhysicalItem:
			{
				MyFixedPoint itemAmount = inventory.GetItemAmount(myStoreItem.Item.Value);
				if (amount > itemAmount)
				{
					SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughAmount);
					break;
				}
				if (UseConveyorSystem)
				{
					if (!base.CubeGrid.GridSystems.ConveyorSystem.PushGenerateItem(myStoreItem.Item.Value, amount, this, partialPush: false, calcImmediately: true))
					{
						MyInventory inventory2 = this.GetInventory();
						if (!inventory2.CanItemsBeAdded(amount, myStoreItem.Item.Value))
						{
							SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughInventorySpace);
							break;
						}
						MyObjectBuilder_Base objectBuilder = MyObjectBuilderSerializer.CreateNewObject(myStoreItem.Item.Value);
						inventory2.AddItems(amount, objectBuilder);
					}
				}
				else
				{
					MyInventory inventory3 = this.GetInventory();
					if (!inventory3.CanItemsBeAdded(amount, myStoreItem.Item.Value))
					{
						SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughInventorySpace);
						break;
					}
					MyObjectBuilder_Base objectBuilder2 = MyObjectBuilderSerializer.CreateNewObject(myStoreItem.Item.Value);
					inventory3.AddItems(amount, objectBuilder2);
				}
				inventory.RemoveItemsOfType(amount, myStoreItem.Item.Value);
				myStoreItem.Amount -= amount;
				if (myStoreItem.OnTransaction != null)
				{
					myStoreItem.OnTransaction(amount, myStoreItem.Amount, num, base.OwnerId, player.Identity.IdentityId);
				}
				MyBankingSystem.ChangeBalance(player.Identity.IdentityId, num);
				MyBankingSystem.ChangeBalance(base.OwnerId, -num);
				SendSellItemResult(id, myStoreItem.Item.Value.SubtypeName, num, amount, MyStoreSellItemResults.Success);
				break;
			}
			}
		}

		private void SellToStation(long id, int amount, MyPlayer player, MyStation station, long sourceEntityId, long lastEconomyTick)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Economy session component not found.");
				return;
			}
			if (lastEconomyTick != component.LastEconomyTick.Ticks)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.ItemsTimeout);
				return;
			}
			Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(player.Identity.IdentityId, station.FactionId);
			float num = 1f;
			if (relationBetweenPlayerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
			{
				num = component.GetOrdersFriendlyBonus(relationBetweenPlayerAndFaction.Item2);
			}
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(sourceEntityId, out entity))
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Entity not found.");
				return;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = (entity as MyCubeBlock)) != null && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Player is not big owner of the grid.");
				return;
			}
			if (entity is MyCharacter && player.Character != entity)
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Player entity and inventory entity is different.");
				return;
			}
			if (!entity.TryGetInventory(out MyInventory inventory))
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Inventory not found.");
				return;
			}
			if (player == null || player.Character == null)
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Player not found.");
				return;
			}
			MyStoreItem storeItemById = station.GetStoreItemById(id);
			if (storeItemById == null)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.ItemNotFound);
				return;
			}
			if (amount > storeItemById.Amount)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.WrongAmount);
				return;
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(station.FactionId, out MyAccountInfo account))
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Owner does not have account.");
				return;
			}
			long num2 = (long)((float)((long)storeItemById.PricePerUnit * (long)amount) * num);
			if (num2 > account.Balance)
			{
				SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughMoney);
				return;
			}
			if (num2 < 0)
			{
				MyLog.Default.WriteToLogAndAssert("SellToStation - Wrong price for the item.");
				return;
			}
			switch (storeItemById.ItemType)
			{
			case ItemTypes.Oxygen:
			case ItemTypes.Hydrogen:
			case ItemTypes.Grid:
				break;
			case ItemTypes.PhysicalItem:
			{
				MyFixedPoint itemAmount = inventory.GetItemAmount(storeItemById.Item.Value);
				if (amount > itemAmount)
				{
					SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughAmount);
					break;
				}
				inventory.RemoveItemsOfType(amount, storeItemById.Item.Value);
				storeItemById.Amount -= amount;
				MyBankingSystem.ChangeBalance(player.Identity.IdentityId, num2);
				MyBankingSystem.ChangeBalance(station.FactionId, -num2);
				SendSellItemResult(id, storeItemById.Item.Value.SubtypeName, num2, amount, MyStoreSellItemResults.Success);
				break;
			}
			}
		}

		private void SendSellItemResult(long id, int amount, MyStoreSellItemResults result)
		{
			SendSellItemResult(id, string.Empty, 0L, amount, result);
		}

		private void SendSellItemResult(long id, string name, long price, int amount, MyStoreSellItemResults result)
		{
			MyStoreSellItemResult arg = new MyStoreSellItemResult
			{
				Result = result,
				ItemId = id,
				Amount = amount
			};
			MyLog.Default.WriteLine($"SendSellItemResult - {result}, {id}, {name}, {price}, {amount}, {MyEventContext.Current.Sender.Value}");
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnSellItemResult, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 1485)]
		[Reliable]
		[Client]
		private void OnSellItemResult(MyStoreSellItemResult result)
		{
			m_localRequestSellItemCallback?.Invoke(result);
			m_localRequestSellItemCallback = null;
		}

		private bool HasAccess()
		{
			long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			GetPlayer(identityId);
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(identityId);
			bool flag = false;
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(base.OwnerId);
			if (myFaction != null)
			{
				flag = MySession.Static.Factions.IsNpcFaction(myFaction.Tag);
			}
			if (!AnyoneCanUse || (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies && flag))
			{
				return HasPlayerAccess(identityId);
			}
			return true;
		}

		private MyPlayer GetPlayer(long identityId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null || myIdentity.Character == null)
			{
				return null;
			}
			return MyPlayer.GetPlayerFromCharacter(myIdentity.Character);
		}

		public void CreateGetConnectedGridInventoriesRequest(Action<List<long>> resultCallback)
		{
			m_localRequestConnectedInventoriesItemCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.GetConnectedGridInventories);
		}

		[Event(null, 1524)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetConnectedGridInventories()
		{
			if (HasAccess())
			{
				long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				List<long> arg = new List<long>();
				foreach (MySlimBlock block in base.CubeGrid.GetBlocks())
				{
					MyShipConnector myShipConnector = block.FatBlock as MyShipConnector;
					if (myShipConnector != null && myShipConnector != null && myShipConnector.Connected && (bool)myShipConnector.TradingEnabled && ((Sandbox.ModAPI.Ingame.IMyShipConnector)myShipConnector).IsConnected)
					{
						arg = myShipConnector.GetInventoryEntities(identityId);
					}
				}
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnGetConnectedGridInventoriesResult, arg, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1552)]
		[Reliable]
		[Client]
		private void OnGetConnectedGridInventoriesResult(List<long> inventories)
		{
			m_localRequestConnectedInventoriesItemCallback?.Invoke(inventories);
			m_localRequestConnectedInventoriesItemCallback = null;
		}

		public void CreateGetGridInventoriesRequest(Action<List<long>> resultCallback)
		{
			m_localRequestInventoriesItemCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.GetGridInventories);
		}

		[Event(null, 1565)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetGridInventories()
		{
			if (HasAccess())
			{
				long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				List<long> list = new List<long>();
				if (base.CubeGrid.GridSystems.ConveyorSystem != null)
				{
					base.CubeGrid.GridSystems.ConveyorSystem.GetGridInventories(this, null, identityId, list);
				}
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnGetGridInventoriesResult, list, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1584)]
		[Reliable]
		[Client]
		private void OnGetGridInventoriesResult(List<long> inventories)
		{
			m_localRequestInventoriesItemCallback?.Invoke(inventories);
			m_localRequestInventoriesItemCallback = null;
		}

		internal void CreateNewOfferRequest(SerializableDefinitionId itemId, int offerAmount, int offerPricePerUnit, Action<MyStoreCreationResult> resultCallback)
		{
			m_localRequestCreateOfferCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.CreateNewOffer, itemId, offerAmount, offerPricePerUnit);
		}

		[Event(null, 1597)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CreateNewOffer(SerializableDefinitionId itemId, int amount, int pricePerUnit)
		{
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (HasPlayerAccess(num) && base.OwnerId == num)
			{
				MyStoreItem item;
				MyStoreCreationResult arg = CreateNewOffer_Internal(itemId, amount, pricePerUnit, chargeListingFee: true, out item);
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnCreateNewOfferResult, arg, MyEventContext.Current.Sender);
			}
		}

		private MyStoreCreationResult CreateNewOffer_Internal(SerializableDefinitionId itemId, int amount, int pricePerUnit, bool chargeListingFee, out MyStoreItem item)
		{
			item = null;
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				MyLog.Default.WriteToLogAndAssert("CreateNewOffer_Internal - Economy session component not found.");
				return MyStoreCreationResult.Error;
			}
			if (PlayerItems.Count >= component.GetStoreCreationLimitPerPlayer())
			{
				return MyStoreCreationResult.Fail_CreationLimitHard;
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(base.OwnerId, out MyAccountInfo account))
			{
				MyLog.Default.WriteToLogAndAssert("CreateNewOffer_Internal - Owner does not have account.");
				return MyStoreCreationResult.Error;
			}
			long num = (long)amount * (long)pricePerUnit;
			if (num <= 0)
			{
				MyLog.Default.WriteToLogAndAssert("CreateNewOffer_Internal - Wrong price.");
				return MyStoreCreationResult.Error;
			}
			if (component.GetMinimumItemPrice(itemId) > pricePerUnit)
			{
				return MyStoreCreationResult.Fail_PricePerUnitIsLowerThanMinimum;
			}
			if (chargeListingFee)
			{
				long num2 = (long)((float)num * component.EconomyDefinition.ListingFee);
				if (num2 > account.Balance)
				{
					return MyStoreCreationResult.Error;
				}
				component.AddCurrencyDestroyed(num2);
				MyBankingSystem.ChangeBalance(base.OwnerId, -num2);
			}
			MyStoreItem myStoreItem = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), itemId, amount, pricePerUnit, StoreItemTypes.Offer, 0);
			PlayerItems.Add(myStoreItem);
			item = myStoreItem;
			return MyStoreCreationResult.Success;
		}

		[Event(null, 1668)]
		[Reliable]
		[Client]
		private void OnCreateNewOfferResult(MyStoreCreationResult result)
		{
			m_localRequestCreateOfferCallback?.Invoke(result);
			m_localRequestCreateOfferCallback = null;
		}

		internal void CreateNewOrderRequest(SerializableDefinitionId itemId, int orderAmount, int orderPricePerUnit, Action<MyStoreCreationResult> resultCallback)
		{
			m_localRequestCreateOrderCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.CreateNewOrder, itemId, orderAmount, orderPricePerUnit);
		}

		[Event(null, 1681)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CreateNewOrder(SerializableDefinitionId itemId, int amount, int pricePerUnit)
		{
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (HasPlayerAccess(num) && base.OwnerId == num)
			{
				MyStoreItem item;
				MyStoreCreationResult arg = CreateNewOrder_Internal(itemId, amount, pricePerUnit, chargeListingFee: true, out item);
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnCreateNewOrderResult, arg, MyEventContext.Current.Sender);
			}
		}

		private MyStoreCreationResult CreateNewOrder_Internal(SerializableDefinitionId itemId, int amount, int pricePerUnit, bool chargeListingFee, out MyStoreItem item)
		{
			item = null;
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				MyLog.Default.WriteToLogAndAssert("CreateNewOrder_Internal - Economy session component not found.");
				return MyStoreCreationResult.Error;
			}
			if (PlayerItems.Count >= component.GetStoreCreationLimitPerPlayer())
			{
				return MyStoreCreationResult.Fail_CreationLimitHard;
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(base.OwnerId, out MyAccountInfo account))
			{
				MyLog.Default.WriteToLogAndAssert("CreateNewOrder_Internal - Owner does not have account.");
				return MyStoreCreationResult.Error;
			}
			long num = (long)amount * (long)pricePerUnit;
			if (num <= 0)
			{
				MyLog.Default.WriteToLogAndAssert("CreateNewOrder_Internal - Wrong price.");
				return MyStoreCreationResult.Error;
			}
			if (chargeListingFee)
			{
				long num2 = (long)((float)num * component.EconomyDefinition.ListingFee);
				if (num2 > account.Balance)
				{
					return MyStoreCreationResult.Error;
				}
				component.AddCurrencyDestroyed(num2);
				MyBankingSystem.ChangeBalance(base.OwnerId, -num2);
			}
			MyStoreItem myStoreItem = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), itemId, amount, pricePerUnit, StoreItemTypes.Order, 0);
			PlayerItems.Add(myStoreItem);
			item = myStoreItem;
			return MyStoreCreationResult.Success;
		}

		[Event(null, 1747)]
		[Reliable]
		[Client]
		private void OnCreateNewOrderResult(MyStoreCreationResult result)
		{
			m_localRequestCreateOrderCallback?.Invoke(result);
			m_localRequestCreateOrderCallback = null;
		}

		public void CreateCancelStoreItemRequest(long id, Action<bool> resultCallback)
		{
			m_localRequestCancelStoreItemCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.CancelStoreItemServer, id);
		}

		[Event(null, 1760)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CancelStoreItemServer(long id)
		{
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (HasPlayerAccess(num) && base.OwnerId == num)
			{
				bool arg = CancelStoreItem(id);
				MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnCancelStoreItemResult, arg, MyEventContext.Current.Sender);
			}
		}

		public bool CancelStoreItem(long id)
		{
			MyStoreItem myStoreItem = null;
			foreach (MyStoreItem playerItem in PlayerItems)
			{
				if (playerItem.Id == id)
				{
					myStoreItem = playerItem;
					break;
				}
			}
			if (myStoreItem == null)
			{
				return false;
			}
			if (myStoreItem.OnCancel != null)
			{
				myStoreItem.OnCancel();
			}
			PlayerItems.Remove(myStoreItem);
			return true;
		}

		[Event(null, 1803)]
		[Reliable]
		[Client]
		private void OnCancelStoreItemResult(bool result)
		{
			m_localRequestCancelStoreItemCallback?.Invoke(result);
			m_localRequestCancelStoreItemCallback = null;
		}

		internal void CreateChangeBalanceRequest(int amount, long targetEntityId, Action<MyStoreBuyItemResults> resultCallback)
		{
			m_localRequestChangeBalanceCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.ChangeBalance, amount, targetEntityId);
		}

		[Event(null, 1816)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void ChangeBalance(int amount, long targetEntityId)
		{
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MyPlayer player = GetPlayer(num);
			if (player == null || player.Character == null)
			{
				MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player not found.");
				return;
			}
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(targetEntityId, out entity))
			{
				MyLog.Default.WriteToLogAndAssert("ChangeBalance - Entity not found.");
				return;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = (entity as MyCubeBlock)) != null && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
			{
				MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player is not big owner of the grid.");
				return;
			}
			if (entity is MyCharacter && player.Character != entity)
			{
				MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player entity and inventory entity is different.");
				return;
			}
			if (!entity.TryGetInventory(out MyInventory inventory))
			{
				MyLog.Default.WriteToLogAndAssert("ChangeBalance - Inventory not found.");
				return;
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(num, out MyAccountInfo account))
			{
				string message = "ChangeBalance - Player does not have account.";
				MyLog.Default.WriteToLogAndAssert(message);
				return;
			}
			MyDefinitionId physicalItemId = MyBankingSystem.BankingSystemDefinition.PhysicalItemId;
			if (amount > 0)
			{
				MyFixedPoint itemAmount = inventory.GetItemAmount(physicalItemId);
				if (amount > itemAmount)
				{
					MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeBalanceResult, MyStoreBuyItemResults.WrongAmount, MyEventContext.Current.Sender);
					return;
				}
				if (MyBankingSystem.ChangeBalance(num, amount))
				{
					MyObjectBuilderSerializer.CreateNewObject(physicalItemId);
					inventory.RemoveItemsOfType(amount, physicalItemId);
				}
			}
			else
			{
				int num2 = -amount;
				if (num2 > account.Balance)
				{
					MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeBalanceResult, MyStoreBuyItemResults.NotEnoughMoney, MyEventContext.Current.Sender);
					return;
				}
				if (!inventory.CheckConstraint(physicalItemId))
				{
					MyLog.Default.WriteToLogAndAssert("ChangeBalance - Item can not be transfered to this inventory.");
					return;
				}
				if (!inventory.CanItemsBeAdded(num2, physicalItemId))
				{
					MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeBalanceResult, MyStoreBuyItemResults.NotEnoughInventorySpace, MyEventContext.Current.Sender);
					return;
				}
				if (MyBankingSystem.ChangeBalance(num, amount))
				{
					MyObjectBuilder_Base objectBuilder = MyObjectBuilderSerializer.CreateNewObject(physicalItemId);
					inventory.AddItems(num2, objectBuilder);
				}
			}
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.OnChangeBalanceResult, MyStoreBuyItemResults.Success, MyEventContext.Current.Sender);
		}

		[Event(null, 1908)]
		[Reliable]
		[Client]
		private void OnChangeBalanceResult(MyStoreBuyItemResults result)
		{
			m_localRequestChangeBalanceCallback?.Invoke(result);
			m_localRequestChangeBalanceCallback = null;
		}

		internal void ShowPreview(long storeItemId)
		{
			MyMultiplayer.RaiseEvent(this, (MyStoreBlock x) => x.ShowPreviewImplementation, storeItemId);
		}

		[Event(null, 1920)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void ShowPreviewImplementation(long storeItemId)
		{
			MyProjectorBase myProjectorBase = null;
			foreach (MySlimBlock block in base.CubeGrid.GetBlocks())
			{
				MyProjectorBase myProjectorBase2;
				if ((myProjectorBase2 = (block.FatBlock as MyProjectorBase)) != null && myProjectorBase2.DisplayNameText == "Store Preview")
				{
					myProjectorBase = myProjectorBase2;
					break;
				}
			}
			if (myProjectorBase == null)
			{
				return;
			}
			MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
			if (stationByGridId != null)
			{
				MyStoreItem storeItemById = stationByGridId.GetStoreItemById(storeItemId);
				if (storeItemById != null)
				{
					myProjectorBase.SelectPrefab(storeItemById.PrefabName);
				}
			}
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		public PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty Constraint")
			};
		}

		public PullInformation GetPushInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty constraint")
			};
		}

		public MyStoreInsertResults InsertOffer(MyStoreItemData item, out long id)
		{
			id = 0L;
			MyStoreItem item2;
			MyStoreCreationResult num = CreateNewOffer_Internal(item.ItemId, item.Amount, item.PricePerUnit, chargeListingFee: false, out item2);
			if (num == MyStoreCreationResult.Success)
			{
				id = item2.Id;
				item2.SetActions(item.OnTransaction, item.OnCancel);
			}
			return Convert(num);
		}

		public MyStoreInsertResults InsertOffer(MyStoreItemDataSimple item, out long id)
		{
			MyDefinitionId v = item.ItemId;
			MyStoreItemData item2 = new MyStoreItemData(v, item.Amount, item.PricePerUnit, null, null);
			return InsertOffer(item2, out id);
		}

		public MyStoreInsertResults InsertOrder(MyStoreItemData item, out long id)
		{
			id = 0L;
			MyStoreItem item2;
			MyStoreCreationResult num = CreateNewOrder_Internal(item.ItemId, item.Amount, item.PricePerUnit, chargeListingFee: false, out item2);
			if (num == MyStoreCreationResult.Success)
			{
				id = item2.Id;
				item2.SetActions(item.OnTransaction, item.OnCancel);
			}
			return Convert(num);
		}

		public MyStoreInsertResults InsertOrder(MyStoreItemDataSimple item, out long id)
		{
			MyDefinitionId v = item.ItemId;
			MyStoreItemData item2 = new MyStoreItemData(v, item.Amount, item.PricePerUnit, null, null);
			return InsertOrder(item2, out id);
		}

		private static MyStoreInsertResults Convert(MyStoreCreationResult input)
		{
			switch (input)
			{
			case MyStoreCreationResult.Success:
				return MyStoreInsertResults.Success;
			case MyStoreCreationResult.Fail_CreationLimitHard:
				return MyStoreInsertResults.Fail_StoreLimitReached;
			case MyStoreCreationResult.Fail_PricePerUnitIsLowerThanMinimum:
				return MyStoreInsertResults.Fail_PricePerUnitIsLessThanMinimum;
			default:
				return MyStoreInsertResults.Error;
			}
		}

		public void GetPlayerStoreItems(List<MyStoreQueryItem> storeItems)
		{
			if (storeItems != null)
			{
				foreach (MyStoreItem playerItem in PlayerItems)
				{
					MyStoreQueryItem myStoreQueryItem = default(MyStoreQueryItem);
					myStoreQueryItem.Id = playerItem.Id;
					myStoreQueryItem.ItemId = playerItem.Item.Value;
					myStoreQueryItem.Amount = playerItem.Amount;
					myStoreQueryItem.PricePerUnit = playerItem.PricePerUnit;
					MyStoreQueryItem item = myStoreQueryItem;
					storeItems.Add(item);
				}
			}
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}
	}
}
