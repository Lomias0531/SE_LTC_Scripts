namespace VRage.Noise.Modifiers
{
	/// <summary>
	/// Inverts the output value from a source module.
	/// </summary>
	public class MyInvert : IMyModule
	{
		public IMyModule Module
		{
			get;
			set;
		}

		public MyInvert(IMyModule module)
		{
			Module = module;
		}

		public double GetValue(double x)
		{
			return 0.0 - Module.GetValue(x);
		}

		public double GetValue(double x, double y)
		{
			return 0.0 - Module.GetValue(x, y);
		}

		public double GetValue(double x, double y, double z)
		{
			return 0.0 - Module.GetValue(x, y, z);
		}
	}
}
