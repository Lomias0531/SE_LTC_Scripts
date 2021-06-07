using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Localization
{
	public class MyLocalization
	{
		private class CampaignEvaluate : ITextEvaluator
		{
			private static Action m_loader;

			public CampaignEvaluate(Action loader)
			{
				m_loader = loader;
			}

			public string TokenEvaluate(string token, string context)
			{
				return Evaluate(token, context);
			}

			public static string Evaluate(string token, string context, bool assert = true)
			{
				MyLocalizationContext myLocalizationContext = Static[context ?? "Common"];
				if (myLocalizationContext == null)
				{
					return "";
				}
				if (myLocalizationContext.IdsCount == 0)
				{
					m_localizationLoadingLock.AcquireExclusive();
					if (myLocalizationContext.IdsCount == 0)
					{
						m_loader();
					}
					m_localizationLoadingLock.ReleaseExclusive();
				}
				StringBuilder stringBuilder = myLocalizationContext[MyStringId.GetOrCompute(token)];
				if (stringBuilder != null)
				{
					return stringBuilder.ToString();
				}
				return "";
			}
		}

		private class UniversalEvaluate : ITextEvaluator
		{
			public string TokenEvaluate(string token, string context)
			{
				string text = CampaignEvaluate.Evaluate(token, context, assert: false);
				if (string.IsNullOrEmpty(text))
				{
					StringBuilder stringBuilder = MyTexts.Get(MyStringId.GetOrCompute(token));
					if (stringBuilder != null)
					{
						return stringBuilder.ToString();
					}
					return "";
				}
				return text;
			}
		}

		public struct MyBundle
		{
			public MyStringId BundleId;

			public List<string> FilePaths;
		}

		private Dictionary<string, string> m_pathToContextTranslator = new Dictionary<string, string>();

		private static readonly FastResourceLock m_localizationLoadingLock = new FastResourceLock();

		public static readonly string LOCALIZATION_FOLDER = "Data\\Localization";

		private static readonly StringBuilder m_defaultLocalization = new StringBuilder("Failed localization attempt. Missing or not loaded contexts.");

		private static MyLocalization m_instance;

		private readonly Dictionary<MyStringId, MyLocalizationContext> m_contexts = new Dictionary<MyStringId, MyLocalizationContext>(MyStringId.Comparer);

		private readonly Dictionary<MyStringId, MyLocalizationContext> m_disposableContexts = new Dictionary<MyStringId, MyLocalizationContext>(MyStringId.Comparer);

		private readonly Dictionary<MyStringId, MyBundle> m_loadedBundles = new Dictionary<MyStringId, MyBundle>(MyStringId.Comparer);

		private static readonly string LOCALIZATION_TAG_CAMPAIGN = "LOCC";

		private static readonly string LOCALIZATION_TAG = "LOC";

		public Dictionary<string, string> PathToContextTranslator => m_pathToContextTranslator;

		public static MyLocalization Static
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyLocalization();
					m_instance.Init();
				}
				return m_instance;
			}
		}

		/// <summary>
		/// Simplified accessor.
		/// </summary>
		/// <param name="contextName">Name id of context.</param>
		/// <param name="tag">Tag to translate.</param>
		/// <returns>Localized String builder.</returns>
		public StringBuilder this[MyStringId contextName, MyStringId tag] => Get(contextName, tag);

		/// <summary>
		/// Simplified accessor. Preferably use the string id version.
		/// </summary>
		/// <param name="contexName">Name of the context.</param>
		/// <param name="tag">Name of the tag.</param>
		/// <returns></returns>
		public StringBuilder this[string contexName, string tag] => this[MyStringId.GetOrCompute(contexName), MyStringId.GetOrCompute(tag)];

		/// <summary>
		/// Simplified accessor.
		/// </summary>
		/// <param name="contextName">Name id of context.</param>
		/// <returns>Context of given name.</returns>
		public MyLocalizationContext this[MyStringId contextName]
		{
			get
			{
				if (m_disposableContexts.TryGetValue(contextName, out MyLocalizationContext value))
				{
					return value;
				}
				m_contexts.TryGetValue(contextName, out value);
				return value;
			}
		}

		/// <summary>
		/// Simplified accessor. Preferably use the string id version.
		/// </summary>
		/// <param name="contextName">Name id of context.</param>
		/// <returns>Context of given name.</returns>
		public MyLocalizationContext this[string contextName] => this[MyStringId.GetOrCompute(contextName)];

		/// <summary>
		/// Initializes singleton.
		/// </summary>
		public static void Initialize()
		{
			_ = Static;
		}

		private MyLocalization()
		{
		}

		private void Init()
		{
			foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, LOCALIZATION_FOLDER), "*.sbl", MySearchOption.AllDirectories))
			{
				LoadLocalizationFile(file, MyStringId.NullOrEmpty);
			}
		}

		public void InitLoader(Action loader)
		{
			MyTexts.RegisterEvaluator(LOCALIZATION_TAG_CAMPAIGN, new CampaignEvaluate(loader));
			MyTexts.RegisterEvaluator(LOCALIZATION_TAG, new UniversalEvaluate());
		}

		private MyLocalizationContext LoadLocalizationFile(string filePath, MyStringId bundleId, bool disposableContext = false)
		{
			if (!MyFileSystem.FileExists(filePath))
			{
				MyLog.Default.WriteLine("File does not exist: " + filePath);
				return null;
			}
			if (!MyObjectBuilderSerializer.DeserializeXML(filePath, out MyObjectBuilder_Localization objectBuilder))
			{
				return null;
			}
			MyLocalizationContext myLocalizationContext = CreateOrGetContext(MyStringId.GetOrCompute(objectBuilder.Context), disposableContext);
			myLocalizationContext.InsertFileInfo(new MyLocalizationContext.LocalizationFileInfo(filePath, objectBuilder, bundleId));
			return myLocalizationContext;
		}

		private MyLocalizationContext CreateOrGetContext(MyStringId contextId, bool disposable)
		{
			MyLocalizationContext value = null;
			if (!disposable)
			{
				m_contexts.TryGetValue(contextId, out value);
				if (value == null)
				{
					m_contexts.Add(contextId, value = new MyLocalizationContext(contextId));
					if (m_disposableContexts.TryGetValue(contextId, out MyLocalizationContext value2))
					{
						value.TwinContext = value2;
						value2.TwinContext = value;
					}
				}
			}
			else
			{
				m_disposableContexts.TryGetValue(contextId, out value);
				if (value == null)
				{
					m_disposableContexts.Add(contextId, value = new MyLocalizationContext(contextId));
					if (m_contexts.TryGetValue(contextId, out MyLocalizationContext value3))
					{
						value.TwinContext = value3;
						value3.TwinContext = value;
					}
				}
			}
			return value;
		}

		/// <summary>
		/// Switches all contexts to provided language.
		/// </summary>
		/// <param name="language">Language name.</param>
		public void Switch(string language)
		{
			foreach (MyLocalizationContext value in m_contexts.Values)
			{
				value.Switch(language);
			}
			foreach (MyLocalizationContext value2 in m_disposableContexts.Values)
			{
				value2.Switch(language);
			}
		}

		/// <summary>
		/// Tries to dispose disposable context.
		/// </summary>
		/// <param name="nameId">Name id of context.</param>
		public bool DisposeContext(MyStringId nameId)
		{
			if (m_disposableContexts.TryGetValue(nameId, out MyLocalizationContext value))
			{
				value.Dispose();
				m_disposableContexts.Remove(nameId);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Tries to dispose all disposable contexts.
		/// </summary>
		public void DisposeAll()
		{
			m_disposableContexts.Values.ForEach(delegate(MyLocalizationContext context)
			{
				context.Dispose();
			});
			m_disposableContexts.Clear();
		}

		/// <summary>
		/// Loads bundle of files under bundle id.
		/// </summary>
		/// <param name="bundle">Data bundle.</param>
		/// <param name="influencedContexts">Contexts that got some new data in the process.</param>
		/// <param name="disposableContexts">Created contexts will be disposable or persistent.</param>
		public void LoadBundle(MyBundle bundle, HashSet<MyLocalizationContext> influencedContexts = null, bool disposableContexts = true)
		{
			if (m_loadedBundles.ContainsKey(bundle.BundleId))
			{
				NotifyBundleConflict(bundle.BundleId);
			}
			else
			{
				foreach (string filePath in bundle.FilePaths)
				{
					MyLocalizationContext myLocalizationContext = LoadLocalizationFile(filePath, bundle.BundleId, disposableContext: true);
					if (myLocalizationContext != null)
					{
						influencedContexts?.Add(myLocalizationContext);
					}
					if (myLocalizationContext != null)
					{
						if (m_pathToContextTranslator.ContainsKey(filePath))
						{
							m_pathToContextTranslator[filePath] = myLocalizationContext.Name.String;
						}
						else
						{
							m_pathToContextTranslator.Add(filePath, myLocalizationContext.Name.String);
						}
					}
				}
			}
		}

		/// <summary>
		/// Unloads bundle of files from the system by given id.
		/// </summary>
		/// <param name="bundleId"></param>
		public void UnloadBundle(MyStringId bundleId)
		{
			foreach (MyLocalizationContext value in m_contexts.Values)
			{
				value.UnloadBundle(bundleId);
			}
			foreach (MyLocalizationContext value2 in m_disposableContexts.Values)
			{
				value2.UnloadBundle(bundleId);
			}
		}

		/// <summary>
		/// Returns localization for given context and id.
		/// </summary>
		/// <param name="contextId">Context name id.</param>
		/// <param name="id">Message identifier.</param>
		/// <returns>String builder with localization.</returns>
		public StringBuilder Get(MyStringId contextId, MyStringId id)
		{
			StringBuilder stringBuilder = m_defaultLocalization;
			if (m_disposableContexts.TryGetValue(contextId, out MyLocalizationContext value))
			{
				stringBuilder = value.Localize(id);
				if (stringBuilder != null)
				{
					return stringBuilder;
				}
			}
			if (m_contexts.TryGetValue(contextId, out value))
			{
				stringBuilder = value.Localize(id);
			}
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder();
			}
			return stringBuilder;
		}

		private void NotifyBundleConflict(MyStringId bundleId)
		{
			string msg = "MyLocalization: Bundle conflict - Bundle already loaded: " + bundleId.String;
			MyLog.Default.WriteLine(msg);
		}
	}
}
