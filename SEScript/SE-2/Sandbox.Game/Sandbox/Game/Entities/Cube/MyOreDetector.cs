using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_OreDetector))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyOreDetector),
		typeof(Sandbox.ModAPI.Ingame.IMyOreDetector)
	})]
	public class MyOreDetector : MyFunctionalBlock, IMyComponentOwner<MyOreDetectorComponent>, Sandbox.ModAPI.IMyOreDetector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyOreDetector
	{
		protected class m_broadcastUsingAntennas_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType broadcastUsingAntennas;
				ISyncType result = broadcastUsingAntennas = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyOreDetector)P_0).m_broadcastUsingAntennas = (Sync<bool, SyncDirection.BothWays>)broadcastUsingAntennas;
				return result;
			}
		}

		protected class m_range_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType range;
				ISyncType result = range = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyOreDetector)P_0).m_range = (Sync<float, SyncDirection.BothWays>)range;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyOreDetector_003C_003EActor : IActivator, IActivator<MyOreDetector>
		{
			private sealed override object CreateInstance()
			{
				return new MyOreDetector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOreDetector CreateInstance()
			{
				return new MyOreDetector();
			}

			MyOreDetector IActivator<MyOreDetector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyOreDetectorDefinition m_definition;

		private readonly MyOreDetectorComponent m_oreDetectorComponent = new MyOreDetectorComponent();

		private Sync<bool, SyncDirection.BothWays> m_broadcastUsingAntennas;

		private Sync<float, SyncDirection.BothWays> m_range;

		private static readonly short UPDATE_HUD_TIMEOUT = 200;

		public float Range
		{
			get
			{
				return m_oreDetectorComponent.DetectionRadius / m_definition.MaximumRange * 100f;
			}
			set
			{
				m_range.Value = value;
			}
		}

		public bool BroadcastUsingAntennas
		{
			get
			{
				return m_oreDetectorComponent.BroadcastUsingAntennas;
			}
			set
			{
				m_oreDetectorComponent.BroadcastUsingAntennas = value;
				RaisePropertiesChanged();
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyOreDetector.BroadcastUsingAntennas
		{
			get
			{
				return BroadcastUsingAntennas;
			}
			set
			{
				BroadcastUsingAntennas = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyOreDetector.Range => Range;

		public MyOreDetector()
		{
			CreateTerminalControls();
			m_broadcastUsingAntennas.ValueChanged += delegate
			{
				BroadcastChanged();
			};
			m_range.ValueChanged += delegate
			{
				UpdateRange();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyOreDetector>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MyOreDetector> myTerminalControlSlider = new MyTerminalControlSlider<MyOreDetector>("Range", MySpaceTexts.BlockPropertyTitle_OreDetectorRange, MySpaceTexts.BlockPropertyDescription_OreDetectorRange);
				myTerminalControlSlider.SetLimits((MyOreDetector x) => 0f, (MyOreDetector x) => x.m_definition.MaximumRange);
				myTerminalControlSlider.DefaultValue = 100f;
				myTerminalControlSlider.Getter = ((MyOreDetector x) => x.Range * x.m_definition.MaximumRange * 0.01f);
				myTerminalControlSlider.Setter = delegate(MyOreDetector x, float v)
				{
					x.Range = v / x.m_definition.MaximumRange * 100f;
				};
				myTerminalControlSlider.Writer = delegate(MyOreDetector x, StringBuilder result)
				{
					result.AppendInt32((int)x.m_oreDetectorComponent.DetectionRadius).Append(" m");
				};
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlCheckbox<MyOreDetector> obj = new MyTerminalControlCheckbox<MyOreDetector>("BroadcastUsingAntennas", MySpaceTexts.BlockPropertyDescription_BroadcastUsingAntennas, MySpaceTexts.BlockPropertyDescription_BroadcastUsingAntennas)
				{
					Getter = ((MyOreDetector x) => x.m_oreDetectorComponent.BroadcastUsingAntennas),
					Setter = delegate(MyOreDetector x, bool v)
					{
						x.m_broadcastUsingAntennas.Value = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		private void BroadcastChanged()
		{
			BroadcastUsingAntennas = m_broadcastUsingAntennas;
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			m_definition = (base.BlockDefinition as MyOreDetectorDefinition);
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(m_definition.ResourceSinkGroup, 0.002f, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			base.ResourceSink = myResourceSinkComponent;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_OreDetector myObjectBuilder_OreDetector = objectBuilder as MyObjectBuilder_OreDetector;
			if (myObjectBuilder_OreDetector.DetectionRadius != 0f)
			{
				m_oreDetectorComponent.DetectionRadius = MathHelper.Clamp(myObjectBuilder_OreDetector.DetectionRadius, 1f, m_definition.MaximumRange);
			}
			else
			{
				m_oreDetectorComponent.DetectionRadius = MathHelper.Clamp(0.5f * m_definition.MaximumRange, 1f, m_definition.MaximumRange);
			}
			m_oreDetectorComponent.BroadcastUsingAntennas = myObjectBuilder_OreDetector.BroadcastUsingAntennas;
			m_broadcastUsingAntennas.SetLocalValue(m_oreDetectorComponent.BroadcastUsingAntennas);
			MyOreDetectorComponent oreDetectorComponent = m_oreDetectorComponent;
			oreDetectorComponent.OnCheckControl = (MyOreDetectorComponent.CheckControlDelegate)Delegate.Combine(oreDetectorComponent.OnCheckControl, new MyOreDetectorComponent.CheckControlDelegate(OnCheckControl));
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			base.OnClose += MyOreDetector_OnClose;
		}

		private void MyOreDetector_OnClose(MyEntity obj)
		{
			if (m_oreDetectorComponent != null)
			{
				m_oreDetectorComponent.DiscardNextQuery();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_OreDetector obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_OreDetector;
			obj.DetectionRadius = m_oreDetectorComponent.DetectionRadius;
			obj.BroadcastUsingAntennas = m_oreDetectorComponent.BroadcastUsingAntennas;
			return obj;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			if (!base.Enabled)
			{
				m_oreDetectorComponent.Clear();
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			if (!base.IsWorking)
			{
				m_oreDetectorComponent.Clear();
			}
		}

		public override void OnUnregisteredFromGridSystems()
		{
			m_oreDetectorComponent.Clear();
			base.OnUnregisteredFromGridSystems();
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			bool flag = HasLocalPlayerAccess();
			if (!base.IsWorking)
			{
				return;
			}
			bool flag2 = false;
			if (flag && MySession.Static.LocalCharacter != null)
			{
				if (m_oreDetectorComponent.BroadcastUsingAntennas)
				{
					MyCharacter localCharacter = MySession.Static.LocalCharacter;
					MyCubeGrid myCubeGrid = GetTopMostParent() as MyCubeGrid;
					if (myCubeGrid != null)
					{
						MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid);
						if (group != null && localCharacter.HasAccessToLogicalGroup(group.GroupData))
						{
							flag2 = true;
						}
					}
				}
				else
				{
					IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
					if (controlledEntity != null && controlledEntity.Entity != null)
					{
						MyCubeGrid myCubeGrid2 = controlledEntity.Entity.GetTopMostParent() as MyCubeGrid;
						if (myCubeGrid2 != null)
						{
							MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group2 = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid2);
							MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group3 = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
							if (myCubeGrid2 == base.CubeGrid || (group2 != null && group3 != null && group2.GroupData == group3.GroupData))
							{
								flag2 = true;
							}
						}
					}
				}
			}
			if (flag2)
			{
				m_oreDetectorComponent.Update(base.PositionComp.GetPosition(), base.EntityId, checkControl: false);
				m_oreDetectorComponent.SetRelayedRequest = true;
			}
			else
			{
				m_oreDetectorComponent.Clear();
			}
		}

		private bool OnCheckControl()
		{
			MyCubeGrid myCubeGrid = null;
			if (MySession.Static.ControlledEntity != null)
			{
				myCubeGrid = (MySession.Static.ControlledEntity.Entity.Parent as MyCubeGrid);
			}
			if (myCubeGrid == null)
			{
				return false;
			}
			bool flag = myCubeGrid == base.CubeGrid || MyCubeGridGroups.Static.Logical.HasSameGroup(myCubeGrid, base.CubeGrid);
			return base.IsWorking && flag;
		}

		private void UpdateRange()
		{
			m_oreDetectorComponent.DetectionRadius = (float)m_range / 100f * m_definition.MaximumRange;
			RaisePropertiesChanged();
			CheckEmissiveState();
		}

		public override void CheckEmissiveState(bool force = false)
		{
			base.CheckEmissiveState(force);
			if (base.IsWorking)
			{
				if (m_oreDetectorComponent.DetectionRadius < 1E-05f)
				{
					SetEmissiveStateDisabled();
				}
				else
				{
					SetEmissiveStateWorking();
				}
			}
			else if (base.IsFunctional)
			{
				SetEmissiveStateDisabled();
			}
			else
			{
				SetEmissiveStateDamaged();
			}
		}

		bool IMyComponentOwner<MyOreDetectorComponent>.GetComponent(out MyOreDetectorComponent component)
		{
			component = m_oreDetectorComponent;
			return base.IsWorking;
		}
	}
}
