using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyPlayer
	{
		IMyNetworkClient Client
		{
			get;
		}

		/// <summary>
		/// List of grids where the player owns at least one block.
		/// </summary>
		/// <remarks>Not synced.</remarks>
		HashSet<long> Grids
		{
			get;
		}

		/// <summary>
		/// Gets the EntityController for the player.
		/// </summary>
		IMyEntityController Controller
		{
			get;
		}

		/// <summary>
		/// Gets the Steam user id for the player.
		/// </summary>
		ulong SteamUserId
		{
			get;
		}

		/// <summary>
		/// Visible player name
		/// </summary>
		string DisplayName
		{
			get;
		}

		[Obsolete("Use IdentityId instead.")]
		long PlayerID
		{
			get;
		}

		/// <summary>
		/// Unique id for the current player identity.
		/// </summary>
		/// <remarks>This will change when the player dies with permadeath enabled.</remarks>
		long IdentityId
		{
			get;
		}

		/// <summary>
		/// Gets if the player is an admin on the server.
		/// </summary>
		[Obsolete("Use Promote Level instead")]
		bool IsAdmin
		{
			get;
		}

		/// <summary>
		/// Gets if the player is promoted to Space Master.
		/// </summary>
		[Obsolete("Use Promote Level instead")]
		bool IsPromoted
		{
			get;
		}

		/// <summary>
		/// Gets the player's promote level
		/// </summary>
		MyPromoteLevel PromoteLevel
		{
			get;
		}

		/// <summary>
		/// Gets the Character entity for the player.
		/// </summary>
		IMyCharacter Character
		{
			get;
		}

		/// <summary>
		/// Gets if the player is a bot (non-human)
		/// </summary>
		bool IsBot
		{
			get;
		}

		/// <summary>
		/// Gets the identity for the player
		/// </summary>
		IMyIdentity Identity
		{
			get;
		}

		/// <summary>
		/// Gets the entity id for the player's respawn ship(s).
		/// </summary>
		ListReader<long> RespawnShip
		{
			get;
		}

		/// <summary>
		/// Gets or sets all the player's build color slots
		/// </summary>
		/// <remarks>Not synced.</remarks>
		List<Vector3> BuildColorSlots
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of the default build colors.
		/// </summary>
		ListReader<Vector3> DefaultBuildColorSlots
		{
			get;
		}

		/// <summary>
		/// Gets or sets the build color for the selected slot.
		/// </summary>
		/// <remarks>Not synced.</remarks>
		Vector3 SelectedBuildColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the selected slot for the build color.
		/// </summary>
		/// <remarks>Not synced.</remarks>
		int SelectedBuildColorSlot
		{
			get;
			set;
		}

		/// <summary>
		/// Event triggered when the player's identity changed (eg. died w/permadeath on)
		/// </summary>
		event Action<IMyPlayer, IMyIdentity> IdentityChanged;

		/// <summary>
		/// Gets the relationship between this player and another.
		/// </summary>
		/// <param name="playerId">Player to test relationship against</param>
		/// <returns></returns>
		MyRelationsBetweenPlayerAndBlock GetRelationTo(long playerId);

		/// <summary>
		/// Adds a grid to the player's Grids list.
		/// </summary>
		/// <param name="gridEntityId"></param>
		void AddGrid(long gridEntityId);

		/// <summary>
		/// Removes a grid from the player's Grids list.
		/// </summary>
		/// <param name="gridEntityId"></param>
		void RemoveGrid(long gridEntityId);

		/// <summary>
		/// Gets the player position
		/// </summary>
		/// <returns></returns>
		Vector3D GetPosition();

		/// <summary>
		/// Switches to slot containing color, if present. Otherwise sets active slot to color.
		/// </summary>
		/// <param name="color"></param>
		/// <remarks>Not synced.</remarks>
		void ChangeOrSwitchToColor(Vector3 color);

		/// <summary>
		/// Sets build colors back to defaults.
		/// </summary>
		void SetDefaultColors();

		/// <summary>
		/// Spawns the player as a new character (changes the model).
		/// </summary>
		/// <param name="character"></param>
		void SpawnIntoCharacter(IMyCharacter character);

		/// <summary>
		/// Spawns the player at a specific place. Must be called on server.
		/// </summary>
		/// <param name="worldMatrix">Spawn position</param>
		/// <param name="velocity">Velocity to provide to player</param>
		/// <param name="spawnedBy">Entity triggering respawn (can be null)</param>
		/// <param name="findFreePlace">Find a safe place to spawn near the position</param>
		void SpawnAt(MatrixD worldMatrix, Vector3 velocity, IMyEntity spawnedBy, bool findFreePlace = true, string modelName = null, Color? color = null);

		/// <summary>
		/// Spawns the player at a specific place. Must be called on server.
		/// </summary>
		/// <param name="worldMatrix">Spawn position</param>
		/// <param name="velocity">Velocity to provide to player</param>
		/// <param name="spawnedBy">Entity triggering respawn (can be null)</param>
		void SpawnAt(MatrixD worldMatrix, Vector3 velocity, IMyEntity spawnedBy);

		/// <summary>
		/// Gets balance of an account associated with player.
		/// </summary>
		/// <param name="balance">Returns current balance of the account. (If called on client, can return delayed value, as changes to balace have to be synchronized first)</param>
		/// <returns>True if account was found. Otherwise false.</returns>
		bool TryGetBalanceInfo(out long balance);

		/// <summary>
		/// Gets balance of an account associated with player. Format is 'BALANCE CURRENCYSHORTNAME'.
		/// </summary>
		/// <returns>Current balance of the account in form of formatted string. If Banking System does not exist method returns null.</returns>
		string GetBalanceShortString();

		/// <summary>
		/// Changes the balance of the account of this player by given amount. Sends a message to server with the request.
		/// </summary>
		/// <param name="amount">Amount by which to change te balance.</param>
		void RequestChangeBalance(long amount);
	}
}
