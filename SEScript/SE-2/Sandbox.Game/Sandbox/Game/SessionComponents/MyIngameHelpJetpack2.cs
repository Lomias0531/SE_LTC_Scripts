using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Jetpack2", 50)]
	internal class MyIngameHelpJetpack2 : MyIngameHelpObjective
	{
		private bool m_dampenersChanged;

		private bool m_dampenersInitialState;

		public MyIngameHelpJetpack2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Jetpack_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Jetpack"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack2_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack2_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.DAMPING)
					},
					FinishCondition = DampenersCondition
				}
			};
			FollowingId = "IngameHelp_JetpackTip";
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null && controlledEntity.EnabledThrusts)
			{
				m_dampenersInitialState = controlledEntity.EnabledDamping;
			}
		}

		private bool DampenersCondition()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null && controlledEntity.EnabledThrusts && m_dampenersInitialState != controlledEntity.EnabledDamping)
			{
				m_dampenersChanged = true;
			}
			return m_dampenersChanged;
		}
	}
}
