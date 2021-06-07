using System.Collections.Generic;

namespace Sandbox.Graphics.GUI
{
	public class MyDragAndDropRestriction
	{
		public List<ushort> ObjectBuilders
		{
			get;
			private set;
		}

		public List<ushort> ObjectBuilderTypes
		{
			get;
			private set;
		}

		public MyDragAndDropRestriction()
		{
			ObjectBuilders = new List<ushort>();
			ObjectBuilderTypes = new List<ushort>();
		}
	}
}
