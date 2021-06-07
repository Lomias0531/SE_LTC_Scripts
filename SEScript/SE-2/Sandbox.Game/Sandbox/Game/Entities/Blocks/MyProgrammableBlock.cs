using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.IntergridCommunication;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Compiler;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.Library;
using VRage.ModAPI;
using VRage.Network;
using VRage.Scripting;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MyProgrammableBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyProgrammableBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyProgrammableBlock)
	})]
	public class MyProgrammableBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyProgrammableBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProgrammableBlock, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner
	{
		public enum ScriptTerminationReason
		{
			None,
			NoScript,
			NoEntryPoint,
			InstructionOverflow,
			OwnershipChange,
			RuntimeException,
			AlreadyRunning
		}

		private class RuntimeInfo : IMyGridProgramRuntimeInfo
		{
			private static readonly double STOPWATCH_MS_FREQUENCY = 1000.0 / (double)Stopwatch.Frequency;

			private const long TIMESPAN_TICKS_PER_FRAME = 166666L;

			private long m_startTicks;

			private int m_lastRunFrame;

			private readonly MyProgrammableBlock m_block;

			public IlInjector.ICounterHandle InjectorHandle
			{
				get;
				set;
			}

			public TimeSpan TimeSinceLastRun => new TimeSpan((long)(MySession.Static.GameplayFrameCounter - m_lastRunFrame) * 166666L);

			public double LastRunTimeMs
			{
				get;
				private set;
			}

			public int MaxInstructionCount => InjectorHandle.MaxInstructionCount;

			public int CurrentInstructionCount => InjectorHandle.InstructionCount;

			public int MaxCallChainDepth => InjectorHandle.MaxMethodCallCount;

			public int CurrentCallChainDepth => InjectorHandle.MethodCallCount;

			public UpdateFrequency UpdateFrequency
			{
				get
				{
					UpdateFrequency updateFrequency = UpdateFrequency.None;
					MyEntityUpdateEnum needsUpdate = m_block.ScriptComponent.NeedsUpdate;
					if (needsUpdate.HasFlag(MyEntityUpdateEnum.EACH_FRAME))
					{
						updateFrequency |= UpdateFrequency.Update1;
					}
					if (needsUpdate.HasFlag(MyEntityUpdateEnum.EACH_10TH_FRAME))
					{
						updateFrequency |= UpdateFrequency.Update10;
					}
					if (needsUpdate.HasFlag(MyEntityUpdateEnum.EACH_100TH_FRAME))
					{
						updateFrequency |= UpdateFrequency.Update100;
					}
					if (m_block.ScriptComponent.NextUpdate.HasFlag(UpdateType.Once))
					{
						updateFrequency |= UpdateFrequency.Once;
					}
					return updateFrequency;
				}
				set
				{
					if ((value & ~(UpdateFrequency.Update1 | UpdateFrequency.Update10 | UpdateFrequency.Update100 | UpdateFrequency.Once)) != 0)
					{
						throw new ArgumentException("Unsupported flags in UpdateFrequency");
					}
					if (value == UpdateFrequency.None)
					{
						m_block.ScriptComponent.NextUpdate = UpdateType.None;
						return;
					}
					MyEntityUpdateEnum needsUpdate = m_block.ScriptComponent.NeedsUpdate;
					needsUpdate = ((!value.HasFlag(UpdateFrequency.Update1)) ? (needsUpdate & ~MyEntityUpdateEnum.EACH_FRAME) : (needsUpdate | MyEntityUpdateEnum.EACH_FRAME));
					needsUpdate = ((!value.HasFlag(UpdateFrequency.Update10)) ? (needsUpdate & ~MyEntityUpdateEnum.EACH_10TH_FRAME) : (needsUpdate | MyEntityUpdateEnum.EACH_10TH_FRAME));
					needsUpdate = ((!value.HasFlag(UpdateFrequency.Update100)) ? (needsUpdate & ~MyEntityUpdateEnum.EACH_100TH_FRAME) : (needsUpdate | MyEntityUpdateEnum.EACH_100TH_FRAME));
					if (value.HasFlag(UpdateFrequency.Once))
					{
						m_block.ScriptComponent.NextUpdate |= UpdateType.Once;
						needsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					}
					else
					{
						m_block.ScriptComponent.NextUpdate &= ~UpdateType.Once;
					}
					m_block.ScriptComponent.NeedsUpdate = needsUpdate;
				}
			}

			public RuntimeInfo(MyProgrammableBlock block)
			{
				m_block = block;
			}

			public void Reset()
			{
				m_startTicks = 0L;
				LastRunTimeMs = 0.0;
				m_lastRunFrame = MySession.Static.GameplayFrameCounter;
			}

			public void BeginMainOperation()
			{
				m_startTicks = Stopwatch.GetTimestamp();
			}

			public void EndMainOperation()
			{
				long timestamp = Stopwatch.GetTimestamp();
				m_lastRunFrame = MySession.Static.GameplayFrameCounter;
				LastRunTimeMs = (double)(timestamp - m_startTicks) * STOPWATCH_MS_FREQUENCY;
			}

			public void BeginSaveOperation()
			{
				LastRunTimeMs = 0.0;
			}
		}

		public class MyGridTerminalWrapper : Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
		{
			private Sandbox.ModAPI.Ingame.IMyGridTerminalSystem m_terminalInstance;

			internal void SetInstance(MyGridTerminalSystem terminalSystem)
			{
				m_terminalInstance = terminalSystem;
			}

			void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocks(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks)
			{
				m_terminalInstance.GetBlocks(blocks);
			}

			void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroups(List<Sandbox.ModAPI.Ingame.IMyBlockGroup> blockGroups, Func<Sandbox.ModAPI.Ingame.IMyBlockGroup, bool> collect)
			{
				m_terminalInstance.GetBlockGroups(blockGroups, collect);
			}

			void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
			{
				m_terminalInstance.GetBlocksOfType<T>(blocks, collect);
			}

			void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect)
			{
				m_terminalInstance.GetBlocksOfType(blocks, collect);
			}

			void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.SearchBlocksOfName(string name, List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
			{
				m_terminalInstance.SearchBlocksOfName(name, blocks, collect);
			}

			Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithName(string name)
			{
				return m_terminalInstance.GetBlockWithName(name);
			}

			Sandbox.ModAPI.Ingame.IMyBlockGroup Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroupWithName(string name)
			{
				return m_terminalInstance.GetBlockGroupWithName(name);
			}

			Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithId(long id)
			{
				return m_terminalInstance.GetBlockWithId(id);
			}
		}

		protected sealed class Recompile_003C_003ESystem_Boolean : ICallSite<MyProgrammableBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in bool instantiate, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.Recompile(instantiate);
			}
		}

		protected sealed class WriteProgramResponse_003C_003ESystem_String : ICallSite<MyProgrammableBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in string response, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.WriteProgramResponse(response);
			}
		}

		protected sealed class OpenEditorRequest_003C_003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OpenEditorRequest();
			}
		}

		protected sealed class OpenEditorSucess_003C_003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OpenEditorSucess();
			}
		}

		protected sealed class OpenEditorFailure_003C_003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OpenEditorFailure();
			}
		}

		protected sealed class CloseEditor_003C_003E : ICallSite<MyProgrammableBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CloseEditor();
			}
		}

		protected sealed class UpdateProgram_003C_003ESystem_Byte_003C_0023_003E : ICallSite<MyProgrammableBlock, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in byte[] program, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UpdateProgram(program);
			}
		}

		protected sealed class RunProgramRequest_003C_003ESystem_Byte_003C_0023_003E_0023Sandbox_ModAPI_Ingame_UpdateType : ICallSite<MyProgrammableBlock, byte[], UpdateType, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in byte[] argument, in UpdateType updateType, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RunProgramRequest(argument, updateType);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyProgrammableBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyProgrammableBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyProgrammableBlock, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyProgrammableBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyProgrammableBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyProgrammableBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyProgrammableBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProgrammableBlock @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyProgrammableBlock_003C_003EActor : IActivator, IActivator<MyProgrammableBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyProgrammableBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProgrammableBlock CreateInstance()
			{
				return new MyProgrammableBlock();
			}

			MyProgrammableBlock IActivator<MyProgrammableBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly string[] NEW_LINES = new string[2]
		{
			"\r\n",
			"\n"
		};

		private const string DEFAULT_SCRIPT_TEMPLATE = "public Program()\r\n{{\r\n{0}\r\n}}\r\n\r\npublic void Save()\r\n{{\r\n{1}\r\n}}\r\n\r\npublic void Main(string argument, UpdateType updateSource)\r\n{{\r\n{2}\r\n}}\r\n";

		private const int MAX_NUM_EXECUTED_INSTRUCTIONS = 50000;

		private const int MAX_NUM_METHOD_CALLS = 10000;

		private const int MAX_ECHO_LENGTH = 8000;

		private IMyGridProgram m_instance;

		private RuntimeInfo m_runtime;

		private string m_programData;

		private string m_storageData;

		private string m_editorData;

		private string m_terminalRunArgument = string.Empty;

		private readonly StringBuilder m_echoOutput = new StringBuilder();

		private bool m_consoleOpen;

		private MyGuiScreenEditor m_editorScreen;

		private Assembly m_assembly;

		private readonly List<string> m_compilerErrors = new List<string>();

		private readonly List<MyScriptCompiler.Message> m_compilerMessages = new List<MyScriptCompiler.Message>();

		private ScriptTerminationReason m_terminationReason;

		private bool m_isRunning;

		private bool m_mainMethodSupportsArgument;

		private ulong m_userId;

		private readonly List<MyCubeGrid> m_groupCache = new List<MyCubeGrid>();

		private bool m_needsInstantiation;

		private MyGridTerminalWrapper m_terminalWrapper = new MyGridTerminalWrapper();

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBoxMultiPanel;

		internal MyIngameScriptComponent ScriptComponent;

		private bool m_isTextPanelOpen;

		public string TerminalRunArgument
		{
			get
			{
				return m_terminalRunArgument;
			}
			set
			{
				m_terminalRunArgument = (value ?? string.Empty);
			}
		}

		public ulong UserId
		{
			get
			{
				return m_userId;
			}
			set
			{
				m_userId = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyProgrammableBlock.IsRunning => m_isRunning;

		string Sandbox.ModAPI.IMyProgrammableBlock.ProgramData
		{
			get
			{
				return m_programData;
			}
			set
			{
				m_editorData = (m_programData = value);
				if (Sync.IsServer)
				{
					Recompile();
				}
				else
				{
					SendUpdateProgramRequest(m_programData);
				}
			}
		}

		string Sandbox.ModAPI.IMyProgrammableBlock.StorageData
		{
			get
			{
				if (m_instance == null)
				{
					return null;
				}
				return m_instance.Storage;
			}
			set
			{
				if (m_instance != null)
				{
					m_instance.Storage = value;
				}
			}
		}

		bool Sandbox.ModAPI.IMyProgrammableBlock.HasCompileErrors => m_compilerErrors.Count > 0;

		int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount
		{
			get
			{
				if (m_multiPanel == null)
				{
					return 0;
				}
				return m_multiPanel.SurfaceCount;
			}
		}

		MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => m_multiPanel;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_multiPanel == null)
				{
					return null;
				}
				return m_multiPanel.PanelComponent;
			}
		}

		public bool IsTextPanelOpen
		{
			get
			{
				return m_isTextPanelOpen;
			}
			set
			{
				if (m_isTextPanelOpen != value)
				{
					m_isTextPanelOpen = value;
					RaisePropertiesChanged();
				}
			}
		}

		public MyProgrammableBlock()
		{
			CreateTerminalControls();
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyProgrammableBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyProgrammableBlock>(tooltip: (!MySession.GameServiceName.Equals("Steam")) ? MySpaceTexts.TerminalControlPanel_EditCode_Tooltip : MySpaceTexts.TerminalControlPanel_EditCode_Tooltip_Steam, id: "Edit", title: MySpaceTexts.TerminalControlPanel_EditCode, action: delegate(MyProgrammableBlock b)
				{
					b.SendOpenEditorRequest();
				})
				{
					Visible = ((MyProgrammableBlock b) => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts),
					Enabled = ((MyProgrammableBlock b) => MySession.Static.IsUserScripter(Sync.MyId))
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyProgrammableBlock>("ConsoleCommand", MySpaceTexts.TerminalControlPanel_RunArgument, MySpaceTexts.TerminalControlPanel_RunArgument_ToolTip)
				{
					Visible = ((MyProgrammableBlock e) => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts),
					Getter = ((MyProgrammableBlock e) => new StringBuilder(e.TerminalRunArgument)),
					Setter = delegate(MyProgrammableBlock e, StringBuilder v)
					{
						e.TerminalRunArgument = v.ToString();
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyProgrammableBlock>("TerminalRun", MySpaceTexts.TerminalControlPanel_RunCode, MySpaceTexts.TerminalControlPanel_RunCode_Tooltip, delegate(MyProgrammableBlock b)
				{
					b.Run(b.TerminalRunArgument, UpdateType.Terminal);
				})
				{
					Visible = ((MyProgrammableBlock b) => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts),
					Enabled = ((MyProgrammableBlock b) => b.IsWorking && b.IsFunctional)
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyProgrammableBlock>("Recompile", MySpaceTexts.TerminalControlPanel_Recompile, MySpaceTexts.TerminalControlPanel_Recompile_Tooltip, delegate(MyProgrammableBlock b)
				{
					b.SendRecompile();
				})
				{
					Visible = ((MyProgrammableBlock b) => MyFakes.ENABLE_PROGRAMMABLE_BLOCK && MySession.Static.EnableIngameScripts),
					Enabled = ((MyProgrammableBlock b) => b.IsWorking && b.IsFunctional)
				});
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyProgrammableBlock>("Run", MyTexts.Get(MySpaceTexts.TerminalControlPanel_RunCode), OnRunApplied, null, MyTerminalActionIcons.START)
				{
					Enabled = ((MyProgrammableBlock b) => b.IsFunctional),
					DoUserParameterRequest = RequestRunArgument,
					ParameterDefinitions = 
					{
						TerminalActionParameter.Get(string.Empty)
					}
				});
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyProgrammableBlock>("RunWithDefaultArgument", MyTexts.Get(MySpaceTexts.TerminalControlPanel_RunCodeDefault), OnRunDefaultApplied, MyTerminalActionIcons.START)
				{
					Enabled = ((MyProgrammableBlock b) => b.IsFunctional)
				});
				MyMultiTextPanelComponent.CreateTerminalControls<MyProgrammableBlock>();
			}
		}

		private static void OnRunApplied(MyProgrammableBlock programmableBlock, ListReader<TerminalActionParameter> parameters)
		{
			string argument = null;
			TerminalActionParameter terminalActionParameter = parameters.FirstOrDefault();
			if (!terminalActionParameter.IsEmpty && terminalActionParameter.TypeCode == TypeCode.String)
			{
				argument = (terminalActionParameter.Value as string);
			}
			programmableBlock.Run(argument, UpdateType.Trigger);
		}

		private static void OnRunDefaultApplied(MyProgrammableBlock programmableBlock)
		{
			programmableBlock.Run(programmableBlock.TerminalRunArgument, UpdateType.Trigger);
		}

		private static void RequestRunArgument(IList<TerminalActionParameter> list, Action<bool> callback)
		{
			MyGuiScreenDialogText myGuiScreenDialogText = new MyGuiScreenDialogText(string.Empty, MySpaceTexts.DialogText_RunArgument);
			myGuiScreenDialogText.OnConfirmed += delegate(string argument)
			{
				list[0] = TerminalActionParameter.Get(argument);
				callback(obj: true);
			};
			MyGuiSandbox.AddScreen(myGuiScreenDialogText);
		}

		private static string ToIndentedComment(string input)
		{
			string[] value = input.Split(NEW_LINES, StringSplitOptions.None);
			return "    // " + string.Join("\n    // ", value);
		}

		private void OpenEditor()
		{
			if (m_editorData == null)
			{
				string arg = ToIndentedComment(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_DefaultScript_Constructor).Trim());
				string arg2 = ToIndentedComment(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_DefaultScript_Save).Trim());
				string arg3 = ToIndentedComment(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_DefaultScript_Main).Trim());
				m_editorData = $"public Program()\r\n{{\r\n{arg}\r\n}}\r\n\r\npublic void Save()\r\n{{\r\n{arg2}\r\n}}\r\n\r\npublic void Main(string argument, UpdateType updateSource)\r\n{{\r\n{arg3}\r\n}}\r\n";
			}
			m_editorScreen = new MyGuiScreenEditor(m_editorData, SaveCode, SaveCode);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_editorScreen);
		}

		private void SaveCode()
		{
			if (m_editorScreen.TextTooLong())
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeChanged), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
				return;
			}
			m_editorData = (m_programData = m_editorScreen.Description.Text.ToString());
			if (Sync.IsServer)
			{
				Recompile();
			}
			else
			{
				SendUpdateProgramRequest(m_programData);
			}
		}

		public void SendRecompile()
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.Recompile, arg2: true);
		}

		[Event(null, 339)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void Recompile(bool instantiate = true)
		{
			m_compilerErrors.Clear();
			m_compilerMessages.Clear();
			m_echoOutput.Clear();
			UpdateStorage();
			Compile(m_programData, m_storageData, instantiate);
		}

		private void UpdateStorage()
		{
			if (m_instance == null)
			{
				return;
			}
			m_storageData = m_instance.Storage;
			if (m_instance.HasSaveMethod)
			{
				RunSandboxedProgramAction(delegate(IMyGridProgram program)
				{
					m_runtime.BeginSaveOperation();
					m_instance.ElapsedTime = TimeSpan.Zero;
					program.Save();
				}, out string response);
				SetDetailedInfo(response);
				if (m_instance != null)
				{
					m_storageData = m_instance.Storage;
				}
			}
		}

		private void SaveCode(ResultEnum result)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			SendCloseEditor();
			if (m_editorScreen.TextTooLong())
			{
				MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeChanged), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
				return;
			}
			base.DetailedInfo.Clear();
			RaisePropertiesChanged();
			if (result == ResultEnum.OK)
			{
				SaveCode();
			}
			else if (m_editorScreen.Description.Text.ToString() != m_programData)
			{
				MyGuiScreenMessageBox obj = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeChanged), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_SaveChanges), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false);
				obj.ResultCallback = delegate(MyGuiScreenMessageBox.ResultEnum result2)
				{
					if (result2 == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						SaveCode(ResultEnum.OK);
					}
					else
					{
						m_editorData = m_programData;
					}
				};
				MyScreenManager.AddScreen(obj);
			}
		}

		public ScriptTerminationReason ExecuteCode(string argument, UpdateType updateSource, out string response)
		{
			if (MySession.Static != null && MySession.Static.Settings != null && !MySession.Static.EnableIngameScripts)
			{
				response = MyTexts.GetString("ProgrammableBlock_Error_ScriptsDisabled");
				return ScriptTerminationReason.None;
			}
			return RunSandboxedProgramAction(delegate(IMyGridProgram program)
			{
				m_runtime.BeginMainOperation();
				m_instance.ElapsedTime = m_runtime.TimeSinceLastRun;
				program.Main(argument, updateSource);
				m_runtime.EndMainOperation();
			}, out response);
		}

		public ScriptTerminationReason RunSandboxedProgramAction(Action<IMyGridProgram> action, out string response)
		{
			if (MySandboxGame.Static.UpdateThread != Thread.CurrentThread && MyModWatchdog.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_PBParallelInvocation))
			{
				MyLog.Default.Log(MyLogSeverity.Error, "PB invoked from parallel thread (logged only once)!" + Environment.NewLine + Environment.StackTrace);
			}
			if (m_isRunning)
			{
				response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_AllreadyRunning);
				return ScriptTerminationReason.AlreadyRunning;
			}
			if (m_terminationReason != 0)
			{
				response = base.DetailedInfo.ToString();
				return m_terminationReason;
			}
			base.DetailedInfo.Clear();
			m_echoOutput.Clear();
			if (m_assembly == null)
			{
				response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoAssembly);
				return ScriptTerminationReason.NoScript;
			}
			if (m_instance == null)
			{
				if (!m_needsInstantiation || !CheckIsWorking() || !base.Enabled)
				{
					response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoAssembly);
					return ScriptTerminationReason.NoScript;
				}
				m_needsInstantiation = false;
				CreateInstance(m_assembly, m_compilerErrors, m_storageData);
				if (m_instance == null)
				{
					response = base.DetailedInfo.ToString();
					return m_terminationReason;
				}
			}
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
			MyGridTerminalSystem terminalSystem = group.GroupData.TerminalSystem;
			m_terminalWrapper.SetInstance(terminalSystem);
			MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Logical).GetGroupNodes(base.CubeGrid, m_groupCache);
			group.GroupData.UpdateGridOwnership(m_groupCache, base.OwnerId);
			m_groupCache.Clear();
			terminalSystem.UpdateGridBlocksOwnership(base.OwnerId);
			m_instance.GridTerminalSystem = m_terminalWrapper;
			m_isRunning = true;
			response = "";
			try
			{
				IlInjector.ICounterHandle counterHandle = IlInjector.BeginRunBlock(50000, 10000);
				int num = counterHandle.Depth - 1;
				try
				{
					m_runtime.InjectorHandle = counterHandle;
					action(m_instance);
				}
				finally
				{
					counterHandle.Dispose();
					if (counterHandle.Depth != num)
					{
						MyLog.Default.Log(MyLogSeverity.Error, "PB {0} invoke depth leak: {1} -> {2}", base.EntityId, num, counterHandle.Depth);
					}
				}
				if (m_echoOutput.Length > 0)
				{
					response = m_echoOutput.ToString();
				}
				return m_terminationReason;
			}
			catch (Exception innerException)
			{
				if (innerException is TargetInvocationException)
				{
					innerException = innerException.InnerException;
				}
				if (m_echoOutput.Length > 0)
				{
					response = m_echoOutput.ToString();
				}
				if (innerException is ScriptOutOfRangeException)
				{
					if (IlInjector.IsWithinRunBlock())
					{
						response += MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NestedTooComplex);
						return ScriptTerminationReason.InstructionOverflow;
					}
					response += MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_TooComplex);
					OnProgramTermination(ScriptTerminationReason.InstructionOverflow);
				}
				else
				{
					string fullName = typeof(MyGridProgram).FullName;
					string text = innerException.StackTrace;
					int num2 = text.IndexOf(fullName);
					if (num2 > 0)
					{
						int length = text.LastIndexOf(MyEnvironment.NewLine, num2, num2, StringComparison.InvariantCulture);
						text = text.Substring(0, length);
					}
					response = response + MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_ExceptionCaught) + innerException.Message + "\n" + text;
					OnProgramTermination(ScriptTerminationReason.RuntimeException);
				}
				return m_terminationReason;
			}
			finally
			{
				m_runtime.InjectorHandle = null;
				m_isRunning = false;
			}
		}

		private void OnProgramTermination(ScriptTerminationReason reason)
		{
			m_terminationReason = reason;
			m_instance = null;
			m_assembly = null;
			m_echoOutput.Clear();
			m_runtime.Reset();
		}

		public void Run(string argument, UpdateType updateSource)
		{
			if (base.IsWorking && base.IsFunctional && base.CubeGrid.Physics != null)
			{
				MySimpleProfiler.Begin("Scripts", MySimpleProfiler.ProfilingBlockType.BLOCK, "Run");
				if (Sync.IsServer)
				{
					ExecuteCode(argument, updateSource, out string response);
					SetDetailedInfo(response);
				}
				else
				{
					SendRunProgramRequest(argument, updateSource);
				}
				MySimpleProfiler.End("Run");
			}
		}

		private void SetDetailedInfo(string detailedInfo)
		{
			if (base.DetailedInfo.ToString() != detailedInfo)
			{
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.WriteProgramResponse, detailedInfo);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyProgrammableBlockDefinition myProgrammableBlockDefinition = base.BlockDefinition as MyProgrammableBlockDefinition;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(myProgrammableBlockDefinition.ResourceSinkGroup, 0.0005f, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			myResourceSinkComponent.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_MyProgrammableBlock myObjectBuilder_MyProgrammableBlock = (MyObjectBuilder_MyProgrammableBlock)objectBuilder;
			m_editorData = (m_programData = myObjectBuilder_MyProgrammableBlock.Program);
			m_storageData = myObjectBuilder_MyProgrammableBlock.Storage;
			m_terminalRunArgument = myObjectBuilder_MyProgrammableBlock.DefaultRunArgument;
			if (Sync.IsServer && !string.IsNullOrEmpty(m_programData))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				m_needsInstantiation = true;
			}
			m_runtime = new RuntimeInfo(this);
			ScriptComponent = base.GameLogic.GetAs<MyIngameScriptComponent>();
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if (Sync.IsServer && Sync.Clients != null)
			{
				MyClientCollection clients = Sync.Clients;
				clients.ClientRemoved = (Action<ulong>)Delegate.Combine(clients.ClientRemoved, new Action<ulong>(ProgrammableBlock_ClientRemoved));
			}
			if (myProgrammableBlockDefinition.ScreenAreas != null && myProgrammableBlockDefinition.ScreenAreas.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_multiPanel = new MyMultiTextPanelComponent(this, myProgrammableBlockDefinition.ScreenAreas, myObjectBuilder_MyProgrammableBlock.TextPanels);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
			if (Sync.IsServer && MySession.Static.EnableIngameScripts && m_programData != null && m_assembly == null)
			{
				Recompile(instantiate: false);
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
			if (Sync.Clients != null)
			{
				MyClientCollection clients = Sync.Clients;
				clients.ClientRemoved = (Action<ulong>)Delegate.Remove(clients.ClientRemoved, new Action<ulong>(ProgrammableBlock_ClientRemoved));
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (Sync.IsServer && m_needsInstantiation)
			{
				m_needsInstantiation = false;
				if (MySession.Static.EnableIngameScripts)
				{
					if (m_programData != null && m_assembly == null)
					{
						Recompile(instantiate: false);
					}
					if (m_assembly != null && m_instance == null)
					{
						CreateInstance(m_assembly, m_compilerErrors, m_storageData);
					}
				}
				else
				{
					string @string = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NotAllowed);
					MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.WriteProgramResponse, @string);
				}
			}
			if (!base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_multiPanel != null)
			{
				m_multiPanel.Reset();
			}
			if (base.ResourceSink != null)
			{
				UpdateScreen();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_MyProgrammableBlock myObjectBuilder_MyProgrammableBlock = (MyObjectBuilder_MyProgrammableBlock)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_MyProgrammableBlock.Program = m_programData;
			myObjectBuilder_MyProgrammableBlock.DefaultRunArgument = m_terminalRunArgument;
			if (Sync.IsServer)
			{
				UpdateStorage();
				if (m_instance != null)
				{
					myObjectBuilder_MyProgrammableBlock.Storage = m_instance.Storage;
				}
				else
				{
					myObjectBuilder_MyProgrammableBlock.Storage = m_storageData;
				}
			}
			if (m_multiPanel != null)
			{
				myObjectBuilder_MyProgrammableBlock.TextPanels = m_multiPanel.Serialize();
			}
			return myObjectBuilder_MyProgrammableBlock;
		}

		private void Compile(string program, string storage, bool instantiate = false)
		{
			if (Sync.IsServer)
			{
				ScriptComponent.NeedsUpdate = MyEntityUpdateEnum.NONE;
				ScriptComponent.NextUpdate = UpdateType.None;
				if (MySession.Static.EnableIngameScripts && !base.CubeGrid.IsPreview && base.CubeGrid.CreatePhysics)
				{
					m_terminationReason = ScriptTerminationReason.None;
					try
					{
						m_assembly = MyScriptCompiler.Static.Compile(MyApiTarget.Ingame, Path.Combine(MyFileSystem.UserDataPath, GetAssemblyName()), MyScriptCompiler.Static.GetIngameScript(program, "Program", typeof(MyGridProgram).Name), m_compilerMessages, "PB: " + base.DisplayName + " (" + base.EntityId + ")").Result;
						m_compilerErrors.Clear();
						m_compilerErrors.AddRange(m_compilerMessages.Select((MyScriptCompiler.Message m) => m.Text));
						if (instantiate)
						{
							CreateInstance(m_assembly, m_compilerErrors, storage);
						}
					}
					catch (Exception ex)
					{
						string detailedInfo = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_ExceptionCaught) + ex.Message;
						SetDetailedInfo(detailedInfo);
					}
				}
			}
		}

		private string GetAssemblyName()
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.EntityId);
			stringBuilder.Append("-");
			for (int i = 0; i < base.CustomName.Length; i++)
			{
				char c = base.CustomName[i];
				if (invalidFileNameChars.Contains(c))
				{
					stringBuilder.Append("_");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			stringBuilder.Append(".dll");
			return stringBuilder.ToString();
		}

		private bool CreateInstance(Assembly assembly, IEnumerable<string> messages, string storage)
		{
			m_needsInstantiation = false;
			string response = string.Join("\n", messages);
			if (assembly == null)
			{
				return false;
			}
			Type type = assembly.GetType("Program");
			if (type != null)
			{
				m_instance = (FormatterServices.GetUninitializedObject(type) as IMyGridProgram);
				ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				if (m_instance == null || constructor == null)
				{
					response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoValidConstructor) + "\n\n" + response;
					SetDetailedInfo(response);
					return false;
				}
				m_runtime.Reset();
				m_instance.Runtime = m_runtime;
				m_instance.Storage = storage;
				m_instance.Me = this;
				m_instance.Echo = EchoTextToDetailInfo;
				MyIGCSystemSessionComponent.Static.EvictContextFor(this);
				MyIntergridCommunicationContext m_IGCContextCache = null;
				m_instance.IGC_ContextGetter = delegate
				{
					if (m_IGCContextCache == null)
					{
						m_IGCContextCache = MyIGCSystemSessionComponent.Static.GetOrMakeContextFor(this);
					}
					return m_IGCContextCache;
				};
				RunSandboxedProgramAction(delegate(IMyGridProgram p)
				{
					constructor.Invoke(p, null);
					if (!m_instance.HasMainMethod)
					{
						if (m_echoOutput.Length > 0)
						{
							response = response + "\n\n" + m_echoOutput.ToString();
						}
						response = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_NoMain) + "\n\n" + response;
						OnProgramTermination(ScriptTerminationReason.NoEntryPoint);
					}
				}, out response);
				SetDetailedInfo(response);
			}
			return true;
		}

		private void EchoTextToDetailInfo(string line)
		{
			line = (line ?? string.Empty);
			int num = line.Length + 1;
			if (num > 8000)
			{
				m_echoOutput.Clear();
				line = line.Substring(0, 8000);
				num = 8000;
			}
			int num2 = m_echoOutput.Length + num;
			if (num2 > 8000)
			{
				m_echoOutput.Remove(0, num2 - 8000);
			}
			m_echoOutput.Append(line);
			m_echoOutput.Append('\n');
		}

		private void ShowEditorAllReadyOpen()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Editor is opened by another player.")));
		}

		public void UpdateProgram(string program)
		{
			m_editorData = (m_programData = program);
			if (Sync.IsServer)
			{
				Recompile();
			}
		}

		[Event(null, 932)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void WriteProgramResponse(string response)
		{
			base.DetailedInfo.Clear();
			base.DetailedInfo.Append(response);
			RaisePropertiesChanged();
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			if (MySession.Static.SurvivalMode)
			{
				OnProgramTermination(ScriptTerminationReason.OwnershipChange);
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.WriteProgramResponse, MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Exception_Ownershipchanged));
				}
			}
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override bool CheckIsWorking()
		{
			bool num = base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();
			if (num && m_needsInstantiation)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			return num;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			base.ResourceSink.Update();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(CheckIsWorking());
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene();
			}
		}

		private void UpdateScreen()
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateScreen(CheckIsWorking());
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public void ProgrammableBlock_ClientRemoved(ulong playerId)
		{
			if (playerId == m_userId)
			{
				SendCloseEditor();
			}
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void SendOpenEditorRequest()
		{
			if (Sync.IsServer)
			{
				if (!m_consoleOpen)
				{
					m_consoleOpen = true;
					OpenEditor();
				}
				else
				{
					ShowEditorAllReadyOpen();
				}
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OpenEditorRequest);
			}
		}

		[Event(null, 1045)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OpenEditorRequest()
		{
			if (!m_consoleOpen)
			{
				UserId = MyEventContext.Current.Sender.Value;
				m_consoleOpen = true;
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OpenEditorSucess, new EndpointId(UserId));
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OpenEditorFailure, new EndpointId(UserId));
			}
		}

		[Event(null, 1060)]
		[Reliable]
		[Client]
		private void OpenEditorSucess()
		{
			OpenEditor();
		}

		[Event(null, 1066)]
		[Reliable]
		[Client]
		private void OpenEditorFailure()
		{
			ShowEditorAllReadyOpen();
		}

		private void SendCloseEditor()
		{
			if (Sync.IsServer)
			{
				m_consoleOpen = false;
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.CloseEditor);
			}
		}

		[Event(null, 1084)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void CloseEditor()
		{
			m_consoleOpen = false;
		}

		private void SendUpdateProgramRequest(string program)
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.UpdateProgram, StringCompressor.CompressString(program));
		}

		[Event(null, 1095)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void UpdateProgram(byte[] program)
		{
			string text = StringCompressor.DecompressString(program);
			if (Sync.IsServer && (text.Length > 100000 || (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value))))
			{
				MyEventContext.ValidationFailed();
			}
			else
			{
				UpdateProgram(text);
			}
		}

		private void SendRunProgramRequest(string argument, UpdateType updateSource)
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.RunProgramRequest, StringCompressor.CompressString(argument ?? string.Empty), updateSource);
		}

		[Event(null, 1120)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void RunProgramRequest(byte[] argument, UpdateType updateType)
		{
			Run(StringCompressor.DecompressString(argument), updateType);
		}

		bool Sandbox.ModAPI.Ingame.IMyProgrammableBlock.TryRun(string argument)
		{
			if (m_instance == null || m_isRunning || !base.IsWorking || !base.IsFunctional)
			{
				return false;
			}
			if (!base.IsFunctional || !base.IsWorking)
			{
				return false;
			}
			string response;
			ScriptTerminationReason num = ExecuteCode(argument ?? "", UpdateType.Script, out response);
			SetDetailedInfo(response);
			if (num == ScriptTerminationReason.InstructionOverflow)
			{
				throw new ScriptOutOfRangeException("This exception crashes the game only when Mod runs programmable block with script that exceeds the stack limit. Consider catching this exception or rewriting your script.");
			}
			return num == ScriptTerminationReason.None;
		}

		void Sandbox.ModAPI.IMyProgrammableBlock.Recompile()
		{
			SendRecompile();
		}

		void Sandbox.ModAPI.IMyProgrammableBlock.Run()
		{
			Run(TerminalRunArgument, UpdateType.Mod);
		}

		void Sandbox.ModAPI.IMyProgrammableBlock.Run(string argument)
		{
			Run(argument, UpdateType.Mod);
		}

		void Sandbox.ModAPI.IMyProgrammableBlock.Run(string argument, UpdateType updateSource)
		{
			Run(argument, updateSource);
		}

		bool Sandbox.ModAPI.IMyProgrammableBlock.TryRun(string argument)
		{
			if (m_instance == null || m_isRunning || !base.IsWorking || !base.IsFunctional)
			{
				return false;
			}
			if (!base.IsFunctional || !base.IsWorking)
			{
				return false;
			}
			string response;
			ScriptTerminationReason num = ExecuteCode(argument ?? "", UpdateType.Mod, out response);
			SetDetailedInfo(response);
			if (num == ScriptTerminationReason.InstructionOverflow)
			{
				throw new ScriptOutOfRangeException("This exception crashes the game only when Mod runs programmable block with script that exceeds the stack limit. Consider catching this exception or rewriting your script.");
			}
			return num == ScriptTerminationReason.None;
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 1456)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.RemoveItems(panelIndex, selection);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		[Event(null, 1467)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			m_multiPanel?.SelectItems(panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 1478)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 1492)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			}
			RaisePropertiesChanged();
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			CreateTextBox(isEditable, new StringBuilder(PanelComponent.Text.ToString()), isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBoxMultiPanel);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 1547)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		[Event(null, 1558)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBoxMultiPanel != null)
			{
				if (m_textBoxMultiPanel.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(isPublic: true);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBoxMultiPanel.Description.Text.Remove(100000, m_textBoxMultiPanel.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBoxMultiPanel.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBoxMultiPanel);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					SendChangeDescriptionMessage(m_textBoxMultiPanel.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text.Clear().Append((object)description);
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyProgrammableBlock x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 1651)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			PanelComponent.Text.Clear().Append((object)stringBuilder);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}
	}
}
