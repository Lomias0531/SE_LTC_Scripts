using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenWorkshopTags : MyGuiScreenBase
	{
		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private static List<MyGuiControlCheckbox> m_checkboxes = new List<MyGuiControlCheckbox>();

		private Action<MyGuiScreenMessageBox.ResultEnum, string[]> m_callback;

		private string m_typeTag;

		private static MyGuiScreenWorkshopTags Static;

		private const int TAGS_MAX_LENGTH = 128;

		private static Dictionary<string, MyStringId> m_activeTags;

		public MyGuiScreenWorkshopTags(string typeTag, MyWorkshop.Category[] categories, string[] tags, Action<MyGuiScreenMessageBox.ResultEnum, string[]> callback)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.596374035f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			Static = this;
			m_typeTag = (typeTag ?? "");
			m_activeTags = new Dictionary<string, MyStringId>(categories.Length);
			for (int i = 0; i < categories.Length; i++)
			{
				MyWorkshop.Category category = categories[i];
				m_activeTags.Add(category.Id, category.LocalizableName);
			}
			m_callback = callback;
			base.EnabledBackgroundFade = true;
			base.CanHideOthers = true;
			RecreateControls(constructor: true);
			SetSelectedTags(tags);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionWorkshopTags, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			Vector2 vector = new Vector2(-0.125f, -0.2f);
			Vector2 vector2 = new Vector2(0f, 0.05f);
			m_checkboxes.Clear();
			int num = 0;
			foreach (KeyValuePair<string, MyStringId> activeTag in m_activeTags)
			{
				num++;
				if (num == 8)
				{
					vector = new Vector2(0.125f, -0.2f);
				}
				AddLabeledCheckbox(vector += vector2, activeTag.Key, activeTag.Value);
				if (m_typeTag == "mod")
				{
					string str = activeTag.Key.Replace(" ", string.Empty);
					string text = Path.Combine(MyFileSystem.ContentPath, "Textures", "GUI", "Icons", "buttons", str + ".dds");
					if (File.Exists(text))
					{
						AddIcon(vector + new Vector2(-0.05f, 0f), text, new Vector2(0.04f, 0.05f));
					}
				}
			}
			vector += vector2;
			Controls.Add(m_okButton = MakeButton(vector += vector2, MyCommonTexts.Ok, MySpaceTexts.ToolTipNewsletter_Ok, OnOkClick, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
			Controls.Add(m_cancelButton = MakeButton(vector, MyCommonTexts.Cancel, MySpaceTexts.ToolTipOptionsSpace_Cancel, OnCancelClick, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			Vector2 value = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 value2 = (m_size.Value / 2f - value) * new Vector2(0f, 1f);
			float num2 = 25f;
			m_okButton.Position = value2 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_cancelButton.Position = value2 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			base.CloseButtonEnabled = true;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWorkshopTags";
		}

		private MyGuiControlCheckbox AddLabeledCheckbox(Vector2 position, string tag, MyStringId text)
		{
			MyGuiControlLabel control = MakeLabel(position, text);
			MyGuiControlCheckbox myGuiControlCheckbox = MakeCheckbox(position, text);
			Controls.Add(control);
			Controls.Add(myGuiControlCheckbox);
			myGuiControlCheckbox.UserData = tag;
			m_checkboxes.Add(myGuiControlCheckbox);
			return myGuiControlCheckbox;
		}

		private MyGuiControlImage AddIcon(Vector2 position, string texture, Vector2 size)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Position = position,
				Size = size
			};
			myGuiControlImage.SetTexture(texture);
			Controls.Add(myGuiControlImage);
			return myGuiControlImage;
		}

		private MyGuiControlLabel MakeLabel(Vector2 position, MyStringId text)
		{
			return new MyGuiControlLabel(position, null, MyTexts.GetString(text));
		}

		private MyGuiControlCheckbox MakeCheckbox(Vector2 position, MyStringId tooltip)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(position, null, MyTexts.GetString(tooltip), isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnCheckboxChanged));
			return myGuiControlCheckbox;
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			string @string = MyTexts.GetString(toolTip);
			return new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Default, null, null, originAlign, @string, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		private static void OnCheckboxChanged(MyGuiControlCheckbox obj)
		{
			if (obj != null && obj.IsChecked && Static.GetSelectedTagsLength() >= 128)
			{
				obj.IsChecked = false;
			}
		}

		private void OnOkClick(MyGuiControlButton obj)
		{
			CloseScreen();
			m_callback(MyGuiScreenMessageBox.ResultEnum.YES, GetSelectedTags());
		}

		private void OnCancelClick(MyGuiControlButton obj)
		{
			CloseScreen();
			m_callback(MyGuiScreenMessageBox.ResultEnum.CANCEL, GetSelectedTags());
		}

		protected override void Canceling()
		{
			base.Canceling();
			m_callback(MyGuiScreenMessageBox.ResultEnum.CANCEL, GetSelectedTags());
		}

		public int GetSelectedTagsLength()
		{
			int num = m_typeTag.Length;
			foreach (MyGuiControlCheckbox checkbox in m_checkboxes)
			{
				if (checkbox.IsChecked)
				{
					num += ((string)checkbox.UserData).Length;
				}
			}
			return num;
		}

		public string[] GetSelectedTags()
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(m_typeTag))
			{
				list.Add(m_typeTag);
			}
			foreach (MyGuiControlCheckbox checkbox in m_checkboxes)
			{
				if (checkbox.IsChecked)
				{
					list.Add((string)checkbox.UserData);
				}
			}
			return list.ToArray();
		}

		public void SetSelectedTags(string[] tags)
		{
			if (tags != null)
			{
				foreach (string text in tags)
				{
					foreach (MyGuiControlCheckbox checkbox in m_checkboxes)
					{
						if (text.Equals((string)checkbox.UserData, StringComparison.InvariantCultureIgnoreCase))
						{
							checkbox.IsChecked = true;
						}
					}
				}
			}
		}
	}
}
