using System.Collections.Generic;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage.Generics
{
	/// <summary>
	/// Definition of transition to some node.
	/// </summary>
	public class MyStateMachineTransition
	{
		public MyStringId Name = MyStringId.NullOrEmpty;

		public MyStateMachineNode TargetNode;

		public List<IMyCondition> Conditions = new List<IMyCondition>();

		public int? Priority;

		public int Id
		{
			get;
			private set;
		}

		public virtual bool Evaluate()
		{
			for (int i = 0; i < Conditions.Count; i++)
			{
				if (!Conditions[i].Evaluate())
				{
					return false;
				}
			}
			return true;
		}

		public void _SetId(int newId)
		{
			Id = newId;
		}

		public override string ToString()
		{
			if (TargetNode != null)
			{
				return "transition -> " + TargetNode.Name;
			}
			return "transition -> (null)";
		}
	}
}
