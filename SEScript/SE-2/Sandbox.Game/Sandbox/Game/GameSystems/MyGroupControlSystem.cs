using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyGroupControlSystem
	{
		private MyShipController m_currentShipController;

		private readonly CachingHashSet<MyShipController> m_groupControllers = new CachingHashSet<MyShipController>();

		private readonly HashSet<MyCubeGrid> m_cubeGrids = new HashSet<MyCubeGrid>();

		private bool m_controlDirty;

		private bool m_firstControlRecalculation;

		private MyEntity m_relativeDampeningEntity;

		public MyEntity RelativeDampeningEntity
		{
			get
			{
				return m_relativeDampeningEntity;
			}
			set
			{
				if (m_relativeDampeningEntity != value)
				{
					if (m_relativeDampeningEntity != null)
					{
						m_relativeDampeningEntity.OnClose -= relativeDampeningEntityClosed;
					}
					m_relativeDampeningEntity = value;
					if (m_relativeDampeningEntity != null)
					{
						m_relativeDampeningEntity.OnClose += relativeDampeningEntityClosed;
					}
				}
			}
		}

		private MyShipController CurrentShipController
		{
			get
			{
				return m_currentShipController;
			}
			set
			{
				if (value != m_currentShipController)
				{
					if (value == null)
					{
						MyShipController currentShipController = m_currentShipController;
						m_currentShipController = value;
						MyGridPhysicalHierarchy.Static.UpdateRoot(currentShipController.CubeGrid);
					}
					else
					{
						m_currentShipController = value;
						MyGridPhysicalHierarchy.Static.UpdateRoot(m_currentShipController.CubeGrid);
					}
				}
			}
		}

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (!m_controlDirty)
				{
					if (CurrentShipController != null)
					{
						return CurrentShipController.ControllerInfo.Controller != null;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsLocallyControlled => GetController()?.Player.IsLocalPlayer ?? false;

		public bool IsControlled => GetController() != null;

		private void relativeDampeningEntityClosed(MyEntity entity)
		{
			m_relativeDampeningEntity = null;
		}

		public MyGroupControlSystem()
		{
			CurrentShipController = null;
			m_controlDirty = false;
			m_firstControlRecalculation = true;
		}

		public void UpdateBeforeSimulation()
		{
			m_groupControllers.ApplyChanges();
			if (m_controlDirty)
			{
				UpdateControl();
				m_controlDirty = false;
				m_firstControlRecalculation = false;
			}
			UpdateControls();
		}

		public void UpdateBeforeSimulation100()
		{
			if (RelativeDampeningEntity != null && CurrentShipController != null)
			{
				MyEntityThrustComponent.UpdateRelativeDampeningEntity(CurrentShipController, RelativeDampeningEntity);
			}
		}

		private void UpdateControl()
		{
			MyShipController myShipController = null;
			foreach (MyShipController groupController in m_groupControllers)
			{
				if (myShipController == null)
				{
					myShipController = groupController;
				}
				else if (MyShipController.HasPriorityOver(groupController, myShipController))
				{
					myShipController = groupController;
				}
			}
			CurrentShipController = myShipController;
			if (Sync.IsServer && CurrentShipController != null)
			{
				_ = CurrentShipController.ControllerInfo.Controller;
				foreach (MyCubeGrid cubeGrid in m_cubeGrids)
				{
					if (CurrentShipController.ControllerInfo.Controller != null)
					{
						Sync.Players.TryExtendControl(CurrentShipController, cubeGrid);
					}
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
				{
					CurrentShipController.GridWheels.InitControl(CurrentShipController.Entity);
				}
			}
		}

		public void RemoveControllerBlock(MyShipController controllerBlock)
		{
			m_groupControllers.ApplyAdditions();
			if (m_groupControllers.Contains(controllerBlock))
			{
				m_groupControllers.Remove(controllerBlock);
			}
			if (controllerBlock == CurrentShipController)
			{
				m_controlDirty = true;
				m_cubeGrids.ForEach(delegate(MyCubeGrid x)
				{
					x.MarkForUpdate();
				});
			}
			if (Sync.IsServer && controllerBlock == CurrentShipController)
			{
				Sync.Players.ReduceAllControl(CurrentShipController);
				CurrentShipController = null;
			}
		}

		public void AddControllerBlock(MyShipController controllerBlock)
		{
			m_groupControllers.Add(controllerBlock);
			bool flag = false;
			if (CurrentShipController != null && CurrentShipController.CubeGrid != controllerBlock.CubeGrid)
			{
				MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(controllerBlock.CubeGrid);
				if (group != null)
				{
					foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
					{
						if (node.NodeData == CurrentShipController.CubeGrid)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag && CurrentShipController != null && CurrentShipController.CubeGrid != controllerBlock.CubeGrid)
			{
				RemoveControllerBlock(CurrentShipController);
				CurrentShipController = null;
			}
			bool flag2 = CurrentShipController == null || MyShipController.HasPriorityOver(controllerBlock, CurrentShipController);
			if (flag2)
			{
				m_controlDirty = true;
				m_cubeGrids.ForEach(delegate(MyCubeGrid x)
				{
					x.MarkForUpdate();
				});
			}
			if (Sync.IsServer && CurrentShipController != null && flag2)
			{
				Sync.Players.ReduceAllControl(CurrentShipController);
			}
		}

		public void RemoveGrid(MyCubeGrid CubeGrid)
		{
			if (Sync.IsServer && CurrentShipController != null && CurrentShipController.ControllerInfo.Controller != null)
			{
				Sync.Players.ReduceControl(CurrentShipController, CubeGrid);
			}
			m_cubeGrids.Remove(CubeGrid);
		}

		public void AddGrid(MyCubeGrid CubeGrid)
		{
			m_cubeGrids.Add(CubeGrid);
			if (Sync.IsServer && !m_controlDirty && CurrentShipController != null && CurrentShipController.ControllerInfo.Controller != null)
			{
				Sync.Players.ExtendControl(CurrentShipController, CubeGrid);
			}
		}

		public MyEntityController GetController()
		{
			if (CurrentShipController != null)
			{
				return CurrentShipController.ControllerInfo.Controller;
			}
			return null;
		}

		public MyShipController GetShipController()
		{
			return CurrentShipController;
		}

		public void DebugDraw(float startYCoord)
		{
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "Controlled group controllers:", Color.GreenYellow, 0.5f);
			startYCoord += 13f;
			foreach (MyShipController groupController in m_groupControllers)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "  " + groupController.ToString(), Color.LightYellow, 0.5f);
				startYCoord += 13f;
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "Controlled group grids:", Color.GreenYellow, 0.5f);
			startYCoord += 13f;
			foreach (MyCubeGrid cubeGrid in m_cubeGrids)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "  " + cubeGrid.ToString(), Color.LightYellow, 0.5f);
				startYCoord += 13f;
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, startYCoord), "  " + CurrentShipController, Color.OrangeRed, 0.5f);
			startYCoord += 13f;
		}

		public void UpdateControls()
		{
			foreach (MyShipController groupController in m_groupControllers)
			{
				groupController.UpdateControls();
			}
		}
	}
}
