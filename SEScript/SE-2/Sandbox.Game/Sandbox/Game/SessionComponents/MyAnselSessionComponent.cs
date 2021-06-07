using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game.Components;
using VRage.Input;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyAnselSessionComponent : MySessionComponentBase
	{
		private struct MyScreenTool
		{
			private bool m_isHidden;

			private MyAnselGuiScreen m_anselGuiScreen;

			private bool m_prevMinimalHud;

			public void Init()
			{
				m_anselGuiScreen = new MyAnselGuiScreen();
			}

			public void Hide()
			{
				if (!m_isHidden)
				{
					m_prevMinimalHud = MyHud.MinimalHud;
					MyHud.MinimalHud = true;
					MyGuiSandbox.AddScreen(m_anselGuiScreen);
					m_isHidden = true;
				}
			}

			public void Restore()
			{
				if (m_isHidden)
				{
					MyGuiSandbox.RemoveScreen(m_anselGuiScreen);
					MyHud.MinimalHud = m_prevMinimalHud;
					m_isHidden = false;
				}
			}
		}

		private MyScreenTool m_screenTool;

		private bool m_isSessionRunning;

		private readonly MyNullInput m_nullInput = new MyNullInput();

		private bool m_prevHeadRenderingEnabled;

		private IMyInput m_prevInput;

		private IAnsel m_ansel;

		private void SessionStarted()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				m_prevHeadRenderingEnabled = localCharacter.HeadRenderingEnabled;
				localCharacter.EnableHead(enabled: true);
			}
			m_prevInput = MyInput.Static;
			MyInput.Static = m_nullInput;
			if (m_ansel.IsGamePausable)
			{
				MySandboxGame.PausePush();
			}
			MyRenderProxy.SetFrameTimeStep(-1f);
		}

		private void SessionEnded()
		{
			if (m_ansel != null && m_ansel.IsGamePausable)
			{
				MySandboxGame.PausePop();
			}
			MyInput.Static = m_prevInput;
			MySession.Static.LocalCharacter?.EnableHead(m_prevHeadRenderingEnabled);
			MyRenderProxy.SetFrameTimeStep();
		}

		public override void LoadData()
		{
			m_screenTool.Init();
			m_ansel = MyVRage.Platform.Ansel;
			m_ansel.IsGamePausable = !Sync.MultiplayerActive;
			m_ansel.IsSessionEnabled = true;
			if (!MyFakes.ENABLE_ANSEL_IN_MULTIPLAYER)
			{
				m_ansel.IsSessionEnabled = !Sync.MultiplayerActive;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_ansel.IsSessionEnabled = false;
			if (m_isSessionRunning)
			{
				m_ansel.StopSession();
				m_ansel = null;
				SessionEnded();
			}
		}

		public override void Draw()
		{
			base.Draw();
			bool isSessionRunning = m_ansel.IsSessionRunning;
			if (isSessionRunning != m_isSessionRunning)
			{
				if (m_isSessionRunning)
				{
					SessionEnded();
					m_screenTool.Restore();
				}
				else
				{
					SessionStarted();
				}
				m_isSessionRunning = isSessionRunning;
			}
			if (isSessionRunning)
			{
				if (m_ansel.IsOverlayEnabled)
				{
					m_screenTool.Restore();
				}
				else
				{
					m_screenTool.Hide();
				}
			}
		}
	}
}
