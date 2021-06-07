using VRage.Library.Collections;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Network
{
	/// <summary>
	/// Base class for game-defined client state.
	/// It's set of data required by server, sent from client.
	/// E.g. current client area of interest, context (game, terminal, inventory etc...)
	/// Abstract class for performance reasons (often casting)
	/// </summary>
	public abstract class MyClientStateBase
	{
		public MyTimeSpan ClientTimeStamp;

		private short m_ping;

		/// <summary>
		/// Client endpoint, don't serialize it in Serialize()
		/// </summary>
		public Endpoint EndpointId
		{
			get;
			set;
		}

		public int PlayerSerialId
		{
			get;
			set;
		}

		public virtual Vector3D? Position
		{
			get;
			protected set;
		}

		public short Ping
		{
			get
			{
				return m_ping;
			}
			set
			{
				m_ping = value;
			}
		}

		public abstract IMyReplicable ControlledReplicable
		{
			get;
		}

		public abstract IMyReplicable CharacterReplicable
		{
			get;
		}

		public bool IsControllingCharacter
		{
			get;
			protected set;
		}

		public bool IsControllingJetpack
		{
			get;
			protected set;
		}

		public bool IsControllingGrid
		{
			get;
			protected set;
		}

		/// <summary>
		/// Serializes state into/from bit stream.
		/// EndpointId should be ignored.
		/// </summary>
		public abstract void Serialize(BitStream stream, bool outOfOrder);

		public abstract void Update();

		public abstract void ResetControlledEntityControls();
	}
}
