using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxSetterNode : MyVisualSyntaxNode
	{
		private string m_inputVariableName;

		private MyVisualSyntaxNode m_inputNode;

		internal override bool SequenceDependent => true;

		public new MyObjectBuilder_VariableSetterScriptNode ObjectBuilder => (MyObjectBuilder_VariableSetterScriptNode)m_objectBuilder;

		public MyVisualSyntaxSetterNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		private StatementSyntax GetCorrectAssignmentsExpression()
		{
			Type type = MyVisualScriptingProxy.GetType(base.Navigator.GetVariable(ObjectBuilder.VariableName).ObjectBuilder.VariableType);
			if (type == typeof(string))
			{
				if (ObjectBuilder.ValueInputID.NodeID == -1)
				{
					return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.StringLiteralExpression);
				}
			}
			else if (type == typeof(Vector3D))
			{
				if (ObjectBuilder.ValueInputID.NodeID == -1)
				{
					return MySyntaxFactory.SimpleAssignment(ObjectBuilder.VariableName, MySyntaxFactory.NewVector3D(ObjectBuilder.VariableValue));
				}
			}
			else if (type == typeof(bool))
			{
				if (ObjectBuilder.ValueInputID.NodeID == -1)
				{
					SyntaxKind expressionKind = (MySyntaxFactory.NormalizeBool(ObjectBuilder.VariableValue) == "true") ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression;
					return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, expressionKind);
				}
			}
			else if (ObjectBuilder.ValueInputID.NodeID == -1)
			{
				return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.NumericLiteralExpression);
			}
			return MySyntaxFactory.SimpleAssignment(ObjectBuilder.VariableName, SyntaxFactory.IdentifierName(m_inputVariableName));
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			if (ObjectBuilder.ValueInputID.NodeID != -1)
			{
				m_inputVariableName = m_inputNode.VariableSyntaxName(ObjectBuilder.ValueInputID.VariableName);
			}
			expressions.Add(GetCorrectAssignmentsExpression());
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceOutputID != -1)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutputID);
					SequenceOutputs.Add(nodeByID);
				}
				if (ObjectBuilder.SequenceInputID != -1)
				{
					MyVisualSyntaxNode nodeByID2 = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInputID);
					SequenceInputs.Add(nodeByID2);
				}
				if (ObjectBuilder.ValueInputID.NodeID != -1)
				{
					m_inputNode = base.Navigator.GetNodeByID(ObjectBuilder.ValueInputID.NodeID);
					Inputs.Add(m_inputNode);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
