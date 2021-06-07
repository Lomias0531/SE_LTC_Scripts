using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Sandbox.ModAPI;
using System.Collections.Immutable;

namespace VRage.Scripting.Analyzers
{
	/// <summary>
	///     This analyzer scans a syntax tree for prohibited type and member references.
	/// </summary>
	internal class WhitelistDiagnosticAnalyzer : DiagnosticAnalyzer
	{
		internal static readonly SyntaxAnnotation INJECTED_ANNOTATION = new SyntaxAnnotation("Injected");

		internal static readonly DiagnosticDescriptor PROHIBITED_MEMBER_RULE = new DiagnosticDescriptor("ProhibitedMemberRule", "Prohibited Type Or Member", "The type or member '{0}' is prohibited", "Whitelist", DiagnosticSeverity.Error, true, null, null);

		internal static readonly DiagnosticDescriptor PROHIBITED_LANGUAGE_ELEMENT_RULE = new DiagnosticDescriptor("ProhibitedLanguageElement", "Prohibited Language Element", "The language element '{0}' is prohibited", "Whitelist", DiagnosticSeverity.Error, true, null, null);

		private readonly MyScriptWhitelist m_whitelist;

		private readonly MyWhitelistTarget m_target;

		private readonly ImmutableArray<DiagnosticDescriptor> m_supportedDiagnostics = ImmutableArray.Create(PROHIBITED_MEMBER_RULE, PROHIBITED_LANGUAGE_ELEMENT_RULE);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => m_supportedDiagnostics;

		public WhitelistDiagnosticAnalyzer(MyScriptWhitelist whitelist, MyWhitelistTarget target)
		{
			m_whitelist = whitelist;
			m_target = target;
		}

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction<SyntaxKind>(Analyze, SyntaxKind.PragmaWarningDirectiveTrivia, SyntaxKind.DestructorDeclaration, SyntaxKind.AliasQualifiedName, SyntaxKind.QualifiedName, SyntaxKind.GenericName, SyntaxKind.IdentifierName);
		}

		private void Analyze(SyntaxNodeAnalysisContext context)
		{
			SyntaxNode node = context.Node;
			if (node.HasAnnotation(INJECTED_ANNOTATION))
			{
				return;
			}
			if (node.Kind() == SyntaxKind.DestructorDeclaration && m_target == MyWhitelistTarget.Ingame)
			{
				Diagnostic diagnostic = Diagnostic.Create(PROHIBITED_LANGUAGE_ELEMENT_RULE, node.GetLocation(), "Finalizer");
				context.ReportDiagnostic(diagnostic);
			}
			else if (node.Kind() == SyntaxKind.PragmaWarningDirectiveTrivia)
			{
				Diagnostic diagnostic2 = Diagnostic.Create(PROHIBITED_LANGUAGE_ELEMENT_RULE, node.GetLocation(), "#pragma warning");
				context.ReportDiagnostic(diagnostic2);
			}
			else
			{
				if (IsQualifiedName(node.Parent))
				{
					return;
				}
				SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(node);
				if (symbolInfo.Symbol != null && !symbolInfo.Symbol.IsInSource())
				{
					if (!m_whitelist.IsWhitelisted(symbolInfo.Symbol, m_target))
					{
						Diagnostic diagnostic3 = Diagnostic.Create(PROHIBITED_MEMBER_RULE, node.GetLocation(), symbolInfo.Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
						context.ReportDiagnostic(diagnostic3);
					}
					else
					{
						string key = symbolInfo.Symbol.ContainingNamespace.ToString();
						lock (MyScriptCompiler.UsedNamespaces)
						{
							if (MyScriptCompiler.UsedNamespaces.ContainsKey(key))
							{
								MyScriptCompiler.UsedNamespaces[key]++;
							}
							else
							{
								MyScriptCompiler.UsedNamespaces.Add(key, 1);
							}
						}
					}
				}
			}
		}

		private bool IsQualifiedName(SyntaxNode arg)
		{
			SyntaxKind syntaxKind = arg.Kind();
			if (syntaxKind == SyntaxKind.QualifiedName || syntaxKind == SyntaxKind.AliasQualifiedName)
			{
				return true;
			}
			return false;
		}
	}
}
