using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ShipGrinder))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyShipGrinder),
		typeof(Sandbox.ModAPI.Ingame.IMyShipGrinder)
	})]
	public class MyShipGrinder : MyShipToolBase, Sandbox.ModAPI.IMyShipGrinder, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyShipGrinder
	{
		private class MyShipGrinderSubpart : MyEntitySubpart
		{
			private class Sandbox_Game_Weapons_MyShipGrinder_003C_003EMyShipGrinderSubpart_003C_003EActor : IActivator, IActivator<MyShipGrinderSubpart>
			{
				private sealed override object CreateInstance()
				{
					return new MyShipGrinderSubpart();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyShipGrinderSubpart CreateInstance()
				{
					return new MyShipGrinderSubpart();
				}

				MyShipGrinderSubpart IActivator<MyShipGrinderSubpart>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public new MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade Render => (MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade)base.Render;

			public override void InitComponents()
			{
				base.Render = new MyRenderComponentShipGrinder.MyRenderComponentShipGrinderBlade();
				base.InitComponents();
			}
		}

		private class Sandbox_Game_Weapons_MyShipGrinder_003C_003EActor : IActivator, IActivator<MyShipGrinder>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipGrinder();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipGrinder CreateInstance()
			{
				return new MyShipGrinder();
			}

			MyShipGrinder IActivator<MyShipGrinder>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MySoundPair IDLE_SOUND = new MySoundPair("ToolPlayGrindIdle");

		private static MySoundPair METAL_SOUND = new MySoundPair("ToolPlayGrindMetal");

		private const string PARTICLE_EFFECT = "ShipGrinder";

		private static string[] BLADE_SUBPART_IDs = new string[2]
		{
			"grinder1",
			"grinder2"
		};

		private MyParticleEffect m_particleEffect1;

		private MyParticleEffect m_particleEffect2;

		private MyFlareDefinition m_flare;

		private MyShipGrinderDefinition m_grinderDef;

		private const float RANDOM_IMPULSE_SCALE = 500f;

		private static List<MyPhysicalInventoryItem> m_tmpItemList = new List<MyPhysicalInventoryItem>();

		private bool m_wantsToShake;

		private MyCubeGrid m_otherGrid;

		private Matrix m_particleDummyMatrix1;

		private Matrix m_particleDummyMatrix2;

		private List<MyShipGrinderSubpart> m_bladeSubparts = new List<MyShipGrinderSubpart>();

		public override void InitComponents()
		{
			base.Render = new MyRenderComponentShipGrinder();
			base.InitComponents();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			if (base.CubeGrid.GridSizeEnum == MyCubeSize.Large)
			{
				IDLE_SOUND.Init("ToolLrgGrindIdle");
				METAL_SOUND.Init("ToolLrgGrindMetal");
			}
			m_grinderDef = (base.BlockDefinition as MyShipGrinderDefinition);
			if (m_grinderDef != null && m_grinderDef.Flare != "")
			{
				MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), m_grinderDef.Flare);
				m_flare = (MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition);
			}
			base.HeatUpFrames = 15;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			LoadParticleDummyMatrices();
		}

		private void LoadParticleDummyMatrices()
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(base.BlockDefinition.Model).Dummies)
			{
				if (dummy.Key.ToLower().Contains("particles1"))
				{
					m_particleDummyMatrix1 = dummy.Value.Matrix;
				}
				else if (dummy.Key.ToLower().Contains("particles2"))
				{
					m_particleDummyMatrix2 = dummy.Value.Matrix;
				}
			}
		}

		public override void OnControlAcquired(MyCharacter owner)
		{
			base.OnControlAcquired(owner);
			if (owner != null && owner.Parent != null && owner == MySession.Static.LocalCharacter && !owner.Parent.Components.Contains(typeof(MyCasterComponent)))
			{
				MyCasterComponent component = new MyCasterComponent(new MyDrillSensorRayCast(0f, DEFAULT_REACH_DISTANCE, base.BlockDefinition));
				owner.Parent.Components.Add(component);
				m_controller = owner;
			}
		}

		public override void OnControlReleased()
		{
			base.OnControlReleased();
			if (m_controller != null && m_controller.Parent != null && m_controller == MySession.Static.LocalCharacter && m_controller.Parent.Components.Contains(typeof(MyCasterComponent)))
			{
				m_controller.Parent.Components.Remove(typeof(MyCasterComponent));
			}
		}

		protected override bool Activate(HashSet<MySlimBlock> targets)
		{
			int num = targets.Count;
			m_otherGrid = null;
			if (targets.Count > 0)
			{
				m_otherGrid = targets.FirstElement().CubeGrid;
			}
			float num2 = 0.25f / (float)Math.Min(4, targets.Count);
			foreach (MySlimBlock target in targets)
			{
				if ((!MySession.Static.IsScenario && !MySession.Static.Settings.ScenarioEditMode) || target.CubeGrid.BlocksDestructionEnabled)
				{
					m_otherGrid = target.CubeGrid;
					if (m_otherGrid.Physics == null || !m_otherGrid.Physics.Enabled)
					{
						num--;
					}
					else
					{
						MyCubeBlockDefinition.PreloadConstructionModels(target.BlockDefinition);
						if (Sync.IsServer)
						{
							float amount = MySession.Static.GrinderSpeedMultiplier * 4f * num2;
							MyDamageInformation info = new MyDamageInformation(isDeformation: false, amount, MyDamageType.Grind, base.EntityId);
							if (target.UseDamageSystem)
							{
								MyDamageSystem.Static.RaiseBeforeDamageApplied(target, ref info);
							}
							if (target.CubeGrid.Editable)
							{
								target.DecreaseMountLevel(info.Amount, this.GetInventory(), useDefaultDeconstructEfficiency: false, base.OwnerId);
								target.MoveItemsFromConstructionStockpile(this.GetInventory());
							}
							if (target.UseDamageSystem)
							{
								MyDamageSystem.Static.RaiseAfterDamageApplied(target, info);
							}
							if (target.IsFullyDismounted)
							{
								if (target.FatBlock != null && target.FatBlock.HasInventory)
								{
									EmptyBlockInventories(target.FatBlock);
								}
								if (target.UseDamageSystem)
								{
									MyDamageSystem.Static.RaiseDestroyed(target, info);
								}
								target.SpawnConstructionStockpile();
								target.CubeGrid.RazeBlock(target.Min, 0uL);
							}
						}
						if (num > 0)
						{
							SetBuildingMusic(200);
						}
					}
				}
			}
			m_wantsToShake = (num != 0);
			return num != 0;
		}

		private void EmptyBlockInventories(MyCubeBlock block)
		{
			for (int i = 0; i < block.InventoryCount; i++)
			{
				MyInventory inventory = block.GetInventory(i);
				if (!inventory.Empty())
				{
					m_tmpItemList.Clear();
					m_tmpItemList.AddRange(inventory.GetItems());
					foreach (MyPhysicalInventoryItem tmpItem in m_tmpItemList)
					{
						MyInventory.Transfer(inventory, this.GetInventory(), tmpItem.ItemId);
					}
				}
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_wantsToShake && m_otherGrid != null && m_otherGrid.Physics != null && !m_otherGrid.Physics.IsStatic && MySession.Static.EnableToolShake && MyFakes.ENABLE_TOOL_SHAKE)
			{
				Vector3 randomVector = MyUtils.GetRandomVector3();
				ApplyImpulse(m_otherGrid, randomVector);
				if (base.CubeGrid.Physics != null && !base.CubeGrid.Physics.IsStatic)
				{
					ApplyImpulse(base.CubeGrid, randomVector);
				}
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeActivate >= 250)
			{
				m_wantsToShake = false;
				m_otherGrid = null;
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Sync.IsServer && base.IsFunctional && base.UseConveyorSystem && this.GetInventory().GetItems().Count > 0)
			{
				MyGridConveyorSystem.PushAnyRequest(this, this.GetInventory(), base.OwnerId);
			}
		}

		protected override void StartAnimation()
		{
			base.StartAnimation();
			foreach (MyShipGrinderSubpart bladeSubpart in m_bladeSubparts)
			{
				bladeSubpart.Render.UpdateBladeSpeed(MathF.PI * 5f);
			}
		}

		protected override void StopAnimation()
		{
			base.StopAnimation();
			foreach (MyShipGrinderSubpart bladeSubpart in m_bladeSubparts)
			{
				bladeSubpart.Render.UpdateBladeSpeed(0f);
			}
		}

		protected override void StartEffects()
		{
			Vector3D worldPosition = base.WorldMatrix.Translation;
			MatrixD effectMatrix = m_particleDummyMatrix1 * base.PositionComp.LocalMatrix;
			MyParticlesManager.TryCreateParticleEffect("ShipGrinder", ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_particleEffect1);
			effectMatrix = m_particleDummyMatrix2 * base.PositionComp.LocalMatrix;
			MyParticlesManager.TryCreateParticleEffect("ShipGrinder", ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_particleEffect2);
		}

		protected override void StopEffects()
		{
			if (m_particleEffect1 != null)
			{
				m_particleEffect1.Stop();
				m_particleEffect1 = null;
			}
			if (m_particleEffect2 != null)
			{
				m_particleEffect2.Stop();
				m_particleEffect2 = null;
			}
		}

		protected override void StopLoopSound()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		protected override void PlayLoopSound(bool activated)
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.PlaySingleSound(activated ? METAL_SOUND : IDLE_SOUND, stopPrevious: true, m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying);
			}
		}

		private void ApplyImpulse(MyCubeGrid grid, Vector3 force)
		{
			MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(grid);
			if (((Sync.IsServer && controllingPlayer == null) || MySession.Static.LocalHumanPlayer == controllingPlayer) && grid.Physics != null)
			{
				grid.Physics.ApplyImpulse(force * base.CubeGrid.GridSize * 500f, base.PositionComp.GetPosition());
			}
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Grinding, shooter, 0uL))
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			return base.CanShoot(action, shooter, out status);
		}

		public override void RefreshModels(string model, string modelCollision)
		{
			m_bladeSubparts.Clear();
			base.RefreshModels(model, modelCollision);
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			if (BLADE_SUBPART_IDs.Contains(data.Name))
			{
				MyShipGrinderSubpart myShipGrinderSubpart = new MyShipGrinderSubpart();
				m_bladeSubparts.Add(myShipGrinderSubpart);
				return myShipGrinderSubpart;
			}
			return base.InstantiateSubpart(subpartDummy, ref data);
		}

		public override PullInformation GetPullInformation()
		{
			return null;
		}

		public override PullInformation GetPushInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
		}
	}
}
