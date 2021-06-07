using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Import
{
	public class Bone
	{
		public string Name
		{
			get;
			set;
		}

		public Matrix LocalTransform
		{
			get;
			set;
		}

		public Bone Parent
		{
			get;
			set;
		}

		public List<Bone> Children
		{
			get;
			private set;
		}

		public Bone()
		{
			Children = new List<Bone>();
		}

		public override string ToString()
		{
			return Name + ": " + base.ToString();
		}
	}
}
