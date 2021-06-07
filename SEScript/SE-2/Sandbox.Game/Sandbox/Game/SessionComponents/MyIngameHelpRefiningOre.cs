using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_RefiningOre", 210)]
	internal class MyIngameHelpRefiningOre : MyIngameHelpObjective
	{
		private HashSet<MyRefinery> m_observedRefineries = new HashSet<MyRefinery>();

		private bool m_ingotProduced;

		public MyIngameHelpRefiningOre()
		{
			TitleEnum = MySpaceTexts.IngameHelp_RefiningOre_Title;
			RequiredIds = new string[2]
			{
				"IngameHelp_HandDrill",
				"IngameHelp_Building"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(OreInInventory));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_RefiningOre_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_RefiningOre_Detail2,
					FinishCondition = IngotFromRefinery
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MyInventory.OnTransferByUser += MyInventory_OnTransferByUser;
		}

		private void MyInventory_OnTransferByUser(IMyInventory inventory1, IMyInventory inventory2, IMyInventoryItem item, MyFixedPoint amount)
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && item.Content is MyObjectBuilder_Ore && inventory1.Owner == myCharacter)
			{
				MyRefinery myRefinery = inventory2.Owner as MyRefinery;
				if (myRefinery != null && !m_observedRefineries.Contains(myRefinery))
				{
					myRefinery.OutputInventory.ContentsAdded += OutputInventory_ContentsAdded;
					m_observedRefineries.Add(myRefinery);
				}
			}
		}

		private void OutputInventory_ContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			if (item.Content is MyObjectBuilder_Ingot)
			{
				m_ingotProduced = true;
			}
		}

		private bool OreInInventory()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				foreach (MyPhysicalInventoryItem item in myCharacter.GetInventory().GetItems())
				{
					if (item.Content is MyObjectBuilder_Ore)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool IngotFromRefinery()
		{
			return m_ingotProduced;
		}
	}
}
