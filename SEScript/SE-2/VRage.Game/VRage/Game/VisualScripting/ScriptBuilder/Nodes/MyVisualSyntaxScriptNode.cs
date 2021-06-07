using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Represents a method call from local instance of Script class.
	/// Contains some data that are used out of the graph generation process.
	/// </summary>
	public class MyVisualSyntaxScriptNode : MyVisualSyntaxNode
	{
		private readonly string m_instanceName;

		public new MyObjectBuilder_ScriptScriptNode ObjectBuilder => (MyObjectBuilder_ScriptScriptNode)m_objectBuilder;

		public MyVisualSyntaxScriptNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
			m_instanceName = "m_scriptInstance_" + (int)ObjectBuilder.ID;
		}

		/// <summary>
		/// MyClass m_instanceName = new MyClass();
		/// </summary>
		/// <returns></returns>
		public MemberDeclarationSyntax InstanceDeclaration()
		{
			return SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(ObjectBuilder.Name)).WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(m_instanceName)).WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(ObjectBuilder.Name)).WithArgumentList(SyntaxFactory.ArgumentList()))))));
		}

		public StatementSyntax DisposeCallDeclaration()
		{
			return SyntaxFactory.ExpressionStatement(MySyntaxFactory.MethodInvocation("Dispose", null, m_instanceName));
		}

		private StatementSyntax CreateScriptInvocationSyntax(List<StatementSyntax> dependentStatements)
		{
			List<string> inputVariableNames = ObjectBuilder.Inputs.Select((MyInputParameterSerializationData t, int index) => Inputs[index].VariableSyntaxName(t.Input.VariableName)).ToList();
			List<string> outputVarNames = ObjectBuilder.Outputs.Select((MyOutputParameterSerializationData t, int index) => Outputs[index].VariableSyntaxName(t.Name)).ToList();
			InvocationExpressionSyntax invocationExpressionSyntax = MySyntaxFactory.MethodInvocation("RunScript", inputVariableNames, outputVarNames, m_instanceName);
			if (dependentStatements == null)
			{
				return SyntaxFactory.ExpressionStatement(invocationExpressionSyntax);
			}
			return MySyntaxFactory.IfExpressionSyntax(invocationExpressionSyntax, dependentStatements);
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			expressions.AddRange(ObjectBuilder.Outputs.Select((MyOutputParameterSerializationData t, int index) => MySyntaxFactory.LocalVariable(t.Type, Outputs[index].VariableSyntaxName(t.Name))));
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions)
		{
			CollectInputExpressions(expressions);
			List<StatementSyntax> list = new List<StatementSyntax>();
			foreach (MyVisualSyntaxNode sequenceOutput in SequenceOutputs)
			{
				sequenceOutput.CollectSequenceExpressions(list);
			}
			StatementSyntax item = CreateScriptInvocationSyntax(list);
			expressions.Add(item);
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			MyOutputParameterSerializationData myOutputParameterSerializationData = ObjectBuilder.Outputs.FirstOrDefault((MyOutputParameterSerializationData o) => o.Name == variableIdentifier);
			if (myOutputParameterSerializationData != null)
			{
				int num = ObjectBuilder.Outputs.IndexOf(myOutputParameterSerializationData);
				if (num != -1)
				{
					variableIdentifier = Outputs[num].VariableSyntaxName(variableIdentifier);
				}
			}
			return variableIdentifier;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceOutput != -1)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutput);
					SequenceOutputs.Add(nodeByID);
				}
				if (ObjectBuilder.SequenceInput != -1)
				{
					MyVisualSyntaxNode nodeByID2 = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInput);
					SequenceInputs.Add(nodeByID2);
				}
				foreach (MyInputParameterSerializationData input in ObjectBuilder.Inputs)
				{
					if (input.Input.NodeID == -1)
					{
						throw new Exception("Output node missing input data. NodeID: " + (int)ObjectBuilder.ID);
					}
					MyVisualSyntaxNode nodeByID3 = base.Navigator.GetNodeByID(input.Input.NodeID);
					Inputs.Add(nodeByID3);
				}
				foreach (MyOutputParameterSerializationData output in ObjectBuilder.Outputs)
				{
					if (output.Outputs.Ids.Count != 0)
					{
						foreach (MyVariableIdentifier id in output.Outputs.Ids)
						{
							MyVisualSyntaxNode nodeByID4 = base.Navigator.GetNodeByID(id.NodeID);
							Outputs.Add(nodeByID4);
						}
					}
					else
					{
						MyVisualSyntaxFakeOutputNode item = new MyVisualSyntaxFakeOutputNode(ObjectBuilder.ID);
						Outputs.Add(item);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
