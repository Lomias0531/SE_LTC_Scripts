using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRage.Game.ObjectBuilders.Components;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.Components.Session
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof(MyObjectBuilder_SharedStorageComponent), null)]
	public class MySessionComponentScriptSharedStorage : MySessionComponentBase
	{
		private MyObjectBuilder_SharedStorageComponent m_objectBuilder;

		private static MySessionComponentScriptSharedStorage m_instance;

		public static MySessionComponentScriptSharedStorage Instance => m_instance;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_SharedStorageComponent myObjectBuilder_SharedStorageComponent = sessionComponent as MyObjectBuilder_SharedStorageComponent;
			m_objectBuilder = new MyObjectBuilder_SharedStorageComponent
			{
				BoolStorage = myObjectBuilder_SharedStorageComponent.BoolStorage,
				FloatStorage = myObjectBuilder_SharedStorageComponent.FloatStorage,
				StringStorage = myObjectBuilder_SharedStorageComponent.StringStorage,
				IntStorage = myObjectBuilder_SharedStorageComponent.IntStorage,
				Vector3DStorage = myObjectBuilder_SharedStorageComponent.Vector3DStorage,
				LongStorage = myObjectBuilder_SharedStorageComponent.LongStorage,
				ExistingFieldsAndStaticAttribute = myObjectBuilder_SharedStorageComponent.ExistingFieldsAndStaticAttribute
			};
			m_instance = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_instance = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			return m_objectBuilder;
		}

		public bool Write(string variableName, int value, bool @static = false)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (!m_objectBuilder.IntStorage.Dictionary.ContainsKey(variableName))
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.ContainsKey(variableName))
				{
					return false;
				}
				m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.Add(variableName, @static);
				m_objectBuilder.IntStorage.Dictionary.Add(variableName, value);
			}
			else
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary[variableName])
				{
					return false;
				}
				m_objectBuilder.IntStorage.Dictionary[variableName] = value;
			}
			return true;
		}

		public bool Write(string variableName, long value, bool @static = false)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (!m_objectBuilder.LongStorage.Dictionary.ContainsKey(variableName))
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.ContainsKey(variableName))
				{
					return false;
				}
				m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.Add(variableName, @static);
				m_objectBuilder.LongStorage.Dictionary.Add(variableName, value);
			}
			else
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary[variableName])
				{
					return false;
				}
				m_objectBuilder.LongStorage.Dictionary[variableName] = value;
			}
			return true;
		}

		public bool Write(string variableName, bool value, bool @static = false)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (!m_objectBuilder.BoolStorage.Dictionary.ContainsKey(variableName))
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.ContainsKey(variableName))
				{
					return false;
				}
				m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.Add(variableName, @static);
				m_objectBuilder.BoolStorage.Dictionary.Add(variableName, value);
			}
			else
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary[variableName])
				{
					return false;
				}
				m_objectBuilder.BoolStorage.Dictionary[variableName] = value;
			}
			return true;
		}

		public bool Write(string variableName, float value, bool @static = false)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (!m_objectBuilder.FloatStorage.Dictionary.ContainsKey(variableName))
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.ContainsKey(variableName))
				{
					return false;
				}
				m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.Add(variableName, @static);
				m_objectBuilder.FloatStorage.Dictionary.Add(variableName, value);
			}
			else
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary[variableName])
				{
					return false;
				}
				m_objectBuilder.FloatStorage.Dictionary[variableName] = value;
			}
			return true;
		}

		public bool Write(string variableName, string value, bool @static = false)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (!m_objectBuilder.StringStorage.Dictionary.ContainsKey(variableName))
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.ContainsKey(variableName))
				{
					return false;
				}
				m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.Add(variableName, @static);
				m_objectBuilder.StringStorage.Dictionary.Add(variableName, value);
			}
			else
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary[variableName])
				{
					return false;
				}
				m_objectBuilder.StringStorage.Dictionary[variableName] = value;
			}
			return true;
		}

		public bool Write(string variableName, Vector3D value, bool @static = false)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (!m_objectBuilder.Vector3DStorage.Dictionary.ContainsKey(variableName))
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.ContainsKey(variableName))
				{
					return false;
				}
				m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary.Add(variableName, @static);
				m_objectBuilder.Vector3DStorage.Dictionary.Add(variableName, value);
			}
			else
			{
				if (m_objectBuilder.ExistingFieldsAndStaticAttribute.Dictionary[variableName])
				{
					return false;
				}
				m_objectBuilder.Vector3DStorage.Dictionary[variableName] = value;
			}
			return true;
		}

		public int ReadInt(string variableName)
		{
			if (m_objectBuilder == null)
			{
				return -1;
			}
			if (m_objectBuilder.IntStorage.Dictionary.TryGetValue(variableName, out int value))
			{
				return value;
			}
			return -1;
		}

		public long ReadLong(string variableName)
		{
			if (m_objectBuilder == null)
			{
				return -1L;
			}
			if (m_objectBuilder.LongStorage.Dictionary.TryGetValue(variableName, out long value))
			{
				return value;
			}
			return -1L;
		}

		public float ReadFloat(string variableName)
		{
			if (m_objectBuilder == null)
			{
				return 0f;
			}
			if (m_objectBuilder.FloatStorage.Dictionary.TryGetValue(variableName, out float value))
			{
				return value;
			}
			return 0f;
		}

		public string ReadString(string variableName)
		{
			if (m_objectBuilder == null)
			{
				return null;
			}
			if (m_objectBuilder.StringStorage.Dictionary.TryGetValue(variableName, out string value))
			{
				return value;
			}
			return null;
		}

		public Vector3D ReadVector3D(string variableName)
		{
			if (m_objectBuilder == null)
			{
				return Vector3D.Zero;
			}
			if (m_objectBuilder.Vector3DStorage.Dictionary.TryGetValue(variableName, out SerializableVector3D value))
			{
				return value;
			}
			return Vector3D.Zero;
		}

		public bool ReadBool(string variableName)
		{
			if (m_objectBuilder == null)
			{
				return false;
			}
			if (m_objectBuilder.BoolStorage.Dictionary.TryGetValue(variableName, out bool value))
			{
				return value;
			}
			return false;
		}

		public SerializableDictionary<string, bool> GetExistingFieldsandStaticAttributes()
		{
			return m_objectBuilder.ExistingFieldsAndStaticAttribute;
		}

		public SerializableDictionary<string, bool> GetBools()
		{
			return m_objectBuilder.BoolStorage;
		}

		public SerializableDictionary<string, int> GetInts()
		{
			return m_objectBuilder.IntStorage;
		}

		public SerializableDictionary<string, long> GetLongs()
		{
			return m_objectBuilder.LongStorage;
		}

		public SerializableDictionary<string, string> GetStrings()
		{
			return m_objectBuilder.StringStorage;
		}

		public SerializableDictionary<string, float> GetFloats()
		{
			return m_objectBuilder.FloatStorage;
		}

		public SerializableDictionary<string, SerializableVector3D> GetVector3D()
		{
			return m_objectBuilder.Vector3DStorage;
		}

		public Dictionary<string, bool> GetBoolsByRegex(Regex nameRegex)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (KeyValuePair<string, bool> item in m_objectBuilder.BoolStorage.Dictionary)
			{
				if (nameRegex.IsMatch(item.Key))
				{
					dictionary.Add(item.Key, item.Value);
				}
			}
			return dictionary;
		}
	}
}
