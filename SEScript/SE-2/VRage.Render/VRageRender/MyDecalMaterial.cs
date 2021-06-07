using VRage.Utils;

namespace VRageRender
{
	public class MyDecalMaterial
	{
		public bool Transparent;

		public string StringId
		{
			get;
			private set;
		}

		public MyDecalMaterialDesc Material
		{
			get;
			private set;
		}

		public MyStringHash Target
		{
			get;
			private set;
		}

		public MyStringHash Source
		{
			get;
			private set;
		}

		public float MinSize
		{
			get;
			private set;
		}

		public float MaxSize
		{
			get;
			private set;
		}

		public float Depth
		{
			get;
			private set;
		}

		/// <summary>
		/// Positive infinity for random rotation
		/// </summary>
		public float Rotation
		{
			get;
			private set;
		}

		public MyDecalMaterial(MyDecalMaterialDesc materialDef, bool transparent, MyStringHash target, MyStringHash source, float minSize, float maxSize, float depth, float rotation)
		{
			StringId = MyDecalMaterials.GetStringId(source, target);
			Material = materialDef;
			Target = target;
			Source = source;
			MinSize = minSize;
			MaxSize = maxSize;
			Depth = depth;
			Rotation = rotation;
			Transparent = transparent;
		}
	}
}
