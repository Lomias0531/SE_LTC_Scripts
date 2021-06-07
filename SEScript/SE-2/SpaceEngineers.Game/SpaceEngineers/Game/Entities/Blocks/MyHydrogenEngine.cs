using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using SpaceEngineers.Game.EntityComponents.Renders;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_HydrogenEngine))]
	public class MyHydrogenEngine : MyGasFueledPowerProducer
	{
		private class MyRotatingSubpartSubpart : MyEntitySubpart
		{
			public new MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent Render => (MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent)base.Render;

			public override void InitComponents()
			{
				base.Render = new MyRenderComponentHydrogenEngine.MyRotatingSubpartRenderComponent();
				base.InitComponents();
			}
		}

		private class MyPistonSubpart : MyEntitySubpart
		{
			public new MyRenderComponentHydrogenEngine.MyPistonRenderComponent Render => (MyRenderComponentHydrogenEngine.MyPistonRenderComponent)base.Render;

			public override void InitComponents()
			{
				base.Render = new MyRenderComponentHydrogenEngine.MyPistonRenderComponent();
				base.InitComponents();
			}
		}

		private bool m_renderAnimationEnabled = true;

		private List<MyPistonSubpart> m_pistons = new List<MyPistonSubpart>();

		private List<MyRotatingSubpartSubpart> m_rotatingSubparts = new List<MyRotatingSubpartSubpart>();

		public new MyHydrogenEngineDefinition BlockDefinition => (MyHydrogenEngineDefinition)base.BlockDefinition;

		public new MyRenderComponentHydrogenEngine Render => (MyRenderComponentHydrogenEngine)base.Render;

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			UpdateVisuals();
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			UpdateVisuals();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (!Sync.IsDedicated)
			{
				bool flag = Vector3D.DistanceSquared(MySector.MainCamera.Position, base.PositionComp.GetPosition()) < (double)BlockDefinition.AnimationVisibilityDistanceSq;
				if (flag != m_renderAnimationEnabled)
				{
					m_renderAnimationEnabled = flag;
					UpdateVisuals();
				}
			}
		}

		private void UpdateVisuals()
		{
			float speed = 0f;
			if (m_renderAnimationEnabled && base.IsWorking)
			{
				speed = BlockDefinition.AnimationSpeed;
			}
			foreach (MyPistonSubpart piston in m_pistons)
			{
				piston.Render.SetSpeed(speed);
			}
			foreach (MyRotatingSubpartSubpart rotatingSubpart in m_rotatingSubparts)
			{
				rotatingSubpart.Render.SetSpeed(speed);
			}
		}

		protected override string GetDefaultEmissiveParts(byte index)
		{
			if (index != 0)
			{
				return null;
			}
			return "Emissive2";
		}

		public override void InitComponents()
		{
			base.Render = new MyRenderComponentHydrogenEngine();
			base.InitComponents();
		}

		public override void RefreshModels(string modelPath, string modelCollisionPath)
		{
			m_pistons.Clear();
			m_rotatingSubparts.Clear();
			base.RefreshModels(modelPath, modelCollisionPath);
			UpdateVisuals();
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			string name = data.Name;
			if (name.Contains("Piston"))
			{
				MyPistonSubpart myPistonSubpart = new MyPistonSubpart();
				float animationOffset = 0f;
				float[] pistonAnimationOffsets = BlockDefinition.PistonAnimationOffsets;
				if (pistonAnimationOffsets != null && pistonAnimationOffsets.Length != 0)
				{
					animationOffset = pistonAnimationOffsets[m_pistons.Count % pistonAnimationOffsets.Length];
				}
				myPistonSubpart.Render.AnimationOffset = animationOffset;
				m_pistons.Add(myPistonSubpart);
				return myPistonSubpart;
			}
			if (name.Contains("Propeller") || name.Contains("Camshaft"))
			{
				MyRotatingSubpartSubpart myRotatingSubpartSubpart = new MyRotatingSubpartSubpart();
				m_rotatingSubparts.Add(myRotatingSubpartSubpart);
				return myRotatingSubpartSubpart;
			}
			return base.InstantiateSubpart(subpartDummy, ref data);
		}
	}
}
