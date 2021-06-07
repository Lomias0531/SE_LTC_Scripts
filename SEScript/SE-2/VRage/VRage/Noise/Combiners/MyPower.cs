using System;

namespace VRage.Noise.Combiners
{
	public class MyPower : IMyModule
	{
		private double powerOffset;

		public IMyModule Base
		{
			get;
			set;
		}

		public IMyModule Power
		{
			get;
			set;
		}

		public MyPower(IMyModule baseModule, IMyModule powerModule, double powerOffset = 0.0)
		{
			Base = baseModule;
			Power = powerModule;
			this.powerOffset = powerOffset;
		}

		public double GetValue(double x)
		{
			return Math.Pow(Base.GetValue(x), powerOffset + Power.GetValue(x));
		}

		public double GetValue(double x, double y)
		{
			return Math.Pow(Base.GetValue(x, y), powerOffset + Power.GetValue(x, y));
		}

		public double GetValue(double x, double y, double z)
		{
			return Math.Pow(Base.GetValue(x, y, z), powerOffset + Power.GetValue(x, y, z));
		}
	}
}
