using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenSpawnDefinedEntity : MyGuiScreenBase
	{
		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlListbox m_containersListBox;

		private MyGuiControlCheckbox m_replicableEntityCheckBox;

		private new Vector3 m_position;

		public MyGuiScreenSpawnDefinedEntity(Vector3 position)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			m_position = position;
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenSpawnDefinedEntity";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.46f), null, "Select entity to spawn", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			if (m_containersListBox == null)
			{
				m_containersListBox = new MyGuiControlListbox();
			}
			m_containersListBox.ClearItems();
			m_containersListBox.MultiSelect = false;
			m_containersListBox.Name = "Containers";
			List<MyDefinitionId> definedContainers = new List<MyDefinitionId>();
			MyDefinitionManager.Static.GetDefinedEntityContainers(ref definedContainers);
			foreach (MyDefinitionId item2 in definedContainers)
			{
				string text = item2.ToString();
				if (text.StartsWith("MyObjectBuilder_"))
				{
					text = text.Remove(0, "MyObectBuilder_".Length + 1);
				}
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(text), null, null, item2);
				m_containersListBox.Add(item);
			}
			m_containersListBox.VisibleRowsCount = definedContainers.Count + 1;
			m_containersListBox.Position = new Vector2(0f, 0f);
			m_containersListBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_containersListBox.ItemSize = new Vector2(0.36f, 0.036f);
			m_containersListBox.Size = new Vector2(0.4f, 0.6f);
			Controls.Add(m_containersListBox);
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.35f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.35f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			Controls.Add(m_confirmButton);
			Controls.Add(m_cancelButton);
			m_confirmButton.ButtonClicked += confirmButton_OnButtonClick;
			m_cancelButton.ButtonClicked += cancelButton_OnButtonClick;
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			new MyContainerDefinition();
			foreach (MyGuiControlListbox.Item selectedItem in m_containersListBox.SelectedItems)
			{
				if (selectedItem.UserData is MyDefinitionId)
				{
					MyEntities.CreateEntityAndAdd((MyDefinitionId)selectedItem.UserData, fadeIn: false, setPosAndRot: true, m_position);
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
