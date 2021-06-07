using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Inventory", 110)]
	internal class MyIngameHelpInventory : MyIngameHelpObjective
	{
		private IMyUseObject m_interactiveObject;

		private bool m_fPressed;

		private bool m_iPressed;

		public MyIngameHelpInventory()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Inventory_Title;
			RequiredIds = new string[2]
			{
				"IngameHelp_Movement",
				"IngameHelp_Jetpack2"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(LookingOnInteractiveObject));
			Details = new MyIngameHelpDetail[3]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail2,
					FinishCondition = UsePressed
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail3,
					FinishCondition = IPressed
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_InventoryTip";
			MyCharacterDetectorComponent.OnInteractiveObjectChanged += MyCharacterDetectorComponent_OnInteractiveObjectChanged;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MyCharacterDetectorComponent.OnInteractiveObjectUsed += MyCharacterDetectorComponent_OnInteractiveObjectUsed;
		}

		private void MyCharacterDetectorComponent_OnInteractiveObjectChanged(IMyUseObject obj)
		{
			if (obj is MyFloatingObject)
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
				m_fPressed = true;
			}
		}

		private bool LookingOnInteractiveObject()
		{
			return m_interactiveObject != null;
		}

		private bool UsePressed()
		{
			return m_fPressed;
		}

		private bool IPressed()
		{
			if (MyScreenManager.Screens.Any((MyGuiScreenBase x) => x is MyGuiScreenTerminal) && MyGuiScreenTerminal.GetCurrentScreen() == MyTerminalPageEnum.Inventory)
			{
				m_iPressed = true;
			}
			return m_iPressed;
		}
	}
}
