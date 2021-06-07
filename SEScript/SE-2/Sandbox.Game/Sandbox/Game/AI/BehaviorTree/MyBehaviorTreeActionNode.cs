using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeType(typeof(MyObjectBuilder_BehaviorTreeActionNode), typeof(MyBehaviorTreeNodeMemory))]
	internal class MyBehaviorTreeActionNode : MyBehaviorTreeNode
	{
		private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeActionNode_003C_003EActor : IActivator, IActivator<MyBehaviorTreeActionNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyBehaviorTreeActionNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBehaviorTreeActionNode CreateInstance()
			{
				return new MyBehaviorTreeActionNode();
			}

			MyBehaviorTreeActionNode IActivator<MyBehaviorTreeActionNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyStringId m_actionName;

		private object[] m_parameters;

		public bool ReturnsRunning
		{
			get;
			private set;
		}

		public override bool IsRunningStateSource => ReturnsRunning;

		public MyBehaviorTreeActionNode()
		{
			m_actionName = MyStringId.NullOrEmpty;
			m_parameters = null;
			ReturnsRunning = true;
		}

		public override void Construct(MyObjectBuilder_BehaviorTreeNode nodeDefinition, MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
		{
			base.Construct(nodeDefinition, treeDesc);
			MyObjectBuilder_BehaviorTreeActionNode myObjectBuilder_BehaviorTreeActionNode = (MyObjectBuilder_BehaviorTreeActionNode)nodeDefinition;
			if (!string.IsNullOrEmpty(myObjectBuilder_BehaviorTreeActionNode.ActionName))
			{
				m_actionName = MyStringId.GetOrCompute(myObjectBuilder_BehaviorTreeActionNode.ActionName);
				treeDesc.ActionIds.Add(m_actionName);
			}
			if (myObjectBuilder_BehaviorTreeActionNode.Parameters == null)
			{
				return;
			}
			MyObjectBuilder_BehaviorTreeActionNode.TypeValue[] parameters = myObjectBuilder_BehaviorTreeActionNode.Parameters;
			m_parameters = new object[parameters.Length];
			for (int i = 0; i < m_parameters.Length; i++)
			{
				MyObjectBuilder_BehaviorTreeActionNode.TypeValue typeValue = parameters[i];
				if (typeValue is MyObjectBuilder_BehaviorTreeActionNode.MemType)
				{
					string str = (string)typeValue.GetValue();
					m_parameters[i] = (Boxed<MyStringId>)MyStringId.GetOrCompute(str);
				}
				else
				{
					m_parameters[i] = typeValue.GetValue();
				}
			}
		}

		public override MyBehaviorTreeState Tick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			if (bot.ActionCollection.ReturnsRunning(m_actionName))
			{
				bot.BotMemory.ProcessLastRunningNode(this);
			}
			MyBehaviorTreeNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex);
			if (!nodeMemoryByIndex.InitCalled)
			{
				nodeMemoryByIndex.InitCalled = true;
				if (bot.ActionCollection.ContainsInitAction(m_actionName))
				{
					bot.ActionCollection.PerformInitAction(bot, m_actionName);
				}
			}
			return nodeMemoryByIndex.NodeState = bot.ActionCollection.PerformAction(bot, m_actionName, m_parameters);
		}

		public override void PostTick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			MyBehaviorTreeNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex);
			if (nodeMemoryByIndex.InitCalled)
			{
				if (bot.ActionCollection.ContainsPostAction(m_actionName))
				{
					bot.ActionCollection.PerformPostAction(bot, m_actionName);
				}
				nodeMemoryByIndex.InitCalled = false;
			}
		}

		public override void DebugDraw(Vector2 position, Vector2 size, List<MyBehaviorTreeNodeMemory> nodesMemory)
		{
			MyRenderProxy.DebugDrawText2D(position, "A:" + m_actionName.ToString(), nodesMemory[base.MemoryIndex].NodeStateColor, MyBehaviorTreeNode.DEBUG_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}

		public override int GetHashCode()
		{
			int hashCode = base.GetHashCode();
			hashCode = ((hashCode * 397) ^ m_actionName.ToString().GetHashCode());
			if (m_parameters != null)
			{
				object[] parameters = m_parameters;
				foreach (object obj in parameters)
				{
					hashCode = ((hashCode * 397) ^ obj.ToString().GetHashCode());
				}
			}
			return hashCode;
		}

		public override string ToString()
		{
			return "ACTION: " + m_actionName.ToString();
		}

		public string GetActionName()
		{
			return m_actionName.ToString();
		}
	}
}
