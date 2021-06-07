using Sandbox.Definitions;
using System.Collections.Generic;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public struct MyBiomeMaterial
	{
		private class MyComparer : IEqualityComparer<MyBiomeMaterial>
		{
			public unsafe bool Equals(MyBiomeMaterial x, MyBiomeMaterial y)
			{
				ushort* ptr = (ushort*)(&x);
				ushort* ptr2 = (ushort*)(&y);
				return *ptr == *ptr2;
			}

			public unsafe int GetHashCode(MyBiomeMaterial obj)
			{
				return ((ushort*)(&obj))->GetHashCode();
			}
		}

		public readonly byte Biome;

		public readonly byte Material;

		public static IEqualityComparer<MyBiomeMaterial> Comparer = new MyComparer();

		public MyBiomeMaterial(byte biome, byte material)
		{
			Biome = biome;
			Material = material;
		}

		public override int GetHashCode()
		{
			return ((Biome << 8) | Material).GetHashCode();
		}

		public override string ToString()
		{
			return $"Biome[{Biome}]:{MyDefinitionManager.Static.GetVoxelMaterialDefinition(Material).Id.SubtypeName}";
		}
	}
}
