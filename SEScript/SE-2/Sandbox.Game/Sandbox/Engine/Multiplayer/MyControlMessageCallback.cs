using Sandbox.Game.Multiplayer;
using System;
using VRage;
using VRage.Serialization;

namespace Sandbox.Engine.Multiplayer
{
	public class MyControlMessageCallback<TMsg> : IControlMessageCallback<TMsg>, ITransportCallback where TMsg : struct
	{
		private readonly ISerializer<TMsg> m_serializer;

		private readonly ControlMessageHandler<TMsg> m_callback;

		public readonly MyMessagePermissions Permission;

		public MyControlMessageCallback(ControlMessageHandler<TMsg> callback, ISerializer<TMsg> serializer, MyMessagePermissions permission)
		{
			m_callback = callback;
			m_serializer = serializer;
			Permission = permission;
		}

		public void Write(ByteStream destination, ref TMsg msg)
		{
			m_serializer.Serialize(destination, ref msg);
		}

		void ITransportCallback.Receive(ByteStream source, ulong sender)
		{
			if (MySyncLayer.CheckReceivePermissions(sender, Permission))
			{
				TMsg data;
				try
				{
					m_serializer.Deserialize(source, out data);
				}
				catch (Exception innerException)
				{
					MySandboxGame.Log.WriteLine(new Exception($"Error deserializing '{typeof(TMsg).Name}', message size '{source.Length}'", innerException));
					return;
				}
				m_callback(ref data, sender);
			}
		}
	}
}
