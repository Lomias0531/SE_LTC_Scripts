using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using VRage.Compiler;
using VRage.Scripting.Analyzers;

namespace VRage.Scripting.Rewriters
{
	internal class InstructionCountingRewriter : CSharpSyntaxRewriter
	{
		private const string COMPILER_METHODS_POSTFIX = "";

		private const string COMPILER_METHOD_IS_DEAD = "IsDead";

		private const string COMPILER_METHOD_COUNT_INSTRUCTION = "CountInstructions";

		private const string COMPILER_METHOD_EXIT_METHOD = "ExitMethod";

		private const string COMPILER_METHOD_ENTER_METHOD = "EnterMethod";

		private const string COMPILER_METHOD_YIELD_GUARD_METHOD = "YieldGuard";

		private readonly CSharpCompilation m_compilation;

		private readonly MyScriptCompiler m_compiler;

		private SemanticModel m_semanticModel;

		private SyntaxTree m_syntaxTree;

		/// <summary>
		///     Injected nodes should not be whitelist checked, so they are tagged with an
		///     annotation to allow the whitelist analyzer to skip them.
		/// </summary>
		/// <param name="identifierName"></param>
		/// <returns></returns>
		private static NameSyntax AnnotatedIdentifier(string identifierName)
		{
			int num = identifierName.LastIndexOf('.');
			if (num >= 0)
			{
				return Annotated(SyntaxFactory.QualifiedName(AnnotatedIdentifier(identifierName.Substring(0, num)), Annotated(SyntaxFactory.IdentifierName(identifierName.Substring(num + 1)))));
			}
			return Annotated(SyntaxFactory.IdentifierName(identifierName));
		}

		/// <summary>
		///     Injected nodes should not be whitelist checked, so they are tagged with an
		///     annotation to allow the whitelist analyzer to skip them.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="node"></param>
		/// <returns></returns>
		private static T Annotated<T>(T node) where T : SyntaxNode
		{
			return node.WithAdditionalAnnotations(WhitelistDiagnosticAnalyzer.INJECTED_ANNOTATION);
		}

		/// <summary>
		///     Generates an unique-ish identifier to use for variables. If a user manages to collide with these names,
		///     then they _really_ need to learn proper naming conventions...
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		private static string GenerateUniqueIdentifier(FileLinePositionSpan location)
		{
			return $"__gen_{location.StartLinePosition.Line}_{location.StartLinePosition.Character}";
		}

		public InstructionCountingRewriter(MyScriptCompiler compiler, CSharpCompilation compilation, SyntaxTree syntaxTree)
		{
			m_compiler = compiler;
			m_compilation = compilation;
			m_syntaxTree = syntaxTree;
		}

		/// <summary>
		///     Generates dead checking if statement with injected body block
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		private StatementSyntax DeadCheckIfStatement(StatementSyntax body)
		{
			return SyntaxFactory.IfStatement(SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, IsDeadCall()), InjectedBlock(body));
		}

		/// <summary>
		///     Generates a finally clause with injected block
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		private FinallyClauseSyntax InjectedFinally(StatementSyntax body)
		{
			return SyntaxFactory.FinallyClause(SyntaxFactory.Block(DeadCheckIfStatement(body)));
		}

		/// <summary>
		///     Creates a call to the instruction counter.
		/// </summary>
		/// <returns></returns>
		private StatementSyntax InstructionCounterCall()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(IlInjector).FullName), (SimpleNameSyntax)AnnotatedIdentifier("CountInstructions"))));
		}

		/// <summary>
		///     Creates a call to the call chain depth counter.
		/// </summary>
		/// <returns></returns>
		private StatementSyntax EnterMethodCall()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(IlInjector).FullName), (SimpleNameSyntax)AnnotatedIdentifier("EnterMethod"))));
		}

		/// <summary>
		///     Creates a call to the call chain depth counter.
		/// </summary>
		/// <returns></returns>
		private StatementSyntax ExitMethodCall()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(IlInjector).FullName), (SimpleNameSyntax)AnnotatedIdentifier("ExitMethod"))));
		}

		/// <summary>
		///     Creates a call to determine whether or not a given script instance is dead.
		/// </summary>
		/// <returns></returns>
		private static ExpressionSyntax IsDeadCall()
		{
			return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(IlInjector).FullName), (SimpleNameSyntax)AnnotatedIdentifier("IsDead")));
		}

		/// <summary>
		///     Injects guard to yield expression to prevent it from leaking method-chain counter
		/// </summary>
		/// <param name="expression">Original yield expression</param>
		/// <param name="genericAttribute">Explicit guard type</param>
		/// <returns>Injected yield expression</returns>
		private ExpressionSyntax YieldGuard(ExpressionSyntax expression, TypeSyntax genericAttribute = null)
		{
			SyntaxToken identifier = SyntaxFactory.Identifier("YieldGuard");
			return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(name: Annotated((genericAttribute != null) ? ((SimpleNameSyntax)SyntaxFactory.GenericName(identifier, SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(genericAttribute)))) : ((SimpleNameSyntax)SyntaxFactory.IdentifierName(identifier))), kind: SyntaxKind.SimpleMemberAccessExpression, expression: AnnotatedIdentifier(typeof(IlInjector).FullName))).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(expression.WithoutTrivia())))).WithTriviaFrom(expression);
		}

		/// <summary>
		///     Gets the locations to generate #line pragmas for to generate correct error messages, to be used with the
		///     <see cref="M:VRage.Scripting.Rewriters.InstructionCountingRewriter.InjectedBlock(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax,Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax)" /> method.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private FileLinePositionSpan GetBlockResumeLocation(SyntaxNode node)
		{
			BlockSyntax blockSyntax = node as BlockSyntax;
			if (blockSyntax != null)
			{
				if (blockSyntax.Statements.Count == 0)
				{
					return blockSyntax.CloseBraceToken.GetLocation().GetMappedLineSpan();
				}
				return blockSyntax.Statements[0].GetLocation().GetMappedLineSpan();
			}
			return node.GetLocation().GetMappedLineSpan();
		}

		/// <summary>
		///     Either injects counter methods into an existing block syntax, or generates a block syntax to place the counter
		///     method in.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="injection"></param>
		/// <returns></returns>
		private BlockSyntax InjectedBlock(StatementSyntax node, StatementSyntax injection = null)
		{
			injection = (injection ?? InstructionCounterCall());
			BlockSyntax blockSyntax = node as BlockSyntax;
			if (blockSyntax != null)
			{
				return blockSyntax.WithStatements(blockSyntax.Statements.Insert(0, injection));
			}
			return SyntaxFactory.Block(injection, node);
		}

		/// <summary>
		/// Wraps methodBody into try/finally and makes sure that EnterMehod and ExitMethod compiler calls are emited
		/// </summary>
		/// <param name="methodBody"></param>
		/// <returns></returns>
		private BlockSyntax InjectBlockAsMethodBody(BlockSyntax methodBody)
		{
			return SyntaxFactory.Block(EnterMethodCall(), InstructionCounterCall(), SyntaxFactory.TryStatement(methodBody, default(SyntaxList<CatchClauseSyntax>), SyntaxFactory.FinallyClause(SyntaxFactory.Block(ExitMethodCall()))));
		}

		/// <summary>
		///     Generates the instruction counter and call chain depth counter for any form of method (except properties which must
		///     be handled by themselves).
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private SyntaxNode ProcessMethod(BaseMethodDeclarationSyntax node)
		{
			if (node.Body != null)
			{
				return node.WithBody(InjectBlockAsMethodBody(node.Body));
			}
			MethodDeclarationSyntax methodDeclarationSyntax = node as MethodDeclarationSyntax;
			if (methodDeclarationSyntax != null)
			{
				bool flag = (methodDeclarationSyntax.ReturnType as PredefinedTypeSyntax)?.Keyword.IsKind(SyntaxKind.VoidKeyword) ?? false;
				return methodDeclarationSyntax.WithExpressionBody(null).WithBody(CreateDelegateMethodBody(methodDeclarationSyntax.ExpressionBody.Expression, !flag));
			}
			OperatorDeclarationSyntax operatorDeclarationSyntax = node as OperatorDeclarationSyntax;
			if (operatorDeclarationSyntax != null)
			{
				return operatorDeclarationSyntax.WithExpressionBody(null).WithBody(CreateDelegateMethodBody(operatorDeclarationSyntax.ExpressionBody.Expression, hasReturnValue: true));
			}
			ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax = node as ConversionOperatorDeclarationSyntax;
			if (conversionOperatorDeclarationSyntax != null)
			{
				return conversionOperatorDeclarationSyntax.WithExpressionBody(null).WithBody(CreateDelegateMethodBody(conversionOperatorDeclarationSyntax.ExpressionBody.Expression, hasReturnValue: true));
			}
			if (node is ConstructorDeclarationSyntax || node is DestructorDeclarationSyntax)
			{
				throw new ArgumentException("Constructors and destructors have to have bodies!", "node");
			}
			throw new ArgumentException("Unknown " + node.GetType().FullName, "node");
		}

		/// <summary>
		///     Replaces an expression based method declaration with a block based one in order to facilitate instruction and
		///     method call chain counting.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="hasReturnValue"></param>
		/// <returns></returns>
		private BlockSyntax CreateDelegateMethodBody(ExpressionSyntax expression, bool hasReturnValue)
		{
			StatementSyntax statementSyntax = (!hasReturnValue) ? ((StatementSyntax)SyntaxFactory.ExpressionStatement(expression)) : ((StatementSyntax)SyntaxFactory.ReturnStatement(expression));
			return InjectBlockAsMethodBody(SyntaxFactory.Block(statementSyntax));
		}

		/// <summary>
		///     Generates the instruction counter and call chain depth counter for any form of delegate (anonymous methods,
		///     lambdas).
		/// </summary>
		/// <param name="node"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private SyntaxNode ProcessAnonymousFunction(AnonymousFunctionExpressionSyntax node, INamedTypeSymbol type)
		{
			BlockSyntax blockSyntax = node.Body as BlockSyntax;
			if (blockSyntax != null)
			{
				return node.WithBody((CSharpSyntaxNode)InjectBlockAsMethodBody(blockSyntax));
			}
			if (type == null || type.DelegateInvokeMethod == null)
			{
				return node;
			}
			return node.WithBody((CSharpSyntaxNode)CreateDelegateMethodBody((ExpressionSyntax)node.Body, !type.DelegateInvokeMethod.ReturnsVoid));
		}

		public override SyntaxNode VisitCatchClause(CatchClauseSyntax node)
		{
			if (node.Span.IsEmpty || node.Block.Span.IsEmpty)
			{
				return base.VisitCatchClause(node);
			}
			FileLinePositionSpan blockResumeLocation = GetBlockResumeLocation(node.Block);
			node = (CatchClauseSyntax)base.VisitCatchClause(node);
			if (node.Declaration == null)
			{
				node = node.WithDeclaration(SyntaxFactory.CatchDeclaration(SyntaxFactory.ParseTypeName(typeof(Exception).FullName), SyntaxFactory.Identifier(GenerateUniqueIdentifier(blockResumeLocation))));
			}
			else
			{
				if (node.Declaration.Type.IsMissing)
				{
					return node;
				}
				if (node.Declaration.Identifier.IsKind(SyntaxKind.None))
				{
					node = node.WithDeclaration(node.Declaration.WithIdentifier(SyntaxFactory.Identifier(GenerateUniqueIdentifier(blockResumeLocation))));
				}
			}
			string exceptionIdentifier = node.Declaration.Identifier.ValueText;
			PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxFactory.ParenthesizedExpression(m_compiler.UnblockableIngameExceptions.Aggregate(null, delegate(BinaryExpressionSyntax aggregation, Type current)
			{
				BinaryExpressionSyntax binaryExpressionSyntax = SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, SyntaxFactory.IdentifierName(exceptionIdentifier), AnnotatedIdentifier(current.FullName));
				return (aggregation == null) ? binaryExpressionSyntax : SyntaxFactory.BinaryExpression(SyntaxKind.LogicalOrExpression, binaryExpressionSyntax, aggregation);
			}))).WithLeadingTrivia(SyntaxFactory.Trivia(Annotated(SyntaxFactory.PragmaWarningDirectiveTrivia(SyntaxFactory.Token(SyntaxKind.DisableKeyword), SyntaxFactory.SingletonSeparatedList((ExpressionSyntax)SyntaxFactory.IdentifierName("CS0184")), isActive: true)))).WithTrailingTrivia(SyntaxFactory.Trivia(Annotated(SyntaxFactory.PragmaWarningDirectiveTrivia(SyntaxFactory.Token(SyntaxKind.RestoreKeyword), SyntaxFactory.SingletonSeparatedList((ExpressionSyntax)SyntaxFactory.IdentifierName("CS0184")), isActive: true))));
			node = ((node.Filter != null) ? node.WithFilter(SyntaxFactory.CatchFilterClause(SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, prefixUnaryExpressionSyntax, SyntaxFactory.ParenthesizedExpression(node.Filter.FilterExpression)))) : node.WithFilter(SyntaxFactory.CatchFilterClause(prefixUnaryExpressionSyntax)));
			return node.WithBlock(InjectedBlock(node.Block));
		}

		public override SyntaxNode VisitFinallyClause(FinallyClauseSyntax node)
		{
			if (node.Span.IsEmpty || node.Block.Span.IsEmpty)
			{
				return base.VisitFinallyClause(node);
			}
			node = (FinallyClauseSyntax)base.VisitFinallyClause(node);
			return InjectedFinally(node.Block);
		}

		public override SyntaxNode VisitUsingStatement(UsingStatementSyntax node)
		{
			FileLinePositionSpan blockResumeLocation = GetBlockResumeLocation(node);
			ExpressionSyntax expressionSyntax = null;
			StatementSyntax statementSyntax = null;
			SeparatedSyntaxList<VariableDeclaratorSyntax> separatedSyntaxList;
			if (node.Declaration != null)
			{
				separatedSyntaxList = node.Declaration.Variables;
				if (separatedSyntaxList.Count > 0)
				{
					statementSyntax = SyntaxFactory.LocalDeclarationStatement(node.Declaration);
					separatedSyntaxList = node.Declaration.Variables;
					expressionSyntax = SyntaxFactory.IdentifierName(separatedSyntaxList[0].Identifier);
					goto IL_00aa;
				}
			}
			if (node.Expression != null)
			{
				expressionSyntax = SyntaxFactory.IdentifierName(GenerateUniqueIdentifier(blockResumeLocation));
				IdentifierNameSyntax type = SyntaxFactory.IdentifierName("var");
				separatedSyntaxList = default(SeparatedSyntaxList<VariableDeclaratorSyntax>);
				statementSyntax = SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(type, separatedSyntaxList.Add(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(GenerateUniqueIdentifier(blockResumeLocation)), null, SyntaxFactory.EqualsValueClause(node.Expression)))));
			}
			goto IL_00aa;
			IL_00aa:
			if (expressionSyntax == null || node.Statement.IsMissing || node.UsingKeyword.IsMissing || node.OpenParenToken.IsMissing || node.CloseParenToken.IsMissing)
			{
				return base.VisitUsingStatement(node);
			}
			node = (UsingStatementSyntax)base.VisitUsingStatement(node);
			return SyntaxFactory.Block(statementSyntax, SyntaxFactory.TryStatement(InjectedBlock(node.Statement), default(SyntaxList<CatchClauseSyntax>), InjectedFinally(SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, SyntaxFactory.IdentifierName("Dispose"))))))));
		}

		public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
		{
			node = (IfStatementSyntax)base.VisitIfStatement(node);
			node = node.WithStatement(InjectedBlock(node.Statement));
			return node;
		}

		public override SyntaxNode VisitElseClause(ElseClauseSyntax node)
		{
			node = (ElseClauseSyntax)base.VisitElseClause(node);
			if (node.Statement.Kind() == SyntaxKind.IfStatement)
			{
				return node;
			}
			node = node.WithStatement(InjectedBlock(node.Statement));
			return node;
		}

		public override SyntaxNode VisitGotoStatement(GotoStatementSyntax node)
		{
			if (node.CaseOrDefaultKeyword.Kind() != 0)
			{
				return base.VisitGotoStatement(node);
			}
			node = (GotoStatementSyntax)base.VisitGotoStatement(node);
			return InjectedBlock(node);
		}

		public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node)
		{
			node = (SwitchSectionSyntax)base.VisitSwitchSection(node);
			if (node.Statements.Count > 0)
			{
				node = node.WithStatements(node.Statements.Insert(0, InstructionCounterCall()));
			}
			return node;
		}

		public override SyntaxNode VisitDoStatement(DoStatementSyntax node)
		{
			node = (DoStatementSyntax)base.VisitDoStatement(node);
			node = node.WithStatement(InjectedBlock(node.Statement));
			return node;
		}

		public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node)
		{
			node = (WhileStatementSyntax)base.VisitWhileStatement(node);
			node = node.WithStatement(InjectedBlock(node.Statement));
			return node;
		}

		public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
		{
			node = (ForEachStatementSyntax)base.VisitForEachStatement(node);
			node = node.WithStatement(InjectedBlock(node.Statement));
			return node;
		}

		public override SyntaxNode VisitForStatement(ForStatementSyntax node)
		{
			node = (ForStatementSyntax)base.VisitForStatement(node);
			node = node.WithStatement(InjectedBlock(node.Statement));
			return node;
		}

		public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
		{
			if (node.ExpressionBody != null)
			{
				node = (PropertyDeclarationSyntax)base.VisitPropertyDeclaration(node);
				PropertyDeclarationSyntax propertyDeclarationSyntax = node.WithExpressionBody(null);
				SyntaxList<AccessorDeclarationSyntax> syntaxList = default(SyntaxList<AccessorDeclarationSyntax>);
				return propertyDeclarationSyntax.WithAccessorList(SyntaxFactory.AccessorList(syntaxList.Add(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration, CreateDelegateMethodBody(node.ExpressionBody.Expression, hasReturnValue: true)))));
			}
			return base.VisitPropertyDeclaration(node);
		}

		public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node)
		{
			if (node.Body == null)
			{
				return base.VisitAccessorDeclaration(node);
			}
			node = (AccessorDeclarationSyntax)base.VisitAccessorDeclaration(node);
			return node.WithBody(InjectBlockAsMethodBody(node.Body));
		}

		public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
		{
			if (node.Body == null)
			{
				return base.VisitConstructorDeclaration(node);
			}
			node = (ConstructorDeclarationSyntax)base.VisitConstructorDeclaration(node);
			return ProcessMethod(node);
		}

		public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node)
		{
			if (node.Body == null)
			{
				return base.VisitDestructorDeclaration(node);
			}
			node = (DestructorDeclarationSyntax)base.VisitDestructorDeclaration(node);
			return ProcessMethod(node);
		}

		public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node)
		{
			if (node.Body == null && node.ExpressionBody == null)
			{
				return base.VisitOperatorDeclaration(node);
			}
			node = (OperatorDeclarationSyntax)base.VisitOperatorDeclaration(node);
			return ProcessMethod(node);
		}

		public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
		{
			if (node.Body == null && node.ExpressionBody == null)
			{
				return base.VisitConversionOperatorDeclaration(node);
			}
			node = (ConversionOperatorDeclarationSyntax)base.VisitConversionOperatorDeclaration(node);
			return ProcessMethod(node);
		}

		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			if (node.Body == null && node.ExpressionBody == null)
			{
				return base.VisitMethodDeclaration(node);
			}
			node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
			return ProcessMethod(node);
		}

		public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
		{
			INamedTypeSymbol type = m_semanticModel.GetTypeInfo(node).ConvertedType as INamedTypeSymbol;
			node = (AnonymousMethodExpressionSyntax)base.VisitAnonymousMethodExpression(node);
			return ProcessAnonymousFunction(node, type);
		}

		public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
		{
			INamedTypeSymbol type = m_semanticModel.GetTypeInfo(node).ConvertedType as INamedTypeSymbol;
			node = (ParenthesizedLambdaExpressionSyntax)base.VisitParenthesizedLambdaExpression(node);
			return ProcessAnonymousFunction(node, type);
		}

		public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
		{
			INamedTypeSymbol type = m_semanticModel.GetTypeInfo(node).ConvertedType as INamedTypeSymbol;
			node = (SimpleLambdaExpressionSyntax)base.VisitSimpleLambdaExpression(node);
			return ProcessAnonymousFunction(node, type);
		}

		public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node)
		{
			if (node.ExpressionBody == null)
			{
				return base.VisitIndexerDeclaration(node);
			}
			if (node.AccessorList != null)
			{
				return node;
			}
			node = (IndexerDeclarationSyntax)base.VisitIndexerDeclaration(node);
			IndexerDeclarationSyntax indexerDeclarationSyntax = node.WithExpressionBody(null);
			SyntaxList<AccessorDeclarationSyntax> syntaxList = default(SyntaxList<AccessorDeclarationSyntax>);
			return indexerDeclarationSyntax.WithAccessorList(SyntaxFactory.AccessorList(syntaxList.Add(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration, CreateDelegateMethodBody(node.ExpressionBody.Expression, hasReturnValue: true)))));
		}

		/// <summary>
		/// Injects `yield return`-statement's expression with call to <see cref="M:VRage.Compiler.IlInjector.YieldGuard``1(``0)" /> to prevent <see cref="!:IlInjector.m_callChainDepth" /> from leaking.
		/// </summary>
		public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
		{
			node = (YieldStatementSyntax)base.VisitYieldStatement(node);
			if (node.ReturnOrBreakKeyword.IsKind(SyntaxKind.BreakKeyword))
			{
				return node;
			}
			if (node.Expression == null || node.Expression.IsMissing)
			{
				return node;
			}
			MethodDeclarationSyntax methodDeclarationSyntax = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();
			if (methodDeclarationSyntax == null)
			{
				return node;
			}
			NameSyntax nameSyntax = methodDeclarationSyntax.ReturnType as NameSyntax;
			if (nameSyntax == null)
			{
				return node;
			}
			QualifiedNameSyntax qualifiedNameSyntax = nameSyntax as QualifiedNameSyntax;
			if (qualifiedNameSyntax != null)
			{
				nameSyntax = qualifiedNameSyntax.Right;
			}
			TypeSyntax genericAttribute = (nameSyntax.Arity != 0) ? ((GenericNameSyntax)nameSyntax).TypeArgumentList.Arguments[0] : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));
			new LinePosition(node.Expression.GetLocation().GetMappedLineSpan().EndLinePosition.Line + 1, 0);
			return SyntaxFactory.Block(node.WithExpression(YieldGuard(node.Expression, genericAttribute)), EnterMethodCall());
		}

		/// <summary>
		///     Creates a new rewritten syntax tree with instruction- and call chain depth counting.
		/// </summary>
		/// <returns></returns>
		public SyntaxTree Rewrite()
		{
			CSharpSyntaxNode node = (CSharpSyntaxNode)m_syntaxTree.GetRoot();
			m_semanticModel = m_compilation.GetSemanticModel(m_syntaxTree);
			CSharpSyntaxNode root = (CSharpSyntaxNode)Visit(node);
			return m_syntaxTree.WithRootAndOptions(root, m_syntaxTree.Options);
		}
	}
}
