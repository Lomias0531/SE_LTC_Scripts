using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxFunctionNode : MyVisualSyntaxNode
	{
		private readonly MethodInfo m_methodInfo;

		private MyVisualSyntaxNode m_sequenceOutputNode;

		private MyVisualSyntaxNode m_instance;

		private readonly Type m_scriptBaseType;

		private readonly Dictionary<ParameterInfo, MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>> m_parametersToInputs = new Dictionary<ParameterInfo, MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>>();

		internal override bool SequenceDependent => m_methodInfo.IsSequenceDependent();

		public UsingDirectiveSyntax Using
		{
			get;
			private set;
		}

		public new MyObjectBuilder_FunctionScriptNode ObjectBuilder => (MyObjectBuilder_FunctionScriptNode)m_objectBuilder;

		public MyVisualSyntaxFunctionNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
			m_objectBuilder = (MyObjectBuilder_FunctionScriptNode)ob;
			m_methodInfo = MyVisualScriptingProxy.GetMethod(ObjectBuilder.Type);
			m_scriptBaseType = scriptBaseType;
			if (m_methodInfo == null)
			{
				string text = ObjectBuilder.Type.Remove(0, ObjectBuilder.Type.LastIndexOf('.') + 1);
				int num = text.IndexOf('(');
				if (scriptBaseType != null && num > 0)
				{
					text = text.Remove(num);
					m_methodInfo = scriptBaseType.GetMethod(text);
				}
			}
			if (m_methodInfo == null && !string.IsNullOrEmpty(ObjectBuilder.DeclaringType))
			{
				Type type = MyVisualScriptingProxy.GetType(ObjectBuilder.DeclaringType);
				if (type != null)
				{
					m_methodInfo = MyVisualScriptingProxy.GetMethod(type, ObjectBuilder.Type);
				}
			}
			if (m_methodInfo == null && !string.IsNullOrEmpty(ObjectBuilder.ExtOfType))
			{
				Type type2 = MyVisualScriptingProxy.GetType(ObjectBuilder.ExtOfType);
				m_methodInfo = MyVisualScriptingProxy.GetMethod(type2, ObjectBuilder.Type);
			}
			if (m_methodInfo != null)
			{
				InitUsing();
			}
		}

		private void InitUsing()
		{
			if (!(m_methodInfo.DeclaringType == null))
			{
				Using = MySyntaxFactory.UsingStatementSyntax(m_methodInfo.DeclaringType.Namespace);
			}
		}

		internal override void Reset()
		{
			base.Reset();
			m_parametersToInputs.Clear();
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			base.CollectInputExpressions(expressions);
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			ParameterInfo[] parameters = m_methodInfo.GetParameters();
			int i = 0;
			if (m_methodInfo.IsDefined(typeof(ExtensionAttribute), inherit: false))
			{
				i++;
			}
			for (; i < parameters.Length; i++)
			{
				ParameterInfo parameter = parameters[i];
				MyTuple<MyVisualSyntaxNode, MyVariableIdentifier> value2;
				if (parameter.IsOut)
				{
					string text = VariableSyntaxName(parameter.Name);
					expressions.Add(MySyntaxFactory.LocalVariable(parameter.ParameterType.GetElementType().Signature(), text));
					list.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(text)).WithNameColon(SyntaxFactory.NameColon(parameter.Name)).WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword)));
				}
				else if (m_parametersToInputs.TryGetValue(parameter, out value2))
				{
					string name = value2.Item1.VariableSyntaxName(value2.Item2.VariableName);
					list.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(name)).WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
				}
				else
				{
					MyParameterValue myParameterValue = ObjectBuilder.InputParameterValues.Find((MyParameterValue value) => value.ParameterName == parameter.Name);
					if (myParameterValue == null)
					{
						if (parameter.HasDefaultValue)
						{
							continue;
						}
						list.Add(MySyntaxFactory.ConstantDefaultArgument(parameter.ParameterType).WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
					}
					else
					{
						list.Add(MySyntaxFactory.ConstantArgument(parameter.ParameterType.Signature(), MyTexts.SubstituteTexts(myParameterValue.Value)).WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
					}
				}
				list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
			}
			if (list.Count > 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			InvocationExpressionSyntax invocationExpressionSyntax = null;
			if (m_methodInfo.IsStatic && !m_methodInfo.IsDefined(typeof(ExtensionAttribute)))
			{
				invocationExpressionSyntax = MySyntaxFactory.MethodInvocationExpressionSyntax(SyntaxFactory.IdentifierName(m_methodInfo.DeclaringType.FullName + "." + m_methodInfo.Name), SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list)));
			}
			else if (m_methodInfo.DeclaringType == m_scriptBaseType)
			{
				invocationExpressionSyntax = MySyntaxFactory.MethodInvocationExpressionSyntax(SyntaxFactory.IdentifierName(m_methodInfo.Name), SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list)));
			}
			else
			{
				if (m_instance == null)
				{
					throw new Exception("FunctionNode: " + (int)ObjectBuilder.ID + " Is missing mandatory instance input.");
				}
				string name2 = m_instance.VariableSyntaxName(ObjectBuilder.InstanceInputID.VariableName);
				invocationExpressionSyntax = MySyntaxFactory.MethodInvocationExpressionSyntax(SyntaxFactory.IdentifierName(m_methodInfo.Name), SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list)), SyntaxFactory.IdentifierName(name2));
			}
			if (m_methodInfo.ReturnType == typeof(void))
			{
				expressions.Add(SyntaxFactory.ExpressionStatement(invocationExpressionSyntax));
			}
			else
			{
				expressions.Add(MySyntaxFactory.LocalVariable(string.Empty, VariableSyntaxName("Return"), invocationExpressionSyntax));
			}
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "outParamFunctionNode_" + (int)ObjectBuilder.ID + "_" + variableIdentifier;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (SequenceDependent)
				{
					if (ObjectBuilder.SequenceOutputID != -1)
					{
						m_sequenceOutputNode = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutputID);
						SequenceOutputs.Add(m_sequenceOutputNode);
					}
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInputID);
					SequenceInputs.Add(nodeByID);
				}
				else
				{
					foreach (IdentifierList outputParametersID in ObjectBuilder.OutputParametersIDs)
					{
						foreach (MyVariableIdentifier id in outputParametersID.Ids)
						{
							MyVisualSyntaxNode nodeByID2 = base.Navigator.GetNodeByID(id.NodeID);
							Outputs.Add(nodeByID2);
						}
					}
				}
				ParameterInfo[] parameters = m_methodInfo.GetParameters();
				Inputs.Capacity = ObjectBuilder.InputParameterIDs.Count;
				if (ObjectBuilder.Version == 0)
				{
					for (int i = 0; i < ObjectBuilder.InputParameterIDs.Count; i++)
					{
						MyVariableIdentifier item = ObjectBuilder.InputParameterIDs[i];
						MyVisualSyntaxNode nodeByID3 = base.Navigator.GetNodeByID(item.NodeID);
						if (nodeByID3 != null)
						{
							Inputs.Add(nodeByID3);
							m_parametersToInputs.Add(parameters[i], new MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>(nodeByID3, item));
						}
					}
				}
				else
				{
					int j = 0;
					if (m_methodInfo.IsDefined(typeof(ExtensionAttribute), inherit: false))
					{
						j++;
					}
					for (; j < parameters.Length; j++)
					{
						ParameterInfo parameter = parameters[j];
						MyVariableIdentifier item2 = ObjectBuilder.InputParameterIDs.Find((MyVariableIdentifier ident) => ident.OriginName == parameter.Name);
						if (string.IsNullOrEmpty(item2.OriginName))
						{
							continue;
						}
						MyVisualSyntaxNode nodeByID4 = base.Navigator.GetNodeByID(item2.NodeID);
						if (nodeByID4 == null)
						{
							if (parameter.HasDefaultValue)
							{
							}
						}
						else
						{
							Inputs.Add(nodeByID4);
							m_parametersToInputs.ContainsKey(parameter);
							m_parametersToInputs.Add(parameter, new MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>(nodeByID4, item2));
						}
					}
					if (ObjectBuilder.InstanceInputID.NodeID != -1)
					{
						m_instance = base.Navigator.GetNodeByID(ObjectBuilder.InstanceInputID.NodeID);
						if (m_instance != null)
						{
							Inputs.Add(m_instance);
						}
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
