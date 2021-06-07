using System.Collections.Generic;
using VRageRender.Import;

namespace VRageRender.Models
{
	public class MyMeshMaterial
	{
		public MyMeshDrawTechnique DrawTechnique;

		public string Name;

		public string GlassCW;

		public string GlassCCW;

		public bool GlassSmooth;

		public Dictionary<string, string> Textures;

		public override int GetHashCode()
		{
			int num = 1;
			int num2 = 0;
			int num3 = 3;
			foreach (KeyValuePair<string, string> texture in Textures)
			{
				num = ((num * 397) ^ texture.Key.GetHashCode());
				num2 += 1 << ++num3;
				num = ((num * 397) ^ texture.Value.GetHashCode());
				num2 += 1 << ++num3;
			}
			return (num * 397) ^ num2;
		}
	}
}
