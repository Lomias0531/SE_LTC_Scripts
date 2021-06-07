using Microsoft.CodeAnalysis;
using System;

namespace VRage.Scripting
{
	/// <summary>
	///     Roslyn does not provide a good way to compare a <see cref="T:System.Type" /> with an <see cref="T:Microsoft.CodeAnalysis.ISymbol" />. These
	///     extensions aim to provide "good enough" comparisons. In addition it adds a few other key types to be used
	///     for the <see cref="T:VRage.Scripting.MyScriptWhitelist" />.
	/// </summary>
	internal static class TypeKeyExtensions
	{
		public static string GetWhitelistKey(this ISymbol symbol, TypeKeyQuantity quantity)
		{
			INamespaceSymbol namespaceSymbol = symbol as INamespaceSymbol;
			if (namespaceSymbol != null)
			{
				return namespaceSymbol.GetWhitelistKey(quantity);
			}
			ITypeSymbol typeSymbol = symbol as ITypeSymbol;
			if (typeSymbol != null)
			{
				return typeSymbol.GetWhitelistKey(quantity);
			}
			IMethodSymbol methodSymbol = symbol as IMethodSymbol;
			if (methodSymbol != null && methodSymbol.IsGenericMethod && !methodSymbol.IsDefinition)
			{
				methodSymbol = methodSymbol.OriginalDefinition;
				if (methodSymbol.IsExtensionMethod && methodSymbol.ReducedFrom != null)
				{
					methodSymbol = methodSymbol.ReducedFrom;
				}
				return methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) + ", " + symbol.ContainingAssembly.Name;
			}
			return (((symbol is IEventSymbol || symbol is IFieldSymbol || symbol is IPropertySymbol || symbol is IMethodSymbol) ? symbol : null) ?? throw new ArgumentException("Invalid symbol type: Expected namespace, type or type member", "symbol")).ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) + ", " + symbol.ContainingAssembly.Name;
		}

		public static string GetWhitelistKey(this ITypeSymbol symbol, TypeKeyQuantity quantity)
		{
			symbol = ResolveRootType(symbol);
			switch (quantity)
			{
			case TypeKeyQuantity.ThisOnly:
				return symbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) + ", " + symbol.ContainingAssembly.Name;
			case TypeKeyQuantity.AllMembers:
				return symbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) + "+*, " + symbol.ContainingAssembly.Name;
			default:
				throw new ArgumentOutOfRangeException("quantity", quantity, null);
			}
		}

		private static ITypeSymbol ResolveRootType(ITypeSymbol symbol)
		{
			INamedTypeSymbol namedTypeSymbol = symbol as INamedTypeSymbol;
			if (namedTypeSymbol != null && namedTypeSymbol.IsGenericType && !namedTypeSymbol.IsDefinition)
			{
				symbol = namedTypeSymbol.OriginalDefinition;
				return symbol;
			}
			IPointerTypeSymbol pointerTypeSymbol = symbol as IPointerTypeSymbol;
			if (pointerTypeSymbol != null)
			{
				return pointerTypeSymbol.PointedAtType;
			}
			return symbol;
		}

		public static string GetWhitelistKey(this INamespaceSymbol symbol, TypeKeyQuantity quantity)
		{
			switch (quantity)
			{
			case TypeKeyQuantity.ThisOnly:
				return symbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) + ", " + symbol.ContainingAssembly.Name;
			case TypeKeyQuantity.AllMembers:
				return symbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) + ".*, " + symbol.ContainingAssembly.Name;
			default:
				throw new ArgumentOutOfRangeException("quantity", quantity, null);
			}
		}
	}
}
