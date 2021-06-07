using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
	public class MyConfigDedicated<T> : IMyConfigDedicated where T : MyObjectBuilder_SessionSettings, new()
	{
		private XmlSerializer m_serializer;

		private string m_fileName;

		private MyConfigDedicatedData<T> m_data;

		public T SessionSettings
		{
			get
			{
				return m_data.SessionSettings;
			}
			set
			{
				m_data.SessionSettings = value;
			}
		}

		public string LoadWorld
		{
			get
			{
				return m_data.LoadWorld;
			}
			set
			{
				m_data.LoadWorld = value;
			}
		}

		public string IP
		{
			get
			{
				return m_data.IP;
			}
			set
			{
				m_data.IP = value;
			}
		}

		public int SteamPort
		{
			get
			{
				return m_data.SteamPort;
			}
			set
			{
				m_data.SteamPort = value;
			}
		}

		public int ServerPort
		{
			get
			{
				return m_data.ServerPort;
			}
			set
			{
				m_data.ServerPort = value;
			}
		}

		public int AsteroidAmount
		{
			get
			{
				return m_data.AsteroidAmount;
			}
			set
			{
				m_data.AsteroidAmount = value;
			}
		}

		public ulong GroupID
		{
			get
			{
				return m_data.GroupID;
			}
			set
			{
				m_data.GroupID = value;
			}
		}

		public List<string> Administrators
		{
			get
			{
				return m_data.Administrators;
			}
			set
			{
				m_data.Administrators = value;
			}
		}

		public List<ulong> Banned
		{
			get
			{
				return m_data.Banned;
			}
			set
			{
				m_data.Banned = value;
			}
		}

		public List<ulong> Reserved
		{
			get
			{
				return m_data.Reserved;
			}
			set
			{
				m_data.Reserved = value;
			}
		}

		public string ServerName
		{
			get
			{
				return m_data.ServerName;
			}
			set
			{
				m_data.ServerName = value;
			}
		}

		public string WorldName
		{
			get
			{
				return m_data.WorldName;
			}
			set
			{
				m_data.WorldName = value;
			}
		}

		public string PremadeCheckpointPath
		{
			get
			{
				return m_data.PremadeCheckpointPath;
			}
			set
			{
				m_data.PremadeCheckpointPath = value;
			}
		}

		public bool PauseGameWhenEmpty
		{
			get
			{
				return m_data.PauseGameWhenEmpty;
			}
			set
			{
				m_data.PauseGameWhenEmpty = value;
			}
		}

		public string MessageOfTheDay
		{
			get
			{
				return m_data.MessageOfTheDay;
			}
			set
			{
				m_data.MessageOfTheDay = value;
			}
		}

		public string MessageOfTheDayUrl
		{
			get
			{
				return m_data.MessageOfTheDayUrl;
			}
			set
			{
				m_data.MessageOfTheDayUrl = value;
			}
		}

		public bool AutoRestartEnabled
		{
			get
			{
				return m_data.AutoRestartEnabled;
			}
			set
			{
				m_data.AutoRestartEnabled = value;
			}
		}

		public int AutoRestatTimeInMin
		{
			get
			{
				return m_data.AutoRestatTimeInMin;
			}
			set
			{
				m_data.AutoRestatTimeInMin = value;
			}
		}

		public bool AutoRestartSave
		{
			get
			{
				return m_data.AutoRestartSave;
			}
			set
			{
				m_data.AutoRestartSave = value;
			}
		}

		public bool AutoUpdateEnabled
		{
			get
			{
				return m_data.AutoUpdateEnabled;
			}
			set
			{
				m_data.AutoUpdateEnabled = value;
			}
		}

		public int AutoUpdateCheckIntervalInMin
		{
			get
			{
				return m_data.AutoUpdateCheckIntervalInMin;
			}
			set
			{
				m_data.AutoUpdateCheckIntervalInMin = value;
			}
		}

		public int AutoUpdateRestartDelayInMin
		{
			get
			{
				return m_data.AutoUpdateRestartDelayInMin;
			}
			set
			{
				m_data.AutoUpdateRestartDelayInMin = value;
			}
		}

		public string AutoUpdateSteamBranch
		{
			get
			{
				return m_data.AutoUpdateSteamBranch;
			}
			set
			{
				m_data.AutoUpdateSteamBranch = value;
			}
		}

		public string AutoUpdateBranchPassword
		{
			get
			{
				return m_data.AutoUpdateBranchPassword;
			}
			set
			{
				m_data.AutoUpdateBranchPassword = value;
			}
		}

		public bool IgnoreLastSession
		{
			get
			{
				return m_data.IgnoreLastSession;
			}
			set
			{
				m_data.IgnoreLastSession = value;
			}
		}

		public string ServerDescription
		{
			get
			{
				return m_data.ServerDescription;
			}
			set
			{
				m_data.ServerDescription = value;
			}
		}

		public string ServerPasswordHash
		{
			get
			{
				return m_data.ServerPasswordHash;
			}
			set
			{
				m_data.ServerPasswordHash = value;
			}
		}

		public string ServerPasswordSalt
		{
			get
			{
				return m_data.ServerPasswordSalt;
			}
			set
			{
				m_data.ServerPasswordSalt = value;
			}
		}

		public bool RemoteApiEnabled
		{
			get
			{
				return m_data.RemoteApiEnabled;
			}
			set
			{
				m_data.RemoteApiEnabled = value;
			}
		}

		public string RemoteSecurityKey
		{
			get
			{
				return m_data.RemoteSecurityKey;
			}
			set
			{
				m_data.RemoteSecurityKey = value;
			}
		}

		public int RemoteApiPort
		{
			get
			{
				return m_data.RemoteApiPort;
			}
			set
			{
				m_data.RemoteApiPort = value;
			}
		}

		public List<string> Plugins
		{
			get
			{
				return m_data.Plugins;
			}
			set
			{
				m_data.Plugins = value;
			}
		}

		public float WatcherInterval
		{
			get
			{
				return m_data.WatcherInterval;
			}
			set
			{
				m_data.WatcherInterval = value;
			}
		}

		public float WatcherSimulationSpeedMinimum
		{
			get
			{
				return m_data.WatcherSimulationSpeedMinimum;
			}
			set
			{
				m_data.WatcherSimulationSpeedMinimum = value;
			}
		}

		public int ManualActionDelay
		{
			get
			{
				return m_data.ManualActionDelay;
			}
			set
			{
				m_data.ManualActionDelay = value;
			}
		}

		public string ManualActionChatMessage
		{
			get
			{
				return m_data.ManualActionChatMessage;
			}
			set
			{
				m_data.ManualActionChatMessage = value;
			}
		}

		public bool AutodetectDependencies
		{
			get
			{
				return m_data.AutodetectDependencies;
			}
			set
			{
				m_data.AutodetectDependencies = value;
			}
		}

		List<string> IMyConfigDedicated.Administrators
		{
			get
			{
				return Administrators;
			}
			set
			{
				Administrators = value;
			}
		}

		int IMyConfigDedicated.AsteroidAmount
		{
			get
			{
				return AsteroidAmount;
			}
			set
			{
				AsteroidAmount = value;
			}
		}

		List<ulong> IMyConfigDedicated.Banned
		{
			get
			{
				return Banned;
			}
			set
			{
				Banned = value;
			}
		}

		List<ulong> IMyConfigDedicated.Reserved
		{
			get
			{
				return Reserved;
			}
			set
			{
				Reserved = value;
			}
		}

		ulong IMyConfigDedicated.GroupID
		{
			get
			{
				return GroupID;
			}
			set
			{
				GroupID = value;
			}
		}

		string IMyConfigDedicated.LoadWorld
		{
			get
			{
				return LoadWorld;
			}
			set
			{
				LoadWorld = value;
			}
		}

		bool IMyConfigDedicated.PauseGameWhenEmpty
		{
			get
			{
				return PauseGameWhenEmpty;
			}
			set
			{
				PauseGameWhenEmpty = value;
			}
		}

		string IMyConfigDedicated.MessageOfTheDay
		{
			get
			{
				return MessageOfTheDay;
			}
			set
			{
				MessageOfTheDay = value;
			}
		}

		string IMyConfigDedicated.MessageOfTheDayUrl
		{
			get
			{
				return MessageOfTheDayUrl;
			}
			set
			{
				MessageOfTheDayUrl = value;
			}
		}

		bool IMyConfigDedicated.AutoRestartEnabled
		{
			get
			{
				return AutoRestartEnabled;
			}
			set
			{
				AutoRestartEnabled = value;
			}
		}

		int IMyConfigDedicated.AutoRestatTimeInMin
		{
			get
			{
				return AutoRestatTimeInMin;
			}
			set
			{
				AutoRestatTimeInMin = value;
			}
		}

		bool IMyConfigDedicated.AutoUpdateEnabled
		{
			get
			{
				return AutoUpdateEnabled;
			}
			set
			{
				AutoUpdateEnabled = value;
			}
		}

		int IMyConfigDedicated.AutoUpdateCheckIntervalInMin
		{
			get
			{
				return AutoUpdateCheckIntervalInMin;
			}
			set
			{
				AutoUpdateCheckIntervalInMin = value;
			}
		}

		int IMyConfigDedicated.AutoUpdateRestartDelayInMin
		{
			get
			{
				return AutoUpdateRestartDelayInMin;
			}
			set
			{
				AutoUpdateRestartDelayInMin = value;
			}
		}

		string IMyConfigDedicated.AutoUpdateSteamBranch
		{
			get
			{
				return AutoUpdateSteamBranch;
			}
			set
			{
				AutoUpdateSteamBranch = value;
			}
		}

		string IMyConfigDedicated.AutoUpdateBranchPassword
		{
			get
			{
				return AutoUpdateBranchPassword;
			}
			set
			{
				AutoUpdateBranchPassword = value;
			}
		}

		bool IMyConfigDedicated.AutoRestartSave
		{
			get
			{
				return AutoRestartSave;
			}
			set
			{
				AutoRestartSave = value;
			}
		}

		string IMyConfigDedicated.ServerName
		{
			get
			{
				return ServerName;
			}
			set
			{
				ServerName = value;
			}
		}

		MyObjectBuilder_SessionSettings IMyConfigDedicated.SessionSettings
		{
			get
			{
				return SessionSettings;
			}
			set
			{
				SessionSettings = (T)value;
			}
		}

		string IMyConfigDedicated.WorldName
		{
			get
			{
				return WorldName;
			}
			set
			{
				WorldName = value;
			}
		}

		string IMyConfigDedicated.ServerPasswordHash
		{
			get
			{
				return ServerPasswordHash;
			}
			set
			{
				ServerPasswordHash = value;
			}
		}

		string IMyConfigDedicated.ServerPasswordSalt
		{
			get
			{
				return ServerPasswordSalt;
			}
			set
			{
				ServerPasswordSalt = value;
			}
		}

		string IMyConfigDedicated.RemoteSecurityKey
		{
			get
			{
				return RemoteSecurityKey;
			}
			set
			{
				RemoteSecurityKey = value;
			}
		}

		bool IMyConfigDedicated.RemoteApiEnabled
		{
			get
			{
				return RemoteApiEnabled;
			}
			set
			{
				RemoteApiEnabled = value;
			}
		}

		int IMyConfigDedicated.RemoteApiPort
		{
			get
			{
				return RemoteApiPort;
			}
			set
			{
				RemoteApiPort = value;
			}
		}

		List<string> IMyConfigDedicated.Plugins
		{
			get
			{
				return Plugins;
			}
			set
			{
				Plugins = value;
			}
		}

		float IMyConfigDedicated.WatcherInterval
		{
			get
			{
				return WatcherInterval;
			}
			set
			{
				WatcherInterval = value;
			}
		}

		float IMyConfigDedicated.WatcherSimulationSpeedMinimum
		{
			get
			{
				return WatcherSimulationSpeedMinimum;
			}
			set
			{
				WatcherSimulationSpeedMinimum = value;
			}
		}

		int IMyConfigDedicated.ManualActionDelay
		{
			get
			{
				return ManualActionDelay;
			}
			set
			{
				ManualActionDelay = value;
			}
		}

		string IMyConfigDedicated.ManualActionChatMessage
		{
			get
			{
				return ManualActionChatMessage;
			}
			set
			{
				ManualActionChatMessage = value;
			}
		}

		bool IMyConfigDedicated.AutodetectDependencies
		{
			get
			{
				return AutodetectDependencies;
			}
			set
			{
				AutodetectDependencies = value;
			}
		}

		public bool SaveChatToLog
		{
			get
			{
				return m_data.SaveChatToLog;
			}
			set
			{
				m_data.SaveChatToLog = value;
			}
		}

		bool IMyConfigDedicated.SaveChatToLog
		{
			get
			{
				return SaveChatToLog;
			}
			set
			{
				SaveChatToLog = value;
			}
		}

		public MyConfigDedicated(string fileName)
		{
			m_fileName = fileName;
			try
			{
				m_serializer = new XmlSerializer(typeof(MyConfigDedicatedData<T>));
			}
			catch (Exception)
			{
			}
			SetDefault();
		}

		private void SetDefault()
		{
			m_data = new MyConfigDedicatedData<T>();
			GenerateSalt();
			GenerateRemoteSecurityKey();
		}

		private void GenerateSalt()
		{
			byte[] inArray;
			RandomNumberGenerator.Create().GetBytes(inArray = new byte[16]);
			ServerPasswordSalt = Convert.ToBase64String(inArray);
		}

		public void Load(string path = null)
		{
			if (string.IsNullOrEmpty(path))
			{
				path = GetFilePath();
			}
			if (!File.Exists(path))
			{
				SetDefault();
				return;
			}
			try
			{
				using (FileStream stream = File.OpenRead(path))
				{
					m_data = (MyConfigDedicatedData<T>)m_serializer.Deserialize(stream);
				}
			}
			catch (Exception ex)
			{
				if (MyLog.Default != null)
				{
					MyLog.Default.WriteLine("Exception during DS config load: " + ex.ToString());
				}
				SetDefault();
				return;
			}
			if (string.IsNullOrEmpty(ServerPasswordSalt) && string.IsNullOrEmpty(ServerPasswordHash))
			{
				GenerateSalt();
			}
		}

		public void Save(string path = null)
		{
			if (string.IsNullOrEmpty(path))
			{
				path = GetFilePath();
			}
			using (FileStream stream = File.Create(path))
			{
				m_serializer.Serialize(stream, m_data);
			}
		}

		public string GetFilePath()
		{
			return Path.Combine(MyFileSystem.UserDataPath, m_fileName);
		}

		string IMyConfigDedicated.GetFilePath()
		{
			return GetFilePath();
		}

		void IMyConfigDedicated.Save(string path = null)
		{
			Save(path);
		}

		public void SetPassword(string password)
		{
			if (string.IsNullOrEmpty(ServerPasswordSalt))
			{
				GenerateSalt();
			}
			byte[] salt = Convert.FromBase64String(ServerPasswordSalt);
			byte[] bytes = new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(20);
			ServerPasswordHash = Convert.ToBase64String(bytes);
		}

		public void GenerateRemoteSecurityKey()
		{
			byte[] inArray;
			RandomNumberGenerator.Create().GetBytes(inArray = new byte[16]);
			RemoteSecurityKey = Convert.ToBase64String(inArray);
		}
	}
}
