using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ObjectBuilders;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
	public class MyMultiTextPanelComponent : IMyTextSurfaceProvider
	{
		private List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();

		private MyRenderComponentScreenAreas m_render;

		private MyTerminalBlock m_block;

		private int m_selectedPanel;

		private bool m_isOutofRange;

		private Action<int, int[]> m_addImagesToSelectionRequest;

		private Action<int, int[]> m_removeImagesFromSelectionRequest;

		private Action<int, string> m_changeTextRequest;

		private Action<int, MySerializableSpriteCollection> m_updateSpriteCollection;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_panels.Count != 0)
				{
					return m_panels[m_selectedPanel];
				}
				return null;
			}
		}

		public int SurfaceCount
		{
			get
			{
				if (m_panels == null)
				{
					return 0;
				}
				return m_panels.Count;
			}
		}

		public int SelectedPanelIndex => m_selectedPanel;

		int IMyTextSurfaceProvider.SurfaceCount => SurfaceCount;

		public static void CreateTerminalControls<T>() where T : MyTerminalBlock, IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner
		{
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("PanelList", MyStringId.GetOrCompute("LCD Panels"), MySpaceTexts.Blank)
			{
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					FillPanels(x.MultiTextPanel, list1, list2);
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.SelectPanel(y);
				},
				Visible = ((T x) => x.SurfaceCount > 1),
				Enabled = ((T x) => x.SurfaceCount > 0)
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("Content", MySpaceTexts.BlockPropertyTitle_PanelContent, MySpaceTexts.Blank)
			{
				Visible = ((T x) => x.SurfaceCount > 0),
				Enabled = ((T x) => x.SurfaceCount > 0),
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					MyTextPanelComponent.FillContentComboBoxContent(x);
				},
				Getter = ((T x) => (long)((x.PanelComponent == null) ? ContentType.NONE : x.PanelComponent.ContentType)),
				Setter = delegate(T x, long y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.ContentType = (ContentType)y;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType != ContentType.NONE),
				Enabled = ((T x) => x.SurfaceCount > 0)
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("Script", MySpaceTexts.BlockPropertyTitle_PanelScript, MySpaceTexts.Blank)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT),
				Enabled = ((T x) => x.SurfaceCount > 0),
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FillScriptsContent(list1, list2);
					}
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.SelectScriptToDraw(y);
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("ScriptForegroundColor", MySpaceTexts.BlockPropertyTitle_FontColor)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT),
				Enabled = ((T x) => x.SurfaceCount > 0),
				Getter = ((T x) => (x.PanelComponent == null) ? Color.White : x.PanelComponent.ScriptForegroundColor),
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.ScriptForegroundColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("ScriptBackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT),
				Enabled = ((T x) => x.SurfaceCount > 0),
				Getter = ((T x) => (x.PanelComponent == null) ? Color.Black : x.PanelComponent.ScriptBackgroundColor),
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.ScriptBackgroundColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("ShowTextPanel", MySpaceTexts.BlockPropertyTitle_TextPanelShowPublicTextPanel, MySpaceTexts.Blank, delegate(T x)
			{
				x.OpenWindow(isEditable: true, sync: true, isPublic: true);
			})
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0 && !x.IsTextPanelOpen),
				SupportsMultipleBlocks = false
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("Font", MySpaceTexts.BlockPropertyTitle_Font, MySpaceTexts.Blank)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0),
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					MyTextPanelComponent.FillFontComboBoxContent(x);
				},
				Getter = ((T x) => (x.PanelComponent == null) ? 0 : ((int)x.PanelComponent.Font.SubtypeId)),
				Setter = delegate(T x, long y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.Font = new MyDefinitionId(typeof(MyObjectBuilder_FontDefinition), MyStringHash.TryGet((int)y));
					}
				}
			});
			MyTerminalControlSlider<T> myTerminalControlSlider = new MyTerminalControlSlider<T>("FontSize", MySpaceTexts.BlockPropertyTitle_LCDScreenTextSize, MySpaceTexts.Blank);
			myTerminalControlSlider.Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
			myTerminalControlSlider.Enabled = ((T x) => x.SurfaceCount > 0);
			myTerminalControlSlider.SetLimits(0.1f, 10f);
			myTerminalControlSlider.DefaultValue = 1f;
			myTerminalControlSlider.Getter = ((T x) => (x.PanelComponent == null) ? 1f : x.PanelComponent.FontSize);
			myTerminalControlSlider.Setter = delegate(T x, float v)
			{
				if (x.PanelComponent != null)
				{
					x.PanelComponent.FontSize = v;
				}
			};
			myTerminalControlSlider.Writer = delegate(T x, StringBuilder result)
			{
				if (x.PanelComponent != null)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.FontSize, 3));
				}
			};
			myTerminalControlSlider.EnableActions(0.05f, (T x) => x.SurfaceCount > 0, (T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("FontColor", MySpaceTexts.BlockPropertyTitle_FontColor)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0),
				Getter = ((T x) => (x.PanelComponent == null) ? Color.White : x.PanelComponent.FontColor),
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FontColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<T>("alignment", MySpaceTexts.BlockPropertyTitle_Alignment, MySpaceTexts.Blank)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0),
				ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> x)
				{
					MyTextPanelComponent.FillAlignmentComboBoxContent(x);
				},
				Getter = ((T x) => (long)((x.PanelComponent == null) ? TextAlignment.LEFT : x.PanelComponent.Alignment)),
				Setter = delegate(T x, long y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.Alignment = (TextAlignment)y;
					}
				}
			});
			MyTerminalControlSlider<T> myTerminalControlSlider2 = new MyTerminalControlSlider<T>("TextPaddingSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenTextPadding, MySpaceTexts.Blank);
			myTerminalControlSlider2.Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
			myTerminalControlSlider2.Enabled = ((T x) => x.SurfaceCount > 0);
			myTerminalControlSlider2.SetLimits(0f, 50f);
			myTerminalControlSlider2.DefaultValue = 0f;
			myTerminalControlSlider2.Getter = ((T x) => (x.PanelComponent == null) ? 0f : x.PanelComponent.TextPadding);
			myTerminalControlSlider2.Setter = delegate(T x, float v)
			{
				if (x.PanelComponent != null)
				{
					x.PanelComponent.TextPadding = v;
				}
			};
			myTerminalControlSlider2.Writer = delegate(T x, StringBuilder result)
			{
				if (x.PanelComponent != null)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.TextPadding, 1)).Append("%");
				}
			};
			myTerminalControlSlider2.EnableActions(0.05f, (T x) => x.SurfaceCount > 0, (T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<T>
			{
				Visible = ((T x) => x.SurfaceCount > 0 && (x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE || x.PanelComponent.ContentType == ContentType.SCRIPT)),
				Enabled = ((T x) => x.SurfaceCount > 0)
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlColor<T>("BackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0),
				Getter = ((T x) => (x.PanelComponent == null) ? Color.Black : x.PanelComponent.BackgroundColor),
				Setter = delegate(T x, Color v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.BackgroundColor = v;
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("ImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenDefinitionsTextures, MySpaceTexts.Blank, multiSelect: true)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0),
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FillListContent(list1, list2);
					}
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					x.PanelComponent.SelectImageToDraw(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("SelectTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectTextures, MySpaceTexts.Blank, delegate(T x)
			{
				x.PanelComponent.AddImagesToSelection();
			})
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0)
			});
			MyTerminalControlSlider<T> myTerminalControlSlider3 = new MyTerminalControlSlider<T>("ChangeIntervalSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenRefreshInterval, MySpaceTexts.Blank);
			myTerminalControlSlider3.Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
			myTerminalControlSlider3.Enabled = ((T x) => x.SurfaceCount > 0);
			myTerminalControlSlider3.SetLimits(0f, 30f);
			myTerminalControlSlider3.DefaultValue = 0f;
			myTerminalControlSlider3.Getter = ((T x) => (x.PanelComponent == null) ? 0f : x.PanelComponent.ChangeInterval);
			myTerminalControlSlider3.Setter = delegate(T x, float v)
			{
				if (x.PanelComponent != null)
				{
					x.PanelComponent.ChangeInterval = v;
				}
			};
			myTerminalControlSlider3.Writer = delegate(T x, StringBuilder result)
			{
				if (x.PanelComponent != null)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.ChangeInterval, 3)).Append(" s");
				}
			};
			myTerminalControlSlider3.EnableActions(0.05f, (T x) => x.SurfaceCount > 0, (T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<T>("SelectedImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectedTextures, MySpaceTexts.Blank, multiSelect: true)
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0),
				ListContent = delegate(T x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.FillSelectedListContent(list1, list2);
					}
				},
				ItemSelected = delegate(T x, List<MyGuiControlListbox.Item> y)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.SelectImage(y);
					}
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<T>("RemoveSelectedTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenRemoveSelectedTextures, MySpaceTexts.Blank, delegate(T x)
			{
				x.PanelComponent.RemoveImagesFromSelection();
			})
			{
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0)
			});
			MyTerminalControlCheckbox<T> obj = new MyTerminalControlCheckbox<T>("PreserveAspectRatio", MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio, MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio)
			{
				Getter = ((T x) => x.PanelComponent != null && x.PanelComponent.PreserveAspectRatio),
				Setter = delegate(T x, bool v)
				{
					if (x.PanelComponent != null)
					{
						x.PanelComponent.PreserveAspectRatio = v;
					}
				},
				Visible = ((T x) => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE),
				Enabled = ((T x) => x.SurfaceCount > 0)
			};
			obj.EnableAction((T x) => x.SurfaceCount > 0);
			MyTerminalControlFactory.AddControl(obj);
		}

		private static void FillPanels(MyMultiTextPanelComponent multiText, ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			listBoxContent.Clear();
			listBoxSelectedItems.Clear();
			if (multiText == null)
			{
				return;
			}
			for (int i = 0; i < multiText.m_panels.Count; i++)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(multiText.m_panels[i].DisplayName), null, null, i);
				listBoxContent.Add(item);
				if (multiText.m_selectedPanel == i)
				{
					listBoxSelectedItems.Add(item);
				}
			}
		}

		public MyMultiTextPanelComponent(MyTerminalBlock block, List<ScreenArea> screens, List<MySerializedTextPanelData> panels)
		{
			m_block = block;
			m_render = (block.Render as MyRenderComponentScreenAreas);
			if (screens.Count <= 0)
			{
				return;
			}
			m_panels = new List<MyTextPanelComponent>();
			for (int i = 0; i < screens.Count; i++)
			{
				ScreenArea screenArea = screens[i];
				string @string = MyTexts.GetString(screenArea.DisplayName);
				MyTextPanelComponent myTextPanelComponent = new MyTextPanelComponent(i, block, screenArea.Name, @string, screenArea.TextureResolution, screenArea.ScreenWidth, screenArea.ScreenHeight, useOnlineTexture: false);
				m_panels.Add(myTextPanelComponent);
				block.SyncType.Append(myTextPanelComponent);
				myTextPanelComponent.Init((panels != null && panels.Count > i) ? panels[i].Sprites : default(MySerializableSpriteCollection), screenArea.Script, AddImagesRequest, RemoveImagesRequest, ChangeTextRequest, SpriteCollectionUpdate);
			}
			if (panels == null)
			{
				return;
			}
			MyDefinitionManager.Static.GetLCDTexturesDefinitions();
			for (int j = 0; j < panels.Count && j < screens.Count; j++)
			{
				MyTextPanelComponent.ContentMetadata content = new MyTextPanelComponent.ContentMetadata
				{
					ContentType = panels[j].ContentType,
					BackgroundColor = panels[j].BackgroundColor,
					ChangeInterval = panels[j].ChangeInterval,
					PreserveAspectRatio = panels[j].PreserveAspectRatio,
					TextPadding = panels[j].TextPadding
				};
				MyTextPanelComponent.FontData font = new MyTextPanelComponent.FontData
				{
					Alignment = (TextAlignment)panels[j].Alignment,
					Size = panels[j].FontSize,
					TextColor = panels[j].FontColor,
					Name = panels[j].Font.SubtypeName
				};
				MyTextPanelComponent.ScriptData script = new MyTextPanelComponent.ScriptData
				{
					Script = (panels[j].SelectedScript ?? string.Empty),
					CustomizeScript = panels[j].CustomizeScripts,
					BackgroundColor = panels[j].ScriptBackgroundColor,
					ForegroundColor = panels[j].ScriptForegroundColor
				};
				m_panels[j].CurrentSelectedTexture = panels[j].CurrentShownTexture;
				if (panels[j].SelectedImages != null)
				{
					foreach (string selectedImage in panels[j].SelectedImages)
					{
						MyLCDTextureDefinition definition = MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(selectedImage);
						if (definition != null)
						{
							m_panels[j].SelectedTexturesToDraw.Add(definition);
						}
					}
					m_panels[j].CurrentSelectedTexture = Math.Min(m_panels[j].CurrentSelectedTexture, m_panels[j].SelectedTexturesToDraw.Count);
				}
				m_panels[j].Text.Clear().Append(panels[j].Text);
				if (panels[j].ContentType == ContentType.IMAGE)
				{
					content.ContentType = ContentType.TEXT_AND_IMAGE;
				}
				else
				{
					content.ContentType = panels[j].ContentType;
				}
				m_panels[j].SetLocalValues(content, font, script);
			}
		}

		public void Init(Action<int, int[]> addImagesRequest, Action<int, int[]> removeImagesRequest, Action<int, string> changeTextRequest, Action<int, MySerializableSpriteCollection> updateSpriteCollection)
		{
			m_addImagesToSelectionRequest = addImagesRequest;
			m_removeImagesFromSelectionRequest = removeImagesRequest;
			m_changeTextRequest = changeTextRequest;
			m_updateSpriteCollection = updateSpriteCollection;
		}

		private void AddImagesRequest(MyTextPanelComponent panel, int[] selection)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1 && m_addImagesToSelectionRequest != null)
				{
					m_addImagesToSelectionRequest(num, selection);
				}
			}
		}

		public void SelectItems(int panelIndex, int[] selection)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].SelectItems(selection);
			}
		}

		private void RemoveImagesRequest(MyTextPanelComponent panel, int[] selection)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1 && m_removeImagesFromSelectionRequest != null)
				{
					m_removeImagesFromSelectionRequest(num, selection);
				}
			}
		}

		public void RemoveItems(int panelIndex, int[] selection)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].RemoveItems(selection);
			}
		}

		private void ChangeTextRequest(MyTextPanelComponent panel, string text)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1)
				{
					m_changeTextRequest?.Invoke(num, text);
				}
			}
		}

		public void ChangeText(int panelIndex, string text)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].Text.Clear().Append(text);
			}
		}

		private void SpriteCollectionUpdate(MyTextPanelComponent panel, MySerializableSpriteCollection sprites)
		{
			if (panel != null)
			{
				int num = m_panels.IndexOf(panel);
				if (num != -1)
				{
					m_updateSpriteCollection?.Invoke(num, sprites);
				}
			}
		}

		public void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (panelIndex >= 0 && panelIndex < m_panels.Count)
			{
				m_panels[panelIndex].UpdateSpriteCollection(sprites);
			}
		}

		public void SetRender(MyRenderComponentScreenAreas render)
		{
			m_render = render;
			if (m_panels != null && m_panels.Count != 0)
			{
				for (int i = 0; i < m_panels.Count; i++)
				{
					m_panels[i].SetRender(m_render);
				}
			}
		}

		public void AddToScene(int? renderObjectIndex = null)
		{
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.SetRender(m_render);
				panel.Reset();
				m_render.AddScreenArea(m_render.RenderObjectIDs, panel.Name);
				if (renderObjectIndex.HasValue)
				{
					panel.SetRenderObjectIndex(renderObjectIndex.Value);
				}
			}
		}

		public void Reset()
		{
			foreach (MyTextPanelComponent panel in m_panels)
			{
				panel.Reset();
			}
		}

		public List<MySerializedTextPanelData> Serialize()
		{
			if (m_panels.Count > 0)
			{
				List<MySerializedTextPanelData> list = new List<MySerializedTextPanelData>();
				for (int i = 0; i < m_panels.Count; i++)
				{
					MySerializedTextPanelData mySerializedTextPanelData = new MySerializedTextPanelData();
					mySerializedTextPanelData.Alignment = (int)m_panels[i].Alignment;
					mySerializedTextPanelData.BackgroundColor = m_panels[i].BackgroundColor;
					mySerializedTextPanelData.ChangeInterval = m_panels[i].ChangeInterval;
					mySerializedTextPanelData.CurrentShownTexture = m_panels[i].CurrentSelectedTexture;
					mySerializedTextPanelData.Font = m_panels[i].Font;
					mySerializedTextPanelData.FontColor = m_panels[i].FontColor;
					mySerializedTextPanelData.FontSize = m_panels[i].FontSize;
					if (m_panels[i].SelectedTexturesToDraw.Count > 0)
					{
						mySerializedTextPanelData.SelectedImages = new List<string>();
						foreach (MyLCDTextureDefinition item in m_panels[i].SelectedTexturesToDraw)
						{
							mySerializedTextPanelData.SelectedImages.Add(item.Id.SubtypeName);
						}
					}
					mySerializedTextPanelData.Text = m_panels[i].Text.ToString();
					mySerializedTextPanelData.TextPadding = m_panels[i].TextPadding;
					mySerializedTextPanelData.PreserveAspectRatio = m_panels[i].PreserveAspectRatio;
					mySerializedTextPanelData.ContentType = ((m_panels[i].ContentType == ContentType.IMAGE) ? ContentType.TEXT_AND_IMAGE : m_panels[i].ContentType);
					mySerializedTextPanelData.SelectedScript = m_panels[i].Script;
					mySerializedTextPanelData.CustomizeScripts = m_panels[i].CustomizeScripts;
					mySerializedTextPanelData.ScriptBackgroundColor = m_panels[i].ScriptBackgroundColor;
					mySerializedTextPanelData.ScriptForegroundColor = m_panels[i].ScriptForegroundColor;
					list.Add(mySerializedTextPanelData);
				}
				return list;
			}
			return null;
		}

		public void SelectPanel(int index)
		{
			m_selectedPanel = index;
		}

		public void UpdateScreen(bool isWorking)
		{
			bool isInRange = IsInRange();
			for (int i = 0; i < m_panels.Count; i++)
			{
				m_panels[i].UpdateAfterSimulation(isWorking, isInRange);
			}
		}

		public void UpdateAfterSimulation(bool isWorking = true)
		{
			if (m_block.IsFunctional)
			{
				UpdateScreen(isWorking);
				return;
			}
			for (int i = 0; i < m_panels.Count; i++)
			{
				m_panels[i].Reset();
			}
		}

		public bool IsInRange()
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null)
			{
				return false;
			}
			return Vector3D.Distance((MatrixD.CreateTranslation(m_block.PositionComp.LocalVolume.Center) * m_block.WorldMatrix).Translation, mainCamera.Position) < (double)GetDrawDistanceForQuality(MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality);
		}

		public static float GetDrawDistanceForQuality(MyTextureQuality quality)
		{
			switch (quality)
			{
			case MyTextureQuality.LOW:
				return 60f;
			case MyTextureQuality.MEDIUM:
				return 120f;
			case MyTextureQuality.HIGH:
				return 180f;
			default:
				return 120f;
			}
		}

		public IMyTextSurface GetSurface(int index)
		{
			if (index >= 0 && m_panels != null && index < m_panels.Count)
			{
				return m_panels[index];
			}
			return null;
		}

		IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index)
		{
			return GetSurface(index);
		}
	}
}
