using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Input;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Stuck", 470)]
	internal class MyIngameHelpStuck : MyIngameHelpObjective
	{
		private Queue<float> m_averageSpeed = new Queue<float>();

		private int CountForAverage = 60;

		public MyIngameHelpStuck()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Stuck_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(StuckedInsideGrid));
			Details = new MyIngameHelpDetail[3]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Stuck_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Stuck_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.TOGGLE_REACTORS)
					},
					FinishCondition = MovingInsideGrid
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Stuck_Detail3,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.LANDING_GEAR)
					},
					FinishCondition = MovingInsideGrid
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_StuckTip";
		}

		public override bool IsCritical()
		{
			return StuckedInsideGrid();
		}

		private bool StuckedInsideGrid()
		{
			MyCockpit myCockpit = MySession.Static.ControlledEntity as MyCockpit;
			if (myCockpit != null && myCockpit.BlockDefinition.EnableShipControl && myCockpit.ControlThrusters && myCockpit.EntityThrustComponent != null && myCockpit.EntityThrustComponent.ThrustCount > 0)
			{
				if (MyInput.Static.IsGameControlPressed(MyControlsSpace.FORWARD) || MyInput.Static.IsGameControlPressed(MyControlsSpace.BACKWARD) || MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT) || MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT))
				{
					m_averageSpeed.Enqueue(myCockpit.CubeGrid.Physics.LinearVelocity.LengthSquared());
					if (m_averageSpeed.Count < CountForAverage)
					{
						return false;
					}
					if (m_averageSpeed.Count > CountForAverage)
					{
						m_averageSpeed.Dequeue();
					}
					return m_averageSpeed.Average() < 0.001f;
				}
				m_averageSpeed.Clear();
			}
			return false;
		}

		private bool MovingInsideGrid()
		{
			MyCockpit myCockpit = MySession.Static.ControlledEntity as MyCockpit;
			if (myCockpit != null && myCockpit.BlockDefinition.EnableShipControl && myCockpit.ControlThrusters && myCockpit.EntityThrustComponent != null && myCockpit.EntityThrustComponent.ThrustCount > 0)
			{
				if (MyInput.Static.IsGameControlPressed(MyControlsSpace.FORWARD) || MyInput.Static.IsGameControlPressed(MyControlsSpace.BACKWARD) || MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT) || MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT))
				{
					m_averageSpeed.Enqueue(myCockpit.CubeGrid.Physics.LinearVelocity.LengthSquared());
					if (m_averageSpeed.Count < CountForAverage)
					{
						return false;
					}
					if (m_averageSpeed.Count > CountForAverage)
					{
						m_averageSpeed.Dequeue();
					}
					return m_averageSpeed.Average() > 1f;
				}
				m_averageSpeed.Clear();
			}
			return false;
		}
	}
}
