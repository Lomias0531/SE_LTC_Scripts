using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI.BehaviorTree
{
	[GenerateActivator]
	[MyBehaviorTreeNodeType(typeof(MyObjectBuilder_BehaviorTreeNode))]
	public abstract class MyBehaviorTreeNode
	{
		protected static float DEBUG_TEXT_SCALE = 0.5f;

		protected static float DEBUG_TEXT_Y_OFFSET = 60f;

		protected static float DEBUG_SCALE = 0.4f;

		protected static float DEBUG_ROOT_OFFSET = 20f;

		protected static float DEBUG_LINE_OFFSET_MULT = 25f;

		public const string ParentName = "Par_N";

		public string m_runningActionName = "";

		public int MemoryIndex
		{
			get;
			private set;
		}

		public Type MemoryType
		{
			get;
			private set;
		}

		public abstract bool IsRunningStateSource
		{
			get;
		}

		public MyBehaviorTreeNode()
		{
			object[] customAttributes = GetType().GetCustomAttributes(inherit: false);
			foreach (object obj in customAttributes)
			{
				if (obj.GetType() == typeof(MyBehaviorTreeNodeTypeAttribute))
				{
					MyBehaviorTreeNodeTypeAttribute myBehaviorTreeNodeTypeAttribute = obj as MyBehaviorTreeNodeTypeAttribute;
					MemoryType = myBehaviorTreeNodeTypeAttribute.MemoryType;
				}
			}
		}

		public virtual void Construct(MyObjectBuilder_BehaviorTreeNode nodeDefinition, MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
		{
			MemoryIndex = treeDesc.MemorableNodesCounter++;
			treeDesc.Nodes.Add(this);
		}

		public abstract MyBehaviorTreeState Tick(IMyBot bot, MyPerTreeBotMemory nodesMemory);

		public virtual void PostTick(IMyBot bot, MyPerTreeBotMemory nodesMemory)
		{
		}

		public abstract void DebugDraw(Vector2 position, Vector2 size, List<MyBehaviorTreeNodeMemory> nodesMemory);

		public virtual MyBehaviorTreeNodeMemory GetNewMemoryObject()
		{
			if (MemoryType != null && (MemoryType.IsSubclassOf(typeof(MyBehaviorTreeNodeMemory)) || MemoryType == typeof(MyBehaviorTreeNodeMemory)))
			{
				return Activator.CreateInstance(MemoryType) as MyBehaviorTreeNodeMemory;
			}
			return new MyBehaviorTreeNodeMemory();
		}

		public override int GetHashCode()
		{
			return MemoryIndex;
		}
	}
}
