namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	///     Represents the results of a configuration parsing.
	/// </summary>
	public struct MyIniParseResult
	{
		private int m_lineNo;

		private readonly TextPtr m_ptr;

		/// <summary>
		///     Gets a description of the error that occurred during parsing. Will be <c>null</c> if no error occurred.
		/// </summary>
		public readonly string Error;

		/// <summary>
		///     Gets the line number where an error occured.
		/// </summary>
		public int LineNo
		{
			get
			{
				if (m_lineNo == 0)
				{
					m_lineNo = m_ptr.FindLineNo();
				}
				return m_lineNo;
			}
		}

		/// <summary>
		///     Determines the success of the configuration parsing.
		/// </summary>
		public bool Success
		{
			get
			{
				if (IsDefined)
				{
					return Error == null;
				}
				return false;
			}
		}

		/// <summary>
		/// Determines if the value of this result is defined, meaning whether the <see cref="P:VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult.Success" /> actually holds any meaning.
		/// </summary>
		public bool IsDefined => m_ptr.IsEmpty;

		/// <summary>
		///     Compares the two <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult" />s.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(MyIniParseResult a, MyIniParseResult b)
		{
			return a.Equals(b);
		}

		/// <summary>
		///     Compares the two <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult" />s.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(MyIniParseResult a, MyIniParseResult b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		///     Compares this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult" /> with another.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(MyIniParseResult other)
		{
			if (LineNo == other.LineNo)
			{
				return string.Equals(Error, other.Error);
			}
			return false;
		}

		/// <summary>
		///     Compares this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult" /> with another.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is MyIniParseResult)
			{
				return Equals((MyIniParseResult)obj);
			}
			return false;
		}

		/// <summary>
		///     Gets the hash code for this <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult" />.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (LineNo * 397) ^ ((Error != null) ? Error.GetHashCode() : 0);
		}

		internal MyIniParseResult(TextPtr ptr, string error)
		{
			m_lineNo = 0;
			m_ptr = ptr;
			Error = error;
		}

		/// <summary>
		///     Generates a generic error message in the form of <c>Line N: Error</c>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (!Success)
			{
				return $"Line {(int)LineNo}: {Error}";
			}
			return "Success";
		}
	}
}
