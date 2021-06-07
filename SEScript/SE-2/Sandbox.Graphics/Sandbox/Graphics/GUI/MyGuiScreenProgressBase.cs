using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyGuiScreenProgressBase : MyGuiScreenBase
	{
		private bool m_controlsCreated;

		private bool m_loaded;

		private MyStringId m_progressText;

		private string m_progressTextString;

		private MyStringId? m_cancelText;

		protected MyGuiControlLabel m_progressTextLabel;

		protected MyGuiControlRotatingWheel m_rotatingWheel;

		private string m_wheelTexture;

		protected bool ReturnToMainMenuOnError;

		public MyGuiControlRotatingWheel RotatingWheel => m_rotatingWheel;

		public MyStringId ProgressText
		{
			get
			{
				return m_progressText;
			}
			set
			{
				if (m_progressText != value)
				{
					m_progressText = value;
					m_progressTextLabel.TextEnum = value;
				}
			}
		}

		public string ProgressTextString
		{
			get
			{
				return m_progressTextString;
			}
			set
			{
				if (m_progressTextString != value)
				{
					m_progressTextString = value;
					m_progressTextLabel.Text = value;
				}
			}
		}

		public event Action ProgressCancelled;

		public MyGuiScreenProgressBase(MyStringId progressText, MyStringId? cancelText = null, bool isTopMostScreen = true, bool enableBackgroundFade = true)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, null, isTopMostScreen)
		{
			m_progressText = progressText;
			m_cancelText = cancelText;
			base.EnabledBackgroundFade = enableBackgroundFade;
			base.DrawMouseCursor = m_cancelText.HasValue;
			m_closeOnEsc = m_cancelText.HasValue;
			m_drawEvenWithoutFocus = true;
			base.CanHideOthers = false;
			base.CanBeHidden = false;
			RecreateControls(constructor: true);
		}

		protected virtual void OnCancelClick(MyGuiControlButton sender)
		{
			Canceling();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_controlsCreated = false;
			LoadControls();
		}

		private void LoadControls()
		{
			m_wheelTexture = "Textures\\GUI\\screens\\screen_loading_wheel.dds";
			m_size = new Vector2(299f / 800f, 23f / 75f);
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_LOAD_BORDER
			};
			myGuiControlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			myGuiControlCompositePanel.Position = new Vector2(0f, 0f);
			myGuiControlCompositePanel.Size = m_size.Value;
			Controls.Add(myGuiControlCompositePanel);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			if (!m_cancelText.HasValue)
			{
				myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.718f / 2f, m_size.Value.Y / 2f - 0.256f), m_size.Value.X * 0.718f);
			}
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.718f / 2f, m_size.Value.Y / 2f - 0.079f), m_size.Value.X * 0.718f);
			Controls.Add(myGuiControlSeparatorList);
			m_progressTextLabel = new MyGuiControlLabel(new Vector2(0f, -0.098f), null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_progressTextLabel.TextEnum = m_progressText;
			Controls.Add(m_progressTextLabel);
			float num = 0f;
			float num2 = 0.015f;
			if (!m_cancelText.HasValue)
			{
				m_rotatingWheel = new MyGuiControlRotatingWheel(new Vector2(0f - num, num2 - 0.003f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.36f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, m_wheelTexture);
			}
			else
			{
				m_rotatingWheel = new MyGuiControlRotatingWheel(new Vector2(0f - num, num2 - 0.028f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.36f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, m_wheelTexture);
			}
			Controls.Add(m_rotatingWheel);
			if (m_cancelText.HasValue)
			{
				MyGuiControlButton control = new MyGuiControlButton(new Vector2(num, num2 + 0.069f), MyGuiControlButtonStyleEnum.ToolbarButton, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(m_cancelText.Value), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick);
				Controls.Add(control);
			}
			m_controlsCreated = true;
		}

		public void Cancel()
		{
			Canceling();
		}

		protected override void Canceling()
		{
			base.Canceling();
			this.ProgressCancelled?.Invoke();
			Action action = null;
		}

		public override bool Draw()
		{
			if (!m_controlsCreated)
			{
				LoadControls();
			}
			return base.Draw();
		}

		public override void LoadContent()
		{
			if (!m_loaded)
			{
				m_loaded = true;
				ProgressStart();
			}
			base.LoadContent();
		}

		public override void UnloadContent()
		{
			m_loaded = false;
			base.UnloadContent();
		}

		protected abstract void ProgressStart();
	}
}
