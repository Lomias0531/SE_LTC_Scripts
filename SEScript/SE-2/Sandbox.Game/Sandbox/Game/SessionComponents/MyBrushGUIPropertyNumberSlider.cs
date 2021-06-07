using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public class MyBrushGUIPropertyNumberSlider : IMyVoxelBrushGUIProperty
	{
		private MyGuiControlLabel m_label;

		private MyGuiControlLabel m_labelValue;

		private MyGuiControlSlider m_sliderValue;

		public Action ValueChanged;

		public float Value;

		public float ValueMin;

		public float ValueMax;

		public float ValueStep;

		public MyBrushGUIPropertyNumberSlider(float value, float valueMin, float valueMax, float valueStep, MyVoxelBrushGUIPropertyOrder order, MyStringId labelText)
		{
			Vector2 position = new Vector2(-0.1f, -0.2f);
			Vector2 position2 = new Vector2(0.16f, -0.2f);
			Vector2 position3 = new Vector2(-0.1f, -0.173f);
			switch (order)
			{
			case MyVoxelBrushGUIPropertyOrder.Second:
				position.Y = -0.116f;
				position2.Y = -0.116f;
				position3.Y = -0.089f;
				break;
			case MyVoxelBrushGUIPropertyOrder.Third:
				position.Y = -0.032f;
				position2.Y = -0.032f;
				position3.Y = -0.005f;
				break;
			}
			Value = value;
			ValueMin = valueMin;
			ValueMax = valueMax;
			ValueStep = valueStep;
			m_label = new MyGuiControlLabel
			{
				Position = position,
				TextEnum = labelText,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_labelValue = new MyGuiControlLabel
			{
				Position = position2,
				Text = Value.ToString(),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_sliderValue = new MyGuiControlSlider
			{
				Position = position3,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_sliderValue.Size = new Vector2(0.263f, 0.1f);
			m_sliderValue.MaxValue = ValueMax;
			m_sliderValue.Value = Value;
			m_sliderValue.MinValue = ValueMin;
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(Slider_ValueChanged));
		}

		private void Slider_ValueChanged(MyGuiControlSlider sender)
		{
			float num = 1f / ValueStep;
			float num2 = m_sliderValue.Value * num;
			Value = MathHelper.Clamp((float)(int)num2 / num, ValueMin, ValueMax);
			m_labelValue.Text = Value.ToString();
			if (ValueChanged != null)
			{
				ValueChanged();
			}
		}

		public void AddControlsToList(List<MyGuiControlBase> list)
		{
			list.Add(m_label);
			list.Add(m_labelValue);
			list.Add(m_sliderValue);
		}
	}
}
