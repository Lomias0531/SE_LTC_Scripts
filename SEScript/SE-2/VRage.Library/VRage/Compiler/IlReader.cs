using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace VRage.Compiler
{
	/// <summary>
	/// Reads method body and returns instructions
	/// </summary>
	public class IlReader
	{
		public class IlInstruction
		{
			public OpCode OpCode;

			public object Operand;

			public long Offset;

			public long LocalVariableIndex;

			public string FormatOperand()
			{
				switch (OpCode.OperandType)
				{
				case OperandType.InlineField:
				case OperandType.InlineMethod:
				case OperandType.InlineTok:
				case OperandType.InlineType:
					if (Operand is MethodInfo)
					{
						MethodInfo methodInfo = (MethodInfo)Operand;
						string arg = methodInfo.ToString().Substring(methodInfo.ReturnType.Name.ToString().Length + 1);
						return $"{methodInfo.ReturnType} {methodInfo.DeclaringType}::{arg}";
					}
					if (Operand is ConstructorInfo)
					{
						ConstructorInfo constructorInfo = (ConstructorInfo)Operand;
						string arg2 = constructorInfo.ToString().Substring("Void".Length + 1);
						return $"{constructorInfo.DeclaringType}::{arg2}";
					}
					return Operand.ToString();
				case OperandType.InlineNone:
					return string.Empty;
				default:
					return Operand.ToString();
				}
			}

			public override string ToString()
			{
				return string.Concat(OpCode, " ", FormatOperand());
			}
		}

		private BinaryReader stream;

		private OpCode[] singleByteOpCode;

		private OpCode[] doubleByteOpCode;

		private byte[] instructions;

		private IList<LocalVariableInfo> locals;

		private ParameterInfo[] parameters;

		private Type[] typeArgs;

		private Type[] methodArgs;

		private MethodBase currentMethod;

		private List<IlInstruction> ilInstructions;

		public IList<LocalVariableInfo> Locals => locals;

		public IlReader()
		{
			CreateOpCodes();
		}

		private void CreateOpCodes()
		{
			singleByteOpCode = new OpCode[225];
			doubleByteOpCode = new OpCode[31];
			FieldInfo[] opCodeFields = GetOpCodeFields();
			for (int i = 0; i < opCodeFields.Length; i++)
			{
				OpCode opCode = (OpCode)opCodeFields[i].GetValue(null);
				if (opCode.OpCodeType != OpCodeType.Nternal)
				{
					if (opCode.Size == 1)
					{
						singleByteOpCode[opCode.Value] = opCode;
					}
					else
					{
						doubleByteOpCode[opCode.Value & 0xFF] = opCode;
					}
				}
			}
		}

		public List<IlInstruction> ReadInstructions(MethodBase method)
		{
			ilInstructions = new List<IlInstruction>();
			currentMethod = method;
			MethodBody methodBody = method.GetMethodBody();
			parameters = method.GetParameters();
			if (methodBody == null)
			{
				return ilInstructions;
			}
			locals = methodBody.LocalVariables;
			instructions = method.GetMethodBody().GetILAsByteArray();
			ByteStream input = new ByteStream(instructions, instructions.Length);
			stream = new BinaryReader(input);
			if (!typeof(ConstructorInfo).IsAssignableFrom(method.GetType()))
			{
				methodArgs = method.GetGenericArguments();
			}
			if (method.DeclaringType != null)
			{
				typeArgs = method.DeclaringType.GetGenericArguments();
			}
			IlInstruction ilInstruction = null;
			while (stream.BaseStream.Position < stream.BaseStream.Length)
			{
				ilInstruction = new IlInstruction();
				bool isDoubleByte = false;
				OpCode code = ilInstruction.OpCode = ReadOpCode(ref isDoubleByte);
				ilInstruction.Offset = stream.BaseStream.Position - 1;
				if (isDoubleByte)
				{
					ilInstruction.Offset--;
				}
				ilInstruction.Operand = ReadOperand(code, method.Module, ref ilInstruction.LocalVariableIndex);
				ilInstructions.Add(ilInstruction);
			}
			return ilInstructions;
		}

		private object ReadOperand(OpCode code, Module module, ref long localVariableIndex)
		{
			object result = null;
			switch (code.OperandType)
			{
			case OperandType.InlineSwitch:
			{
				int num3 = stream.ReadInt32();
				int[] array = new int[num3];
				int[] array2 = new int[num3];
				for (int i = 0; i < num3; i++)
				{
					array2[i] = stream.ReadInt32();
				}
				for (int j = 0; j < num3; j++)
				{
					array[j] = (int)stream.BaseStream.Position + array2[j];
				}
				result = array;
				break;
			}
			case OperandType.ShortInlineBrTarget:
				result = ((code.FlowControl == FlowControl.Branch || code.FlowControl == FlowControl.Cond_Branch) ? ((object)(stream.ReadSByte() + stream.BaseStream.Position)) : ((object)stream.ReadSByte()));
				break;
			case OperandType.InlineBrTarget:
				result = stream.ReadInt32() + stream.BaseStream.Position;
				break;
			case OperandType.ShortInlineI:
				result = ((!(code == OpCodes.Ldc_I4_S)) ? ((object)stream.ReadByte()) : ((object)(sbyte)stream.ReadByte()));
				break;
			case OperandType.InlineI:
				result = stream.ReadInt32();
				break;
			case OperandType.ShortInlineR:
				result = stream.ReadSingle();
				break;
			case OperandType.InlineR:
				result = stream.ReadDouble();
				break;
			case OperandType.InlineI8:
				result = stream.ReadInt64();
				break;
			case OperandType.InlineSig:
				result = module.ResolveSignature(stream.ReadInt32());
				break;
			case OperandType.InlineString:
				result = module.ResolveString(stream.ReadInt32());
				break;
			case OperandType.InlineField:
			case OperandType.InlineMethod:
			case OperandType.InlineTok:
			case OperandType.InlineType:
				result = module.ResolveMember(stream.ReadInt32(), typeArgs, methodArgs);
				break;
			case OperandType.ShortInlineVar:
			{
				int num2 = stream.ReadByte();
				result = GetVariable(code, num2);
				localVariableIndex = num2;
				break;
			}
			case OperandType.InlineVar:
			{
				int num = stream.ReadUInt16();
				result = GetVariable(code, num);
				localVariableIndex = num;
				break;
			}
			default:
				throw new NotSupportedException();
			case OperandType.InlineNone:
				break;
			}
			return result;
		}

		private OpCode ReadOpCode(ref bool isDoubleByte)
		{
			isDoubleByte = false;
			byte b = stream.ReadByte();
			if (b != 254)
			{
				return singleByteOpCode[b];
			}
			isDoubleByte = true;
			return doubleByteOpCode[stream.ReadByte()];
		}

		private object GetVariable(OpCode code, int index)
		{
			if (code.Name.Contains("loc"))
			{
				return locals[index];
			}
			if (!currentMethod.IsStatic)
			{
				index--;
			}
			return parameters[index];
		}

		private FieldInfo[] GetOpCodeFields()
		{
			return typeof(OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
		}
	}
}
