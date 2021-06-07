using System;
using VRage.Game.ModAPI.Interfaces;

namespace VRage.Game.ModAPI
{
	public interface IMyEntityController
	{
		IMyControllableEntity ControlledEntity
		{
			get;
		}

		event Action<IMyControllableEntity, IMyControllableEntity> ControlledEntityChanged;

		void TakeControl(IMyControllableEntity entity);
	}
}
