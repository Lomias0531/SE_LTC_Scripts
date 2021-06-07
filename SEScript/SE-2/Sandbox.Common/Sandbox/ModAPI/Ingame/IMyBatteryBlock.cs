using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyBatteryBlock : IMyPowerProducer, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		bool HasCapacityRemaining
		{
			get;
		}

		float CurrentStoredPower
		{
			get;
		}

		float MaxStoredPower
		{
			get;
		}

		float CurrentInput
		{
			get;
		}

		float MaxInput
		{
			get;
		}

		bool IsCharging
		{
			get;
		}

		ChargeMode ChargeMode
		{
			get;
			set;
		}

		[Obsolete("Use ChargeMode instead")]
		bool OnlyRecharge
		{
			get;
			set;
		}

		[Obsolete("Use ChargeMode instead")]
		bool OnlyDischarge
		{
			get;
			set;
		}

		[Obsolete("Semi-auto is no longer a valid mode, if you want to check for Auto instead, use ChargeMode")]
		bool SemiautoEnabled
		{
			get;
			set;
		}
	}
}
