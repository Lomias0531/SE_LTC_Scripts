using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.SessionComponents;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudQuestlog
	{
		private bool m_isVisible;

		private bool m_isUsedByVisualScripting;

		private string m_questTitle;

		private List<MultilineData> m_content = new List<MultilineData>();

		public readonly Vector2 QuestlogSize = new Vector2(0.4f, 0.22f);

		public bool HighlightChanges = true;

		public string QuestTitle
		{
			get
			{
				return m_questTitle;
			}
			set
			{
				m_questTitle = value;
				RaiseValueChanged();
			}
		}

		public bool Visible
		{
			get
			{
				return m_isVisible;
			}
			set
			{
				m_isVisible = value;
				if (!m_isVisible)
				{
					IsUsedByVisualScripting = false;
				}
			}
		}

		public bool IsUsedByVisualScripting
		{
			get
			{
				return m_isUsedByVisualScripting;
			}
			set
			{
				m_isUsedByVisualScripting = value;
			}
		}

		public event Action ValueChanged;

		private void RaiseValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged();
			}
		}

		public MultilineData[] GetQuestGetails()
		{
			return m_content.ToArray();
		}

		public void CleanDetails()
		{
			m_content.Clear();
			RaiseValueChanged();
		}

		public void AddDetail(string value, bool useTyping = true, bool isObjective = false)
		{
			MultilineData multilineData = new MultilineData();
			multilineData.Data = value;
			multilineData.IsObjective = isObjective;
			if (!useTyping)
			{
				multilineData.CharactersDisplayed = -1;
			}
			m_content.Add(multilineData);
			RaiseValueChanged();
		}

		public bool IsCompleted(int id)
		{
			if (id >= m_content.Count || id < 0)
			{
				return false;
			}
			return m_content[id].Completed;
		}

		public bool SetCompleted(int id, bool completed = true)
		{
			if (id >= m_content.Count || id < 0)
			{
				return false;
			}
			if (m_content[id].Completed == completed)
			{
				return false;
			}
			m_content[id].Completed = completed;
			RaiseValueChanged();
			return true;
		}

		public bool SetAllCompleted(bool completed = true)
		{
			foreach (MultilineData item in m_content)
			{
				item.Completed = completed;
			}
			RaiseValueChanged();
			return true;
		}

		public void ModifyDetail(int id, string value, bool useTyping = true)
		{
			if (id < m_content.Count && id >= 0)
			{
				MultilineData multilineData = m_content[id];
				multilineData.Data = value;
				m_content[id] = multilineData;
				if (!useTyping)
				{
					m_content[id].CharactersDisplayed = -1;
				}
				RaiseValueChanged();
			}
		}

		public void Save()
		{
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			if (component != null)
			{
				component.QuestlogData = GetObjectBuilder();
			}
		}

		public MyObjectBuilder_Questlog GetObjectBuilder()
		{
			MyObjectBuilder_Questlog myObjectBuilder_Questlog = new MyObjectBuilder_Questlog();
			myObjectBuilder_Questlog.Title = QuestTitle;
			myObjectBuilder_Questlog.Visible = Visible;
			myObjectBuilder_Questlog.IsUsedByVisualScripting = IsUsedByVisualScripting;
			myObjectBuilder_Questlog.LineData.Capacity = m_content.Count;
			myObjectBuilder_Questlog.LineData.AddRange(m_content);
			return myObjectBuilder_Questlog;
		}

		public void Init()
		{
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			if (component != null)
			{
				MyObjectBuilder_Questlog questlogData = component.QuestlogData;
				if (questlogData != null)
				{
					m_content.Clear();
					m_content.AddRange(questlogData.LineData);
					QuestTitle = questlogData.Title;
					Visible = (questlogData.LineData.Count > 0 && questlogData.Visible);
					IsUsedByVisualScripting = questlogData.IsUsedByVisualScripting;
				}
			}
		}
	}
}
