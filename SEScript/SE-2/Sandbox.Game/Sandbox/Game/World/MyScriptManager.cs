using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Scripting;
using VRage.Utils;

namespace Sandbox.Game.World
{
	public class MyScriptManager
	{
		public static MyScriptManager Static;

		private string[] Separators = new string[1]
		{
			" "
		};

		public readonly Dictionary<MyModContext, HashSet<MyStringId>> ScriptsPerMod = new Dictionary<MyModContext, HashSet<MyStringId>>();

		public Dictionary<MyStringId, Assembly> Scripts = new Dictionary<MyStringId, Assembly>(MyStringId.Comparer);

		public Dictionary<Type, HashSet<Type>> EntityScripts = new Dictionary<Type, HashSet<Type>>();

		public Dictionary<Tuple<Type, string>, HashSet<Type>> SubEntityScripts = new Dictionary<Tuple<Type, string>, HashSet<Type>>();

		public Dictionary<string, Type> StatScripts = new Dictionary<string, Type>();

		public Dictionary<MyStringId, Type> InGameScripts = new Dictionary<MyStringId, Type>(MyStringId.Comparer);

		public Dictionary<MyStringId, StringBuilder> InGameScriptsCode = new Dictionary<MyStringId, StringBuilder>(MyStringId.Comparer);

		private List<string> m_errors = new List<string>();

		private List<MyScriptCompiler.Message> m_messages = new List<MyScriptCompiler.Message>();

		private List<string> m_cachedFiles = new List<string>();

		private static Dictionary<string, bool> testFiles = new Dictionary<string, bool>();

		public static string CompatibilityUsings = "using VRage;\r\nusing VRage.Game.Components;\r\nusing VRage.ObjectBuilders;\r\nusing VRage.ModAPI;\r\nusing VRage.Game.ModAPI;\r\nusing Sandbox.Common.ObjectBuilders;\r\nusing VRage.Game;\r\nusing Sandbox.ModAPI;\r\nusing VRage.Game.ModAPI.Interfaces;\r\nusing SpaceEngineers.Game.ModAPI;\r\n#line 1\r\n";

		private static Dictionary<string, string> m_compatibilityChanges = new Dictionary<string, string>
		{
			{
				"using VRage.Common.Voxels;",
				""
			},
			{
				"VRage.Common.Voxels.",
				""
			},
			{
				"Sandbox.ModAPI.IMyEntity",
				"VRage.ModAPI.IMyEntity"
			},
			{
				"Sandbox.Common.ObjectBuilders.MyObjectBuilder_EntityBase",
				"VRage.ObjectBuilders.MyObjectBuilder_EntityBase"
			},
			{
				"Sandbox.Common.MyEntityUpdateEnum",
				"VRage.ModAPI.MyEntityUpdateEnum"
			},
			{
				"using Sandbox.Common.ObjectBuilders.Serializer;",
				""
			},
			{
				"Sandbox.Common.ObjectBuilders.Serializer.",
				""
			},
			{
				"Sandbox.Common.MyMath",
				"VRageMath.MyMath"
			},
			{
				"Sandbox.Common.ObjectBuilders.VRageData.SerializableVector3I",
				"VRage.SerializableVector3I"
			},
			{
				"VRage.Components",
				"VRage.Game.Components"
			},
			{
				"using Sandbox.Common.ObjectBuilders.VRageData;",
				""
			},
			{
				"Sandbox.Common.ObjectBuilders.MyOnlineModeEnum",
				"VRage.Game.MyOnlineModeEnum"
			},
			{
				"Sandbox.Common.ObjectBuilders.Definitions.MyDamageType",
				"VRage.Game.MyDamageType"
			},
			{
				"Sandbox.Common.ObjectBuilders.VRageData.SerializableBlockOrientation",
				"VRage.Game.SerializableBlockOrientation"
			},
			{
				"Sandbox.Common.MySessionComponentDescriptor",
				"VRage.Game.Components.MySessionComponentDescriptor"
			},
			{
				"Sandbox.Common.MyUpdateOrder",
				"VRage.Game.Components.MyUpdateOrder"
			},
			{
				"Sandbox.Common.MySessionComponentBase",
				"VRage.Game.Components.MySessionComponentBase"
			},
			{
				"Sandbox.Common.MyFontEnum",
				"VRage.Game.MyFontEnum"
			},
			{
				"Sandbox.Common.MyRelationsBetweenPlayerAndBlock",
				"VRage.Game.MyRelationsBetweenPlayerAndBlock"
			},
			{
				"Sandbox.Common.Components",
				"VRage.Game.Components"
			},
			{
				"using Sandbox.Common.Input;",
				""
			},
			{
				"using Sandbox.Common.ModAPI;",
				""
			}
		};

		private Dictionary<string, string> m_scriptsToSave = new Dictionary<string, string>();

		public void LoadData()
		{
			MySandboxGame.Log.WriteLine("MyScriptManager.LoadData() - START");
			MySandboxGame.Log.IncreaseIndent();
			Static = this;
			Scripts.Clear();
			EntityScripts.Clear();
			SubEntityScripts.Clear();
			TryAddEntityScripts(MyModContext.BaseGame, MyPlugins.SandboxAssembly);
			TryAddEntityScripts(MyModContext.BaseGame, MyPlugins.SandboxGameAssembly);
			if (MySession.Static.CurrentPath != null)
			{
				LoadScripts(MySession.Static.CurrentPath, MyModContext.BaseGame);
			}
			if (MySession.Static.Mods != null)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in MySession.Static.Mods)
				{
					MyModContext myModContext = new MyModContext();
					myModContext.Init(mod);
					try
					{
						LoadScripts(mod.GetPath(), myModContext);
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine($"Fatal error compiling {myModContext.ModId}: {myModContext.ModName}. This item is likely not a mod and should be removed from the mod list.");
						MyLog.Default.WriteLine(ex);
						throw;
					}
				}
			}
			foreach (Assembly value in Scripts.Values)
			{
				if (MyFakes.ENABLE_TYPES_FROM_MODS)
				{
					MyGlobalTypeMetadata.Static.RegisterAssembly(value);
				}
				MySandboxGame.Log.WriteLine($"Script loaded: {value.FullName}");
			}
			MyTextSurfaceScriptFactory.LoadScripts();
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyScriptManager.LoadData() - END");
		}

		private void LoadScripts(string path, MyModContext mod = null)
		{
			if (MyFakes.ENABLE_SCRIPTS)
			{
				string text = Path.Combine(path, "Data", "Scripts");
				IEnumerable<string> files = MyFileSystem.GetFiles(text, "*.cs");
				try
				{
					if (files.Count() == 0)
					{
						return;
					}
				}
				catch (Exception)
				{
					MySandboxGame.Log.WriteLine($"Failed to load scripts from: {path}");
					return;
				}
				bool zipped = MyZipFileProvider.IsZipFile(path);
				List<string> list = new List<string>();
				string[] array = files.First().Split(new char[1]
				{
					'\\'
				});
				int num = text.Split(new char[1]
				{
					'\\'
				}).Length;
				if (num >= array.Length)
				{
					MySandboxGame.Log.WriteLine(string.Format("\nWARNING: Mod \"{0}\" contains misplaced .cs files ({2}). Scripts are supposed to be at {1}.\n", path, text, files.First()));
					return;
				}
				string text2 = array[num];
				foreach (string item in files)
				{
					array = item.Split(new char[1]
					{
						'\\'
					});
					if (!(array[array.Length - 1].Split(new char[1]
					{
						'.'
					}).Last() != "cs"))
					{
						int num2 = Array.IndexOf(array, "Scripts") + 1;
						if (array[num2] == text2)
						{
							list.Add(item);
						}
						else
						{
							Compile(list, $"{mod.ModId}_{text2}", zipped, mod);
							list.Clear();
							text2 = array[num];
							list.Add(item);
						}
					}
				}
				Compile(list.ToArray(), Path.Combine(MyFileSystem.ModsPath, $"{mod.ModId}_{text2}"), zipped, mod);
				list.Clear();
			}
		}

		private void Compile(IEnumerable<string> scriptFiles, string assemblyName, bool zipped, MyModContext context)
		{
			Assembly assembly = null;
			bool flag = false;
			if (zipped)
			{
				string text = Path.Combine(Path.GetTempPath(), MyPerGameSettings.BasicGameInfo.GameNameSafe, Path.GetFileName(assemblyName));
				if (Directory.Exists(text))
				{
					Directory.Delete(text, recursive: true);
				}
				foreach (string scriptFile in scriptFiles)
				{
					try
					{
						string text2 = Path.Combine(text, Path.GetFileName(scriptFile));
						using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(scriptFile)))
						{
							using (StreamWriter streamWriter = new StreamWriter(MyFileSystem.OpenWrite(text2)))
							{
								streamWriter.Write(streamReader.ReadToEnd());
							}
						}
						m_cachedFiles.Add(text2);
					}
					catch (Exception ex)
					{
						MySandboxGame.Log.WriteLine(ex);
						MyDefinitionErrors.Add(context, $"Cannot load {Path.GetFileName(scriptFile)}", TErrorSeverity.Error);
						MyDefinitionErrors.Add(context, ex.Message, TErrorSeverity.Error);
					}
				}
				assembly = MyScriptCompiler.Static.Compile(MyApiTarget.Mod, assemblyName, m_cachedFiles.Select((string file) => new Script(file, UpdateCompatibility(file))), m_messages, context.ModName).Result;
				flag = (assembly != null);
			}
			else
			{
				assembly = MyScriptCompiler.Static.Compile(MyApiTarget.Mod, assemblyName, scriptFiles.Select((string file) => new Script(file, UpdateCompatibility(file))), m_messages, context.ModName).Result;
				flag = (assembly != null);
			}
			if (assembly != null && flag)
			{
				AddAssembly(context, MyStringId.GetOrCompute(assemblyName), assembly);
			}
			else
			{
				MyDefinitionErrors.Add(context, $"Compilation of {assemblyName} failed:", TErrorSeverity.Error);
				MySandboxGame.Log.IncreaseIndent();
				foreach (MyScriptCompiler.Message message in m_messages)
				{
					MyDefinitionErrors.Add(context, message.Text, message.Severity);
				}
				MySandboxGame.Log.DecreaseIndent();
				m_errors.Clear();
			}
			m_cachedFiles.Clear();
		}

		public static string UpdateCompatibility(string filename)
		{
			using (Stream stream = MyFileSystem.OpenRead(filename))
			{
				if (stream != null)
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						string text = streamReader.ReadToEnd();
						text = text.Insert(0, CompatibilityUsings);
						foreach (KeyValuePair<string, string> compatibilityChange in m_compatibilityChanges)
						{
							text = text.Replace(compatibilityChange.Key, compatibilityChange.Value);
						}
						return text;
					}
				}
			}
			return null;
		}

		private void AddAssembly(MyModContext context, MyStringId myStringId, Assembly assembly)
		{
			if (Scripts.ContainsKey(myStringId))
			{
				MySandboxGame.Log.WriteLine($"Script already in list {myStringId.ToString()}");
				return;
			}
			if (!ScriptsPerMod.TryGetValue(context, out HashSet<MyStringId> value))
			{
				value = new HashSet<MyStringId>();
				ScriptsPerMod.Add(context, value);
			}
			value.Add(myStringId);
			Scripts.Add(myStringId, assembly);
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				MyConsole.AddCommand(new MyCommandScript(types[i]));
			}
			TryAddEntityScripts(context, assembly);
			AddStatScripts(assembly);
		}

		private void TryAddEntityScripts(MyModContext context, Assembly assembly)
		{
			Type typeFromHandle = typeof(MyGameLogicComponent);
			Type typeFromHandle2 = typeof(MyObjectBuilder_Base);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(MyEntityComponentDescriptor), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					MyEntityComponentDescriptor myEntityComponentDescriptor = (MyEntityComponentDescriptor)customAttributes[0];
					try
					{
						if (!myEntityComponentDescriptor.EntityUpdate.HasValue)
						{
							MyDefinitionErrors.Add(context, "**WARNING!**\r\nScript for " + myEntityComponentDescriptor.EntityBuilderType.Name + " is using the obsolete MyEntityComponentDescriptor overload!\r\nYou must use the 3 parameter overload to properly register script updates!\r\nThis script will most likely not work as intended!\r\n**WARNING!**", TErrorSeverity.Warning);
						}
						if (myEntityComponentDescriptor.EntityBuilderSubTypeNames != null && myEntityComponentDescriptor.EntityBuilderSubTypeNames.Length != 0)
						{
							string[] entityBuilderSubTypeNames = myEntityComponentDescriptor.EntityBuilderSubTypeNames;
							foreach (string item in entityBuilderSubTypeNames)
							{
								if (typeFromHandle.IsAssignableFrom(type) && typeFromHandle2.IsAssignableFrom(myEntityComponentDescriptor.EntityBuilderType))
								{
									Tuple<Type, string> key = new Tuple<Type, string>(myEntityComponentDescriptor.EntityBuilderType, item);
									if (!SubEntityScripts.TryGetValue(key, out HashSet<Type> value))
									{
										value = new HashSet<Type>();
										SubEntityScripts.Add(key, value);
									}
									else
									{
										MyDefinitionErrors.Add(context, "Possible entity type script logic collision", TErrorSeverity.Notice);
									}
									value.Add(type);
								}
							}
						}
						else if (typeFromHandle.IsAssignableFrom(type) && typeFromHandle2.IsAssignableFrom(myEntityComponentDescriptor.EntityBuilderType))
						{
							if (!EntityScripts.TryGetValue(myEntityComponentDescriptor.EntityBuilderType, out HashSet<Type> value2))
							{
								value2 = new HashSet<Type>();
								EntityScripts.Add(myEntityComponentDescriptor.EntityBuilderType, value2);
							}
							else
							{
								MyDefinitionErrors.Add(context, "Possible entity type script logic collision", TErrorSeverity.Notice);
							}
							value2.Add(type);
						}
					}
					catch (Exception)
					{
						MySandboxGame.Log.WriteLine("Exception during loading of type : " + type.Name);
					}
				}
			}
		}

		private void AddStatScripts(Assembly assembly)
		{
			Type typeFromHandle = typeof(MyStatLogic);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(MyStatLogicDescriptor), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					string componentName = ((MyStatLogicDescriptor)customAttributes[0]).ComponentName;
					if (typeFromHandle.IsAssignableFrom(type) && !StatScripts.ContainsKey(componentName))
					{
						StatScripts.Add(componentName, type);
					}
				}
			}
		}

		protected void UnloadData()
		{
			Scripts.Clear();
			InGameScripts.Clear();
			InGameScriptsCode.Clear();
			EntityScripts.Clear();
			m_scriptsToSave.Clear();
			MyTextSurfaceScriptFactory.UnloadScripts();
		}

		public void SaveData()
		{
			WriteScripts(MySession.Static.CurrentPath);
		}

		private void ReadScripts(string path)
		{
			string text = Path.Combine(path, "Data", "Scripts");
			IEnumerable<string> files = MyFileSystem.GetFiles(text, "*.cs");
			try
			{
				if (files.Count() == 0)
				{
					return;
				}
			}
			catch (Exception)
			{
				MySandboxGame.Log.WriteLine($"Failed to load scripts from: {path}");
				return;
			}
			foreach (string item in files)
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(item)))
					{
						m_scriptsToSave.Add(item.Substring(text.Length + 1), streamReader.ReadToEnd());
					}
				}
				catch (Exception ex2)
				{
					MySandboxGame.Log.WriteLine(ex2);
				}
			}
		}

		private void WriteScripts(string path)
		{
			try
			{
				string arg = Path.Combine(path, "Data", "Scripts");
				foreach (KeyValuePair<string, string> item in m_scriptsToSave)
				{
					using (StreamWriter streamWriter = new StreamWriter(MyFileSystem.OpenWrite($"{arg}\\{item.Key}")))
					{
						streamWriter.Write(item.Value);
					}
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
			}
		}

		public void Init(MyObjectBuilder_ScriptManager scriptBuilder)
		{
			if (scriptBuilder != null)
			{
				MyAPIUtilities.Static.Variables = scriptBuilder.variables.Dictionary;
			}
			LoadData();
		}

		public MyObjectBuilder_ScriptManager GetObjectBuilder()
		{
			return new MyObjectBuilder_ScriptManager
			{
				variables = 
				{
					Dictionary = MyAPIUtilities.Static.Variables
				}
			};
		}

		public Type GetScriptType(MyModContext context, string qualifiedTypeName)
		{
			if (!ScriptsPerMod.TryGetValue(context, out HashSet<MyStringId> value))
			{
				return null;
			}
			foreach (MyStringId item in value)
			{
				Type type = Scripts[item].GetType(qualifiedTypeName);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}
	}
}
