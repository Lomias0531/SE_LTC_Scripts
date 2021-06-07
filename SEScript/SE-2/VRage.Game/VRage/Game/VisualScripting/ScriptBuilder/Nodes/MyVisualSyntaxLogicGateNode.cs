using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Sequence independent node that creates syntax for basic 
	/// boolean algebra operations. AND OR NOT XOR NAND NOR.
	/// It joins all provided values into a change of operations
	/// started and ended with parenthesis.
	/// </summary>
	public class MyVisualSyntaxLogicGateNode : MyVisualSyntaxNode
	{
		private readonly Dictionary<MyVisualSyntaxNode, string> m_inputsToVariableNames = new Dictionary<MyVisualSyntaxNode, string>();

		public new MyObjectBuilder_LogicGateScriptNode ObjectBuilder => (MyObjectBuilder_LogicGateScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		private SyntaxKind OperationKind
		{
			get
			{
				switch (ObjectBuilder.Operation)
				{
				case MyObjectBuilder_LogicGateScriptNode.LogicOperation.AND:
				case MyObjectBuilder_LogicGateScriptNode.LogicOperation.NAND:
					return SyntaxKind.LogicalAndExpression;
				case MyObjectBuilder_LogicGateScriptNode.LogicOperation.OR:
				case MyObjectBuilder_LogicGateScriptNode.LogicOperation.NOR:
					return SyntaxKind.LogicalOrExpression;
				case MyObjectBuilder_LogicGateScriptNode.LogicOperation.XOR:
					return SyntaxKind.ExclusiveOrExpression;
				default:
					return SyntaxKind.LogicalNotExpression;
				}
			}
		}

		public MyVisualSyntaxLogicGateNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "logicGate_" + (int)ObjectBuilder.ID + "_output";
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			if (Inputs.Count == 1)
			{
				LocalDeclarationStatementSyntax item = MySyntaxFactory.LocalVariable(initializer: (ObjectBuilder.Operation != MyObjectBuilder_LogicGateScriptNode.LogicOperation.NOT) ? ((ExpressionSyntax)SyntaxFactory.IdentifierName(Inputs[0].VariableSyntaxName(m_inputsToVariableNames[Inputs[0]]))) : ((ExpressionSyntax)SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxFactory.IdentifierName(Inputs[0].VariableSyntaxName(m_inputsToVariableNames[Inputs[0]])))), typeData: typeof(bool).Signature(), variableName: VariableSyntaxName());
				expressions.Add(item);
			}
			else if (Inputs.Count > 1)
			{
				ExpressionSyntax expressionSyntax = SyntaxFactory.BinaryExpression(OperationKind, SyntaxFactory.IdentifierName(Inputs[0].VariableSyntaxName(m_inputsToVariableNames[Inputs[0]])), SyntaxFactory.IdentifierName(Inputs[1].VariableSyntaxName(m_inputsToVariableNames[Inputs[1]])));
				for (int i = 2; i < Inputs.Count; i++)
				{
					MyVisualSyntaxNode myVisualSyntaxNode = Inputs[i];
					expressionSyntax = SyntaxFactory.BinaryExpression(OperationKind, expressionSyntax, SyntaxFactory.IdentifierName(myVisualSyntaxNode.VariableSyntaxName(m_inputsToVariableNames[myVisualSyntaxNode])));
				}
				if (ObjectBuilder.Operation == MyObjectBuilder_LogicGateScriptNode.LogicOperation.NAND || ObjectBuilder.Operation == MyObjectBuilder_LogicGateScriptNode.LogicOperation.NOR)
				{
					expressionSyntax = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxFactory.ParenthesizedExpression(expressionSyntax));
				}
				LocalDeclarationStatementSyntax item2 = MySyntaxFactory.LocalVariable(typeof(bool).Signature(), VariableSyntaxName(), expressionSyntax);
				expressions.Add(item2);
			}
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (MyVariableIdentifier valueInput in ObjectBuilder.ValueInputs)
				{
					MyVisualSyntaxNode myVisualSyntaxNode = TryRegisterNode(valueInput.NodeID, Inputs);
					if (myVisualSyntaxNode != null)
					{
						m_inputsToVariableNames.Add(myVisualSyntaxNode, valueInput.VariableName);
					}
				}
				foreach (MyVariableIdentifier valueOutput in ObjectBuilder.ValueOutputs)
				{
					TryRegisterNode(valueOutput.NodeID, Outputs);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
