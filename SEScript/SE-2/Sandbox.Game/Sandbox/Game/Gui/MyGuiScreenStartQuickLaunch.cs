#define VRAGE
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenStartQuickLaunch : MyGuiScreenBase
	{
		private MyQuickLaunchType m_quickLaunchType;

		private bool m_childScreenLaunched;

		public static MyGuiScreenStartQuickLaunch CurrentScreen;

		public MyGuiScreenStartQuickLaunch(MyQuickLaunchType quickLaunchType, MyStringId progressText)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			m_quickLaunchType = quickLaunchType;
			CurrentScreen = this;
		}

		public override void LoadContent()
		{
			base.LoadContent();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenStartQuickLaunch";
		}

		private static MyWorldGenerator.Args CreateBasicQuickstartArgs()
		{
			MyWorldGenerator.Args result = default(MyWorldGenerator.Args);
			result.Scenario = MyDefinitionManager.Static.GetScenarioDefinition(new MyDefinitionId(typeof(MyObjectBuilder_ScenarioDefinition), "EasyStart1"));
			result.AsteroidAmount = 0;
			return result;
		}

		private static MyObjectBuilder_SessionSettings CreateBasicQuickStartSettings()
		{
			MyObjectBuilder_SessionSettings myObjectBuilder_SessionSettings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
			myObjectBuilder_SessionSettings.GameMode = MyGameModeEnum.Creative;
			myObjectBuilder_SessionSettings.EnableToolShake = true;
			myObjectBuilder_SessionSettings.EnableSunRotation = (MyPerGameSettings.Game == GameEnum.SE_GAME);
			myObjectBuilder_SessionSettings.VoxelGeneratorVersion = 4;
			myObjectBuilder_SessionSettings.CargoShipsEnabled = true;
			myObjectBuilder_SessionSettings.EnableOxygen = true;
			myObjectBuilder_SessionSettings.EnableSpiders = false;
			myObjectBuilder_SessionSettings.EnableWolfs = false;
			MyWorldGenerator.SetProceduralSettings(-1, myObjectBuilder_SessionSettings);
			return myObjectBuilder_SessionSettings;
		}

		public static void QuickstartSandbox(MyObjectBuilder_SessionSettings quickstartSettings, MyWorldGenerator.Args? quickstartArgs)
		{
			MyLog.Default.WriteLine("QuickstartSandbox - START");
			MyScreenManager.RemoveAllScreensExcept(null);
			MySessionLoader.StartLoading(delegate
			{
				MyObjectBuilder_SessionSettings settings = quickstartSettings ?? CreateBasicQuickStartSettings();
				MyWorldGenerator.Args generationArgs = quickstartArgs ?? CreateBasicQuickstartArgs();
				List<MyObjectBuilder_Checkpoint.ModItem> mods = new List<MyObjectBuilder_Checkpoint.ModItem>(0);
				MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Quickstart);
				MySession.Start("Created " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "", "", settings, mods, generationArgs);
			});
			MyLog.Default.WriteLine("QuickstartSandbox - END");
		}

		public override bool Update(bool hasFocus)
		{
			if (!hasFocus)
			{
				return base.Update(hasFocus);
			}
			if (m_childScreenLaunched && hasFocus)
			{
				CloseScreenNow();
			}
			if (m_childScreenLaunched)
			{
				return base.Update(hasFocus);
			}
			if (MyInput.Static.IsKeyPress(MyKeys.Escape))
			{
				MySessionLoader.UnloadAndExitToMenu();
				return base.Update(hasFocus);
			}
			switch (m_quickLaunchType)
			{
			case MyQuickLaunchType.LAST_SANDBOX:
			{
				string lastSessionPath = MyLocalCache.GetLastSessionPath();
				if (lastSessionPath != null && Directory.Exists(lastSessionPath))
				{
					MySessionLoader.LoadSingleplayerSession(lastSessionPath);
				}
				else
				{
					MySandboxGame.AfterLogos();
				}
				m_childScreenLaunched = true;
				break;
			}
			case MyQuickLaunchType.NEW_SANDBOX:
				QuickstartSandbox(null, null);
				m_childScreenLaunched = true;
				break;
			default:
				throw new InvalidBranchException();
			}
			return base.Update(hasFocus);
		}
	}
}
