using System.Collections.Generic;

namespace Sandbox.Graphics.GUI
{
	public class ShadowTextureSet
	{
		public List<ShadowTexture> Textures
		{
			get;
			private set;
		}

		public ShadowTextureSet()
		{
			Textures = new List<ShadowTexture>();
		}

		public void AddTextures(IEnumerable<ShadowTexture> textures)
		{
			Textures.AddRange(textures);
			Textures.Sort(delegate(ShadowTexture t1, ShadowTexture t2)
			{
				if (t1.MinWidth == t2.MinWidth)
				{
					return 0;
				}
				return (!(t1.MinWidth < t2.MinWidth)) ? 1 : (-1);
			});
		}

		public ShadowTexture GetOptimalTexture(float size)
		{
			int num = 0;
			int num2 = Textures.Count - 1;
			for (int num3 = num2 - num; num3 >= 0; num3 = num2 - num)
			{
				int num4 = num3 / 2 + num;
				ShadowTexture shadowTexture = Textures[num4];
				float minWidth = shadowTexture.MinWidth;
				if (size == minWidth || (num3 == 0 && size > minWidth))
				{
					return shadowTexture;
				}
				if (minWidth > size)
				{
					num2 = num4 - 1;
				}
				else
				{
					num = num4 + 1;
				}
			}
			if (num2 <= 0)
			{
				return Textures[num];
			}
			return Textures[num2];
		}
	}
}
