using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Debug;
using EmptyKeys.UserInterface.Media;
using ParallelTasks;
using Sandbox;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Audio;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	public abstract class MyGuiScreenMvvmBase : MyGuiScreenBase
	{
		private DebugViewModel m_debug;

		protected UIRoot m_view;

		private MyViewModelBase m_viewModel;

		private int m_elapsedTime;

		private int m_previousTime;

		private bool m_layoutUpdated;

		public MyGuiScreenMvvmBase(MyViewModelBase viewModel)
			: base(new Vector2(0.5f, 0.5f))
		{
			base.EnabledBackgroundFade = true;
			m_closeOnEsc = true;
			m_drawEvenWithoutFocus = true;
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			m_viewModel = viewModel;
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			viewModel.MaxWidth = (float)safeGuiRectangle.Width * (1f / UIElement.DpiScaleX);
			MySession.Static.LocalCharacter.CharacterDied += OnCharacterDied;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			RecreateControls(constructor: false);
		}

		public abstract UIRoot CreateView();

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_view = CreateView();
			if (m_view == null)
			{
				throw new NullReferenceException("View is empty");
			}
			m_viewModel.BackgroundOverlay = new ColorW(1f, 1f, 1f, MySandboxGame.Config.UIBkOpacity);
			ImageManager.Instance.LoadImages(null);
			SoundSourceCollection defaultValue = new SoundSourceCollection
			{
				new SoundSource
				{
					SoundType = SoundType.ButtonsClick,
					SoundAsset = GuiSounds.MouseClick.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.ButtonsHover,
					SoundAsset = GuiSounds.MouseOver.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.CheckBoxHover,
					SoundAsset = GuiSounds.MouseOver.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.TabControlSelect,
					SoundAsset = GuiSounds.MouseClick.ToString()
				},
				new SoundSource
				{
					SoundType = SoundType.ListBoxSelect,
					SoundAsset = GuiSounds.MouseClick.ToString()
				}
			};
			SoundManager.SoundsProperty.DefaultMetadata.DefaultValue = defaultValue;
			SoundManager.Instance.AddSound(GuiSounds.MouseClick.ToString());
			SoundManager.Instance.AddSound(GuiSounds.MouseOver.ToString());
			SoundManager.Instance.AddSound(GuiSounds.Item.ToString());
			SoundManager.Instance.LoadSounds(null);
			m_view.DataContext = m_viewModel;
			Parallel.Start(delegate
			{
				m_view.UpdateLayout(0.0);
			}, delegate
			{
				m_layoutUpdated = true;
				m_viewModel.InitializeData();
			});
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (m_layoutUpdated)
			{
				m_view.UpdateInput(m_elapsedTime);
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		private void OnCharacterDied(MyCharacter character)
		{
			CloseScreen();
		}

		public override bool CloseScreen()
		{
			m_viewModel.OnScreenClosing();
			m_layoutUpdated = false;
			VisualTreeHelper.Instance.ClearParentCache();
			MySession.Static.LocalCharacter.CharacterDied -= OnCharacterDied;
			return base.CloseScreen();
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			if (!m_layoutUpdated)
			{
				return false;
			}
			m_elapsedTime = MySandboxGame.TotalTimeInMilliseconds - m_previousTime;
			m_view.UpdateLayout(m_elapsedTime);
			m_view.Draw(m_elapsedTime);
			m_previousTime = MySandboxGame.TotalTimeInMilliseconds;
			return true;
		}
	}
}
