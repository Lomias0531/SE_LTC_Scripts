#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Game.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenClaimGameItem : MyGuiScreenBase
	{
		private long m_playerId;

		private MyContainerDropComponent m_container;

		public MyGuiScreenClaimGameItem(MyContainerDropComponent container, long playerId)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.41f, 0.4f), isTopMostScreen: true)
		{
			m_playerId = playerId;
			m_container = container;
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
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2(-0.15f, -0.107f), new Vector2(0.3f, 0.17f), null, null, new string[1]
			{
				"Textures\\GUI\\ClaimItem.png"
			}, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlImage.BorderEnabled = true;
			myGuiControlImage.BorderSize = 2;
			myGuiControlImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
			Controls.Add(myGuiControlImage);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0f, 0.085f), null, MyTexts.GetString(MyCommonTexts.ScreenClaimItemText), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel.Font = "White";
			Elements.Add(myGuiControlLabel);
			MyGuiControlButton control = new MyGuiControlButton(new Vector2(0f, 0.168f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnClaimButtonClick);
			Controls.Add(control);
		}

		private void OnClaimButtonClick(MyGuiControlButton obj)
		{
			MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
			if (component != null)
			{
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.DropContainer, new Dictionary<string, object>
				{
					{
						"Competetive",
						m_container.Competetive
					}
				});
				component.ContainerOpened(m_container, m_playerId);
			}
			CloseScreen();
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenClaimGameItem";
		}
	}
}
