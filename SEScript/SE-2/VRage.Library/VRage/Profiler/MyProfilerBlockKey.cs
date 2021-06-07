using System;

namespace VRage.Profiler
{
	public struct MyProfilerBlockKey : IEquatable<MyProfilerBlockKey>
	{
		public string File;

		public string Member;

		public string Name;

		public int Line;

		public int ParentId;

		public int HashCode;

		public MyProfilerBlockKey(string file, string member, string name, int line, int parentId)
		{
			File = file;
			Member = member;
			Name = name;
			Line = line;
			ParentId = parentId;
			HashCode = file.GetHashCode();
			HashCode = ((397 * HashCode) ^ member.GetHashCode());
			HashCode = ((397 * HashCode) ^ (name ?? string.Empty).GetHashCode());
			HashCode = ((397 * HashCode) ^ parentId.GetHashCode());
		}

		public bool IsSameLocation(MyProfilerBlockKey obj)
		{
			if (Name == obj.Name)
			{
				return Member == obj.Member;
			}
			return false;
		}

		public bool IsSimilarLocation(MyProfilerBlockKey obj)
		{
			int num = obj.File.IndexOf("Sources");
			int num2 = File.IndexOf("Sources");
			if (num == -1)
			{
				num = 0;
			}
			if (num2 == -1)
			{
				num2 = 0;
			}
			if (IsSameLocation(obj) && File.Substring(num2) == obj.File.Substring(num))
			{
				return Math.Abs(Line - obj.Line) < 40;
			}
			return false;
		}

		public bool Equals(MyProfilerBlockKey obj)
		{
			if (ParentId == obj.ParentId && File == obj.File && Line == obj.Line)
			{
				return IsSameLocation(obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode;
		}
	}
}
