using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeType(typeof(MyObjectBuilder_BehaviorControlBaseNode), typeof(MyBehaviorTreeControlNodeMemory))]
	internal abstract class MyBehaviorTreeControlBaseNode : MyBehaviorTreeNode
	{
		protected List<MyBehaviorTreeNode> m_children;

		protected bool m_isMemorable;

		protected string m_name;

		public abstract MyBehaviorTreeState SearchedValue
		{
			get;
		}

		public abstract MyBehaviorTreeState FinalValue
		{
			get;
		}

		public abstract string DebugSign
		{
			get;
		}

		public override bool IsRunningStateSource => false;

		public override void Construct(MyObjectBuilder_BehaviorTreeNode nodeDefinition, MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
		{
			base.Construct(nodeDefinition, treeDesc);
			MyObjectBuilder_BehaviorControlBaseNode myObjectBuilder_BehaviorControlBaseNode = (MyObjectBuilder_BehaviorControlBaseNode)nodeDefinition;
			m_children = new List<MyBehaviorTreeNode>(myObjectBuilder_BehaviorControlBaseNode.BTNodes.Length);
			m_isMemorable = myObjectBuilder_BehaviorControlBaseNode.IsMemorable;
			m_name = myObjectBuilder_BehaviorControlBaseNode.Name;
			MyObjectBuilder_BehaviorTreeNode[] bTNodes = myObjectBuilder_BehaviorControlBaseNode.BTNodes;
			foreach (MyObjectBuilder_BehaviorTreeNode myObjectBuilder_BehaviorTreeNode in bTNodes)
			{
				MyBehaviorTreeNode myBehaviorTreeNode = MyBehaviorTreeNodeFactory.CreateBTNode(myObjectBuilder_BehaviorTreeNode);
				myBehaviorTreeNode.Construct(myObjectBuilder_BehaviorTreeNode, treeDesc);
				m_children.Add(myBehaviorTreeNode);
			}
		}

		public override MyBehaviorTreeState Tick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			MyBehaviorTreeControlNodeMemory myBehaviorTreeControlNodeMemory = botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex) as MyBehaviorTreeControlNodeMemory;
			for (int i = myBehaviorTreeControlNodeMemory.InitialIndex; i < m_children.Count; i++)
			{
				bot.BotMemory.RememberNode(m_children[i].MemoryIndex);
				if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
				{
					if (!(m_children[i] is MyBehaviorTreeControlBaseNode))
					{
						if (!(m_children[i] is MyBehaviorTreeActionNode))
						{
							if (m_children[i] is MyBehaviorTreeDecoratorNode)
							{
								(m_children[i] as MyBehaviorTreeDecoratorNode).GetName();
							}
						}
						else
						{
							(m_children[i] as MyBehaviorTreeActionNode).GetActionName();
						}
					}
					else
					{
						_ = (m_children[i] as MyBehaviorTreeControlBaseNode).m_name;
					}
					m_runningActionName = "";
				}
				MyBehaviorTreeState myBehaviorTreeState = m_children[i].Tick(bot, botTreeMemory);
				if (myBehaviorTreeState == SearchedValue || myBehaviorTreeState == FinalValue)
				{
					m_children[i].PostTick(bot, botTreeMemory);
				}
				if (myBehaviorTreeState == MyBehaviorTreeState.RUNNING || myBehaviorTreeState == SearchedValue)
				{
					myBehaviorTreeControlNodeMemory.NodeState = myBehaviorTreeState;
					if (myBehaviorTreeState == MyBehaviorTreeState.RUNNING)
					{
						if (m_isMemorable)
						{
							myBehaviorTreeControlNodeMemory.InitialIndex = i;
						}
					}
					else
					{
						bot.BotMemory.ForgetNode();
					}
					return myBehaviorTreeState;
				}
				bot.BotMemory.ForgetNode();
			}
			myBehaviorTreeControlNodeMemory.NodeState = FinalValue;
			myBehaviorTreeControlNodeMemory.InitialIndex = 0;
			return FinalValue;
		}

		[Conditional("DEBUG")]
		private void RecordRunningNodeName(MyBehaviorTreeState state, MyBehaviorTreeNode node)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_BOTS)
			{
				return;
			}
			m_runningActionName = "";
			if (state != MyBehaviorTreeState.RUNNING)
			{
				return;
			}
			if (node is MyBehaviorTreeActionNode)
			{
				MyBehaviorTreeActionNode myBehaviorTreeActionNode = (MyBehaviorTreeActionNode)node;
				m_runningActionName = myBehaviorTreeActionNode.GetActionName();
				return;
			}
			string text = node.m_runningActionName;
			if (text.Contains("Par_N"))
			{
				text = text.Replace("Par_N", m_name + "-");
			}
			m_runningActionName = text;
		}

		public override void PostTick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
		{
			botTreeMemory.GetNodeMemoryByIndex(base.MemoryIndex).PostTickMemory();
			foreach (MyBehaviorTreeNode child in m_children)
			{
				child.PostTick(bot, botTreeMemory);
			}
		}

		public override void DebugDraw(Vector2 pos, Vector2 size, List<MyBehaviorTreeNodeMemory> nodesMemory)
		{
			MyRenderProxy.DebugDrawText2D(pos, DebugSign, nodesMemory[base.MemoryIndex].NodeStateColor, MyBehaviorTreeNode.DEBUG_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			size.X *= MyBehaviorTreeNode.DEBUG_SCALE;
			Vector2 vector = (m_children.Count > 1) ? (pos - size * 0.5f) : pos;
			vector.Y += MyBehaviorTreeNode.DEBUG_TEXT_Y_OFFSET;
			size.X /= Math.Max(m_children.Count - 1, 1);
			foreach (MyBehaviorTreeNode child in m_children)
			{
				Vector2 value = vector - pos;
				value.Normalize();
				Vector2 pointFrom = pos + value * MyBehaviorTreeNode.DEBUG_LINE_OFFSET_MULT;
				Vector2 pointTo = vector - value * MyBehaviorTreeNode.DEBUG_LINE_OFFSET_MULT;
				MyRenderProxy.DebugDrawLine2D(pointFrom, pointTo, nodesMemory[child.MemoryIndex].NodeStateColor, nodesMemory[child.MemoryIndex].NodeStateColor);
				child.DebugDraw(vector, size, nodesMemory);
				vector.X += size.X;
			}
		}

		public override int GetHashCode()
		{
			int hashCode = base.GetHashCode();
			hashCode = ((hashCode * 397) ^ m_isMemorable.GetHashCode());
			hashCode = ((hashCode * 397) ^ SearchedValue.GetHashCode());
			hashCode = ((hashCode * 397) ^ FinalValue.GetHashCode());
			for (int i = 0; i < m_children.Count; i++)
			{
				hashCode = ((hashCode * 397) ^ m_children[i].GetHashCode());
			}
			return hashCode;
		}

		public override string ToString()
		{
			return m_name;
		}
	}
}
