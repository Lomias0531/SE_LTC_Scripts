using Sandbox.Game.Entities;
using System;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyControllableEntityControlHelper : MyAbstractControlMenuItem
	{
		protected IMyControllableEntity m_entity;

		private Action<IMyControllableEntity> m_action;

		private Func<IMyControllableEntity, bool> m_valueGetter;

		private string m_label;

		private string m_value;

		private string m_onValue;

		private string m_offValue;

		public override string Label => m_label;

		public override string CurrentValue => m_value;

		public MyControllableEntityControlHelper(MyStringId controlId, Action<IMyControllableEntity> action, Func<IMyControllableEntity, bool> valueGetter, MyStringId label, MySupportKeysEnum supportKeys = MySupportKeysEnum.NONE)
			: this(controlId, action, valueGetter, label, MyCommonTexts.ControlMenuItemValue_On, MyCommonTexts.ControlMenuItemValue_Off, supportKeys)
		{
		}

		public MyControllableEntityControlHelper(MyStringId controlId, Action<IMyControllableEntity> action, Func<IMyControllableEntity, bool> valueGetter, MyStringId label, MyStringId onValue, MyStringId offValue, MySupportKeysEnum supportKeys = MySupportKeysEnum.NONE)
			: base(controlId, supportKeys)
		{
			m_action = action;
			m_valueGetter = valueGetter;
			m_label = MyTexts.GetString(label);
			m_onValue = MyTexts.GetString(onValue);
			m_offValue = MyTexts.GetString(offValue);
		}

		public void SetEntity(IMyControllableEntity entity)
		{
			m_entity = entity;
		}

		public override void Activate()
		{
			m_action(m_entity);
		}

		public override void Next()
		{
			Activate();
		}

		public override void Previous()
		{
			Activate();
		}

		public override void UpdateValue()
		{
			if (m_valueGetter(m_entity))
			{
				m_value = m_onValue;
			}
			else
			{
				m_value = m_offValue;
			}
		}
	}
}
