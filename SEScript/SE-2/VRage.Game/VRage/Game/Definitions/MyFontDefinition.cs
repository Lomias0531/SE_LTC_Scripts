using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FontDefinition), null)]
	public class MyFontDefinition : MyDefinitionBase
	{
		private class VRage_Game_Definitions_MyFontDefinition_003C_003EActor : IActivator, IActivator<MyFontDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFontDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFontDefinition CreateInstance()
			{
				return new MyFontDefinition();
			}

			MyFontDefinition IActivator<MyFontDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static Dictionary<MyStringHash, MyFont> m_fontsById = new Dictionary<MyStringHash, MyFont>();

		private MyObjectBuilder_FontDefinition m_ob;

		private List<MyObjectBuilder_FontData> m_currentResources;

		/// <summary>
		/// Checks the validity of inderlying data.
		/// </summary>
		public bool IsValid => m_ob != null;

		/// <summary>
		/// Old resource path used by old mods.
		/// </summary>
		public string CompatibilityPath => m_ob.Path;

		/// <summary>
		/// Texture color multiplier.
		/// </summary>
		public Color? ColorMask => m_ob.ColorMask;

		/// <summary>
		/// True will make this definition load as DEBUG font and default fallback.
		/// </summary>
		public bool Default => m_ob.Default;

		/// <summary>
		///
		/// </summary>
		public IEnumerable<MyObjectBuilder_FontData> Resources => m_currentResources;

		/// <summary>
		/// Call to switch the Resources to different language variant.
		/// </summary>
		/// <param name="language">Language string identifier base on MyLanguagesEnum.</param>
		public void UseLanguage(string language)
		{
			MyObjectBuilder_FontDefinition.LanguageResources languageResources = m_ob.LanguageSpecificDefinitions.FirstOrDefault((MyObjectBuilder_FontDefinition.LanguageResources x) => x.Language == language);
			if (languageResources == null)
			{
				m_currentResources = m_ob.Resources;
				return;
			}
			m_currentResources = languageResources.Resources;
			SortBySize(m_currentResources);
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			m_ob = (builder as MyObjectBuilder_FontDefinition);
			m_currentResources = m_ob.Resources;
			SortBySize(m_currentResources);
			m_fontsById[Id.SubtypeId] = ((!string.IsNullOrEmpty(CompatibilityPath)) ? new MyFont(CompatibilityPath) : new MyFont(Resources.FirstOrDefault()?.Path));
		}

		private static void SortBySize(List<MyObjectBuilder_FontData> resources)
		{
			resources?.Sort((MyObjectBuilder_FontData dataX, MyObjectBuilder_FontData dataY) => dataX.Size.CompareTo(dataY.Size));
		}

		public static MyFont GetFont(MyStringHash fontId)
		{
			return m_fontsById[fontId];
		}

		public static Vector2 MeasureStringRaw(string font, StringBuilder text, float scale)
		{
			if (m_fontsById.TryGetValue(MyStringHash.GetOrCompute(font), out MyFont value))
			{
				return value.MeasureString(text, scale);
			}
			return Vector2.Zero;
		}
	}
}
