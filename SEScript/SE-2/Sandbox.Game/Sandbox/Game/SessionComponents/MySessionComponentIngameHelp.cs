using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000)]
	public class MySessionComponentIngameHelp : MySessionComponentBase
	{
		private struct ObjectiveDescription
		{
			public string Id;

			public Type Type;

			public int Priority;

			public ObjectiveDescription(string id, Type type, int priority)
			{
				Id = id;
				Type = type;
				Priority = priority;
			}

			public override string ToString()
			{
				return Id;
			}
		}

		public static float DEFAULT_INITIAL_DELAY;

		public static float DEFAULT_OBJECTIVE_DELAY;

		public static float TIMEOUT_DELAY;

		public static readonly HashSet<string> EssentialObjectiveIds;

		private static List<ObjectiveDescription> m_objectiveDescriptions;

		private Dictionary<MyStringHash, MyIngameHelpObjective> m_availableObjectives = new Dictionary<MyStringHash, MyIngameHelpObjective>();

		private MyIngameHelpObjective m_currentObjective;

		private MyIngameHelpObjective m_nextObjective;

		private float m_currentDelayCounter = DEFAULT_INITIAL_DELAY;

		private float m_currentTimeoutCounter = TIMEOUT_DELAY;

		private float m_timeSinceLastObjective;

		private bool m_hintsEnabled = true;

		private MyCueId m_newObjectiveCue = MySoundPair.GetCueId("HudGPSNotification3");

		private MyCueId m_detailFinishedCue = MySoundPair.GetCueId("HudGPSNotification2");

		private MyCueId m_objectiveFinishedCue = MySoundPair.GetCueId("HudGPSNotification1");

		public IEnumerable<MyStringHash> AvailableObjectives => m_objectiveDescriptions.Select((ObjectiveDescription x) => MyStringHash.GetOrCompute(x.Id));

		public static void RegisterFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(IngameObjectiveAttribute), inherit: false);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					IngameObjectiveAttribute ingameObjectiveAttribute = (IngameObjectiveAttribute)customAttributes[j];
					m_objectiveDescriptions.Add(new ObjectiveDescription(ingameObjectiveAttribute.Id, type, ingameObjectiveAttribute.Priority));
				}
			}
			m_objectiveDescriptions.Sort((ObjectiveDescription x, ObjectiveDescription y) => x.Priority.CompareTo(y.Priority));
		}

		static MySessionComponentIngameHelp()
		{
			DEFAULT_INITIAL_DELAY = 5f;
			DEFAULT_OBJECTIVE_DELAY = 5f;
			TIMEOUT_DELAY = 120f;
			EssentialObjectiveIds = new HashSet<string>();
			m_objectiveDescriptions = new List<ObjectiveDescription>();
			EssentialObjectiveIds.Add("IngameHelp_Movement");
			EssentialObjectiveIds.Add("IngameHelp_Camera");
			EssentialObjectiveIds.Add("IngameHelp_Intro");
			EssentialObjectiveIds.Add("IngameHelp_Jetpack");
			RegisterFromAssembly(Assembly.GetAssembly(typeof(MySessionComponentIngameHelp)));
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_hintsEnabled = false;
				SetUpdateOrder(MyUpdateOrder.NoUpdate);
			}
			else
			{
				m_hintsEnabled = MySandboxGame.Config.GoodBotHints;
				Init();
			}
		}

		private void Init()
		{
			m_currentObjective = null;
			m_currentDelayCounter = DEFAULT_INITIAL_DELAY;
			m_availableObjectives.Clear();
			foreach (ObjectiveDescription objectiveDescription in m_objectiveDescriptions)
			{
				if (!MySandboxGame.Config.TutorialsFinished.Contains(objectiveDescription.Id))
				{
					MyIngameHelpObjective myIngameHelpObjective = (MyIngameHelpObjective)Activator.CreateInstance(objectiveDescription.Type);
					myIngameHelpObjective.Id = objectiveDescription.Id;
					m_availableObjectives.Add(MyStringHash.GetOrCompute(myIngameHelpObjective.Id), myIngameHelpObjective);
				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_hintsEnabled != MySandboxGame.Config.GoodBotHints)
			{
				m_hintsEnabled = MySandboxGame.Config.GoodBotHints;
				if (!m_hintsEnabled)
				{
					m_currentObjective = null;
					MyHud.Questlog.CleanDetails();
					MyHud.Questlog.Visible = false;
					return;
				}
			}
			if (!m_hintsEnabled || MySession.Static == null || !MySession.Static.Ready || !MySession.Static.Settings.EnableGoodBotHints || MyHud.Questlog.IsUsedByVisualScripting)
			{
				return;
			}
			if (MyGuiScreenGamePlay.ActiveGameplayScreen == null)
			{
				if (m_availableObjectives.Count > 0 && (m_currentObjective == null || !m_currentObjective.IsCritical()))
				{
					MyIngameHelpObjective myIngameHelpObjective = TryToFindObjective(onlyCritical: true);
					if (myIngameHelpObjective != null)
					{
						if (m_currentObjective != null)
						{
							CancelObjective();
						}
						SetObjective(myIngameHelpObjective);
						return;
					}
				}
				if (m_currentObjective == null && m_currentDelayCounter > 0f)
				{
					m_currentDelayCounter -= 0.0166666675f;
					if (m_currentDelayCounter < 0f)
					{
						m_currentDelayCounter = 0f;
						MyHud.Questlog.Visible = false;
					}
					return;
				}
			}
			if (MyGuiScreenGamePlay.ActiveGameplayScreen == null)
			{
				if (m_currentObjective != null && m_currentTimeoutCounter > 0f)
				{
					m_currentTimeoutCounter -= 0.0166666675f;
					if (m_currentTimeoutCounter <= 0f)
					{
						m_currentTimeoutCounter = 0f;
						m_currentDelayCounter = (float)TimeSpan.FromMinutes(5.0).TotalSeconds;
						CancelObjective();
						return;
					}
				}
				if (m_currentObjective == null && m_availableObjectives.Count > 0)
				{
					MyIngameHelpObjective myIngameHelpObjective2 = TryToFindObjective();
					if (myIngameHelpObjective2 != null)
					{
						SetObjective(myIngameHelpObjective2);
					}
				}
			}
			if (m_currentObjective != null)
			{
				bool flag = true;
				int num = 0;
				MyIngameHelpDetail[] details = m_currentObjective.Details;
				foreach (MyIngameHelpDetail myIngameHelpDetail in details)
				{
					if (myIngameHelpDetail.FinishCondition != null)
					{
						bool flag2 = myIngameHelpDetail.FinishCondition();
						if (flag2 && !MyHud.Questlog.IsCompleted(num))
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudObjectiveComplete);
							MyHud.Questlog.SetCompleted(num);
						}
						flag = (flag && flag2);
					}
					num++;
				}
				if (flag)
				{
					FinishObjective();
				}
			}
			else
			{
				m_timeSinceLastObjective += 0.0166666675f;
			}
		}

		public void ForceObjective(MyStringHash id)
		{
			if (m_currentObjective != null)
			{
				CancelObjective();
			}
			if (!m_availableObjectives.TryGetValue(id, out MyIngameHelpObjective value))
			{
				value = (MyIngameHelpObjective)Activator.CreateInstance(m_objectiveDescriptions.First((ObjectiveDescription x) => x.Id == id.String).Type);
			}
			SetObjective(value);
		}

		private void SetObjective(MyIngameHelpObjective objective)
		{
			MyAudio.Static.PlaySound(m_newObjectiveCue);
			objective.OnBeforeActivate();
			MyHud.Questlog.CleanDetails();
			MyHud.Questlog.Visible = true;
			MyHud.Questlog.QuestTitle = MyTexts.GetString(objective.TitleEnum);
			MyIngameHelpDetail[] details = objective.Details;
			foreach (MyIngameHelpDetail myIngameHelpDetail in details)
			{
				string value = (myIngameHelpDetail.Args == null) ? MyTexts.GetString(myIngameHelpDetail.TextEnum) : string.Format(MyTexts.GetString(myIngameHelpDetail.TextEnum), myIngameHelpDetail.Args);
				MyHud.Questlog.AddDetail(value, useTyping: true, myIngameHelpDetail.FinishCondition != null);
			}
			m_currentDelayCounter = objective.DelayToHide;
			m_currentObjective = objective;
			m_currentTimeoutCounter = TIMEOUT_DELAY;
			m_currentObjective.OnActivated();
		}

		private void FinishObjective()
		{
			MySandboxGame.Config.TutorialsFinished.Add(m_currentObjective.Id);
			MySandboxGame.Config.Save();
			m_availableObjectives.Remove(MyStringHash.GetOrCompute(m_currentObjective.Id));
			MyIngameHelpObjective value = null;
			MyAudio.Static.PlaySound(m_objectiveFinishedCue);
			if (!string.IsNullOrEmpty(m_currentObjective.FollowingId) && m_availableObjectives.TryGetValue(MyStringHash.GetOrCompute(m_currentObjective.FollowingId), out value))
			{
				m_nextObjective = value;
			}
			m_currentObjective = null;
			m_timeSinceLastObjective = 0f;
		}

		private void CancelObjective()
		{
			m_currentObjective = null;
			m_timeSinceLastObjective = 0f;
			MyHud.Questlog.Visible = false;
		}

		public bool TryCancelObjective()
		{
			m_currentDelayCounter = 0f;
			if (m_currentObjective == null)
			{
				return false;
			}
			CancelObjective();
			m_currentDelayCounter = 0f;
			return true;
		}

		private MyIngameHelpObjective TryToFindObjective(bool onlyCritical = false)
		{
			if (onlyCritical)
			{
				foreach (MyIngameHelpObjective value in m_availableObjectives.Values)
				{
					bool flag = true;
					if (value.RequiredIds != null)
					{
						string[] requiredIds = value.RequiredIds;
						foreach (string str in requiredIds)
						{
							if (m_availableObjectives.ContainsKey(MyStringHash.GetOrCompute(str)))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag && value.IsCritical())
					{
						return value;
					}
				}
				return null;
			}
			if (m_nextObjective != null)
			{
				MyIngameHelpObjective nextObjective = m_nextObjective;
				m_nextObjective = null;
				return nextObjective;
			}
			foreach (MyIngameHelpObjective value2 in m_availableObjectives.Values)
			{
				bool flag2 = true;
				if (value2.RequiredIds != null)
				{
					string[] requiredIds = value2.RequiredIds;
					foreach (string str2 in requiredIds)
					{
						if (m_availableObjectives.ContainsKey(MyStringHash.GetOrCompute(str2)))
						{
							flag2 = false;
							break;
						}
					}
				}
				if (!(value2.DelayToAppear > 0f) || !(m_timeSinceLastObjective >= value2.DelayToAppear))
				{
					if (value2.RequiredCondition != null)
					{
						flag2 &= value2.RequiredCondition();
					}
					else if (value2.DelayToAppear > 0f)
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					return value2;
				}
			}
			return null;
		}

		public void Reset()
		{
			MySandboxGame.Config.TutorialsFinished.Clear();
			MySandboxGame.Config.Save();
			Init();
		}

		internal static IReadOnlyList<MyIngameHelpObjective> GetFinishedObjectives()
		{
			List<MyIngameHelpObjective> list = new List<MyIngameHelpObjective>();
			foreach (string id in MySandboxGame.Config.TutorialsFinished)
			{
				ObjectiveDescription objectiveDescription = m_objectiveDescriptions.FirstOrDefault((ObjectiveDescription x) => x.Id == id);
				if (!string.IsNullOrEmpty(objectiveDescription.Id))
				{
					MyIngameHelpObjective myIngameHelpObjective = (MyIngameHelpObjective)Activator.CreateInstance(objectiveDescription.Type);
					myIngameHelpObjective.Id = objectiveDescription.Id;
					list.Add(myIngameHelpObjective);
				}
			}
			return list;
		}
	}
}
