using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ConveyorConnector))]
	public class MyConveyorConnector : MyCubeBlock, IMyConveyorSegmentBlock, Sandbox.ModAPI.IMyConveyorTube, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyConveyorTube
	{
		private class Sandbox_Game_Entities_MyConveyorConnector_003C_003EActor : IActivator, IActivator<MyConveyorConnector>
		{
			private sealed override object CreateInstance()
			{
				return new MyConveyorConnector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyConveyorConnector CreateInstance()
			{
				return new MyConveyorConnector();
			}

			MyConveyorConnector IActivator<MyConveyorConnector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly MyConveyorSegment m_segment = new MyConveyorSegment();

		private bool m_working;

		private bool m_emissivitySet;

		private MyResourceStateEnum m_state;

		public override float MaxGlassDistSq => 22500f;

		public MyConveyorSegment ConveyorSegment => m_segment;

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_emissivitySet = false;
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorSegment(m_segment));
		}

		public void InitializeConveyorSegment()
		{
			MyConveyorLine.BlockLinePositionInformation[] blockLinePositions = MyConveyorLine.GetBlockLinePositions(this);
			if (blockLinePositions.Length != 0)
			{
				ConveyorLinePosition connectingPosition = PositionToGridCoords(blockLinePositions[0].Position).GetConnectingPosition();
				ConveyorLinePosition connectingPosition2 = PositionToGridCoords(blockLinePositions[1].Position).GetConnectingPosition();
				m_segment.Init(this, connectingPosition, connectingPosition2, blockLinePositions[0].LineType);
			}
		}

		private ConveyorLinePosition PositionToGridCoords(ConveyorLinePosition position)
		{
			ConveyorLinePosition result = default(ConveyorLinePosition);
			Matrix result2 = default(Matrix);
			base.Orientation.GetMatrix(out result2);
			Vector3 value = Vector3.Transform(new Vector3(position.LocalGridPosition), result2);
			result.LocalGridPosition = Vector3I.Round(value) + base.Position;
			result.Direction = base.Orientation.TransformDirection(position.Direction);
			return result;
		}

		public override void UpdateBeforeSimulation100()
		{
			if (m_segment.ConveyorLine != null)
			{
				MyResourceStateEnum myResourceStateEnum = m_segment.ConveyorLine.UpdateIsWorking();
				if (!m_emissivitySet || m_working != m_segment.ConveyorLine.IsWorking || m_state != myResourceStateEnum)
				{
					m_working = m_segment.ConveyorLine.IsWorking;
					m_state = myResourceStateEnum;
					SetEmissiveStateWorking();
				}
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			if (base.IsWorking)
			{
				if (m_state == MyResourceStateEnum.OverloadBlackout)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
				}
				m_emissivitySet = true;
				if (m_working)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			if (base.IsFunctional && !m_working)
			{
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
			}
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			m_emissivitySet = false;
		}
	}
}
