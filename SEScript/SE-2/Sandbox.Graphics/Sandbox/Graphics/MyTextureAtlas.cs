using System.Collections.Generic;

namespace Sandbox.Graphics
{
	public class MyTextureAtlas : Dictionary<string, MyTextureAtlasItem>
	{
		public MyTextureAtlas(int numItems)
			: base(numItems)
		{
		}
	}
}
