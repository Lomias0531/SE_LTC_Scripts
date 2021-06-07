using ProtoBuf;
using Sandbox.Engine.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MyConfigBase
	{
		[Serializable]
		[ProtoContract]
		[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
		public class MyObjectBuilder_ConfigData
		{
			[Serializable]
			[ProtoContract]
			[XmlInclude(typeof(List<string>))]
			[XmlInclude(typeof(MyConfig.MyDebugInputData))]
			[XmlInclude(typeof(MyObjectBuilder_ServerFilterOptions))]
			[XmlInclude(typeof(SerializableDictionary<string, string>))]
			[XmlInclude(typeof(SerializableDictionary<string, MyConfig.MyDebugInputData>))]
			[XmlInclude(typeof(SerializableDictionary<string, SerializableDictionary<string, string>>))]
			public class InnerData
			{
				protected class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EInnerData_003C_003EValue_003C_003EAccessor : IMemberAccessor<InnerData, object>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref InnerData owner, in object value)
					{
						owner.Value = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref InnerData owner, out object value)
					{
						value = owner.Value;
					}
				}

				private class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EInnerData_003C_003EActor : IActivator, IActivator<InnerData>
				{
					private sealed override object CreateInstance()
					{
						return new InnerData();
					}

					object IActivator.CreateInstance()
					{
						//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
						return this.CreateInstance();
					}

					private sealed override InnerData CreateInstance()
					{
						return new InnerData();
					}

					InnerData IActivator<InnerData>.CreateInstance()
					{
						//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
						return this.CreateInstance();
					}
				}

				[ProtoMember(1)]
				public object Value;
			}

			protected class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ConfigData, SerializableDictionary<string, InnerData>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_ConfigData owner, in SerializableDictionary<string, InnerData> value)
				{
					owner.Data = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_ConfigData owner, out SerializableDictionary<string, InnerData> value)
				{
					value = owner.Data;
				}
			}

			private class Sandbox_Engine_Utils_MyConfigBase_003C_003EMyObjectBuilder_ConfigData_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ConfigData>
			{
				private sealed override object CreateInstance()
				{
					return new MyObjectBuilder_ConfigData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyObjectBuilder_ConfigData CreateInstance()
				{
					return new MyObjectBuilder_ConfigData();
				}

				MyObjectBuilder_ConfigData IActivator<MyObjectBuilder_ConfigData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(4)]
			public SerializableDictionary<string, InnerData> Data = new SerializableDictionary<string, InnerData>();
		}

		public class ConfigData
		{
			public Dictionary<string, object> Dictionary
			{
				get;
				private set;
			} = new Dictionary<string, object>();


			public void Init(MyObjectBuilder_ConfigData ob)
			{
				Dictionary = (ob?.Data?.Dictionary.ToDictionary((KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData> x) => x.Key, (KeyValuePair<string, MyObjectBuilder_ConfigData.InnerData> x) => x.Value.Value) ?? new Dictionary<string, object>());
			}

			public void InitBackCompatibility(Dictionary<string, object> dictionary)
			{
				Dictionary = dictionary;
			}

			public MyObjectBuilder_ConfigData GetObjectBuilder()
			{
				return new MyObjectBuilder_ConfigData
				{
					Data = new SerializableDictionary<string, MyObjectBuilder_ConfigData.InnerData>(Dictionary.ToDictionary((KeyValuePair<string, object> x) => x.Key, (KeyValuePair<string, object> x) => new MyObjectBuilder_ConfigData.InnerData
					{
						Value = x.Value
					}))
				};
			}
		}

		protected readonly ConfigData m_values = new ConfigData();

		private string m_path;

		private XmlSerializer m_serializer;

		private XmlSerializer Serializer
		{
			get
			{
				if (m_serializer == null)
				{
					Type type = Assembly.Load((Attribute.GetCustomAttribute(typeof(MyObjectBuilder_ConfigData), typeof(XmlSerializerAssemblyAttribute)) as XmlSerializerAssemblyAttribute).AssemblyName).GetType("Microsoft.Xml.Serialization.GeneratedAssembly." + typeof(MyObjectBuilder_ConfigData).Name + "Serializer");
					m_serializer = (XmlSerializer)Activator.CreateInstance(type);
				}
				return m_serializer;
			}
		}

		public MyConfigBase(string fileName)
		{
			m_path = Path.Combine(MyFileSystem.UserDataPath, fileName);
		}

		protected string GetParameterValue(string parameterName)
		{
			try
			{
				if (!m_values.Dictionary.TryGetValue(parameterName, out object value))
				{
					return "";
				}
				return (string)value;
			}
			catch
			{
				return "";
			}
		}

		protected SerializableDictionary<string, TValue> GetParameterValueDictionary<TValue>(string parameterName)
		{
			m_values.Dictionary.TryGetValue(parameterName, out object value);
			SerializableDictionary<string, TValue> result;
			if ((result = (value as SerializableDictionary<string, TValue>)) != null)
			{
				return result;
			}
			result = new SerializableDictionary<string, TValue>();
			SerializableDictionary<string, object> serializableDictionary;
			if ((serializableDictionary = (value as SerializableDictionary<string, object>)) != null)
			{
				foreach (KeyValuePair<string, object> item in serializableDictionary.Dictionary)
				{
					object value2;
					object obj;
					if ((value2 = item.Value) is TValue)
					{
						TValue val = (TValue)value2;
						obj = val;
					}
					else
					{
						obj = default(TValue);
					}
					TValue value3 = (TValue)obj;
					result.Dictionary.Add(item.Key, value3);
				}
			}
			m_values.Dictionary[parameterName] = result;
			return result;
		}

		protected T GetParameterValueT<T>(string parameterName)
		{
			try
			{
				if (!m_values.Dictionary.TryGetValue(parameterName, out object value))
				{
					return default(T);
				}
				return (T)value;
			}
			catch
			{
				return default(T);
			}
		}

		protected void SetParameterValue(string parameterName, string value)
		{
			m_values.Dictionary[parameterName] = value;
		}

		protected void SetParameterValue(string parameterName, float value)
		{
			m_values.Dictionary[parameterName] = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
		}

		protected void SetParameterValue(string parameterName, bool? value)
		{
			m_values.Dictionary[parameterName] = (value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture.NumberFormat) : "");
		}

		protected void SetParameterValue(string parameterName, int value)
		{
			m_values.Dictionary[parameterName] = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
		}

		protected void SetParameterValue(string parameterName, int? value)
		{
			m_values.Dictionary[parameterName] = ((!value.HasValue) ? "" : value.Value.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		protected void SetParameterValue(string parameterName, Vector3I value)
		{
			SetParameterValue(parameterName, $"{value.X}, {value.Y}, {value.Z}");
		}

		protected void RemoveParameterValue(string parameterName)
		{
			m_values.Dictionary.Remove(parameterName);
		}

		protected T? GetOptionalEnum<T>(string name) where T : struct, IComparable, IFormattable, IConvertible
		{
			int? intFromString = MyUtils.GetIntFromString(GetParameterValue(name));
			if (intFromString.HasValue && Enum.IsDefined(typeof(T), intFromString.Value))
			{
				return (T)(object)intFromString.Value;
			}
			return null;
		}

		protected void SetOptionalEnum<T>(string name, T? value) where T : struct, IComparable, IFormattable, IConvertible
		{
			if (value.HasValue)
			{
				SetParameterValue(name, (int)(object)value.Value);
			}
			else
			{
				RemoveParameterValue(name);
			}
		}

		public void Save()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySandboxGame.Log.WriteLine("MyConfig.Save() - START");
				MySandboxGame.Log.IncreaseIndent();
				try
				{
					MySandboxGame.Log.WriteLine("Path: " + m_path, LoggingOptions.CONFIG_ACCESS);
					try
					{
						using (Stream output = MyFileSystem.OpenWrite(m_path))
						{
							XmlWriterSettings settings = new XmlWriterSettings
							{
								Indent = true,
								NewLineHandling = NewLineHandling.None
							};
							using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
							{
								Serializer.Serialize(xmlWriter, m_values.GetObjectBuilder());
							}
						}
					}
					catch (Exception arg)
					{
						MySandboxGame.Log.WriteLine("Exception occured, but application is continuing. Exception: " + arg);
					}
				}
				finally
				{
					MySandboxGame.Log.DecreaseIndent();
					MySandboxGame.Log.WriteLine("MyConfig.Save() - END");
				}
			}
		}

		protected virtual void NewConfigWasStarted()
		{
		}

		public void Load()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySandboxGame.Log.WriteLine("MyConfig.Load() - START");
				using (MySandboxGame.Log.IndentUsing(LoggingOptions.CONFIG_ACCESS))
				{
					MySandboxGame.Log.WriteLine("Path: " + m_path, LoggingOptions.CONFIG_ACCESS);
					string msg = "";
					try
					{
						if (!File.Exists(m_path))
						{
							MySandboxGame.Log.WriteLine("Config file not found! " + m_path);
							NewConfigWasStarted();
						}
						else
						{
							using (Stream input = MyFileSystem.OpenRead(m_path))
							{
								using (XmlReader xmlReader = XmlReader.Create(input))
								{
									try
									{
										m_values.Init((MyObjectBuilder_ConfigData)Serializer.Deserialize(xmlReader));
									}
									catch (InvalidOperationException)
									{
										SerializableDictionary<string, object> serializableDictionary = (SerializableDictionary<string, object>)new XmlSerializer(typeof(SerializableDictionary<string, object>), new Type[5]
										{
											typeof(SerializableDictionary<string, string>),
											typeof(List<string>),
											typeof(SerializableDictionary<string, MyConfig.MyDebugInputData>),
											typeof(MyConfig.MyDebugInputData),
											typeof(MyObjectBuilder_ServerFilterOptions)
										}).Deserialize(xmlReader);
										m_values.InitBackCompatibility(serializableDictionary.Dictionary);
									}
								}
							}
						}
					}
					catch (Exception arg)
					{
						MySandboxGame.Log.WriteLine("Exception occured, but application is continuing. Exception: " + arg);
						MySandboxGame.Log.WriteLine("Config:");
						MySandboxGame.Log.WriteLine(msg);
					}
					foreach (KeyValuePair<string, object> item in m_values.Dictionary)
					{
						if (item.Value == null)
						{
							MySandboxGame.Log.WriteLine("ERROR: " + item.Key + " is null!", LoggingOptions.CONFIG_ACCESS);
						}
						else
						{
							MySandboxGame.Log.WriteLine(item.Key + ": " + item.Value.ToString(), LoggingOptions.CONFIG_ACCESS);
						}
					}
				}
				MySandboxGame.Log.WriteLine("MyConfig.Load() - END");
			}
		}
	}
}
