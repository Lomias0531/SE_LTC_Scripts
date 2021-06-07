using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyMotorSuspension : IMyMotorBase, IMyMechanicalConnectionBlock, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		bool Steering
		{
			get;
			set;
		}

		bool Propulsion
		{
			get;
			set;
		}

		bool InvertSteer
		{
			get;
			set;
		}

		bool InvertPropulsion
		{
			get;
			set;
		}

		[Obsolete]
		float Damping
		{
			get;
		}

		float Strength
		{
			get;
			set;
		}

		float Friction
		{
			get;
			set;
		}

		float Power
		{
			get;
			set;
		}

		float Height
		{
			get;
			set;
		}

		/// <summary>
		/// Wheel's current steering angle
		/// </summary>
		float SteerAngle
		{
			get;
		}

		/// <summary>
		/// Max steering angle in radians.
		/// </summary>
		float MaxSteerAngle
		{
			get;
			set;
		}

		/// <summary>
		/// Speed at which wheel steers.
		/// </summary>
		[Obsolete]
		float SteerSpeed
		{
			get;
		}

		/// <summary>
		/// Speed at which wheel returns from steering.
		/// </summary>
		[Obsolete]
		float SteerReturnSpeed
		{
			get;
		}

		/// <summary>
		/// Suspension travel, value from 0 to 1.
		/// </summary>
		[Obsolete]
		float SuspensionTravel
		{
			get;
		}

		/// <summary>
		/// Gets or sets brake applied to the wheel.
		/// </summary>
		bool Brake
		{
			get;
			set;
		}

		/// <summary>
		/// Enables or disalbes AirShock function.
		/// </summary>
		bool AirShockEnabled
		{
			get;
			set;
		}
	}
}
