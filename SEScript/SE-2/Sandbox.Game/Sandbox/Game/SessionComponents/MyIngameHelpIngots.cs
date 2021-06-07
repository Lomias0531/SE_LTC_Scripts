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
	[IngameObjective("IngameHelp_Ingots", 220)]
	internal class MyIngameHelpIngots : MyIngameHelpObjective
	{
		private HashSet<MyAssembler> m_observedAssemblers = new HashSet<MyAssembler>();

		private bool m_ingotAdded;

		private bool m_steelProduced;

		public MyIngameHelpIngots()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Ingots_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_RefiningOre"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(IngotsInInventory));
			Details = new MyIngameHelpDetail[3]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Ingots_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Ingots_Detail2,
					FinishCondition = PutToAssembler
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Ingots_Detail3,
					FinishCondition = SteelFromAssembler
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
			if (myCharacter != null && item.Content is MyObjectBuilder_Ingot && item.Content.SubtypeName == "Iron" && inventory1.Owner == myCharacter)
			{
				MyAssembler myAssembler = inventory2.Owner as MyAssembler;
				if (myAssembler != null && !m_observedAssemblers.Contains(myAssembler))
				{
					myAssembler.OutputInventory.ContentsAdded += OutputInventory_ContentsAdded;
					m_observedAssemblers.Add(myAssembler);
					m_ingotAdded = true;
				}
			}
		}

		private bool PutToAssembler()
		{
			return m_ingotAdded;
		}

		private void OutputInventory_ContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			if (item.Content is MyObjectBuilder_Component && item.Content.SubtypeName == "SteelPlate")
			{
				m_steelProduced = true;
			}
		}

		private bool IngotsInInventory()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				foreach (MyPhysicalInventoryItem item in myCharacter.GetInventory().GetItems())
				{
					if (item.Content is MyObjectBuilder_Ingot && item.Content.SubtypeName == "Iron")
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool SteelFromAssembler()
		{
			return m_steelProduced;
		}
	}
}
