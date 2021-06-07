using Sandbox;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Definitions.SafeZone;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Scripting;
using VRageMath;

namespace SpaceEngineers.Game
{
	public class MySpaceGameCustomInitialization : MySandboxGame.IGameCustomInitialization
	{
		public void InitIlChecker()
		{
			using (IMyWhitelistBatch myWhitelistBatch = MyScriptCompiler.Static.Whitelist.OpenBatch())
			{
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel), typeof(LandingGearMode));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(SpaceEngineers.Game.ModAPI.IMyButtonPanel), typeof(MySafeZoneBlockDefinition));
			}
		}

		public void InitIlCompiler()
		{
			MyScriptCompiler.Static.IgnoredWarnings.Add("CS0105");
			MyModWatchdog.Init(MySandboxGame.Static.UpdateThread);
			MyScriptCompiler.Static.AddReferencedAssemblies(Path.Combine(Assembly.Load("netstandard").Location), Path.Combine(MyFileSystem.ExePath, "Sandbox.Game.dll"), Path.Combine(MyFileSystem.ExePath, "Sandbox.Common.dll"), Path.Combine(MyFileSystem.ExePath, "Sandbox.Graphics.dll"), Path.Combine(MyFileSystem.ExePath, "VRage.dll"), Path.Combine(MyFileSystem.ExePath, "VRage.Library.dll"), Path.Combine(MyFileSystem.ExePath, "VRage.Math.dll"), Path.Combine(MyFileSystem.ExePath, "VRage.Game.dll"), Path.Combine(MyFileSystem.ExePath, "VRage.Render.dll"), Path.Combine(MyFileSystem.ExePath, "VRage.Input.dll"), Path.Combine(MyFileSystem.ExePath, "SpaceEngineers.ObjectBuilders.dll"), Path.Combine(MyFileSystem.ExePath, "SpaceEngineers.Game.dll"), Path.Combine(MyFileSystem.ExePath, "System.Collections.Immutable.dll"), Path.Combine(MyFileSystem.ExePath, "ProtoBuf.Net.Core.dll"));
			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly x) => x.GetName().Name == "System.Runtime");
			if (assembly != null)
			{
				MyScriptCompiler.Static.AddReferencedAssemblies(assembly.Location);
			}
			assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly x) => x.GetName().Name == "System.Collections");
			if (assembly != null)
			{
				MyScriptCompiler.Static.AddReferencedAssemblies(assembly.Location);
			}
			MyScriptCompiler.Static.AddImplicitIngameNamespacesFromTypes(typeof(MyTuple), typeof(Vector2), typeof(VRage.Game.Game), typeof(ITerminalAction), typeof(Sandbox.ModAPI.Ingame.IMyGridTerminalSystem), typeof(MyModelComponent), typeof(IMyComponentAggregate), typeof(ListReader<>), typeof(MyObjectBuilder_FactionDefinition), typeof(IMyCubeBlock), typeof(MyIni), typeof(ImmutableArray), typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent), typeof(MySprite));
			MyScriptCompiler.Static.AddConditionalCompilationSymbols(GetPrefixedBranchName(), "STABLE", string.Empty, string.Empty, "VERSION_" + ((Version)MyFinalBuildConstants.APP_VERSION).Minor, "BUILD_" + ((Version)MyFinalBuildConstants.APP_VERSION).Build);
			if (MyFakes.ENABLE_ROSLYN_SCRIPT_DIAGNOSTICS)
			{
				MyScriptCompiler.Static.DiagnosticOutputPath = Path.Combine(MyFileSystem.UserDataPath, "ScriptDiagnostics");
			}
		}

		private string GetPrefixedBranchName()
		{
			string branchName = MyGameService.BranchName;
			branchName = ((!string.IsNullOrEmpty(branchName)) ? Regex.Replace(branchName, "[^a-zA-Z0-9_]", "_").ToUpper() : "STABLE");
			return "BRANCH_" + branchName;
		}
	}
}
