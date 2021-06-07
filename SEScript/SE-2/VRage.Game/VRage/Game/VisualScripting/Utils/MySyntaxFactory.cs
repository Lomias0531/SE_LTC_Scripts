using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRageMath;

namespace VRage.Game.VisualScripting.Utils
{
	public static class MySyntaxFactory
	{
		/// <summary>
		/// Creates using directive from identifiers "System","Collection".
		/// </summary>
		/// <param name="identifiers">Separated identifiers.</param>
		/// <returns>Null if less than 2 identifiers passed.</returns>
		public static UsingDirectiveSyntax UsingStatementSyntax(string @namespace)
		{
			string[] array = @namespace.Split(new char[1]
			{
				'.'
			});
			if (array.Length < 2)
			{
				return SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(array[0]));
			}
			QualifiedNameSyntax qualifiedNameSyntax = SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(array[0]), SyntaxFactory.IdentifierName(array[1]));
			for (int i = 2; i < array.Length; i++)
			{
				qualifiedNameSyntax = SyntaxFactory.QualifiedName(qualifiedNameSyntax, SyntaxFactory.IdentifierName(array[i]));
			}
			return SyntaxFactory.UsingDirective(qualifiedNameSyntax);
		}

		/// <summary>
		/// Generic Field declaration creation.
		/// </summary>
		/// <param name="type">Field type.</param>
		/// <param name="fieldVariableName">Name of the field.</param>
		/// <param name="modifiers">Modifiers - null creates public modifier list</param>
		/// <returns>Complete field declaration syntax.</returns>
		public static FieldDeclarationSyntax GenericFieldDeclaration(Type type, string fieldVariableName, SyntaxTokenList? modifiers = null)
		{
			if (!modifiers.HasValue)
			{
				modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			}
			return SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(GenericTypeSyntax(type), SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(fieldVariableName))))).WithModifiers(modifiers.Value);
		}

		public static MethodDeclarationSyntax MethodDeclaration(MethodInfo method)
		{
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			ParameterInfo[] parameters = method.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				ParameterSyntax node = SyntaxFactory.Parameter(SyntaxFactory.Identifier(parameterInfo.Name)).WithType(GenericTypeSyntax(parameterInfo.ParameterType));
				list.Add(node);
				if (i < parameters.Length - 1)
				{
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			return SyntaxFactory.MethodDeclaration(GenericTypeSyntax(method.ReturnType), SyntaxFactory.Identifier(method.Name)).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(list)))
				.WithBody(SyntaxFactory.Block());
		}

		/// <summary>
		/// Generic type syntax creation.
		/// </summary>
		/// <param name="type">C# type</param>
		/// <returns>Type Syntax</returns>
		public static TypeSyntax GenericTypeSyntax(Type type)
		{
			if (!type.IsGenericType)
			{
				if (type == typeof(void))
				{
					return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
				}
				return SyntaxFactory.IdentifierName(type.FullName);
			}
			Type[] genericArguments = type.GetGenericArguments();
			List<TypeSyntax> list = new List<TypeSyntax>();
			Type[] array = genericArguments;
			foreach (Type type2 in array)
			{
				list.Add(GenericTypeSyntax(type2));
			}
			return SyntaxFactory.GenericName(SyntaxFactory.Identifier(type.Name.Remove(type.Name.IndexOf('`'))), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(list)));
		}

		/// <summary>
		/// Creates generic type object creation syntax. new type(argumentExpressions).
		/// </summary>
		/// <param name="type">Type of created object.</param>
		/// <param name="argumentExpressions">Initializer argument expressions.</param>
		/// <returns>Object creatation expression.</returns>
		public static ObjectCreationExpressionSyntax GenericObjectCreation(Type type, IEnumerable<ExpressionSyntax> argumentExpressions = null)
		{
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				IdentifierNameSyntax node = SyntaxFactory.IdentifierName(genericArguments[i].FullName);
				list.Add(node);
				if (i < genericArguments.Length - 1)
				{
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			TypeSyntax type2 = GenericTypeSyntax(type);
			List<ArgumentSyntax> list2 = new List<ArgumentSyntax>();
			if (argumentExpressions != null)
			{
				foreach (ExpressionSyntax argumentExpression in argumentExpressions)
				{
					ArgumentSyntax item = SyntaxFactory.Argument(argumentExpression);
					list2.Add(item);
				}
			}
			return SyntaxFactory.ObjectCreationExpression(type2).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(list2)));
		}

		public static LiteralExpressionSyntax Literal(string typeSignature, string val)
		{
			Type type = MyVisualScriptingProxy.GetType(typeSignature);
			if (type != null)
			{
				if (type == typeof(float))
				{
					float value = string.IsNullOrEmpty(val) ? 0f : float.Parse(val);
					return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value));
				}
				if (type == typeof(int))
				{
					int value2 = (!string.IsNullOrEmpty(val)) ? int.Parse(val) : 0;
					return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value2));
				}
				if (type == typeof(long))
				{
					long value3 = string.IsNullOrEmpty(val) ? 0 : long.Parse(val);
					return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value3));
				}
				if (type == typeof(bool))
				{
					if (NormalizeBool(val) == "true")
					{
						return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
					}
					return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				}
			}
			return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(val));
		}

		public static ArgumentSyntax ConstantArgument(string typeSignature, string value)
		{
			Type type = MyVisualScriptingProxy.GetType(typeSignature);
			if (type == typeof(Color) || type.IsEnum)
			{
				return SyntaxFactory.Argument(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(typeSignature), SyntaxFactory.IdentifierName(value)));
			}
			if (type == typeof(Vector3D))
			{
				return SyntaxFactory.Argument(NewVector3D(value));
			}
			return SyntaxFactory.Argument(Literal(typeSignature, value));
		}

		public static ArgumentSyntax ConstantDefaultArgument(Type type)
		{
			if (type.IsClass)
			{
				return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
			}
			if (type == typeof(int) || type == typeof(float) || type == typeof(long) || type == typeof(double))
			{
				return SyntaxFactory.Argument(Literal(type.Signature(), "0"));
			}
			return SyntaxFactory.Argument(SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(type.Signature())).WithArgumentList(SyntaxFactory.ArgumentList()));
		}

		public static ObjectCreationExpressionSyntax NewVector3D(string vectorData)
		{
			string[] array = vectorData.Split(new char[1]
			{
				' '
			});
			double value = double.Parse(array[0].Replace("X:", ""));
			double value2 = double.Parse(array[1].Replace("Y:", ""));
			double value3 = double.Parse(array[2].Replace("Z:", ""));
			return SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("VRageMath.Vector3D")).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[5]
			{
				SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value))),
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value2))),
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value3)))
			})));
		}

		public static ObjectCreationExpressionSyntax NewVector3D(Vector3D vector)
		{
			return SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("VRageMath.Vector3D")).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[5]
			{
				SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(vector.X))),
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(vector.Y))),
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(vector.Z)))
			})));
		}

		/// <summary>
		/// deletageIdentifier += methodName;
		/// </summary>
		/// <param name="deletageIdentifier"></param>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public static ExpressionStatementSyntax DelegateAssignment(string deletageIdentifier, string methodName)
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.AddAssignmentExpression, SyntaxFactory.IdentifierName(deletageIdentifier), SyntaxFactory.IdentifierName(methodName)));
		}

		/// <summary>
		/// delegateIdentifier -= methodName
		/// </summary>
		/// <param name="deletageIdentifier"></param>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public static ExpressionStatementSyntax DelegateRemoval(string deletageIdentifier, string methodName)
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SubtractAssignmentExpression, SyntaxFactory.IdentifierName(deletageIdentifier), SyntaxFactory.IdentifierName(methodName)));
		}

		/// <summary>
		/// Creates public class with empty body.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static ClassDeclarationSyntax PublicClass(string name)
		{
			return SyntaxFactory.ClassDeclaration(name).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).NormalizeWhitespace();
		}

		/// <summary>
		/// Creates basic member declaration with provided name and type.
		/// </summary>
		/// <param name="memberName">Valid name.</param>
		/// <param name="memberType">Valid type.</param>
		/// <returns></returns>
		public static MemberDeclarationSyntax MemberDeclaration(string memberName, string memberType)
		{
			VariableDeclarationSyntax variableDeclarationSyntax = SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(memberType));
			VariableDeclaratorSyntax node = SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(memberName));
			return SyntaxFactory.FieldDeclaration(variableDeclarationSyntax.WithVariables(SyntaxFactory.SingletonSeparatedList(node))).NormalizeWhitespace();
		}

		/// <summary>
		/// Creates assignment expression for given variableName and value.
		/// </summary>
		/// <param name="variableName"></param>
		/// <param name="value"></param>
		/// <param name="expressionKind"></param>
		/// <returns></returns>
		public static ExpressionStatementSyntax VariableAssignmentExpression(string variableName, string value, SyntaxKind expressionKind)
		{
			bool flag = false;
			SyntaxToken token;
			switch (expressionKind)
			{
			case SyntaxKind.StringLiteralExpression:
				token = SyntaxFactory.Literal(value);
				break;
			case SyntaxKind.TrueLiteralExpression:
			case SyntaxKind.FalseLiteralExpression:
				return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), SyntaxFactory.LiteralExpression(expressionKind))).NormalizeWhitespace();
			default:
				if (Enumerable.Contains(value, '-'))
				{
					flag = true;
					value = value.Replace("-", "");
				}
				token = SyntaxFactory.ParseToken(value);
				break;
			}
			LiteralExpressionSyntax literalExpressionSyntax = SyntaxFactory.LiteralExpression(expressionKind, token);
			if (flag)
			{
				return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), SyntaxFactory.PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, literalExpressionSyntax))).NormalizeWhitespace();
			}
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), literalExpressionSyntax)).NormalizeWhitespace();
		}

		public static AssignmentExpressionSyntax VariableAssignment(string variableName, ExpressionSyntax rightSide)
		{
			return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), rightSide);
		}

		public static string NormalizeBool(string value)
		{
			value = value.ToLower();
			if (value == "0")
			{
				return "false";
			}
			if (value == "1")
			{
				return "true";
			}
			return value;
		}

		/// <summary>
		/// Creates assignment expression variableName = vectorType(x,y,z);
		/// </summary>
		/// <param name="variableName"></param>
		/// <param name="vectorType"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static ExpressionStatementSyntax VectorAssignmentExpression(string variableName, string vectorType, double x, double y, double z)
		{
			ArgumentSyntax node = VectorArgumentSyntax(x);
			ArgumentSyntax node2 = VectorArgumentSyntax(y);
			ArgumentSyntax node3 = VectorArgumentSyntax(z);
			ArgumentListSyntax argumentList = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[5]
			{
				node,
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				node2,
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				node3
			}));
			ObjectCreationExpressionSyntax right = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(vectorType)).WithArgumentList(argumentList);
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), right)).NormalizeWhitespace();
		}

		/// <summary>
		/// Expression of type "var variableName = new type(values....);"
		/// </summary>
		public static LocalDeclarationStatementSyntax ReferenceTypeInstantiation(string variableName, string type, params LiteralExpressionSyntax[] values)
		{
			SyntaxNodeOrTokenList nodesAndTokens = SyntaxFactory.NodeOrTokenList();
			for (int i = 0; i < values.Length; i++)
			{
				LiteralExpressionSyntax expression = values[i];
				nodesAndTokens.Add(SyntaxFactory.Argument(expression));
				if (i + 1 != values.Length)
				{
					nodesAndTokens.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			ArgumentListSyntax argumentList = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(nodesAndTokens));
			ObjectCreationExpressionSyntax initializer = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(type)).WithArgumentList(argumentList);
			return LocalVariable(type, variableName, initializer);
		}

		/// <summary>
		/// Creates arguments of type NumericLiteral for given value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static ArgumentSyntax VectorArgumentSyntax(double value)
		{
			return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value)));
		}

		/// <summary>
		/// Craetes expression of type "var resultVariableName = leftSide 'operation' rightSide;"
		/// </summary>
		/// <param name="resultVariableName"></param>
		/// <param name="leftSide"></param>
		/// <param name="rightSide"></param>
		/// <param name="operation"></param>
		/// <returns></returns>
		public static LocalDeclarationStatementSyntax ArithmeticStatement(string resultVariableName, string leftSide, string rightSide, string operation)
		{
			VariableDeclarationSyntax variableDeclarationSyntax = SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"));
			ExpressionSyntax value = SyntaxFactory.ParseExpression(leftSide + " " + operation + " " + rightSide);
			SeparatedSyntaxList<VariableDeclaratorSyntax> variables = SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(resultVariableName)).WithInitializer(SyntaxFactory.EqualsValueClause(value)));
			return SyntaxFactory.LocalDeclarationStatement(variableDeclarationSyntax.WithVariables(variables));
		}

		/// <summary>
		/// Creates conditional statement with given statements for true/false clauses and condition syntax.
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="statements"></param>
		/// <param name="elseStatements"></param>
		/// <returns></returns>
		public static IfStatementSyntax IfExpressionSyntax(ExpressionSyntax condition, List<StatementSyntax> statements, List<StatementSyntax> elseStatements = null)
		{
			if (elseStatements == null || elseStatements.Count == 0)
			{
				return SyntaxFactory.IfStatement(condition, SyntaxFactory.Block(statements)).NormalizeWhitespace();
			}
			return SyntaxFactory.IfStatement(condition, SyntaxFactory.Block(statements)).WithElse(SyntaxFactory.ElseClause(SyntaxFactory.Block(elseStatements))).NormalizeWhitespace();
		}

		/// <summary>
		/// Creates conditional statement with given statements for true/false clauses and condition variable name.
		/// </summary>
		/// <param name="conditionVariableName"></param>
		/// <param name="statements"></param>
		/// <param name="elseStatements"></param>
		/// <returns></returns>
		public static IfStatementSyntax IfExpressionSyntax(string conditionVariableName, List<StatementSyntax> statements, List<StatementSyntax> elseStatements)
		{
			return IfExpressionSyntax(SyntaxFactory.IdentifierName(conditionVariableName), statements, elseStatements);
		}

		/// <summary>
		/// Creates declaration statement for expression of "var varName = (type)castedVariableName;"
		/// </summary>
		/// <param name="castedVariableName"></param>
		/// <param name="type"></param>
		/// <param name="resultVariableName"></param>
		/// <returns></returns>
		public static LocalDeclarationStatementSyntax CastExpression(string castedVariableName, string type, string resultVariableName)
		{
			VariableDeclarationSyntax variableDeclarationSyntax = SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"));
			SeparatedSyntaxList<VariableDeclaratorSyntax> variables = SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(resultVariableName)).WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.CastExpression(SyntaxFactory.PredefinedType(SyntaxFactory.ParseToken(type)), SyntaxFactory.IdentifierName(castedVariableName)))));
			return SyntaxFactory.LocalDeclarationStatement(variableDeclarationSyntax.WithVariables(variables)).NormalizeWhitespace();
		}

		/// <summary>
		/// Creates public void methodName(paramType1 paramName1,..,paramTypeN paramNameN) {}.
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="inputParameterNames"></param>
		/// <param name="inputParameterTypes"></param>
		/// <returns></returns>
		public static MethodDeclarationSyntax PublicMethodDeclaration(string methodName, SyntaxKind predefinedReturnType, List<string> inputParameterNames = null, List<string> inputParameterTypes = null, List<string> outputParameterNames = null, List<string> outputParameterTypes = null)
		{
			MethodDeclarationSyntax methodDeclarationSyntax = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(predefinedReturnType)), SyntaxFactory.Identifier(methodName)).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));
			List<SyntaxNodeOrToken> list = null;
			if (inputParameterNames != null)
			{
				list = Parameters(inputParameterNames, inputParameterTypes);
				if (outputParameterNames != null && outputParameterNames.Count > 0 && inputParameterNames.Count > 0)
				{
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
					list.AddRange(Parameters(outputParameterNames, outputParameterTypes, areOutputs: true));
				}
			}
			else if (outputParameterNames != null)
			{
				list = Parameters(outputParameterNames, outputParameterTypes, areOutputs: true);
			}
			if (list != null)
			{
				ParameterListSyntax parameterList = SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(list));
				return methodDeclarationSyntax.WithParameterList(parameterList).WithBody(SyntaxFactory.Block()).NormalizeWhitespace();
			}
			return methodDeclarationSyntax.WithBody(SyntaxFactory.Block()).NormalizeWhitespace();
		}

		/// <summary>
		/// Craetes parameter list syntax from given parameterNames.
		/// </summary>
		/// <param name="parameterNames"></param>
		/// <param name="types"></param>
		/// <param name="areOutputs">Adds out keyword to every single one.</param>
		/// <returns></returns>
		private static List<SyntaxNodeOrToken> Parameters(List<string> parameterNames, List<string> types, bool areOutputs = false)
		{
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			for (int i = 0; i < parameterNames.Count; i++)
			{
				string name = parameterNames[i];
				ParameterSyntax parameterSyntax = null;
				string text = types[i];
				Type type = MyVisualScriptingProxy.GetType(text);
				parameterSyntax = ((!(type == null)) ? ParameterSyntax(name, type) : ParameterSyntax(name, text));
				if (areOutputs)
				{
					parameterSyntax = parameterSyntax.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.OutKeyword)));
				}
				list.Add(parameterSyntax);
				if (i < parameterNames.Count - 1)
				{
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			return list;
		}

		/// <summary>
		/// Creates parameter syntax out of given data.
		/// </summary>
		/// <param name="name">Unique variable name within script class.</param>
		/// <param name="typeSyntaxNode"></param>
		/// <returns></returns>
		public static ParameterSyntax ParameterSyntax(string name, string typeIdentifier)
		{
			return SyntaxFactory.Parameter(SyntaxFactory.Identifier(name)).WithType(SyntaxFactory.ParseTypeName(typeIdentifier));
		}

		/// <summary>
		/// Creates parameter syntax out of given data.
		/// </summary>
		/// <param name="name">Unique variable name within script class.</param>
		/// <param name="type">The type of the parameter</param>
		/// <returns></returns>
		public static ParameterSyntax ParameterSyntax(string name, Type type)
		{
			return SyntaxFactory.Parameter(SyntaxFactory.Identifier(name)).WithType(GenericTypeSyntax(type));
		}

		/// <summary>
		/// Creates syntax for output variable declaration.
		/// Can be used to create var type.
		/// </summary>
		/// <param name="typeData"></param>
		/// <param name="variableName"></param>
		/// <returns></returns>
		public static LocalDeclarationStatementSyntax LocalVariable(string typeData, string variableName, ExpressionSyntax initializer = null)
		{
			VariableDeclarationSyntax variableDeclarationSyntax = SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName((typeData.Length > 0) ? typeData : "var"));
			VariableDeclaratorSyntax variableDeclaratorSyntax = SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(variableName));
			if (initializer != null)
			{
				variableDeclaratorSyntax = variableDeclaratorSyntax.WithInitializer(SyntaxFactory.EqualsValueClause(initializer));
			}
			return SyntaxFactory.LocalDeclarationStatement(variableDeclarationSyntax.WithVariables(SyntaxFactory.SingletonSeparatedList(variableDeclaratorSyntax))).NormalizeWhitespace();
		}

		public static LocalDeclarationStatementSyntax LocalVariable(Type type, string varName, ExpressionSyntax initializerExpressionSyntax = null)
		{
			return SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(GenericTypeSyntax(type), SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(varName)).WithInitializer(SyntaxFactory.EqualsValueClause(initializerExpressionSyntax)))));
		}

		/// <summary>
		///  Creates syntax of type "Method(arg0,arg1,..);"
		/// </summary>
		/// <param name="className">Used for static method invocation.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="orderedVariableNames">Should be the same order as method signature.</param>
		/// <returns></returns>
		public static InvocationExpressionSyntax MethodInvocation(string methodName, IEnumerable<string> orderedVariableNames, string className = null)
		{
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			if (orderedVariableNames != null)
			{
				foreach (string orderedVariableName in orderedVariableNames)
				{
					list.Add(CreateArgumentSyntax(orderedVariableName));
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			if (list.Count > 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			ArgumentListSyntax argumentList = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list));
			if (string.IsNullOrEmpty(className))
			{
				return SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(methodName)).WithArgumentList(argumentList);
			}
			return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(className), SyntaxFactory.IdentifierName(methodName))).WithArgumentList(argumentList);
		}

		public static InvocationExpressionSyntax MethodInvocation(string methodName, IEnumerable<string> inputVariableNames, IEnumerable<string> outputVarNames, string className = null)
		{
			List<ArgumentSyntax> list = new List<ArgumentSyntax>();
			if (inputVariableNames != null)
			{
				foreach (string inputVariableName in inputVariableNames)
				{
					list.Add(CreateArgumentSyntax(inputVariableName));
				}
			}
			if (outputVarNames != null)
			{
				foreach (string outputVarName in outputVarNames)
				{
					list.Add(CreateOutArgumentSyntax(outputVarName));
				}
			}
			ArgumentListSyntax argumentList = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(list));
			if (string.IsNullOrEmpty(className))
			{
				return SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(methodName)).WithArgumentList(argumentList);
			}
			return SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(className), SyntaxFactory.IdentifierName(methodName))).WithArgumentList(argumentList);
		}

		public static InvocationExpressionSyntax MethodInvocationExpressionSyntax(IdentifierNameSyntax methodName, ArgumentListSyntax arguments, IdentifierNameSyntax instance = null)
		{
			InvocationExpressionSyntax invocationExpressionSyntax = null;
			invocationExpressionSyntax = ((instance != null) ? SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, instance, methodName)) : SyntaxFactory.InvocationExpression(methodName));
			return invocationExpressionSyntax.WithArgumentList(arguments);
		}

		/// <summary>
		/// Craetes simple argument.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static ArgumentSyntax CreateArgumentSyntax(string value)
		{
			return SyntaxFactory.Argument(SyntaxFactory.ParseExpression(value));
		}

		public static ArgumentSyntax CreateOutArgumentSyntax(string value)
		{
			return SyntaxFactory.Argument(SyntaxFactory.ParseExpression(value)).WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword));
		}

		/// <summary>
		/// Creates assignment for given variable.
		/// variableName = rightSide;
		/// </summary>
		/// <param name="variableName"></param>
		/// <param name="rightSide"></param>
		/// <returns></returns>
		public static ExpressionStatementSyntax SimpleAssignment(string variableName, ExpressionSyntax rightSide)
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(variableName), rightSide)).NormalizeWhitespace();
		}

		/// <summary>
		/// Creates corresponding parameterless constructor to passed class.
		/// </summary>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		public static ConstructorDeclarationSyntax Constructor(ClassDeclarationSyntax classDeclaration)
		{
			return SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier(classDeclaration.Identifier.Text)).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithBody(SyntaxFactory.Block())
				.NormalizeWhitespace();
		}
	}
}
