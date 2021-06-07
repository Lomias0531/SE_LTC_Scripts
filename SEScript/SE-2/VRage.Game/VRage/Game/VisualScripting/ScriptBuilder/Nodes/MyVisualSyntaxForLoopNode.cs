using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Sequence dependent node that creates syntax for For loops
	/// with support for custom initial index, increment and last index.
	/// </summary>
	public class MyVisualSyntaxForLoopNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_bodySequence;

		private MyVisualSyntaxNode m_finishSequence;

		private MyVisualSyntaxNode m_firstInput;

		private MyVisualSyntaxNode m_lastInput;

		private MyVisualSyntaxNode m_incrementInput;

		private readonly List<MyVisualSyntaxNode> m_toCollectNodeCache = new List<MyVisualSyntaxNode>();

		public new MyObjectBuilder_ForLoopScriptNode ObjectBuilder => (MyObjectBuilder_ForLoopScriptNode)m_objectBuilder;

		public MyVisualSyntaxForLoopNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "forEach_" + (int)ObjectBuilder.ID + "_counter";
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions)
		{
			m_toCollectNodeCache.Clear();
			foreach (MyVisualSyntaxNode subTreeNode in SubTreeNodes)
			{
				bool flag = false;
				foreach (MyVisualSyntaxNode output in subTreeNode.Outputs)
				{
					if (output == this && !output.Collected)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					subTreeNode.CollectInputExpressions(expressions);
				}
				else
				{
					m_toCollectNodeCache.Add(subTreeNode);
				}
			}
			if (m_bodySequence != null)
			{
				List<StatementSyntax> list = new List<StatementSyntax>();
				foreach (MyVisualSyntaxNode item2 in m_toCollectNodeCache)
				{
					item2.CollectInputExpressions(list);
				}
				m_bodySequence.CollectSequenceExpressions(list);
				ExpressionSyntax value = (m_firstInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(int).Signature(), ObjectBuilder.FirstIndexValue)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_firstInput.VariableSyntaxName(ObjectBuilder.FirstIndexValueInput.VariableName)));
				ExpressionSyntax right = (m_lastInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(int).Signature(), ObjectBuilder.LastIndexValue)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_lastInput.VariableSyntaxName(ObjectBuilder.LastIndexValueInput.VariableName)));
				ExpressionSyntax right2 = (m_incrementInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(int).Signature(), ObjectBuilder.IncrementValue)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_incrementInput.VariableSyntaxName(ObjectBuilder.IncrementValueInput.VariableName)));
				ForStatementSyntax item = SyntaxFactory.ForStatement(SyntaxFactory.Block(list)).WithDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword))).WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(VariableSyntaxName())).WithInitializer(SyntaxFactory.EqualsValueClause(value))))).WithCondition(SyntaxFactory.BinaryExpression(SyntaxKind.LessThanOrEqualExpression, SyntaxFactory.IdentifierName(VariableSyntaxName()), right))
					.WithIncrementors(SyntaxFactory.SingletonSeparatedList((ExpressionSyntax)SyntaxFactory.AssignmentExpression(SyntaxKind.AddAssignmentExpression, SyntaxFactory.IdentifierName(VariableSyntaxName()), right2)));
				expressions.Add(item);
			}
			if (m_finishSequence != null)
			{
				m_finishSequence.CollectSequenceExpressions(expressions);
			}
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
				{
					TryRegisterNode(sequenceInput, SequenceInputs);
				}
				if (ObjectBuilder.SequenceOutput != -1)
				{
					m_finishSequence = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutput);
					if (m_finishSequence != null)
					{
						SequenceOutputs.Add(m_finishSequence);
					}
				}
				if (ObjectBuilder.SequenceBody != -1)
				{
					m_bodySequence = base.Navigator.GetNodeByID(ObjectBuilder.SequenceBody);
					if (m_bodySequence != null)
					{
						SequenceOutputs.Add(m_bodySequence);
					}
				}
				foreach (MyVariableIdentifier counterValueOutput in ObjectBuilder.CounterValueOutputs)
				{
					TryRegisterNode(counterValueOutput.NodeID, Outputs);
				}
				m_firstInput = TryRegisterNode(ObjectBuilder.FirstIndexValueInput.NodeID, Inputs);
				m_lastInput = TryRegisterNode(ObjectBuilder.LastIndexValueInput.NodeID, Inputs);
				m_incrementInput = TryRegisterNode(ObjectBuilder.IncrementValueInput.NodeID, Inputs);
			}
			base.Preprocess(currentDepth);
		}
	}
}
