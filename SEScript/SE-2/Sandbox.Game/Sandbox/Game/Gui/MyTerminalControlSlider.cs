using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Text;
using VRage;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlSlider<TBlock> : MyTerminalValueControl<TBlock, float>, IMyTerminalControlSlider, IMyTerminalControl, IMyTerminalValueControl<float>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		public delegate float FloatFunc(TBlock block, float val);

		public MyStringId Title;

		public MyStringId Tooltip;

		private MyGuiControlSlider m_slider;

		private MyGuiControlBlockProperty m_control;

		private Action<float> m_amountConfirmed;

		public bool AmountDialogEnabled = true;

		public WriterDelegate Writer;

		public WriterDelegate CompactWriter;

		public AdvancedWriterDelegate AdvancedWriter;

		public FloatFunc Normalizer = (TBlock b, float f) => f;

		public FloatFunc Denormalizer = (TBlock b, float f) => f;

		public GetterDelegate DefaultValueGetter;

		private Action<MyGuiControlSlider> m_valueChanged;

		public float? DefaultValue
		{
			set
			{
				DefaultValueGetter = (value.HasValue ? ((GetterDelegate)((TBlock block) => value.Value)) : null);
			}
		}

		public string Formatter
		{
			set
			{
				Writer = ((value != null) ? ((WriterDelegate)delegate(TBlock block, StringBuilder result)
				{
					result.AppendFormat(value, GetValue(block));
				}) : null);
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Title
		{
			get
			{
				return Title;
			}
			set
			{
				Title = value;
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Tooltip
		{
			get
			{
				return Tooltip;
			}
			set
			{
				Tooltip = value;
			}
		}

		Action<IMyTerminalBlock, StringBuilder> IMyTerminalControlSlider.Writer
		{
			get
			{
				WriterDelegate oldWriter = Writer;
				return delegate(IMyTerminalBlock x, StringBuilder y)
				{
					oldWriter((TBlock)x, y);
				};
			}
			set
			{
				Writer = value.Invoke;
			}
		}

		public MyTerminalControlSlider(string id, MyStringId title, MyStringId tooltip)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			CompactWriter = CompactWriterMethod;
			m_amountConfirmed = AmountSetter;
			Serializer = delegate(BitStream stream, ref float value)
			{
				stream.Serialize(ref value);
			};
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_slider = new MyGuiControlSlider(width: MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, position: Vector2.Zero);
			m_valueChanged = OnValueChange;
			m_slider.ValueChanged = m_valueChanged;
			m_slider.SliderClicked = OnSliderClicked;
			m_control = new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_slider);
			return m_control;
		}

		public void CompactWriterMethod(TBlock block, StringBuilder appendTo)
		{
			int length = appendTo.Length;
			Writer(block, appendTo);
			int num = FirstIndexOf(appendTo, length, ".,");
			if (num >= 0)
			{
				RemoveNumbersFrom(num, appendTo);
			}
		}

		private int FirstIndexOf(StringBuilder sb, int start, string chars, int count = int.MaxValue)
		{
			int num = Math.Min(start + count, sb.Length);
			for (int i = start; i < num; i++)
			{
				char c = sb[i];
				for (int j = 0; j < chars.Length; j++)
				{
					if (c == chars[j])
					{
						return i;
					}
				}
			}
			return -1;
		}

		private void RemoveNumbersFrom(int index, StringBuilder sb)
		{
			sb.Remove(index, 1);
			while (index < sb.Length && ((sb[index] >= '0' && sb[index] <= '9') || sb[index] == ' '))
			{
				sb.Remove(index, 1);
			}
			if (sb[0] == '-' && sb[1] == '0')
			{
				sb.Remove(0, 1);
			}
		}

		public void SetLimits(float min, float max)
		{
			Normalizer = ((TBlock block, float f) => MathHelper.Clamp((f - min) / (max - min), 0f, 1f));
			Denormalizer = ((TBlock block, float f) => MathHelper.Clamp(min + f * (max - min), min, max));
		}

		public void SetLogLimits(float min, float max)
		{
			Normalizer = ((TBlock block, float f) => MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0f, 1f));
			Denormalizer = ((TBlock block, float f) => MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max));
		}

		public void SetDualLogLimits(float absMin, float absMax, float centerBand)
		{
			Normalizer = ((TBlock block, float f) => DualLogNormalizer(block, f, absMin, absMax, centerBand));
			Denormalizer = ((TBlock block, float f) => DualLogDenormalizer(block, f, absMin, absMax, centerBand));
		}

		private static float DualLogDenormalizer(TBlock block, float value, float min, float max, float centerBand)
		{
			float value2 = value * 2f - 1f;
			if (Math.Abs(value2) < centerBand)
			{
				return 0f;
			}
			return MathHelper.Clamp(MathHelper.InterpLog((Math.Abs(value2) - centerBand) / (1f - centerBand), min, max), min, max) * (float)Math.Sign(value2);
		}

		private static float DualLogNormalizer(TBlock block, float value, float min, float max, float centerBand)
		{
			if (Math.Abs(value) < min)
			{
				return 0.5f;
			}
			float num = 0.5f - centerBand / 2f;
			float num2 = MathHelper.Clamp(MathHelper.InterpLogInv(Math.Abs(value), min, max), 0f, 1f) * num;
			if (value < 0f)
			{
				return num - num2;
			}
			return num2 + num + centerBand;
		}

		public void SetLimits(GetterDelegate minGetter, GetterDelegate maxGetter)
		{
			Normalizer = delegate(TBlock block, float f)
			{
				float num3 = minGetter(block);
				float num4 = maxGetter(block);
				return MathHelper.Clamp((f - num3) / (num4 - num3), 0f, 1f);
			};
			Denormalizer = delegate(TBlock block, float f)
			{
				float num = minGetter(block);
				float num2 = maxGetter(block);
				return MathHelper.Clamp(num + f * (num2 - num), num, num2);
			};
		}

		void IMyTerminalControlSlider.SetLimits(Func<IMyTerminalBlock, float> minGetter, Func<IMyTerminalBlock, float> maxGetter)
		{
			GetterDelegate minGetter2 = minGetter.Invoke;
			GetterDelegate maxGetter2 = maxGetter.Invoke;
			SetLimits(minGetter2, maxGetter2);
		}

		public void SetLogLimits(GetterDelegate minGetter, GetterDelegate maxGetter)
		{
			Normalizer = delegate(TBlock block, float f)
			{
				float amount = minGetter(block);
				float amount2 = maxGetter(block);
				return MathHelper.Clamp(MathHelper.InterpLogInv(f, amount, amount2), 0f, 1f);
			};
			Denormalizer = delegate(TBlock block, float f)
			{
				float num = minGetter(block);
				float num2 = maxGetter(block);
				return MathHelper.Clamp(MathHelper.InterpLog(f, num, num2), num, num2);
			};
		}

		void IMyTerminalControlSlider.SetLogLimits(Func<IMyTerminalBlock, float> minGetter, Func<IMyTerminalBlock, float> maxGetter)
		{
			GetterDelegate minGetter2 = minGetter.Invoke;
			GetterDelegate maxGetter2 = maxGetter.Invoke;
			SetLogLimits(minGetter2, maxGetter2);
		}

		public void SetDualLogLimits(GetterDelegate minGetter, GetterDelegate maxGetter, float centerBand)
		{
			Normalizer = delegate(TBlock block, float f)
			{
				float min2 = minGetter(block);
				float max2 = maxGetter(block);
				return DualLogNormalizer(block, f, min2, max2, centerBand);
			};
			Denormalizer = delegate(TBlock block, float f)
			{
				float min = minGetter(block);
				float max = maxGetter(block);
				return DualLogDenormalizer(block, f, min, max, centerBand);
			};
		}

		void IMyTerminalControlSlider.SetDualLogLimits(Func<IMyTerminalBlock, float> minGetter, Func<IMyTerminalBlock, float> maxGetter, float centerBand)
		{
			GetterDelegate minGetter2 = minGetter.Invoke;
			GetterDelegate maxGetter2 = maxGetter.Invoke;
			SetDualLogLimits(minGetter2, maxGetter2, centerBand);
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null && m_slider != null)
			{
				m_slider.ValueChanged = null;
				m_slider.DefaultValue = ((DefaultValueGetter != null) ? new float?(Normalizer(firstBlock, DefaultValueGetter(firstBlock))) : null);
				m_slider.Value = Normalizer(firstBlock, GetValue(firstBlock));
				m_slider.ValueChanged = m_valueChanged;
				UpdateDetailedInfo(firstBlock);
			}
		}

		private void UpdateDetailedInfo(TBlock block)
		{
			if (AdvancedWriter != null)
			{
				m_control.SetDetailedInfo(AdvancedWriter, block);
			}
			else
			{
				m_control.SetDetailedInfo(Writer, block);
			}
		}

		private void OnValueChange(MyGuiControlSlider slider)
		{
			SetValue(slider.Value);
			UpdateDetailedInfo(base.FirstBlock);
		}

		private void SetValue(float value)
		{
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				SetValue(targetBlock, Denormalizer(targetBlock, value));
			}
		}

		private void AmountSetter(float value)
		{
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				m_slider.Value = Normalizer(firstBlock, value);
			}
		}

		private bool OnSliderClicked(MyGuiControlSlider arg)
		{
			TBlock firstBlock = base.FirstBlock;
			if (AmountDialogEnabled && MyInput.Static.IsAnyCtrlKeyPressed() && firstBlock != null)
			{
				MyGuiScreenDialogAmount obj = new MyGuiScreenDialogAmount(Denormalizer(firstBlock, 0f), Denormalizer(firstBlock, 1f), defaultAmount: Denormalizer(firstBlock, arg.Value), caption: MyCommonTexts.DialogAmount_SetValueCaption, minMaxDecimalDigits: 3, parseAsInteger: false, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity);
				obj.OnConfirmed += m_amountConfirmed;
				MyGuiSandbox.AddScreen(obj);
				return true;
			}
			return false;
		}

		private void IncreaseAction(TBlock block, float step)
		{
			float num = Normalizer(block, GetValue(block));
			SetValue(block, Denormalizer(block, MathHelper.Clamp(num + step, 0f, 1f)));
		}

		private void DecreaseAction(TBlock block, float step)
		{
			float num = Normalizer(block, GetValue(block));
			SetValue(block, Denormalizer(block, MathHelper.Clamp(num - step, 0f, 1f)));
		}

		private void ResetAction(TBlock block)
		{
			if (DefaultValueGetter != null)
			{
				SetValue(block, DefaultValueGetter(block));
			}
		}

		private void ActionWriter(TBlock block, StringBuilder appendTo)
		{
			(CompactWriter ?? Writer)(block, appendTo);
		}

		private void SetActions(params MyTerminalAction<TBlock>[] actions)
		{
			base.Actions = actions;
		}

		public void EnableActions(string increaseIcon, string decreaseIcon, StringBuilder increaseName, StringBuilder decreaseName, float step, string resetIcon = null, StringBuilder resetName = null, Func<TBlock, bool> enabled = null, Func<TBlock, bool> callable = null)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>("Increase" + Id, increaseName, delegate(TBlock b)
			{
				IncreaseAction(b, step);
			}, ActionWriter, increaseIcon, enabled, callable);
			MyTerminalAction<TBlock> myTerminalAction2 = new MyTerminalAction<TBlock>("Decrease" + Id, decreaseName, delegate(TBlock b)
			{
				DecreaseAction(b, step);
			}, ActionWriter, decreaseIcon, enabled, callable);
			if (resetIcon != null)
			{
				SetActions(myTerminalAction, myTerminalAction2, new MyTerminalAction<TBlock>("Reset" + Id, resetName, ResetAction, ActionWriter, resetIcon, enabled, callable));
			}
			else
			{
				SetActions(myTerminalAction, myTerminalAction2);
			}
		}

		public override void SetValue(TBlock block, float value)
		{
			base.SetValue(block, MathHelper.Clamp(value, Denormalizer(block, 0f), Denormalizer(block, 1f)));
		}

		public override float GetDefaultValue(TBlock block)
		{
			return DefaultValueGetter(block);
		}

		public override float GetMinimum(TBlock block)
		{
			return Denormalizer(block, 0f);
		}

		public override float GetMaximum(TBlock block)
		{
			return Denormalizer(block, 1f);
		}

		public override float GetValue(TBlock block)
		{
			return base.GetValue(block);
		}
	}
}
