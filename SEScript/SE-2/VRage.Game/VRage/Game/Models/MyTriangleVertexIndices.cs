namespace VRage.Game.Models
{
	/// <summary>
	/// structure used to set up the mesh
	/// </summary>
	public struct MyTriangleVertexIndices
	{
		public int I0;

		public int I1;

		public int I2;

		public MyTriangleVertexIndices(int i0, int i1, int i2)
		{
			I0 = i0;
			I1 = i1;
			I2 = i2;
		}

		public void Set(int i0, int i1, int i2)
		{
			I0 = i0;
			I1 = i1;
			I2 = i2;
		}
	}
}
