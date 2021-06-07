using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Reflection;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Simple method declaration node for implementing the interface methods.
	/// </summary>
	public class MyVisualSyntaxInterfaceMethodNode : MyVisualSyntaxNode, IMyVisualSyntaxEntryPoint
	{
		private readonly MethodInfo m_method;

		public new MyObjectBuilder_InterfaceMethodNode ObjectBuilder => (MyObjectBuilder_InterfaceMethodNode)m_objectBuilder;

		public MyVisualSyntaxInterfaceMethodNode(MyObjectBuilder_ScriptNode ob, Type baseClass)
			: base(ob)
		{
			m_method = baseClass.GetMethod(ObjectBuilder.MethodName);
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			foreach (string outputName in ObjectBuilder.OutputNames)
			{
				if (outputName == variableIdentifier)
				{
					return variableIdentifier;
				}
			}
			return null;
		}

		public void AddSequenceInput(MyVisualSyntaxNode parent)
		{
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (int sequenceOutputID in ObjectBuilder.SequenceOutputIDs)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(sequenceOutputID);
					SequenceOutputs.Add(nodeByID);
				}
			}
			base.Preprocess(currentDepth);
		}

		public MethodDeclarationSyntax GetMethodDeclaration()
		{
			return MySyntaxFactory.PublicMethodDeclaration(m_method.Name, SyntaxKind.VoidKeyword, ObjectBuilder.OutputNames, ObjectBuilder.OuputTypes);
		}
	}
}
