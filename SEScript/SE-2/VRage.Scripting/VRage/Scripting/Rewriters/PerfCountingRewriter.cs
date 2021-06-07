using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using VRage.Scripting.Analyzers;
using VRage.Scripting.CompilerMethods;

namespace VRage.Scripting.Rewriters
{
	internal class PerfCountingRewriter : CSharpSyntaxRewriter
	{
		private const string COMPILER_METHODS_POSTFIX = "";

		private const string COMPILER_METHOD_EXIT_METHOD = "ExitMethod";

		private const string COMPILER_METHOD_ENTER_METHOD = "EnterMethod";

		private const string COMPILER_METHOD_YIELD_GUARD_METHOD = "YieldGuard";

		private const string COMPILER_METHOD_REENTER_YIELD_METHOD = "ReenterYieldMethod";

		private readonly int m_modID;

		private readonly SyntaxTree m_syntaxTree;

		private readonly SemanticModel m_semanticModel;

		private readonly CSharpCompilation m_compilation;

		protected PerfCountingRewriter(CSharpCompilation compilation, SyntaxTree syntaxTree, int modId)
		{
			m_modID = modId;
			m_syntaxTree = syntaxTree;
			m_compilation = compilation;
			m_semanticModel = compilation.GetSemanticModel(syntaxTree);
		}

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

		private ArgumentSyntax MakeModIdArg()
		{
			return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(m_modID)));
		}

		/// <summary>
		///     Creates an EnterMethod API call
		/// </summary>
		/// <returns></returns>
		private StatementSyntax EnterMethodCall()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(ModPerfCounter).FullName), (SimpleNameSyntax)AnnotatedIdentifier("EnterMethod"))).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(MakeModIdArg()))));
		}

		/// <summary>
		///     Creates an ExitMethod API call
		/// </summary>
		/// <returns></returns>
		private StatementSyntax ExitMethodCall()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(ModPerfCounter).FullName), (SimpleNameSyntax)AnnotatedIdentifier("ExitMethod"))).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(MakeModIdArg()))));
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
			return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(name: Annotated((genericAttribute != null) ? ((SimpleNameSyntax)SyntaxFactory.GenericName(identifier, SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(genericAttribute)))) : ((SimpleNameSyntax)SyntaxFactory.IdentifierName(identifier))), kind: SyntaxKind.SimpleMemberAccessExpression, expression: AnnotatedIdentifier(typeof(ModPerfCounter).FullName))).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new ArgumentSyntax[2]
			{
				MakeModIdArg(),
				SyntaxFactory.Argument(expression.WithoutTrivia())
			}))).WithTriviaFrom(expression);
		}

		/// <summary>
		///     Creates an ExitMethod API call
		/// </summary>
		/// <returns></returns>
		private StatementSyntax ReenterYieldMethodCall()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, AnnotatedIdentifier(typeof(ModPerfCounter).FullName), (SimpleNameSyntax)AnnotatedIdentifier("ReenterYieldMethod"))).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(MakeModIdArg()))));
		}

		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
			if (node.ExpressionBody != null)
			{
				bool isVoid = (node.ReturnType as PredefinedTypeSyntax)?.Keyword.IsKind(SyntaxKind.VoidKeyword) ?? false;
				return node.WithExpressionBody(null).WithBody(ProcessMethod(node.ExpressionBody, isVoid));
			}
			return node.WithBody(ProcessMethod(node.Body));
		}

		public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node)
		{
			node = (IndexerDeclarationSyntax)base.VisitIndexerDeclaration(node);
			if (node.ExpressionBody != null)
			{
				return node.WithExpressionBody(null).WithAccessorList(ArrowToGetter(node.ExpressionBody));
			}
			return node;
		}

		/// <summary>
		/// Catch expression bodied properties and rewrite them into full fat get-only ones so <see cref="M:VRage.Scripting.Rewriters.PerfCountingRewriter.VisitAccessorDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax)" /> can handle the rest.
		/// </summary>
		public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
		{
			node = (PropertyDeclarationSyntax)base.VisitPropertyDeclaration(node);
			if (node.ExpressionBody != null)
			{
				return node.WithExpressionBody(null).WithAccessorList(ArrowToGetter(node.ExpressionBody));
			}
			return node;
		}

		public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node)
		{
			node = (AccessorDeclarationSyntax)base.VisitAccessorDeclaration(node);
			return node.WithBody(ProcessMethod(node.Body));
		}

		public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node)
		{
			node = (OperatorDeclarationSyntax)base.VisitOperatorDeclaration(node);
			if (node.ExpressionBody != null)
			{
				return node.WithExpressionBody(null).WithBody(ProcessMethod(node.ExpressionBody, isVoid: false));
			}
			return node.WithBody(InjectMethodBody(node.Body));
		}

		/// <summary>
		/// Should not be allowed for neither Mods nor PB but inject it too, just in case
		/// </summary>
		public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node)
		{
			node = (DestructorDeclarationSyntax)base.VisitDestructorDeclaration(node);
			return node.WithBody(InjectMethodBody(node.Body));
		}

		public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
		{
			node = (ConstructorDeclarationSyntax)base.VisitConstructorDeclaration(node);
			return node.WithBody(InjectMethodBody(node.Body));
		}

		public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
		{
			INamedTypeSymbol type = m_semanticModel.GetTypeInfo(node).ConvertedType as INamedTypeSymbol;
			node = (SimpleLambdaExpressionSyntax)base.VisitSimpleLambdaExpression(node);
			BlockSyntax blockSyntax = ProcessAnonymousFunction(node, type);
			if (blockSyntax == null)
			{
				return node;
			}
			return node.WithBody(blockSyntax);
		}

		public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
		{
			INamedTypeSymbol type = m_semanticModel.GetTypeInfo(node).ConvertedType as INamedTypeSymbol;
			node = (AnonymousMethodExpressionSyntax)base.VisitAnonymousMethodExpression(node);
			BlockSyntax blockSyntax = ProcessAnonymousFunction(node, type);
			if (blockSyntax == null)
			{
				return node;
			}
			return node.WithBody(blockSyntax);
		}

		public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
		{
			INamedTypeSymbol type = m_semanticModel.GetTypeInfo(node).ConvertedType as INamedTypeSymbol;
			node = (ParenthesizedLambdaExpressionSyntax)base.VisitParenthesizedLambdaExpression(node);
			BlockSyntax blockSyntax = ProcessAnonymousFunction(node, type);
			if (blockSyntax == null)
			{
				return node;
			}
			return node.WithBody(blockSyntax);
		}

		/// <summary>
		/// Injects `yield return`-statement's expression with call to <see cref="M:VRage.Scripting.CompilerMethods.ModPerfCounter.YieldGuard``1(System.Int32,``0)" /> to prevent <see cref="!:ModPerfCounter.CallStackDepth" /> from leaking.
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
			return SyntaxFactory.Block(node.WithExpression(YieldGuard(node.Expression, genericAttribute)), ReenterYieldMethodCall());
		}

		private BlockSyntax ProcessMethod(BlockSyntax body)
		{
			if (body == null)
			{
				return null;
			}
			return InjectMethodBody(body);
		}

		private BlockSyntax ProcessMethod(ExpressionSyntax body, bool isVoid)
		{
			return ProcessMethod(ExpressionBodyToBlock(body, isVoid));
		}

		private BlockSyntax ProcessMethod(ArrowExpressionClauseSyntax expression, bool isVoid)
		{
			return ProcessMethod(expression.Expression, isVoid);
		}

		private BlockSyntax ExpressionBodyToBlock(ExpressionSyntax expression, bool isVoid)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			StatementSyntax statementSyntax = isVoid ? ((StatementSyntax)SyntaxFactory.ExpressionStatement(expression)) : ((StatementSyntax)SyntaxFactory.ReturnStatement(expression));
			return SyntaxFactory.Block(statementSyntax);
		}

		private BlockSyntax InjectMethodBody(BlockSyntax block)
		{
			return SyntaxFactory.Block(SyntaxFactory.TryStatement(SyntaxFactory.Block(EnterMethodCall(), block), SyntaxFactory.List<CatchClauseSyntax>(), SyntaxFactory.FinallyClause(SyntaxFactory.Block(ExitMethodCall()))));
		}

		private AccessorListSyntax ArrowToGetter(ArrowExpressionClauseSyntax expression)
		{
			return SyntaxFactory.AccessorList(SyntaxFactory.SingletonList(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration, ProcessMethod(expression.Expression, isVoid: false))));
		}

		private BlockSyntax ProcessAnonymousFunction(AnonymousFunctionExpressionSyntax node, INamedTypeSymbol type)
		{
			BlockSyntax blockSyntax = node.Body as BlockSyntax;
			if (blockSyntax != null)
			{
				return ProcessMethod(blockSyntax);
			}
			if (type == null || type.DelegateInvokeMethod == null)
			{
				return null;
			}
			return ProcessMethod(node.Body as ExpressionSyntax, type.DelegateInvokeMethod.ReturnsVoid);
		}

		public static SyntaxTree Rewrite(CSharpCompilation compilation, SyntaxTree syntaxTree, int modId)
		{
			SyntaxNode root = new PerfCountingRewriter(compilation, syntaxTree, modId).Visit(syntaxTree.GetRoot());
			return syntaxTree.WithRootAndOptions(root, syntaxTree.Options);
		}
	}
}
