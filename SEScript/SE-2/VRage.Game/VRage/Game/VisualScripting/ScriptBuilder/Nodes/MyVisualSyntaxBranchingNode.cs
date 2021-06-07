using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxBranchingNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_nextTrueSequenceNode;

		private MyVisualSyntaxNode m_nextFalseSequenceNode;

		private MyVisualSyntaxNode m_comparerNode;

		public new MyObjectBuilder_BranchingScriptNode ObjectBuilder => (MyObjectBuilder_BranchingScriptNode)m_objectBuilder;

		public MyVisualSyntaxBranchingNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions)
		{
			CollectInputExpressions(expressions);
			List<StatementSyntax> list = new List<StatementSyntax>();
			List<StatementSyntax> list2 = new List<StatementSyntax>();
			if (m_nextTrueSequenceNode != null)
			{
				m_nextTrueSequenceNode.CollectSequenceExpressions(list);
			}
			if (m_nextFalseSequenceNode != null)
			{
				m_nextFalseSequenceNode.CollectSequenceExpressions(list2);
			}
			string conditionVariableName = m_comparerNode.VariableSyntaxName(ObjectBuilder.InputID.VariableName);
			expressions.Add(MySyntaxFactory.IfExpressionSyntax(conditionVariableName, list, list2));
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceTrueOutputID != -1)
				{
					m_nextTrueSequenceNode = base.Navigator.GetNodeByID(ObjectBuilder.SequenceTrueOutputID);
					SequenceOutputs.Add(m_nextTrueSequenceNode);
				}
				if (ObjectBuilder.SequnceFalseOutputID != -1)
				{
					m_nextFalseSequenceNode = base.Navigator.GetNodeByID(ObjectBuilder.SequnceFalseOutputID);
					SequenceOutputs.Add(m_nextFalseSequenceNode);
				}
				if (ObjectBuilder.SequenceInputID != -1)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInputID);
					SequenceInputs.Add(nodeByID);
				}
				if (ObjectBuilder.InputID.NodeID == -1)
				{
					throw new Exception("Branching node has no comparer input. NodeID: " + (int)ObjectBuilder.ID);
				}
				m_comparerNode = base.Navigator.GetNodeByID(ObjectBuilder.InputID.NodeID);
				Inputs.Add(m_comparerNode);
			}
			base.Preprocess(currentDepth);
		}
	}
}
