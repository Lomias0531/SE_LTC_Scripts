using VRage.Library.Collections;
using VRage.Network;
using VRage.Replication;
using VRage.Serialization;

namespace VRage.Sync
{
	/// <summary>
	/// Implementation of synchronized property value.
	/// </summary>
	/// <typeparam name="T">property type</typeparam>
	/// <typeparam name="TSyncDirection">Allowed synchronization direction, can be SyncDirection.BothWays or SyncDirection.FromServer.</typeparam>
	public class Sync<T, TSyncDirection> : SyncBase where TSyncDirection : SyncDirection
	{
		/// <summary>
		/// Static instance of type serializer for this sync property.
		/// </summary>
		public static readonly MySerializer<T> TypeSerializer = MyFactory.GetSerializer<T>();

		/// <summary>
		/// Local value storage.
		/// </summary>
		private T m_value;

		/// <summary>
		/// Validate handler is raised on server after deserialization.
		/// </summary>
		public SyncValidate<T> Validate;

		/// <summary>
		/// Value setter/getter.
		/// </summary>
		public T Value
		{
			get
			{
				return m_value;
			}
			set
			{
				SetValue(ref value, validate: false);
			}
		}

		private bool IsServer
		{
			get
			{
				if (MyMultiplayerMinimalBase.Instance != null)
				{
					return MyMultiplayerMinimalBase.Instance.IsServer;
				}
				return true;
			}
		}

		/// <summary>
		/// Only synchronizes with server/client if enabled.
		/// </summary>
		public bool Enabled
		{
			get;
			set;
		}

		public Sync(int id, ISerializerInfo serializeInfo)
			: base(typeof(T), id, serializeInfo)
		{
			Enabled = true;
		}

		/// <summary>
		/// Convert value of this sync property to string.
		/// </summary>
		public override string ToString()
		{
			return Value.ToString();
		}

		/// <summary>
		/// Validate the given external value.
		/// </summary>
		/// <param name="value">external value of the same type as this sync property</param>
		/// <returns>true if valid</returns>
		private bool IsValid(ref T value)
		{
			return Validate?.Invoke(value) ?? true;
		}

		/// <summary>
		/// Set the value of this sync property.
		/// </summary>
		/// <param name="newValue">New value to be assigned to this instance</param>
		/// <param name="validate">Validate the new value</param>
		/// <param name="ignoreSyncDirection">If true, value arrived from server (pass SyncDirection test).</param>
		/// <returns>false if the value was not set</returns>
		private bool SetValue(ref T newValue, bool validate, bool ignoreSyncDirection = false, bool received = false)
		{
			if (!ignoreSyncDirection && MyMultiplayerMinimalBase.Instance != null && !MyMultiplayerMinimalBase.Instance.IsServer && typeof(TSyncDirection) == typeof(SyncDirection.FromServer))
			{
				return false;
			}
			if (TypeSerializer.Equals(ref m_value, ref newValue))
			{
				return true;
			}
			if (!validate || IsValid(ref newValue))
			{
				m_value = newValue;
				RaiseValueChanged(Enabled && (!received || IsServer));
				return true;
			}
			return false;
		}

		/// <summary>
		/// Validates the value and sets it (when valid).
		/// </summary>
		public void ValidateAndSet(T newValue)
		{
			SetValue(ref newValue, validate: true);
		}

		/// <summary>
		/// Sets new value only locally if on client. USE ONLY WITH VALUES YOU GOT FROM THE SERVER VIA OTHER CHANNELS! (e.g. when initializing from object builder)
		/// Behaves like regular set on the server
		/// </summary>
		public void SetLocalValue(T newValue)
		{
			if (MyMultiplayerMinimalBase.Instance != null && !MyMultiplayerMinimalBase.Instance.IsServer)
			{
				SetValue(ref newValue, validate: false, ignoreSyncDirection: true, received: true);
			}
			else
			{
				SetValue(ref newValue, validate: false);
			}
		}

		public override SyncBase Clone(int newId)
		{
			Sync<T, TSyncDirection> sync = new Sync<T, TSyncDirection>(newId, SerializeInfo);
			SyncBase.CopyValueChanged(this, sync);
			sync.Validate = Validate;
			sync.m_value = m_value;
			return sync;
		}

		public override bool Serialize(BitStream stream, bool validate, bool setValueIfValid = true)
		{
			if (stream.Reading)
			{
				MySerializer.CreateAndRead(stream, out T value, SerializeInfo);
				if (setValueIfValid)
				{
					return SetValue(ref value, validate, ignoreSyncDirection: true, received: true);
				}
				return false;
			}
			MySerializer.Write(stream, ref m_value, SerializeInfo);
			return true;
		}

		public static implicit operator T(Sync<T, TSyncDirection> sync)
		{
			return sync.Value;
		}
	}
}
