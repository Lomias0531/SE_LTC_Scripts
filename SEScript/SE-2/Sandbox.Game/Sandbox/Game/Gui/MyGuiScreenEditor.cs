using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenEditor : MyGuiScreenBase
	{
		private const string CODE_WRAPPER_BEFORE = "using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\nusing VRage.Game.ModAPI.Ingame;\nusing SpaceEngineers.Game.ModAPI.Ingame;\npublic class Program: MyGridProgram\n{\n";

		private const string CODE_WRAPPER_AFTER = "\n}";

		private Action<ResultEnum> m_resultCallback;

		private Action m_saveCodeCallback;

		private string m_description = "";

		private ResultEnum m_screenResult = ResultEnum.CANCEL;

		public const int MAX_NUMBER_CHARACTERS = 100000;

		private List<string> m_compilerErrors = new List<string>();

		private MyGuiControlMultilineText m_descriptionBox;

		private MyGuiControlCompositePanel m_descriptionBackgroundPanel;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_openWorkshopButton;

		private MyGuiControlButton m_checkCodeButton;

		private MyGuiControlButton m_help;

		private MyGuiControlLabel m_lineCounter;

		private MyGuiControlLabel m_TextTooLongMessage;

		private MyGuiControlLabel m_LetterCounter;

		private MyGuiControlMultilineEditableText m_editorWindow;

		public MyGuiControlMultilineText Description => m_descriptionBox;

		public MyGuiScreenEditor(string description, Action<ResultEnum> resultCallback, Action saveCodeCallback)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1f, 0.9f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_description = description;
			m_saveCodeCallback = saveCodeCallback;
			m_resultCallback = resultCallback;
			base.CanBeHidden = true;
			base.CanHideOthers = true;
			m_closeOnEsc = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenEditor";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MySpaceTexts.ProgrammableBlock_CodeEditor_Title, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.905f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.905f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.905f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.905f);
			Controls.Add(myGuiControlSeparatorList);
			m_okButton = new MyGuiControlButton(new Vector2(-0.184f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OkButtonClicked, toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_CodeEditor_SaveExit_Tooltip));
			Controls.Add(m_okButton);
			m_checkCodeButton = new MyGuiControlButton(new Vector2(-0.001f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CheckCode), toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CheckCode_Tooltip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: CheckCodeButtonClicked);
			Controls.Add(m_checkCodeButton);
			m_help = new MyGuiControlButton(new Vector2(0.182f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_Help), toolTip: string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_HelpTooltip), MySession.GameServiceName), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: HelpButtonClicked);
			Controls.Add(m_help);
			m_openWorkshopButton = new MyGuiControlButton(new Vector2(0.365f, 0.378f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.ProgrammableBlock_Editor_BrowseScripts), toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_BrowseWorkshop_Tooltip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OpenWorkshopButtonClicked);
			Controls.Add(m_openWorkshopButton);
			m_descriptionBackgroundPanel = new MyGuiControlCompositePanel();
			m_descriptionBackgroundPanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
			m_descriptionBackgroundPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_descriptionBackgroundPanel.Position = new Vector2(-0.451f, -0.356f);
			m_descriptionBackgroundPanel.Size = new Vector2(0.902f, 0.664f);
			Controls.Add(m_descriptionBackgroundPanel);
			m_descriptionBox = AddMultilineText(offset: new Vector2(-0.446f, -0.356f), size: new Vector2(0.5f, 0.44f));
			m_descriptionBox.TextPadding = new MyGuiBorderThickness(0.012f, 0f, 0f, 0f);
			m_descriptionBox.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_descriptionBox.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_descriptionBox.Text = new StringBuilder(m_description);
			m_descriptionBox.Position = Vector2.Zero;
			m_descriptionBox.Size = m_descriptionBackgroundPanel.Size - new Vector2(0f, 0.03f);
			m_descriptionBox.Position = new Vector2(0f, -0.024f);
			m_lineCounter = new MyGuiControlLabel(new Vector2(-0.45f, 0.357f), null, string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), 1, m_editorWindow.GetTotalNumLines()), null, 0.8f, "White");
			Elements.Add(m_lineCounter);
			m_LetterCounter = new MyGuiControlLabel(new Vector2(-0.45f, -0.397f), null, null, null, 0.8f, "White");
			Elements.Add(m_LetterCounter);
			m_TextTooLongMessage = new MyGuiControlLabel(new Vector2(-0.34f, -0.4f), null, null, null, 0.8f, "Red");
			Elements.Add(m_TextTooLongMessage);
			base.FocusedControl = m_descriptionBox;
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
			}
		}

		protected MyGuiControlMultilineText AddMultilineText(Vector2? size = null, Vector2? offset = null, float textScale = 1f, bool selectable = false, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2 vector = size ?? base.Size ?? new Vector2(1.2f, 0.5f);
			MyGuiControlMultilineEditableText myGuiControlMultilineEditableText = m_editorWindow = new MyGuiControlMultilineEditableText(vector / 2f + (offset ?? Vector2.Zero), vector, Color.White.ToVector4(), "White", 0.8f, textAlign, null, drawScrollbarV: true, drawScrollbarH: true, textBoxAlign);
			Controls.Add(myGuiControlMultilineEditableText);
			return myGuiControlMultilineEditableText;
		}

		public bool TextTooLong()
		{
			return m_editorWindow.Text.Length > 100000;
		}

		public override bool CloseScreen()
		{
			CallResultCallback(m_screenResult);
			return base.CloseScreen();
		}

		public void SetDescription(string desc)
		{
			m_description = desc;
			m_descriptionBox.Clear();
			m_descriptionBox.Text = new StringBuilder(m_description);
		}

		public void AppendTextToDescription(string text, Vector4 color, string font = "White", float scale = 1f)
		{
			m_description += text;
			m_descriptionBox.AppendText(text, font, scale, color);
		}

		public void AppendTextToDescription(string text, string font = "White", float scale = 1f)
		{
			m_description += text;
			m_descriptionBox.AppendText(text, font, scale, Vector4.One);
		}

		private void HelpButtonClicked(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(MySteamConstants.URL_BROWSE_WORKSHOP_INGAMESCRIPTS_HELP, "Steam Workshop");
		}

		private void SaveCodeButtonClicked(MyGuiControlButton button)
		{
			if (m_saveCodeCallback != null)
			{
				m_saveCodeCallback();
			}
		}

		private void OkButtonClicked(MyGuiControlButton button)
		{
			m_screenResult = ResultEnum.OK;
			CloseScreen();
		}

		private void OpenWorkshopButtonClicked(MyGuiControlButton button)
		{
			m_openWorkshopButton.Enabled = false;
			m_checkCodeButton.Enabled = false;
			m_editorWindow.Enabled = false;
			m_okButton.Enabled = false;
			HideScreen();
			if (MyFakes.I_AM_READY_FOR_NEW_SCRIPT_SCREEN)
			{
				MyScreenManager.AddScreen(MyGuiBlueprintScreen_Reworked.CreateScriptScreen(ScriptSelected, GetCode, WorkshopWindowClosed));
			}
			else
			{
				MyScreenManager.AddScreen(new MyGuiIngameScriptsPage(ScriptSelected, GetCode, WorkshopWindowClosed));
			}
		}

		private string GetCode()
		{
			return m_descriptionBox.Text.ToString();
		}

		private void WorkshopWindowClosed()
		{
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
			}
			UnhideScreen();
			base.FocusedControl = m_descriptionBox;
			m_openWorkshopButton.Enabled = true;
			m_checkCodeButton.Enabled = true;
			m_editorWindow.Enabled = true;
			m_okButton.Enabled = true;
		}

		private void ScriptSelected(string scriptPath)
		{
			string text = null;
			string extension = Path.GetExtension(scriptPath);
			if (extension == ".cs" && File.Exists(scriptPath))
			{
				text = File.ReadAllText(scriptPath);
			}
			else if (extension == ".bin")
			{
				foreach (string file in MyFileSystem.GetFiles(scriptPath, ".cs", MySearchOption.AllDirectories))
				{
					if (MyFileSystem.FileExists(file))
					{
						using (Stream stream = MyFileSystem.OpenRead(file))
						{
							using (StreamReader streamReader = new StreamReader(stream))
							{
								text = streamReader.ReadToEnd();
							}
						}
					}
				}
			}
			else if (MyFileSystem.IsDirectory(scriptPath))
			{
				foreach (string file2 in MyFileSystem.GetFiles(scriptPath, "*.cs", MySearchOption.AllDirectories))
				{
					if (MyFileSystem.FileExists(file2))
					{
						text = File.ReadAllText(file2);
						break;
					}
				}
			}
			if (text != null)
			{
				SetDescription(Regex.Replace(text, "\r\n", " \n"));
				m_lineCounter.Text = string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), m_editorWindow.GetCurrentCarriageLine(), m_editorWindow.GetTotalNumLines());
				m_openWorkshopButton.Enabled = true;
				m_checkCodeButton.Enabled = true;
				m_editorWindow.Enabled = true;
				m_okButton.Enabled = true;
			}
		}

		private void CheckCodeButtonClicked(MyGuiControlButton button)
		{
			string program = Description.Text.ToString();
			m_compilerErrors.Clear();
			Assembly assembly = null;
			if (CompileProgram(program, m_compilerErrors, ref assembly))
			{
				if (m_compilerErrors.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string compilerError in m_compilerErrors)
					{
						stringBuilder.Append(compilerError);
						stringBuilder.Append('\n');
					}
					MyScreenManager.AddScreen(new MyGuiScreenEditorError(stringBuilder.ToString()));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeEditor_Title), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CompilationOk)));
				}
			}
			else
			{
				MyScreenManager.AddScreen(new MyGuiScreenEditorError(string.Join("\n", m_compilerErrors)));
			}
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
			}
			base.FocusedControl = m_descriptionBox;
		}

		public static bool CompileProgram(string program, List<string> errors, ref Assembly assembly)
		{
			if (!string.IsNullOrEmpty(program))
			{
				List<MyScriptCompiler.Message> list = new List<MyScriptCompiler.Message>();
				assembly = MyScriptCompiler.Static.Compile(MyApiTarget.Ingame, Path.Combine(MyFileSystem.UserDataPath, "EditorCode.dll"), MyScriptCompiler.Static.GetIngameScript(program, "Program", typeof(MyGridProgram).Name), list, "PB Code editor").Result;
				errors.Clear();
				errors.AddRange(from m in list
					orderby m.Severity descending
					select m.Text);
				return assembly != null;
			}
			return false;
		}

		private string FormatError(string error)
		{
			try
			{
				char[] separator = new char[4]
				{
					':',
					')',
					'(',
					','
				};
				string[] array = error.Split(separator);
				if (array.Length > 2)
				{
					int num = Convert.ToInt32(array[2]) - m_editorWindow.MeasureNumLines("using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\nusing VRage.Game.ModAPI.Ingame;\nusing SpaceEngineers.Game.ModAPI.Ingame;\npublic class Program: MyGridProgram\n{\n");
					string text = array[6];
					for (int i = 7; i < array.Length; i++)
					{
						if (!string.IsNullOrWhiteSpace(array[i]))
						{
							text = text + "," + array[i];
						}
					}
					return string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CompilationFailedErrorFormat), num, text);
				}
				return error;
			}
			catch (Exception)
			{
				return error;
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (hasFocus && m_editorWindow.CarriageMoved())
			{
				m_lineCounter.Text = string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), m_editorWindow.GetCurrentCarriageLine(), m_editorWindow.GetTotalNumLines());
			}
			if (hasFocus)
			{
				m_LetterCounter.Text = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CharacterLimit) + " " + $"{m_editorWindow.Text.Length} / {100000}";
				if (TextTooLong())
				{
					m_LetterCounter.Font = "Red";
				}
				else
				{
					m_LetterCounter.Font = "White";
				}
				m_TextTooLongMessage.Text = (TextTooLong() ? MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong) : "");
			}
			return base.Update(hasFocus);
		}

		protected override void Canceling()
		{
			base.Canceling();
			m_screenResult = ResultEnum.CANCEL;
		}

		protected void CallResultCallback(ResultEnum result)
		{
			if (m_resultCallback != null)
			{
				m_resultCallback(result);
			}
		}
	}
}
