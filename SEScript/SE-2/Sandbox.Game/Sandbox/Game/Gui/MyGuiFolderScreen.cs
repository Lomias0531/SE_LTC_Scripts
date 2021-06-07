using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiFolderScreen : MyGuiScreenBase
	{
		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 0.6f);

		private Action<bool, string> m_onFinishedAction;

		private Func<string, bool> m_isItem;

		private string m_rootPath = string.Empty;

		private string m_pathLocalInitial = string.Empty;

		private string m_pathLocalCurrent = string.Empty;

		private MyGuiControlLabel m_pathLabel;

		private MyGuiControlListbox m_fileList;

		private MyGuiControlButton m_buttonOk;

		private MyGuiControlButton m_buttonRefresh;

		public MyGuiFolderScreen(bool hideOthers, Action<bool, string> OnFinished, string rootPath, string localPath, Func<string, bool> isItem = null)
			: base(new Vector2(0.5f, 0.5f), size: SCREEN_SIZE, backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity)
		{
			if (OnFinished == null)
			{
				CloseScreen();
			}
			base.CanHideOthers = hideOthers;
			m_onFinishedAction = OnFinished;
			m_rootPath = rootPath;
			m_pathLocalCurrent = (m_pathLocalInitial = localPath);
			if (isItem != null)
			{
				m_isItem = isItem;
			}
			else
			{
				m_isItem = IsItem_Default;
			}
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 position = new Vector2(0f, 0.23f);
			Vector2 position2 = new Vector2(0.15f, 0.23f);
			Vector2 position3 = new Vector2(0f, 0.02f);
			Vector2 value = new Vector2(-0.143f, -0.2f);
			Vector2 size = new Vector2(0.143f, 0.035f);
			Vector2 size2 = new Vector2(0.026f, 0.035f);
			Vector2 size3 = new Vector2(0.32f, 0.38f);
			Vector2 value2 = new Vector2(0.5f, 0.5f);
			m_fileList = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.Blueprints);
			m_fileList.Position = position3;
			m_fileList.Size = size3;
			m_fileList.ItemDoubleClicked += OnItemDoubleClick;
			m_fileList.ItemClicked += OnItemClick;
			m_fileList.VisibleRowsCount = 11;
			Controls.Add(m_fileList);
			AddCaption(MySpaceTexts.ScreenFolders_Caption);
			m_buttonOk = CreateButton(size, MyTexts.Get(MySpaceTexts.ScreenFolders_ButOpen), OnOk, enabled: true, MySpaceTexts.ScreenFolders_Tooltip_Open);
			m_buttonOk.Position = position;
			m_buttonOk.ShowTooltipWhenDisabled = true;
			m_buttonRefresh = CreateButton(size2, null, OnRefresh, enabled: true, MySpaceTexts.ScreenFolders_Tooltip_Refresh);
			m_buttonRefresh.Position = position2;
			m_buttonRefresh.ShowTooltipWhenDisabled = true;
			m_pathLabel = new MyGuiControlLabel(value, value2);
			Controls.Add(m_pathLabel);
			UpdatePathLabel();
			CreateButtonIcon(m_buttonRefresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			base.CloseButtonEnabled = true;
			RepopulateList();
		}

		public void UpdatePathLabel()
		{
			string text = "./" + BuildNewPath();
			if (text.Length > 40)
			{
				m_pathLabel.Text = text.Substring(text.Length - 41, 40);
			}
			else
			{
				m_pathLabel.Text = text;
			}
		}

		private static bool IsItem_Default(string path)
		{
			return false;
		}

		private void RepopulateList()
		{
			m_fileList.Items.Clear();
			List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			string path = Path.Combine(m_rootPath, m_pathLocalCurrent);
			if (!Directory.Exists(path))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(path);
			List<string> list3 = new List<string>();
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1]
				{
					'\\'
				});
				list3.Add(array2[array2.Length - 1]);
			}
			for (int j = 0; j < list3.Count; j++)
			{
				if (m_isItem(directories[j]))
				{
					MyFileItem myFileItem = new MyFileItem
					{
						Type = MyFileItemType.File,
						Name = list3[j],
						Path = directories[j]
					};
					string normal = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal;
					StringBuilder text = new StringBuilder(list3[j]);
					string toolTip = directories[j];
					object userData = myFileItem;
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text, toolTip, normal, userData);
					list2.Add(item);
				}
				else
				{
					MyFileItem myFileItem = new MyFileItem
					{
						Type = MyFileItemType.Directory,
						Name = list3[j],
						Path = directories[j]
					};
					string normal = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal;
					StringBuilder text2 = new StringBuilder(list3[j]);
					string toolTip2 = directories[j];
					object userData = myFileItem;
					MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(text2, toolTip2, normal, userData);
					list.Add(item2);
				}
			}
			if (!string.IsNullOrEmpty(m_pathLocalCurrent))
			{
				MyFileItem myFileItem2 = new MyFileItem
				{
					Type = MyFileItemType.Directory,
					Name = string.Empty,
					Path = string.Empty
				};
				StringBuilder text3 = new StringBuilder("[..]");
				string pathLocalCurrent = m_pathLocalCurrent;
				object userData = myFileItem2;
				MyGuiControlListbox.Item item3 = new MyGuiControlListbox.Item(text3, pathLocalCurrent, MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal, userData);
				m_fileList.Add(item3);
			}
			foreach (MyGuiControlListbox.Item item4 in list)
			{
				m_fileList.Add(item4);
			}
			foreach (MyGuiControlListbox.Item item5 in list2)
			{
				m_fileList.Add(item5);
			}
			UpdatePathLabel();
		}

		protected MyGuiControlButton CreateButton(Vector2 size, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			myGuiControlButton.Size = size;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			Controls.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		private MyGuiControlImage CreateButtonIcon(MyGuiControlButton butt, string texture)
		{
			float num = 0.95f * Math.Min(butt.Size.X, butt.Size.Y);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(size: new Vector2(num * 0.75f, num), position: butt.Position + new Vector2(-0.0016f, 0.015f), backgroundColor: null, backgroundTexture: null, textures: new string[1]
			{
				texture
			});
			Controls.Add(myGuiControlImage);
			return myGuiControlImage;
		}

		public string BuildNewPath()
		{
			string text = "";
			if (m_fileList.SelectedItems.Count != 1)
			{
				return m_pathLocalCurrent;
			}
			MyFileItem myFileItem = (MyFileItem)m_fileList.SelectedItems[0].UserData;
			if (myFileItem.Type != MyFileItemType.Directory)
			{
				return m_pathLocalCurrent;
			}
			if (string.IsNullOrEmpty(myFileItem.Path))
			{
				string[] array = m_pathLocalCurrent.Split(new char[1]
				{
					Path.DirectorySeparatorChar
				});
				if (array.Length > 1)
				{
					array[array.Length - 1] = string.Empty;
					return Path.Combine(array);
				}
				return string.Empty;
			}
			return Path.Combine(m_pathLocalCurrent, myFileItem.Name);
		}

		private void OnItemClick(MyGuiControlListbox list)
		{
			_ = list.SelectedItems[0];
			UpdatePathLabel();
		}

		private void OnItemDoubleClick(MyGuiControlListbox list)
		{
			_ = list.SelectedItems[0];
			m_pathLocalCurrent = BuildNewPath();
			RepopulateList();
		}

		private void OnRefresh(MyGuiControlButton button)
		{
			RecreateControls(constructor: false);
		}

		private void OnOk(MyGuiControlButton button)
		{
			m_onFinishedAction(arg1: true, BuildNewPath());
			CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiFolderScreen";
		}
	}
}
