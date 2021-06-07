using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MyPlayer : IMyPlayer
	{
		[Serializable]
		public struct PlayerId : IComparable<PlayerId>
		{
			public class PlayerIdComparerType : IEqualityComparer<PlayerId>
			{
				public bool Equals(PlayerId left, PlayerId right)
				{
					return left == right;
				}

				public int GetHashCode(PlayerId playerId)
				{
					return playerId.GetHashCode();
				}
			}

			protected class Sandbox_Game_World_MyPlayer_003C_003EPlayerId_003C_003ESteamId_003C_003EAccessor : IMemberAccessor<PlayerId, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerId owner, in ulong value)
				{
					owner.SteamId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerId owner, out ulong value)
				{
					value = owner.SteamId;
				}
			}

			protected class Sandbox_Game_World_MyPlayer_003C_003EPlayerId_003C_003ESerialId_003C_003EAccessor : IMemberAccessor<PlayerId, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerId owner, in int value)
				{
					owner.SerialId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerId owner, out int value)
				{
					value = owner.SerialId;
				}
			}

			public ulong SteamId;

			public int SerialId;

			public static readonly PlayerIdComparerType Comparer = new PlayerIdComparerType();

			public bool IsValid => SteamId != 0;

			public PlayerId(ulong steamId)
				: this(steamId, 0)
			{
			}

			public PlayerId(ulong steamId, int serialId)
			{
				SteamId = steamId;
				SerialId = serialId;
			}

			public static bool operator ==(PlayerId a, PlayerId b)
			{
				if (a.SteamId == b.SteamId)
				{
					return a.SerialId == b.SerialId;
				}
				return false;
			}

			public static bool operator !=(PlayerId a, PlayerId b)
			{
				return !(a == b);
			}

			public override string ToString()
			{
				return SteamId + ":" + SerialId;
			}

			public override bool Equals(object obj)
			{
				if (obj is PlayerId)
				{
					return (PlayerId)obj == this;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (SteamId.GetHashCode() * 571) ^ SerialId.GetHashCode();
			}

			public int CompareTo(PlayerId other)
			{
				if (SteamId < other.SteamId)
				{
					return -1;
				}
				if (SteamId > other.SteamId)
				{
					return 1;
				}
				if (SerialId < other.SerialId)
				{
					return -1;
				}
				if (SerialId > other.SerialId)
				{
					return 1;
				}
				return 0;
			}

			public static PlayerId operator ++(PlayerId id)
			{
				id.SerialId++;
				return id;
			}

			public static PlayerId operator --(PlayerId id)
			{
				id.SerialId--;
				return id;
			}
		}

		public const int BUILD_COLOR_SLOTS_COUNT = 14;

		private static readonly List<Vector3> m_buildColorDefaults;

		private MyNetworkClient m_client;

		private MyIdentity m_identity;

		private int m_selectedBuildColorSlot;

		private string m_buildArmorSkin = string.Empty;

		private List<Vector3> m_buildColorHSVSlots = new List<Vector3>();

		private bool m_forceRealPlayer;

		public HashSet<long> Grids = new HashSet<long>();

		public List<long> CachedControllerId;

		public static int SelectedColorSlot
		{
			get
			{
				if (MySession.Static.LocalHumanPlayer == null)
				{
					return 0;
				}
				return MySession.Static.LocalHumanPlayer.SelectedBuildColorSlot;
			}
		}

		public static Vector3 SelectedColor
		{
			get
			{
				if (MySession.Static.LocalHumanPlayer == null)
				{
					return m_buildColorDefaults[0];
				}
				return MySession.Static.LocalHumanPlayer.SelectedBuildColor;
			}
		}

		public static ListReader<Vector3> ColorSlots
		{
			get
			{
				if (MySession.Static.LocalHumanPlayer == null)
				{
					return new ListReader<Vector3>(m_buildColorDefaults);
				}
				return MySession.Static.LocalHumanPlayer.BuildColorSlots;
			}
		}

		public static ListReader<Vector3> DefaultBuildColorSlots => m_buildColorDefaults;

		public static string SelectedArmorSkin => MySession.Static.LocalHumanPlayer?.BuildArmorSkin ?? string.Empty;

		public MyNetworkClient Client => m_client;

		public MyIdentity Identity
		{
			get
			{
				return m_identity;
			}
			set
			{
				m_identity = value;
				if (this.IdentityChanged != null)
				{
					this.IdentityChanged(this, value);
				}
			}
		}

		public MyEntityController Controller
		{
			get;
			private set;
		}

		public string DisplayName
		{
			get;
			private set;
		}

		public int SelectedBuildColorSlot
		{
			get
			{
				return m_selectedBuildColorSlot;
			}
			set
			{
				m_selectedBuildColorSlot = MathHelper.Clamp(value, 0, m_buildColorHSVSlots.Count - 1);
				MySession.Static.OnLocalPlayerSkinOrColorChanged.InvokeIfNotNull();
			}
		}

		public Vector3 SelectedBuildColor
		{
			get
			{
				return m_buildColorHSVSlots[m_selectedBuildColorSlot];
			}
			set
			{
				m_buildColorHSVSlots[m_selectedBuildColorSlot] = value;
			}
		}

		public List<Vector3> BuildColorSlots
		{
			get
			{
				return m_buildColorHSVSlots;
			}
			set
			{
				SetBuildColorSlots(value);
			}
		}

		public string BuildArmorSkin
		{
			get
			{
				return m_buildArmorSkin;
			}
			set
			{
				m_buildArmorSkin = value;
				MySession.Static.OnLocalPlayerSkinOrColorChanged.InvokeIfNotNull();
			}
		}

		public bool IsLocalPlayer => m_client == Sync.Clients.LocalClient;

		public bool IsRemotePlayer => m_client != Sync.Clients.LocalClient;

		public bool IsRealPlayer
		{
			get
			{
				if (!m_forceRealPlayer)
				{
					return Id.SerialId == 0;
				}
				return true;
			}
		}

		public bool IsBot => !IsRealPlayer;

		public bool IsImmortal
		{
			get
			{
				if (IsRealPlayer)
				{
					return Id.SerialId != 0;
				}
				return false;
			}
		}

		public MyCharacter Character => Identity.Character;

		public PlayerId Id
		{
			get;
			protected set;
		}

		public List<long> RespawnShip
		{
			get
			{
				if (m_identity == null)
				{
					return null;
				}
				return m_identity.RespawnShips;
			}
		}

		IMyNetworkClient IMyPlayer.Client => Client;

		HashSet<long> IMyPlayer.Grids => Grids;

		IMyEntityController IMyPlayer.Controller => Controller;

		string IMyPlayer.DisplayName => DisplayName;

		ulong IMyPlayer.SteamUserId => Id.SteamId;

		long IMyPlayer.PlayerID => Identity.IdentityId;

		long IMyPlayer.IdentityId => Identity.IdentityId;

		bool IMyPlayer.IsAdmin => MySession.Static.IsUserAdmin(Id.SteamId);

		bool IMyPlayer.IsPromoted => MySession.Static.IsUserSpaceMaster(Id.SteamId);

		MyPromoteLevel IMyPlayer.PromoteLevel => MySession.Static.GetUserPromoteLevel(Id.SteamId);

		IMyCharacter IMyPlayer.Character => Character;

		Vector3 IMyPlayer.SelectedBuildColor
		{
			get
			{
				return SelectedBuildColor;
			}
			set
			{
				SelectedBuildColor = value;
			}
		}

		int IMyPlayer.SelectedBuildColorSlot
		{
			get
			{
				return SelectedBuildColorSlot;
			}
			set
			{
				SelectedBuildColorSlot = value;
			}
		}

		bool IMyPlayer.IsBot => IsBot;

		IMyIdentity IMyPlayer.Identity => Identity;

		ListReader<long> IMyPlayer.RespawnShip
		{
			get
			{
				if (m_identity == null)
				{
					return null;
				}
				return m_identity.RespawnShips;
			}
		}

		List<Vector3> IMyPlayer.BuildColorSlots
		{
			get
			{
				return BuildColorSlots;
			}
			set
			{
				BuildColorSlots = value;
			}
		}

		ListReader<Vector3> IMyPlayer.DefaultBuildColorSlots => DefaultBuildColorSlots;

		public event Action<MyPlayer, MyIdentity> IdentityChanged;

		event Action<IMyPlayer, IMyIdentity> IMyPlayer.IdentityChanged
		{
			add
			{
				IdentityChanged += GetDelegate(value);
			}
			remove
			{
				IdentityChanged -= GetDelegate(value);
			}
		}

		static MyPlayer()
		{
			m_buildColorDefaults = new List<Vector3>();
			InitDefaultColors();
		}

		public MyPlayer(MyNetworkClient client, PlayerId id)
		{
			m_client = client;
			Id = id;
			Controller = new MyEntityController(this);
		}

		public void GetColorPreviousCurrentNext(out Vector3 prev, out Vector3 cur, out Vector3 next)
		{
			prev = m_buildColorHSVSlots[(m_selectedBuildColorSlot + m_buildColorHSVSlots.Count - 1) % m_buildColorHSVSlots.Count];
			cur = m_buildColorHSVSlots[m_selectedBuildColorSlot];
			next = m_buildColorHSVSlots[(m_selectedBuildColorSlot + 1) % m_buildColorHSVSlots.Count];
		}

		public void Init(MyObjectBuilder_Player objectBuilder)
		{
			DisplayName = objectBuilder.DisplayName;
			Identity = Sync.Players.TryGetIdentity(objectBuilder.IdentityId);
			m_forceRealPlayer = objectBuilder.ForceRealPlayer;
			m_buildArmorSkin = objectBuilder.BuildArmorSkin;
			m_selectedBuildColorSlot = objectBuilder.BuildColorSlot;
			if (m_buildColorHSVSlots.Count < 14)
			{
				int count = m_buildColorHSVSlots.Count;
				for (int i = 0; i < 14 - count; i++)
				{
					m_buildColorHSVSlots.Add(MyRenderComponentBase.OldBlackToHSV);
				}
			}
			if (objectBuilder.BuildColorSlots == null || objectBuilder.BuildColorSlots.Count == 0)
			{
				SetDefaultColors();
			}
			else if (objectBuilder.BuildColorSlots.Count == 14)
			{
				m_buildColorHSVSlots = objectBuilder.BuildColorSlots;
			}
			else if (objectBuilder.BuildColorSlots.Count > 14)
			{
				m_buildColorHSVSlots = new List<Vector3>(14);
				for (int j = 0; j < 14; j++)
				{
					m_buildColorHSVSlots.Add(objectBuilder.BuildColorSlots[j]);
				}
			}
			else
			{
				m_buildColorHSVSlots = objectBuilder.BuildColorSlots;
				for (int k = m_buildColorHSVSlots.Count - 1; k < 14; k++)
				{
					m_buildColorHSVSlots.Add(MyRenderComponentBase.OldBlackToHSV);
				}
			}
			if (Sync.IsServer && Id.SerialId == 0)
			{
				if (MyCubeBuilder.AllPlayersColors == null)
				{
					MyCubeBuilder.AllPlayersColors = new Dictionary<PlayerId, List<Vector3>>();
				}
				if (!MyCubeBuilder.AllPlayersColors.ContainsKey(Id))
				{
					MyCubeBuilder.AllPlayersColors.Add(Id, m_buildColorHSVSlots);
				}
				else
				{
					MyCubeBuilder.AllPlayersColors.TryGetValue(Id, out m_buildColorHSVSlots);
				}
			}
		}

		public MyObjectBuilder_Player GetObjectBuilder()
		{
			MyObjectBuilder_Player myObjectBuilder_Player = new MyObjectBuilder_Player
			{
				DisplayName = DisplayName,
				IdentityId = Identity.IdentityId,
				Connected = true,
				ForceRealPlayer = m_forceRealPlayer
			};
			myObjectBuilder_Player.BuildArmorSkin = m_buildArmorSkin;
			myObjectBuilder_Player.BuildColorSlot = m_selectedBuildColorSlot;
			if (!IsColorsSetToDefaults(m_buildColorHSVSlots))
			{
				myObjectBuilder_Player.BuildColorSlots = new List<Vector3>();
				{
					foreach (Vector3 buildColorHSVSlot in m_buildColorHSVSlots)
					{
						myObjectBuilder_Player.BuildColorSlots.Add(buildColorHSVSlot);
					}
					return myObjectBuilder_Player;
				}
			}
			return myObjectBuilder_Player;
		}

		public static bool IsColorsSetToDefaults(List<Vector3> colors)
		{
			if (colors.Count != 14)
			{
				return false;
			}
			for (int i = 0; i < 14; i++)
			{
				if (colors[i] != m_buildColorDefaults[i])
				{
					return false;
				}
			}
			return true;
		}

		public void SetDefaultColors()
		{
			for (int i = 0; i < 14; i++)
			{
				m_buildColorHSVSlots[i] = m_buildColorDefaults[i];
			}
		}

		private static void InitDefaultColors()
		{
			if (m_buildColorDefaults.Count < 14)
			{
				int count = m_buildColorDefaults.Count;
				for (int i = 0; i < 14 - count; i++)
				{
					m_buildColorDefaults.Add(MyRenderComponentBase.OldBlackToHSV);
				}
			}
			m_buildColorDefaults[0] = MyRenderComponentBase.OldGrayToHSV;
			m_buildColorDefaults[1] = MyRenderComponentBase.OldRedToHSV;
			m_buildColorDefaults[2] = MyRenderComponentBase.OldGreenToHSV;
			m_buildColorDefaults[3] = MyRenderComponentBase.OldBlueToHSV;
			m_buildColorDefaults[4] = MyRenderComponentBase.OldYellowToHSV;
			m_buildColorDefaults[5] = MyRenderComponentBase.OldWhiteToHSV;
			m_buildColorDefaults[6] = MyRenderComponentBase.OldBlackToHSV;
			for (int j = 7; j < 14; j++)
			{
				m_buildColorDefaults[j] = m_buildColorDefaults[j - 7] + new Vector3(0f, 0.15f, 0.2f);
			}
		}

		public void ChangeOrSwitchToColor(Vector3 color)
		{
			for (int i = 0; i < 14; i++)
			{
				if (m_buildColorHSVSlots[i] == color)
				{
					m_selectedBuildColorSlot = i;
					return;
				}
			}
			SelectedBuildColor = color;
		}

		public void SetBuildColorSlots(List<Vector3> newColors)
		{
			for (int i = 0; i < 14; i++)
			{
				m_buildColorHSVSlots[i] = MyRenderComponentBase.OldBlackToHSV;
			}
			for (int j = 0; j < Math.Min(newColors.Count, 14); j++)
			{
				m_buildColorHSVSlots[j] = newColors[j];
			}
			if (MyCubeBuilder.AllPlayersColors != null && MyCubeBuilder.AllPlayersColors.Remove(Id))
			{
				MyCubeBuilder.AllPlayersColors.Add(Id, m_buildColorHSVSlots);
			}
		}

		public Vector3D GetPosition()
		{
			if (Controller.ControlledEntity != null && Controller.ControlledEntity.Entity != null)
			{
				return Controller.ControlledEntity.Entity.PositionComp.GetPosition();
			}
			return Vector3D.Zero;
		}

		public void SpawnAt(MatrixD worldMatrix, Vector3 velocity, MyEntity spawnedBy, MyBotDefinition botDefinition, bool findFreePlace = true, string modelName = null, Color? color = null)
		{
			if (!Sync.IsServer || Identity == null)
			{
				return;
			}
			MatrixD worldMatrix2 = worldMatrix;
			string displayName = Identity.DisplayName;
			string model = modelName ?? Identity.Model;
			Vector3? colorMask = color.HasValue ? new Vector3?(color.Value.ToVector3()) : null;
			bool useInventory = Id.SerialId == 0;
			long identityId = Identity.IdentityId;
			MyCharacter myCharacter = MyCharacter.CreateCharacter(worldMatrix2, velocity, displayName, model, colorMask, botDefinition, findNearPos: false, AIMode: false, null, useInventory, identityId);
			if (findFreePlace)
			{
				float num = myCharacter.Render.GetModel().BoundingBox.Size.Length() / 2f;
				num *= 0.9f;
				Vector3 value = worldMatrix.Up;
				value.Normalize();
				Vector3 value2 = value * (num + 0.01f);
				MatrixD matrix = worldMatrix;
				matrix.Translation = worldMatrix.Translation + value2;
				Vector3D? vector3D = MyEntities.FindFreePlace(ref matrix, matrix.GetDirectionVector(Base6Directions.Direction.Up), num, 200, 15, 0.2f);
				if (!vector3D.HasValue)
				{
					vector3D = MyEntities.FindFreePlace(ref matrix, matrix.GetDirectionVector(Base6Directions.Direction.Right), num, 200, 15, 0.2f);
					if (!vector3D.HasValue)
					{
						vector3D = MyEntities.FindFreePlace(worldMatrix.Translation + value2, num, 200, 15, 0.2f);
					}
				}
				if (vector3D.HasValue)
				{
					worldMatrix.Translation = vector3D.Value - value2;
					myCharacter.PositionComp.SetWorldMatrix(worldMatrix);
				}
			}
			Sync.Players.SetPlayerCharacter(this, myCharacter, spawnedBy);
			Sync.Players.RevivePlayer(this);
		}

		public void SpawnIntoCharacter(MyCharacter character)
		{
			Sync.Players.SetPlayerCharacter(this, character, null);
			Sync.Players.RevivePlayer(this);
		}

		public static MyRelationsBetweenPlayerAndBlock GetRelationBetweenPlayers(long playerId1, long playerId2)
		{
			if (playerId1 == playerId2)
			{
				return MyRelationsBetweenPlayerAndBlock.Owner;
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId1);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(playerId2);
			if (myFaction == null || myFaction2 == null)
			{
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			}
			if (myFaction == myFaction2)
			{
				return MyRelationsBetweenPlayerAndBlock.FactionShare;
			}
			if (MySession.Static.Factions.GetRelationBetweenFactions(myFaction.FactionId, myFaction2.FactionId).Item1 == MyRelationsBetweenFactions.Neutral)
			{
				return MyRelationsBetweenPlayerAndBlock.Neutral;
			}
			return MyRelationsBetweenPlayerAndBlock.Enemies;
		}

		public MyRelationsBetweenPlayerAndBlock GetRelationTo(long playerId)
		{
			if (Identity == null)
			{
				return MyRelationsBetweenPlayerAndBlock.Enemies;
			}
			return GetRelationBetweenPlayers(Identity.IdentityId, playerId);
		}

		public static MyRelationsBetweenPlayers GetRelationsBetweenPlayers(long playerId1, long playerId2)
		{
			return MyIDModule.GetRelationPlayerPlayer(playerId1, playerId2);
		}

		public void RemoveGrid(long gridEntityId)
		{
			Grids.Remove(gridEntityId);
		}

		public void AddGrid(long gridEntityId)
		{
			Grids.Add(gridEntityId);
		}

		public static MyPlayer GetPlayerFromCharacter(MyCharacter character)
		{
			if (character == null)
			{
				return null;
			}
			if (character.ControllerInfo != null && character.ControllerInfo.Controller != null)
			{
				return character.ControllerInfo.Controller.Player;
			}
			return null;
		}

		public static MyPlayer GetPlayerFromWeapon(IMyGunBaseUser gunUser)
		{
			if (gunUser == null)
			{
				return null;
			}
			MyCharacter myCharacter = gunUser.Owner as MyCharacter;
			if (myCharacter != null)
			{
				return GetPlayerFromCharacter(myCharacter);
			}
			return null;
		}

		public void ReleaseControls()
		{
			Controller.SaveCamera();
			if (Controller.ControlledEntity != null)
			{
				Controller.ControlledEntity.ControllerInfo.ReleaseControls();
			}
		}

		public void AcquireControls()
		{
			if (Controller.ControlledEntity != null)
			{
				Controller.ControlledEntity.ControllerInfo.AcquireControls();
			}
			Controller.SetCamera();
		}

		private Action<MyPlayer, MyIdentity> GetDelegate(Action<IMyPlayer, IMyIdentity> value)
		{
			return (Action<MyPlayer, MyIdentity>)Delegate.CreateDelegate(typeof(Action<MyPlayer, MyIdentity>), value.Target, value.Method);
		}

		MyRelationsBetweenPlayerAndBlock IMyPlayer.GetRelationTo(long playerId)
		{
			return GetRelationTo(playerId);
		}

		void IMyPlayer.RemoveGrid(long gridEntityId)
		{
			RemoveGrid(gridEntityId);
		}

		void IMyPlayer.AddGrid(long gridEntityId)
		{
			AddGrid(gridEntityId);
		}

		Vector3D IMyPlayer.GetPosition()
		{
			return GetPosition();
		}

		void IMyPlayer.ChangeOrSwitchToColor(Vector3 color)
		{
			ChangeOrSwitchToColor(color);
		}

		void IMyPlayer.SetDefaultColors()
		{
			SetDefaultColors();
		}

		void IMyPlayer.SpawnIntoCharacter(IMyCharacter character)
		{
			SpawnIntoCharacter((MyCharacter)character);
		}

		void IMyPlayer.SpawnAt(MatrixD worldMatrix, Vector3 velocity, IMyEntity spawnedBy, bool findFreePlace, string modelName, Color? color)
		{
			SpawnAt(worldMatrix, velocity, (MyEntity)spawnedBy, null, findFreePlace, modelName, color);
		}

		void IMyPlayer.SpawnAt(MatrixD worldMatrix, Vector3 velocity, IMyEntity spawnedBy)
		{
			SpawnAt(worldMatrix, velocity, (MyEntity)spawnedBy, null);
		}

		bool IMyPlayer.TryGetBalanceInfo(out long balance)
		{
			balance = 0L;
			if (MyBankingSystem.Static != null && MyBankingSystem.Static.TryGetAccountInfo(Identity.IdentityId, out MyAccountInfo account))
			{
				balance = account.Balance;
				return true;
			}
			return false;
		}

		string IMyPlayer.GetBalanceShortString()
		{
			if (MyBankingSystem.Static == null)
			{
				return null;
			}
			return MyBankingSystem.Static.GetBalanceShortString(Identity.IdentityId);
		}

		void IMyPlayer.RequestChangeBalance(long amount)
		{
			if (MyBankingSystem.Static != null)
			{
				MyBankingSystem.RequestBalanceChange(Identity.IdentityId, amount);
			}
		}
	}
}
