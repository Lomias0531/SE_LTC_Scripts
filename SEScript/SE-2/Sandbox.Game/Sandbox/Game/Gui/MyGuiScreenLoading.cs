using Sandbox.AppCode;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenLoading : MyGuiScreenBase
	{
		public static readonly int STREAMING_TIMEOUT = 900;

		public static MyGuiScreenLoading Static;

		private MyGuiScreenBase m_screenToLoad;

		private readonly MyGuiScreenGamePlay m_screenToUnload;

		private string m_backgroundScreenTexture;

		private string m_backgroundTextureFromConstructor;

		private string m_customTextFromConstructor;

		private string m_rotatingWheelTexture;

		private string m_gameLogoTexture;

		private MyLoadingScreenText m_currentText;

		private MyGuiControlMultilineText m_multiTextControl;

		private StringBuilder m_authorWithDash;

		private MyGuiControlRotatingWheel m_wheel;

		private bool m_exceptionDuringLoad;

		public static string LastBackgroundTexture;

		public Action OnLoadingXMLAllowed;

		public static int m_currentTextIdx = 0;

		private volatile bool m_loadInDrawFinished;

		private bool m_loadFinished;

		private bool m_isStreamed;

		private int m_streamingTimeout;

		private string m_font = "LoadingScreen";

		private static long lastEnvWorkingSet = 0L;

		private static long lastGc = 0L;

		private static long lastVid = 0L;

		public event Action OnScreenLoadingFinished;

		public MyGuiScreenLoading(MyGuiScreenBase screenToLoad, MyGuiScreenGamePlay screenToUnload, string textureFromConstructor, string customText = null)
			: base(Vector2.Zero)
		{
			base.CanBeHidden = false;
			m_isTopMostScreen = true;
			MyLoadingPerformance.Instance.StartTiming();
			Static = this;
			m_screenToLoad = screenToLoad;
			m_screenToUnload = screenToUnload;
			m_closeOnEsc = false;
			base.DrawMouseCursor = false;
			m_loadInDrawFinished = false;
			m_drawEvenWithoutFocus = true;
			m_currentText = MyLoadingScreenText.GetRandomText();
			m_isFirstForUnload = true;
			MyGuiSandbox.SetMouseCursorVisibility(visible: false);
			m_rotatingWheelTexture = "Textures\\GUI\\screens\\screen_loading_wheel_loading_screen.dds";
			m_backgroundTextureFromConstructor = textureFromConstructor;
			m_customTextFromConstructor = customText;
			m_loadFinished = false;
			if (m_screenToLoad != null)
			{
				MySandboxGame.IsUpdateReady = false;
				MySandboxGame.AreClipmapsReady = (!Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated || MyExternalAppBase.Static != null);
				MySandboxGame.RenderTasksFinished = (Sandbox.Engine.Platform.Game.IsDedicated || MyExternalAppBase.Static != null);
			}
			m_authorWithDash = new StringBuilder();
			RecreateControls(constructor: true);
			MyInput.Static.EnableInput(enable: false);
			if (Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated || MyMultiplayer.Static == null)
			{
				m_isStreamed = true;
			}
			else
			{
				MyMultiplayer.Static.LocalRespawnRequested += OnLocalRespawnRequested;
			}
		}

		private void OnLocalRespawnRequested()
		{
			(MyMultiplayer.Static as MyMultiplayerClientBase).RequestBatchConfirmation();
			MyMultiplayer.Static.PendingReplicablesDone += MyMultiplayer_PendingReplicablesDone;
			MyMultiplayer.Static.LocalRespawnRequested -= OnLocalRespawnRequested;
			m_streamingTimeout = 0;
		}

		private void MyMultiplayer_PendingReplicablesDone()
		{
			m_isStreamed = true;
			if (MySession.Static.VoxelMaps.Instances.Count > 0)
			{
				MySandboxGame.AreClipmapsReady = false;
			}
			MyMultiplayer.Static.PendingReplicablesDone -= MyMultiplayer_PendingReplicablesDone;
		}

		public MyGuiScreenLoading(MyGuiScreenBase screenToLoad, MyGuiScreenGamePlay screenToUnload)
			: this(screenToLoad, screenToUnload, null)
		{
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 vector = MyGuiManager.MeasureString(m_font, MyTexts.Get(MyCommonTexts.LoadingPleaseWaitUppercase), 1.1f);
			m_wheel = new MyGuiControlRotatingWheel(MyGuiConstants.LOADING_PLEASE_WAIT_POSITION - new Vector2(0f, 0.09f + vector.Y), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.36f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, m_rotatingWheelTexture, manualRotationUpdate: false, MyPerGameSettings.GUI.MultipleSpinningWheels);
			m_multiTextControl = new MyGuiControlMultilineText(contents: string.IsNullOrEmpty(m_customTextFromConstructor) ? new StringBuilder(m_currentText.ToString()) : new StringBuilder(m_customTextFromConstructor), position: Vector2.One * 0.5f, size: new Vector2(0.9f, 0.2f), backgroundColor: Vector4.One, font: m_font, textScale: 1f, textAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, drawScrollbarV: false, drawScrollbarH: false);
			m_multiTextControl.BorderEnabled = false;
			m_multiTextControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			m_multiTextControl.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			Controls.Add(m_wheel);
			RefreshText();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenLoading";
		}

		public override void LoadContent()
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenLoading.LoadContent - START");
			MySandboxGame.Log.IncreaseIndent();
			m_backgroundScreenTexture = (m_backgroundTextureFromConstructor ?? GetRandomBackgroundTexture());
			m_gameLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";
			if (m_screenToUnload != null)
			{
				m_screenToUnload.IsLoaded = false;
				m_screenToUnload.CloseScreenNow();
			}
			base.LoadContent();
			MyRenderProxy.LimitMaxQueueSize = true;
			if (m_screenToLoad != null && !m_loadInDrawFinished && m_loadFinished)
			{
				m_screenToLoad.State = MyGuiScreenState.OPENING;
				m_screenToLoad.LoadContent();
			}
			else
			{
				m_loadFinished = false;
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyGuiScreenLoading.LoadContent - END");
		}

		private static string GetRandomBackgroundTexture()
		{
			string str = MyUtils.GetRandomInt(MyPerGameSettings.GUI.LoadingScreenIndexRange.X, MyPerGameSettings.GUI.LoadingScreenIndexRange.Y + 1).ToString().PadLeft(3, '0');
			return "Textures\\GUI\\Screens\\loading_background_" + str + ".dds";
		}

		public override void UnloadContent()
		{
			if (m_backgroundScreenTexture != null)
			{
				MyRenderProxy.UnloadTexture(m_backgroundScreenTexture);
			}
			if (m_backgroundTextureFromConstructor != null)
			{
				MyRenderProxy.UnloadTexture(m_backgroundTextureFromConstructor);
			}
			if (m_backgroundScreenTexture != null)
			{
				MyRenderProxy.UnloadTexture(m_rotatingWheelTexture);
			}
			if (m_screenToLoad != null && !m_loadFinished && m_loadInDrawFinished)
			{
				m_screenToLoad.UnloadContent();
				m_screenToLoad.UnloadData();
				m_screenToLoad = null;
			}
			if (m_screenToLoad != null && !m_loadInDrawFinished)
			{
				m_screenToLoad.UnloadContent();
			}
			MyRenderProxy.LimitMaxQueueSize = false;
			base.UnloadContent();
			Static = null;
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			if (base.State == MyGuiScreenState.OPENED && !m_loadFinished)
			{
				MyHud.ScreenEffects.FadeScreen(0f);
				MyAudio.Static.Mute = true;
				MyAudio.Static.StopMusic();
				MyAudio.Static.ChangeGlobalVolume(0f, 0f);
				DrawLoading();
				if (m_screenToLoad != null)
				{
					MySandboxGame.Log.WriteLine("RunLoadingAction - START");
					RunLoad();
					MySandboxGame.Log.WriteLine("RunLoadingAction - END");
				}
				if (m_screenToLoad != null)
				{
					MyScreenManager.AddScreenNow(m_screenToLoad);
					m_screenToLoad.Update(hasFocus: false);
				}
				m_screenToLoad = null;
				m_loadFinished = true;
				m_wheel.ManualRotationUpdate = true;
			}
			m_streamingTimeout++;
			bool flag = Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated || MyMultiplayer.Static == null || !MyFakes.ENABLE_WAIT_UNTIL_MULTIPLAYER_READY || m_isStreamed || (MyFakes.LOADING_STREAMING_TIMEOUT_ENABLED && m_streamingTimeout >= STREAMING_TIMEOUT);
			if (m_loadFinished && MySandboxGame.IsGameReady && flag && MySandboxGame.AreClipmapsReady)
			{
				MyHud.ScreenEffects.FadeScreen(1f, 5f);
				if (!m_exceptionDuringLoad && this.OnScreenLoadingFinished != null)
				{
					this.OnScreenLoadingFinished();
					this.OnScreenLoadingFinished = null;
				}
				CloseScreenNow();
				DrawLoading();
			}
			return true;
		}

		private void RunLoad()
		{
			m_exceptionDuringLoad = false;
			try
			{
				m_screenToLoad.RunLoadingAction();
			}
			catch (MyLoadingNeedXMLException ex)
			{
				m_exceptionDuringLoad = true;
				if (OnLoadingXMLAllowed != null)
				{
					UnloadOnException(exitToMainMenu: false);
					if (MySandboxGame.Static.SuppressLoadingDialogs)
					{
						OnLoadingXMLAllowed();
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.LoadingNeedsXML), MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), null, null, null, null, delegate
						{
							OnLoadingXMLAllowed();
						}));
					}
				}
				else
				{
					OnLoadException(ex, new StringBuilder(ex.Message), 1.5f);
				}
			}
			catch (MyLoadingException ex2)
			{
				OnLoadException(ex2, new StringBuilder(ex2.Message), 1.5f);
				m_exceptionDuringLoad = true;
			}
			catch (Exception e)
			{
				OnLoadException(e, MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded));
				m_exceptionDuringLoad = true;
			}
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyInput.Static.EnableInput(enable: true);
			MyAudio.Static.Mute = false;
		}

		private void UnloadOnException(bool exitToMainMenu)
		{
			m_screenToLoad = null;
			if (MyGuiScreenGamePlay.Static != null)
			{
				MyGuiScreenGamePlay.Static.UnloadData();
				MyGuiScreenGamePlay.Static.UnloadContent();
			}
			MySandboxGame.IsUpdateReady = true;
			MySandboxGame.AreClipmapsReady = true;
			MySandboxGame.RenderTasksFinished = true;
			try
			{
				if (exitToMainMenu)
				{
					MySessionLoader.UnloadAndExitToMenu();
				}
				else
				{
					MySessionLoader.Unload();
				}
			}
			catch (Exception ex)
			{
				MySession.Static = null;
				MySandboxGame.Log.WriteLine("ERROR: failed unload after exception in loading !");
				MySandboxGame.Log.WriteLine(ex);
			}
		}

		private void OnLoadException(Exception e, StringBuilder errorText, float heightMultiplier = 1f)
		{
			MySandboxGame.Log.WriteLine("ERROR: Loading screen failed");
			MySandboxGame.Log.WriteLine(e);
			UnloadOnException(exitToMainMenu: true);
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, errorText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
			Vector2 value = myGuiScreenMessageBox.Size.Value;
			value.Y *= heightMultiplier;
			myGuiScreenMessageBox.Size = value;
			myGuiScreenMessageBox.RecreateControls(constructor: false);
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
				{
					m_currentText = MyLoadingScreenText.GetText(++m_currentTextIdx, forceGamepad: true);
					RefreshText();
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
				{
					m_currentText = MyLoadingScreenText.GetText(--m_currentTextIdx, forceGamepad: true);
					RefreshText();
				}
			}
		}

		public bool DrawLoading()
		{
			MyRenderProxy.AfterUpdate(null);
			MyRenderProxy.BeforeUpdate();
			DrawInternal();
			bool result = base.Draw();
			MyRenderProxy.AfterUpdate(null);
			MyRenderProxy.BeforeUpdate();
			return result;
		}

		private void DrawInternal()
		{
			Color color = new Color(255, 255, 255, 250);
			color.A = (byte)((float)(int)color.A * m_transitionAlpha);
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", fullscreenRectangle, Color.Black);
			MyGuiManager.GetSafeHeightFullScreenPictureSize(MyGuiConstants.LOADING_BACKGROUND_TEXTURE_REAL_SIZE, out Rectangle outRect);
			MyGuiManager.DrawSpriteBatch(m_backgroundScreenTexture, outRect, new Color(new Vector4(1f, 1f, 1f, m_transitionAlpha)));
			MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background_fade.dds", outRect, new Color(new Vector4(1f, 1f, 1f, m_transitionAlpha)));
			MyGuiSandbox.DrawGameLogoHandler(m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
			LastBackgroundTexture = m_backgroundScreenTexture;
			MyGuiManager.DrawString(m_font, MyTexts.Get(MyCommonTexts.LoadingPleaseWaitUppercase), MyGuiConstants.LOADING_PLEASE_WAIT_POSITION, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.1f, new Color(MyGuiConstants.LOADING_PLEASE_WAIT_COLOR * m_transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
			if (string.IsNullOrEmpty(m_customTextFromConstructor))
			{
				string font = m_font;
				Vector2 positionAbsoluteBottomLeft = m_multiTextControl.GetPositionAbsoluteBottomLeft();
				Vector2 textSize = m_multiTextControl.TextSize;
				Vector2 size = m_multiTextControl.Size;
				MyGuiManager.DrawString(normalizedCoord: positionAbsoluteBottomLeft + new Vector2((size.X - textSize.X) * 0.5f + 0.025f, 0.025f), font: font, text: m_authorWithDash, scale: MyGuiSandbox.GetDefaultTextScaleWithLanguage());
			}
			m_multiTextControl.Draw(1f, 1f);
		}

		public override bool Draw()
		{
			DrawInternal();
			return base.Draw();
		}

		private void RefreshText()
		{
			if (string.IsNullOrEmpty(m_customTextFromConstructor))
			{
				m_multiTextControl.TextEnum = MyStringId.GetOrCompute(m_currentText.ToString());
				if (m_currentText is MyLoadingScreenQuote)
				{
					m_authorWithDash.Clear().Append("- ").AppendStringBuilder(MyTexts.Get((m_currentText as MyLoadingScreenQuote).Author))
						.Append(" -");
				}
			}
		}

		public override void OnRemoved()
		{
			base.OnRemoved();
		}
	}
}
