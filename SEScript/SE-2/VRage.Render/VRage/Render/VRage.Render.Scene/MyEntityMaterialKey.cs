using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Render.Scene
{
	public struct MyEntityMaterialKey
	{
		public class MyEntityMaterialKeyComparerType : IEqualityComparer<MyEntityMaterialKey>
		{
			public bool Equals(MyEntityMaterialKey left, MyEntityMaterialKey right)
			{
				return left.Material == right.Material;
			}

			public int GetHashCode(MyEntityMaterialKey materialKey)
			{
				return materialKey.Material.GetHashCode();
			}
		}

		public MyStringId Material;

		public static MyEntityMaterialKeyComparerType Comparer = new MyEntityMaterialKeyComparerType();

		public MyEntityMaterialKey(string key)
		{
			Material = MyStringId.GetOrCompute(key);
		}
	}
}
