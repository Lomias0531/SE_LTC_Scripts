using System;
using System.Text;

namespace VRage.Utils
{
	public class MyVersion
	{
		public readonly int Version;

		public readonly StringBuilder FormattedText;

		public readonly StringBuilder FormattedTextFriendly;

		private readonly Version _version;

		public MyVersion(int version)
		{
			Version = version;
			FormattedText = new StringBuilder(MyBuildNumbers.ConvertBuildNumberFromIntToString(version));
			string text = MyBuildNumbers.ConvertBuildNumberFromIntToStringFriendly(version, ".");
			FormattedTextFriendly = new StringBuilder(text);
			_version = new Version(text);
		}

		public static implicit operator MyVersion(int version)
		{
			return new MyVersion(version);
		}

		public static implicit operator int(MyVersion version)
		{
			return version.Version;
		}

		public static implicit operator Version(MyVersion version)
		{
			return version._version;
		}

		public override string ToString()
		{
			return Version.ToString();
		}
	}
}
