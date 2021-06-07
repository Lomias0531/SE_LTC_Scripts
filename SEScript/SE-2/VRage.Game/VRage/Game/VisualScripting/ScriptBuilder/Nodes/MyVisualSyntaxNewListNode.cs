using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxNewListNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_NewListScriptNode ObjectBuilder => (MyObjectBuilder_NewListScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public MyVisualSyntaxNewListNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "newListNode_" + (int)ObjectBuilder.ID;
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			Type type = MyVisualScriptingProxy.GetType(ObjectBuilder.Type);
			Type type2 = typeof(List<>).MakeGenericType(type);
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			for (int i = 0; i < ObjectBuilder.DefaultEntries.Count; i++)
			{
				string val = ObjectBuilder.DefaultEntries[i];
				LiteralExpressionSyntax node = MySyntaxFactory.Literal(ObjectBuilder.Type, val);
				list.Add(node);
				if (i < ObjectBuilder.DefaultEntries.Count - 1)
				{
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			ArrayCreationExpressionSyntax arrayCreationExpressionSyntax = null;
			if (list.Count > 0)
			{
				arrayCreationExpressionSyntax = SyntaxFactory.ArrayCreationExpression(SyntaxFactory.ArrayType(SyntaxFactory.IdentifierName(ObjectBuilder.Type), SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList((ExpressionSyntax)SyntaxFactory.OmittedArraySizeExpression())))), SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression, SyntaxFactory.SeparatedList<ExpressionSyntax>(list)));
			}
			ObjectCreationExpressionSyntax initializerExpressionSyntax = MySyntaxFactory.GenericObjectCreation(type2, (arrayCreationExpressionSyntax == null) ? null : new ArrayCreationExpressionSyntax[1]
			{
				arrayCreationExpressionSyntax
			});
			LocalDeclarationStatementSyntax item = MySyntaxFactory.LocalVariable(type2, VariableSyntaxName(), initializerExpressionSyntax);
			expressions.Add(item);
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (MyVariableIdentifier connection in ObjectBuilder.Connections)
				{
					TryRegisterNode(connection.NodeID, Outputs);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
