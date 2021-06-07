using System;
using VRage.Game;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyGasTank
	{
		float GasCapacity
		{
			get;
		}

		double FilledRatio
		{
			get;
		}

		Action FilledRatioChanged
		{
			get;
			set;
		}

		bool IsResourceStorage(MyDefinitionId resourceDefinition);
	}
}
