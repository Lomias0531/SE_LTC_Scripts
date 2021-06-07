using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Networking
{
	public class MyLocalCache
	{
		private const string CHECKPOINT_FILE = "Sandbox.sbc";

		private const string WORLD_CONFIGURATION_FILE = "Sandbox_config.sbc";

		private const string LAST_LOADED_TIMES_FILE = "LastLoaded.sbl";

		private const string LAST_SESSION_FILE = "LastSession.sbl";

		private static readonly string activeInventoryFile = "ActiveInventory.sbl";

		private static bool m_initialized;

		public static MyObjectBuilder_LastSession LastSessionOverride;

		public static string LastLoadedTimesPath => Path.Combine(MyFileSystem.SavesPath, "LastLoaded.sbl");

		public static string LastSessionPath => Path.Combine(MyFileSystem.SavesPath, "LastSession.sbl");

		public static string ContentSessionsPath => "Worlds";

		public static string MissionSessionsPath => "Missions";

		public static string AISchoolSessionsPath => "AISchool";

		private static string GetSectorPath(string sessionPath, Vector3I sectorPosition)
		{
			return Path.Combine(sessionPath, GetSectorName(sectorPosition) + ".sbs");
		}

		private static string GetSectorName(Vector3I sectorPosition)
		{
			return string.Format("{0}_{1}_{2}_{3}_", "SANDBOX", sectorPosition.X, sectorPosition.Y, sectorPosition.Z);
		}

		public static string GetSessionSavesPath(string sessionUniqueName, bool contentFolder, bool createIfNotExists = true)
		{
			string text = (!contentFolder) ? Path.Combine(MyFileSystem.SavesPath, sessionUniqueName) : Path.Combine(MyFileSystem.ContentPath, ContentSessionsPath, sessionUniqueName);
			if (createIfNotExists)
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		private static MyWorldInfo LoadWorldInfo(string sessionPath)
		{
			MyWorldInfo myWorldInfo = null;
			try
			{
				XDocument xDocument = null;
				string path = Path.Combine(sessionPath, "Sandbox.sbc");
				if (!File.Exists(path))
				{
					return null;
				}
				myWorldInfo = new MyWorldInfo();
				using (Stream input = MyFileSystem.OpenRead(path).UnwrapGZip())
				{
					XmlReaderSettings settings = new XmlReaderSettings
					{
						CheckCharacters = false
					};
					using (XmlReader reader = XmlReader.Create(input, settings))
					{
						xDocument = XDocument.Load(reader);
					}
				}
				XElement root = xDocument.Root;
				XElement xElement = root.Element("SessionName");
				XElement xElement2 = root.Element("Description");
				XElement xElement3 = root.Element("LastSaveTime");
				root.Element("WorldID");
				XElement xElement4 = root.Element("WorkshopId");
				XElement xElement5 = root.Element("Briefing");
				XElement xElement6 = root.Element("Settings");
				XElement xElement7 = (xElement6 != null) ? root.Element("Settings").Element("ScenarioEditMode") : null;
				XElement xElement8 = (xElement6 != null) ? root.Element("Settings").Element("ExperimentalMode") : null;
				XElement xElement9 = (xElement6 != null) ? root.Element("Settings").Element("HasPlanets") : null;
				if (xElement8 != null)
				{
					bool.TryParse(xElement8.Value, out myWorldInfo.IsExperimental);
				}
				if (xElement != null)
				{
					myWorldInfo.SessionName = MyStatControlText.SubstituteTexts(xElement.Value);
				}
				if (xElement2 != null)
				{
					myWorldInfo.Description = xElement2.Value;
				}
				if (xElement3 != null)
				{
					DateTime.TryParse(xElement3.Value, out myWorldInfo.LastSaveTime);
				}
				if (xElement4 != null && ulong.TryParse(xElement4.Value, out ulong result))
				{
					myWorldInfo.WorkshopId = result;
				}
				if (xElement5 != null)
				{
					myWorldInfo.Briefing = xElement5.Value;
				}
				if (xElement7 != null)
				{
					bool.TryParse(xElement7.Value, out myWorldInfo.ScenarioEditMode);
				}
				if (xElement9 == null)
				{
					return myWorldInfo;
				}
				bool.TryParse(xElement9.Value, out myWorldInfo.HasPlanets);
				return myWorldInfo;
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
				myWorldInfo.IsCorrupted = true;
				return myWorldInfo;
			}
		}

		public static MyObjectBuilder_Checkpoint LoadCheckpoint(string sessionPath, out ulong sizeInBytes)
		{
			sizeInBytes = 0uL;
			string path = Path.Combine(sessionPath, "Sandbox.sbc");
			if (!File.Exists(path))
			{
				return null;
			}
			MyObjectBuilder_Checkpoint objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, out sizeInBytes);
			if (objectBuilder != null && string.IsNullOrEmpty(objectBuilder.SessionName))
			{
				objectBuilder.SessionName = Path.GetFileNameWithoutExtension(path);
			}
			ulong sizeInBytes2 = 0uL;
			MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = LoadWorldConfiguration(sessionPath, out sizeInBytes2);
			if (myObjectBuilder_WorldConfiguration != null)
			{
				MyLog.Default.WriteLineAndConsole("Sandbox world configuration file found, overriding checkpoint settings.");
				objectBuilder.Settings = myObjectBuilder_WorldConfiguration.Settings;
				objectBuilder.Mods = myObjectBuilder_WorldConfiguration.Mods;
				sizeInBytes += sizeInBytes2;
			}
			if (objectBuilder != null)
			{
				CheckExperimental(objectBuilder.Settings);
			}
			return objectBuilder;
		}

		private static void CheckExperimental(MyObjectBuilder_SessionSettings settings)
		{
			if (settings != null && !settings.ExperimentalMode && (settings.IsSettingsExperimental() || (MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Plugins != null && MySandboxGame.ConfigDedicated.Plugins.Count != 0) || (MySandboxGame.Config.ExperimentalMode && MySandboxGame.ConfigDedicated == null)))
			{
				settings.ExperimentalMode = true;
			}
		}

		private static MyObjectBuilder_WorldConfiguration LoadWorldConfiguration(string sessionPath)
		{
			ulong sizeInBytes = 0uL;
			return LoadWorldConfiguration(sessionPath, out sizeInBytes);
		}

		private static MyObjectBuilder_WorldConfiguration LoadWorldConfiguration(string sessionPath, out ulong sizeInBytes)
		{
			string text = Path.Combine(sessionPath, "Sandbox_config.sbc");
			if (!File.Exists(text))
			{
				sizeInBytes = 0uL;
				return null;
			}
			MyLog.Default.WriteLineAndConsole("Loading Sandbox world configuration file " + text);
			MyObjectBuilder_WorldConfiguration objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder, out sizeInBytes);
			return objectBuilder;
		}

		public static MyObjectBuilder_Sector LoadSector(string sessionPath, Vector3I sectorPosition, bool allowXml, out ulong sizeInBytes, out bool needsXml)
		{
			return LoadSector(GetSectorPath(sessionPath, sectorPosition), allowXml, out sizeInBytes, out needsXml);
		}

		private static MyObjectBuilder_Sector LoadSector(string path, bool allowXml, out ulong sizeInBytes, out bool needsXml)
		{
			MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Sector>();
			sizeInBytes = 0uL;
			needsXml = false;
			MyObjectBuilder_Sector objectBuilder = null;
			string path2 = path + "B5";
			if (MyFileSystem.FileExists(path2))
			{
				MyObjectBuilderSerializer.DeserializePB(path2, out objectBuilder, out sizeInBytes);
				if (objectBuilder == null || objectBuilder.SectorObjects == null)
				{
					if (allowXml)
					{
						MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, out sizeInBytes);
						if (objectBuilder != null)
						{
							MyObjectBuilderSerializer.SerializePB(path2, compress: false, objectBuilder);
						}
					}
					else
					{
						needsXml = true;
					}
				}
			}
			else if (allowXml)
			{
				MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, out sizeInBytes);
				if (!MyFileSystem.FileExists(path2))
				{
					MyObjectBuilderSerializer.SerializePB(path + "B5", compress: false, objectBuilder);
				}
			}
			else
			{
				needsXml = true;
			}
			if (objectBuilder == null)
			{
				MySandboxGame.Log.WriteLine("Incorrect save data");
				return null;
			}
			return objectBuilder;
		}

		public static MyObjectBuilder_CubeGrid LoadCubeGrid(string sessionPath, string fileName, out ulong sizeInBytes)
		{
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(sessionPath, fileName), out MyObjectBuilder_CubeGrid objectBuilder, out sizeInBytes);
			if (objectBuilder == null)
			{
				MySandboxGame.Log.WriteLine("Incorrect save data");
				return null;
			}
			return objectBuilder;
		}

		public static bool SaveSector(MyObjectBuilder_Sector sector, string sessionPath, Vector3I sectorPosition, out ulong sizeInBytes)
		{
			string sectorPath = GetSectorPath(sessionPath, sectorPosition);
			bool result = MyObjectBuilderSerializer.SerializeXML(sectorPath, MySandboxGame.Config.CompressSaveGames, sector, out sizeInBytes);
			MyObjectBuilderSerializer.SerializePB(sectorPath + "B5", MySandboxGame.Config.CompressSaveGames, sector, out sizeInBytes);
			return result;
		}

		public static bool SaveCheckpoint(MyObjectBuilder_Checkpoint checkpoint, string sessionPath)
		{
			ulong sizeInBytes;
			return SaveCheckpoint(checkpoint, sessionPath, out sizeInBytes);
		}

		public static bool SaveCheckpoint(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, out ulong sizeInBytes)
		{
			bool num = MyObjectBuilderSerializer.SerializeXML(Path.Combine(sessionPath, "Sandbox.sbc"), MySandboxGame.Config.CompressSaveGames, checkpoint, out sizeInBytes);
			MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_WorldConfiguration>();
			myObjectBuilder_WorldConfiguration.Settings = checkpoint.Settings;
			myObjectBuilder_WorldConfiguration.Mods = checkpoint.Mods;
			ulong sizeInBytes2 = 0uL;
			bool result = num & SaveWorldConfiguration(myObjectBuilder_WorldConfiguration, sessionPath, out sizeInBytes2);
			sizeInBytes += sizeInBytes2;
			return result;
		}

		private static bool SaveWorldConfiguration(MyObjectBuilder_WorldConfiguration configuration, string sessionPath)
		{
			ulong sizeInBytes;
			return SaveWorldConfiguration(configuration, sessionPath, out sizeInBytes);
		}

		private static bool SaveWorldConfiguration(MyObjectBuilder_WorldConfiguration configuration, string sessionPath, out ulong sizeInBytes)
		{
			string text = Path.Combine(sessionPath, "Sandbox_config.sbc");
			MyLog.Default.WriteLineAndConsole("Saving Sandbox world configuration file " + text);
			return MyObjectBuilderSerializer.SerializeXML(text, compress: false, configuration, out sizeInBytes);
		}

		public static bool SaveRespawnShip(MyObjectBuilder_CubeGrid cubegrid, string sessionPath, string fileName, out ulong sizeInBytes)
		{
			return MyObjectBuilderSerializer.SerializeXML(Path.Combine(sessionPath, fileName), MySandboxGame.Config.CompressSaveGames, cubegrid, out sizeInBytes);
		}

		public static List<Tuple<string, MyWorldInfo>> GetAvailableWorldInfos(string customPath = null)
		{
			MySandboxGame.Log.WriteLine("Loading available saves - START");
			List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
			{
				GetWorldInfoFromDirectory(customPath ?? MyFileSystem.SavesPath, result);
			}
			MySandboxGame.Log.WriteLine("Loading available saves - END");
			return result;
		}

		public static List<Tuple<string, MyWorldInfo>> GetAvailableMissionInfos()
		{
			return GetAvailableInfosFromDirectory("mission", MissionSessionsPath);
		}

		public static List<Tuple<string, MyWorldInfo>> GetAvailableAISchoolInfos()
		{
			return GetAvailableInfosFromDirectory("AI school scenarios", AISchoolSessionsPath);
		}

		private static List<Tuple<string, MyWorldInfo>> GetAvailableInfosFromDirectory(string worldCategory, string worldDirectoryPath)
		{
			string str = "Loading available " + worldCategory;
			MySandboxGame.Log.WriteLine(str + " - START");
			List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
			{
				GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, worldDirectoryPath), result);
			}
			MySandboxGame.Log.WriteLine(str + " - END");
			return result;
		}

		public static List<Tuple<string, MyWorldInfo>> GetAvailableTutorialInfos()
		{
			MySandboxGame.Log.WriteLine("Loading available tutorials - START");
			List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
			{
				string path = Path.Combine("Tutorials", "Basic");
				string path2 = Path.Combine("Tutorials", "Intermediate");
				string path3 = Path.Combine("Tutorials", "Advanced");
				string path4 = Path.Combine("Tutorials", "Planetary");
				GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, path), result);
				GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, path2), result);
				GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, path3), result);
				GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, path4), result);
			}
			MySandboxGame.Log.WriteLine("Loading available tutorials - END");
			return result;
		}

		public static void GetWorldInfoFromDirectory(string path, List<Tuple<string, MyWorldInfo>> result)
		{
			bool flag = Directory.Exists(path);
			MySandboxGame.Log.WriteLine($"GetWorldInfoFromDirectory (Exists: {flag}) '{path}'");
			if (!flag)
			{
				return;
			}
			string[] directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
			foreach (string text in directories)
			{
				MyWorldInfo myWorldInfo = LoadWorldInfo(text);
				if (myWorldInfo != null && string.IsNullOrEmpty(myWorldInfo.SessionName))
				{
					myWorldInfo.SessionName = Path.GetFileName(text);
				}
				result.Add(Tuple.Create(text, myWorldInfo));
			}
		}

		public static string GetLastSessionPath()
		{
			return CheckLastSession(GetLastSession());
		}

		private static string CheckLastSession(MyObjectBuilder_LastSession lastSession)
		{
			if (lastSession == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(lastSession.Path))
			{
				string text = Path.Combine(lastSession.IsContentWorlds ? MyFileSystem.ContentPath : MyFileSystem.SavesPath, lastSession.Path);
				if (Directory.Exists(text))
				{
					return text;
				}
			}
			return null;
		}

		public static MyObjectBuilder_LastSession GetLastSession()
		{
			if (LastSessionOverride != null && CheckLastSession(LastSessionOverride) != null)
			{
				return LastSessionOverride;
			}
			if (!File.Exists(LastSessionPath))
			{
				return null;
			}
			MyObjectBuilder_LastSession objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(LastSessionPath, out objectBuilder);
			return objectBuilder;
		}

		public static bool SaveLastSessionInfo(string sessionPath, bool isOnline, bool isLobby, string gameName, string serverIP, int serverPort)
		{
			MyObjectBuilder_LastSession myObjectBuilder_LastSession = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_LastSession>();
			myObjectBuilder_LastSession.IsOnline = isOnline;
			myObjectBuilder_LastSession.IsLobby = isLobby;
			if (isOnline)
			{
				if (isLobby)
				{
					myObjectBuilder_LastSession.GameName = gameName;
					myObjectBuilder_LastSession.ServerIP = serverIP;
				}
				else
				{
					myObjectBuilder_LastSession.GameName = gameName;
					myObjectBuilder_LastSession.ServerIP = serverIP;
					myObjectBuilder_LastSession.ServerPort = serverPort;
				}
			}
			else if (sessionPath != null)
			{
				myObjectBuilder_LastSession.Path = sessionPath;
				myObjectBuilder_LastSession.GameName = gameName;
				myObjectBuilder_LastSession.IsContentWorlds = sessionPath.StartsWith(MyFileSystem.ContentPath, StringComparison.InvariantCultureIgnoreCase);
			}
			ulong sizeInBytes;
			return MyObjectBuilderSerializer.SerializeXML(LastSessionPath, compress: false, myObjectBuilder_LastSession, out sizeInBytes);
		}

		public static void ClearLastSessionInfo()
		{
			string path = Path.Combine(MyFileSystem.SavesPath, "LastSession.sbl");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public static void LoadInventoryConfig(MyCharacter character, bool setModel = true, bool setColor = true)
		{
			if (character == null)
			{
				throw new ArgumentNullException("character");
			}
			if (!MyGameService.IsActive)
			{
				return;
			}
			string path = Path.Combine(MyFileSystem.SavesPath, activeInventoryFile);
			if (!MyFileSystem.FileExists(path))
			{
				ResetAllInventorySlots(character);
				return;
			}
			if (!MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_SkinInventory objectBuilder))
			{
				ResetAllInventorySlots(character);
				return;
			}
			if (objectBuilder.Character != null && MyGameService.InventoryItems != null)
			{
				List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
				List<MyGameInventoryItemSlot> list2 = Enum.GetValues(typeof(MyGameInventoryItemSlot)).Cast<MyGameInventoryItemSlot>().ToList();
				list2.Remove(MyGameInventoryItemSlot.None);
				foreach (ulong itemId in objectBuilder.Character)
				{
					MyGameInventoryItem myGameInventoryItem = MyGameService.InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == itemId);
					if (myGameInventoryItem != null)
					{
						myGameInventoryItem.IsInUse = true;
						list.Add(myGameInventoryItem);
						list2.Remove(myGameInventoryItem.ItemDefinition.ItemSlot);
					}
				}
				if (character.Components.TryGet(out MyAssetModifierComponent comp))
				{
					MyGameService.GetItemsCheckData(list, delegate(byte[] checkDataResult)
					{
						comp.TryAddAssetModifier(checkDataResult);
					});
					foreach (MyGameInventoryItemSlot item in list2)
					{
						comp.ResetSlot(item);
					}
				}
			}
			else
			{
				ResetAllInventorySlots(character);
			}
			if (setModel && !string.IsNullOrEmpty(objectBuilder.Model))
			{
				character.ModelName = objectBuilder.Model;
			}
			if (setColor)
			{
				character.ColorMask = objectBuilder.Color;
			}
		}

		public static void ResetAllInventorySlots(MyCharacter character)
		{
			if (character.Components.TryGet(out MyAssetModifierComponent component))
			{
				foreach (MyGameInventoryItemSlot value in Enum.GetValues(typeof(MyGameInventoryItemSlot)))
				{
					if (value != 0)
					{
						component.ResetSlot(value);
					}
				}
			}
		}

		public static void LoadInventoryConfig(MyEntity toolEntity, MyAssetModifierComponent skinComponent)
		{
			if (toolEntity == null)
			{
				throw new ArgumentNullException("toolEntity");
			}
			if (skinComponent == null)
			{
				throw new ArgumentNullException("skinComponent");
			}
			if (!MyGameService.IsActive)
			{
				return;
			}
			string path = Path.Combine(MyFileSystem.SavesPath, activeInventoryFile);
			if (MyFileSystem.FileExists(path) && MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_SkinInventory objectBuilder) && objectBuilder.Tools != null && MyGameService.InventoryItems != null)
			{
				IMyHandheldGunObject<MyDeviceBase> myHandheldGunObject = toolEntity as IMyHandheldGunObject<MyDeviceBase>;
				MyPhysicalItemDefinition physicalItemDefinition = myHandheldGunObject.PhysicalItemDefinition;
				MyGameInventoryItemSlot myGameInventoryItemSlot = MyGameInventoryItemSlot.None;
				if (myHandheldGunObject is MyHandDrill)
				{
					myGameInventoryItemSlot = MyGameInventoryItemSlot.Drill;
				}
				else if (myHandheldGunObject is MyAutomaticRifleGun)
				{
					myGameInventoryItemSlot = MyGameInventoryItemSlot.Rifle;
				}
				else if (myHandheldGunObject is MyWelder)
				{
					myGameInventoryItemSlot = MyGameInventoryItemSlot.Welder;
				}
				else if (myHandheldGunObject is MyAngleGrinder)
				{
					myGameInventoryItemSlot = MyGameInventoryItemSlot.Grinder;
				}
				if (myGameInventoryItemSlot != 0)
				{
					List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
					foreach (ulong itemId in objectBuilder.Tools)
					{
						MyGameInventoryItem myGameInventoryItem = MyGameService.InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == itemId);
						if (myGameInventoryItem != null && physicalItemDefinition != null && (physicalItemDefinition == null || myGameInventoryItem.ItemDefinition.ItemSlot == myGameInventoryItemSlot))
						{
							myGameInventoryItem.IsInUse = true;
							list.Add(myGameInventoryItem);
						}
					}
					MyGameService.GetItemsCheckData(list, delegate(byte[] checkDataResult)
					{
						skinComponent.TryAddAssetModifier(checkDataResult);
					});
				}
			}
		}

		public static bool GetCharacterInfoFromInventoryConfig(ref string model, ref Color color)
		{
			if (!MyGameService.IsActive)
			{
				return false;
			}
			string path = Path.Combine(MyFileSystem.SavesPath, activeInventoryFile);
			if (!MyFileSystem.FileExists(path))
			{
				return false;
			}
			if (!MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_SkinInventory objectBuilder))
			{
				return false;
			}
			model = objectBuilder.Model;
			color = new Color(objectBuilder.Color.X, objectBuilder.Color.Y, objectBuilder.Color.Z);
			return true;
		}

		public static void SaveInventoryConfig(MyCharacter character)
		{
			if (character != null && MyGameService.IsActive)
			{
				MyObjectBuilder_SkinInventory myObjectBuilder_SkinInventory = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SkinInventory>();
				myObjectBuilder_SkinInventory.Character = new List<ulong>();
				myObjectBuilder_SkinInventory.Color = character.ColorMask;
				myObjectBuilder_SkinInventory.Model = character.ModelName;
				myObjectBuilder_SkinInventory.Tools = new List<ulong>();
				if (MyGameService.InventoryItems != null)
				{
					foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
					{
						if (inventoryItem.IsInUse)
						{
							switch (inventoryItem.ItemDefinition.ItemSlot)
							{
							case MyGameInventoryItemSlot.None:
							case MyGameInventoryItemSlot.Face:
							case MyGameInventoryItemSlot.Helmet:
							case MyGameInventoryItemSlot.Gloves:
							case MyGameInventoryItemSlot.Boots:
							case MyGameInventoryItemSlot.Suit:
								myObjectBuilder_SkinInventory.Character.Add(inventoryItem.ID);
								break;
							case MyGameInventoryItemSlot.Rifle:
							case MyGameInventoryItemSlot.Welder:
							case MyGameInventoryItemSlot.Grinder:
							case MyGameInventoryItemSlot.Drill:
								myObjectBuilder_SkinInventory.Tools.Add(inventoryItem.ID);
								break;
							}
						}
					}
				}
				MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.SavesPath, activeInventoryFile), compress: false, myObjectBuilder_SkinInventory, out ulong _);
			}
		}
	}
}
