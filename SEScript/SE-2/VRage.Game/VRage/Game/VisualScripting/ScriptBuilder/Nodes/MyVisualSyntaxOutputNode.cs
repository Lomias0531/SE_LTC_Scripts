using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Output node that fills in the output values of a method.
	/// </summary>
	public class MyVisualSyntaxOutputNode : MyVisualSyntaxNode
	{
		private readonly List<MyVisualSyntaxNode> m_inputNodes = new List<MyVisualSyntaxNode>();

		public new MyObjectBuilder_OutputScriptNode ObjectBuilder => (MyObjectBuilder_OutputScriptNode)m_objectBuilder;

		public MyVisualSyntaxOutputNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void Reset()
		{
			base.Reset();
			m_inputNodes.Clear();
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			List<StatementSyntax> list = new List<StatementSyntax>(ObjectBuilder.Inputs.Count);
			for (int i = 0; i < ObjectBuilder.Inputs.Count; i++)
			{
				string name = m_inputNodes[i].VariableSyntaxName(ObjectBuilder.Inputs[i].Input.VariableName);
				ExpressionStatementSyntax item = MySyntaxFactory.SimpleAssignment(ObjectBuilder.Inputs[i].Name, SyntaxFactory.IdentifierName(name));
				list.Add(item);
			}
			expressions.AddRange(list);
			expressions.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)));
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceInputID != -1)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInputID);
					SequenceInputs.Add(nodeByID);
				}
				foreach (MyInputParameterSerializationData input in ObjectBuilder.Inputs)
				{
					if (input.Input.NodeID == -1)
					{
						throw new Exception("Output node missing input for " + input.Name + ". NodeID: " + (int)ObjectBuilder.ID);
					}
					MyVisualSyntaxNode nodeByID2 = base.Navigator.GetNodeByID(input.Input.NodeID);
					m_inputNodes.Add(nodeByID2);
					Inputs.Add(nodeByID2);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
