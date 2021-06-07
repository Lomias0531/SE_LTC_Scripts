using EmptyKeys.UserInterface.Mvvm;
using Multiplayer;
using Sandbox;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.AI;
using SpaceEngineers.Game.GUI;
using SpaceEngineers.Game.VoiceChat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using World;

namespace SpaceEngineers.Game
{
	public class SpaceEngineersGame : MySandboxGame
	{
		public const int SE_VERSION = 1193101;

		private Vector2I m_initializedScreenSize;

		public SpaceEngineersGame(string[] commandlineArgs)
			: base(commandlineArgs, (IntPtr)0)
		{
			MySandboxGame.GameCustomInitialization = new MySpaceGameCustomInitialization();
			FillCredits();
		}

		private void OnRenderInitialized(Vector2I size)
		{
			OnScreenSize = (Action<Vector2I>)Delegate.Remove(OnScreenSize, new Action<Vector2I>(OnRenderInitialized));
			m_initializedScreenSize = size;
			MySandboxGame.m_windowCreatedEvent.Set();
		}

		protected override void InitiliazeRender(IntPtr windowHandle)
		{
			OnScreenSize = (Action<Vector2I>)Delegate.Combine(OnScreenSize, new Action<Vector2I>(OnRenderInitialized));
			base.InitiliazeRender(windowHandle);
			StartIntroVideo();
		}

		private void StartIntroVideo()
		{
			if (MyFakes.ENABLE_LOGOS && MyFakes.ENABLE_LOGOS_ASAP)
			{
				MyScreenManager.AddScreen(MyGuiScreenInitialLoading.GetInstance);
				MyRenderProxy.Settings.RenderThreadHighPriority = true;
				MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
				string videoFile = Path.Combine(MyFileSystem.ContentPath, "Videos\\KSH.wmv");
				base.IntroVideoId = MyRenderProxy.PlayVideo(videoFile, 1f);
				MyRenderProxy.UpdateVideo(base.IntroVideoId);
				MyRenderProxy.DrawVideo(base.IntroVideoId, new Rectangle(0, 0, m_initializedScreenSize.X, m_initializedScreenSize.Y), Color.White, MyVideoRectangleFitMode.AutoFit);
				MyRenderProxy.AfterUpdate(null);
				MyRenderProxy.BeforeUpdate();
				MyVRage.Platform.Window.ShowAndFocus();
			}
		}

		public static void SetupBasicGameInfo()
		{
			MyPerGameSettings.BasicGameInfo.GameVersion = 1193101;
			MyPerGameSettings.BasicGameInfo.GameName = "Space Engineers";
			MyPerGameSettings.BasicGameInfo.GameNameSafe = "SpaceEngineers";
			MyPerGameSettings.BasicGameInfo.ApplicationName = "SpaceEngineers";
			MyPerGameSettings.BasicGameInfo.GameAcronym = "SE";
			MyPerGameSettings.BasicGameInfo.MinimumRequirementsWeb = "http://www.spaceengineersgame.com";
			MyPerGameSettings.BasicGameInfo.SplashScreenImage = "..\\Content\\Textures\\Logo\\splashscreen.png";
		}

		public static void SetupPerGameSettings()
		{
			MyPerGameSettings.Game = GameEnum.SE_GAME;
			MyPerGameSettings.GameIcon = "SpaceEngineers.ico";
			MyPerGameSettings.EnableGlobalGravity = false;
			MyPerGameSettings.GameModAssembly = "SpaceEngineers.Game.dll";
			MyPerGameSettings.GameModObjBuildersAssembly = "SpaceEngineers.ObjectBuilders.dll";
			MyPerGameSettings.OffsetVoxelMapByHalfVoxel = true;
			MyPerGameSettings.EnablePregeneratedAsteroidHack = true;
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySandboxGame.ConfigDedicated = new MyConfigDedicated<MyObjectBuilder_SessionSettings>("SpaceEngineers-Dedicated.cfg");
			}
			MySandboxGame.GameCustomInitialization = new MySpaceGameCustomInitialization();
			MyPerGameSettings.ShowObfuscationStatus = false;
			MyPerGameSettings.UseNewDamageEffects = true;
			MyPerGameSettings.EnableResearch = true;
			MyPerGameSettings.UseVolumeLimiter = (MyFakes.ENABLE_NEW_SOUNDS && MyFakes.ENABLE_REALISTIC_LIMITER);
			MyPerGameSettings.UseSameSoundLimiter = true;
			MyPerGameSettings.UseMusicController = true;
			MyPerGameSettings.UseReverbEffect = true;
			MyPerGameSettings.Destruction = false;
			MyMusicTrack value = default(MyMusicTrack);
			value.TransitionCategory = MyStringId.GetOrCompute("NoRandom");
			value.MusicCategory = MyStringId.GetOrCompute("MusicMenu");
			MyPerGameSettings.MainMenuTrack = value;
			MyPerGameSettings.BallFriendlyPhysics = false;
			if (MyFakes.ENABLE_CESTMIR_PATHFINDING)
			{
				MyPerGameSettings.PathfindingType = typeof(MyPathfinding);
			}
			else
			{
				MyPerGameSettings.PathfindingType = typeof(MyRDPathfinding);
			}
			MyPerGameSettings.BotFactoryType = typeof(MySpaceBotFactory);
			MyPerGameSettings.ControlMenuInitializerType = typeof(MySpaceControlMenuInitializer);
			MyPerGameSettings.EnableScenarios = true;
			MyPerGameSettings.EnableJumpDrive = true;
			MyPerGameSettings.EnableShipSoundSystem = true;
			MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE = true;
			MyFakes.ENABLE_DRIVING_PARTICLES = true;
			MyPerGameSettings.EnablePathfinding = false;
			MyPerGameSettings.CharacterGravityMultiplier = 2f;
			MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS_HELPERS = true;
			MyPerGameSettings.EnableRagdollInJetpack = true;
			MyPerGameSettings.GUI.OptionsScreen = typeof(MyGuiScreenOptionsSpace);
			MyPerGameSettings.GUI.PerformanceWarningScreen = typeof(MyGuiScreenPerformanceWarnings);
			MyPerGameSettings.GUI.CreateFactionScreen = typeof(MyGuiScreenCreateOrEditFactionSpace);
			MyPerGameSettings.GUI.MainMenu = typeof(MyGuiScreenMainMenu);
			MyPerGameSettings.DefaultGraphicsRenderer = MySandboxGame.DirectX11RendererKey;
			MyPerGameSettings.EnableWelderAutoswitch = true;
			MyPerGameSettings.InventoryMass = true;
			MyPerGameSettings.CompatHelperType = typeof(MySpaceSessionCompatHelper);
			MyPerGameSettings.GUI.MainMenuBackgroundVideos = new string[10]
			{
				"Videos\\Background01_720p.wmv",
				"Videos\\Background02_720p.wmv",
				"Videos\\Background03_720p.wmv",
				"Videos\\Background04_720p.wmv",
				"Videos\\Background05_720p.wmv",
				"Videos\\Background09_720p.wmv",
				"Videos\\Background10_720p.wmv",
				"Videos\\Background11_720p.wmv",
				"Videos\\Background12_720p.wmv",
				"Videos\\Background13_720p.wmv"
			};
			MyPerGameSettings.VoiceChatEnabled = true;
			MyPerGameSettings.VoiceChatLogic = typeof(MyVoiceChatLogic);
			MyPerGameSettings.ClientStateType = typeof(MySpaceClientState);
			MyVoxelPhysicsBody.UseLod1VoxelPhysics = false;
			MyPerGameSettings.EnableAi = true;
			MyPerGameSettings.EnablePathfinding = true;
			MyFakesLocal.SetupLocalPerGameSettings();
		}

		private static void FillCredits()
		{
			MyCreditsDepartment myCreditsDepartment = new MyCreditsDepartment("{LOCG:Department_ExecutiveProducer}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment);
			myCreditsDepartment.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment.Persons.Add(new MyCreditsPerson("MAREK ROSA"));
			MyCreditsDepartment myCreditsDepartment2 = new MyCreditsDepartment("{LOCG:Department_LeadProducer}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment2);
			myCreditsDepartment2.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment2.Persons.Add(new MyCreditsPerson("PETR MINARIK"));
			MyCreditsDepartment myCreditsDepartment3 = new MyCreditsDepartment("{LOCG:Department_TeamOperations}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment3);
			myCreditsDepartment3.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment3.Persons.Add(new MyCreditsPerson("VLADISLAV POLGAR"));
			MyCreditsDepartment myCreditsDepartment4 = new MyCreditsDepartment("{LOCG:Department_TechnicalDirector}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment4);
			myCreditsDepartment4.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment4.Persons.Add(new MyCreditsPerson("JAN \"CENDA\" HLOUSEK"));
			MyCreditsDepartment myCreditsDepartment5 = new MyCreditsDepartment("{LOCG:Department_LeadProgrammers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment5);
			myCreditsDepartment5.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment5.Persons.Add(new MyCreditsPerson("FILIP DUSEK"));
			myCreditsDepartment5.Persons.Add(new MyCreditsPerson("JAN \"CENDA\" HLOUSEK"));
			myCreditsDepartment5.Persons.Add(new MyCreditsPerson("PETR MINARIK"));
			MyCreditsDepartment myCreditsDepartment6 = new MyCreditsDepartment("{LOCG:Department_Programmers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment6);
			myCreditsDepartment6.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("PETR BERANEK"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("MIRO FARKAS"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("SANDRA LENARDOVA"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("MARTIN PAVLICEK"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("GRZEGORZ ZADROGA"));
			MyCreditsDepartment myCreditsDepartment7 = new MyCreditsDepartment("{LOCG:Department_LeadDesigner}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment7);
			myCreditsDepartment7.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment7.Persons.Add(new MyCreditsPerson("JOACHIM KOOLHOF"));
			MyCreditsDepartment myCreditsDepartment8 = new MyCreditsDepartment("{LOCG:Department_Designers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment8);
			myCreditsDepartment8.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("MARTIN JASSO"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("PAVEL KONFRST"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
			MyCreditsDepartment myCreditsDepartment9 = new MyCreditsDepartment("{LOCG:Department_LeadArtist}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment9);
			myCreditsDepartment9.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment9.Persons.Add(new MyCreditsPerson("NATIQ AGHAYEV"));
			MyCreditsDepartment myCreditsDepartment10 = new MyCreditsDepartment("{LOCG:Department_Artists}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment10);
			myCreditsDepartment10.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment10.Persons.Add(new MyCreditsPerson("KRISTIAAN RENAERTS"));
			myCreditsDepartment10.Persons.Add(new MyCreditsPerson("JAN TRAUSKE"));
			MyCreditsDepartment myCreditsDepartment11 = new MyCreditsDepartment("{LOCG:Department_SoundDesign}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment11);
			myCreditsDepartment11.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment11.Persons.Add(new MyCreditsPerson("LUKAS TVRDON"));
			MyCreditsDepartment myCreditsDepartment12 = new MyCreditsDepartment("{LOCG:Department_Music}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment12);
			myCreditsDepartment12.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment12.Persons.Add(new MyCreditsPerson("KAREL ANTONIN"));
			myCreditsDepartment12.Persons.Add(new MyCreditsPerson("ANNA KALHAUSOVA (cello)"));
			myCreditsDepartment12.Persons.Add(new MyCreditsPerson("MARIE SVOBODOVA (vocals)"));
			MyCreditsDepartment myCreditsDepartment13 = new MyCreditsDepartment("{LOCG:Department_Video}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment13);
			myCreditsDepartment13.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment13.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
			MyCreditsDepartment myCreditsDepartment14 = new MyCreditsDepartment("{LOCG:Department_LeadTester}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment14);
			myCreditsDepartment14.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment14.Persons.Add(new MyCreditsPerson("ONDREJ NAHALKA"));
			MyCreditsDepartment myCreditsDepartment15 = new MyCreditsDepartment("{LOCG:Department_Testers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment15);
			myCreditsDepartment15.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment15.Persons.Add(new MyCreditsPerson("KATERINA CERVENA"));
			myCreditsDepartment15.Persons.Add(new MyCreditsPerson("JAN HRIVNAC"));
			myCreditsDepartment15.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
			myCreditsDepartment15.Persons.Add(new MyCreditsPerson("VOJTECH NEORAL"));
			myCreditsDepartment15.Persons.Add(new MyCreditsPerson("JAN PETRZILKA"));
			MyCreditsDepartment myCreditsDepartment16 = new MyCreditsDepartment("{LOCG:Department_CommunityPr}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment16);
			myCreditsDepartment16.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("JESSE BAULE"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
			MyCreditsDepartment myCreditsDepartment17 = new MyCreditsDepartment("{LOCG:Department_Office}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment17);
			myCreditsDepartment17.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("MARIANNA HIRCAKOVA"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("PETR KREJCI"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("LUCIE KRESTOVA"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("VACLAV NOVOTNY"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("TOMAS STROUHAL"));
			MyCreditsDepartment myCreditsDepartment18 = new MyCreditsDepartment("{LOCG:Department_CommunityManagers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment18);
			myCreditsDepartment18.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("Dr Vagax"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("Conrad Larson"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("Dan2D3D"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("RayvenQ"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("Redphoenix"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("TodesRitter"));
			MyCreditsDepartment myCreditsDepartment19 = new MyCreditsDepartment("{LOCG:Department_ModContributors}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment19);
			myCreditsDepartment19.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Tyrsis"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Daniel \"Phoenix84\" Osborne"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Morten \"Malware\" Aune Lyrstad"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Arindel"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Darth Biomech"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Night Lone"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Mexmer"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("JD.Horx"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("John \"Jimmacle\" Gross"));
			MyCreditsDepartment myCreditsDepartment20 = new MyCreditsDepartment("{LOCG:Department_Translators}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment20);
			myCreditsDepartment20.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Damian \"Truzaku\" Komarek"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Julian Tomaszewski"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("George Grivas"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Олег \"AaLeSsHhKka\" Цюпка"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Maxim \"Ma)(imuM\" Lyashuk"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Axazel"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Baly94"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Dyret"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("gon.gged"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Huberto"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("HunterNephilim"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("nintendo22"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Quellix"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("raviool"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Dr. Bell"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Dominik Frydl"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Daniel Hloušek"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Andre Camara Marchi"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Ociotek Traducciones"));
			myCreditsDepartment20.LogoTexture = "Textures\\Logo\\TranslatorsCN.dds";
			myCreditsDepartment20.LogoScale = 0.85f;
			myCreditsDepartment20.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsDepartment20.LogoTexture);
			myCreditsDepartment20.LogoOffsetPost = 0.11f;
			MyCreditsDepartment myCreditsDepartment21 = new MyCreditsDepartment("{LOCG:Department_SpecialThanks}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment21);
			myCreditsDepartment21.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ABDULAZIZ ALDIGS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("DUSAN ANDRAS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ONDREJ ANGELOVIC"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("IVAN BARAN"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ANTON \"TOTAL\" BAUER"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ALES BRICH"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("JOAO CARIAS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("THEO ESCAMEZ"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ALEX FLOREA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("JAN GOLMIC"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("CESTMIR HOUSKA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("JAKUB HRNCIR"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("LUKAS CHRAPEK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("DANIEL ILHA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("LUKAS JANDIK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MARKETA JAROSOVA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MARTIN KOCISEK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("JOELLEN KOESTER"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("GREGORY KONTADAKIS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MARKO KORHONEN"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("TOMAS KOSEK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("RADOVAN KOTRLA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MARTIN KROSLAK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MICHAL KUCIS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("DANIEL LEIMBACH"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("RADKA LISA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("PERCY LIU"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("GEORGE MAMAKOS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("BRANT MARTIN"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("JAN NEKVAPIL"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MAREK OBRSAL"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("PAVEL OCOVAJ"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("PREMYSL PASKA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ONDREJ PETRZILKA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("FRANCESKO PRETTO"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("TOMAS PSENICKA"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("DOMINIK RAGANCIK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("TOMAS RAMPAS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("DUSAN REPIK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("VILEM SOULAK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("RASTKO STANOJEVIC"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("SLOBODAN STEVIC"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("TIM TOXOPEUS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("JAN VEBERSIK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("LUKAS VILIM"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MATEJ VLK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("ADAM WILLIAMS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("CHARLES WINTERS"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MICHAL WROBEL"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MICHAL ZAK"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("MICHAL ZAVADAK"));
			MyCreditsDepartment myCreditsDepartment22 = new MyCreditsDepartment("{LOCG:Department_MoreInfo}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment22);
			myCreditsDepartment22.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("{LOCG:Person_Web}"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("{LOCG:Person_FB}"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("{LOCG:Person_Twitter}"));
			MyCreditsNotice myCreditsNotice = new MyCreditsNotice();
			myCreditsNotice.LogoScale = 0.8f;
			myCreditsNotice.LogoTexture = "Textures\\Logo\\vrage_logo_2_0_small.dds";
			myCreditsNotice.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsNotice.LogoTexture);
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_01}"));
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_02}"));
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_03}"));
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_04}"));
			MyPerGameSettings.Credits.CreditNotices.Add(myCreditsNotice);
			MyCreditsNotice myCreditsNotice2 = new MyCreditsNotice();
			myCreditsNotice2.LogoTexture = "Textures\\Logo\\havok.dds";
			myCreditsNotice2.LogoScale = 0.65f;
			myCreditsNotice2.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsNotice2.LogoTexture);
			myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_05}"));
			myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_06}"));
			myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_07}"));
			MyPerGameSettings.Credits.CreditNotices.Add(myCreditsNotice2);
			SetupSecrets();
		}

		private static void SetupSecrets()
		{
			MyPerGameSettings.GA_Public_GameKey = "27bae5ba5219bcd64ddbf83113eabb30";
			MyPerGameSettings.GA_Public_SecretKey = "d04e0431f97f90fae73b9d6ea99fc9746695bd11";
			MyPerGameSettings.GA_Dev_GameKey = "3a6b6ebdc48552beba3efe173488d8ba";
			MyPerGameSettings.GA_Dev_SecretKey = "caecaaa4a91f6b2598cf8ffb931b3573f20b4343";
			MyPerGameSettings.GA_Pirate_GameKey = "41827f7c8bfed902495e0e27cb57c495";
			MyPerGameSettings.GA_Pirate_SecretKey = "493b7cb3f0a472f940c0ba0c38efbb49e902cbec";
			MyPerGameSettings.GA_Other_GameKey = "4f02769277e62b4344da70967e99a2a0";
			MyPerGameSettings.GA_Other_SecretKey = "7fa773c228ce9534181adcfebf30d18bc6807d2b";
		}

		protected override void InitInput()
		{
			base.InitInput();
			MyGuiDescriptor myGuiDescriptor = new MyGuiDescriptor(MyCommonTexts.ControlName_ToggleSignalsMode, MyCommonTexts.ControlName_ToggleSignalsMode_Tooltip);
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOGGLE_SIGNALS, myGuiDescriptor);
			MyControl control = new MyControl(MyControlsSpace.TOGGLE_SIGNALS, myGuiDescriptor.NameEnum, MyGuiControlTypeEnum.Spectator, null, MyKeys.H, null, null, myGuiDescriptor.DescriptionEnum);
			MyInput.Static.AddDefaultControl(MyControlsSpace.TOGGLE_SIGNALS, control);
			myGuiDescriptor = new MyGuiDescriptor(MyCommonTexts.ControlName_CubeSizeMode, MyCommonTexts.ControlName_CubeSizeMode_Tooltip);
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, myGuiDescriptor);
			control = new MyControl(MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, myGuiDescriptor.NameEnum, MyGuiControlTypeEnum.Systems2, null, MyKeys.R, null, null, myGuiDescriptor.DescriptionEnum);
			MyInput.Static.AddDefaultControl(MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, control);
		}

		protected override void CheckGraphicsCard(MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
		{
			base.CheckGraphicsCard(msgVideoAdapters);
			MyAdapterInfo myAdapterInfo = msgVideoAdapters.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
			MyPerformanceSettings defaults = MyGuiScreenOptionsGraphics.GetPreset(myAdapterInfo.Quality);
			if (myAdapterInfo.VRAM < 512000000)
			{
				defaults.RenderSettings.TextureQuality = MyTextureQuality.LOW;
			}
			else if (myAdapterInfo.VRAM < 2000000000 && myAdapterInfo.Quality == MyRenderQualityEnum.HIGH)
			{
				defaults.RenderSettings.TextureQuality = MyTextureQuality.MEDIUM;
			}
			MyVideoSettingsManager.UpdateRenderSettingsFromConfig(ref defaults, myAdapterInfo.Quality == MyRenderQualityEnum.XBOX_X);
		}

		public static void SetupAnalytics()
		{
			if (MySandboxGame.Config.GDPRConsent ?? false)
			{
				MyVRage.Platform.InitAnalytics("27bae5ba5219bcd64ddbf83113eabb30:d04e0431f97f90fae73b9d6ea99fc9746695bd11", 1193101.ToString());
				MyAnalyticsManager.Instance.RegisterAnalyticsTracker(MyVRage.Platform.Analytics);
			}
			MyOpickaAnalytics orCreateInstance = MyOpickaAnalytics.GetOrCreateInstance(MyPerGameSettings.BasicGameInfo.GameAcronym, MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString());
			MyAnalyticsManager.Instance.RegisterAnalyticsTracker(orCreateInstance);
		}

		protected override void InitServices()
		{
			base.InitServices();
			ServiceManager.Instance.AddService((IMyGuiScreenFactoryService)new MyGuiScreenFactoryService());
		}
	}
}
