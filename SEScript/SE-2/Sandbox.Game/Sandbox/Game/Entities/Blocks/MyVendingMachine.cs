using Sandbox.Common.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_VendingMachine))]
	public class MyVendingMachine : MyStoreBlock, IMyVendingMachine, Sandbox.ModAPI.IMyStoreBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
	{
		protected sealed class PlayActionSound_003C_003E : ICallSite<MyVendingMachine, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyVendingMachine @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PlayActionSound();
			}
		}

		protected class m_selectedItemIdx_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType selectedItemIdx;
				ISyncType result = selectedItemIdx = new Sync<int, SyncDirection.BothWays>(P_1, P_2);
				((MyVendingMachine)P_0).m_selectedItemIdx = (Sync<int, SyncDirection.BothWays>)selectedItemIdx;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyVendingMachine_003C_003EActor : IActivator, IActivator<MyVendingMachine>
		{
			private sealed override object CreateInstance()
			{
				return new MyVendingMachine();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVendingMachine CreateInstance()
			{
				return new MyVendingMachine();
			}

			MyVendingMachine IActivator<MyVendingMachine>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Sync<int, SyncDirection.BothWays> m_selectedItemIdx;

		private List<MyStoreItem> m_shopItems = new List<MyStoreItem>();

		private long m_lastEcoTick;

		private bool m_firstItemsSync = true;

		private Matrix m_throwOutDummy = Matrix.Identity;

		public int SelectedItemIdx => m_selectedItemIdx;

		public MyStoreItem SelectedItem
		{
			get
			{
				if (m_shopItems.Count <= 0)
				{
					return null;
				}
				return m_shopItems[m_selectedItemIdx];
			}
		}

		protected override bool UseConveyorSystem
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public new MyVendingMachineDefinition BlockDefinition => (MyVendingMachineDefinition)base.BlockDefinition;

		public event Action OnSelectedItemChanged;

		public event Action<MyStoreBuyItemResult> OnBuyItemResult;

		public MyVendingMachine()
		{
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			m_selectedItemIdx.SetLocalValue(-1);
			if (Sync.IsServer)
			{
				m_selectedItemIdx.ShouldValidate = false;
			}
			base.Init(objectBuilder, cubeGrid);
			if (BlockDefinition.DefaultItems != null && BlockDefinition.DefaultItems.Count > 0)
			{
				base.PlayerItems.Clear();
				foreach (MyObjectBuilder_StoreItem defaultItem in BlockDefinition.DefaultItems)
				{
					base.PlayerItems.Add(new MyStoreItem(defaultItem));
				}
			}
			if (!string.IsNullOrEmpty(BlockDefinition.ThrowOutDummy) && base.Model.Dummies.TryGetValue(BlockDefinition.ThrowOutDummy, out MyModelDummy value))
			{
				m_throwOutDummy = value.Matrix;
			}
			m_selectedItemIdx.ValueChanged += OnSelectedItemChangedInternal;
		}

		private void OnShopItemsRecieved(List<MyStoreItem> storeItems, long lastEconomyTick, float offersBonus, float ordersBonus)
		{
			GenerateShopItems(storeItems);
			m_lastEcoTick = lastEconomyTick;
		}

		private void GenerateShopItems(List<MyStoreItem> storeItems)
		{
			if (storeItems.Count == 0)
			{
				m_selectedItemIdx.Value = -1;
				return;
			}
			m_shopItems.Clear();
			foreach (MyStoreItem storeItem in storeItems)
			{
				if (storeItem.Item.HasValue && storeItem.StoreItemType != StoreItemTypes.Order)
				{
					MyPhysicalItemDefinition definition = null;
					if (MyDefinitionManager.Static.TryGetDefinition(storeItem.Item.Value, out definition))
					{
						m_shopItems.Add(storeItem);
					}
				}
			}
			m_selectedItemIdx.Value = MathHelper.Clamp(m_selectedItemIdx, 0, m_shopItems.Count - 1);
		}

		private void OnSelectedItemChangedInternal(SyncBase obj)
		{
			this.OnSelectedItemChanged?.Invoke();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyVendingMachine>())
			{
				base.CreateTerminalControls();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			return base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_VendingMachine;
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (m_firstItemsSync)
			{
				CreateGetStoreItemsRequest(0L, OnShopItemsRecieved);
				m_firstItemsSync = false;
			}
		}

		public void SelectNextItem()
		{
			if (m_shopItems.Count != 0)
			{
				if ((int)m_selectedItemIdx + 1 >= m_shopItems.Count)
				{
					m_selectedItemIdx.Value = 0;
				}
				else
				{
					m_selectedItemIdx.Value++;
				}
				UpdateStoreContent();
			}
		}

		public void SelectPreviewsItem()
		{
			if (m_shopItems.Count != 0)
			{
				if ((int)m_selectedItemIdx - 1 < 0)
				{
					m_selectedItemIdx.Value = m_shopItems.Count - 1;
				}
				else
				{
					m_selectedItemIdx.Value--;
				}
				UpdateStoreContent();
			}
		}

		public void Buy()
		{
			if (MySession.Static != null && MySession.Static.LocalCharacter != null && (int)m_selectedItemIdx >= 0 && SelectedItem != null && SelectedItem.Amount > 0)
			{
				CreateBuyRequest(SelectedItem.Id, 1, MySession.Static.LocalCharacterEntityId, m_lastEcoTick, OnBuyCallback);
			}
		}

		private void OnBuyCallback(MyStoreBuyItemResult buyItemResult)
		{
			this.OnBuyItemResult?.Invoke(buyItemResult);
			UpdateStoreContent();
		}

		public void UpdateStoreContent()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				CreateGetStoreItemsRequest(0L, OnShopItemsRecieved);
			}
		}

		protected override void OnPlayerStoreItemsChanged(List<MyStoreItem> storeItems, long lastEconomyTick)
		{
			base.OnPlayerStoreItemsChanged(storeItems, lastEconomyTick);
			OnShopItemsRecieved(storeItems, lastEconomyTick, 1f, 1f);
		}

		private void OnInventoryContentsChanged(MyInventoryBase obj)
		{
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			if (BlockDefinition.AdditionalEmissiveMaterials != null)
			{
				foreach (string additionalEmissiveMaterial in BlockDefinition.AdditionalEmissiveMaterials)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], additionalEmissiveMaterial, Color.White, 1f);
				}
			}
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			if (BlockDefinition.AdditionalEmissiveMaterials != null)
			{
				foreach (string additionalEmissiveMaterial in BlockDefinition.AdditionalEmissiveMaterials)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], additionalEmissiveMaterial, Color.White, 0f);
				}
			}
		}

		protected override void OnItemBought(MyInventory inventory, MyDefinitionId definitionId, long totalPrice, int amount)
		{
			if (!string.IsNullOrEmpty(BlockDefinition.ThrowOutDummy))
			{
				MatrixD value = (MatrixD)m_throwOutDummy * base.PositionComp.WorldMatrix;
				foreach (MyPhysicalInventoryItem item in inventory.GetItems())
				{
					if (item.Content.GetId() == definitionId && item.Amount >= 1)
					{
						inventory.RemoveItems(item.ItemId, 1, sendEvent: false, spawn: true, value, OnItemSpawned);
						break;
					}
				}
			}
		}

		private void OnItemSpawned(MyDefinitionId definitionId, MyEntity item)
		{
			MatrixD worldMatrix = item.PositionComp.WorldMatrix;
			worldMatrix = MatrixD.CreateRotationX(1.570796012878418) * worldMatrix;
			worldMatrix.Translation = item.PositionComp.WorldMatrix.Translation;
			item.PositionComp.WorldMatrix = worldMatrix;
			if (BlockDefinition.ThrowOutItems != null && BlockDefinition.ThrowOutItems.TryGetValue(definitionId.SubtypeName, out float value) && item.Physics != null)
			{
				MatrixD matrixD = (MatrixD)m_throwOutDummy * base.PositionComp.WorldMatrix;
				item.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, matrixD.Right * value, null, null);
			}
		}

		[Event(null, 300)]
		[Reliable]
		[Broadcast]
		private void PlayActionSound()
		{
			m_soundEmitter.PlaySound(m_actionSound);
		}
	}
}
