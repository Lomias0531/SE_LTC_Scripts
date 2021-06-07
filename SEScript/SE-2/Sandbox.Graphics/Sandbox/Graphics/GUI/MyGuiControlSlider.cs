using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlSlider : MyGuiControlSliderBase
	{
		private int m_labelDecimalPlaces;

		private string m_labelFormat = "{0}";

		private float m_minValue;

		private float m_maxValue;

		private bool m_intValue;

		private float m_range;

		public new Action<MyGuiControlSlider> ValueChanged;

		public new Func<MyGuiControlSlider, bool> SliderClicked;

		public int LabelDecimalPlaces
		{
			get
			{
				return m_labelDecimalPlaces;
			}
			set
			{
				m_labelDecimalPlaces = value;
			}
		}

		public float ValueNormalized => base.Ratio;

		public float? DefaultValue
		{
			get
			{
				if (!DefaultRatio.HasValue)
				{
					return null;
				}
				return RatioToValue(DefaultRatio.Value);
			}
			set
			{
				if (value.HasValue)
				{
					DefaultRatio = ValueToRatio(value.Value);
				}
				else
				{
					DefaultRatio = null;
				}
			}
		}

		public float MinValue
		{
			get
			{
				return m_minValue;
			}
			set
			{
				m_minValue = value;
				m_range = m_maxValue - m_minValue;
				Refresh();
			}
		}

		public float MaxValue
		{
			get
			{
				return m_maxValue;
			}
			set
			{
				m_maxValue = value;
				m_range = m_maxValue - m_minValue;
				Refresh();
			}
		}

		protected override float MinimumStep
		{
			get
			{
				if (!m_intValue)
				{
					return base.MinimumStep;
				}
				return 1f / m_range;
			}
		}

		public bool IntValue
		{
			get
			{
				return m_intValue;
			}
			set
			{
				m_intValue = value;
				Refresh();
			}
		}

		public MyGuiControlSlider(Vector2? position = null, float minValue = 0f, float maxValue = 1f, float width = 0.29f, float? defaultValue = null, Vector4? color = null, string labelText = null, int labelDecimalPlaces = 1, float labelScale = 0.8f, float labelSpaceWidth = 0f, string labelFont = "White", string toolTip = null, MyGuiControlSliderStyleEnum visualStyle = MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, bool intValue = false, bool showLabel = false)
			: base(position, width, null, null, color, labelScale, labelSpaceWidth, labelFont, toolTip, visualStyle, originAlign, showLabel)
		{
			m_minValue = minValue;
			m_maxValue = maxValue;
			m_range = m_maxValue - m_minValue;
			base.Propeties = new MyGuiSliderProperties
			{
				FormatLabel = FormatValue,
				RatioFilter = FilterRatio,
				RatioToValue = RatioToValue,
				ValueToRatio = ValueToRatio
			};
			DefaultRatio = (defaultValue.HasValue ? new float?(ValueToRatio(defaultValue.Value)) : null);
			base.Ratio = (DefaultRatio ?? minValue);
			m_intValue = intValue;
			LabelDecimalPlaces = labelDecimalPlaces;
			m_labelFormat = labelText;
			UpdateLabel();
		}

		public void SetBounds(float minValue, float maxValue)
		{
			m_minValue = minValue;
			m_maxValue = maxValue;
			m_range = maxValue - minValue;
			Refresh();
		}

		protected override void OnValueChange()
		{
			ValueChanged.InvokeIfNotNull(this);
		}

		protected override bool OnSliderClicked()
		{
			if (SliderClicked != null)
			{
				return SliderClicked(this);
			}
			return false;
		}

		private void Refresh()
		{
			base.Ratio = base.Ratio;
			UpdateLabel();
		}

		private float RatioToValue(float ratio)
		{
			if (m_intValue)
			{
				return (float)Math.Round(ratio * m_range + m_minValue);
			}
			return ratio * m_range + m_minValue;
		}

		private float ValueToRatio(float ratio)
		{
			return (ratio - m_minValue) / m_range;
		}

		private float FilterRatio(float ratio)
		{
			ratio = MathHelper.Clamp(ratio, 0f, 1f);
			if (m_intValue)
			{
				return ValueToRatio((float)Math.Round(RatioToValue(ratio)));
			}
			return ratio;
		}

		private string FormatValue(float value)
		{
			if (m_labelFormat != null)
			{
				return string.Format(m_labelFormat, MyValueFormatter.GetFormatedFloat(value, LabelDecimalPlaces));
			}
			return null;
		}
	}
}
