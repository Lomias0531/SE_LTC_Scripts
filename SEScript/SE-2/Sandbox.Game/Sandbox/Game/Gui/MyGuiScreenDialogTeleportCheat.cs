using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDialogTeleportCheat : MyGuiScreenBase
	{
		private List<IMyGps> m_prefabDefinitions = new List<IMyGps>();

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlCombobox m_prefabs;

		public MyGuiScreenDialogTeleportCheat()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogTravelToCheat";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, "Select gps you want to reach. (Dont use for grids with subgrids.)", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_prefabs = new MyGuiControlCombobox(new Vector2(0.2f, 0f), new Vector2(0.3f, 0.05f));
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			List<IMyGps> list = new List<IMyGps>();
			MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
			foreach (IMyGps item in list)
			{
				int count = m_prefabDefinitions.Count;
				m_prefabDefinitions.Add(item);
				m_prefabs.AddItem(count, item.Name);
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
			int num = (int)m_prefabs.GetSelectedKey();
			IMyGps myGps = m_prefabDefinitions[(num != -1) ? num : 0];
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyAlesDebugInputComponent.TravelToWaypoint, myGps.Coords);
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
