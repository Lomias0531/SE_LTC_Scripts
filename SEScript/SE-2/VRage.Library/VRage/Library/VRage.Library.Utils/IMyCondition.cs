namespace VRage.Library.Utils
{
	/// <summary>
	/// Interface of totally generic condition.
	/// </summary>
	public interface IMyCondition
	{
		/// <summary>
		/// Evaluate the condition, it can be true/false.
		/// </summary>
		bool Evaluate();
	}
	public interface IMyCondition<T>
	{
		bool Evaluate(T value);
	}
}
