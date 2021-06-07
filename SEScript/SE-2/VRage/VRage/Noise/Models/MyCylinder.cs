using System;

namespace VRage.Noise.Models
{
	/// <summary>
	/// Maps the output of a module onto a cylinder.
	/// </summary>
	internal class MyCylinder : IMyModule
	{
		public IMyModule Module
		{
			get;
			set;
		}

		public MyCylinder(IMyModule module)
		{
			Module = module;
		}

		public double GetValue(double x)
		{
			throw new NotImplementedException();
		}

		public double GetValue(double angle, double height)
		{
			double x = Math.Cos(angle);
			double z = Math.Sin(angle);
			return Module.GetValue(x, height, z);
		}

		public double GetValue(double x, double y, double z)
		{
			throw new NotImplementedException();
		}
	}
}
