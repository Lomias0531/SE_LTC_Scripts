using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Ownership", 90)]
	internal class MyIngameHelpOwnership : MyIngameHelpObjective
	{
		private bool m_accessDeniedHappened;

		private bool m_blockHacked;

		private HashSet<MyTerminalBlock> m_hackingBlocks = new HashSet<MyTerminalBlock>();

		public MyIngameHelpOwnership()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Ownership_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Building"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(AccessDeniedHappened));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Ownership_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Ownership_Detail2,
					FinishCondition = BlockHackedCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_OwnershipTip";
			if (MyHud.Notifications != null)
			{
				MyHud.Notifications.OnNotificationAdded += Notifications_OnNotificationAdded;
			}
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MySlimBlock.OnAnyBlockHackedChanged += MyTerminalBlock_OnAnyBlockHackedChanged;
		}

		private void MyTerminalBlock_OnAnyBlockHackedChanged(MyTerminalBlock obj, long grinderOwner)
		{
			MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
			if (!m_hackingBlocks.Contains(obj) && myCharacter != null && myCharacter.GetPlayerIdentityId() == grinderOwner)
			{
				m_hackingBlocks.Add(obj);
				obj.OwnershipChanged += obj_OwnershipChanged;
			}
		}

		private void obj_OwnershipChanged(MyTerminalBlock obj)
		{
			MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
			if (myCharacter != null && myCharacter.GetPlayerIdentityId() == obj.OwnerId)
			{
				m_blockHacked = true;
			}
		}

		private void Notifications_OnNotificationAdded(MyNotificationSingletons obj)
		{
			if (obj == MyNotificationSingletons.AccessDenied)
			{
				m_accessDeniedHappened = true;
			}
		}

		private bool BlockHackedCondition()
		{
			return m_blockHacked;
		}

		private bool AccessDeniedHappened()
		{
			return m_accessDeniedHappened;
		}
	}
}
