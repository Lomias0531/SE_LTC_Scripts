using Sandbox.Game.AI.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Game;
using VRage.Game.AI;
using VRage.Utils;

namespace Sandbox.Game.AI
{
	public class ActionCollection
	{
		public class BotActionDesc
		{
			public Action<IMyBot> InitAction;

			public object[] ActionParams;

			public Dictionary<int, MyTuple<Type, MyMemoryParameterType>> ParametersDesc;

			public Func<IMyBot, object[], MyBehaviorTreeState> _Action;

			public Action<IMyBot> PostAction;

			public bool ReturnsRunning;
		}

		private Dictionary<MyStringId, BotActionDesc> m_actions = new Dictionary<MyStringId, BotActionDesc>(MyStringId.Comparer);

		private ActionCollection()
		{
		}

		public void AddInitAction(string actionName, Action<IMyBot> action)
		{
			AddInitAction(MyStringId.GetOrCompute(actionName), action);
		}

		public void AddInitAction(MyStringId actionName, Action<IMyBot> action)
		{
			if (!m_actions.ContainsKey(actionName))
			{
				AddBotActionDesc(actionName);
			}
			m_actions[actionName].InitAction = action;
		}

		public void AddAction(string actionName, MethodInfo methodInfo, bool returnsRunning, Func<IMyBot, object[], MyBehaviorTreeState> action)
		{
			AddAction(MyStringId.GetOrCompute(actionName), methodInfo, returnsRunning, action);
		}

		public void AddAction(MyStringId actionId, MethodInfo methodInfo, bool returnsRunning, Func<IMyBot, object[], MyBehaviorTreeState> action)
		{
			if (!m_actions.ContainsKey(actionId))
			{
				AddBotActionDesc(actionId);
			}
			BotActionDesc botActionDesc = m_actions[actionId];
			ParameterInfo[] parameters = methodInfo.GetParameters();
			botActionDesc._Action = action;
			botActionDesc.ActionParams = new object[parameters.Length];
			botActionDesc.ParametersDesc = new Dictionary<int, MyTuple<Type, MyMemoryParameterType>>();
			botActionDesc.ReturnsRunning = returnsRunning;
			for (int i = 0; i < parameters.Length; i++)
			{
				BTMemParamAttribute customAttribute = parameters[i].GetCustomAttribute<BTMemParamAttribute>(inherit: true);
				if (customAttribute != null)
				{
					botActionDesc.ParametersDesc.Add(i, new MyTuple<Type, MyMemoryParameterType>(parameters[i].ParameterType.GetElementType(), customAttribute.MemoryType));
				}
			}
		}

		public void AddPostAction(string actionName, Action<IMyBot> action)
		{
			AddPostAction(MyStringId.GetOrCompute(actionName), action);
		}

		public void AddPostAction(MyStringId actionId, Action<IMyBot> action)
		{
			if (!m_actions.ContainsKey(actionId))
			{
				AddBotActionDesc(actionId);
			}
			m_actions[actionId].PostAction = action;
		}

		private void AddBotActionDesc(MyStringId actionId)
		{
			m_actions.Add(actionId, new BotActionDesc());
		}

		public void PerformInitAction(IMyBot bot, MyStringId actionId)
		{
			m_actions[actionId]?.InitAction(bot);
		}

		public MyBehaviorTreeState PerformAction(IMyBot bot, MyStringId actionId, object[] args)
		{
			BotActionDesc botActionDesc = m_actions[actionId];
			if (botActionDesc == null)
			{
				return MyBehaviorTreeState.ERROR;
			}
			MyPerTreeBotMemory currentTreeBotMemory = bot.BotMemory.CurrentTreeBotMemory;
			if (botActionDesc.ParametersDesc.Count == 0)
			{
				return botActionDesc._Action(bot, args);
			}
			if (args == null)
			{
				return MyBehaviorTreeState.FAILURE;
			}
			LoadActionParams(botActionDesc, args, currentTreeBotMemory);
			MyBehaviorTreeState result = botActionDesc._Action(bot, botActionDesc.ActionParams);
			SaveActionParams(botActionDesc, args, currentTreeBotMemory);
			return result;
		}

		private void LoadActionParams(BotActionDesc action, object[] args, MyPerTreeBotMemory botMemory)
		{
			for (int i = 0; i < args.Length; i++)
			{
				object obj = args[i];
				if (obj is Boxed<MyStringId> && action.ParametersDesc.ContainsKey(i))
				{
					MyTuple<Type, MyMemoryParameterType> myTuple = action.ParametersDesc[i];
					Boxed<MyStringId> box = obj as Boxed<MyStringId>;
					MyBBMemoryValue value = null;
					if (botMemory.TryGetFromBlackboard(box, out value))
					{
						if (value == null || (value.GetType() == myTuple.Item1 && myTuple.Item2 != MyMemoryParameterType.OUT))
						{
							action.ActionParams[i] = value;
							continue;
						}
						_ = (value.GetType() != myTuple.Item1);
						action.ActionParams[i] = null;
					}
					else
					{
						action.ActionParams[i] = null;
					}
				}
				else
				{
					action.ActionParams[i] = obj;
				}
			}
		}

		private void SaveActionParams(BotActionDesc action, object[] args, MyPerTreeBotMemory botMemory)
		{
			foreach (int key in action.ParametersDesc.Keys)
			{
				MyStringId id = args[key] as Boxed<MyStringId>;
				if (action.ParametersDesc[key].Item2 != MyMemoryParameterType.IN)
				{
					botMemory.SaveToBlackboard(id, action.ActionParams[key] as MyBBMemoryValue);
				}
			}
		}

		public void PerformPostAction(IMyBot bot, MyStringId actionId)
		{
			m_actions[actionId]?.PostAction(bot);
		}

		public bool ContainsInitAction(MyStringId actionId)
		{
			return m_actions[actionId].InitAction != null;
		}

		public bool ContainsPostAction(MyStringId actionId)
		{
			return m_actions[actionId].PostAction != null;
		}

		public bool ContainsAction(MyStringId actionId)
		{
			return m_actions[actionId]._Action != null;
		}

		public bool ContainsActionDesc(MyStringId actionId)
		{
			return m_actions.ContainsKey(actionId);
		}

		public bool ReturnsRunning(MyStringId actionId)
		{
			return m_actions[actionId].ReturnsRunning;
		}

		public static ActionCollection CreateActionCollection(IMyBot bot)
		{
			ActionCollection actionCollection = new ActionCollection();
			MethodInfo[] methods = bot.BotActions.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				ExtractAction(actionCollection, methodInfo);
			}
			return actionCollection;
		}

		private static void ExtractAction(ActionCollection actions, MethodInfo methodInfo)
		{
			MyBehaviorTreeActionAttribute customAttribute = methodInfo.GetCustomAttribute<MyBehaviorTreeActionAttribute>();
			if (customAttribute != null)
			{
				switch (customAttribute.ActionType)
				{
				case MyBehaviorTreeActionType.INIT:
					actions.AddInitAction(customAttribute.ActionName, delegate(IMyBot x)
					{
						methodInfo.Invoke(x.BotActions, null);
					});
					break;
				case MyBehaviorTreeActionType.BODY:
					actions.AddAction(customAttribute.ActionName, methodInfo, customAttribute.ReturnsRunning, (IMyBot x, object[] y) => (MyBehaviorTreeState)methodInfo.Invoke(x.BotActions, y));
					break;
				case MyBehaviorTreeActionType.POST:
					actions.AddPostAction(customAttribute.ActionName, delegate(IMyBot x)
					{
						methodInfo.Invoke(x.BotActions, null);
					});
					break;
				}
			}
		}
	}
}
