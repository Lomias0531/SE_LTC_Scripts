using System;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
	public class MySyncedBlock : MyCubeBlock, IMyEventProxy, IMyEventOwner, IMySyncedEntity
	{
		private class Sandbox_Game_Entities_Cube_MySyncedBlock_003C_003EActor : IActivator, IActivator<MySyncedBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MySyncedBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySyncedBlock CreateInstance()
			{
				return new MySyncedBlock();
			}

			MySyncedBlock IActivator<MySyncedBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SyncType SyncType
		{
			get;
			set;
		}

		public event Action<SyncBase> SyncPropertyChanged
		{
			add
			{
				SyncType.PropertyChanged += value;
			}
			remove
			{
				SyncType.PropertyChanged -= value;
			}
		}

		public MySyncedBlock()
		{
			SyncType = SyncHelpers.Compose(this);
		}
	}
}
