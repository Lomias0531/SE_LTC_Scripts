using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game.Entity.UseObject;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HandDrill", 210)]
	internal class MyIngameHelpHandDrill : MyIngameHelpObjective
	{
		private IMyUseObject m_interactiveObject;

		private bool m_rockPicked;

		private bool m_isDrilling;

		private bool m_diggedTunnel;

		public MyIngameHelpHandDrill()
		{
			TitleEnum = MySpaceTexts.IngameHelp_HandDrill_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(PlayerHasHandDrill));
			Details = new MyIngameHelpDetail[4]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail2,
					FinishCondition = PlayerIsDrillingStone
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail3,
					FinishCondition = PickedRocks
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail4,
					FinishCondition = DiggedTunnel
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			MyCharacterDetectorComponent.OnInteractiveObjectChanged += MyCharacterDetectorComponent_OnInteractiveObjectChanged;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MyCharacterDetectorComponent.OnInteractiveObjectUsed += MyCharacterDetectorComponent_OnInteractiveObjectUsed;
		}

		private bool PlayerHasHandDrill()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				return myCharacter.EquippedTool is MyHandDrill;
			}
			return false;
		}

		private bool PlayerIsDrillingStone()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				MyHandDrill myHandDrill = myCharacter.EquippedTool as MyHandDrill;
				if (myHandDrill != null && myHandDrill.IsShooting && myHandDrill.DrilledEntity is MyVoxelBase && myHandDrill.CollectingOre)
				{
					m_isDrilling = true;
				}
			}
			return m_isDrilling;
		}

		private void MyCharacterDetectorComponent_OnInteractiveObjectChanged(IMyUseObject obj)
		{
			if (obj is MyFloatingObject && ((MyFloatingObject)obj).ItemDefinition.Id.SubtypeName.Contains("Stone"))
			{
				m_interactiveObject = obj;
			}
			else
			{
				m_interactiveObject = null;
			}
		}

		private void MyCharacterDetectorComponent_OnInteractiveObjectUsed(IMyUseObject obj)
		{
			if (m_interactiveObject == obj)
			{
				m_rockPicked = true;
			}
		}

		private bool PickedRocks()
		{
			return m_rockPicked;
		}

		private bool DiggedTunnel()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				MyHandDrill myHandDrill = myCharacter.EquippedTool as MyHandDrill;
				if (myHandDrill != null && myHandDrill.IsShooting && myHandDrill.DrilledEntity is MyVoxelBase && !myHandDrill.CollectingOre)
				{
					m_diggedTunnel = true;
				}
			}
			return m_diggedTunnel;
		}
	}
}
