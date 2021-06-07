using System;
using System.Globalization;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.ModAPI.Ingame
{
	public struct TerminalActionParameter
	{
		/// <summary>
		/// Gets an empty parameter.
		/// </summary>
		public static readonly TerminalActionParameter Empty;

		public readonly TypeCode TypeCode;

		public readonly object Value;

		public bool IsEmpty => TypeCode == TypeCode.Empty;

		private static Type ToType(TypeCode code)
		{
			switch (code)
			{
			case TypeCode.Boolean:
				return typeof(bool);
			case TypeCode.Byte:
				return typeof(byte);
			case TypeCode.Char:
				return typeof(char);
			case TypeCode.DateTime:
				return typeof(DateTime);
			case TypeCode.Decimal:
				return typeof(decimal);
			case TypeCode.Double:
				return typeof(double);
			case TypeCode.Int16:
				return typeof(short);
			case TypeCode.Int32:
				return typeof(int);
			case TypeCode.Int64:
				return typeof(long);
			case TypeCode.SByte:
				return typeof(sbyte);
			case TypeCode.Single:
				return typeof(float);
			case TypeCode.String:
				return typeof(string);
			case TypeCode.UInt16:
				return typeof(ushort);
			case TypeCode.UInt32:
				return typeof(uint);
			case TypeCode.UInt64:
				return typeof(ulong);
			default:
				return null;
			}
		}

		/// <summary>
		/// Creates a <see cref="T:Sandbox.ModAPI.Ingame.TerminalActionParameter" /> from a serialized value in a string and a type code.
		/// </summary>
		/// <param name="serializedValue"></param>
		/// <param name="typeCode"></param>
		/// <returns></returns>
		public static TerminalActionParameter Deserialize(string serializedValue, TypeCode typeCode)
		{
			AssertTypeCodeValidity(typeCode);
			Type left = ToType(typeCode);
			if (left == null)
			{
				return Empty;
			}
			object value = Convert.ChangeType(serializedValue, typeCode, CultureInfo.InvariantCulture);
			return new TerminalActionParameter(typeCode, value);
		}

		/// <summary>
		/// Creates a <see cref="T:Sandbox.ModAPI.Ingame.TerminalActionParameter" /> from the given value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TerminalActionParameter Get(object value)
		{
			if (value == null)
			{
				return Empty;
			}
			TypeCode typeCode = Type.GetTypeCode(value.GetType());
			AssertTypeCodeValidity(typeCode);
			return new TerminalActionParameter(typeCode, value);
		}

		private static void AssertTypeCodeValidity(TypeCode typeCode)
		{
			if ((uint)typeCode <= 2u)
			{
				throw new ArgumentException("Only primitive types are allowed for action parameters", "value");
			}
		}

		private TerminalActionParameter(TypeCode typeCode, object value)
		{
			TypeCode = typeCode;
			Value = value;
		}

		public MyObjectBuilder_ToolbarItemActionParameter GetObjectBuilder()
		{
			MyObjectBuilder_ToolbarItemActionParameter myObjectBuilder_ToolbarItemActionParameter = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemActionParameter>();
			myObjectBuilder_ToolbarItemActionParameter.TypeCode = TypeCode;
			myObjectBuilder_ToolbarItemActionParameter.Value = ((Value == null) ? null : Convert.ToString(Value, CultureInfo.InvariantCulture));
			return myObjectBuilder_ToolbarItemActionParameter;
		}
	}
}
