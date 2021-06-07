using System.Reflection;

namespace VRageRender.Animations
{
	public interface IMyAnimatedProperty : IMyConstProperty
	{
		void GetInterpolatedValue(float time, out object value);

		int AddKey(float time, object val);

		void RemoveKey(float time);

		void RemoveKey(int index);

		void RemoveKeyByID(int id);

		void ClearKeys();

		int GetKeysCount();

		void SetKey(int index, float time);

		void SetKey(int index, float time, object value);

		void GetKey(int index, out float time, out object value);

		void GetKey(int index, out int id, out float time, out object value);

		void SetKeyByID(int id, float time);

		void SetKeyByID(int id, float time, object value);

		void GetKeyByID(int id, out float time, out object value);
	}
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true, ApplyToMembers = true)]
	public interface IMyAnimatedProperty<T> : IMyAnimatedProperty, IMyConstProperty
	{
		[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
		void GetInterpolatedValue<U>(float time, out U value) where U : T;

		[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
		int AddKey<U>(float time, U val) where U : T;
	}
}
