using Sandbox.Game.GUI;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenGameCredits : MyGuiScreenBase
	{
		private Color color = new Color(255, 255, 255, 220);

		private const float NUMBER_OF_SECONDS_TO_SCROLL_THROUGH_WHOLE_SCREEN = 30f;

		private float m_movementSpeedMultiplier = 1f;

		private float m_scrollingPositionY;

		private string m_keenswhLogoTexture;

		private float m_startTimeInMilliseconds;

		public MyGuiScreenGameCredits()
			: base(Vector2.Zero)
		{
			m_startTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyGuiDrawAlignEnum myGuiDrawAlignEnum = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(MyGuiManager.ComputeFullscreenGuiCoordinate(myGuiDrawAlignEnum, 54, 84), MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui, null, null, null, myGuiDrawAlignEnum);
			myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
			Controls.Add(myGuiControlPanel);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenGameCredits";
		}

		public override void LoadContent()
		{
			base.DrawMouseCursor = false;
			m_closeOnEsc = true;
			m_keenswhLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";
			ResetScrollingPositionY();
			base.LoadContent();
		}

		private void ResetScrollingPositionY(float offset = 0f)
		{
			m_scrollingPositionY = 0.99f + offset;
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			m_scrollingPositionY -= 0.000555555569f * m_movementSpeedMultiplier;
			return true;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Down))
				{
					m_movementSpeedMultiplier = 10f;
				}
				else if (MyInput.Static.IsNewKeyReleased(MyKeys.Down))
				{
					m_movementSpeedMultiplier = 1f;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Up))
				{
					m_movementSpeedMultiplier = -10f;
				}
				else if (MyInput.Static.IsNewKeyReleased(MyKeys.Up))
				{
					m_movementSpeedMultiplier = 1f;
				}
			}
		}

		private Color ChangeTextAlpha(Color origColor, float coordY)
		{
			float num = 0.05f;
			float num2 = 0.3f;
			float scale = MathHelper.Clamp((coordY - num) / (num2 - num), 0f, 1f);
			return origColor * scale;
		}

		public Vector2 GetScreenLeftTopPosition()
		{
			float num = 25f * MyGuiManager.GetSafeScreenScale();
			MyGuiManager.GetSafeFullscreenRectangle();
			return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2(num, num));
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			float num = m_scrollingPositionY;
			string font = "GameCredits";
			for (int i = 0; i < MyPerGameSettings.Credits.Departments.Count; i++)
			{
				MyGuiManager.DrawString(font, new StringBuilder(MyStatControlText.SubstituteTexts(MyPerGameSettings.Credits.Departments[i].Name.ToString())), new Vector2(0.5f, num), 0.78f, ChangeTextAlpha(color, num), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				num += 0.05f;
				for (int j = 0; j < MyPerGameSettings.Credits.Departments[i].Persons.Count; j++)
				{
					MyGuiManager.DrawString(font, new StringBuilder(MyStatControlText.SubstituteTexts(MyPerGameSettings.Credits.Departments[i].Persons[j].Name.ToString())), new Vector2(0.5f, num), 1.04f, ChangeTextAlpha(color, num), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					num += 0.05f;
				}
				MyCreditsDepartment myCreditsDepartment = MyPerGameSettings.Credits.Departments[i];
				if (myCreditsDepartment.LogoTexture != null)
				{
					num += myCreditsDepartment.LogoOffsetPre;
					if (!myCreditsDepartment.LogoTextureSize.HasValue)
					{
						throw new InvalidBranchException();
					}
					MyGuiManager.DrawSpriteBatch(myCreditsDepartment.LogoTexture, new Vector2(0.5f, num), MyGuiManager.GetNormalizedSize(myCreditsDepartment.LogoTextureSize.Value, myCreditsDepartment.LogoScale.Value), ChangeTextAlpha(color, num), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					num += myCreditsDepartment.LogoOffsetPost;
				}
				num += 0.04f;
			}
			num += 0.05f;
			for (int k = 0; k < MyPerGameSettings.Credits.CreditNotices.Count; k++)
			{
				MyCreditsNotice myCreditsNotice = MyPerGameSettings.Credits.CreditNotices[k];
				if (myCreditsNotice.LogoTexture != null)
				{
					if (!myCreditsNotice.LogoTextureSize.HasValue)
					{
						throw new InvalidBranchException();
					}
					MyGuiManager.DrawSpriteBatch(myCreditsNotice.LogoTexture, new Vector2(0.5f, num), MyGuiManager.GetNormalizedSize(myCreditsNotice.LogoTextureSize.Value, myCreditsNotice.LogoScale.Value), ChangeTextAlpha(color, num), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					num += myCreditsNotice.LogoOffset;
				}
				for (int l = 0; l < myCreditsNotice.CreditNoticeLines.Count; l++)
				{
					MyGuiManager.DrawString(font, new StringBuilder(MyStatControlText.SubstituteTexts(myCreditsNotice.CreditNoticeLines[l].ToString())), new Vector2(0.5f, num), 0.78f, ChangeTextAlpha(color, num), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					num += 0.025f;
				}
				num += 0.15f;
			}
			if (num <= 0f)
			{
				ResetScrollingPositionY();
			}
			MyGuiSandbox.DrawGameLogoHandler(m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
			return true;
		}
	}
}
