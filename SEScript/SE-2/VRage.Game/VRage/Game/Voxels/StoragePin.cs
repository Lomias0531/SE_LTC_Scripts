using System;

namespace VRage.Game.Voxels
{
	public struct StoragePin : IDisposable
	{
		private readonly IMyStorage m_storage;

		public bool Valid => m_storage != null;

		public StoragePin(IMyStorage storage)
		{
			m_storage = storage;
		}

		public void Dispose()
		{
			if (m_storage != null)
			{
				m_storage.Unpin();
			}
		}
	}
}
