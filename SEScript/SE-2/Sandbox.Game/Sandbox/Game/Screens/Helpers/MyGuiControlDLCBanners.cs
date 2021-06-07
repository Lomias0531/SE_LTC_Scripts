using LitJson;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Http;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlDLCBanners : MyGuiControlParent
	{
		private enum MyBannerStatus
		{
			Offline,
			Installed,
			NotInstalled
		}

		private class MyBanner
		{
			public bool Enabled;

			public MyBannerStatus Status;

			public uint PackageID;

			public string PackageURL;

			public string Image;

			public string HighlightImage;

			public string CaptionLine1;

			public string CaptionLine2;

			public string Tooltip;
		}

		private class MyBannerResponse
		{
			public string Status;

			public string Version;

			public string Language;

			public string Platform;

			public double CycleInterval;

			public double FadeDuration;

			public List<MyBanner> Data = new List<MyBanner>();

			public int NotInstalledCount => Data.Count((MyBanner x) => x.Status != MyBannerStatus.Installed && x.Enabled);

			public MyBanner this[int index]
			{
				get
				{
					if (index < 0 && index >= NotInstalledCount)
					{
						throw new IndexOutOfRangeException();
					}
					for (int i = 0; i < Data.Count; i++)
					{
						if (Data[i].Enabled && Data[i].Status != MyBannerStatus.Installed)
						{
							if (index == 0)
							{
								return Data[i];
							}
							index--;
						}
					}
					return null;
				}
			}

			public int IndexOf(MyBanner data)
			{
				for (int i = 0; i < NotInstalledCount; i++)
				{
					if (this[i] == data)
					{
						return i;
					}
				}
				return -1;
			}
		}

		private class MyImageDownloadTaskData : WorkData
		{
			public Dictionary<string, string> ImagesToTest = new Dictionary<string, string>();
		}

		private const string PROMO_URL = "https://crashlogs.keenswh.com/api/promotions?format_version=1.0&platform={0}&language={1}&game={2}&game_version={3}";

		private MyGuiControlImageButton m_image;

		private MyGuiControlButton m_firstLine;

		private MyGuiControlButton m_secondLine;

		private float m_cycleInterval = 5f;

		private float m_fadeDuration = 0.6f;

		private float m_timeTillNextDLC = 5f;

		private float m_transition;

		private bool m_isTransitioning;

		private MyGuiControlImageButton m_oldImage;

		private MyGuiControlButton m_oldFirstLine;

		private MyGuiControlButton m_oldSecondLine;

		private static MyBannerResponse m_cachedData;

		private MyGuiControlCompositePanel m_backgroundPanel;

		private MyGuiControlCompositePanel m_backgroundPanel_BlueLine;

		private MyGuiControlButton m_buttonNext;

		private MyGuiControlButton m_buttonPrev;

		public MyGuiControlDLCBanners()
		{
			m_backgroundPanel = new MyGuiControlCompositePanel
			{
				ColorMask = new Vector4(1f, 1f, 1f, 0.8f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND
			};
			base.Controls.Add(m_backgroundPanel);
			m_backgroundPanel_BlueLine = new MyGuiControlCompositePanel
			{
				ColorMask = new Vector4(1f, 1f, 1f, 1f),
				BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND_BlueLine
			};
			base.Controls.Add(m_backgroundPanel_BlueLine);
			m_buttonPrev = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ArrowLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnPrevButtonClicked)
			{
				Name = "Previous"
			};
			base.Controls.Add(m_buttonPrev);
			m_buttonNext = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ArrowRight, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnNextButtonClicked)
			{
				Name = "Next"
			};
			base.Controls.Add(m_buttonNext);
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture()
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture()
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture()
				}
			};
			m_image = new MyGuiControlImageButton("Button", null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnImageClicked);
			m_image.BackgroundTexture = null;
			m_image.ApplyStyle(style);
			base.Controls.Add(m_image);
			m_oldImage = new MyGuiControlImageButton("Button", null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnImageClicked);
			m_oldImage.BackgroundTexture = null;
			m_oldImage.Alpha = 0f;
			m_oldImage.ApplyStyle(style);
			base.Controls.Add(m_oldImage);
			m_firstLine = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.StripeLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnLabelClicked);
			m_firstLine.VisualStyle = MyGuiControlButtonStyleEnum.UrlTextNoLine;
			m_firstLine.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			base.Controls.Add(m_firstLine);
			m_oldFirstLine = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.StripeLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnLabelClicked);
			m_oldFirstLine.VisualStyle = MyGuiControlButtonStyleEnum.UrlTextNoLine;
			m_oldFirstLine.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_oldFirstLine.Alpha = 0f;
			base.Controls.Add(m_oldFirstLine);
			m_secondLine = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.StripeLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnLabelClicked);
			m_secondLine.VisualStyle = MyGuiControlButtonStyleEnum.UrlTextNoLine;
			m_secondLine.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			base.Controls.Add(m_secondLine);
			m_oldSecondLine = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.StripeLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnLabelClicked);
			m_oldSecondLine.VisualStyle = MyGuiControlButtonStyleEnum.UrlTextNoLine;
			m_oldSecondLine.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_oldSecondLine.Alpha = 0f;
			base.Controls.Add(m_oldSecondLine);
			if (MyGameService.Service != null)
			{
				RequestData();
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			m_backgroundPanel.Size = base.Size;
			m_backgroundPanel_BlueLine.Size = base.Size;
			m_backgroundPanel_BlueLine.Position = new Vector2(base.Size.X - 0.004f, 0f);
			Vector2 size = base.Size - new Vector2(0.004f, 0.052f);
			m_image.Size = size;
			m_image.Position = new Vector2(-0.5f, -0.5f) * base.Size;
			m_oldImage.Size = size;
			m_oldImage.Position = new Vector2(-0.5f, -0.5f) * base.Size;
			m_secondLine.Size = new Vector2(base.Size.X, 0.026f);
			m_secondLine.Position = new Vector2(0f, 0.5f * base.Size.Y - 0.006f);
			m_oldSecondLine.Size = new Vector2(base.Size.X, 0.026f);
			m_oldSecondLine.Position = new Vector2(0f, 0.5f * base.Size.Y - 0.006f);
			m_firstLine.Size = new Vector2(base.Size.X, 0.026f);
			m_firstLine.Position = new Vector2(0f, m_secondLine.Position.Y - m_secondLine.Size.Y);
			m_oldFirstLine.Size = new Vector2(base.Size.X, 0.026f);
			m_oldFirstLine.Position = new Vector2(0f, m_oldSecondLine.Position.Y - m_oldSecondLine.Size.Y);
			m_buttonPrev.Position = new Vector2(-0.49f * base.Size.X, 0.5f * base.Size.Y - m_secondLine.Size.Y - 0.005f);
			m_buttonNext.Position = new Vector2(0.49f * base.Size.X, 0.5f * base.Size.Y - m_secondLine.Size.Y - 0.005f);
			m_buttonPrev.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_buttonNext.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (m_isTransitioning)
			{
				m_transition += 0.0166666675f;
				if (m_transition >= m_fadeDuration)
				{
					m_transition = m_fadeDuration;
					m_isTransitioning = false;
				}
				float num = m_transition / m_fadeDuration;
				num = 1f - num;
				num *= num;
				num = 1f - num;
				m_oldImage.Alpha = 1f - num;
				m_image.Alpha = num;
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public override void Update()
		{
			base.Update();
			if (m_cachedData != null && m_cachedData.NotInstalledCount > 1)
			{
				m_timeTillNextDLC -= 0.0166666675f;
				if (m_timeTillNextDLC <= 0f)
				{
					m_timeTillNextDLC = m_cycleInterval;
					OnNextButtonClicked(null);
				}
			}
		}

		public void RequestData()
		{
			string url = $"https://crashlogs.keenswh.com/api/promotions?format_version=1.0&platform={MySession.GameServiceName}&language={MySandboxGame.Config.Language.ToString()}&game={MyPerGameSettings.BasicGameInfo.GameAcronym}&game_version={MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString()}";
			MyVRage.Platform.Http.SendRequestAsync(url, null, HttpMethod.GET, OnResponseReceived);
		}

		private void OnResponseReceived(HttpStatusCode statusCode, string content)
		{
			if (statusCode == HttpStatusCode.OK)
			{
				MyBannerResponse data = null;
				try
				{
					data = JsonMapper.ToObject<MyBannerResponse>(content);
				}
				catch (Exception arg)
				{
					MyLog.Default.WriteLine($"MyBannerResponse reponse error: {arg}\n{content}");
				}
				if (data != null)
				{
					MySandboxGame.Static.Invoke(delegate
					{
						UpdateData(data);
					}, "MyGuiControlDLCBanners");
					return;
				}
			}
			MySandboxGame.Static.Invoke(RequestDataFailed, "MyGuiControlDLCBanners");
		}

		private void RequestDataFailed()
		{
		}

		private void DownloadImages(WorkData workData)
		{
			MyImageDownloadTaskData imageData = workData as MyImageDownloadTaskData;
			string text = Path.Combine(MyFileSystem.UserDataPath, "Promo");
			try
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
			}
			catch
			{
				return;
			}
			int pendingImages = 0;
			HashSet<string> hashSet = new HashSet<string>(imageData.ImagesToTest.Keys);
			bool flag = false;
			foreach (string image in hashSet)
			{
				string fileName = Path.GetFileName(image);
				string text2 = text + Path.DirectorySeparatorChar + fileName;
				if (!File.Exists(text2))
				{
					try
					{
						imageData.ImagesToTest[image] = text2;
						flag = true;
						Interlocked.Increment(ref pendingImages);
						MyVRage.Platform.Http.DownloadAsync(image, text2, null, delegate(HttpStatusCode x)
						{
							if (x != HttpStatusCode.OK)
							{
								imageData.ImagesToTest[image] = string.Empty;
							}
							if (Interlocked.Decrement(ref pendingImages) == 0)
							{
								MySandboxGame.Static.Invoke(delegate
								{
									OnImagesDownloaded(workData);
									ShowDLC(m_cachedData[0]);
								}, "MyGuiControlDLCBanners");
							}
						});
					}
					catch
					{
						imageData.ImagesToTest[image] = string.Empty;
					}
				}
				else
				{
					imageData.ImagesToTest[image] = text2;
				}
			}
			if (!flag)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					OnImagesDownloaded(workData);
					ShowDLC(m_cachedData[0]);
				}, "MyGuiControlDLCBanners");
			}
		}

		private void UpdateData(MyBannerResponse data)
		{
			m_cachedData = data;
			m_cycleInterval = ((m_cachedData.CycleInterval == 0.0) ? 5f : ((float)m_cachedData.CycleInterval));
			m_fadeDuration = ((m_cachedData.FadeDuration == 0.0) ? 0.6f : ((float)m_cachedData.FadeDuration));
			foreach (MyBanner datum in m_cachedData.Data)
			{
				if (datum.PackageID != 0)
				{
					datum.Status = ((MyGameService.IsDlcInstalled(datum.PackageID) || MyGameService.HasInventoryItemWithDefinitionId((int)datum.PackageID)) ? MyBannerStatus.Installed : MyBannerStatus.NotInstalled);
				}
				else
				{
					datum.Status = MyBannerStatus.Offline;
				}
			}
			base.Visible = (m_cachedData.NotInstalledCount > 0);
			if (m_cachedData.NotInstalledCount == 0)
			{
				return;
			}
			m_buttonNext.Visible = (m_cachedData.NotInstalledCount > 1);
			m_buttonPrev.Visible = (m_cachedData.NotInstalledCount > 1);
			MyImageDownloadTaskData myImageDownloadTaskData = new MyImageDownloadTaskData();
			for (int i = 0; i < m_cachedData.NotInstalledCount; i++)
			{
				MyBanner myBanner = m_cachedData[i];
				if (myBanner.Image.StartsWith("http"))
				{
					try
					{
						string text = ConvertImageURLToFile(myBanner.Image);
						if (File.Exists(text))
						{
							myBanner.Image = text;
						}
						else
						{
							myImageDownloadTaskData.ImagesToTest.Add(myBanner.Image, "");
						}
					}
					catch
					{
						myImageDownloadTaskData.ImagesToTest.Add(myBanner.Image, "");
					}
				}
				if (myBanner.HighlightImage.StartsWith("http"))
				{
					try
					{
						string text2 = ConvertImageURLToFile(myBanner.HighlightImage);
						if (File.Exists(text2))
						{
							myBanner.HighlightImage = text2;
						}
						else
						{
							myImageDownloadTaskData.ImagesToTest[myBanner.HighlightImage] = "";
						}
					}
					catch
					{
						myImageDownloadTaskData.ImagesToTest[myBanner.HighlightImage] = "";
					}
				}
			}
			Parallel.Start(DownloadImages, null, myImageDownloadTaskData);
		}

		private void OnImagesDownloaded(WorkData workData)
		{
			MyImageDownloadTaskData myImageDownloadTaskData = workData as MyImageDownloadTaskData;
			foreach (MyBanner datum in m_cachedData.Data)
			{
				if (myImageDownloadTaskData.ImagesToTest.TryGetValue(datum.Image, out string value))
				{
					datum.Image = value;
				}
				if (myImageDownloadTaskData.ImagesToTest.TryGetValue(datum.HighlightImage, out string value2))
				{
					datum.HighlightImage = value2;
				}
			}
		}

		private void ShowDLC(MyBanner dlc)
		{
			if (m_image.UserData is MyBanner)
			{
				m_oldFirstLine.Text = m_firstLine.Text;
				m_oldFirstLine.SetToolTip(MyTexts.GetString(dlc.Tooltip));
				m_oldSecondLine.Text = string.Format(MyTexts.GetString(dlc.CaptionLine2), MySession.GameServiceName);
				m_oldSecondLine.SetToolTip(MyTexts.GetString(dlc.Tooltip));
				m_oldImage.ApplyStyle(m_image.CurrentStyle);
				m_oldImage.SetToolTip(MyTexts.GetString(dlc.Tooltip));
				m_oldImage.UserData = dlc;
				m_oldFirstLine.UserData = dlc;
				m_oldSecondLine.UserData = dlc;
				m_transition = 0f;
				m_isTransitioning = true;
			}
			m_firstLine.Text = MyTexts.GetString(dlc.CaptionLine1);
			m_firstLine.SetToolTip(MyTexts.GetString(dlc.Tooltip));
			m_secondLine.Text = string.Format(MyTexts.GetString(dlc.CaptionLine2), MySession.GameServiceName);
			m_secondLine.SetToolTip(MyTexts.GetString(dlc.Tooltip));
			string centerTexture = dlc.Image.StartsWith("http") ? "" : dlc.Image;
			string centerTexture2 = dlc.HighlightImage.StartsWith("http") ? "" : dlc.HighlightImage;
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture2)
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture2)
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(centerTexture)
				}
			};
			m_image.ApplyStyle(style);
			m_image.SetToolTip(MyTexts.GetString(dlc.Tooltip));
			m_image.UserData = dlc;
			m_firstLine.UserData = dlc;
			m_secondLine.UserData = dlc;
			m_timeTillNextDLC = m_cycleInterval;
		}

		private string ConvertImageURLToFile(string imageUrl)
		{
			return string.Concat(Path.Combine(MyFileSystem.UserDataPath, "Promo"), str2: Path.GetFileName(imageUrl), str1: Path.DirectorySeparatorChar.ToString());
		}

		private void OnImageClicked(MyGuiControlImageButton imageButton)
		{
			MyBanner myBanner = imageButton.UserData as MyBanner;
			if (myBanner != null && !string.IsNullOrWhiteSpace(myBanner.PackageURL))
			{
				MyGuiSandbox.OpenUrl(myBanner.PackageURL, UrlOpenMode.SteamOrExternalWithConfirm);
			}
		}

		private void OnLabelClicked(MyGuiControlButton labelButton)
		{
			MyBanner myBanner = labelButton.UserData as MyBanner;
			if (myBanner != null && !string.IsNullOrWhiteSpace(myBanner.PackageURL))
			{
				MyGuiSandbox.OpenUrl(myBanner.PackageURL, UrlOpenMode.SteamOrExternalWithConfirm);
			}
		}

		private void OnNextButtonClicked(MyGuiControlButton button)
		{
			MyBanner data = m_image.UserData as MyBanner;
			int num = m_cachedData.IndexOf(data);
			num++;
			if (num >= m_cachedData.NotInstalledCount)
			{
				num = 0;
			}
			MyBanner dlc = m_cachedData[num];
			ShowDLC(dlc);
		}

		private void OnPrevButtonClicked(MyGuiControlButton button)
		{
			MyBanner data = m_image.UserData as MyBanner;
			int num = m_cachedData.IndexOf(data);
			num--;
			if (num < 0)
			{
				num = m_cachedData.NotInstalledCount - 1;
			}
			MyBanner dlc = m_cachedData[num];
			ShowDLC(dlc);
		}
	}
}
