using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenCreateOrEditFaction : MyGuiScreenBase
	{
		protected MyGuiControlTextbox m_shortcut;

		protected MyGuiControlTextbox m_name;

		protected MyGuiControlMultilineEditableText m_desc;

		protected MyGuiControlMultilineEditableText m_privInfo;

		protected MyGuiControlImage m_factionIcon;

		protected MyGuiControlImageButton m_editFactionIconBtn;

		protected SerializableDefinitionId m_factionIconGroupId;

		protected int m_factionIconId;

		protected Vector3 m_factionColor;

		protected Vector3 m_factionIconColor;

		protected IMyFaction m_editFaction;

		public MyGuiScreenCreateOrEditFaction(ref IMyFaction editData)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(41f / 70f, 335f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = false;
			m_editFaction = editData;
			RecreateControls(constructor: true);
		}

		public MyGuiScreenCreateOrEditFaction()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(41f / 70f, 335f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = false;
		}

		public void Init(ref IMyFaction editData)
		{
			m_editFaction = editData;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenCreateOrEditFaction";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
		}

		protected virtual void OnOkClick(MyGuiControlButton sender)
		{
			bool num = m_editFaction == null || m_editFaction.IsLeader(MySession.Static.LocalPlayerId);
			bool flag = MySession.Static.IsUserAdmin(Sync.MyId);
			if (!num && !flag)
			{
				ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsMissingRights));
				CloseScreenNow();
				return;
			}
			m_shortcut.Text = m_shortcut.Text.Replace(" ", string.Empty);
			m_name.Text = m_name.Text.Trim();
			if (m_shortcut.Text.Length != 3 && m_shortcut.Enabled)
			{
				ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsTag));
				return;
			}
			if (MySession.Static.Factions.FactionTagExists(m_shortcut.Text, m_editFaction))
			{
				ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsTagAlreadyExists));
				return;
			}
			if (m_name.Text.Length < 4)
			{
				ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsNameTooShort));
				return;
			}
			if (MySession.Static.Factions.FactionNameExists(m_name.Text, m_editFaction))
			{
				ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsNameAlreadyExists));
				return;
			}
			if (m_editFaction != null)
			{
				MySession.Static.Factions.EditFaction(m_editFaction.FactionId, m_shortcut.Text, m_name.Text, m_desc.Text.ToString(), m_privInfo.Text.ToString(), m_factionIconGroupId, m_factionIconId, m_factionColor, m_factionIconColor);
				CloseScreenNow();
				return;
			}
			MySession.Static.Factions.CreateFaction(MySession.Static.LocalPlayerId, m_shortcut.Text, m_name.Text, m_desc.Text.ToString(), m_privInfo.Text.ToString(), MyFactionTypes.PlayerMade, factionIconGroupId: m_factionIconGroupId, factionIconId: m_factionIconId, factionColor: m_factionColor, factionIconColor: m_factionIconColor);
			CloseScreenNow();
		}

		protected void OnCancelClick(MyGuiControlButton sender)
		{
			CloseScreenNow();
		}

		protected void ShowErrorBox(StringBuilder text)
		{
			StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, messageCaption);
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.CloseBeforeCallback = true;
			myGuiScreenMessageBox.CanHideOthers = false;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}
	}
}
