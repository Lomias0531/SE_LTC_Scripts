using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenConfigComponents : MyGuiScreenBase
	{
		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private long m_entityId;

		private List<MyEntity> m_entities;

		private MyGuiControlCombobox m_entitiesSelection;

		private MyGuiControlListbox m_removeComponentsListBox;

		private MyGuiControlListbox m_addComponentsListBox;

		public MyGuiScreenConfigComponents(List<MyEntity> entities)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			m_entities = entities;
			m_entityId = entities.FirstOrDefault().EntityId;
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenConfigComponents";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.46f), null, "Select components to remove and components to add", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			if (m_entitiesSelection == null)
			{
				m_entitiesSelection = new MyGuiControlCombobox();
				m_entitiesSelection.ItemSelected += EntitySelected;
			}
			m_entitiesSelection.Position = new Vector2(0f, -0.42f);
			m_entitiesSelection.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_entitiesSelection.ClearItems();
			foreach (MyEntity entity2 in m_entities)
			{
				m_entitiesSelection.AddItem(entity2.EntityId, entity2.ToString());
			}
			m_entitiesSelection.SelectItemByKey(m_entityId, sendEvent: false);
			Controls.Add(m_entitiesSelection);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.39f), null, $"EntityID = {m_entityId}", null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			if (MyEntities.TryGetEntityById(m_entityId, out MyEntity entity))
			{
				Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.36f), null, string.Format("Name: {1}, Type: {0}", entity.GetType().Name, entity.DisplayNameText), null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			}
			Controls.Add(new MyGuiControlLabel(new Vector2(-0.21f, -0.32f), null, "Select components to remove", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			if (m_removeComponentsListBox == null)
			{
				m_removeComponentsListBox = new MyGuiControlListbox();
			}
			m_removeComponentsListBox.ClearItems();
			m_removeComponentsListBox.MultiSelect = true;
			m_removeComponentsListBox.Name = "RemoveComponents";
			if (MyComponentContainerExtension.TryGetEntityComponentTypes(m_entityId, out List<Type> components))
			{
				foreach (Type item3 in components)
				{
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item3.Name), null, null, item3);
					m_removeComponentsListBox.Add(item);
				}
				m_removeComponentsListBox.VisibleRowsCount = components.Count + 1;
			}
			m_removeComponentsListBox.Position = new Vector2(-0.21f, 0f);
			m_removeComponentsListBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_removeComponentsListBox.ItemSize = new Vector2(0.38f, 0.036f);
			m_removeComponentsListBox.Size = new Vector2(0.4f, 0.6f);
			Controls.Add(m_removeComponentsListBox);
			Controls.Add(new MyGuiControlLabel(new Vector2(0.21f, -0.32f), null, "Select components to add", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			if (m_addComponentsListBox == null)
			{
				m_addComponentsListBox = new MyGuiControlListbox();
			}
			m_addComponentsListBox.ClearItems();
			m_addComponentsListBox.MultiSelect = true;
			m_addComponentsListBox.Name = "AddComponents";
			components.Clear();
			List<MyDefinitionId> definedComponents = new List<MyDefinitionId>();
			MyDefinitionManager.Static.GetDefinedEntityComponents(ref definedComponents);
			foreach (MyDefinitionId item4 in definedComponents)
			{
				string text = item4.ToString();
				if (text.StartsWith("MyObjectBuilder_"))
				{
					text = text.Remove(0, "MyObectBuilder_".Length + 1);
				}
				MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(new StringBuilder(text), null, null, item4);
				m_addComponentsListBox.Add(item2);
			}
			m_addComponentsListBox.VisibleRowsCount = definedComponents.Count + 1;
			m_addComponentsListBox.Position = new Vector2(0.21f, 0f);
			m_addComponentsListBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_addComponentsListBox.ItemSize = new Vector2(0.36f, 0.036f);
			m_addComponentsListBox.Size = new Vector2(0.4f, 0.6f);
			Controls.Add(m_addComponentsListBox);
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.35f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.35f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			Controls.Add(m_confirmButton);
			Controls.Add(m_cancelButton);
			m_confirmButton.ButtonClicked += confirmButton_OnButtonClick;
			m_cancelButton.ButtonClicked += cancelButton_OnButtonClick;
		}

		private void EntitySelected()
		{
			m_entityId = m_entitiesSelection.GetSelectedKey();
			RecreateControls(contructor: false);
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			foreach (MyGuiControlListbox.Item selectedItem in m_removeComponentsListBox.SelectedItems)
			{
				MyComponentContainerExtension.TryRemoveComponent(m_entityId, selectedItem.UserData as Type);
			}
			foreach (MyGuiControlListbox.Item selectedItem2 in m_addComponentsListBox.SelectedItems)
			{
				if (selectedItem2.UserData is MyDefinitionId)
				{
					MyComponentContainerExtension.TryAddComponent(m_entityId, (MyDefinitionId)selectedItem2.UserData);
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
