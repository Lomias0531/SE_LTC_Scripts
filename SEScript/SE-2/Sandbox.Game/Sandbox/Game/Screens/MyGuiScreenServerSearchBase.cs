using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenServerSearchBase : MyGuiScreenBase
	{
		protected enum SearchPageEnum
		{
			Settings,
			Advanced,
			Mods
		}

		private static List<MyWorkshopItem> m_subscribedMods;

		private static List<MyWorkshopItem> m_settingsMods;

		private static bool m_needsModRefresh = true;

		protected SearchPageEnum CurrentPage;

		protected Vector2 CurrentPosition;

		protected MyGuiScreenJoinGame JoinScreen;

		protected float Padding = 0.02f;

		protected MyGuiControlScrollablePanel Panel;

		protected MyGuiControlParent Parent;

		protected MyGuiControlCheckbox m_advancedCheckbox;

		protected MyGuiControlButton m_searchButton;

		protected MyGuiControlButton m_settingsButton;

		protected MyGuiControlButton m_advancedButton;

		protected MyGuiControlButton m_modsButton;

		private MyGuiControlRotatingWheel m_loadingWheel;

		protected bool EnableAdvanced
		{
			get
			{
				if (FilterOptions.AdvancedFilter)
				{
					return JoinScreen.EnableAdvancedSearch;
				}
				return false;
			}
		}

		protected Vector2 WindowSize => new Vector2(base.Size.Value.X - 0.1f, base.Size.Value.Y - m_settingsButton.Size.Y * 2f - Padding * 16f);

		protected MyServerFilterOptions FilterOptions
		{
			get
			{
				return JoinScreen.FilterOptions;
			}
			set
			{
				JoinScreen.FilterOptions = value;
			}
		}

		public MyGuiScreenServerSearchBase(MyGuiScreenJoinGame joinScreen)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9398855f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			JoinScreen = joinScreen;
			CreateScreen();
		}

		private void CreateScreen()
		{
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ServerSearch, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.15f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList3);
			CurrentPosition = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f - 0.003f, m_size.Value.Y / 2f - 0.095f);
			float y = CurrentPosition.Y;
			m_settingsButton = AddButton(MyCommonTexts.ServerDetails_Settings, SettingsButtonClick, null, enabled: true, addToParent: false);
			m_settingsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Settings));
			CurrentPosition.Y = y;
			CurrentPosition.X += m_settingsButton.Size.X + Padding / 3.6f;
			m_advancedButton = AddButton(MyCommonTexts.Advanced, AdvancedButtonClick, null, enabled: true, addToParent: false);
			m_advancedButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Advanced));
			CurrentPosition.Y = y;
			CurrentPosition.X += m_settingsButton.Size.X + Padding / 3.6f;
			m_modsButton = AddButton(MyCommonTexts.WorldSettings_Mods, ModsButtonClick, null, enabled: true, addToParent: false);
			m_modsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Mods));
			CurrentPosition.Y = y;
			CurrentPosition.X += m_settingsButton.Size.X + Padding;
			m_loadingWheel = new MyGuiControlRotatingWheel(m_modsButton.Position + new Vector2(0.137f, -0.004f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(-0.003f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerSearch_Defaults));
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Defaults));
			myGuiControlButton.ButtonClicked += DefaultSettingsClick;
			myGuiControlButton.ButtonClicked += DefaultModsClick;
			Controls.Add(myGuiControlButton);
			m_searchButton = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(0.18f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenMods_SearchLabel));
			m_searchButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Search));
			m_searchButton.ButtonClicked += SearchClick;
			Controls.Add(m_searchButton);
			CurrentPosition = -WindowSize / 2f;
			switch (CurrentPage)
			{
			case SearchPageEnum.Settings:
				base.FocusedControl = m_settingsButton;
				m_settingsButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_settingsButton.HasHighlight = true;
				m_settingsButton.Selected = true;
				CurrentPosition.Y += Padding * 2f;
				DrawSettingsSelector();
				DrawTopControls();
				DrawMidControls();
				break;
			case SearchPageEnum.Mods:
				base.FocusedControl = m_modsButton;
				m_modsButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_modsButton.HasHighlight = true;
				m_modsButton.Selected = true;
				DrawModSelector();
				break;
			case SearchPageEnum.Advanced:
				base.FocusedControl = m_advancedButton;
				m_advancedButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_advancedButton.HasHighlight = true;
				m_advancedButton.Selected = true;
				DrawAdvancedSelector();
				DrawBottomControls();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void DefaultSettingsClick(MyGuiControlButton myGuiControlButton)
		{
			FilterOptions.SetDefaults();
			RecreateControls(constructor: false);
		}

		private void DefaultModsClick(MyGuiControlButton myGuiControlButton)
		{
			FilterOptions.Mods.Clear();
			RecreateControls(constructor: false);
		}

		private void CancelButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void ModsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = SearchPageEnum.Mods;
			if (m_needsModRefresh)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, LoadModsBeginAction, LoadModsEndAction));
				m_needsModRefresh = false;
			}
			else
			{
				RecreateControls(constructor: false);
				m_loadingWheel.Visible = false;
			}
			base.FocusedControl = m_modsButton;
		}

		private void SettingsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = SearchPageEnum.Settings;
			RecreateControls(constructor: false);
			base.FocusedControl = m_settingsButton;
		}

		private void AdvancedButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = SearchPageEnum.Advanced;
			RecreateControls(constructor: false);
			base.FocusedControl = m_advancedButton;
		}

		private void DrawModSelector()
		{
			Parent = new MyGuiControlParent();
			Panel = new MyGuiControlScrollablePanel(Parent);
			Panel.ScrollbarVEnabled = true;
			Panel.PositionX += 0.0075f;
			Panel.PositionY += m_settingsButton.Size.Y / 2f + Padding * 1.7f;
			Panel.Size = new Vector2(base.Size.Value.X - 0.1f, base.Size.Value.Y - m_settingsButton.Size.Y * 2f - Padding * 13.7f);
			Controls.Add(Panel);
			m_advancedCheckbox = new MyGuiControlCheckbox(new Vector2(-0.0435f, -0.279f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvancedTooltip));
			m_advancedCheckbox.IsChecked = FilterOptions.AdvancedFilter;
			MyGuiControlCheckbox advancedCheckbox = m_advancedCheckbox;
			advancedCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(advancedCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.AdvancedFilter = c.IsChecked;
				RecreateControls(constructor: false);
			});
			m_advancedCheckbox.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(m_advancedCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_advancedCheckbox.Position - new Vector2(m_advancedCheckbox.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvanced));
			myGuiControlLabel.SetToolTip(MyCommonTexts.ServerSearch_EnableAdvancedTooltip);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlLabel);
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(new Vector2(0.246499985f, -0.279f));
			myGuiControlCheckbox.IsChecked = FilterOptions.ModsExclusive;
			myGuiControlCheckbox.SetToolTip(MyCommonTexts.ServerSearch_ExclusiveTooltip);
			myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.ModsExclusive = c.IsChecked;
			};
			myGuiControlCheckbox.Enabled = true;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(myGuiControlCheckbox.Position - new Vector2(myGuiControlCheckbox.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_Exclusive));
			myGuiControlLabel2.SetToolTip(MyCommonTexts.ServerSearch_ExclusiveTooltip);
			myGuiControlLabel2.Enabled = true;
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			Controls.Add(myGuiControlCheckbox);
			Controls.Add(myGuiControlLabel2);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.23f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerSearch_Clear));
			MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(CurrentPosition);
			float num = myGuiControlCheckbox2.Size.Y * (float)(m_subscribedMods.Count + m_settingsMods.Count) + myGuiControlButton.Size.Y / 2f + Padding;
			CurrentPosition = -Panel.Size / 2f;
			CurrentPosition.Y = (0f - num) / 2f + myGuiControlCheckbox2.Size.Y / 2f - 0.005f;
			CurrentPosition.X -= 0.0225f;
			Parent.Size = new Vector2(Panel.Size.X, num);
			m_subscribedMods.Sort((MyWorkshopItem a, MyWorkshopItem b) => a.Title.CompareTo(b.Title));
			m_settingsMods.Sort((MyWorkshopItem a, MyWorkshopItem b) => a.Title.CompareTo(b.Title));
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlButton.ButtonClicked += DefaultModsClick;
			myGuiControlButton.Position = CurrentPosition + new Vector2(Padding, (0f - Padding) * 6f);
			Parent.Controls.Add(myGuiControlButton);
			foreach (MyWorkshopItem mod2 in m_subscribedMods)
			{
				int num2 = Math.Min(mod2.Description.Length, 128);
				int num3 = mod2.Description.IndexOf("\n");
				if (num3 > 0)
				{
					num2 = Math.Min(num2, num3 - 1);
				}
				MyGuiControlCheckbox myGuiControlCheckbox3 = AddCheckbox(mod2.Title, delegate(MyGuiControlCheckbox c)
				{
					ModCheckboxClick(c, mod2.Id);
				}, mod2.Description.Substring(0, num2));
				myGuiControlCheckbox3.IsChecked = FilterOptions.Mods.Contains(mod2.Id);
				myGuiControlCheckbox3.Enabled = FilterOptions.AdvancedFilter;
			}
			foreach (MyWorkshopItem mod in m_settingsMods)
			{
				int num4 = Math.Min(mod.Description.Length, 128);
				int num5 = mod.Description.IndexOf("\n");
				if (num5 > 0)
				{
					num4 = Math.Min(num4, num5 - 1);
				}
				MyGuiControlCheckbox myGuiControlCheckbox4 = AddCheckbox(mod.Title, delegate(MyGuiControlCheckbox c)
				{
					ModCheckboxClick(c, mod.Id);
				}, mod.Description.Substring(0, num4), "DarkBlue", EnableAdvanced);
				myGuiControlCheckbox4.IsChecked = FilterOptions.Mods.Contains(mod.Id);
				myGuiControlCheckbox4.Enabled = FilterOptions.AdvancedFilter;
			}
		}

		private void DrawAdvancedSelector()
		{
			m_advancedCheckbox = new MyGuiControlCheckbox(new Vector2(-0.0435f, -0.279f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvancedTooltip));
			m_advancedCheckbox.IsChecked = FilterOptions.AdvancedFilter;
			MyGuiControlCheckbox advancedCheckbox = m_advancedCheckbox;
			advancedCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(advancedCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.AdvancedFilter = c.IsChecked;
				RecreateControls(constructor: false);
			});
			m_advancedCheckbox.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(m_advancedCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_advancedCheckbox.Position - new Vector2(m_advancedCheckbox.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvanced));
			myGuiControlLabel.SetToolTip(MyCommonTexts.ServerSearch_EnableAdvancedTooltip);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlLabel);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.23f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			CurrentPosition.Y += 0.07f;
		}

		private void DrawSettingsSelector()
		{
			CurrentPosition.Y = -0.279f;
			AddCheckboxDuo(new MyStringId?[2]
			{
				MyCommonTexts.WorldSettings_GameModeCreative,
				MyCommonTexts.WorldSettings_GameModeSurvival
			}, new Action<MyGuiControlCheckbox>[2]
			{
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.CreativeMode = c.IsChecked;
				},
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.SurvivalMode = c.IsChecked;
				}
			}, new MyStringId?[2]
			{
				MySpaceTexts.ToolTipJoinGameServerSearch_Creative,
				MySpaceTexts.ToolTipJoinGameServerSearch_Survival
			}, new bool[2]
			{
				FilterOptions.CreativeMode,
				FilterOptions.SurvivalMode
			});
			AddCheckboxDuo(new MyStringId?[2]
			{
				MyCommonTexts.MultiplayerCompatibleVersions,
				MyCommonTexts.MultiplayerJoinSameGameData
			}, new Action<MyGuiControlCheckbox>[2]
			{
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.SameVersion = c.IsChecked;
				},
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.SameData = c.IsChecked;
				}
			}, new MyStringId?[2]
			{
				MySpaceTexts.ToolTipJoinGameServerSearch_CompatibleVersions,
				MySpaceTexts.ToolTipJoinGameServerSearch_SameGameData
			}, new bool[2]
			{
				FilterOptions.SameVersion,
				FilterOptions.SameData
			});
			Vector2 currentPosition = CurrentPosition;
			AddCheckboxDuo(new string[2]
			{
				MyTexts.GetString(MyCommonTexts.MultiplayerJoinAllowedGroups),
				null
			}, new Action<MyGuiControlCheckbox>[1]
			{
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.AllowedGroups = c.IsChecked;
				}
			}, new string[1]
			{
				string.Format(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_AllowedGroups), MyGameService.Service.ServiceName)
			}, new bool[1]
			{
				FilterOptions.AllowedGroups
			});
			CurrentPosition = currentPosition;
			AddIndeterminateDuo(new MyStringId?[2]
			{
				null,
				MyCommonTexts.MultiplayerJoinHasPassword
			}, new Action<MyGuiControlIndeterminateCheckbox>[2]
			{
				null,
				delegate(MyGuiControlIndeterminateCheckbox c)
				{
					switch (c.State)
					{
					case CheckStateEnum.Checked:
						FilterOptions.HasPassword = true;
						break;
					case CheckStateEnum.Unchecked:
						FilterOptions.HasPassword = false;
						break;
					case CheckStateEnum.Indeterminate:
						FilterOptions.HasPassword = null;
						break;
					}
				}
			}, new MyStringId?[2]
			{
				null,
				MySpaceTexts.ToolTipJoinGameServerSearch_HasPassword
			}, new CheckStateEnum[2]
			{
				CheckStateEnum.Indeterminate,
				(!FilterOptions.HasPassword.HasValue || !FilterOptions.HasPassword.Value) ? (FilterOptions.HasPassword.HasValue ? CheckStateEnum.Unchecked : CheckStateEnum.Indeterminate) : CheckStateEnum.Checked
			});
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.325f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.409f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.657f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
		}

		private void ModCheckboxClick(MyGuiControlCheckbox c, ulong modId)
		{
			if (c.IsChecked)
			{
				FilterOptions.Mods.Add(modId);
			}
			else
			{
				FilterOptions.Mods.Remove(modId);
			}
		}

		protected virtual void DrawTopControls()
		{
			CurrentPosition.Y = -0.0225f;
			AddNumericRangeOption(MyCommonTexts.MultiplayerJoinOnlinePlayers, delegate(SerializableRange r)
			{
				FilterOptions.PlayerCount = r;
			}, FilterOptions.PlayerCount, FilterOptions.CheckPlayer, delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.CheckPlayer = c.IsChecked;
			});
			AddNumericRangeOption(MyCommonTexts.JoinGame_ColumnTitle_Mods, delegate(SerializableRange r)
			{
				FilterOptions.ModCount = r;
			}, FilterOptions.ModCount, FilterOptions.CheckMod, delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.CheckMod = c.IsChecked;
			});
			AddNumericRangeOption(MySpaceTexts.WorldSettings_ViewDistance, delegate(SerializableRange r)
			{
				FilterOptions.ViewDistance = r;
			}, FilterOptions.ViewDistance, FilterOptions.CheckDistance, delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.CheckDistance = c.IsChecked;
			});
		}

		protected virtual void DrawMidControls()
		{
			Vector2 currentPosition = CurrentPosition;
			CurrentPosition.Y += Padding * 1.32f;
			CurrentPosition.X += Padding / 2.4f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(MyCommonTexts.JoinGame_ColumnTitle_Ping));
			myGuiControlLabel.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlLabel);
			CurrentPosition.X += Padding * 2.3f;
			MyGuiControlSlider myGuiControlSlider = new MyGuiControlSlider(CurrentPosition + new Vector2(0.215f, 0f), -1f, 1000f, 0.29f, FilterOptions.Ping, null, string.Empty, 1, 0f);
			myGuiControlSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlSlider.LabelDecimalPlaces = 0;
			myGuiControlSlider.IntValue = true;
			myGuiControlSlider.Size = new Vector2(0.45f - myGuiControlLabel.Size.X, 1f);
			myGuiControlSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Ping));
			myGuiControlSlider.PositionX += myGuiControlSlider.Size.X / 2f;
			myGuiControlSlider.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlSlider);
			CurrentPosition.X += myGuiControlSlider.Size.X / 2f + Padding * 14f;
			MyGuiControlLabel val = new MyGuiControlLabel(CurrentPosition, null, "<" + myGuiControlSlider.Value + "ms", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider x)
			{
				val.Text = "<" + x.Value + "ms";
				FilterOptions.Ping = (int)x.Value;
			});
			val.Enabled = JoinScreen.EnableAdvancedSearch;
			Controls.Add(val);
			CurrentPosition = currentPosition;
			CurrentPosition.Y += 0.04f;
		}

		protected virtual void DrawBottomControls()
		{
		}

		private void SearchClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenServerSearchBase";
		}

		private IMyAsyncResult LoadModsBeginAction()
		{
			return new MyModsLoadListResult(FilterOptions.Mods);
		}

		private void LoadModsEndAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyModsLoadListResult obj = (MyModsLoadListResult)result;
			m_subscribedMods = obj.SubscribedMods;
			m_settingsMods = obj.SetMods;
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			RecreateControls(constructor: false);
		}

		protected MyGuiControlButton AddButton(MyStringId text, Action<MyGuiControlButton> onClick, MyStringId? tooltip = null, bool enabled = true, bool addToParent = true)
		{
			Vector2? position = CurrentPosition;
			StringBuilder text2 = MyTexts.Get(text);
			string toolTip = tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : string.Empty;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.ToolbarButton, null, Color.Yellow.ToVector4(), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, toolTip, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.Enabled = enabled;
			myGuiControlButton.PositionX += myGuiControlButton.Size.X / 2f;
			if (addToParent)
			{
				Controls.Add(myGuiControlButton);
			}
			else
			{
				Controls.Add(myGuiControlButton);
			}
			CurrentPosition.Y += myGuiControlButton.Size.Y + Padding;
			return myGuiControlButton;
		}

		protected void AddNumericRangeOption(MyStringId text, Action<SerializableRange> onEntry, SerializableRange currentRange, bool active, Action<MyGuiControlCheckbox> onEnable, bool enabled = true)
		{
			float x = CurrentPosition.X;
			CurrentPosition.X = (0f - WindowSize.X) / 2f + Padding * 12.6f;
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(CurrentPosition + new Vector2(0f, 0.003f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableNumericTooltip));
			myGuiControlCheckbox.PositionX += myGuiControlCheckbox.Size.X / 2f;
			myGuiControlCheckbox.IsChecked = active;
			myGuiControlCheckbox.Enabled = true;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onEnable);
			Controls.Add(myGuiControlCheckbox);
			CurrentPosition.X += myGuiControlCheckbox.Size.X / 2f + Padding;
			MyGuiControlTextbox minText = new MyGuiControlTextbox(CurrentPosition, currentRange.Min.ToString(), 6, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			minText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			minText.Size = new Vector2(0.12f, minText.Size.Y);
			minText.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_MinimumFilterValue));
			minText.Enabled = myGuiControlCheckbox.IsChecked;
			Controls.Add(minText);
			CurrentPosition.X += minText.Size.X / 1.5f + Padding + 0.028f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(CurrentPosition, null, "-");
			Controls.Add(myGuiControlLabel);
			CurrentPosition.X += myGuiControlLabel.Size.X / 2f + Padding / 2f;
			MyGuiControlTextbox maxText = new MyGuiControlTextbox(CurrentPosition, float.IsInfinity(currentRange.Max) ? "-1" : currentRange.Max.ToString(), 6, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			maxText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			maxText.Size = new Vector2(0.12f, maxText.Size.Y);
			maxText.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_MaximumFilterValue));
			maxText.Enabled = myGuiControlCheckbox.IsChecked;
			Controls.Add(maxText);
			CurrentPosition.X += maxText.Size.X / 1.5f + Padding + 0.01f;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(-0.27f, CurrentPosition.Y), null, MyTexts.GetString(text));
			myGuiControlLabel2.Enabled = true;
			Controls.Add(myGuiControlLabel2);
			CurrentPosition.X = x;
			CurrentPosition.Y += myGuiControlLabel2.Size.Y + Padding;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox c)
			{
				MyGuiControlTextbox myGuiControlTextbox = minText;
				bool enabled2 = maxText.Enabled = c.IsChecked;
				myGuiControlTextbox.Enabled = enabled2;
			});
			if (onEntry != null)
			{
				maxText.TextChanged += delegate
				{
					if (float.TryParse(minText.Text, out float result3) && float.TryParse(maxText.Text, out float result4))
					{
						if (result4 == -1f)
						{
							result4 = float.PositiveInfinity;
						}
						if (result3 < 0f)
						{
							result3 = 0f;
						}
						onEntry(new SerializableRange(result3, result4));
					}
				};
				minText.TextChanged += delegate
				{
					if (float.TryParse(minText.Text, out float result) && float.TryParse(maxText.Text, out float result2))
					{
						if (result2 == -1f)
						{
							result2 = float.PositiveInfinity;
						}
						if (result < 0f)
						{
							result = 0f;
						}
						onEntry(new SerializableRange(result, result2));
					}
				};
			}
		}

		protected MyGuiControlCheckbox AddCheckbox(MyStringId text, Action<MyGuiControlCheckbox> onClick, MyStringId? tooltip = null, string font = null, bool enabled = true)
		{
			return AddCheckbox(MyTexts.GetString(text), onClick, tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : null, font, enabled);
		}

		protected MyGuiControlCheckbox AddCheckbox(string text, Action<MyGuiControlCheckbox> onClick, string tooltip = null, string font = null, bool enabled = true)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(CurrentPosition, null, tooltip ?? string.Empty);
			myGuiControlCheckbox.PositionX += myGuiControlCheckbox.Size.X / 2f + Padding * 26f;
			Parent.Controls.Add(myGuiControlCheckbox);
			if (onClick != null)
			{
				myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onClick);
			}
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(CurrentPosition, null, MyTexts.GetString(text));
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.PositionX = myGuiControlCheckbox.PositionX - Padding * 25.8f;
			if (!string.IsNullOrEmpty(tooltip))
			{
				myGuiControlLabel.SetToolTip(tooltip);
			}
			if (!string.IsNullOrEmpty(font))
			{
				myGuiControlLabel.Font = font;
			}
			Parent.Controls.Add(myGuiControlLabel);
			CurrentPosition.Y += myGuiControlCheckbox.Size.Y;
			myGuiControlCheckbox.Enabled = enabled;
			myGuiControlLabel.Enabled = enabled;
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox[] AddCheckboxDuo(MyStringId?[] text, Action<MyGuiControlCheckbox>[] onClick, MyStringId?[] tooltip, bool[] values)
		{
			string[] array = new string[text.Length];
			string[] array2 = new string[tooltip.Length];
			for (int i = 0; i < text.Length; i++)
			{
				array[i] = (text[i].HasValue ? MyTexts.GetString(text[i].Value) : string.Empty);
			}
			for (int j = 0; j < tooltip.Length; j++)
			{
				array2[j] = (tooltip[j].HasValue ? MyTexts.GetString(tooltip[j].Value) : string.Empty);
			}
			return AddCheckboxDuo(array, onClick, array2, values);
		}

		protected MyGuiControlCheckbox[] AddCheckboxDuo(string[] text, Action<MyGuiControlCheckbox>[] onClick, string[] tooltip, bool[] values)
		{
			MyGuiControlCheckbox[] array = new MyGuiControlCheckbox[2];
			float x = CurrentPosition.X;
			if (!string.IsNullOrEmpty(text[0]))
			{
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(CurrentPosition, null, (!string.IsNullOrEmpty(tooltip[0])) ? MyTexts.GetString(tooltip[0]) : string.Empty);
				myGuiControlCheckbox.PositionX = -0.0435f;
				myGuiControlCheckbox.IsChecked = values[0];
				array[0] = myGuiControlCheckbox;
				if (onClick[0] != null)
				{
					myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onClick[0]);
				}
				CurrentPosition.X = myGuiControlCheckbox.PositionX + myGuiControlCheckbox.Size.X / 2f + Padding / 3f;
				MyGuiControlLabel control = new MyGuiControlLabel(myGuiControlCheckbox.Position - new Vector2(myGuiControlCheckbox.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(text[0]));
				Controls.Add(myGuiControlCheckbox);
				Controls.Add(control);
			}
			if (!string.IsNullOrEmpty(text[1]))
			{
				MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(CurrentPosition, null, (!string.IsNullOrEmpty(tooltip[1])) ? MyTexts.GetString(tooltip[1]) : string.Empty);
				myGuiControlCheckbox2.PositionX = 0.262f;
				myGuiControlCheckbox2.IsChecked = values[1];
				array[1] = myGuiControlCheckbox2;
				if (onClick[1] != null)
				{
					myGuiControlCheckbox2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox2.IsCheckedChanged, onClick[1]);
				}
				CurrentPosition.X = myGuiControlCheckbox2.PositionX + myGuiControlCheckbox2.Size.X / 2f + Padding / 2f;
				MyGuiControlLabel control2 = new MyGuiControlLabel(myGuiControlCheckbox2.Position - new Vector2(myGuiControlCheckbox2.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(text[1]));
				Controls.Add(myGuiControlCheckbox2);
				Controls.Add(control2);
			}
			CurrentPosition.X = x;
			CurrentPosition.Y += array.First((MyGuiControlCheckbox c) => c != null).Size.Y / 2f + Padding + 0.005f;
			return array;
		}

		protected MyGuiControlIndeterminateCheckbox[] AddIndeterminateDuo(MyStringId?[] text, Action<MyGuiControlIndeterminateCheckbox>[] onClick, MyStringId?[] tooltip, CheckStateEnum[] values, bool enabled = true)
		{
			MyGuiControlIndeterminateCheckbox[] array = new MyGuiControlIndeterminateCheckbox[2];
			float x = CurrentPosition.X;
			if (text[0].HasValue)
			{
				MyGuiControlIndeterminateCheckbox myGuiControlIndeterminateCheckbox = new MyGuiControlIndeterminateCheckbox(CurrentPosition, null, tooltip[0].HasValue ? MyTexts.GetString(tooltip[0].Value) : string.Empty);
				myGuiControlIndeterminateCheckbox.PositionX = -0.0435f;
				myGuiControlIndeterminateCheckbox.State = values[0];
				array[0] = myGuiControlIndeterminateCheckbox;
				if (onClick[0] != null)
				{
					myGuiControlIndeterminateCheckbox.IsCheckedChanged = (Action<MyGuiControlIndeterminateCheckbox>)Delegate.Combine(myGuiControlIndeterminateCheckbox.IsCheckedChanged, onClick[0]);
				}
				CurrentPosition.X = myGuiControlIndeterminateCheckbox.PositionX + myGuiControlIndeterminateCheckbox.Size.X / 2f + Padding / 3f;
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(myGuiControlIndeterminateCheckbox.Position - new Vector2(myGuiControlIndeterminateCheckbox.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(text[0].Value));
				myGuiControlIndeterminateCheckbox.Enabled = enabled;
				myGuiControlLabel.Enabled = enabled;
				Controls.Add(myGuiControlIndeterminateCheckbox);
				Controls.Add(myGuiControlLabel);
			}
			if (text[1].HasValue)
			{
				MyGuiControlIndeterminateCheckbox myGuiControlIndeterminateCheckbox2 = new MyGuiControlIndeterminateCheckbox(CurrentPosition, null, tooltip[1].HasValue ? MyTexts.GetString(tooltip[1].Value) : string.Empty);
				myGuiControlIndeterminateCheckbox2.PositionX = 0.262f;
				myGuiControlIndeterminateCheckbox2.State = values[1];
				array[1] = myGuiControlIndeterminateCheckbox2;
				if (onClick[1] != null)
				{
					myGuiControlIndeterminateCheckbox2.IsCheckedChanged = (Action<MyGuiControlIndeterminateCheckbox>)Delegate.Combine(myGuiControlIndeterminateCheckbox2.IsCheckedChanged, onClick[1]);
				}
				CurrentPosition.X = myGuiControlIndeterminateCheckbox2.PositionX + myGuiControlIndeterminateCheckbox2.Size.X / 2f + Padding / 2f;
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(myGuiControlIndeterminateCheckbox2.Position - new Vector2(myGuiControlIndeterminateCheckbox2.Size.X / 2f + Padding * 10.45f, 0f), null, MyTexts.GetString(text[1].Value));
				myGuiControlIndeterminateCheckbox2.Enabled = enabled;
				myGuiControlLabel2.Enabled = enabled;
				Controls.Add(myGuiControlIndeterminateCheckbox2);
				Controls.Add(myGuiControlLabel2);
			}
			CurrentPosition.X = x;
			CurrentPosition.Y += array.First((MyGuiControlIndeterminateCheckbox c) => c != null).Size.Y / 2f + Padding + 0.005f;
			return array;
		}
	}
}
