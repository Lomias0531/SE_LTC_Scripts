using System;

namespace VRage.Compiler
{
	public class ScriptOutOfRangeException : Exception
	{
		public ScriptOutOfRangeException()
		{
		}

		public ScriptOutOfRangeException(string message)
			: base(message)
		{
		}
	}
}
