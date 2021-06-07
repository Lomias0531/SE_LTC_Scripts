using Sandbox.Engine.Utils;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenIntroVideo : MyGuiScreenBase
	{
		private struct Subtitle
		{
			public TimeSpan StartTime;

			public TimeSpan Length;

			public StringBuilder Text;

			public Subtitle(int startMs, int lengthMs, MyStringId textEnum)
			{
				StartTime = TimeSpan.FromMilliseconds(startMs);
				Length = TimeSpan.FromMilliseconds(lengthMs);
				Text = MyTexts.Get(textEnum);
			}
		}

		private uint m_videoID = uint.MaxValue;

		private bool m_playbackStarted;

		private string[] m_videos;

		private string m_currentVideo = "";

		private List<Subtitle> m_subtitles = new List<Subtitle>();

		private int m_currentSubtitleIndex;

		private float m_volume = 1f;

		private int m_transitionTime = 300;

		private Vector4 m_colorMultiplier = Vector4.One;

		private static readonly string m_videoOverlay = "Textures\\GUI\\Screens\\main_menu_overlay.dds";

		private bool m_loop = true;

		private bool m_videoOverlayEnabled = true;

		public Vector4 OverlayColorMask
		{
			get;
			set;
		}

		public MyGuiScreenIntroVideo(string[] videos, uint videoId)
			: this(videos, loop: false, videoOverlayEnabled: false, canHaveFocus: true, 1f, closeOnEsc: true, 300, videoId)
		{
			MyRenderProxy.Settings.RenderThreadHighPriority = true;
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
		}

		public MyGuiScreenIntroVideo(string[] videos, bool loop, bool videoOverlayEnabled, bool canHaveFocus, float volume, bool closeOnEsc, int transitionTime, uint videoId = 0u)
			: base(Vector2.Zero)
		{
			base.DrawMouseCursor = false;
			base.CanHaveFocus = canHaveFocus;
			m_closeOnEsc = closeOnEsc;
			m_drawEvenWithoutFocus = true;
			m_videos = videos;
			m_videoOverlayEnabled = videoOverlayEnabled;
			m_loop = loop;
			m_volume = volume;
			m_transitionTime = transitionTime;
			if (videos == null && videoId != 0)
			{
				m_playbackStarted = true;
				m_videoID = videoId;
			}
			OverlayColorMask = new Vector4(1f, 1f, 1f, 1f);
		}

		public static MyGuiScreenIntroVideo CreateBackgroundScreen()
		{
			return new MyGuiScreenIntroVideo(MyPerGameSettings.GUI.MainMenuBackgroundVideos, loop: true, videoOverlayEnabled: true, canHaveFocus: false, 0f, closeOnEsc: false, 1500);
		}

		private static void AddCloseEvent(Action onVideoFinished, MyGuiScreenIntroVideo result)
		{
			result.Closed += delegate
			{
				onVideoFinished();
			};
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenIntroVideo";
		}

		private void LoadRandomVideo()
		{
			int randomInt = MyUtils.GetRandomInt(0, m_videos.Length);
			if (m_videos.Length != 0)
			{
				m_currentVideo = m_videos[randomInt];
			}
		}

		public override void LoadContent()
		{
			if (m_videos != null)
			{
				m_playbackStarted = false;
				LoadRandomVideo();
			}
			base.LoadContent();
		}

		public override void CloseScreenNow()
		{
			if (base.State != MyGuiScreenState.CLOSED)
			{
				UnloadContent();
			}
			MyRenderProxy.Settings.RenderThreadHighPriority = false;
			Thread.CurrentThread.Priority = ThreadPriority.Normal;
			base.CloseScreenNow();
		}

		private void CloseVideo()
		{
			if (m_videoID != uint.MaxValue)
			{
				MyRenderProxy.CloseVideo(m_videoID);
				m_videoID = uint.MaxValue;
			}
		}

		public override void UnloadContent()
		{
			CloseVideo();
			m_currentVideo = "";
			base.UnloadContent();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewLeftMousePressed() || MyInput.Static.IsNewRightMousePressed() || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CUTSCENE_SKIPPER, MyControlStateType.PRESSED))
			{
				Canceling();
			}
		}

		private void Loop()
		{
			m_currentSubtitleIndex = 0;
			LoadRandomVideo();
			TryPlayVideo();
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			if (!m_playbackStarted)
			{
				TryPlayVideo();
				m_playbackStarted = true;
			}
			else
			{
				if (MyRenderProxy.IsVideoValid(m_videoID) && MyRenderProxy.GetVideoState(m_videoID) != 0)
				{
					if (m_loop)
					{
						Loop();
					}
					else
					{
						CloseScreen();
					}
				}
				if (base.State == MyGuiScreenState.CLOSING && MyRenderProxy.IsVideoValid(m_videoID))
				{
					MyRenderProxy.SetVideoVolume(m_videoID, m_transitionAlpha);
				}
			}
			return true;
		}

		public override int GetTransitionOpeningTime()
		{
			return m_transitionTime;
		}

		public override int GetTransitionClosingTime()
		{
			return m_transitionTime;
		}

		private void TryPlayVideo()
		{
			if (MyFakes.ENABLE_VIDEO_PLAYER)
			{
				CloseVideo();
				string text = Path.Combine(MyFileSystem.ContentPath, m_currentVideo);
				if (File.Exists(text))
				{
					m_videoID = MyRenderProxy.PlayVideo(text, m_volume);
				}
			}
		}

		public override bool CloseScreen()
		{
			bool num = base.CloseScreen();
			MyRenderProxy.Settings.RenderThreadHighPriority = false;
			Thread.CurrentThread.Priority = ThreadPriority.Normal;
			if (num)
			{
				CloseVideo();
			}
			return num;
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			if (MyRenderProxy.IsVideoValid(m_videoID))
			{
				MyRenderProxy.UpdateVideo(m_videoID);
				Vector4 vector = m_colorMultiplier * m_transitionAlpha;
				MyRenderProxy.DrawVideo(m_videoID, MyGuiManager.GetSafeFullscreenRectangle(), new Color(vector), MyVideoRectangleFitMode.AutoFit);
			}
			if (m_videoOverlayEnabled)
			{
				DrawVideoOverlay();
			}
			return true;
		}

		private void DrawVideoOverlay()
		{
			MyGuiManager.DrawSpriteBatch(m_videoOverlay, MyGuiManager.GetSafeFullscreenRectangle(), OverlayColorMask * m_transitionAlpha);
		}
	}
}
