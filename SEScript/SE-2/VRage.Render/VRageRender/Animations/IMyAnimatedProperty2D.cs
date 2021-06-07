namespace VRageRender.Animations
{
	public interface IMyAnimatedProperty2D : IMyAnimatedProperty, IMyConstProperty
	{
		IMyAnimatedProperty CreateEmptyKeys();

		void GetInterpolatedKeys(float overallTime, float multiplier, IMyAnimatedProperty interpolatedKeys);
	}
	public interface IMyAnimatedProperty2D<T, V, W> : IMyAnimatedProperty2D, IMyAnimatedProperty, IMyConstProperty
	{
		X GetInterpolatedValue<X>(float overallTime, float time) where X : V;

		void GetInterpolatedKeys(float overallTime, W variance, float multiplier, IMyAnimatedProperty interpolatedKeys);
	}
}
