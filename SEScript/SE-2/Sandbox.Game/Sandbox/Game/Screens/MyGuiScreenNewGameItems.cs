using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenNewGameItems : MyGuiScreenBase
	{
		private List<MyGameInventoryItem> items;

		private MyGuiControlLabel itemName;

		private MyGuiControlImage itemBackground;

		private MyGuiControlImage itemImage;

		public MyGuiScreenNewGameItems(List<MyGameInventoryItem> newItems)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.41f, 0.4f), isTopMostScreen: true)
		{
			items = newItems;
			MyCueId cueId = MySoundPair.GetCueId("ArcNewItemImpact");
			MyAudio.Static.PlaySound(cueId);
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionClaimGameItem, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.74f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.74f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0f, -0.096f), null, MyTexts.GetString(MyCommonTexts.ScreenCaptionNewItem), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel.Font = "White";
			Elements.Add(myGuiControlLabel);
			itemName = new MyGuiControlLabel(new Vector2(0f, 0.03f), null, "Item Name", Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			itemName.Font = "Blue";
			Controls.Add(itemName);
			itemBackground = new MyGuiControlImage(size: new Vector2(0.07f, 0.09f), position: new Vector2(0f, -0.025f), backgroundColor: null, backgroundTexture: null, textures: new string[1]
			{
				"Textures\\GUI\\blank.dds"
			});
			itemBackground.Margin = new Thickness(0.005f);
			Controls.Add(itemBackground);
			itemImage = new MyGuiControlImage(size: new Vector2(0.06f, 0.08f), position: new Vector2(0f, -0.025f));
			itemImage.Margin = new Thickness(0.005f);
			Controls.Add(itemImage);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(0f, 0.085f), null, MyTexts.GetString(MyCommonTexts.ScreenNewItemVisit), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel2.Font = "White";
			Elements.Add(myGuiControlLabel2);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(0f, 0.168f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok));
			myGuiControlButton.ButtonClicked += OnOkButtonClick;
			Controls.Add(myGuiControlButton);
			LoadFirstItem();
		}

		private void LoadFirstItem()
		{
			MyGameInventoryItem myGameInventoryItem = items.FirstOrDefault();
			if (myGameInventoryItem != null)
			{
				itemName.Text = myGameInventoryItem.ItemDefinition.Name;
				itemBackground.ColorMask = (string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(myGameInventoryItem.ItemDefinition.BackgroundColor));
				string[] array = new string[1]
				{
					"Textures\\GUI\\Blank.dds"
				};
				if (!string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.IconTexture))
				{
					array[0] = myGameInventoryItem.ItemDefinition.IconTexture;
				}
				itemImage.SetTextures(array);
			}
		}

		private void OnOkButtonClick(MyGuiControlButton obj)
		{
			if (items.Count() > 1)
			{
				items.RemoveAt(0);
				LoadFirstItem();
			}
			else
			{
				CloseScreen();
			}
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenNewGameItems";
		}
	}
}
