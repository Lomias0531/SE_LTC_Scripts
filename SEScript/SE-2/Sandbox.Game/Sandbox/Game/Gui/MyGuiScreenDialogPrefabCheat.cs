using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDialogPrefabCheat : MyGuiScreenBase
	{
		private List<MyPrefabDefinition> m_prefabDefinitions = new List<MyPrefabDefinition>();

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlCombobox m_prefabs;

		public MyGuiScreenDialogPrefabCheat()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogPrefabCheat";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, "Select the name of the prefab that you want to spawn", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_prefabs = new MyGuiControlCombobox(new Vector2(0.2f, 0f), new Vector2(0.3f, 0.05f));
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			foreach (KeyValuePair<string, MyPrefabDefinition> prefabDefinition in MyDefinitionManager.Static.GetPrefabDefinitions())
			{
				int count = m_prefabDefinitions.Count;
				m_prefabDefinitions.Add(prefabDefinition.Value);
				m_prefabs.AddItem(count, new StringBuilder(prefabDefinition.Key));
			}
			Controls.Add(m_prefabs);
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
			MyPrefabDefinition myPrefabDefinition = m_prefabDefinitions[(int)m_prefabs.GetSelectedKey()];
			Vector3D position = MySector.MainCamera.Position;
			Vector3 forwardVector = MySector.MainCamera.ForwardVector;
			MatrixD arg = MatrixD.CreateWorld(up: MySector.MainCamera.UpVector, position: position + forwardVector * 70f, forward: forwardVector);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyCestmirDebugInputComponent.AddPrefabServer, myPrefabDefinition.Id.SubtypeName, arg);
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
