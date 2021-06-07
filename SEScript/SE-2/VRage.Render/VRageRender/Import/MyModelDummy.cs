using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace VRageRender.Import
{
	public class MyModelDummy : IMyModelDummy
	{
		public const string SUBBLOCK_PREFIX = "subblock_";

		public const string SUBPART_PREFIX = "subpart_";

		public const string ATTRIBUTE_FILE = "file";

		public const string ATTRIBUTE_HIGHLIGHT = "highlight";

		public const string ATTRIBUTE_HIGHLIGHT_TYPE = "highlighttype";

		public const string ATTRIBUTE_HIGHLIGHT_SEPARATOR = ";";

		public string Name;

		public Dictionary<string, object> CustomData;

		public Matrix Matrix;

		string IMyModelDummy.Name => Name;

		Matrix IMyModelDummy.Matrix => Matrix;
	}
}
