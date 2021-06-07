using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public sealed class MyGuiScreenSimpleNewGame : MyGuiScreenBase
	{
		private class DataItem
		{
			public Action Action;

			public MyStringId CaptionText;

			public MyStringId DescriptionText;

			public string Texture;

			public DataItem(MyStringId captionText, MyStringId descriptionText, string texture, Action action)
			{
				Action = action;
				CaptionText = captionText;
				DescriptionText = descriptionText;
				Texture = texture;
			}
		}

		private class Item : MyGuiControlParent
		{
			private float m_currentScale;

			private MyGuiControlLabel m_text;

			private MyGuiControlImage m_image;

			private MyGuiControlImage m_upperBackground;

			private MyGuiControlParent m_lowerBackground;

			private Vector2 m_baseSize;

			private float m_baseCaptionSize;

			private float m_space;

			private Vector2 m_offset;

			private int m_dataIndex;

			private DataItem m_data;

			private Vector2 m_imageOffset = ITEM_IMAGE_POSITION;

			private Vector2 m_CaptionOffset = ITEM_CAPTION_POSITION;

			private Vector2 m_upperImageOffset = ITEM_UPPER_BACKGROUND_POSITION;

			public Action<Item> OnItemClicked;

			public Item(Vector2 size, float space, Vector2 offset)
			{
				m_baseSize = size;
				m_space = space;
				m_offset = offset;
				m_baseCaptionSize = ITEM_CAPTION_SCALE;
				base.Size = m_baseSize;
				base.Position = m_offset;
				base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				m_image = new MyGuiControlImage();
				m_image.SetTexture("Textures\\GUI\\Icons\\scenarios\\PreviewCustomWorld.jpg");
				m_image.Position = ITEM_IMAGE_POSITION;
				m_text = new MyGuiControlLabel
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
				};
				m_text.Size = m_text.GetTextSize();
				m_text.TextEnum = MyCommonTexts.SimpleNewGame_TheFirstJump;
				m_text.Position = m_CaptionOffset;
				m_lowerBackground = new MyGuiControlParent(m_image.Position, new Vector2(m_baseSize.X, ITEM_LOWER_BACKGROUND_HEIGHT))
				{
					BackgroundTexture = DARK_BACKGROUND_TEXTURE
				};
				m_upperBackground = new MyGuiControlImage(m_upperImageOffset, new Vector2(m_baseSize.X, ITEM_UPPER_BACKGROUND_HEIGHT));
				m_upperBackground.SetTexture("Textures\\GUI\\Controls\\TransGradient_ca.DDS");
				base.Controls.Add(m_lowerBackground);
				base.Controls.Add(m_upperBackground);
				base.Controls.Add(m_image);
				base.Controls.Add(m_text);
			}

			public void SetData(DataItem data, int index)
			{
				m_data = data;
				m_dataIndex = index;
				m_image.SetTexture(data.Texture);
				float num = MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
				float num2 = 0.935f;
				Vector2 value = new Vector2(1f, 0.5f);
				m_image.Size = new Vector2(base.Size.X * num2, num * base.Size.X * num2) * value;
				m_text.TextEnum = data.CaptionText;
			}

			public DataItem GetData()
			{
				return m_data;
			}

			public int GetDataIndex()
			{
				return m_dataIndex;
			}

			public void ActivateAction()
			{
				if (m_data != null && m_data.Action != null)
				{
					m_data.Action();
				}
			}

			public override MyGuiControlBase HandleInput()
			{
				if (MyInput.Static.IsPrimaryButtonPressed())
				{
					Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
					Vector2 size = GetPositionAbsoluteBottomRight() - positionAbsoluteTopLeft;
					if (new RectangleF(positionAbsoluteTopLeft, size).Contains(MyGuiManager.MouseCursorPosition) && OnItemClicked != null)
					{
						OnItemClicked(this);
					}
				}
				return base.HandleInput();
			}

			public void SetScale(float scale)
			{
				base.Size = new Vector2(m_baseSize.X * scale, m_baseSize.Y * scale);
				float num = MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
				float num2 = 0.935f;
				Vector2 value = new Vector2(1f, 0.5f);
				m_image.Size = new Vector2(base.Size.X * num2, num * base.Size.X * num2) * value;
				m_text.Size = m_text.GetTextSize() * scale;
				m_text.TextScale = m_baseCaptionSize * scale;
				m_image.Position = ITEM_IMAGE_POSITION * scale;
				m_text.Position = m_CaptionOffset * scale;
				m_lowerBackground.Size = new Vector2(m_baseSize.X, ITEM_LOWER_BACKGROUND_HEIGHT) * scale;
				m_lowerBackground.Position = m_image.Position;
				m_upperBackground.Size = new Vector2(m_baseSize.X, ITEM_UPPER_BACKGROUND_HEIGHT) * scale;
				m_upperBackground.Position = m_upperImageOffset * scale;
			}

			public void SetOpacity(float opacity)
			{
				Vector4 colorMask = new Vector4(opacity);
				m_image.ColorMask = colorMask;
				m_lowerBackground.ColorMask = colorMask;
				m_upperBackground.ColorMask = colorMask;
				m_text.ColorMask = colorMask;
			}

			public void SetPosition(Vector2 position)
			{
				base.Position = position + m_offset;
			}
		}

		private static MyGuiCompositeTexture BUTTON_TEXTURE_LEFT = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\LeftArrow_ca.dds");

		private static MyGuiCompositeTexture BUTTON_TEXTURE_RIGHT = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\RightArrow_ca.dds");

		private static MyGuiControlImageButton.StyleDefinition STYLE_BUTTON_LEFT = new MyGuiControlImageButton.StyleDefinition
		{
			Active = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Disabled = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Normal = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Highlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			ActiveHighlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_LEFT
			},
			Padding = new MyGuiBorderThickness(0.005f, 0.005f)
		};

		private static MyGuiControlImageButton.StyleDefinition STYLE_BUTTON_RIGHT = new MyGuiControlImageButton.StyleDefinition
		{
			Active = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Disabled = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Normal = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Highlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			ActiveHighlight = new MyGuiControlImageButton.StateDefinition
			{
				Texture = BUTTON_TEXTURE_RIGHT
			},
			Padding = new MyGuiBorderThickness(0.005f, 0.005f)
		};

		private static readonly float ITEM_SPACING = 0.02f;

		private static readonly Vector2 ITEM_SIZE = new Vector2(0.3f, 0.55f);

		private static readonly Vector2 ITEM_POSITION_OFFSET = new Vector2(0f, -0.087f);

		private static readonly Vector2 ITEM_IMAGE_POSITION = new Vector2(0f, 0.166f);

		private static readonly Vector2 ITEM_CAPTION_POSITION = new Vector2(0f, 0.015f);

		private static readonly Vector2 ITEM_UPPER_BACKGROUND_POSITION = new Vector2(0f, -0.2f);

		private static readonly float ITEM_LOWER_BACKGROUND_HEIGHT = 0.218f;

		private static readonly float ITEM_UPPER_BACKGROUND_HEIGHT = 0.5f;

		private static readonly float START_BUTTON_HEIGHT = 0.06f;

		private static readonly float ITEM_CAPTION_SCALE = 1.15f;

		private static readonly MyGuiCompositeTexture DARK_BACKGROUND_TEXTURE = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\DarkBlueBackground.png");

		private static readonly float DEFAULT_OPACITY = 0.92f;

		private readonly int ITEM_COUNT = 9;

		private readonly float SHIFT_SPEED = 0.045f;

		private DataItem m_activeItem;

		private int m_activeIndex;

		private int m_nextIndex;

		private List<DataItem> m_items = new List<DataItem>();

		private List<Item> m_guiItems = new List<Item>();

		private int m_activeGuiItem;

		private MyGuiControlButton m_buttonStart;

		private MyGuiControlImageButton m_buttonLeft;

		private MyGuiControlImageButton m_buttonRight;

		private DataItem m_campaignTheFirstJump;

		private DataItem m_campaignLearningToSurvive;

		private DataItem m_campaignNeverSurrender;

		private MyGuiControlMultilineText m_description;

		private float m_animationValueCurrent;

		private float m_animationLinearCurrent;

		private float m_animationLinearNext;

		private float m_animationSpeed;

		private float m_animationDelinearizingValue;

		private int m_guiItemsMiddle;

		private bool IsAnimating
		{
			get
			{
				if (m_animationLinearCurrent == m_animationLinearNext)
				{
					return m_animationSpeed != 0f;
				}
				return true;
			}
		}

		public MyGuiScreenSimpleNewGame()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8f, 0.7f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
			m_backgroundColor = Color.Transparent;
			m_backgroundFadeColor = Vector4.Zero;
			SetVideoOverlayColor(new Vector4(0f, 0f, 0f, 1f));
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyObjectBuilder_Campaign leaningToSurvive = null;
			MyObjectBuilder_Campaign neverSurrender = null;
			MyObjectBuilder_Campaign campaign = null;
			foreach (MyObjectBuilder_Campaign campaign2 in MyCampaignManager.Static.Campaigns)
			{
				switch (campaign2.Name)
				{
				case "The First Jump":
					campaign = campaign2;
					break;
				case "Learning to Survive":
					leaningToSurvive = campaign2;
					break;
				case "Never Surrender":
					neverSurrender = campaign2;
					break;
				}
			}
			m_campaignTheFirstJump = AddItem(MyCommonTexts.SimpleNewGame_TheFirstJump, MyCommonTexts.SimpleNewGame_TheFirstJump_Description, campaign.ImagePath, delegate
			{
				MySandboxGame.Config.CampaignStarted_TheFirstJump = true;
				MySandboxGame.Config.Save();
				StartScenario(campaign, preferOnline: false);
			});
			m_campaignLearningToSurvive = AddItem(MyCommonTexts.SimpleNewGame_LearningToSurvive, MyCommonTexts.SimpleNewGame_LearningToSurvive_Description, leaningToSurvive.ImagePath, delegate
			{
				MySandboxGame.Config.CampaignStarted_LearningToSurvive = true;
				MySandboxGame.Config.Save();
				StartScenario(leaningToSurvive, preferOnline: false);
			});
			m_campaignNeverSurrender = AddItem(MyCommonTexts.SimpleNewGame_NeverSurrender, MyCommonTexts.SimpleNewGame_NeverSurrender_Description, neverSurrender.ImagePath, delegate
			{
				MySandboxGame.Config.CampaignStarted_NeverSurrender = true;
				MySandboxGame.Config.Save();
				StartScenario(neverSurrender, MyFakes.PREFER_ONLINE);
			});
			AddWorld(MyCommonTexts.SimpleNewGame_Creative, MyCommonTexts.SimpleNewGame_Creative_Description, MyGameModeEnum.Creative, MyFakes.PREFER_ONLINE, "Red Ship");
			AddItem(MyCommonTexts.SimpleNewGame_Workshop, MyCommonTexts.WorkshopScreen_Description, "Textures\\GUI\\Icons\\Workshop.jpg", delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenNewWorkshopGame());
			});
			AddItem(MyCommonTexts.SimpleNewGame_Custom, MyCommonTexts.WorldSettingsScreen_Description, "Textures\\GUI\\Icons\\scenarios\\PreviewAlienPlanet.jpg", delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenWorldSettings());
			});
			Vector2 size = new Vector2(ITEM_SIZE.X, START_BUTTON_HEIGHT);
			new Vector2(0.03f, 0.04f);
			Vector2 value = new Vector2(0.0017f, 0.225000009f);
			m_buttonStart = new MyGuiControlButton(value + new Vector2(0f, 0.0025f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.SimpleNewGame_Start));
			m_buttonStart.VisualStyle = MyGuiControlButtonStyleEnum.RectangularBorderLess;
			m_buttonStart.Size = size;
			m_buttonStart.ButtonClicked += OnStartClicked;
			m_buttonStart.ColorMask = new Vector4(DEFAULT_OPACITY);
			m_buttonStart.TextScale = 1.65f;
			m_buttonStart.BorderSize = 0;
			m_buttonStart.BorderEnabled = false;
			float num = 0.75f * START_BUTTON_HEIGHT;
			float sTART_BUTTON_HEIGHT = START_BUTTON_HEIGHT;
			Vector2 value2 = new Vector2(0.01f + 0.5f * num, 0f);
			BUTTON_TEXTURE_LEFT.MarkDirty();
			BUTTON_TEXTURE_RIGHT.MarkDirty();
			m_buttonLeft = new MyGuiControlImageButton("Button", value - (new Vector2(0.5f * m_buttonStart.Size.X, 0f) + value2))
			{
				CanHaveFocus = false
			};
			m_buttonLeft.Text = string.Empty;
			m_buttonLeft.ApplyStyle(STYLE_BUTTON_LEFT);
			m_buttonLeft.Size = new Vector2(num, sTART_BUTTON_HEIGHT);
			m_buttonLeft.ButtonClicked += OnLeftClicked;
			m_buttonLeft.ColorMask = new Vector4(DEFAULT_OPACITY);
			m_buttonRight = new MyGuiControlImageButton("Button", value + (new Vector2(0.5f * m_buttonStart.Size.X, 0f) + value2))
			{
				CanHaveFocus = false
			};
			m_buttonRight.Text = string.Empty;
			m_buttonRight.ApplyStyle(STYLE_BUTTON_RIGHT);
			m_buttonRight.Size = new Vector2(num, sTART_BUTTON_HEIGHT);
			m_buttonRight.ButtonClicked += OnRightClicked;
			m_buttonRight.ColorMask = new Vector4(DEFAULT_OPACITY);
			Controls.Add(m_buttonStart);
			Controls.Add(m_buttonLeft);
			Controls.Add(m_buttonRight);
			Vector2 value3 = new Vector2(0f, 0.35f);
			m_description = new MyGuiControlMultilineText(size: new Vector2(0.7f, 0.1f), position: value3)
			{
				BackgroundTexture = DARK_BACKGROUND_TEXTURE,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
			};
			m_description.TextScale = 0.65f;
			m_description.TextPadding = new MyGuiBorderThickness(0.01f, 0.01f);
			for (int i = 0; i < m_items.Count; i++)
			{
				Item item = BuildGuiItem();
				m_guiItems.Add(item);
			}
			m_guiItemsMiddle = 0;
			Controls.Add(m_description);
			InitialWorldSelection();
			void AddWorld(MyStringId captionText, MyStringId descriptionText, MyGameModeEnum mode, bool preferOnline, string world)
			{
				string worldPath = Path.Combine(MyFileSystem.ContentPath, "CustomWorlds", world);
				AddItem(captionText, descriptionText, Path.Combine(worldPath, "thumb.jpg"), delegate
				{
					StartWorld(worldPath, mode, preferOnline);
				});
			}
		}

		private void InitialWorldSelection()
		{
			if (!MySandboxGame.Config.CampaignStarted_TheFirstJump && m_campaignTheFirstJump != null)
			{
				ResetActiveItem(m_campaignTheFirstJump);
			}
			else if (!MySandboxGame.Config.CampaignStarted_LearningToSurvive && m_campaignLearningToSurvive != null)
			{
				ResetActiveItem(m_campaignLearningToSurvive);
			}
			else if (!MySandboxGame.Config.CampaignStarted_NeverSurrender && m_campaignNeverSurrender != null)
			{
				ResetActiveItem(m_campaignNeverSurrender);
			}
			else
			{
				ResetActiveIndex(0);
			}
		}

		private void OnStartClicked(MyGuiControlButton obj)
		{
			if (m_activeItem != null)
			{
				m_activeItem.Action();
			}
		}

		private void OnLeftClicked(MyGuiControlImageButton obj)
		{
			int amount = 1;
			ShiftItems(amount, SHIFT_SPEED);
		}

		private void OnRightClicked(MyGuiControlImageButton obj)
		{
			int amount = -1;
			ShiftItems(amount, SHIFT_SPEED);
		}

		private DataItem AddItem(MyStringId captionText, MyStringId descriptionText, string texture, Action action)
		{
			DataItem dataItem = new DataItem(captionText, descriptionText, texture, action);
			m_items.Add(dataItem);
			return dataItem;
		}

		private void StartScenario(MyObjectBuilder_Campaign scenario, bool preferOnline)
		{
			MyCampaignManager.Static.SwitchCampaign(scenario.Name, scenario.IsVanilla, scenario.PublishedFileId, scenario.ModFolderPath);
			if (!preferOnline || !MyGameService.IsActive)
			{
				MyCampaignManager.Static.RunNewCampaign(scenario.Name, MyOnlineModeEnum.OFFLINE, MyMultiplayerLobby.MAX_PLAYERS);
			}
			else
			{
				MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: false, delegate(bool granted)
				{
					MyCampaignManager.Static.RunNewCampaign(scenario.Name, granted ? MyOnlineModeEnum.FRIENDS : MyOnlineModeEnum.OFFLINE, MyMultiplayerLobby.MAX_PLAYERS);
				});
			}
		}

		private void StartWorld(string sessionPath, MyGameModeEnum gameMode, bool preferOnline)
		{
			if (!preferOnline || !MyGameService.IsActive)
			{
				StartWorld(sessionPath, gameMode, MyOnlineModeEnum.OFFLINE);
			}
			else
			{
				MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: false, delegate(bool granted)
				{
					StartWorld(sessionPath, gameMode, granted ? MyOnlineModeEnum.FRIENDS : MyOnlineModeEnum.OFFLINE);
				});
			}
		}

		private void StartWorld(string sessionPath, MyGameModeEnum gameMode, MyOnlineModeEnum onlineMode)
		{
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
			if (checkpoint != null)
			{
				checkpoint.Settings.GameMode = gameMode;
				checkpoint.Settings.OnlineMode = onlineMode;
				checkpoint.Settings.VoxelGeneratorVersion = MyFakes.DEFAULT_PROCEDURAL_ASTEROID_GENERATOR;
				MySessionLoader.LoadSingleplayerSession(checkpoint, sessionPath, sizeInBytes, delegate
				{
					MyAsyncSaving.Start(null, Path.Combine(MyFileSystem.SavesPath, checkpoint.SessionName.Replace(':', '-')));
				});
			}
		}

		private void ResetActiveIndex(int i)
		{
			if (m_items.Count > i && i >= 0)
			{
				SetActiveIndex(i);
				BindItemsToGUI();
			}
		}

		private void ResetActiveItem(DataItem item)
		{
			BindItemsToGUI();
			int num = -1;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i] == item)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				SetActiveIndex(num);
				m_guiItemsMiddle = num;
			}
		}

		private void SetActiveIndex(int i)
		{
			if (m_items.Count > i && i >= 0)
			{
				m_activeIndex = i;
				m_activeItem = m_items[i];
				m_description.Text = MyTexts.Get(m_activeItem.DescriptionText);
				m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				if (i <= 0)
				{
					m_buttonLeft.Visible = false;
				}
				else
				{
					m_buttonLeft.Visible = true;
				}
				if (i >= m_items.Count - 1)
				{
					m_buttonRight.Visible = false;
				}
				else
				{
					m_buttonRight.Visible = true;
				}
			}
		}

		private void BindItemsToGUI()
		{
			int num = Math.Min(m_items.Count, m_guiItems.Count);
			for (int i = 0; i < num; i++)
			{
				m_guiItems[i].SetData(m_items[i], i);
			}
		}

		private void ShiftItems(int amount, float speed)
		{
			if (m_activeIndex - amount < 0 || m_activeIndex - amount >= m_items.Count)
			{
				return;
			}
			if (IsAnimating)
			{
				if (Math.Sign(amount) == Math.Sign(m_animationLinearNext))
				{
					float num = m_animationLinearNext + ((amount > 0) ? 1f : (-1f));
					m_animationSpeed = m_animationSpeed * (m_animationLinearCurrent / m_animationLinearNext) + speed;
					m_animationLinearNext = Math.Sign(num - m_animationLinearCurrent);
					m_animationDelinearizingValue = Math.Abs(num - m_animationValueCurrent);
					m_animationLinearCurrent = 0f;
					int activeIndex = (m_activeIndex + -amount % m_items.Count + m_items.Count) % m_items.Count;
					SetActiveIndex(activeIndex);
				}
			}
			else
			{
				m_animationDelinearizingValue = Math.Abs(amount);
				m_animationSpeed = speed;
				m_animationLinearCurrent = 0f;
				m_animationLinearNext = ((amount > 0) ? 1f : (-1f));
				int activeIndex2 = (m_activeIndex + -amount % m_items.Count + m_items.Count) % m_items.Count;
				SetActiveIndex(activeIndex2);
			}
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (IsAnimating)
			{
				if (m_animationLinearCurrent < m_animationLinearNext)
				{
					if (m_animationLinearCurrent + m_animationSpeed >= m_animationLinearNext)
					{
						m_animationLinearCurrent = (m_animationLinearNext = 0f);
						m_animationSpeed = 0f;
						m_animationValueCurrent = (float)Math.Round(m_animationValueCurrent);
					}
					else
					{
						float num = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationLinearCurrent += m_animationSpeed;
						float num2 = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationValueCurrent += m_animationDelinearizingValue * (num2 - num);
					}
				}
				else if (m_animationLinearCurrent > m_animationLinearNext)
				{
					if (m_animationLinearCurrent - m_animationSpeed <= m_animationLinearNext)
					{
						m_animationLinearCurrent = (m_animationLinearNext = 0f);
						m_animationSpeed = 0f;
						m_animationValueCurrent = (float)Math.Round(m_animationValueCurrent);
					}
					else
					{
						float num3 = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationLinearCurrent -= m_animationSpeed;
						float num4 = RescaleTransitionSineSymmetric(m_animationLinearCurrent);
						m_animationValueCurrent += m_animationDelinearizingValue * (num4 - num3);
					}
				}
			}
			if (m_animationValueCurrent <= -1f)
			{
				m_animationValueCurrent += 1f;
				m_guiItemsMiddle++;
			}
			else if (m_animationValueCurrent >= 1f)
			{
				m_animationValueCurrent -= 1f;
				m_guiItemsMiddle--;
			}
			for (int i = 0; i < m_guiItems.Count; i++)
			{
				float num5 = ComputeScale((float)i + m_animationValueCurrent);
				Vector2 position = ComputePosition((float)i + m_animationValueCurrent, num5);
				m_guiItems[i].SetScale(num5);
				m_guiItems[i].SetOpacity(num5 * DEFAULT_OPACITY);
				m_guiItems[i].SetPosition(position);
			}
			return result;
		}

		private void AddItemToStartRemoveFromEnd()
		{
			int previoudIdx = 0;
			GetDataItemPrevious(m_guiItems[0], out DataItem previousData, out previoudIdx);
			Item item = BuildGuiItem(previousData);
			item.SetData(previousData, previoudIdx);
			m_guiItems.Insert(0, item);
			Controls.Remove(m_guiItems[m_guiItems.Count - 1]);
			m_guiItems.RemoveAt(m_guiItems.Count - 1);
		}

		private void AddItemToEndRemoveFromStart()
		{
			int nextIdx = 0;
			GetDataItemNext(m_guiItems[m_guiItems.Count - 1], out DataItem nextData, out nextIdx);
			Item item = BuildGuiItem(nextData);
			item.SetData(nextData, nextIdx);
			m_guiItems.Insert(m_guiItems.Count, item);
			Controls.Remove(m_guiItems[0]);
			m_guiItems.RemoveAt(0);
		}

		private void GetDataItemPrevious(Item item, out DataItem previousData, out int previoudIdx)
		{
			int num = (item.GetDataIndex() + m_items.Count - 1) % m_items.Count;
			previousData = m_items[num];
			previoudIdx = num;
		}

		private void GetDataItemNext(Item item, out DataItem nextData, out int nextIdx)
		{
			int num = (item.GetDataIndex() + 1) % m_items.Count;
			nextData = m_items[num];
			nextIdx = num;
		}

		private Item BuildGuiItem(DataItem data = null)
		{
			Item item = new Item(ITEM_SIZE, ITEM_SPACING, ITEM_POSITION_OFFSET);
			item.OnItemClicked = (Action<Item>)Delegate.Combine(item.OnItemClicked, new Action<Item>(OnItemClicked));
			Controls.Add(item);
			return item;
		}

		private void OnItemClicked(Item item)
		{
			if (IsAnimating)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < m_guiItems.Count; i++)
			{
				if (m_guiItems[i] == item)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				int num2 = num - m_guiItemsMiddle;
				if (num2 != 0)
				{
					ShiftItems(-num2, SHIFT_SPEED);
				}
			}
		}

		public float RescaleTransitionSineSymmetric(float input)
		{
			return (float)Math.Sign(input) * RescaleTransitionSine(Math.Abs(input));
		}

		public float RescaleTransitionSine(float input)
		{
			return (float)Math.Sin((double)input * Math.PI * 0.5);
		}

		public float ComputeScale(float coef)
		{
			float value = coef - (float)m_guiItemsMiddle;
			return Math.Max(1f - 0.2f * Math.Abs(value), 0f);
		}

		public Vector2 ComputePosition(float coef, float scale)
		{
			float num = coef - (float)m_guiItemsMiddle;
			float num2 = 0.2f * num;
			float x = 0.85f * num2 * (1.22f + 0.635f * scale * scale);
			float y = 0.128f * (1f - scale);
			return new Vector2(x, y);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_LEFT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_LEFT) || MyInput.Static.IsNewKeyPressed(MyKeys.Left))
			{
				int amount = 1;
				ShiftItems(amount, SHIFT_SPEED);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_RIGHT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_RIGHT) || MyInput.Static.IsNewKeyPressed(MyKeys.Right))
			{
				int amount2 = -1;
				ShiftItems(amount2, SHIFT_SPEED);
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			MyGuiSandbox.DrawGameLogoHandler(m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
			return result;
		}

		public override string GetFriendlyName()
		{
			return "SimpleNewGame";
		}

		public override bool CloseScreen()
		{
			SetVideoOverlayColor(new Vector4(1f, 1f, 1f, 1f));
			return base.CloseScreen();
		}

		private void SetVideoOverlayColor(Vector4 color)
		{
			MyGuiScreenIntroVideo firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>();
			if (firstScreenOfType != null)
			{
				firstScreenOfType.OverlayColorMask = color;
			}
		}
	}
}
