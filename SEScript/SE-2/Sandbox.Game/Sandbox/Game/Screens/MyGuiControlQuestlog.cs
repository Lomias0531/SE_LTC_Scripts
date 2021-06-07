using Sandbox.Game.Gui;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Audio;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiControlQuestlog : MyGuiControlBase
	{
		private static readonly float ANIMATION_PERIOD = 10f;

		private static readonly int NUMER_OF_PERIODS = 3;

		private static readonly int CHARACTER_TYPING_FREQUENCY = 2;

		private IMySourceVoice m_currentSoundID;

		public MyHudQuestlog QuestInfo;

		private Vector2 m_position;

		private float m_currentFrame = float.MaxValue;

		private int m_timer;

		private bool m_characterWasAdded;

		public MyGuiControlQuestlog(Vector2 position)
		{
			if (MyGuiManager.FullscreenHudEnabled)
			{
				m_position = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(position);
			}
			else
			{
				m_position = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(position);
			}
			base.Size = MyHud.Questlog.QuestlogSize;
			base.Position = m_position + base.Size / 2f;
			BackgroundTexture = new MyGuiCompositeTexture(MyGuiConstants.TEXTURE_QUESTLOG_BACKGROUND_INFO.Texture);
			base.ColorMask = MyGuiConstants.SCREEN_BACKGROUND_COLOR;
			QuestInfo = MyHud.Questlog;
			base.VisibleChanged += VisibilityChanged;
			QuestInfo.ValueChanged += QuestInfo_ValueChanged;
		}

		private void QuestInfo_ValueChanged()
		{
			base.Position = m_position + base.Size / 2f;
			RecreateControls();
			if (QuestInfo.HighlightChanges)
			{
				m_currentFrame = 0f;
			}
			else
			{
				m_currentFrame = float.MaxValue;
			}
		}

		public override void Update()
		{
			base.Update();
			m_timer++;
			if (m_timer % CHARACTER_TYPING_FREQUENCY == 0)
			{
				m_timer = 0;
				if (m_characterWasAdded)
				{
					UpdateCharacterDisplay();
				}
			}
		}

		private void VisibilityChanged(object sender, bool isVisible)
		{
			if (base.Visible)
			{
				base.Position = m_position + base.Size / 2f;
				RecreateControls();
				m_currentFrame = 0f;
				return;
			}
			m_currentFrame = float.MaxValue;
			if (m_currentSoundID != null)
			{
				m_currentSoundID.Stop();
				m_currentSoundID = null;
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (m_currentFrame < (float)NUMER_OF_PERIODS * ANIMATION_PERIOD && QuestInfo.HighlightChanges)
			{
				backgroundTransitionAlpha = MathHelper.Clamp(((float)Math.Cos((float)(Math.PI * 2.0 * (double)(m_currentFrame / ANIMATION_PERIOD))) + 1.5f) * 0.5f, 0f, 1f);
				m_currentFrame += 1f;
			}
			else if (m_currentFrame == (float)NUMER_OF_PERIODS * ANIMATION_PERIOD && m_currentSoundID != null)
			{
				m_currentSoundID.Stop();
				m_currentSoundID = null;
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
		}

		private void UpdateCharacterDisplay()
		{
			int num = 0;
			MultilineData[] questGetails = QuestInfo.GetQuestGetails();
			int num2 = 0;
			MyGuiControlMultilineText myGuiControlMultilineText;
			while (true)
			{
				if (num2 >= Elements.Count)
				{
					return;
				}
				myGuiControlMultilineText = (Elements[num2] as MyGuiControlMultilineText);
				if (myGuiControlMultilineText != null)
				{
					m_characterWasAdded = false;
					if (num < questGetails.Length)
					{
						questGetails[num].CharactersDisplayed = myGuiControlMultilineText.CharactersDisplayed;
						num++;
					}
					if (!m_characterWasAdded && myGuiControlMultilineText.CharactersDisplayed != -1)
					{
						break;
					}
				}
				num2++;
			}
			myGuiControlMultilineText.CharactersDisplayed++;
			m_characterWasAdded = true;
		}

		public void RecreateControls()
		{
			if (QuestInfo == null || Elements == null)
			{
				return;
			}
			Elements.Clear();
			Vector2 value = -base.Size / 2f;
			Vector2 value2 = new Vector2(0.015f, 0.015f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
			myGuiControlLabel.Text = QuestInfo.QuestTitle;
			myGuiControlLabel.Position = value + value2;
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlLabel.Visible = true;
			myGuiControlLabel.Font = "White";
			Elements.Add(myGuiControlLabel);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(value + value2 + new Vector2(0f, 0.03f), base.Size.X - 2f * value2.X, 0.003f);
			myGuiControlSeparatorList.Visible = true;
			Elements.Add(myGuiControlSeparatorList);
			m_characterWasAdded = true;
			Vector2 value3 = new Vector2(0f, 0.025f);
			float textScale = 0.65f;
			float scale = 0.7f;
			MultilineData[] questGetails = QuestInfo.GetQuestGetails();
			int num = 0;
			for (int i = 0; i < questGetails.Length; i++)
			{
				if (questGetails[i] == null || questGetails[i].Data == null)
				{
					continue;
				}
				MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(size: new Vector2(base.Size.X * 0.92f, value3.Y * 5f), position: value + value2 + new Vector2(0f, 0.04f) + value3 * num, backgroundColor: null, font: questGetails[i].Completed ? "Green" : (questGetails[i].IsObjective ? "White" : "Blue"), textScale: 0.8f, textAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, contents: null, drawScrollbarV: false, drawScrollbarH: false, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				myGuiControlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				myGuiControlMultilineText.CharactersDisplayed = questGetails[i].CharactersDisplayed;
				myGuiControlMultilineText.TextScale = textScale;
				if (questGetails.Length == 2 && i == 1)
				{
					myGuiControlMultilineText.AppendLine();
				}
				if (questGetails.Length == 3 && i == 1)
				{
					myGuiControlMultilineText.AppendLine();
				}
				string[] array = string.Format("{0}{1}", questGetails[i].Completed ? "• " : (questGetails[i].IsObjective ? "• " : ""), questGetails[i].Data).Split(new char[1]
				{
					'*'
				});
				string text = "";
				bool flag = false;
				bool flag2 = true;
				for (int j = 0; j < array.Length; j++)
				{
					if (!flag)
					{
						text = array[j];
					}
					else
					{
						if (flag2)
						{
							myGuiControlMultilineText.AppendLine();
							flag2 = false;
						}
						myGuiControlMultilineText.AppendText(array[j], "UrlHighlight", myGuiControlMultilineText.TextScale * 1.2f, Color.White.ToVector4());
					}
					flag = !flag;
				}
				string[] array2 = text.Split('[', ']');
				bool flag3 = false;
				string[] array3 = array2;
				foreach (string text2 in array3)
				{
					if (flag3)
					{
						if (!questGetails[i].Completed)
						{
							myGuiControlMultilineText.AppendText(text2, "UrlHighlight", scale, Color.Yellow.ToVector4());
						}
						else
						{
							myGuiControlMultilineText.AppendText(text2, "UrlHighlight", scale, Color.Green.ToVector4());
						}
					}
					else
					{
						myGuiControlMultilineText.AppendText(text2);
					}
					flag3 = !flag3;
				}
				myGuiControlMultilineText.Visible = true;
				num += myGuiControlMultilineText.NumberOfRows;
				Elements.Add(myGuiControlMultilineText);
			}
		}
	}
}
