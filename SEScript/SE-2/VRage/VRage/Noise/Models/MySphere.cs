using System;

namespace VRage.Noise.Models
{
	/// <summary>
	/// Maps the output of a module onto a sphere.
	/// </summary>
	internal class MySphere : IMyModule
	{
		public IMyModule Module
		{
			get;
			set;
		}

		protected void LatLonToXYZ(double lat, double lon, out double x, out double y, out double z)
		{
			double num = Math.Cos(Math.PI / 180.0 * lat);
			x = Math.Cos(Math.PI / 180.0 * lon) * num;
			y = Math.Sin(Math.PI / 180.0 * lat);
			z = Math.Sin(Math.PI / 180.0 * lon) * num;
		}

		public MySphere(IMyModule module)
		{
			Module = module;
		}

		public double GetValue(double x)
		{
			throw new NotImplementedException();
		}

		public double GetValue(double latitude, double longitude)
		{
			LatLonToXYZ(latitude, longitude, out double x, out double y, out double z);
			return Module.GetValue(x, y, z);
		}

		public double GetValue(double x, double y, double z)
		{
			throw new NotImplementedException();
		}
	}
}
