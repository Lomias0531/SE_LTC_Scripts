using System.Collections.Generic;
using VRageMath;

namespace VRage.Noise.Modifiers
{
	internal class MyTerrace : IMyModule
	{
		public List<double> ControlPoints;

		public IMyModule Module
		{
			get;
			set;
		}

		public bool Invert
		{
			get;
			set;
		}

		private double Terrace(double value, int countMask)
		{
			int i;
			for (i = 0; i <= countMask && !(value < ControlPoints[i]); i++)
			{
			}
			int num = MathHelper.Clamp(i - 1, 0, countMask);
			int num2 = MathHelper.Clamp(i, 0, countMask);
			if (num == num2)
			{
				return ControlPoints[num2];
			}
			double num3 = ControlPoints[num];
			double num4 = ControlPoints[num2];
			double num5 = (value - num3) / (num4 - num3);
			if (Invert)
			{
				num5 = 1.0 - num5;
				double num6 = num3;
				num3 = num4;
				num4 = num6;
			}
			num5 *= num5;
			return MathHelper.Lerp(num3, num4, num5);
		}

		public MyTerrace(IMyModule module, bool invert = false)
		{
			Module = module;
			Invert = invert;
			ControlPoints = new List<double>(2);
		}

		public double GetValue(double x)
		{
			return Terrace(Module.GetValue(x), ControlPoints.Count - 1);
		}

		public double GetValue(double x, double y)
		{
			return Terrace(Module.GetValue(x, y), ControlPoints.Count - 1);
		}

		public double GetValue(double x, double y, double z)
		{
			return Terrace(Module.GetValue(x, y, z), ControlPoints.Count - 1);
		}
	}
}
