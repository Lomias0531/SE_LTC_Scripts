using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMySession
	{
		float AssemblerEfficiencyMultiplier
		{
			get;
		}

		float AssemblerSpeedMultiplier
		{
			get;
		}

		bool AutoHealing
		{
			get;
		}

		uint AutoSaveInMinutes
		{
			get;
		}

		IMyCameraController CameraController
		{
			get;
		}

		bool CargoShipsEnabled
		{
			get;
		}

		[Obsolete("Client saving not supported anymore")]
		bool ClientCanSave
		{
			get;
		}

		bool CreativeMode
		{
			get;
		}

		string CurrentPath
		{
			get;
		}

		string Description
		{
			get;
			set;
		}

		IMyCamera Camera
		{
			get;
		}

		double CameraTargetDistance
		{
			get;
			set;
		}

		IMyPlayer LocalHumanPlayer
		{
			get;
		}

		/// <summary>
		/// Obtaining values from config is slow and can allocate memory!
		/// Do it only when necessary.
		/// </summary>
		IMyConfig Config
		{
			get;
		}

		TimeSpan ElapsedPlayTime
		{
			get;
		}

		bool EnableCopyPaste
		{
			get;
		}

		MyEnvironmentHostilityEnum EnvironmentHostility
		{
			get;
		}

		DateTime GameDateTime
		{
			get;
			set;
		}

		float GrinderSpeedMultiplier
		{
			get;
		}

		float HackSpeedMultiplier
		{
			get;
		}

		float InventoryMultiplier
		{
			get;
		}

		float CharactersInventoryMultiplier
		{
			get;
		}

		float BlocksInventorySizeMultiplier
		{
			get;
		}

		bool IsCameraAwaitingEntity
		{
			get;
			set;
		}

		List<MyObjectBuilder_Checkpoint.ModItem> Mods
		{
			get;
			set;
		}

		/// <summary>
		/// Gets if the current camera is the current controlled object (not spectator)
		/// </summary>
		/// <returns></returns>
		bool IsCameraControlledObject
		{
			get;
		}

		/// <summary>
		/// Gets if the current camera is the user controlled spectator
		/// </summary>
		/// <returns></returns>
		bool IsCameraUserControlledSpectator
		{
			get;
		}

		bool IsServer
		{
			get;
		}

		short MaxFloatingObjects
		{
			get;
		}

		short MaxBackupSaves
		{
			get;
		}

		short MaxPlayers
		{
			get;
		}

		bool MultiplayerAlive
		{
			get;
			set;
		}

		bool MultiplayerDirect
		{
			get;
			set;
		}

		double MultiplayerLastMsg
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		float NegativeIntegrityTotal
		{
			get;
			set;
		}

		MyOnlineModeEnum OnlineMode
		{
			get;
		}

		string Password
		{
			get;
			set;
		}

		float PositiveIntegrityTotal
		{
			get;
			set;
		}

		float RefinerySpeedMultiplier
		{
			get;
		}

		bool ShowPlayerNamesOnHud
		{
			get;
		}

		bool SurvivalMode
		{
			get;
		}

		bool ThrusterDamage
		{
			get;
		}

		string ThumbPath
		{
			get;
		}

		TimeSpan TimeOnBigShip
		{
			get;
		}

		TimeSpan TimeOnFoot
		{
			get;
		}

		TimeSpan TimeOnJetpack
		{
			get;
		}

		TimeSpan TimeOnSmallShip
		{
			get;
		}

		bool WeaponsEnabled
		{
			get;
		}

		float WelderSpeedMultiplier
		{
			get;
		}

		ulong? WorkshopId
		{
			get;
		}

		IMyVoxelMaps VoxelMaps
		{
			get;
		}

		IMyPlayer Player
		{
			get;
		}

		IMyControllableEntity ControlledObject
		{
			get;
		}

		MyObjectBuilder_SessionSettings SessionSettings
		{
			get;
		}

		IMyFactionCollection Factions
		{
			get;
		}

		IMyDamageSystem DamageSystem
		{
			get;
		}

		IMyGpsCollection GPS
		{
			get;
		}

		BoundingBoxD WorldBoundaries
		{
			get;
		}

		/// <summary>
		/// Gets the local player's promote level.
		/// </summary>
		MyPromoteLevel PromoteLevel
		{
			get;
		}

		/// <summary>
		/// Checks if the local player is an admin or is promoted to space master (or higher).
		/// </summary>
		bool HasCreativeRights
		{
			get;
		}

		[Obsolete("Use HasCreativeRights")]
		bool HasAdminPrivileges
		{
			get;
		}

		Version Version
		{
			get;
		}

		IMyOxygenProviderSystem OxygenProviderSystem
		{
			get;
		}

		event Action OnSessionReady;

		event Action OnSessionLoading;

		void BeforeStartComponents();

		void Draw();

		void GameOver();

		void GameOver(MyStringId? customMessage);

		MyObjectBuilder_Checkpoint GetCheckpoint(string saveName);

		MyObjectBuilder_Sector GetSector();

		Dictionary<string, byte[]> GetVoxelMapsArray();

		MyObjectBuilder_World GetWorld();

		bool IsPausable();

		void RegisterComponent(MySessionComponentBase component, MyUpdateOrder updateOrder, int priority);

		bool Save(string customSaveName = null);

		void SetCameraController(MyCameraControllerEnum cameraControllerEnum, IMyEntity cameraEntity = null, Vector3D? position = null);

		void SetAsNotReady();

		void Unload();

		void UnloadDataComponents();

		void UnloadMultiplayer();

		void UnregisterComponent(MySessionComponentBase component);

		void Update(MyTimeSpan time);

		void UpdateComponents();

		/// <summary>
		/// Gets a remote player's promote level.
		/// </summary>
		/// <param name="steamId"></param>
		/// <returns></returns>
		MyPromoteLevel GetUserPromoteLevel(ulong steamId);

		/// <summary>
		/// Checks if a given player is an admin (or higher).
		/// </summary>
		/// <param name="steamId"></param>
		/// <returns></returns>
		bool IsUserAdmin(ulong steamId);

		[Obsolete("Use GetUserPromoteLevel")]
		bool IsUserPromoted(ulong steamId);

		/// <summary>
		/// Change the update order of a session component.
		///
		/// There is a proxy for this method in the session component itself.
		/// </summary>
		/// <param name="component">The component to set the update order for</param>
		/// <param name="order">The update order</param>
		void SetComponentUpdateOrder(MySessionComponentBase component, MyUpdateOrder order);
	}
}
