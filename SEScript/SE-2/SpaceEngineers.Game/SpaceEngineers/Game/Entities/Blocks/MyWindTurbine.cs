using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using SpaceEngineers.Game.EntityComponents.Renders;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_WindTurbine))]
	public class MyWindTurbine : MyEnvironmentalPowerProducer
	{
		public class TurbineSubpart : MyEntitySubpart
		{
			public new MyWindTurbine Parent => (MyWindTurbine)base.Parent;

			public new MyRenderComponentWindTurbine.TurbineRenderComponent Render => (MyRenderComponentWindTurbine.TurbineRenderComponent)base.Render;

			public override void InitComponents()
			{
				base.Render = new MyRenderComponentWindTurbine.TurbineRenderComponent();
				base.InitComponents();
			}
		}

		private int m_nextUpdateRay;

		private float m_effectivity;

		private bool m_paralleRaycastRunning;

		private readonly Action<MyPhysics.HitInfo?> m_onRaycastCompleted;

		private readonly Action<List<MyPhysics.HitInfo>> m_onRaycastCompletedList;

		private List<MyPhysics.HitInfo> m_cachedHitList = new List<MyPhysics.HitInfo>();

		private Action m_updateEffectivity;

		protected float Effectivity
		{
			get
			{
				return m_effectivity;
			}
			set
			{
				if (m_effectivity != value)
				{
					m_effectivity = value;
					OnProductionChanged();
					UpdateVisuals();
				}
			}
		}

		protected override float CurrentProductionRatio
		{
			get
			{
				if (!base.Enabled || !base.IsWorking)
				{
					return 0f;
				}
				return m_effectivity * Math.Min(1f, GetOrCreateSharedComponent().WindSpeed / BlockDefinition.OptimalWindSpeed);
			}
		}

		public new MyWindTurbineDefinition BlockDefinition => (MyWindTurbineDefinition)base.BlockDefinition;

		public float[] RayEffectivities
		{
			get;
			private set;
		}

		public MyWindTurbine()
		{
			m_updateEffectivity = UpdateEffectivity;
			m_onRaycastCompleted = OnRaycastCompleted;
			m_onRaycastCompletedList = OnRaycastCompleted;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyObjectBuilder_WindTurbine myObjectBuilder_WindTurbine = (MyObjectBuilder_WindTurbine)objectBuilder;
			RayEffectivities = myObjectBuilder_WindTurbine.ImmediateEffectivities;
			if (RayEffectivities == null)
			{
				RayEffectivities = new float[BlockDefinition.RaycastersCount];
			}
			base.Init(objectBuilder, cubeGrid);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override void InitComponents()
		{
			base.Render = new MyRenderComponentWindTurbine();
			base.InitComponents();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_WindTurbine obj = (MyObjectBuilder_WindTurbine)base.GetObjectBuilderCubeBlock(copy);
			obj.ImmediateEffectivities = (float[])RayEffectivities.Clone();
			return obj;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			GetOrCreateSharedComponent().UpdateWindSpeed();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			GetOrCreateSharedComponent().Update10();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			GetOrCreateSharedComponent().UpdateWindSpeed();
		}

		public void UpdateNextRay()
		{
			if (!m_paralleRaycastRunning)
			{
				m_paralleRaycastRunning = true;
				GetRaycaster(m_nextUpdateRay, out Vector3D start, out Vector3D end);
				if (m_nextUpdateRay != 0)
				{
					MyPhysics.CastRayParallel(ref start, ref end, 0, m_onRaycastCompleted);
					return;
				}
				m_cachedHitList.AssertEmpty();
				MyPhysics.CastRayParallel(ref start, ref end, m_cachedHitList, 28, m_onRaycastCompletedList);
			}
		}

		private void OnRaycastCompleted(List<MyPhysics.HitInfo> hitList)
		{
			using (hitList.GetClearToken())
			{
				foreach (MyPhysics.HitInfo hit in hitList)
				{
					if (hit.HkHitInfo.Body.Layer == 28)
					{
						OnRaycastCompleted(hit);
						return;
					}
				}
				OnRaycastCompleted((MyPhysics.HitInfo?)null);
			}
		}

		private void OnRaycastCompleted(MyPhysics.HitInfo? hitInfo)
		{
			float num = 1f;
			if (hitInfo.HasValue)
			{
				float hitFraction = hitInfo.Value.HkHitInfo.HitFraction;
				float minRaycasterClearance = BlockDefinition.MinRaycasterClearance;
				num = ((!(hitFraction <= minRaycasterClearance)) ? ((hitFraction - minRaycasterClearance) / (1f - minRaycasterClearance)) : 0f);
			}
			RayEffectivities[m_nextUpdateRay] = num;
			m_nextUpdateRay++;
			if (m_nextUpdateRay >= BlockDefinition.RaycastersCount)
			{
				m_nextUpdateRay = 0;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.MarkedForClose)
				{
					UpdateEffectivity();
					m_paralleRaycastRunning = false;
				}
			}, "Turbine update");
		}

		private void UpdateEffectivity()
		{
			if (!base.IsWorking)
			{
				Effectivity = 0f;
				return;
			}
			float num = 0f;
			for (int i = 1; i < RayEffectivities.Length; i++)
			{
				num += RayEffectivities[i];
			}
			num /= BlockDefinition.RaycastersToFullEfficiency;
			num *= MathHelper.Lerp(0.5f, 1f, RayEffectivities[0]);
			Effectivity = Math.Min(1f, num);
		}

		public void GetRaycaster(int id, out Vector3D start, out Vector3D end)
		{
			MatrixD worldMatrix = base.WorldMatrix;
			start = worldMatrix.Translation;
			if (id == 0)
			{
				end = start + GetOrCreateSharedComponent().GravityNormal * BlockDefinition.OptimalGroundClearance;
				return;
			}
			int num = RayEffectivities.Length - 1;
			float angle = MathF.PI * 2f / (float)num * (float)(id - 1);
			int raycasterSize = BlockDefinition.RaycasterSize;
			end = start + raycasterSize * (MyMath.FastSin(angle) * worldMatrix.Left + MyMath.FastCos(angle) * worldMatrix.Forward);
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			GetOrCreateSharedComponent().Register(this);
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			GetOrCreateSharedComponent().Unregister(this);
		}

		private MySharedWindComponent GetOrCreateSharedComponent()
		{
			MyEntityComponentContainer components = base.CubeGrid.Components;
			MySharedWindComponent mySharedWindComponent = components.Get<MySharedWindComponent>();
			if (mySharedWindComponent == null)
			{
				mySharedWindComponent = new MySharedWindComponent();
				components.Add(mySharedWindComponent);
			}
			return mySharedWindComponent;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.IsWorking)
			{
				OnStartWorking();
			}
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			OnIsWorkingChanged();
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			OnIsWorkingChanged();
		}

		private void OnIsWorkingChanged()
		{
			float effectivity = Effectivity;
			UpdateEffectivity();
			if (Effectivity == effectivity)
			{
				UpdateVisuals();
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			if (!base.Enabled)
			{
				UpdateVisuals();
			}
		}

		public override void RefreshModels(string modelPath, string modelCollisionPath)
		{
			base.RefreshModels(modelPath, modelCollisionPath);
			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			if (!MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, GetEmissiveState(), out MyEmissiveColorStateResult result))
			{
				result.EmissiveColor = Color.Green;
			}
			float speed = CurrentProductionRatio * BlockDefinition.TurbineRotationSpeed;
			foreach (TurbineSubpart value in base.Subparts.Values)
			{
				value.Render.SetSpeed(speed);
				value.Render.SetColor(result.EmissiveColor);
			}
		}

		private MyStringHash GetEmissiveState()
		{
			CheckIsWorking();
			if (base.IsWorking)
			{
				if (GetOrCreateSharedComponent().IsEnabled && Effectivity > 0f)
				{
					return MyCubeBlock.m_emissiveNames.Working;
				}
				return MyCubeBlock.m_emissiveNames.Warning;
			}
			if (base.IsFunctional)
			{
				return MyCubeBlock.m_emissiveNames.Disabled;
			}
			return MyCubeBlock.m_emissiveNames.Damaged;
		}

		public void OnEnvironmentChanged()
		{
			UpdateVisuals();
			OnProductionChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder sb)
		{
			base.UpdateDetailedInfo(sb);
			MyTexts.AppendFormat(arg0: ((double)Effectivity > 0.95) ? MySpaceTexts.Turbine_WindClearanceOptimal : ((Effectivity > 0.6f) ? MySpaceTexts.Turbine_WindClearanceGood : ((!(Effectivity > 0f)) ? MySpaceTexts.Turbine_WindClearanceNone : MySpaceTexts.Turbine_WindClearancePoor)), stringBuilder: sb, textEnum: MySpaceTexts.Turbine_WindClearance);
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			return new TurbineSubpart();
		}
	}
}
