using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage.GameServices
{
	public class MyModMetadataLoader
	{
		public static ModMetadataFile Parse(string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				return null;
			}
			ModMetadataFile result = null;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModMetadataFile));
				using (TextReader textReader = new StringReader(xml))
				{
					result = (ModMetadataFile)xmlSerializer.Deserialize(textReader);
					return result;
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed parsing mod metadata: {0}", ex.Message);
				return result;
			}
		}

		public static string Serialize(ModMetadataFile data)
		{
			if (data == null)
			{
				return null;
			}
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModMetadataFile));
				using (TextWriter textWriter = new StringWriter())
				{
					xmlSerializer.Serialize(textWriter, data);
					return textWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed serializing mod metadata: {0}", ex.Message);
				return null;
			}
		}

		public static ModMetadataFile Load(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return null;
			}
			ModMetadataFile result = null;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModMetadataFile));
				Stream stream = MyFileSystem.OpenRead(filename);
				if (stream == null)
				{
					return result;
				}
				result = (ModMetadataFile)xmlSerializer.Deserialize(stream);
				stream.Close();
				return result;
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed loading mod metadata file: {0} with exception: {1}", filename, ex.Message);
				return result;
			}
		}

		public static bool Save(string filename, ModMetadataFile file)
		{
			if (string.IsNullOrEmpty(filename) || file == null)
			{
				return false;
			}
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModMetadataFile));
				TextWriter textWriter = new StreamWriter(filename);
				xmlSerializer.Serialize(textWriter, file);
				textWriter.Close();
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed saving mod metadata file: {0} with exception: {1}", filename, ex.Message);
				return false;
			}
			return true;
		}
	}
}
