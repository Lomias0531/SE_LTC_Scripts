using Sandbox.Engine.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.BehaviorTree
{
	internal class MyBehaviorTreeRoot : MyBehaviorTreeNode
	{
		private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeRoot_003C_003EActor : IActivator, IActivator<MyBehaviorTreeRoot>
		{
			private sealed override object CreateInstance()
			{
				return new MyBehaviorTreeRoot();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBehaviorTreeRoot CreateInstance()
			{
				return new MyBehaviorTreeRoot();
			}

			MyBehaviorTreeRoot IActivator<MyBehaviorTreeRoot>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyBehaviorTreeNode m_child;

		public override bool IsRunningStateSource => false;

		public override void Construct(MyObjectBuilder_BehaviorTreeNode nodeDefinition, MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
		{
			base.Construct(nodeDefinition, treeDesc);
			m_child = MyBehaviorTreeNodeFactory.CreateBTNode(nodeDefinition);
			m_child.Construct(nodeDefinition, treeDesc);
		}

		public override MyBehaviorTreeState Tick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			bot.BotMemory.RememberNode(m_child.MemoryIndex);
			if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
			{
				bot.LastBotMemory = bot.BotMemory.Clone();
			}
			MyBehaviorTreeState myBehaviorTreeState = m_child.Tick(bot, botTreeMemory);
			botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex).NodeState = myBehaviorTreeState;
			if (myBehaviorTreeState != MyBehaviorTreeState.RUNNING)
			{
				bot.BotMemory.ForgetNode();
			}
			return myBehaviorTreeState;
		}

		[Conditional("DEBUG")]
		private void RecordRunningNodeName(IMyBot bot, MyBehaviorTreeState state)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_BOTS && bot is MyAgentBot)
			{
				switch (state)
				{
				case MyBehaviorTreeState.RUNNING:
					(bot as MyAgentBot).LastActions.AddLastAction(m_child.m_runningActionName);
					break;
				case MyBehaviorTreeState.ERROR:
					(bot as MyAgentBot).LastActions.AddLastAction("error");
					break;
				case MyBehaviorTreeState.FAILURE:
					(bot as MyAgentBot).LastActions.AddLastAction("failure");
					break;
				case MyBehaviorTreeState.SUCCESS:
					(bot as MyAgentBot).LastActions.AddLastAction("failure");
					break;
				case MyBehaviorTreeState.NOT_TICKED:
					(bot as MyAgentBot).LastActions.AddLastAction("not ticked");
					break;
				}
			}
		}

		public override void DebugDraw(Vector2 pos, Vector2 size, List<MyBehaviorTreeNodeMemory> nodesMemory)
		{
			MyRenderProxy.DebugDrawText2D(pos, "ROOT", nodesMemory[base.MemoryIndex].NodeStateColor, MyBehaviorTreeNode.DEBUG_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			pos.Y += MyBehaviorTreeNode.DEBUG_ROOT_OFFSET;
			m_child.DebugDraw(pos, size, nodesMemory);
		}

		public override MyBehaviorTreeNodeMemory GetNewMemoryObject()
		{
			return new MyBehaviorTreeNodeMemory();
		}

		public override int GetHashCode()
		{
			return m_child.GetHashCode();
		}
	}
}
