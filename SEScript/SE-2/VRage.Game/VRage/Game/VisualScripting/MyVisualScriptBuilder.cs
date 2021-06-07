using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.ScriptBuilder;
using VRage.Game.VisualScripting.ScriptBuilder.Nodes;
using VRage.Game.VisualScripting.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.VisualScripting
{
	/// <summary>
	/// Creates class syntax for provided file.
	///
	/// Notes:
	/// WorldScripts
	///     Consist of Event methods having only input purpose, so they have no output variables and void return value.
	///     One event type with same signature can appear on multiple places in the script. Such situaltion mean that 
	///     the method body will have multiple sections that will be later evaluated independently without 
	///     any order dependency.
	///
	/// NormalScripts
	///     Should have only one input as entry point of the method and multiple or none Output nodes. Output nodes have
	///     parameters defined. Method signature will contain input variables (from input node), output variables (from 
	///     outputs - all outputs must have same signature) and bool return value. 
	///     Return value tells the system if the output node was reached and whenever we should or should not continue
	///     executing the sequence chain.
	/// </summary>
	public class MyVisualScriptBuilder
	{
		private string m_scriptFilePath;

		private string m_scriptName;

		private MyObjectBuilder_VisualScript m_objectBuilder;

		private Type m_baseType;

		private CompilationUnitSyntax m_compilationUnit;

		private MyVisualScriptNavigator m_navigator;

		private ClassDeclarationSyntax m_scriptClassDeclaration;

		private ConstructorDeclarationSyntax m_constructor;

		private MethodDeclarationSyntax m_disposeMethod;

		private NamespaceDeclarationSyntax m_namespaceDeclaration;

		private readonly List<MemberDeclarationSyntax> m_fieldDeclarations = new List<MemberDeclarationSyntax>();

		private readonly List<MethodDeclarationSyntax> m_methodDeclarations = new List<MethodDeclarationSyntax>();

		private readonly List<StatementSyntax> m_helperStatementList = new List<StatementSyntax>();

		private readonly MyVisualSyntaxBuilderNode m_builderNode = new MyVisualSyntaxBuilderNode();

		public string Syntax => m_compilationUnit.ToFullString().Replace("\\\\n", "\\n");

		public string ScriptName => m_scriptName;

		public List<string> Dependencies => m_objectBuilder.DependencyFilePaths;

		public string ScriptFilePath
		{
			get
			{
				return m_scriptFilePath;
			}
			set
			{
				m_scriptFilePath = value;
			}
		}

		public string ErrorMessage
		{
			get;
			set;
		}

		private void Clear()
		{
			m_fieldDeclarations.Clear();
			m_methodDeclarations.Clear();
		}

		/// <summary>
		/// Loads the script file.
		/// </summary>
		/// <returns></returns>
		public bool Load()
		{
			if (string.IsNullOrEmpty(m_scriptFilePath))
			{
				return false;
			}
			MyObjectBuilder_VSFiles objectBuilder;
			using (Stream reader = MyFileSystem.OpenRead(m_scriptFilePath))
			{
				if (!MyObjectBuilderSerializer.DeserializeXML(reader, out objectBuilder))
				{
					ErrorMessage = "Deserialization failed : " + m_scriptFilePath;
					return false;
				}
			}
			try
			{
				ErrorMessage = string.Empty;
				if (objectBuilder.LevelScript != null)
				{
					m_objectBuilder = objectBuilder.LevelScript;
				}
				else if (objectBuilder.VisualScript != null)
				{
					m_objectBuilder = objectBuilder.VisualScript;
				}
				m_navigator = new MyVisualScriptNavigator(m_objectBuilder);
				m_scriptName = m_objectBuilder.Name;
				if (m_objectBuilder.Interface != null)
				{
					m_baseType = MyVisualScriptingProxy.GetType(m_objectBuilder.Interface);
				}
			}
			catch (Exception ex)
			{
				string text = "Error occured during the graph reconstruction: " + ex;
				MyLog.Default.WriteLine(text);
				MyLog.Default.WriteLine(ex);
				ErrorMessage = text;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Creates syntax of a class generated out of interactionNodes.
		/// </summary>
		/// <returns></returns>
		public bool Build()
		{
			if (string.IsNullOrEmpty(m_scriptFilePath))
			{
				return false;
			}
			try
			{
				Clear();
				CreateClassSyntax();
				CreateDisposeMethod();
				CreateVariablesAndConstructorSyntax();
				CreateScriptInstances();
				CreateMethods();
				CreateNamespaceDeclaration();
				FinalizeSyntax();
			}
			catch (Exception ex)
			{
				string text = "Script: " + m_scriptName + " failed to build. Error message: " + ex.Message;
				MyLog.Default.WriteLine(text);
				MyLog.Default.WriteLine(ex);
				ErrorMessage = text;
				return false;
			}
			return true;
		}

		private void CreateDisposeMethod()
		{
			m_disposeMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), SyntaxFactory.Identifier("Dispose")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithBody(SyntaxFactory.Block());
		}

		private void CreateClassSyntax()
		{
			IdentifierNameSyntax identifierNameSyntax = SyntaxFactory.IdentifierName("IMyLevelScript");
			if (!(m_objectBuilder is MyObjectBuilder_VisualLevelScript))
			{
				identifierNameSyntax = (string.IsNullOrEmpty(m_objectBuilder.Interface) ? null : SyntaxFactory.IdentifierName(m_baseType.Name));
			}
			m_scriptClassDeclaration = MySyntaxFactory.PublicClass(m_scriptName);
			if (identifierNameSyntax != null)
			{
				m_scriptClassDeclaration = m_scriptClassDeclaration.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList((BaseTypeSyntax)SyntaxFactory.SimpleBaseType(identifierNameSyntax))));
			}
		}

		private void CreateVariablesAndConstructorSyntax()
		{
			m_constructor = MySyntaxFactory.Constructor(m_scriptClassDeclaration);
			foreach (MyVisualSyntaxVariableNode item in m_navigator.OfType<MyVisualSyntaxVariableNode>())
			{
				m_fieldDeclarations.Add(item.CreateFieldDeclaration());
				m_constructor = m_constructor.AddBodyStatements(item.CreateInitializationSyntax());
			}
		}

		private void CreateMethods()
		{
			if (!string.IsNullOrEmpty(m_objectBuilder.Interface))
			{
				foreach (MyVisualSyntaxInterfaceMethodNode item in m_navigator.OfType<MyVisualSyntaxInterfaceMethodNode>())
				{
					MethodDeclarationSyntax methodDeclaration = item.GetMethodDeclaration();
					ProcessNodes(new MyVisualSyntaxInterfaceMethodNode[1]
					{
						item
					}, ref methodDeclaration);
					m_methodDeclarations.Add(methodDeclaration);
				}
			}
			List<MyVisualSyntaxEventNode> list = m_navigator.OfType<MyVisualSyntaxEventNode>();
			list.AddRange(m_navigator.OfType<MyVisualSyntaxKeyEventNode>());
			while (list.Count > 0)
			{
				MyVisualSyntaxEventNode firstEvent = list[0];
				IEnumerable<MyVisualSyntaxEventNode> eventsWithSameName = list.Where((MyVisualSyntaxEventNode @event) => @event.ObjectBuilder.Name == firstEvent.ObjectBuilder.Name);
				MethodDeclarationSyntax methodDeclaration2 = MySyntaxFactory.PublicMethodDeclaration(firstEvent.EventName, SyntaxKind.VoidKeyword, firstEvent.ObjectBuilder.OutputNames, firstEvent.ObjectBuilder.OuputTypes);
				ProcessNodes(eventsWithSameName, ref methodDeclaration2);
				m_constructor = m_constructor.AddBodyStatements(MySyntaxFactory.DelegateAssignment(firstEvent.ObjectBuilder.Name, methodDeclaration2.Identifier.ToString()));
				m_disposeMethod = m_disposeMethod.AddBodyStatements(MySyntaxFactory.DelegateRemoval(firstEvent.ObjectBuilder.Name, methodDeclaration2.Identifier.ToString()));
				m_methodDeclarations.Add(methodDeclaration2);
				list.RemoveAll((MyVisualSyntaxEventNode @event) => eventsWithSameName.Contains(@event));
			}
			List<MyVisualSyntaxInputNode> list2 = m_navigator.OfType<MyVisualSyntaxInputNode>();
			List<MyVisualSyntaxOutputNode> list3 = m_navigator.OfType<MyVisualSyntaxOutputNode>();
			if (list2.Count > 0)
			{
				MyVisualSyntaxInputNode myVisualSyntaxInputNode = list2[0];
				MethodDeclarationSyntax methodDeclarationSyntax = null;
				if (list3.Count > 0)
				{
					List<string> list4 = new List<string>(list3[0].ObjectBuilder.Inputs.Count);
					List<string> list5 = new List<string>(list3[0].ObjectBuilder.Inputs.Count);
					foreach (MyInputParameterSerializationData input in list3[0].ObjectBuilder.Inputs)
					{
						list4.Add(input.Name);
						list5.Add(input.Type);
					}
					methodDeclarationSyntax = MySyntaxFactory.PublicMethodDeclaration("RunScript", SyntaxKind.BoolKeyword, myVisualSyntaxInputNode.ObjectBuilder.OutputNames, myVisualSyntaxInputNode.ObjectBuilder.OuputTypes, list4, list5);
				}
				else
				{
					methodDeclarationSyntax = MySyntaxFactory.PublicMethodDeclaration("RunScript", SyntaxKind.BoolKeyword, myVisualSyntaxInputNode.ObjectBuilder.OutputNames, myVisualSyntaxInputNode.ObjectBuilder.OuputTypes);
				}
				ProcessNodes(new MyVisualSyntaxInputNode[1]
				{
					myVisualSyntaxInputNode
				}, ref methodDeclarationSyntax, new ReturnStatementSyntax[1]
				{
					SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression))
				});
				m_methodDeclarations.Add(methodDeclarationSyntax);
			}
		}

		private void ProcessNodes(IEnumerable<MyVisualSyntaxNode> nodes, ref MethodDeclarationSyntax methodDeclaration, IEnumerable<StatementSyntax> statementsToAppend = null)
		{
			m_helperStatementList.Clear();
			m_navigator.ResetNodes();
			m_builderNode.Reset();
			m_builderNode.SequenceOutputs.AddRange(nodes);
			m_builderNode.Navigator = m_navigator;
			foreach (IMyVisualSyntaxEntryPoint node in nodes)
			{
				node.AddSequenceInput(m_builderNode);
			}
			m_builderNode.Preprocess();
			m_builderNode.CollectSequenceExpressions(m_helperStatementList);
			if (statementsToAppend != null)
			{
				m_helperStatementList.AddRange(statementsToAppend);
			}
			methodDeclaration = methodDeclaration.AddBodyStatements(m_helperStatementList.ToArray());
		}

		private void AddMissionLogicScriptMethods()
		{
			MethodDeclarationSyntax item = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)), SyntaxFactory.Identifier("GetOwnerId")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithBody(SyntaxFactory.Block(SyntaxFactory.SingletonList((StatementSyntax)SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName("OwnerId")))));
			PropertyDeclarationSyntax item2 = SyntaxFactory.PropertyDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)), SyntaxFactory.Identifier("OwnerId")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new AccessorDeclarationSyntax[2]
			{
				SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
				SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
			})));
			PropertyDeclarationSyntax item3 = SyntaxFactory.PropertyDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)), SyntaxFactory.Identifier("TransitionTo")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new AccessorDeclarationSyntax[2]
			{
				SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
				SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
			})));
			MethodDeclarationSyntax item4 = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), SyntaxFactory.Identifier("Complete")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Parameter(SyntaxFactory.Identifier("transitionName")).WithType(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword))).WithDefault(SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("Completed")))))))
				.WithBody(SyntaxFactory.Block(SyntaxFactory.SingletonList((StatementSyntax)SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName("TransitionTo"), SyntaxFactory.IdentifierName("transitionName"))))));
			m_methodDeclarations.Add(item4);
			m_fieldDeclarations.Add(item3);
			m_fieldDeclarations.Add(item2);
			m_methodDeclarations.Add(item);
		}

		private void CreateScriptInstances()
		{
			List<MyVisualSyntaxScriptNode> list = m_navigator.OfType<MyVisualSyntaxScriptNode>();
			if (list != null)
			{
				foreach (MyVisualSyntaxScriptNode item in list)
				{
					m_fieldDeclarations.Add(item.InstanceDeclaration());
					m_disposeMethod = m_disposeMethod.AddBodyStatements(item.DisposeCallDeclaration());
				}
			}
		}

		private void CreateNamespaceDeclaration()
		{
			m_namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("VisualScripting.CustomScripts"));
		}

		private void AddMissingInterfaceMethods()
		{
			if (m_baseType == null || !m_baseType.IsInterface)
			{
				return;
			}
			MethodInfo[] methods = m_baseType.GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				bool flag = false;
				foreach (MethodDeclarationSyntax methodDeclaration in m_methodDeclarations)
				{
					if (methodDeclaration.Identifier.ToFullString() == methodInfo.Name)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					VisualScriptingMember customAttribute = methodInfo.GetCustomAttribute<VisualScriptingMember>();
					if (customAttribute != null && !customAttribute.Reserved && !methodInfo.IsSpecialName)
					{
						m_methodDeclarations.Add(MySyntaxFactory.MethodDeclaration(methodInfo));
					}
				}
			}
		}

		private void FinalizeSyntax()
		{
			bool flag = false;
			for (int i = 0; i < m_methodDeclarations.Count; i++)
			{
				if (m_methodDeclarations[i].Identifier.ToString() == m_disposeMethod.Identifier.ToString())
				{
					if (m_disposeMethod.Body.Statements.Count > 0)
					{
						m_methodDeclarations[i] = m_methodDeclarations[i].AddBodyStatements(m_disposeMethod.Body);
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				m_methodDeclarations.Add(m_disposeMethod);
			}
			AddMissingInterfaceMethods();
			if (m_baseType == typeof(IMyStateMachineScript))
			{
				AddMissionLogicScriptMethods();
			}
			m_scriptClassDeclaration = m_scriptClassDeclaration.AddMembers(m_fieldDeclarations.ToArray());
			m_scriptClassDeclaration = m_scriptClassDeclaration.AddMembers(m_constructor);
			ClassDeclarationSyntax scriptClassDeclaration = m_scriptClassDeclaration;
			MemberDeclarationSyntax[] items = m_methodDeclarations.ToArray();
			m_scriptClassDeclaration = scriptClassDeclaration.AddMembers(items);
			m_namespaceDeclaration = m_namespaceDeclaration.AddMembers(m_scriptClassDeclaration);
			List<UsingDirectiveSyntax> list = new List<UsingDirectiveSyntax>();
			HashSet<string> hashSet = new HashSet<string>();
			UsingDirectiveSyntax usingDirectiveSyntax = MySyntaxFactory.UsingStatementSyntax("VRage.Game.VisualScripting");
			UsingDirectiveSyntax usingDirectiveSyntax2 = MySyntaxFactory.UsingStatementSyntax("System.Collections.Generic");
			list.Add(usingDirectiveSyntax);
			list.Add(usingDirectiveSyntax2);
			hashSet.Add(usingDirectiveSyntax.ToFullString());
			hashSet.Add(usingDirectiveSyntax2.ToFullString());
			foreach (MyVisualSyntaxFunctionNode item in m_navigator.OfType<MyVisualSyntaxFunctionNode>())
			{
				if (hashSet.Add(item.Using.ToFullString()))
				{
					list.Add(item.Using);
				}
			}
			foreach (MyVisualSyntaxVariableNode item2 in m_navigator.OfType<MyVisualSyntaxVariableNode>())
			{
				if (hashSet.Add(item2.Using.ToFullString()))
				{
					list.Add(item2.Using);
				}
			}
			m_compilationUnit = SyntaxFactory.CompilationUnit().WithUsings(SyntaxFactory.List(list)).AddMembers(m_namespaceDeclaration)
				.NormalizeWhitespace();
		}
	}
}
