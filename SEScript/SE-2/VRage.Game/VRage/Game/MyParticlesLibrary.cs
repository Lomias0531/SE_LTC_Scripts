using System.Collections.Generic;
using System.IO;
using System.Xml;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	public class MyParticlesLibrary
	{
		private static Dictionary<string, MyParticleEffect> m_libraryEffectsString = new Dictionary<string, MyParticleEffect>();

		private static Dictionary<int, MyParticleEffect> m_libraryEffectsId = new Dictionary<int, MyParticleEffect>();

		private static readonly int Version = 0;

		private static string m_loadedFile;

		public static string DefaultLibrary = "Particles\\MyParticlesLibrary.mwl";

		public static int RedundancyDetected = 0;

		public static string LoadedFile => m_loadedFile;

		public static void InitDefault()
		{
		}

		public static void AddParticleEffect(MyParticleEffect effect)
		{
			if (m_libraryEffectsString.TryGetValue(effect.Name, out MyParticleEffect value))
			{
				RemoveParticleEffect(value);
			}
			m_libraryEffectsString[effect.Name] = effect;
			m_libraryEffectsId[effect.ID] = effect;
		}

		public static bool EffectExists(string name)
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			return m_libraryEffectsString.ContainsKey(name);
		}

		public static MyParticleEffect GetParticleEffect(string name)
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			return m_libraryEffectsString[name];
		}

		public static void UpdateParticleEffectID(int id)
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			m_libraryEffectsId.TryGetValue(id, out MyParticleEffect value);
			if (value != null)
			{
				m_libraryEffectsId.Remove(id);
				m_libraryEffectsId.Add(value.ID, value);
			}
		}

		public static void UpdateParticleEffectName(string name)
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			m_libraryEffectsString.TryGetValue(name, out MyParticleEffect value);
			if (value != null)
			{
				m_libraryEffectsString.Remove(name);
				m_libraryEffectsString.Add(value.Name, value);
			}
		}

		public static void RemoveParticleEffect(string name, bool instant = true)
		{
			m_libraryEffectsString.TryGetValue(name, out MyParticleEffect value);
			if (value != null)
			{
				m_libraryEffectsString.Remove(value.Name);
				m_libraryEffectsId.Remove(value.ID);
				value.Close(notify: false, instant);
				MyParticlesManager.EffectsPool.Deallocate(value);
			}
		}

		public static void RemoveParticleEffect(MyParticleEffect effect)
		{
			RemoveParticleEffect(effect.Name);
		}

		public static IEnumerable<int> GetParticleEffectsIDs()
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			return m_libraryEffectsId.Keys;
		}

		public static IEnumerable<string> GetParticleEffectsNames()
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			return m_libraryEffectsString.Keys;
		}

		public static IReadOnlyDictionary<int, MyParticleEffect> GetParticleEffectsById()
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			return m_libraryEffectsId;
		}

		public static IReadOnlyDictionary<string, MyParticleEffect> GetParticleEffectsByName()
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			return m_libraryEffectsString;
		}

		public static bool GetParticleEffectsID(string name, out int id)
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			if (m_libraryEffectsString.TryGetValue(name, out MyParticleEffect value))
			{
				id = value.GetID();
				return true;
			}
			id = -1;
			return false;
		}

		public static bool GetParticleEffectsID(int id, out string name)
		{
			if (LoadedFile == null)
			{
				InitDefault();
			}
			if (m_libraryEffectsId.TryGetValue(id, out MyParticleEffect value))
			{
				name = value.Name;
				return true;
			}
			name = string.Empty;
			return false;
		}

		public static void Serialize(string file)
		{
			using (FileStream output = File.Create(file))
			{
				XmlWriterSettings settings = new XmlWriterSettings
				{
					Indent = true
				};
				using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
				{
					Serialize(xmlWriter);
					xmlWriter.Flush();
				}
				m_loadedFile = file;
			}
		}

		public static void Deserialize(string file)
		{
			try
			{
				string text = Path.Combine(MyFileSystem.ContentPath, file);
				using (Stream input = MyFileSystem.OpenRead(text))
				{
					XmlReaderSettings settings = new XmlReaderSettings
					{
						IgnoreWhitespace = true
					};
					using (XmlReader reader = XmlReader.Create(input, settings))
					{
						Deserialize(reader);
					}
					m_loadedFile = text;
				}
			}
			catch (IOException ex)
			{
				MyLog.Default.WriteLine("ERROR: Failed to load particles library.");
				MyLog.Default.WriteLine(ex);
				MyVRage.Platform.MessageBox(ex.Message, "Loading Error", MessageBoxOptions.OkOnly);
				throw;
			}
		}

		public static void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Definitions");
			writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
			writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
			writer.WriteStartElement("ParticleEffects");
			foreach (KeyValuePair<string, MyParticleEffect> item in m_libraryEffectsString)
			{
				item.Value.Serialize(writer);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public static void Close()
		{
			foreach (MyParticleEffect value in m_libraryEffectsString.Values)
			{
				value.Close(notify: false, forceInstant: true);
				MyParticlesManager.EffectsPool.Deallocate(value);
			}
			m_libraryEffectsString.Clear();
			m_libraryEffectsId.Clear();
		}

		public static void Deserialize(XmlReader reader)
		{
			Close();
			RedundancyDetected = 0;
			reader.ReadStartElement();
			reader.ReadElementContentAsInt();
			reader.ReadStartElement();
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				MyParticleEffect myParticleEffect = MyParticlesManager.EffectsPool.Allocate();
				myParticleEffect.Deserialize(reader);
				AddParticleEffect(myParticleEffect);
			}
			reader.ReadEndElement();
			reader.ReadEndElement();
		}

		public static MyParticleEffect CreateParticleEffect(string name, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID)
		{
			if (m_libraryEffectsString.ContainsKey(name))
			{
				return m_libraryEffectsString[name].CreateInstance(ref effectMatrix, ref worldPosition, parentID);
			}
			return null;
		}

		public static void RemoveParticleEffectInstance(MyParticleEffect effect)
		{
			effect.Close(notify: true, forceInstant: false);
			if (m_libraryEffectsString.ContainsKey(effect.Name))
			{
				MyConcurrentList<MyParticleEffect> instances = m_libraryEffectsString[effect.Name].GetInstances();
				if (instances != null && instances.Contains(effect))
				{
					MyParticlesManager.EffectsPool.Deallocate(effect);
					m_libraryEffectsString[effect.Name].RemoveInstance(effect);
				}
			}
		}

		public static void DebugDraw()
		{
			foreach (MyParticleEffect value in m_libraryEffectsString.Values)
			{
				MyConcurrentList<MyParticleEffect> instances = value.GetInstances();
				if (instances != null)
				{
					foreach (MyParticleEffect item in instances)
					{
						item.DebugDraw();
					}
				}
			}
		}
	}
}
