using System;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Sync
{
	public abstract class SyncBase : IBitSerializable, ISyncType
	{
		public readonly int Id;

		public readonly Type ValueType;

		public readonly MySerializeInfo SerializeInfo;

		public string DebugName;

		public bool ShouldValidate = true;

		/// <summary>
		/// ValueChanged event is raised when value is set locally (settings Value property) or remotely (through deserialization).
		/// When validation fails, value is not changed and ValueChanged is not raised.
		/// </summary>
		public event Action<SyncBase> ValueChanged;

		public event Action<SyncBase> ValueChangedNotify;

		public SyncBase(Type valueType, int id, ISerializerInfo serializeInfo)
		{
			ValueType = valueType;
			Id = id;
			SerializeInfo = (MySerializeInfo)serializeInfo;
		}

		protected void RaiseValueChanged(bool notify)
		{
			this.ValueChanged?.Invoke(this);
			if (notify)
			{
				this.ValueChangedNotify?.Invoke(this);
			}
		}

		public abstract SyncBase Clone(int newId);

		public abstract bool Serialize(BitStream stream, bool validate, bool setValueIfValid = true);

		protected static void CopyValueChanged(SyncBase from, SyncBase to)
		{
			to.ValueChanged = from.ValueChanged;
			to.ValueChangedNotify = from.ValueChangedNotify;
		}

		public static implicit operator BitReaderWriter(SyncBase sync)
		{
			return new BitReaderWriter(sync);
		}

		public void SetDebugName(string debugName)
		{
			DebugName = debugName;
		}
	}
}
