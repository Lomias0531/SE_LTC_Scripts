using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDialogInventoryCheat : MyGuiScreenBase
	{
		private List<MyPhysicalItemDefinition> m_physicalItemDefinitions = new List<MyPhysicalItemDefinition>();

		private MyGuiControlTextbox m_amountTextbox;

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlCombobox m_items;

		private static double m_lastAmount;

		private static int m_lastSelectedItem;

		private static int addedAsteroidsCount;

		public MyGuiScreenDialogInventoryCheat()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogInventoryCheat";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, "Select the amount and type of items to spawn in your inventory", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_amountTextbox = new MyGuiControlTextbox(new Vector2(-0.2f, 0f), null, 9, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			m_items = new MyGuiControlCombobox(new Vector2(0.2f, 0f), new Vector2(0.3f, 0.05f));
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
			{
				MyPhysicalItemDefinition myPhysicalItemDefinition = allDefinition as MyPhysicalItemDefinition;
				if (myPhysicalItemDefinition != null && myPhysicalItemDefinition.CanSpawnFromScreen)
				{
					int count = m_physicalItemDefinitions.Count;
					m_physicalItemDefinitions.Add(myPhysicalItemDefinition);
					m_items.AddItem(count, allDefinition.DisplayNameText);
				}
			}
			Controls.Add(m_amountTextbox);
			Controls.Add(m_items);
			Controls.Add(m_confirmButton);
			Controls.Add(m_cancelButton);
			m_amountTextbox.Text = $"{m_lastAmount}";
			m_items.SelectItemByIndex(m_lastSelectedItem);
			m_confirmButton.ButtonClicked += confirmButton_OnButtonClick;
			m_cancelButton.ButtonClicked += cancelButton_OnButtonClick;
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsKeyPress(MyKeys.Enter))
			{
				confirmButton_OnButtonClick(m_confirmButton);
			}
			if (MyInput.Static.IsKeyPress(MyKeys.Escape))
			{
				cancelButton_OnButtonClick(m_cancelButton);
			}
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity != null && myEntity.HasInventory)
			{
				double result = 0.0;
				double.TryParse(m_amountTextbox.Text, out result);
				m_lastAmount = result;
				MyFixedPoint myFixedPoint = (MyFixedPoint)result;
				if (m_items.GetSelectedKey() < 0 || (int)m_items.GetSelectedKey() >= m_physicalItemDefinitions.Count)
				{
					return;
				}
				MyDefinitionId id = m_physicalItemDefinitions[(int)m_items.GetSelectedKey()].Id;
				m_lastSelectedItem = (int)m_items.GetSelectedKey();
				MyInventory inventory = myEntity.GetInventory();
				if (inventory != null)
				{
					if (!MySession.Static.CreativeMode)
					{
						myFixedPoint = MyFixedPoint.Min(inventory.ComputeAmountThatFits(id), myFixedPoint);
					}
					MyObjectBuilder_PhysicalObject objectBuilder = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(id);
					inventory.DebugAddItems(myFixedPoint, objectBuilder);
				}
			}
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
