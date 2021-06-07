using System.Collections.Generic;
using VRageMath;

namespace VRage.Noise.Modifiers
{
	/// <summary>
	/// Maps the output value from a source module onto an arbitrary function curve.
	/// </summary>
	public class MyCurve : IMyModule
	{
		public List<MyCurveControlPoint> ControlPoints;

		public IMyModule Module
		{
			get;
			set;
		}

		public MyCurve(IMyModule module)
		{
			Module = module;
			ControlPoints = new List<MyCurveControlPoint>(4);
		}

		public double GetValue(double x)
		{
			double value = Module.GetValue(x);
			int num = ControlPoints.Count - 1;
			int i;
			for (i = 0; i <= num && !(value < ControlPoints[i].Input); i++)
			{
			}
			int index = MathHelper.Clamp(i - 2, 0, num);
			int num2 = MathHelper.Clamp(i - 1, 0, num);
			int num3 = MathHelper.Clamp(i, 0, num);
			int index2 = MathHelper.Clamp(i + 1, 0, num);
			if (num2 == num3)
			{
				return ControlPoints[num2].Output;
			}
			double t = (value - ControlPoints[num2].Input) / (ControlPoints[num3].Input - ControlPoints[num2].Input);
			return MathHelper.CubicInterp(ControlPoints[index].Output, ControlPoints[num2].Output, ControlPoints[num3].Output, ControlPoints[index2].Output, t);
		}

		public double GetValue(double x, double y)
		{
			double value = Module.GetValue(x, y);
			int num = ControlPoints.Count - 1;
			int i;
			for (i = 0; i <= num && !(value < ControlPoints[i].Input); i++)
			{
			}
			int index = MathHelper.Clamp(i - 2, 0, num);
			int num2 = MathHelper.Clamp(i - 1, 0, num);
			int num3 = MathHelper.Clamp(i, 0, num);
			int index2 = MathHelper.Clamp(i + 1, 0, num);
			if (num2 == num3)
			{
				return ControlPoints[num2].Output;
			}
			double t = (value - ControlPoints[num2].Input) / (ControlPoints[num3].Input - ControlPoints[num2].Input);
			return MathHelper.CubicInterp(ControlPoints[index].Output, ControlPoints[num2].Output, ControlPoints[num3].Output, ControlPoints[index2].Output, t);
		}

		public double GetValue(double x, double y, double z)
		{
			double value = Module.GetValue(x, y, z);
			int num = ControlPoints.Count - 1;
			int i;
			for (i = 0; i <= num && !(value < ControlPoints[i].Input); i++)
			{
			}
			int index = MathHelper.Clamp(i - 2, 0, num);
			int num2 = MathHelper.Clamp(i - 1, 0, num);
			int num3 = MathHelper.Clamp(i, 0, num);
			int index2 = MathHelper.Clamp(i + 1, 0, num);
			if (num2 == num3)
			{
				return ControlPoints[num2].Output;
			}
			double t = (value - ControlPoints[num2].Input) / (ControlPoints[num3].Input - ControlPoints[num2].Input);
			return MathHelper.CubicInterp(ControlPoints[index].Output, ControlPoints[num2].Output, ControlPoints[num3].Output, ControlPoints[index2].Output, t);
		}
	}
}
