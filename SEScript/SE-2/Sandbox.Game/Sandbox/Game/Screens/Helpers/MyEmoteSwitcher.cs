using Sandbox.Definitions;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyEmoteSwitcher : MySessionComponentBase
	{
		private struct MyPrioritizedDefinition
		{
			public int Priority;

			public MyDefinitionBase Definition;
		}

		private class MyPrioritizedComparer : IComparer<MyPrioritizedDefinition>
		{
			public static MyPrioritizedComparer Static = new MyPrioritizedComparer();

			private MyPrioritizedComparer()
			{
			}

			public int Compare(MyPrioritizedDefinition x, MyPrioritizedDefinition y)
			{
				return y.Priority.CompareTo(x.Priority);
			}
		}

		private static readonly int PAGE_SIZE = 4;

		private List<MyPrioritizedDefinition> m_animations = new List<MyPrioritizedDefinition>();

		private int m_currentPage;

		private bool m_isActive;

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			private set
			{
				if (m_isActive != value)
				{
					m_isActive = value;
					this.OnActiveStateChanged?.Invoke();
				}
			}
		}

		public int AnimationCount
		{
			get;
			private set;
		}

		public int AnimationPageCount
		{
			get;
			private set;
		}

		public int CurrentPage
		{
			get
			{
				return m_currentPage;
			}
			private set
			{
				if (m_currentPage != value)
				{
					if (value < 0)
					{
						m_currentPage = 0;
					}
					else if (value < AnimationPageCount)
					{
						m_currentPage = value;
					}
					else if (AnimationPageCount >= 0)
					{
						m_currentPage = AnimationPageCount - 1;
					}
					else
					{
						m_currentPage = 0;
					}
					this.OnPageChanged?.Invoke();
				}
			}
		}

		public event Action OnActiveStateChanged;

		public event Action OnPageChanged;

		public MyEmoteSwitcher()
		{
			InitializeAnimationList();
		}

		public override void HandleInput()
		{
			MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (!MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER, MyControlStateType.PRESSED))
			{
				IsActive = false;
				return;
			}
			IsActive = true;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER_LEFT))
			{
				PreviousPage();
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER_RIGHT))
			{
				NextPage();
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_1))
			{
				ActivateEmote(0);
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_2))
			{
				ActivateEmote(1);
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_3))
			{
				ActivateEmote(2);
			}
			if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_4))
			{
				ActivateEmote(3);
			}
		}

		private void InitializeAnimationList()
		{
			m_animations.Clear();
			MyPrioritizedDefinition item;
			foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (animationDefinition.Public)
				{
					List<MyPrioritizedDefinition> animations = m_animations;
					item = new MyPrioritizedDefinition
					{
						Definition = animationDefinition,
						Priority = animationDefinition.Priority
					};
					animations.Add(item);
				}
			}
			foreach (MyEmoteDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyEmoteDefinition>())
			{
				if (definition.Public)
				{
					List<MyPrioritizedDefinition> animations2 = m_animations;
					item = new MyPrioritizedDefinition
					{
						Definition = definition,
						Priority = definition.Priority
					};
					animations2.Add(item);
				}
			}
			m_animations.Sort(MyPrioritizedComparer.Static);
			AnimationCount = m_animations.Count;
			AnimationPageCount = ((AnimationCount % PAGE_SIZE == 0) ? (AnimationCount / PAGE_SIZE) : (AnimationCount / PAGE_SIZE + 1));
			CurrentPage = 0;
		}

		public string GetIconUp()
		{
			return GetIcon(0);
		}

		public string GetIconLeft()
		{
			return GetIcon(1);
		}

		public string GetIconRight()
		{
			return GetIcon(2);
		}

		public string GetIconDown()
		{
			return GetIcon(3);
		}

		public string GetIcon(int id)
		{
			int linearIndex = LinearizeIndex(id);
			return GetIconLinear(linearIndex);
		}

		private void NextPage()
		{
			CurrentPage++;
		}

		private void PreviousPage()
		{
			CurrentPage--;
		}

		public string GetIconLinear(int linearIndex)
		{
			if (linearIndex < 0 || linearIndex >= AnimationCount)
			{
				return string.Empty;
			}
			MyAnimationDefinition myAnimationDefinition;
			if ((myAnimationDefinition = (m_animations[linearIndex].Definition as MyAnimationDefinition)) != null)
			{
				if (myAnimationDefinition.Icons.Length == 0)
				{
					return string.Empty;
				}
				return myAnimationDefinition.Icons[0];
			}
			MyEmoteDefinition myEmoteDefinition;
			if ((myEmoteDefinition = (m_animations[linearIndex].Definition as MyEmoteDefinition)) != null)
			{
				if (myEmoteDefinition.Icons.Length == 0)
				{
					return string.Empty;
				}
				return myEmoteDefinition.Icons[0];
			}
			return string.Empty;
		}

		private void ActivateEmote(int id)
		{
			int linearIndex = LinearizeIndex(id);
			ActivateEmoteLinear(linearIndex);
		}

		private void ActivateEmoteLinear(int linearIndex)
		{
			if (linearIndex >= 0 && linearIndex < AnimationCount)
			{
				MySession.Static.ControlledEntity.SwitchToWeapon(null);
				MyAnimationDefinition animationDefinition;
				MyEmoteDefinition emoteDefinition;
				if ((animationDefinition = (m_animations[linearIndex].Definition as MyAnimationDefinition)) != null)
				{
					MyAnimationActivator.Activate(animationDefinition);
				}
				else if ((emoteDefinition = (m_animations[linearIndex].Definition as MyEmoteDefinition)) != null)
				{
					MyAnimationActivator.Activate(emoteDefinition);
				}
			}
		}

		private int LinearizeIndex(int id)
		{
			return m_currentPage * PAGE_SIZE + id;
		}
	}
}
