using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxArithmeticNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_inputANode;

		private MyVisualSyntaxNode m_inputBNode;

		public new MyObjectBuilder_ArithmeticScriptNode ObjectBuilder => (MyObjectBuilder_ArithmeticScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public MyVisualSyntaxArithmeticNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			string text = null;
			string text2 = null;
			text = m_inputANode.VariableSyntaxName(ObjectBuilder.InputAID.VariableName);
			text2 = ((m_inputBNode != null) ? m_inputBNode.VariableSyntaxName(ObjectBuilder.InputBID.VariableName) : "null");
			if (m_inputBNode == null && ObjectBuilder.Operation != "==" && ObjectBuilder.Operation != "!=")
			{
				throw new Exception("Null check with Operation " + ObjectBuilder.Operation + " is prohibited.");
			}
			expressions.Add(MySyntaxFactory.ArithmeticStatement(VariableSyntaxName(), text, text2, ObjectBuilder.Operation));
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "arithmeticResult_" + (int)m_objectBuilder.ID;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.InputAID.NodeID == -1)
				{
					throw new Exception("Missing inputA in arithmetic node: " + (int)ObjectBuilder.ID);
				}
				m_inputANode = base.Navigator.GetNodeByID(ObjectBuilder.InputAID.NodeID);
				if (ObjectBuilder.InputBID.NodeID != -1)
				{
					m_inputBNode = base.Navigator.GetNodeByID(ObjectBuilder.InputBID.NodeID);
					if (m_inputBNode == null)
					{
						throw new Exception("Missing inputB in arithmetic node: " + (int)ObjectBuilder.ID);
					}
				}
				for (int i = 0; i < ObjectBuilder.OutputNodeIDs.Count; i++)
				{
					if (ObjectBuilder.OutputNodeIDs[i].NodeID == -1)
					{
						throw new Exception("-1 output in arithmetic node: " + (int)ObjectBuilder.ID);
					}
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.OutputNodeIDs[i].NodeID);
					Outputs.Add(nodeByID);
				}
				Inputs.Add(m_inputANode);
				if (m_inputBNode != null)
				{
					Inputs.Add(m_inputBNode);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
