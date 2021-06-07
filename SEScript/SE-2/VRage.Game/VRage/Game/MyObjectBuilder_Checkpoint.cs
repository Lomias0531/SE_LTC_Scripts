using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.Definitions;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Checkpoint : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct PlayerId
		{
			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerId_003C_003EClientId_003C_003EAccessor : IMemberAccessor<PlayerId, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerId owner, in ulong value)
				{
					owner.ClientId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerId owner, out ulong value)
				{
					value = owner.ClientId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerId_003C_003ESerialId_003C_003EAccessor : IMemberAccessor<PlayerId, int>
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

			private class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerId_003C_003EActor : IActivator, IActivator<PlayerId>
			{
				private sealed override object CreateInstance()
				{
					return default(PlayerId);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override PlayerId CreateInstance()
				{
					return (PlayerId)(object)default(PlayerId);
				}

				PlayerId IActivator<PlayerId>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(183)]
			public ulong ClientId;

			[ProtoMember(185)]
			public int SerialId;

			public PlayerId(ulong steamId)
			{
				ClientId = steamId;
				SerialId = 0;
			}

			public void AssignFrom(ulong steamId)
			{
				ClientId = steamId;
				SerialId = 0;
			}
		}

		[ProtoContract]
		public struct PlayerItem
		{
			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerItem_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<PlayerItem, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerItem owner, in long value)
				{
					owner.PlayerId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerItem owner, out long value)
				{
					value = owner.PlayerId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerItem_003C_003EIsDead_003C_003EAccessor : IMemberAccessor<PlayerItem, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerItem owner, in bool value)
				{
					owner.IsDead = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerItem owner, out bool value)
				{
					value = owner.IsDead;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerItem_003C_003EName_003C_003EAccessor : IMemberAccessor<PlayerItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerItem owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerItem owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerItem_003C_003ESteamId_003C_003EAccessor : IMemberAccessor<PlayerItem, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerItem owner, in ulong value)
				{
					owner.SteamId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerItem owner, out ulong value)
				{
					value = owner.SteamId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerItem_003C_003EModel_003C_003EAccessor : IMemberAccessor<PlayerItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref PlayerItem owner, in string value)
				{
					owner.Model = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref PlayerItem owner, out string value)
				{
					value = owner.Model;
				}
			}

			private class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayerItem_003C_003EActor : IActivator, IActivator<PlayerItem>
			{
				private sealed override object CreateInstance()
				{
					return default(PlayerItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override PlayerItem CreateInstance()
				{
					return (PlayerItem)(object)default(PlayerItem);
				}

				PlayerItem IActivator<PlayerItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(55)]
			public long PlayerId;

			[ProtoMember(58)]
			public bool IsDead;

			[ProtoMember(61)]
			public string Name;

			[ProtoMember(64)]
			public ulong SteamId;

			[ProtoMember(67)]
			public string Model;

			public PlayerItem(long id, string name, bool isDead, ulong steamId, string model)
			{
				PlayerId = id;
				IsDead = isDead;
				Name = name;
				SteamId = steamId;
				Model = model;
			}
		}

		[ProtoContract]
		public struct ModItem
		{
			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EModItem_003C_003Em_workshopItem_003C_003EAccessor : IMemberAccessor<ModItem, MyWorkshopItem>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModItem owner, in MyWorkshopItem value)
				{
					owner.m_workshopItem = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModItem owner, out MyWorkshopItem value)
				{
					value = owner.m_workshopItem;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EModItem_003C_003EName_003C_003EAccessor : IMemberAccessor<ModItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModItem owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModItem owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EModItem_003C_003EPublishedFileId_003C_003EAccessor : IMemberAccessor<ModItem, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModItem owner, in ulong value)
				{
					owner.PublishedFileId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModItem owner, out ulong value)
				{
					value = owner.PublishedFileId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EModItem_003C_003EIsDependency_003C_003EAccessor : IMemberAccessor<ModItem, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModItem owner, in bool value)
				{
					owner.IsDependency = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModItem owner, out bool value)
				{
					value = owner.IsDependency;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EModItem_003C_003EFriendlyName_003C_003EAccessor : IMemberAccessor<ModItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModItem owner, in string value)
				{
					owner.FriendlyName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModItem owner, out string value)
				{
					value = owner.FriendlyName;
				}
			}

			private class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EModItem_003C_003EActor : IActivator, IActivator<ModItem>
			{
				private sealed override object CreateInstance()
				{
					return default(ModItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ModItem CreateInstance()
				{
					return (ModItem)(object)default(ModItem);
				}

				ModItem IActivator<ModItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private MyWorkshopItem m_workshopItem;

			[ProtoMember(70)]
			public string Name;

			[ProtoMember(73)]
			[DefaultValue(0)]
			public ulong PublishedFileId;

			[ProtoMember(76)]
			[DefaultValue(false)]
			public bool IsDependency;

			[ProtoMember(79)]
			[XmlAttribute]
			public string FriendlyName;

			public bool ShouldSerializeName()
			{
				return Name != null;
			}

			public bool ShouldSerializePublishedFileId()
			{
				return PublishedFileId != 0;
			}

			public bool ShouldSerializeIsDependency()
			{
				return true;
			}

			public bool ShouldSerializeFriendlyName()
			{
				return !string.IsNullOrEmpty(FriendlyName);
			}

			public ModItem(ulong publishedFileId)
			{
				Name = publishedFileId + ".sbm";
				PublishedFileId = publishedFileId;
				FriendlyName = string.Empty;
				IsDependency = false;
				m_workshopItem = null;
			}

			public ModItem(ulong publishedFileId, bool isDependency)
			{
				Name = publishedFileId + ".sbm";
				PublishedFileId = publishedFileId;
				FriendlyName = string.Empty;
				IsDependency = isDependency;
				m_workshopItem = null;
			}

			public ModItem(string name, ulong publishedFileId)
			{
				Name = (name ?? (publishedFileId + ".sbm"));
				PublishedFileId = publishedFileId;
				FriendlyName = string.Empty;
				IsDependency = false;
				m_workshopItem = null;
			}

			public ModItem(string name, ulong publishedFileId, string friendlyName)
			{
				Name = (name ?? (publishedFileId + ".sbm"));
				PublishedFileId = publishedFileId;
				FriendlyName = friendlyName;
				IsDependency = false;
				m_workshopItem = null;
			}

			public override string ToString()
			{
				return $"{FriendlyName} ({PublishedFileId})";
			}

			public void SetModData(MyWorkshopItem workshopItem)
			{
				m_workshopItem = workshopItem;
			}

			public string GetPath()
			{
				if (m_workshopItem != null)
				{
					return m_workshopItem.Folder;
				}
				return Path.Combine(MyFileSystem.ModsPath, Name);
			}
		}

		[ProtoContract]
		public struct RespawnCooldownItem
		{
			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERespawnCooldownItem_003C_003EPlayerSteamId_003C_003EAccessor : IMemberAccessor<RespawnCooldownItem, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownItem owner, in ulong value)
				{
					owner.PlayerSteamId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownItem owner, out ulong value)
				{
					value = owner.PlayerSteamId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERespawnCooldownItem_003C_003EPlayerSerialId_003C_003EAccessor : IMemberAccessor<RespawnCooldownItem, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownItem owner, in int value)
				{
					owner.PlayerSerialId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownItem owner, out int value)
				{
					value = owner.PlayerSerialId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERespawnCooldownItem_003C_003ERespawnShipId_003C_003EAccessor : IMemberAccessor<RespawnCooldownItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownItem owner, in string value)
				{
					owner.RespawnShipId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownItem owner, out string value)
				{
					value = owner.RespawnShipId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERespawnCooldownItem_003C_003ECooldown_003C_003EAccessor : IMemberAccessor<RespawnCooldownItem, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnCooldownItem owner, in int value)
				{
					owner.Cooldown = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnCooldownItem owner, out int value)
				{
					value = owner.Cooldown;
				}
			}

			private class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERespawnCooldownItem_003C_003EActor : IActivator, IActivator<RespawnCooldownItem>
			{
				private sealed override object CreateInstance()
				{
					return default(RespawnCooldownItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override RespawnCooldownItem CreateInstance()
				{
					return (RespawnCooldownItem)(object)default(RespawnCooldownItem);
				}

				RespawnCooldownItem IActivator<RespawnCooldownItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(91)]
			public ulong PlayerSteamId;

			[ProtoMember(94)]
			public int PlayerSerialId;

			[ProtoMember(97)]
			public string RespawnShipId;

			[ProtoMember(100)]
			public int Cooldown;
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECurrentSector_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableVector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableVector3I value)
			{
				owner.CurrentSector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableVector3I value)
			{
				value = owner.CurrentSector;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EElapsedGameTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in long value)
			{
				owner.ElapsedGameTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out long value)
			{
				value = owner.ElapsedGameTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESessionName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.SessionName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.SessionName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESpectatorPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyPositionAndOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyPositionAndOrientation value)
			{
				owner.SpectatorPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyPositionAndOrientation value)
			{
				value = owner.SpectatorPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESpectatorSpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableVector2 value)
			{
				owner.SpectatorSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableVector2 value)
			{
				value = owner.SpectatorSpeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESpectatorIsLightOn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.SpectatorIsLightOn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.SpectatorIsLightOn;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECameraController_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyCameraControllerEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyCameraControllerEnum value)
			{
				owner.CameraController = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyCameraControllerEnum value)
			{
				value = owner.CameraController;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECameraEntity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in long value)
			{
				owner.CameraEntity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out long value)
			{
				value = owner.CameraEntity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EControlledObject_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in long value)
			{
				owner.ControlledObject = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out long value)
			{
				value = owner.ControlledObject;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPassword_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.Password = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.Password;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.Description = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.Description;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ELastSaveTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, DateTime>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in DateTime value)
			{
				owner.LastSaveTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out DateTime value)
			{
				value = owner.LastSaveTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESpectatorDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in float value)
			{
				owner.SpectatorDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out float value)
			{
				value = owner.SpectatorDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EWorkshopId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, ulong?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in ulong? value)
			{
				owner.WorkshopId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out ulong? value)
			{
				value = owner.WorkshopId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECharacterToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_Toolbar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyObjectBuilder_Toolbar value)
			{
				owner.CharacterToolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyObjectBuilder_Toolbar value)
			{
				value = owner.CharacterToolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EControlledEntities_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<long, PlayerId>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<long, PlayerId> value)
			{
				owner.ControlledEntities = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<long, PlayerId> value)
			{
				value = owner.ControlledEntities;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_SessionSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyObjectBuilder_SessionSettings value)
			{
				owner.Settings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyObjectBuilder_SessionSettings value)
			{
				value = owner.Settings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EScriptManagerData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_ScriptManager>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyObjectBuilder_ScriptManager value)
			{
				owner.ScriptManagerData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyObjectBuilder_ScriptManager value)
			{
				value = owner.ScriptManagerData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAppVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in int value)
			{
				owner.AppVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out int value)
			{
				value = owner.AppVersion;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EFactions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_FactionCollection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyObjectBuilder_FactionCollection value)
			{
				owner.Factions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyObjectBuilder_FactionCollection value)
			{
				value = owner.Factions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EMods_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<ModItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<ModItem> value)
			{
				owner.Mods = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<ModItem> value)
			{
				value = owner.Mods;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPromotedUsers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<ulong, MyPromoteLevel>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<ulong, MyPromoteLevel> value)
			{
				owner.PromotedUsers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<ulong, MyPromoteLevel> value)
			{
				value = owner.PromotedUsers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECreativeTools_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, HashSet<ulong>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in HashSet<ulong> value)
			{
				owner.CreativeTools = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out HashSet<ulong> value)
			{
				value = owner.CreativeTools;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EScenario_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDefinitionId value)
			{
				owner.Scenario = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDefinitionId value)
			{
				value = owner.Scenario;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERespawnCooldowns_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<RespawnCooldownItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<RespawnCooldownItem> value)
			{
				owner.RespawnCooldowns = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<RespawnCooldownItem> value)
			{
				value = owner.RespawnCooldowns;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EIdentities_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Identity>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<MyObjectBuilder_Identity> value)
			{
				owner.Identities = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<MyObjectBuilder_Identity> value)
			{
				value = owner.Identities;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EClients_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_Client>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<MyObjectBuilder_Client> value)
			{
				owner.Clients = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<MyObjectBuilder_Client> value)
			{
				value = owner.Clients;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPreviousEnvironmentHostility_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyEnvironmentHostilityEnum?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyEnvironmentHostilityEnum? value)
			{
				owner.PreviousEnvironmentHostility = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyEnvironmentHostilityEnum? value)
			{
				value = owner.PreviousEnvironmentHostility;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESharedToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in ulong value)
			{
				owner.SharedToolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out ulong value)
			{
				value = owner.SharedToolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAllPlayersData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<PlayerId, MyObjectBuilder_Player>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<PlayerId, MyObjectBuilder_Player> value)
			{
				owner.AllPlayersData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<PlayerId, MyObjectBuilder_Player> value)
			{
				value = owner.AllPlayersData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAllPlayersColors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<PlayerId, List<Vector3>>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<PlayerId, List<Vector3>> value)
			{
				owner.AllPlayersColors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<PlayerId, List<Vector3>> value)
			{
				value = owner.AllPlayersColors;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EChatHistory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_ChatHistory>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<MyObjectBuilder_ChatHistory> value)
			{
				owner.ChatHistory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<MyObjectBuilder_ChatHistory> value)
			{
				value = owner.ChatHistory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EFactionChatHistory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_FactionChatHistory>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<MyObjectBuilder_FactionChatHistory> value)
			{
				owner.FactionChatHistory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<MyObjectBuilder_FactionChatHistory> value)
			{
				value = owner.FactionChatHistory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ENonPlayerIdentities_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<long> value)
			{
				owner.NonPlayerIdentities = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<long> value)
			{
				value = owner.NonPlayerIdentities;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EGps_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<long, MyObjectBuilder_Gps>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<long, MyObjectBuilder_Gps> value)
			{
				owner.Gps = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<long, MyObjectBuilder_Gps> value)
			{
				value = owner.Gps;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EWorldBoundaries_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableBoundingBoxD?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableBoundingBoxD? value)
			{
				owner.WorldBoundaries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableBoundingBoxD? value)
			{
				value = owner.WorldBoundaries;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESessionComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<MyObjectBuilder_SessionComponent>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<MyObjectBuilder_SessionComponent> value)
			{
				owner.SessionComponents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<MyObjectBuilder_SessionComponent> value)
			{
				value = owner.SessionComponents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EGameDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDefinitionId value)
			{
				owner.GameDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDefinitionId value)
			{
				value = owner.GameDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESessionComponentEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, HashSet<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in HashSet<string> value)
			{
				owner.SessionComponentEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out HashSet<string> value)
			{
				value = owner.SessionComponentEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESessionComponentDisabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, HashSet<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in HashSet<string> value)
			{
				owner.SessionComponentDisabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out HashSet<string> value)
			{
				value = owner.SessionComponentDisabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EInGameTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, DateTime>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in DateTime value)
			{
				owner.InGameTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out DateTime value)
			{
				value = owner.InGameTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EMissionTriggers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyObjectBuilder_SessionComponentMission>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyObjectBuilder_SessionComponentMission value)
			{
				owner.MissionTriggers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyObjectBuilder_SessionComponentMission value)
			{
				value = owner.MissionTriggers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EBriefing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.Briefing = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.Briefing;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EBriefingVideo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.BriefingVideo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.BriefingVideo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECustomLoadingScreenImage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.CustomLoadingScreenImage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.CustomLoadingScreenImage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECustomLoadingScreenText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.CustomLoadingScreenText = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.CustomLoadingScreenText;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECustomSkybox_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				owner.CustomSkybox = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				value = owner.CustomSkybox;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERequiresDX_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in int value)
			{
				owner.RequiresDX = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out int value)
			{
				value = owner.RequiresDX;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EVicinityModelsCache_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<string> value)
			{
				owner.VicinityModelsCache = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<string> value)
			{
				value = owner.VicinityModelsCache;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EVicinityArmorModelsCache_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<string> value)
			{
				owner.VicinityArmorModelsCache = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<string> value)
			{
				value = owner.VicinityArmorModelsCache;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EVicinityVoxelCache_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<string> value)
			{
				owner.VicinityVoxelCache = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<string> value)
			{
				value = owner.VicinityVoxelCache;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<ulong, MyObjectBuilder_Player>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<ulong, MyObjectBuilder_Player> value)
			{
				owner.Players = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<ulong, MyObjectBuilder_Player> value)
			{
				value = owner.Players;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EConnectedPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<PlayerId, MyObjectBuilder_Player>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<PlayerId, MyObjectBuilder_Player> value)
			{
				owner.ConnectedPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<PlayerId, MyObjectBuilder_Player> value)
			{
				value = owner.ConnectedPlayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EDisconnectedPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<PlayerId, long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<PlayerId, long> value)
			{
				owner.DisconnectedPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<PlayerId, long> value)
			{
				value = owner.DisconnectedPlayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAllPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, List<PlayerItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in List<PlayerItem> value)
			{
				owner.AllPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out List<PlayerItem> value)
			{
				value = owner.AllPlayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERemoteAdminSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, SerializableDictionary<ulong, int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in SerializableDictionary<ulong, int> value)
			{
				owner.RemoteAdminSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out SerializableDictionary<ulong, int> value)
			{
				value = owner.RemoteAdminSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EGameTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, DateTime>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in DateTime value)
			{
				owner.GameTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out DateTime value)
			{
				value = owner.GameTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EOnlineMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyOnlineModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyOnlineModeEnum value)
			{
				owner.OnlineMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyOnlineModeEnum value)
			{
				value = owner.OnlineMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAutoHealing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.AutoHealing = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.AutoHealing;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EEnableCopyPaste_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.EnableCopyPaste = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.EnableCopyPaste;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EMaxPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in short value)
			{
				owner.MaxPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out short value)
			{
				value = owner.MaxPlayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EWeaponsEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.WeaponsEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.WeaponsEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EShowPlayerNamesOnHud_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.ShowPlayerNamesOnHud = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.ShowPlayerNamesOnHud;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EMaxFloatingObjects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in short value)
			{
				owner.MaxFloatingObjects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out short value)
			{
				value = owner.MaxFloatingObjects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EGameMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, MyGameModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyGameModeEnum value)
			{
				owner.GameMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyGameModeEnum value)
			{
				value = owner.GameMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EInventorySizeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in float value)
			{
				owner.InventorySizeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out float value)
			{
				value = owner.InventorySizeMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAssemblerSpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in float value)
			{
				owner.AssemblerSpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out float value)
			{
				value = owner.AssemblerSpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAssemblerEfficiencyMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in float value)
			{
				owner.AssemblerEfficiencyMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out float value)
			{
				value = owner.AssemblerEfficiencyMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ERefinerySpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in float value)
			{
				owner.RefinerySpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out float value)
			{
				value = owner.RefinerySpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EThrusterDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.ThrusterDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.ThrusterDamage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ECargoShipsEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.CargoShipsEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.CargoShipsEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EAutoSave_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Checkpoint, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in bool value)
			{
				owner.AutoSave = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out bool value)
			{
				value = owner.AutoSave;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Checkpoint_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Checkpoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Checkpoint owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Checkpoint owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Checkpoint_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Checkpoint>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Checkpoint();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Checkpoint CreateInstance()
			{
				return new MyObjectBuilder_Checkpoint();
			}

			MyObjectBuilder_Checkpoint IActivator<MyObjectBuilder_Checkpoint>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static SerializableDefinitionId DEFAULT_SCENARIO = new SerializableDefinitionId(typeof(MyObjectBuilder_ScenarioDefinition), "EmptyWorld");

		public static DateTime DEFAULT_DATE = new DateTime(1215, 7, 1, 12, 0, 0);

		[ProtoMember(1)]
		public SerializableVector3I CurrentSector;

		/// <summary>
		/// This is long because TimeSpan is not serialized
		/// </summary>
		[ProtoMember(4)]
		public long ElapsedGameTime;

		[ProtoMember(7)]
		public string SessionName;

		[ProtoMember(10)]
		public MyPositionAndOrientation SpectatorPosition = new MyPositionAndOrientation(Matrix.Identity);

		[ProtoMember(11)]
		public SerializableVector2 SpectatorSpeed = new SerializableVector2(0.1f, 1f);

		[ProtoMember(13)]
		public bool SpectatorIsLightOn;

		[ProtoMember(16)]
		[DefaultValue(MyCameraControllerEnum.Spectator)]
		public MyCameraControllerEnum CameraController;

		[ProtoMember(19)]
		public long CameraEntity;

		[ProtoMember(22)]
		[DefaultValue(-1)]
		public long ControlledObject = -1L;

		[ProtoMember(25)]
		public string Password;

		[ProtoMember(28)]
		public string Description;

		[ProtoMember(31)]
		public DateTime LastSaveTime;

		[ProtoMember(34)]
		public float SpectatorDistance;

		[ProtoMember(37)]
		[DefaultValue(null)]
		public ulong? WorkshopId;

		[ProtoMember(40)]
		public MyObjectBuilder_Toolbar CharacterToolbar;

		[ProtoMember(43)]
		public SerializableDictionary<long, PlayerId> ControlledEntities;

		[ProtoMember(46)]
		[XmlElement("Settings", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_SessionSettings>))]
		public MyObjectBuilder_SessionSettings Settings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();

		public MyObjectBuilder_ScriptManager ScriptManagerData;

		[ProtoMember(49)]
		public int AppVersion;

		[ProtoMember(52)]
		[DefaultValue(null)]
		public MyObjectBuilder_FactionCollection Factions;

		[ProtoMember(82)]
		public List<ModItem> Mods;

		[ProtoMember(85)]
		public SerializableDictionary<ulong, MyPromoteLevel> PromotedUsers;

		public HashSet<ulong> CreativeTools;

		[ProtoMember(88)]
		public SerializableDefinitionId Scenario = DEFAULT_SCENARIO;

		[ProtoMember(103)]
		public List<RespawnCooldownItem> RespawnCooldowns;

		[ProtoMember(106)]
		public List<MyObjectBuilder_Identity> Identities;

		[ProtoMember(109)]
		public List<MyObjectBuilder_Client> Clients;

		[ProtoMember(112)]
		public MyEnvironmentHostilityEnum? PreviousEnvironmentHostility;

		[ProtoMember(113)]
		[DefaultValue(0)]
		public ulong SharedToolbar;

		[ProtoMember(115)]
		public SerializableDictionary<PlayerId, MyObjectBuilder_Player> AllPlayersData;

		[ProtoMember(118)]
		public SerializableDictionary<PlayerId, List<Vector3>> AllPlayersColors;

		[ProtoMember(121)]
		public List<MyObjectBuilder_ChatHistory> ChatHistory;

		[ProtoMember(124)]
		public List<MyObjectBuilder_FactionChatHistory> FactionChatHistory;

		[ProtoMember(127)]
		public List<long> NonPlayerIdentities;

		[ProtoMember(130)]
		public SerializableDictionary<long, MyObjectBuilder_Gps> Gps;

		[ProtoMember(133)]
		public SerializableBoundingBoxD? WorldBoundaries;

		[ProtoMember(136)]
		[XmlArrayItem("MyObjectBuilder_SessionComponent", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_SessionComponent>))]
		public List<MyObjectBuilder_SessionComponent> SessionComponents;

		[ProtoMember(139)]
		public SerializableDefinitionId GameDefinition = MyGameDefinition.Default;

		[ProtoMember(142)]
		public HashSet<string> SessionComponentEnabled = new HashSet<string>();

		[ProtoMember(145)]
		public HashSet<string> SessionComponentDisabled = new HashSet<string>();

		[ProtoMember(148)]
		public DateTime InGameTime = DEFAULT_DATE;

		[ProtoMember(151)]
		public MyObjectBuilder_SessionComponentMission MissionTriggers;

		[ProtoMember(154)]
		public string Briefing;

		[ProtoMember(157)]
		public string BriefingVideo;

		public string CustomLoadingScreenImage;

		public string CustomLoadingScreenText;

		[ProtoMember(160)]
		public string CustomSkybox = "";

		[ProtoMember(163)]
		[DefaultValue(9)]
		public int RequiresDX = 9;

		[ProtoMember(166)]
		public List<string> VicinityModelsCache;

		[ProtoMember(169)]
		public List<string> VicinityArmorModelsCache;

		[ProtoMember(172)]
		public List<string> VicinityVoxelCache;

		public SerializableDictionary<ulong, MyObjectBuilder_Player> Players;

		[ProtoMember(175)]
		public SerializableDictionary<PlayerId, MyObjectBuilder_Player> ConnectedPlayers;

		[ProtoMember(178)]
		public SerializableDictionary<PlayerId, long> DisconnectedPlayers;

		public List<PlayerItem> AllPlayers;

		[ProtoMember(181)]
		public SerializableDictionary<ulong, int> RemoteAdminSettings = new SerializableDictionary<ulong, int>();

		/// <summary>
		/// Obsolete. Use ElapsedGameTime
		/// </summary>
		public DateTime GameTime
		{
			get
			{
				return new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(ElapsedGameTime);
			}
			set
			{
				ElapsedGameTime = (value - new DateTime(2081, 1, 1)).Ticks;
			}
		}

		public MyOnlineModeEnum OnlineMode
		{
			get
			{
				return Settings.OnlineMode;
			}
			set
			{
				Settings.OnlineMode = value;
			}
		}

		public bool AutoHealing
		{
			get
			{
				return Settings.AutoHealing;
			}
			set
			{
				Settings.AutoHealing = value;
			}
		}

		public bool EnableCopyPaste
		{
			get
			{
				return Settings.EnableCopyPaste;
			}
			set
			{
				Settings.EnableCopyPaste = value;
			}
		}

		public short MaxPlayers
		{
			get
			{
				return Settings.MaxPlayers;
			}
			set
			{
				Settings.MaxPlayers = value;
			}
		}

		public bool WeaponsEnabled
		{
			get
			{
				return Settings.WeaponsEnabled;
			}
			set
			{
				Settings.WeaponsEnabled = value;
			}
		}

		public bool ShowPlayerNamesOnHud
		{
			get
			{
				return Settings.ShowPlayerNamesOnHud;
			}
			set
			{
				Settings.ShowPlayerNamesOnHud = value;
			}
		}

		public short MaxFloatingObjects
		{
			get
			{
				return Settings.MaxFloatingObjects;
			}
			set
			{
				Settings.MaxFloatingObjects = value;
			}
		}

		public MyGameModeEnum GameMode
		{
			get
			{
				return Settings.GameMode;
			}
			set
			{
				Settings.GameMode = value;
			}
		}

		public float InventorySizeMultiplier
		{
			get
			{
				return Settings.InventorySizeMultiplier;
			}
			set
			{
				Settings.InventorySizeMultiplier = value;
			}
		}

		public float AssemblerSpeedMultiplier
		{
			get
			{
				return Settings.AssemblerSpeedMultiplier;
			}
			set
			{
				Settings.AssemblerSpeedMultiplier = value;
			}
		}

		public float AssemblerEfficiencyMultiplier
		{
			get
			{
				return Settings.AssemblerEfficiencyMultiplier;
			}
			set
			{
				Settings.AssemblerEfficiencyMultiplier = value;
			}
		}

		public float RefinerySpeedMultiplier
		{
			get
			{
				return Settings.RefinerySpeedMultiplier;
			}
			set
			{
				Settings.RefinerySpeedMultiplier = value;
			}
		}

		public bool ThrusterDamage
		{
			get
			{
				return Settings.ThrusterDamage;
			}
			set
			{
				Settings.ThrusterDamage = value;
			}
		}

		public bool CargoShipsEnabled
		{
			get
			{
				return Settings.CargoShipsEnabled;
			}
			set
			{
				Settings.CargoShipsEnabled = value;
			}
		}

		public bool AutoSave
		{
			get
			{
				return Settings.AutoSaveInMinutes != 0;
			}
			set
			{
				Settings.AutoSaveInMinutes = (value ? 5u : 0u);
			}
		}

		public bool ShouldSerializeWorkshopId()
		{
			return WorkshopId.HasValue;
		}

		public bool ShouldSerializeClients()
		{
			if (Clients != null)
			{
				return Clients.Count != 0;
			}
			return false;
		}

		public bool ShouldSerializeAllPlayersColors()
		{
			if (AllPlayersColors != null)
			{
				return AllPlayersColors.Dictionary.Count > 0;
			}
			return false;
		}

		public bool ShouldSerializeWorldBoundaries()
		{
			return WorldBoundaries.HasValue;
		}

		public bool ShouldSerializeInGameTime()
		{
			return InGameTime != DEFAULT_DATE;
		}

		public bool ShouldSerializeGameTime()
		{
			return false;
		}

		public bool ShouldSerializeOnlineMode()
		{
			return false;
		}

		public bool ShouldSerializeAutoHealing()
		{
			return false;
		}

		public bool ShouldSerializeConnectedPlayers()
		{
			return false;
		}

		public bool ShouldSerializeDisconnectedPlayers()
		{
			return false;
		}

		public bool ShouldSerializeEnableCopyPaste()
		{
			return false;
		}

		public bool ShouldSerializeMaxPlayers()
		{
			return false;
		}

		public bool ShouldSerializeWeaponsEnabled()
		{
			return false;
		}

		public bool ShouldSerializeShowPlayerNamesOnHud()
		{
			return false;
		}

		public bool ShouldSerializeMaxFloatingObjects()
		{
			return false;
		}

		public bool ShouldSerializeGameMode()
		{
			return false;
		}

		public bool ShouldSerializeInventorySizeMultiplier()
		{
			return false;
		}

		public bool ShouldSerializeAssemblerSpeedMultiplier()
		{
			return false;
		}

		public bool ShouldSerializeAssemblerEfficiencyMultiplier()
		{
			return false;
		}

		public bool ShouldSerializeRefinerySpeedMultiplier()
		{
			return false;
		}

		public bool ShouldSerializeThrusterDamage()
		{
			return false;
		}

		public bool ShouldSerializeCargoShipsEnabled()
		{
			return false;
		}

		public bool ShouldSerializeAllPlayers()
		{
			return false;
		}

		public bool ShouldSerializeAutoSave()
		{
			return false;
		}
	}
}
