using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Input;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public static class MyGuiSandbox
	{
		public static Regex urlRgx = new Regex("^(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?$");

		internal static IMyGuiSandbox Gui = new MyNullGui();

		private static Dictionary<Type, Type> m_createdScreenTypes = new Dictionary<Type, Type>();

		public static int TotalGamePlayTimeInMilliseconds;

		public static Action<object> GuiControlCreated;

		public static Action<object> GuiControlRemoved;

		private static Regex[] WWW_WHITELIST = new Regex[4]
		{
			new Regex("^(http[s]{0,1}://){0,1}[^/]*youtube.com/.*", RegexOptions.IgnoreCase),
			new Regex("^(http[s]{0,1}://){0,1}[^/]*youtu.be/.*", RegexOptions.IgnoreCase),
			new Regex("^(http[s]{0,1}://){0,1}[^/]*steamcommunity.com/.*", RegexOptions.IgnoreCase),
			new Regex("^(http[s]{0,1}://){0,1}[^/]*forum[s]{0,1}.keenswh.com/.*", RegexOptions.IgnoreCase)
		};

		public static Vector2 MouseCursorPosition => Gui.MouseCursorPosition;

		public static Action<float, Vector2> DrawGameLogoHandler
		{
			get
			{
				return Gui.DrawGameLogoHandler;
			}
			set
			{
				Gui.DrawGameLogoHandler = value;
			}
		}

		public static void SetMouseCursorVisibility(bool visible, bool changePosition = true)
		{
			Gui.SetMouseCursorVisibility(visible, changePosition);
		}

		public static void Ansel_WarningMessage(bool pauseAllowed, bool spectatorEnabled)
		{
			if (!pauseAllowed || !spectatorEnabled)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!pauseAllowed)
				{
					stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.MessageBoxTextAnselCannotPauseOnlineGame));
					stringBuilder.AppendLine("");
				}
				if (!spectatorEnabled)
				{
					stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.MessageBoxTextAnselSpectatorDisabled));
					stringBuilder.AppendLine("");
				}
				stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.MessageBoxTextAnselTimeout));
				AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.NONE_TIMEOUT, stringBuilder, MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, null, 4000));
			}
		}

		public static bool Ansel_IsSpectatorEnabled()
		{
			return MyGuiScreenGamePlay.SpectatorEnabled;
		}

		public static void LoadData(bool nullGui)
		{
			MyVRage.Platform.Ansel.WarningMessageDelegate += Ansel_WarningMessage;
			MyVRage.Platform.Ansel.IsSpectatorEnabledDelegate += Ansel_IsSpectatorEnabled;
			if (!nullGui)
			{
				Gui = new MyDX9Gui();
			}
			Gui.LoadData();
		}

		public static void LoadContent()
		{
			Gui.LoadContent();
		}

		public static bool IsUrlWhitelisted(string wwwLink)
		{
			Regex[] wWW_WHITELIST = WWW_WHITELIST;
			for (int i = 0; i < wWW_WHITELIST.Length; i++)
			{
				if (wWW_WHITELIST[i].IsMatch(wwwLink))
				{
					return true;
				}
			}
			return false;
		}

		public static void OpenUrlWithFallback(string url, string urlFriendlyName, bool useWhitelist = false)
		{
			if (useWhitelist && !IsUrlWhitelisted(url))
			{
				MySandboxGame.Log.WriteLine("URL NOT ALLOWED: " + url);
				return;
			}
			StringBuilder confirmMessage = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextOpenUrlOverlayNotEnabled), urlFriendlyName, MySession.GameServiceName);
			OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm, confirmMessage);
		}

		public static bool IsUrlValid(string url)
		{
			return urlRgx.IsMatch(url);
		}

		public static void OpenUrl(string url, UrlOpenMode openMode, StringBuilder confirmMessage = null)
		{
			bool num = (openMode & UrlOpenMode.SteamOverlay) != 0;
			bool flag = (openMode & UrlOpenMode.ExternalBrowser) != 0;
			bool flag2 = (openMode & UrlOpenMode.ConfirmExternal) != 0;
			if ((!num || !Gui.OpenSteamOverlay(url)) && flag)
			{
				if (flag2)
				{
					AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: confirmMessage ?? new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextOpenBrowser, url), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							OpenExternalBrowser(url);
						}
					}));
				}
				else
				{
					OpenExternalBrowser(url);
				}
			}
		}

		public static void OpenExternalBrowser(string url)
		{
			if (!MyVRage.Platform.OpenUrl(url))
			{
				StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.TitleFailedToStartInternetBrowser);
				AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, stringBuilder));
			}
		}

		public static void UnloadContent()
		{
			Gui.UnloadContent();
		}

		public static void SwitchDebugScreensEnabled()
		{
			Gui.SwitchDebugScreensEnabled();
		}

		public static void ShowModErrors()
		{
			Gui.ShowModErrors();
		}

		public static bool IsDebugScreenEnabled()
		{
			return Gui.IsDebugScreenEnabled();
		}

		public static MyGuiScreenBase CreateScreen(Type screenType, params object[] args)
		{
			return Activator.CreateInstance(screenType, args) as MyGuiScreenBase;
		}

		public static T CreateScreen<T>(params object[] args) where T : MyGuiScreenBase
		{
			Type value = null;
			if (!m_createdScreenTypes.TryGetValue(typeof(T), out value))
			{
				Type typeFromHandle = typeof(T);
				value = typeFromHandle;
				ChooseScreenType<T>(ref value, MyPlugins.GameAssembly);
				ChooseScreenType<T>(ref value, MyPlugins.SandboxAssembly);
				ChooseScreenType<T>(ref value, MyPlugins.UserAssemblies);
				m_createdScreenTypes[typeFromHandle] = value;
			}
			return Activator.CreateInstance(value, args) as T;
		}

		private static void ChooseScreenType<T>(ref Type createdType, Assembly[] assemblies) where T : MyGuiScreenBase
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					ChooseScreenType<T>(ref createdType, assembly);
				}
			}
		}

		private static void ChooseScreenType<T>(ref Type createdType, Assembly assembly) where T : MyGuiScreenBase
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			int num = 0;
			Type type;
			while (true)
			{
				if (num < types.Length)
				{
					type = types[num];
					if (typeof(T).IsAssignableFrom(type))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			createdType = type;
		}

		public static void AddScreen(MyGuiScreenBase screen)
		{
			Gui.AddScreen(screen);
			if (GuiControlCreated != null)
			{
				GuiControlCreated(screen);
			}
			screen.Closed += delegate(MyGuiScreenBase x)
			{
				if (GuiControlRemoved != null)
				{
					GuiControlRemoved(x);
				}
			};
			if (MyAPIGateway.GuiControlCreated != null)
			{
				MyAPIGateway.GuiControlCreated(screen);
			}
		}

		public static void InsertScreen(MyGuiScreenBase screen, int index)
		{
			Gui.InsertScreen(screen, index);
			if (GuiControlCreated != null)
			{
				GuiControlCreated(screen);
			}
			screen.Closed += delegate(MyGuiScreenBase x)
			{
				if (GuiControlRemoved != null)
				{
					GuiControlRemoved(x);
				}
			};
			if (MyAPIGateway.GuiControlCreated != null)
			{
				MyAPIGateway.GuiControlCreated(screen);
			}
		}

		public static void RemoveScreen(MyGuiScreenBase screen)
		{
			Gui.RemoveScreen(screen);
			if (GuiControlRemoved != null)
			{
				GuiControlRemoved(screen);
			}
		}

		public static void HandleInput()
		{
			Gui.HandleInput();
		}

		public static void HandleInputAfterSimulation()
		{
			Gui.HandleInputAfterSimulation();
		}

		public static void Update(int totalTimeInMS)
		{
			Gui.Update(totalTimeInMS);
		}

		public static void Draw()
		{
			Gui.Draw();
		}

		public static void BackToIntroLogos(Action afterLogosAction)
		{
			Gui.BackToIntroLogos(afterLogosAction);
		}

		public static void BackToMainMenu()
		{
			Gui.BackToMainMenu();
		}

		public static float GetDefaultTextScaleWithLanguage()
		{
			return Gui.GetDefaultTextScaleWithLanguage();
		}

		public static void TakeScreenshot(int width, int height, string saveToPath = null, bool ignoreSprites = false, bool showNotification = true)
		{
			Gui.TakeScreenshot(width, height, saveToPath, ignoreSprites, showNotification);
		}

		public static MyGuiScreenMessageBox CreateMessageBox(MyMessageBoxStyleEnum styleEnum = MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType buttonType = MyMessageBoxButtonsType.OK, StringBuilder messageText = null, StringBuilder messageCaption = null, MyStringId? okButtonText = null, MyStringId? cancelButtonText = null, MyStringId? yesButtonText = null, MyStringId? noButtonText = null, Action<MyGuiScreenMessageBox.ResultEnum> callback = null, int timeoutInMiliseconds = 0, MyGuiScreenMessageBox.ResultEnum focusedResult = MyGuiScreenMessageBox.ResultEnum.YES, bool canHideOthers = true, Vector2? size = null, bool useOpacity = true)
		{
			return new MyGuiScreenMessageBox(styleEnum, buttonType, messageText, messageCaption, okButtonText ?? MyCommonTexts.Ok, cancelButtonText ?? MyCommonTexts.Cancel, yesButtonText ?? MyCommonTexts.Yes, noButtonText ?? MyCommonTexts.No, callback, timeoutInMiliseconds, focusedResult, canHideOthers, size, useOpacity ? MySandboxGame.Config.UIBkOpacity : 1f, useOpacity ? MySandboxGame.Config.UIOpacity : 1f);
		}

		public static void Show(StringBuilder text, MyStringId caption = default(MyStringId), MyMessageBoxStyleEnum type = MyMessageBoxStyleEnum.Error)
		{
			AddScreen(CreateMessageBox(type, MyMessageBoxButtonsType.OK, text, MyTexts.Get(caption)));
		}

		public static void Show(MyStringId text, MyStringId caption = default(MyStringId), MyMessageBoxStyleEnum type = MyMessageBoxStyleEnum.Error)
		{
			AddScreen(CreateMessageBox(type, MyMessageBoxButtonsType.OK, MyTexts.Get(text), MyTexts.Get(caption)));
		}

		public static void DrawGameLogo(float transitionAlpha, Vector2 position)
		{
			Gui.DrawGameLogo(transitionAlpha, position);
		}

		public static void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size)
		{
			Gui.DrawBadge(texture, transitionAlpha, position, size);
		}

		public static string GetKeyName(MyStringId control)
		{
			MyControl gameControl = MyInput.Static.GetGameControl(control);
			if (gameControl != null)
			{
				return gameControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			}
			return "";
		}
	}
}
