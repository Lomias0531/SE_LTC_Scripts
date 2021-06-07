namespace VRage.Sync
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using VRage.Library.Collections;
    using VRage.Network;
    using VRage.Replication;
    using VRage.Serialization;

    public class Sync<T, TSyncDirection> : SyncBase where TSyncDirection: SyncDirection
    {
        public static readonly MySerializer<T> TypeSerializer;
        private T m_value;
        public SyncValidate<T> Validate;

        static Sync()
        {
            VRage.Sync.Sync<T, TSyncDirection>.TypeSerializer = MyFactory.GetSerializer<T>();
        }

        public Sync(int id, ISerializerInfo serializeInfo) : base((Type) typeof(T), id, serializeInfo)
        {
            this.Enabled = true;
        }

        public override SyncBase Clone(int newId)
        {
            VRage.Sync.Sync<T, TSyncDirection> to = new VRage.Sync.Sync<T, TSyncDirection>(newId, base.SerializeInfo);
            CopyValueChanged(this, to);
            to.Validate = this.Validate;
            to.m_value = this.m_value;
            return to;
        }

        private bool IsValid(ref T value)
        {
            SyncValidate<T> validate = this.Validate;
            if (validate != null)
            {
                return validate(value);
            }
            return true;
        }

        public static implicit operator T(VRage.Sync.Sync<T, TSyncDirection> sync) => 
            sync.Value;

        public override bool Serialize(BitStream stream, bool validate, bool setValueIfValid = true)
        {
            if (stream.Reading)
            {
                MySerializer.CreateAndRead<T>(stream, out T local, base.SerializeInfo);
                return (setValueIfValid && this.SetValue(ref local, validate, true, true));
            }
            MySerializer.Write<T>(stream, ref this.m_value, base.SerializeInfo);
            return true;
        }

        public void SetLocalValue(T newValue)
        {
            if ((MyMultiplayerMinimalBase.Instance != null) && !MyMultiplayerMinimalBase.Instance.IsServer)
            {
                this.SetValue(ref newValue, false, true, true);
            }
            else
            {
                this.SetValue(ref newValue, false, false, false);
            }
        }

        private bool SetValue(ref T newValue, bool validate, bool ignoreSyncDirection = false, bool received = false)
        {
            if ((ignoreSyncDirection || (MyMultiplayerMinimalBase.Instance == null)) || (MyMultiplayerMinimalBase.Instance.IsServer || (typeof(TSyncDirection) != typeof(SyncDirection.FromServer))))
            {
                if (VRage.Sync.Sync<T, TSyncDirection>.TypeSerializer.Equals(ref this.m_value, ref newValue))
                {
                    return true;
                }
                if (!validate || this.IsValid(ref newValue))
                {
                    this.m_value = newValue;
                    base.RaiseValueChanged(this.Enabled && (!received || this.IsServer));
                    return true;
                }
            }
            return false;
        }

        public override string ToString() => 
            this.Value.ToString();

        public void ValidateAndSet(T newValue)
        {
            this.SetValue(ref newValue, true, false, false);
        }

        public T Value
        {
            get => 
                this.m_value;
            set => 
                this.SetValue(ref value, false, false, false);
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

        public bool Enabled { get; set; }
    }
}

