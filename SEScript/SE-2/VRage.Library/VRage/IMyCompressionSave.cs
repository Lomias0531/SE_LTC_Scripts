using System;

namespace VRage
{
	public interface IMyCompressionSave : IDisposable
	{
		void Add(byte[] value);

		void Add(byte[] value, int count);

		void Add(float value);

		void Add(int value);

		void Add(byte value);
	}
}
