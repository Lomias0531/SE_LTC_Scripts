using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenWorldGeneratorSettings : MyGuiScreenBase
	{
		public enum AsteroidAmountEnum
		{
			None = 0,
			Normal = 4,
			More = 7,
			Many = 0x10,
			ProceduralLow = -1,
			ProceduralNormal = -2,
			ProceduralHigh = -3,
			ProceduralNone = -4
		}

		public enum MyFloraDensityEnum
		{
			NONE = 0,
			LOW = 10,
			MEDIUM = 20,
			HIGH = 30,
			EXTREME = 40
		}

		private MyGuiScreenWorldSettings m_parent;

		private int? m_asteroidAmount;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlCombobox m_asteroidAmountCombo;

		private MyGuiControlLabel m_asteroidAmountLabel;

		public int AsteroidAmount
		{
			get
			{
				if (!m_asteroidAmount.HasValue)
				{
					return -1;
				}
				return m_asteroidAmount.Value;
			}
			set
			{
				m_asteroidAmount = value;
				switch (value)
				{
				case 0:
					m_asteroidAmountCombo.SelectItemByKey(0L);
					break;
				case 4:
					m_asteroidAmountCombo.SelectItemByKey(4L);
					break;
				case 7:
					m_asteroidAmountCombo.SelectItemByKey(7L);
					break;
				case 16:
					m_asteroidAmountCombo.SelectItemByKey(16L);
					break;
				case -4:
					m_asteroidAmountCombo.SelectItemByKey(-4L);
					break;
				case -1:
					m_asteroidAmountCombo.SelectItemByKey(-1L);
					break;
				case -2:
					m_asteroidAmountCombo.SelectItemByKey(-2L);
					break;
				case -3:
					m_asteroidAmountCombo.SelectItemByKey(-3L);
					break;
				}
			}
		}

		public event Action OnOkButtonClicked;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWorldGeneratorSettings";
		}

		public static Vector2 CalcSize()
		{
			float y = 0.3f;
			return new Vector2(0.65f, y);
		}

		public MyGuiScreenWorldGeneratorSettings(MyGuiScreenWorldSettings parent)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, CalcSize(), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_parent = parent;
			RecreateControls(constructor: true);
			SetSettingsToControls();
		}

		public void GetSettings(MyObjectBuilder_SessionSettings output)
		{
		}

		protected virtual void SetSettingsToControls()
		{
		}

		public override void RecreateControls(bool constructor)
		{
			float x = 0.309375018f;
			base.RecreateControls(constructor);
			AddCaption(MySpaceTexts.ScreenCaptionWorldGeneratorSettings);
			m_asteroidAmountCombo = new MyGuiControlCombobox(null, new Vector2(x, 0.04f));
			m_asteroidAmountCombo.ItemSelected += m_asteroidAmountCombo_ItemSelected;
			m_asteroidAmountCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAsteroidAmount));
			if (MyFakes.ENABLE_ASTEROID_FIELDS)
			{
				m_asteroidAmountCombo.AddItem(-4L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNone);
				m_asteroidAmountCombo.AddItem(-1L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralLow);
				m_asteroidAmountCombo.AddItem(-2L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNormal);
				if (Environment.Is64BitProcess)
				{
					m_asteroidAmountCombo.AddItem(-3L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralHigh);
				}
			}
			m_asteroidAmountLabel = MakeLabel(MySpaceTexts.Asteroid_Amount);
			Controls.Add(m_asteroidAmountLabel);
			Controls.Add(m_asteroidAmountCombo);
			int num = 0;
			float y = 0.12f;
			float x2 = 0.055f;
			float x3 = 0.25f;
			Vector2 value = new Vector2(0f, 0.052f);
			Vector2 value2 = -m_size.Value / 2f + new Vector2(x2, y);
			Vector2 value3 = value2 + new Vector2(x3, 0f);
			foreach (MyGuiControlBase control in Controls)
			{
				control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				if (control is MyGuiControlLabel)
				{
					control.Position = value2 + value * num;
				}
				else
				{
					control.Position = value3 + value * num++;
				}
			}
			Vector2 value4 = m_size.Value / 2f - new Vector2(0.23f, 0.03f);
			m_okButton = new MyGuiControlButton(value4 - new Vector2(0.01f, 0f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OkButtonClicked);
			m_cancelButton = new MyGuiControlButton(value4 + new Vector2(0.01f, 0f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, CancelButtonClicked);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
		}

		private void grassDensitySlider_ValueChanged(MyGuiControlSlider slider)
		{
			MyRenderProxy.Settings.User.GrassDensityFactor = slider.Value;
			MyRenderProxy.SetSettingsDirty();
		}

		private void m_asteroidAmountCombo_ItemSelected()
		{
			m_asteroidAmount = (int)m_asteroidAmountCombo.GetSelectedKey();
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(textEnum));
		}

		private MyFloraDensityEnum FloraDensityEnumKey(int floraDensity)
		{
			if (Enum.IsDefined(typeof(MyFloraDensityEnum), (MyFloraDensityEnum)floraDensity))
			{
				return (MyFloraDensityEnum)floraDensity;
			}
			return MyFloraDensityEnum.LOW;
		}

		private void CancelButtonClicked(object sender)
		{
			CloseScreen();
		}

		private void OkButtonClicked(object sender)
		{
			if (this.OnOkButtonClicked != null)
			{
				this.OnOkButtonClicked();
			}
			CloseScreen();
		}

		protected new MyGuiControlLabel AddCaption(MyStringId textEnum, Vector4? captionTextColor = null, Vector2? captionOffset = null, float captionScale = 0.8f)
		{
			return AddCaption(MyTexts.GetString(textEnum), captionTextColor, captionOffset, captionScale);
		}
	}
}
