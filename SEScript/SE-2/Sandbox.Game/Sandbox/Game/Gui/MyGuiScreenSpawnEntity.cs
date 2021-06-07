using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenSpawnEntity : MyGuiScreenBase
	{
		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlListbox m_addComponentsListBox;

		private MyGuiControlCheckbox m_replicableEntityCheckBox;

		private new Vector3 m_position;

		public MyGuiScreenSpawnEntity(Vector3 position)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			m_position = position;
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenSpawnEntity";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.46f), null, "Select components to include in entity", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_replicableEntityCheckBox = new MyGuiControlCheckbox();
			m_replicableEntityCheckBox.Position = new Vector2(0f, -0.42f);
			m_replicableEntityCheckBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			Controls.Add(m_replicableEntityCheckBox);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.39f), null, "MyEntityReplicable / MyEntity", null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.32f), null, "Select components to add", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			if (m_addComponentsListBox == null)
			{
				m_addComponentsListBox = new MyGuiControlListbox();
			}
			m_addComponentsListBox.ClearItems();
			m_addComponentsListBox.MultiSelect = true;
			m_addComponentsListBox.Name = "AddComponents";
			List<MyDefinitionId> definedComponents = new List<MyDefinitionId>();
			MyDefinitionManager.Static.GetDefinedEntityComponents(ref definedComponents);
			foreach (MyDefinitionId item2 in definedComponents)
			{
				string text = item2.ToString();
				if (text.StartsWith("MyObjectBuilder_"))
				{
					text = text.Remove(0, "MyObectBuilder_".Length + 1);
				}
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(text), null, null, item2);
				m_addComponentsListBox.Add(item);
			}
			m_addComponentsListBox.VisibleRowsCount = definedComponents.Count + 1;
			m_addComponentsListBox.Position = new Vector2(0f, 0f);
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

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			MyContainerDefinition myContainerDefinition = new MyContainerDefinition();
			foreach (MyGuiControlListbox.Item selectedItem in m_addComponentsListBox.SelectedItems)
			{
				if (selectedItem.UserData is MyDefinitionId)
				{
					MyDefinitionId myDefinitionId = (MyDefinitionId)selectedItem.UserData;
					MyContainerDefinition.DefaultComponent defaultComponent = new MyContainerDefinition.DefaultComponent();
					defaultComponent.BuilderType = myDefinitionId.TypeId;
					defaultComponent.SubtypeId = myDefinitionId.SubtypeId;
					myContainerDefinition.DefaultComponents.Add(defaultComponent);
				}
			}
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = null;
			if (m_replicableEntityCheckBox.IsChecked)
			{
				myObjectBuilder_EntityBase = new MyObjectBuilder_ReplicableEntity();
				myContainerDefinition.Id = new MyDefinitionId(typeof(MyObjectBuilder_ReplicableEntity), "DebugTest");
			}
			else
			{
				myObjectBuilder_EntityBase = new MyObjectBuilder_EntityBase();
				myContainerDefinition.Id = new MyDefinitionId(typeof(MyObjectBuilder_EntityBase), "DebugTest");
			}
			MyDefinitionManager.Static.SetEntityContainerDefinition(myContainerDefinition);
			myObjectBuilder_EntityBase.SubtypeName = myContainerDefinition.Id.SubtypeName;
			myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation(m_position, Vector3.Forward, Vector3.Up);
			MyEntities.CreateFromObjectBuilderAndAdd(myObjectBuilder_EntityBase, fadeIn: false);
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
