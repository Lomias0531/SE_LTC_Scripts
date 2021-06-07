using ParallelTasks;
using Sandbox;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace SpaceEngineers.Game.EntityComponents.GameLogic
{
	public class MySolarGameLogicComponent : MyGameLogicComponent
	{
		private const int NUMBER_OF_PIVOTS = 8;

		private float m_maxOutput;

		private Vector3 m_panelOrientation;

		private float m_panelOffset;

		private bool m_isTwoSided;

		private MyFunctionalBlock m_solarBlock;

		private bool m_initialized;

		private byte m_debugCurrentPivot;

		private bool[] m_debugIsPivotInSun = new bool[8];

		private bool m_isBackgroundProcessing;

		private byte m_currentPivot;

		private float m_angleToSun;

		private int m_pivotsInSun;

		private bool[] m_isPivotInSun = new bool[8];

		private List<MyPhysics.HitInfo> m_hitList = new List<MyPhysics.HitInfo>();

		private Vector3D m_to;

		private Vector3D m_from;

		private Action ComputeSunAngleFunc;

		private Action<List<MyPhysics.HitInfo>> OnRayCastCompletedFunc;

		private Action OnSunAngleComputedFunc;

		public float MaxOutput
		{
			get
			{
				return m_maxOutput;
			}
			set
			{
				if (m_maxOutput != value)
				{
					m_maxOutput = value;
					this.OnProductionChanged.InvokeIfNotNull();
				}
			}
		}

		public Vector3 PanelOrientation => m_panelOrientation;

		public float PanelOffset => m_panelOffset;

		public byte DebugCurrentPivot => m_debugCurrentPivot;

		public bool[] DebugIsPivotInSun => m_debugIsPivotInSun;

		public event Action OnProductionChanged;

		public MySolarGameLogicComponent()
		{
			ComputeSunAngleFunc = ComputeSunAngle;
			OnSunAngleComputedFunc = OnSunAngleComputed;
			OnRayCastCompletedFunc = OnRayCastCompleted;
		}

		public void Initialize(Vector3 panelOrientation, bool isTwoSided, float panelOffset, MyFunctionalBlock solarBlock)
		{
			m_initialized = true;
			m_panelOrientation = panelOrientation;
			m_isTwoSided = isTwoSided;
			m_panelOffset = panelOffset;
			m_solarBlock = solarBlock;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			return null;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (m_solarBlock.CubeGrid.Physics == null)
			{
				return;
			}
			if (!m_solarBlock.IsWorking)
			{
				MaxOutput = 0f;
			}
			else if (!m_isBackgroundProcessing)
			{
				m_isBackgroundProcessing = true;
				m_currentPivot = m_debugCurrentPivot;
				for (int i = 0; i < 8; i++)
				{
					m_isPivotInSun[i] = m_debugIsPivotInSun[i];
				}
				Parallel.Start(ComputeSunAngleFunc);
			}
		}

		private void ComputeSunAngle()
		{
			m_angleToSun = Vector3.Dot(Vector3.Transform(m_panelOrientation, m_solarBlock.WorldMatrix.GetOrientation()), MySector.DirectionToSunNormalized);
			if ((m_angleToSun < 0f && !m_isTwoSided) || !m_solarBlock.IsFunctional)
			{
				MySandboxGame.Static.Invoke(OnSunAngleComputedFunc, "SolarGamelogic:OnSunAngleComputed");
				return;
			}
			if (MySectorWeatherComponent.IsOnDarkSide(m_solarBlock.WorldMatrix.Translation))
			{
				m_isPivotInSun.ForEach(delegate(bool x)
				{
					x = false;
				});
				m_pivotsInSun = 0;
				MySandboxGame.Static.Invoke(OnSunAngleComputedFunc, "SolarGamelogic:OnSunAngleComputed");
				return;
			}
			m_currentPivot %= 8;
			MatrixD orientation = m_solarBlock.WorldMatrix.GetOrientation();
			float num = (float)m_solarBlock.WorldMatrix.Forward.Dot(Vector3.Transform(m_panelOrientation, orientation));
			float num2 = (m_solarBlock.BlockDefinition.CubeSize == MyCubeSize.Large) ? 2.5f : 0.5f;
			Vector3D translation = m_solarBlock.WorldMatrix.Translation;
			translation += ((float)((int)m_currentPivot % 4) - 1.5f) * num2 * num * ((float)m_solarBlock.BlockDefinition.Size.X / 4f) * m_solarBlock.WorldMatrix.Left;
			translation += ((float)((int)m_currentPivot / 4) - 0.5f) * num2 * num * ((float)m_solarBlock.BlockDefinition.Size.Y / 2f) * m_solarBlock.WorldMatrix.Up;
			translation += num2 * num * ((float)m_solarBlock.BlockDefinition.Size.Z / 2f) * Vector3.Transform(m_panelOrientation, orientation) * m_panelOffset;
			m_from = translation + MySector.DirectionToSunNormalized * 100f;
			m_to = translation + MySector.DirectionToSunNormalized * m_solarBlock.CubeGrid.GridSize / 4f;
			MyPhysics.CastRayParallel(ref m_to, ref m_from, m_hitList, 15, OnRayCastCompletedFunc);
		}

		private void OnRayCastCompleted(List<MyPhysics.HitInfo> hits)
		{
			m_isPivotInSun[m_currentPivot] = true;
			foreach (MyPhysics.HitInfo hit in hits)
			{
				IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
				if (hitEntity != m_solarBlock.CubeGrid)
				{
					m_isPivotInSun[m_currentPivot] = false;
					break;
				}
				MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
				Vector3I? vector3I = myCubeGrid.RayCastBlocks(m_from, m_to);
				if (vector3I.HasValue && myCubeGrid.GetCubeBlock(vector3I.Value) != m_solarBlock.SlimBlock)
				{
					m_isPivotInSun[m_currentPivot] = false;
					break;
				}
			}
			m_pivotsInSun = 0;
			bool[] isPivotInSun = m_isPivotInSun;
			for (int i = 0; i < isPivotInSun.Length; i++)
			{
				if (isPivotInSun[i])
				{
					m_pivotsInSun++;
				}
			}
			MySandboxGame.Static.Invoke(OnSunAngleComputedFunc, "SolarGamelogic:OnSunAngleComputed");
		}

		private void OnSunAngleComputed()
		{
			m_isBackgroundProcessing = false;
			if ((m_angleToSun < 0f && !m_isTwoSided) || !m_solarBlock.Enabled)
			{
				MaxOutput = 0f;
				return;
			}
			float num = m_angleToSun;
			if (num < 0f)
			{
				num = ((!m_isTwoSided) ? 0f : Math.Abs(num));
			}
			num = (MaxOutput = num * ((float)m_pivotsInSun / 8f));
			m_debugCurrentPivot = m_currentPivot;
			m_debugCurrentPivot++;
			for (int i = 0; i < 8; i++)
			{
				m_debugIsPivotInSun[i] = m_isPivotInSun[i];
			}
		}
	}
}
