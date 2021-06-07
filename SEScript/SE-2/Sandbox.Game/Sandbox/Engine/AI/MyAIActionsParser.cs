using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.ObjectBuilders.AI;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Engine.AI
{
	[PreloadRequired]
	public static class MyAIActionsParser
	{
		private static bool ENABLE_PARSING;

		private static string SERIALIZE_PATH;

		static MyAIActionsParser()
		{
			ENABLE_PARSING = true;
			SERIALIZE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MedievalEngineers", "BehaviorDescriptors.xml");
			_ = ENABLE_PARSING;
		}

		public static HashSet<Type> GetAllTypesFromAssemblies()
		{
			HashSet<Type> hashSet = new HashSet<Type>();
			GetTypesFromAssembly(MyPlugins.SandboxGameAssembly, hashSet);
			GetTypesFromAssembly(MyPlugins.GameAssembly, hashSet);
			GetTypesFromAssembly(MyPlugins.UserAssemblies, hashSet);
			return hashSet;
		}

		private static void GetTypesFromAssembly(Assembly[] assemblies, HashSet<Type> outputTypes)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					GetTypesFromAssembly(assemblies[i], outputTypes);
				}
			}
		}

		private static void GetTypesFromAssembly(Assembly assembly, HashSet<Type> outputTypes)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(inherit: false);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					if (customAttributes[j] is MyBehaviorDescriptorAttribute)
					{
						outputTypes.Add(type);
					}
				}
			}
		}

		private static Dictionary<string, List<MethodInfo>> ParseMethods(HashSet<Type> types)
		{
			Dictionary<string, List<MethodInfo>> dictionary = new Dictionary<string, List<MethodInfo>>();
			foreach (Type type in types)
			{
				MyBehaviorDescriptorAttribute customAttribute = type.GetCustomAttribute<MyBehaviorDescriptorAttribute>();
				MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					MyBehaviorTreeActionAttribute customAttribute2 = methodInfo.GetCustomAttribute<MyBehaviorTreeActionAttribute>();
					if (customAttribute2 != null && customAttribute2.ActionType == MyBehaviorTreeActionType.BODY)
					{
						bool flag = true;
						ParameterInfo[] parameters = methodInfo.GetParameters();
						foreach (ParameterInfo element in parameters)
						{
							BTParamAttribute customAttribute3 = element.GetCustomAttribute<BTParamAttribute>();
							BTMemParamAttribute customAttribute4 = element.GetCustomAttribute<BTMemParamAttribute>();
							if (customAttribute3 == null && customAttribute4 == null)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							List<MethodInfo> value = null;
							if (!dictionary.TryGetValue(customAttribute.DescriptorCategory, out value))
							{
								value = new List<MethodInfo>();
								dictionary[customAttribute.DescriptorCategory] = value;
							}
							value.Add(methodInfo);
						}
					}
				}
			}
			return dictionary;
		}

		private static void SerializeToXML(string path, Dictionary<string, List<MethodInfo>> data)
		{
			MyAIBehaviorData myAIBehaviorData = MyObjectBuilderSerializer.CreateNewObject<MyAIBehaviorData>();
			myAIBehaviorData.Entries = new MyAIBehaviorData.CategorizedData[data.Count];
			int num = 0;
			foreach (KeyValuePair<string, List<MethodInfo>> datum in data)
			{
				MyAIBehaviorData.CategorizedData categorizedData = new MyAIBehaviorData.CategorizedData();
				categorizedData.Category = datum.Key;
				categorizedData.Descriptors = new MyAIBehaviorData.ActionData[datum.Value.Count];
				int num2 = 0;
				foreach (MethodInfo item in datum.Value)
				{
					MyAIBehaviorData.ActionData actionData = new MyAIBehaviorData.ActionData();
					MyBehaviorTreeActionAttribute customAttribute = item.GetCustomAttribute<MyBehaviorTreeActionAttribute>();
					actionData.ActionName = customAttribute.ActionName;
					actionData.ReturnsRunning = customAttribute.ReturnsRunning;
					ParameterInfo[] parameters = item.GetParameters();
					actionData.Parameters = new MyAIBehaviorData.ParameterData[parameters.Length];
					int num3 = 0;
					ParameterInfo[] array = parameters;
					foreach (ParameterInfo parameterInfo in array)
					{
						BTMemParamAttribute customAttribute2 = parameterInfo.GetCustomAttribute<BTMemParamAttribute>();
						BTParamAttribute customAttribute3 = parameterInfo.GetCustomAttribute<BTParamAttribute>();
						MyAIBehaviorData.ParameterData parameterData = new MyAIBehaviorData.ParameterData();
						parameterData.Name = parameterInfo.Name;
						parameterData.TypeFullName = parameterInfo.ParameterType.FullName;
						if (customAttribute2 != null)
						{
							parameterData.MemType = customAttribute2.MemoryType;
						}
						else if (customAttribute3 != null)
						{
							parameterData.MemType = MyMemoryParameterType.PARAMETER;
						}
						actionData.Parameters[num3] = parameterData;
						num3++;
					}
					categorizedData.Descriptors[num2] = actionData;
					num2++;
				}
				myAIBehaviorData.Entries[num] = categorizedData;
				num++;
			}
			MyObjectBuilderSerializer.SerializeXML(path, compress: false, myAIBehaviorData);
		}
	}
}
