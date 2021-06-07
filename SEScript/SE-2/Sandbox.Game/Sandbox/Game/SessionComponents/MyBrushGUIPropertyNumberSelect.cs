using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public class MyBrushGUIPropertyNumberSelect : IMyVoxelBrushGUIProperty
	{
		private MyGuiControlButton m_lowerValue;

		private MyGuiControlButton m_upperValue;

		private MyGuiControlLabel m_label;

		private MyGuiControlLabel m_labelValue;

		public Action ValueIncreased;

		public Action ValueDecreased;

		public float Value;

		public float ValueMin;

		public float ValueMax;

		public float ValueStep;

		public MyBrushGUIPropertyNumberSelect(float value, float valueMin, float valueMax, float valueStep, MyVoxelBrushGUIPropertyOrder order, MyStringId labelText)
		{
			Vector2 position = new Vector2(-0.1f, -0.15f);
			Vector2 position2 = new Vector2(0.035f, -0.15f);
			Vector2 position3 = new Vector2(0f, -0.1475f);
			Vector2 position4 = new Vector2(0.08f, -0.1475f);
			switch (order)
			{
			case MyVoxelBrushGUIPropertyOrder.Second:
				position.Y = -0.07f;
				position2.Y = -0.07f;
				position3.Y = -0.0675f;
				position4.Y = -0.0675f;
				break;
			case MyVoxelBrushGUIPropertyOrder.Third:
				position.Y = 0.01f;
				position2.Y = 0.01f;
				position3.Y = 0.0125f;
				position4.Y = 0.0125f;
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
			m_lowerValue = new MyGuiControlButton
			{
				Position = position3,
				VisualStyle = MyGuiControlButtonStyleEnum.ArrowLeft,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_upperValue = new MyGuiControlButton
			{
				Position = position4,
				VisualStyle = MyGuiControlButtonStyleEnum.ArrowRight,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_labelValue = new MyGuiControlLabel
			{
				Position = position2,
				Text = Value.ToString(),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_lowerValue.ButtonClicked += LowerClicked;
			m_upperValue.ButtonClicked += UpperClicked;
		}

		private void LowerClicked(MyGuiControlButton sender)
		{
			Value = MathHelper.Clamp(Value - ValueStep, ValueMin, ValueMax);
			m_labelValue.Text = Value.ToString();
			if (ValueDecreased != null)
			{
				ValueDecreased();
			}
		}

		private void UpperClicked(MyGuiControlButton sender)
		{
			Value = MathHelper.Clamp(Value + ValueStep, ValueMin, ValueMax);
			m_labelValue.Text = Value.ToString();
			if (ValueIncreased != null)
			{
				ValueIncreased();
			}
		}

		public void AddControlsToList(List<MyGuiControlBase> list)
		{
			list.Add(m_lowerValue);
			list.Add(m_upperValue);
			list.Add(m_label);
			list.Add(m_labelValue);
		}
	}
}
