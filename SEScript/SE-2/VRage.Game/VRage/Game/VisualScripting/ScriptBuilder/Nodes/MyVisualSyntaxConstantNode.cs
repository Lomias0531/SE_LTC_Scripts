using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxConstantNode : MyVisualSyntaxNode
	{
		internal override bool SequenceDependent => false;

		public new MyObjectBuilder_ConstantScriptNode ObjectBuilder => (MyObjectBuilder_ConstantScriptNode)m_objectBuilder;

		public MyVisualSyntaxConstantNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			string text = ObjectBuilder.Value ?? string.Empty;
			Type type = MyVisualScriptingProxy.GetType(ObjectBuilder.Type);
			base.CollectInputExpressions(expressions);
			if (type == typeof(Color) || type.IsEnum)
			{
				expressions.Add(MySyntaxFactory.LocalVariable(ObjectBuilder.Type, VariableSyntaxName(), SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(ObjectBuilder.Type), SyntaxFactory.IdentifierName(text))));
			}
			else if (type == typeof(Vector3D))
			{
				expressions.Add(MySyntaxFactory.LocalVariable(ObjectBuilder.Type, VariableSyntaxName(), MySyntaxFactory.NewVector3D(ObjectBuilder.Vector)));
			}
			else
			{
				expressions.Add(MySyntaxFactory.LocalVariable(ObjectBuilder.Type, VariableSyntaxName(), MySyntaxFactory.Literal(ObjectBuilder.Type, text)));
			}
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "constantNode_" + (int)ObjectBuilder.ID;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				for (int i = 0; i < ObjectBuilder.OutputIds.Ids.Count; i++)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.OutputIds.Ids[i].NodeID);
					if (nodeByID != null)
					{
						Outputs.Add(nodeByID);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
