using System;
using System.Runtime.CompilerServices;

namespace VRage.Compiler
{
	public class IlInjector
	{
		public interface ICounterHandle : IDisposable
		{
			int InstructionCount
			{
				get;
			}

			int MaxInstructionCount
			{
				get;
			}

			int MethodCallCount
			{
				get;
			}

			int MaxMethodCallCount
			{
				get;
			}

			int Depth
			{
				get;
			}
		}

		private class InstructionCounterHandle : ICounterHandle, IDisposable
		{
			private int m_runDepth;

			public int Depth => m_runDepth;

			public int InstructionCount => m_numInstructions;

			public int MaxInstructionCount => m_numMaxInstructions;

			public int MethodCallCount => m_callChainDepth;

			public int MaxMethodCallCount => m_maxCallChainDepth;

			public void AddRef(int maxInstructions, int maxMethodCount)
			{
				m_runDepth++;
				if (m_runDepth == 1)
				{
					RestartCountingInstructions(maxInstructions);
					RestartCountingMethods(maxMethodCount);
					ResetIsDead();
				}
			}

			public void Dispose()
			{
				if (m_runDepth > 0)
				{
					m_runDepth--;
				}
			}
		}

		private static InstructionCounterHandle m_instructionCounterHandle = new InstructionCounterHandle();

		private static bool m_isDead;

		private static int m_numInstructions = 0;

		private static int m_numMaxInstructions = 0;

		private static ulong m_minAllowedStackPointer = 0uL;

		private const int ALLOWED_STACK_CONSUMPTION = 1048576;

		private static int m_numMethodCalls = 0;

		private static int m_maxMethodCalls = 0;

		private static int m_maxCallChainDepth = 1000;

		private static int m_callChainDepth = 0;

		public static int NumInstructions => m_numInstructions;

		public static int CallChainDepth => m_callChainDepth;

		public static bool IsWithinRunBlock()
		{
			return m_instructionCounterHandle.Depth > 0;
		}

		public static ICounterHandle BeginRunBlock(int maxInstructions, int maxMethodCalls)
		{
			m_instructionCounterHandle.AddRef(maxInstructions, maxMethodCalls);
			return m_instructionCounterHandle;
		}

		private static void RestartCountingInstructions(int maxInstructions)
		{
			m_numInstructions = 0;
			m_numMaxInstructions = maxInstructions;
			m_minAllowedStackPointer = GetStackPointer() - 1048576;
		}

		private static void ResetIsDead()
		{
			m_isDead = false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CountInstructions()
		{
			m_numInstructions++;
			if (m_numInstructions > m_numMaxInstructions)
			{
				m_isDead = true;
				throw new ScriptOutOfRangeException();
			}
		}

		public unsafe static ulong GetStackPointer()
		{
			int num = 0;
			int* ptr = &num;
			return (ulong)ptr;
		}

		private static void RestartCountingMethods(int maxMethodCalls)
		{
			m_numMethodCalls = 0;
			m_maxMethodCalls = maxMethodCalls;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CountMethodCalls()
		{
			m_numMethodCalls++;
			if (m_numMethodCalls > m_maxMethodCalls)
			{
				m_isDead = true;
				throw new ScriptOutOfRangeException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EnterMethod_Profile([CallerMemberName] string member = "")
		{
			EnterMethod();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EnterMethod()
		{
			m_callChainDepth++;
			ulong stackPointer = GetStackPointer();
			if (m_callChainDepth > m_maxCallChainDepth || stackPointer < m_minAllowedStackPointer)
			{
				m_callChainDepth--;
				m_isDead = true;
				throw new ScriptOutOfRangeException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExitMethod_Profile()
		{
			ExitMethod();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExitMethod()
		{
			m_callChainDepth--;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T YieldGuard_Profile<T>(T value)
		{
			return YieldGuard(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T YieldGuard<T>(T value)
		{
			ExitMethod();
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDead()
		{
			return m_isDead;
		}
	}
}
