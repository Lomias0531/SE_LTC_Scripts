using Sandbox.Engine.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeType(typeof(MyObjectBuilder_BehaviorTreeDecoratorNode), typeof(MyBehaviorTreeDecoratorNodeMemory))]
	public class MyBehaviorTreeDecoratorNode : MyBehaviorTreeNode
	{
		private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeDecoratorNode_003C_003EActor : IActivator, IActivator<MyBehaviorTreeDecoratorNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyBehaviorTreeDecoratorNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBehaviorTreeDecoratorNode CreateInstance()
			{
				return new MyBehaviorTreeDecoratorNode();
			}

			MyBehaviorTreeDecoratorNode IActivator<MyBehaviorTreeDecoratorNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyBehaviorTreeNode m_child;

		private IMyDecoratorLogic m_decoratorLogic;

		private MyBehaviorTreeState m_defaultReturnValue;

		private string m_decoratorLogicName;

		private MyDecoratorDefaultReturnValues DecoratorDefaultReturnValue => (MyDecoratorDefaultReturnValues)m_defaultReturnValue;

		public override bool IsRunningStateSource => m_defaultReturnValue == MyBehaviorTreeState.RUNNING;

		public string GetName()
		{
			return m_decoratorLogicName;
		}

		public MyBehaviorTreeDecoratorNode()
		{
			m_child = null;
			m_decoratorLogic = null;
		}

		public override void Construct(MyObjectBuilder_BehaviorTreeNode nodeDefinition, MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
		{
			base.Construct(nodeDefinition, treeDesc);
			MyObjectBuilder_BehaviorTreeDecoratorNode myObjectBuilder_BehaviorTreeDecoratorNode = nodeDefinition as MyObjectBuilder_BehaviorTreeDecoratorNode;
			m_defaultReturnValue = (MyBehaviorTreeState)myObjectBuilder_BehaviorTreeDecoratorNode.DefaultReturnValue;
			m_decoratorLogicName = myObjectBuilder_BehaviorTreeDecoratorNode.DecoratorLogic.GetType().Name;
			m_decoratorLogic = GetDecoratorLogic(myObjectBuilder_BehaviorTreeDecoratorNode.DecoratorLogic);
			m_decoratorLogic.Construct(myObjectBuilder_BehaviorTreeDecoratorNode.DecoratorLogic);
			if (myObjectBuilder_BehaviorTreeDecoratorNode.BTNode != null)
			{
				m_child = MyBehaviorTreeNodeFactory.CreateBTNode(myObjectBuilder_BehaviorTreeDecoratorNode.BTNode);
				m_child.Construct(myObjectBuilder_BehaviorTreeDecoratorNode.BTNode, treeDesc);
			}
		}

		public override MyBehaviorTreeState Tick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			MyBehaviorTreeDecoratorNodeMemory myBehaviorTreeDecoratorNodeMemory = botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex) as MyBehaviorTreeDecoratorNodeMemory;
			if (m_child == null)
			{
				return m_defaultReturnValue;
			}
			if (myBehaviorTreeDecoratorNodeMemory.ChildState != MyBehaviorTreeState.RUNNING)
			{
				m_decoratorLogic.Update(myBehaviorTreeDecoratorNodeMemory.DecoratorLogicMemory);
				if (m_decoratorLogic.CanRun(myBehaviorTreeDecoratorNodeMemory.DecoratorLogicMemory))
				{
					return TickChild(bot, botTreeMemory, myBehaviorTreeDecoratorNodeMemory);
				}
				if (IsRunningStateSource)
				{
					bot.BotMemory.ProcessLastRunningNode(this);
				}
				botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex).NodeState = m_defaultReturnValue;
				if (MyDebugDrawSettings.DEBUG_DRAW_BOTS && m_defaultReturnValue == MyBehaviorTreeState.RUNNING)
				{
					m_runningActionName = "Par_N" + m_decoratorLogicName;
				}
				return m_defaultReturnValue;
			}
			return TickChild(bot, botTreeMemory, myBehaviorTreeDecoratorNodeMemory);
		}

		private MyBehaviorTreeState TickChild(IMyBot bot, MyPerTreeBotMemory botTreeMemory, MyBehaviorTreeDecoratorNodeMemory thisMemory)
		{
			bot.BotMemory.RememberNode(m_child.MemoryIndex);
			MyBehaviorTreeState myBehaviorTreeState2 = thisMemory.NodeState = m_child.Tick(bot, botTreeMemory);
			thisMemory.ChildState = myBehaviorTreeState2;
			if (myBehaviorTreeState2 != MyBehaviorTreeState.RUNNING)
			{
				bot.BotMemory.ForgetNode();
			}
			return myBehaviorTreeState2;
		}

		[Conditional("DEBUG")]
		private void RecordRunningNodeName(MyBehaviorTreeState state)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_BOTS && state == MyBehaviorTreeState.RUNNING)
			{
				m_runningActionName = m_child.m_runningActionName;
			}
		}

		public override void PostTick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			base.PostTick(bot, botTreeMemory);
			MyBehaviorTreeDecoratorNodeMemory myBehaviorTreeDecoratorNodeMemory = botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex) as MyBehaviorTreeDecoratorNodeMemory;
			if (myBehaviorTreeDecoratorNodeMemory.ChildState != 0)
			{
				myBehaviorTreeDecoratorNodeMemory.PostTickMemory();
				if (m_child != null)
				{
					m_child.PostTick(bot, botTreeMemory);
				}
			}
			else if (IsRunningStateSource)
			{
				myBehaviorTreeDecoratorNodeMemory.PostTickMemory();
			}
		}

		public override void DebugDraw(Vector2 position, Vector2 size, List<MyBehaviorTreeNodeMemory> nodesMemory)
		{
		}

		private static IMyDecoratorLogic GetDecoratorLogic(MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData)
		{
			if (logicData is MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic)
			{
				return new MyBehaviorTreeDecoratorTimerLogic();
			}
			if (logicData is MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic)
			{
				return new MyBehaviorTreeDecoratorCounterLogic();
			}
			return null;
		}

		public override MyBehaviorTreeNodeMemory GetNewMemoryObject()
		{
			MyBehaviorTreeDecoratorNodeMemory obj = base.GetNewMemoryObject() as MyBehaviorTreeDecoratorNodeMemory;
			obj.DecoratorLogicMemory = m_decoratorLogic.GetNewMemoryObject();
			return obj;
		}

		public override int GetHashCode()
		{
			return (((((((base.GetHashCode() * 397) ^ m_child.GetHashCode()) * 397) ^ m_decoratorLogic.GetHashCode()) * 397) ^ m_decoratorLogicName.GetHashCode()) * 397) ^ DecoratorDefaultReturnValue.GetHashCode();
		}

		public override string ToString()
		{
			return "DEC: " + m_decoratorLogic.ToString();
		}
	}
}
