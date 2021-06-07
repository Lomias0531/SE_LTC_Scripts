using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public class MyRuntimeEnvironmentItemInfo
	{
		public MyItemTypeDefinition Type;

		public MyStringHash Subtype;

		public float Offset;

		public float Density;

		public short Index;

		public MyRuntimeEnvironmentItemInfo(MyProceduralEnvironmentDefinition def, MyEnvironmentItemInfo info, int id)
		{
			Index = (short)id;
			Type = def.ItemTypes[info.Type];
			Subtype = info.Subtype;
			Offset = info.Offset;
			Density = info.Density;
		}
	}
}
