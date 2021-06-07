using System;

namespace VRage.Noise.Patterns
{
	/// <summary>
	/// Noise that outputs dounut-like ring
	/// </summary>
	public class MyRing : IMyModule
	{
		private double m_thickness;

		private double m_thicknessSqr;

		public double Radius
		{
			get;
			set;
		}

		public double Thickness
		{
			get
			{
				return m_thickness;
			}
			set
			{
				m_thickness = value;
				m_thicknessSqr = value * value;
			}
		}

		public MyRing(double radius, double thickness)
		{
			Radius = radius;
			Thickness = thickness;
		}

		public double GetValue(double x)
		{
			double num = Math.Sqrt(x * x) - Radius;
			return clampToRing(num * num);
		}

		public double GetValue(double x, double y)
		{
			double num = Math.Sqrt(x * x + y * y) - Radius;
			return clampToRing(num * num);
		}

		public double GetValue(double x, double y, double z)
		{
			if (Math.Abs(z) < Thickness)
			{
				double num = Math.Sqrt(x * x + y * y);
				if (Math.Abs(num - Radius) < Thickness)
				{
					double num2 = x / num * Radius - x;
					double num3 = y / num * Radius - y;
					double squareDstFromRing = num2 * num2 + num3 * num3 + z * z;
					return clampToRing(squareDstFromRing);
				}
				return 0.0;
			}
			return 0.0;
		}

		private double clampToRing(double squareDstFromRing)
		{
			if (squareDstFromRing < m_thicknessSqr)
			{
				return 1.0 - squareDstFromRing / m_thicknessSqr;
			}
			return 0.0;
		}
	}
}
