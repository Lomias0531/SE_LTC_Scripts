using Sandbox.Game.Components;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyTriggerManipulator
	{
		private Vector3D m_currentPosition;

		private readonly List<MyTriggerComponent> m_currentQuery = new List<MyTriggerComponent>();

		private readonly Predicate<MyTriggerComponent> m_triggerEvaluationpPredicate;

		private MyTriggerComponent m_selectedTrigger;

		public Vector3D CurrentPosition
		{
			get
			{
				return m_currentPosition;
			}
			set
			{
				if (!(value == m_currentPosition))
				{
					Vector3D currentPosition = m_currentPosition;
					m_currentPosition = value;
					OnPositionChanged(currentPosition, m_currentPosition);
				}
			}
		}

		public List<MyTriggerComponent> CurrentQuery => m_currentQuery;

		public MyTriggerComponent SelectedTrigger
		{
			get
			{
				return m_selectedTrigger;
			}
			set
			{
				if (m_selectedTrigger != value)
				{
					if (m_selectedTrigger != null)
					{
						m_selectedTrigger.CustomDebugColor = Color.Red;
					}
					m_selectedTrigger = value;
					if (m_selectedTrigger != null)
					{
						m_selectedTrigger.CustomDebugColor = Color.Yellow;
					}
				}
			}
		}

		public MyTriggerManipulator(Predicate<MyTriggerComponent> triggerEvaluationPredicate = null)
		{
			m_triggerEvaluationpPredicate = triggerEvaluationPredicate;
		}

		protected virtual void OnPositionChanged(Vector3D oldPosition, Vector3D newPosition)
		{
			List<MyTriggerComponent> intersectingTriggers = MySessionComponentTriggerSystem.Static.GetIntersectingTriggers(newPosition);
			m_currentQuery.Clear();
			foreach (MyTriggerComponent item in intersectingTriggers)
			{
				if (m_triggerEvaluationpPredicate != null)
				{
					if (m_triggerEvaluationpPredicate(item))
					{
						m_currentQuery.Add(item);
					}
				}
				else
				{
					m_currentQuery.Add(item);
				}
			}
		}

		public void SelectClosest(Vector3D position)
		{
			double num = double.MaxValue;
			if (SelectedTrigger != null)
			{
				SelectedTrigger.CustomDebugColor = Color.Red;
			}
			foreach (MyTriggerComponent item in m_currentQuery)
			{
				double num2 = (item.Center - position).LengthSquared();
				if (num2 < num)
				{
					num = num2;
					SelectedTrigger = item;
				}
			}
			if (Math.Abs(num - double.MaxValue) < double.Epsilon)
			{
				SelectedTrigger = null;
			}
			if (SelectedTrigger != null)
			{
				SelectedTrigger.CustomDebugColor = Color.Yellow;
			}
		}
	}
}
