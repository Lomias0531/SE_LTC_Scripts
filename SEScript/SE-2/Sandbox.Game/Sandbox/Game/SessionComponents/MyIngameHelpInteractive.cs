using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using System;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Interactive", 23)]
	internal class MyIngameHelpInteractive : MyIngameHelpObjective
	{
		private float LOOKING_TIME = 1f;

		private IMyUseObject m_interactiveObject;

		private bool m_fPressed;

		private float m_lookingCounter;

		private bool m_kPressed;

		private bool m_iPressed;

		public MyIngameHelpInteractive()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Interactive_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			Details = new MyIngameHelpDetail[4]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail2,
					FinishCondition = UsePressed
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail3,
					FinishCondition = KPressed
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Interactive_Detail4,
					FinishCondition = IPressed
				}
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(LookingOnInteractiveObjectDelayed));
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_InteractiveTip";
			MyCharacterDetectorComponent.OnInteractiveObjectChanged += MyCharacterDetectorComponent_OnInteractiveObjectChanged;
		}

		private void MyCharacterDetectorComponent_OnInteractiveObjectChanged(IMyUseObject obj)
		{
			if (obj is MyUseObjectBase)
			{
				MyCubeBlock myCubeBlock;
				if ((myCubeBlock = (obj.Owner as MyCubeBlock)) != null && myCubeBlock.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies)
				{
					m_interactiveObject = obj;
				}
			}
			else
			{
				m_interactiveObject = null;
			}
		}

		private bool IsFriendly()
		{
			if (m_interactiveObject == null)
			{
				return false;
			}
			MyCubeBlock myCubeBlock;
			if ((myCubeBlock = (m_interactiveObject.Owner as MyCubeBlock)) != null && myCubeBlock.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies)
			{
				return true;
			}
			return false;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MyCharacterDetectorComponent.OnInteractiveObjectUsed += MyCharacterDetectorComponent_OnInteractiveObjectUsed;
		}

		private bool LookingOnInteractiveObject()
		{
			if (m_interactiveObject != null)
			{
				return IsFriendly();
			}
			return false;
		}

		private bool LookingOnInteractiveObjectDelayed()
		{
			if (LookingOnInteractiveObject())
			{
				m_lookingCounter += 0.0166666675f;
			}
			else
			{
				m_lookingCounter = 0f;
			}
			return m_lookingCounter > LOOKING_TIME;
		}

		private void MyCharacterDetectorComponent_OnInteractiveObjectUsed(IMyUseObject obj)
		{
			if (m_interactiveObject == obj)
			{
				m_fPressed = true;
			}
		}

		private bool UsePressed()
		{
			return m_fPressed;
		}

		private bool KPressed()
		{
			if (LookingOnInteractiveObject() && m_interactiveObject.SupportedActions.HasFlag(UseActionEnum.OpenTerminal) && MyGuiScreenTerminal.GetCurrentScreen() != MyTerminalPageEnum.None && MyGuiScreenTerminal.GetCurrentScreen() != 0)
			{
				m_kPressed = true;
			}
			return m_kPressed;
		}

		private bool IPressed()
		{
			if (LookingOnInteractiveObject() && m_interactiveObject.SupportedActions.HasFlag(UseActionEnum.OpenTerminal) && MyGuiScreenTerminal.GetCurrentScreen() == MyTerminalPageEnum.Inventory)
			{
				m_iPressed = true;
			}
			return m_iPressed;
		}
	}
}
