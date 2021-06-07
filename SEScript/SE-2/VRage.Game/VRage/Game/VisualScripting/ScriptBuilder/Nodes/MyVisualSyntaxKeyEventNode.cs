using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Special case of Event node that also generates some syntax.
	/// Creates a simple if statement that filters the input to this node.
	/// </summary>
	public class MyVisualSyntaxKeyEventNode : MyVisualSyntaxEventNode
	{
		public new MyObjectBuilder_KeyEventScriptNode ObjectBuilder => (MyObjectBuilder_KeyEventScriptNode)m_objectBuilder;

		public MyVisualSyntaxKeyEventNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions)
		{
			if (ObjectBuilder.SequenceOutputID == -1)
			{
				return;
			}
			MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutputID);
			List<StatementSyntax> list = new List<StatementSyntax>();
			nodeByID.CollectSequenceExpressions(list);
			List<int> list2 = new List<int>();
			ParameterInfo[] parameters = m_fieldInfo.FieldType.GetMethod("Invoke").GetParameters();
			VisualScriptingEvent customAttribute = m_fieldInfo.FieldType.GetCustomAttribute<VisualScriptingEvent>();
			for (int i = 0; i < parameters.Length && i < customAttribute.IsKey.Length; i++)
			{
				if (customAttribute.IsKey[i])
				{
					list2.Add(i);
				}
			}
			IfStatementSyntax item = MySyntaxFactory.IfExpressionSyntax(CreateAndClauses(list2.Count - 1, list2), list);
			expressions.Add(item);
		}

		private ExpressionSyntax CreateAndClauses(int index, List<int> keyIndexes)
		{
			LiteralExpressionSyntax right = MySyntaxFactory.Literal(ObjectBuilder.OuputTypes[keyIndexes[index]], ObjectBuilder.Keys[keyIndexes[index]]);
			if (index == 0)
			{
				return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, SyntaxFactory.IdentifierName(ObjectBuilder.OutputNames[keyIndexes[index]]), right);
			}
			BinaryExpressionSyntax right2 = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, SyntaxFactory.IdentifierName(ObjectBuilder.OutputNames[keyIndexes[index]]), right);
			return SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, CreateAndClauses(--index, keyIndexes), right2);
		}
	}
}
