using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.ScriptBuilder.Nodes;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	internal class MyVisualScriptNavigator
	{
		private readonly Dictionary<int, MyVisualSyntaxNode> m_idToNode = new Dictionary<int, MyVisualSyntaxNode>();

		private readonly Dictionary<Type, List<MyVisualSyntaxNode>> m_nodesByType = new Dictionary<Type, List<MyVisualSyntaxNode>>();

		private readonly Dictionary<string, MyVisualSyntaxVariableNode> m_variablesByName = new Dictionary<string, MyVisualSyntaxVariableNode>();

		private readonly List<MyVisualSyntaxNode> m_freshNodes = new List<MyVisualSyntaxNode>();

		public List<MyVisualSyntaxNode> FreshNodes => m_freshNodes;

		public MyVisualScriptNavigator(MyObjectBuilder_VisualScript scriptOb)
		{
			Type type = string.IsNullOrEmpty(scriptOb.Interface) ? null : MyVisualScriptingProxy.GetType(scriptOb.Interface);
			foreach (MyObjectBuilder_ScriptNode node in scriptOb.Nodes)
			{
				MyVisualSyntaxNode myVisualSyntaxNode;
				if (node is MyObjectBuilder_NewListScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxNewListNode(node);
				}
				else if (node is MyObjectBuilder_SwitchScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxSwitchNode(node);
				}
				else if (node is MyObjectBuilder_LocalizationScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxLocalizationNode(node);
				}
				else if (node is MyObjectBuilder_LogicGateScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxLogicGateNode(node);
				}
				else if (node is MyObjectBuilder_ForLoopScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxForLoopNode(node);
				}
				else if (node is MyObjectBuilder_SequenceScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxSequenceNode(node);
				}
				else if (node is MyObjectBuilder_ArithmeticScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxArithmeticNode(node);
				}
				else if (node is MyObjectBuilder_InterfaceMethodNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxInterfaceMethodNode(node, type);
				}
				else if (node is MyObjectBuilder_KeyEventScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxKeyEventNode(node);
				}
				else if (node is MyObjectBuilder_BranchingScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxBranchingNode(node);
				}
				else if (node is MyObjectBuilder_InputScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxInputNode(node);
				}
				else if (node is MyObjectBuilder_CastScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxCastNode(node);
				}
				else if (node is MyObjectBuilder_EventScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxEventNode(node);
				}
				else if (node is MyObjectBuilder_FunctionScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxFunctionNode(node, type);
				}
				else if (node is MyObjectBuilder_VariableSetterScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxSetterNode(node);
				}
				else if (node is MyObjectBuilder_TriggerScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxTriggerNode(node);
				}
				else if (node is MyObjectBuilder_VariableScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxVariableNode(node);
				}
				else if (node is MyObjectBuilder_ConstantScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxConstantNode(node);
				}
				else if (node is MyObjectBuilder_GetterScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxGetterNode(node);
				}
				else if (node is MyObjectBuilder_OutputScriptNode)
				{
					myVisualSyntaxNode = new MyVisualSyntaxOutputNode(node);
				}
				else
				{
					if (!(node is MyObjectBuilder_ScriptScriptNode))
					{
						continue;
					}
					myVisualSyntaxNode = new MyVisualSyntaxScriptNode(node);
				}
				myVisualSyntaxNode.Navigator = this;
				m_idToNode.Add(node.ID, myVisualSyntaxNode);
				Type type2 = myVisualSyntaxNode.GetType();
				if (!m_nodesByType.ContainsKey(type2))
				{
					m_nodesByType.Add(type2, new List<MyVisualSyntaxNode>());
				}
				m_nodesByType[type2].Add(myVisualSyntaxNode);
				if (type2 == typeof(MyVisualSyntaxVariableNode))
				{
					m_variablesByName.Add(((MyObjectBuilder_VariableScriptNode)node).VariableName, (MyVisualSyntaxVariableNode)myVisualSyntaxNode);
				}
			}
		}

		public MyVisualSyntaxNode GetNodeByID(int id)
		{
			m_idToNode.TryGetValue(id, out MyVisualSyntaxNode value);
			return value;
		}

		public List<T> OfType<T>() where T : MyVisualSyntaxNode
		{
			List<MyVisualSyntaxNode> list = new List<MyVisualSyntaxNode>();
			foreach (KeyValuePair<Type, List<MyVisualSyntaxNode>> item in m_nodesByType)
			{
				if (typeof(T) == item.Key)
				{
					list.AddRange(item.Value);
				}
			}
			return list.ConvertAll((MyVisualSyntaxNode node) => (T)node);
		}

		public void ResetNodes()
		{
			foreach (KeyValuePair<int, MyVisualSyntaxNode> item in m_idToNode)
			{
				item.Value.Reset();
			}
		}

		public MyVisualSyntaxVariableNode GetVariable(string name)
		{
			m_variablesByName.TryGetValue(name, out MyVisualSyntaxVariableNode value);
			return value;
		}
	}
}
