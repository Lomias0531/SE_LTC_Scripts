using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenCongratulation : MyGuiScreenBase
	{
		private int m_messageId;

		public MyGuiScreenCongratulation(int messageId)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.7f, 0.54f))
		{
			m_messageId = messageId;
			RecreateControls(constructor: true);
			if (MyAudio.Static != null)
			{
				MyCueId cueId = MySoundPair.GetCueId("ArcNewItemImpact");
				MyAudio.Static.PlaySound(cueId);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenCongratulation";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 value = base.Size ?? new Vector2(1.2f, 0.5f);
			Vector2 value2 = new Vector2(0f, -0.22f);
			MyGuiControlLabel control = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.Campaign_Congratulation_Caption), position: value2, size: null, colorMask: null, textScale: 1.5f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			Controls.Add(control);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(Vector2.Zero, value, Color.White.ToVector4(), "White", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			Controls.Add(myGuiControlMultilineText);
			MyGuiControlButton control2 = new MyGuiControlButton(new Vector2(0f, 0.22f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OkButtonClicked);
			Controls.Add(control2);
			string empty = string.Empty;
			int messageId = m_messageId;
			empty = MyTexts.GetString(MyCommonTexts.Campaign_Congratulation_Text);
			string text = "Textures\\GUI\\PromotedEngineer.dds";
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, new Vector2(0.12f, 0.16f), null, null, new string[1]
			{
				text
			});
			myGuiControlImage.Position = new Vector2(0f, -0.03f);
			Controls.Add(myGuiControlImage);
			myGuiControlMultilineText.Text = new StringBuilder(empty);
		}

		private void OkButtonClicked(MyGuiControlButton button)
		{
			CloseScreen();
		}
	}
}
