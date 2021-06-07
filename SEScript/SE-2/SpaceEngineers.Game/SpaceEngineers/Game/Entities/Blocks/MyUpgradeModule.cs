using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_UpgradeModule))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyUpgradeModule),
		typeof(Sandbox.ModAPI.Ingame.IMyUpgradeModule)
	})]
	public class MyUpgradeModule : MyFunctionalBlock, Sandbox.ModAPI.IMyUpgradeModule, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUpgradeModule
	{
		private ConveyorLinePosition[] m_connectionPositions;

		private Dictionary<ConveyorLinePosition, MyCubeBlock> m_connectedBlocks;

		private MyUpgradeModuleInfo[] m_upgrades;

		private int m_connectedBlockCount;

		/// <summary>
		/// These are sorted so that dummy index and emissivity index match
		/// </summary>
		private SortedDictionary<string, MyModelDummy> m_dummies;

		private bool m_needsRefresh;

		private MyResourceStateEnum m_oldResourceState = MyResourceStateEnum.NoPower;

		private new MyUpgradeModuleDefinition BlockDefinition => (MyUpgradeModuleDefinition)base.BlockDefinition;

		uint Sandbox.ModAPI.Ingame.IMyUpgradeModule.UpgradeCount => (uint)m_upgrades.Length;

		uint Sandbox.ModAPI.Ingame.IMyUpgradeModule.Connections
		{
			get
			{
				uint num = 0u;
				MyCubeBlock myCubeBlock = null;
				foreach (MyCubeBlock value in m_connectedBlocks.Values)
				{
					if (myCubeBlock != value && value != null)
					{
						num++;
						myCubeBlock = value;
					}
				}
				return num;
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_connectedBlocks = new Dictionary<ConveyorLinePosition, MyCubeBlock>();
			m_dummies = new SortedDictionary<string, MyModelDummy>(MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies);
			InitDummies();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyUpgradeModule_IsWorkingChanged;
			m_upgrades = BlockDefinition.Upgrades;
			UpdateIsWorking();
		}

		private void MyUpgradeModule_IsWorkingChanged(MyCubeBlock obj)
		{
			RefreshEffects();
			UpdateEmissivity();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			UpdateIsWorking();
			UpdateEmissivity();
		}

		private void CubeGrid_OnBlockRemoved(MySlimBlock obj)
		{
			if (obj != SlimBlock)
			{
				m_needsRefresh = true;
			}
		}

		private void CubeGrid_OnBlockAdded(MySlimBlock obj)
		{
			if (obj != SlimBlock)
			{
				m_needsRefresh = true;
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			InitDummies();
			m_needsRefresh = true;
			UpdateEmissivity();
			base.CubeGrid.OnBlockAdded += CubeGrid_OnBlockAdded;
			base.CubeGrid.OnBlockRemoved += CubeGrid_OnBlockRemoved;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (m_needsRefresh)
			{
				RefreshConnections();
				m_needsRefresh = false;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			if (base.CubeGrid.GridSystems.ResourceDistributor.ResourceState != m_oldResourceState)
			{
				m_oldResourceState = base.CubeGrid.GridSystems.ResourceDistributor.ResourceState;
				UpdateEmissivity();
			}
			m_oldResourceState = base.CubeGrid.GridSystems.ResourceDistributor.ResourceState;
			if (m_soundEmitter != null)
			{
				bool flag = false;
				foreach (MyCubeBlock value in m_connectedBlocks.Values)
				{
					flag |= (value != null && value.ResourceSink != null && value.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && value.IsWorking);
					if (flag)
					{
						break;
					}
				}
				flag &= base.IsWorking;
				if (flag && m_connectedBlockCount > 0 && (!m_soundEmitter.IsPlaying || m_soundEmitter.SoundPair != m_baseIdleSound))
				{
					m_soundEmitter.PlaySound(m_baseIdleSound, stopPrevious: true);
				}
				else if ((!flag || m_connectedBlockCount == 0) && m_soundEmitter.IsPlaying && m_soundEmitter.SoundPair == m_baseIdleSound)
				{
					m_soundEmitter.StopSound(forced: false);
				}
			}
		}

		private void InitDummies()
		{
			m_connectedBlocks.Clear();
			m_connectionPositions = MyMultilineConveyorEndpoint.GetLinePositions(this, m_dummies, "detector_upgrade");
			for (int i = 0; i < m_connectionPositions.Length; i++)
			{
				m_connectionPositions[i] = MyMultilineConveyorEndpoint.PositionToGridCoords(m_connectionPositions[i], this);
				m_connectedBlocks.Add(m_connectionPositions[i], null);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			UpdateEmissivity();
		}

		private void RefreshConnections()
		{
			ConveyorLinePosition[] connectionPositions = m_connectionPositions;
			for (int i = 0; i < connectionPositions.Length; i++)
			{
				ConveyorLinePosition key = connectionPositions[i];
				ConveyorLinePosition connectingPosition = key.GetConnectingPosition();
				MySlimBlock cubeBlock = base.CubeGrid.GetCubeBlock(connectingPosition.LocalGridPosition);
				if (cubeBlock != null && cubeBlock.FatBlock != null)
				{
					MyCubeBlock myCubeBlock = cubeBlock.FatBlock;
					MyCubeBlock value = null;
					m_connectedBlocks.TryGetValue(key, out value);
					if (myCubeBlock != null && !myCubeBlock.GetComponent().ConnectionPositions.Contains(connectingPosition))
					{
						myCubeBlock = null;
					}
					if (myCubeBlock == value)
					{
						continue;
					}
					if (value != null && value.CurrentAttachedUpgradeModules != null)
					{
						value.CurrentAttachedUpgradeModules.Remove(base.EntityId);
					}
					if (myCubeBlock != null)
					{
						if (myCubeBlock.CurrentAttachedUpgradeModules == null)
						{
							myCubeBlock.CurrentAttachedUpgradeModules = new Dictionary<long, AttachedUpgradeModule>();
						}
						if (myCubeBlock.CurrentAttachedUpgradeModules.ContainsKey(base.EntityId))
						{
							myCubeBlock.CurrentAttachedUpgradeModules[base.EntityId].SlotCount++;
						}
						else
						{
							myCubeBlock.CurrentAttachedUpgradeModules.Add(base.EntityId, new AttachedUpgradeModule(this, 1, CanAffectBlock(myCubeBlock)));
						}
					}
					if (base.IsWorking)
					{
						if (value != null)
						{
							RemoveEffectFromBlock(value);
						}
						if (myCubeBlock != null)
						{
							AddEffectToBlock(myCubeBlock);
						}
					}
					m_connectedBlocks[key] = myCubeBlock;
					continue;
				}
				MyCubeBlock value2 = null;
				m_connectedBlocks.TryGetValue(key, out value2);
				if (value2 != null)
				{
					if (value2 != null && value2.CurrentAttachedUpgradeModules != null)
					{
						value2.CurrentAttachedUpgradeModules.Remove(base.EntityId);
					}
					if (base.IsWorking)
					{
						RemoveEffectFromBlock(value2);
					}
					m_connectedBlocks[key] = null;
				}
			}
			UpdateEmissivity();
		}

		private void RefreshEffects()
		{
			foreach (MyCubeBlock value in m_connectedBlocks.Values)
			{
				if (value != null)
				{
					if (base.IsWorking)
					{
						AddEffectToBlock(value);
					}
					else
					{
						RemoveEffectFromBlock(value);
					}
				}
			}
		}

		private bool CanAffectBlock(MyCubeBlock block)
		{
			MyUpgradeModuleInfo[] upgrades = m_upgrades;
			foreach (MyUpgradeModuleInfo myUpgradeModuleInfo in upgrades)
			{
				if (block.UpgradeValues.ContainsKey(myUpgradeModuleInfo.UpgradeType))
				{
					return true;
				}
			}
			return false;
		}

		private void RemoveEffectFromBlock(MyCubeBlock block)
		{
			MyUpgradeModuleInfo[] upgrades = m_upgrades;
			for (int i = 0; i < upgrades.Length; i++)
			{
				MyUpgradeModuleInfo myUpgradeModuleInfo = upgrades[i];
				if (!block.UpgradeValues.TryGetValue(myUpgradeModuleInfo.UpgradeType, out float value))
				{
					continue;
				}
				double num = value;
				if (myUpgradeModuleInfo.ModifierType == MyUpgradeModifierType.Additive)
				{
					num -= (double)myUpgradeModuleInfo.Modifier;
					if (num < 0.0)
					{
						num = 0.0;
					}
				}
				else
				{
					num /= (double)myUpgradeModuleInfo.Modifier;
					if (num < 1.0)
					{
						_ = num + 1E-07;
						_ = 1.0;
						num = 1.0;
					}
				}
				block.UpgradeValues[myUpgradeModuleInfo.UpgradeType] = (float)num;
			}
			block.CommitUpgradeValues();
		}

		private void AddEffectToBlock(MyCubeBlock block)
		{
			MyUpgradeModuleInfo[] upgrades = m_upgrades;
			for (int i = 0; i < upgrades.Length; i++)
			{
				MyUpgradeModuleInfo myUpgradeModuleInfo = upgrades[i];
				if (block.UpgradeValues.TryGetValue(myUpgradeModuleInfo.UpgradeType, out float value))
				{
					double num = value;
					num = ((myUpgradeModuleInfo.ModifierType != MyUpgradeModifierType.Additive) ? (num * (double)myUpgradeModuleInfo.Modifier) : (num + (double)myUpgradeModuleInfo.Modifier));
					block.UpgradeValues[myUpgradeModuleInfo.UpgradeType] = (float)num;
				}
			}
			block.CommitUpgradeValues();
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		private void UpdateEmissivity()
		{
			m_connectedBlockCount = 0;
			if (m_connectedBlocks == null)
			{
				return;
			}
			for (int i = 0; i < m_connectionPositions.Length; i++)
			{
				string emissiveName = "Emissive" + i;
				Color emissivePartColor = Color.Green;
				float emissivity = 1f;
				MyCubeBlock value = null;
				m_connectedBlocks.TryGetValue(m_connectionPositions[i], out value);
				if (value != null)
				{
					m_connectedBlockCount++;
				}
				MyEmissiveColorStateResult result;
				if (base.IsWorking && m_oldResourceState != MyResourceStateEnum.NoPower)
				{
					if (value != null)
					{
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
						{
							emissivePartColor = result.EmissiveColor;
						}
					}
					else
					{
						emissivePartColor = Color.Yellow;
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
						{
							emissivePartColor = result.EmissiveColor;
						}
					}
				}
				else if (base.IsFunctional)
				{
					emissivePartColor = Color.Red;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						emissivePartColor = result.EmissiveColor;
					}
				}
				else
				{
					emissivePartColor = Color.Black;
					emissivity = 0f;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
					{
						emissivePartColor = result.EmissiveColor;
					}
				}
				if (base.Render.RenderObjectIDs[0] != uint.MaxValue)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], emissiveName, emissivePartColor, emissivity);
				}
			}
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			base.CubeGrid.OnBlockAdded -= CubeGrid_OnBlockAdded;
			base.CubeGrid.OnBlockRemoved -= CubeGrid_OnBlockRemoved;
			SlimBlock.ComponentStack.IsFunctionalChanged -= ComponentStack_IsFunctionalChanged;
			ClearConnectedBlocks();
		}

		private void ClearConnectedBlocks()
		{
			foreach (MyCubeBlock value in m_connectedBlocks.Values)
			{
				if (value != null && base.IsWorking)
				{
					RemoveEffectFromBlock(value);
				}
				if (value != null && value.CurrentAttachedUpgradeModules != null)
				{
					value.CurrentAttachedUpgradeModules.Remove(base.EntityId);
				}
			}
			m_connectedBlocks.Clear();
		}

		protected int GetBlockConnectionCount(MyCubeBlock cubeBlock)
		{
			int num = 0;
			foreach (MyCubeBlock value in m_connectedBlocks.Values)
			{
				if (value == cubeBlock)
				{
					num++;
				}
			}
			return num;
		}

		void Sandbox.ModAPI.Ingame.IMyUpgradeModule.GetUpgradeList(out List<MyUpgradeModuleInfo> upgradelist)
		{
			upgradelist = new List<MyUpgradeModuleInfo>();
			MyUpgradeModuleInfo[] upgrades = m_upgrades;
			foreach (MyUpgradeModuleInfo item in upgrades)
			{
				upgradelist.Add(item);
			}
		}
	}
}
