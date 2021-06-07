using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
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
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Assembler))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyAssembler),
		typeof(Sandbox.ModAPI.Ingame.IMyAssembler)
	})]
	public class MyAssembler : MyProductionBlock, Sandbox.ModAPI.IMyAssembler, Sandbox.ModAPI.IMyProductionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, Sandbox.ModAPI.Ingame.IMyAssembler, IMyEventProxy, IMyEventOwner
	{
		public enum StateEnum
		{
			Ok,
			Disabled,
			NotWorking,
			NotEnoughPower,
			MissingItems,
			InventoryFull
		}

		protected sealed class ModeSwitchCallback_003C_003ESystem_Boolean : ICallSite<MyAssembler, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAssembler @this, in bool disassembleEnabled, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ModeSwitchCallback(disassembleEnabled);
			}
		}

		protected sealed class ModeSwitchClient_003C_003ESystem_Boolean : ICallSite<MyAssembler, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAssembler @this, in bool disassembleEnabled, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ModeSwitchClient(disassembleEnabled);
			}
		}

		protected sealed class RepeatEnabledCallback_003C_003ESystem_Boolean_0023System_Boolean : ICallSite<MyAssembler, bool, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAssembler @this, in bool disassembleEnabled, in bool repeatEnable, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RepeatEnabledCallback(disassembleEnabled, repeatEnable);
			}
		}

		protected sealed class DisassembleAllCallback_003C_003E : ICallSite<MyAssembler, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAssembler @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DisassembleAllCallback();
			}
		}

		protected class m_slave_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType slave;
				ISyncType result = slave = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyAssembler)P_0).m_slave = (Sync<bool, SyncDirection.BothWays>)slave;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyAssembler_003C_003EActor : IActivator, IActivator<MyAssembler>
		{
			private sealed override object CreateInstance()
			{
				return new MyAssembler();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAssembler CreateInstance()
			{
				return new MyAssembler();
			}

			MyAssembler IActivator<MyAssembler>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyEntity m_currentUser;

		private MyAssemblerDefinition m_assemblerDef;

		private float m_currentProgress;

		private StateEnum m_currentState;

		private readonly Sync<bool, SyncDirection.BothWays> m_slave;

		private bool m_repeatDisassembleEnabled;

		private bool m_repeatAssembleEnabled;

		private bool m_disassembleEnabled;

		private readonly List<MyEntity> m_inventoryOwners = new List<MyEntity>();

		private List<MyTuple<MyFixedPoint, MyBlueprintDefinitionBase.Item>> m_requiredComponents;

		private const float TIME_IN_ADVANCE = 5f;

		private bool m_isProcessing;

		private bool m_soundStartedFromInventory;

		private List<QueueItem> m_otherQueue;

		private List<MyAssembler> m_assemblers = new List<MyAssembler>();

		private int m_assemblerKeyCounter;

		private MyCubeGrid m_cubeGrid;

		private bool m_inventoryOwnersDirty = true;

		private static readonly List<IMyConveyorEndpoint> m_conveyorEndpoints = new List<IMyConveyorEndpoint>();

		private static MyAssembler m_assemblerForPathfinding;

		private static readonly Predicate<IMyConveyorEndpoint> m_vertexPredicate = VertexRules;

		private static readonly Predicate<IMyConveyorEndpoint> m_edgePredicate = EdgeRules;

		public bool InventoryOwnersDirty
		{
			get
			{
				return m_inventoryOwnersDirty;
			}
			set
			{
				m_inventoryOwnersDirty = value;
			}
		}

		public bool IsSlave
		{
			get
			{
				return m_slave;
			}
			set
			{
				if (!(!SupportsAdvancedFunctions && value))
				{
					m_slave.Value = value;
				}
			}
		}

		public float CurrentProgress
		{
			get
			{
				return m_currentProgress;
			}
			set
			{
				if (m_currentProgress != value)
				{
					m_currentProgress = value;
					if (this.CurrentProgressChanged != null)
					{
						this.CurrentProgressChanged(this);
					}
				}
			}
		}

		public StateEnum CurrentState
		{
			get
			{
				return m_currentState;
			}
			private set
			{
				if (m_currentState != value)
				{
					m_currentState = value;
					if (this.CurrentStateChanged != null)
					{
						this.CurrentStateChanged(this);
					}
				}
			}
		}

		public int CurrentItemIndex
		{
			get
			{
				if (!m_currentQueueItem.HasValue)
				{
					return -1;
				}
				return m_queue.FindIndex((QueueItem x) => x.ItemId == m_currentQueueItem.Value.ItemId);
			}
		}

		public bool RepeatEnabled
		{
			get
			{
				if (!m_disassembleEnabled)
				{
					return m_repeatAssembleEnabled;
				}
				return m_repeatDisassembleEnabled;
			}
			private set
			{
				if (!(!SupportsAdvancedFunctions && value))
				{
					if (m_disassembleEnabled)
					{
						SetRepeat(ref m_repeatDisassembleEnabled, value);
					}
					else
					{
						SetRepeat(ref m_repeatAssembleEnabled, value);
					}
				}
			}
		}

		public virtual bool SupportsAdvancedFunctions => true;

		public bool DisassembleEnabled
		{
			get
			{
				return m_disassembleEnabled;
			}
			private set
			{
				if (m_disassembleEnabled != value && !(!SupportsAdvancedFunctions && value))
				{
					CurrentProgress = 0f;
					m_disassembleEnabled = value;
					SwapQueue(ref m_otherQueue);
					RebuildQueueInRepeatDisassembling();
					UpdateInventoryFlags();
					m_currentState = StateEnum.Ok;
					if (this.CurrentModeChanged != null)
					{
						this.CurrentModeChanged(this);
					}
					if (this.CurrentStateChanged != null)
					{
						this.CurrentStateChanged(this);
					}
				}
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyAssembler.DisassembleEnabled => DisassembleEnabled;

		MyAssemblerMode Sandbox.ModAPI.Ingame.IMyAssembler.Mode
		{
			get
			{
				if (!DisassembleEnabled)
				{
					return MyAssemblerMode.Assembly;
				}
				return MyAssemblerMode.Disassembly;
			}
			set
			{
				RequestDisassembleEnabled(value == MyAssemblerMode.Disassembly);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyAssembler.CooperativeMode
		{
			get
			{
				return IsSlave;
			}
			set
			{
				IsSlave = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyAssembler.Repeating
		{
			get
			{
				return RepeatEnabled;
			}
			set
			{
				RequestRepeatEnabled(value);
			}
		}

		public virtual int GUIPriority => (int)MathHelper.Lerp(200f, 500f, 1f - m_assemblerDef.AssemblySpeed);

		public event Action<MyAssembler> CurrentProgressChanged;

		public event Action<MyAssembler> CurrentStateChanged;

		public event Action<MyAssembler> CurrentModeChanged;

		event Action<Sandbox.ModAPI.IMyAssembler> Sandbox.ModAPI.IMyAssembler.CurrentProgressChanged
		{
			add
			{
				CurrentProgressChanged += GetDelegate(value);
			}
			remove
			{
				CurrentProgressChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyAssembler> Sandbox.ModAPI.IMyAssembler.CurrentStateChanged
		{
			add
			{
				CurrentStateChanged += GetDelegate(value);
			}
			remove
			{
				CurrentStateChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyAssembler> Sandbox.ModAPI.IMyAssembler.CurrentModeChanged
		{
			add
			{
				CurrentModeChanged += GetDelegate(value);
			}
			remove
			{
				CurrentModeChanged -= GetDelegate(value);
			}
		}

		public MyAssembler()
		{
			CreateTerminalControls();
			m_otherQueue = new List<QueueItem>();
			m_slave.ValueChanged += delegate
			{
				OnSlaveChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyAssembler>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyAssembler> obj = new MyTerminalControlCheckbox<MyAssembler>("slaveMode", MySpaceTexts.Assembler_SlaveMode, MySpaceTexts.Assembler_SlaveMode)
				{
					Enabled = ((MyAssembler x) => x.SupportsAdvancedFunctions)
				};
				obj.Visible = obj.Enabled;
				obj.Getter = ((MyAssembler x) => x.IsSlave);
				obj.Setter = delegate(MyAssembler x, bool v)
				{
					if (x.RepeatEnabled)
					{
						x.RequestRepeatEnabled(newRepeatEnable: false);
					}
					x.IsSlave = v;
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.UpgradeValues.Add("Productivity", 0f);
			base.UpgradeValues.Add("PowerEfficiency", 1f);
			base.Init(objectBuilder, cubeGrid);
			m_cubeGrid = cubeGrid;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_assemblerDef = (base.BlockDefinition as MyAssemblerDefinition);
			if (base.InventoryAggregate.InventoryCount > 2)
			{
				FixInputOutputInventories(m_assemblerDef.InputInventoryConstraint, m_assemblerDef.OutputInventoryConstraint);
			}
			base.InputInventory.Constraint = m_assemblerDef.InputInventoryConstraint;
			base.OutputInventory.Constraint = m_assemblerDef.OutputInventoryConstraint;
			base.InputInventory.FilterItemsUsingConstraint();
			MyObjectBuilder_Assembler myObjectBuilder_Assembler = (MyObjectBuilder_Assembler)objectBuilder;
			if (myObjectBuilder_Assembler.OtherQueue != null)
			{
				m_otherQueue.Clear();
				if (m_otherQueue.Capacity < myObjectBuilder_Assembler.OtherQueue.Length)
				{
					m_otherQueue.Capacity = myObjectBuilder_Assembler.OtherQueue.Length;
				}
				for (int i = 0; i < myObjectBuilder_Assembler.OtherQueue.Length; i++)
				{
					MyObjectBuilder_ProductionBlock.QueueItem queueItem = myObjectBuilder_Assembler.OtherQueue[i];
					MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(queueItem.Id);
					if (myBlueprintDefinitionBase != null)
					{
						m_otherQueue.Add(new QueueItem
						{
							Blueprint = myBlueprintDefinitionBase,
							Amount = queueItem.Amount
						});
					}
					else
					{
						MySandboxGame.Log.WriteLine($"No blueprint that produces a single result with Id '{queueItem.Id}'");
					}
				}
			}
			CurrentProgress = myObjectBuilder_Assembler.CurrentProgress;
			if (DisassembleEnabled)
			{
				m_disassembleEnabled = myObjectBuilder_Assembler.DisassembleEnabled;
			}
			else
			{
				m_disassembleEnabled = false;
			}
			m_repeatAssembleEnabled = myObjectBuilder_Assembler.RepeatAssembleEnabled;
			m_repeatDisassembleEnabled = myObjectBuilder_Assembler.RepeatDisassembleEnabled;
			m_slave.SetLocalValue(myObjectBuilder_Assembler.SlaveEnabled);
			UpdateInventoryFlags();
			m_baseIdleSound = base.BlockDefinition.PrimarySound;
			m_processSound = base.BlockDefinition.ActionSound;
			base.OnUpgradeValuesChanged += delegate
			{
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			};
			base.ResourceSink.RequiredInputChanged += PowerReceiver_RequiredInputChanged;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Assembler myObjectBuilder_Assembler = (MyObjectBuilder_Assembler)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_Assembler.CurrentProgress = CurrentProgress;
			myObjectBuilder_Assembler.DisassembleEnabled = m_disassembleEnabled;
			myObjectBuilder_Assembler.RepeatAssembleEnabled = m_repeatAssembleEnabled;
			myObjectBuilder_Assembler.RepeatDisassembleEnabled = m_repeatDisassembleEnabled;
			myObjectBuilder_Assembler.SlaveEnabled = m_slave;
			if (m_otherQueue.Count > 0)
			{
				myObjectBuilder_Assembler.OtherQueue = new MyObjectBuilder_ProductionBlock.QueueItem[m_otherQueue.Count];
				for (int i = 0; i < m_otherQueue.Count; i++)
				{
					myObjectBuilder_Assembler.OtherQueue[i] = new MyObjectBuilder_ProductionBlock.QueueItem
					{
						Amount = m_otherQueue[i].Amount,
						Id = m_otherQueue[i].Blueprint.Id
					};
				}
			}
			else
			{
				myObjectBuilder_Assembler.OtherQueue = null;
			}
			return myObjectBuilder_Assembler;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.AppendFormat("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(GetOperationalPowerConsumption(), detailedInfo);
			detailedInfo.AppendFormat("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.AppendFormat("\n\n");
			detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.BlockPropertiesText_Productivity));
			detailedInfo.Append(((base.UpgradeValues["Productivity"] + 1f) * 100f).ToString("F0"));
			detailedInfo.Append("%\n");
			detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.BlockPropertiesText_Efficiency));
			detailedInfo.Append((base.UpgradeValues["PowerEfficiency"] * 100f).ToString("F0"));
			detailedInfo.Append("%\n\n");
			PrintUpgradeModuleInfo(detailedInfo);
		}

		private void PowerReceiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private static bool VertexRules(IMyConveyorEndpoint vertex)
		{
			if (vertex.CubeBlock is MyAssembler)
			{
				return vertex.CubeBlock != m_assemblerForPathfinding;
			}
			return false;
		}

		private static bool EdgeRules(IMyConveyorEndpoint edge)
		{
			if (edge.CubeBlock.OwnerId == 0L)
			{
				return true;
			}
			return m_assemblerForPathfinding.FriendlyWithBlock(edge.CubeBlock);
		}

		private MyAssembler GetMasterAssembler()
		{
			m_conveyorEndpoints.Clear();
			m_assemblerForPathfinding = this;
			MyGridConveyorSystem.FindReachable(base.ConveyorEndpoint, m_conveyorEndpoints, m_vertexPredicate, m_edgePredicate);
			m_conveyorEndpoints.ShuffleList();
			foreach (IMyConveyorEndpoint conveyorEndpoint in m_conveyorEndpoints)
			{
				MyAssembler myAssembler = conveyorEndpoint.CubeBlock as MyAssembler;
				if (myAssembler != null && !myAssembler.DisassembleEnabled && !myAssembler.IsSlave && myAssembler.m_queue.Count > 0)
				{
					return myAssembler;
				}
			}
			return null;
		}

		private void GetItemFromOtherAssemblers(float remainingTime)
		{
			float num = MySession.Static.AssemblerSpeedMultiplier * (((MyAssemblerDefinition)base.BlockDefinition).AssemblySpeed + base.UpgradeValues["Productivity"]);
			MyAssembler masterAssembler = GetMasterAssembler();
			if (masterAssembler == null)
			{
				return;
			}
			if (masterAssembler.m_repeatAssembleEnabled)
			{
				if (m_queue.Count == 0)
				{
					while (remainingTime > 0f)
					{
						foreach (QueueItem item in masterAssembler.m_queue)
						{
							remainingTime -= (float)(item.Blueprint.BaseProductionTimeInSeconds / num * item.Amount);
							InsertQueueItemRequest(m_queue.Count, item.Blueprint, item.Amount);
						}
					}
				}
			}
			else
			{
				if (masterAssembler.m_queue.Count <= 0)
				{
					return;
				}
				QueueItem? queueItem = masterAssembler.TryGetQueueItem(0);
				if (queueItem.HasValue && queueItem.Value.Amount > 1)
				{
					int num2 = Math.Min((int)queueItem.Value.Amount - 1, Convert.ToInt32(Math.Ceiling(remainingTime / (queueItem.Value.Blueprint.BaseProductionTimeInSeconds / num))));
					if (num2 > 0)
					{
						masterAssembler.RemoveFirstQueueItemAnnounce(num2, masterAssembler.CurrentProgress);
						InsertQueueItemRequest(m_queue.Count, queueItem.Value.Blueprint, num2);
					}
				}
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (m_inventoryOwnersDirty)
			{
				GetCoveyorInventoryOwners();
			}
			if (!Sync.IsServer || !base.IsWorking || !m_useConveyorSystem)
			{
				return;
			}
			if (DisassembleEnabled)
			{
				if (base.OutputInventory.VolumeFillFactor < 0.99f)
				{
					QueueItem? queueItem = TryGetFirstQueueItem();
					if (queueItem.HasValue && !base.OutputInventory.ContainItems(queueItem.Value.Amount, queueItem.Value.Blueprint.Results[0].Id))
					{
						base.CubeGrid.GridSystems.ConveyorSystem.PullItem(queueItem.Value.Blueprint.Results[0].Id, queueItem.Value.Amount, this, base.OutputInventory, remove: false, calcImmediately: false);
					}
				}
				if (base.InputInventory.VolumeFillFactor > 0.75f)
				{
					MyGridConveyorSystem.PushAnyRequest(this, base.InputInventory, base.OwnerId);
				}
				return;
			}
			if (base.InputInventory.VolumeFillFactor < 0.99f)
			{
				bool flag = false;
				int num = 0;
				float num2 = 0f;
				do
				{
					using (MyUtils.ReuseCollection(ref m_requiredComponents))
					{
						float num3 = num2;
						QueueItem? queueItem2 = TryGetQueueItem(num);
						float num4 = 5f - num2;
						if (queueItem2.HasValue)
						{
							float num5 = ((MyAssemblerDefinition)base.BlockDefinition).AssemblySpeed + base.UpgradeValues["Productivity"];
							float num6 = MySession.Static.AssemblerSpeedMultiplier * num5;
							int num7 = 1;
							if (queueItem2.Value.Blueprint.BaseProductionTimeInSeconds / num6 < num4)
							{
								num7 = Math.Min((int)queueItem2.Value.Amount, Convert.ToInt32(Math.Ceiling(num4 / (queueItem2.Value.Blueprint.BaseProductionTimeInSeconds / num6))));
							}
							num2 += (float)num7 * queueItem2.Value.Blueprint.BaseProductionTimeInSeconds / num6;
							if (num2 < 5f)
							{
								flag = true;
							}
							MyFixedPoint b = (MyFixedPoint)(1f / GetEfficiencyMultiplierForBlueprint(queueItem2.Value.Blueprint));
							MyBlueprintDefinitionBase.Item[] prerequisites = queueItem2.Value.Blueprint.Prerequisites;
							for (int i = 0; i < prerequisites.Length; i++)
							{
								MyBlueprintDefinitionBase.Item item = prerequisites[i];
								MyFixedPoint myFixedPoint = item.Amount * b;
								MyFixedPoint myFixedPoint2 = myFixedPoint * num7;
								bool flag2 = false;
								for (int j = 0; j < m_requiredComponents.Count; j++)
								{
									MyBlueprintDefinitionBase.Item item2 = m_requiredComponents[j].Item2;
									if (item2.Id == item.Id)
									{
										item2.Amount += myFixedPoint2;
										MyFixedPoint arg = m_requiredComponents[j].Item1 + myFixedPoint;
										m_requiredComponents[j] = MyTuple.Create(arg, item2);
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									m_requiredComponents.Add(MyTuple.Create(myFixedPoint, new MyBlueprintDefinitionBase.Item
									{
										Amount = myFixedPoint2,
										Id = item.Id
									}));
								}
							}
						}
						foreach (MyTuple<MyFixedPoint, MyBlueprintDefinitionBase.Item> requiredComponent in m_requiredComponents)
						{
							MyBlueprintDefinitionBase.Item item3 = requiredComponent.Item2;
							MyFixedPoint itemAmount = base.InputInventory.GetItemAmount(item3.Id);
							MyFixedPoint myFixedPoint3 = item3.Amount - itemAmount;
							if (!(myFixedPoint3 <= 0) && base.CubeGrid.GridSystems.ConveyorSystem.PullItem(item3.Id, myFixedPoint3, this, base.InputInventory, remove: false, calcImmediately: false) == 0 && requiredComponent.Item1 > itemAmount)
							{
								flag = true;
								num2 = num3;
							}
						}
						num++;
						if (num >= m_queue.Count)
						{
							flag = false;
						}
					}
				}
				while (flag);
				if (IsSlave && !RepeatEnabled)
				{
					float num8 = 5f - num2;
					if (num8 > 0f)
					{
						GetItemFromOtherAssemblers(num8);
					}
				}
			}
			if (base.OutputInventory.VolumeFillFactor > 0.75f)
			{
				MyGridConveyorSystem.PushAnyRequest(this, base.OutputInventory, base.OwnerId);
			}
		}

		protected override void UpdateProduction(int timeDelta)
		{
			if (!base.Enabled)
			{
				CurrentState = StateEnum.Disabled;
				base.IsProducing = false;
				return;
			}
			if ((!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) || base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) < GetOperationalPowerConsumption()) && !base.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, GetOperationalPowerConsumption()))
			{
				CurrentState = StateEnum.NotEnoughPower;
				base.IsProducing = false;
				return;
			}
			if (!base.IsWorking)
			{
				CurrentState = StateEnum.NotWorking;
				base.IsProducing = false;
				return;
			}
			if (base.IsQueueEmpty)
			{
				CurrentState = StateEnum.Ok;
				base.IsProducing = false;
				return;
			}
			int num = 0;
			while (timeDelta > 0 && num < m_queue.Count)
			{
				if (base.IsQueueEmpty)
				{
					CurrentProgress = 0f;
					base.IsProducing = false;
					return;
				}
				if (!m_currentQueueItem.HasValue)
				{
					m_currentQueueItem = TryGetQueueItem(num);
				}
				MyBlueprintDefinitionBase blueprint = m_currentQueueItem.Value.Blueprint;
				CurrentState = CheckInventory(blueprint);
				if (CurrentState != 0)
				{
					num++;
					m_currentQueueItem = null;
					continue;
				}
				float num2 = calculateBlueprintProductionTime(blueprint) - CurrentProgress * calculateBlueprintProductionTime(blueprint);
				if ((float)timeDelta >= num2)
				{
					if (Sync.IsServer)
					{
						if (DisassembleEnabled)
						{
							FinishDisassembling(blueprint);
						}
						else
						{
							if (RepeatEnabled)
							{
								InsertQueueItemRequest(-1, blueprint);
							}
							FinishAssembling(blueprint);
						}
						RemoveQueueItemRequest(m_queue.IndexOf(m_currentQueueItem.Value), 1);
						m_currentQueueItem = null;
					}
					timeDelta -= (int)Math.Ceiling(num2);
					CurrentProgress = 0f;
					m_currentQueueItem = null;
				}
				else
				{
					CurrentProgress += (float)timeDelta / calculateBlueprintProductionTime(blueprint);
					timeDelta = 0;
				}
			}
			base.IsProducing = (base.IsWorking && !base.IsQueueEmpty && CurrentState == StateEnum.Ok);
		}

		private float calculateBlueprintProductionTime(MyBlueprintDefinitionBase currentBlueprint)
		{
			return currentBlueprint.BaseProductionTimeInSeconds * 1000f / (MySession.Static.AssemblerSpeedMultiplier * ((MyAssemblerDefinition)base.BlockDefinition).AssemblySpeed + base.UpgradeValues["Productivity"]);
		}

		private void FinishAssembling(MyBlueprintDefinitionBase blueprint)
		{
			MyFixedPoint b = (MyFixedPoint)(1f / GetEfficiencyMultiplierForBlueprint(blueprint));
			for (int i = 0; i < blueprint.Prerequisites.Length; i++)
			{
				MyBlueprintDefinitionBase.Item item = blueprint.Prerequisites[i];
				base.InputInventory.RemoveItemsOfType(item.Amount * b, item.Id);
			}
			MyBlueprintDefinitionBase.Item[] results = blueprint.Results;
			for (int j = 0; j < results.Length; j++)
			{
				MyBlueprintDefinitionBase.Item item2 = results[j];
				MyObjectBuilderType typeId = item2.Id.TypeId;
				MyDefinitionId id = item2.Id;
				MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(typeId, id.SubtypeName);
				base.OutputInventory.AddItems(item2.Amount, myObjectBuilder_PhysicalObject);
				if (MyVisualScriptLogicProvider.NewItemBuilt != null)
				{
					NewBuiltItemEvent newItemBuilt = MyVisualScriptLogicProvider.NewItemBuilt;
					long entityId = base.EntityId;
					long entityId2 = base.CubeGrid.EntityId;
					string name = Name;
					string name2 = base.CubeGrid.Name;
					string itemTypeName = myObjectBuilder_PhysicalObject.TypeId.ToString();
					string subtypeName = myObjectBuilder_PhysicalObject.SubtypeName;
					MyFixedPoint amount = item2.Amount;
					newItemBuilt(entityId, entityId2, name, name2, itemTypeName, subtypeName, amount.ToIntSafe());
				}
			}
		}

		private void FinishDisassembling(MyBlueprintDefinitionBase blueprint)
		{
			if (RepeatEnabled && Sync.IsServer)
			{
				base.OutputInventory.ContentsChanged -= OutputInventory_ContentsChanged;
			}
			MyBlueprintDefinitionBase.Item[] results = blueprint.Results;
			for (int i = 0; i < results.Length; i++)
			{
				MyBlueprintDefinitionBase.Item item = results[i];
				base.OutputInventory.RemoveItemsOfType(item.Amount, item.Id);
			}
			if (RepeatEnabled && Sync.IsServer)
			{
				base.OutputInventory.ContentsChanged += OutputInventory_ContentsChanged;
			}
			MyFixedPoint b = (MyFixedPoint)(1f / GetEfficiencyMultiplierForBlueprint(blueprint));
			for (int j = 0; j < blueprint.Prerequisites.Length; j++)
			{
				MyBlueprintDefinitionBase.Item item2 = blueprint.Prerequisites[j];
				MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(item2.Id.TypeId, item2.Id.SubtypeName);
				base.InputInventory.AddItems(item2.Amount * b, objectBuilder);
			}
		}

		private StateEnum CheckInventory(MyBlueprintDefinitionBase blueprint)
		{
			MyFixedPoint amountMultiplier = (MyFixedPoint)(1f / GetEfficiencyMultiplierForBlueprint(blueprint));
			if (DisassembleEnabled)
			{
				if (!CheckInventoryCapacity(base.InputInventory, blueprint.Prerequisites, amountMultiplier))
				{
					return StateEnum.InventoryFull;
				}
				if (!CheckInventoryContents(base.OutputInventory, blueprint.Results, 1))
				{
					return StateEnum.MissingItems;
				}
			}
			else
			{
				if (!CheckInventoryCapacity(base.OutputInventory, blueprint.Results, 1))
				{
					return StateEnum.InventoryFull;
				}
				if (!CheckInventoryContents(base.InputInventory, blueprint.Prerequisites, amountMultiplier))
				{
					return StateEnum.MissingItems;
				}
			}
			return StateEnum.Ok;
		}

		private bool CheckInventoryCapacity(MyInventory inventory, MyBlueprintDefinitionBase.Item item, MyFixedPoint amountMultiplier)
		{
			return inventory.CanItemsBeAdded(item.Amount * amountMultiplier, item.Id);
		}

		private bool CheckInventoryCapacity(MyInventory inventory, MyBlueprintDefinitionBase.Item[] items, MyFixedPoint amountMultiplier)
		{
			if (MySession.Static.CreativeMode)
			{
				return true;
			}
			MyFixedPoint b = 0;
			for (int i = 0; i < items.Length; i++)
			{
				MyBlueprintDefinitionBase.Item item = items[i];
				MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Id);
				if (physicalItemDefinition != null)
				{
					b += (MyFixedPoint)physicalItemDefinition.Volume * item.Amount * amountMultiplier;
				}
			}
			return inventory.CurrentVolume + b <= inventory.MaxVolume;
		}

		private bool CheckInventoryContents(MyInventory inventory, MyBlueprintDefinitionBase.Item item, MyFixedPoint amountMultiplier)
		{
			return inventory.ContainItems(item.Amount * amountMultiplier, item.Id);
		}

		private bool CheckInventoryContents(MyInventory inventory, MyBlueprintDefinitionBase.Item[] item, MyFixedPoint amountMultiplier)
		{
			for (int i = 0; i < item.Length; i++)
			{
				if (!inventory.ContainItems(item[i].Amount * amountMultiplier, item[i].Id))
				{
					return false;
				}
			}
			return true;
		}

		protected override void OnQueueChanged()
		{
			if (CurrentState == StateEnum.MissingItems && base.IsQueueEmpty)
			{
				CurrentState = ((!base.Enabled) ? StateEnum.Disabled : ((!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) ? StateEnum.NotEnoughPower : ((!base.IsFunctional) ? StateEnum.NotWorking : StateEnum.Ok)));
			}
			base.IsProducing = (base.IsWorking && !base.IsQueueEmpty);
			base.OnQueueChanged();
		}

		protected override void RemoveFirstQueueItem(int index, MyFixedPoint amount, float progress = 0f)
		{
			CurrentProgress = progress;
			if (CurrentItemIndex == index)
			{
				m_currentQueueItem = null;
			}
			base.RemoveFirstQueueItem(index, amount);
		}

		protected override void RemoveQueueItem(int itemIdx)
		{
			if (itemIdx == 0)
			{
				CurrentProgress = 0f;
			}
			base.RemoveQueueItem(itemIdx);
		}

		protected override void InsertQueueItem(int idx, MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			if (idx == 0)
			{
				QueueItem? queueItem = TryGetFirstQueueItem();
				if (queueItem.HasValue && queueItem.Value.Blueprint != blueprint)
				{
					CurrentProgress = 0f;
				}
			}
			base.InsertQueueItem(idx, blueprint, amount);
		}

		private void SetRepeat(ref bool currentValue, bool newValue)
		{
			if (currentValue != newValue)
			{
				currentValue = newValue;
				RebuildQueueInRepeatDisassembling();
				if (this.CurrentModeChanged != null)
				{
					this.CurrentModeChanged(this);
				}
			}
		}

		private void OnSlaveChanged()
		{
			if (this.CurrentModeChanged != null)
			{
				this.CurrentModeChanged(this);
			}
		}

		private void OutputInventory_ContentsChanged(MyInventoryBase inventory)
		{
			if (DisassembleEnabled && RepeatEnabled && Sync.IsServer)
			{
				RebuildQueueInRepeatDisassembling();
			}
		}

		public void RequestDisassembleEnabled(bool newDisassembleEnabled)
		{
			if (newDisassembleEnabled != DisassembleEnabled)
			{
				MyMultiplayer.RaiseEvent(this, (MyAssembler x) => x.ModeSwitchCallback, newDisassembleEnabled);
			}
		}

		[Event(null, 837)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void ModeSwitchCallback(bool disassembleEnabled)
		{
			MyMultiplayer.RaiseEvent(this, (MyAssembler x) => x.ModeSwitchClient, disassembleEnabled);
			DisassembleEnabled = disassembleEnabled;
		}

		[Event(null, 844)]
		[Reliable]
		[Broadcast]
		private void ModeSwitchClient(bool disassembleEnabled)
		{
			DisassembleEnabled = disassembleEnabled;
		}

		public void RequestRepeatEnabled(bool newRepeatEnable)
		{
			if (newRepeatEnable != RepeatEnabled)
			{
				MyMultiplayer.RaiseEvent(this, (MyAssembler x) => x.RepeatEnabledCallback, DisassembleEnabled, newRepeatEnable);
			}
		}

		[Event(null, 856)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void RepeatEnabledCallback(bool disassembleEnabled, bool repeatEnable)
		{
			RepeatEnabledSuccess(disassembleEnabled, repeatEnable);
		}

		public void RequestDisassembleAll()
		{
			MyMultiplayer.RaiseEvent(this, (MyAssembler x) => x.DisassembleAllCallback);
		}

		[Event(null, 867)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DisassembleAllCallback()
		{
			DisassembleAllInOutput();
		}

		private void RepeatEnabledSuccess(bool disassembleMode, bool repeatEnabled)
		{
			if (disassembleMode)
			{
				SetRepeat(ref m_repeatDisassembleEnabled, repeatEnabled);
			}
			else
			{
				SetRepeat(ref m_repeatAssembleEnabled, repeatEnabled);
			}
		}

		private void RebuildQueueInRepeatDisassembling()
		{
			if (DisassembleEnabled && RepeatEnabled)
			{
				RequestDisassembleAll();
			}
		}

		private void UpdateInventoryFlags()
		{
			base.OutputInventory.SetFlags(DisassembleEnabled ? MyInventoryFlags.CanReceive : MyInventoryFlags.CanSend);
			base.InputInventory.SetFlags((!DisassembleEnabled) ? MyInventoryFlags.CanReceive : MyInventoryFlags.CanSend);
		}

		private void DisassembleAllInOutput()
		{
			ClearQueue(sendEvent: false);
			List<MyPhysicalInventoryItem> items = base.OutputInventory.GetItems();
			List<Tuple<MyBlueprintDefinitionBase, MyFixedPoint>> list = new List<Tuple<MyBlueprintDefinitionBase, MyFixedPoint>>();
			bool flag = true;
			foreach (MyPhysicalInventoryItem item4 in items)
			{
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(item4.Content.GetId());
				if (myBlueprintDefinitionBase == null)
				{
					flag = false;
					list.Clear();
					break;
				}
				Tuple<MyBlueprintDefinitionBase, MyFixedPoint> item = Tuple.Create(myBlueprintDefinitionBase, item4.Amount);
				list.Add(item);
			}
			if (flag)
			{
				foreach (Tuple<MyBlueprintDefinitionBase, MyFixedPoint> item5 in list)
				{
					InsertQueueItemRequest(-1, item5.Item1, item5.Item2);
				}
				return;
			}
			InitializeInventoryCounts(inputInventory: false);
			for (int i = 0; i < m_assemblerDef.BlueprintClasses.Count; i++)
			{
				foreach (MyBlueprintDefinitionBase item6 in m_assemblerDef.BlueprintClasses[i])
				{
					MyFixedPoint myFixedPoint = MyFixedPoint.MaxValue;
					MyBlueprintDefinitionBase.Item[] results = item6.Results;
					MyFixedPoint value;
					for (int j = 0; j < results.Length; j++)
					{
						MyBlueprintDefinitionBase.Item item2 = results[j];
						value = 0;
						MyProductionBlock.m_tmpInventoryCounts.TryGetValue(item2.Id, out value);
						if (value == 0)
						{
							myFixedPoint = 0;
							break;
						}
						myFixedPoint = MyFixedPoint.Min((MyFixedPoint)((double)value / (double)item2.Amount), myFixedPoint);
					}
					if (item6.Atomic)
					{
						myFixedPoint = MyFixedPoint.Floor(myFixedPoint);
					}
					if (myFixedPoint > 0)
					{
						InsertQueueItemRequest(-1, item6, myFixedPoint);
						results = item6.Results;
						for (int j = 0; j < results.Length; j++)
						{
							MyBlueprintDefinitionBase.Item item3 = results[j];
							MyProductionBlock.m_tmpInventoryCounts.TryGetValue(item3.Id, out value);
							value -= item3.Amount * myFixedPoint;
							if (value == 0)
							{
								MyProductionBlock.m_tmpInventoryCounts.Remove(item3.Id);
							}
							else
							{
								MyProductionBlock.m_tmpInventoryCounts[item3.Id] = value;
							}
						}
					}
				}
			}
			MyProductionBlock.m_tmpInventoryCounts.Clear();
		}

		public void GetCoveyorInventoryOwners()
		{
			m_inventoryOwners.Clear();
			List<IMyConveyorEndpoint> list = new List<IMyConveyorEndpoint>();
			MyGridConveyorSystem.FindReachable(base.ConveyorEndpoint, list, (IMyConveyorEndpoint vertex) => vertex.CubeBlock != null && FriendlyWithBlock(vertex.CubeBlock) && vertex.CubeBlock.HasInventory);
			foreach (IMyConveyorEndpoint item in list)
			{
				m_inventoryOwners.Add(item.CubeBlock);
			}
			m_inventoryOwnersDirty = false;
		}

		public bool CheckConveyorResources(MyFixedPoint? amount, MyDefinitionId contentId)
		{
			foreach (MyEntity inventoryOwner in m_inventoryOwners)
			{
				if (inventoryOwner != null)
				{
					MyEntity myEntity = inventoryOwner;
					if (myEntity != null && myEntity.HasInventory)
					{
						MyInventoryFlags flags = myEntity.GetInventory().GetFlags();
						MyInventoryFlags myInventoryFlags = MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend;
						List<MyInventory> list = new List<MyInventory>();
						for (int i = 0; i < myEntity.InventoryCount; i++)
						{
							list.Add(myEntity.GetInventory(i));
						}
						foreach (MyInventory item in list)
						{
							if (item.ContainItems(amount, contentId) && (flags == myInventoryFlags || flags == MyInventoryFlags.CanSend || myEntity == this))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		protected override float GetOperationalPowerConsumption()
		{
			return base.GetOperationalPowerConsumption() * (1f + base.UpgradeValues["Productivity"]) * (1f / base.UpgradeValues["PowerEfficiency"]);
		}

		protected override void OnInventoryAddedToAggregate(MyInventoryAggregate aggregate, MyInventoryBase inventory)
		{
			base.OnInventoryAddedToAggregate(aggregate, inventory);
			if (inventory == base.OutputInventory && Sync.IsServer)
			{
				base.OutputInventory.ContentsChanged += OutputInventory_ContentsChanged;
			}
		}

		protected override void OnBeforeInventoryRemovedFromAggregate(MyInventoryAggregate aggregate, MyInventoryBase inventory)
		{
			base.OnBeforeInventoryRemovedFromAggregate(aggregate, inventory);
			if (inventory == base.OutputInventory && Sync.IsServer)
			{
				base.OutputInventory.ContentsChanged -= OutputInventory_ContentsChanged;
			}
		}

		private Action<MyAssembler> GetDelegate(Action<Sandbox.ModAPI.IMyAssembler> value)
		{
			return (Action<MyAssembler>)Delegate.CreateDelegate(typeof(Action<MyAssembler>), value.Target, value.Method);
		}

		public virtual float GetEfficiencyMultiplierForBlueprint(MyBlueprintDefinitionBase blueprint)
		{
			return MySession.Static.AssemblerEfficiencyMultiplier;
		}
	}
}
