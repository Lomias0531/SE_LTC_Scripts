using System;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyCharacter : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyControllableEntity, IMyCameraController, IMyDestroyableObject, IMyDecalProxy
	{
		/// <summary>
		/// Gets or, for non-player controlled characters, sets the aimed point direction.
		/// </summary>
		/// <remarks>For characters, which are not controlled by player, this will set the aimed point, otherwise the aimed point is determined from camera matrix</remarks>
		Vector3D AimedPoint
		{
			get;
			set;
		}

		/// <summary>
		/// The character definition. Cast to MyCharacterDefinition.
		/// </summary>
		/// <remarks>Until refactoring is complete, casting this to MyCharacterDefinition is needed.</remarks>
		MyDefinitionBase Definition
		{
			get;
		}

		/// <summary>
		/// Gets the amount of oxygen in the surrounding environment
		/// </summary>
		float EnvironmentOxygenLevel
		{
			get;
		}

		/// <summary>
		/// Gets the amount of oxygen at the character location from air pressure system (grids with airtightness)
		/// </summary>
		float OxygenLevel
		{
			get;
		}

		/// <summary>
		/// Gets the base mass of the character
		/// </summary>
		float BaseMass
		{
			get;
		}

		/// <summary>
		/// Gets the entire mass of the character, including inventory
		/// </summary>
		float CurrentMass
		{
			get;
		}

		/// <summary>
		/// Returns the amount of energy the suit has, values will range between 0 and 1, where 0 is no charge and 1 is full charge.
		/// </summary>
		float SuitEnergyLevel
		{
			get;
		}

		/// <summary>
		/// Returns true if this character is dead
		/// </summary>
		bool IsDead
		{
			get;
		}

		/// <summary>
		/// Returns true if this character is a player character, otherwise false.
		/// </summary>
		bool IsPlayer
		{
			get;
		}

		/// <summary>
		/// Returns true if this character is an AI character, otherwise false.
		/// </summary>
		bool IsBot
		{
			get;
		}

		/// <summary>
		/// Gets the character's current movement state.
		/// </summary>
		MyCharacterMovementEnum CurrentMovementState
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the character's previous movement state.
		/// </summary>
		MyCharacterMovementEnum PreviousMovementState
		{
			get;
		}

		VRage.ModAPI.IMyEntity EquippedTool
		{
			get;
		}

		/// <summary>
		/// Event triggered when character dies
		/// </summary>
		event Action<IMyCharacter> CharacterDied;

		[Obsolete("OnMovementStateChanged is deprecated, use MovementStateChanged")]
		event CharacterMovementStateDelegate OnMovementStateChanged;

		/// <summary>
		/// Called when the movement state changes
		/// </summary>
		event CharacterMovementStateChangedDelegate MovementStateChanged;

		/// <summary>
		/// Returns the amount of gas left in the suit, values will range between 0 and 1, where 0 is no gas and 1 is full gas.
		/// </summary>
		/// <param name="gasDefinitionId">Definition Id of the gas. Common example: new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen")</param>
		/// <returns></returns>
		float GetSuitGasFillLevel(MyDefinitionId gasDefinitionId);

		/// <summary>
		/// Kills the character
		/// </summary>
		/// <param name="killData"></param>
		void Kill(object killData = null);

		/// <summary>
		/// Trigger animation event in the new animation system.
		/// If there is a transition leading from current animation state having same name as this event, 
		/// animation state machine will change state accordingly.
		/// If not, nothing happens.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="sync">Synchronize over network</param>
		void TriggerCharacterAnimationEvent(string eventName, bool sync);

		/// <summary>
		/// Returns outside temperature around character. If character is in presurrized/oxygen environment,
		/// then the temperature is always friendly. 
		/// </summary>
		/// <returns>0 for extreme freeze, 0.5 for cozy, 1.0 for extreme hot</returns>
		float GetOutsideTemperature();
	}
}
