using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using System;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
	public class MySyncLayer
	{
		private class DefaultProtoSerializer<T>
		{
			public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>(MyObjectBuilderSerializer.Serializer);
		}

		internal readonly MyTransportLayer TransportLayer;

		internal readonly MyClientCollection Clients;

		internal MySyncLayer(MyTransportLayer transportLayer)
		{
			TransportLayer = transportLayer;
			Clients = new MyClientCollection();
		}

		internal void RegisterClientEvents(MyMultiplayerBase multiplayer)
		{
			multiplayer.ClientJoined += OnClientJoined;
			multiplayer.ClientLeft += OnClientLeft;
			foreach (ulong member in multiplayer.Members)
			{
				if (member != Sync.MyId)
				{
					OnClientJoined(member);
				}
			}
		}

		private void OnClientJoined(ulong steamUserId)
		{
			if (!Clients.HasClient(steamUserId))
			{
				Clients.AddClient(steamUserId);
			}
		}

		private void OnClientLeft(ulong steamUserId, MyChatMemberStateChangeEnum leaveReason)
		{
			Clients.RemoveClient(steamUserId);
		}

		public static bool CheckSendPermissions(ulong target, MyMessagePermissions permission)
		{
			switch (permission)
			{
			case MyMessagePermissions.FromServer | MyMessagePermissions.ToServer:
				return Sync.ServerId == target || Sync.IsServer;
			case MyMessagePermissions.FromServer:
				return Sync.IsServer;
			case MyMessagePermissions.ToServer:
				return Sync.ServerId == target;
			default:
				return false;
			}
		}

		public static bool CheckReceivePermissions(ulong sender, MyMessagePermissions permission)
		{
			switch (permission)
			{
			case MyMessagePermissions.FromServer | MyMessagePermissions.ToServer:
				return Sync.ServerId == sender || Sync.IsServer;
			case MyMessagePermissions.FromServer:
				return Sync.ServerId == sender;
			case MyMessagePermissions.ToServer:
				return Sync.IsServer;
			default:
				return false;
			}
		}

		internal static ISerializer<TMsg> GetSerializer<TMsg>() where TMsg : struct
		{
			if (Attribute.IsDefined(typeof(TMsg), typeof(ProtoContractAttribute)))
			{
				return CreateProto<TMsg>();
			}
			if (BlittableHelper<TMsg>.IsBlittable)
			{
				return (ISerializer<TMsg>)Activator.CreateInstance(typeof(BlitSerializer<>).MakeGenericType(typeof(TMsg)));
			}
			return null;
		}

		private static ISerializer<TMsg> CreateProto<TMsg>()
		{
			return DefaultProtoSerializer<TMsg>.Default;
		}

		private static ISerializer<TMsg> CreateBlittable<TMsg>() where TMsg : unmanaged
		{
			return BlitSerializer<TMsg>.Default;
		}
	}
}
