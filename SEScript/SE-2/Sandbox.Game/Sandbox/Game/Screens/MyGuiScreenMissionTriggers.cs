using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Triggers;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenMissionTriggers : MyGuiScreenBase
	{
		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlLabel m_videoLabel;

		protected MyGuiControlTextbox m_videoTextbox;

		private MyGuiControlCombobox[] m_winCombo = new MyGuiControlCombobox[6];

		private MyGuiControlCombobox[] m_loseCombo = new MyGuiControlCombobox[6];

		private MyTrigger[] m_winTrigger = new MyTrigger[6];

		private MyGuiControlButton[] m_winButton = new MyGuiControlButton[6];

		private MyTrigger[] m_loseTrigger = new MyTrigger[6];

		private MyGuiControlButton[] m_loseButton = new MyGuiControlButton[6];

		private MyGuiScreenAdvancedScenarioSettings m_advanced;

		private static List<Type> m_triggerTypes;

		static MyGuiScreenMissionTriggers()
		{
			m_triggerTypes = GetTriggerTypes();
		}

		public MyGuiScreenMissionTriggers()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8f, 0.8f))
		{
			RecreateControls(constructor: true);
		}

		public static List<Type> GetTriggerTypes()
		{
			return (from type in Assembly.GetCallingAssembly().GetTypes()
				where type.IsSubclassOf(typeof(MyTrigger)) && (MyFakes.ENABLE_NEW_TRIGGERS || (type != typeof(MyTriggerTimeLimit) && type != typeof(MyTriggerBlockDestroyed)))
				select type).ToList();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			AddCaption(MySpaceTexts.MissionScreenCaption);
			AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK, new Vector2(0f, 0.08f), new Vector2(0.75f, 0.45f), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_okButton = new MyGuiControlButton(new Vector2(0.17f, 0.37f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Refresh), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkButtonClick);
			m_cancelButton = new MyGuiControlButton(new Vector2(0.38f, 0.37f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelButtonClick);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_videoLabel = new MyGuiControlLabel(new Vector2(-0.375f, -0.18f), null, MyTexts.Get(MySpaceTexts.GuiLabelVideoOnStart).ToString());
			m_videoTextbox = new MyGuiControlTextbox(m_videoLabel.Position, MySession.Static.BriefingVideo, 85);
			Controls.Add(m_videoLabel);
			Controls.Add(m_videoTextbox);
			m_videoTextbox.PositionX = m_videoLabel.Position.X + m_videoLabel.Size.X + m_videoTextbox.Size.X / 2f + 0.03f;
			m_videoTextbox.TextChanged += OnVideoTextboxChanged;
			OnVideoTextboxChanged(m_videoTextbox);
			bACK_BUTTON_SIZE = new Vector2(0.05f, 0.05f);
			Vector2 value = new Vector2(0.15f, -0.05f);
			MyGuiControlLabel control = new MyGuiControlLabel(new Vector2(value.X - 0.37f, value.Y - 0.06f), new Vector2(455f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, MyTexts.Get(MySpaceTexts.GuiMissionTriggersWinCondition).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(control);
			MyGuiControlLabel control2 = new MyGuiControlLabel(new Vector2(value.X, value.Y - 0.06f), new Vector2(455f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, MyTexts.Get(MySpaceTexts.GuiMissionTriggersLostCondition).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(control2);
			for (int i = 0; i < 6; i++)
			{
				value.X -= 0.37f;
				m_winCombo[i] = new MyGuiControlCombobox(value);
				m_winCombo[i].ItemSelected += OnWinComboSelect;
				m_winCombo[i].AddItem(-1L, "");
				foreach (Type triggerType in m_triggerTypes)
				{
					m_winCombo[i].AddItem(triggerType.GetHashCode(), MyTexts.Get((MyStringId)triggerType.GetMethod("GetCaption").Invoke(null, null)));
				}
				Controls.Add(m_winCombo[i]);
				m_winButton[i] = new MyGuiControlButton(new Vector2(value.X + 0.15f, value.Y), MyGuiControlButtonStyleEnum.Tiny, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder("*"), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnWinEditButtonClick);
				m_winButton[i].Enabled = false;
				Controls.Add(m_winButton[i]);
				value.X += 0.37f;
				m_loseCombo[i] = new MyGuiControlCombobox(value);
				m_loseCombo[i].ItemSelected += OnLoseComboSelect;
				m_loseCombo[i].AddItem(-1L, "");
				foreach (Type triggerType2 in m_triggerTypes)
				{
					triggerType2.GetMethod("GetFriendlyName");
					m_loseCombo[i].AddItem(triggerType2.GetHashCode(), MyTexts.Get((MyStringId)triggerType2.GetMethod("GetCaption").Invoke(null, null)));
				}
				Controls.Add(m_loseCombo[i]);
				m_loseButton[i] = new MyGuiControlButton(new Vector2(value.X + 0.15f, value.Y), MyGuiControlButtonStyleEnum.Tiny, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder("*"), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnLoseEditButtonClick);
				m_loseButton[i].Enabled = false;
				Controls.Add(m_loseButton[i]);
				value.Y += 0.05f;
			}
			SetDefaultValues();
		}

		private void OnWinComboSelect()
		{
			for (int i = 0; i < 6; i++)
			{
				if (m_winTrigger[i] == null && m_winCombo[i].GetSelectedKey() != -1)
				{
					m_winTrigger[i] = CreateNew(m_winCombo[i].GetSelectedKey());
				}
				else if (m_winTrigger[i] != null && m_winCombo[i].GetSelectedKey() == -1)
				{
					m_winTrigger[i] = null;
				}
				else if (m_winTrigger[i] != null && m_winCombo[i].GetSelectedKey() != m_winTrigger[i].GetType().GetHashCode())
				{
					m_winTrigger[i] = CreateNew(m_winCombo[i].GetSelectedKey());
				}
				m_winButton[i].Enabled = (m_winCombo[i].GetSelectedKey() != -1);
			}
		}

		private void OnLoseComboSelect()
		{
			for (int i = 0; i < 6; i++)
			{
				if (m_loseTrigger[i] == null && m_loseCombo[i].GetSelectedKey() != -1)
				{
					m_loseTrigger[i] = CreateNew(m_loseCombo[i].GetSelectedKey());
				}
				else if (m_loseTrigger[i] != null && m_loseCombo[i].GetSelectedKey() == -1)
				{
					m_loseTrigger[i] = null;
				}
				else if (m_loseTrigger[i] != null && m_loseCombo[i].GetSelectedKey() != m_loseTrigger[i].GetType().GetHashCode())
				{
					m_loseTrigger[i] = CreateNew(m_loseCombo[i].GetSelectedKey());
				}
				m_loseButton[i].Enabled = (m_loseCombo[i].GetSelectedKey() != -1);
			}
		}

		private MyTrigger CreateNew(long hash)
		{
			foreach (Type triggerType in m_triggerTypes)
			{
				if (triggerType.GetHashCode() == hash)
				{
					return (MyTrigger)Activator.CreateInstance(triggerType);
				}
			}
			return null;
		}

		private void SetDefaultValues()
		{
			if (!MySessionComponentMissionTriggers.Static.MissionTriggers.TryGetValue(MyMissionTriggers.DefaultPlayerId, out MyMissionTriggers value))
			{
				value = new MyMissionTriggers();
				MySessionComponentMissionTriggers.Static.MissionTriggers.Add(MyMissionTriggers.DefaultPlayerId, value);
				return;
			}
			int num = 0;
			foreach (MyTrigger winTrigger in value.WinTriggers)
			{
				for (int i = 0; i < m_winCombo[num].GetItemsCount(); i++)
				{
					if (m_winCombo[num].GetItemByIndex(i).Key == winTrigger.GetType().GetHashCode())
					{
						m_winCombo[num].ItemSelected -= OnWinComboSelect;
						m_winCombo[num].SelectItemByIndex(i);
						m_winCombo[num].ItemSelected += OnWinComboSelect;
						m_winTrigger[num] = (MyTrigger)winTrigger.Clone();
						m_winButton[num].Enabled = true;
						break;
					}
					m_winButton[num].Enabled = false;
				}
				num++;
			}
			num = 0;
			foreach (MyTrigger loseTrigger in value.LoseTriggers)
			{
				for (int j = 0; j < m_loseCombo[num].GetItemsCount(); j++)
				{
					if (m_loseCombo[num].GetItemByIndex(j).Key == loseTrigger.GetType().GetHashCode())
					{
						m_loseCombo[num].ItemSelected -= OnLoseComboSelect;
						m_loseCombo[num].SelectItemByIndex(j);
						m_loseCombo[num].ItemSelected += OnLoseComboSelect;
						m_loseTrigger[num] = (MyTrigger)loseTrigger.Clone();
						m_loseButton[num].Enabled = true;
						break;
					}
					m_loseButton[num].Enabled = false;
				}
				num++;
			}
		}

		protected MyGuiControlCompositePanel AddCompositePanel(MyGuiCompositeTexture texture, Vector2 position, Vector2 size, MyGuiDrawAlignEnum panelAlign)
		{
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = texture
			};
			myGuiControlCompositePanel.Position = position;
			myGuiControlCompositePanel.Size = size;
			myGuiControlCompositePanel.OriginAlign = panelAlign;
			Controls.Add(myGuiControlCompositePanel);
			return myGuiControlCompositePanel;
		}

		private int getButtonNr(object sender)
		{
			for (int i = 0; i < 6; i++)
			{
				if (sender == m_winButton[i] || sender == m_loseButton[i])
				{
					return i;
				}
			}
			return -1;
		}

		private void OnWinEditButtonClick(object sender)
		{
			m_winTrigger[getButtonNr(sender)].DisplayGUI();
		}

		private void OnLoseEditButtonClick(object sender)
		{
			m_loseTrigger[getButtonNr(sender)].DisplayGUI();
		}

		private void OnOkButtonClick(object sender)
		{
			SaveData();
			CloseScreen();
		}

		private void OnCancelButtonClick(object sender)
		{
			CloseScreen();
		}

		private void OnAdvancedButtonClick(object sender)
		{
			m_advanced = new MyGuiScreenAdvancedScenarioSettings(this);
			MyGuiSandbox.AddScreen(m_advanced);
		}

		public override bool CloseScreen()
		{
			m_videoTextbox.TextChanged -= OnVideoTextboxChanged;
			return base.CloseScreen();
		}

		private void SaveData()
		{
			MySession.Static.BriefingVideo = m_videoTextbox.Text;
			foreach (KeyValuePair<MyPlayer.PlayerId, MyMissionTriggers> missionTrigger in MySessionComponentMissionTriggers.Static.MissionTriggers)
			{
				missionTrigger.Value.HideNotification();
			}
			MySessionComponentMissionTriggers.Static.MissionTriggers.Clear();
			MyMissionTriggers myMissionTriggers = new MyMissionTriggers();
			MySessionComponentMissionTriggers.Static.MissionTriggers.Add(MyMissionTriggers.DefaultPlayerId, myMissionTriggers);
			for (int i = 0; i < 6; i++)
			{
				if (m_winTrigger[i] != null)
				{
					myMissionTriggers.WinTriggers.Add(m_winTrigger[i]);
				}
				if (m_loseTrigger[i] != null)
				{
					myMissionTriggers.LoseTriggers.Add(m_loseTrigger[i]);
				}
			}
		}

		private void OnVideoTextboxChanged(MyGuiControlTextbox source)
		{
			if (source.Text.Length == 0 || MyGuiSandbox.IsUrlWhitelisted(source.Text))
			{
				source.SetToolTip((MyToolTips)null);
				source.ColorMask = Vector4.One;
				m_okButton.Enabled = true;
			}
			else
			{
				source.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.WwwLinkNotAllowed), MyGameService.Service.ServiceName));
				source.ColorMask = Color.Red.ToVector4();
				m_okButton.Enabled = false;
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMissionTriggers";
		}
	}
}
