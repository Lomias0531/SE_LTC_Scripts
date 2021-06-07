using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Camera", 20)]
	internal class MyIngameHelpCamera : MyIngameHelpObjective
	{
		private bool m_cameraModeSwitched;

		private bool m_initialCameraMode;

		private bool m_cameraDistanceChanged;

		private double m_initialCameraDistance;

		public MyIngameHelpCamera()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Camera_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			Details = new MyIngameHelpDetail[3]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Camera_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Camera_Detail2,
					FinishCondition = CameraModeCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Camera_Detail3,
					FinishCondition = AltWheelCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			RequiredCondition = ThirdPersonEnabledCondition;
		}

		private bool ThirdPersonEnabledCondition()
		{
			return MySession.Static.Settings.Enable3rdPersonView;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				m_initialCameraDistance = MyThirdPersonSpectator.Static.GetViewerDistance();
				m_initialCameraMode = localCharacter.IsInFirstPersonView;
			}
		}

		private bool CameraModeCondition()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				m_cameraModeSwitched |= (localCharacter.IsInFirstPersonView != m_initialCameraMode);
			}
			return m_cameraModeSwitched;
		}

		private bool AltWheelCondition()
		{
			m_cameraDistanceChanged |= (m_initialCameraDistance != MyThirdPersonSpectator.Static.GetViewerDistance());
			return m_cameraDistanceChanged;
		}
	}
}
