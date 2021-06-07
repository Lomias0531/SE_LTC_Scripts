using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;

namespace Sandbox.Game.SessionComponents.IngameHelp
{
	[IngameObjective("IngameHelp_Datapad", 300)]
	internal class MyIngameHelpDatapad : MyIngameHelpObjective
	{
		private MyDefinitionId m_datapadDefinitionId;

		private bool m_datapadOpenned;

		private bool m_iPressed;

		public MyIngameHelpDatapad()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Datapad_Title;
			RequiredIds = new string[3]
			{
				"IngameHelp_Intro",
				"IngameHelp_HUD",
				"IngameHelp_EconomyStation"
			};
			Details = new MyIngameHelpDetail[3]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Datapad_Desc
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail3,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.INVENTORY)
					},
					FinishCondition = IPressed
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Datapad_Detail1,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.SECONDARY_TOOL_ACTION)
					},
					FinishCondition = CheckDatapadOpened
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			RequiredCondition = CheckDatapadInInventory;
			MyGuiDatapadEditScreen.OnDatapadOpened += OnDatapadOpened;
		}

		private void OnDatapadOpened()
		{
			m_datapadOpenned = true;
			MyGuiDatapadEditScreen.OnDatapadOpened -= OnDatapadOpened;
		}

		private bool CheckDatapadInInventory()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity == null)
			{
				return false;
			}
			MyInventoryBase myInventoryBase = myEntity.Components.Get<MyInventoryBase>();
			if (myInventoryBase == null)
			{
				return false;
			}
			if (m_datapadDefinitionId.TypeId.IsNull)
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component == null)
				{
					return false;
				}
				m_datapadDefinitionId = component.EconomyDefinition.DatapadDefinition;
			}
			return myInventoryBase.GetItemAmount(m_datapadDefinitionId) > 0;
		}

		private bool CheckDatapadOpened()
		{
			return m_datapadOpenned;
		}

		private bool IPressed()
		{
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.INVENTORY))
			{
				m_iPressed = true;
			}
			return m_iPressed;
		}
	}
}
