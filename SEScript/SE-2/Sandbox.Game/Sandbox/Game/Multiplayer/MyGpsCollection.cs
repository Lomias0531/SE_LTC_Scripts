using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public class MyGpsCollection : IMyGpsCollection
	{
		[Serializable]
		private struct AddMsg
		{
			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<AddMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in long value)
				{
					owner.IdentityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out long value)
				{
					value = owner.IdentityId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EName_003C_003EAccessor : IMemberAccessor<AddMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<AddMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in string value)
				{
					owner.DisplayName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out string value)
				{
					value = owner.DisplayName;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EDescription_003C_003EAccessor : IMemberAccessor<AddMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in string value)
				{
					owner.Description = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out string value)
				{
					value = owner.Description;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003ECoords_003C_003EAccessor : IMemberAccessor<AddMsg, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in Vector3D value)
				{
					owner.Coords = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out Vector3D value)
				{
					value = owner.Coords;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EShowOnHud_003C_003EAccessor : IMemberAccessor<AddMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in bool value)
				{
					owner.ShowOnHud = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out bool value)
				{
					value = owner.ShowOnHud;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EIsFinal_003C_003EAccessor : IMemberAccessor<AddMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in bool value)
				{
					owner.IsFinal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out bool value)
				{
					value = owner.IsFinal;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EAlwaysVisible_003C_003EAccessor : IMemberAccessor<AddMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in bool value)
				{
					owner.AlwaysVisible = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out bool value)
				{
					value = owner.AlwaysVisible;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<AddMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EContractId_003C_003EAccessor : IMemberAccessor<AddMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in long value)
				{
					owner.ContractId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out long value)
				{
					value = owner.ContractId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EGPSColor_003C_003EAccessor : IMemberAccessor<AddMsg, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in Color value)
				{
					owner.GPSColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out Color value)
				{
					value = owner.GPSColor;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EIsContainerGPS_003C_003EAccessor : IMemberAccessor<AddMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in bool value)
				{
					owner.IsContainerGPS = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out bool value)
				{
					value = owner.IsContainerGPS;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EPlaySoundOnCreation_003C_003EAccessor : IMemberAccessor<AddMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in bool value)
				{
					owner.PlaySoundOnCreation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out bool value)
				{
					value = owner.PlaySoundOnCreation;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg_003C_003EIsObjective_003C_003EAccessor : IMemberAccessor<AddMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddMsg owner, in bool value)
				{
					owner.IsObjective = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddMsg owner, out bool value)
				{
					value = owner.IsObjective;
				}
			}

			public long IdentityId;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string Name;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string DisplayName;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string Description;

			public Vector3D Coords;

			public bool ShowOnHud;

			public bool IsFinal;

			public bool AlwaysVisible;

			public long EntityId;

			public long ContractId;

			public Color GPSColor;

			public bool IsContainerGPS;

			public bool PlaySoundOnCreation;

			public bool IsObjective;
		}

		[Serializable]
		private struct ModifyMsg
		{
			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<ModifyMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in long value)
				{
					owner.IdentityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out long value)
				{
					value = owner.IdentityId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003EHash_003C_003EAccessor : IMemberAccessor<ModifyMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in int value)
				{
					owner.Hash = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out int value)
				{
					value = owner.Hash;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003EName_003C_003EAccessor : IMemberAccessor<ModifyMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003EDescription_003C_003EAccessor : IMemberAccessor<ModifyMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in string value)
				{
					owner.Description = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out string value)
				{
					value = owner.Description;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003ECoords_003C_003EAccessor : IMemberAccessor<ModifyMsg, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in Vector3D value)
				{
					owner.Coords = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out Vector3D value)
				{
					value = owner.Coords;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003EGPSColor_003C_003EAccessor : IMemberAccessor<ModifyMsg, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in Color value)
				{
					owner.GPSColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out Color value)
				{
					value = owner.GPSColor;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg_003C_003EContractId_003C_003EAccessor : IMemberAccessor<ModifyMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ModifyMsg owner, in long value)
				{
					owner.ContractId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ModifyMsg owner, out long value)
				{
					value = owner.ContractId;
				}
			}

			public long IdentityId;

			public int Hash;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string Name;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string Description;

			public Vector3D Coords;

			public Color GPSColor;

			public long ContractId;
		}

		protected sealed class OnAddGps_003C_003ESandbox_Game_Multiplayer_MyGpsCollection_003C_003EAddMsg : ICallSite<IMyEventOwner, AddMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnAddGps(msg);
			}
		}

		protected sealed class DeleteRequest_003C_003ESystem_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DeleteRequest(identityId, gpsHash);
			}
		}

		protected sealed class DeleteSuccess_003C_003ESystem_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DeleteSuccess(identityId, gpsHash);
			}
		}

		protected sealed class ModifyRequest_003C_003ESandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg : ICallSite<IMyEventOwner, ModifyMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ModifyMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ModifyRequest(msg);
			}
		}

		protected sealed class ModifySuccess_003C_003ESandbox_Game_Multiplayer_MyGpsCollection_003C_003EModifyMsg : ICallSite<IMyEventOwner, ModifyMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ModifyMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ModifySuccess(msg);
			}
		}

		protected sealed class ShowOnHudRequest_003C_003ESystem_Int64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in bool show, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowOnHudRequest(identityId, gpsHash, show);
			}
		}

		protected sealed class ShowOnHudSuccess_003C_003ESystem_Int64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in bool show, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowOnHudSuccess(identityId, gpsHash, show);
			}
		}

		protected sealed class AlwaysVisibleRequest_003C_003ESystem_Int64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in bool alwaysVisible, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AlwaysVisibleRequest(identityId, gpsHash, alwaysVisible);
			}
		}

		protected sealed class AlwaysVisibleSuccess_003C_003ESystem_Int64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in bool alwaysVisible, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AlwaysVisibleSuccess(identityId, gpsHash, alwaysVisible);
			}
		}

		protected sealed class ColorRequest_003C_003ESystem_Int64_0023System_Int32_0023VRageMath_Color : ICallSite<IMyEventOwner, long, int, Color, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in Color color, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ColorRequest(identityId, gpsHash, color);
			}
		}

		protected sealed class ColorSuccess_003C_003ESystem_Int64_0023System_Int32_0023VRageMath_Color : ICallSite<IMyEventOwner, long, int, Color, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in int gpsHash, in Color color, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ColorSuccess(identityId, gpsHash, color);
			}
		}

		private Dictionary<long, Dictionary<int, MyGps>> m_playerGpss = new Dictionary<long, Dictionary<int, MyGps>>();

		private StringBuilder m_NamingSearch = new StringBuilder();

		private long lastPlayerId;

		private static readonly int PARSE_MAX_COUNT = 20;

		private static readonly string m_ScanPattern = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):";

		private static readonly string m_ScanPatternExtended = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):(.*)";

		private static List<IMyGps> reusableList = new List<IMyGps>();

		public Dictionary<int, MyGps> this[long id] => m_playerGpss[id];

		public event Action<long> ListChanged;

		public event Action<long, int> GpsChanged;

		public event Action<long, int> GpsAdded;

		public bool ExistsForPlayer(long id)
		{
			Dictionary<int, MyGps> value;
			return m_playerGpss.TryGetValue(id, out value);
		}

		public void SendAddGps(long identityId, ref MyGps gps, long entityId = 0L, bool playSoundOnCreation = true)
		{
			if (identityId != 0L)
			{
				AddMsg arg = default(AddMsg);
				arg.IdentityId = identityId;
				arg.Name = gps.Name;
				arg.DisplayName = gps.DisplayName;
				arg.Description = gps.Description;
				arg.Coords = gps.Coords;
				arg.ShowOnHud = gps.ShowOnHud;
				arg.IsFinal = ((!gps.DiscardAt.HasValue) ? true : false);
				arg.AlwaysVisible = gps.AlwaysVisible;
				arg.EntityId = entityId;
				arg.ContractId = gps.ContractId;
				arg.GPSColor = gps.GPSColor;
				arg.IsContainerGPS = gps.IsContainerGPS;
				arg.PlaySoundOnCreation = playSoundOnCreation;
				arg.IsObjective = gps.IsObjective;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnAddGps, arg);
			}
		}

		[Event(null, 121)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnAddGps(AddMsg msg)
		{
			if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(msg.IdentityId) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				MyEventContext.ValidationFailed();
				return;
			}
			MyGps gps = new MyGps();
			gps.Name = msg.Name;
			gps.DisplayName = msg.DisplayName;
			gps.Description = msg.Description;
			gps.Coords = msg.Coords;
			gps.ShowOnHud = msg.ShowOnHud;
			gps.AlwaysVisible = msg.AlwaysVisible;
			gps.DiscardAt = null;
			gps.GPSColor = msg.GPSColor;
			gps.IsContainerGPS = msg.IsContainerGPS;
			gps.IsObjective = msg.IsObjective;
			gps.ContractId = msg.ContractId;
			if (!msg.IsFinal)
			{
				gps.SetDiscardAt();
			}
			if (msg.EntityId != 0L)
			{
				MyEntity entityById = MyEntities.GetEntityById(msg.EntityId);
				if (entityById != null)
				{
					gps.SetEntity(entityById);
				}
				else
				{
					gps.SetEntityId(msg.EntityId);
				}
			}
			gps.UpdateHash();
			if (MySession.Static.Gpss.AddPlayerGps(msg.IdentityId, ref gps) && gps.ShowOnHud && msg.IdentityId == MySession.Static.LocalPlayerId)
			{
				MyHud.GpsMarkers.RegisterMarker(gps);
				if (msg.PlaySoundOnCreation)
				{
					MyCueId cueId = MySoundPair.GetCueId("HudGPSNotification3");
					MyAudio.Static.PlaySound(cueId);
				}
			}
			MySession.Static.Gpss.ListChanged.InvokeIfNotNull(msg.IdentityId);
			MySession.Static.Gpss.GpsAdded.InvokeIfNotNull(msg.IdentityId, gps.Hash);
		}

		public void SendDelete(long identityId, int gpsHash)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DeleteRequest, identityId, gpsHash);
		}

		[Event(null, 191)]
		[Reliable]
		[Server]
		private static void DeleteRequest(long identityId, int gpsHash)
		{
			Dictionary<int, MyGps> value;
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(identityId) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out value) && value.ContainsKey(gpsHash))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DeleteSuccess, identityId, gpsHash);
			}
		}

		[Event(null, 207)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void DeleteSuccess(long identityId, int gpsHash)
		{
			if (MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value) && value.TryGetValue(gpsHash, out MyGps value2))
			{
				if (value2.ShowOnHud)
				{
					MyHud.GpsMarkers.UnregisterMarker(value2);
				}
				value.Remove(gpsHash);
				value2.Close();
				MySession.Static.Gpss.ListChanged?.Invoke(identityId);
			}
		}

		public void SendModifyGps(long identityId, MyGps gps)
		{
			ModifyMsg arg = default(ModifyMsg);
			arg.IdentityId = identityId;
			arg.Name = gps.Name;
			arg.Description = gps.Description;
			arg.Coords = gps.Coords;
			arg.Hash = gps.Hash;
			arg.GPSColor = gps.GPSColor;
			arg.ContractId = gps.ContractId;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModifyRequest, arg);
		}

		[Event(null, 247)]
		[Reliable]
		[Server]
		private static void ModifyRequest(ModifyMsg msg)
		{
			Dictionary<int, MyGps> value;
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(msg.IdentityId) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MySession.Static.Gpss.m_playerGpss.TryGetValue(msg.IdentityId, out value) && value.ContainsKey(msg.Hash))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModifySuccess, msg);
			}
		}

		[Event(null, 263)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ModifySuccess(ModifyMsg msg)
		{
			if (MySession.Static.Gpss.m_playerGpss.TryGetValue(msg.IdentityId, out Dictionary<int, MyGps> value) && value.TryGetValue(msg.Hash, out MyGps value2))
			{
				value2.Name = msg.Name;
				value2.Description = msg.Description;
				value2.Coords = msg.Coords;
				value2.DiscardAt = null;
				value2.GPSColor = msg.GPSColor;
				value2.ContractId = msg.ContractId;
				MySession.Static.Gpss.GpsChanged?.Invoke(msg.IdentityId, value2.Hash);
				value.Remove(value2.Hash);
				MyHud.GpsMarkers.UnregisterMarker(value2);
				value2.UpdateHash();
				if (value.ContainsKey(value2.Hash))
				{
					value.TryGetValue(value2.Hash, out MyGps value3);
					MyHud.GpsMarkers.UnregisterMarker(value3);
					value.Remove(value2.Hash);
					value.Add(value2.Hash, value2);
					MySession.Static.Gpss.ListChanged?.Invoke(msg.IdentityId);
				}
				else
				{
					value.Add(value2.Hash, value2);
				}
				if (msg.IdentityId == MySession.Static.LocalPlayerId && value2.ShowOnHud)
				{
					MyHud.GpsMarkers.RegisterMarker(value2);
				}
			}
		}

		public void ChangeShowOnHud(long identityId, int gpsHash, bool show)
		{
			SendChangeShowOnHud(identityId, gpsHash, show);
		}

		private void SendChangeShowOnHud(long identityId, int gpsHash, bool show)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowOnHudRequest, identityId, gpsHash, show);
		}

		[Event(null, 323)]
		[Reliable]
		[Server]
		private static void ShowOnHudRequest(long identityId, int gpsHash, bool show)
		{
			Dictionary<int, MyGps> value;
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(identityId) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out value))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowOnHudSuccess, identityId, gpsHash, show);
			}
		}

		[Event(null, 338)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ShowOnHudSuccess(long identityId, int gpsHash, bool show)
		{
			if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value) || !value.TryGetValue(gpsHash, out MyGps value2))
			{
				return;
			}
			value2.ShowOnHud = show;
			if (!show)
			{
				value2.AlwaysVisible = false;
			}
			value2.DiscardAt = null;
			MySession.Static.Gpss.GpsChanged?.Invoke(identityId, gpsHash);
			if (identityId == MySession.Static.LocalPlayerId)
			{
				if (value2.ShowOnHud)
				{
					MyHud.GpsMarkers.RegisterMarker(value2);
				}
				else
				{
					MyHud.GpsMarkers.UnregisterMarker(value2);
				}
			}
		}

		public void ChangeAlwaysVisible(long identityId, int gpsHash, bool alwaysVisible)
		{
			SendChangeAlwaysVisible(identityId, gpsHash, alwaysVisible);
		}

		private void SendChangeAlwaysVisible(long identityId, int gpsHash, bool alwaysVisible)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AlwaysVisibleRequest, identityId, gpsHash, alwaysVisible);
		}

		[Event(null, 381)]
		[Reliable]
		[Server]
		private static void AlwaysVisibleRequest(long identityId, int gpsHash, bool alwaysVisible)
		{
			Dictionary<int, MyGps> value;
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(identityId) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out value))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => AlwaysVisibleSuccess, identityId, gpsHash, alwaysVisible);
			}
		}

		[Event(null, 396)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void AlwaysVisibleSuccess(long identityId, int gpsHash, bool alwaysVisible)
		{
			if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value) || !value.TryGetValue(gpsHash, out MyGps value2))
			{
				return;
			}
			value2.AlwaysVisible = alwaysVisible;
			value2.ShowOnHud |= alwaysVisible;
			value2.DiscardAt = null;
			MySession.Static.Gpss.GpsChanged?.Invoke(identityId, gpsHash);
			if (identityId == MySession.Static.LocalPlayerId)
			{
				if (value2.ShowOnHud)
				{
					MyHud.GpsMarkers.RegisterMarker(value2);
				}
				else
				{
					MyHud.GpsMarkers.UnregisterMarker(value2);
				}
			}
		}

		public void ChangeColor(long identityId, int gpsHash, Color color)
		{
			SendChangeColor(identityId, gpsHash, color);
		}

		private void SendChangeColor(long identityId, int gpsHash, Color color)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ColorRequest, identityId, gpsHash, color);
		}

		[Event(null, 439)]
		[Reliable]
		[Server]
		private static void ColorRequest(long identityId, int gpsHash, Color color)
		{
			Dictionary<int, MyGps> value;
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(identityId) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out value))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ColorSuccess, identityId, gpsHash, color);
			}
		}

		[Event(null, 454)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ColorSuccess(long identityId, int gpsHash, Color color)
		{
			if (MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value) && value.TryGetValue(gpsHash, out MyGps value2))
			{
				value2.GPSColor = color;
				value2.DiscardAt = null;
				MySession.Static.Gpss.GpsChanged?.Invoke(identityId, gpsHash);
			}
		}

		public bool AddPlayerGps(long identityId, ref MyGps gps)
		{
			if (gps == null)
			{
				return false;
			}
			if (!m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value))
			{
				value = new Dictionary<int, MyGps>();
				m_playerGpss.Add(identityId, value);
			}
			if (value.ContainsKey(gps.Hash))
			{
				value.TryGetValue(gps.Hash, out MyGps value2);
				if (value2.DiscardAt.HasValue)
				{
					value2.SetDiscardAt();
				}
				return false;
			}
			value.Add(gps.Hash, gps);
			return true;
		}

		public MyGps GetGps(int hash)
		{
			foreach (Dictionary<int, MyGps> value2 in MySession.Static.Gpss.m_playerGpss.Values)
			{
				if (value2.TryGetValue(hash, out MyGps value))
				{
					return value;
				}
			}
			return null;
		}

		public MyGps GetGpsByEntityId(long identityId, long entityId)
		{
			if (!m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value))
			{
				return null;
			}
			foreach (MyGps value2 in value.Values)
			{
				if (value2.EntityId == entityId)
				{
					return value2;
				}
			}
			return null;
		}

		public MyGps GetGpsByContractId(long identityId, long contractId)
		{
			if (!m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value))
			{
				return null;
			}
			foreach (MyGps value2 in value.Values)
			{
				if (value2.ContractId == contractId)
				{
					return value2;
				}
			}
			return null;
		}

		public void GetNameForNewCurrent(StringBuilder name)
		{
			int num = 0;
			name.Clear().Append(MySession.Static.LocalHumanPlayer.DisplayName).Append(" #");
			if (m_playerGpss.TryGetValue(MySession.Static.LocalPlayerId, out Dictionary<int, MyGps> value))
			{
				foreach (KeyValuePair<int, MyGps> item in value)
				{
					if (item.Value.Name.StartsWith(name.ToString()))
					{
						m_NamingSearch.Clear().Append(item.Value.Name, name.Length, item.Value.Name.Length - name.Length);
						int num2;
						try
						{
							num2 = int.Parse(m_NamingSearch.ToString());
						}
						catch (SystemException)
						{
							continue;
						}
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			num++;
			name.Append(num);
		}

		public void LoadGpss(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (MyFakes.ENABLE_GPS && checkpoint.Gps != null)
			{
				foreach (KeyValuePair<long, MyObjectBuilder_Gps> item in checkpoint.Gps.Dictionary)
				{
					foreach (MyObjectBuilder_Gps.Entry entry in item.Value.Entries)
					{
						MyGps myGps = new MyGps(entry);
						if (!m_playerGpss.TryGetValue(item.Key, out Dictionary<int, MyGps> value))
						{
							value = new Dictionary<int, MyGps>();
							m_playerGpss.Add(item.Key, value);
						}
						if (!value.ContainsKey(myGps.GetHashCode()))
						{
							value.Add(myGps.GetHashCode(), myGps);
							if (myGps.ShowOnHud && item.Key == MySession.Static.LocalPlayerId && MySession.Static.LocalPlayerId != 0L)
							{
								MyHud.GpsMarkers.RegisterMarker(myGps);
							}
						}
					}
				}
			}
		}

		public void updateForHud()
		{
			if (lastPlayerId != MySession.Static.LocalPlayerId)
			{
				if (m_playerGpss.TryGetValue(lastPlayerId, out Dictionary<int, MyGps> value))
				{
					foreach (KeyValuePair<int, MyGps> item in value)
					{
						MyHud.GpsMarkers.UnregisterMarker(item.Value);
					}
				}
				lastPlayerId = MySession.Static.LocalPlayerId;
				if (m_playerGpss.TryGetValue(lastPlayerId, out value))
				{
					foreach (KeyValuePair<int, MyGps> item2 in value)
					{
						if (item2.Value.ShowOnHud)
						{
							MyHud.GpsMarkers.RegisterMarker(item2.Value);
						}
					}
				}
			}
			DiscardOld();
		}

		public void SaveGpss(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (MyFakes.ENABLE_GPS)
			{
				DiscardOld();
				if (checkpoint.Gps == null)
				{
					checkpoint.Gps = new SerializableDictionary<long, MyObjectBuilder_Gps>();
				}
				foreach (KeyValuePair<long, Dictionary<int, MyGps>> item in m_playerGpss)
				{
					if (!checkpoint.Gps.Dictionary.TryGetValue(item.Key, out MyObjectBuilder_Gps value))
					{
						value = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Gps>();
					}
					if (value.Entries == null)
					{
						value.Entries = new List<MyObjectBuilder_Gps.Entry>();
					}
					foreach (KeyValuePair<int, MyGps> item2 in item.Value)
					{
						if (!item2.Value.IsLocal && (!Sync.IsServer || item2.Value.EntityId == 0L || MyEntities.GetEntityById(item2.Value.EntityId) != null))
						{
							value.Entries.Add(GetObjectBuilderEntry(item2.Value));
						}
					}
					checkpoint.Gps.Dictionary.Add(item.Key, value);
				}
			}
		}

		public MyObjectBuilder_Gps.Entry GetObjectBuilderEntry(MyGps gps)
		{
			MyObjectBuilder_Gps.Entry result = default(MyObjectBuilder_Gps.Entry);
			result.name = gps.Name;
			result.description = gps.Description;
			result.coords = gps.Coords;
			result.isFinal = ((!gps.DiscardAt.HasValue) ? true : false);
			result.showOnHud = gps.ShowOnHud;
			result.alwaysVisible = gps.AlwaysVisible;
			result.color = gps.GPSColor;
			result.entityId = gps.EntityId;
			result.contractId = gps.ContractId;
			result.DisplayName = gps.DisplayName;
			result.isObjective = gps.IsObjective;
			return result;
		}

		public void DiscardOld()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<long, Dictionary<int, MyGps>> item in m_playerGpss)
			{
				foreach (KeyValuePair<int, MyGps> item2 in item.Value)
				{
					if (item2.Value.DiscardAt.HasValue && TimeSpan.Compare(MySession.Static.ElapsedPlayTime, item2.Value.DiscardAt.Value) > 0)
					{
						list.Add(item2.Value.Hash);
					}
				}
				foreach (int item3 in list)
				{
					MyGps myGps = item.Value[item3];
					if (myGps.ShowOnHud)
					{
						MyHud.GpsMarkers.UnregisterMarker(myGps);
					}
					item.Value.Remove(item3);
				}
				list.Clear();
			}
		}

		internal void RegisterChat(MyMultiplayerBase multiplayer)
		{
			if (!Sync.IsDedicated && MyFakes.ENABLE_GPS)
			{
				multiplayer.ChatMessageReceived += ParseChat;
			}
		}

		internal void UnregisterChat(MyMultiplayerBase multiplayer)
		{
			if (!Sync.IsDedicated && MyFakes.ENABLE_GPS)
			{
				multiplayer.ChatMessageReceived -= ParseChat;
			}
		}

		private void ParseChat(ulong steamUserId, string messageText, ChatChannel channel, long targetId, string customAuthorName = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_FromChatDescPrefix)).Append(MyMultiplayer.Static.GetMemberName(steamUserId));
			ScanText(messageText, stringBuilder);
		}

		public static bool ParseOneGPS(string input, StringBuilder name, ref Vector3D coords)
		{
			foreach (Match item in Regex.Matches(input, m_ScanPattern))
			{
				double value;
				double value2;
				double value3;
				try
				{
					value = double.Parse(item.Groups[2].Value, CultureInfo.InvariantCulture);
					value = Math.Round(value, 2);
					value2 = double.Parse(item.Groups[3].Value, CultureInfo.InvariantCulture);
					value2 = Math.Round(value2, 2);
					value3 = double.Parse(item.Groups[4].Value, CultureInfo.InvariantCulture);
					value3 = Math.Round(value3, 2);
				}
				catch (SystemException)
				{
					continue;
				}
				name.Clear().Append(item.Groups[1].Value);
				coords.X = value;
				coords.Y = value2;
				coords.Z = value3;
				return true;
			}
			return false;
		}

		public static bool ParseOneGPSExtended(string input, StringBuilder name, ref Vector3D coords, StringBuilder additionalData)
		{
			foreach (Match item in Regex.Matches(input, m_ScanPatternExtended))
			{
				double value;
				double value2;
				double value3;
				try
				{
					value = double.Parse(item.Groups[2].Value, CultureInfo.InvariantCulture);
					value = Math.Round(value, 2);
					value2 = double.Parse(item.Groups[3].Value, CultureInfo.InvariantCulture);
					value2 = Math.Round(value2, 2);
					value3 = double.Parse(item.Groups[4].Value, CultureInfo.InvariantCulture);
					value3 = Math.Round(value3, 2);
				}
				catch (SystemException)
				{
					continue;
				}
				name.Clear().Append(item.Groups[1].Value);
				coords.X = value;
				coords.Y = value2;
				coords.Z = value3;
				additionalData.Clear();
				if (item.Groups.Count == 6 && !string.IsNullOrWhiteSpace(item.Groups[5].Value))
				{
					additionalData.Append(item.Groups[5].Value);
				}
				return true;
			}
			return false;
		}

		public int ScanText(string input, StringBuilder desc)
		{
			return ScanText(input, desc.ToString());
		}

		public int ScanText(string input, string desc = null)
		{
			int num = 0;
			foreach (Match item in Regex.Matches(input, m_ScanPattern))
			{
				string value = item.Groups[1].Value;
				double value2;
				double value3;
				double value4;
				try
				{
					value2 = double.Parse(item.Groups[2].Value, CultureInfo.InvariantCulture);
					value2 = Math.Round(value2, 2);
					value3 = double.Parse(item.Groups[3].Value, CultureInfo.InvariantCulture);
					value3 = Math.Round(value3, 2);
					value4 = double.Parse(item.Groups[4].Value, CultureInfo.InvariantCulture);
					value4 = Math.Round(value4, 2);
				}
				catch (SystemException)
				{
					continue;
				}
				MyGps gps = new MyGps
				{
					Name = value,
					Description = desc,
					Coords = new Vector3D(value2, value3, value4),
					ShowOnHud = false
				};
				gps.UpdateHash();
				MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
				num++;
				if (num == PARSE_MAX_COUNT)
				{
					return num;
				}
			}
			return num;
		}

		IMyGps IMyGpsCollection.Create(string name, string description, Vector3D coords, bool showOnHud, bool temporary)
		{
			MyGps myGps = new MyGps();
			myGps.Name = name;
			myGps.Description = description;
			myGps.Coords = coords;
			myGps.ShowOnHud = showOnHud;
			myGps.GPSColor = new Color(117, 201, 241);
			if (temporary)
			{
				myGps.SetDiscardAt();
			}
			else
			{
				myGps.DiscardAt = null;
			}
			myGps.UpdateHash();
			return myGps;
		}

		List<IMyGps> IMyGpsCollection.GetGpsList(long identityId)
		{
			reusableList.Clear();
			GetGpsList(identityId, reusableList);
			return reusableList;
		}

		public void GetGpsList(long identityId, List<IMyGps> list)
		{
			if (m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value))
			{
				foreach (MyGps value2 in value.Values)
				{
					list.Add(value2);
				}
			}
		}

		public IMyGps GetGpsByName(long identityId, string gpsName)
		{
			if (!m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> value))
			{
				return null;
			}
			foreach (MyGps value2 in value.Values)
			{
				if (value2.Name == gpsName)
				{
					return value2;
				}
			}
			return null;
		}

		void IMyGpsCollection.AddGps(long identityId, IMyGps gps)
		{
			MyGps gps2 = (MyGps)gps;
			SendAddGps(identityId, ref gps2, 0L);
		}

		void IMyGpsCollection.RemoveGps(long identityId, IMyGps gps)
		{
			SendDelete(identityId, (gps as MyGps).Hash);
		}

		void IMyGpsCollection.RemoveGps(long identityId, int gpsHash)
		{
			SendDelete(identityId, gpsHash);
		}

		void IMyGpsCollection.ModifyGps(long identityId, IMyGps gps)
		{
			MyGps gps2 = (MyGps)gps;
			SendModifyGps(identityId, gps2);
		}

		void IMyGpsCollection.SetShowOnHud(long identityId, int gpsHash, bool show)
		{
			SendChangeShowOnHud(identityId, gpsHash, show);
		}

		void IMyGpsCollection.SetShowOnHud(long identityId, IMyGps gps, bool show)
		{
			SendChangeShowOnHud(identityId, (gps as MyGps).Hash, show);
		}

		void IMyGpsCollection.AddLocalGps(IMyGps gps)
		{
			MyGps gps2 = (MyGps)gps;
			gps2.IsLocal = true;
			if (AddPlayerGps(MySession.Static.LocalPlayerId, ref gps2) && gps.ShowOnHud)
			{
				MyHud.GpsMarkers.RegisterMarker(gps2);
			}
		}

		void IMyGpsCollection.RemoveLocalGps(IMyGps gps)
		{
			RemovePlayerGps(gps.Hash);
		}

		void IMyGpsCollection.RemoveLocalGps(int gpsHash)
		{
			RemovePlayerGps(gpsHash);
		}

		private void RemovePlayerGps(int gpsHash)
		{
			if (MySession.Static.Gpss.m_playerGpss.TryGetValue(MySession.Static.LocalPlayerId, out Dictionary<int, MyGps> value) && value.TryGetValue(gpsHash, out MyGps value2))
			{
				if (value2.ShowOnHud)
				{
					MyHud.GpsMarkers.UnregisterMarker(value2);
				}
				value.Remove(gpsHash);
				MySession.Static.Gpss.ListChanged?.Invoke(MySession.Static.LocalPlayerId);
			}
		}
	}
}
