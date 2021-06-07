using System;
using System.IO;

namespace VRage.Game.ModAPI
{
	public interface IMyUtilities
	{
		IMyConfigDedicated ConfigDedicated
		{
			get;
		}

		IMyGamePaths GamePaths
		{
			get;
		}

		bool IsDedicated
		{
			get;
		}

		event MessageEnteredDel MessageEntered;

		event Action<ulong, string> MessageRecieved;

		string GetTypeName(Type type);

		void ShowNotification(string message, int disappearTimeMs = 2000, string font = "White");

		/// <summary>
		/// Create a notification object.
		/// The object needs to have Show() called on it to be shown.
		/// On top of that you can dynamically change the text, font and adjust the time to live.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="disappearTimeMs"></param>
		/// <param name="font"></param>
		/// <returns>The notification object.</returns>
		IMyHudNotification CreateNotification(string message, int disappearTimeMs = 2000, string font = "White");

		void ShowMessage(string sender, string messageText);

		void SendMessage(string messageText);

		bool FileExistsInGlobalStorage(string file);

		bool FileExistsInLocalStorage(string file, Type callingType);

		bool FileExistsInWorldStorage(string file, Type callingType);

		void DeleteFileInGlobalStorage(string file);

		void DeleteFileInLocalStorage(string file, Type callingType);

		void DeleteFileInWorldStorage(string file, Type callingType);

		TextReader ReadFileInGlobalStorage(string file);

		TextReader ReadFileInLocalStorage(string file, Type callingType);

		/// <summary>
		/// Read text file from the current world's Storage directory.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="callingType"></param>
		/// <returns></returns>
		/// <remarks>This directory is under Saves\&lt;SteamId&gt;\&lt;WorldName&gt;\Storage</remarks>
		TextReader ReadFileInWorldStorage(string file, Type callingType);

		TextWriter WriteFileInGlobalStorage(string file);

		TextWriter WriteFileInLocalStorage(string file, Type callingType);

		/// <summary>
		/// Write text file to the current world's Storage directory.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="callingType"></param>
		/// <returns></returns>
		TextWriter WriteFileInWorldStorage(string file, Type callingType);

		string SerializeToXML<T>(T objToSerialize);

		T SerializeFromXML<T>(string buffer);

		/// <summary>
		/// Uses ProtoBuf to serialize an object into a byte array
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		byte[] SerializeToBinary<T>(T obj);

		/// <summary>
		/// Uses ProtoBuf to deserialize an object from a byte array
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		T SerializeFromBinary<T>(byte[] data);

		void InvokeOnGameThread(Action action, string invokerName = "ModAPI");

		void ShowMissionScreen(string screenTitle = null, string currentObjectivePrefix = null, string currentObjective = null, string screenDescription = null, Action<ResultEnum> callback = null, string okButtonCaption = null);

		IMyHudObjectiveLine GetObjectiveLine();

		BinaryReader ReadBinaryFileInGlobalStorage(string file);

		BinaryReader ReadBinaryFileInLocalStorage(string file, Type callingType);

		/// <summary>
		/// Read file from the current world's Storage directory.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="callingType"></param>
		/// <returns></returns>
		BinaryReader ReadBinaryFileInWorldStorage(string file, Type callingType);

		BinaryWriter WriteBinaryFileInGlobalStorage(string file);

		BinaryWriter WriteBinaryFileInLocalStorage(string file, Type callingType);

		/// <summary>
		/// Write file to the current world's Storage directory.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="callingType"></param>
		/// <returns></returns>
		BinaryWriter WriteBinaryFileInWorldStorage(string file, Type callingType);

		void SetVariable<T>(string name, T value);

		bool GetVariable<T>(string name, out T value);

		bool RemoveVariable(string name);

		/// <summary>
		/// Adds a handler to the mod message system.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="messageHandler"></param>
		void RegisterMessageHandler(long id, Action<object> messageHandler);

		/// <summary>
		/// Removes a handler from the mod message system.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="messageHandler"></param>
		void UnregisterMessageHandler(long id, Action<object> messageHandler);

		/// <summary>
		/// Allows passing data between mods on the same client.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="payload"></param>
		void SendModMessage(long id, object payload);
	}
}
