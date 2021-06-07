using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.RenderDirect.ActorComponents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Thrust))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyThrust),
		typeof(Sandbox.ModAPI.Ingame.IMyThrust)
	})]
	public class MyThrust : MyFunctionalBlock, Sandbox.ModAPI.IMyThrust, Sandbox.ModAPI.Ingame.IMyThrust, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyConveyorEndpointBlock
	{
		protected class m_thrustOverride_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType thrustOverride;
				ISyncType result = thrustOverride = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyThrust)P_0).m_thrustOverride = (Sync<float, SyncDirection.BothWays>)thrustOverride;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyThrust_003C_003EActor : IActivator, IActivator<MyThrust>
		{
			private sealed override object CreateInstance()
			{
				return new MyThrust();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyThrust CreateInstance()
			{
				return new MyThrust();
			}

			MyThrust IActivator<MyThrust>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Vector3D m_particleLocalOffset = Vector3D.Zero;

		private MyParticleEffect m_landingEffect;

		private static int m_maxNumberLandingEffects = 10;

		private static int m_landingEffectCount = 0;

		private MyPhysics.HitInfo? m_lastHitInfo;

		private MyEntityThrustComponent m_thrustComponent;

		public float ThrustLengthRand;

		private float m_maxBillboardDistanceSquared;

		private bool m_propellerActive;

		private MyEntity m_propellerEntity;

		private bool m_flamesCalculate;

		private bool m_propellerCalculate;

		private float m_propellerMaxDistance;

		private static readonly ConcurrentDictionary<string, List<MyThrustFlameAnimator.FlameInfo>> m_flameCache = new ConcurrentDictionary<string, List<MyThrustFlameAnimator.FlameInfo>>();

		private ListReader<MyThrustFlameAnimator.FlameInfo> m_flames;

		private const int FRAME_DELAY = 100;

		private static readonly List<HkBodyCollision> m_flameCollisionsList = new List<HkBodyCollision>();

		private int m_parallelThrustDamageTaskCount;

		public float LastKnownForceMultiplier;

		private float m_currentStrength;

		private bool m_renderNeedsUpdate;

		private readonly Sync<float, SyncDirection.BothWays> m_thrustOverride;

		private MyStringId m_flameLengthMaterialId;

		private MyStringId m_flamePointMaterialId;

		private static HashSet<HkShape> m_blockSet = new HashSet<HkShape>();

		private static List<VRage.ModAPI.IMyEntity> m_alreadyDamagedEntities = new List<VRage.ModAPI.IMyEntity>();

		private float m_thrustMultiplier = 1f;

		private float m_powerConsumptionMultiplier = 1f;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		public new MyThrustDefinition BlockDefinition
		{
			get;
			private set;
		}

		public new MyRenderComponentThrust Render
		{
			get
			{
				return (MyRenderComponentThrust)base.Render;
			}
			set
			{
				base.Render = value;
			}
		}

		public MyFuelConverterInfo FuelConverterDefinition
		{
			get;
			private set;
		}

		public MyFlareDefinition Flares
		{
			get;
			private set;
		}

		public MyGasProperties FuelDefinition
		{
			get;
			private set;
		}

		public MyEntity Propeller => m_propellerEntity;

		public Vector3 ThrustForce => -ThrustForwardVector * (BlockDefinition.ForceMagnitude * m_thrustMultiplier);

		public float ThrustForceLength => BlockDefinition.ForceMagnitude * m_thrustMultiplier;

		public float ThrustOverride
		{
			get
			{
				return (float)m_thrustOverride * m_thrustMultiplier * BlockDefinition.ForceMagnitude * 0.01f;
			}
			set
			{
				float num = value / (m_thrustMultiplier * BlockDefinition.ForceMagnitude * 0.01f);
				if (float.IsInfinity(num) || float.IsNaN(num))
				{
					num = 0f;
				}
				m_thrustOverride.Value = MathHelper.Clamp(num, 0f, 100f);
			}
		}

		public float ThrustOverrideOverForceLen => (float)m_thrustOverride * 0.01f;

		public Vector3I ThrustForwardVector => Base6Directions.GetIntVector(base.Orientation.Forward);

		public bool IsPowered
		{
			get
			{
				if (m_thrustComponent != null)
				{
					return m_thrustComponent.IsThrustPoweredByType(this, ref FuelDefinition.Id);
				}
				return false;
			}
		}

		public float MaxPowerConsumption => BlockDefinition.MaxPowerConsumption * m_powerConsumptionMultiplier;

		public float MinPowerConsumption => BlockDefinition.MinPowerConsumption * m_powerConsumptionMultiplier;

		public float CurrentStrength
		{
			get
			{
				return m_currentStrength;
			}
			set
			{
				if (m_currentStrength != value)
				{
					m_currentStrength = value;
					InvokeRenderUpdate();
				}
			}
		}

		public ListReader<MyThrustFlameAnimator.FlameInfo> Flames => m_flames;

		public MyStringId FlameLengthMaterial => m_flameLengthMaterialId;

		public MyStringId FlamePointMaterial => m_flamePointMaterialId;

		public float FlameDamageLengthScale => BlockDefinition.FlameDamageLengthScale;

		public Vector3I GridThrustDirection
		{
			get
			{
				MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
				if (myShipController == null)
				{
					myShipController = base.CubeGrid.GridSystems.ControlSystem.GetShipController();
				}
				if (myShipController != null)
				{
					myShipController.Orientation.GetQuaternion(out Quaternion result);
					return Vector3I.Transform(ThrustForwardVector, Quaternion.Inverse(result));
				}
				return Vector3I.Zero;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyThrust.ThrustOverride
		{
			get
			{
				return ThrustOverride;
			}
			set
			{
				ThrustOverride = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyThrust.ThrustOverridePercentage
		{
			get
			{
				return (float)m_thrustOverride / 100f;
			}
			set
			{
				m_thrustOverride.Value = MathHelper.Clamp(value, 0f, 1f) * 100f;
			}
		}

		float Sandbox.ModAPI.IMyThrust.ThrustMultiplier
		{
			get
			{
				return m_thrustMultiplier;
			}
			set
			{
				m_thrustMultiplier = value;
				if (m_thrustMultiplier < 0.01f)
				{
					m_thrustMultiplier = 0.01f;
				}
				if (m_thrustComponent != null)
				{
					m_thrustComponent.MarkDirty();
				}
			}
		}

		float Sandbox.ModAPI.IMyThrust.PowerConsumptionMultiplier
		{
			get
			{
				return m_powerConsumptionMultiplier;
			}
			set
			{
				m_powerConsumptionMultiplier = value;
				if (m_powerConsumptionMultiplier < 0.01f)
				{
					m_powerConsumptionMultiplier = 0.01f;
				}
				if (m_thrustComponent != null)
				{
					m_thrustComponent.MarkDirty();
				}
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			}
		}

		float Sandbox.ModAPI.Ingame.IMyThrust.MaxThrust => BlockDefinition.ForceMagnitude * m_thrustMultiplier;

		float Sandbox.ModAPI.Ingame.IMyThrust.MaxEffectiveThrust
		{
			get
			{
				if (m_thrustComponent == null)
				{
					return 0f;
				}
				return BlockDefinition.ForceMagnitude * m_thrustMultiplier * m_thrustComponent.GetLastThrustMultiplier(this);
			}
		}

		float Sandbox.ModAPI.Ingame.IMyThrust.CurrentThrust => CurrentStrength * BlockDefinition.ForceMagnitude * m_thrustMultiplier;

		Vector3I Sandbox.ModAPI.Ingame.IMyThrust.GridThrustDirection => GridThrustDirection;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public event Action<MyThrust, float> ThrustOverrideChanged;

		event Action<Sandbox.ModAPI.IMyThrust, float> Sandbox.ModAPI.IMyThrust.ThrustOverrideChanged
		{
			add
			{
				ThrustOverrideChanged += GetDelegate(value);
			}
			remove
			{
				ThrustOverrideChanged -= GetDelegate(value);
			}
		}

		protected override bool CheckIsWorking()
		{
			if (IsPowered)
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public static void RandomizeFlameProperties(float strength, float flameScale, ref float thrustRadiusRand, ref float thrustLengthRand)
		{
			thrustRadiusRand = MyUtils.GetRandomFloat(0.9f, 1.1f);
		}

		public MyThrust()
		{
			CreateTerminalControls();
			Render = new MyRenderComponentThrust();
			AddDebugRenderComponent(new MyDebugRenderComponentThrust(this));
			m_thrustOverride.ValueChanged += delegate
			{
				ThrustOverrideValueChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyThrust>())
			{
				base.CreateTerminalControls();
				float threshold = 1f;
				MyTerminalControlSlider<MyThrust> myTerminalControlSlider = new MyTerminalControlSlider<MyThrust>("Override", MySpaceTexts.BlockPropertyTitle_ThrustOverride, MySpaceTexts.BlockPropertyDescription_ThrustOverride);
				myTerminalControlSlider.Getter = ((MyThrust x) => (float)x.m_thrustOverride * x.BlockDefinition.ForceMagnitude * 0.01f);
				myTerminalControlSlider.Setter = delegate(MyThrust x, float v)
				{
					x.m_thrustOverride.Value = ((v <= threshold) ? 0f : (v / x.BlockDefinition.ForceMagnitude * 100f));
					x.RaisePropertiesChanged();
				};
				myTerminalControlSlider.DefaultValue = 0f;
				myTerminalControlSlider.SetLimits((MyThrust x) => 0f, (MyThrust x) => x.BlockDefinition.ForceMagnitude);
				myTerminalControlSlider.EnableActions();
				myTerminalControlSlider.Writer = delegate(MyThrust x, StringBuilder result)
				{
					if (x.ThrustOverride < 1f)
					{
						result.Append((object)MyTexts.Get(MyCommonTexts.Disabled));
					}
					else
					{
						MyValueFormatter.AppendForceInBestUnit((x.m_thrustComponent != null) ? (x.ThrustOverride * x.m_thrustComponent.GetLastThrustMultiplier(x)) : 0f, result);
					}
				};
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

		private void ThrustOverrideValueChanged()
		{
			this.ThrustOverrideChanged.InvokeIfNotNull(this, ThrustOverride);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Thrust obj = (MyObjectBuilder_Thrust)base.GetObjectBuilderCubeBlock(copy);
			obj.ThrustOverride = ThrustOverride;
			return obj;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			if (!cubeGrid.Components.TryGet(out MyEntityThrustComponent component))
			{
				component = new MyThrusterBlockThrustComponent();
				component.Init();
				cubeGrid.Components.Add(component);
			}
			m_thrustComponent = component;
			BlockDefinition = (MyThrustDefinition)base.BlockDefinition;
			MyDefinitionId defId = default(MyDefinitionId);
			if (!BlockDefinition.FuelConverter.FuelId.IsNull())
			{
				defId = BlockDefinition.FuelConverter.FuelId;
			}
			m_flameLengthMaterialId = MyStringId.GetOrCompute(BlockDefinition.FlameLengthMaterial);
			m_flamePointMaterialId = MyStringId.GetOrCompute(BlockDefinition.FlamePointMaterial);
			MyGasProperties definition = null;
			if (MyFakes.ENABLE_HYDROGEN_FUEL)
			{
				MyDefinitionManager.Static.TryGetDefinition(defId, out definition);
			}
			FuelDefinition = (definition ?? new MyGasProperties
			{
				Id = MyResourceDistributorComponent.ElectricityId,
				EnergyDensity = 1f
			});
			base.Init(objectBuilder, cubeGrid);
			base.NeedsWorldMatrix = false;
			base.InvalidateOnMove = false;
			MyObjectBuilder_Thrust myObjectBuilder_Thrust = (MyObjectBuilder_Thrust)objectBuilder;
			m_thrustOverride.SetLocalValue(MathHelper.Clamp(myObjectBuilder_Thrust.ThrustOverride, 0f, BlockDefinition.ForceMagnitude) * 100f / BlockDefinition.ForceMagnitude);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), BlockDefinition.FlameFlare);
			MyFlareDefinition myFlareDefinition = MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition;
			Flares = (myFlareDefinition ?? new MyFlareDefinition());
			m_maxBillboardDistanceSquared = BlockDefinition.FlameVisibilityDistance * BlockDefinition.FlameVisibilityDistance;
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			FuelConverterDefinition = ((!MyFakes.ENABLE_HYDROGEN_FUEL) ? new MyFuelConverterInfo
			{
				Efficiency = 1f
			} : BlockDefinition.FuelConverter);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += CubeBlock_OnWorkingChanged;
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			MyEntitySubpart myEntitySubpart = base.InstantiateSubpart(subpartDummy, ref data);
			myEntitySubpart.NeedsWorldMatrix = false;
			myEntitySubpart.Render = new MyRenderComponentThrust.MyPropellerRenderComponent();
			return myEntitySubpart;
		}

		private bool LoadPropeller()
		{
			if (BlockDefinition.PropellerUse && BlockDefinition.PropellerEntity != null && base.Subparts.TryGetValue(BlockDefinition.PropellerEntity, out MyEntitySubpart value))
			{
				m_propellerEntity = value;
				m_propellerMaxDistance = BlockDefinition.PropellerMaxDistance * BlockDefinition.PropellerMaxDistance;
				return true;
			}
			return false;
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			InvokeRenderUpdate();
		}

		private void InvokeRenderUpdate()
		{
			if (!m_renderNeedsUpdate && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_renderNeedsUpdate = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
		}

		private void RenderUpdate()
		{
			MyRenderComponentThrust render = Render;
			MyThrustDefinition blockDefinition = BlockDefinition;
			float currentStrength = m_currentStrength;
			render.UpdateFlameProperties(m_flamesCalculate, currentStrength);
			if (m_propellerActive)
			{
				float num = 0f;
				if (m_propellerCalculate)
				{
					num = ((currentStrength > 0f) ? blockDefinition.PropellerFullSpeed : blockDefinition.PropellerIdleSpeed);
				}
				render.UpdatePropellerSpeed(num * (MathF.PI * 2f));
			}
			m_renderNeedsUpdate = false;
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			if (!base.CubeGrid.Components.TryGet(out MyEntityThrustComponent component))
			{
				component = new MyThrusterBlockThrustComponent();
				component.Init();
				base.CubeGrid.Components.Add(component);
			}
			m_thrustComponent = component;
			if (base.IsFunctional)
			{
				m_thrustComponent.Register(this, ThrustForwardVector, OnRegisteredToThrustComponent);
			}
			m_thrustComponent.DampenersEnabled = base.CubeGrid.DampenersEnabled;
		}

		public void ClearThrustComponent()
		{
			m_thrustComponent = null;
		}

		private bool OnRegisteredToThrustComponent()
		{
			MyResourceSinkComponent myResourceSinkComponent = m_thrustComponent.ResourceSink(this);
			myResourceSinkComponent.IsPoweredChanged += Sink_IsPoweredChanged;
			myResourceSinkComponent.Update();
			return true;
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			if (!base.CubeGrid.MarkedForClose && m_thrustComponent != null)
			{
				m_thrustComponent.ResourceSink(this).IsPoweredChanged -= Sink_IsPoweredChanged;
				if (m_thrustComponent.IsRegistered(this, ThrustForwardVector))
				{
					m_thrustComponent.Unregister(this, ThrustForwardVector);
				}
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			LoadDummies();
		}

		public void Sink_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		public void CubeBlock_OnWorkingChanged(MyCubeBlock block)
		{
			if (m_landingEffect != null)
			{
				m_landingEffect.Stop(instant: false);
				m_landingEffect = null;
				m_landingEffectCount--;
			}
			bool flag = false;
			if (base.IsWorking)
			{
				flag = UpdateRenderDistance();
			}
			else
			{
				flag = (m_flamesCalculate || m_flamesCalculate);
				m_flamesCalculate = false;
				m_propellerCalculate = false;
			}
			if (flag)
			{
				InvokeRenderUpdate();
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			if (base.IsFunctional && (m_thrustComponent == null || !m_thrustComponent.IsRegistered(this, ThrustForwardVector)))
			{
				OnRegisteredToGridSystems();
			}
			else if (!base.IsFunctional && m_thrustComponent != null && m_thrustComponent.IsRegistered(this, ThrustForwardVector))
			{
				OnUnregisteredFromGridSystems();
			}
			if (base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				base.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
			}
		}

		private void LoadDummies()
		{
			MyModel model = base.Model;
			m_flames = m_flameCache.GetOrAdd(model.AssetName, model, delegate(MyModel m, string _)
			{
				List<MyThrustFlameAnimator.FlameInfo> list = new List<MyThrustFlameAnimator.FlameInfo>();
				foreach (KeyValuePair<string, MyModelDummy> item in m.Dummies.OrderBy((KeyValuePair<string, MyModelDummy> s) => s.Key))
				{
					if (item.Key.StartsWith("thruster_flame", StringComparison.InvariantCultureIgnoreCase))
					{
						list.Add(new MyThrustFlameAnimator.FlameInfo
						{
							Position = item.Value.Matrix.Translation,
							Direction = Vector3.Normalize(item.Value.Matrix.Forward),
							Radius = Math.Max(item.Value.Matrix.Scale.X, item.Value.Matrix.Scale.Y) * 0.5f
						});
					}
				}
				return list;
			});
			if (BlockDefinition != null)
			{
				m_propellerActive = LoadPropeller();
			}
			Render.UpdateFlameAnimatorData();
		}

		protected override void Closing()
		{
			if (m_landingEffect != null)
			{
				m_landingEffect.Stop(instant: false);
				m_landingEffect = null;
				m_landingEffectCount--;
			}
			base.Closing();
		}

		public override void GetTerminalName(StringBuilder result)
		{
			string directionString = GetDirectionString();
			if (directionString == null)
			{
				base.GetTerminalName(result);
			}
			else
			{
				result.Append(DisplayNameText).Append(" (").Append(directionString)
					.Append(") ");
			}
		}

		public override void UpdateAfterSimulation10()
		{
			if (m_renderNeedsUpdate)
			{
				RenderUpdate();
			}
			base.UpdateAfterSimulation10();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			ThrustParticlesPositionUpdate();
		}

		public override void UpdateBeforeSimulation100()
		{
			ThrustDamageAsync();
			base.UpdateBeforeSimulation100();
			ThrustParticles();
		}

		private void ThrustParticles()
		{
			if (base.IsWorking)
			{
				GetLocalMatrix(out Matrix localMatrix);
				Vector3 translation = localMatrix.Translation;
				_ = base.CubeGrid.GridScale;
				foreach (MyThrustFlameAnimator.FlameInfo flame in Flames)
				{
					Vector3D normal = Vector3D.TransformNormal(flame.Direction, (MatrixD)localMatrix);
					Vector3D position = Vector3D.TransformNormal(flame.Position, (MatrixD)localMatrix) + translation;
					position = Vector3D.Transform(position, base.CubeGrid.WorldMatrix);
					normal = Vector3D.TransformNormal(normal, base.CubeGrid.WorldMatrix);
					m_lastHitInfo = ((ThrustLengthRand <= 1E-05f) ? null : MyPhysics.CastRay(position, position + normal * ThrustLengthRand * 2.5 * flame.Radius, 15));
					MyEntity myEntity = m_lastHitInfo.HasValue ? (m_lastHitInfo.Value.HkHitInfo.GetHitEntity() as MyEntity) : null;
					bool flag = false;
					string effectName = "Landing_Jet_Ground";
					if (myEntity != null)
					{
						if (myEntity is MyVoxelPhysics || myEntity is MyVoxelMap)
						{
							flag = true;
							effectName = "Landing_Jet_Ground";
							MyVoxelBase myVoxelBase = null;
							if (myEntity is MyVoxelPhysics)
							{
								myVoxelBase = (myEntity as MyVoxelPhysics).RootVoxel;
								effectName = "Landing_Jet_Ground";
							}
							else
							{
								myVoxelBase = (myEntity as MyVoxelMap);
								effectName = "Landing_Jet_Ground_Dust";
							}
							Vector3D worldPosition = m_lastHitInfo.Value.Position;
							MyVoxelMaterialDefinition materialAt = myVoxelBase.GetMaterialAt(ref worldPosition);
							if (materialAt != null && !string.IsNullOrEmpty(materialAt.LandingEffect))
							{
								effectName = materialAt.LandingEffect;
							}
						}
						else if (myEntity.GetTopMostParent() is MyCubeGrid && myEntity.GetTopMostParent() != GetTopMostParent())
						{
							flag = true;
							effectName = ((base.CubeGrid.GridSizeEnum != 0) ? "Landing_Jet_Grid_Small" : "Landing_Jet_Grid_Large");
						}
					}
					if (!flag)
					{
						if (m_landingEffect != null)
						{
							m_landingEffect.Stop(instant: false);
							m_landingEffect = null;
							m_landingEffectCount--;
							base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
						}
					}
					else if (m_landingEffect == null)
					{
						if (m_landingEffectCount < m_maxNumberLandingEffects && MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateFromTransformScale(Quaternion.CreateFromForwardUp(-m_lastHitInfo.Value.HkHitInfo.Normal, Vector3.CalculatePerpendicularVector(m_lastHitInfo.Value.HkHitInfo.Normal)), m_lastHitInfo.Value.Position, Vector3D.One), out m_landingEffect))
						{
							m_landingEffectCount++;
							m_landingEffect.UserScale = base.CubeGrid.GridSize;
							base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
						}
					}
					else if (m_lastHitInfo.HasValue)
					{
						m_particleLocalOffset = Vector3D.Transform(m_lastHitInfo.Value.Position, base.PositionComp.WorldMatrixInvScaled);
					}
				}
			}
		}

		private void ThrustParticlesPositionUpdate()
		{
			if (m_landingEffect != null)
			{
				Vector3D translation = Vector3D.Transform(m_particleLocalOffset, base.WorldMatrix);
				m_landingEffect.SetTranslation(translation);
			}
		}

		private void ThrustDamageAsync()
		{
			if (m_flames.Count > 0 && MySession.Static.ThrusterDamage && base.IsWorking && base.CubeGrid.InScene && base.CubeGrid.Physics != null && base.CubeGrid.Physics.Enabled)
			{
				if (!MySandboxGame.IsPaused)
				{
					ThrustLengthRand = CurrentStrength * 10f * MyUtils.GetRandomFloat(0.6f, 1f) * BlockDefinition.FlameLengthScale;
				}
				if (Sync.IsServer && (CurrentStrength != 0f || MyFakes.INACTIVE_THRUSTER_DMG) && MyFakes.INACTIVE_THRUSTER_DMG)
				{
					foreach (MyThrustFlameAnimator.FlameInfo flame in m_flames)
					{
						MatrixD matrixWorld = base.WorldMatrix;
						LineD damageCapsuleLine = GetDamageCapsuleLine(flame, ref matrixWorld);
						ThrustDamageShapeCast(damageCapsuleLine, flame, m_flameCollisionsList);
						ThrustDamageDealDamage(flame, m_flameCollisionsList);
					}
				}
			}
		}

		private void ThrustDamageShapeCast(LineD damageLine, MyThrustFlameAnimator.FlameInfo flameInfo, List<HkBodyCollision> outFlameCollisionsList)
		{
			HkShape shape = (damageLine.Length == 0.0) ? ((HkShape)new HkSphereShape(flameInfo.Radius * BlockDefinition.FlameDamageLengthScale)) : ((HkShape)new HkCapsuleShape(Vector3.Zero, damageLine.To - damageLine.From, flameInfo.Radius * BlockDefinition.FlameDamageLengthScale));
			MyPhysics.GetPenetrationsShape(shape, ref damageLine.From, ref Quaternion.Identity, outFlameCollisionsList, 15);
			shape.RemoveReference();
		}

		private void ThrustDamageDealDamage(MyThrustFlameAnimator.FlameInfo flameInfo, List<HkBodyCollision> flameCollisionsList)
		{
			using (MyUtils.ReuseCollection(ref m_blockSet))
			{
				using (MyUtils.ReuseCollection(ref m_alreadyDamagedEntities))
				{
					foreach (HkBodyCollision flameCollisions in flameCollisionsList)
					{
						MyCubeGrid myCubeGrid = flameCollisions.GetCollisionEntity() as MyCubeGrid;
						if (myCubeGrid != null)
						{
							m_blockSet.Add(myCubeGrid.Physics.RigidBody.GetShape().GetContainer().GetShape(flameCollisions.ShapeKey));
						}
					}
					foreach (HkBodyCollision flameCollisions2 in flameCollisionsList)
					{
						VRage.ModAPI.IMyEntity myEntity = flameCollisions2.GetCollisionEntity();
						if (myEntity != null && !myEntity.Equals(this))
						{
							if (!(myEntity is MyCharacter))
							{
								myEntity = myEntity.GetTopMostParent();
							}
							if (!m_alreadyDamagedEntities.Contains(myEntity))
							{
								m_alreadyDamagedEntities.Add(myEntity);
								if (myEntity is IMyDestroyableObject)
								{
									(myEntity as IMyDestroyableObject).DoDamage(flameInfo.Radius * BlockDefinition.FlameDamage * 100f, MyDamageType.Environment, sync: true, null, base.EntityId);
								}
								else if (myEntity is MyCubeGrid)
								{
									MyCubeGrid myCubeGrid2 = myEntity as MyCubeGrid;
									if (myCubeGrid2.BlocksDestructionEnabled)
									{
										MatrixD matrixWorld = base.WorldMatrix;
										LineD damageCapsuleLine = GetDamageCapsuleLine(flameInfo, ref matrixWorld);
										DamageGrid(flameInfo, damageCapsuleLine, myCubeGrid2, m_blockSet);
									}
								}
							}
						}
					}
				}
			}
			flameCollisionsList.Clear();
		}

		private void DamageGrid(MyThrustFlameAnimator.FlameInfo flameInfo, LineD l, MyCubeGrid grid, HashSet<HkShape> shapes)
		{
			float num = flameInfo.Radius * BlockDefinition.FlameDamageLengthScale;
			Vector3 vector = new Vector3(num, num, l.Length * 0.5);
			MatrixD worldMatrix = base.WorldMatrix;
			worldMatrix.Translation = (l.To - l.From) * 0.5 + l.From;
			BoundingBoxD box = new BoundingBoxD(-vector, vector);
			new MyOrientedBoundingBoxD(box, worldMatrix);
			List<MySlimBlock> list = new List<MySlimBlock>();
			grid.GetBlocksIntersectingOBB(box, worldMatrix, list);
			foreach (MySlimBlock item in list)
			{
				if (item != SlimBlock && item != null && (base.CubeGrid.GridSizeEnum == MyCubeSize.Large || (double)item.BlockDefinition.DeformationRatio > 0.25))
				{
					List<HkShape> shapesFromPosition = item.CubeGrid.GetShapesFromPosition(item.Min);
					if (shapesFromPosition != null)
					{
						foreach (HkShape item2 in shapesFromPosition)
						{
							if (shapes.Contains(item2))
							{
								item.DoDamage(100f * BlockDefinition.FlameDamage, MyDamageType.Environment, sync: true, null, base.EntityId);
							}
						}
					}
				}
			}
		}

		public LineD GetDamageCapsuleLine(MyThrustFlameAnimator.FlameInfo info, ref MatrixD matrixWorld)
		{
			Vector3D vector3D = Vector3D.Transform(info.Position, matrixWorld);
			Vector3D vector3D2 = Vector3.TransformNormal(info.Direction, matrixWorld);
			float num = ThrustLengthRand * info.Radius * 0.5f;
			num *= BlockDefinition.FlameDamageLengthScale;
			if (num > info.Radius)
			{
				return new LineD(vector3D, vector3D + vector3D2 * (2f * num - info.Radius), 2f * num - info.Radius);
			}
			LineD result = new LineD(vector3D + vector3D2 * num, vector3D + vector3D2 * num, 0.0);
			result.Direction = vector3D2;
			return result;
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			UpdateIsWorking();
			if (base.IsWorking)
			{
				UpdateSoundState();
				if (UpdateRenderDistance())
				{
					RenderUpdate();
				}
			}
		}

		private bool UpdateRenderDistance()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return false;
			}
			bool result = false;
			double num = Vector3D.DistanceSquared(MySector.MainCamera.Position, base.PositionComp.GetPosition());
			bool flag = num < (double)m_maxBillboardDistanceSquared;
			if (flag != m_flamesCalculate)
			{
				result = true;
				m_flamesCalculate = flag;
			}
			if (m_propellerActive)
			{
				bool flag2 = num < (double)m_propellerMaxDistance;
				if (flag2 != m_propellerCalculate)
				{
					result = true;
					m_propellerCalculate = flag2;
				}
			}
			return result;
		}

		private void UpdateSoundState()
		{
			if (m_soundEmitter == null)
			{
				return;
			}
			if (CurrentStrength > 0.1f)
			{
				if (!m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.PlaySound(BlockDefinition.PrimarySound, stopPrevious: true);
				}
			}
			else
			{
				m_soundEmitter.StopSound(forced: false);
			}
			if (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying)
			{
				float semitones = 8f * (CurrentStrength - 0.5f * MyConstants.MAX_THRUST) / MyConstants.MAX_THRUST;
				m_soundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(semitones);
			}
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.AppendFormat("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			if (FuelDefinition.Id.SubtypeName == "Electricity")
			{
				MyValueFormatter.AppendWorkInBestUnit(MaxPowerConsumption, detailedInfo);
			}
			else
			{
				MyValueFormatter.AppendVolumeInBestUnit(MaxPowerConsumption, detailedInfo);
			}
			detailedInfo.AppendFormat("\n");
		}

		private string GetDirectionString()
		{
			Vector3I gridThrustDirection = GridThrustDirection;
			if (gridThrustDirection != Vector3I.Zero)
			{
				if (gridThrustDirection.X == 1)
				{
					return MyTexts.GetString(MyCommonTexts.Thrust_Left);
				}
				if (gridThrustDirection.X == -1)
				{
					return MyTexts.GetString(MyCommonTexts.Thrust_Right);
				}
				if (gridThrustDirection.Y == 1)
				{
					return MyTexts.GetString(MyCommonTexts.Thrust_Down);
				}
				if (gridThrustDirection.Y == -1)
				{
					return MyTexts.GetString(MyCommonTexts.Thrust_Up);
				}
				if (gridThrustDirection.Z == 1)
				{
					return MyTexts.GetString(MyCommonTexts.Thrust_Forward);
				}
				if (gridThrustDirection.Z == -1)
				{
					return MyTexts.GetString(MyCommonTexts.Thrust_Back);
				}
			}
			return null;
		}

		public static Action<MyThrust, float> GetDelegate(Action<Sandbox.ModAPI.IMyThrust, float> value)
		{
			return (Action<MyThrust, float>)Delegate.CreateDelegate(typeof(Action<MyThrust, float>), value.Target, value.Method);
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
