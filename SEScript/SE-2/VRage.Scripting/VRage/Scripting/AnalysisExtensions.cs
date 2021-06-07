using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace VRage.Scripting
{
	/// <summary>
	///     Contains various utilities used by the scripting engine.
	/// </summary>
	internal static class AnalysisExtensions
	{
		public static ISymbol GetOverriddenSymbol(this ISymbol symbol)
		{
			if (!symbol.IsOverride)
			{
				return null;
			}
			ITypeSymbol typeSymbol = symbol as ITypeSymbol;
			if (typeSymbol != null)
			{
				return typeSymbol.BaseType;
			}
			IEventSymbol eventSymbol = symbol as IEventSymbol;
			if (eventSymbol != null)
			{
				return eventSymbol.OverriddenEvent;
			}
			IPropertySymbol propertySymbol = symbol as IPropertySymbol;
			if (propertySymbol != null)
			{
				return propertySymbol.OverriddenProperty;
			}
			return (symbol as IMethodSymbol)?.OverriddenMethod;
		}

		public static bool IsMemberSymbol(this ISymbol symbol)
		{
			if (!(symbol is IEventSymbol) && !(symbol is IFieldSymbol) && !(symbol is IPropertySymbol))
			{
				return symbol is IMethodSymbol;
			}
			return true;
		}

		public static BaseMethodDeclarationSyntax WithBody(this BaseMethodDeclarationSyntax item, BlockSyntax body)
		{
			ConstructorDeclarationSyntax constructorDeclarationSyntax = item as ConstructorDeclarationSyntax;
			if (constructorDeclarationSyntax != null)
			{
				return constructorDeclarationSyntax.WithBody(body);
			}
			ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax = item as ConversionOperatorDeclarationSyntax;
			if (conversionOperatorDeclarationSyntax != null)
			{
				return conversionOperatorDeclarationSyntax.WithBody(body);
			}
			DestructorDeclarationSyntax destructorDeclarationSyntax = item as DestructorDeclarationSyntax;
			if (destructorDeclarationSyntax != null)
			{
				return destructorDeclarationSyntax.WithBody(body);
			}
			MethodDeclarationSyntax methodDeclarationSyntax = item as MethodDeclarationSyntax;
			if (methodDeclarationSyntax != null)
			{
				return methodDeclarationSyntax.WithBody(body);
			}
			OperatorDeclarationSyntax operatorDeclarationSyntax = item as OperatorDeclarationSyntax;
			if (operatorDeclarationSyntax != null)
			{
				return operatorDeclarationSyntax.WithBody(body);
			}
			throw new ArgumentException("Unknown " + typeof(BaseMethodDeclarationSyntax).FullName, "item");
		}

		public static AnonymousFunctionExpressionSyntax WithBody(this AnonymousFunctionExpressionSyntax item, CSharpSyntaxNode body)
		{
			AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax = item as AnonymousMethodExpressionSyntax;
			if (anonymousMethodExpressionSyntax != null)
			{
				return anonymousMethodExpressionSyntax.WithBody(body);
			}
			ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax = item as ParenthesizedLambdaExpressionSyntax;
			if (parenthesizedLambdaExpressionSyntax != null)
			{
				return parenthesizedLambdaExpressionSyntax.WithBody(body);
			}
			SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax = item as SimpleLambdaExpressionSyntax;
			if (simpleLambdaExpressionSyntax != null)
			{
				return simpleLambdaExpressionSyntax.WithBody(body);
			}
			throw new ArgumentException("Unknown " + typeof(AnonymousFunctionExpressionSyntax).FullName, "item");
		}

		public static bool IsInSource(this ISymbol symbol)
		{
			for (int i = 0; i < symbol.Locations.Length; i++)
			{
				if (!symbol.Locations[i].IsInSource)
				{
					return false;
				}
			}
			return true;
		}
	}
}
