using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxCastNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_nextSequenceNode;

		private MyVisualSyntaxNode m_inputNode;

		public new MyObjectBuilder_CastScriptNode ObjectBuilder => (MyObjectBuilder_CastScriptNode)m_objectBuilder;

		public MyVisualSyntaxCastNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			expressions.Add(MySyntaxFactory.CastExpression(m_inputNode.VariableSyntaxName(ObjectBuilder.InputID.VariableName), ObjectBuilder.Type, VariableSyntaxName()));
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "castResult_" + (int)ObjectBuilder.ID;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceOuputID != -1)
				{
					m_nextSequenceNode = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOuputID);
					SequenceOutputs.Add(m_nextSequenceNode);
				}
				MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInputID);
				SequenceInputs.Add(nodeByID);
				if (ObjectBuilder.InputID.NodeID == -1)
				{
					throw new Exception("Cast node has no input. NodeId: " + (int)ObjectBuilder.ID);
				}
				m_inputNode = base.Navigator.GetNodeByID(ObjectBuilder.InputID.NodeID);
				Inputs.Add(m_inputNode);
			}
			base.Preprocess(currentDepth);
		}
	}
}
