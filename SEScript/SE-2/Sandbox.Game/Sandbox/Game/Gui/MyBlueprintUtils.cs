using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyBlueprintUtils
	{
		public static readonly string THUMB_IMAGE_NAME = "thumb.png";

		public static readonly string DEFAULT_SCRIPT_NAME = "Script";

		public static readonly string SCRIPT_EXTENSION = ".cs";

		public static readonly string BLUEPRINT_WORKSHOP_EXTENSION = ".sbb";

		public static readonly string BLUEPRINT_LOCAL_NAME = "bp.sbc";

		public static readonly string STEAM_THUMBNAIL_NAME = "Textures\\GUI\\Icons\\IngameProgrammingIcon.png";

		public static readonly string BLUEPRINT_CLOUD_DIRECTORY = "Blueprints/cloud";

		public static readonly string SCRIPTS_DIRECTORY = "IngameScripts";

		public static readonly string BLUEPRINT_DIRECTORY = "Blueprints";

		public static readonly string BLUEPRINT_DEFAULT_DIRECTORY = Path.Combine(MyFileSystem.ContentPath, "Data", "Blueprints");

		public static readonly string SCRIPT_FOLDER_LOCAL = Path.Combine(MyFileSystem.UserDataPath, SCRIPTS_DIRECTORY, "local");

		public static readonly string SCRIPT_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, SCRIPTS_DIRECTORY, "workshop");

		public static readonly string BLUEPRINT_FOLDER_LOCAL = Path.Combine(MyFileSystem.UserDataPath, BLUEPRINT_DIRECTORY, "local");

		public static readonly string BLUEPRINT_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, BLUEPRINT_DIRECTORY, "workshop");

		public static readonly string BLUEPRINT_WORKSHOP_TEMP = Path.Combine(BLUEPRINT_FOLDER_WORKSHOP, "temp");

		public static MyObjectBuilder_Definitions LoadPrefab(string filePath)
		{
			MyObjectBuilder_Definitions objectBuilder = null;
			bool flag = false;
			string path = filePath + "B5";
			if (MyFileSystem.FileExists(path))
			{
				flag = MyObjectBuilderSerializer.DeserializePB(path, out objectBuilder);
				if (objectBuilder == null || objectBuilder.ShipBlueprints == null)
				{
					flag = MyObjectBuilderSerializer.DeserializeXML(filePath, out objectBuilder);
					if (objectBuilder != null)
					{
						MyObjectBuilderSerializer.SerializePB(path, compress: false, objectBuilder);
					}
				}
			}
			else if (MyFileSystem.FileExists(filePath))
			{
				flag = MyObjectBuilderSerializer.DeserializeXML(filePath, out objectBuilder);
				if (flag)
				{
					MyObjectBuilderSerializer.SerializePB(path, compress: false, objectBuilder);
				}
			}
			if (!flag)
			{
				return null;
			}
			return objectBuilder;
		}

		public static MyObjectBuilder_Definitions LoadPrefabFromCloud(MyBlueprintItemInfo info)
		{
			MyObjectBuilder_Definitions objectBuilder = null;
			if (!string.IsNullOrEmpty(info.CloudPathPB))
			{
				byte[] array = MyGameService.LoadFromCloud(info.CloudPathPB);
				if (array != null)
				{
					using (MemoryStream reader = new MemoryStream(array))
					{
						MyObjectBuilderSerializer.DeserializePB(reader, out objectBuilder);
						return objectBuilder;
					}
				}
			}
			else if (!string.IsNullOrEmpty(info.CloudPathXML))
			{
				byte[] array2 = MyGameService.LoadFromCloud(info.CloudPathXML);
				if (array2 != null)
				{
					using (MemoryStream stream = new MemoryStream(array2))
					{
						using (Stream reader2 = stream.UnwrapGZip())
						{
							MyObjectBuilderSerializer.DeserializeXML(reader2, out objectBuilder);
							return objectBuilder;
						}
					}
				}
			}
			return objectBuilder;
		}

		public static bool CopyFileFromCloud(string pathFull, string pathRel)
		{
			byte[] array = MyGameService.LoadFromCloud(pathRel);
			if (array == null)
			{
				return false;
			}
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				memoryStream.Seek(0L, SeekOrigin.Begin);
				MyFileSystem.CreateDirectoryRecursive(Path.GetDirectoryName(pathFull));
				using (FileStream fileStream = new FileStream(pathFull, FileMode.OpenOrCreate))
				{
					memoryStream.CopyTo(fileStream);
					fileStream.Flush();
				}
			}
			return true;
		}

		public static MyObjectBuilder_Definitions LoadWorkshopPrefab(string archive, ulong? publishedItemId, bool isOldBlueprintScreen)
		{
			if ((!File.Exists(archive) && !MyFileSystem.DirectoryExists(archive)) || !publishedItemId.HasValue)
			{
				return null;
			}
			MyWorkshopItem myWorkshopItem;
			if (isOldBlueprintScreen)
			{
				myWorkshopItem = MyGuiBlueprintScreen.m_subscribedItemsList.Find((MyWorkshopItem item) => item.Id == publishedItemId);
			}
			else
			{
				using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireSharedUsing())
				{
					myWorkshopItem = MyGuiBlueprintScreen_Reworked.GetSubscribedItemsList(Content.Blueprint).Find((MyWorkshopItem item) => item.Id == publishedItemId);
				}
			}
			if (myWorkshopItem == null)
			{
				return null;
			}
			string text = Path.Combine(archive, BLUEPRINT_LOCAL_NAME);
			string text2 = text + "B5";
			if (!MyFileSystem.FileExists(text2) && publishedItemId.HasValue)
			{
				string text3 = Path.Combine(BLUEPRINT_WORKSHOP_TEMP, publishedItemId.Value.ToString());
				MyFileSystem.EnsureDirectoryExists(text3);
				text2 = Path.Combine(text3, BLUEPRINT_LOCAL_NAME);
				text2 += "B5";
			}
			bool flag = false;
			MyObjectBuilder_Definitions objectBuilder = null;
			bool num = MyFileSystem.FileExists(text2);
			bool flag2 = MyFileSystem.FileExists(text);
			bool flag3 = false;
			if (num && flag2)
			{
				FileInfo fileInfo = new FileInfo(text2);
				FileInfo fileInfo2 = new FileInfo(text);
				if (fileInfo.LastWriteTimeUtc >= fileInfo2.LastWriteTimeUtc)
				{
					flag3 = true;
				}
			}
			if (flag3)
			{
				flag = MyObjectBuilderSerializer.DeserializePB(text2, out objectBuilder);
				if (objectBuilder == null || objectBuilder.ShipBlueprints == null)
				{
					flag = MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder);
				}
			}
			else if (flag2)
			{
				flag = MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder);
				if (flag && publishedItemId.HasValue)
				{
					MyObjectBuilderSerializer.SerializePB(text2, compress: false, objectBuilder);
				}
			}
			if (flag)
			{
				objectBuilder.ShipBlueprints[0].Description = myWorkshopItem.Description;
				objectBuilder.ShipBlueprints[0].CubeGrids[0].DisplayName = myWorkshopItem.Title;
				objectBuilder.ShipBlueprints[0].DLCs = new string[myWorkshopItem.DLCs.Count];
				for (int i = 0; i < myWorkshopItem.DLCs.Count; i++)
				{
					if (MyDLCs.TryGetDLC(myWorkshopItem.DLCs[i], out MyDLCs.MyDLC dlc))
					{
						objectBuilder.ShipBlueprints[0].DLCs[i] = dlc.Name;
					}
				}
				return objectBuilder;
			}
			return null;
		}

		public static void PublishBlueprint(MyObjectBuilder_Definitions prefab, string blueprintName, string currentLocalDirectory)
		{
			string file = Path.Combine(BLUEPRINT_FOLDER_LOCAL, currentLocalDirectory, blueprintName);
			string title = prefab.ShipBlueprints[0].CubeGrids[0].DisplayName;
			string description = prefab.ShipBlueprints[0].Description;
			ulong publishId = prefab.ShipBlueprints[0].WorkshopId;
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: new StringBuilder("Publish"), messageText: new StringBuilder("Do you want to publish this blueprint?"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum val)
			{
				if (val == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					Action<MyGuiScreenMessageBox.ResultEnum, string[]> action = delegate(MyGuiScreenMessageBox.ResultEnum tagsResult, string[] outTags)
					{
						if (tagsResult == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							HashSet<uint> hashSet = new HashSet<uint>();
							MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = prefab.ShipBlueprints;
							foreach (MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition in shipBlueprints)
							{
								if (myObjectBuilder_ShipBlueprintDefinition.DLCs != null)
								{
									string[] dLCs = myObjectBuilder_ShipBlueprintDefinition.DLCs;
									foreach (string text in dLCs)
									{
										uint result2;
										if (MyDLCs.TryGetDLC(text, out MyDLCs.MyDLC dlc))
										{
											hashSet.Add(dlc.AppId);
										}
										else if (uint.TryParse(text, out result2))
										{
											hashSet.Add(result2);
										}
									}
								}
							}
							MyWorkshop.PublishBlueprintAsync(file, title, description, publishId, outTags, hashSet.ToArray(), MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, MyWorkshopItemPublisher publishedItem)
							{
								if (success)
								{
									prefab.ShipBlueprints[0].WorkshopId = publishedItem.Id;
									SavePrefabToFile(prefab, blueprintName, currentLocalDirectory, replace: true);
									MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublished), MySession.GameServiceName), new StringBuilder("BLUEPRINT PUBLISHED"), null, null, null, null, delegate
									{
										MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(publishedItem.Id));
									}));
								}
								else
								{
									StringBuilder messageText = (result != MyGameServiceCallResult.AccessDenied) ? new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishFailed), MySession.WorkshopServiceName, MySession.GameServiceName) : MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied);
									MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
								}
							});
						}
					};
					if (MyWorkshop.BlueprintCategories.Length != 0)
					{
						MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("blueprint", MyWorkshop.BlueprintCategories, null, action));
					}
					else
					{
						action(MyGuiScreenMessageBox.ResultEnum.YES, new string[1]
						{
							"blueprint"
						});
					}
				}
			}));
		}

		public static void SavePrefabToFile(MyObjectBuilder_Definitions prefab, string name, string currentDirectory, bool replace = false, MyBlueprintTypeEnum type = MyBlueprintTypeEnum.LOCAL)
		{
			if (type == MyBlueprintTypeEnum.LOCAL && MySandboxGame.Config.EnableSteamCloud)
			{
				type = MyBlueprintTypeEnum.CLOUD;
			}
			string text = string.Empty;
			switch (type)
			{
			case MyBlueprintTypeEnum.CLOUD:
				text = Path.Combine(BLUEPRINT_CLOUD_DIRECTORY, name);
				break;
			case MyBlueprintTypeEnum.LOCAL:
				text = Path.Combine(BLUEPRINT_FOLDER_LOCAL, currentDirectory, name);
				break;
			case MyBlueprintTypeEnum.STEAM:
			case MyBlueprintTypeEnum.SHARED:
			case MyBlueprintTypeEnum.DEFAULT:
				text = Path.Combine(BLUEPRINT_FOLDER_WORKSHOP, "temp", name);
				break;
			}
			string filePath = string.Empty;
			try
			{
				if (type == MyBlueprintTypeEnum.CLOUD)
				{
					filePath = Path.Combine(text, BLUEPRINT_LOCAL_NAME);
					SaveToCloud(prefab, filePath, replace);
				}
				else
				{
					SaveToDisk(prefab, name, replace, type, text, currentDirectory, ref filePath);
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine($"Failed to write prefab at file {filePath}, message: {ex.Message}, stack:{ex.StackTrace}");
			}
		}

		public static void SaveToCloud(MyObjectBuilder_Definitions prefab, string filePath, bool replace)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bool num = MyObjectBuilderSerializer.SerializeXML(memoryStream, prefab, MyObjectBuilderSerializer.XmlCompression.Gzip);
				if (num)
				{
					MyGameService.SaveToCloudAsync(buffer: memoryStream.ToArray(), fileName: filePath, completedAction: delegate(bool result)
					{
						if (result)
						{
							using (MemoryStream memoryStream2 = new MemoryStream())
							{
								if (MyObjectBuilderSerializer.SerializePB(memoryStream2, prefab))
								{
									byte[] buffer2 = memoryStream2.ToArray();
									filePath += "B5";
									MyGameService.SaveToCloud(filePath, buffer2);
								}
							}
						}
					});
				}
				if (!num)
				{
					ShowBlueprintSaveError();
				}
			}
		}

		public static void SaveToCloudFile(string pathFull, string pathRel)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (FileStream fileStream = new FileStream(pathFull, FileMode.Open, FileAccess.Read))
				{
					fileStream.CopyTo(memoryStream);
					byte[] buffer = memoryStream.ToArray();
					MyGameService.SaveToCloud(pathRel, buffer);
				}
			}
		}

		private static void ShowBlueprintSaveError()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Error"), messageText: new StringBuilder("There was a problem with saving blueprint/script")));
		}

		public static void SaveToDisk(MyObjectBuilder_Definitions prefab, string name, bool replace, MyBlueprintTypeEnum type, string file, string currentDirectory, ref string filePath)
		{
			if (!replace)
			{
				int num = 1;
				while (MyFileSystem.DirectoryExists(file))
				{
					file = Path.Combine(BLUEPRINT_FOLDER_LOCAL, currentDirectory, name + "_" + num);
					num++;
				}
				if (num > 1)
				{
					name += new StringBuilder("_" + (num - 1));
				}
			}
			filePath = Path.Combine(file, BLUEPRINT_LOCAL_NAME);
			bool num2 = MyObjectBuilderSerializer.SerializeXML(filePath, compress: false, prefab);
			if (num2 && type == MyBlueprintTypeEnum.LOCAL)
			{
				MyObjectBuilderSerializer.SerializePB(filePath + "B5", compress: false, prefab);
			}
			if (!num2)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Error"), messageText: new StringBuilder("There was a problem with saving blueprint")));
				if (Directory.Exists(file))
				{
					Directory.Delete(file, recursive: true);
				}
			}
		}

		public static int GetNumberOfBlocks(ref MyObjectBuilder_Definitions prefab)
		{
			int num = 0;
			MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in cubeGrids)
			{
				num += myObjectBuilder_CubeGrid.CubeBlocks.Count;
			}
			return num;
		}

		public static MyGuiControlButton CreateButton(MyGuiScreenDebugBase screen, float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = screen.AddButton(text, onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		public static MyGuiControlButton CreateButtonString(MyGuiScreenDebugBase screen, float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, string tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = screen.AddButton(text, onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip != null)
			{
				myGuiControlButton.SetToolTip(tooltip);
			}
			return myGuiControlButton;
		}

		public static void PublishScript(MyGuiControlButton button, string directory, MyBlueprintItemInfo script, Action OnPublished)
		{
			string path = Path.Combine(SCRIPT_FOLDER_LOCAL, directory, script.Data.Name, "modinfo.sbmi");
			if (File.Exists(path) && MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_ModInfo objectBuilder))
			{
				script.PublishedItemId = objectBuilder.WorkshopId;
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_PublishScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum val)
			{
				if (val == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					string fullPath = Path.Combine(SCRIPT_FOLDER_LOCAL, directory, script.Data.Name);
					MyWorkshop.PublishIngameScriptAsync(fullPath, script.Data.Name, script.Data.Description ?? "", script.PublishedItemId, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, MyWorkshopItemPublisher publishedFile)
					{
						if (success)
						{
							MyWorkshop.GenerateModInfo(fullPath, publishedFile.Id, Sync.MyId);
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublished), MySession.GameServiceName), MyTexts.Get(MySpaceTexts.ProgrammableBlock_PublishScriptPublished), null, null, null, null, delegate
							{
								OnPublished();
								MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(publishedFile.Id));
							}));
						}
						else
						{
							StringBuilder messageText = (result != MyGameServiceCallResult.AccessDenied) ? new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishFailed), MySession.WorkshopServiceName, MySession.GameServiceName) : MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied);
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
						}
					});
				}
			}));
		}

		public static bool IsItem_Blueprint(string path)
		{
			return File.Exists(path + "\\bp.sbc");
		}

		public static bool IsItem_Script(string path)
		{
			return File.Exists(path + "\\Script.cs");
		}

		private MyBlueprintUtils()
		{
		}
	}
}
