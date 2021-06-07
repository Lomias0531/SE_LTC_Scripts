using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal abstract class MyGuiDetailScreenBase : MyGuiBlueprintScreenBase
	{
		protected static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		protected float m_textScale;

		protected string m_blueprintName;

		protected MyGuiControlListbox.Item m_selectedItem;

		protected MyObjectBuilder_Definitions m_loadedPrefab;

		protected MyGuiControlMultilineText m_textField;

		protected MyGuiControlMultilineText m_descriptionField;

		protected MyGuiControlImage m_thumbnailImage;

		protected Action<MyGuiControlListbox.Item> callBack;

		protected MyGuiBlueprintScreenBase m_parent;

		protected MyGuiBlueprintTextDialog m_dialog;

		protected bool m_killScreen;

		protected Vector2 m_offset = new Vector2(-0.01f, 0f);

		protected int maxNameLenght = 40;

		public MyGuiDetailScreenBase(bool isTopMostScreen, MyGuiBlueprintScreenBase parent, string thumbnailTexture, MyGuiControlListbox.Item selectedItem, float textScale)
			: base(new Vector2(0.5f, 0.5f), new Vector2(0.778f, 0.594f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen)
		{
			m_thumbnailImage = new MyGuiControlImage
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK
			};
			m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
			m_thumbnailImage.SetTexture(thumbnailTexture);
			m_thumbnailImage.BorderEnabled = true;
			m_thumbnailImage.BorderSize = 1;
			m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
			m_selectedItem = selectedItem;
			m_blueprintName = selectedItem.Text.ToString();
			m_textScale = textScale;
			m_parent = parent;
			base.CloseButtonEnabled = true;
		}

		protected int GetNumberOfBlocks()
		{
			int num = 0;
			MyObjectBuilder_CubeGrid[] cubeGrids = m_loadedPrefab.ShipBlueprints[0].CubeGrids;
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in cubeGrids)
			{
				num += myObjectBuilder_CubeGrid.CubeBlocks.Count;
			}
			return num;
		}

		protected int GetNumberOfBattlePoints()
		{
			return (int)m_loadedPrefab.ShipBlueprints[0].Points;
		}

		protected void RefreshTextField()
		{
			if (m_textField != null)
			{
				string text = m_blueprintName;
				if (text.Length > 25)
				{
					text = text.Substring(0, 25) + "...";
				}
				m_textField.Clear();
				m_textField.AppendText(MyTexts.GetString(MySpaceTexts.BlueprintInfo_Name) + text);
				m_textField.AppendLine();
				MyCubeSize gridSizeEnum = m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].GridSizeEnum;
				m_textField.AppendText(MyTexts.GetString(MyCommonTexts.BlockPropertiesText_Type));
				if (m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].IsStatic && gridSizeEnum == MyCubeSize.Large)
				{
					m_textField.AppendText(MyTexts.GetString(MyCommonTexts.DetailStaticGrid));
				}
				else if (gridSizeEnum == MyCubeSize.Small)
				{
					m_textField.AppendText(MyTexts.GetString(MyCommonTexts.DetailSmallGrid));
				}
				else
				{
					m_textField.AppendText(MyTexts.GetString(MyCommonTexts.DetailLargeGrid));
				}
				m_textField.AppendLine();
				m_textField.AppendText(MyTexts.GetString(MySpaceTexts.BlueprintInfo_NumberOfBlocks) + GetNumberOfBlocks());
				m_textField.AppendLine();
				m_textField.AppendText(MyTexts.GetString(MySpaceTexts.BlueprintInfo_Author) + m_loadedPrefab.ShipBlueprints[0].DisplayName);
				m_textField.AppendLine();
			}
		}

		protected void CreateTextField()
		{
			Vector2 vector = new Vector2(-0.325f, -0.2f) + m_offset;
			Vector2 vector2 = new Vector2(0.175f, 0.175f);
			Vector2 value = new Vector2(0.005f, -0.04f);
			AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER, vector, vector2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_textField = new MyGuiControlMultilineText();
			m_textField = CreateMultilineText(offset: vector + value, textScale: m_textScale, size: vector2 - value);
			RefreshTextField();
		}

		protected void CreateDescription()
		{
			Vector2 vector = new Vector2(-0.325f, -0.005f) + m_offset;
			Vector2 vector2 = new Vector2(0.67f, 0.155f);
			Vector2 vector3 = new Vector2(0.005f, -0.04f);
			AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER, vector, vector2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_descriptionField = CreateMultilineText(offset: vector + vector3, textScale: m_textScale, size: vector2 - (vector3 + new Vector2(0f, 0.045f)));
			RefreshDescriptionField();
		}

		protected void RefreshDescriptionField()
		{
			if (m_descriptionField != null)
			{
				m_descriptionField.Clear();
				string description = m_loadedPrefab.ShipBlueprints[0].Description;
				if (description != null)
				{
					m_descriptionField.AppendText(description);
				}
			}
		}

		protected MyGuiControlMultilineText CreateMultilineText(Vector2? size = null, Vector2? offset = null, float textScale = 1f, bool selectable = false)
		{
			Vector2 vector = size ?? base.Size ?? new Vector2(0.5f, 0.5f);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(m_currentPosition + vector / 2f + (offset ?? Vector2.Zero), vector, null, "Debug", m_scale * textScale, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, selectable);
			Controls.Add(myGuiControlMultilineText);
			return myGuiControlMultilineText;
		}

		public override void RecreateControls(bool constructor)
		{
			if (m_loadedPrefab == null)
			{
				CloseScreen();
				return;
			}
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionBlueprintDetails, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.86f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.86f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.86f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.86f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlButton obj = MyBlueprintUtils.CreateButton(this, 1f, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Close), OnCloseButton, enabled: true, textScale: m_textScale, tooltip: MySpaceTexts.ToolTipNewsletter_Close);
			obj.Position = new Vector2(0f, m_size.Value.Y / 2f - 0.097f);
			obj.VisualStyle = MyGuiControlButtonStyleEnum.Default;
			CreateTextField();
			CreateDescription();
			CreateButtons();
			m_thumbnailImage.Position = new Vector2(-0.035f, -0.112f) + m_offset;
			m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
			Controls.Add(m_thumbnailImage);
		}

		protected void CallResultCallback(MyGuiControlListbox.Item val)
		{
			callBack(val);
		}

		protected override void Canceling()
		{
			CallResultCallback(m_selectedItem);
			base.Canceling();
		}

		protected abstract void CreateButtons();

		protected void OnCloseButton(MyGuiControlButton button)
		{
			CloseScreen();
			CallResultCallback(m_selectedItem);
		}

		public override bool Update(bool hasFocus)
		{
			if (m_killScreen)
			{
				CallResultCallback(null);
				CloseScreen();
			}
			return base.Update(hasFocus);
		}
	}
}
